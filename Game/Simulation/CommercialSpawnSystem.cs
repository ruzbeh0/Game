// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CommercialSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CommercialSpawnSystem : GameSystemBase
  {
    private EntityQuery m_CommercialCompanyPrefabGroup;
    private EntityQuery m_PropertyLessCompanyGroup;
    private EntityQuery m_DemandParameterQuery;
    private CommercialDemandSystem m_CommercialDemandSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private NativeArray<uint> m_LastSpawnedCommercialFrame;
    private CommercialSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialDemandSystem = this.World.GetOrCreateSystemManaged<CommercialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialCompanyPrefabGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<CommercialCompanyData>(), ComponentType.ReadOnly<IndustrialProcessData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PropertyLessCompanyGroup = this.GetEntityQuery(ComponentType.ReadOnly<CommercialCompany>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<PropertyRenter>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_LastSpawnedCommercialFrame = new NativeArray<uint>(EconomyUtils.ResourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CommercialCompanyPrefabGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_LastSpawnedCommercialFrame.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_SimulationSystem.frameIndex / 16U % 8U != 1U || this.m_CommercialDemandSystem.companyDemand <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CommercialSpawnSystem.SpawnCompanyJob jobData = new CommercialSpawnSystem.SpawnCompanyJob()
      {
        m_CompanyPrefabs = this.m_CommercialCompanyPrefabGroup.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_PropertyLessCompanies = this.m_PropertyLessCompanyGroup.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_Archetypes = this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup,
        m_Processes = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ResourceDemands = this.m_CommercialDemandSystem.GetResourceDemands(out deps),
        m_Random = RandomSeed.Next().GetRandom((int) this.m_SimulationSystem.frameIndex),
        m_FrameIndex = this.m_SimulationSystem.frameIndex,
        m_LastSpawnedCommercialFrame = this.m_LastSpawnedCommercialFrame,
        m_DemandParameterData = this.m_DemandParameterQuery.GetSingleton<DemandParameterData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData.Schedule<CommercialSpawnSystem.SpawnCompanyJob>(JobUtils.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CommercialDemandSystem.AddReader(this.Dependency);
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
    public CommercialSpawnSystem()
    {
    }

    [BurstCompile]
    private struct SpawnCompanyJob : IJob
    {
      [ReadOnly]
      public NativeList<Entity> m_CompanyPrefabs;
      [ReadOnly]
      public NativeList<Entity> m_PropertyLessCompanies;
      [ReadOnly]
      public NativeArray<int> m_ResourceDemands;
      [ReadOnly]
      public Random m_Random;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public DemandParameterData m_DemandParameterData;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_Processes;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> m_Archetypes;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      public int m_EmptySignatureBuildingCount;
      public NativeArray<uint> m_LastSpawnedCommercialFrame;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        ResourceIterator iterator = ResourceIterator.GetIterator();
        while (iterator.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator.resource);
          // ISSUE: reference to a compiler-generated field
          int resourceDemand = this.m_ResourceDemands[resourceIndex];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_EmptySignatureBuildingCount > 0 || resourceDemand > 0 && (long) (this.m_FrameIndex - this.m_LastSpawnedCommercialFrame[resourceIndex]) > (long) this.m_DemandParameterData.m_FrameIntervalForSpawning.y)
          {
            bool flag = false;
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_PropertyLessCompanies.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              Entity propertyLessCompany = this.m_PropertyLessCompanies[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_Prefabs.HasComponent(propertyLessCompany))
              {
                // ISSUE: reference to a compiler-generated field
                Entity prefab = this.m_Prefabs[propertyLessCompany].m_Prefab;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Processes.HasComponent(prefab) && this.m_Processes[prefab].m_Output.m_Resource == iterator.resource)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
            {
              // ISSUE: reference to a compiler-generated method
              this.SpawnCompany(iterator.resource);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_LastSpawnedCommercialFrame[resourceIndex] = this.m_FrameIndex;
            }
          }
        }
      }

      private void SpawnCompany(Resource resource)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CompanyPrefabs.Length <= 0)
          return;
        int max = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_CompanyPrefabs.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((resource & this.m_Processes[this.m_CompanyPrefabs[index]].m_Output.m_Resource) != Resource.NoResource)
            ++max;
        }
        if (max == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        int num = this.m_Random.NextInt(max);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_CompanyPrefabs.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((resource & this.m_Processes[this.m_CompanyPrefabs[index]].m_Output.m_Resource) != Resource.NoResource)
          {
            if (num == 0)
            {
              // ISSUE: reference to a compiler-generated field
              Entity companyPrefab = this.m_CompanyPrefabs[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(this.m_CommandBuffer.CreateEntity(this.m_Archetypes[companyPrefab].m_Archetype), new PrefabRef()
              {
                m_Prefab = companyPrefab
              });
            }
            --num;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> __Game_Prefabs_ArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ArchetypeData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.ArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
      }
    }
  }
}
