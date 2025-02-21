// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateObjectsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Effects;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateObjectsSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private SimulationSystem m_SimulationSystem;
    private ModificationBarrier1 m_ModificationBarrier;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_DeletedQuery;
    private EntityQuery m_EconomyParameterQuery;
    private ComponentTypeSet m_SubTypes;
    private ComponentTypeSet m_StoppedUpdateFrameTypes;
    private NativeHashMap<OwnerDefinition, Entity> m_ReusedOwnerMap;
    private JobHandle m_OwnerMapReadDeps;
    private JobHandle m_OwnerMapWriteDeps;
    private GenerateObjectsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ReusedOwnerMap = new NativeHashMap<OwnerDefinition, Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<CreationDefinition>(),
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<ObjectDefinition>(),
          ComponentType.ReadOnly<NetCourse>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.Object>(), ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubTypes = new ComponentTypeSet(ComponentType.ReadWrite<Game.Objects.SubObject>(), ComponentType.ReadWrite<Game.Net.SubNet>(), ComponentType.ReadWrite<Game.Areas.SubArea>());
      // ISSUE: reference to a compiler-generated field
      this.m_StoppedUpdateFrameTypes = new ComponentTypeSet(ComponentType.ReadWrite<Stopped>(), ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<ParkedTrain>(), ComponentType.ReadWrite<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapReadDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapWriteDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReusedOwnerMap.Dispose();
      base.OnDestroy();
    }

    public NativeHashMap<OwnerDefinition, Entity> GetReusedOwnerMap(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_OwnerMapWriteDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_ReusedOwnerMap;
    }

    public void AddOwnerMapReader(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapReadDeps = JobHandle.CombineDependencies(this.m_OwnerMapReadDeps, dependencies);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapReadDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapWriteDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReusedOwnerMap.Clear();
      base.OnStopRunning();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<GenerateObjectsSystem.CreationData> nativeQueue = new NativeQueue<GenerateObjectsSystem.CreationData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<GenerateObjectsSystem.CreationData> nativeList = new NativeList<GenerateObjectsSystem.CreationData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelMultiHashMap<GenerateObjectsSystem.OldObjectKey, GenerateObjectsSystem.OldObjectValue> parallelMultiHashMap = new NativeParallelMultiHashMap<GenerateObjectsSystem.OldObjectKey, GenerateObjectsSystem.OldObjectValue>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapReadDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapWriteDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReusedOwnerMap.Clear();
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
      this.__TypeHandle.__Game_Net_Roundabout_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_ObjectDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateObjectsSystem.FillCreationListJob jobData1 = new GenerateObjectsSystem.FillCreationListJob()
      {
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_OwnerDefinitionType = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle,
        m_ObjectDefinitionType = this.__TypeHandle.__Game_Tools_ObjectDefinition_RO_ComponentTypeHandle,
        m_NetCourseType = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabLocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LocalConnectData = this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup,
        m_RoundaboutData = this.__TypeHandle.__Game_Net_Roundabout_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_CreationQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateObjectsSystem.FillOldObjectsJob jobData2 = new GenerateObjectsSystem.FillOldObjectsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_OldObjectMap = parallelMultiHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateObjectsSystem.CollectCreationDataJob jobData3 = new GenerateObjectsSystem.CollectCreationDataJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_CreationQueue = nativeQueue,
        m_CreationList = nativeList,
        m_OldObjectMap = parallelMultiHashMap,
        m_ReusedOwnerMap = this.m_ReusedOwnerMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MovingObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GenerateObjectsSystem.CreateObjectsJob jobData4 = new GenerateObjectsSystem.CreateObjectsJob()
      {
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_SubTypes = this.m_SubTypes,
        m_StoppedUpdateFrameTypes = this.m_StoppedUpdateFrameTypes,
        m_CreationList = nativeList.AsDeferredJobArray(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_StoppedData = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_RecentData = this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_SurfaceData = this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup,
        m_StackData = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup,
        m_UnderConstructionData = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_ServiceUpgradeData = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup,
        m_ObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_MovingObjectData = this.__TypeHandle.__Game_Prefabs_MovingObjectData_RO_ComponentLookup,
        m_NetObjectData = this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup,
        m_PrefabTreeData = this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabEffectData = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<GenerateObjectsSystem.FillCreationListJob>(this.m_DefinitionQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      JobHandle job1 = jobData2.Schedule<GenerateObjectsSystem.FillOldObjectsJob>(this.m_DeletedQuery, this.Dependency);
      JobHandle inputDeps = jobData3.Schedule<GenerateObjectsSystem.CollectCreationDataJob>(JobHandle.CombineDependencies(jobHandle1, job1));
      NativeList<GenerateObjectsSystem.CreationData> list = nativeList;
      JobHandle dependsOn = inputDeps;
      JobHandle jobHandle2 = jobData4.Schedule<GenerateObjectsSystem.CreateObjectsJob, GenerateObjectsSystem.CreationData>(list, 1, dependsOn);
      nativeQueue.Dispose(inputDeps);
      nativeList.Dispose(jobHandle2);
      parallelMultiHashMap.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerMapWriteDeps = inputDeps;
      this.Dependency = jobHandle2;
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
    public GenerateObjectsSystem()
    {
    }

    private struct CreationData : IComparable<GenerateObjectsSystem.CreationData>
    {
      public CreationDefinition m_CreationDefinition;
      public OwnerDefinition m_OwnerDefinition;
      public ObjectDefinition m_ObjectDefinition;
      public Entity m_OldEntity;
      public bool m_HasDefinition;

      public CreationData(
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        ObjectDefinition objectDefinition,
        bool hasDefinition)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CreationDefinition = creationDefinition;
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerDefinition = ownerDefinition;
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectDefinition = objectDefinition;
        // ISSUE: reference to a compiler-generated field
        this.m_OldEntity = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_HasDefinition = hasDefinition;
      }

      public int CompareTo(GenerateObjectsSystem.CreationData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_CreationDefinition.m_Original.Index - other.m_CreationDefinition.m_Original.Index;
      }
    }

    private struct OldObjectKey : IEquatable<GenerateObjectsSystem.OldObjectKey>
    {
      public Entity m_Prefab;
      public Entity m_SubPrefab;
      public Entity m_Original;
      public Entity m_Owner;

      public bool Equals(GenerateObjectsSystem.OldObjectKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Prefab.Equals(other.m_Prefab) && this.m_SubPrefab.Equals(other.m_SubPrefab) && this.m_Original.Equals(other.m_Original) && this.m_Owner.Equals(other.m_Owner);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (((17 * 31 + this.m_Prefab.GetHashCode()) * 31 + this.m_SubPrefab.GetHashCode()) * 31 + this.m_Original.GetHashCode()) * 31 + this.m_Owner.GetHashCode();
      }
    }

    private struct OldObjectValue
    {
      public Entity m_Entity;
      public Transform m_Transform;
    }

    [BurstCompile]
    private struct FillOldObjectsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainer> m_EditorContainerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public NativeParallelMultiHashMap<GenerateObjectsSystem.OldObjectKey, GenerateObjectsSystem.OldObjectValue> m_OldObjectMap;

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
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EditorContainer> nativeArray5 = chunk.GetNativeArray<EditorContainer>(ref this.m_EditorContainerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          GenerateObjectsSystem.OldObjectKey key;
          // ISSUE: reference to a compiler-generated field
          key.m_Prefab = nativeArray6[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          key.m_SubPrefab = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          key.m_Original = nativeArray3[index].m_Original;
          // ISSUE: reference to a compiler-generated field
          key.m_Owner = Entity.Null;
          if (nativeArray5.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            key.m_SubPrefab = nativeArray5[index].m_Prefab;
          }
          if (nativeArray2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            key.m_Owner = nativeArray2[index].m_Owner;
          }
          // ISSUE: variable of a compiler-generated type
          GenerateObjectsSystem.OldObjectValue oldObjectValue;
          // ISSUE: reference to a compiler-generated field
          oldObjectValue.m_Entity = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          oldObjectValue.m_Transform = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          this.m_OldObjectMap.Add(key, oldObjectValue);
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
    private struct FillCreationListJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> m_OwnerDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<ObjectDefinition> m_ObjectDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> m_NetCourseType;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_ObjectData;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_PrefabLocalConnectData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_RoadData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<LocalConnect> m_LocalConnectData;
      [ReadOnly]
      public ComponentLookup<Roundabout> m_RoundaboutData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public NativeQueue<GenerateObjectsSystem.CreationData>.ParallelWriter m_CreationQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<OwnerDefinition> nativeArray2 = chunk.GetNativeArray<OwnerDefinition>(ref this.m_OwnerDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectDefinition> nativeArray3 = chunk.GetNativeArray<ObjectDefinition>(ref this.m_ObjectDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetCourse> nativeArray4 = chunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          ObjectDefinition objectDefinition = nativeArray3[index];
          CreationDefinition creationDefinition = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(creationDefinition.m_Owner))
          {
            OwnerDefinition ownerDefinition = new OwnerDefinition();
            if (nativeArray2.Length != 0)
              ownerDefinition = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectData.HasComponent(creationDefinition.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_CreationQueue.Enqueue(new GenerateObjectsSystem.CreationData(creationDefinition, ownerDefinition, objectDefinition, true));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(creationDefinition.m_Original) && this.m_ObjectData.HasComponent(this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_CreationQueue.Enqueue(new GenerateObjectsSystem.CreationData(creationDefinition, ownerDefinition, objectDefinition, true));
              }
            }
          }
        }
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          CreationDefinition creationDefinition = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(creationDefinition.m_Owner) && (creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
          {
            NetCourse netCourse = nativeArray4[index];
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(netCourse.m_StartPosition.m_Entity);
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(netCourse.m_EndPosition.m_Entity);
            Entity original = Entity.Null;
            if ((creationDefinition.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
              original = creationDefinition.m_Original;
            if (netCourse.m_StartPosition.m_Entity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckConnectedEdges(netCourse.m_StartPosition.m_Entity, original);
            }
            if (netCourse.m_EndPosition.m_Entity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckConnectedEdges(netCourse.m_EndPosition.m_Entity, original);
            }
            bool isStandalone = nativeArray2.Length == 0;
            if (creationDefinition.m_Prefab != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckEdgesForLocalConnectOrAttachment(netCourse.m_StartPosition.m_Flags, netCourse.m_StartPosition.m_Entity, netCourse.m_StartPosition.m_Position, netCourse.m_StartPosition.m_Elevation, creationDefinition.m_Prefab, isStandalone);
              if (!netCourse.m_StartPosition.m_Position.Equals(netCourse.m_EndPosition.m_Position))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckEdgesForLocalConnectOrAttachment(netCourse.m_EndPosition.m_Flags, netCourse.m_EndPosition.m_Entity, netCourse.m_EndPosition.m_Position, netCourse.m_EndPosition.m_Elevation, creationDefinition.m_Prefab, isStandalone);
                // ISSUE: reference to a compiler-generated method
                this.CheckNodesForLocalConnect(MathUtils.Cut(netCourse.m_Curve, new float2(netCourse.m_StartPosition.m_CourseDelta, netCourse.m_EndPosition.m_CourseDelta)), creationDefinition.m_Prefab, isStandalone);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(creationDefinition.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                Entity prefab = this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab;
                // ISSUE: reference to a compiler-generated method
                this.CheckEdgesForLocalConnectOrAttachment(netCourse.m_StartPosition.m_Flags, netCourse.m_StartPosition.m_Entity, netCourse.m_StartPosition.m_Position, netCourse.m_StartPosition.m_Elevation, prefab, isStandalone);
                if (!netCourse.m_StartPosition.m_Position.Equals(netCourse.m_EndPosition.m_Position))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckEdgesForLocalConnectOrAttachment(netCourse.m_EndPosition.m_Flags, netCourse.m_EndPosition.m_Entity, netCourse.m_EndPosition.m_Position, netCourse.m_EndPosition.m_Elevation, prefab, isStandalone);
                }
              }
            }
          }
        }
      }

      private void CheckConnectedEdges(Entity entity, Entity deleteEdge)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_EdgeData[entity];
          // ISSUE: reference to a compiler-generated method
          this.CheckSubObjects(edge.m_Start);
          // ISSUE: reference to a compiler-generated method
          this.CheckSubObjects(edge.m_End);
          // ISSUE: reference to a compiler-generated method
          this.CheckNodeEdges(edge.m_Start, edge.m_End);
          // ISSUE: reference to a compiler-generated method
          this.CheckNodeEdges(edge.m_End, edge.m_Start);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[entity];
          for (int index = 0; index < connectedNode.Length; ++index)
          {
            Entity node = connectedNode[index].m_Node;
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(node);
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeEdges(node);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NodeData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[entity];
          if (deleteEdge != Entity.Null && connectedEdge.Length != 3)
            deleteEdge = Entity.Null;
          for (int index1 = 0; index1 < connectedEdge.Length; ++index1)
          {
            Entity edge1 = connectedEdge[index1].m_Edge;
            // ISSUE: reference to a compiler-generated field
            Edge edge2 = this.m_EdgeData[edge1];
            if (edge2.m_Start == entity)
            {
              if (deleteEdge != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckNodeEdges(edge2.m_End, entity);
              }
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge2.m_End);
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge1);
            }
            else if (edge2.m_End == entity)
            {
              if (deleteEdge != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckNodeEdges(edge2.m_Start, entity);
              }
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge2.m_Start);
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeEdges(edge2.m_Start, edge2.m_End);
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeEdges(edge2.m_End, edge2.m_Start);
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge2.m_Start);
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge2.m_End);
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge1);
            }
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[edge1];
            for (int index2 = 0; index2 < connectedNode.Length; ++index2)
            {
              Entity node = connectedNode[index2].m_Node;
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(node);
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeEdges(node);
            }
          }
        }
      }

      private void CheckNodeEdges(Entity node, Entity otherNode)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge2 = this.m_EdgeData[edge1];
          if (edge2.m_Start == node)
          {
            if (edge2.m_End != otherNode)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge2.m_End);
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge1);
            }
          }
          else if (edge2.m_End == node)
          {
            if (edge2.m_Start != otherNode)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge2.m_Start);
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(edge1);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(edge2.m_Start);
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(edge2.m_End);
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(edge1);
          }
        }
      }

      private void CheckNodeEdges(Entity node)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge2 = this.m_EdgeData[edge1];
          if (edge2.m_Start == node)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(edge2.m_End);
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(edge1);
          }
          else if (edge2.m_End == node)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(edge2.m_Start);
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(edge1);
          }
        }
      }

      private void CheckSubObjects(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[entity];
        for (int index = 0; index < subObject1.Length; ++index)
        {
          Entity subObject2 = subObject1[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachedData.HasComponent(subObject2) && this.m_AttachedData[subObject2].m_Parent == entity)
          {
            CreationDefinition creationDefinition = new CreationDefinition();
            creationDefinition.m_Original = subObject2;
            creationDefinition.m_Flags |= CreationFlags.Attach;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OwnerData.HasComponent(subObject2))
            {
              // ISSUE: reference to a compiler-generated field
              Transform transform = this.m_TransformData[subObject2];
              ObjectDefinition objectDefinition = new ObjectDefinition()
              {
                m_Position = transform.m_Position,
                m_Rotation = transform.m_Rotation
              };
              Game.Objects.Elevation componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ElevationData.TryGetComponent(subObject2, out componentData))
              {
                objectDefinition.m_Elevation = componentData.m_Elevation;
                objectDefinition.m_ParentMesh = ObjectUtils.GetSubParentMesh(componentData.m_Flags);
                if ((componentData.m_Flags & ElevationFlags.Lowered) != (ElevationFlags) 0)
                  creationDefinition.m_Flags |= CreationFlags.Lowered;
              }
              else
                objectDefinition.m_ParentMesh = -1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_CreationQueue.Enqueue(new GenerateObjectsSystem.CreationData(creationDefinition, new OwnerDefinition(), objectDefinition, false));
            }
          }
        }
      }

      private void CheckEdgesForLocalConnectOrAttachment(
        CoursePosFlags flags,
        Entity ignoreEntity,
        float3 position,
        float2 elevation,
        Entity prefab,
        bool isStandalone)
      {
        Bounds1 bounds1 = new Bounds1(float.MaxValue, float.MinValue);
        float2 x = (float2) 0.0f;
        Layer layer1 = Layer.None;
        Layer layer2 = Layer.None;
        Layer layer3 = Layer.None;
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[prefab];
        NetGeometryData netGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[prefab];
        }
        if (math.all(elevation >= netGeometryData.m_ElevationLimit * 2f) || !math.all(elevation < 0.0f) && (netData.m_RequiredLayers & (Layer.PowerlineLow | Layer.PowerlineHigh)) != Layer.None)
        {
          float y = netGeometryData.m_DefaultWidth * 0.5f;
          float num = position.y - math.cmin(elevation);
          bounds1 |= new Bounds1(num - y, num + y);
          x = math.max(x, (float2) y);
          layer1 |= Layer.Road;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabLocalConnectData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          LocalConnectData localConnectData = this.m_PrefabLocalConnectData[prefab];
          if ((localConnectData.m_Flags & LocalConnectFlags.ExplicitNodes) == (LocalConnectFlags) 0 || (flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) != (CoursePosFlags) 0)
          {
            float2 y = (float2) (netGeometryData.m_DefaultWidth * 0.5f + localConnectData.m_SearchDistance);
            y.y += math.select(0.0f, 8f, !isStandalone && (double) localConnectData.m_SearchDistance != 0.0 && (netGeometryData.m_Flags & Game.Net.GeometryFlags.SubOwner) == (Game.Net.GeometryFlags) 0);
            bounds1 |= position.y + localConnectData.m_HeightRange;
            x = math.max(x, y);
            layer2 |= localConnectData.m_Layers;
            layer3 |= netData.m_ConnectLayers;
          }
        }
        if (layer1 == Layer.None && layer2 == Layer.None && layer3 == Layer.None)
          return;
        float num1 = math.cmax(x);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateObjectsSystem.FillCreationListJob.EdgeIterator iterator = new GenerateObjectsSystem.FillCreationListJob.EdgeIterator()
        {
          m_Bounds = new Bounds3(position - num1, position + num1)
          {
            y = bounds1
          },
          m_Position = position.xz,
          m_ConnectRadius = x,
          m_AttachLayers = layer1,
          m_ConnectLayers = layer2,
          m_LocalConnectLayers = layer3,
          m_IgnoreEntity = ignoreEntity,
          m_JobData = this
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<GenerateObjectsSystem.FillCreationListJob.EdgeIterator>(ref iterator);
      }

      private void CheckNodesForLocalConnect(Bezier4x3 curve, Entity prefab, bool isStandalone)
      {
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[prefab];
        NetGeometryData netGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[prefab];
        }
        float2 x = (float2) (netGeometryData.m_DefaultWidth * 0.5f);
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoadData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          x.y += math.select(0.0f, 8f, isStandalone && (this.m_RoadData[prefab].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0);
        }
        float num = math.cmax(x) + 4f;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateObjectsSystem.FillCreationListJob.NodeIterator iterator = new GenerateObjectsSystem.FillCreationListJob.NodeIterator()
        {
          m_Bounds = MathUtils.Expand(MathUtils.Bounds(curve), new float3(num, 100f, num)),
          m_Curve = curve,
          m_ConnectRadius = x,
          m_ConnectLayers = netData.m_ConnectLayers,
          m_JobData = this
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<GenerateObjectsSystem.FillCreationListJob.NodeIterator>(ref iterator);
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

      private struct EdgeIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float2 m_Position;
        public float2 m_ConnectRadius;
        public Layer m_AttachLayers;
        public Layer m_ConnectLayers;
        public Layer m_LocalConnectLayers;
        public Entity m_IgnoreEntity;
        public GenerateObjectsSystem.FillCreationListJob m_JobData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || entity == this.m_IgnoreEntity || !this.m_JobData.m_CurveData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_JobData.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_JobData.m_NetData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_AttachLayers & netData.m_ConnectLayers) == Layer.None && ((this.m_ConnectLayers & netData.m_ConnectLayers) == Layer.None || (this.m_LocalConnectLayers & netData.m_LocalConnectLayers) == Layer.None))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_JobData.m_EdgeData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (edge.m_Start == this.m_IgnoreEntity || edge.m_End == this.m_IgnoreEntity)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_JobData.m_CurveData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_JobData.m_NetGeometryData[prefabRef.m_Prefab];
          RoadData roadData = new RoadData();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_JobData.m_RoadData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            roadData = this.m_JobData.m_RoadData[prefabRef.m_Prefab];
          }
          // ISSUE: reference to a compiler-generated field
          double num1 = (double) MathUtils.Distance(curve.m_Bezier.xz, this.m_Position, out float _);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num2 = math.select(this.m_ConnectRadius.x, this.m_ConnectRadius.y, !this.m_JobData.m_OwnerData.HasComponent(entity) && (roadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0);
          double num3 = (double) netGeometryData.m_DefaultWidth * 0.5 + (double) num2;
          bool flag = num1 <= num3;
          if (!flag)
          {
            Roundabout componentData1;
            Game.Net.Node componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_JobData.m_RoundaboutData.TryGetComponent(edge.m_Start, out componentData1) && this.m_JobData.m_NodeData.TryGetComponent(edge.m_Start, out componentData2) && (double) math.distance(componentData2.m_Position.xz, this.m_Position) <= (double) componentData1.m_Radius + (double) num2)
              flag = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_JobData.m_RoundaboutData.TryGetComponent(edge.m_End, out componentData1) && this.m_JobData.m_NodeData.TryGetComponent(edge.m_End, out componentData2) && (double) math.distance(componentData2.m_Position.xz, this.m_Position) <= (double) componentData1.m_Radius + (double) num2)
              flag = true;
          }
          if (!flag)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_JobData.CheckSubObjects(entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_JobData.CheckSubObjects(edge.m_Start);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_JobData.CheckSubObjects(edge.m_End);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_JobData.CheckNodeEdges(edge.m_Start, edge.m_End);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_JobData.CheckNodeEdges(edge.m_End, edge.m_Start);
        }
      }

      private struct NodeIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Bezier4x3 m_Curve;
        public float2 m_ConnectRadius;
        public Layer m_ConnectLayers;
        public GenerateObjectsSystem.FillCreationListJob m_JobData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_JobData.m_NodeData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckNode(entity);
        }

        private void CheckNode(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_JobData.m_LocalConnectData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_JobData.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_JobData.m_PrefabLocalConnectData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          LocalConnectData localConnectData = this.m_JobData.m_PrefabLocalConnectData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if ((this.m_ConnectLayers & localConnectData.m_Layers) == Layer.None)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_JobData.m_NetGeometryData[prefabRef.m_Prefab];
          float num1 = math.max(0.0f, netGeometryData.m_DefaultWidth * 0.5f + localConnectData.m_SearchDistance);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_JobData.m_NodeData[entity];
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Bounds, new Bounds3(node.m_Position - num1, node.m_Position + num1)
          {
            y = node.m_Position.y + localConnectData.m_HeightRange
          }))
            return;
          // ISSUE: reference to a compiler-generated field
          double num2 = (double) MathUtils.Distance(this.m_Curve.xz, node.m_Position.xz, out float _);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num3 = math.select(this.m_ConnectRadius.x, this.m_ConnectRadius.y, this.m_JobData.m_OwnerData.HasComponent(entity) && (double) localConnectData.m_SearchDistance != 0.0 && (netGeometryData.m_Flags & Game.Net.GeometryFlags.SubOwner) == (Game.Net.GeometryFlags) 0);
          double num4 = (double) num1 + (double) num3;
          if (num2 > num4)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_JobData.CheckSubObjects(entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_JobData.CheckNodeEdges(entity);
        }
      }
    }

    [BurstCompile]
    private struct CollectCreationDataJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public NativeQueue<GenerateObjectsSystem.CreationData> m_CreationQueue;
      public NativeList<GenerateObjectsSystem.CreationData> m_CreationList;
      public NativeParallelMultiHashMap<GenerateObjectsSystem.OldObjectKey, GenerateObjectsSystem.OldObjectValue> m_OldObjectMap;
      public NativeHashMap<OwnerDefinition, Entity> m_ReusedOwnerMap;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CreationList.ResizeUninitialized(this.m_CreationQueue.Count);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_CreationList.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CreationList[index] = this.m_CreationQueue.Dequeue();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CreationList.Sort<GenerateObjectsSystem.CreationData>();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateObjectsSystem.CreationData creationData = new GenerateObjectsSystem.CreationData();
        int num1 = 0;
        int index1 = 0;
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        while (num1 < this.m_CreationList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GenerateObjectsSystem.CreationData creation = this.m_CreationList[num1++];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (creation.m_CreationDefinition.m_Original != creationData.m_CreationDefinition.m_Original || creation.m_CreationDefinition.m_Original == Entity.Null)
          {
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CreationList[index1++] = creationData;
            }
            creationData = creation;
            flag = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (creation.m_HasDefinition)
              creationData = creation;
          }
        }
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CreationList[index1++] = creationData;
        }
        // ISSUE: reference to a compiler-generated field
        if (index1 < this.m_CreationList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CreationList.RemoveRange(index1, this.m_CreationList.Length - index1);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index2 = 0; index2 < this.m_CreationList.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GenerateObjectsSystem.CreationData creation = this.m_CreationList[index2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((creation.m_CreationDefinition.m_Prefab == Entity.Null || (creation.m_CreationDefinition.m_Flags & CreationFlags.Upgrade) == (CreationFlags) 0) && this.m_PrefabRefData.HasComponent(creation.m_CreationDefinition.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            creation.m_CreationDefinition.m_Prefab = this.m_PrefabRefData[creation.m_CreationDefinition.m_Original].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            this.m_CreationList[index2] = creation;
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index3 = 0; index3 < this.m_CreationList.Length; ++index3)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GenerateObjectsSystem.CreationData creation = this.m_CreationList[index3];
          // ISSUE: reference to a compiler-generated field
          if (creation.m_OwnerDefinition.m_Prefab == Entity.Null)
          {
            // ISSUE: variable of a compiler-generated type
            GenerateObjectsSystem.OldObjectKey key1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            key1.m_Prefab = creation.m_CreationDefinition.m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            key1.m_SubPrefab = creation.m_CreationDefinition.m_SubPrefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            key1.m_Original = creation.m_CreationDefinition.m_Original;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            key1.m_Owner = creation.m_CreationDefinition.m_Owner;
            // ISSUE: variable of a compiler-generated type
            GenerateObjectsSystem.OldObjectValue oldObjectValue;
            NativeParallelMultiHashMapIterator<GenerateObjectsSystem.OldObjectKey> it1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OldObjectMap.TryGetFirstValue(key1, out oldObjectValue, out it1))
            {
              float num2 = float.MaxValue;
              // ISSUE: reference to a compiler-generated field
              Entity entity = oldObjectValue.m_Entity;
              NativeParallelMultiHashMapIterator<GenerateObjectsSystem.OldObjectKey> it2 = it1;
              // ISSUE: reference to a compiler-generated field
              do
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num3 = math.distancesq(creation.m_ObjectDefinition.m_Position, oldObjectValue.m_Transform.m_Position);
                if ((double) num3 < (double) num2)
                {
                  num2 = num3;
                  // ISSUE: reference to a compiler-generated field
                  entity = oldObjectValue.m_Entity;
                  it2 = it1;
                }
              }
              while (this.m_OldObjectMap.TryGetNextValue(out oldObjectValue, ref it1));
              // ISSUE: reference to a compiler-generated field
              creation.m_OldEntity = entity;
              // ISSUE: reference to a compiler-generated field
              this.m_OldObjectMap.Remove(it2);
              // ISSUE: reference to a compiler-generated field
              this.m_CreationList[index3] = creation;
              OwnerDefinition key2;
              // ISSUE: reference to a compiler-generated field
              key2.m_Prefab = creation.m_CreationDefinition.m_Prefab;
              // ISSUE: reference to a compiler-generated field
              key2.m_Position = creation.m_ObjectDefinition.m_Position;
              // ISSUE: reference to a compiler-generated field
              key2.m_Rotation = creation.m_ObjectDefinition.m_Rotation;
              // ISSUE: reference to a compiler-generated field
              this.m_ReusedOwnerMap.TryAdd(key2, entity);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index4 = 0; index4 < this.m_CreationList.Length; ++index4)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GenerateObjectsSystem.CreationData creation = this.m_CreationList[index4];
          Entity entity1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (creation.m_OwnerDefinition.m_Prefab != Entity.Null && this.m_ReusedOwnerMap.TryGetValue(creation.m_OwnerDefinition, out entity1))
          {
            // ISSUE: variable of a compiler-generated type
            GenerateObjectsSystem.OldObjectKey key3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            key3.m_Prefab = creation.m_CreationDefinition.m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            key3.m_SubPrefab = creation.m_CreationDefinition.m_SubPrefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            key3.m_Original = creation.m_CreationDefinition.m_Original;
            // ISSUE: reference to a compiler-generated field
            key3.m_Owner = entity1;
            // ISSUE: variable of a compiler-generated type
            GenerateObjectsSystem.OldObjectValue oldObjectValue;
            NativeParallelMultiHashMapIterator<GenerateObjectsSystem.OldObjectKey> it3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OldObjectMap.TryGetFirstValue(key3, out oldObjectValue, out it3))
            {
              float num4 = float.MaxValue;
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = oldObjectValue.m_Entity;
              NativeParallelMultiHashMapIterator<GenerateObjectsSystem.OldObjectKey> it4 = it3;
              // ISSUE: reference to a compiler-generated field
              do
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num5 = math.distancesq(creation.m_ObjectDefinition.m_Position, oldObjectValue.m_Transform.m_Position);
                if ((double) num5 < (double) num4)
                {
                  num4 = num5;
                  // ISSUE: reference to a compiler-generated field
                  entity2 = oldObjectValue.m_Entity;
                  it4 = it3;
                }
              }
              while (this.m_OldObjectMap.TryGetNextValue(out oldObjectValue, ref it3));
              // ISSUE: reference to a compiler-generated field
              creation.m_OldEntity = entity2;
              // ISSUE: reference to a compiler-generated field
              this.m_OldObjectMap.Remove(it4);
              // ISSUE: reference to a compiler-generated field
              this.m_CreationList[index4] = creation;
              OwnerDefinition key4;
              // ISSUE: reference to a compiler-generated field
              key4.m_Prefab = creation.m_CreationDefinition.m_Prefab;
              // ISSUE: reference to a compiler-generated field
              key4.m_Position = creation.m_ObjectDefinition.m_Position;
              // ISSUE: reference to a compiler-generated field
              key4.m_Rotation = creation.m_ObjectDefinition.m_Rotation;
              // ISSUE: reference to a compiler-generated field
              this.m_ReusedOwnerMap.TryAdd(key4, entity2);
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct CreateObjectsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ComponentTypeSet m_SubTypes;
      [ReadOnly]
      public ComponentTypeSet m_StoppedUpdateFrameTypes;
      [ReadOnly]
      public NativeArray<GenerateObjectsSystem.CreationData> m_CreationList;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Stopped> m_StoppedData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<Recent> m_RecentData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_DamagedData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Surface> m_SurfaceData;
      [ReadOnly]
      public ComponentLookup<Stack> m_StackData;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> m_UnderConstructionData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<ServiceUpgradeData> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_ObjectData;
      [ReadOnly]
      public ComponentLookup<MovingObjectData> m_MovingObjectData;
      [ReadOnly]
      public ComponentLookup<NetObjectData> m_NetObjectData;
      [ReadOnly]
      public ComponentLookup<TreeData> m_PrefabTreeData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<EffectData> m_PrefabEffectData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameterData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        GenerateObjectsSystem.CreationData creation = this.m_CreationList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CreateObject(index, creation.m_CreationDefinition, creation.m_OwnerDefinition, creation.m_ObjectDefinition, creation.m_OldEntity);
      }

      private void CreateObject(
        int jobIndex,
        CreationDefinition definitionData,
        OwnerDefinition ownerDefinition,
        ObjectDefinition objectDefinition,
        Entity oldEntity)
      {
        PrefabRef component1 = new PrefabRef();
        component1.m_Prefab = definitionData.m_Prefab;
        if (definitionData.m_Original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Hidden>(jobIndex, definitionData.m_Original, new Hidden());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, definitionData.m_Original, new BatchesUpdated());
        }
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(jobIndex);
        ObjectGeometryData componentData1;
        // ISSUE: reference to a compiler-generated field
        bool component2 = this.m_PrefabObjectData.TryGetComponent(component1.m_Prefab, out componentData1);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = (definitionData.m_Flags & CreationFlags.Permanent) != (CreationFlags) 0 || this.m_PrefabData.IsComponentEnabled(component1.m_Prefab);
        Tree componentData2 = new Tree();
        // ISSUE: reference to a compiler-generated field
        bool flag2 = flag1 && this.m_PrefabTreeData.HasComponent(component1.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        if (flag2 && !this.m_TreeData.TryGetComponent(definitionData.m_Original, out componentData2))
          componentData2 = ObjectUtils.InitializeTreeState(objectDefinition.m_Age);
        Temp component3 = new Temp();
        component3.m_Original = definitionData.m_Original;
        component3.m_Flags |= TempFlags.Essential;
        PlaceableObjectData componentData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceableObjectData.TryGetComponent(definitionData.m_Prefab, out componentData3))
        {
          component3.m_Value = (int) componentData3.m_ConstructionCost;
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            component3.m_Value = ObjectUtils.GetContructionCost(component3.m_Value, componentData2, in this.m_EconomyParameterData);
          }
        }
        else
        {
          ServiceUpgradeData componentData4;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceUpgradeData.TryGetComponent(definitionData.m_Prefab, out componentData4))
            component3.m_Value = (int) componentData4.m_UpgradeCost;
        }
        if ((definitionData.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
        {
          component3.m_Flags |= TempFlags.Delete;
          Recent componentData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RecentData.TryGetComponent(definitionData.m_Original, out componentData5))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component3.m_Cost = -ObjectUtils.GetRefundAmount(componentData5, this.m_SimulationFrame, this.m_EconomyParameterData);
          }
        }
        else if ((definitionData.m_Flags & CreationFlags.Select) != (CreationFlags) 0)
        {
          component3.m_Flags |= TempFlags.Select;
          if ((definitionData.m_Flags & CreationFlags.Dragging) != (CreationFlags) 0)
            component3.m_Flags |= TempFlags.Dragging;
        }
        else if (definitionData.m_Original != Entity.Null)
        {
          if ((definitionData.m_Flags & CreationFlags.Relocate) != (CreationFlags) 0)
          {
            component3.m_Flags |= TempFlags.Modify;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_EditorMode)
            {
              Recent componentData6;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component3.m_Cost = !this.m_RecentData.TryGetComponent(definitionData.m_Original, out componentData6) ? ObjectUtils.GetRelocationCost(component3.m_Value) : ObjectUtils.GetRelocationCost(component3.m_Value, componentData6, this.m_SimulationFrame, this.m_EconomyParameterData);
            }
          }
          else
          {
            if ((definitionData.m_Flags & CreationFlags.Upgrade) != (CreationFlags) 0)
            {
              component3.m_Flags |= TempFlags.Upgrade;
              PrefabRef componentData7;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_EditorMode && this.m_PrefabRefData.TryGetComponent(definitionData.m_Original, out componentData7) && componentData7.m_Prefab != definitionData.m_Prefab)
              {
                int num = 0;
                PlaceableObjectData componentData8;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PlaceableObjectData.TryGetComponent(componentData7.m_Prefab, out componentData8))
                {
                  num = (int) componentData8.m_ConstructionCost;
                  if (flag2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    num = ObjectUtils.GetContructionCost(num, componentData2, in this.m_EconomyParameterData);
                  }
                }
                Recent componentData9;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                component3.m_Cost = !this.m_RecentData.TryGetComponent(definitionData.m_Original, out componentData9) ? ObjectUtils.GetUpgradeCost(component3.m_Value, num) : ObjectUtils.GetUpgradeCost(component3.m_Value, num, componentData9, this.m_SimulationFrame, this.m_EconomyParameterData);
              }
            }
            else if ((definitionData.m_Flags & CreationFlags.Duplicate) != (CreationFlags) 0)
              component3.m_Flags |= TempFlags.Duplicate;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((definitionData.m_Flags & CreationFlags.Repair) != (CreationFlags) 0 && !this.m_EditorMode && this.m_DestroyedData.HasComponent(definitionData.m_Original))
            {
              Recent componentData10;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component3.m_Cost = !this.m_RecentData.TryGetComponent(definitionData.m_Original, out componentData10) ? ObjectUtils.GetRebuildCost(component3.m_Value) : ObjectUtils.GetRebuildCost(component3.m_Value, componentData10, this.m_SimulationFrame, this.m_EconomyParameterData);
            }
            if ((definitionData.m_Flags & CreationFlags.Parent) != (CreationFlags) 0)
              component3.m_Flags |= TempFlags.Parent;
          }
        }
        else
        {
          component3.m_Flags |= TempFlags.Create;
          if ((definitionData.m_Flags & CreationFlags.Optional) != (CreationFlags) 0)
            component3.m_Flags |= TempFlags.Optional;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EditorMode)
            component3.m_Cost = component3.m_Value;
        }
        ElevationFlags flags = (ElevationFlags) 0;
        if (math.abs(objectDefinition.m_ParentMesh) >= 1000)
          flags |= ElevationFlags.Stacked;
        if (objectDefinition.m_ParentMesh < 0)
          flags |= ElevationFlags.OnGround;
        if ((definitionData.m_Flags & CreationFlags.Lowered) != (CreationFlags) 0)
          flags |= ElevationFlags.Lowered;
        if (oldEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, oldEntity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldEntity, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(jobIndex, oldEntity, new Transform(objectDefinition.m_Position, objectDefinition.m_Rotation));
          if (ownerDefinition.m_Prefab == Entity.Null && definitionData.m_Owner == Entity.Null && (componentData1.m_Flags & Game.Objects.GeometryFlags.Physical) != Game.Objects.GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_SurfaceData.HasComponent(definitionData.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Game.Objects.Surface>(jobIndex, oldEntity, this.m_SurfaceData[definitionData.m_Original]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Game.Objects.Surface>(jobIndex, oldEntity, new Game.Objects.Surface());
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((definitionData.m_Flags & CreationFlags.Attach) != (CreationFlags) 0 || this.m_NetObjectData.HasComponent(component1.m_Prefab))
          {
            Attached componentData11;
            // ISSUE: reference to a compiler-generated field
            this.m_AttachedData.TryGetComponent(oldEntity, out componentData11);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_CommandBuffer.AddComponent<Attached>(jobIndex, oldEntity, this.CreateAttached(definitionData.m_Attached, componentData11.m_Parent, objectDefinition.m_Position));
          }
          else
          {
            Attached componentData12;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AttachedData.TryGetComponent(oldEntity, out componentData12))
            {
              componentData12.m_OldParent = componentData12.m_Parent;
              componentData12.m_Parent = oldEntity;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Attached>(jobIndex, oldEntity, componentData12);
            }
          }
          if ((definitionData.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, oldEntity, component3);
          }
          if ((double) objectDefinition.m_Elevation != 0.0 || objectDefinition.m_ParentMesh != -1 || definitionData.m_SubPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Game.Objects.Elevation>(jobIndex, oldEntity, new Game.Objects.Elevation(objectDefinition.m_Elevation, flags));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Game.Objects.Elevation>(jobIndex, oldEntity, new Game.Objects.Elevation(objectDefinition.m_Elevation, flags));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Game.Objects.Elevation>(jobIndex, oldEntity);
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Tree>(jobIndex, oldEntity, componentData2);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode && (ownerDefinition.m_Prefab != Entity.Null || definitionData.m_Owner != Entity.Null))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<LocalTransformCache>(jobIndex, oldEntity, new LocalTransformCache()
            {
              m_Position = objectDefinition.m_LocalPosition,
              m_Rotation = objectDefinition.m_LocalRotation,
              m_ParentMesh = objectDefinition.m_ParentMesh,
              m_GroupIndex = objectDefinition.m_GroupIndex,
              m_Probability = objectDefinition.m_Probability,
              m_PrefabSubIndex = objectDefinition.m_PrefabSubIndex
            });
          }
          if (component2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PseudoRandomSeedData.HasComponent(definitionData.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, this.m_PseudoRandomSeedData[definitionData.m_Original]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, new PseudoRandomSeed((ushort) definitionData.m_RandomSeed));
            }
          }
          if (definitionData.m_SubPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<EditorContainer>(jobIndex, oldEntity, new EditorContainer()
            {
              m_Prefab = definitionData.m_SubPrefab,
              m_Scale = objectDefinition.m_Scale,
              m_Intensity = objectDefinition.m_Intensity,
              m_GroupIndex = objectDefinition.m_GroupIndex
            });
          }
          if (!(definitionData.m_Original != Entity.Null))
            return;
          UnderConstruction componentData13;
          // ISSUE: reference to a compiler-generated field
          if (this.m_UnderConstructionData.TryGetComponent(definitionData.m_Original, out componentData13))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<UnderConstruction>(jobIndex, oldEntity, componentData13);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_UnderConstructionData.HasComponent(oldEntity))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<UnderConstruction>(jobIndex, oldEntity);
          }
        }
        else
        {
          Entity entity;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StoppedData.HasComponent(definitionData.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MovingObjectData[component1.m_Prefab].m_StoppedArchetype);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode && this.m_MovingObjectData.HasComponent(component1.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MovingObjectData[component1.m_Prefab].m_StoppedArchetype);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity, in this.m_StoppedUpdateFrameTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Static>(jobIndex, entity, new Static());
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ObjectData[component1.m_Prefab].m_Archetype);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(jobIndex, entity, new Transform(objectDefinition.m_Position, objectDefinition.m_Rotation));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, component1);
          if (ownerDefinition.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity, new Owner());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(jobIndex, entity, ownerDefinition);
          }
          else if (definitionData.m_Owner != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity, new Owner(definitionData.m_Owner));
          }
          else if ((componentData1.m_Flags & Game.Objects.GeometryFlags.Physical) != Game.Objects.GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_SurfaceData.HasComponent(definitionData.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Game.Objects.Surface>(jobIndex, entity, this.m_SurfaceData[definitionData.m_Original]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Game.Objects.Surface>(jobIndex, entity, new Game.Objects.Surface());
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((definitionData.m_Flags & CreationFlags.Attach) != (CreationFlags) 0 || this.m_NetObjectData.HasComponent(component1.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_CommandBuffer.AddComponent<Attached>(jobIndex, entity, this.CreateAttached(definitionData.m_Attached, Entity.Null, objectDefinition.m_Position));
          }
          if ((definitionData.m_Flags & CreationFlags.Repair) == (CreationFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DamagedData.HasComponent(definitionData.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Damaged>(jobIndex, entity, this.m_DamagedData[definitionData.m_Original]);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_DestroyedData.HasComponent(definitionData.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Destroyed>(jobIndex, entity, this.m_DestroyedData[definitionData.m_Original]);
            }
          }
          if ((definitionData.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity, component3);
            if (component2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Animation>(jobIndex, entity, new Animation());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<InterpolatedTransform>(jobIndex, entity, new InterpolatedTransform());
            }
            Game.Prefabs.BuildingData componentData14;
            // ISSUE: reference to a compiler-generated field
            if (flag1 && this.m_PrefabBuildingData.TryGetComponent(component1.m_Prefab, out componentData14) && (componentData14.m_Flags & Game.Prefabs.BuildingFlags.BackAccess) != (Game.Prefabs.BuildingFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BackSide>(jobIndex, entity, new BackSide());
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((definitionData.m_Flags & CreationFlags.Native) != (CreationFlags) 0 || this.m_NativeData.HasComponent(definitionData.m_Original) && (component3.m_Flags & (TempFlags.Modify | TempFlags.Upgrade)) == (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Native>(jobIndex, entity, new Native());
          }
          if ((double) objectDefinition.m_Elevation != 0.0 || objectDefinition.m_ParentMesh != -1 || definitionData.m_SubPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Game.Objects.Elevation>(jobIndex, entity, new Game.Objects.Elevation(objectDefinition.m_Elevation, flags));
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Tree>(jobIndex, entity, componentData2);
          }
          StackData componentData15;
          // ISSUE: reference to a compiler-generated field
          if (flag1 && this.m_PrefabStackData.TryGetComponent(component1.m_Prefab, out componentData15))
          {
            Stack componentData16;
            // ISSUE: reference to a compiler-generated field
            if (this.m_StackData.TryGetComponent(definitionData.m_Original, out componentData16))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Stack>(jobIndex, entity, componentData16);
            }
            else
            {
              if (componentData15.m_Direction == StackDirection.Up)
              {
                componentData16.m_Range.min = componentData15.m_FirstBounds.min - objectDefinition.m_Elevation;
                componentData16.m_Range.max = componentData15.m_LastBounds.max;
              }
              else
              {
                componentData16.m_Range.min = componentData15.m_FirstBounds.min;
                componentData16.m_Range.max = componentData15.m_FirstBounds.max + MathUtils.Size(componentData15.m_MiddleBounds) * 2f + MathUtils.Size(componentData15.m_LastBounds);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Stack>(jobIndex, entity, componentData16);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode)
          {
            bool flag3 = true;
            if (ownerDefinition.m_Prefab != Entity.Null || definitionData.m_Owner != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalTransformCache>(jobIndex, entity, new LocalTransformCache()
              {
                m_Position = objectDefinition.m_LocalPosition,
                m_Rotation = objectDefinition.m_LocalRotation,
                m_ParentMesh = objectDefinition.m_ParentMesh,
                m_GroupIndex = objectDefinition.m_GroupIndex,
                m_Probability = objectDefinition.m_Probability,
                m_PrefabSubIndex = objectDefinition.m_PrefabSubIndex
              });
              // ISSUE: reference to a compiler-generated field
              flag3 = this.m_ServiceUpgradeData.HasComponent(definitionData.m_Prefab);
            }
            if (flag3)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<EnabledEffect>(jobIndex, entity);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_SubTypes);
            }
          }
          if (component2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PseudoRandomSeedData.HasComponent(definitionData.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, this.m_PseudoRandomSeedData[definitionData.m_Original]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, new PseudoRandomSeed((ushort) definitionData.m_RandomSeed));
            }
          }
          if (definitionData.m_SubPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<EditorContainer>(jobIndex, entity, new EditorContainer()
            {
              m_Prefab = definitionData.m_SubPrefab,
              m_Scale = objectDefinition.m_Scale,
              m_Intensity = objectDefinition.m_Intensity,
              m_GroupIndex = objectDefinition.m_GroupIndex
            });
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabEffectData.HasComponent(definitionData.m_SubPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddBuffer<EnabledEffect>(jobIndex, entity);
            }
          }
          if (definitionData.m_Original != Entity.Null)
          {
            UnderConstruction componentData17;
            // ISSUE: reference to a compiler-generated field
            if (this.m_UnderConstructionData.TryGetComponent(definitionData.m_Original, out componentData17))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<UnderConstruction>(jobIndex, entity, componentData17);
            }
            Relative componentData18;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_RelativeData.TryGetComponent(definitionData.m_Original, out componentData18))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Relative>(jobIndex, entity, componentData18);
          }
          else
          {
            if ((definitionData.m_Flags & CreationFlags.Construction) == (CreationFlags) 0)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<UnderConstruction>(jobIndex, entity, new UnderConstruction()
            {
              m_Speed = (byte) random.NextInt(39, 89)
            });
          }
        }
      }

      private Attached CreateAttached(Entity parent, Entity oldParent, float3 position)
      {
        Attached attached = new Attached(parent, oldParent, 0.0f);
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurveData.HasComponent(parent))
        {
          // ISSUE: reference to a compiler-generated field
          double num = (double) MathUtils.Distance(this.m_CurveData[parent].m_Bezier, position, out attached.m_CurvePosition);
        }
        return attached;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> __Game_Tools_OwnerDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ObjectDefinition> __Game_Tools_ObjectDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> __Game_Tools_NetCourse_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> __Game_Prefabs_LocalConnectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnect> __Game_Net_LocalConnect_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Roundabout> __Game_Net_Roundabout_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stopped> __Game_Objects_Stopped_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Recent> __Game_Tools_Recent_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Surface> __Game_Objects_Surface_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stack> __Game_Objects_Stack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceUpgradeData> __Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingObjectData> __Game_Prefabs_MovingObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetObjectData> __Game_Prefabs_NetObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EffectData> __Game_Prefabs_EffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OwnerDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_ObjectDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_NetCourse_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCourse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RO_ComponentLookup = state.GetComponentLookup<LocalConnectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RO_ComponentLookup = state.GetComponentLookup<RoadData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LocalConnect_RO_ComponentLookup = state.GetComponentLookup<LocalConnect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Roundabout_RO_ComponentLookup = state.GetComponentLookup<Roundabout>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentLookup = state.GetComponentLookup<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Recent_RO_ComponentLookup = state.GetComponentLookup<Recent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Surface_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Surface>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentLookup = state.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentLookup = state.GetComponentLookup<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup = state.GetComponentLookup<ServiceUpgradeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MovingObjectData_RO_ComponentLookup = state.GetComponentLookup<MovingObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetObjectData_RO_ComponentLookup = state.GetComponentLookup<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectData_RO_ComponentLookup = state.GetComponentLookup<EffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
      }
    }
  }
}
