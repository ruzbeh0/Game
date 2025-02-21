// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CrimeAccumulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CrimeAccumulationSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private CitySystem m_CitySystem;
    private LocalEffectSystem m_LocalEffectSystem;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_CrimeProducerQuery;
    private EntityQuery m_PoliceConfigurationQuery;
    private EntityArchetype m_PatrolRequestArchetype;
    private CrimeAccumulationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalEffectSystem = this.World.GetOrCreateSystemManaged<LocalEffectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProducerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<CrimeProducer>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PatrolRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PolicePatrolRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CrimeProducerQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      PoliceConfigurationData singleton = this.m_PoliceConfigurationQuery.GetSingleton<PoliceConfigurationData>();
      if (this.EntityManager.HasEnabledComponent<Locked>(singleton.m_PoliceServicePrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex / 64U & 15U;
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProducerQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProducerQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CrimeAccumulationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new CrimeAccumulationSystem.CrimeAccumulationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CrimeProducerType = this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentTypeHandle,
        m_PolicePatrolRequestData = this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup,
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_CrimeAccumulationData = this.__TypeHandle.__Game_Prefabs_CrimeAccumulationData_RO_ComponentLookup,
        m_ServiceObjectData = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_ServiceCoverages = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_City = this.m_CitySystem.City,
        m_PatrolRequestArchetype = this.m_PatrolRequestArchetype,
        m_PoliceConfigurationData = singleton,
        m_LocalEffectData = this.m_LocalEffectSystem.GetReadData(out dependencies),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_RandomSeed = RandomSeed.Next()
      }.ScheduleParallel<CrimeAccumulationSystem.CrimeAccumulationJob>(this.m_CrimeProducerQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LocalEffectSystem.AddLocalEffectReader(jobHandle);
      this.Dependency = jobHandle;
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
    public CrimeAccumulationSystem()
    {
    }

    [BurstCompile]
    private struct CrimeAccumulationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<CrimeProducer> m_CrimeProducerType;
      [ReadOnly]
      public ComponentLookup<PolicePatrolRequest> m_PolicePatrolRequestData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<CrimeAccumulationData> m_CrimeAccumulationData;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjectData;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverages;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public EntityArchetype m_PatrolRequestArchetype;
      [ReadOnly]
      public PoliceConfigurationData m_PoliceConfigurationData;
      [ReadOnly]
      public LocalEffectSystem.ReadData m_LocalEffectData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray2 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray4 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CrimeProducer> nativeArray6 = chunk.GetNativeArray<CrimeProducer>(ref this.m_CrimeProducerType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          Transform transform = nativeArray3[index];
          // ISSUE: reference to a compiler-generated method
          float baseCrimeRate = this.GetBaseCrimeRate(nativeArray5[index].m_Prefab);
          if ((double) baseCrimeRate != 0.0)
          {
            if (nativeArray2.Length != 0)
            {
              Building building = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_ServiceCoverages.HasBuffer(building.m_RoadEdge))
              {
                // ISSUE: reference to a compiler-generated field
                float serviceCoverage = NetUtils.GetServiceCoverage(this.m_ServiceCoverages[building.m_RoadEdge], CoverageService.Police, building.m_CurvePosition);
                // ISSUE: reference to a compiler-generated field
                baseCrimeRate *= (float) ((double) this.m_PoliceConfigurationData.m_CrimeIncreaseMultiplier * (double) math.max(0.0f, 10f - serviceCoverage) / 10.0);
              }
              else if (building.m_RoadEdge == Entity.Null)
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_LocalEffectData.ApplyModifier(ref baseCrimeRate, transform.m_Position, LocalModifierType.CrimeAccumulation);
            if (nativeArray4.Length != 0)
            {
              CurrentDistrict currentDistrict = nativeArray4[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_DistrictModifiers.HasBuffer(currentDistrict.m_District))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<DistrictModifier> districtModifier = this.m_DistrictModifiers[currentDistrict.m_District];
                AreaUtils.ApplyModifier(ref baseCrimeRate, districtModifier, DistrictModifierType.CrimeAccumulation);
              }
            }
            CityUtils.ApplyModifier(ref baseCrimeRate, cityModifier, CityModifierType.CrimeAccumulation);
            float num = math.max(0.0f, baseCrimeRate);
            CrimeProducer producer = nativeArray6[index];
            // ISSUE: reference to a compiler-generated field
            producer.m_Crime = math.min(this.m_PoliceConfigurationData.m_MaxCrimeAccumulation, producer.m_Crime + num);
            // ISSUE: reference to a compiler-generated method
            this.RequestPatrolIfNeeded(unfilteredChunkIndex, entity, ref producer, ref random);
            nativeArray6[index] = producer;
          }
        }
      }

      private float GetBaseCrimeRate(Entity prefab)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnableBuildingData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingData[prefab];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CrimeAccumulationData.HasComponent(spawnableBuildingData.m_ZonePrefab))
          {
            // ISSUE: reference to a compiler-generated field
            return this.m_CrimeAccumulationData[spawnableBuildingData.m_ZonePrefab].m_CrimeRate;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceObjectData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ServiceObjectData serviceObjectData = this.m_ServiceObjectData[prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_CrimeAccumulationData.HasComponent(serviceObjectData.m_Service))
            {
              // ISSUE: reference to a compiler-generated field
              return this.m_CrimeAccumulationData[serviceObjectData.m_Service].m_CrimeRate;
            }
          }
        }
        return 0.0f;
      }

      private void RequestPatrolIfNeeded(
        int jobIndex,
        Entity entity,
        ref CrimeProducer producer,
        ref Random random)
      {
        PolicePatrolRequest componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) producer.m_Crime < (double) this.m_PoliceConfigurationData.m_CrimeAccumulationTolerance || this.m_PolicePatrolRequestData.TryGetComponent(producer.m_PatrolRequest, out componentData) && (componentData.m_Target == entity || (int) componentData.m_DispatchIndex == (int) producer.m_DispatchIndex))
          return;
        producer.m_PatrolRequest = Entity.Null;
        producer.m_DispatchIndex = (byte) 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PatrolRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PolicePatrolRequest>(jobIndex, entity1, new PolicePatrolRequest(entity, producer.m_Crime / this.m_PoliceConfigurationData.m_MaxCrimeAccumulation));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<CrimeProducer> __Game_Buildings_CrimeProducer_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PolicePatrolRequest> __Game_Simulation_PolicePatrolRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CrimeAccumulationData> __Game_Prefabs_CrimeAccumulationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CrimeProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup = state.GetComponentLookup<PolicePatrolRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CrimeAccumulationData_RO_ComponentLookup = state.GetComponentLookup<CrimeAccumulationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
