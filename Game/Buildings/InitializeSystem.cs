// Decompiled with JetBrains decompiler
// Type: Game.Buildings.InitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class InitializeSystem : GameSystemBase
  {
    private ModificationBarrier2 m_ModificationBarrier;
    private ElectricityRoadConnectionGraphSystem m_ElectricityRoadConnectionGraphSystem;
    private WaterPipeRoadConnectionGraphSystem m_WaterPipeRoadConnectionGraphSystem;
    private EntityQuery m_CoverageQuery;
    private EntityQuery m_BuildingQuery;
    private ComponentTypeSet m_DestroyedBuildingComponents;
    private InitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<ElectricityRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<WaterPipeRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CoverageQuery = this.GetEntityQuery(ComponentType.ReadOnly<CoverageServiceType>(), ComponentType.ReadOnly<Game.Pathfind.CoverageElement>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Placeholder>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<ServiceUpgrade>(), ComponentType.Exclude<Placeholder>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyedBuildingComponents = new ComponentTypeSet(ComponentType.ReadOnly<ElectricityConsumer>(), ComponentType.ReadOnly<WaterConsumer>(), ComponentType.ReadOnly<GarbageProducer>(), ComponentType.ReadOnly<MailProducer>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CoverageQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        JobHandle producerJob = new InitializeSystem.InitializeCoverageTypeJob()
        {
          m_EntitiesType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_PrefabCoverageData = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup,
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
        }.ScheduleParallel<InitializeSystem.InitializeCoverageTypeJob>(this.m_CoverageQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
        this.Dependency = producerJob;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BuildingQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PostFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new InitializeSystem.InitializeBuildingsJob()
      {
        m_EntitiesType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_PoliceStationType = this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentTypeHandle,
        m_PostFacilityType = this.__TypeHandle.__Game_Buildings_PostFacility_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_AbandonedType = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle,
        m_ElectricityConsumerData = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_WaterConsumerData = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_GarbageProducerData = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
        m_MailProducerData = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
        m_ConsumptionData = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_ObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_DestroyedBuildingComponents = this.m_DestroyedBuildingComponents,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_UpdatedElectricityRoadEdges = this.m_ElectricityRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps1).AsParallelWriter(),
        m_UpdatedWaterPipeRoadEdges = this.m_WaterPipeRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps2).AsParallelWriter()
      }.ScheduleParallel<InitializeSystem.InitializeBuildingsJob>(this.m_BuildingQuery, JobHandle.CombineDependencies(this.Dependency, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ElectricityRoadConnectionGraphSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterPipeRoadConnectionGraphSystem.AddQueueWriter(jobHandle);
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
    public InitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeCoverageTypeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntitiesType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<CoverageData> m_PrefabCoverageData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntitiesType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          PrefabRef prefabRef = nativeArray2[index];
          Entity e = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          CoverageData coverageData = this.m_PrefabCoverageData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetSharedComponent<CoverageServiceType>(unfilteredChunkIndex, e, new CoverageServiceType(coverageData.m_Service));
        }
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

    [BurstCompile]
    private struct InitializeBuildingsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntitiesType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<PoliceStation> m_PoliceStationType;
      [ReadOnly]
      public ComponentTypeHandle<PostFacility> m_PostFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Abandoned> m_AbandonedType;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumerData;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumerData;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducerData;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducerData;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> m_ConsumptionData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_ObjectData;
      public ComponentTypeSet m_DestroyedBuildingComponents;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<Entity>.ParallelWriter m_UpdatedElectricityRoadEdges;
      public NativeQueue<Entity>.ParallelWriter m_UpdatedWaterPipeRoadEdges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntitiesType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray2 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<PoliceStation>(ref this.m_PoliceStationType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<PostFacility>(ref this.m_PostFacilityType);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = chunk.Has<Destroyed>(ref this.m_DestroyedType);
        // ISSUE: reference to a compiler-generated field
        bool flag4 = chunk.Has<Created>(ref this.m_CreatedType);
        // ISSUE: reference to a compiler-generated field
        bool flag5 = chunk.Has<Abandoned>(ref this.m_AbandonedType);
        if (!flag4 && flag3)
          return;
        TypeIndex typeIndex1 = TypeManager.GetTypeIndex<ElectricityConsumer>();
        TypeIndex typeIndex2 = TypeManager.GetTypeIndex<WaterConsumer>();
        TypeIndex typeIndex3 = TypeManager.GetTypeIndex<GarbageProducer>();
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity prefab = nativeArray3[index1].m_Prefab;
          Entity entity = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConsumptionData.HasComponent(prefab) && (this.m_BuildingData[prefab].m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) != (Game.Prefabs.BuildingFlags) 0)
          {
            if (flag4 && !flag1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CrimeProducer>(unfilteredChunkIndex, entity);
            }
            // ISSUE: reference to a compiler-generated field
            if (!flag2 && !flag3 && !flag5 && (flag4 || !this.m_MailProducerData.HasComponent(entity)))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<MailProducer>(unfilteredChunkIndex, entity);
            }
          }
          if (flag3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, entity, in this.m_DestroyedBuildingComponents);
          }
          ObjectData componentData;
          // ISSUE: reference to a compiler-generated field
          if (!flag4 && !flag5 && this.m_ObjectData.TryGetComponent(prefab, out componentData))
          {
            Building building = nativeArray2[index1];
            NativeArray<ComponentType> componentTypes = componentData.m_Archetype.GetComponentTypes();
            for (int index2 = 0; index2 < componentTypes.Length; ++index2)
            {
              ComponentType componentType = componentTypes[index2];
              if (componentType.TypeIndex == typeIndex1)
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_ElectricityConsumerData.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<ElectricityConsumer>(unfilteredChunkIndex, entity);
                  if (building.m_RoadEdge != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_UpdatedElectricityRoadEdges.Enqueue(building.m_RoadEdge);
                  }
                }
              }
              else if (componentType.TypeIndex == typeIndex2)
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_WaterConsumerData.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<WaterConsumer>(unfilteredChunkIndex, entity);
                  if (building.m_RoadEdge != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_UpdatedWaterPipeRoadEdges.Enqueue(building.m_RoadEdge);
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (componentType.TypeIndex == typeIndex3 && !this.m_GarbageProducerData.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<GarbageProducer>(unfilteredChunkIndex, entity);
                }
              }
            }
            componentTypes.Dispose();
          }
        }
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
      [ReadOnly]
      public ComponentLookup<CoverageData> __Game_Prefabs_CoverageData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PoliceStation> __Game_Buildings_PoliceStation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PostFacility> __Game_Buildings_PostFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Abandoned> __Game_Buildings_Abandoned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CoverageData_RO_ComponentLookup = state.GetComponentLookup<CoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PoliceStation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PoliceStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PostFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PostFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
      }
    }
  }
}
