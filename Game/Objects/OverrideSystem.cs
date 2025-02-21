// Decompiled with JetBrains decompiler
// Type: Game.Objects.OverrideSystem
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
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class OverrideSystem : GameSystemBase
  {
    private UpdateCollectSystem m_ObjectUpdateCollectSystem;
    private Game.Net.UpdateCollectSystem m_NetUpdateCollectSystem;
    private Game.Areas.UpdateCollectSystem m_AreaUpdateCollectSystem;
    private SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private ToolSystem m_ToolSystem;
    private ComponentTypeSet m_OverriddenUpdatedSet;
    private OverrideSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectUpdateCollectSystem = this.World.GetOrCreateSystemManaged<UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Net.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Areas.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_OverriddenUpdatedSet = new ComponentTypeSet(ComponentType.ReadWrite<Overridden>(), ComponentType.ReadWrite<Updated>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ObjectUpdateCollectSystem.isUpdated && !this.m_NetUpdateCollectSystem.netsUpdated && !this.m_AreaUpdateCollectSystem.lotsUpdated)
        return;
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeHashSet<Entity> objectSet = new NativeHashSet<Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<OverrideSystem.TreeAction> nativeQueue1 = new NativeQueue<OverrideSystem.TreeAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<OverrideSystem.OverridableAction> nativeQueue2 = new NativeQueue<OverrideSystem.OverridableAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.CollectUpdatedObjects(nativeList, objectSet));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Marker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      OverrideSystem.CheckObjectOverrideJob jobData1 = new OverrideSystem.CheckObjectOverrideJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CreatedData = this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup,
        m_OverriddenData = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_StackData = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup,
        m_MarkerData = this.__TypeHandle.__Game_Objects_Marker_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_ObjectArray = nativeList.AsDeferredJobArray(),
        m_ObjectSet = objectSet,
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies1),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies3),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_TreeActions = nativeQueue1.AsParallelWriter(),
        m_OverridableActions = nativeQueue2.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      OverrideSystem.UpdateObjectOverrideJob jobData2 = new OverrideSystem.UpdateObjectOverrideJob()
      {
        m_OverriddenUpdatedSet = this.m_OverriddenUpdatedSet,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(false, out dependencies4),
        m_Actions = nativeQueue1,
        m_OverridableActions = nativeQueue2,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      };
      JobHandle jobHandle1 = jobData1.Schedule<OverrideSystem.CheckObjectOverrideJob, Entity>(nativeList, 1, JobHandle.CombineDependencies(this.Dependency, JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3)));
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle1, dependencies4);
      JobHandle jobHandle2 = jobData2.Schedule<OverrideSystem.UpdateObjectOverrideJob>(dependsOn);
      nativeList.Dispose(jobHandle1);
      objectSet.Dispose(jobHandle1);
      nativeQueue1.Dispose(jobHandle2);
      nativeQueue2.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = jobHandle2;
    }

    private JobHandle CollectUpdatedObjects(
      NativeList<Entity> updateObjectsList,
      NativeHashSet<Entity> objectSet)
    {
      NativeQueue<Entity> nativeQueue1 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue2 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue3 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> staticSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies1);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: variable of a compiler-generated type
      OverrideSystem.FindUpdatedObjectsJob jobData;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ObjectUpdateCollectSystem.isUpdated)
      {
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedBounds = this.m_ObjectUpdateCollectSystem.GetUpdatedBounds(out dependencies2);
        // ISSUE: object of a compiler-generated type is created
        jobData = new OverrideSystem.FindUpdatedObjectsJob();
        // ISSUE: reference to a compiler-generated field
        jobData.m_Bounds = updatedBounds.AsDeferredJobArray();
        // ISSUE: reference to a compiler-generated field
        jobData.m_SearchTree = staticSearchTree;
        // ISSUE: reference to a compiler-generated field
        jobData.m_ResultQueue = nativeQueue1.AsParallelWriter();
        JobHandle jobHandle2 = jobData.Schedule<OverrideSystem.FindUpdatedObjectsJob, Bounds2>(updatedBounds, 1, JobHandle.CombineDependencies(dependencies2, dependencies1));
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
        // ISSUE: object of a compiler-generated type is created
        jobData = new OverrideSystem.FindUpdatedObjectsJob();
        // ISSUE: reference to a compiler-generated field
        jobData.m_Bounds = updatedNetBounds.AsDeferredJobArray();
        // ISSUE: reference to a compiler-generated field
        jobData.m_SearchTree = staticSearchTree;
        // ISSUE: reference to a compiler-generated field
        jobData.m_ResultQueue = nativeQueue2.AsParallelWriter();
        JobHandle jobHandle3 = jobData.Schedule<OverrideSystem.FindUpdatedObjectsJob, Bounds2>(updatedNetBounds, 1, JobHandle.CombineDependencies(dependencies3, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetUpdateCollectSystem.AddNetBoundsReader(jobHandle3);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle3);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.lotsUpdated)
      {
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedLotBounds = this.m_AreaUpdateCollectSystem.GetUpdatedLotBounds(out dependencies4);
        // ISSUE: object of a compiler-generated type is created
        jobData = new OverrideSystem.FindUpdatedObjectsJob();
        // ISSUE: reference to a compiler-generated field
        jobData.m_Bounds = updatedLotBounds.AsDeferredJobArray();
        // ISSUE: reference to a compiler-generated field
        jobData.m_SearchTree = staticSearchTree;
        // ISSUE: reference to a compiler-generated field
        jobData.m_ResultQueue = nativeQueue3.AsParallelWriter();
        JobHandle jobHandle4 = jobData.Schedule<OverrideSystem.FindUpdatedObjectsJob, Bounds2>(updatedLotBounds, 1, JobHandle.CombineDependencies(dependencies4, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddLotBoundsReader(jobHandle4);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle4);
      }
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps = new OverrideSystem.CollectObjectsJob()
      {
        m_Queue1 = nativeQueue1,
        m_Queue2 = nativeQueue2,
        m_Queue3 = nativeQueue3,
        m_ResultList = updateObjectsList,
        m_ObjectSet = objectSet
      }.Schedule<OverrideSystem.CollectObjectsJob>(jobHandle1);
      nativeQueue1.Dispose(inputDeps);
      nativeQueue2.Dispose(inputDeps);
      nativeQueue3.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle1);
      return inputDeps;
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
    public OverrideSystem()
    {
    }

    private struct TreeAction
    {
      public Entity m_Entity;
      public BoundsMask m_Mask;
    }

    private struct OverridableAction : IComparable<OverrideSystem.OverridableAction>
    {
      public Entity m_Entity;
      public Entity m_Other;
      public BoundsMask m_Mask;
      public sbyte m_Priority;
      public bool m_OtherOverridden;

      public int CompareTo(OverrideSystem.OverridableAction other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(this.m_Entity.Index - other.m_Entity.Index, (int) this.m_Priority - (int) other.m_Priority, (int) this.m_Priority != (int) other.m_Priority);
      }
    }

    [BurstCompile]
    private struct UpdateObjectOverrideJob : IJob
    {
      [ReadOnly]
      public ComponentTypeSet m_OverriddenUpdatedSet;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      public NativeQueue<OverrideSystem.TreeAction> m_Actions;
      public NativeQueue<OverrideSystem.OverridableAction> m_OverridableActions;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<OverrideSystem.OverridableAction> array = this.m_OverridableActions.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        if (array.Length != 0)
        {
          array.Sort<OverrideSystem.OverridableAction>();
          NativeHashMap<Entity, bool> overridden = new NativeHashMap<Entity, bool>(array.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: variable of a compiler-generated type
          OverrideSystem.TreeAction treeAction;
          // ISSUE: reference to a compiler-generated field
          while (this.m_Actions.TryDequeue(out treeAction))
          {
            QuadTreeBoundsXZ bounds;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectSearchTree.TryGet(treeAction.m_Entity, out bounds))
            {
              // ISSUE: reference to a compiler-generated field
              bounds.m_Mask = bounds.m_Mask & ~(BoundsMask.AllLayers | BoundsMask.NotOverridden) | treeAction.m_Mask;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ObjectSearchTree.Update(treeAction.m_Entity, bounds);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            overridden.Add(treeAction.m_Entity, treeAction.m_Mask == (BoundsMask) 0);
          }
          for (int index = 0; index < array.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            OverrideSystem.OverridableAction overridableAction = array[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            overridden[overridableAction.m_Entity] = overridableAction.m_Other != Entity.Null;
          }
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          OverrideSystem.OverridableAction action1 = new OverrideSystem.OverridableAction();
          bool collision = false;
          for (int index = 0; index < array.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            OverrideSystem.OverridableAction overridableAction = array[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (overridableAction.m_Entity != action1.m_Entity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (action1.m_Entity != Entity.Null && action1.m_Other != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                overridden[action1.m_Entity] = collision;
                // ISSUE: reference to a compiler-generated method
                this.UpdateObject(action1, overridden, collision);
              }
              action1 = overridableAction;
              collision = false;
            }
            // ISSUE: reference to a compiler-generated field
            if (!collision && overridableAction.m_Other != Entity.Null)
            {
              bool flag;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              collision = !overridden.TryGetValue(overridableAction.m_Other, out flag) ? !overridableAction.m_OtherOverridden : !flag;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (action1.m_Entity != Entity.Null && action1.m_Other != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateObject(action1, overridden, collision);
          }
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          OverrideSystem.OverridableAction action2 = new OverrideSystem.OverridableAction();
          for (int index = 0; index < array.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            OverrideSystem.OverridableAction overridableAction = array[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (overridableAction.m_Entity != action2.m_Entity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (action2.m_Entity != Entity.Null && action2.m_Other == Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateObject(action2, overridden, overridden[action2.m_Entity]);
              }
              action2 = overridableAction;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (action2.m_Entity != Entity.Null && action2.m_Other == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateObject(action2, overridden, overridden[action2.m_Entity]);
          }
          overridden.Dispose();
        }
        else
        {
          // ISSUE: variable of a compiler-generated type
          OverrideSystem.TreeAction treeAction;
          // ISSUE: reference to a compiler-generated field
          while (this.m_Actions.TryDequeue(out treeAction))
          {
            QuadTreeBoundsXZ bounds;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectSearchTree.TryGet(treeAction.m_Entity, out bounds))
            {
              // ISSUE: reference to a compiler-generated field
              bounds.m_Mask = bounds.m_Mask & ~(BoundsMask.AllLayers | BoundsMask.NotOverridden) | treeAction.m_Mask;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ObjectSearchTree.Update(treeAction.m_Entity, bounds);
            }
          }
        }
      }

      private void UpdateObject(
        OverrideSystem.OverridableAction action,
        NativeHashMap<Entity, bool> overridden,
        bool collision)
      {
        // ISSUE: reference to a compiler-generated field
        bool flag = ((uint) action.m_Priority & 2U) > 0U;
        if (collision != flag)
        {
          if (collision)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(action.m_Entity, in this.m_OverriddenUpdatedSet);
            // ISSUE: reference to a compiler-generated field
            action.m_Mask = (BoundsMask) 0;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(action.m_Entity, new Updated());
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Overridden>(action.m_Entity);
          }
          QuadTreeBoundsXZ bounds;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectSearchTree.TryGet(action.m_Entity, out bounds))
          {
            // ISSUE: reference to a compiler-generated field
            bounds.m_Mask = bounds.m_Mask & ~(BoundsMask.AllLayers | BoundsMask.NotOverridden) | action.m_Mask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ObjectSearchTree.Update(action.m_Entity, bounds);
          }
        }
        DynamicBuffer<SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(action.m_Entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          if (overridden.ContainsKey(subObject))
            overridden[subObject] = collision;
        }
      }
    }

    [BurstCompile]
    private struct FindUpdatedObjectsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.FindUpdatedObjectsJob.Iterator iterator = new OverrideSystem.FindUpdatedObjectsJob.Iterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_ResultQueue = this.m_ResultQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<OverrideSystem.FindUpdatedObjectsJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultQueue.Enqueue(objectEntity);
        }
      }
    }

    [BurstCompile]
    private struct CollectObjectsJob : IJob
    {
      public NativeQueue<Entity> m_Queue1;
      public NativeQueue<Entity> m_Queue2;
      public NativeQueue<Entity> m_Queue3;
      public NativeList<Entity> m_ResultList;
      public NativeHashSet<Entity> m_ObjectSet;

      public void Execute()
      {
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue1.TryDequeue(out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectSet.Add(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResultList.Add(in entity1);
          }
        }
        Entity entity2;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue2.TryDequeue(out entity2))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectSet.Add(entity2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResultList.Add(in entity2);
          }
        }
        Entity entity3;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue3.TryDequeue(out entity3))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectSet.Add(entity3))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResultList.Add(in entity3);
          }
        }
      }
    }

    [BurstCompile]
    private struct CheckObjectOverrideJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Created> m_CreatedData;
      [ReadOnly]
      public ComponentLookup<Overridden> m_OverriddenData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Stack> m_StackData;
      [ReadOnly]
      public ComponentLookup<Marker> m_MarkerData;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeArray<Entity> m_ObjectArray;
      [ReadOnly]
      public NativeHashSet<Entity> m_ObjectSet;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<OverrideSystem.TreeAction>.ParallelWriter m_TreeActions;
      public NativeQueue<OverrideSystem.OverridableAction>.ParallelWriter m_OverridableActions;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_ObjectArray[index];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity1];
        ObjectGeometryData componentData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData1) || (componentData1.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) != GeometryFlags.Overridable)
          return;
        NativeList<Entity> overridableCollisions = new NativeList<Entity>();
        Entity entity2 = entity1;
        bool collision = false;
        Owner componentData2;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity2, out componentData2))
        {
          entity2 = componentData2.m_Owner;
          ObjectGeometryData componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabObjectGeometryData.TryGetComponent(this.m_PrefabRefData[componentData2.m_Owner].m_Prefab, out componentData3) && (componentData3.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectSet.Contains(entity2))
              return;
            // ISSUE: reference to a compiler-generated field
            collision |= this.m_OverriddenData.HasComponent(entity2);
          }
          else
            break;
        }
        // ISSUE: reference to a compiler-generated method
        this.CheckObject(index, entity1, prefabRef, componentData1, collision, false, ref overridableCollisions);
        if (!overridableCollisions.IsCreated)
          return;
        overridableCollisions.Dispose();
      }

      private void CheckObject(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        ObjectGeometryData prefabGeometryData,
        bool collision,
        bool delayedResolve,
        ref NativeList<Entity> overridableCollisions)
      {
        if (!collision)
        {
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TransformData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool ignoreMarkers = !this.m_EditorMode || this.m_OwnerData.HasComponent(entity);
          StackData componentData1 = new StackData();
          Stack componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Bounds3 bounds3 = !this.m_StackData.TryGetComponent(entity, out componentData2) || !this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData1) ? ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, prefabGeometryData) : ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData2, prefabGeometryData, componentData1);
          Elevation componentData3;
          // ISSUE: reference to a compiler-generated field
          CollisionMask collisionMask = !this.m_ElevationData.TryGetComponent(entity, out componentData3) ? ObjectUtils.GetCollisionMask(prefabGeometryData, ignoreMarkers) : ObjectUtils.GetCollisionMask(prefabGeometryData, componentData3, ignoreMarkers);
          Entity entity1 = entity;
          Entity parent = Entity.Null;
          Owner componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.TryGetComponent(entity1, out componentData4) && !this.m_BuildingData.HasComponent(entity1))
          {
            entity1 = componentData4.m_Owner;
            Attached componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AttachedData.TryGetComponent(componentData4.m_Owner, out componentData5))
              parent = componentData5.m_Parent;
          }
          if (overridableCollisions.IsCreated)
            overridableCollisions.Clear();
          // ISSUE: reference to a compiler-generated field
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
          OverrideSystem.CheckObjectOverrideJob.ObjectIterator iterator1 = new OverrideSystem.CheckObjectOverrideJob.ObjectIterator()
          {
            m_TopLevelEntity = entity1,
            m_ObjectEntity = entity,
            m_ObjectBounds = bounds3,
            m_CollisionMask = collisionMask,
            m_ObjectTransform = transform,
            m_ObjectStack = componentData2,
            m_PrefabGeometryData = prefabGeometryData,
            m_ObjectStackData = componentData1,
            m_OwnerData = this.m_OwnerData,
            m_TransformData = this.m_TransformData,
            m_ElevationData = this.m_ElevationData,
            m_AttachmentData = this.m_AttachmentData,
            m_StackData = this.m_StackData,
            m_BuildingData = this.m_BuildingData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
            m_PrefabStackData = this.m_PrefabStackData,
            m_OverridableCollisions = overridableCollisions,
            m_EditorMode = this.m_EditorMode
          };
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
          OverrideSystem.CheckObjectOverrideJob.NetIterator iterator2 = new OverrideSystem.CheckObjectOverrideJob.NetIterator()
          {
            m_TopLevelEntity = entity1,
            m_AttachedParent = parent,
            m_ObjectEntity = entity,
            m_ObjectBounds = bounds3,
            m_CollisionMask = collisionMask,
            m_ObjectTransform = transform,
            m_ObjectStack = componentData2,
            m_PrefabGeometryData = prefabGeometryData,
            m_ObjectStackData = componentData1,
            m_OwnerData = this.m_OwnerData,
            m_BuildingData = this.m_BuildingData,
            m_EdgeData = this.m_EdgeData,
            m_EdgeGeometryData = this.m_EdgeGeometryData,
            m_StartNodeGeometryData = this.m_StartNodeGeometryData,
            m_EndNodeGeometryData = this.m_EndNodeGeometryData,
            m_CompositionData = this.m_CompositionData,
            m_PrefabCompositionData = this.m_PrefabCompositionData,
            m_EditorMode = this.m_EditorMode
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          OverrideSystem.CheckObjectOverrideJob.AreaIterator iterator3 = new OverrideSystem.CheckObjectOverrideJob.AreaIterator()
          {
            m_TopLevelEntity = entity1,
            m_ObjectEntity = entity,
            m_ObjectBounds = bounds3,
            m_CollisionMask = collisionMask,
            m_ObjectTransform = transform,
            m_PrefabGeometryData = prefabGeometryData,
            m_OwnerData = this.m_OwnerData,
            m_BuildingData = this.m_BuildingData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabAreaGeometryData = this.m_PrefabAreaGeometryData,
            m_AreaNodes = this.m_AreaNodes,
            m_AreaTriangles = this.m_AreaTriangles
          };
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedEdges.HasBuffer(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator1.m_TopLevelEdges = this.m_ConnectedEdges[entity1];
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedNodes.HasBuffer(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator1.m_TopLevelNodes = this.m_ConnectedNodes[entity1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator1.m_TopLevelEdge = this.m_EdgeData[entity1];
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectSearchTree.Iterate<OverrideSystem.CheckObjectOverrideJob.ObjectIterator>(ref iterator1);
          // ISSUE: reference to a compiler-generated field
          if (!iterator1.m_CollisionFound)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_NetSearchTree.Iterate<OverrideSystem.CheckObjectOverrideJob.NetIterator>(ref iterator2);
            // ISSUE: reference to a compiler-generated field
            if (!iterator2.m_CollisionFound)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_AreaSearchTree.Iterate<OverrideSystem.CheckObjectOverrideJob.AreaIterator>(ref iterator3);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision = iterator1.m_CollisionFound || iterator2.m_CollisionFound || iterator3.m_CollisionFound;
          // ISSUE: reference to a compiler-generated field
          overridableCollisions = iterator1.m_OverridableCollisions;
          if (!collision)
          {
            if (overridableCollisions.IsCreated && overridableCollisions.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              OverrideSystem.OverridableAction overridableAction = new OverrideSystem.OverridableAction()
              {
                m_Entity = entity,
                m_Mask = this.GetBoundsMask(entity, prefabGeometryData)
              };
              // ISSUE: reference to a compiler-generated field
              if (this.m_CreatedData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                overridableAction.m_Priority |= (sbyte) 1;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_OverriddenData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                overridableAction.m_Priority |= (sbyte) 2;
              }
              // ISSUE: reference to a compiler-generated field
              for (int index = 0; index < iterator1.m_OverridableCollisions.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                overridableAction.m_Other = iterator1.m_OverridableCollisions[index];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                overridableAction.m_OtherOverridden = this.m_OverriddenData.HasComponent(overridableAction.m_Other);
                // ISSUE: reference to a compiler-generated field
                this.m_OverridableActions.Enqueue(overridableAction);
              }
              delayedResolve = true;
            }
            else if (delayedResolve)
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              OverrideSystem.OverridableAction overridableAction = new OverrideSystem.OverridableAction()
              {
                m_Entity = entity,
                m_Mask = this.GetBoundsMask(entity, prefabGeometryData)
              };
              // ISSUE: reference to a compiler-generated field
              if (this.m_OverriddenData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                overridableAction.m_Priority |= (sbyte) 2;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_OverridableActions.Enqueue(overridableAction);
            }
          }
        }
        if (!collision & delayedResolve)
        {
          DynamicBuffer<SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
            return;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity subObject = bufferData[index].m_SubObject;
            ObjectGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.TryGetComponent(this.m_PrefabRefData[subObject].m_Prefab, out componentData) && (componentData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckObject(jobIndex, subObject, prefabRef, componentData, false, true, ref overridableCollisions);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          bool flag = collision != this.m_OverriddenData.HasComponent(entity);
          if (flag)
          {
            if (collision)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Overridden>(jobIndex, entity, new Overridden());
              // ISSUE: reference to a compiler-generated method
              this.AddTreeAction(entity, prefabGeometryData, true);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Overridden>(jobIndex, entity);
              // ISSUE: reference to a compiler-generated method
              this.AddTreeAction(entity, prefabGeometryData, false);
            }
          }
          DynamicBuffer<SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
            return;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity subObject = bufferData[index].m_SubObject;
            ObjectGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.TryGetComponent(this.m_PrefabRefData[subObject].m_Prefab, out componentData) && (componentData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable && (flag || !collision && this.m_ObjectSet.Contains(subObject)))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckObject(jobIndex, subObject, prefabRef, componentData, collision, delayedResolve, ref overridableCollisions);
            }
          }
        }
      }

      private void AddTreeAction(
        Entity entity,
        ObjectGeometryData prefabObjectGeometryData,
        bool isOverridden)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.TreeAction treeAction = new OverrideSystem.TreeAction()
        {
          m_Entity = entity
        };
        if (!isOverridden)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          treeAction.m_Mask = this.GetBoundsMask(entity, prefabObjectGeometryData);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_TreeActions.Enqueue(treeAction);
      }

      private BoundsMask GetBoundsMask(Entity entity, ObjectGeometryData prefabObjectGeometryData)
      {
        BoundsMask boundsMask = BoundsMask.NotOverridden;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode || !this.m_MarkerData.HasComponent(entity) || this.m_OutsideConnectionData.HasComponent(entity))
        {
          MeshLayer layers = prefabObjectGeometryData.m_Layers;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_OwnerData.TryGetComponent(entity, out componentData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          boundsMask |= CommonUtils.GetBoundsMask(Game.Net.SearchSystem.GetLayers(componentData, new Game.Net.UtilityLane(), layers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
        }
        return boundsMask;
      }

      private struct ObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_TopLevelEntity;
        public Entity m_ObjectEntity;
        public Bounds3 m_ObjectBounds;
        public Transform m_ObjectTransform;
        public Stack m_ObjectStack;
        public CollisionMask m_CollisionMask;
        public ObjectGeometryData m_PrefabGeometryData;
        public StackData m_ObjectStackData;
        public DynamicBuffer<ConnectedEdge> m_TopLevelEdges;
        public DynamicBuffer<ConnectedNode> m_TopLevelNodes;
        public Edge m_TopLevelEdge;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<Elevation> m_ElevationData;
        public ComponentLookup<Attachment> m_AttachmentData;
        public ComponentLookup<Stack> m_StackData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
        public ComponentLookup<StackData> m_PrefabStackData;
        public NativeList<Entity> m_OverridableCollisions;
        public bool m_CollisionFound;
        public bool m_EditorMode;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CollisionFound)
            return false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return (this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0 ? MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz) : MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CollisionFound)
            return;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz))
              return;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds))
              return;
          }
          Entity entity = objectEntity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.HasComponent(entity) && !this.m_BuildingData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            entity = this.m_OwnerData[entity].m_Owner;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_TopLevelEntity == entity || this.m_AttachmentData.HasComponent(entity) && this.m_AttachmentData[entity].m_Attached == this.m_TopLevelEntity)
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TopLevelEdges.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_TopLevelEdges.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TopLevelEdges[index].m_Edge == entity)
                return;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TopLevelNodes.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              for (int index = 0; index < this.m_TopLevelNodes.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TopLevelNodes[index].m_Node == entity)
                  return;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TopLevelEdge.m_Start == entity || this.m_TopLevelEdge.m_End == entity)
                return;
            }
          }
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[objectEntity];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
            return;
          bool overridableCollision = false;
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData geometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
          if ((geometryData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable)
            overridableCollision = true;
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TransformData[objectEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool ignoreMarkers = !this.m_EditorMode || this.m_OwnerData.HasComponent(objectEntity);
          Elevation componentData1;
          // ISSUE: reference to a compiler-generated field
          CollisionMask mask2 = !this.m_ElevationData.TryGetComponent(objectEntity, out componentData1) ? ObjectUtils.GetCollisionMask(geometryData, ignoreMarkers) : ObjectUtils.GetCollisionMask(geometryData, componentData1, ignoreMarkers);
          // ISSUE: reference to a compiler-generated field
          if ((this.m_CollisionMask & mask2) == (CollisionMask) 0)
            return;
          float3 float3_1 = MathUtils.Center(bounds.m_Bounds);
          float3 pos = new float3();
          bool flag = false;
          StackData componentData2 = new StackData();
          Stack componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StackData.TryGetComponent(objectEntity, out componentData3))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData2);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (((this.m_CollisionMask & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds)) && !flag)
          {
            // ISSUE: reference to a compiler-generated field
            quaternion q1 = math.inverse(this.m_ObjectTransform.m_Rotation);
            quaternion q2 = math.inverse(transform.m_Rotation);
            // ISSUE: reference to a compiler-generated field
            float3 v = this.m_ObjectTransform.m_Position - float3_1;
            float3 float3_2 = math.mul(q1, v);
            float3 float3_3 = math.mul(q2, transform.m_Position - float3_1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Bounds3 bounds1 = ObjectUtils.GetBounds(this.m_ObjectStack, this.m_PrefabGeometryData, this.m_ObjectStackData);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
              bounds1.min.y = math.max(bounds1.min.y, 0.0f);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_PrefabGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
              {
                Cylinder3 cylinder3 = new Cylinder3();
                // ISSUE: reference to a compiler-generated field
                cylinder3.circle = new Circle2((float) ((double) this.m_PrefabGeometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_2.xz);
                // ISSUE: reference to a compiler-generated field
                cylinder3.height = new Bounds1(bounds1.min.y + 0.01f, this.m_PrefabGeometryData.m_LegSize.y + 0.01f) + float3_2.y;
                // ISSUE: reference to a compiler-generated field
                cylinder3.rotation = this.m_ObjectTransform.m_Rotation;
                Bounds3 bounds2 = ObjectUtils.GetBounds(componentData3, geometryData, componentData2);
                if ((geometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
                  bounds2.min.y = math.max(bounds2.min.y, 0.0f);
                if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
                {
                  if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                  {
                    if (ValidationHelpers.Intersect(cylinder3, new Cylinder3()
                    {
                      circle = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                      height = new Bounds1(bounds2.min.y + 0.01f, geometryData.m_LegSize.y + 0.01f) + float3_3.y,
                      rotation = transform.m_Rotation
                    }, ref pos))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.AddCollision(overridableCollision, objectEntity);
                      return;
                    }
                  }
                  else if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
                  {
                    Box3 box = new Box3();
                    box.bounds.min.y = bounds2.min.y + 0.01f;
                    box.bounds.min.xz = geometryData.m_LegSize.xz * -0.5f + 0.01f;
                    box.bounds.max.y = geometryData.m_LegSize.y + 0.01f;
                    box.bounds.max.xz = geometryData.m_LegSize.xz * 0.5f - 0.01f;
                    box.bounds += float3_3;
                    box.rotation = transform.m_Rotation;
                    if (MathUtils.Intersect(cylinder3, box, out Bounds3 _, out Bounds3 _))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.AddCollision(overridableCollision, objectEntity);
                      return;
                    }
                  }
                  bounds2.min.y = geometryData.m_LegSize.y;
                }
                if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
                {
                  if (ValidationHelpers.Intersect(cylinder3, new Cylinder3()
                  {
                    circle = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                    height = new Bounds1(bounds2.min.y + 0.01f, bounds2.max.y - 0.01f) + float3_3.y,
                    rotation = transform.m_Rotation
                  }, ref pos))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddCollision(overridableCollision, objectEntity);
                    return;
                  }
                }
                else
                {
                  Box3 box = new Box3()
                  {
                    bounds = bounds2 + float3_3
                  };
                  box.bounds = MathUtils.Expand(box.bounds, (float3) -0.01f);
                  box.rotation = transform.m_Rotation;
                  if (MathUtils.Intersect(cylinder3, box, out Bounds3 _, out Bounds3 _))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddCollision(overridableCollision, objectEntity);
                    return;
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
                {
                  Box3 box3 = new Box3();
                  box3.bounds.min.y = bounds1.min.y + 0.01f;
                  // ISSUE: reference to a compiler-generated field
                  box3.bounds.min.xz = this.m_PrefabGeometryData.m_LegSize.xz * -0.5f + 0.01f;
                  // ISSUE: reference to a compiler-generated field
                  box3.bounds.max.y = this.m_PrefabGeometryData.m_LegSize.y + 0.01f;
                  // ISSUE: reference to a compiler-generated field
                  box3.bounds.max.xz = this.m_PrefabGeometryData.m_LegSize.xz * 0.5f - 0.01f;
                  box3.bounds += float3_2;
                  // ISSUE: reference to a compiler-generated field
                  box3.rotation = this.m_ObjectTransform.m_Rotation;
                  Bounds3 bounds3 = ObjectUtils.GetBounds(componentData3, geometryData, componentData2);
                  if ((geometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
                    bounds3.min.y = math.max(bounds3.min.y, 0.0f);
                  if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
                  {
                    if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                    {
                      if (MathUtils.Intersect(new Cylinder3()
                      {
                        circle = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                        height = new Bounds1(bounds3.min.y + 0.01f, geometryData.m_LegSize.y + 0.01f) + float3_3.y,
                        rotation = transform.m_Rotation
                      }, box3, out Bounds3 _, out Bounds3 _))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.AddCollision(overridableCollision, objectEntity);
                        return;
                      }
                    }
                    else if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
                    {
                      Box3 box2 = new Box3();
                      box2.bounds.min.y = bounds3.min.y + 0.01f;
                      box2.bounds.min.xz = geometryData.m_LegSize.xz * -0.5f + 0.01f;
                      box2.bounds.max.y = geometryData.m_LegSize.y + 0.01f;
                      box2.bounds.max.xz = geometryData.m_LegSize.xz * 0.5f - 0.01f;
                      box2.bounds += float3_3;
                      box2.rotation = transform.m_Rotation;
                      if (MathUtils.Intersect(box3, box2, out Bounds3 _, out Bounds3 _))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.AddCollision(overridableCollision, objectEntity);
                        return;
                      }
                    }
                    bounds3.min.y = geometryData.m_LegSize.y;
                  }
                  if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
                  {
                    if (MathUtils.Intersect(new Cylinder3()
                    {
                      circle = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                      height = new Bounds1(bounds3.min.y + 0.01f, bounds3.max.y - 0.01f) + float3_3.y,
                      rotation = transform.m_Rotation
                    }, box3, out Bounds3 _, out Bounds3 _))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.AddCollision(overridableCollision, objectEntity);
                      return;
                    }
                  }
                  else
                  {
                    Box3 box2 = new Box3()
                    {
                      bounds = bounds3 + float3_3
                    };
                    box2.bounds = MathUtils.Expand(box2.bounds, (float3) -0.01f);
                    box2.rotation = transform.m_Rotation;
                    if (MathUtils.Intersect(box3, box2, out Bounds3 _, out Bounds3 _))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.AddCollision(overridableCollision, objectEntity);
                      return;
                    }
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              bounds1.min.y = this.m_PrefabGeometryData.m_LegSize.y;
            }
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              Cylinder3 cylinder3 = new Cylinder3();
              // ISSUE: reference to a compiler-generated field
              cylinder3.circle = new Circle2((float) ((double) this.m_PrefabGeometryData.m_Size.x * 0.5 - 0.0099999997764825821), float3_2.xz);
              cylinder3.height = new Bounds1(bounds1.min.y + 0.01f, bounds1.max.y - 0.01f) + float3_2.y;
              // ISSUE: reference to a compiler-generated field
              cylinder3.rotation = this.m_ObjectTransform.m_Rotation;
              Bounds3 bounds4 = ObjectUtils.GetBounds(componentData3, geometryData, componentData2);
              if ((geometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
                bounds4.min.y = math.max(bounds4.min.y, 0.0f);
              if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
              {
                if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                {
                  if (ValidationHelpers.Intersect(cylinder3, new Cylinder3()
                  {
                    circle = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                    height = new Bounds1(bounds4.min.y + 0.01f, geometryData.m_LegSize.y + 0.01f) + float3_3.y,
                    rotation = transform.m_Rotation
                  }, ref pos))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddCollision(overridableCollision, objectEntity);
                    return;
                  }
                }
                else if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
                {
                  Box3 box = new Box3();
                  box.bounds.min.y = bounds4.min.y + 0.01f;
                  box.bounds.min.xz = geometryData.m_LegSize.xz * -0.5f + 0.01f;
                  box.bounds.max.y = geometryData.m_LegSize.y + 0.01f;
                  box.bounds.max.xz = geometryData.m_LegSize.xz * 0.5f - 0.01f;
                  box.bounds += float3_3;
                  box.rotation = transform.m_Rotation;
                  if (MathUtils.Intersect(cylinder3, box, out Bounds3 _, out Bounds3 _))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddCollision(overridableCollision, objectEntity);
                    return;
                  }
                }
                bounds4.min.y = geometryData.m_LegSize.y;
              }
              if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
              {
                if (ValidationHelpers.Intersect(cylinder3, new Cylinder3()
                {
                  circle = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                  height = new Bounds1(bounds4.min.y + 0.01f, bounds4.max.y - 0.01f) + float3_3.y,
                  rotation = transform.m_Rotation
                }, ref pos))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                  return;
                }
              }
              else
              {
                Box3 box = new Box3()
                {
                  bounds = bounds4 + float3_3
                };
                box.bounds = MathUtils.Expand(box.bounds, (float3) -0.01f);
                box.rotation = transform.m_Rotation;
                if (MathUtils.Intersect(cylinder3, box, out Bounds3 _, out Bounds3 _))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                  return;
                }
              }
            }
            else
            {
              Box3 box3 = new Box3()
              {
                bounds = bounds1 + float3_2
              };
              box3.bounds = MathUtils.Expand(box3.bounds, (float3) -0.01f);
              // ISSUE: reference to a compiler-generated field
              box3.rotation = this.m_ObjectTransform.m_Rotation;
              Bounds3 bounds5 = ObjectUtils.GetBounds(componentData3, geometryData, componentData2);
              if ((geometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
                bounds5.min.y = math.max(bounds5.min.y, 0.0f);
              if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
              {
                if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                {
                  if (MathUtils.Intersect(new Cylinder3()
                  {
                    circle = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                    height = new Bounds1(bounds5.min.y + 0.01f, geometryData.m_LegSize.y + 0.01f) + float3_3.y,
                    rotation = transform.m_Rotation
                  }, box3, out Bounds3 _, out Bounds3 _))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddCollision(overridableCollision, objectEntity);
                    return;
                  }
                }
                else if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
                {
                  Box3 box2 = new Box3();
                  box2.bounds.min.y = bounds5.min.y + 0.01f;
                  box2.bounds.min.xz = geometryData.m_LegSize.xz * -0.5f + 0.01f;
                  box2.bounds.max.y = geometryData.m_LegSize.y + 0.01f;
                  box2.bounds.max.xz = geometryData.m_LegSize.xz * 0.5f - 0.01f;
                  box2.bounds += float3_3;
                  box2.rotation = transform.m_Rotation;
                  if (MathUtils.Intersect(box3, box2, out Bounds3 _, out Bounds3 _))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddCollision(overridableCollision, objectEntity);
                    return;
                  }
                }
                bounds5.min.y = geometryData.m_LegSize.y;
              }
              if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
              {
                if (MathUtils.Intersect(new Cylinder3()
                {
                  circle = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), float3_3.xz),
                  height = new Bounds1(bounds5.min.y + 0.01f, bounds5.max.y - 0.01f) + float3_3.y,
                  rotation = transform.m_Rotation
                }, box3, out Bounds3 _, out Bounds3 _))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                  return;
                }
              }
              else
              {
                Box3 box2 = new Box3()
                {
                  bounds = bounds5 + float3_3
                };
                box2.bounds = MathUtils.Expand(box2.bounds, (float3) -0.01f);
                box2.rotation = transform.m_Rotation;
                if (MathUtils.Intersect(box3, box2, out Bounds3 _, out Bounds3 _))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                  return;
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, mask2))
            return;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Circle2 circle2_1 = new Circle2((float) ((double) this.m_PrefabGeometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), (this.m_ObjectTransform.m_Position - float3_1).xz);
              if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
              {
                if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                {
                  Circle2 circle2_2 = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                  if (!MathUtils.Intersect(circle2_1, circle2_2))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
                else
                {
                  if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                    return;
                  if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(new Bounds3()
                  {
                    min = {
                      xz = geometryData.m_LegSize.xz * -0.5f
                    },
                    max = {
                      xz = geometryData.m_LegSize.xz * 0.5f
                    }
                  }, (float3) -0.01f)).xz, circle2_1, out Bounds2 _))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
              }
              else if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
              {
                Circle2 circle2_3 = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                if (!MathUtils.Intersect(circle2_1, circle2_3))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
              else
              {
                if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(geometryData.m_Bounds, (float3) -0.01f)).xz, circle2_1, out Bounds2 _))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                return;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Quad2 xz1 = ObjectUtils.CalculateBaseCorners(this.m_ObjectTransform.m_Position - float3_1, this.m_ObjectTransform.m_Rotation, MathUtils.Expand(new Bounds3()
              {
                min = {
                  xz = this.m_PrefabGeometryData.m_LegSize.xz * -0.5f
                },
                max = {
                  xz = this.m_PrefabGeometryData.m_LegSize.xz * 0.5f
                }
              }, (float3) -0.01f)).xz;
              if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
              {
                if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                {
                  Circle2 circle = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                  if (!MathUtils.Intersect(xz1, circle, out Bounds2 _))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
                else
                {
                  if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                    return;
                  Quad2 xz2 = ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(new Bounds3()
                  {
                    min = {
                      xz = geometryData.m_LegSize.xz * -0.5f
                    },
                    max = {
                      xz = geometryData.m_LegSize.xz * 0.5f
                    }
                  }, (float3) -0.01f)).xz;
                  if (!MathUtils.Intersect(xz1, xz2, out Bounds2 _))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
              }
              else if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
              {
                Circle2 circle = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                if (!MathUtils.Intersect(xz1, circle, out Bounds2 _))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
              else
              {
                Quad2 xz3 = ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(geometryData.m_Bounds, (float3) -0.01f)).xz;
                if (!MathUtils.Intersect(xz1, xz3, out Bounds2 _))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Circle2 circle2_4 = new Circle2((float) ((double) this.m_PrefabGeometryData.m_Size.x * 0.5 - 0.0099999997764825821), (this.m_ObjectTransform.m_Position - float3_1).xz);
              if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
              {
                if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                {
                  Circle2 circle2_5 = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                  if (!MathUtils.Intersect(circle2_4, circle2_5))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
                else
                {
                  if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                    return;
                  if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(new Bounds3()
                  {
                    min = {
                      xz = geometryData.m_LegSize.xz * -0.5f
                    },
                    max = {
                      xz = geometryData.m_LegSize.xz * 0.5f
                    }
                  }, (float3) -0.01f)).xz, circle2_4, out Bounds2 _))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
              }
              else if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
              {
                Circle2 circle2_6 = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                if (!MathUtils.Intersect(circle2_4, circle2_6))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
              else
              {
                if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(geometryData.m_Bounds, (float3) -0.01f)).xz, circle2_4, out Bounds2 _))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(this.m_ObjectTransform.m_Position - float3_1, this.m_ObjectTransform.m_Rotation, MathUtils.Expand(this.m_PrefabGeometryData.m_Bounds, (float3) -0.01f));
              Quad2 xz4 = baseCorners.xz;
              if ((geometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
              {
                if ((geometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
                {
                  Circle2 circle = new Circle2((float) ((double) geometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                  if (!MathUtils.Intersect(xz4, circle, out Bounds2 _))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
                else
                {
                  if ((geometryData.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                    return;
                  baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(new Bounds3()
                  {
                    min = {
                      xz = geometryData.m_LegSize.xz * -0.5f
                    },
                    max = {
                      xz = geometryData.m_LegSize.xz * 0.5f
                    }
                  }, (float3) -0.01f));
                  Quad2 xz5 = baseCorners.xz;
                  if (!MathUtils.Intersect(xz4, xz5, out Bounds2 _))
                    return;
                  // ISSUE: reference to a compiler-generated method
                  this.AddCollision(overridableCollision, objectEntity);
                }
              }
              else if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
              {
                Circle2 circle = new Circle2((float) ((double) geometryData.m_Size.x * 0.5 - 0.0099999997764825821), (transform.m_Position - float3_1).xz);
                if (!MathUtils.Intersect(xz4, circle, out Bounds2 _))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
              else
              {
                baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position - float3_1, transform.m_Rotation, MathUtils.Expand(geometryData.m_Bounds, (float3) -0.01f));
                Quad2 xz6 = baseCorners.xz;
                if (!MathUtils.Intersect(xz4, xz6, out Bounds2 _))
                  return;
                // ISSUE: reference to a compiler-generated method
                this.AddCollision(overridableCollision, objectEntity);
              }
            }
          }
        }

        private void AddCollision(bool overridableCollision, Entity other)
        {
          if (overridableCollision)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OverridableCollisions.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_OverridableCollisions = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_OverridableCollisions.Add(in other);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CollisionFound = true;
          }
        }
      }

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_TopLevelEntity;
        public Entity m_AttachedParent;
        public Entity m_ObjectEntity;
        public Bounds3 m_ObjectBounds;
        public CollisionMask m_CollisionMask;
        public Transform m_ObjectTransform;
        public Stack m_ObjectStack;
        public ObjectGeometryData m_PrefabGeometryData;
        public StackData m_ObjectStackData;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<Edge> m_EdgeData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
        public bool m_CollisionFound;
        public bool m_EditorMode;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CollisionFound)
            return false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return (this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0 ? MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz) : MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CollisionFound)
            return;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz))
              return;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds))
              return;
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EdgeGeometryData.HasComponent(edgeEntity))
            return;
          // ISSUE: reference to a compiler-generated field
          Edge edge1 = this.m_EdgeData[edgeEntity];
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_ObjectEntity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (edgeEntity == this.m_AttachedParent || edge1.m_Start == this.m_AttachedParent || edge1.m_End == this.m_AttachedParent)
            return;
          Owner componentData1;
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.TryGetComponent(entity1, out componentData1))
          {
            entity1 = componentData1.m_Owner;
            if (edgeEntity == entity1 || edge1.m_Start == entity1 || edge1.m_End == entity1)
              return;
          }
          Entity entity2 = edgeEntity;
          bool flag = false;
          Owner componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (; this.m_OwnerData.TryGetComponent(entity2, out componentData2) && !this.m_BuildingData.HasComponent(entity2); entity2 = componentData2.m_Owner)
            flag = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TopLevelEntity == entity2)
            return;
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionData[edgeEntity];
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry1 = this.m_EdgeGeometryData[edgeEntity];
          // ISSUE: reference to a compiler-generated field
          StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[edgeEntity];
          // ISSUE: reference to a compiler-generated field
          EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[edgeEntity];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[composition.m_Edge];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[composition.m_StartNode];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData3 = this.m_PrefabCompositionData[composition.m_EndNode];
          // ISSUE: reference to a compiler-generated field
          CollisionMask collisionMask1 = NetUtils.GetCollisionMask(netCompositionData1, !this.m_EditorMode | flag);
          // ISSUE: reference to a compiler-generated field
          CollisionMask collisionMask2 = NetUtils.GetCollisionMask(netCompositionData2, !this.m_EditorMode | flag);
          // ISSUE: reference to a compiler-generated field
          CollisionMask collisionMask3 = NetUtils.GetCollisionMask(netCompositionData3, !this.m_EditorMode | flag);
          CollisionMask mask2 = collisionMask1 | collisionMask2 | collisionMask3;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_CollisionMask & mask2) == (CollisionMask) 0)
            return;
          DynamicBuffer<NetCompositionArea> areas1_1 = new DynamicBuffer<NetCompositionArea>();
          DynamicBuffer<NetCompositionArea> areas1_2 = new DynamicBuffer<NetCompositionArea>();
          DynamicBuffer<NetCompositionArea> areas1_3 = new DynamicBuffer<NetCompositionArea>();
          float3 float3_1 = MathUtils.Center(bounds.m_Bounds);
          Bounds3 intersection1 = new Bounds3();
          Bounds2 intersection2 = new Bounds2();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_CollisionMask & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 float3_2 = math.mul(math.inverse(this.m_ObjectTransform.m_Rotation), this.m_ObjectTransform.m_Position - float3_1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Bounds3 bounds1 = ObjectUtils.GetBounds(this.m_ObjectStack, this.m_PrefabGeometryData, this.m_ObjectStackData);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
              bounds1.min.y = math.max(bounds1.min.y, 0.0f);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_PrefabGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
              {
                Cylinder3 cylinder2 = new Cylinder3();
                // ISSUE: reference to a compiler-generated field
                cylinder2.circle = new Circle2(this.m_PrefabGeometryData.m_LegSize.x * 0.5f, float3_2.xz);
                // ISSUE: reference to a compiler-generated field
                cylinder2.height = new Bounds1(bounds1.min.y, this.m_PrefabGeometryData.m_LegSize.y) + float3_2.y;
                // ISSUE: reference to a compiler-generated field
                cylinder2.rotation = this.m_ObjectTransform.m_Rotation;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((collisionMask1 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1, cylinder2, this.m_ObjectBounds, netCompositionData1, areas1_1, ref intersection1))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                  return;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((collisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1, cylinder2, this.m_ObjectBounds, netCompositionData2, areas1_2, ref intersection1))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                  return;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((collisionMask3 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1, cylinder2, this.m_ObjectBounds, netCompositionData3, areas1_3, ref intersection1))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                  return;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
                {
                  Box3 box2 = new Box3();
                  box2.bounds.min.y = bounds1.min.y;
                  // ISSUE: reference to a compiler-generated field
                  box2.bounds.min.xz = this.m_PrefabGeometryData.m_LegSize.xz * -0.5f;
                  // ISSUE: reference to a compiler-generated field
                  box2.bounds.max.y = this.m_PrefabGeometryData.m_LegSize.y;
                  // ISSUE: reference to a compiler-generated field
                  box2.bounds.max.xz = this.m_PrefabGeometryData.m_LegSize.xz * 0.5f;
                  box2.bounds += float3_2;
                  // ISSUE: reference to a compiler-generated field
                  box2.rotation = this.m_ObjectTransform.m_Rotation;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((collisionMask1 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1, box2, this.m_ObjectBounds, netCompositionData1, areas1_1, ref intersection1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CollisionFound = true;
                    return;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((collisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1, box2, this.m_ObjectBounds, netCompositionData2, areas1_2, ref intersection1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CollisionFound = true;
                    return;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((collisionMask3 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1, box2, this.m_ObjectBounds, netCompositionData3, areas1_3, ref intersection1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CollisionFound = true;
                    return;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              bounds1.min.y = this.m_PrefabGeometryData.m_LegSize.y;
            }
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              Cylinder3 cylinder2 = new Cylinder3();
              // ISSUE: reference to a compiler-generated field
              cylinder2.circle = new Circle2(this.m_PrefabGeometryData.m_Size.x * 0.5f, float3_2.xz);
              cylinder2.height = new Bounds1(bounds1.min.y, bounds1.max.y) + float3_2.y;
              // ISSUE: reference to a compiler-generated field
              cylinder2.rotation = this.m_ObjectTransform.m_Rotation;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((collisionMask1 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1, cylinder2, this.m_ObjectBounds, netCompositionData1, areas1_1, ref intersection1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
                return;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((collisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1, cylinder2, this.m_ObjectBounds, netCompositionData2, areas1_2, ref intersection1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
                return;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((collisionMask3 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1, cylinder2, this.m_ObjectBounds, netCompositionData3, areas1_3, ref intersection1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
                return;
              }
            }
            else
            {
              Box3 box2 = new Box3();
              box2.bounds = bounds1 + float3_2;
              // ISSUE: reference to a compiler-generated field
              box2.rotation = this.m_ObjectTransform.m_Rotation;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((collisionMask1 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1, box2, this.m_ObjectBounds, netCompositionData1, areas1_1, ref intersection1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
                return;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((collisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1, box2, this.m_ObjectBounds, netCompositionData2, areas1_2, ref intersection1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
                return;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((collisionMask3 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1, box2, this.m_ObjectBounds, netCompositionData3, areas1_3, ref intersection1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
                return;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, mask2))
            return;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Circle2 circle2 = new Circle2(this.m_PrefabGeometryData.m_LegSize.x * 0.5f, (this.m_ObjectTransform.m_Position - float3_1).xz);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask1) && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1.xz, circle2, this.m_ObjectBounds.xz, netCompositionData1, areas1_1, ref intersection2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask2) && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1.xz, circle2, this.m_ObjectBounds.xz, netCompositionData2, areas1_2, ref intersection2))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask3) || !Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1.xz, circle2, this.m_ObjectBounds.xz, netCompositionData3, areas1_3, ref intersection2))
                    return;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                return;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Quad2 xz = ObjectUtils.CalculateBaseCorners(this.m_ObjectTransform.m_Position - float3_1, this.m_ObjectTransform.m_Rotation, new Bounds3()
              {
                min = {
                  xz = this.m_PrefabGeometryData.m_LegSize.xz * -0.5f
                },
                max = {
                  xz = this.m_PrefabGeometryData.m_LegSize.xz * 0.5f
                }
              }).xz;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask1) && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1.xz, xz, this.m_ObjectBounds.xz, netCompositionData1, areas1_1, ref intersection2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask2) && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1.xz, xz, this.m_ObjectBounds.xz, netCompositionData2, areas1_2, ref intersection2))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask3) || !Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1.xz, xz, this.m_ObjectBounds.xz, netCompositionData3, areas1_3, ref intersection2))
                    return;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Circle2 circle2 = new Circle2(this.m_PrefabGeometryData.m_Size.x * 0.5f, (this.m_ObjectTransform.m_Position - float3_1).xz);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask1) && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1.xz, circle2, this.m_ObjectBounds.xz, netCompositionData1, areas1_1, ref intersection2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask2) && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1.xz, circle2, this.m_ObjectBounds.xz, netCompositionData2, areas1_2, ref intersection2))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask3) || !Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1.xz, circle2, this.m_ObjectBounds.xz, netCompositionData3, areas1_3, ref intersection2))
                    return;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Quad2 xz = ObjectUtils.CalculateBaseCorners(this.m_ObjectTransform.m_Position - float3_1, this.m_ObjectTransform.m_Rotation, this.m_PrefabGeometryData.m_Bounds).xz;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask1) && Game.Net.ValidationHelpers.Intersect(edge1, this.m_ObjectEntity, edgeGeometry1, -float3_1.xz, xz, this.m_ObjectBounds.xz, netCompositionData1, areas1_1, ref intersection2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CollisionFound = true;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask2) && Game.Net.ValidationHelpers.Intersect(edge1.m_Start, this.m_ObjectEntity, startNodeGeometry.m_Geometry, -float3_1.xz, xz, this.m_ObjectBounds.xz, netCompositionData2, areas1_2, ref intersection2))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask3) || !Game.Net.ValidationHelpers.Intersect(edge1.m_End, this.m_ObjectEntity, endNodeGeometry.m_Geometry, -float3_1.xz, xz, this.m_ObjectBounds.xz, netCompositionData3, areas1_3, ref intersection2))
                    return;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CollisionFound = true;
                }
              }
            }
          }
        }
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Entity m_TopLevelEntity;
        public Entity m_ObjectEntity;
        public Bounds3 m_ObjectBounds;
        public CollisionMask m_CollisionMask;
        public Transform m_ObjectTransform;
        public ObjectGeometryData m_PrefabGeometryData;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
        public BufferLookup<Game.Areas.Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;
        public bool m_CollisionFound;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return !this.m_CollisionFound && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CollisionFound || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz))
            return;
          Entity entity = areaItem.m_Area;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.HasComponent(entity) && !this.m_BuildingData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            entity = this.m_OwnerData[entity].m_Owner;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_TopLevelEntity == entity)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          AreaGeometryData areaGeometryData = this.m_PrefabAreaGeometryData[this.m_PrefabRefData[areaItem.m_Area].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if ((areaGeometryData.m_Flags & Game.Areas.GeometryFlags.CanOverrideObjects) == (Game.Areas.GeometryFlags) 0 || (this.m_CollisionMask & AreaUtils.GetCollisionMask(areaGeometryData)) == (CollisionMask) 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Triangle2 xz = AreaUtils.GetTriangle3(this.m_AreaNodes[areaItem.m_Area], this.m_AreaTriangles[areaItem.m_Area][areaItem.m_Triangle]).xz;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_PrefabGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Circle2 circle = new Circle2(this.m_PrefabGeometryData.m_Size.x * 0.5f, this.m_ObjectTransform.m_Position.xz);
            if (!MathUtils.Intersect(xz, circle))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CollisionFound = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(this.m_ObjectTransform.m_Position, this.m_ObjectTransform.m_Rotation, this.m_PrefabGeometryData.m_Bounds).xz, xz))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CollisionFound = true;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Created> __Game_Common_Created_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Overridden> __Game_Common_Overridden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stack> __Game_Objects_Stack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Marker> __Game_Objects_Marker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentLookup = state.GetComponentLookup<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentLookup = state.GetComponentLookup<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentLookup = state.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Marker_RO_ComponentLookup = state.GetComponentLookup<Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
      }
    }
  }
}
