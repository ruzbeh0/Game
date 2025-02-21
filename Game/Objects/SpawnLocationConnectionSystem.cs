// Decompiled with JetBrains decompiler
// Type: Game.Objects.SpawnLocationConnectionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class SpawnLocationConnectionSystem : GameSystemBase
  {
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Net.UpdateCollectSystem m_NetUpdateCollectSystem;
    private AirwaySystem m_AirwaySystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Areas.UpdateCollectSystem m_AreaUpdateCollectSystem;
    private SearchSystem m_ObjectSearchSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_UpdatedQuery;
    private SpawnLocationConnectionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Net.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Areas.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<SpawnLocation>(),
          ComponentType.ReadOnly<Updated>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<RoadConnectionUpdated>(),
          ComponentType.ReadOnly<Game.Common.Event>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      bool flag = !this.m_UpdatedQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!flag && !this.m_NetUpdateCollectSystem.netsUpdated && !this.m_AreaUpdateCollectSystem.lotsUpdated && !this.m_AreaUpdateCollectSystem.spacesUpdated)
        return;
      NativeQueue<Entity> nativeQueue1 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue2 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue3 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue4 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: variable of a compiler-generated type
      SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob jobData1;
      // ISSUE: reference to a compiler-generated field
      if (this.m_NetUpdateCollectSystem.netsUpdated)
      {
        JobHandle dependencies1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedNetBounds = this.m_NetUpdateCollectSystem.GetUpdatedNetBounds(out dependencies1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        jobData1 = new SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob();
        // ISSUE: reference to a compiler-generated field
        jobData1.m_Bounds = updatedNetBounds.AsDeferredJobArray();
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        jobData1.m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData1.m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        jobData1.m_ResultQueue = nativeQueue1.AsParallelWriter();
        JobHandle jobHandle2 = jobData1.Schedule<SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob, Bounds2>(updatedNetBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetUpdateCollectSystem.AddNetBoundsReader(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle2);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.lotsUpdated)
      {
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedLotBounds = this.m_AreaUpdateCollectSystem.GetUpdatedLotBounds(out dependencies3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        jobData1 = new SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob();
        // ISSUE: reference to a compiler-generated field
        jobData1.m_Bounds = updatedLotBounds.AsDeferredJobArray();
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        jobData1.m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData1.m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        jobData1.m_ResultQueue = nativeQueue2.AsParallelWriter();
        JobHandle jobHandle3 = jobData1.Schedule<SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob, Bounds2>(updatedLotBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies3, dependencies4));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddLotBoundsReader(jobHandle3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle3);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle3);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.spacesUpdated)
      {
        JobHandle dependencies5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedSpaceBounds = this.m_AreaUpdateCollectSystem.GetUpdatedSpaceBounds(out dependencies5);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        jobData1 = new SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob();
        // ISSUE: reference to a compiler-generated field
        jobData1.m_Bounds = updatedSpaceBounds.AsDeferredJobArray();
        JobHandle dependencies6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        jobData1.m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies6);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData1.m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        jobData1.m_ResultQueue = nativeQueue3.AsParallelWriter();
        JobHandle jobHandle4 = jobData1.Schedule<SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob, Bounds2>(updatedSpaceBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies5, dependencies6));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddSpaceBoundsReader(jobHandle4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle4);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle4);
      }
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        JobHandle job1 = new SpawnLocationConnectionSystem.CheckUpdatedSpawnLocationsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_RoadConnectionUpdatedType = this.__TypeHandle.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle,
          m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
          m_ResultQueue = nativeQueue4.AsParallelWriter()
        }.ScheduleParallel<SpawnLocationConnectionSystem.CheckUpdatedSpawnLocationsJob>(this.m_UpdatedQuery, this.Dependency);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, job1);
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SpawnLocationConnectionSystem.ListUpdatedSpawnLocationsJob jobData2 = new SpawnLocationConnectionSystem.ListUpdatedSpawnLocationsJob()
      {
        m_UpdatedQueue1 = nativeQueue1,
        m_UpdatedQueue2 = nativeQueue2,
        m_UpdatedQueue3 = nativeQueue3,
        m_UpdatedQueue4 = nativeQueue4,
        m_UpdatedList = nativeList
      };
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
      this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_MovedLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies7;
      JobHandle dependencies8;
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SpawnLocationConnectionSystem.FindSpawnLocationConnectionJob jobData3 = new SpawnLocationConnectionSystem.FindSpawnLocationConnectionJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_MovedLocationData = this.__TypeHandle.__Game_Objects_MovedLocation_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_LotData = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RW_ComponentLookup,
        m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
        m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_Entities = nativeList.AsDeferredJobArray(),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies7),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies8),
        m_AirwayData = this.m_AirwaySystem.GetAirwayData(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      JobHandle jobHandle5 = jobData2.Schedule<SpawnLocationConnectionSystem.ListUpdatedSpawnLocationsJob>(jobHandle1);
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle5, dependencies7, dependencies8);
      JobHandle jobHandle6 = jobData3.Schedule<SpawnLocationConnectionSystem.FindSpawnLocationConnectionJob, Entity>(list, 1, dependsOn);
      nativeQueue1.Dispose(jobHandle5);
      nativeQueue2.Dispose(jobHandle5);
      nativeQueue3.Dispose(jobHandle5);
      nativeQueue4.Dispose(jobHandle5);
      nativeList.Dispose(jobHandle6);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle6);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle6);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle6);
      this.Dependency = jobHandle6;
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
    public SpawnLocationConnectionSystem()
    {
    }

    [BurstCompile]
    private struct FindUpdatedSpawnLocationsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public ComponentLookup<SpawnLocation> m_SpawnLocationData;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob.Iterator iterator = new SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob.Iterator()
        {
          m_Bounds = MathUtils.Expand(this.m_Bounds[index], (float2) 32f),
          m_SpawnLocationData = this.m_SpawnLocationData,
          m_ResultQueue = this.m_ResultQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<SpawnLocationConnectionSystem.FindUpdatedSpawnLocationsJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<SpawnLocation> m_SpawnLocationData;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_SpawnLocationData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultQueue.Enqueue(entity);
        }
      }
    }

    [BurstCompile]
    private struct CheckUpdatedSpawnLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<RoadConnectionUpdated> m_RoadConnectionUpdatedType;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<RoadConnectionUpdated> nativeArray1 = chunk.GetNativeArray<RoadConnectionUpdated>(ref this.m_RoadConnectionUpdatedType);
        if (nativeArray1.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SpawnLocationElement> spawnLocation = this.m_SpawnLocations[nativeArray1[index1].m_Building];
            for (int index2 = 0; index2 < spawnLocation.Length; ++index2)
            {
              if (spawnLocation[index2].m_Type == SpawnLocationType.SpawnLocation)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ResultQueue.Enqueue(spawnLocation[index2].m_SpawnLocation);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResultQueue.Enqueue(nativeArray2[index]);
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

    [BurstCompile]
    private struct ListUpdatedSpawnLocationsJob : IJob
    {
      public NativeQueue<Entity> m_UpdatedQueue1;
      public NativeQueue<Entity> m_UpdatedQueue2;
      public NativeQueue<Entity> m_UpdatedQueue3;
      public NativeQueue<Entity> m_UpdatedQueue4;
      public NativeList<Entity> m_UpdatedList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_UpdatedQueue1.Count;
        // ISSUE: reference to a compiler-generated field
        int num1 = count + this.m_UpdatedQueue2.Count;
        // ISSUE: reference to a compiler-generated field
        int num2 = num1 + this.m_UpdatedQueue3.Count;
        // ISSUE: reference to a compiler-generated field
        int length = num2 + this.m_UpdatedQueue4.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedList.ResizeUninitialized(length);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedList[index] = this.m_UpdatedQueue1.Dequeue();
        }
        for (int index = count; index < num1; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedList[index] = this.m_UpdatedQueue2.Dequeue();
        }
        for (int index = num1; index < num2; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedList[index] = this.m_UpdatedQueue3.Dequeue();
        }
        for (int index = num2; index < length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedList[index] = this.m_UpdatedQueue4.Dequeue();
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> updatedList = this.m_UpdatedList;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SpawnLocationConnectionSystem.ListUpdatedSpawnLocationsJob.EntityComparer entityComparer = new SpawnLocationConnectionSystem.ListUpdatedSpawnLocationsJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        SpawnLocationConnectionSystem.ListUpdatedSpawnLocationsJob.EntityComparer comp = entityComparer;
        updatedList.Sort<Entity, SpawnLocationConnectionSystem.ListUpdatedSpawnLocationsJob.EntityComparer>(comp);
        Entity entity = Entity.Null;
        int num3 = 0;
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num3 < this.m_UpdatedList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity updated = this.m_UpdatedList[num3++];
          if (updated != entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdatedList[index1++] = updated;
            entity = updated;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index1 >= this.m_UpdatedList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedList.RemoveRangeSwapBack(index1, this.m_UpdatedList.Length - index1);
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    private struct FindSpawnLocationConnectionJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<MovedLocation> m_MovedLocationData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_LotData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_Lanes;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public AirwayHelpers.AirwayData m_AirwayData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SpawnLocationData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdatedData.HasComponent(entity) && this.m_MovedLocationData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<MovedLocation>(index, entity);
        }
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        Game.Prefabs.SpawnLocationData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSpawnLocationData.TryGetComponent(prefabRef.m_Prefab, out componentData))
        {
          if (componentData.m_ConnectionType == RouteConnectionType.Air)
          {
            SpawnLocation spawnLocation = new SpawnLocation();
            float maxValue = float.MaxValue;
            AirwayHelpers.AirwayMap airwayMap;
            if ((componentData.m_RoadTypes & RoadTypes.Helicopter) != RoadTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              airwayMap = this.m_AirwayData.helicopterMap;
              // ISSUE: reference to a compiler-generated field
              airwayMap.FindClosestLane(transform.m_Position, this.m_CurveData, ref spawnLocation.m_ConnectedLane1, ref spawnLocation.m_CurvePosition1, ref maxValue);
            }
            if ((componentData.m_RoadTypes & RoadTypes.Airplane) != RoadTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              airwayMap = this.m_AirwayData.airplaneMap;
              // ISSUE: reference to a compiler-generated field
              airwayMap.FindClosestLane(transform.m_Position, this.m_CurveData, ref spawnLocation.m_ConnectedLane1, ref spawnLocation.m_CurvePosition1, ref maxValue);
            }
            // ISSUE: reference to a compiler-generated method
            this.SetSpawnLocation(index, entity, spawnLocation);
            return;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRouteConnectionData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[prefabRef.m_Prefab];
            if (componentData.m_ConnectionType != RouteConnectionType.None && componentData.m_ConnectionType == routeConnectionData.m_AccessConnectionType && componentData.m_ActivityMask.m_Mask == 0U)
            {
              // ISSUE: reference to a compiler-generated method
              this.SetSpawnLocation(index, entity, new SpawnLocation());
              return;
            }
          }
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
          SpawnLocationConnectionSystem.FindSpawnLocationConnectionJob.Iterator iterator = new SpawnLocationConnectionSystem.FindSpawnLocationConnectionJob.Iterator()
          {
            m_Bounds = new Bounds3(transform.m_Position - 32f, transform.m_Position + 32f),
            m_Position = transform.m_Position,
            m_MaxDistance = 32f,
            m_SpawnLocationData = componentData,
            m_CurveData = this.m_CurveData,
            m_CarLaneData = this.m_CarLaneData,
            m_SlaveLaneData = this.m_SlaveLaneData,
            m_ConnectionLaneData = this.m_ConnectionLaneData,
            m_LotData = this.m_LotData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabNetLaneData = this.m_PrefabNetLaneData,
            m_PrefabCarLaneData = this.m_PrefabCarLaneData,
            m_PrefabTrackLaneData = this.m_PrefabTrackLaneData,
            m_Lanes = this.m_Lanes,
            m_AreaNodes = this.m_AreaNodes,
            m_AreaTriangles = this.m_AreaTriangles
          };
          float distance;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachedData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Attached attached = this.m_AttachedData[entity];
            SpawnLocation spawnLocation;
            // ISSUE: reference to a compiler-generated method
            if (iterator.TryFindLanes(attached.m_Parent, out distance, out spawnLocation))
            {
              // ISSUE: reference to a compiler-generated method
              this.SetSpawnLocation(index, entity, spawnLocation);
              return;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Owner owner = this.m_OwnerData[entity];
            SpawnLocation spawnLocation;
            // ISSUE: reference to a compiler-generated method
            if (iterator.TryFindLanes(owner.m_Owner, out distance, out spawnLocation))
            {
              // ISSUE: reference to a compiler-generated method
              this.SetSpawnLocation(index, entity, spawnLocation);
              return;
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<SpawnLocationConnectionSystem.FindSpawnLocationConnectionJob.Iterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          this.m_AreaSearchTree.Iterate<SpawnLocationConnectionSystem.FindSpawnLocationConnectionJob.Iterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          if (iterator.m_BestLocation.m_ConnectedLane1 != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.SetSpawnLocation(index, entity, iterator.m_BestLocation);
            return;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Owner owner = this.m_OwnerData[entity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_BuildingData.HasComponent(owner.m_Owner) && this.ShouldConnectToBuildingRoad(componentData, transform.m_Position, entity, owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              Building building = this.m_BuildingData[owner.m_Owner];
              SpawnLocation spawnLocation;
              // ISSUE: reference to a compiler-generated method
              if (iterator.TryFindLanes(building.m_RoadEdge, out distance, out spawnLocation))
              {
                // ISSUE: reference to a compiler-generated method
                this.SetSpawnLocation(index, entity, spawnLocation);
                return;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.SetSpawnLocation(index, entity, new SpawnLocation());
      }

      private bool ShouldConnectToBuildingRoad(
        Game.Prefabs.SpawnLocationData spawnLocationData,
        float3 position,
        Entity entity,
        Entity owner)
      {
        switch (spawnLocationData.m_ConnectionType)
        {
          case RouteConnectionType.None:
          case RouteConnectionType.Track:
          case RouteConnectionType.Air:
            return false;
          case RouteConnectionType.Road:
          case RouteConnectionType.Parking:
            if ((spawnLocationData.m_RoadTypes & RoadTypes.Car) == RoadTypes.None)
              return false;
            break;
        }
        if (spawnLocationData.m_ActivityMask.m_Mask != 0U)
          return false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnLocations.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SpawnLocationElement> spawnLocation1 = this.m_SpawnLocations[owner];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BuildingData buildingData = this.m_PrefabBuildingData[this.m_PrefabRefData[owner].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          float3 frontPosition = BuildingUtils.CalculateFrontPosition(this.m_TransformData[owner], buildingData.m_LotSize.y);
          float num = math.distance(position, frontPosition);
          for (int index = 0; index < spawnLocation1.Length; ++index)
          {
            if (spawnLocation1[index].m_Type == SpawnLocationType.SpawnLocation)
            {
              Entity spawnLocation2 = spawnLocation1[index].m_SpawnLocation;
              if (!(entity == spawnLocation2))
              {
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef = this.m_PrefabRefData[spawnLocation2];
                // ISSUE: reference to a compiler-generated field
                Game.Prefabs.SpawnLocationData spawnLocationData1 = this.m_PrefabSpawnLocationData[prefabRef.m_Prefab];
                if (spawnLocationData1.m_ConnectionType == spawnLocationData.m_ConnectionType && (spawnLocationData.m_ConnectionType != RouteConnectionType.Road && spawnLocationData.m_ConnectionType != RouteConnectionType.Parking || (spawnLocationData1.m_RoadTypes & spawnLocationData.m_RoadTypes) != RoadTypes.None) && spawnLocationData1.m_ActivityMask.m_Mask == 0U)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabRouteConnectionData.HasComponent(prefabRef.m_Prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[prefabRef.m_Prefab];
                    if (spawnLocationData.m_ConnectionType == routeConnectionData.m_AccessConnectionType)
                      continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransformData.HasComponent(spawnLocation2) && (double) math.distance(this.m_TransformData[spawnLocation2].m_Position, frontPosition) < (double) num)
                    return false;
                }
              }
            }
          }
        }
        return true;
      }

      private void SetSpawnLocation(int jobIndex, Entity entity, SpawnLocation spawnLocation)
      {
        // ISSUE: reference to a compiler-generated field
        SpawnLocation spawnLocation1 = this.m_SpawnLocationData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!(spawnLocation1.m_ConnectedLane1 != spawnLocation.m_ConnectedLane1) && !(spawnLocation1.m_ConnectedLane2 != spawnLocation.m_ConnectedLane2) && (double) spawnLocation1.m_CurvePosition1 == (double) spawnLocation.m_CurvePosition1 && (double) spawnLocation1.m_CurvePosition2 == (double) spawnLocation.m_CurvePosition2 && !this.m_UpdatedData.HasComponent(spawnLocation.m_ConnectedLane1) && !this.m_UpdatedData.HasComponent(spawnLocation.m_ConnectedLane2))
          return;
        spawnLocation.m_AccessRestriction = spawnLocation1.m_AccessRestriction;
        spawnLocation.m_GroupIndex = spawnLocation1.m_GroupIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_SpawnLocationData[entity] = spawnLocation;
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdatedData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, entity, new PathfindUpdated());
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float3 m_Position;
        public float m_MaxDistance;
        public Game.Prefabs.SpawnLocationData m_SpawnLocationData;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
        public ComponentLookup<SlaveLane> m_SlaveLaneData;
        public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
        public ComponentLookup<Game.Areas.Lot> m_LotData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
        public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
        public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
        public BufferLookup<Game.Net.SubLane> m_Lanes;
        public BufferLookup<Game.Areas.Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;
        public SpawnLocation m_BestLocation;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          float distance;
          SpawnLocation spawnLocation;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.TryFindLanes(entity, out distance, out spawnLocation) || (double) distance >= (double) this.m_MaxDistance)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Bounds = new Bounds3(this.m_Position - distance, this.m_Position + distance);
          // ISSUE: reference to a compiler-generated field
          this.m_MaxDistance = distance;
          // ISSUE: reference to a compiler-generated field
          this.m_BestLocation = spawnLocation;
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_Lanes.HasBuffer(item.m_Area))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[item.m_Area];
          // ISSUE: reference to a compiler-generated field
          Triangle triangle = this.m_AreaTriangles[item.m_Area][item.m_Triangle];
          Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, triangle);
          float2 t1;
          // ISSUE: reference to a compiler-generated field
          float num1 = MathUtils.Distance(triangle3, this.m_Position, out t1);
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 >= (double) this.m_MaxDistance)
            return;
          float3 position = MathUtils.Position(triangle3, t1);
          // ISSUE: reference to a compiler-generated field
          if (this.m_LotData.HasComponent(item.m_Area))
          {
            bool3 bool3 = AreaUtils.IsEdge(areaNode, triangle) & (math.cmin(triangle.m_Indices.xy) != 0 | math.cmax(triangle.m_Indices.xy) != 1) & (math.cmin(triangle.m_Indices.yz) != 0 | math.cmax(triangle.m_Indices.yz) != 1) & (math.cmin(triangle.m_Indices.zx) != 0 | math.cmax(triangle.m_Indices.zx) != 1);
            float t2;
            if (bool3.x && (double) MathUtils.Distance(triangle3.ab, position, out t2) < 0.10000000149011612 || bool3.y && (double) MathUtils.Distance(triangle3.bc, position, out t2) < 0.10000000149011612 || bool3.z && (double) MathUtils.Distance(triangle3.ca, position, out t2) < 0.10000000149011612)
              return;
          }
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[item.m_Area];
          SpawnLocation spawnLocation = new SpawnLocation();
          float num2 = float.MaxValue;
          for (int index = 0; index < lane.Length; ++index)
          {
            Entity subLane = lane[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_ConnectionLaneData.HasComponent(subLane) && this.CheckLaneType(this.m_ConnectionLaneData[subLane]))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane];
              float2 t3;
              bool2 x = new bool2(MathUtils.Intersect(triangle3.xz, curve.m_Bezier.a.xz, out t3), MathUtils.Intersect(triangle3.xz, curve.m_Bezier.d.xz, out t3));
              if (math.any(x))
              {
                float t4;
                float num3 = MathUtils.Distance(curve.m_Bezier, position, out t4);
                if ((double) num3 < (double) num2)
                {
                  float2 float2 = math.select(new float2(0.0f, 0.49f), math.select(new float2(0.51f, 1f), new float2(0.0f, 1f), x.x), x.y);
                  num2 = num3;
                  spawnLocation.m_ConnectedLane1 = subLane;
                  spawnLocation.m_CurvePosition1 = math.clamp(t4, float2.x, float2.y);
                }
              }
            }
          }
          if (!(spawnLocation.m_ConnectedLane1 != Entity.Null))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Bounds = new Bounds3(this.m_Position - num1, this.m_Position + num1);
          // ISSUE: reference to a compiler-generated field
          this.m_MaxDistance = num1;
          // ISSUE: reference to a compiler-generated field
          this.m_BestLocation = spawnLocation;
        }

        private bool CheckLaneType(Game.Net.ConnectionLane connectionLane)
        {
          // ISSUE: reference to a compiler-generated field
          switch (this.m_SpawnLocationData.m_ConnectionType)
          {
            case RouteConnectionType.Road:
            case RouteConnectionType.Cargo:
            case RouteConnectionType.Parking:
              // ISSUE: reference to a compiler-generated field
              return (connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (connectionLane.m_RoadTypes & this.m_SpawnLocationData.m_RoadTypes) != RoadTypes.None;
            case RouteConnectionType.Pedestrian:
              return (connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0;
            case RouteConnectionType.Track:
              // ISSUE: reference to a compiler-generated field
              return (connectionLane.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0 && (connectionLane.m_TrackTypes & this.m_SpawnLocationData.m_TrackTypes) != TrackTypes.None;
            default:
              return false;
          }
        }

        private bool CheckLaneType(Entity lane)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[lane];
          // ISSUE: reference to a compiler-generated field
          NetLaneData netLaneData = this.m_PrefabNetLaneData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          switch (this.m_SpawnLocationData.m_ConnectionType)
          {
            case RouteConnectionType.Road:
            case RouteConnectionType.Cargo:
            case RouteConnectionType.Parking:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              return (netLaneData.m_Flags & LaneFlags.Road) != (LaneFlags) 0 && (this.m_PrefabCarLaneData[prefabRef.m_Prefab].m_RoadTypes & this.m_SpawnLocationData.m_RoadTypes) != RoadTypes.None;
            case RouteConnectionType.Pedestrian:
              return (netLaneData.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0;
            case RouteConnectionType.Track:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              return (netLaneData.m_Flags & LaneFlags.Track) != (LaneFlags) 0 && (this.m_PrefabTrackLaneData[prefabRef.m_Prefab].m_TrackTypes & this.m_SpawnLocationData.m_TrackTypes) != TrackTypes.None;
            default:
              return false;
          }
        }

        public bool TryFindLanes(
          Entity entity,
          out float distance,
          out SpawnLocation spawnLocation)
        {
          distance = float.MaxValue;
          spawnLocation = new SpawnLocation();
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Lanes.HasBuffer(entity))
            return false;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[entity];
          for (int index = 0; index < lane.Length; ++index)
          {
            Entity subLane = lane[index].m_SubLane;
            // ISSUE: reference to a compiler-generated method
            if (this.CheckLaneType(subLane))
            {
              float t;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float num = MathUtils.Distance(this.m_CurveData[subLane].m_Bezier, this.m_Position, out t);
              if ((double) num < (double) distance)
              {
                distance = num;
                spawnLocation.m_ConnectedLane1 = subLane;
                spawnLocation.m_CurvePosition1 = t;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_SlaveLaneData.HasComponent(spawnLocation.m_ConnectedLane1))
          {
            // ISSUE: reference to a compiler-generated field
            SlaveLane slaveLane = this.m_SlaveLaneData[spawnLocation.m_ConnectedLane1];
            if ((int) slaveLane.m_MasterIndex < lane.Length)
              spawnLocation.m_ConnectedLane1 = lane[(int) slaveLane.m_MasterIndex].m_SubLane;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (spawnLocation.m_ConnectedLane1 != Entity.Null && (this.m_SpawnLocationData.m_ConnectionType == RouteConnectionType.Road || this.m_SpawnLocationData.m_ConnectionType == RouteConnectionType.Cargo || this.m_SpawnLocationData.m_ConnectionType == RouteConnectionType.Parking))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.CarLane carLane1 = this.m_CarLaneData[spawnLocation.m_ConnectedLane1];
            for (int index = 0; index < lane.Length; ++index)
            {
              Entity subLane = lane[index].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!(subLane == spawnLocation.m_ConnectedLane1) && this.m_CarLaneData.HasComponent(subLane) && !this.m_SlaveLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.CarLane carLane2 = this.m_CarLaneData[subLane];
                if ((int) carLane1.m_CarriagewayGroup == (int) carLane2.m_CarriagewayGroup)
                {
                  spawnLocation.m_ConnectedLane2 = subLane;
                  spawnLocation.m_CurvePosition2 = math.select(spawnLocation.m_CurvePosition1, 1f - spawnLocation.m_CurvePosition1, ((carLane1.m_Flags ^ carLane2.m_Flags) & CarLaneFlags.Invert) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter));
                  break;
                }
              }
            }
          }
          return spawnLocation.m_ConnectedLane1 != Entity.Null;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RoadConnectionUpdated> __Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovedLocation> __Game_Objects_MovedLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      public ComponentLookup<SpawnLocation> __Game_Objects_SpawnLocation_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RoadConnectionUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RO_BufferLookup = state.GetBufferLookup<SpawnLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_MovedLocation_RO_ComponentLookup = state.GetComponentLookup<MovedLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RW_ComponentLookup = state.GetComponentLookup<SpawnLocation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
      }
    }
  }
}
