// Decompiled with JetBrains decompiler
// Type: Game.Common.GroupBuilder`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;
using Unity.Mathematics;

#nullable disable
namespace Game.Common
{
  public struct GroupBuilder<T> where T : unmanaged, IEquatable<T>
  {
    private NativeList<int> m_Groups;
    private NativeParallelHashMap<T, int> m_GroupIndex;
    private NativeList<GroupBuilder<T>.Result> m_Results;

    public GroupBuilder(Allocator allocator)
    {
      this.m_Groups = new NativeList<int>(32, (AllocatorManager.AllocatorHandle) allocator);
      this.m_GroupIndex = new NativeParallelHashMap<T, int>(32, (AllocatorManager.AllocatorHandle) allocator);
      this.m_Results = new NativeList<GroupBuilder<T>.Result>(32, (AllocatorManager.AllocatorHandle) allocator);
    }

    public void Dispose()
    {
      this.m_Groups.Dispose();
      this.m_GroupIndex.Dispose();
      this.m_Results.Dispose();
    }

    public void AddSingle(T item)
    {
      if (this.m_GroupIndex.TryGetValue(item, out int _))
        return;
      int group = this.CreateGroup();
      this.AddToGroup(item, group);
    }

    public void AddPair(T item1, T item2)
    {
      int index1;
      if (this.m_GroupIndex.TryGetValue(item1, out index1))
      {
        int group1 = this.m_Groups[index1];
        int index2;
        if (this.m_GroupIndex.TryGetValue(item2, out index2))
        {
          int group2 = this.m_Groups[index2];
          if (group1 == group2)
            return;
          this.MergeGroups(group1, group2);
        }
        else
          this.AddToGroup(item2, group1);
      }
      else
      {
        int index3;
        if (this.m_GroupIndex.TryGetValue(item2, out index3))
        {
          int group = this.m_Groups[index3];
          this.AddToGroup(item1, group);
        }
        else
        {
          int group = this.CreateGroup();
          this.AddToGroup(item1, group);
          this.AddToGroup(item2, group);
        }
      }
    }

    public bool TryGetFirstGroup(
      out NativeArray<GroupBuilder<T>.Result> group,
      out GroupBuilder<T>.Iterator iterator)
    {
      NativeArray<int> nativeArray = this.m_Groups.AsArray();
      NativeArray<GroupBuilder<T>.Result> array = this.m_Results.AsArray();
      if (array.Length == 0)
      {
        group = new NativeArray<GroupBuilder<T>.Result>();
        iterator = new GroupBuilder<T>.Iterator();
        return false;
      }
      for (int index = 0; index < nativeArray.Length; ++index)
        nativeArray[index] = nativeArray[nativeArray[index]];
      for (int index = 0; index < array.Length; ++index)
      {
        GroupBuilder<T>.Result result = array[index];
        result.m_Group = nativeArray[result.m_Group];
        array[index] = result;
      }
      array.Sort<GroupBuilder<T>.Result>();
      iterator = new GroupBuilder<T>.Iterator(array[0].m_Group);
      return this.TryGetNextGroup(out group, ref iterator);
    }

    public bool TryGetNextGroup(
      out NativeArray<GroupBuilder<T>.Result> group,
      ref GroupBuilder<T>.Iterator iterator)
    {
      NativeArray<GroupBuilder<T>.Result> nativeArray = this.m_Results.AsArray();
      for (int index = iterator.m_StartIndex + 1; index < nativeArray.Length; ++index)
      {
        GroupBuilder<T>.Result result = nativeArray[index];
        if (result.m_Group != iterator.m_GroupIndex)
        {
          group = nativeArray.GetSubArray(iterator.m_StartIndex, index - iterator.m_StartIndex);
          iterator.m_StartIndex = index;
          iterator.m_GroupIndex = result.m_Group;
          return true;
        }
      }
      if (nativeArray.Length > iterator.m_StartIndex)
      {
        group = nativeArray.GetSubArray(iterator.m_StartIndex, nativeArray.Length - iterator.m_StartIndex);
        iterator.m_StartIndex = nativeArray.Length;
        return true;
      }
      group = new NativeArray<GroupBuilder<T>.Result>();
      return false;
    }

    private int CreateGroup()
    {
      int length = this.m_Groups.Length;
      this.m_Groups.Add(in length);
      return length;
    }

    private int MergeGroups(int index1, int index2)
    {
      int num = math.min(index1, index2);
      this.m_Groups[math.max(index1, index2)] = num;
      return num;
    }

    private void AddToGroup(T item, int index)
    {
      this.m_GroupIndex.TryAdd(item, index);
      this.m_Results.Add(new GroupBuilder<T>.Result(item, index));
    }

    public struct Result : IComparable<GroupBuilder<T>.Result>
    {
      public T m_Item;
      public int m_Group;

      public Result(T item, int group)
      {
        this.m_Item = item;
        this.m_Group = group;
      }

      public int CompareTo(GroupBuilder<T>.Result other) => this.m_Group - other.m_Group;
    }

    public struct Iterator
    {
      public int m_StartIndex;
      public int m_GroupIndex;

      public Iterator(int groupIndex)
      {
        this.m_StartIndex = 0;
        this.m_GroupIndex = groupIndex;
      }
    }
  }
}
