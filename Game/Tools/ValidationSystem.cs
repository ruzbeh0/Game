// Decompiled with JetBrains decompiler
// Type: Game.Tools.ValidationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ValidationSystem : GameSystemBase
  {
    private ModificationEndBarrier m_ModificationBarrier;
    private ToolSystem m_ToolSystem;
    private ValidationSystem.Components m_Components;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private InstanceCountSystem m_InstanceCountSystem;
    private CitySystem m_CitySystem;
    private IconCommandSystem m_IconCommandSystem;
    private WaterSystem m_WaterSystem;
    private TerrainSystem m_TerrainSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_UpdatedAreaQuery;
    private EntityQuery m_ToolErrorPrefabQuery;
    private ValidationSystem.ChunkType m_ChunkType;
    private ValidationSystem.EntityData m_EntityData;
    private ValidationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Components = this.World.GetOrCreateSystemManaged<ValidationSystem.Components>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceCountSystem = this.World.GetOrCreateSystemManaged<InstanceCountSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Relative>(), ComponentType.Exclude<Moving>(), ComponentType.Exclude<Stopped>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedAreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<Area>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ToolErrorPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<NotificationIconData>(), ComponentType.ReadOnly<ToolErrorData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_ChunkType = new ValidationSystem.ChunkType((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_EntityData = new ValidationSystem.EntityData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeList<ValidationSystem.BoundsData> nativeList1 = new NativeList<ValidationSystem.BoundsData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<ValidationSystem.BoundsData> nativeList2 = new NativeList<ValidationSystem.BoundsData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<ErrorData> nativeQueue1 = new NativeQueue<ErrorData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<ErrorData> nativeQueue2 = new NativeQueue<ErrorData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<Entity> nativeArray = new NativeArray<Entity>(27, Allocator.TempJob);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_UpdatedQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ChunkType.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_EntityData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ValidationSystem.BoundsListJob jobData1 = new ValidationSystem.BoundsListJob()
      {
        m_Chunks = archetypeChunkListAsync.AsDeferredJobArray(),
        m_ChunkType = this.m_ChunkType,
        m_EntityData = this.m_EntityData,
        m_EdgeList = nativeList1,
        m_ObjectList = nativeList2
      };
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
      JobHandle deps;
      JobHandle dependencies5;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ValidationSystem.ValidationJob jobData2 = new ValidationSystem.ValidationJob()
      {
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_Chunks = archetypeChunkListAsync.AsDeferredJobArray(),
        m_ChunkType = this.m_ChunkType,
        m_EntityData = this.m_EntityData,
        m_EdgeList = nativeList1,
        m_ObjectList = nativeList2,
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies1),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies3),
        m_InstanceCounts = this.m_InstanceCountSystem.GetInstanceCounts(true, out dependencies4),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_GroundWaterMap = this.m_GroundWaterSystem.GetMap(true, out dependencies5),
        m_ErrorQueue = nativeQueue1.AsParallelWriter()
      };
      JobHandle job1_1 = JobUtils.CombineDependencies(dependencies1, dependencies2, dependencies3, dependencies4, deps, dependencies5);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Components.m_ErrorMap = new NativeHashMap<Entity, ErrorSeverity>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_UpdatedAreaQuery.IsEmptyIgnoreFilter)
      {
        NativeList<AreaSearchItem> nativeList3 = new NativeList<AreaSearchItem>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ValidationSystem.CollectAreaTrianglesJob jobData3 = new ValidationSystem.CollectAreaTrianglesJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_NativeType = this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle,
          m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
          m_AreaTriangles = nativeList3
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ValidationSystem.ValidateAreaTrianglesJob jobData4 = new ValidationSystem.ValidateAreaTrianglesJob()
        {
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_AreaTriangles = nativeList3.AsDeferredJobArray(),
          m_EntityData = jobData2.m_EntityData,
          m_ObjectSearchTree = jobData2.m_ObjectSearchTree,
          m_NetSearchTree = jobData2.m_NetSearchTree,
          m_AreaSearchTree = jobData2.m_AreaSearchTree,
          m_WaterSurfaceData = jobData2.m_WaterSurfaceData,
          m_TerrainHeightData = jobData2.m_TerrainHeightData,
          m_ErrorQueue = nativeQueue2.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle job0 = jobData3.Schedule<ValidationSystem.CollectAreaTrianglesJob>(this.m_UpdatedAreaQuery, this.Dependency);
        NativeList<AreaSearchItem> list = nativeList3;
        JobHandle dependsOn = JobHandle.CombineDependencies(job0, job1_1);
        jobHandle1 = jobData4.Schedule<ValidationSystem.ValidateAreaTrianglesJob, AreaSearchItem>(list, 1, dependsOn);
        nativeList3.Dispose(jobHandle1);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ToolErrorData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ValidationSystem.FillErrorPrefabsJob jobData5 = new ValidationSystem.FillErrorPrefabsJob()
      {
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ToolErrorType = this.__TypeHandle.__Game_Prefabs_ToolErrorData_RO_ComponentTypeHandle,
        m_ErrorPrefabs = nativeArray
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_PlayerMoney_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ValidationSystem.ProcessValidationResultsJob jobData6 = new ValidationSystem.ProcessValidationResultsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_BrushType = this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PlayerMoney = this.__TypeHandle.__Game_City_PlayerMoney_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_Chunks = archetypeChunkListAsync,
        m_City = this.m_CitySystem.City,
        m_ErrorMap = this.m_Components.m_ErrorMap,
        m_ErrorPrefabs = nativeArray,
        m_ErrorQueue1 = nativeQueue1,
        m_ErrorQueue2 = nativeQueue2,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      };
      this.Dependency = JobHandle.CombineDependencies(this.Dependency, outJobHandle);
      JobHandle job0_1 = jobData1.Schedule<ValidationSystem.BoundsListJob>(this.Dependency);
      JobHandle jobHandle2 = JobHandle.CombineDependencies(jobData2.Schedule<ValidationSystem.ValidationJob, ArchetypeChunk>(archetypeChunkListAsync, 1, JobHandle.CombineDependencies(job0_1, job1_1)), jobHandle1);
      // ISSUE: reference to a compiler-generated field
      JobHandle job1_2 = jobData5.Schedule<ValidationSystem.FillErrorPrefabsJob>(this.m_ToolErrorPrefabQuery, this.Dependency);
      JobHandle dependsOn1 = JobHandle.CombineDependencies(jobHandle2, job1_2);
      JobHandle jobHandle3 = jobData6.Schedule<ValidationSystem.ProcessValidationResultsJob>(dependsOn1);
      nativeList1.Dispose(jobHandle2);
      nativeList2.Dispose(jobHandle2);
      nativeQueue1.Dispose(jobHandle3);
      nativeQueue2.Dispose(jobHandle3);
      archetypeChunkListAsync.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InstanceCountSystem.AddCountReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Components.m_ErrorMapDeps = jobHandle3;
      this.Dependency = jobHandle3;
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
    public ValidationSystem()
    {
    }

    public struct ChunkType
    {
      public EntityTypeHandle m_Entity;
      public ComponentTypeHandle<Temp> m_Temp;
      public ComponentTypeHandle<Owner> m_Owner;
      public ComponentTypeHandle<Native> m_Native;
      public ComponentTypeHandle<Brush> m_Brush;
      public ComponentTypeHandle<PrefabRef> m_PrefabRef;
      public ComponentTypeHandle<Game.Objects.Object> m_Object;
      public ComponentTypeHandle<Transform> m_Transform;
      public ComponentTypeHandle<Attached> m_Attached;
      public ComponentTypeHandle<Game.Objects.NetObject> m_NetObject;
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnection;
      public ComponentTypeHandle<Building> m_Building;
      public ComponentTypeHandle<Game.Buildings.ServiceUpgrade> m_ServiceUpgrade;
      public ComponentTypeHandle<Game.Routes.TransportStop> m_TransportStop;
      public ComponentTypeHandle<Game.Net.Edge> m_Edge;
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometry;
      public ComponentTypeHandle<StartNodeGeometry> m_StartNodeGeometry;
      public ComponentTypeHandle<EndNodeGeometry> m_EndNodeGeometry;
      public ComponentTypeHandle<Composition> m_Composition;
      public ComponentTypeHandle<Lane> m_Lane;
      public ComponentTypeHandle<Game.Net.TrackLane> m_TrackLane;
      public ComponentTypeHandle<Curve> m_Curve;
      public ComponentTypeHandle<EdgeLane> m_EdgeLane;
      public ComponentTypeHandle<Fixed> m_Fixed;
      public ComponentTypeHandle<Area> m_Area;
      public ComponentTypeHandle<Geometry> m_AreaGeometry;
      public ComponentTypeHandle<Storage> m_AreaStorage;
      public BufferTypeHandle<Game.Areas.Node> m_AreaNode;
      public BufferTypeHandle<RouteWaypoint> m_RouteWaypoint;
      public BufferTypeHandle<RouteSegment> m_RouteSegment;
      public ComponentTypeHandle<Game.Simulation.WaterSourceData> m_WaterSourceData;

      public ChunkType(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = system.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.m_Temp = system.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = system.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Native = system.GetComponentTypeHandle<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Brush = system.GetComponentTypeHandle<Brush>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRef = system.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Object = system.GetComponentTypeHandle<Game.Objects.Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Transform = system.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Attached = system.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_NetObject = system.GetComponentTypeHandle<Game.Objects.NetObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_OutsideConnection = system.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Building = system.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceUpgrade = system.GetComponentTypeHandle<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_TransportStop = system.GetComponentTypeHandle<Game.Routes.TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Edge = system.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeGeometry = system.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometry = system.GetComponentTypeHandle<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometry = system.GetComponentTypeHandle<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Composition = system.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = system.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_TrackLane = system.GetComponentTypeHandle<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Curve = system.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeLane = system.GetComponentTypeHandle<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Fixed = system.GetComponentTypeHandle<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Area = system.GetComponentTypeHandle<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaGeometry = system.GetComponentTypeHandle<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaStorage = system.GetComponentTypeHandle<Storage>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaNode = system.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteWaypoint = system.GetBufferTypeHandle<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteSegment = system.GetBufferTypeHandle<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_WaterSourceData = system.GetComponentTypeHandle<Game.Simulation.WaterSourceData>(true);
      }

      public void Update(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Temp.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Owner.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Native.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Brush.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRef.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Object.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Transform.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Attached.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_NetObject.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_OutsideConnection.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Building.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceUpgrade.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_TransportStop.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Edge.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Composition.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_TrackLane.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Curve.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeLane.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Fixed.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Area.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaStorage.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaNode.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteWaypoint.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteSegment.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_WaterSourceData.Update(system);
      }
    }

    public struct EntityData
    {
      public ComponentLookup<Owner> m_Owner;
      public ComponentLookup<Hidden> m_Hidden;
      public ComponentLookup<Temp> m_Temp;
      public ComponentLookup<Native> m_Native;
      public ComponentLookup<Transform> m_Transform;
      public ComponentLookup<Game.Objects.Elevation> m_ObjectElevation;
      public ComponentLookup<Secondary> m_Secondary;
      public ComponentLookup<AssetStamp> m_AssetStamp;
      public ComponentLookup<Attachment> m_Attachment;
      public ComponentLookup<Attached> m_Attached;
      public ComponentLookup<Stack> m_Stack;
      public ComponentLookup<Building> m_Building;
      public BufferLookup<InstalledUpgrade> m_Upgrades;
      public ComponentLookup<Game.Net.Node> m_Node;
      public ComponentLookup<Game.Net.Edge> m_Edge;
      public ComponentLookup<EdgeGeometry> m_EdgeGeometry;
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometry;
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometry;
      public ComponentLookup<Composition> m_Composition;
      public ComponentLookup<Game.Net.Elevation> m_NetElevation;
      public ComponentLookup<Lane> m_Lane;
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLane;
      public ComponentLookup<Game.Net.CarLane> m_CarLane;
      public ComponentLookup<Game.Net.TrackLane> m_TrackLane;
      public ComponentLookup<Curve> m_Curve;
      public BufferLookup<Game.Net.SubLane> m_Lanes;
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      public ComponentLookup<Area> m_Area;
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      public BufferLookup<Triangle> m_AreaTriangles;
      public ComponentLookup<PathInformation> m_PathInformation;
      public ComponentLookup<Route> m_Route;
      public ComponentLookup<Connected> m_RouteConnected;
      public ComponentLookup<OnFire> m_OnFire;
      public ComponentLookup<PrefabRef> m_PrefabRef;
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometry;
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuilding;
      public ComponentLookup<PlaceableObjectData> m_PlaceableObject;
      public ComponentLookup<StackData> m_PrefabStackData;
      public ComponentLookup<NetObjectData> m_PrefabNetObject;
      public ComponentLookup<PlaceableNetData> m_PlaceableNet;
      public ComponentLookup<NetCompositionData> m_PrefabComposition;
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometry;
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometry;
      public ComponentLookup<StorageAreaData> m_PrefabStorageArea;
      public ComponentLookup<LotData> m_PrefabLotData;
      public ComponentLookup<CarLaneData> m_CarLaneData;
      public ComponentLookup<TrackLaneData> m_TrackLaneData;
      public ComponentLookup<RouteConnectionData> m_RouteConnectionData;
      public ComponentLookup<TransportStopData> m_TransportStopData;
      public ComponentLookup<TransportLineData> m_TransportLineData;
      public ComponentLookup<WaterPumpingStationData> m_WaterPumpingStationData;
      public ComponentLookup<GroundWaterPoweredData> m_GroundWaterPoweredData;
      public ComponentLookup<TerraformingData> m_TerraformingData;
      public ComponentLookup<ServiceUpgradeData> m_ServiceUpgradeData;
      public BufferLookup<NetCompositionArea> m_PrefabCompositionAreas;
      public BufferLookup<FixedNetElement> m_PrefabFixedElements;

      public EntityData(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = system.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Hidden = system.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Temp = system.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Native = system.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Transform = system.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectElevation = system.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Secondary = system.GetComponentLookup<Secondary>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_AssetStamp = system.GetComponentLookup<AssetStamp>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Attachment = system.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Attached = system.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Stack = system.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Building = system.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Upgrades = system.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Node = system.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Edge = system.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeGeometry = system.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometry = system.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometry = system.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Composition = system.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_NetElevation = system.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = system.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PedestrianLane = system.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_CarLane = system.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_TrackLane = system.GetComponentLookup<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Curve = system.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Lanes = system.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_ConnectedNodes = system.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_ConnectedEdges = system.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Area = system.GetComponentLookup<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaNodes = system.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTriangles = system.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PathInformation = system.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_Route = system.GetComponentLookup<Route>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteConnected = system.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_OnFire = system.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRef = system.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabObjectGeometry = system.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabBuilding = system.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PlaceableObject = system.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabStackData = system.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetObject = system.GetComponentLookup<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PlaceableNet = system.GetComponentLookup<PlaceableNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabComposition = system.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetGeometry = system.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabAreaGeometry = system.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabStorageArea = system.GetComponentLookup<StorageAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabLotData = system.GetComponentLookup<LotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_CarLaneData = system.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_TrackLaneData = system.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteConnectionData = system.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_TransportStopData = system.GetComponentLookup<TransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_TransportLineData = system.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_WaterPumpingStationData = system.GetComponentLookup<WaterPumpingStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_GroundWaterPoweredData = system.GetComponentLookup<GroundWaterPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_TerraformingData = system.GetComponentLookup<TerraformingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceUpgradeData = system.GetComponentLookup<ServiceUpgradeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabCompositionAreas = system.GetBufferLookup<NetCompositionArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabFixedElements = system.GetBufferLookup<FixedNetElement>(true);
      }

      public void Update(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Owner.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Hidden.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Temp.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Native.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Transform.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectElevation.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Secondary.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_AssetStamp.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Attachment.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Attached.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Stack.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Building.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Upgrades.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Node.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Edge.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Composition.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_NetElevation.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PedestrianLane.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_CarLane.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_TrackLane.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Curve.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Lanes.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_ConnectedNodes.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_ConnectedEdges.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Area.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaNodes.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTriangles.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PathInformation.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_Route.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteConnected.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_OnFire.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRef.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabObjectGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabBuilding.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PlaceableObject.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabStackData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetObject.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PlaceableNet.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabComposition.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabAreaGeometry.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabStorageArea.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabLotData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_CarLaneData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_TrackLaneData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteConnectionData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_TransportStopData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_TransportLineData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_WaterPumpingStationData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_GroundWaterPoweredData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_TerraformingData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceUpgradeData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabCompositionAreas.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabFixedElements.Update(system);
      }
    }

    public struct BoundsData
    {
      public Bounds3 m_Bounds;
      public Entity m_Entity;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct BoundsComparerX : IComparer<ValidationSystem.BoundsData>
    {
      public int Compare(ValidationSystem.BoundsData x, ValidationSystem.BoundsData y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(math.select(1, -1, (double) x.m_Bounds.min.x < (double) y.m_Bounds.min.x), 0, (double) x.m_Bounds.min.x == (double) y.m_Bounds.min.x);
      }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct BoundsComparerZ : IComparer<ValidationSystem.BoundsData>
    {
      public int Compare(ValidationSystem.BoundsData x, ValidationSystem.BoundsData y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(math.select(1, -1, (double) x.m_Bounds.min.z < (double) y.m_Bounds.min.z), 0, (double) x.m_Bounds.min.z == (double) y.m_Bounds.min.z);
      }
    }

    [BurstCompile]
    private struct BoundsListJob : IJob
    {
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ValidationSystem.ChunkType m_ChunkType;
      [ReadOnly]
      public ValidationSystem.EntityData m_EntityData;
      public NativeList<ValidationSystem.BoundsData> m_EdgeList;
      public NativeList<ValidationSystem.BoundsData> m_ObjectList;

      public void Execute()
      {
        Bounds3 bounds1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        Bounds3 bounds2 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray1 = chunk.GetNativeArray<Transform>(ref this.m_ChunkType.m_Transform);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<EdgeGeometry> nativeArray2 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_ChunkType.m_EdgeGeometry);
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.BoundsData boundsData1;
          if (nativeArray2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<StartNodeGeometry> nativeArray5 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_ChunkType.m_StartNodeGeometry);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<EndNodeGeometry> nativeArray6 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_ChunkType.m_EndNodeGeometry);
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              if ((nativeArray4[index2].m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
              {
                EdgeGeometry edgeGeometry = nativeArray2[index2];
                StartNodeGeometry startNodeGeometry = nativeArray5[index2];
                EndNodeGeometry endNodeGeometry = nativeArray6[index2];
                // ISSUE: object of a compiler-generated type is created
                boundsData1 = new ValidationSystem.BoundsData();
                // ISSUE: reference to a compiler-generated field
                boundsData1.m_Entity = nativeArray3[index2];
                // ISSUE: reference to a compiler-generated field
                boundsData1.m_Bounds = edgeGeometry.m_Bounds | startNodeGeometry.m_Geometry.m_Bounds | endNodeGeometry.m_Geometry.m_Bounds;
                // ISSUE: variable of a compiler-generated type
                ValidationSystem.BoundsData boundsData2 = boundsData1;
                // ISSUE: reference to a compiler-generated field
                this.m_EdgeList.Add(in boundsData2);
                // ISSUE: reference to a compiler-generated field
                bounds1 |= boundsData2.m_Bounds;
              }
            }
          }
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray7 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray8 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray9 = chunk.GetNativeArray<PrefabRef>(ref this.m_ChunkType.m_PrefabRef);
            for (int index3 = 0; index3 < nativeArray7.Length; ++index3)
            {
              if ((nativeArray8[index3].m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
              {
                PrefabRef prefabRef = nativeArray9[index3];
                ObjectGeometryData componentData1;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_EntityData.m_PrefabObjectGeometry.TryGetComponent(prefabRef.m_Prefab, out componentData1))
                {
                  // ISSUE: object of a compiler-generated type is created
                  boundsData1 = new ValidationSystem.BoundsData();
                  // ISSUE: reference to a compiler-generated field
                  boundsData1.m_Entity = nativeArray7[index3];
                  // ISSUE: variable of a compiler-generated type
                  ValidationSystem.BoundsData boundsData3 = boundsData1;
                  Transform transform = nativeArray1[index3];
                  Stack componentData2;
                  StackData componentData3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  boundsData3.m_Bounds = !this.m_EntityData.m_Stack.TryGetComponent(boundsData3.m_Entity, out componentData2) || !this.m_EntityData.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData3) ? ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData1) : ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData2, componentData1, componentData3);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ObjectList.Add(in boundsData3);
                  // ISSUE: reference to a compiler-generated field
                  bounds2 |= boundsData3.m_Bounds;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeList.Length >= 2)
        {
          float3 float3 = MathUtils.Size(bounds1);
          if ((double) float3.z > (double) float3.x)
          {
            // ISSUE: reference to a compiler-generated field
            NativeList<ValidationSystem.BoundsData> edgeList = this.m_EdgeList;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.BoundsComparerZ boundsComparerZ = new ValidationSystem.BoundsComparerZ();
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.BoundsComparerZ comp = boundsComparerZ;
            edgeList.Sort<ValidationSystem.BoundsData, ValidationSystem.BoundsComparerZ>(comp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeList<ValidationSystem.BoundsData> edgeList = this.m_EdgeList;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.BoundsComparerX boundsComparerX = new ValidationSystem.BoundsComparerX();
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.BoundsComparerX comp = boundsComparerX;
            edgeList.Sort<ValidationSystem.BoundsData, ValidationSystem.BoundsComparerX>(comp);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectList.Length < 2)
          return;
        float3 float3_1 = MathUtils.Size(bounds2);
        if ((double) float3_1.z > (double) float3_1.x)
        {
          // ISSUE: reference to a compiler-generated field
          NativeList<ValidationSystem.BoundsData> objectList = this.m_ObjectList;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.BoundsComparerZ boundsComparerZ = new ValidationSystem.BoundsComparerZ();
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.BoundsComparerZ comp = boundsComparerZ;
          objectList.Sort<ValidationSystem.BoundsData, ValidationSystem.BoundsComparerZ>(comp);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeList<ValidationSystem.BoundsData> objectList = this.m_ObjectList;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.BoundsComparerX boundsComparerX = new ValidationSystem.BoundsComparerX();
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.BoundsComparerX comp = boundsComparerX;
          objectList.Sort<ValidationSystem.BoundsData, ValidationSystem.BoundsComparerX>(comp);
        }
      }
    }

    [BurstCompile]
    private struct ValidationJob : IJobParallelForDefer
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ValidationSystem.ChunkType m_ChunkType;
      [ReadOnly]
      public ValidationSystem.EntityData m_EntityData;
      [ReadOnly]
      public NativeList<ValidationSystem.BoundsData> m_EdgeList;
      [ReadOnly]
      public NativeList<ValidationSystem.BoundsData> m_ObjectList;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public NativeParallelHashMap<Entity, int> m_InstanceCounts;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public NativeArray<GroundWater> m_GroundWaterMap;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;
      [NativeDisableContainerSafetyRestriction]
      private NativeList<ConnectedNode> m_TempNodes;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TempFlags tempFlags = chunk.Has<Native>(ref this.m_ChunkType.m_Native) ? TempFlags.Select | TempFlags.Duplicate : TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Objects.Object>(ref this.m_ChunkType.m_Object))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_ChunkType.m_Owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_ChunkType.m_Transform);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Attached> nativeArray5 = chunk.GetNativeArray<Attached>(ref this.m_ChunkType.m_Attached);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.NetObject> nativeArray6 = chunk.GetNativeArray<Game.Objects.NetObject>(ref this.m_ChunkType.m_NetObject);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_ChunkType.m_PrefabRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Building> nativeArray8 = chunk.GetNativeArray<Building>(ref this.m_ChunkType.m_Building);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Game.Objects.OutsideConnection>(ref this.m_ChunkType.m_OutsideConnection);
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Temp temp1 = nativeArray2[index1];
            if ((temp1.m_Flags & tempFlags) == (TempFlags) 0)
            {
              Entity entity = nativeArray1[index1];
              Transform transform1 = nativeArray4[index1];
              PrefabRef prefabRef1 = nativeArray7[index1];
              Owner owner1 = new Owner();
              if (nativeArray3.Length != 0)
                owner1 = nativeArray3[index1];
              Attached attached1 = new Attached();
              if (nativeArray5.Length != 0)
                attached1 = nativeArray5[index1];
              Temp temp2 = temp1;
              Owner owner2 = owner1;
              Transform transform2 = transform1;
              PrefabRef prefabRef2 = prefabRef1;
              Attached attached2 = attached1;
              int num1 = flag ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              int num2 = this.m_EditorMode ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ValidationSystem.EntityData entityData = this.m_EntityData;
              // ISSUE: reference to a compiler-generated field
              NativeList<ValidationSystem.BoundsData> edgeList = this.m_EdgeList;
              // ISSUE: reference to a compiler-generated field
              NativeList<ValidationSystem.BoundsData> objectList = this.m_ObjectList;
              // ISSUE: reference to a compiler-generated field
              NativeQuadTree<Entity, QuadTreeBoundsXZ> objectSearchTree = this.m_ObjectSearchTree;
              // ISSUE: reference to a compiler-generated field
              NativeQuadTree<Entity, QuadTreeBoundsXZ> netSearchTree = this.m_NetSearchTree;
              // ISSUE: reference to a compiler-generated field
              NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> areaSearchTree = this.m_AreaSearchTree;
              // ISSUE: reference to a compiler-generated field
              NativeParallelHashMap<Entity, int> instanceCounts = this.m_InstanceCounts;
              // ISSUE: reference to a compiler-generated field
              WaterSurfaceData waterSurfaceData = this.m_WaterSurfaceData;
              // ISSUE: reference to a compiler-generated field
              TerrainHeightData terrainHeightData = this.m_TerrainHeightData;
              // ISSUE: reference to a compiler-generated field
              NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
              Game.Objects.ValidationHelpers.ValidateObject(entity, temp2, owner2, transform2, prefabRef2, attached2, num1 != 0, num2 != 0, entityData, edgeList, objectList, objectSearchTree, netSearchTree, areaSearchTree, instanceCounts, waterSurfaceData, terrainHeightData, errorQueue);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((temp1.m_Flags & (TempFlags.Delete | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) != (TempFlags) 0 && temp1.m_Original != Entity.Null && this.m_EntityData.m_OnFire.HasComponent(temp1.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ErrorQueue.Enqueue(new ErrorData()
              {
                m_ErrorType = ErrorType.OnFire,
                m_ErrorSeverity = ErrorSeverity.Error,
                m_TempEntity = nativeArray1[index1],
                m_Position = (float3) float.NaN
              });
            }
          }
          for (int index2 = 0; index2 < nativeArray8.Length; ++index2)
          {
            if ((nativeArray2[index2].m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
            {
              Entity entity = nativeArray1[index2];
              Building building1 = nativeArray8[index2];
              Transform transform3 = nativeArray4[index2];
              PrefabRef prefabRef3 = nativeArray7[index2];
              Building building2 = building1;
              Transform transform4 = transform3;
              PrefabRef prefabRef4 = prefabRef3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ValidationSystem.EntityData entityData = this.m_EntityData;
              // ISSUE: reference to a compiler-generated field
              NativeArray<GroundWater> groundWaterMap = this.m_GroundWaterMap;
              // ISSUE: reference to a compiler-generated field
              NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
              Game.Buildings.ValidationHelpers.ValidateBuilding(entity, building2, transform4, prefabRef4, entityData, groundWaterMap, errorQueue);
            }
          }
          for (int index3 = 0; index3 < nativeArray6.Length; ++index3)
          {
            if ((nativeArray2[index3].m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
            {
              Entity entity = nativeArray1[index3];
              Game.Objects.NetObject netObject1 = nativeArray6[index3];
              Transform transform5 = nativeArray4[index3];
              PrefabRef prefabRef5 = nativeArray7[index3];
              Attached attached3 = new Attached();
              if (nativeArray5.Length != 0)
                attached3 = nativeArray5[index3];
              Game.Objects.NetObject netObject2 = netObject1;
              Transform transform6 = transform5;
              PrefabRef prefabRef6 = prefabRef5;
              Attached attached4 = attached3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ValidationSystem.EntityData entityData = this.m_EntityData;
              // ISSUE: reference to a compiler-generated field
              NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
              Game.Objects.ValidationHelpers.ValidateNetObject(entity, netObject2, transform6, prefabRef6, attached4, entityData, errorQueue);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Game.Routes.TransportStop>(ref this.m_ChunkType.m_TransportStop))
          {
            for (int index4 = 0; index4 < nativeArray1.Length; ++index4)
            {
              Temp temp = nativeArray2[index4];
              if ((temp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
              {
                Entity entity = nativeArray1[index4];
                Transform transform = nativeArray4[index4];
                PrefabRef prefabRef = nativeArray7[index4];
                Owner owner = new Owner();
                if (nativeArray3.Length != 0)
                  owner = nativeArray3[index4];
                Attached attached = new Attached();
                if (nativeArray5.Length != 0)
                  attached = nativeArray5[index4];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Game.Routes.ValidationHelpers.ValidateStop(this.m_EditorMode, entity, temp, owner, transform, prefabRef, attached, this.m_EntityData, this.m_ErrorQueue);
              }
            }
          }
          if (flag)
          {
            for (int index5 = 0; index5 < nativeArray1.Length; ++index5)
            {
              if ((nativeArray2[index5].m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Game.Objects.ValidationHelpers.ValidateOutsideConnection(nativeArray1[index5], nativeArray4[index5], this.m_TerrainHeightData, this.m_ErrorQueue);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Buildings.ServiceUpgrade>(ref this.m_ChunkType.m_ServiceUpgrade))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray9 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray10 = chunk.GetNativeArray<PrefabRef>(ref this.m_ChunkType.m_PrefabRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray11 = chunk.GetNativeArray<Owner>(ref this.m_ChunkType.m_Owner);
          for (int index6 = 0; index6 < nativeArray9.Length; ++index6)
          {
            Entity entity = nativeArray9[index6];
            PrefabRef prefabRef7 = nativeArray10[index6];
            Owner owner3 = new Owner();
            if (nativeArray11.Length != 0)
              owner3 = nativeArray11[index6];
            Owner owner4 = owner3;
            PrefabRef prefabRef8 = prefabRef7;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.EntityData entityData = this.m_EntityData;
            // ISSUE: reference to a compiler-generated field
            NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
            Game.Buildings.ValidationHelpers.ValidateUpgrade(entity, owner4, prefabRef8, entityData, errorQueue);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Net.Edge>(ref this.m_ChunkType.m_Edge))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray12 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray13 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray14 = chunk.GetNativeArray<Owner>(ref this.m_ChunkType.m_Owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.Edge> nativeArray15 = chunk.GetNativeArray<Game.Net.Edge>(ref this.m_ChunkType.m_Edge);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<EdgeGeometry> nativeArray16 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_ChunkType.m_EdgeGeometry);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<StartNodeGeometry> nativeArray17 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_ChunkType.m_StartNodeGeometry);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<EndNodeGeometry> nativeArray18 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_ChunkType.m_EndNodeGeometry);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Composition> nativeArray19 = chunk.GetNativeArray<Composition>(ref this.m_ChunkType.m_Composition);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Fixed> nativeArray20 = chunk.GetNativeArray<Fixed>(ref this.m_ChunkType.m_Fixed);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray21 = chunk.GetNativeArray<PrefabRef>(ref this.m_ChunkType.m_PrefabRef);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TempNodes.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TempNodes = new NativeList<ConnectedNode>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          }
          bool flag = nativeArray20.Length != 0;
          for (int index7 = 0; index7 < nativeArray16.Length; ++index7)
          {
            Temp temp3 = nativeArray13[index7];
            if ((temp3.m_Flags & tempFlags) == (TempFlags) 0)
            {
              Entity entity = nativeArray12[index7];
              Game.Net.Edge edge1 = nativeArray15[index7];
              EdgeGeometry edgeGeometry1 = nativeArray16[index7];
              StartNodeGeometry startNodeGeometry1 = nativeArray17[index7];
              EndNodeGeometry endNodeGeometry1 = nativeArray18[index7];
              Composition composition1 = nativeArray19[index7];
              PrefabRef prefabRef9 = nativeArray21[index7];
              Owner owner5 = new Owner();
              if (nativeArray14.Length != 0)
                owner5 = nativeArray14[index7];
              Fixed @fixed = new Fixed() { m_Index = -1 };
              if (flag)
                @fixed = nativeArray20[index7];
              Temp temp4 = temp3;
              Owner owner6 = owner5;
              Fixed _fixed = @fixed;
              Game.Net.Edge edge2 = edge1;
              EdgeGeometry edgeGeometry2 = edgeGeometry1;
              StartNodeGeometry startNodeGeometry2 = startNodeGeometry1;
              EndNodeGeometry endNodeGeometry2 = endNodeGeometry1;
              Composition composition2 = composition1;
              PrefabRef prefabRef10 = prefabRef9;
              // ISSUE: reference to a compiler-generated field
              int num = this.m_EditorMode ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ValidationSystem.EntityData entityData = this.m_EntityData;
              // ISSUE: reference to a compiler-generated field
              NativeList<ValidationSystem.BoundsData> edgeList = this.m_EdgeList;
              // ISSUE: reference to a compiler-generated field
              NativeQuadTree<Entity, QuadTreeBoundsXZ> objectSearchTree = this.m_ObjectSearchTree;
              // ISSUE: reference to a compiler-generated field
              NativeQuadTree<Entity, QuadTreeBoundsXZ> netSearchTree = this.m_NetSearchTree;
              // ISSUE: reference to a compiler-generated field
              NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> areaSearchTree = this.m_AreaSearchTree;
              // ISSUE: reference to a compiler-generated field
              WaterSurfaceData waterSurfaceData = this.m_WaterSurfaceData;
              // ISSUE: reference to a compiler-generated field
              TerrainHeightData terrainHeightData = this.m_TerrainHeightData;
              // ISSUE: reference to a compiler-generated field
              NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
              // ISSUE: reference to a compiler-generated field
              NativeList<ConnectedNode> tempNodes = this.m_TempNodes;
              Game.Net.ValidationHelpers.ValidateEdge(entity, temp4, owner6, _fixed, edge2, edgeGeometry2, startNodeGeometry2, endNodeGeometry2, composition2, prefabRef10, num != 0, entityData, edgeList, objectSearchTree, netSearchTree, areaSearchTree, waterSurfaceData, terrainHeightData, errorQueue, tempNodes);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Lane>(ref this.m_ChunkType.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray22 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray23 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray24 = chunk.GetNativeArray<Owner>(ref this.m_ChunkType.m_Owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Lane> nativeArray25 = chunk.GetNativeArray<Lane>(ref this.m_ChunkType.m_Lane);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.TrackLane> nativeArray26 = chunk.GetNativeArray<Game.Net.TrackLane>(ref this.m_ChunkType.m_TrackLane);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray27 = chunk.GetNativeArray<Curve>(ref this.m_ChunkType.m_Curve);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<EdgeLane> nativeArray28 = chunk.GetNativeArray<EdgeLane>(ref this.m_ChunkType.m_EdgeLane);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray29 = chunk.GetNativeArray<PrefabRef>(ref this.m_ChunkType.m_PrefabRef);
          for (int index8 = 0; index8 < nativeArray26.Length; ++index8)
          {
            if ((nativeArray23[index8].m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
            {
              Entity entity = nativeArray22[index8];
              Lane lane1 = nativeArray25[index8];
              Game.Net.TrackLane trackLane1 = nativeArray26[index8];
              Curve curve1 = nativeArray27[index8];
              PrefabRef prefabRef11 = nativeArray29[index8];
              Owner owner7 = new Owner();
              if (nativeArray24.Length != 0)
                owner7 = nativeArray24[index8];
              EdgeLane edgeLane1 = new EdgeLane();
              if (nativeArray28.Length != 0)
                edgeLane1 = nativeArray28[index8];
              Owner owner8 = owner7;
              Lane lane2 = lane1;
              Game.Net.TrackLane trackLane2 = trackLane1;
              Curve curve2 = curve1;
              EdgeLane edgeLane2 = edgeLane1;
              PrefabRef prefabRef12 = prefabRef11;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ValidationSystem.EntityData entityData = this.m_EntityData;
              // ISSUE: reference to a compiler-generated field
              NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
              Game.Net.ValidationHelpers.ValidateLane(entity, owner8, lane2, trackLane2, curve2, edgeLane2, prefabRef12, entityData, errorQueue);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Area>(ref this.m_ChunkType.m_Area))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray30 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray31 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray32 = chunk.GetNativeArray<Owner>(ref this.m_ChunkType.m_Owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Area> nativeArray33 = chunk.GetNativeArray<Area>(ref this.m_ChunkType.m_Area);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Geometry> nativeArray34 = chunk.GetNativeArray<Geometry>(ref this.m_ChunkType.m_AreaGeometry);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Storage> nativeArray35 = chunk.GetNativeArray<Storage>(ref this.m_ChunkType.m_AreaStorage);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Areas.Node> bufferAccessor = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_ChunkType.m_AreaNode);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray36 = chunk.GetNativeArray<PrefabRef>(ref this.m_ChunkType.m_PrefabRef);
          for (int index9 = 0; index9 < nativeArray30.Length; ++index9)
          {
            Temp temp = nativeArray31[index9];
            if ((temp.m_Flags & tempFlags) == (TempFlags) 0)
            {
              Entity entity = nativeArray30[index9];
              Area area = nativeArray33[index9];
              DynamicBuffer<Game.Areas.Node> nodes = bufferAccessor[index9];
              PrefabRef prefabRef = nativeArray36[index9];
              Geometry geometry = new Geometry();
              if (nativeArray34.Length != 0)
                geometry = nativeArray34[index9];
              Storage storage = new Storage();
              if (nativeArray35.Length != 0)
                storage = nativeArray35[index9];
              Owner owner = new Owner();
              if (nativeArray32.Length != 0)
                owner = nativeArray32[index9];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Areas.ValidationHelpers.ValidateArea(this.m_EditorMode, entity, temp, owner, area, geometry, storage, nodes, prefabRef, this.m_EntityData, this.m_ObjectSearchTree, this.m_NetSearchTree, this.m_AreaSearchTree, this.m_WaterSurfaceData, this.m_TerrainHeightData, this.m_ErrorQueue);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<RouteSegment>(ref this.m_ChunkType.m_RouteSegment))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray37 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray38 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray39 = chunk.GetNativeArray<PrefabRef>(ref this.m_ChunkType.m_PrefabRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteWaypoint> bufferAccessor1 = chunk.GetBufferAccessor<RouteWaypoint>(ref this.m_ChunkType.m_RouteWaypoint);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteSegment> bufferAccessor2 = chunk.GetBufferAccessor<RouteSegment>(ref this.m_ChunkType.m_RouteSegment);
          for (int index10 = 0; index10 < nativeArray37.Length; ++index10)
          {
            Temp temp5 = nativeArray38[index10];
            if ((temp5.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
            {
              Entity entity = nativeArray37[index10];
              PrefabRef prefabRef13 = nativeArray39[index10];
              DynamicBuffer<RouteWaypoint> dynamicBuffer1 = bufferAccessor1[index10];
              DynamicBuffer<RouteSegment> dynamicBuffer2 = bufferAccessor2[index10];
              Temp temp6 = temp5;
              PrefabRef prefabRef14 = prefabRef13;
              DynamicBuffer<RouteWaypoint> waypoints = dynamicBuffer1;
              DynamicBuffer<RouteSegment> segments = dynamicBuffer2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ValidationSystem.EntityData entityData = this.m_EntityData;
              // ISSUE: reference to a compiler-generated field
              NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
              Game.Routes.ValidationHelpers.ValidateRoute(entity, temp6, prefabRef14, waypoints, segments, entityData, errorQueue);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode && chunk.Has<Brush>(ref this.m_ChunkType.m_Brush))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray40 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Brush> nativeArray41 = chunk.GetNativeArray<Brush>(ref this.m_ChunkType.m_Brush);
          for (int index11 = 0; index11 < nativeArray40.Length; ++index11)
          {
            Brush brush = nativeArray41[index11];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EntityData.m_TerraformingData.HasComponent(brush.m_Tool))
            {
              Entity entity = nativeArray40[index11];
              Bounds3 bounds = new Bounds3(brush.m_Position - brush.m_Size * 0.4f, brush.m_Position + brush.m_Size * 0.4f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Areas.ValidationHelpers.BrushAreaIterator iterator = new Game.Areas.ValidationHelpers.BrushAreaIterator()
              {
                m_BrushEntity = entity,
                m_Brush = brush,
                m_BrushBounds = bounds,
                m_Data = this.m_EntityData,
                m_ErrorQueue = this.m_ErrorQueue
              };
              // ISSUE: reference to a compiler-generated field
              this.m_AreaSearchTree.Iterate<Game.Areas.ValidationHelpers.BrushAreaIterator>(ref iterator);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Objects.ValidationHelpers.ValidateWorldBounds(entity, new Owner(), bounds, this.m_EntityData, this.m_TerrainHeightData, this.m_ErrorQueue);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<Game.Simulation.WaterSourceData>(ref this.m_ChunkType.m_WaterSourceData))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray42 = chunk.GetNativeArray(this.m_ChunkType.m_Entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray43 = chunk.GetNativeArray<Temp>(ref this.m_ChunkType.m_Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray44 = chunk.GetNativeArray<Transform>(ref this.m_ChunkType.m_Transform);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Simulation.WaterSourceData> nativeArray45 = chunk.GetNativeArray<Game.Simulation.WaterSourceData>(ref this.m_ChunkType.m_WaterSourceData);
        for (int index12 = 0; index12 < nativeArray42.Length; ++index12)
        {
          Temp temp = nativeArray43[index12];
          if ((temp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0 || (temp.m_Flags & TempFlags.Dragging) != (TempFlags) 0)
          {
            Entity entity = nativeArray42[index12];
            Transform transform7 = nativeArray44[index12];
            Game.Simulation.WaterSourceData waterSourceData1 = nativeArray45[index12];
            Transform transform8 = transform7;
            Game.Simulation.WaterSourceData waterSourceData2 = waterSourceData1;
            // ISSUE: reference to a compiler-generated field
            TerrainHeightData terrainHeightData = this.m_TerrainHeightData;
            // ISSUE: reference to a compiler-generated field
            NativeQueue<ErrorData>.ParallelWriter errorQueue = this.m_ErrorQueue;
            Game.Objects.ValidationHelpers.ValidateWaterSource(entity, transform8, waterSourceData2, terrainHeightData, errorQueue);
          }
        }
      }
    }

    [BurstCompile]
    private struct CollectAreaTrianglesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Native> m_NativeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      public NativeList<AreaSearchItem> m_AreaTriangles;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Triangle> bufferAccessor = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
        // ISSUE: reference to a compiler-generated field
        TempFlags tempFlags = chunk.Has<Native>(ref this.m_NativeType) ? TempFlags.Select : TempFlags.Delete | TempFlags.Select;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          if ((nativeArray2[index].m_Flags & tempFlags) == (TempFlags) 0)
          {
            Entity area = nativeArray1[index];
            DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor[index];
            for (int triangle = 0; triangle < dynamicBuffer.Length; ++triangle)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_AreaTriangles.Add(new AreaSearchItem(area, triangle));
            }
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
    private struct ValidateAreaTrianglesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeArray<AreaSearchItem> m_AreaTriangles;
      [ReadOnly]
      public ValidationSystem.EntityData m_EntityData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        AreaSearchItem areaTriangle1 = this.m_AreaTriangles[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Area area = this.m_EntityData.m_Area[areaTriangle1.m_Area];
        if ((area.m_Flags & AreaFlags.Slave) != (AreaFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Triangle> areaTriangle2 = this.m_EntityData.m_AreaTriangles[areaTriangle1.m_Area];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Temp temp = this.m_EntityData.m_Temp[areaTriangle1.m_Area];
        Owner owner = new Owner();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_EntityData.m_Owner.HasComponent(areaTriangle1.m_Area))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          owner = this.m_EntityData.m_Owner[areaTriangle1.m_Area];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Areas.ValidationHelpers.ValidateTriangle(this.m_EditorMode, (area.m_Flags & AreaFlags.Complete) == (AreaFlags) 0, areaTriangle1.m_Area, temp, owner, areaTriangle2[areaTriangle1.m_Triangle], this.m_EntityData, this.m_ObjectSearchTree, this.m_NetSearchTree, this.m_AreaSearchTree, this.m_WaterSurfaceData, this.m_TerrainHeightData, this.m_ErrorQueue);
      }
    }

    [BurstCompile]
    private struct FillErrorPrefabsJob : IJobChunk
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ToolErrorData> m_ToolErrorType;
      public NativeArray<Entity> m_ErrorPrefabs;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ToolErrorData> nativeArray2 = chunk.GetNativeArray<ToolErrorData>(ref this.m_ToolErrorType);
        // ISSUE: reference to a compiler-generated field
        ToolErrorFlags toolErrorFlags = this.m_EditorMode ? ToolErrorFlags.DisableInEditor : ToolErrorFlags.DisableInGame;
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          ToolErrorData toolErrorData = nativeArray2[index];
          if ((toolErrorData.m_Flags & toolErrorFlags) == (ToolErrorFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ErrorPrefabs[(int) toolErrorData.m_Error] = entity;
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

    private struct IconKey : IEquatable<ValidationSystem.IconKey>
    {
      public Entity m_Owner;
      public Entity m_Target;
      public Entity m_Prefab;

      public bool Equals(ValidationSystem.IconKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Owner.Equals(other.m_Owner) && this.m_Target.Equals(other.m_Target) && this.m_Prefab.Equals(other.m_Prefab);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return ((17 * 31 + this.m_Owner.GetHashCode()) * 31 + this.m_Target.GetHashCode()) * 31 + this.m_Prefab.GetHashCode();
      }
    }

    public struct IconValue
    {
      public Bounds3 m_Bounds;
      public IconPriority m_Severity;
      public bool m_Cancelled;
    }

    [BurstCompile]
    private struct ProcessValidationResultsJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Brush> m_BrushType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PlayerMoney> m_PlayerMoney;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<Entity> m_ErrorPrefabs;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public Entity m_City;
      public NativeHashMap<Entity, ErrorSeverity> m_ErrorMap;
      public NativeQueue<ErrorData> m_ErrorQueue1;
      public NativeQueue<ErrorData> m_ErrorQueue2;
      public EntityCommandBuffer m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute()
      {
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap = new NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        int totalCost = 0;
        Entity brushEntity = Entity.Null;
        float4 brushPosition = new float4();
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CalculateCost(this.m_Chunks[index], ref totalCost, ref brushEntity, ref brushPosition);
        }
        int num = totalCost;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.ProcessQueue(this.m_ErrorQueue1, iconMap, brushEntity, brushPosition, ref totalCost);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.ProcessQueue(this.m_ErrorQueue2, iconMap, brushEntity, brushPosition, ref totalCost);
        PlayerMoney componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlayerMoney.TryGetComponent(this.m_City, out componentData))
        {
          int costLimit = math.max(0, componentData.money);
          bool flag = totalCost > costLimit;
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_Chunks.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CancelOptionalWithMoneyError(this.m_Chunks[index], iconMap, brushEntity, brushPosition, ref totalCost, costLimit);
              if (totalCost <= costLimit)
                break;
            }
            flag = totalCost > costLimit;
          }
          if (!flag && num > costLimit)
          {
            flag = true;
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_Chunks.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (!this.AllCancelled(this.m_Chunks[index], ErrorSeverity.Cancel))
              {
                flag = false;
                break;
              }
            }
          }
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_Chunks.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.ProcessMoneyErrors(this.m_Chunks[index], iconMap, brushEntity, brushPosition, ref totalCost);
            }
          }
        }
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        NativeHashMap<Entity, ErrorSeverity>.Enumerator enumerator1 = this.m_ErrorMap.GetEnumerator();
        while (enumerator1.MoveNext())
        {
          if (enumerator1.Current.Value == ErrorSeverity.CancelError)
            flag1 = brushEntity != Entity.Null;
        }
        enumerator1.Dispose();
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_Chunks.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!this.AllCancelled(this.m_Chunks[index], ErrorSeverity.CancelError))
            {
              flag1 = false;
              break;
            }
          }
        }
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue>.Enumerator enumerator2 = iconMap.GetEnumerator();
        while (enumerator2.MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.IconKey key = enumerator2.Current.Key;
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.IconValue iconValue = enumerator2.Current.Value;
          // ISSUE: reference to a compiler-generated field
          if (iconValue.m_Cancelled)
          {
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              if (key.m_Owner != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.AddError(key.m_Owner, ErrorSeverity.Error);
              }
              // ISSUE: reference to a compiler-generated field
              if (key.m_Target != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.AddError(key.m_Target, ErrorSeverity.Error);
              }
            }
            else
              continue;
          }
          else
          {
            ErrorSeverity errorSeverity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ErrorMap.TryGetValue(key.m_Owner, out errorSeverity) && errorSeverity >= ErrorSeverity.Cancel)
              continue;
          }
          // ISSUE: reference to a compiler-generated field
          if (math.any(math.isnan(iconValue.m_Bounds.min)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Add(key.m_Owner, key.m_Prefab, iconValue.m_Severity, target: key.m_Target, isTemp: true);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            float3 location = MathUtils.Center(iconValue.m_Bounds);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Add(key.m_Owner, key.m_Prefab, location, iconValue.m_Severity, flags: (IconFlags) 0, target: key.m_Target, isTemp: true);
          }
        }
        enumerator2.Dispose();
        iconMap.Dispose();
      }

      private bool AllCancelled(ArchetypeChunk chunk, ErrorSeverity limit)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          ErrorSeverity errorSeverity;
          // ISSUE: reference to a compiler-generated field
          if ((nativeArray2[index].m_Flags & TempFlags.Cancel) == (TempFlags) 0 && (!this.m_ErrorMap.TryGetValue(nativeArray1[index], out errorSeverity) || errorSeverity < limit))
            return false;
        }
        return true;
      }

      private void CalculateCost(
        ArchetypeChunk chunk,
        ref int totalCost,
        ref Entity brushEntity,
        ref float4 brushPosition)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Brush> nativeArray3 = chunk.GetNativeArray<Brush>(ref this.m_BrushType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Temp temp = nativeArray2[index];
          totalCost += temp.m_Cost;
        }
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Brush brush = nativeArray3[index];
          brushEntity = nativeArray1[index];
          brushPosition += new float4(brush.m_Position * brush.m_Strength, brush.m_Strength);
        }
      }

      private void CancelOptionalWithMoneyError(
        ArchetypeChunk chunk,
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap,
        Entity brushEntity,
        float4 brushPosition,
        ref int totalCost,
        int costLimit)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray4 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        ErrorData error = new ErrorData();
        error.m_ErrorSeverity = ErrorSeverity.Error;
        error.m_ErrorType = ErrorType.NotEnoughMoney;
        error.m_Position = (float3) float.NaN;
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          if (nativeArray2[index].m_Cost > 0)
          {
            error.m_TempEntity = nativeArray1[index];
            // ISSUE: reference to a compiler-generated method
            if (this.CancelOptional(error, iconMap, brushEntity, brushPosition, ref totalCost) && totalCost <= costLimit)
              return;
          }
        }
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          if (nativeArray2[index].m_Cost > 0)
          {
            error.m_TempEntity = nativeArray1[index];
            // ISSUE: reference to a compiler-generated method
            if (this.CancelOptional(error, iconMap, brushEntity, brushPosition, ref totalCost) && totalCost <= costLimit)
              break;
          }
        }
      }

      private void ProcessMoneyErrors(
        ArchetypeChunk chunk,
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap,
        Entity brushEntity,
        float4 brushPosition,
        ref int totalCost)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray4 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        ErrorData error = new ErrorData();
        error.m_ErrorSeverity = ErrorSeverity.Error;
        error.m_ErrorType = ErrorType.NotEnoughMoney;
        error.m_Position = (float3) float.NaN;
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          if (nativeArray2[index].m_Cost > 0)
          {
            error.m_TempEntity = nativeArray1[index];
            // ISSUE: reference to a compiler-generated method
            this.ProcessError(error, iconMap, brushEntity, brushPosition, ref totalCost);
          }
        }
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          if (nativeArray2[index].m_Cost > 0)
          {
            error.m_TempEntity = nativeArray1[index];
            // ISSUE: reference to a compiler-generated method
            this.ProcessError(error, iconMap, brushEntity, brushPosition, ref totalCost);
          }
        }
      }

      private void ProcessQueue(
        NativeQueue<ErrorData> errorQueue,
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap,
        Entity brushEntity,
        float4 brushPosition,
        ref int totalCost)
      {
        ErrorData error;
        while (errorQueue.TryDequeue(out error))
        {
          // ISSUE: reference to a compiler-generated method
          this.ProcessError(error, iconMap, brushEntity, brushPosition, ref totalCost);
        }
      }

      private void ProcessError(
        ErrorData error,
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap,
        Entity brushEntity,
        float4 brushPosition,
        ref int totalCost)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!(this.m_ErrorPrefabs[(int) error.m_ErrorType] != Entity.Null) || this.CancelOptional(error, iconMap, brushEntity, brushPosition, ref totalCost))
          return;
        if (error.m_ErrorSeverity >= ErrorSeverity.Cancel)
        {
          Temp componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TempData.TryGetComponent(error.m_TempEntity, out componentData))
          {
            // ISSUE: reference to a compiler-generated method
            this.Cancel(error, componentData, iconMap, brushEntity, brushPosition, false, ref totalCost);
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TempData.TryGetComponent(error.m_PermanentEntity, out componentData))
            return;
          error.m_TempEntity = error.m_PermanentEntity;
          // ISSUE: reference to a compiler-generated method
          this.Cancel(error, componentData, iconMap, brushEntity, brushPosition, false, ref totalCost);
        }
        else
        {
          if (error.m_ErrorSeverity != ErrorSeverity.Override)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddIcon(error, iconMap, false);
          }
          if (error.m_TempEntity != Entity.Null && (error.m_ErrorSeverity >= ErrorSeverity.Error || error.m_PermanentEntity == Entity.Null && error.m_ErrorSeverity == ErrorSeverity.Override))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddError(error.m_TempEntity, error.m_ErrorSeverity);
          }
          if (!(error.m_PermanentEntity != Entity.Null) || error.m_ErrorSeverity < ErrorSeverity.Override)
            return;
          // ISSUE: reference to a compiler-generated method
          this.AddError(error.m_PermanentEntity, error.m_ErrorSeverity);
        }
      }

      private bool CancelOptional(
        ErrorData error,
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap,
        Entity brushEntity,
        float4 brushPosition,
        ref int totalCost)
      {
        Owner componentData1;
        for (; error.m_TempEntity != Entity.Null; error.m_TempEntity = componentData1.m_Owner)
        {
          Temp componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TempData.TryGetComponent(error.m_TempEntity, out componentData2) && (componentData2.m_Flags & TempFlags.Optional) != (TempFlags) 0)
          {
            bool addCancelledError = error.m_ErrorSeverity == ErrorSeverity.Error;
            if (addCancelledError)
            {
              ErrorData error1 = error;
              Owner componentData3;
              for (error1.m_TempEntity = error.m_PermanentEntity; error1.m_TempEntity != Entity.Null; error1.m_TempEntity = componentData3.m_Owner)
              {
                Temp componentData4;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.TryGetComponent(error1.m_TempEntity, out componentData4) && (componentData4.m_Flags & TempFlags.Optional) != (TempFlags) 0)
                {
                  addCancelledError = false;
                  // ISSUE: reference to a compiler-generated method
                  this.Cancel(error1, componentData4, iconMap, brushEntity, brushPosition, false, ref totalCost);
                  break;
                }
                // ISSUE: reference to a compiler-generated field
                if (!this.m_OwnerData.TryGetComponent(error1.m_TempEntity, out componentData3))
                  break;
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.Cancel(error, componentData2, iconMap, brushEntity, brushPosition, addCancelledError, ref totalCost);
            return true;
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.TryGetComponent(error.m_TempEntity, out componentData1))
            break;
        }
        return false;
      }

      private void Cancel(
        ErrorData error,
        Temp temp,
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap,
        Entity brushEntity,
        float4 brushPosition,
        bool addCancelledError,
        ref int totalCost)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.AddError(error.m_TempEntity, error.m_ErrorSeverity == ErrorSeverity.Error || error.m_ErrorSeverity == ErrorSeverity.CancelError ? ErrorSeverity.CancelError : ErrorSeverity.Cancel))
        {
          totalCost -= temp.m_Cost;
          temp.m_Flags |= TempFlags.Hidden | TempFlags.Cancel;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Temp>(error.m_TempEntity, temp);
        }
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(error.m_TempEntity, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            error.m_TempEntity = bufferData[index].m_SubObject;
            Temp componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.TryGetComponent(error.m_TempEntity, out componentData))
            {
              // ISSUE: reference to a compiler-generated method
              this.Cancel(error, componentData, iconMap, brushEntity, brushPosition, false, ref totalCost);
            }
          }
        }
        if (!addCancelledError || !(brushEntity != Entity.Null))
          return;
        if ((double) brushPosition.w != 0.0)
          brushPosition /= brushPosition.w;
        error.m_TempEntity = brushEntity;
        error.m_Position = brushPosition.xyz;
        // ISSUE: reference to a compiler-generated method
        this.AddIcon(error, iconMap, true);
      }

      private void AddIcon(
        ErrorData error,
        NativeHashMap<ValidationSystem.IconKey, ValidationSystem.IconValue> iconMap,
        bool cancelled)
      {
        // ISSUE: variable of a compiler-generated type
        ValidationSystem.IconKey key;
        // ISSUE: reference to a compiler-generated field
        key.m_Owner = error.m_TempEntity;
        // ISSUE: reference to a compiler-generated field
        key.m_Target = error.m_PermanentEntity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        key.m_Prefab = this.m_ErrorPrefabs[(int) error.m_ErrorType];
        // ISSUE: variable of a compiler-generated type
        ValidationSystem.IconValue iconValue1;
        // ISSUE: reference to a compiler-generated field
        iconValue1.m_Bounds = new Bounds3(error.m_Position, error.m_Position);
        // ISSUE: reference to a compiler-generated field
        iconValue1.m_Cancelled = cancelled;
        switch (error.m_ErrorSeverity)
        {
          case ErrorSeverity.Warning:
            // ISSUE: reference to a compiler-generated field
            iconValue1.m_Severity = IconPriority.Warning;
            break;
          case ErrorSeverity.Error:
            // ISSUE: reference to a compiler-generated field
            iconValue1.m_Severity = IconPriority.Error;
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            iconValue1.m_Severity = IconPriority.Info;
            break;
        }
        // ISSUE: variable of a compiler-generated type
        ValidationSystem.IconValue iconValue2;
        if (iconMap.TryGetValue(key, out iconValue2))
        {
          if (math.any(math.isnan(error.m_Position)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iconValue1.m_Bounds = iconValue2.m_Bounds;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!math.any(math.isnan(iconValue2.m_Bounds.min)))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iconValue1.m_Bounds |= iconValue2.m_Bounds;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          iconValue1.m_Severity = (IconPriority) math.max((int) iconValue1.m_Severity, (int) iconValue2.m_Severity);
          iconMap[key] = iconValue1;
        }
        else
          iconMap.Add(key, iconValue1);
      }

      private bool AddError(Entity entity, ErrorSeverity severity)
      {
        ErrorSeverity errorSeverity;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ErrorMap.TryGetValue(entity, out errorSeverity))
        {
          if (severity <= errorSeverity)
            return false;
          // ISSUE: reference to a compiler-generated field
          this.m_ErrorMap[entity] = severity;
          return errorSeverity < ErrorSeverity.Cancel;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ErrorMap.Add(entity, severity);
        return true;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Native> __Game_Common_Native_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Triangle> __Game_Areas_Triangle_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ToolErrorData> __Game_Prefabs_ToolErrorData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Brush> __Game_Tools_Brush_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlayerMoney> __Game_City_PlayerMoney_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferTypeHandle = state.GetBufferTypeHandle<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ToolErrorData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ToolErrorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Brush_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Brush>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_PlayerMoney_RO_ComponentLookup = state.GetComponentLookup<PlayerMoney>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
