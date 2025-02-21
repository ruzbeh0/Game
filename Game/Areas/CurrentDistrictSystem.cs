// Decompiled with JetBrains decompiler
// Type: Game.Areas.CurrentDistrictSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
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
namespace Game.Areas
{
  [CompilerGenerated]
  public class CurrentDistrictSystem : GameSystemBase
  {
    private UpdateCollectSystem m_UpdateCollectSystem;
    private SearchSystem m_AreaSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_CurrentDistrictQuery;
    private CurrentDistrictSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateCollectSystem = this.World.GetOrCreateSystemManaged<UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentDistrictQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<CurrentDistrict>(),
          ComponentType.ReadOnly<BorderDistrict>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentDistrictQuery.IsEmptyIgnoreFilter && !this.m_UpdateCollectSystem.districtsUpdated)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdateCollectSystem.districtsUpdated)
      {
        NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        JobHandle dependencies1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedDistrictBounds = this.m_UpdateCollectSystem.GetUpdatedDistrictBounds(out dependencies1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies2;
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.FindUpdatedDistrictItemsJob jobData1 = new CurrentDistrictSystem.FindUpdatedDistrictItemsJob()
        {
          m_Bounds = updatedDistrictBounds.AsDeferredJobArray(),
          m_ObjectTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2),
          m_NetTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies3),
          m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
          m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
          m_UpdateBuffer = nativeQueue.AsParallelWriter()
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.CollectUpdatedDistrictItemsJob jobData2 = new CurrentDistrictSystem.CollectUpdatedDistrictItemsJob()
        {
          m_UpdateBuffer = nativeQueue,
          m_UpdateList = nativeList
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_BorderDistrict_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_CurrentDistrict_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.FindDistrictParallelJob jobData3 = new CurrentDistrictSystem.FindDistrictParallelJob()
        {
          m_UpdateList = nativeList.AsDeferredJobArray(),
          m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies4),
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
          m_DistrictData = this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup,
          m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
          m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
          m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
          m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
          m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
          m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
          m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RW_ComponentLookup,
          m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RW_ComponentLookup
        };
        JobHandle job1 = JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3);
        JobHandle jobHandle1 = jobData1.Schedule<CurrentDistrictSystem.FindUpdatedDistrictItemsJob, Bounds2>(updatedDistrictBounds, 1, JobHandle.CombineDependencies(this.Dependency, job1));
        JobHandle jobHandle2 = jobData2.Schedule<CurrentDistrictSystem.CollectUpdatedDistrictItemsJob>(jobHandle1);
        NativeList<Entity> list = nativeList;
        JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle2, dependencies4);
        JobHandle jobHandle3 = jobData3.Schedule<CurrentDistrictSystem.FindDistrictParallelJob, Entity>(list, 1, dependsOn);
        nativeQueue.Dispose(jobHandle2);
        nativeList.Dispose(jobHandle3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_UpdateCollectSystem.AddDistrictBoundsReader(jobHandle1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle3);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle3);
        this.Dependency = jobHandle3;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentDistrictQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_BorderDistrict_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new CurrentDistrictSystem.FindDistrictChunkJob()
      {
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RW_ComponentTypeHandle,
        m_BorderDistrictType = this.__TypeHandle.__Game_Areas_BorderDistrict_RW_ComponentTypeHandle,
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
        m_DistrictData = this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup
      }.ScheduleParallel<CurrentDistrictSystem.FindDistrictChunkJob>(this.m_CurrentDistrictQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
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
    public CurrentDistrictSystem()
    {
    }

    [BurstCompile]
    private struct FindUpdatedDistrictItemsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetTree;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      public NativeQueue<Entity>.ParallelWriter m_UpdateBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.FindUpdatedDistrictItemsJob.ObjectIterator iterator1 = new CurrentDistrictSystem.FindUpdatedDistrictItemsJob.ObjectIterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_CurrentDistrictData = this.m_CurrentDistrictData,
          m_UpdateBuffer = this.m_UpdateBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectTree.Iterate<CurrentDistrictSystem.FindUpdatedDistrictItemsJob.ObjectIterator>(ref iterator1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.FindUpdatedDistrictItemsJob.NetIterator iterator2 = new CurrentDistrictSystem.FindUpdatedDistrictItemsJob.NetIterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_BorderDistrictData = this.m_BorderDistrictData,
          m_UpdateBuffer = this.m_UpdateBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetTree.Iterate<CurrentDistrictSystem.FindUpdatedDistrictItemsJob.NetIterator>(ref iterator2);
      }

      private struct ObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
        public NativeQueue<Entity>.ParallelWriter m_UpdateBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_CurrentDistrictData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateBuffer.Enqueue(entity);
        }
      }

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<BorderDistrict> m_BorderDistrictData;
        public NativeQueue<Entity>.ParallelWriter m_UpdateBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_BorderDistrictData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateBuffer.Enqueue(entity);
        }
      }
    }

    [BurstCompile]
    private struct CollectUpdatedDistrictItemsJob : IJob
    {
      public NativeQueue<Entity> m_UpdateBuffer;
      public NativeList<Entity> m_UpdateList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_UpdateBuffer.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateList.ResizeUninitialized(count);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateList[index] = this.m_UpdateBuffer.Dequeue();
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> updateList = this.m_UpdateList;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.CollectUpdatedDistrictItemsJob.EntityComparer entityComparer = new CurrentDistrictSystem.CollectUpdatedDistrictItemsJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.CollectUpdatedDistrictItemsJob.EntityComparer comp = entityComparer;
        updateList.Sort<Entity, CurrentDistrictSystem.CollectUpdatedDistrictItemsJob.EntityComparer>(comp);
        Entity entity = Entity.Null;
        int num = 0;
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_UpdateList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity update = this.m_UpdateList[num++];
          if (update != entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateList[index1++] = update;
            entity = update;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index1 >= this.m_UpdateList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateList.RemoveRange(index1, this.m_UpdateList.Length - index1);
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    private struct FindDistrictParallelJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Entity> m_UpdateList;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity update = this.m_UpdateList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.DistrictIterator iterator = new CurrentDistrictSystem.DistrictIterator()
        {
          m_DistrictData = this.m_DistrictData,
          m_Nodes = this.m_Nodes,
          m_Triangles = this.m_Triangles
        };
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentDistrictData.HasComponent(update))
        {
          // ISSUE: reference to a compiler-generated field
          CurrentDistrict currentDistrict = this.m_CurrentDistrictData[update];
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TransformData[update];
          // ISSUE: reference to a compiler-generated field
          iterator.m_Position = transform.m_Position.xz;
          // ISSUE: reference to a compiler-generated field
          iterator.m_Result = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          this.m_AreaTree.Iterate<CurrentDistrictSystem.DistrictIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          if (currentDistrict.m_District != iterator.m_Result)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentDistrictData[update] = new CurrentDistrict(iterator.m_Result);
            // ISSUE: reference to a compiler-generated method
            this.CheckChangedDistrict(index, update);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BorderDistrictData.HasComponent(update))
          return;
        // ISSUE: reference to a compiler-generated field
        BorderDistrict borderDistrict = this.m_BorderDistrictData[update];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[update];
        // ISSUE: reference to a compiler-generated field
        iterator.m_Position = edgeGeometry.m_Start.m_Left.d.xz;
        // ISSUE: reference to a compiler-generated field
        iterator.m_Result = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTree.Iterate<CurrentDistrictSystem.DistrictIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        Entity result1 = iterator.m_Result;
        // ISSUE: reference to a compiler-generated field
        iterator.m_Position = edgeGeometry.m_Start.m_Right.d.xz;
        // ISSUE: reference to a compiler-generated field
        iterator.m_Result = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTree.Iterate<CurrentDistrictSystem.DistrictIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        Entity result2 = iterator.m_Result;
        if (!(borderDistrict.m_Left != result1) && !(borderDistrict.m_Right != result2))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_BorderDistrictData[update] = new BorderDistrict(result1, result2);
        // ISSUE: reference to a compiler-generated method
        this.CheckChangedDistrict(index, update);
      }

      private void CheckChangedDistrict(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdatedData.HasComponent(entity) || !this.m_SubLanes.HasBuffer(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubLane> subLane1 = this.m_SubLanes[entity];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.HasComponent(subLane2) || this.m_PedestrianLaneData.HasComponent(subLane2) || this.m_ParkingLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, subLane2);
          }
        }
      }
    }

    [BurstCompile]
    private struct FindDistrictChunkJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      public ComponentTypeHandle<BorderDistrict> m_BorderDistrictType;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public BufferLookup<Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray1 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<BorderDistrict> nativeArray2 = chunk.GetNativeArray<BorderDistrict>(ref this.m_BorderDistrictType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CurrentDistrictSystem.DistrictIterator iterator = new CurrentDistrictSystem.DistrictIterator()
        {
          m_DistrictData = this.m_DistrictData,
          m_Nodes = this.m_Nodes,
          m_Triangles = this.m_Triangles
        };
        if (nativeArray1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          for (int index = 0; index < nativeArray3.Length; ++index)
          {
            Transform transform = nativeArray3[index];
            // ISSUE: reference to a compiler-generated field
            iterator.m_Position = transform.m_Position.xz;
            // ISSUE: reference to a compiler-generated field
            iterator.m_Result = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            this.m_AreaTree.Iterate<CurrentDistrictSystem.DistrictIterator>(ref iterator);
            // ISSUE: reference to a compiler-generated field
            nativeArray1[index] = new CurrentDistrict(iterator.m_Result);
          }
        }
        if (nativeArray2.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeGeometry> nativeArray4 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          EdgeGeometry edgeGeometry = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          iterator.m_Position = edgeGeometry.m_Start.m_Left.d.xz;
          // ISSUE: reference to a compiler-generated field
          iterator.m_Result = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          this.m_AreaTree.Iterate<CurrentDistrictSystem.DistrictIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          Entity result1 = iterator.m_Result;
          // ISSUE: reference to a compiler-generated field
          iterator.m_Position = edgeGeometry.m_Start.m_Right.d.xz;
          // ISSUE: reference to a compiler-generated field
          iterator.m_Result = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          this.m_AreaTree.Iterate<CurrentDistrictSystem.DistrictIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          Entity result2 = iterator.m_Result;
          nativeArray2[index] = new BorderDistrict(result1, result2);
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

    private struct DistrictIterator : 
      INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
    {
      public float2 m_Position;
      public ComponentLookup<District> m_DistrictData;
      public BufferLookup<Node> m_Nodes;
      public BufferLookup<Triangle> m_Triangles;
      public Entity m_Result;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        // ISSUE: reference to a compiler-generated field
        return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position) || !this.m_DistrictData.HasComponent(areaItem.m_Area))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Node> node = this.m_Nodes[areaItem.m_Area];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Triangle> triangle = this.m_Triangles[areaItem.m_Area];
        // ISSUE: reference to a compiler-generated field
        if (triangle.Length <= areaItem.m_Triangle || !MathUtils.Intersect(AreaUtils.GetTriangle2(node, triangle[areaItem.m_Triangle]), this.m_Position, out float2 _))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Result = areaItem.m_Area;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<District> __Game_Areas_District_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RW_ComponentLookup;
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RW_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RW_ComponentTypeHandle;
      public ComponentTypeHandle<BorderDistrict> __Game_Areas_BorderDistrict_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_District_RO_ComponentLookup = state.GetComponentLookup<District>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RW_ComponentLookup = state.GetComponentLookup<CurrentDistrict>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RW_ComponentLookup = state.GetComponentLookup<BorderDistrict>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BorderDistrict>();
      }
    }
  }
}
