// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PrefabUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public static class PrefabUtils
  {
    public static T[] ToArray<T>(HashSet<T> hashSet)
    {
      T[] array = new T[hashSet.Count];
      hashSet.CopyTo(array);
      return array;
    }

    public static bool HasUnlockedPrefab<T>(EntityManager entityManager, EntityQuery unlockQuery) where T : unmanaged
    {
      if (!unlockQuery.IsEmptyIgnoreFilter)
      {
        using (NativeArray<Unlock> componentDataArray = unlockQuery.ToComponentDataArray<Unlock>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int index = 0; index < componentDataArray.Length; ++index)
          {
            if (entityManager.HasComponent<T>(componentDataArray[index].m_Prefab))
              return true;
          }
        }
      }
      return false;
    }

    public static bool HasUnlockedPrefabAll<T1, T2>(
      EntityManager entityManager,
      EntityQuery unlockQuery)
      where T1 : unmanaged
      where T2 : unmanaged
    {
      if (!unlockQuery.IsEmptyIgnoreFilter)
      {
        using (NativeArray<Unlock> componentDataArray = unlockQuery.ToComponentDataArray<Unlock>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int index = 0; index < componentDataArray.Length; ++index)
          {
            if (entityManager.HasComponent<T1>(componentDataArray[index].m_Prefab) && entityManager.HasComponent<T2>(componentDataArray[index].m_Prefab))
              return true;
          }
        }
      }
      return false;
    }

    public static bool HasUnlockedPrefabAny<T1, T2, T3, T4>(
      EntityManager entityManager,
      EntityQuery unlockQuery)
      where T1 : unmanaged
      where T2 : unmanaged
      where T3 : unmanaged
      where T4 : unmanaged
    {
      if (!unlockQuery.IsEmptyIgnoreFilter)
      {
        using (NativeArray<Unlock> componentDataArray = unlockQuery.ToComponentDataArray<Unlock>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int index = 0; index < componentDataArray.Length; ++index)
          {
            if (entityManager.HasComponent<T1>(componentDataArray[index].m_Prefab) || entityManager.HasComponent<T2>(componentDataArray[index].m_Prefab) || entityManager.HasComponent<T3>(componentDataArray[index].m_Prefab) || entityManager.HasComponent<T4>(componentDataArray[index].m_Prefab))
              return true;
          }
        }
      }
      return false;
    }
  }
}
