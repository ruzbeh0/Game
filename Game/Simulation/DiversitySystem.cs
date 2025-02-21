// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DiversitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DiversitySystem : GameSystemBase, IPostDeserialize
  {
    private EntityQuery m_AtmosphereQuery;
    private EntityQuery m_AtmospherePrefabQuery;
    private EntityQuery m_BiomeQuery;
    private EntityQuery m_BiomePrefabQuery;
    private EntityQuery m_EditorContainerQuery;

    public void ApplyAtmospherePreset(Entity atmospherePrefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AtmosphereQuery.SetSingleton<AtmosphereData>(this.m_AtmosphereQuery.GetSingleton<AtmosphereData>() with
      {
        m_AtmospherePrefab = atmospherePrefab
      });
    }

    public void ApplyBiomePreset(Entity biomePrefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BiomeQuery.SetSingleton<BiomeData>(this.m_BiomeQuery.GetSingleton<BiomeData>() with
      {
        m_BiomePrefab = biomePrefab
      });
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_AtmospherePrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (entityArray1.Length == 0)
      {
        COSystemBase.baseLog.InfoFormat("WARNING: PostDeserialize({0}): no Atmosphere prefabs found", (object) context);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray2 = this.m_BiomePrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        if (entityArray2.Length == 0)
        {
          COSystemBase.baseLog.InfoFormat("WARNING: PostDeserialize({0}): no Biome prefabs found", (object) context);
        }
        else
        {
          EntityManager entityManager;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AtmosphereQuery.IsEmptyIgnoreFilter)
          {
            Entity prefab = entityArray1[0];
            Entity entity = this.EntityManager.CreateEntity();
            entityManager = this.EntityManager;
            entityManager.AddComponentData<AtmosphereData>(entity, new AtmosphereData(prefab));
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_BiomeQuery.IsEmptyIgnoreFilter)
          {
            Entity prefab = entityArray2[0];
            entityManager = this.EntityManager;
            Entity entity = entityManager.CreateEntity();
            entityManager = this.EntityManager;
            entityManager.AddComponentData<BiomeData>(entity, new BiomeData(prefab));
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorContainerQuery.IsEmptyIgnoreFilter)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> entityArray3 = this.m_EditorContainerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          foreach (Entity entity in entityArray3)
          {
            Game.Objects.Transform component1;
            if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(entity, out component1) && component1.m_Position.Equals(float3.zero))
            {
              UnityEngine.Debug.Log((object) ("There is invalid EditorContainer in the map:" + entity.ToString()));
              Owner component2;
              Game.Objects.Transform component3;
              if (this.EntityManager.TryGetComponent<Owner>(entity, out component2) && this.EntityManager.TryGetComponent<Game.Objects.Transform>(component2.m_Owner, out component3))
              {
                entityManager = this.EntityManager;
                entityManager.SetComponentData<Game.Objects.Transform>(entity, component3);
              }
            }
          }
          entityArray3.Dispose();
        }
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AtmosphereQuery = this.GetEntityQuery(ComponentType.ReadOnly<AtmosphereData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AtmospherePrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<AtmospherePrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BiomeQuery = this.GetEntityQuery(ComponentType.ReadOnly<BiomeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BiomePrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<BiomePrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EditorContainerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Tools.EditorContainer>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.ReadOnly<Owner>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    [Preserve]
    public DiversitySystem()
    {
    }
  }
}
