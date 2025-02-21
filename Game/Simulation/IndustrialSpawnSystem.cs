// Decompiled with JetBrains decompiler
// Type: Game.Simulation.IndustrialSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class IndustrialSpawnSystem : GameSystemBase
  {
    private EntityQuery m_IndustrialCompanyPrefabQuery;
    private EntityQuery m_StorageCompanyPrefabQuery;
    private EntityQuery m_ExtractorQuery;
    private EntityQuery m_ExtractorCompanyQuery;
    private EntityQuery m_ExistingIndustrialQuery;
    private EntityQuery m_ExistingExtractorQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private SimulationSystem m_SimulationSystem;
    private ResourceSystem m_ResourceSystem;
    private CitySystem m_CitySystem;
    private IndustrialSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<IndustrialCompanyData>(), ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.Exclude<StorageCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.ReadOnly<StorageCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorQuery = this.GetEntityQuery(ComponentType.ReadOnly<ExtractorProperty>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorCompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ExtractorCompany>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExistingIndustrialQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialCompany>(), ComponentType.Exclude<Game.Companies.ExtractorCompany>(), ComponentType.Exclude<Game.Companies.StorageCompany>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<PropertyRenter>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExistingExtractorQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ExtractorCompany>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<PropertyRenter>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_IndustrialCompanyPrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_SimulationSystem.frameIndex / 16U % 8U != 2U || this.m_IndustrialDemandSystem.industrialCompanyDemand + this.m_IndustrialDemandSystem.storageCompanyDemand + this.m_IndustrialDemandSystem.officeCompanyDemand <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      JobHandle outJobHandle3;
      JobHandle outJobHandle4;
      JobHandle deps1;
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IndustrialSpawnSystem.CheckSpawnJob jobData = new IndustrialSpawnSystem.CheckSpawnJob()
      {
        m_ArchetypeType = this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ProcessType = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle,
        m_IndustrialChunks = this.m_IndustrialCompanyPrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_StorageChunks = this.m_StorageCompanyPrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_ExistingExtractorPrefabs = this.m_ExistingExtractorQuery.ToComponentDataListAsync<PrefabRef>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle3),
        m_ExistingIndustrialPrefabs = this.m_ExistingIndustrialQuery.ToComponentDataListAsync<PrefabRef>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle4),
        m_ProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_Populations = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_ResourceDemands = this.m_IndustrialDemandSystem.GetResourceDemands(out deps1),
        m_WarehouseDemands = this.m_IndustrialDemandSystem.GetStorageCompanyDemands(out deps2),
        m_City = this.m_CitySystem.City,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData.Schedule<IndustrialSpawnSystem.CheckSpawnJob>(JobUtils.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2, outJobHandle3, outJobHandle4, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public IndustrialSpawnSystem()
    {
    }

    [BurstCompile]
    private struct CheckSpawnJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProcessData> m_ProcessType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Prefabs.ArchetypeData> m_ArchetypeType;
      [ReadOnly]
      public NativeArray<int> m_ResourceDemands;
      [ReadOnly]
      public NativeArray<int> m_WarehouseDemands;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_ProcessDatas;
      [ReadOnly]
      public ComponentLookup<Population> m_Populations;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_StorageChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_IndustrialChunks;
      [ReadOnly]
      public NativeList<PrefabRef> m_ExistingExtractorPrefabs;
      [ReadOnly]
      public NativeList<PrefabRef> m_ExistingIndustrialPrefabs;
      public int m_EmptySignatureBuildingCount;
      public Entity m_City;
      public EntityCommandBuffer m_CommandBuffer;
      public uint m_SimulationFrame;

      private void SpawnCompany(
        NativeList<ArchetypeChunk> chunks,
        Resource resource,
        ref Unity.Mathematics.Random random)
      {
        int max = 0;
        for (int index1 = 0; index1 < chunks.Length; ++index1)
        {
          ArchetypeChunk chunk = chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<IndustrialProcessData> nativeArray = chunk.GetNativeArray<IndustrialProcessData>(ref this.m_ProcessType);
          for (int index2 = 0; index2 < chunk.Count; ++index2)
          {
            if ((resource & nativeArray[index2].m_Output.m_Resource) != Resource.NoResource)
              ++max;
          }
        }
        if (max <= 0)
          return;
        int num = random.NextInt(max);
        for (int index3 = 0; index3 < chunks.Length; ++index3)
        {
          ArchetypeChunk chunk = chunks[index3];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<IndustrialProcessData> nativeArray2 = chunk.GetNativeArray<IndustrialProcessData>(ref this.m_ProcessType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Prefabs.ArchetypeData> nativeArray3 = chunk.GetNativeArray<Game.Prefabs.ArchetypeData>(ref this.m_ArchetypeType);
          for (int index4 = 0; index4 < chunk.Count; ++index4)
          {
            if ((resource & nativeArray2[index4].m_Output.m_Resource) != Resource.NoResource)
            {
              if (num == 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.Spawn(nativeArray1[index4], nativeArray3[index4]);
                return;
              }
              --num;
            }
          }
        }
      }

      private void Spawn(Entity prefab, Game.Prefabs.ArchetypeData archetypeData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(this.m_CommandBuffer.CreateEntity(archetypeData.m_Archetype), new PrefabRef()
        {
          m_Prefab = prefab
        });
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = new Unity.Mathematics.Random(this.m_SimulationFrame);
        ResourceIterator iterator = ResourceIterator.GetIterator();
        while (iterator.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator.resource);
          // ISSUE: reference to a compiler-generated field
          int resourceDemand = this.m_ResourceDemands[resourceIndex];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ResourceData resourceData = this.m_ResourceDatas[this.m_ResourcePrefabs[iterator.resource]];
          if (resourceData.m_IsProduceable)
          {
            if (!resourceData.m_IsMaterial)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Population population = this.m_Populations[this.m_City];
              // ISSUE: reference to a compiler-generated field
              if (this.m_EmptySignatureBuildingCount > 0 || random.NextInt(Mathf.RoundToInt(5000f / math.min(5f, math.max(1f, math.log10((float) (1 + population.m_Population)))))) < resourceDemand)
              {
                bool flag = false;
                // ISSUE: reference to a compiler-generated field
                for (int index = 0; index < this.m_ExistingIndustrialPrefabs.Length; ++index)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ProcessDatas[this.m_ExistingIndustrialPrefabs[index].m_Prefab].m_Output.m_Resource == iterator.resource)
                  {
                    flag = true;
                    break;
                  }
                }
                if (!flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.SpawnCompany(this.m_IndustrialChunks, iterator.resource, ref random);
                }
              }
            }
            else
            {
              bool flag = false;
              // ISSUE: reference to a compiler-generated field
              for (int index = 0; index < this.m_ExistingExtractorPrefabs.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ProcessDatas[this.m_ExistingExtractorPrefabs[index].m_Prefab].m_Output.m_Resource == iterator.resource)
                {
                  flag = true;
                  break;
                }
              }
              if (!flag)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.SpawnCompany(this.m_IndustrialChunks, iterator.resource, ref random);
                break;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (resourceData.m_IsTradable && this.m_WarehouseDemands[resourceIndex] > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.SpawnCompany(this.m_StorageChunks, iterator.resource, ref random);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Prefabs.ArchetypeData> __Game_Prefabs_ArchetypeData_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ArchetypeData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Prefabs.ArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
      }
    }
  }
}
