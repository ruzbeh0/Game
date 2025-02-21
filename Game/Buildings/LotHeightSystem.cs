// Decompiled with JetBrains decompiler
// Type: Game.Buildings.LotHeightSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class LotHeightSystem : GameSystemBase
  {
    private Game.Objects.UpdateCollectSystem m_ObjectUpdateCollectSystem;
    private Game.Net.UpdateCollectSystem m_NetUpdateCollectSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_UpdateQuery;
    private EntityQuery m_AllQuery;
    private bool m_Loaded;
    private LotHeightSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Objects.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Net.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(ComponentType.ReadWrite<Lot>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllQuery = this.GetEntityQuery(ComponentType.ReadWrite<Lot>(), ComponentType.Exclude<Deleted>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = loaded ? this.m_AllQuery : this.m_UpdateQuery;
      bool flag = !query.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ObjectUpdateCollectSystem.isUpdated && !this.m_NetUpdateCollectSystem.netsUpdated && !flag)
        return;
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> staticSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies1);
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue1 = new NativeQueue<Entity>();
      NativeQueue<Entity> nativeQueue2 = new NativeQueue<Entity>();
      JobHandle jobHandle1 = new JobHandle();
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle job1 = new LotHeightSystem.AddUpdatedLotsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_ResultList = nativeList
        }.Schedule<LotHeightSystem.AddUpdatedLotsJob>(query, this.Dependency);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, job1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ObjectUpdateCollectSystem.isUpdated)
      {
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedBounds = this.m_ObjectUpdateCollectSystem.GetUpdatedBounds(out dependencies2);
        nativeQueue1 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle2 = new LotHeightSystem.FindUpdatedLotsJob()
        {
          m_Bounds = updatedBounds.AsDeferredJobArray(),
          m_SearchTree = staticSearchTree,
          m_LotData = this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup,
          m_ResultQueue = nativeQueue1.AsParallelWriter()
        }.Schedule<LotHeightSystem.FindUpdatedLotsJob, Bounds2>(updatedBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies2, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectUpdateCollectSystem.AddBoundsReader(jobHandle2);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_NetUpdateCollectSystem.netsUpdated)
      {
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedNetBounds = this.m_NetUpdateCollectSystem.GetUpdatedNetBounds(out dependencies3);
        nativeQueue2 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle3 = new LotHeightSystem.FindUpdatedLotsJob()
        {
          m_Bounds = updatedNetBounds.AsDeferredJobArray(),
          m_SearchTree = staticSearchTree,
          m_LotData = this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup,
          m_ResultQueue = nativeQueue2.AsParallelWriter()
        }.Schedule<LotHeightSystem.FindUpdatedLotsJob, Bounds2>(updatedNetBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies3, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetUpdateCollectSystem.AddNetBoundsReader(jobHandle3);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle3);
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LotHeightSystem.CollectLotsJob jobData1 = new LotHeightSystem.CollectLotsJob()
      {
        m_Queue1 = nativeQueue1,
        m_Queue2 = nativeQueue2,
        m_ResultList = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Lot_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LotHeightSystem.UpdateLotHeightsJob jobData2 = new LotHeightSystem.UpdateLotHeightsJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabNetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabBuildingTerraformData = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_LotData = this.__TypeHandle.__Game_Buildings_Lot_RW_ComponentLookup,
        m_IsLoaded = loaded,
        m_LotList = nativeList,
        m_StaticObjectSearchTree = staticSearchTree,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies4),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      JobHandle jobHandle4 = jobData1.Schedule<LotHeightSystem.CollectLotsJob>(jobHandle1);
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle4, dependencies1, dependencies4);
      JobHandle jobHandle5 = jobData2.Schedule<LotHeightSystem.UpdateLotHeightsJob, Entity>(list, 1, dependsOn);
      if (nativeQueue1.IsCreated)
        nativeQueue1.Dispose(jobHandle4);
      if (nativeQueue2.IsCreated)
        nativeQueue2.Dispose(jobHandle4);
      nativeList.Dispose(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle5);
      this.Dependency = jobHandle5;
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
    public LotHeightSystem()
    {
    }

    [BurstCompile]
    private struct AddUpdatedLotsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public NativeList<Entity> m_ResultList;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResultList.AddRange(chunk.GetNativeArray(this.m_EntityType));
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
    private struct FindUpdatedLotsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      [ReadOnly]
      public ComponentLookup<Lot> m_LotData;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LotHeightSystem.FindUpdatedLotsJob.Iterator iterator = new LotHeightSystem.FindUpdatedLotsJob.Iterator()
        {
          m_Bounds = MathUtils.Expand(this.m_Bounds[index], (float2) 8f),
          m_LotData = this.m_LotData,
          m_ResultQueue = this.m_ResultQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<LotHeightSystem.FindUpdatedLotsJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Lot> m_LotData;
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
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_LotData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultQueue.Enqueue(entity);
        }
      }
    }

    [BurstCompile]
    private struct CollectLotsJob : IJob
    {
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeQueue<Entity> m_Queue1;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeQueue<Entity> m_Queue2;
      public NativeList<Entity> m_ResultList;

      public void Execute()
      {
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Queue1.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          num1 += this.m_Queue1.Count;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Queue2.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          num1 += this.m_Queue2.Count;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResultList.Capacity = this.m_ResultList.Length + num1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Queue1.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> array = this.m_Queue1.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          this.m_ResultList.AddRange(array);
          array.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Queue2.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> array = this.m_Queue2.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          this.m_ResultList.AddRange(array);
          array.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ResultList.Sort<Entity>();
        Entity entity = Entity.Null;
        int num2 = 0;
        int index = 0;
        // ISSUE: reference to a compiler-generated field
        while (num2 < this.m_ResultList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity result = this.m_ResultList[num2++];
          if (result != entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResultList[index++] = result;
            entity = result;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index >= this.m_ResultList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResultList.RemoveRange(index, this.m_ResultList.Length - index);
      }
    }

    private struct Heights
    {
      public Bounds1 m_FlexibleBounds;
      public Bounds1 m_RigidBounds;
      public float m_FlexibleStrength;
      public float m_RigidStrength;

      public void Reset()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FlexibleBounds = new Bounds1();
        // ISSUE: reference to a compiler-generated field
        this.m_RigidBounds = new Bounds1();
        // ISSUE: reference to a compiler-generated field
        this.m_FlexibleStrength = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_RigidStrength = 0.0f;
      }

      public float Center()
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = MathUtils.Center(this.m_FlexibleBounds);
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_RigidStrength != 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float num2 = MathUtils.Center(this.m_RigidBounds);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num3 = math.min(this.m_FlexibleStrength, 1f - this.m_RigidStrength);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num1 = (float) (((double) num1 * (double) num3 + (double) num2 * (double) this.m_RigidStrength) / ((double) num3 + (double) this.m_RigidStrength));
        }
        return num1;
      }
    }

    [BurstCompile]
    private struct UpdateLotHeightsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabNetCompositionData;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> m_PrefabBuildingTerraformData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Lot> m_LotData;
      [ReadOnly]
      public bool m_IsLoaded;
      [ReadOnly]
      public NativeList<Entity> m_LotList;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity lot1 = this.m_LotList[index];
        // ISSUE: reference to a compiler-generated field
        Lot lot2 = this.m_LotData[lot1];
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[lot1];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[lot1];
        Lot lot3 = lot2;
        int2 lotSize = (int2) 1;
        BuildingData componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabBuildingData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
        {
          lotSize = componentData1.m_LotSize;
        }
        else
        {
          BuildingExtensionData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabBuildingExtensionData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            if (!componentData2.m_External)
              return;
            lotSize = componentData2.m_LotSize;
          }
        }
        bool flag = false;
        ObjectGeometryData componentData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
        {
          Game.Objects.GeometryFlags geometryFlags = (componentData3.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None ? Game.Objects.GeometryFlags.CircularLeg : Game.Objects.GeometryFlags.Circular;
          flag = (componentData3.m_Flags & geometryFlags) != 0;
        }
        Quad3 corners1 = BuildingUtils.CalculateCorners(transform, lotSize);
        Quad3 corners2 = BuildingUtils.CalculateCorners(transform, lotSize + 2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        LotHeightSystem.UpdateLotHeightsJob.LotIterator iterator = new LotHeightSystem.UpdateLotHeightsJob.LotIterator()
        {
          m_Ignore = lot1,
          m_OwnerData = this.m_OwnerData,
          m_LotData = this.m_LotData,
          m_TransformData = this.m_TransformData,
          m_EdgeGeometryData = this.m_EdgeGeometryData,
          m_StartNodeGeometryData = this.m_StartNodeGeometryData,
          m_EndNodeGeometryData = this.m_EndNodeGeometryData,
          m_CompositionData = this.m_CompositionData,
          m_OrphanData = this.m_OrphanData,
          m_NodeData = this.m_NodeData,
          m_EdgeData = this.m_EdgeData,
          m_ElevationData = this.m_ElevationData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabBuildingData = this.m_PrefabBuildingData,
          m_PrefabBuildingExtensionData = this.m_PrefabBuildingExtensionData,
          m_PrefabNetGeometryData = this.m_PrefabNetGeometryData,
          m_PrefabNetCompositionData = this.m_PrefabNetCompositionData,
          m_PrefabBuildingTerraformData = this.m_PrefabBuildingTerraformData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData
        };
        if (flag)
        {
          corners1.a = transform.m_Position + (corners1.a - transform.m_Position) * 0.707106769f;
          corners1.b = transform.m_Position + (corners1.b - transform.m_Position) * 0.707106769f;
          corners1.c = transform.m_Position + (corners1.c - transform.m_Position) * 0.707106769f;
          corners1.d = transform.m_Position + (corners1.d - transform.m_Position) * 0.707106769f;
          // ISSUE: reference to a compiler-generated field
          iterator.m_Radius = (float) lotSize.x * 4f;
          // ISSUE: reference to a compiler-generated field
          iterator.m_Position = transform.m_Position;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights1.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights2.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights3.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights4.Reset();
        // ISSUE: reference to a compiler-generated field
        iterator.m_Quad = new Quad3(corners1.a, corners1.b, corners2.b, corners2.a);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LotHeightSystem.Heights heights1 = iterator.m_Heights1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_FrontHeights.y = iterator.m_Heights2.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_FrontHeights.z = iterator.m_Heights3.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_Heights1 = iterator.m_Heights4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights2.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights3.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights4.Reset();
        // ISSUE: reference to a compiler-generated field
        iterator.m_Quad = new Quad3(corners1.b, corners1.c, corners2.c, corners2.b);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_RightHeights.x = iterator.m_Heights1.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_RightHeights.y = iterator.m_Heights2.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_RightHeights.z = iterator.m_Heights3.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_Heights1 = iterator.m_Heights4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights2.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights3.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights4.Reset();
        // ISSUE: reference to a compiler-generated field
        iterator.m_Quad = new Quad3(corners1.c, corners1.d, corners2.d, corners2.c);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_BackHeights.x = iterator.m_Heights1.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_BackHeights.y = iterator.m_Heights2.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_BackHeights.z = iterator.m_Heights3.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_Heights1 = iterator.m_Heights4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights2.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        iterator.m_Heights3.Reset();
        // ISSUE: reference to a compiler-generated field
        iterator.m_Heights4 = heights1;
        // ISSUE: reference to a compiler-generated field
        iterator.m_Quad = new Quad3(corners1.d, corners1.a, corners2.a, corners2.d);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<LotHeightSystem.UpdateLotHeightsJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_LeftHeights.x = iterator.m_Heights1.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_LeftHeights.y = iterator.m_Heights2.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_LeftHeights.z = iterator.m_Heights3.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lot2.m_FrontHeights.x = iterator.m_Heights4.Center();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerData = iterator.m_OwnerData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LotData = iterator.m_LotData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TransformData = iterator.m_TransformData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeGeometryData = iterator.m_EdgeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometryData = iterator.m_StartNodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometryData = iterator.m_EndNodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CompositionData = iterator.m_CompositionData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OrphanData = iterator.m_OrphanData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NodeData = iterator.m_NodeData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeData = iterator.m_EdgeData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ElevationData = iterator.m_ElevationData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefData = iterator.m_PrefabRefData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabBuildingData = iterator.m_PrefabBuildingData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabBuildingExtensionData = iterator.m_PrefabBuildingExtensionData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetGeometryData = iterator.m_PrefabNetGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetCompositionData = iterator.m_PrefabNetCompositionData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabBuildingTerraformData = iterator.m_PrefabBuildingTerraformData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabObjectGeometryData = iterator.m_PrefabObjectGeometryData;
        if (flag)
        {
          float3 float3 = new float3(1.41421354f, 1.03527617f, 1.03527617f);
          lot2.m_FrontHeights *= float3;
          lot2.m_RightHeights *= float3;
          lot2.m_BackHeights *= float3;
          lot2.m_LeftHeights *= float3;
        }
        // ISSUE: reference to a compiler-generated method
        this.CalculateMiddleHeights(lot2.m_FrontHeights.x, ref lot2.m_FrontHeights.y, ref lot2.m_FrontHeights.z, lot2.m_RightHeights.x);
        // ISSUE: reference to a compiler-generated method
        this.CalculateMiddleHeights(lot2.m_RightHeights.x, ref lot2.m_RightHeights.y, ref lot2.m_RightHeights.z, lot2.m_BackHeights.x);
        // ISSUE: reference to a compiler-generated method
        this.CalculateMiddleHeights(lot2.m_BackHeights.x, ref lot2.m_BackHeights.y, ref lot2.m_BackHeights.z, lot2.m_LeftHeights.x);
        // ISSUE: reference to a compiler-generated method
        this.CalculateMiddleHeights(lot2.m_LeftHeights.x, ref lot2.m_LeftHeights.y, ref lot2.m_LeftHeights.z, lot2.m_FrontHeights.x);
        float3 x1 = math.abs(lot2.m_FrontHeights - lot3.m_FrontHeights);
        float3 float3_1 = math.abs(lot2.m_RightHeights - lot3.m_RightHeights);
        float3 x2 = math.abs(lot2.m_BackHeights - lot3.m_BackHeights);
        float3 y1 = math.abs(lot2.m_LeftHeights - lot3.m_LeftHeights);
        float3 y2 = float3_1;
        if ((double) math.cmax(math.max(math.max(x1, y2), math.max(x2, y1))) < 0.0099999997764825821)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_LotData[lot1] = lot2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_IsLoaded)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(index, lot1, new Updated());
      }

      private void CalculateMiddleHeights(float a, ref float b, ref float c, float d)
      {
        float num1 = b - MathUtils.Position(new Bezier4x1(a, b, c, d), 0.333333343f);
        float num2 = c - MathUtils.Position(new Bezier4x1(a, b, c, d), 0.6666667f);
        b += (float) ((double) num1 * 3.0 - (double) num2 * 1.5);
        c += (float) ((double) num2 * 3.0 - (double) num1 * 1.5);
      }

      private struct LotIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public float m_Radius;
        public float3 m_Position;
        public Quad3 m_Quad;
        public Entity m_Ignore;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Lot> m_LotData;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<Orphan> m_OrphanData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<Edge> m_EdgeData;
        public ComponentLookup<Game.Net.Elevation> m_ElevationData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<BuildingData> m_PrefabBuildingData;
        public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
        public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
        public ComponentLookup<NetCompositionData> m_PrefabNetCompositionData;
        public ComponentLookup<BuildingTerraformData> m_PrefabBuildingTerraformData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
        public LotHeightSystem.Heights m_Heights1;
        public LotHeightSystem.Heights m_Heights2;
        public LotHeightSystem.Heights m_Heights3;
        public LotHeightSystem.Heights m_Heights4;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Quad.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Quad.xz) || entity == this.m_Ignore)
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LotData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[entity];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[entity];
            int2 lotSize = (int2) 0;
            BuildingTerraformData componentData1;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PrefabBuildingTerraformData.TryGetComponent(prefabRef.m_Prefab, out componentData1) || !componentData1.m_DontRaise || !componentData1.m_DontLower)
            {
              BuildingData componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabBuildingData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
              {
                lotSize = componentData2.m_LotSize;
              }
              else
              {
                BuildingExtensionData componentData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabBuildingExtensionData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
                  lotSize = math.select((int2) 0, componentData2.m_LotSize, componentData3.m_External);
              }
            }
            if (!math.all(lotSize > 0))
              return;
            bool flag = false;
            ObjectGeometryData componentData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData4))
            {
              Game.Objects.GeometryFlags geometryFlags = (componentData4.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None ? Game.Objects.GeometryFlags.CircularLeg : Game.Objects.GeometryFlags.Circular;
              flag = (componentData4.m_Flags & geometryFlags) != 0;
            }
            if (flag)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckCircle(transform.m_Position, (float) lotSize.x * 4f, false, componentData1.m_DontRaise, componentData1.m_DontLower);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckQuad(BuildingUtils.CalculateCorners(transform, lotSize), false, componentData1.m_DontRaise, componentData1.m_DontLower);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CompositionData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((this.m_PrefabNetGeometryData[this.m_PrefabRefData[entity].m_Prefab].m_Flags & Game.Net.GeometryFlags.FlattenTerrain) == (Game.Net.GeometryFlags) 0)
                return;
              bool ignore;
              // ISSUE: reference to a compiler-generated method
              if (this.HasLotOwner(entity, out ignore))
              {
                // ISSUE: reference to a compiler-generated field
                if (ignore || this.m_ElevationData.HasComponent(entity))
                  return;
                // ISSUE: reference to a compiler-generated field
                Edge edge = this.m_EdgeData[entity];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ElevationData.HasComponent(edge.m_Start) || this.m_ElevationData.HasComponent(edge.m_End))
                  return;
              }
              // ISSUE: reference to a compiler-generated field
              Composition composition = this.m_CompositionData[entity];
              // ISSUE: reference to a compiler-generated field
              EdgeGeometry geometry = this.m_EdgeGeometryData[entity];
              // ISSUE: reference to a compiler-generated field
              StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[entity];
              // ISSUE: reference to a compiler-generated field
              EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[entity];
              // ISSUE: reference to a compiler-generated method
              this.CheckEdgeGeometry(geometry, composition.m_Edge);
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeGeometry(startNodeGeometry.m_Geometry, composition.m_StartNode);
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeGeometry(endNodeGeometry.m_Geometry, composition.m_EndNode);
            }
            else
            {
              bool ignore;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              if (!this.m_OrphanData.HasComponent(entity) || (this.m_PrefabNetGeometryData[this.m_PrefabRefData[entity].m_Prefab].m_Flags & Game.Net.GeometryFlags.FlattenTerrain) == (Game.Net.GeometryFlags) 0 || this.HasLotOwner(entity, out ignore) && (ignore || this.m_ElevationData.HasComponent(entity)))
                return;
              // ISSUE: reference to a compiler-generated field
              Orphan orphan = this.m_OrphanData[entity];
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PrefabNetCompositionData.HasComponent(orphan.m_Composition))
                return;
              // ISSUE: reference to a compiler-generated field
              NetCompositionData netCompositionData = this.m_PrefabNetCompositionData[orphan.m_Composition];
              if ((netCompositionData.m_State & CompositionState.ExclusiveGround) == (CompositionState) 0 || ((netCompositionData.m_Flags.m_Left | netCompositionData.m_Flags.m_Right) & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) != (CompositionFlags.Side) 0)
                return;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CheckCircle(this.m_NodeData[entity].m_Position, netCompositionData.m_Width * 0.5f, true, false, false);
            }
          }
        }

        private bool HasLotOwner(Entity entity, out bool ignore)
        {
          Entity entity1 = entity;
          bool flag = false;
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            entity1 = this.m_OwnerData[entity1].m_Owner;
            // ISSUE: reference to a compiler-generated field
            if (entity1 == this.m_Ignore)
            {
              ignore = true;
              return true;
            }
            // ISSUE: reference to a compiler-generated field
            flag |= this.m_LotData.HasComponent(entity1);
          }
          ignore = false;
          return flag;
        }

        private void CheckEdgeGeometry(EdgeGeometry geometry, Entity composition)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(geometry.m_Bounds.xz, this.m_Quad.xz) || !this.m_PrefabNetCompositionData.HasComponent(composition))
            return;
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabNetCompositionData[composition];
          if ((netCompositionData.m_State & CompositionState.ExclusiveGround) == (CompositionState) 0)
            return;
          if ((netCompositionData.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0)
          {
            if ((double) geometry.m_Start.m_Length.x > 0.05000000074505806)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckCurve(geometry.m_Start.m_Left, true);
            }
            if ((double) geometry.m_End.m_Length.x > 0.05000000074505806)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckCurve(geometry.m_End.m_Left, true);
            }
          }
          if ((netCompositionData.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) != (CompositionFlags.Side) 0)
            return;
          if ((double) geometry.m_Start.m_Length.y > 0.05000000074505806)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckCurve(geometry.m_Start.m_Right, true);
          }
          if ((double) geometry.m_End.m_Length.y <= 0.05000000074505806)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckCurve(geometry.m_End.m_Right, true);
        }

        private void CheckNodeGeometry(EdgeNodeGeometry geometry, Entity composition)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(geometry.m_Bounds.xz, this.m_Quad.xz) || !this.m_PrefabNetCompositionData.HasComponent(composition))
            return;
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabNetCompositionData[composition];
          if ((netCompositionData.m_State & CompositionState.ExclusiveGround) == (CompositionState) 0)
            return;
          if ((netCompositionData.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0)
          {
            if ((double) geometry.m_Left.m_Length.x > 0.05000000074505806)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckCurve(geometry.m_Left.m_Left, true);
            }
            if ((double) geometry.m_Right.m_Length.x > 0.05000000074505806 && (netCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckCurve(geometry.m_Right.m_Left, true);
            }
          }
          if ((netCompositionData.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) != (CompositionFlags.Side) 0)
            return;
          if ((double) geometry.m_Right.m_Length.y > 0.05000000074505806)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckCurve(geometry.m_Right.m_Right, true);
          }
          if ((double) geometry.m_Left.m_Length.y <= 0.05000000074505806 || (netCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckCurve(geometry.m_Left.m_Right, true);
        }

        private void CheckCurve(Bezier4x3 curve, bool rigid)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(MathUtils.Bounds(curve.xz), this.m_Quad.xz))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_1 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.166666672f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_2 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.5f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_3 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.8333333f);
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_Radius != 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_1 = this.m_Position + (float3_1 - this.m_Position) * 1.22474492f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_2 = this.m_Position + (float3_2 - this.m_Position) * 1.41421354f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_3 = this.m_Position + (float3_3 - this.m_Position) * 1.22474492f;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCurve(new Line3.Segment(this.m_Quad.a, float3_1), 0.0f, rigid, curve, ref this.m_Heights1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCurve(new Line3.Segment(float3_1, float3_2), 0.5f, rigid, curve, ref this.m_Heights2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCurve(new Line3.Segment(float3_2, float3_3), 0.5f, rigid, curve, ref this.m_Heights3);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCurve(new Line3.Segment(float3_3, this.m_Quad.b), 1f, rigid, curve, ref this.m_Heights4);
        }

        private void CheckQuad(Quad3 quad, bool rigid, bool dontRaise, bool dontLower)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(quad.xz, this.m_Quad.xz))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_1 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.166666672f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_2 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.5f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_3 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.8333333f);
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_Radius != 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_1 = this.m_Position + (float3_1 - this.m_Position) * 1.22474492f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_2 = this.m_Position + (float3_2 - this.m_Position) * 1.41421354f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_3 = this.m_Position + (float3_3 - this.m_Position) * 1.22474492f;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckQuad(new Line3.Segment(this.m_Quad.a, float3_1), 0.0f, rigid, dontRaise, dontLower, quad, ref this.m_Heights1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckQuad(new Line3.Segment(float3_1, float3_2), 0.5f, rigid, dontRaise, dontLower, quad, ref this.m_Heights2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckQuad(new Line3.Segment(float3_2, float3_3), 0.5f, rigid, dontRaise, dontLower, quad, ref this.m_Heights3);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckQuad(new Line3.Segment(float3_3, this.m_Quad.b), 1f, rigid, dontRaise, dontLower, quad, ref this.m_Heights4);
        }

        private void CheckCircle(
          float3 position,
          float radius,
          bool rigid,
          bool dontRaise,
          bool dontLower)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_1 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.166666672f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_2 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.5f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3_3 = math.lerp(this.m_Quad.a, this.m_Quad.b, 0.8333333f);
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_Radius != 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_1 = this.m_Position + (float3_1 - this.m_Position) * 1.22474492f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_2 = this.m_Position + (float3_2 - this.m_Position) * 1.41421354f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3_3 = this.m_Position + (float3_3 - this.m_Position) * 1.22474492f;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(new Line3.Segment(this.m_Quad.a, float3_1), position, radius, rigid, dontRaise, dontLower, ref this.m_Heights1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(new Line3.Segment(float3_1, float3_2), position, radius, rigid, dontRaise, dontLower, ref this.m_Heights2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(new Line3.Segment(float3_2, float3_3), position, radius, rigid, dontRaise, dontLower, ref this.m_Heights3);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(new Line3.Segment(float3_3, this.m_Quad.b), position, radius, rigid, dontRaise, dontLower, ref this.m_Heights4);
        }

        private void CheckCurve(
          Line3.Segment line,
          float pivotT,
          bool rigid,
          Bezier4x3 curve,
          ref LotHeightSystem.Heights heights)
        {
          Line3.Segment other;
          other.a = curve.a;
          for (int index = 1; index <= 16; ++index)
          {
            other.b = MathUtils.Position(curve, (float) index * (1f / 16f));
            // ISSUE: reference to a compiler-generated method
            this.CheckLine(line, pivotT, rigid, false, false, other, ref heights);
            other.a = other.b;
          }
        }

        private void CheckQuad(
          Line3.Segment line,
          float pivotT,
          bool rigid,
          bool dontRaise,
          bool dontLower,
          Quad3 quad,
          ref LotHeightSystem.Heights heights)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(line, pivotT, rigid, dontRaise, dontLower, quad.ab, ref heights);
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(line, pivotT, rigid, dontRaise, dontLower, quad.bc, ref heights);
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(line, pivotT, rigid, dontRaise, dontLower, quad.cd, ref heights);
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(line, pivotT, rigid, dontRaise, dontLower, quad.da, ref heights);
        }

        private void CheckLine(
          Line3.Segment line,
          float pivotT,
          bool rigid,
          bool dontRaise,
          bool dontLower,
          Line3.Segment other,
          ref LotHeightSystem.Heights heights)
        {
          float3 float3 = MathUtils.Position(line, pivotT);
          float t1;
          float x = MathUtils.Distance(line.xz, other.a.xz, out t1);
          float y = MathUtils.Distance(line.xz, other.b.xz, out t1);
          float num1 = math.min(MathUtils.Distance(other.xz, float3.xz, out t1), math.min(x, y));
          float num2 = MathUtils.Length(other.xz);
          float num3 = math.min(8f, num2 * 16f);
          if ((double) num1 >= (double) num3)
            return;
          float t2;
          double num4 = (double) MathUtils.Distance((Line2) other.xz, float3.xz, out t2);
          float num5 = math.max(0.0f, math.max(t2 - 1f, -t2)) * num2;
          // ISSUE: reference to a compiler-generated method
          this.AddHeight(MathUtils.Position(other.y, t2) - float3.y, (float) ((1.0 - (double) num1 / (double) num3) / (1.0 + (double) num5 / (double) num3)), rigid, dontRaise, dontLower, ref heights);
        }

        private void CheckCircle(
          Line3.Segment line,
          float3 position,
          float radius,
          bool rigid,
          bool dontRaise,
          bool dontLower,
          ref LotHeightSystem.Heights heights)
        {
          float t;
          float num = math.max(0.0f, MathUtils.Distance(line.xz, position.xz, out t) - radius);
          if ((double) num >= 8.0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.AddHeight(position.y - MathUtils.Position(line.y, t), (float) (1.0 - (double) num / 8.0), rigid, dontRaise, dontLower, ref heights);
        }

        private void AddHeight(
          float offset,
          float strength,
          bool rigid,
          bool dontRaise,
          bool dontLower,
          ref LotHeightSystem.Heights heights)
        {
          if ((double) offset > 0.0 & dontRaise | (double) offset < 0.0 & dontLower)
            return;
          if (rigid)
          {
            // ISSUE: reference to a compiler-generated field
            heights.m_RigidBounds |= (float) ((double) offset * (double) strength * 2.0);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            heights.m_RigidStrength = math.max(heights.m_RigidStrength, strength);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            heights.m_FlexibleBounds |= offset * strength;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            heights.m_FlexibleStrength = math.max(heights.m_FlexibleStrength, strength);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentLookup<Lot> __Game_Buildings_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      public ComponentLookup<Lot> __Game_Buildings_Lot_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Lot_RO_ComponentLookup = state.GetComponentLookup<Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup = state.GetComponentLookup<BuildingTerraformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Lot_RW_ComponentLookup = state.GetComponentLookup<Lot>();
      }
    }
  }
}
