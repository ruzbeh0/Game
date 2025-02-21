// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CommuterSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
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
  public class CommuterSpawnSystem : GameSystemBase
  {
    private EntityQuery m_HouseholdPrefabQuery;
    private EntityQuery m_OutsideConnectionQuery;
    private EntityQuery m_CommuterQuery;
    private EntityQuery m_WorkerQuery;
    private EntityQuery m_DemandParameterQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private CountWorkplacesSystem m_CountWorkplacesSystem;
    private CountHouseholdDataSystem m_CountHouseholdDataSystem;
    private CommuterSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountWorkplacesSystem = this.World.GetOrCreateSystemManaged<CountWorkplacesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountHouseholdDataSystem = this.World.GetOrCreateSystemManaged<CountHouseholdDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<Game.Prefabs.HouseholdData>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(), ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(), ComponentType.Exclude<Building>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CommuterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CommuterHousehold>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_WorkerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Worker>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdPrefabQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_OutsideConnectionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      DemandParameterData singleton = this.m_DemandParameterQuery.GetSingleton<DemandParameterData>();
      // ISSUE: reference to a compiler-generated field
      int entityCount1 = this.m_CommuterQuery.CalculateEntityCount();
      // ISSUE: reference to a compiler-generated field
      int entityCount2 = this.m_WorkerQuery.CalculateEntityCount();
      int workerRatioLimit = singleton.m_CommuterWorkerRatioLimit;
      if (entityCount1 * workerRatioLimit >= entityCount2)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      JobHandle outJobHandle3;
      JobHandle outJobHandle4;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CommuterSpawnSystem.SpawnCommuterHouseholdJob jobData = new CommuterSpawnSystem.SpawnCommuterHouseholdJob()
      {
        m_PrefabEntities = this.m_HouseholdPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_Archetypes = this.m_HouseholdPrefabQuery.ToComponentDataListAsync<Game.Prefabs.ArchetypeData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_HouseholdPrefabs = this.m_HouseholdPrefabQuery.ToComponentDataListAsync<Game.Prefabs.HouseholdData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle3),
        m_OutsideConnectionEntities = this.m_OutsideConnectionQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle4),
        m_Employables = this.m_CountHouseholdDataSystem.GetEmployables(),
        m_FreeWorkplaces = this.m_CountWorkplacesSystem.GetFreeWorkplaces(),
        m_DemandParameterData = singleton,
        m_OutsideConnectionDatas = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Frame = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_RandomSeed = RandomSeed.Next()
      };
      this.Dependency = jobData.Schedule<CommuterSpawnSystem.SpawnCommuterHouseholdJob>(JobUtils.CombineDependencies(outJobHandle1, outJobHandle2, outJobHandle3, this.Dependency, outJobHandle4));
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
    public CommuterSpawnSystem()
    {
    }

    [BurstCompile]
    private struct SpawnCommuterHouseholdJob : IJob
    {
      [ReadOnly]
      public NativeList<Entity> m_PrefabEntities;
      [ReadOnly]
      public NativeList<Game.Prefabs.ArchetypeData> m_Archetypes;
      [ReadOnly]
      public NativeList<Game.Prefabs.HouseholdData> m_HouseholdPrefabs;
      [ReadOnly]
      public NativeList<Entity> m_OutsideConnectionEntities;
      [ReadOnly]
      public Workplaces m_FreeWorkplaces;
      [ReadOnly]
      public NativeArray<int> m_Employables;
      [ReadOnly]
      public DemandParameterData m_DemandParameterData;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      public uint m_Frame;
      public EntityCommandBuffer m_CommandBuffer;
      public RandomSeed m_RandomSeed;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom((int) this.m_Frame);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = (this.m_FreeWorkplaces[2] + this.m_FreeWorkplaces[3] + this.m_FreeWorkplaces[4] - (this.m_Employables[2] + this.m_Employables[3] + this.m_Employables[4])) / this.m_DemandParameterData.m_CommuterSlowSpawnFactor;
        for (int index1 = 0; index1 < num; ++index1)
        {
          Entity result;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OutsideConnectionEntities.Length > 0 && BuildingUtils.GetRandomOutsideConnectionByParameters(ref this.m_OutsideConnectionEntities, ref this.m_OutsideConnectionDatas, ref this.m_PrefabRefs, random, this.m_DemandParameterData.m_CommuterOCSpawnParameters, out result))
          {
            // ISSUE: reference to a compiler-generated field
            int index2 = random.NextInt(this.m_HouseholdPrefabs.Length);
            // ISSUE: reference to a compiler-generated field
            Entity prefabEntity = this.m_PrefabEntities[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(this.m_Archetypes[index2].m_Archetype);
            PrefabRef component1 = new PrefabRef()
            {
              m_Prefab = prefabEntity
            };
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(entity, component1);
            Household component2 = new Household()
            {
              m_Flags = HouseholdFlags.Commuter
            };
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Household>(entity, component2);
            CurrentBuilding component3 = new CurrentBuilding()
            {
              m_CurrentBuilding = result
            };
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CommuterHousehold>(entity, new CommuterHousehold()
            {
              m_OriginalFrom = result
            });
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CurrentBuilding>(entity, component3);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
      }
    }
  }
}
