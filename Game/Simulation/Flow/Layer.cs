// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.Layer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

#nullable disable
namespace Game.Simulation.Flow
{
  public struct Layer : IDisposable
  {
    public UnsafeList<CutElement> m_Elements;
    public UnsafeList<CutElementRef> m_ElementRefs;
    private int m_UsedElementCount;
    private int m_UsedElementRefCount;
    private int m_FreeElementIndex;
    private int m_FreeElementRefIndex;

    public Layer(int initialLength, Allocator allocator)
    {
      this.m_Elements = new UnsafeList<CutElement>(initialLength, (AllocatorManager.AllocatorHandle) allocator, NativeArrayOptions.ClearMemory);
      this.m_Elements.Length = initialLength;
      this.m_ElementRefs = new UnsafeList<CutElementRef>(initialLength, (AllocatorManager.AllocatorHandle) allocator, NativeArrayOptions.ClearMemory);
      this.m_ElementRefs.Length = initialLength;
      this.m_UsedElementCount = 0;
      this.m_UsedElementRefCount = 0;
      if (initialLength > 0)
      {
        this.m_FreeElementIndex = 0;
        this.m_FreeElementRefIndex = 0;
        int num1 = initialLength - 1;
        for (int index = 0; index < initialLength; ++index)
        {
          int num2 = index == num1 ? -1 : index + 1;
          CutElement element = this.m_Elements[index] with
          {
            m_NextIndex = num2
          };
          this.m_Elements[index] = element;
          CutElementRef elementRef = this.m_ElementRefs[index] with
          {
            m_NextIndex = num2
          };
          this.m_ElementRefs[index] = elementRef;
        }
      }
      else
      {
        this.m_FreeElementIndex = -1;
        this.m_FreeElementRefIndex = -1;
      }
    }

    public bool isEmpty => this.m_UsedElementCount == 0;

    public int usedElementCount => this.m_UsedElementCount;

    public int usedElementRefCount => this.m_UsedElementRefCount;

    public bool ContainsCutElement(Identifier id)
    {
      if (id.m_Index != -1)
      {
        CutElement element = this.m_Elements[id.m_Index];
        if (element.isCreated && element.m_Version == id.m_Version)
          return true;
      }
      return false;
    }

    public bool ContainsCutElementForConnection(
      Identifier id,
      Connection connection,
      bool admissible)
    {
      if (id.m_Index != -1 && id.m_Index < this.m_Elements.Length)
      {
        CutElement element = this.m_Elements[id.m_Index];
        if (element.isCreated && element.m_Version == id.m_Version && element.m_Edge == connection.m_Edge && element.m_StartNode == connection.m_StartNode && element.m_EndNode == connection.m_EndNode && element.isAdmissible == admissible)
          return true;
      }
      return false;
    }

    public ref CutElement GetCutElement(int index) => ref this.m_Elements.ElementAt(index);

    public int AddCutElement(in CutElement element)
    {
      int index = this.m_FreeElementIndex;
      if (index != -1)
      {
        this.m_FreeElementIndex = this.m_Elements[index].m_NextIndex;
      }
      else
      {
        index = this.m_Elements.Length;
        this.m_Elements.Resize(index + 1, NativeArrayOptions.ClearMemory);
      }
      this.m_Elements[index] = element;
      ++this.m_UsedElementCount;
      return index;
    }

    public void FreeCutElement(int index)
    {
      ref CutElement local = ref this.m_Elements.ElementAt(index);
      local.m_Flags = CutElementFlags.None;
      local.m_NextIndex = this.m_FreeElementIndex;
      this.m_FreeElementIndex = index;
      --this.m_UsedElementCount;
    }

    public ref CutElementRef GetCutElementRef(int index) => ref this.m_ElementRefs.ElementAt(index);

    public int AddCutElementRef(in CutElementRef elementRef)
    {
      int index = this.m_FreeElementRefIndex;
      if (index != -1)
      {
        this.m_FreeElementRefIndex = this.m_ElementRefs[index].m_NextIndex;
      }
      else
      {
        index = this.m_ElementRefs.Length;
        this.m_ElementRefs.Resize(index + 1, NativeArrayOptions.ClearMemory);
      }
      this.m_ElementRefs[index] = elementRef;
      ++this.m_UsedElementRefCount;
      return index;
    }

    public void FreeCutElementRef(int index)
    {
      ref CutElementRef local = ref this.m_ElementRefs.ElementAt(index);
      local.m_Layer = -1;
      local.m_Index = -1;
      local.m_NextIndex = this.m_FreeElementRefIndex;
      this.m_FreeElementRefIndex = index;
      --this.m_UsedElementRefCount;
    }

    public void MergeGroups(int elementId1, int elementId2)
    {
      ref CutElement local1 = ref this.GetCutElement(elementId1);
      CutElement cutElement = this.GetCutElement(elementId2);
      int group1 = local1.m_Group;
      int group2 = cutElement.m_Group;
      if (group1 == group2)
        return;
      int nextIndex = local1.m_NextIndex;
      local1.m_NextIndex = group2;
      int index = group2;
      do
      {
        ref CutElement local2 = ref this.GetCutElement(index);
        local2.m_Group = group1;
        index = local2.m_NextIndex;
        if (index == -1)
          local2.m_NextIndex = nextIndex;
      }
      while (index != -1);
    }

    public void RemoveElementLink(
      int elementIndex,
      int upperLayerIndex,
      int upperLayerElementIndex)
    {
      ref CutElement local1 = ref this.GetCutElement(elementIndex);
      int index1 = -1;
      int index2;
      CutElementRef cutElementRef;
      for (index2 = local1.m_LinkedElements; index2 != -1; index2 = cutElementRef.m_NextIndex)
      {
        cutElementRef = this.GetCutElementRef(index2);
        if (cutElementRef.m_Layer != upperLayerIndex || cutElementRef.m_Index != upperLayerElementIndex)
          index1 = index2;
        else
          break;
      }
      ref CutElementRef local2 = ref this.GetCutElementRef(index2);
      if (index1 == -1)
        local1.m_LinkedElements = local2.m_NextIndex;
      else
        this.GetCutElementRef(index1).m_NextIndex = local2.m_NextIndex;
      this.FreeCutElementRef(index2);
    }

    public static Layer Load(
      LayerState state,
      NativeArray<CutElement> layerElements,
      ref int elementIndex,
      NativeArray<CutElementRef> layerElementRefs,
      ref int elementRefIndex)
    {
      UnsafeList<CutElement> unsafeList1 = new UnsafeList<CutElement>(state.m_ElementsLength, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      Layer.AddRange<CutElement>(ref unsafeList1, layerElements.Slice<CutElement>(elementIndex, state.m_ElementsLength));
      UnsafeList<CutElementRef> unsafeList2 = new UnsafeList<CutElementRef>(state.m_ElementRefsLength, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      Layer.AddRange<CutElementRef>(ref unsafeList2, layerElementRefs.Slice<CutElementRef>(elementRefIndex, state.m_ElementRefsLength));
      elementIndex += state.m_ElementsLength;
      elementRefIndex += state.m_ElementRefsLength;
      return new Layer()
      {
        m_Elements = unsafeList1,
        m_ElementRefs = unsafeList2,
        m_UsedElementCount = state.m_UsedElementCount,
        m_UsedElementRefCount = state.m_UsedElementRefCount,
        m_FreeElementIndex = state.m_FreeElementIndex,
        m_FreeElementRefIndex = state.m_FreeElementRefIndex
      };
    }

    private static unsafe void AddRange<T>(ref UnsafeList<T> unsafeList, NativeSlice<T> slice) where T : unmanaged
    {
      unsafeList.AddRange(slice.GetUnsafeReadOnlyPtr<T>(), slice.Length);
    }

    public void Save(
      NativeList<LayerState> layerStates,
      NativeList<CutElement> layerElements,
      NativeList<CutElementRef> layerElementRefs)
    {
      layerStates.Add(new LayerState()
      {
        m_ElementsLength = this.m_Elements.Length,
        m_ElementRefsLength = this.m_ElementRefs.Length,
        m_UsedElementCount = this.m_UsedElementCount,
        m_UsedElementRefCount = this.m_UsedElementRefCount,
        m_FreeElementIndex = this.m_FreeElementIndex,
        m_FreeElementRefIndex = this.m_FreeElementRefIndex
      });
      Layer.AddRange<CutElement>(layerElements, this.m_Elements);
      Layer.AddRange<CutElementRef>(layerElementRefs, this.m_ElementRefs);
    }

    private static unsafe void AddRange<T>(NativeList<T> list, UnsafeList<T> unsafeList) where T : unmanaged
    {
      list.AddRange((void*) unsafeList.Ptr, unsafeList.Length);
    }

    public void Dispose()
    {
      this.m_Elements.Dispose();
      this.m_ElementRefs.Dispose();
    }
  }
}
