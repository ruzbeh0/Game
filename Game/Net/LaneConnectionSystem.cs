// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneConnectionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class LaneConnectionSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Areas.UpdateCollectSystem m_AreaUpdateCollectSystem;
    private EntityQuery m_UpdatedQuery;
    private LaneConnectionSystem.TypeHandle __TypeHandle;

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
      this.m_AreaUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Areas.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<SubLane>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Node>(),
          ComponentType.ReadOnly<Object>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
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
      if (!flag && !this.m_AreaUpdateCollectSystem.lotsUpdated && !this.m_AreaUpdateCollectSystem.spacesUpdated)
        return;
      NativeQueue<Entity> nativeQueue1 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue2 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue3 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.lotsUpdated)
      {
        JobHandle dependencies1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedLotBounds = this.m_AreaUpdateCollectSystem.GetUpdatedLotBounds(out dependencies1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle2 = new LaneConnectionSystem.FindUpdatedLanesJob()
        {
          m_Bounds = updatedLotBounds.AsDeferredJobArray(),
          m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
          m_ResultQueue = nativeQueue1.AsParallelWriter()
        }.Schedule<LaneConnectionSystem.FindUpdatedLanesJob, Bounds2>(updatedLotBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddLotBoundsReader(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle2);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.spacesUpdated)
      {
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedSpaceBounds = this.m_AreaUpdateCollectSystem.GetUpdatedSpaceBounds(out dependencies3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle3 = new LaneConnectionSystem.FindUpdatedLanesJob()
        {
          m_Bounds = updatedSpaceBounds.AsDeferredJobArray(),
          m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies4),
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
          m_ResultQueue = nativeQueue2.AsParallelWriter()
        }.Schedule<LaneConnectionSystem.FindUpdatedLanesJob, Bounds2>(updatedSpaceBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies3, dependencies4));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddSpaceBoundsReader(jobHandle3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle3);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle3);
      }
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle;
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
        LaneConnectionSystem.CheckUpdatedLanesJob jobData = new LaneConnectionSystem.CheckUpdatedLanesJob()
        {
          m_Chunks = this.m_UpdatedQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
          m_ResultQueue = nativeQueue3
        };
        JobHandle jobHandle4 = jobData.Schedule<LaneConnectionSystem.CheckUpdatedLanesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        jobData.m_Chunks.Dispose(jobHandle4);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle4);
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LaneConnectionSystem.ListUpdatedLanesJob jobData1 = new LaneConnectionSystem.ListUpdatedLanesJob()
      {
        m_UpdatedQueue1 = nativeQueue1,
        m_UpdatedQueue2 = nativeQueue2,
        m_UpdatedQueue3 = nativeQueue3,
        m_UpdatedList = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneConnection_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NavigationAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      LaneConnectionSystem.FindLaneConnectionJob jobData2 = new LaneConnectionSystem.FindLaneConnectionJob()
      {
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabNavigationAreaData = this.__TypeHandle.__Game_Prefabs_NavigationAreaData_RO_ComponentLookup,
        m_LaneConnectionData = this.__TypeHandle.__Game_Net_LaneConnection_RW_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentLookup,
        m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_Entities = nativeList.AsDeferredJobArray(),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      JobHandle jobHandle5 = jobData1.Schedule<LaneConnectionSystem.ListUpdatedLanesJob>(jobHandle1);
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle5, dependencies);
      JobHandle jobHandle6 = jobData2.Schedule<LaneConnectionSystem.FindLaneConnectionJob, Entity>(list, 1, dependsOn);
      nativeQueue1.Dispose(jobHandle5);
      nativeQueue2.Dispose(jobHandle5);
      nativeQueue3.Dispose(jobHandle5);
      nativeList.Dispose(jobHandle6);
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
    public LaneConnectionSystem()
    {
    }

    [BurstCompile]
    private struct FindUpdatedLanesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneConnectionSystem.FindUpdatedLanesJob.Iterator iterator = new LaneConnectionSystem.FindUpdatedLanesJob.Iterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_CurveData = this.m_CurveData,
          m_SubLanes = this.m_SubLanes,
          m_ResultQueue = this.m_ResultQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<LaneConnectionSystem.FindUpdatedLanesJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Curve> m_CurveData;
        public BufferLookup<SubLane> m_SubLanes;
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
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_SubLanes.HasBuffer(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubLane> subLane = this.m_SubLanes[entity];
          for (int index = 0; index < subLane.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (MathUtils.Intersect(MathUtils.Bounds(this.m_CurveData[subLane[index].m_SubLane].m_Bezier.xz), this.m_Bounds))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ResultQueue.Enqueue(entity);
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct CheckUpdatedLanesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;
      public NativeQueue<Entity> m_ResultQueue;

      public void Execute()
      {
        NativeHashSet<Entity> nativeHashSet = new NativeHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
          for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
          {
            DynamicBuffer<SubLane> dynamicBuffer = bufferAccessor[index2];
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ResultQueue.Enqueue(dynamicBuffer[index3].m_SubLane);
            }
            Owner owner;
            if (CollectionUtils.TryGet<Owner>(nativeArray, index2, out owner))
            {
              Owner componentData;
              // ISSUE: reference to a compiler-generated field
              while (this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData))
                owner = componentData;
              DynamicBuffer<SpawnLocationElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnLocations.TryGetBuffer(owner.m_Owner, out bufferData) && nativeHashSet.Add(owner.m_Owner))
              {
                for (int index4 = 0; index4 < bufferData.Length; ++index4)
                {
                  SpawnLocationElement spawnLocationElement = bufferData[index4];
                  if (spawnLocationElement.m_Type == SpawnLocationType.ParkingLane)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResultQueue.Enqueue(spawnLocationElement.m_SpawnLocation);
                  }
                }
              }
            }
          }
        }
        nativeHashSet.Dispose();
      }
    }

    [BurstCompile]
    private struct ListUpdatedLanesJob : IJob
    {
      public NativeQueue<Entity> m_UpdatedQueue1;
      public NativeQueue<Entity> m_UpdatedQueue2;
      public NativeQueue<Entity> m_UpdatedQueue3;
      public NativeList<Entity> m_UpdatedList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_UpdatedQueue1.Count;
        // ISSUE: reference to a compiler-generated field
        int num1 = count + this.m_UpdatedQueue2.Count;
        // ISSUE: reference to a compiler-generated field
        int length = num1 + this.m_UpdatedQueue3.Count;
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
        for (int index = num1; index < length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedList[index] = this.m_UpdatedQueue3.Dequeue();
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> updatedList = this.m_UpdatedList;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneConnectionSystem.ListUpdatedLanesJob.EntityComparer entityComparer = new LaneConnectionSystem.ListUpdatedLanesJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        LaneConnectionSystem.ListUpdatedLanesJob.EntityComparer comp = entityComparer;
        updatedList.Sort<Entity, LaneConnectionSystem.ListUpdatedLanesJob.EntityComparer>(comp);
        Entity entity = Entity.Null;
        int num2 = 0;
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num2 < this.m_UpdatedList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity updated = this.m_UpdatedList[num2++];
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
    private struct FindLaneConnectionJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<NavigationAreaData> m_PrefabNavigationAreaData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<LaneConnection> m_LaneConnectionData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public BufferLookup<SubLane> m_Lanes;
      [ReadOnly]
      public BufferLookup<SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated method
        LaneConnection laneConnection1 = this.FindLaneConnection(entity);
        if (laneConnection1.m_StartLane == Entity.Null && laneConnection1.m_EndLane == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_LaneConnectionData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_UpdatedData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(index, entity, new PathfindUpdated());
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<LaneConnection>(index, entity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneConnectionData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            LaneConnection laneConnection2 = this.m_LaneConnectionData[entity];
            if (!(laneConnection2.m_StartLane != laneConnection1.m_StartLane) && !(laneConnection2.m_EndLane != laneConnection1.m_EndLane) && (double) laneConnection2.m_StartPosition == (double) laneConnection1.m_StartPosition && (double) laneConnection2.m_EndPosition == (double) laneConnection1.m_EndPosition)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_LaneConnectionData[entity] = laneConnection1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_UpdatedData.HasComponent(entity))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(index, entity, new PathfindUpdated());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LaneConnection>(index, entity, laneConnection1);
            // ISSUE: reference to a compiler-generated field
            if (this.m_UpdatedData.HasComponent(entity))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(index, entity, new PathfindUpdated());
          }
        }
      }

      public LaneConnection FindLaneConnection(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SlaveLaneData.HasComponent(entity) || this.m_ConnectionLaneData.HasComponent(entity))
          return new LaneConnection();
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabNetLaneData.HasComponent(prefabRef.m_Prefab))
          return new LaneConnection();
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData = this.m_PrefabNetLaneData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if ((netLaneData.m_Flags & LaneFlags.Parking) != (LaneFlags) 0 && this.m_ParkingLaneData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          ParkingLane parkingLane = this.m_ParkingLaneData[entity];
          LaneConnection laneConnection = new LaneConnection();
          if ((parkingLane.m_Flags & ParkingLaneFlags.FindConnections) != (ParkingLaneFlags) 0)
          {
            parkingLane.m_Flags &= ~ParkingLaneFlags.SecondaryStart;
            parkingLane.m_SecondaryStartNode = new PathNode();
            // ISSUE: reference to a compiler-generated method
            laneConnection = this.FindParkingConnection(entity, ref parkingLane, prefabRef, netLaneData);
            // ISSUE: reference to a compiler-generated field
            this.m_ParkingLaneData[entity] = parkingLane;
          }
          return laneConnection;
        }
        // ISSUE: reference to a compiler-generated method
        return (netLaneData.m_Flags & (LaneFlags.Road | LaneFlags.Pedestrian)) != (LaneFlags) 0 ? this.FindAreaConnection(entity, prefabRef, netLaneData) : new LaneConnection();
      }

      private LaneConnection FindParkingConnection(
        Entity entity,
        ref ParkingLane parkingLane,
        PrefabRef prefabRef,
        NetLaneData netLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[entity];
        // ISSUE: reference to a compiler-generated field
        Owner owner1 = this.m_OwnerData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.HasComponent(owner1.m_Owner) && !this.m_BuildingData.HasComponent(owner1.m_Owner))
        {
          // ISSUE: reference to a compiler-generated field
          owner1 = this.m_OwnerData[owner1.m_Owner];
        }
        LaneConnection result = new LaneConnection();
        float3 pedestrianSearchPosition = MathUtils.Position(curve.m_Bezier, 0.5f);
        float maxValue1 = float.MaxValue;
        float maxValue2 = float.MaxValue;
        float3 float3 = pedestrianSearchPosition;
        float2 xz = MathUtils.Tangent(curve.m_Bezier, 0.5f).xz;
        ParkingLaneData componentData;
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.TryNormalize(ref xz) && this.m_PrefabParkingLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData))
          float3.xz -= MathUtils.RotateRight(xz, componentData.m_SlotAngle) * (componentData.m_SlotSize.y * 0.5f);
        // ISSUE: reference to a compiler-generated method
        this.FindParkingConnection(owner1.m_Owner, pedestrianSearchPosition, float3, ref result, ref maxValue1, ref maxValue2);
        DynamicBuffer<InstalledUpgrade> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InstalledUpgrades.TryGetBuffer(owner1.m_Owner, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindParkingConnection(bufferData[index].m_Upgrade, pedestrianSearchPosition, float3, ref result, ref maxValue1, ref maxValue2);
          }
        }
        if ((netLaneData.m_Flags & LaneFlags.Twoway) != (LaneFlags) 0 && result.m_StartLane != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          Owner owner2 = this.m_OwnerData[result.m_StartLane];
          // ISSUE: reference to a compiler-generated field
          CarLane carLane1 = this.m_CarLaneData[result.m_StartLane];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubLane> lane1 = this.m_Lanes[owner2.m_Owner];
          for (int index = 0; index < lane1.Length; ++index)
          {
            Entity subLane = lane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SlaveLaneData.HasComponent(subLane) && !this.m_ConnectionLaneData.HasComponent(subLane) && this.m_CarLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              CarLane carLane2 = this.m_CarLaneData[subLane];
              if (((carLane1.m_Flags ^ carLane2.m_Flags) & CarLaneFlags.Invert) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (int) carLane1.m_CarriagewayGroup == (int) carLane2.m_CarriagewayGroup)
              {
                // ISSUE: reference to a compiler-generated field
                Lane lane2 = this.m_LaneData[subLane];
                float t;
                // ISSUE: reference to a compiler-generated field
                double num = (double) MathUtils.Distance(this.m_CurveData[subLane].m_Bezier, float3, out t);
                parkingLane.m_SecondaryStartNode = new PathNode(lane2.m_MiddleNode, t);
                parkingLane.m_Flags |= ParkingLaneFlags.SecondaryStart;
                break;
              }
            }
          }
        }
        return result;
      }

      private void FindParkingConnection(
        Entity owner,
        float3 pedestrianSearchPosition,
        float3 roadSearchPosition,
        ref LaneConnection result,
        ref float bestPedestrianDistance,
        ref float bestRoadDistance)
      {
        DynamicBuffer<SubNet> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.TryGetBuffer(owner, out bufferData1))
        {
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
          {
            Entity subNet = bufferData1[index1].m_SubNet;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Lanes.HasBuffer(subNet))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<SubLane> lane = this.m_Lanes[subNet];
              for (int index2 = 0; index2 < lane.Length; ++index2)
              {
                Entity subLane = lane[index2].m_SubLane;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_SlaveLaneData.HasComponent(subLane) && !this.m_ConnectionLaneData.HasComponent(subLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[subLane];
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[subLane];
                  // ISSUE: reference to a compiler-generated field
                  NetLaneData netLaneData = this.m_PrefabNetLaneData[prefabRef.m_Prefab];
                  if ((netLaneData.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0)
                  {
                    if ((double) MathUtils.Distance(MathUtils.Bounds(curve.m_Bezier), pedestrianSearchPosition) < (double) bestPedestrianDistance)
                    {
                      float t;
                      float num = MathUtils.Distance(curve.m_Bezier, pedestrianSearchPosition, out t);
                      if ((double) num < (double) bestPedestrianDistance)
                      {
                        bestPedestrianDistance = num;
                        result.m_EndLane = subLane;
                        result.m_EndPosition = t;
                      }
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if ((netLaneData.m_Flags & LaneFlags.Road) != (LaneFlags) 0 && (this.m_PrefabCarLaneData[prefabRef.m_Prefab].m_RoadTypes & RoadTypes.Car) != RoadTypes.None && (double) MathUtils.Distance(MathUtils.Bounds(curve.m_Bezier), roadSearchPosition) < (double) bestRoadDistance)
                    {
                      float t;
                      float num = MathUtils.Distance(curve.m_Bezier, roadSearchPosition, out t);
                      if ((double) num < (double) bestRoadDistance)
                      {
                        bestRoadDistance = num;
                        result.m_StartLane = subLane;
                        result.m_StartPosition = t;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        DynamicBuffer<Game.Areas.SubArea> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubAreas.TryGetBuffer(owner, out bufferData2))
          return;
        for (int index3 = 0; index3 < bufferData2.Length; ++index3)
        {
          Entity area = bufferData2[index3].m_Area;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Lanes.HasBuffer(area))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[area];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Triangle> areaTriangle = this.m_AreaTriangles[area];
            float num1 = bestPedestrianDistance;
            float num2 = bestRoadDistance;
            Triangle3 triangle3_1 = new Triangle3();
            Triangle3 triangle3_2 = new Triangle3();
            float3 position1 = new float3();
            float3 position2 = new float3();
            bool flag = false;
            for (int index4 = 0; index4 < areaTriangle.Length; ++index4)
            {
              Triangle triangle = areaTriangle[index4];
              Triangle3 triangle3_3 = AreaUtils.GetTriangle3(areaNode, triangle);
              float2 t1;
              float num3 = MathUtils.Distance(triangle3_3, pedestrianSearchPosition, out t1);
              float2 t2;
              float num4 = MathUtils.Distance(triangle3_3, roadSearchPosition, out t2);
              if ((double) num3 < (double) num1)
              {
                num1 = num3;
                triangle3_1 = triangle3_3;
                position1 = MathUtils.Position(triangle3_3, t1);
                flag = true;
              }
              if ((double) num4 < (double) num2)
              {
                num2 = num4;
                triangle3_2 = triangle3_3;
                position2 = MathUtils.Position(triangle3_3, t2);
                flag = true;
              }
            }
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<SubLane> lane = this.m_Lanes[area];
              float num5 = float.MaxValue;
              float num6 = float.MaxValue;
              for (int index5 = 0; index5 < lane.Length; ++index5)
              {
                Entity subLane = lane[index5].m_SubLane;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectionLaneData.HasComponent(subLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane];
                  if ((connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
                  {
                    if ((double) num1 >= (double) bestPedestrianDistance)
                      continue;
                  }
                  else if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && ((double) num2 >= (double) bestRoadDistance || (connectionLane.m_RoadTypes & RoadTypes.Car) == RoadTypes.None))
                    continue;
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[subLane];
                  float2 t3;
                  if ((connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
                  {
                    if (MathUtils.Intersect(triangle3_1.xz, curve.m_Bezier.a.xz, out t3) || MathUtils.Intersect(triangle3_1.xz, curve.m_Bezier.d.xz, out t3))
                    {
                      float t4;
                      float num7 = MathUtils.Distance(curve.m_Bezier, position1, out t4);
                      if ((double) num7 < (double) num5)
                      {
                        num5 = num7;
                        result.m_EndLane = subLane;
                        result.m_EndPosition = t4;
                      }
                    }
                  }
                  else if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (MathUtils.Intersect(triangle3_2.xz, curve.m_Bezier.a.xz, out t3) || MathUtils.Intersect(triangle3_2.xz, curve.m_Bezier.d.xz, out t3)))
                  {
                    float t5;
                    float num8 = MathUtils.Distance(curve.m_Bezier, position2, out t5);
                    if ((double) num8 < (double) num6)
                    {
                      num6 = num8;
                      result.m_StartLane = subLane;
                      result.m_StartPosition = t5;
                    }
                  }
                }
              }
              if ((double) num5 != 3.4028234663852886E+38)
                bestPedestrianDistance = num1;
              if ((double) num6 != 3.4028234663852886E+38)
                bestRoadDistance = num2;
            }
          }
        }
      }

      private LaneConnection FindAreaConnection(
        Entity entity,
        PrefabRef prefabRef,
        NetLaneData netLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[entity];
        ConnectionLaneFlags connectionLaneFlags = (ConnectionLaneFlags) 0;
        RoadTypes roadTypes = RoadTypes.None;
        if ((netLaneData.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0)
          connectionLaneFlags |= ConnectionLaneFlags.Pedestrian;
        if ((netLaneData.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          CarLaneData carLaneData = this.m_PrefabCarLaneData[prefabRef.m_Prefab];
          connectionLaneFlags |= ConnectionLaneFlags.Road;
          roadTypes |= carLaneData.m_RoadTypes;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneConnectionSystem.FindLaneConnectionJob.Iterator iterator = new LaneConnectionSystem.FindLaneConnectionJob.Iterator()
        {
          m_Curve = curve,
          m_MaxDistance = (float2) 2f,
          m_LaneFlags = connectionLaneFlags,
          m_RoadTypes = roadTypes,
          m_CurveData = this.m_CurveData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabNavigationAreaData = this.m_PrefabNavigationAreaData,
          m_Lanes = this.m_Lanes,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<LaneConnectionSystem.FindLaneConnectionJob.Iterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        return iterator.m_BestConnection;
      }

      private struct Iterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Curve m_Curve;
        public float2 m_MaxDistance;
        public ConnectionLaneFlags m_LaneFlags;
        public RoadTypes m_RoadTypes;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<ConnectionLane> m_ConnectionLaneData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<NavigationAreaData> m_PrefabNavigationAreaData;
        public BufferLookup<SubLane> m_Lanes;
        public BufferLookup<Game.Areas.Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;
        public LaneConnection m_BestConnection;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Curve.m_Bezier.a.xz) | MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Curve.m_Bezier.d.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          bool2 x1;
          // ISSUE: reference to a compiler-generated field
          x1.x = MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Curve.m_Bezier.a.xz);
          // ISSUE: reference to a compiler-generated field
          x1.y = MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Curve.m_Bezier.d.xz);
          // ISSUE: reference to a compiler-generated field
          if (!math.any(x1) || !this.m_Lanes.HasBuffer(item.m_Area))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Triangle3 triangle3 = AreaUtils.GetTriangle3(this.m_AreaNodes[item.m_Area], this.m_AreaTriangles[item.m_Area][item.m_Triangle]);
          float2 maxValue1 = (float2) float.MaxValue;
          float2 t1;
          // ISSUE: reference to a compiler-generated field
          if (x1.x && MathUtils.Intersect(triangle3.xz, this.m_Curve.m_Bezier.a.xz, out t1))
          {
            // ISSUE: reference to a compiler-generated field
            maxValue1.x = math.abs(MathUtils.Position(triangle3, t1).y - this.m_Curve.m_Bezier.a.y);
          }
          float2 t2;
          // ISSUE: reference to a compiler-generated field
          if (x1.y && MathUtils.Intersect(triangle3.xz, this.m_Curve.m_Bezier.d.xz, out t2))
          {
            // ISSUE: reference to a compiler-generated field
            maxValue1.y = math.abs(MathUtils.Position(triangle3, t2).y - this.m_Curve.m_Bezier.d.y);
          }
          // ISSUE: reference to a compiler-generated field
          bool2 x2 = maxValue1 < this.m_MaxDistance;
          if (!math.any(x2))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubLane> lane = this.m_Lanes[item.m_Area];
          float2 maxValue2 = (float2) float.MaxValue;
          LaneConnection laneConnection = new LaneConnection();
          for (int index = 0; index < lane.Length; ++index)
          {
            Entity subLane = lane[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((connectionLane.m_Flags & this.m_LaneFlags) != (ConnectionLaneFlags) 0 && !(this.m_RoadTypes != 0 & (connectionLane.m_RoadTypes & this.m_RoadTypes) == RoadTypes.None))
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane];
                float2 t3;
                if (MathUtils.Intersect(triangle3.xz, curve.m_Bezier.a.xz, out t3) || MathUtils.Intersect(triangle3.xz, curve.m_Bezier.d.xz, out t3))
                {
                  if (x2.x)
                  {
                    float t4;
                    // ISSUE: reference to a compiler-generated field
                    float num = MathUtils.Distance(curve.m_Bezier, this.m_Curve.m_Bezier.a, out t4);
                    if ((double) num < (double) maxValue2.x)
                    {
                      maxValue2.x = num;
                      laneConnection.m_StartLane = subLane;
                      laneConnection.m_StartPosition = t4;
                    }
                  }
                  if (x2.y)
                  {
                    float t5;
                    // ISSUE: reference to a compiler-generated field
                    float num = MathUtils.Distance(curve.m_Bezier, this.m_Curve.m_Bezier.d, out t5);
                    if ((double) num < (double) maxValue2.y)
                    {
                      maxValue2.y = num;
                      laneConnection.m_EndLane = subLane;
                      laneConnection.m_EndPosition = t5;
                    }
                  }
                }
              }
            }
          }
          if (laneConnection.m_StartLane != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_MaxDistance.x = maxValue1.x;
            // ISSUE: reference to a compiler-generated field
            this.m_BestConnection.m_StartLane = laneConnection.m_StartLane;
            // ISSUE: reference to a compiler-generated field
            this.m_BestConnection.m_StartPosition = laneConnection.m_StartPosition;
          }
          if (!(laneConnection.m_EndLane != Entity.Null))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_MaxDistance.y = maxValue1.y;
          // ISSUE: reference to a compiler-generated field
          this.m_BestConnection.m_EndLane = laneConnection.m_EndLane;
          // ISSUE: reference to a compiler-generated field
          this.m_BestConnection.m_EndPosition = laneConnection.m_EndPosition;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NavigationAreaData> __Game_Prefabs_NavigationAreaData_RO_ComponentLookup;
      public ComponentLookup<LaneConnection> __Game_Net_LaneConnection_RW_ComponentLookup;
      public ComponentLookup<ParkingLane> __Game_Net_ParkingLane_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RO_BufferLookup = state.GetBufferLookup<SpawnLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NavigationAreaData_RO_ComponentLookup = state.GetComponentLookup<NavigationAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneConnection_RW_ComponentLookup = state.GetComponentLookup<LaneConnection>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RW_ComponentLookup = state.GetComponentLookup<ParkingLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
      }
    }
  }
}
