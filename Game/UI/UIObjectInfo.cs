// Decompiled with JetBrains decompiler
// Type: Game.UI.UIObjectInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Prefabs;
using System;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI
{
  public readonly struct UIObjectInfo : IComparable<UIObjectInfo>
  {
    public Entity entity { get; }

    public PrefabData prefabData { get; }

    public int priority { get; }

    public UIObjectInfo(Entity entity, int priority)
    {
      this.entity = entity;
      this.prefabData = new PrefabData();
      this.priority = priority;
    }

    public UIObjectInfo(Entity entity, PrefabData prefabData, int priority)
    {
      this.entity = entity;
      this.prefabData = prefabData;
      this.priority = priority;
    }

    public int CompareTo(UIObjectInfo other) => this.priority.CompareTo(other.priority);

    public static NativeList<UIObjectInfo> GetSortedObjects(EntityQuery query, Allocator allocator)
    {
      using (NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        using (NativeArray<PrefabData> componentDataArray1 = query.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.Temp))
        {
          using (NativeArray<UIObjectData> componentDataArray2 = query.ToComponentDataArray<UIObjectData>((AllocatorManager.AllocatorHandle) Allocator.Temp))
          {
            int length = entityArray.Length;
            NativeList<UIObjectInfo> list = new NativeList<UIObjectInfo>(length, (AllocatorManager.AllocatorHandle) allocator);
            for (int index = 0; index < length; ++index)
              list.Add(new UIObjectInfo(entityArray[index], componentDataArray1[index], componentDataArray2[index].m_Priority));
            list.Sort<UIObjectInfo>();
            return list;
          }
        }
      }
    }

    public static NativeList<UIObjectInfo> GetSortedObjects(
      EntityManager entityManager,
      EntityQuery query,
      Allocator allocator)
    {
      using (NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        using (NativeArray<PrefabData> componentDataArray = query.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.Temp))
        {
          int length = entityArray.Length;
          NativeList<UIObjectInfo> list = new NativeList<UIObjectInfo>(length, (AllocatorManager.AllocatorHandle) allocator);
          for (int index = 0; index < length; ++index)
          {
            UIObjectData component;
            int priority = entityManager.TryGetComponent<UIObjectData>(entityArray[index], out component) ? component.m_Priority : 0;
            list.Add(new UIObjectInfo(entityArray[index], componentDataArray[index], priority));
          }
          list.Sort<UIObjectInfo>();
          return list;
        }
      }
    }

    public static NativeList<UIObjectInfo> GetSortedObjects(
      EntityManager entityManager,
      NativeList<Entity> entities,
      Allocator allocator)
    {
      int length = entities.Length;
      NativeList<UIObjectInfo> list = new NativeList<UIObjectInfo>(length, (AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < length; ++index)
      {
        Entity entity = entities[index];
        UIObjectData component;
        int priority = entityManager.TryGetComponent<UIObjectData>(entity, out component) ? component.m_Priority : 0;
        list.Add(new UIObjectInfo(entity, entityManager.GetComponentData<PrefabData>(entity), priority));
      }
      list.Sort<UIObjectInfo>();
      return list;
    }

    public static NativeList<UIObjectInfo> GetObjects(
      EntityManager entityManager,
      DynamicBuffer<UIGroupElement> elements,
      Allocator allocator)
    {
      NativeList<UIObjectInfo> objects = new NativeList<UIObjectInfo>(elements.Length, (AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < elements.Length; ++index)
      {
        Entity prefab = elements[index].m_Prefab;
        UIObjectData component;
        int priority = entityManager.TryGetComponent<UIObjectData>(prefab, out component) ? component.m_Priority : 0;
        objects.Add(new UIObjectInfo(prefab, entityManager.GetComponentData<PrefabData>(prefab), priority));
      }
      return objects;
    }

    public static NativeList<UIObjectInfo> GetSortedObjects(
      EntityManager entityManager,
      DynamicBuffer<UIGroupElement> elements,
      Allocator allocator)
    {
      NativeList<UIObjectInfo> list = new NativeList<UIObjectInfo>(elements.Length, (AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < elements.Length; ++index)
      {
        Entity prefab = elements[index].m_Prefab;
        UIObjectData component;
        int priority = entityManager.TryGetComponent<UIObjectData>(prefab, out component) ? component.m_Priority : 0;
        list.Add(new UIObjectInfo(prefab, entityManager.GetComponentData<PrefabData>(prefab), priority));
      }
      list.Sort<UIObjectInfo>();
      return list;
    }
  }
}
