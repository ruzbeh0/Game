// Decompiled with JetBrains decompiler
// Type: Game.Common.RaycastSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using Game.Zones;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Common
{
  [CompilerGenerated]
  public class RaycastSystem : GameSystemBase
  {
    private EntityQuery m_TerrainQuery;
    private EntityQuery m_LabelQuery;
    private EntityQuery m_IconQuery;
    private EntityQuery m_WaterSourceQuery;
    private Game.Zones.SearchSystem m_ZoneSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Objects.SearchSystem m_ObjectsSearchSystem;
    private Game.Routes.SearchSystem m_RouteSearchSystem;
    private IconClusterSystem m_IconClusterSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ToolSystem m_ToolSystem;
    private PreCullingSystem m_PreCullingSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private UpdateSystem m_UpdateSystem;
    private List<object> m_InputContext;
    private List<object> m_ResultContext;
    private NativeList<RaycastInput> m_Input;
    private NativeList<RaycastResult> m_Result;
    private JobHandle m_Dependencies;
    private bool m_Updating;
    private RaycastSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSearchSystem = this.World.GetOrCreateSystemManaged<Game.Zones.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectsSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteSearchSystem = this.World.GetOrCreateSystemManaged<Game.Routes.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconClusterSystem = this.World.GetOrCreateSystemManaged<IconClusterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainQuery = this.GetEntityQuery(ComponentType.ReadOnly<Terrain>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_LabelQuery = this.GetEntityQuery(ComponentType.ReadOnly<District>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_IconQuery = this.GetEntityQuery(ComponentType.ReadOnly<Icon>(), ComponentType.ReadOnly<DisallowCluster>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSourceQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Simulation.WaterSourceData>(), ComponentType.Exclude<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      this.EntityManager.CreateEntity((ComponentType) typeof (Terrain));
      // ISSUE: reference to a compiler-generated field
      this.m_InputContext = new List<object>(1);
      // ISSUE: reference to a compiler-generated field
      this.m_ResultContext = new List<object>(1);
      // ISSUE: reference to a compiler-generated field
      this.m_Input = new NativeList<RaycastInput>(1, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Result = new NativeList<RaycastResult>(1, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_Input.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Result.Dispose();
      base.OnDestroy();
    }

    public void AddInput(object context, RaycastInput input)
    {
      if (input.IsDisabled())
        input.m_TypeMask = TypeMask.None;
      // ISSUE: reference to a compiler-generated method
      this.CompleteRaycast();
      // ISSUE: reference to a compiler-generated field
      this.m_InputContext.Add(context);
      // ISSUE: reference to a compiler-generated field
      this.m_Input.Add(in input);
    }

    public NativeArray<RaycastResult> GetResult(object context)
    {
      // ISSUE: reference to a compiler-generated method
      this.CompleteRaycast();
      int start = -1;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ResultContext.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ResultContext[index] == context)
        {
          start = index;
          break;
        }
      }
      if (start == -1)
        return new NativeArray<RaycastResult>();
      int length = 1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (int index = start + 1; index < this.m_ResultContext.Count && this.m_ResultContext[index] == context; ++index)
        ++length;
      // ISSUE: reference to a compiler-generated field
      return this.m_Result.AsArray().GetSubArray(start, length);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.Raycast);
      // ISSUE: reference to a compiler-generated method
      this.CompleteRaycast();
      // ISSUE: reference to a compiler-generated field
      this.m_ResultContext.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ResultContext.AddRange((IEnumerable<object>) this.m_InputContext);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Result.ResizeUninitialized(this.m_Input.Length);
      // ISSUE: reference to a compiler-generated field
      NativeAccumulator<RaycastResult> accumulator = new NativeAccumulator<RaycastResult>(this.m_Input.Length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Dependencies = this.PerformRaycast(accumulator);
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_Dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RaycastSystem.RaycastResultJob jobData = new RaycastSystem.RaycastResultJob()
      {
        m_Accumulator = accumulator,
        m_Result = this.m_Result
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies = jobData.Schedule<RaycastSystem.RaycastResultJob>(this.m_Input.Length, 1, this.m_Dependencies);
      // ISSUE: reference to a compiler-generated field
      accumulator.Dispose(this.m_Dependencies);
      // ISSUE: reference to a compiler-generated field
      this.m_Updating = true;
    }

    private void CompleteRaycast()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Updating)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Updating = false;
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_InputContext.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Input.Clear();
    }

    private JobHandle PerformRaycast(NativeAccumulator<RaycastResult> accumulator)
    {
      Camera main = Camera.main;
      if ((UnityEngine.Object) main == (UnityEngine.Object) null)
        return new JobHandle();
      TypeMask typeMask = TypeMask.None;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Input.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        RaycastInput raycastInput = this.m_Input[index];
        typeMask |= raycastInput.m_TypeMask;
      }
      if (typeMask == TypeMask.None)
        return new JobHandle();
      int num1 = 2;
      float num2 = math.tan(math.radians(main.fieldOfView) * 0.5f);
      // ISSUE: reference to a compiler-generated field
      NativeArray<RaycastInput> nativeArray1 = this.m_Input.AsArray();
      NativeArray<RaycastResult> nativeArray2 = new NativeArray<RaycastResult>();
      JobHandle jobHandle1 = new JobHandle();
      JobHandle jobHandle2 = new JobHandle();
      JobHandle jobHandle3 = new JobHandle();
      JobHandle jobHandle4 = new JobHandle();
      JobHandle dependencies1 = new JobHandle();
      NativeList<RaycastSystem.EntityResult> list1 = new NativeList<RaycastSystem.EntityResult>();
      NativeList<RaycastSystem.EntityResult> list2 = new NativeList<RaycastSystem.EntityResult>();
      NativeList<PreCullingData> nativeList1 = new NativeList<PreCullingData>();
      TerrainHeightData terrainHeightData = new TerrainHeightData();
      if ((typeMask & (TypeMask.Zones | TypeMask.Areas | TypeMask.WaterSources)) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        nativeArray2 = new NativeArray<RaycastResult>(num1 * this.m_Input.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
      }
      if ((typeMask & (TypeMask.Terrain | TypeMask.Zones | TypeMask.Areas | TypeMask.Water | TypeMask.WaterSources)) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        terrainHeightData = this.m_TerrainSystem.GetHeightData();
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.RaycastTerrainJob jobData = new RaycastSystem.RaycastTerrainJob()
        {
          m_Input = nativeArray1,
          m_TerrainData = terrainHeightData,
          m_WaterData = this.m_WaterSystem.GetSurfaceData(out deps),
          m_TerrainEntity = this.m_TerrainQuery.GetSingletonEntity(),
          m_TerrainResults = nativeArray2,
          m_Results = accumulator.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        int3 resolution = jobData.m_TerrainData.resolution;
        int2 xz1 = resolution.xz;
        // ISSUE: reference to a compiler-generated field
        resolution = jobData.m_WaterData.resolution;
        int2 xz2 = resolution.xz;
        int2 int2 = xz1 / xz2;
        // ISSUE: reference to a compiler-generated field
        resolution = jobData.m_TerrainData.resolution;
        int2 xz3 = resolution.xz;
        // ISSUE: reference to a compiler-generated field
        resolution = jobData.m_WaterData.resolution;
        int2 actual = resolution.xz * int2;
        Assert.AreEqual<int2>(xz3, actual);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (jobData.m_TerrainData.isCreated && jobData.m_WaterData.isCreated)
        {
          jobHandle2 = jobData.Schedule<RaycastSystem.RaycastTerrainJob>(num1 * nativeArray1.Length, 1, JobHandle.CombineDependencies(this.Dependency, deps));
          jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TerrainSystem.AddCPUHeightReader(jobHandle2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_WaterSystem.AddSurfaceReader(jobHandle2);
        }
      }
      // ISSUE: variable of a compiler-generated type
      RaycastSystem.FindEntitiesFromTreeJob entitiesFromTreeJob;
      if ((typeMask & (TypeMask.MovingObjects | TypeMask.Net | TypeMask.Labels)) != TypeMask.None)
      {
        NativeQueue<RaycastSystem.EntityResult> nativeQueue = new NativeQueue<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        list1 = new NativeList<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: object of a compiler-generated type is created
        entitiesFromTreeJob = new RaycastSystem.FindEntitiesFromTreeJob();
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_TypeMask = TypeMask.MovingObjects | TypeMask.Net | TypeMask.Labels;
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_Input = nativeArray1;
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        entitiesFromTreeJob.m_SearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2);
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_EntityQueue = nativeQueue.AsParallelWriter();
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.FindEntitiesFromTreeJob jobData1 = entitiesFromTreeJob;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.DequeEntitiesJob jobData2 = new RaycastSystem.DequeEntitiesJob()
        {
          m_EntityQueue = nativeQueue,
          m_EntityList = list1
        };
        JobHandle jobHandle5 = jobData1.Schedule<RaycastSystem.FindEntitiesFromTreeJob>(nativeArray1.Length, 1, JobHandle.CombineDependencies(this.Dependency, dependencies2));
        JobHandle dependsOn = jobHandle5;
        jobHandle3 = jobData2.Schedule<RaycastSystem.DequeEntitiesJob>(dependsOn);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle5);
        nativeQueue.Dispose(jobHandle3);
      }
      // ISSUE: variable of a compiler-generated type
      RaycastSystem.DequeEntitiesJob dequeEntitiesJob;
      if ((typeMask & (TypeMask.StaticObjects | TypeMask.MovingObjects | TypeMask.Net)) != TypeMask.None)
      {
        NativeQueue<RaycastSystem.EntityResult> nativeQueue = new NativeQueue<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        list2 = new NativeList<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        nativeList1 = this.m_PreCullingSystem.GetCullingData(true, out dependencies1);
        // ISSUE: object of a compiler-generated type is created
        entitiesFromTreeJob = new RaycastSystem.FindEntitiesFromTreeJob();
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_TypeMask = TypeMask.StaticObjects | TypeMask.MovingObjects | TypeMask.Net;
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_Input = nativeArray1;
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        entitiesFromTreeJob.m_SearchTree = this.m_ObjectsSearchSystem.GetStaticSearchTree(true, out dependencies3);
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_EntityQueue = nativeQueue.AsParallelWriter();
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.FindEntitiesFromTreeJob jobData3 = entitiesFromTreeJob;
        // ISSUE: object of a compiler-generated type is created
        dequeEntitiesJob = new RaycastSystem.DequeEntitiesJob();
        // ISSUE: reference to a compiler-generated field
        dequeEntitiesJob.m_EntityQueue = nativeQueue;
        // ISSUE: reference to a compiler-generated field
        dequeEntitiesJob.m_EntityList = list2;
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.DequeEntitiesJob jobData4 = dequeEntitiesJob;
        JobHandle jobHandle6 = jobData3.Schedule<RaycastSystem.FindEntitiesFromTreeJob>(nativeArray1.Length, 1, JobHandle.CombineDependencies(this.Dependency, dependencies3));
        JobHandle dependsOn = jobHandle6;
        jobHandle4 = jobData4.Schedule<RaycastSystem.DequeEntitiesJob>(dependsOn);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectsSearchSystem.AddStaticSearchTreeReader(jobHandle6);
        nativeQueue.Dispose(jobHandle4);
      }
      if ((typeMask & TypeMask.MovingObjects) != TypeMask.None)
      {
        NativeQueue<RaycastSystem.EntityResult> nativeQueue = new NativeQueue<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<RaycastSystem.EntityResult> nativeList2 = new NativeList<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeArray<int4> nativeArray3 = new NativeArray<int4>(nativeArray1.Length, Allocator.TempJob);
        // ISSUE: object of a compiler-generated type is created
        entitiesFromTreeJob = new RaycastSystem.FindEntitiesFromTreeJob();
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_TypeMask = TypeMask.MovingObjects;
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_Input = nativeArray1;
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        entitiesFromTreeJob.m_SearchTree = this.m_ObjectsSearchSystem.GetMovingSearchTree(true, out dependencies4);
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_EntityQueue = nativeQueue.AsParallelWriter();
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.FindEntitiesFromTreeJob jobData5 = entitiesFromTreeJob;
        Game.Objects.RaycastJobs.GetSourceRangesJob jobData6 = new Game.Objects.RaycastJobs.GetSourceRangesJob()
        {
          m_EdgeList = list1,
          m_StaticObjectList = list2,
          m_Ranges = nativeArray3
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Objects.RaycastJobs.ExtractLaneObjectsJob jobData7 = new Game.Objects.RaycastJobs.ExtractLaneObjectsJob()
        {
          m_Input = nativeArray1,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
          m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
          m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
          m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_EdgeList = list1,
          m_StaticObjectList = list2,
          m_Ranges = nativeArray3,
          m_MovingObjectQueue = nativeQueue.AsParallelWriter()
        };
        // ISSUE: object of a compiler-generated type is created
        dequeEntitiesJob = new RaycastSystem.DequeEntitiesJob();
        // ISSUE: reference to a compiler-generated field
        dequeEntitiesJob.m_EntityQueue = nativeQueue;
        // ISSUE: reference to a compiler-generated field
        dequeEntitiesJob.m_EntityList = nativeList2;
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.DequeEntitiesJob jobData8 = dequeEntitiesJob;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshIndex_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshVertex_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SharedMeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Objects.RaycastJobs.RaycastMovingObjectsJob jobData9 = new Game.Objects.RaycastJobs.RaycastMovingObjectsJob()
        {
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
          m_Input = nativeArray1,
          m_ObjectList = nativeList2.AsDeferredJobArray(),
          m_CullingData = nativeList1,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
          m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
          m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
          m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_PrefabQuantityObjectData = this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup,
          m_PrefabMeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
          m_PrefabSharedMeshData = this.__TypeHandle.__Game_Prefabs_SharedMeshData_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
          m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
          m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup,
          m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup,
          m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
          m_Meshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
          m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
          m_Lods = this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup,
          m_Vertices = this.__TypeHandle.__Game_Prefabs_MeshVertex_RO_BufferLookup,
          m_Indices = this.__TypeHandle.__Game_Prefabs_MeshIndex_RO_BufferLookup,
          m_Nodes = this.__TypeHandle.__Game_Prefabs_MeshNode_RO_BufferLookup,
          m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
          m_Results = accumulator.AsParallelWriter()
        };
        JobHandle jobHandle7 = jobData5.Schedule<RaycastSystem.FindEntitiesFromTreeJob>(nativeArray1.Length, 1, JobHandle.CombineDependencies(this.Dependency, dependencies4));
        JobHandle job0 = jobData6.Schedule<Game.Objects.RaycastJobs.GetSourceRangesJob>(JobHandle.CombineDependencies(jobHandle3, jobHandle4));
        JobHandle jobHandle8 = jobData7.Schedule<Game.Objects.RaycastJobs.ExtractLaneObjectsJob>(nativeArray1.Length, 1, JobHandle.CombineDependencies(job0, jobHandle7));
        JobHandle jobHandle9 = jobData8.Schedule<RaycastSystem.DequeEntitiesJob>(jobHandle8);
        NativeList<RaycastSystem.EntityResult> list3 = nativeList2;
        JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle9, dependencies1);
        JobHandle jobHandle10 = jobData9.Schedule<Game.Objects.RaycastJobs.RaycastMovingObjectsJob, RaycastSystem.EntityResult>(list3, 1, dependsOn);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle10);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectsSearchSystem.AddMovingSearchTreeReader(jobHandle7);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PreCullingSystem.AddCullingDataReader(jobHandle10);
        nativeQueue.Dispose(jobHandle9);
        nativeList2.Dispose(jobHandle10);
        nativeArray3.Dispose(jobHandle8);
      }
      if ((typeMask & (TypeMask.StaticObjects | TypeMask.Net)) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshIndex_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshVertex_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SharedMeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ImpostorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_NetObject_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle11 = new Game.Objects.RaycastJobs.RaycastStaticObjectsJob()
        {
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
          m_Input = nativeArray1,
          m_Objects = list2.AsDeferredJobArray(),
          m_CullingData = nativeList1,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
          m_PlaceholderData = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup,
          m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
          m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
          m_NetObjectData = this.__TypeHandle.__Game_Objects_NetObject_RO_ComponentLookup,
          m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
          m_StackData = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup,
          m_SecondaryData = this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup,
          m_UnderConstructionData = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup,
          m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
          m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
          m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_PrefabGrowthScaleData = this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup,
          m_PrefabQuantityObjectData = this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup,
          m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_OverriddenData = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup,
          m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
          m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
          m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
          m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
          m_LotAreaData = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
          m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
          m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup,
          m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup,
          m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
          m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
          m_PrefabMeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
          m_PrefabImpostorData = this.__TypeHandle.__Game_Prefabs_ImpostorData_RO_ComponentLookup,
          m_PrefabSharedMeshData = this.__TypeHandle.__Game_Prefabs_SharedMeshData_RO_ComponentLookup,
          m_Meshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
          m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
          m_Lods = this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup,
          m_Vertices = this.__TypeHandle.__Game_Prefabs_MeshVertex_RO_BufferLookup,
          m_Indices = this.__TypeHandle.__Game_Prefabs_MeshIndex_RO_BufferLookup,
          m_Nodes = this.__TypeHandle.__Game_Prefabs_MeshNode_RO_BufferLookup,
          m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
          m_Results = accumulator.AsParallelWriter()
        }.Schedule<Game.Objects.RaycastJobs.RaycastStaticObjectsJob, RaycastSystem.EntityResult>(list2, 1, JobHandle.CombineDependencies(jobHandle4, dependencies1));
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle11);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PreCullingSystem.AddCullingDataReader(jobHandle11);
      }
      if ((typeMask & TypeMask.Net) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.RaycastJobs.RaycastEdgesJob jobData = new Game.Net.RaycastJobs.RaycastEdgesJob()
        {
          m_FovTan = num2,
          m_Input = nativeArray1,
          m_Edges = list1.AsDeferredJobArray(),
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
          m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
          m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
          m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
          m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
          m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
          m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
          m_PlaceholderData = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup,
          m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
          m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
          m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
          m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
          m_Results = accumulator.AsParallelWriter()
        };
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobData.Schedule<Game.Net.RaycastJobs.RaycastEdgesJob, RaycastSystem.EntityResult>(list1, 1, jobHandle3));
      }
      if ((typeMask & TypeMask.Zones) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        JobHandle jobHandle12 = new Game.Zones.RaycastJobs.FindZoneBlockJob()
        {
          m_Input = nativeArray1,
          m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
          m_Cells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
          m_SearchTree = this.m_ZoneSearchSystem.GetSearchTree(true, out dependencies5),
          m_TerrainResults = nativeArray2,
          m_Results = accumulator.AsParallelWriter()
        }.Schedule<Game.Zones.RaycastJobs.FindZoneBlockJob>(num1 * nativeArray1.Length, 1, JobHandle.CombineDependencies(jobHandle2, dependencies5));
        jobHandle2 = jobHandle12;
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle12);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ZoneSearchSystem.AddSearchTreeReader(jobHandle12);
      }
      if ((typeMask & TypeMask.Areas) != TypeMask.None)
      {
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
        this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle13 = new Game.Areas.RaycastJobs.FindAreaJob()
        {
          m_Input = nativeArray1,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_SpaceData = this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup,
          m_PlaceholderData = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup,
          m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
          m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabAreaData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
          m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
          m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
          m_SearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies6),
          m_TerrainResults = nativeArray2,
          m_Results = accumulator.AsParallelWriter()
        }.Schedule<Game.Areas.RaycastJobs.FindAreaJob>((num1 + 1) * nativeArray1.Length, 1, JobHandle.CombineDependencies(jobHandle2, dependencies6));
        jobHandle2 = jobHandle13;
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle13);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle13);
      }
      if ((typeMask & (TypeMask.RouteWaypoints | TypeMask.RouteSegments)) != TypeMask.None)
      {
        NativeList<Game.Routes.RaycastJobs.RouteItem> nativeList3 = new NativeList<Game.Routes.RaycastJobs.RouteItem>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        JobHandle dependencies7;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Game.Routes.RaycastJobs.FindRoutesFromTreeJob jobData10 = new Game.Routes.RaycastJobs.FindRoutesFromTreeJob()
        {
          m_Input = nativeArray1,
          m_SearchTree = this.m_RouteSearchSystem.GetSearchTree(true, out dependencies7),
          m_RouteList = nativeList3
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_HiddenRoute_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Routes.RaycastJobs.RaycastRoutesJob jobData11 = new Game.Routes.RaycastJobs.RaycastRoutesJob()
        {
          m_Input = nativeArray1,
          m_Routes = nativeList3.AsDeferredJobArray(),
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabRouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
          m_PrefabTransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
          m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
          m_SegmentData = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup,
          m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
          m_HiddenRouteData = this.__TypeHandle.__Game_Routes_HiddenRoute_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_CurveElements = this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup,
          m_Results = accumulator.AsParallelWriter()
        };
        JobHandle jobHandle14 = jobData10.Schedule<Game.Routes.RaycastJobs.FindRoutesFromTreeJob>(JobHandle.CombineDependencies(this.Dependency, dependencies7));
        NativeList<Game.Routes.RaycastJobs.RouteItem> list4 = nativeList3;
        JobHandle dependsOn = jobHandle14;
        JobHandle jobHandle15 = jobData11.Schedule<Game.Routes.RaycastJobs.RaycastRoutesJob, Game.Routes.RaycastJobs.RouteItem>(list4, 1, dependsOn);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle15);
        nativeList3.Dispose(jobHandle15);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_RouteSearchSystem.AddSearchTreeReader(jobHandle14);
      }
      if ((typeMask & TypeMask.Lanes) != TypeMask.None)
      {
        NativeQueue<RaycastSystem.EntityResult> nativeQueue = new NativeQueue<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<RaycastSystem.EntityResult> nativeList4 = new NativeList<RaycastSystem.EntityResult>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: object of a compiler-generated type is created
        entitiesFromTreeJob = new RaycastSystem.FindEntitiesFromTreeJob();
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_LaneExpandFovTan = num2;
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_TypeMask = TypeMask.Lanes;
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_Input = nativeArray1;
        JobHandle dependencies8;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        entitiesFromTreeJob.m_SearchTree = this.m_NetSearchSystem.GetLaneSearchTree(true, out dependencies8);
        // ISSUE: reference to a compiler-generated field
        entitiesFromTreeJob.m_EntityQueue = nativeQueue.AsParallelWriter();
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.FindEntitiesFromTreeJob jobData12 = entitiesFromTreeJob;
        // ISSUE: object of a compiler-generated type is created
        dequeEntitiesJob = new RaycastSystem.DequeEntitiesJob();
        // ISSUE: reference to a compiler-generated field
        dequeEntitiesJob.m_EntityQueue = nativeQueue;
        // ISSUE: reference to a compiler-generated field
        dequeEntitiesJob.m_EntityList = nativeList4;
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.DequeEntitiesJob jobData13 = dequeEntitiesJob;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.RaycastJobs.RaycastLanesJob jobData14 = new Game.Net.RaycastJobs.RaycastLanesJob()
        {
          m_FovTan = num2,
          m_Input = nativeArray1,
          m_Lanes = nativeList4.AsDeferredJobArray(),
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
          m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
          m_Results = accumulator.AsParallelWriter()
        };
        JobHandle jobHandle16 = jobData12.Schedule<RaycastSystem.FindEntitiesFromTreeJob>(nativeArray1.Length, 1, JobHandle.CombineDependencies(this.Dependency, dependencies8));
        JobHandle inputDeps = jobData13.Schedule<RaycastSystem.DequeEntitiesJob>(jobHandle16);
        NativeList<RaycastSystem.EntityResult> list5 = nativeList4;
        JobHandle dependsOn = inputDeps;
        JobHandle jobHandle17 = jobData14.Schedule<Game.Net.RaycastJobs.RaycastLanesJob, RaycastSystem.EntityResult>(list5, 1, dependsOn);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle17);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetSearchSystem.AddLaneSearchTreeReader(jobHandle16);
        nativeQueue.Dispose(inputDeps);
        nativeList4.Dispose(jobHandle17);
      }
      if ((typeMask & TypeMask.Labels) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_LabelExtents_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Areas.RaycastJobs.RaycastLabelsJob jobData15 = new Game.Areas.RaycastJobs.RaycastLabelsJob()
        {
          m_Input = nativeArray1,
          m_CameraRight = (float3) main.transform.right,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_GeometryType = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle,
          m_LabelExtentsType = this.__TypeHandle.__Game_Areas_LabelExtents_RO_BufferTypeHandle,
          m_Results = accumulator.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LabelPosition_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LabelExtents_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.RaycastJobs.RaycastLabelsJob jobData16 = new Game.Net.RaycastJobs.RaycastLabelsJob()
        {
          m_Input = nativeArray1,
          m_CameraRight = (float3) main.transform.right,
          m_Edges = list1.AsDeferredJobArray(),
          m_AggregatedData = this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentLookup,
          m_LabelExtentsData = this.__TypeHandle.__Game_Net_LabelExtents_RO_ComponentLookup,
          m_LabelPositions = this.__TypeHandle.__Game_Net_LabelPosition_RO_BufferLookup,
          m_Results = accumulator.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        jobHandle1 = JobHandle.CombineDependencies(JobHandle.CombineDependencies(jobHandle1, jobData15.ScheduleParallel<Game.Areas.RaycastJobs.RaycastLabelsJob>(this.m_LabelQuery, this.Dependency)), jobData16.Schedule<Game.Net.RaycastJobs.RaycastLabelsJob, RaycastSystem.EntityResult>(list1, 1, jobHandle3));
      }
      if ((typeMask & (TypeMask.StaticObjects | TypeMask.MovingObjects | TypeMask.Icons)) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Object_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        JobHandle dependencies9;
        JobHandle outJobHandle;
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
        Game.Notifications.RaycastJobs.RaycastIconsJob jobData = new Game.Notifications.RaycastJobs.RaycastIconsJob()
        {
          m_Input = nativeArray1,
          m_CameraUp = (float3) main.transform.up,
          m_CameraRight = (float3) main.transform.right,
          m_ClusterData = this.m_IconClusterSystem.GetIconClusterData(true, out dependencies9),
          m_IconChunks = this.m_IconQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_IconType = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_StaticData = this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup,
          m_ObjectData = this.__TypeHandle.__Game_Objects_Object_RO_ComponentLookup,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_PlaceholderData = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup,
          m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
          m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
          m_IconDisplayData = this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup,
          m_Results = accumulator.AsParallelWriter()
        };
        JobHandle jobHandle18 = jobData.Schedule<Game.Notifications.RaycastJobs.RaycastIconsJob>(nativeArray1.Length, 1, JobHandle.CombineDependencies(this.Dependency, dependencies9, outJobHandle));
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle18);
        jobData.m_IconChunks.Dispose(jobHandle18);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IconClusterSystem.AddIconClusterReader(jobHandle18);
      }
      if ((typeMask & TypeMask.WaterSources) != TypeMask.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        JobHandle jobHandle19 = new RaycastSystem.RaycastWaterSourcesJob()
        {
          m_Input = nativeArray1,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_WaterSourceDataType = this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle,
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_TerrainHeightData = terrainHeightData,
          m_PositionOffset = this.m_TerrainSystem.positionOffset,
          m_TerrainResults = nativeArray2,
          m_Results = accumulator.AsParallelWriter()
        }.ScheduleParallel<RaycastSystem.RaycastWaterSourcesJob>(this.m_WaterSourceQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle2));
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle19);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TerrainSystem.AddCPUHeightReader(jobHandle19);
      }
      if ((typeMask & (TypeMask.Zones | TypeMask.Areas | TypeMask.WaterSources)) != TypeMask.None)
        nativeArray2.Dispose(jobHandle1);
      if ((typeMask & (TypeMask.MovingObjects | TypeMask.Net | TypeMask.Labels)) != TypeMask.None)
        list1.Dispose(jobHandle1);
      if ((typeMask & (TypeMask.StaticObjects | TypeMask.MovingObjects | TypeMask.Net)) != TypeMask.None)
        list2.Dispose(jobHandle1);
      return jobHandle1;
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
    public RaycastSystem()
    {
    }

    public struct EntityResult
    {
      public Entity m_Entity;
      public int m_RaycastIndex;
    }

    [BurstCompile]
    private struct FindEntitiesFromTreeJob : IJobParallelFor
    {
      [ReadOnly]
      public float m_LaneExpandFovTan;
      [ReadOnly]
      public TypeMask m_TypeMask;
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      public NativeQueue<RaycastSystem.EntityResult>.ParallelWriter m_EntityQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        RaycastInput raycastInput = this.m_Input[index];
        // ISSUE: reference to a compiler-generated field
        if ((raycastInput.m_TypeMask & this.m_TypeMask) == TypeMask.None)
          return;
        // ISSUE: reference to a compiler-generated field
        float minLaneRadius = Game.Net.RaycastJobs.GetMinLaneRadius(this.m_LaneExpandFovTan, MathUtils.Length(raycastInput.m_Line));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.FindEntitiesFromTreeJob.FindEntitiesIterator iterator = new RaycastSystem.FindEntitiesFromTreeJob.FindEntitiesIterator()
        {
          m_Line = raycastInput.m_Line,
          m_MinOffset = math.min(-raycastInput.m_Offset, (float3) -minLaneRadius),
          m_MaxOffset = math.max(-raycastInput.m_Offset, (float3) minLaneRadius),
          m_EntityQueue = this.m_EntityQueue,
          m_RaycastIndex = index
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<RaycastSystem.FindEntitiesFromTreeJob.FindEntitiesIterator>(ref iterator);
      }

      private struct FindEntitiesIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Line3.Segment m_Line;
        public float3 m_MinOffset;
        public float3 m_MaxOffset;
        public NativeQueue<RaycastSystem.EntityResult>.ParallelWriter m_EntityQueue;
        public int m_RaycastIndex;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          bounds.m_Bounds.min += this.m_MinOffset;
          // ISSUE: reference to a compiler-generated field
          bounds.m_Bounds.max += this.m_MaxOffset;
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          bounds.m_Bounds.min += this.m_MinOffset;
          // ISSUE: reference to a compiler-generated field
          bounds.m_Bounds.max += this.m_MaxOffset;
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_EntityQueue.Enqueue(new RaycastSystem.EntityResult()
          {
            m_Entity = entity,
            m_RaycastIndex = this.m_RaycastIndex
          });
        }
      }
    }

    [BurstCompile]
    private struct DequeEntitiesJob : IJob
    {
      public NativeQueue<RaycastSystem.EntityResult> m_EntityQueue;
      public NativeList<RaycastSystem.EntityResult> m_EntityList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EntityList.ResizeUninitialized(this.m_EntityQueue.Count);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_EntityList.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EntityList[index] = this.m_EntityQueue.Dequeue();
        }
      }
    }

    [BurstCompile]
    private struct RaycastTerrainJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public TerrainHeightData m_TerrainData;
      [ReadOnly]
      public WaterSurfaceData m_WaterData;
      [ReadOnly]
      public Entity m_TerrainEntity;
      [NativeDisableContainerSafetyRestriction]
      public NativeArray<RaycastResult> m_TerrainResults;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        int num = index / this.m_Input.Length;
        // ISSUE: reference to a compiler-generated field
        int index1 = index - num * this.m_Input.Length;
        // ISSUE: reference to a compiler-generated field
        RaycastInput raycastInput = this.m_Input[index1];
        RaycastResult raycastResult = new RaycastResult();
        Line3.Segment segment = raycastInput.m_Line + raycastInput.m_Offset;
        bool outside = (raycastInput.m_Flags & RaycastFlags.Outside) > (RaycastFlags) 0;
        switch (num)
        {
          case 0:
            float t1;
            float3 normal;
            // ISSUE: reference to a compiler-generated field
            if ((raycastInput.m_TypeMask & (TypeMask.Terrain | TypeMask.Zones | TypeMask.Areas | TypeMask.WaterSources)) != TypeMask.None && TerrainUtils.Raycast(ref this.m_TerrainData, segment, outside, out t1, out normal))
            {
              // ISSUE: reference to a compiler-generated field
              raycastResult.m_Owner = this.m_TerrainEntity;
              raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
              raycastResult.m_Hit.m_Position = MathUtils.Position(segment, t1);
              raycastResult.m_Hit.m_HitPosition = raycastResult.m_Hit.m_Position;
              raycastResult.m_Hit.m_HitDirection = normal;
              raycastResult.m_Hit.m_NormalizedDistance = t1 + 1f / math.max(1f, MathUtils.Length(segment));
              if ((raycastInput.m_TypeMask & TypeMask.Terrain) != TypeMask.None)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Results.Accumulate(index1, raycastResult);
                break;
              }
              break;
            }
            break;
          case 1:
            float t2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((raycastInput.m_TypeMask & (TypeMask.Areas | TypeMask.Water)) != TypeMask.None && WaterUtils.Raycast(ref this.m_WaterData, ref this.m_TerrainData, segment, outside, out t2))
            {
              // ISSUE: reference to a compiler-generated field
              raycastResult.m_Owner = this.m_TerrainEntity;
              raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
              raycastResult.m_Hit.m_Position = MathUtils.Position(segment, t2);
              raycastResult.m_Hit.m_HitPosition = raycastResult.m_Hit.m_Position;
              raycastResult.m_Hit.m_NormalizedDistance = t2 + 1f / math.max(1f, MathUtils.Length(segment));
              if ((raycastInput.m_TypeMask & TypeMask.Water) != TypeMask.None)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Results.Accumulate(index1, raycastResult);
                break;
              }
              break;
            }
            break;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TerrainResults.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainResults[index] = raycastResult;
      }
    }

    [BurstCompile]
    private struct RaycastWaterSourcesJob : IJobChunk
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Simulation.WaterSourceData> m_WaterSourceDataType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public float3 m_PositionOffset;
      [ReadOnly]
      public NativeArray<RaycastResult> m_TerrainResults;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Simulation.WaterSourceData> nativeArray2 = chunk.GetNativeArray<Game.Simulation.WaterSourceData>(ref this.m_WaterSourceDataType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Input.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          RaycastInput input = this.m_Input[index1];
          if ((input.m_TypeMask & TypeMask.WaterSources) != TypeMask.None)
          {
            Line3.Segment line = input.m_Line + input.m_Offset;
            RaycastResult raycastResult = new RaycastResult();
            // ISSUE: reference to a compiler-generated field
            if (this.m_TerrainResults.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              raycastResult = this.m_TerrainResults[index1];
            }
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              Game.Simulation.WaterSourceData waterSourceData = nativeArray2[index2];
              Game.Objects.Transform transform = nativeArray3[index2];
              // ISSUE: reference to a compiler-generated field
              transform.m_Position.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, transform.m_Position);
              float3 position = transform.m_Position;
              if (waterSourceData.m_ConstantDepth > 0)
              {
                // ISSUE: reference to a compiler-generated field
                position.y = this.m_PositionOffset.y + waterSourceData.m_Amount;
              }
              else
                position.y += waterSourceData.m_Amount;
              float t;
              if (MathUtils.Intersect(line.y, position.y, out t))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckHit(index1, input, entity, waterSourceData.m_Radius, transform.m_Position, MathUtils.Position(line, t));
              }
              if (raycastResult.m_Owner != Entity.Null && (double) raycastResult.m_Hit.m_HitPosition.y > (double) position.y)
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckHit(index1, input, entity, waterSourceData.m_Radius, transform.m_Position, raycastResult.m_Hit.m_HitPosition);
              }
            }
          }
        }
      }

      private void CheckHit(
        int raycastIndex,
        RaycastInput input,
        Entity entity,
        float radius,
        float3 position,
        float3 hitPosition)
      {
        float num = math.distance(hitPosition.xz, position.xz);
        if ((double) num >= (double) radius)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Results.Accumulate(raycastIndex, new RaycastResult()
        {
          m_Owner = entity,
          m_Hit = {
            m_HitEntity = entity,
            m_Position = position,
            m_HitPosition = hitPosition,
            m_HitDirection = math.up(),
            m_NormalizedDistance = (radius + num) * math.max(1f, math.distance(hitPosition, input.m_Line.a + input.m_Offset))
          }
        });
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
    private struct RaycastResultJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeAccumulator<RaycastResult> m_Accumulator;
      [NativeDisableParallelForRestriction]
      public NativeList<RaycastResult> m_Result;

      public void Execute(int index) => this.m_Result[index] = this.m_Accumulator.GetResult(index);
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> __Game_Prefabs_QuantityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SharedMeshData> __Game_Prefabs_SharedMeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Bone> __Game_Rendering_Bone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LodMesh> __Game_Prefabs_LodMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshVertex> __Game_Prefabs_MeshVertex_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshIndex> __Game_Prefabs_MeshIndex_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshNode> __Game_Prefabs_MeshNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Placeholder> __Game_Objects_Placeholder_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.NetObject> __Game_Objects_NetObject_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stack> __Game_Objects_Stack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Secondary> __Game_Objects_Secondary_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> __Game_Prefabs_GrowthScaleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Overridden> __Game_Common_Overridden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ImpostorData> __Game_Prefabs_ImpostorData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Space> __Game_Areas_Space_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Segment> __Game_Routes_Segment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HiddenRoute> __Game_Routes_HiddenRoute_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CurveElement> __Game_Routes_CurveElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Geometry> __Game_Areas_Geometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.LabelExtents> __Game_Areas_LabelExtents_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Aggregated> __Game_Net_Aggregated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LabelExtents> __Game_Net_LabelExtents_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LabelPosition> __Game_Net_LabelPosition_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Static> __Game_Objects_Static_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Object> __Game_Objects_Object_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> __Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Simulation.WaterSourceData> __Game_Simulation_WaterSourceData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup = state.GetComponentLookup<QuantityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SharedMeshData_RO_ComponentLookup = state.GetComponentLookup<SharedMeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RO_BufferLookup = state.GetBufferLookup<Skeleton>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Bone_RO_BufferLookup = state.GetBufferLookup<Bone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LodMesh_RO_BufferLookup = state.GetBufferLookup<LodMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshVertex_RO_BufferLookup = state.GetBufferLookup<MeshVertex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshIndex_RO_BufferLookup = state.GetBufferLookup<MeshIndex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshNode_RO_BufferLookup = state.GetBufferLookup<MeshNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Placeholder_RO_ComponentLookup = state.GetComponentLookup<Placeholder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_NetObject_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.NetObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentLookup = state.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Secondary_RO_ComponentLookup = state.GetComponentLookup<Secondary>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentLookup = state.GetComponentLookup<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup = state.GetComponentLookup<GrowthScaleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentLookup = state.GetComponentLookup<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ImpostorData_RO_ComponentLookup = state.GetComponentLookup<ImpostorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Space_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Space>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_HiddenRoute_RO_ComponentLookup = state.GetComponentLookup<HiddenRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveElement_RO_BufferLookup = state.GetBufferLookup<CurveElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetLaneGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_LabelExtents_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.LabelExtents>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Aggregated_RO_ComponentLookup = state.GetComponentLookup<Aggregated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelExtents_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LabelExtents>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelPosition_RO_BufferLookup = state.GetBufferLookup<LabelPosition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentLookup = state.GetComponentLookup<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Object_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup = state.GetComponentLookup<NotificationIconDisplayData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Simulation.WaterSourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
      }
    }
  }
}
