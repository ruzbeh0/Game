// Decompiled with JetBrains decompiler
// Type: Game.Serialization.PrefabReferences
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Serialization
{
  public struct PrefabReferences
  {
    [ReadOnly]
    private ComponentLookup<PrefabData> m_PrefabData;
    [ReadOnly]
    private NativeArray<Entity> m_PrefabArray;
    private UnsafeList<bool> m_ReferencedPrefabs;
    private Entity m_LastPrefabIn;
    private Entity m_LastPrefabOut;
    private bool m_IsLoading;

    public PrefabReferences(
      NativeArray<Entity> prefabArray,
      UnsafeList<bool> referencedPrefabs,
      ComponentLookup<PrefabData> prefabData,
      bool isLoading)
    {
      this.m_PrefabArray = prefabArray;
      this.m_ReferencedPrefabs = referencedPrefabs;
      this.m_PrefabData = prefabData;
      this.m_LastPrefabIn = Entity.Null;
      this.m_LastPrefabOut = Entity.Null;
      this.m_IsLoading = isLoading;
    }

    public void SetDirty(Entity prefab)
    {
      if (this.m_IsLoading)
        return;
      PrefabData prefabData = this.m_PrefabData[prefab];
      if (prefabData.m_Index < 0)
        return;
      this.m_ReferencedPrefabs[prefabData.m_Index] = true;
    }

    public void Check(ref Entity prefab)
    {
      if (prefab != this.m_LastPrefabIn)
      {
        PrefabData prefabData = this.m_PrefabData[prefab];
        prefabData.m_Index = math.select(prefabData.m_Index, this.m_ReferencedPrefabs.Length + prefabData.m_Index, prefabData.m_Index < 0);
        this.m_LastPrefabIn = prefab;
        if (this.m_IsLoading)
        {
          this.m_LastPrefabOut = this.m_PrefabArray[prefabData.m_Index];
          if (this.m_LastPrefabOut == this.m_LastPrefabIn)
            this.m_ReferencedPrefabs[prefabData.m_Index] = true;
        }
        else
        {
          this.m_LastPrefabOut = prefab;
          this.m_ReferencedPrefabs[prefabData.m_Index] = true;
        }
      }
      prefab = this.m_LastPrefabOut;
    }

    public Entity Check(EntityManager entityManager, Entity prefab)
    {
      if (prefab == Entity.Null)
        return Entity.Null;
      if (this.m_IsLoading && entityManager.HasComponent<LoadedIndex>(prefab))
        return prefab;
      if (prefab != this.m_LastPrefabIn)
      {
        PrefabData componentData = entityManager.GetComponentData<PrefabData>(prefab);
        componentData.m_Index = math.select(componentData.m_Index, this.m_ReferencedPrefabs.Length + componentData.m_Index, componentData.m_Index < 0);
        this.m_LastPrefabIn = prefab;
        if (this.m_IsLoading)
        {
          this.m_LastPrefabOut = this.m_PrefabArray[componentData.m_Index];
          if (this.m_LastPrefabOut == this.m_LastPrefabIn)
            this.m_ReferencedPrefabs[componentData.m_Index] = true;
        }
        else
        {
          this.m_LastPrefabOut = prefab;
          this.m_ReferencedPrefabs[componentData.m_Index] = true;
        }
      }
      return this.m_LastPrefabOut;
    }
  }
}
