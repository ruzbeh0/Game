// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ParkAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ParkAISystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 256;
    private CitySystem m_CitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_ParkQuery;
    private EntityArchetype m_MaintenanceRequestArchetype;
    private ParkAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / ParkAISystem.kUpdatesPerDay;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ParkQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.Park>(), ComponentType.ReadWrite<ModifiedServiceCoverage>(), ComponentType.ReadOnly<Renter>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<MaintenanceRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ParkQuery);
      // ISSUE: reference to a compiler-generated field
      Assert.IsTrue(262144 / ParkAISystem.kUpdatesPerDay >= 512);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new ParkAISystem.ParkTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ParkType = this.__TypeHandle.__Game_Buildings_Park_RW_ComponentTypeHandle,
        m_MaintenanceConsumerType = this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle,
        m_ModifiedServiceCoverageType = this.__TypeHandle.__Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_CoverageDatas = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup,
        m_ParkDatas = this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup,
        m_MaintenanceRequestData = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_City = this.m_CitySystem.City,
        m_MaintenanceRequestArchetype = this.m_MaintenanceRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<ParkAISystem.ParkTickJob>(this.m_ParkQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
    }

    public static int GetMaintenancePriority(Game.Buildings.Park park, ParkData prefabParkData)
    {
      return (int) prefabParkData.m_MaintenancePool - (int) park.m_Maintenance - (int) prefabParkData.m_MaintenancePool / 10;
    }

    public static ModifiedServiceCoverage GetModifiedServiceCoverage(
      Game.Buildings.Park park,
      ParkData prefabParkData,
      CoverageData prefabCoverageData,
      DynamicBuffer<CityModifier> cityModifiers)
    {
      double num = (double) park.m_Maintenance / (double) math.max(1, (int) prefabParkData.m_MaintenancePool);
      ModifiedServiceCoverage modifiedServiceCoverage = new ModifiedServiceCoverage(prefabCoverageData);
      int y = Mathf.FloorToInt((float) (num / 0.30000001192092896));
      modifiedServiceCoverage.m_Magnitude *= (float) (0.949999988079071 + 0.05000000074505806 * (double) math.min(1, y) + 0.10000000149011612 * (double) math.max(0, y - 1));
      modifiedServiceCoverage.m_Range *= (float) (0.949999988079071 + 0.05000000074505806 * (double) y);
      if (cityModifiers.IsCreated)
        CityUtils.ApplyModifier(ref modifiedServiceCoverage.m_Magnitude, cityModifiers, CityModifierType.ParkEntertainment);
      return modifiedServiceCoverage;
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
    public ParkAISystem()
    {
    }

    [BurstCompile]
    private struct ParkTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Game.Buildings.Park> m_ParkType;
      [ReadOnly]
      public ComponentTypeHandle<MaintenanceConsumer> m_MaintenanceConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<ModifiedServiceCoverage> m_ModifiedServiceCoverageType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentLookup<CoverageData> m_CoverageDatas;
      [ReadOnly]
      public ComponentLookup<ParkData> m_ParkDatas;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public EntityArchetype m_MaintenanceRequestArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.Park> nativeArray2 = chunk.GetNativeArray<Game.Buildings.Park>(ref this.m_ParkType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<MaintenanceConsumer> nativeArray3 = chunk.GetNativeArray<MaintenanceConsumer>(ref this.m_MaintenanceConsumerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ModifiedServiceCoverage> nativeArray5 = chunk.GetNativeArray<ModifiedServiceCoverage>(ref this.m_ModifiedServiceCoverageType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity prefab = nativeArray4[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ParkData parkData = this.m_ParkDatas[prefab];
            Game.Buildings.Park park = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            park.m_Maintenance = (short) math.max(0, (int) park.m_Maintenance - (400 + 50 * bufferAccessor[index].Length) / ParkAISystem.kUpdatesPerDay);
            nativeArray2[index] = park;
            if (nativeArray3.Length != 0)
            {
              MaintenanceConsumer maintenanceConsumer = nativeArray3[index];
              // ISSUE: reference to a compiler-generated method
              this.RequestMaintenanceIfNeeded(unfilteredChunkIndex, entity, park, maintenanceConsumer, parkData);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_CoverageDatas.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              CoverageData coverageData = this.m_CoverageDatas[prefab];
              // ISSUE: reference to a compiler-generated method
              nativeArray5[index] = ParkAISystem.GetModifiedServiceCoverage(park, parkData, coverageData, cityModifier);
            }
          }
        }
      }

      private void RequestMaintenanceIfNeeded(
        int jobIndex,
        Entity entity,
        Game.Buildings.Park park,
        MaintenanceConsumer maintenanceConsumer,
        ParkData prefabParkData)
      {
        // ISSUE: reference to a compiler-generated method
        int maintenancePriority = ParkAISystem.GetMaintenancePriority(park, prefabParkData);
        // ISSUE: reference to a compiler-generated field
        if (maintenancePriority <= 0 || this.m_MaintenanceRequestData.HasComponent(maintenanceConsumer.m_Request))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MaintenanceRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<MaintenanceRequest>(jobIndex, entity1, new MaintenanceRequest(entity, maintenancePriority));
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Buildings.Park> __Game_Buildings_Park_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MaintenanceConsumer> __Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ModifiedServiceCoverage> __Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<CoverageData> __Game_Prefabs_CoverageData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkData> __Game_Prefabs_ParkData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Park>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MaintenanceConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ModifiedServiceCoverage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CoverageData_RO_ComponentLookup = state.GetComponentLookup<CoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkData_RO_ComponentLookup = state.GetComponentLookup<ParkData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
