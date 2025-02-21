// Decompiled with JetBrains decompiler
// Type: Game.Net.FixLaneObjectsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Creatures;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
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
namespace Game.Net
{
  [CompilerGenerated]
  public class FixLaneObjectsSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private EntityQuery m_LaneQuery;
    private LaneObjectUpdater m_LaneObjectUpdater;
    private FixLaneObjectsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<LaneObject>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<ParkingLane>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LaneObjectUpdater = new LaneObjectUpdater((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LaneQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      FixLaneObjectsSystem.CollectLaneObjectsJob jobData = new FixLaneObjectsSystem.CollectLaneObjectsJob()
      {
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_CarCurrentLaneData = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup,
        m_CarTrailerLaneData = this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentLookup,
        m_HumanCurrentLane = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup,
        m_AnimalCurrentLane = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup,
        m_TrainCurrentLane = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup,
        m_WatercraftCurrentLane = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup,
        m_AircraftCurrentLane = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup,
        m_LaneObjectType = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferTypeHandle,
        m_LaneObjectQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_BlockedLane_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_HangaroundLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OutOfControl_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalNavigation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanNavigation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new FixLaneObjectsSystem.FixLaneObjectsJob()
      {
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_CarNavigationData = this.__TypeHandle.__Game_Vehicles_CarNavigation_RO_ComponentLookup,
        m_HumanNavigationData = this.__TypeHandle.__Game_Creatures_HumanNavigation_RO_ComponentLookup,
        m_AnimalNavigationData = this.__TypeHandle.__Game_Creatures_AnimalNavigation_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_WatercraftNavigationData = this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RO_ComponentLookup,
        m_AircraftNavigationData = this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RO_ComponentLookup,
        m_OutOfControlData = this.__TypeHandle.__Game_Vehicles_OutOfControl_RO_ComponentLookup,
        m_HelicopterData = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_HangaroundLocationData = this.__TypeHandle.__Game_Areas_HangaroundLocation_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_CarCurrentLaneData = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentLookup,
        m_CarTrailerLaneData = this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup,
        m_HumanCurrentLane = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentLookup,
        m_AnimalCurrentLane = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup,
        m_TrainCurrentLane = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup,
        m_WatercraftCurrentLane = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentLookup,
        m_AircraftCurrentLane = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentLookup,
        m_BlockedLanes = this.__TypeHandle.__Game_Objects_BlockedLane_RW_BufferLookup,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies2),
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies3),
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies4),
        m_LaneObjectQueue = nativeQueue,
        m_LaneObjectBuffer = this.m_LaneObjectUpdater.Begin(Allocator.TempJob)
      }.Schedule<FixLaneObjectsSystem.FixLaneObjectsJob>(JobHandle.CombineDependencies(jobData.ScheduleParallel<FixLaneObjectsSystem.CollectLaneObjectsJob>(this.m_LaneQuery, this.Dependency), dependencies2, JobHandle.CombineDependencies(dependencies1, dependencies3, dependencies4)));
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_LaneObjectUpdater.Apply((SystemBase) this, jobHandle);
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
    public FixLaneObjectsSystem()
    {
    }

    [BurstCompile]
    private struct CollectLaneObjectsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> m_CarCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<CarTrailerLane> m_CarTrailerLaneData;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLane;
      [ReadOnly]
      public ComponentLookup<AnimalCurrentLane> m_AnimalCurrentLane;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> m_TrainCurrentLane;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> m_WatercraftCurrentLane;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> m_AircraftCurrentLane;
      public BufferTypeHandle<LaneObject> m_LaneObjectType;
      public NativeQueue<Entity>.ParallelWriter m_LaneObjectQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LaneObject> bufferAccessor = chunk.GetBufferAccessor<LaneObject>(ref this.m_LaneObjectType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<LaneObject> dynamicBuffer = bufferAccessor[index1];
          int index2 = 0;
          for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
          {
            LaneObject laneObject = dynamicBuffer[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ParkedCarData.HasComponent(laneObject.m_LaneObject) || this.m_ParkedTrainData.HasComponent(laneObject.m_LaneObject))
            {
              dynamicBuffer[index2++] = laneObject;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarCurrentLaneData.HasComponent(laneObject.m_LaneObject) || this.m_CarTrailerLaneData.HasComponent(laneObject.m_LaneObject) || this.m_HumanCurrentLane.HasComponent(laneObject.m_LaneObject) || this.m_AnimalCurrentLane.HasComponent(laneObject.m_LaneObject) || this.m_TrainCurrentLane.HasComponent(laneObject.m_LaneObject) || this.m_WatercraftCurrentLane.HasComponent(laneObject.m_LaneObject) || this.m_AircraftCurrentLane.HasComponent(laneObject.m_LaneObject))
              {
                dynamicBuffer[index2++] = laneObject;
                // ISSUE: reference to a compiler-generated field
                this.m_LaneObjectQueue.Enqueue(laneObject.m_LaneObject);
              }
            }
          }
          if (index2 != 0)
          {
            if (index2 < dynamicBuffer.Length)
              dynamicBuffer.RemoveRange(index2, dynamicBuffer.Length - index2);
          }
          else
            dynamicBuffer.Clear();
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
    private struct FixLaneObjectsJob : IJob
    {
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<CarNavigation> m_CarNavigationData;
      [ReadOnly]
      public ComponentLookup<HumanNavigation> m_HumanNavigationData;
      [ReadOnly]
      public ComponentLookup<AnimalNavigation> m_AnimalNavigationData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<WatercraftNavigation> m_WatercraftNavigationData;
      [ReadOnly]
      public ComponentLookup<AircraftNavigation> m_AircraftNavigationData;
      [ReadOnly]
      public ComponentLookup<OutOfControl> m_OutOfControlData;
      [ReadOnly]
      public ComponentLookup<Helicopter> m_HelicopterData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<HangaroundLocation> m_HangaroundLocationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      public ComponentLookup<CarCurrentLane> m_CarCurrentLaneData;
      public ComponentLookup<CarTrailerLane> m_CarTrailerLaneData;
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLane;
      public ComponentLookup<AnimalCurrentLane> m_AnimalCurrentLane;
      public ComponentLookup<TrainCurrentLane> m_TrainCurrentLane;
      public ComponentLookup<WatercraftCurrentLane> m_WatercraftCurrentLane;
      public ComponentLookup<AircraftCurrentLane> m_AircraftCurrentLane;
      public BufferLookup<BlockedLane> m_BlockedLanes;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      public NativeQueue<Entity> m_LaneObjectQueue;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneObjectQueue.Count == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_LaneObjectQueue.Count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<BlockedLane> tempBlockedLanes = new NativeList<BlockedLane>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LaneObjectQueue.TryDequeue(out entity))
        {
          if (nativeParallelHashSet.Add(entity))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarCurrentLaneData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateCar(entity, tempBlockedLanes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarTrailerLaneData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.UpdateCarTrailer(entity, tempBlockedLanes);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_HumanCurrentLane.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateHuman(entity);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_AnimalCurrentLane.HasComponent(entity))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateAnimal(entity);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TrainCurrentLane.HasComponent(entity))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.UpdateTrain(entity);
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_WatercraftCurrentLane.HasComponent(entity))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.UpdateWatercraft(entity);
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_AircraftCurrentLane.HasComponent(entity))
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.UpdateAircraft(entity);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        tempBlockedLanes.Dispose();
        nativeParallelHashSet.Dispose();
      }

      private void UpdateAircraft(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        AircraftNavigation navigation = this.m_AircraftNavigationData[entity];
        // ISSUE: reference to a compiler-generated field
        AircraftCurrentLane currentLane = this.m_AircraftCurrentLane[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[entity].m_Prefab];
        Moving moving = new Moving();
        // ISSUE: reference to a compiler-generated field
        if (this.m_MovingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          moving = this.m_MovingData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AircraftNavigationHelpers.CurrentLaneCache currentLaneCache = new AircraftNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
        float num1 = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_HelicopterData.HasComponent(entity);
        AircraftCurrentLane aircraftCurrentLane = currentLane;
        currentLane.m_Lane = Entity.Null;
        float3 float3 = transform.m_Position + moving.m_Velocity * (num1 * 2f);
        float num2 = 100f;
        Bounds3 bounds3 = new Bounds3(float3 - num2, float3 + num2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AircraftNavigationHelpers.FindLaneIterator iterator = new AircraftNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = float3,
          m_MinDistance = num2,
          m_Result = currentLane,
          m_CarType = flag ? RoadTypes.Helicopter : RoadTypes.Airplane,
          m_SubLanes = this.m_SubLanes,
          m_CarLaneData = this.m_CarLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabCarLaneData = this.m_PrefabCarLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<AircraftNavigationHelpers.FindLaneIterator>(ref iterator);
        AircraftCurrentLane result = iterator.m_Result;
        if (aircraftCurrentLane.m_Lane == result.m_Lane)
        {
          result.m_CurvePosition.yz = aircraftCurrentLane.m_CurvePosition.yz;
          result.m_LaneFlags = aircraftCurrentLane.m_LaneFlags;
        }
        else
          result.m_LaneFlags |= AircraftLaneFlags.Obsolete;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        currentLaneCache.CheckChanges(entity, ref result, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
        // ISSUE: reference to a compiler-generated field
        this.m_AircraftCurrentLane[entity] = result;
      }

      private void UpdateWatercraft(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        WatercraftNavigation navigation = this.m_WatercraftNavigationData[entity];
        // ISSUE: reference to a compiler-generated field
        WatercraftCurrentLane result = this.m_WatercraftCurrentLane[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[entity].m_Prefab];
        Moving moving = new Moving();
        // ISSUE: reference to a compiler-generated field
        if (this.m_MovingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          moving = this.m_MovingData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WatercraftNavigationHelpers.CurrentLaneCache currentLaneCache = new WatercraftNavigationHelpers.CurrentLaneCache(ref result, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
        float num1 = 0.266666681f;
        WatercraftCurrentLane watercraftCurrentLane = result;
        result.m_Lane = Entity.Null;
        result.m_ChangeLane = Entity.Null;
        float3 float3 = transform.m_Position + moving.m_Velocity * (num1 * 2f);
        float num2 = 100f;
        Bounds3 bounds3 = new Bounds3(float3 - num2, float3 + num2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WatercraftNavigationHelpers.FindLaneIterator iterator = new WatercraftNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = float3,
          m_MinDistance = num2,
          m_Result = result,
          m_CarType = RoadTypes.Watercraft,
          m_SubLanes = this.m_SubLanes,
          m_CarLaneData = this.m_CarLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_MasterLaneData = this.m_MasterLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabCarLaneData = this.m_PrefabCarLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<WatercraftNavigationHelpers.FindLaneIterator>(ref iterator);
        result = iterator.m_Result;
        if (watercraftCurrentLane.m_Lane == result.m_Lane)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(watercraftCurrentLane.m_ChangeLane) && !this.m_DeletedData.HasComponent(watercraftCurrentLane.m_ChangeLane))
            result.m_ChangeLane = watercraftCurrentLane.m_ChangeLane;
          result.m_CurvePosition.yz = watercraftCurrentLane.m_CurvePosition.yz;
          result.m_LaneFlags = watercraftCurrentLane.m_LaneFlags;
        }
        else
          result.m_LaneFlags |= WatercraftLaneFlags.Obsolete;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        currentLaneCache.CheckChanges(entity, ref result, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
        // ISSUE: reference to a compiler-generated field
        this.m_WatercraftCurrentLane[entity] = result;
      }

      private void UpdateTrain(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        Train train = this.m_TrainData[entity];
        // ISSUE: reference to a compiler-generated field
        TrainCurrentLane currentLane = this.m_TrainCurrentLane[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TrainData prefabTrainData = this.m_PrefabTrainData[this.m_PrefabRefData[entity].m_Prefab];
        Moving moving = new Moving();
        // ISSUE: reference to a compiler-generated field
        if (this.m_MovingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          moving = this.m_MovingData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        TrainNavigationHelpers.CurrentLaneCache currentLaneCache = new TrainNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_LaneData);
        float num = 0.266666681f;
        transform.m_Position += moving.m_Velocity * (num * 2f);
        TrainCurrentLane trainCurrentLane = currentLane;
        currentLane.m_Front.m_Lane = Entity.Null;
        currentLane.m_Rear.m_Lane = Entity.Null;
        currentLane.m_FrontCache.m_Lane = Entity.Null;
        currentLane.m_RearCache.m_Lane = Entity.Null;
        float3 pivot1;
        float3 pivot2;
        VehicleUtils.CalculateTrainNavigationPivots(transform, prefabTrainData, out pivot1, out pivot2);
        if ((train.m_Flags & Game.Vehicles.TrainFlags.Reversed) != (Game.Vehicles.TrainFlags) 0)
          CommonUtils.Swap<float3>(ref pivot1, ref pivot2);
        float range = 100f;
        Bounds3 bounds3 = MathUtils.Expand(MathUtils.Bounds(pivot1, pivot2), (float3) range);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TrainNavigationHelpers.FindLaneIterator iterator = new TrainNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_FrontPivot = pivot1,
          m_RearPivot = pivot2,
          m_MinDistance = (float2) range,
          m_Result = currentLane,
          m_TrackType = prefabTrainData.m_TrackType,
          m_SubLanes = this.m_SubLanes,
          m_TrackLaneData = this.m_TrackLaneData,
          m_CurveData = this.m_CurveData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabTrackLaneData = this.m_PrefabTrackLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<TrainNavigationHelpers.FindLaneIterator>(ref iterator);
        TrainCurrentLane result = iterator.m_Result;
        if (trainCurrentLane.m_Front.m_Lane == result.m_Front.m_Lane)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(trainCurrentLane.m_FrontCache.m_Lane) && !this.m_DeletedData.HasComponent(trainCurrentLane.m_FrontCache.m_Lane))
          {
            result.m_FrontCache.m_Lane = trainCurrentLane.m_FrontCache.m_Lane;
            result.m_FrontCache.m_CurvePosition = trainCurrentLane.m_FrontCache.m_CurvePosition;
            result.m_FrontCache.m_LaneFlags = trainCurrentLane.m_FrontCache.m_LaneFlags;
          }
          result.m_Front.m_CurvePosition.xzw = trainCurrentLane.m_Front.m_CurvePosition.xzw;
          result.m_Front.m_LaneFlags = trainCurrentLane.m_Front.m_LaneFlags;
        }
        else
          result.m_Front.m_LaneFlags |= TrainLaneFlags.Obsolete;
        if (trainCurrentLane.m_Rear.m_Lane == result.m_Rear.m_Lane)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(trainCurrentLane.m_RearCache.m_Lane) && !this.m_DeletedData.HasComponent(trainCurrentLane.m_RearCache.m_Lane))
          {
            result.m_RearCache.m_Lane = trainCurrentLane.m_RearCache.m_Lane;
            result.m_RearCache.m_CurvePosition = trainCurrentLane.m_RearCache.m_CurvePosition;
            result.m_RearCache.m_LaneFlags = trainCurrentLane.m_RearCache.m_LaneFlags;
          }
          result.m_Rear.m_CurvePosition.xzw = trainCurrentLane.m_Rear.m_CurvePosition.xzw;
          result.m_Rear.m_LaneFlags = trainCurrentLane.m_Rear.m_LaneFlags;
        }
        else
          result.m_Rear.m_LaneFlags |= TrainLaneFlags.Obsolete;
        // ISSUE: reference to a compiler-generated field
        currentLaneCache.CheckChanges(entity, result, this.m_LaneObjectBuffer);
        // ISSUE: reference to a compiler-generated field
        this.m_TrainCurrentLane[entity] = result;
      }

      private void UpdateHuman(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        HumanNavigation navigation = this.m_HumanNavigationData[entity];
        // ISSUE: reference to a compiler-generated field
        HumanCurrentLane currentLane = this.m_HumanCurrentLane[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[entity].m_Prefab];
        Moving moving = new Moving();
        // ISSUE: reference to a compiler-generated field
        if (this.m_MovingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          moving = this.m_MovingData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        HumanNavigationHelpers.CurrentLaneCache currentLaneCache = new HumanNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_EntityLookup, this.m_MovingObjectSearchTree);
        HumanCurrentLane humanCurrentLane = currentLane;
        float3 position = transform.m_Position;
        Bounds3 bounds3 = new Bounds3(position - 100f, position + 100f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        HumanNavigationHelpers.FindLaneIterator iterator = new HumanNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = position,
          m_MinDistance = 1000f,
          m_Result = currentLane,
          m_SubLanes = this.m_SubLanes,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles,
          m_PedestrianLaneData = this.m_PedestrianLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_CurveData = this.m_CurveData,
          m_HangaroundLocationData = this.m_HangaroundLocationData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<HumanNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<HumanNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<HumanNavigationHelpers.FindLaneIterator>(ref iterator);
        HumanCurrentLane result = iterator.m_Result;
        if (humanCurrentLane.m_Lane == result.m_Lane)
        {
          result.m_CurvePosition.y = humanCurrentLane.m_CurvePosition.y;
          result.m_Flags = humanCurrentLane.m_Flags;
        }
        else
          result.m_Flags |= CreatureLaneFlags.Obsolete;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        currentLaneCache.CheckChanges(entity, ref result, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
        // ISSUE: reference to a compiler-generated field
        this.m_HumanCurrentLane[entity] = result;
      }

      private void UpdateAnimal(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        AnimalNavigation navigation = this.m_AnimalNavigationData[entity];
        // ISSUE: reference to a compiler-generated field
        AnimalCurrentLane currentLane = this.m_AnimalCurrentLane[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[entity].m_Prefab];
        Moving moving = new Moving();
        // ISSUE: reference to a compiler-generated field
        if (this.m_MovingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          moving = this.m_MovingData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AnimalNavigationHelpers.CurrentLaneCache currentLaneCache = new AnimalNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
        AnimalCurrentLane animalCurrentLane = currentLane;
        float3 position = transform.m_Position;
        Bounds3 bounds3 = new Bounds3(position - 100f, position + 100f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AnimalNavigationHelpers.FindLaneIterator iterator = new AnimalNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = position,
          m_MinDistance = 1000f,
          m_Result = currentLane,
          m_SubLanes = this.m_SubLanes,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles,
          m_PedestrianLaneData = this.m_PedestrianLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_CurveData = this.m_CurveData,
          m_HangaroundLocationData = this.m_HangaroundLocationData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<AnimalNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<AnimalNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<AnimalNavigationHelpers.FindLaneIterator>(ref iterator);
        AnimalCurrentLane result = iterator.m_Result;
        if (animalCurrentLane.m_Lane == result.m_Lane)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(animalCurrentLane.m_NextLane) && !this.m_DeletedData.HasComponent(animalCurrentLane.m_NextLane))
          {
            result.m_NextLane = animalCurrentLane.m_NextLane;
            result.m_NextPosition = animalCurrentLane.m_NextPosition;
            result.m_NextFlags = animalCurrentLane.m_NextFlags;
          }
          result.m_CurvePosition.y = animalCurrentLane.m_CurvePosition.y;
          result.m_Flags = animalCurrentLane.m_Flags;
        }
        else
          result.m_Flags |= CreatureLaneFlags.Obsolete;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        currentLaneCache.CheckChanges(entity, ref result, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
        // ISSUE: reference to a compiler-generated field
        this.m_AnimalCurrentLane[entity] = result;
      }

      private void UpdateCar(Entity entity, NativeList<BlockedLane> tempBlockedLanes)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        CarNavigation navigation = this.m_CarNavigationData[entity];
        // ISSUE: reference to a compiler-generated field
        CarCurrentLane result = this.m_CarCurrentLaneData[entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[entity];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
        Moving moving = new Moving();
        // ISSUE: reference to a compiler-generated field
        if (this.m_MovingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          moving = this.m_MovingData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CarNavigationHelpers.CurrentLaneCache currentLaneCache = new CarNavigationHelpers.CurrentLaneCache(ref result, blockedLane, this.m_EntityLookup, this.m_MovingObjectSearchTree);
        // ISSUE: reference to a compiler-generated field
        if (this.m_OutOfControlData.HasComponent(entity))
        {
          float3 position = transform.m_Position;
          float3 float3 = math.forward(transform.m_Rotation);
          Line3.Segment line = new Line3.Segment(position - float3 * math.max(0.1f, (float) (-(double) objectGeometryData.m_Bounds.min.z - (double) objectGeometryData.m_Size.x * 0.5)), position + float3 * math.max(0.1f, objectGeometryData.m_Bounds.max.z - objectGeometryData.m_Size.x * 0.5f));
          float range = objectGeometryData.m_Size.x * 0.5f;
          Bounds3 bounds3 = MathUtils.Expand(MathUtils.Bounds(line), (float3) range);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarNavigationHelpers.FindBlockedLanesIterator iterator = new CarNavigationHelpers.FindBlockedLanesIterator()
          {
            m_Bounds = bounds3,
            m_Line = line,
            m_Radius = range,
            m_BlockedLanes = tempBlockedLanes,
            m_SubLanes = this.m_SubLanes,
            m_MasterLaneData = this.m_MasterLaneData,
            m_CurveData = this.m_CurveData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabLaneData = this.m_PrefabLaneData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindBlockedLanesIterator>(ref iterator);
        }
        else
        {
          float num1 = 0.266666681f;
          CarCurrentLane carCurrentLane = result;
          result.m_Lane = Entity.Null;
          result.m_ChangeLane = Entity.Null;
          float3 float3 = transform.m_Position + moving.m_Velocity * (num1 * 2f);
          float num2 = 100f;
          Bounds3 bounds3 = new Bounds3(float3 - num2, float3 + num2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarNavigationHelpers.FindLaneIterator iterator = new CarNavigationHelpers.FindLaneIterator()
          {
            m_Bounds = bounds3,
            m_Position = float3,
            m_MinDistance = num2,
            m_Result = result,
            m_CarType = RoadTypes.Car,
            m_SubLanes = this.m_SubLanes,
            m_AreaNodes = this.m_AreaNodes,
            m_AreaTriangles = this.m_AreaTriangles,
            m_CarLaneData = this.m_CarLaneData,
            m_MasterLaneData = this.m_MasterLaneData,
            m_ConnectionLaneData = this.m_ConnectionLaneData,
            m_CurveData = this.m_CurveData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabCarLaneData = this.m_PrefabCarLaneData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          this.m_StaticObjectSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          this.m_AreaSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
          result = iterator.m_Result;
          if (carCurrentLane.m_Lane == result.m_Lane)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(carCurrentLane.m_ChangeLane) && !this.m_DeletedData.HasComponent(carCurrentLane.m_ChangeLane))
              result.m_ChangeLane = carCurrentLane.m_ChangeLane;
            result.m_CurvePosition.yz = carCurrentLane.m_CurvePosition.yz;
            result.m_LaneFlags = carCurrentLane.m_LaneFlags;
          }
          else
            result.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Obsolete;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        currentLaneCache.CheckChanges(entity, ref result, tempBlockedLanes, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
        // ISSUE: reference to a compiler-generated field
        this.m_CarCurrentLaneData[entity] = result;
        tempBlockedLanes.Clear();
      }

      private void UpdateCarTrailer(Entity entity, NativeList<BlockedLane> tempBlockedLanes)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        Controller controller = this.m_ControllerData[entity];
        // ISSUE: reference to a compiler-generated field
        CarTrailerLane trailerLane = this.m_CarTrailerLaneData[entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[entity];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
        CarNavigation tractorNavigation = new CarNavigation();
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarNavigationData.HasComponent(controller.m_Controller))
        {
          // ISSUE: reference to a compiler-generated field
          tractorNavigation = this.m_CarNavigationData[controller.m_Controller];
        }
        Moving moving = new Moving();
        // ISSUE: reference to a compiler-generated field
        if (this.m_MovingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          moving = this.m_MovingData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CarNavigationHelpers.TrailerLaneCache trailerLaneCache = new CarNavigationHelpers.TrailerLaneCache(ref trailerLane, blockedLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
        // ISSUE: reference to a compiler-generated field
        if (this.m_OutOfControlData.HasComponent(entity))
        {
          float3 position = transform.m_Position;
          float3 float3 = math.forward(transform.m_Rotation);
          Line3.Segment line = new Line3.Segment(position - float3 * math.max(0.1f, (float) (-(double) objectGeometryData.m_Bounds.min.z - (double) objectGeometryData.m_Size.x * 0.5)), position + float3 * math.max(0.1f, objectGeometryData.m_Bounds.max.z - objectGeometryData.m_Size.x * 0.5f));
          float range = objectGeometryData.m_Size.x * 0.5f;
          Bounds3 bounds3 = MathUtils.Expand(MathUtils.Bounds(line), (float3) range);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarNavigationHelpers.FindBlockedLanesIterator iterator = new CarNavigationHelpers.FindBlockedLanesIterator()
          {
            m_Bounds = bounds3,
            m_Line = line,
            m_Radius = range,
            m_BlockedLanes = tempBlockedLanes,
            m_SubLanes = this.m_SubLanes,
            m_MasterLaneData = this.m_MasterLaneData,
            m_CurveData = this.m_CurveData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabLaneData = this.m_PrefabLaneData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindBlockedLanesIterator>(ref iterator);
        }
        else
        {
          float num1 = 0.266666681f;
          float3 float3 = transform.m_Position + moving.m_Velocity * (num1 * 2f);
          float num2 = 100f;
          Bounds3 bounds3 = new Bounds3(float3 - num2, float3 + num2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarNavigationHelpers.FindLaneIterator iterator = new CarNavigationHelpers.FindLaneIterator()
          {
            m_Bounds = bounds3,
            m_Position = float3,
            m_MinDistance = num2,
            m_CarType = RoadTypes.Car,
            m_SubLanes = this.m_SubLanes,
            m_AreaNodes = this.m_AreaNodes,
            m_AreaTriangles = this.m_AreaTriangles,
            m_CarLaneData = this.m_CarLaneData,
            m_MasterLaneData = this.m_MasterLaneData,
            m_ConnectionLaneData = this.m_ConnectionLaneData,
            m_CurveData = this.m_CurveData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabCarLaneData = this.m_PrefabCarLaneData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          this.m_StaticObjectSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          this.m_AreaSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
          if (iterator.m_Result.m_Lane != trailerLane.m_Lane)
          {
            trailerLane.m_Lane = iterator.m_Result.m_Lane;
            trailerLane.m_CurvePosition = iterator.m_Result.m_CurvePosition.xy;
            trailerLane.m_NextLane = Entity.Null;
            trailerLane.m_NextPosition = new float2();
          }
          else
          {
            trailerLane.m_CurvePosition.x = iterator.m_Result.m_CurvePosition.x;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PrefabRefData.HasComponent(trailerLane.m_NextLane) || this.m_DeletedData.HasComponent(trailerLane.m_NextLane))
            {
              trailerLane.m_NextLane = Entity.Null;
              trailerLane.m_NextPosition = new float2();
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        trailerLaneCache.CheckChanges(entity, ref trailerLane, tempBlockedLanes, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, tractorNavigation, objectGeometryData);
        // ISSUE: reference to a compiler-generated field
        this.m_CarTrailerLaneData[entity] = trailerLane;
        tempBlockedLanes.Clear();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTrailerLane> __Game_Vehicles_CarTrailerLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup;
      public BufferTypeHandle<LaneObject> __Game_Net_LaneObject_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarNavigation> __Game_Vehicles_CarNavigation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanNavigation> __Game_Creatures_HumanNavigation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalNavigation> __Game_Creatures_AnimalNavigation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftNavigation> __Game_Vehicles_WatercraftNavigation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AircraftNavigation> __Game_Vehicles_AircraftNavigation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutOfControl> __Game_Vehicles_OutOfControl_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HangaroundLocation> __Game_Areas_HangaroundLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentLookup;
      public ComponentLookup<CarTrailerLane> __Game_Vehicles_CarTrailerLane_RW_ComponentLookup;
      public ComponentLookup<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RW_ComponentLookup;
      public ComponentLookup<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentLookup;
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RW_ComponentLookup;
      public ComponentLookup<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RW_ComponentLookup;
      public ComponentLookup<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RW_ComponentLookup;
      public BufferLookup<BlockedLane> __Game_Objects_BlockedLane_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup = state.GetComponentLookup<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailerLane_RO_ComponentLookup = state.GetComponentLookup<CarTrailerLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup = state.GetComponentLookup<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup = state.GetComponentLookup<AnimalCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<WatercraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<AircraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<LaneObject>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigation_RO_ComponentLookup = state.GetComponentLookup<CarNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanNavigation_RO_ComponentLookup = state.GetComponentLookup<HumanNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalNavigation_RO_ComponentLookup = state.GetComponentLookup<AnimalNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigation_RO_ComponentLookup = state.GetComponentLookup<WatercraftNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigation_RO_ComponentLookup = state.GetComponentLookup<AircraftNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OutOfControl_RO_ComponentLookup = state.GetComponentLookup<OutOfControl>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentLookup = state.GetComponentLookup<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_HangaroundLocation_RO_ComponentLookup = state.GetComponentLookup<HangaroundLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentLookup = state.GetComponentLookup<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup = state.GetComponentLookup<CarTrailerLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RW_ComponentLookup = state.GetComponentLookup<HumanCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup = state.GetComponentLookup<AnimalCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentLookup = state.GetComponentLookup<WatercraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RW_ComponentLookup = state.GetComponentLookup<AircraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_BlockedLane_RW_BufferLookup = state.GetBufferLookup<BlockedLane>();
      }
    }
  }
}
