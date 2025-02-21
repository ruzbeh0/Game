// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WorkCarAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WorkCarAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EntityQuery m_VehicleQuery;
    private WorkCarAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 12;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Vehicles.WorkVehicle>(), ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<WorkCarAISystem.WorkAction> nativeQueue = new NativeQueue<WorkCarAISystem.WorkAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WorkVehicle_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WorkCarAISystem.WorkCarTickJob jobData1 = new WorkCarAISystem.WorkCarTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_PlantData = this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabWorkVehicleData = this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup,
        m_PrefabTreeData = this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_WorkVehicleData = this.__TypeHandle.__Game_Vehicles_WorkVehicle_RW_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_WorkQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WorkCarAISystem.WorkCarWorkJob jobData2 = new WorkCarAISystem.WorkCarWorkJob()
      {
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RW_ComponentLookup,
        m_ExtractorData = this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup,
        m_WorkQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<WorkCarAISystem.WorkCarTickJob>(this.m_VehicleQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<WorkCarAISystem.WorkCarWorkJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = inputDeps;
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
    public WorkCarAISystem()
    {
    }

    private struct WorkAction
    {
      public VehicleWorkType m_WorkType;
      public Entity m_Target;
      public Entity m_Owner;
      public float m_WorkAmount;
    }

    [BurstCompile]
    private struct WorkCarTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentTypeHandle<Car> m_CarType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Plant> m_PlantData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_DamagedData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> m_PrefabWorkVehicleData;
      [ReadOnly]
      public ComponentLookup<TreeData> m_PrefabTreeData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.WorkVehicle> m_WorkVehicleData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<WorkCarAISystem.WorkAction>.ParallelWriter m_WorkQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathInformation> nativeArray3 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray5 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray6 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray7 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray8 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor1 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor2 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PathInformation pathInformation = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          Car car = nativeArray6[index];
          CarCurrentLane currentLane = nativeArray5[index];
          PathOwner pathOwner = nativeArray8[index];
          Target target = nativeArray7[index];
          DynamicBuffer<PathElement> path = bufferAccessor2[index];
          DynamicBuffer<LayoutElement> layout = new DynamicBuffer<LayoutElement>();
          if (bufferAccessor1.Length != 0)
            layout = bufferAccessor1[index];
          // ISSUE: reference to a compiler-generated field
          Game.Vehicles.WorkVehicle workVehicle = this.m_WorkVehicleData[entity];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, pathInformation, prefabRef, layout, path, ref workVehicle, ref car, ref currentLane, ref pathOwner, ref target);
          // ISSUE: reference to a compiler-generated field
          this.m_WorkVehicleData[entity] = workVehicle;
          nativeArray6[index] = car;
          nativeArray5[index] = currentLane;
          nativeArray8[index] = pathOwner;
          nativeArray7[index] = target;
        }
      }

      private void Tick(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        PathInformation pathInformation,
        PrefabRef prefabRef,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<PathElement> path,
        ref Game.Vehicles.WorkVehicle workVehicle,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated method
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner) && !this.ResetPath(jobIndex, vehicleEntity, pathInformation, path, ref workVehicle, ref car, ref currentLane, ref target))
        {
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(jobIndex, vehicleEntity, owner, ref workVehicle, ref car, ref pathOwner, ref target);
          // ISSUE: reference to a compiler-generated method
          this.FindPathIfNeeded(vehicleEntity, owner, prefabRef, layout, ref workVehicle, ref car, ref currentLane, ref pathOwner, ref target);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
          {
            if (VehicleUtils.IsStuck(pathOwner) || (workVehicle.m_State & WorkVehicleFlags.Returning) != (WorkVehicleFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
              return;
            }
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(jobIndex, vehicleEntity, owner, ref workVehicle, ref car, ref pathOwner, ref target);
          }
          else if (VehicleUtils.PathEndReached(currentLane))
          {
            if ((workVehicle.m_State & WorkVehicleFlags.Returning) != (WorkVehicleFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
              return;
            }
            // ISSUE: reference to a compiler-generated method
            if (this.PerformWork(jobIndex, vehicleEntity, owner, prefabRef, layout, ref workVehicle, ref target, ref pathOwner))
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, vehicleEntity, owner, ref workVehicle, ref car, ref pathOwner, ref target);
            }
          }
          car.m_Flags |= CarFlags.Warning | CarFlags.Working;
          // ISSUE: reference to a compiler-generated method
          this.FindPathIfNeeded(vehicleEntity, owner, prefabRef, layout, ref workVehicle, ref car, ref currentLane, ref pathOwner, ref target);
        }
      }

      private void FindPathIfNeeded(
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.WorkVehicle workVehicle,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if (!VehicleUtils.RequireNewPath(pathOwner))
          return;
        // ISSUE: reference to a compiler-generated field
        CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) carData.m_MaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road | PathMethod.Offroad,
          m_IgnoredRules = VehicleUtils.GetIgnoredPathfindRules(carData)
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.Offroad;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.Offroad;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_Entity = target.m_Target;
        SetupQueueTarget destination = setupQueueTarget;
        WorkVehicleData workVehicleData1;
        if (layout.IsCreated && layout.Length != 0)
        {
          workVehicleData1 = new WorkVehicleData();
          for (int index = 0; index < layout.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WorkVehicleData workVehicleData2 = this.m_PrefabWorkVehicleData[this.m_PrefabRefData[layout[index].m_Vehicle].m_Prefab];
            if (workVehicleData2.m_WorkType != VehicleWorkType.None)
              workVehicleData1.m_WorkType = workVehicleData2.m_WorkType;
            if (workVehicleData2.m_MapFeature != MapFeature.None)
              workVehicleData1.m_MapFeature = workVehicleData2.m_MapFeature;
            workVehicleData1.m_Resources |= workVehicleData2.m_Resources;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          workVehicleData1 = this.m_PrefabWorkVehicleData[prefabRef.m_Prefab];
        }
        if ((workVehicle.m_State & (WorkVehicleFlags.Returning | WorkVehicleFlags.ExtractorVehicle)) == WorkVehicleFlags.ExtractorVehicle)
        {
          destination.m_Type = workVehicleData1.m_MapFeature != MapFeature.Forest ? SetupTargetType.AreaLocation : SetupTargetType.WoodResource;
          destination.m_Entity = owner.m_Owner;
          destination.m_Value = (int) workVehicleData1.m_WorkType;
        }
        else if ((workVehicle.m_State & (WorkVehicleFlags.Returning | WorkVehicleFlags.StorageVehicle)) == WorkVehicleFlags.StorageVehicle)
        {
          destination.m_Type = SetupTargetType.AreaLocation;
          destination.m_Entity = owner.m_Owner;
          destination.m_Value = (int) workVehicleData1.m_WorkType;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private bool ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        PathInformation pathInformation,
        DynamicBuffer<PathElement> path,
        ref Game.Vehicles.WorkVehicle workVehicle,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathUtils.ResetPath(ref currentLane, path, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes);
        if ((workVehicle.m_State & WorkVehicleFlags.Returning) != (WorkVehicleFlags) 0)
        {
          car.m_Flags &= ~CarFlags.StayOnRoad;
        }
        else
        {
          car.m_Flags |= CarFlags.StayOnRoad;
          target.m_Target = pathInformation.m_Destination;
        }
        return true;
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        Owner ownerData,
        ref Game.Vehicles.WorkVehicle workVehicle,
        ref Car car,
        ref PathOwner pathOwner,
        ref Target target)
      {
        workVehicle.m_State |= WorkVehicleFlags.Returning;
        Entity newTarget = ownerData.m_Owner;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(ownerData.m_Owner))
        {
          // ISSUE: reference to a compiler-generated field
          Owner owner = this.m_OwnerData[ownerData.m_Owner];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          newTarget = !this.m_AttachmentData.HasComponent(owner.m_Owner) ? owner.m_Owner : this.m_AttachmentData[owner.m_Owner].m_Attached;
        }
        VehicleUtils.SetTarget(ref pathOwner, ref target, newTarget);
      }

      private bool PerformWork(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.WorkVehicle workVehicle,
        ref Target target,
        ref PathOwner pathOwner)
      {
        WorkVehicleData workVehicleData1;
        if (layout.IsCreated && layout.Length != 0)
        {
          workVehicleData1 = new WorkVehicleData();
          for (int index = 0; index < layout.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WorkVehicleData workVehicleData2 = this.m_PrefabWorkVehicleData[this.m_PrefabRefData[layout[index].m_Vehicle].m_Prefab];
            if (workVehicleData2.m_WorkType != VehicleWorkType.None)
            {
              workVehicleData1.m_WorkType = workVehicleData2.m_WorkType;
              workVehicleData1.m_MaxWorkAmount += workVehicleData2.m_MaxWorkAmount;
            }
            if (workVehicleData2.m_MapFeature != MapFeature.None)
              workVehicleData1.m_MapFeature = workVehicleData2.m_MapFeature;
            workVehicleData1.m_Resources |= workVehicleData2.m_Resources;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          workVehicleData1 = this.m_PrefabWorkVehicleData[prefabRef.m_Prefab];
        }
        float x = workVehicleData1.m_MaxWorkAmount;
        if ((workVehicle.m_State & WorkVehicleFlags.ExtractorVehicle) != (WorkVehicleFlags) 0)
        {
          switch (workVehicleData1.m_WorkType)
          {
            case VehicleWorkType.Harvest:
              x = 1000f;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TreeData.HasComponent(target.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                Tree tree = this.m_TreeData[target.m_Target];
                // ISSUE: reference to a compiler-generated field
                Plant plant = this.m_PlantData[target.m_Target];
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef1 = this.m_PrefabRefData[target.m_Target];
                Damaged componentData1;
                // ISSUE: reference to a compiler-generated field
                this.m_DamagedData.TryGetComponent(target.m_Target, out componentData1);
                TreeData componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabTreeData.TryGetComponent(prefabRef1.m_Prefab, out componentData2))
                  x = ObjectUtils.CalculateWoodAmount(tree, plant, componentData1, componentData2);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, target.m_Target, new BatchesUpdated());
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_WorkQueue.Enqueue(new WorkCarAISystem.WorkAction()
              {
                m_WorkType = workVehicleData1.m_WorkType,
                m_Target = target.m_Target,
                m_Owner = owner.m_Owner,
                m_WorkAmount = x
              });
              break;
            case VehicleWorkType.Collect:
              // ISSUE: reference to a compiler-generated field
              if (this.m_TreeData.HasComponent(target.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_WorkQueue.Enqueue(new WorkCarAISystem.WorkAction()
                {
                  m_WorkType = workVehicleData1.m_WorkType,
                  m_Target = target.m_Target
                });
              }
              x = workVehicleData1.m_MaxWorkAmount * 0.25f;
              break;
          }
        }
        else if ((workVehicle.m_State & WorkVehicleFlags.StorageVehicle) != (WorkVehicleFlags) 0)
          x = workVehicleData1.m_MaxWorkAmount * 0.25f;
        VehicleUtils.SetTarget(ref pathOwner, ref target, Entity.Null);
        if (layout.IsCreated && layout.Length != 0)
        {
          float num1 = 0.0f;
          float num2 = 0.0f;
          for (int index = 0; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            if (vehicle == vehicleEntity)
            {
              float num3 = math.min(x, workVehicle.m_WorkAmount - workVehicle.m_DoneAmount);
              if ((double) num3 > 0.0)
              {
                workVehicle.m_DoneAmount += num3;
                x -= num3;
                // ISSUE: reference to a compiler-generated method
                this.QuantityUpdated(jobIndex, vehicle);
              }
              num1 += workVehicle.m_DoneAmount;
              num2 += workVehicle.m_WorkAmount;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.WorkVehicle workVehicle1 = this.m_WorkVehicleData[vehicle];
              float num4 = math.min(x, workVehicle1.m_WorkAmount - workVehicle1.m_DoneAmount);
              if ((double) num4 > 0.0)
              {
                workVehicle1.m_DoneAmount += num4;
                x -= num4;
                // ISSUE: reference to a compiler-generated field
                this.m_WorkVehicleData[vehicle] = workVehicle1;
                // ISSUE: reference to a compiler-generated method
                this.QuantityUpdated(jobIndex, vehicle);
              }
              num1 += workVehicle1.m_DoneAmount;
              num2 += workVehicle1.m_WorkAmount;
            }
          }
          if ((double) x < 1.0)
            return (double) num1 > (double) num2 - 1.0;
          for (int index = 0; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            if (vehicle == vehicleEntity)
            {
              if ((double) workVehicle.m_WorkAmount >= 1.0)
                workVehicle.m_DoneAmount += x * workVehicle.m_WorkAmount / num2;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.WorkVehicle workVehicle2 = this.m_WorkVehicleData[vehicle];
              if ((double) workVehicle2.m_WorkAmount >= 1.0)
              {
                workVehicle2.m_DoneAmount += x * workVehicle2.m_WorkAmount / num2;
                // ISSUE: reference to a compiler-generated field
                this.m_WorkVehicleData[vehicle] = workVehicle2;
              }
            }
          }
          return true;
        }
        // ISSUE: reference to a compiler-generated method
        this.QuantityUpdated(jobIndex, vehicleEntity);
        workVehicle.m_DoneAmount += x;
        return (double) workVehicle.m_DoneAmount > (double) workVehicle.m_WorkAmount - 1.0;
      }

      private void QuantityUpdated(int jobIndex, Entity vehicleEntity)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(vehicleEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[vehicleEntity];
        for (int index = 0; index < subObject1.Length; ++index)
        {
          Entity subObject2 = subObject1[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, subObject2, new BatchesUpdated());
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
    private struct WorkCarWorkJob : IJob
    {
      public ComponentLookup<Tree> m_TreeData;
      public ComponentLookup<Extractor> m_ExtractorData;
      public NativeQueue<WorkCarAISystem.WorkAction> m_WorkQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_WorkQueue.Count;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          WorkCarAISystem.WorkAction workAction = this.m_WorkQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          switch (workAction.m_WorkType)
          {
            case VehicleWorkType.Harvest:
              float num = 0.0f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TreeData.HasComponent(workAction.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Tree tree = this.m_TreeData[workAction.m_Target];
                if ((tree.m_State & TreeState.Stump) == (TreeState) 0)
                {
                  tree.m_State &= ~(TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Collected);
                  tree.m_State |= TreeState.Stump;
                  tree.m_Growth = (byte) 0;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_TreeData[workAction.m_Target] = tree;
                  // ISSUE: reference to a compiler-generated field
                  num = workAction.m_WorkAmount;
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ExtractorData.HasComponent(workAction.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Extractor extractor = this.m_ExtractorData[workAction.m_Owner];
                extractor.m_ExtractedAmount -= num;
                // ISSUE: reference to a compiler-generated field
                extractor.m_HarvestedAmount += workAction.m_WorkAmount;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_ExtractorData[workAction.m_Owner] = extractor;
                break;
              }
              break;
            case VehicleWorkType.Collect:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TreeData.HasComponent(workAction.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Tree tree = this.m_TreeData[workAction.m_Target];
                if ((tree.m_State & TreeState.Collected) == (TreeState) 0)
                {
                  tree.m_State |= TreeState.Collected;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_TreeData[workAction.m_Target] = tree;
                  break;
                }
                break;
              }
              break;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Plant> __Game_Objects_Plant_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> __Game_Prefabs_WorkVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentLookup<Game.Vehicles.WorkVehicle> __Game_Vehicles_WorkVehicle_RW_ComponentLookup;
      public ComponentLookup<Tree> __Game_Objects_Tree_RW_ComponentLookup;
      public ComponentLookup<Extractor> __Game_Areas_Extractor_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Plant_RO_ComponentLookup = state.GetComponentLookup<Plant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup = state.GetComponentLookup<WorkVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WorkVehicle_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.WorkVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RW_ComponentLookup = state.GetComponentLookup<Tree>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RW_ComponentLookup = state.GetComponentLookup<Extractor>();
      }
    }
  }
}
