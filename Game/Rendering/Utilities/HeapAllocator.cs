// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.HeapAllocator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering.Utilities
{
  public struct HeapAllocator : IDisposable
  {
    public const int MaxAlignmentLog2 = 63;
    public const int AlignmentBits = 6;
    private NativeList<HeapAllocator.SizeBin> m_SizeBins;
    private NativeList<HeapAllocator.BlocksOfSize> m_Blocks;
    private NativeList<int> m_BlocksFreelist;
    private NativeParallelHashMap<ulong, ulong> m_FreeEndpoints;
    private ulong m_Size;
    private ulong m_Free;
    private readonly int m_MinimumAlignmentLog2;
    private bool m_IsCreated;

    public HeapAllocator(ulong size = 0, uint minimumAlignment = 1)
    {
      this.m_SizeBins = new NativeList<HeapAllocator.SizeBin>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_Blocks = new NativeList<HeapAllocator.BlocksOfSize>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_BlocksFreelist = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_FreeEndpoints = new NativeParallelHashMap<ulong, ulong>(0, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_Size = 0UL;
      this.m_Free = 0UL;
      this.m_MinimumAlignmentLog2 = math.tzcnt(minimumAlignment);
      this.m_IsCreated = true;
      this.Resize(size);
    }

    public uint MinimumAlignment => (uint) (1 << this.m_MinimumAlignmentLog2);

    public ulong FreeSpace => this.m_Free;

    public ulong UsedSpace => this.m_Size - this.m_Free;

    public ulong OnePastHighestUsedAddress
    {
      get
      {
        ulong num;
        return !this.m_FreeEndpoints.TryGetValue(this.m_Size, out num) ? this.m_Size : num;
      }
    }

    public ulong Size => this.m_Size;

    public bool Empty => (long) this.m_Free == (long) this.m_Size;

    public bool Full => this.m_Free == 0UL;

    public bool IsCreated => this.m_IsCreated;

    public void Clear()
    {
      ulong size = this.m_Size;
      this.m_SizeBins.Clear();
      this.m_Blocks.Clear();
      this.m_BlocksFreelist.Clear();
      this.m_FreeEndpoints.Clear();
      this.m_Size = 0UL;
      this.m_Free = 0UL;
      this.Resize(size);
    }

    public void Dispose()
    {
      if (!this.IsCreated)
        return;
      for (int index = 0; index < this.m_Blocks.Length; ++index)
        this.m_Blocks[index].Dispose();
      this.m_FreeEndpoints.Dispose();
      this.m_Blocks.Dispose();
      this.m_BlocksFreelist.Dispose();
      this.m_SizeBins.Dispose();
      this.m_IsCreated = false;
    }

    public bool Resize(ulong newSize)
    {
      if ((long) newSize == (long) this.m_Size)
        return true;
      if (newSize <= this.m_Size)
        return false;
      this.Release(HeapBlock.OfSize(this.m_Size, newSize - this.m_Size));
      this.m_Size = newSize;
      return true;
    }

    public HeapBlock Allocate(ulong size, uint alignment = 1)
    {
      size = HeapAllocator.NextAligned(size, this.m_MinimumAlignmentLog2);
      alignment = math.max(alignment, this.MinimumAlignment);
      HeapAllocator.SizeBin sizeBin1 = new HeapAllocator.SizeBin(size, alignment);
      for (int smallestSufficientBin = this.FindSmallestSufficientBin(sizeBin1); smallestSufficientBin < this.m_SizeBins.Length; ++smallestSufficientBin)
      {
        HeapAllocator.SizeBin sizeBin2 = this.m_SizeBins[smallestSufficientBin];
        if (this.CanFitAllocation(sizeBin1, sizeBin2))
        {
          HeapBlock block = this.PopBlockFromBin(sizeBin2, smallestSufficientBin);
          return this.CutAllocationFromBlock(sizeBin1, block);
        }
      }
      return new HeapBlock();
    }

    public void Release(HeapBlock block)
    {
      block = this.Coalesce(block);
      HeapAllocator.SizeBin bin = new HeapAllocator.SizeBin(block);
      int index = this.FindSmallestSufficientBin(bin);
      if (index >= this.m_SizeBins.Length || bin.CompareTo(this.m_SizeBins[index]) != 0)
        index = this.AddNewBin(ref bin, index);
      this.m_Blocks[this.m_SizeBins[index].blocksId].Push(block);
      this.m_Free += block.Length;
      this.m_FreeEndpoints[block.begin] = block.end;
      this.m_FreeEndpoints[block.end] = block.begin;
    }

    public void DebugValidateInternalState()
    {
      int length1 = this.m_SizeBins.Length;
      int length2 = this.m_BlocksFreelist.Length;
      int expected1 = 0;
      int actual1 = 0;
      for (int index = 0; index < this.m_Blocks.Length; ++index)
      {
        if (this.m_Blocks[index].Empty)
          ++expected1;
        else
          ++actual1;
      }
      UnityEngine.Assertions.Assert.AreEqual(length1, actual1, "There should be exactly one non-empty block list per size bin");
      UnityEngine.Assertions.Assert.AreEqual(expected1, length2, "All empty block lists should be in the free list");
      for (int index = 0; index < this.m_BlocksFreelist.Length; ++index)
        UnityEngine.Assertions.Assert.IsTrue(this.m_Blocks[this.m_BlocksFreelist[index]].Empty, "There should be only empty block lists in the free list");
      ulong expected2 = 0;
      int num = 0;
      for (int index = 0; index < this.m_SizeBins.Length; ++index)
      {
        HeapAllocator.SizeBin sizeBin1 = this.m_SizeBins[index];
        ulong size = sizeBin1.Size;
        uint alignment = sizeBin1.Alignment;
        HeapAllocator.BlocksOfSize block1 = this.m_Blocks[sizeBin1.blocksId];
        UnityEngine.Assertions.Assert.IsFalse(block1.Empty, "All block lists should be non-empty, empty lists should be removed");
        int length3 = block1.Length;
        for (int i = 0; i < length3; ++i)
        {
          HeapBlock block2 = block1.Block(i);
          HeapAllocator.SizeBin sizeBin2 = new HeapAllocator.SizeBin(block2);
          UnityEngine.Assertions.Assert.AreEqual(size, sizeBin2.Size, "Block size should match its bin");
          UnityEngine.Assertions.Assert.AreEqual(alignment, sizeBin2.Alignment, "Block alignment should match its bin");
          expected2 += block2.Length;
          ulong actual2;
          if (this.m_FreeEndpoints.TryGetValue(block2.begin, out actual2))
            UnityEngine.Assertions.Assert.AreEqual(block2.end, actual2, "Free block end does not match stored endpoint");
          else
            UnityEngine.Assertions.Assert.IsTrue(false, "No end endpoint found for free block");
          ulong actual3;
          if (this.m_FreeEndpoints.TryGetValue(block2.end, out actual3))
            UnityEngine.Assertions.Assert.AreEqual(block2.begin, actual3, "Free block begin does not match stored endpoint");
          else
            UnityEngine.Assertions.Assert.IsTrue(false, "No begin endpoint found for free block");
          ++num;
        }
      }
      UnityEngine.Assertions.Assert.AreEqual(expected2, this.FreeSpace, "Free size reported incorrectly");
      UnityEngine.Assertions.Assert.IsTrue(expected2 <= this.Size, "Amount of free size larger than maximum");
      UnityEngine.Assertions.Assert.AreEqual(2 * num, this.m_FreeEndpoints.Count(), "Each free block should have exactly 2 stored endpoints");
    }

    private int FindSmallestSufficientBin(HeapAllocator.SizeBin needle)
    {
      if (this.m_SizeBins.Length == 0)
        return 0;
      int index1 = 0;
      int num1 = this.m_SizeBins.Length;
      int index2;
      while (true)
      {
        int num2 = (num1 - index1) / 2;
        if (num2 != 0)
        {
          index2 = index1 + num2;
          int num3 = needle.CompareTo(this.m_SizeBins[index2]);
          if (num3 < 0)
            num1 = index2;
          else if (num3 > 0)
            index1 = index2;
          else
            goto label_11;
        }
        else
          break;
      }
      return needle.CompareTo(this.m_SizeBins[index1]) <= 0 ? index1 : index1 + 1;
label_11:
      return index2;
    }

    private unsafe int AddNewBin(ref HeapAllocator.SizeBin bin, int index)
    {
      if (this.m_BlocksFreelist.IsEmpty)
      {
        bin.blocksId = this.m_Blocks.Length;
        this.m_Blocks.Add(new HeapAllocator.BlocksOfSize(0));
      }
      else
      {
        int num = this.m_BlocksFreelist.Length - 1;
        bin.blocksId = this.m_BlocksFreelist[num];
        this.m_BlocksFreelist.ResizeUninitialized(num);
      }
      int num1 = this.m_SizeBins.Length - index;
      this.m_SizeBins.ResizeUninitialized(this.m_SizeBins.Length + 1);
      HeapAllocator.SizeBin* unsafePtr = this.m_SizeBins.GetUnsafePtr<HeapAllocator.SizeBin>();
      UnsafeUtility.MemMove((void*) (unsafePtr + (index + 1)), (void*) (unsafePtr + index), (long) (num1 * UnsafeUtility.SizeOf<HeapAllocator.SizeBin>()));
      unsafePtr[index] = bin;
      return index;
    }

    private unsafe void RemoveBinIfEmpty(HeapAllocator.SizeBin bin, int index)
    {
      if (!this.m_Blocks[bin.blocksId].Empty)
        return;
      int num = this.m_SizeBins.Length - (index + 1);
      HeapAllocator.SizeBin* unsafePtr = this.m_SizeBins.GetUnsafePtr<HeapAllocator.SizeBin>();
      UnsafeUtility.MemMove((void*) (unsafePtr + index), (void*) (unsafePtr + (index + 1)), (long) (num * UnsafeUtility.SizeOf<HeapAllocator.SizeBin>()));
      this.m_SizeBins.ResizeUninitialized(this.m_SizeBins.Length - 1);
      this.m_BlocksFreelist.Add(in bin.blocksId);
    }

    private HeapBlock PopBlockFromBin(HeapAllocator.SizeBin bin, int index)
    {
      HeapBlock block = this.m_Blocks[bin.blocksId].Pop();
      this.RemoveEndpoints(block);
      this.m_Free -= block.Length;
      this.RemoveBinIfEmpty(bin, index);
      return block;
    }

    private void RemoveEndpoints(HeapBlock block)
    {
      this.m_FreeEndpoints.Remove(block.begin);
      this.m_FreeEndpoints.Remove(block.end);
    }

    private void RemoveFreeBlock(HeapBlock block)
    {
      this.RemoveEndpoints(block);
      int smallestSufficientBin = this.FindSmallestSufficientBin(new HeapAllocator.SizeBin(block));
      this.m_Blocks[this.m_SizeBins[smallestSufficientBin].blocksId].Remove(block);
      this.RemoveBinIfEmpty(this.m_SizeBins[smallestSufficientBin], smallestSufficientBin);
      this.m_Free -= block.Length;
    }

    private HeapBlock Coalesce(HeapBlock block, ulong endpoint)
    {
      ulong num;
      if (!this.m_FreeEndpoints.TryGetValue(endpoint, out num))
        return block;
      if ((long) endpoint == (long) block.begin)
      {
        HeapBlock block1 = new HeapBlock(num, block.begin);
        this.RemoveFreeBlock(block1);
        return new HeapBlock(block1.begin, block.end);
      }
      HeapBlock block2 = new HeapBlock(block.end, num);
      this.RemoveFreeBlock(block2);
      return new HeapBlock(block.begin, block2.end);
    }

    private HeapBlock Coalesce(HeapBlock block)
    {
      block = this.Coalesce(block, block.begin);
      block = this.Coalesce(block, block.end);
      return block;
    }

    private bool CanFitAllocation(HeapAllocator.SizeBin allocation, HeapAllocator.SizeBin bin)
    {
      if (this.m_Blocks[bin.blocksId].Empty)
        return false;
      return bin.HasCompatibleAlignment(allocation) || bin.Size >= allocation.Size + (ulong) allocation.Alignment;
    }

    private static ulong NextAligned(ulong offset, int alignmentLog2)
    {
      int num = (1 << alignmentLog2) - 1;
      return offset + (ulong) num >> alignmentLog2 << alignmentLog2;
    }

    private HeapBlock CutAllocationFromBlock(HeapAllocator.SizeBin allocation, HeapBlock block)
    {
      if ((long) allocation.Size == (long) block.Length)
        return block;
      ulong num1 = HeapAllocator.NextAligned(block.begin, allocation.AlignmentLog2);
      ulong num2 = num1 + allocation.Size;
      if (num1 > block.begin)
        this.Release(new HeapBlock(block.begin, num1));
      if (num2 < block.end)
        this.Release(new HeapBlock(num2, block.end));
      return new HeapBlock(num1, num2);
    }

    [DebuggerDisplay("Size = {Size}, Alignment = {Alignment}")]
    private struct SizeBin : IComparable<HeapAllocator.SizeBin>, IEquatable<HeapAllocator.SizeBin>
    {
      public ulong sizeClass;
      public int blocksId;

      public SizeBin(ulong size, uint alignment = 1)
      {
        int num = math.min(63, math.tzcnt(alignment));
        this.sizeClass = size << 6 | (ulong) (uint) num;
        this.blocksId = -1;
      }

      public SizeBin(HeapBlock block)
      {
        int num = math.min(63, math.tzcnt(block.begin));
        this.sizeClass = block.Length << 6 | (ulong) (uint) num;
        this.blocksId = -1;
      }

      public int CompareTo(HeapAllocator.SizeBin other)
      {
        return this.sizeClass.CompareTo(other.sizeClass);
      }

      public bool Equals(HeapAllocator.SizeBin other) => this.CompareTo(other) == 0;

      public bool HasCompatibleAlignment(HeapAllocator.SizeBin requiredAlignment)
      {
        return this.AlignmentLog2 >= requiredAlignment.AlignmentLog2;
      }

      public ulong Size => this.sizeClass >> 6;

      public int AlignmentLog2 => (int) this.sizeClass & 63;

      public uint Alignment => (uint) (1 << this.AlignmentLog2);
    }

    private struct BlocksOfSize : IDisposable
    {
      private unsafe UnsafeList<HeapBlock>* m_Blocks;

      public unsafe BlocksOfSize(int dummy)
      {
        this.m_Blocks = (UnsafeList<HeapBlock>*) UnsafeUtility.Malloc((long) UnsafeUtility.SizeOf<UnsafeList<HeapBlock>>(), UnsafeUtility.AlignOf<UnsafeList<HeapBlock>>(), Allocator.Persistent);
        UnsafeUtility.MemClear((void*) this.m_Blocks, (long) UnsafeUtility.SizeOf<UnsafeList<HeapBlock>>());
        this.m_Blocks->Allocator = (AllocatorManager.AllocatorHandle) Allocator.Persistent;
      }

      public unsafe bool Empty => __nonvirtual (this.m_Blocks->Length) == 0;

      public unsafe void Push(HeapBlock block) => this.m_Blocks->Add(in block);

      public unsafe HeapBlock Pop()
      {
        // ISSUE: explicit non-virtual call
        int length = __nonvirtual (this.m_Blocks->Length);
        if (length == 0)
          return new HeapBlock();
        HeapBlock heapBlock = this.Block(length - 1);
        this.m_Blocks->Resize(length - 1, NativeArrayOptions.UninitializedMemory);
        return heapBlock;
      }

      public unsafe bool Remove(HeapBlock block)
      {
        // ISSUE: explicit non-virtual call
        for (int index = 0; index < __nonvirtual (this.m_Blocks->Length); ++index)
        {
          if (block.CompareTo(this.Block(index)) == 0)
          {
            this.m_Blocks->RemoveAtSwapBack(index);
            return true;
          }
        }
        return false;
      }

      public unsafe void Dispose()
      {
        this.m_Blocks->Dispose();
        UnsafeUtility.Free((void*) this.m_Blocks, Allocator.Persistent);
      }

      public unsafe HeapBlock Block(int i)
      {
        return UnsafeUtility.ReadArrayElement<HeapBlock>((void*) this.m_Blocks->Ptr, i);
      }

      public unsafe int Length => __nonvirtual (this.m_Blocks->Length);
    }
  }
}
