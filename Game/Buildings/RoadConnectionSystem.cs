// Decompiled with JetBrains decompiler
// Type: Game.Buildings.RoadConnectionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Audio;
using Game.Common;
using Game.Effects;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class RoadConnectionSystem : GameSystemBase
  {
    private ModificationBarrier4B m_ModificationBarrier;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private IconCommandSystem m_IconCommandSystem;
    private AudioManager m_AudioManager;
    private EntityQuery m_ModificationQuery;
    private EntityQuery m_UpdatedNetQuery;
    private EntityQuery m_TrafficConfigQuery;
    private EntityQuery m_BuildingConfigQuery;
    private EntityQuery m_ConnectionQuery;
    private EntityArchetype m_RoadConnectionEventArchetype;
    private ComponentTypeSet m_AppliedTypes;
    private RoadConnectionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Building>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Net.Edge>(),
          ComponentType.ReadOnly<ConnectedBuilding>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedNetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadOnly<ConnectedBuilding>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<TrafficConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<ConnectionLaneData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_RoadConnectionEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<RoadConnectionUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ModificationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<RoadConnectionSystem.ReplaceRoad> list1 = new NativeList<RoadConnectionSystem.ReplaceRoad>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
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
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
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
      RoadConnectionSystem.CheckRoadConnectionJob jobData1 = new RoadConnectionSystem.CheckRoadConnectionJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
        m_StartNodeGeometryType = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle,
        m_EndNodeGeometryType = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle,
        m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle,
        m_ConnectedBuildingType = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle,
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies1),
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_ReplaceRoadConnectionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RoadConnectionSystem.FillReplacementListJob jobData2 = new RoadConnectionSystem.FillReplacementListJob()
      {
        m_ReplaceRoadConnectionQueue = nativeQueue,
        m_ReplaceRoadConnection = list1
      };
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_UpdatedNetQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_BackSide_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      RoadConnectionSystem.FindRoadConnectionJob jobData3 = new RoadConnectionSystem.FindRoadConnectionJob()
      {
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabNetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_BackSideData = this.__TypeHandle.__Game_Buildings_BackSide_RW_ComponentLookup,
        m_UpdatedNetChunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_ReplaceRoadConnection = list1.AsDeferredJobArray()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RoadConnectionSystem.ReplaceRoadConnectionJob jobData4 = new RoadConnectionSystem.ReplaceRoadConnectionJob()
      {
        m_CreatedData = this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RW_BufferLookup,
        m_RoadConnectionEventArchetype = this.m_RoadConnectionEventArchetype,
        m_ReplaceRoadConnection = list1,
        m_TrafficConfigurationData = this.m_TrafficConfigQuery.GetSingleton<TrafficConfigurationData>(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_SourceUpdateData = this.m_AudioManager.GetSourceUpdateData(out deps)
      };
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync = this.m_ConnectionQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UtilityObject_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      RoadConnectionSystem.UpdateSecondaryLanesJob jobData5 = new RoadConnectionSystem.UpdateSecondaryLanesJob()
      {
        m_WaterConsumerData = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_ElectricityConsumerData = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_UtilityObjectData = this.__TypeHandle.__Game_Objects_UtilityObject_RO_ComponentLookup,
        m_SecondaryData = this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_UtilityLaneData = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabUtilityObjectData = this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup,
        m_PrefabNetLaneArchetypeData = this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_ConnectionPrefabs = entityListAsync,
        m_ReplaceRoadConnection = list1,
        m_AppliedTypes = this.m_AppliedTypes,
        m_BuildingConfigurationData = this.m_BuildingConfigQuery.GetSingleton<BuildingConfigurationData>(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<RoadConnectionSystem.CheckRoadConnectionJob>(this.m_ModificationQuery, JobHandle.CombineDependencies(this.Dependency, dependencies1));
      JobHandle jobHandle2 = jobData2.Schedule<RoadConnectionSystem.FillReplacementListJob>(jobHandle1);
      JobHandle jobHandle3 = jobData3.Schedule<RoadConnectionSystem.FindRoadConnectionJob, RoadConnectionSystem.ReplaceRoad>(list1, 1, JobUtils.CombineDependencies(jobHandle2, dependencies2, outJobHandle1, outJobHandle2));
      JobHandle jobHandle4 = jobData4.Schedule<RoadConnectionSystem.ReplaceRoadConnectionJob>(JobHandle.CombineDependencies(jobHandle3, deps));
      NativeList<RoadConnectionSystem.ReplaceRoad> list2 = list1;
      JobHandle dependsOn = jobHandle3;
      JobHandle jobHandle5 = jobData5.Schedule<RoadConnectionSystem.UpdateSecondaryLanesJob, RoadConnectionSystem.ReplaceRoad>(list2, 1, dependsOn);
      nativeQueue.Dispose(jobHandle2);
      archetypeChunkListAsync.Dispose(jobHandle3);
      entityListAsync.Dispose(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle4);
      this.Dependency = JobHandle.CombineDependencies(jobHandle4, jobHandle5);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      list1.Dispose(this.Dependency);
    }

    private static void CheckDistance(
      EdgeGeometry edgeGeometry,
      EdgeNodeGeometry startGeometry,
      EdgeNodeGeometry endGeometry,
      float3 position,
      bool canBeOnRoad,
      ref float maxDistance)
    {
      if ((double) MathUtils.DistanceSquared(edgeGeometry.m_Bounds.xz, position.xz) < (double) maxDistance * (double) maxDistance)
      {
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(edgeGeometry.m_Start.m_Left, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(edgeGeometry.m_Start.m_Right, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(edgeGeometry.m_End.m_Left, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(edgeGeometry.m_End.m_Right, position, ref maxDistance);
        if (canBeOnRoad)
        {
          // ISSUE: reference to a compiler-generated method
          RoadConnectionSystem.CheckDistance(edgeGeometry.m_Start.m_Left, edgeGeometry.m_Start.m_Right, position, ref maxDistance);
          // ISSUE: reference to a compiler-generated method
          RoadConnectionSystem.CheckDistance(edgeGeometry.m_End.m_Left, edgeGeometry.m_End.m_Right, position, ref maxDistance);
        }
      }
      if ((double) MathUtils.DistanceSquared(startGeometry.m_Bounds.xz, position.xz) < (double) maxDistance * (double) maxDistance)
      {
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(startGeometry.m_Left.m_Left, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(startGeometry.m_Right.m_Right, position, ref maxDistance);
        if ((double) startGeometry.m_MiddleRadius > 0.0)
        {
          // ISSUE: reference to a compiler-generated method
          RoadConnectionSystem.CheckDistance(startGeometry.m_Left.m_Right, position, ref maxDistance);
          // ISSUE: reference to a compiler-generated method
          RoadConnectionSystem.CheckDistance(startGeometry.m_Right.m_Left, position, ref maxDistance);
        }
        if (canBeOnRoad)
        {
          if ((double) startGeometry.m_MiddleRadius > 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            RoadConnectionSystem.CheckDistance(startGeometry.m_Left.m_Left, startGeometry.m_Left.m_Right, position, ref maxDistance);
            // ISSUE: reference to a compiler-generated method
            RoadConnectionSystem.CheckDistance(startGeometry.m_Right.m_Left, startGeometry.m_Middle, position, ref maxDistance);
            // ISSUE: reference to a compiler-generated method
            RoadConnectionSystem.CheckDistance(startGeometry.m_Middle, startGeometry.m_Right.m_Right, position, ref maxDistance);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            RoadConnectionSystem.CheckDistance(startGeometry.m_Left.m_Left, startGeometry.m_Middle, position, ref maxDistance);
            // ISSUE: reference to a compiler-generated method
            RoadConnectionSystem.CheckDistance(startGeometry.m_Middle, startGeometry.m_Right.m_Right, position, ref maxDistance);
          }
        }
      }
      if ((double) MathUtils.DistanceSquared(endGeometry.m_Bounds.xz, position.xz) >= (double) maxDistance * (double) maxDistance)
        return;
      // ISSUE: reference to a compiler-generated method
      RoadConnectionSystem.CheckDistance(endGeometry.m_Left.m_Left, position, ref maxDistance);
      // ISSUE: reference to a compiler-generated method
      RoadConnectionSystem.CheckDistance(endGeometry.m_Right.m_Right, position, ref maxDistance);
      if ((double) endGeometry.m_MiddleRadius > 0.0)
      {
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(endGeometry.m_Left.m_Right, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(endGeometry.m_Right.m_Left, position, ref maxDistance);
      }
      if (!canBeOnRoad)
        return;
      if ((double) endGeometry.m_MiddleRadius > 0.0)
      {
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(endGeometry.m_Left.m_Left, endGeometry.m_Left.m_Right, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(endGeometry.m_Right.m_Left, endGeometry.m_Middle, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(endGeometry.m_Middle, endGeometry.m_Right.m_Right, position, ref maxDistance);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(endGeometry.m_Left.m_Left, endGeometry.m_Middle, position, ref maxDistance);
        // ISSUE: reference to a compiler-generated method
        RoadConnectionSystem.CheckDistance(endGeometry.m_Middle, endGeometry.m_Right.m_Right, position, ref maxDistance);
      }
    }

    private static void CheckDistance(
      Bezier4x3 curve1,
      Bezier4x3 curve2,
      float3 position,
      ref float maxDistance)
    {
      if ((double) MathUtils.DistanceSquared(MathUtils.Bounds(curve1.xz) | MathUtils.Bounds(curve2.xz), position.xz) >= (double) maxDistance * (double) maxDistance)
        return;
      float t;
      double num = (double) MathUtils.Distance(MathUtils.Lerp(curve1.xz, curve2.xz, 0.5f), position.xz, out t);
      float x = MathUtils.Distance(new Line2.Segment(MathUtils.Position(curve1.xz, t), MathUtils.Position(curve2.xz, t)), position.xz, out float _);
      maxDistance = math.min(x, maxDistance);
    }

    private static void CheckDistance(Bezier4x3 curve, float3 position, ref float maxDistance)
    {
      if ((double) MathUtils.DistanceSquared(MathUtils.Bounds(curve.xz), position.xz) >= (double) maxDistance * (double) maxDistance)
        return;
      float x = MathUtils.Distance(curve.xz, position.xz, out float _);
      maxDistance = math.min(x, maxDistance);
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
    public RoadConnectionSystem()
    {
    }

    [BurstCompile]
    private struct CheckRoadConnectionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> m_StartNodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> m_EndNodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> m_SpawnLocationType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedBuilding> m_ConnectedBuildingType;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      public NativeQueue<Entity>.ParallelWriter m_ReplaceRoadConnectionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.SpawnLocation> nativeArray1 = chunk.GetNativeArray<Game.Objects.SpawnLocation>(ref this.m_SpawnLocationType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedBuilding> bufferAccessor = chunk.GetBufferAccessor<ConnectedBuilding>(ref this.m_ConnectedBuildingType);
        if (bufferAccessor.Length != 0)
        {
          for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
          {
            DynamicBuffer<ConnectedBuilding> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ReplaceRoadConnectionQueue.Enqueue(dynamicBuffer[index2].m_Building);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Deleted>(ref this.m_DeletedType))
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<EdgeGeometry> nativeArray2 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<StartNodeGeometry> nativeArray3 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartNodeGeometryType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<EndNodeGeometry> nativeArray4 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndNodeGeometryType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            EdgeGeometry edgeGeometry = nativeArray2[index];
            EdgeNodeGeometry geometry1 = nativeArray3[index].m_Geometry;
            EdgeNodeGeometry geometry2 = nativeArray4[index].m_Geometry;
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
            RoadConnectionSystem.CheckRoadConnectionJob.CheckRoadConnectionIterator iterator = new RoadConnectionSystem.CheckRoadConnectionJob.CheckRoadConnectionIterator()
            {
              m_Bounds = MathUtils.Expand(edgeGeometry.m_Bounds | geometry1.m_Bounds | geometry2.m_Bounds, (float3) 8.4f),
              m_EdgeGeometry = edgeGeometry,
              m_StartGeometry = geometry1,
              m_EndGeometry = geometry2,
              m_BuildingData = this.m_BuildingData,
              m_TransformData = this.m_TransformData,
              m_PrefabRefData = this.m_PrefabRefData,
              m_PrefabBuildingData = this.m_PrefabBuildingData,
              m_EdgeGeometryData = this.m_EdgeGeometryData,
              m_StartNodeGeometryData = this.m_StartNodeGeometryData,
              m_EndNodeGeometryData = this.m_EndNodeGeometryData,
              m_ReplaceRoadConnectionQueue = this.m_ReplaceRoadConnectionQueue
            };
            // ISSUE: reference to a compiler-generated field
            this.m_ObjectSearchTree.Iterate<RoadConnectionSystem.CheckRoadConnectionJob.CheckRoadConnectionIterator>(ref iterator);
          }
        }
        else if (nativeArray1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray5 = chunk.GetNativeArray(this.m_EntityType);
          for (int index = 0; index < nativeArray5.Length; ++index)
          {
            Entity owner = nativeArray5[index];
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(owner, out componentData))
            {
              owner = componentData.m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingData.HasComponent(owner))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ReplaceRoadConnectionQueue.Enqueue(owner);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray6 = chunk.GetNativeArray(this.m_EntityType);
          for (int index = 0; index < nativeArray6.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ReplaceRoadConnectionQueue.Enqueue(nativeArray6[index]);
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

      private struct CheckRoadConnectionIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public EdgeGeometry m_EdgeGeometry;
        public EdgeNodeGeometry m_StartGeometry;
        public EdgeNodeGeometry m_EndGeometry;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<BuildingData> m_PrefabBuildingData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public NativeQueue<Entity>.ParallelWriter m_ReplaceRoadConnectionQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity buildingEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) || !this.m_BuildingData.HasComponent(buildingEntity))
            return;
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TransformData[buildingEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BuildingData buildingData = this.m_PrefabBuildingData[this.m_PrefabRefData[buildingEntity].m_Prefab];
          if ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.NoRoadConnection) != (Game.Prefabs.BuildingFlags) 0)
            return;
          float3 frontPosition = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Bounds.xz, frontPosition.xz))
            return;
          float maxDistance = 8.4f;
          bool canBeOnRoad = (buildingData.m_Flags & Game.Prefabs.BuildingFlags.CanBeOnRoad) > (Game.Prefabs.BuildingFlags) 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          RoadConnectionSystem.CheckDistance(this.m_EdgeGeometry, this.m_StartGeometry, this.m_EndGeometry, frontPosition, canBeOnRoad, ref maxDistance);
          if ((double) maxDistance >= 8.3999996185302734)
            return;
          // ISSUE: reference to a compiler-generated field
          Building building = this.m_BuildingData[buildingEntity];
          if (building.m_RoadEdge != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[building.m_RoadEdge];
            // ISSUE: reference to a compiler-generated field
            EdgeNodeGeometry geometry1 = this.m_StartNodeGeometryData[building.m_RoadEdge].m_Geometry;
            // ISSUE: reference to a compiler-generated field
            EdgeNodeGeometry geometry2 = this.m_EndNodeGeometryData[building.m_RoadEdge].m_Geometry;
            float num1 = 8.4f;
            EdgeNodeGeometry startGeometry = geometry1;
            EdgeNodeGeometry endGeometry = geometry2;
            float3 position = frontPosition;
            int num2 = canBeOnRoad ? 1 : 0;
            ref float local = ref num1;
            // ISSUE: reference to a compiler-generated method
            RoadConnectionSystem.CheckDistance(edgeGeometry, startGeometry, endGeometry, position, num2 != 0, ref local);
            if ((double) maxDistance >= (double) num1)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_ReplaceRoadConnectionQueue.Enqueue(buildingEntity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ReplaceRoadConnectionQueue.Enqueue(buildingEntity);
          }
        }
      }
    }

    [BurstCompile]
    private struct FillReplacementListJob : IJob
    {
      public NativeQueue<Entity> m_ReplaceRoadConnectionQueue;
      public NativeList<RoadConnectionSystem.ReplaceRoad> m_ReplaceRoadConnection;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_ReplaceRoadConnectionQueue.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_ReplaceRoadConnection.ResizeUninitialized(count);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ReplaceRoadConnection[index] = new RoadConnectionSystem.ReplaceRoad(this.m_ReplaceRoadConnectionQueue.Dequeue());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ReplaceRoadConnection.Sort<RoadConnectionSystem.ReplaceRoad>();
        Entity building = Entity.Null;
        int num = 0;
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_ReplaceRoadConnection.Length)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          RoadConnectionSystem.ReplaceRoad replaceRoad = this.m_ReplaceRoadConnection[num++];
          // ISSUE: reference to a compiler-generated field
          if (replaceRoad.m_Building != building)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ReplaceRoadConnection[index1++] = replaceRoad;
            // ISSUE: reference to a compiler-generated field
            building = replaceRoad.m_Building;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index1 >= this.m_ReplaceRoadConnection.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ReplaceRoadConnection.RemoveRange(index1, this.m_ReplaceRoadConnection.Length - index1);
      }
    }

    private struct ReplaceRoad : IComparable<RoadConnectionSystem.ReplaceRoad>
    {
      public Entity m_Building;
      public Entity m_NewRoad;
      public float3 m_FrontPos;
      public float m_CurvePos;
      public bool m_Deleted;
      public bool m_Required;

      public ReplaceRoad(Entity building)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Building = building;
        // ISSUE: reference to a compiler-generated field
        this.m_NewRoad = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_FrontPos = new float3();
        // ISSUE: reference to a compiler-generated field
        this.m_CurvePos = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Deleted = false;
        // ISSUE: reference to a compiler-generated field
        this.m_Required = false;
      }

      public int CompareTo(RoadConnectionSystem.ReplaceRoad other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Building.Index - other.m_Building.Index;
      }
    }

    [BurstCompile]
    private struct FindRoadConnectionJob : IJobParallelForDefer
    {
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabNetCompositionData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<BackSide> m_BackSideData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_UpdatedNetChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public NativeArray<RoadConnectionSystem.ReplaceRoad> m_ReplaceRoadConnection;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        RoadConnectionSystem.ReplaceRoad replaceRoad = this.m_ReplaceRoadConnection[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_DeletedData.HasComponent(replaceRoad.m_Building))
        {
          // ISSUE: reference to a compiler-generated field
          replaceRoad.m_Deleted = true;
          // ISSUE: reference to a compiler-generated field
          this.m_ReplaceRoadConnection[index] = replaceRoad;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BuildingData buildingData = this.m_PrefabBuildingData[this.m_PrefabRefData[replaceRoad.m_Building].m_Prefab];
          BackSide backSide = new BackSide();
          if ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.NoRoadConnection) == (Game.Prefabs.BuildingFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[replaceRoad.m_Building];
            float3 frontPosition1 = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
            // ISSUE: reference to a compiler-generated field
            replaceRoad.m_Required = (buildingData.m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) > (Game.Prefabs.BuildingFlags) 0;
            bool flag1 = (buildingData.m_Flags & Game.Prefabs.BuildingFlags.CanBeOnRoad) > (Game.Prefabs.BuildingFlags) 0;
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
            RoadConnectionSystem.FindRoadConnectionJob.FindRoadConnectionIterator iterator = new RoadConnectionSystem.FindRoadConnectionJob.FindRoadConnectionIterator()
            {
              m_Bounds = new Bounds3(),
              m_MinDistance = float.MaxValue,
              m_BestCurvePos = 0.0f,
              m_BestRoad = Entity.Null,
              m_FrontPosition = transform.m_Position,
              m_CanBeOnRoad = flag1,
              m_ConnectedBuildings = this.m_ConnectedBuildings,
              m_CurveData = this.m_CurveData,
              m_CompositionData = this.m_CompositionData,
              m_EdgeGeometryData = this.m_EdgeGeometryData,
              m_StartNodeGeometryData = this.m_StartNodeGeometryData,
              m_EndNodeGeometryData = this.m_EndNodeGeometryData,
              m_PrefabNetCompositionData = this.m_PrefabNetCompositionData,
              m_DeletedData = this.m_DeletedData
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubNets.HasBuffer(replaceRoad.m_Building))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Net.SubNet> subNet1 = this.m_SubNets[replaceRoad.m_Building];
              for (int index1 = 0; index1 < subNet1.Length; ++index1)
              {
                Entity subNet2 = subNet1[index1].m_SubNet;
                // ISSUE: reference to a compiler-generated field
                if (!this.m_DeletedData.HasComponent(subNet2))
                {
                  // ISSUE: reference to a compiler-generated method
                  iterator.CheckEdge(subNet2);
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_BestRoad == Entity.Null && this.m_TempData.HasComponent(replaceRoad.m_Building))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Temp temp = this.m_TempData[replaceRoad.m_Building];
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubNets.HasBuffer(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Net.SubNet> subNet3 = this.m_SubNets[temp.m_Original];
                for (int index2 = 0; index2 < subNet3.Length; ++index2)
                {
                  Entity subNet4 = subNet3[index2].m_SubNet;
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_DeletedData.HasComponent(subNet4))
                  {
                    // ISSUE: reference to a compiler-generated method
                    iterator.CheckEdge(subNet4);
                  }
                }
              }
            }
            bool flag2 = false;
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_BestRoad == Entity.Null)
            {
              float num = 8.4f;
              // ISSUE: reference to a compiler-generated field
              iterator.m_Bounds = new Bounds3(frontPosition1 - num, frontPosition1 + num);
              // ISSUE: reference to a compiler-generated field
              iterator.m_MinDistance = num;
              // ISSUE: reference to a compiler-generated field
              iterator.m_FrontPosition = frontPosition1;
              // ISSUE: reference to a compiler-generated field
              this.m_NetSearchTree.Iterate<RoadConnectionSystem.FindRoadConnectionJob.FindRoadConnectionIterator>(ref iterator);
              // ISSUE: reference to a compiler-generated field
              for (int index3 = 0; index3 < this.m_UpdatedNetChunks.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                NativeArray<Entity> nativeArray = this.m_UpdatedNetChunks[index3].GetNativeArray(this.m_EntityType);
                for (int index4 = 0; index4 < nativeArray.Length; ++index4)
                {
                  // ISSUE: reference to a compiler-generated method
                  iterator.CheckEdge(nativeArray[index4]);
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              flag2 = (buildingData.m_Flags & Game.Prefabs.BuildingFlags.BackAccess) != (Game.Prefabs.BuildingFlags) 0 && this.m_TempData.HasComponent(replaceRoad.m_Building);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            replaceRoad.m_NewRoad = iterator.m_BestRoad;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            replaceRoad.m_FrontPos = iterator.m_FrontPosition;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            replaceRoad.m_CurvePos = iterator.m_BestCurvePos;
            if (flag2)
            {
              float3 frontPosition2 = BuildingUtils.CalculateFrontPosition(transform, -buildingData.m_LotSize.y);
              float num = 8.4f;
              // ISSUE: reference to a compiler-generated field
              iterator.m_BestRoad = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              iterator.m_BestCurvePos = 0.0f;
              // ISSUE: reference to a compiler-generated field
              iterator.m_Bounds = new Bounds3(frontPosition2 - num, frontPosition2 + num);
              // ISSUE: reference to a compiler-generated field
              iterator.m_MinDistance = num;
              // ISSUE: reference to a compiler-generated field
              iterator.m_FrontPosition = frontPosition2;
              // ISSUE: reference to a compiler-generated field
              this.m_NetSearchTree.Iterate<RoadConnectionSystem.FindRoadConnectionJob.FindRoadConnectionIterator>(ref iterator);
              // ISSUE: reference to a compiler-generated field
              for (int index5 = 0; index5 < this.m_UpdatedNetChunks.Length; ++index5)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                NativeArray<Entity> nativeArray = this.m_UpdatedNetChunks[index5].GetNativeArray(this.m_EntityType);
                for (int index6 = 0; index6 < nativeArray.Length; ++index6)
                {
                  // ISSUE: reference to a compiler-generated method
                  iterator.CheckEdge(nativeArray[index6]);
                }
              }
              // ISSUE: reference to a compiler-generated field
              backSide.m_RoadEdge = iterator.m_BestRoad;
              // ISSUE: reference to a compiler-generated field
              backSide.m_CurvePosition = iterator.m_BestCurvePos;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_BackSideData.HasComponent(replaceRoad.m_Building))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_BackSideData[replaceRoad.m_Building] = backSide;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ReplaceRoadConnection[index] = replaceRoad;
        }
      }

      public struct FindRoadConnectionIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float m_MinDistance;
        public float m_BestCurvePos;
        public Entity m_BestRoad;
        public float3 m_FrontPosition;
        public bool m_CanBeOnRoad;
        public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public ComponentLookup<NetCompositionData> m_PrefabNetCompositionData;
        public ComponentLookup<Deleted> m_DeletedData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) || this.m_DeletedData.HasComponent(edgeEntity))
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckEdge(edgeEntity);
        }

        public void CheckEdge(Entity edgeEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ConnectedBuildings.HasBuffer(edgeEntity))
            return;
          NetCompositionData componentData1 = new NetCompositionData();
          Composition componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompositionData.TryGetComponent(edgeEntity, out componentData2) && this.m_PrefabNetCompositionData.TryGetComponent(componentData2.m_Edge, out componentData1) && (componentData1.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) != (CompositionFlags.General) 0)
            return;
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edgeEntity];
          // ISSUE: reference to a compiler-generated field
          EdgeNodeGeometry geometry1 = this.m_StartNodeGeometryData[edgeEntity].m_Geometry;
          // ISSUE: reference to a compiler-generated field
          EdgeNodeGeometry geometry2 = this.m_EndNodeGeometryData[edgeEntity].m_Geometry;
          // ISSUE: reference to a compiler-generated field
          float minDistance = this.m_MinDistance;
          EdgeNodeGeometry startGeometry = geometry1;
          EdgeNodeGeometry endGeometry = geometry2;
          // ISSUE: reference to a compiler-generated field
          float3 frontPosition = this.m_FrontPosition;
          // ISSUE: reference to a compiler-generated field
          int num1 = this.m_CanBeOnRoad ? 1 : 0;
          ref float local = ref minDistance;
          // ISSUE: reference to a compiler-generated method
          RoadConnectionSystem.CheckDistance(edgeGeometry, startGeometry, endGeometry, frontPosition, num1 != 0, ref local);
          // ISSUE: reference to a compiler-generated field
          if ((double) minDistance >= (double) this.m_MinDistance)
            return;
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[edgeEntity];
          float t;
          // ISSUE: reference to a compiler-generated field
          double num2 = (double) MathUtils.Distance(curve.m_Bezier.xz, this.m_FrontPosition.xz, out t);
          float3 float3 = MathUtils.Position(curve.m_Bezier, t);
          // ISSUE: reference to a compiler-generated field
          if ((((double) math.dot(MathUtils.Right(MathUtils.Tangent(curve.m_Bezier, t).xz), this.m_FrontPosition.xz - float3.xz) >= 0.0 ? (int) componentData1.m_Flags.m_Right : (int) componentData1.m_Flags.m_Left) & 3) != 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Bounds = new Bounds3(this.m_FrontPosition - minDistance, this.m_FrontPosition + minDistance);
          // ISSUE: reference to a compiler-generated field
          this.m_MinDistance = minDistance;
          // ISSUE: reference to a compiler-generated field
          this.m_BestCurvePos = t;
          // ISSUE: reference to a compiler-generated field
          this.m_BestRoad = edgeEntity;
        }
      }
    }

    [BurstCompile]
    private struct ReplaceRoadConnectionJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Created> m_CreatedData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      public ComponentLookup<Building> m_BuildingData;
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      [ReadOnly]
      public EntityArchetype m_RoadConnectionEventArchetype;
      [ReadOnly]
      public NativeList<RoadConnectionSystem.ReplaceRoad> m_ReplaceRoadConnection;
      [ReadOnly]
      public TrafficConfigurationData m_TrafficConfigurationData;
      public EntityCommandBuffer m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;
      public SourceUpdateData m_SourceUpdateData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ReplaceRoadConnection.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          RoadConnectionSystem.ReplaceRoad replaceRoad = this.m_ReplaceRoadConnection[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Building building = this.m_BuildingData[replaceRoad.m_Building];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_CreatedData.HasComponent(replaceRoad.m_Building);
          // ISSUE: reference to a compiler-generated field
          if (replaceRoad.m_NewRoad != building.m_RoadEdge | flag)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(replaceRoad.m_Building))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (building.m_RoadEdge == Entity.Null && replaceRoad.m_NewRoad != Entity.Null && this.m_TempData[replaceRoad.m_Building].m_Original == Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_SourceUpdateData.AddSnap();
              }
              // ISSUE: reference to a compiler-generated field
              building.m_RoadEdge = replaceRoad.m_NewRoad;
              // ISSUE: reference to a compiler-generated field
              building.m_CurvePosition = replaceRoad.m_CurvePos;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_BuildingData[replaceRoad.m_Building] = building;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (building.m_RoadEdge != Entity.Null != (replaceRoad.m_NewRoad != Entity.Null))
              {
                // ISSUE: reference to a compiler-generated field
                if (replaceRoad.m_NewRoad != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(replaceRoad.m_Building, this.m_TrafficConfigurationData.m_RoadConnectionNotification);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!replaceRoad.m_Deleted && replaceRoad.m_Required)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IconCommandBuffer.Add(replaceRoad.m_Building, this.m_TrafficConfigurationData.m_RoadConnectionNotification, replaceRoad.m_FrontPos, IconPriority.Warning);
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RoadConnectionUpdated>(this.m_CommandBuffer.CreateEntity(this.m_RoadConnectionEventArchetype), new RoadConnectionUpdated()
              {
                m_Building = replaceRoad.m_Building,
                m_Old = flag ? Entity.Null : building.m_RoadEdge,
                m_New = replaceRoad.m_NewRoad
              });
              if (building.m_RoadEdge != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedBuilding>(this.m_ConnectedBuildings[building.m_RoadEdge], new ConnectedBuilding(replaceRoad.m_Building));
              }
              // ISSUE: reference to a compiler-generated field
              building.m_RoadEdge = replaceRoad.m_NewRoad;
              // ISSUE: reference to a compiler-generated field
              building.m_CurvePosition = replaceRoad.m_CurvePos;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_BuildingData[replaceRoad.m_Building] = building;
              // ISSUE: reference to a compiler-generated field
              if (replaceRoad.m_NewRoad != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_ConnectedBuildings[replaceRoad.m_NewRoad].Add(new ConnectedBuilding(replaceRoad.m_Building));
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((double) replaceRoad.m_CurvePos != (double) building.m_CurvePosition)
            {
              // ISSUE: reference to a compiler-generated field
              building.m_CurvePosition = replaceRoad.m_CurvePos;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_BuildingData[replaceRoad.m_Building] = building;
            }
          }
        }
      }
    }

    private struct ConnectionLaneKey : IEquatable<RoadConnectionSystem.ConnectionLaneKey>
    {
      private PathNode m_Node1;
      private PathNode m_Node2;

      public ConnectionLaneKey(PathNode node1, PathNode node2)
      {
        if (node1.GetOrder(node2))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Node1 = node2;
          // ISSUE: reference to a compiler-generated field
          this.m_Node2 = node1;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Node1 = node1;
          // ISSUE: reference to a compiler-generated field
          this.m_Node2 = node2;
        }
      }

      public bool Equals(RoadConnectionSystem.ConnectionLaneKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Node1.Equals(other.m_Node1) && this.m_Node2.Equals(other.m_Node2);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (17 * 31 + this.m_Node1.GetHashCode()) * 31 + this.m_Node2.GetHashCode();
      }
    }

    private struct SpawnLocationData
    {
      public Entity m_Entity;
      public Entity m_Original;
      public float3 m_Position;
      public Game.Prefabs.SpawnLocationData m_PrefabData;
      public int m_Group;
    }

    [BurstCompile]
    private struct UpdateSecondaryLanesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumerData;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumerData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.UtilityObject> m_UtilityObjectData;
      [ReadOnly]
      public ComponentLookup<Secondary> m_SecondaryData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.UtilityLane> m_UtilityLaneData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Net.SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.UtilityObjectData> m_PrefabUtilityObjectData;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> m_PrefabNetLaneArchetypeData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public NativeList<Entity> m_ConnectionPrefabs;
      [ReadOnly]
      public NativeList<RoadConnectionSystem.ReplaceRoad> m_ReplaceRoadConnection;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        RoadConnectionSystem.ReplaceRoad replaceRoad = this.m_ReplaceRoadConnection[index];
        Bezier4x3 electricityCurve;
        Entity electricityLanePrefab;
        Entity electricityObjectPrefab;
        PathNode electricityNode;
        Bezier4x3 sewageCurve;
        Entity sewageLanePrefab;
        Entity sewageObjectPrefab;
        PathNode sewageNode;
        Bezier4x3 waterCurve;
        Entity waterLanePrefab;
        Entity waterObjectPrefab;
        PathNode waterNode;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FindRoadUtilityLanes(replaceRoad.m_Building, replaceRoad.m_NewRoad, replaceRoad.m_FrontPos, out electricityCurve, out electricityLanePrefab, out electricityObjectPrefab, out electricityNode, out sewageCurve, out sewageLanePrefab, out sewageObjectPrefab, out sewageNode, out waterCurve, out waterLanePrefab, out waterObjectPrefab, out waterNode);
        Temp temp = new Temp();
        Temp subTemp = new Temp();
        bool isTemp = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.HasComponent(replaceRoad.m_Building))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          temp = this.m_TempData[replaceRoad.m_Building];
          subTemp.m_Flags = temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate);
          if ((temp.m_Flags & (TempFlags.Replace | TempFlags.Upgrade)) != (TempFlags) 0)
            subTemp.m_Flags |= TempFlags.Modify;
          isTemp = true;
        }
        NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> originalConnections;
        Entity originalElectricityLane;
        Entity originalSewageLane;
        Entity originalWaterLane;
        // ISSUE: reference to a compiler-generated method
        this.FindOriginalLanes(temp.m_Original, out originalConnections, out originalElectricityLane, out originalSewageLane, out originalWaterLane);
        Entity originalElectricityObject;
        Entity originalSewageObject;
        Entity originalWaterObject;
        // ISSUE: reference to a compiler-generated method
        this.FindOriginalObjects(temp.m_Original, out originalElectricityObject, out originalSewageObject, out originalWaterObject);
        // ISSUE: reference to a compiler-generated method
        float3 objectPosition1 = this.CalculateObjectPosition(electricityCurve, electricityObjectPrefab, true);
        // ISSUE: reference to a compiler-generated method
        float3 objectPosition2 = this.CalculateObjectPosition(sewageCurve, sewageObjectPrefab, true);
        // ISSUE: reference to a compiler-generated method
        float3 objectPosition3 = this.CalculateObjectPosition(waterCurve, waterObjectPrefab, true);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateLanes(index, replaceRoad.m_Building, replaceRoad.m_FrontPos, replaceRoad.m_Deleted, isTemp, subTemp, originalConnections, electricityCurve, electricityLanePrefab, electricityNode, originalElectricityLane, sewageCurve, sewageLanePrefab, sewageNode, originalSewageLane, waterCurve, waterLanePrefab, waterNode, originalWaterLane);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateObjects(index, replaceRoad.m_Building, replaceRoad.m_FrontPos, isTemp, subTemp, electricityObjectPrefab, originalElectricityObject, objectPosition1, sewageObjectPrefab, originalSewageObject, objectPosition2, waterObjectPrefab, originalWaterObject, objectPosition3);
        if (!originalConnections.IsCreated)
          return;
        originalConnections.Dispose();
      }

      private void UpdateObjects(
        int jobIndex,
        Entity building,
        float3 connectPos,
        bool isTemp,
        Temp subTemp,
        Entity electricityObjectPrefab,
        Entity originalElectricityObject,
        float3 electricityObjectPosition,
        Entity sewageObjectPrefab,
        Entity originalSewageObject,
        float3 sewageObjectPosition,
        Entity waterObjectPrefab,
        Entity originalWaterObject,
        float3 waterObjectPosition)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(building))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[building];
        for (int index = 0; index < subObject1.Length; ++index)
        {
          Entity subObject2 = subObject1[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_UtilityObjectData.HasComponent(subObject2) && this.m_SecondaryData.HasComponent(subObject2))
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[subObject2];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subObject2];
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.UtilityObjectData utilityObjectData = this.m_PrefabUtilityObjectData[prefabRef.m_Prefab];
            if ((utilityObjectData.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None)
            {
              if (prefabRef.m_Prefab != electricityObjectPrefab)
              {
                // ISSUE: reference to a compiler-generated method
                this.DeleteObject(jobIndex, subObject2);
              }
              else
              {
                electricityObjectPrefab = Entity.Null;
                // ISSUE: reference to a compiler-generated field
                if (this.m_DeletedData.HasComponent(subObject2))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, subObject2);
                }
                if (!transform.m_Position.Equals(electricityObjectPosition))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateObject(jobIndex, subObject2, electricityObjectPosition, isTemp, subTemp, originalElectricityObject);
                }
              }
            }
            else if ((utilityObjectData.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None)
            {
              if (prefabRef.m_Prefab != sewageObjectPrefab)
              {
                // ISSUE: reference to a compiler-generated method
                this.DeleteObject(jobIndex, subObject2);
              }
              else
              {
                sewageObjectPrefab = Entity.Null;
                // ISSUE: reference to a compiler-generated field
                if (this.m_DeletedData.HasComponent(subObject2))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, subObject2);
                }
                if (!transform.m_Position.Equals(sewageObjectPosition))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateObject(jobIndex, subObject2, sewageObjectPosition, isTemp, subTemp, originalSewageObject);
                }
              }
            }
            else if ((utilityObjectData.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None)
            {
              if (prefabRef.m_Prefab != waterObjectPrefab)
              {
                // ISSUE: reference to a compiler-generated method
                this.DeleteObject(jobIndex, subObject2);
              }
              else
              {
                waterObjectPrefab = Entity.Null;
                // ISSUE: reference to a compiler-generated field
                if (this.m_DeletedData.HasComponent(subObject2))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, subObject2);
                }
                if (!transform.m_Position.Equals(waterObjectPosition))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateObject(jobIndex, subObject2, waterObjectPosition, isTemp, subTemp, originalWaterObject);
                }
              }
            }
          }
        }
        if (electricityObjectPrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateObject(jobIndex, building, electricityObjectPrefab, electricityObjectPosition, connectPos, isTemp, subTemp, originalElectricityObject);
        }
        if (sewageObjectPrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateObject(jobIndex, building, sewageObjectPrefab, sewageObjectPosition, connectPos, isTemp, subTemp, originalSewageObject);
        }
        if (!(waterObjectPrefab != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated method
        this.CreateObject(jobIndex, building, waterObjectPrefab, waterObjectPosition, connectPos, isTemp, subTemp, originalWaterObject);
      }

      private void UpdateLanes(
        int jobIndex,
        Entity building,
        float3 connectPos,
        bool isDeleted,
        bool isTemp,
        Temp subTemp,
        NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> originalConnections,
        Bezier4x3 electricityCurve,
        Entity electricityLanePrefab,
        PathNode electricityNode,
        Entity originalElectricityLane,
        Bezier4x3 sewageCurve,
        Entity sewageLanePrefab,
        PathNode sewageNode,
        Entity originalSewageLane,
        Bezier4x3 waterCurve,
        Entity waterLanePrefab,
        PathNode waterNode,
        Entity originalWaterLane)
      {
        NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> oldConnections = new NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity>();
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[building];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SecondaryLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_UtilityLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane2];
              // ISSUE: reference to a compiler-generated field
              Lane lane = this.m_LaneData[subLane2];
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[subLane2];
              // ISSUE: reference to a compiler-generated field
              UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[prefabRef.m_Prefab];
              if ((utilityLaneData.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None)
              {
                if (prefabRef.m_Prefab != electricityLanePrefab)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DeleteLane(jobIndex, subLane2);
                }
                else
                {
                  electricityLanePrefab = Entity.Null;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DeletedData.HasComponent(subLane2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, subLane2);
                  }
                  if (!curve.m_Bezier.Equals(electricityCurve) || !lane.m_EndNode.Equals(electricityNode))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateLane(jobIndex, subLane2, electricityCurve, electricityNode, isTemp, subTemp, originalElectricityLane);
                  }
                }
              }
              else if ((utilityLaneData.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None)
              {
                if (prefabRef.m_Prefab != sewageLanePrefab)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DeleteLane(jobIndex, subLane2);
                }
                else
                {
                  sewageLanePrefab = Entity.Null;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DeletedData.HasComponent(subLane2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, subLane2);
                  }
                  if (!curve.m_Bezier.Equals(sewageCurve) || !lane.m_EndNode.Equals(sewageNode))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateLane(jobIndex, subLane2, sewageCurve, sewageNode, isTemp, subTemp, originalSewageLane);
                  }
                }
              }
              else if ((utilityLaneData.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None)
              {
                if (prefabRef.m_Prefab != waterLanePrefab)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DeleteLane(jobIndex, subLane2);
                }
                else
                {
                  waterLanePrefab = Entity.Null;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DeletedData.HasComponent(subLane2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, subLane2);
                  }
                  if (!curve.m_Bezier.Equals(waterCurve) || !lane.m_EndNode.Equals(waterNode))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateLane(jobIndex, subLane2, waterCurve, waterNode, isTemp, subTemp, originalWaterLane);
                  }
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                Lane lane = this.m_LaneData[subLane2];
                if (!oldConnections.IsCreated)
                  oldConnections = new NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity>(subLane1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                // ISSUE: object of a compiler-generated type is created
                oldConnections.TryAdd(new RoadConnectionSystem.ConnectionLaneKey(lane.m_StartNode, lane.m_EndNode), subLane2);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.DeleteLane(jobIndex, subLane2);
              }
            }
          }
        }
        if (electricityLanePrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateLane(jobIndex, building, 65530, electricityLanePrefab, electricityCurve, electricityNode, connectPos, isTemp, subTemp, originalElectricityLane);
        }
        if (sewageLanePrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateLane(jobIndex, building, 65532, sewageLanePrefab, sewageCurve, sewageNode, connectPos, isTemp, subTemp, originalSewageLane);
        }
        if (waterLanePrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateLane(jobIndex, building, 65534, waterLanePrefab, waterCurve, waterNode, connectPos, isTemp, subTemp, originalWaterLane);
        }
        if (!isDeleted)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SpawnLocationElement> spawnLocation1 = this.m_SpawnLocations[building];
          bool flag = spawnLocation1.Length >= 2;
          if (flag)
          {
            Entity entity = building;
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              entity = this.m_OwnerData[entity].m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnLocations.HasBuffer(entity))
              {
                flag = false;
                break;
              }
            }
          }
          if (flag)
          {
            NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> newConnections = new NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity>(spawnLocation1.Length * 4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            NativeArray<RoadConnectionSystem.SpawnLocationData> nativeArray = new NativeArray<RoadConnectionSystem.SpawnLocationData>(spawnLocation1.Length, Allocator.Temp);
            for (int index = 0; index < spawnLocation1.Length; ++index)
            {
              if (spawnLocation1[index].m_Type == SpawnLocationType.SpawnLocation)
              {
                Entity spawnLocation2 = spawnLocation1[index].m_SpawnLocation;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.HasComponent(spawnLocation2))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Game.Prefabs.SpawnLocationData spawnLocationData = this.m_PrefabSpawnLocationData[this.m_PrefabRefData[spawnLocation2].m_Prefab];
                  if (spawnLocationData.m_ActivityMask.m_Mask == 0U && spawnLocationData.m_ConnectionType != RouteConnectionType.Air && spawnLocationData.m_ConnectionType != RouteConnectionType.Track)
                  {
                    Temp componentData1;
                    // ISSUE: reference to a compiler-generated field
                    this.m_TempData.TryGetComponent(spawnLocation2, out componentData1);
                    Game.Objects.SpawnLocation componentData2;
                    // ISSUE: reference to a compiler-generated field
                    this.m_SpawnLocationData.TryGetComponent(spawnLocation2, out componentData2);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: object of a compiler-generated type is created
                    nativeArray[index] = new RoadConnectionSystem.SpawnLocationData()
                    {
                      m_Entity = spawnLocation2,
                      m_Original = componentData1.m_Original,
                      m_Position = this.m_TransformData[spawnLocation2].m_Position,
                      m_PrefabData = spawnLocationData,
                      m_Group = componentData2.m_GroupIndex
                    };
                  }
                }
              }
            }
            for (int index1 = 0; index1 < nativeArray.Length; ++index1)
            {
              // ISSUE: variable of a compiler-generated type
              RoadConnectionSystem.SpawnLocationData spawnLocationData1 = nativeArray[index1];
              // ISSUE: reference to a compiler-generated field
              if (spawnLocationData1.m_PrefabData.m_ConnectionType != RouteConnectionType.None)
              {
                float3 maxValue1 = (float3) float.MaxValue;
                float3 maxValue2 = (float3) float.MaxValue;
                int3 int3_1 = (int3) -1;
                int3 int3_2 = (int3) -1;
                for (int index2 = 0; index2 < nativeArray.Length; ++index2)
                {
                  if (index2 != index1)
                  {
                    // ISSUE: variable of a compiler-generated type
                    RoadConnectionSystem.SpawnLocationData spawnLocationData = nativeArray[index2];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (spawnLocationData.m_Group == spawnLocationData1.m_Group)
                    {
                      // ISSUE: reference to a compiler-generated field
                      switch (spawnLocationData1.m_PrefabData.m_ConnectionType)
                      {
                        case RouteConnectionType.Road:
                        case RouteConnectionType.Air:
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (spawnLocationData.m_PrefabData.m_ConnectionType != RouteConnectionType.Road && spawnLocationData.m_PrefabData.m_ConnectionType != RouteConnectionType.Air || (spawnLocationData1.m_PrefabData.m_RoadTypes & spawnLocationData.m_PrefabData.m_RoadTypes) == RoadTypes.None)
                            continue;
                          break;
                        case RouteConnectionType.Pedestrian:
                          // ISSUE: reference to a compiler-generated field
                          if (spawnLocationData.m_PrefabData.m_ConnectionType == RouteConnectionType.Pedestrian)
                            break;
                          continue;
                        case RouteConnectionType.Track:
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (spawnLocationData.m_PrefabData.m_ConnectionType != RouteConnectionType.Track || (spawnLocationData1.m_PrefabData.m_TrackTypes & spawnLocationData.m_PrefabData.m_TrackTypes) == TrackTypes.None)
                            continue;
                          break;
                        case RouteConnectionType.Cargo:
                          // ISSUE: reference to a compiler-generated field
                          if (spawnLocationData.m_PrefabData.m_ConnectionType == RouteConnectionType.Cargo)
                            break;
                          continue;
                        case RouteConnectionType.Parking:
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (spawnLocationData.m_PrefabData.m_ConnectionType != RouteConnectionType.Parking || (spawnLocationData1.m_PrefabData.m_RoadTypes & spawnLocationData.m_PrefabData.m_RoadTypes) == RoadTypes.None)
                            continue;
                          break;
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      float3 x = spawnLocationData.m_Position - spawnLocationData1.m_Position;
                      float distance = math.length(x);
                      float3 float3 = math.abs(x);
                      bool3 bool3 = float3.xxy >= float3.yzz;
                      if (math.all(bool3.xy))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CheckDistance(x.x, distance, index2, ref maxValue1.x, ref maxValue2.x, ref int3_1.x, ref int3_2.x);
                      }
                      else if (bool3.z)
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CheckDistance(x.y, distance, index2, ref maxValue1.y, ref maxValue2.y, ref int3_1.y, ref int3_2.y);
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CheckDistance(x.z, distance, index2, ref maxValue1.z, ref maxValue2.z, ref int3_1.z, ref int3_2.z);
                      }
                    }
                  }
                }
                float num1 = float.MaxValue;
                int index3 = -1;
                // ISSUE: reference to a compiler-generated field
                if (spawnLocationData1.m_PrefabData.m_ConnectionType == RouteConnectionType.Parking)
                {
                  for (int index4 = 0; index4 < nativeArray.Length; ++index4)
                  {
                    // ISSUE: variable of a compiler-generated type
                    RoadConnectionSystem.SpawnLocationData spawnLocationData = nativeArray[index4];
                    // ISSUE: reference to a compiler-generated field
                    if (spawnLocationData.m_PrefabData.m_ConnectionType == RouteConnectionType.Pedestrian)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      float num2 = math.length(spawnLocationData.m_Position - spawnLocationData1.m_Position);
                      if ((double) num2 < (double) num1)
                      {
                        num1 = num2;
                        index3 = index4;
                      }
                    }
                  }
                }
                if (int3_1.x != -1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, building, isTemp, subTemp, spawnLocationData1, nativeArray[int3_1.x], originalConnections, oldConnections, newConnections);
                }
                if (int3_1.y != -1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, building, isTemp, subTemp, spawnLocationData1, nativeArray[int3_1.y], originalConnections, oldConnections, newConnections);
                }
                if (int3_1.z != -1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, building, isTemp, subTemp, spawnLocationData1, nativeArray[int3_1.z], originalConnections, oldConnections, newConnections);
                }
                if (int3_2.x != -1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, building, isTemp, subTemp, spawnLocationData1, nativeArray[int3_2.x], originalConnections, oldConnections, newConnections);
                }
                if (int3_2.y != -1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, building, isTemp, subTemp, spawnLocationData1, nativeArray[int3_2.y], originalConnections, oldConnections, newConnections);
                }
                if (int3_2.z != -1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, building, isTemp, subTemp, spawnLocationData1, nativeArray[int3_2.z], originalConnections, oldConnections, newConnections);
                }
                if (index3 != -1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, building, isTemp, subTemp, spawnLocationData1, nativeArray[index3], originalConnections, oldConnections, newConnections);
                }
              }
            }
            nativeArray.Dispose();
            newConnections.Dispose();
          }
        }
        if (!oldConnections.IsCreated)
          return;
        NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity>.Enumerator enumerator = oldConnections.GetEnumerator();
        while (enumerator.MoveNext())
        {
          // ISSUE: reference to a compiler-generated method
          this.DeleteLane(jobIndex, enumerator.Current.Value);
        }
        enumerator.Dispose();
        oldConnections.Dispose();
      }

      private void CheckConnection(
        int jobIndex,
        Entity building,
        bool isTemp,
        Temp temp,
        RoadConnectionSystem.SpawnLocationData spawnLocationData1,
        RoadConnectionSystem.SpawnLocationData spawnLocationData2,
        NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> originalConnections,
        NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> oldConnections,
        NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> newConnections)
      {
        // ISSUE: reference to a compiler-generated field
        PathNode node1 = new PathNode(spawnLocationData1.m_Entity, (ushort) 0);
        // ISSUE: reference to a compiler-generated field
        PathNode node2 = new PathNode(spawnLocationData2.m_Entity, (ushort) 0);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RoadConnectionSystem.ConnectionLaneKey key1 = new RoadConnectionSystem.ConnectionLaneKey(node1, node2);
        if (newConnections.ContainsKey(key1))
          return;
        Curve component1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        component1.m_Bezier = NetUtils.StraightCurve(spawnLocationData1.m_Position, spawnLocationData2.m_Position);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        component1.m_Length = math.distance(spawnLocationData1.m_Position, spawnLocationData2.m_Position);
        if (isTemp && originalConnections.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          RoadConnectionSystem.ConnectionLaneKey key2 = new RoadConnectionSystem.ConnectionLaneKey(new PathNode(spawnLocationData1.m_Original, (ushort) 0), new PathNode(spawnLocationData2.m_Original, (ushort) 0));
          Entity entity;
          if (originalConnections.TryGetValue(key2, out entity))
          {
            originalConnections.Remove(key2);
            temp.m_Original = entity;
          }
        }
        Entity entity1;
        if (oldConnections.IsCreated && oldConnections.TryGetValue(key1, out entity1))
        {
          oldConnections.Remove(key1);
          // ISSUE: reference to a compiler-generated field
          if (this.m_DeletedData.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, entity1);
          }
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[entity1];
          if (!component1.m_Bezier.Equals(curve.m_Bezier) && !MathUtils.Invert(component1.m_Bezier).Equals(curve.m_Bezier))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity1, new Updated());
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          newConnections.Add(key1, entity1);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Entity connectionPrefab = this.m_ConnectionPrefabs[0];
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabNetLaneArchetypeData[connectionPrefab];
          Owner component2 = new Owner(building);
          PrefabRef component3 = new PrefabRef(connectionPrefab);
          Game.Net.SecondaryLane component4 = new Game.Net.SecondaryLane();
          Lane component5;
          component5.m_StartNode = node1;
          // ISSUE: reference to a compiler-generated field
          component5.m_MiddleNode = new PathNode(spawnLocationData1.m_Entity, (ushort) 3);
          component5.m_EndNode = node2;
          Game.Net.ConnectionLane component6 = new Game.Net.ConnectionLane();
          component6.m_Flags = ConnectionLaneFlags.Inside;
          // ISSUE: reference to a compiler-generated field
          switch (spawnLocationData1.m_PrefabData.m_ConnectionType)
          {
            case RouteConnectionType.Road:
            case RouteConnectionType.Air:
              component6.m_Flags |= ConnectionLaneFlags.Road;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component6.m_RoadTypes = spawnLocationData1.m_PrefabData.m_RoadTypes | spawnLocationData2.m_PrefabData.m_RoadTypes;
              break;
            case RouteConnectionType.Pedestrian:
              component6.m_Flags |= ConnectionLaneFlags.Pedestrian;
              break;
            case RouteConnectionType.Track:
              component6.m_Flags |= ConnectionLaneFlags.Track;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component6.m_TrackTypes = spawnLocationData1.m_PrefabData.m_TrackTypes | spawnLocationData2.m_PrefabData.m_TrackTypes;
              break;
            case RouteConnectionType.Cargo:
              component6.m_Flags |= ConnectionLaneFlags.AllowCargo;
              break;
            case RouteConnectionType.Parking:
              // ISSUE: reference to a compiler-generated field
              if (spawnLocationData2.m_PrefabData.m_ConnectionType == RouteConnectionType.Pedestrian)
              {
                component6.m_Flags |= ConnectionLaneFlags.Parking;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                component6.m_RoadTypes = spawnLocationData1.m_PrefabData.m_RoadTypes | spawnLocationData2.m_PrefabData.m_RoadTypes;
                break;
              }
              component6.m_Flags |= ConnectionLaneFlags.Road;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component6.m_RoadTypes = spawnLocationData1.m_PrefabData.m_RoadTypes | spawnLocationData2.m_PrefabData.m_RoadTypes;
              break;
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, laneArchetypeData.m_LaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, component5);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Net.ConnectionLane>(jobIndex, entity2, component6);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Game.Net.SecondaryLane>(jobIndex, entity2, component4);
          if ((component6.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<GarageLane>(jobIndex, entity2, new GarageLane());
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity2, temp);
          }
          newConnections.Add(key1, entity2);
        }
      }

      private void CheckDistance(
        float offset,
        float distance,
        int index,
        ref float bestDistance1,
        ref float bestDistance2,
        ref int bestIndex1,
        ref int bestIndex2)
      {
        if ((double) offset >= 0.0)
        {
          if ((double) distance >= (double) bestDistance1)
            return;
          bestDistance1 = distance;
          bestIndex1 = index;
        }
        else
        {
          if ((double) distance >= (double) bestDistance2)
            return;
          bestDistance2 = distance;
          bestIndex2 = index;
        }
      }

      private float3 CalculateObjectPosition(Bezier4x3 curve, Entity prefab, bool start = false)
      {
        float3 objectPosition = start ? curve.a : curve.d;
        if (prefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
          objectPosition.y -= MathUtils.Center(objectGeometryData.m_Bounds).y;
        }
        return objectPosition;
      }

      private void FindOriginalLanes(
        Entity originalBuilding,
        out NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity> originalConnections,
        out Entity originalElectricityLane,
        out Entity originalSewageLane,
        out Entity originalWaterLane)
      {
        originalConnections = new NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity>();
        originalElectricityLane = new Entity();
        originalSewageLane = new Entity();
        originalWaterLane = new Entity();
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(originalBuilding))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[originalBuilding];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SecondaryLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_UtilityLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane2].m_Prefab];
              if ((utilityLaneData.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None)
                originalElectricityLane = subLane2;
              else if ((utilityLaneData.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None)
                originalSewageLane = subLane2;
              else if ((utilityLaneData.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None)
                originalWaterLane = subLane2;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                Lane lane = this.m_LaneData[subLane2];
                if (!originalConnections.IsCreated)
                  originalConnections = new NativeParallelHashMap<RoadConnectionSystem.ConnectionLaneKey, Entity>(subLane1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                // ISSUE: object of a compiler-generated type is created
                originalConnections.TryAdd(new RoadConnectionSystem.ConnectionLaneKey(lane.m_StartNode, lane.m_EndNode), subLane2);
              }
            }
          }
        }
      }

      private void FindOriginalObjects(
        Entity originalBuilding,
        out Entity originalElectricityObject,
        out Entity originalSewageObject,
        out Entity originalWaterObject)
      {
        originalElectricityObject = new Entity();
        originalSewageObject = new Entity();
        originalWaterObject = new Entity();
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(originalBuilding))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[originalBuilding];
        for (int index = 0; index < subObject1.Length; ++index)
        {
          Entity subObject2 = subObject1[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_UtilityObjectData.HasComponent(subObject2) && this.m_SecondaryData.HasComponent(subObject2))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.UtilityObjectData utilityObjectData = this.m_PrefabUtilityObjectData[this.m_PrefabRefData[subObject2].m_Prefab];
            if ((utilityObjectData.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None)
              originalElectricityObject = subObject2;
            else if ((utilityObjectData.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None)
              originalSewageObject = subObject2;
            else if ((utilityObjectData.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None)
              originalWaterObject = subObject2;
          }
        }
      }

      private void FindRoadUtilityLanes(
        Entity building,
        Entity road,
        float3 connectionPosition,
        out Bezier4x3 electricityCurve,
        out Entity electricityLanePrefab,
        out Entity electricityObjectPrefab,
        out PathNode electricityNode,
        out Bezier4x3 sewageCurve,
        out Entity sewageLanePrefab,
        out Entity sewageObjectPrefab,
        out PathNode sewageNode,
        out Bezier4x3 waterCurve,
        out Entity waterLanePrefab,
        out Entity waterObjectPrefab,
        out PathNode waterNode)
      {
        electricityCurve = new Bezier4x3();
        electricityLanePrefab = new Entity();
        electricityObjectPrefab = new Entity();
        electricityNode = new PathNode();
        sewageCurve = new Bezier4x3();
        sewageLanePrefab = new Entity();
        sewageObjectPrefab = new Entity();
        sewageNode = new PathNode();
        waterCurve = new Bezier4x3();
        waterLanePrefab = new Entity();
        waterObjectPrefab = new Entity();
        waterNode = new PathNode();
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.TryGetBuffer(road, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[road];
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[building];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BuildingData buildingData = this.m_PrefabBuildingData[this.m_PrefabRefData[building].m_Prefab];
        int3 int3 = new int3();
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElectricityConsumerData.HasComponent(building) && (buildingData.m_Flags & Game.Prefabs.BuildingFlags.HasLowVoltageNode) == (Game.Prefabs.BuildingFlags) 0)
          int3.x = 4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaterConsumerData.HasComponent(building))
        {
          if ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.HasSewageNode) == (Game.Prefabs.BuildingFlags) 0)
            int3.y = 4;
          if ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.HasWaterNode) == (Game.Prefabs.BuildingFlags) 0)
            int3.z = 4;
        }
        float3 float3_1 = math.rotate(transform.m_Rotation, new float3(0.15f, 0.0f, 0.0f));
        float3 float3_2 = connectionPosition - float3_1 * (float) math.csum(int3.xz);
        float3 float3_3 = transform.m_Position - float3_1 * (float) math.csum(int3.xz);
        float3 float3_4 = float3_2;
        float3 startPos1 = float3_3;
        float3 float3_5 = float3_2 + float3_1 * (float) math.csum(int3.xy);
        float3 float3_6 = float3_3 + float3_1 * (float) math.csum(int3.xy);
        float3 float3_7 = float3_5;
        float3 startPos2 = float3_6;
        float3 float3_8 = float3_5 + float3_1 * (float) math.csum(int3.yz);
        float3 float3_9 = float3_6 + float3_1 * (float) math.csum(int3.yz);
        float3 float3_10 = float3_8;
        float3 startPos3 = float3_9;
        Entity entity1 = Entity.Null;
        Entity entity2 = Entity.Null;
        Entity entity3 = Entity.Null;
        float delta1 = 0.0f;
        float delta2 = 0.0f;
        float delta3 = 0.0f;
        float num1 = float.MaxValue;
        float num2 = float.MaxValue;
        float num3 = float.MaxValue;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subLane = bufferData[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_UtilityLaneData.HasComponent(subLane) && this.m_EdgeLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[subLane];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane].m_Prefab];
            if ((utilityLaneData.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None && int3.x != 0)
            {
              float t;
              float num4 = MathUtils.Distance(curve.m_Bezier, float3_7, out t);
              if ((double) num4 < (double) num1)
              {
                entity1 = subLane;
                delta1 = t;
                num1 = num4;
              }
            }
            if ((utilityLaneData.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None && int3.y != 0)
            {
              float t;
              float num5 = MathUtils.Distance(curve.m_Bezier, float3_4, out t);
              if ((double) num5 < (double) num2)
              {
                entity2 = subLane;
                delta2 = t;
                num2 = num5;
              }
            }
            if ((utilityLaneData.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None && int3.z != 0)
            {
              float t;
              float num6 = MathUtils.Distance(curve.m_Bezier, float3_10, out t);
              if ((double) num6 < (double) num3)
              {
                entity3 = subLane;
                delta3 = t;
                num3 = num6;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated method
        float yoffset1 = this.CalculateYOffset(edgeGeometry, entity1);
        // ISSUE: reference to a compiler-generated method
        float yoffset2 = this.CalculateYOffset(edgeGeometry, entity2);
        // ISSUE: reference to a compiler-generated method
        float yoffset3 = this.CalculateYOffset(edgeGeometry, entity3);
        // ISSUE: reference to a compiler-generated method
        bool2 bool2_1 = this.ShouldCheckNodeLanes(entity1, delta1);
        // ISSUE: reference to a compiler-generated method
        bool2 bool2_2 = this.ShouldCheckNodeLanes(entity2, delta2);
        // ISSUE: reference to a compiler-generated method
        bool2 bool2_3 = this.ShouldCheckNodeLanes(entity3, delta3);
        bool2 x = bool2_1 | bool2_2 | bool2_3;
        if (math.any(x))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge = this.m_EdgeData[road];
          // ISSUE: reference to a compiler-generated field
          if (x.x && this.m_SubLanes.TryGetBuffer(edge.m_Start, out bufferData))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              Entity subLane = bufferData[index].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              if (this.m_UtilityLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane].m_Prefab];
                if ((utilityLaneData.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None && bool2_1.x)
                {
                  float t;
                  float num7 = MathUtils.Distance(curve.m_Bezier, float3_7, out t);
                  if ((double) num7 < (double) num1)
                  {
                    entity1 = subLane;
                    delta1 = t;
                    num1 = num7;
                  }
                }
                if ((utilityLaneData.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None && bool2_2.x)
                {
                  float t;
                  float num8 = MathUtils.Distance(curve.m_Bezier, float3_4, out t);
                  if ((double) num8 < (double) num2)
                  {
                    entity2 = subLane;
                    delta2 = t;
                    num2 = num8;
                  }
                }
                if ((utilityLaneData.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None && bool2_3.x)
                {
                  float t;
                  float num9 = MathUtils.Distance(curve.m_Bezier, float3_10, out t);
                  if ((double) num9 < (double) num3)
                  {
                    entity3 = subLane;
                    delta3 = t;
                    num3 = num9;
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (x.y && this.m_SubLanes.TryGetBuffer(edge.m_End, out bufferData))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              Entity subLane = bufferData[index].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              if (this.m_UtilityLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane].m_Prefab];
                if ((utilityLaneData.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None && bool2_1.y)
                {
                  float t;
                  float num10 = MathUtils.Distance(curve.m_Bezier, float3_7, out t);
                  if ((double) num10 < (double) num1)
                  {
                    entity1 = subLane;
                    delta1 = t;
                    num1 = num10;
                  }
                }
                if ((utilityLaneData.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None && bool2_2.y)
                {
                  float t;
                  float num11 = MathUtils.Distance(curve.m_Bezier, float3_4, out t);
                  if ((double) num11 < (double) num2)
                  {
                    entity2 = subLane;
                    delta2 = t;
                    num2 = num11;
                  }
                }
                if ((utilityLaneData.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None && bool2_3.y)
                {
                  float t;
                  float num12 = MathUtils.Distance(curve.m_Bezier, float3_10, out t);
                  if ((double) num12 < (double) num3)
                  {
                    entity3 = subLane;
                    delta3 = t;
                    num3 = num12;
                  }
                }
              }
            }
          }
        }
        if (entity1 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          electricityCurve = this.CalculateConnectCurve(startPos2, float3_7, entity1, yoffset1, delta1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          electricityLanePrefab = this.GetLanePrefab(entity1, this.m_BuildingConfigurationData.m_ElectricityConnectionLane, int3.x);
          // ISSUE: reference to a compiler-generated method
          electricityObjectPrefab = this.GetNodeObjectPrefab(electricityLanePrefab);
          // ISSUE: reference to a compiler-generated method
          electricityNode = this.GetPathNode(entity1, delta1);
        }
        if (entity2 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          sewageCurve = this.CalculateConnectCurve(startPos1, float3_4, entity2, yoffset2, delta2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          sewageLanePrefab = this.GetLanePrefab(entity2, this.m_BuildingConfigurationData.m_SewageConnectionLane, int3.y);
          // ISSUE: reference to a compiler-generated method
          sewageObjectPrefab = this.GetNodeObjectPrefab(sewageLanePrefab);
          // ISSUE: reference to a compiler-generated method
          sewageNode = this.GetPathNode(entity2, delta2);
        }
        if (!(entity3 != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated method
        waterCurve = this.CalculateConnectCurve(startPos3, float3_10, entity3, yoffset3, delta3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        waterLanePrefab = this.GetLanePrefab(entity3, this.m_BuildingConfigurationData.m_WaterConnectionLane, int3.z);
        // ISSUE: reference to a compiler-generated method
        waterObjectPrefab = this.GetNodeObjectPrefab(waterLanePrefab);
        // ISSUE: reference to a compiler-generated method
        waterNode = this.GetPathNode(entity3, delta3);
      }

      private bool2 ShouldCheckNodeLanes(Entity bestLane, float delta)
      {
        if (bestLane != Entity.Null)
        {
          bool2 x = delta == new float2(0.0f, 1f);
          if (math.any(x))
          {
            // ISSUE: reference to a compiler-generated field
            EdgeLane edgeLane = this.m_EdgeLaneData[bestLane];
            return math.select(edgeLane.m_EdgeDelta.x, edgeLane.m_EdgeDelta.y, x.y) == new float2(0.0f, 1f);
          }
        }
        return (bool2) false;
      }

      private float CalculateYOffset(EdgeGeometry edgeGeometry, Entity roadLane)
      {
        if (!(roadLane != Entity.Null))
          return 0.0f;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[roadLane];
        // ISSUE: reference to a compiler-generated field
        EdgeLane edgeLane = this.m_EdgeLaneData[roadLane];
        if ((double) edgeLane.m_EdgeDelta.x > 0.5)
        {
          float t = math.saturate((float) ((double) edgeLane.m_EdgeDelta.x * 2.0 - 1.0));
          return curve.m_Bezier.a.y - math.lerp(MathUtils.Position(edgeGeometry.m_End.m_Left, t).y, MathUtils.Position(edgeGeometry.m_End.m_Right, t).y, 0.5f);
        }
        float t1 = math.saturate(edgeLane.m_EdgeDelta.x * 2f);
        return curve.m_Bezier.a.y - math.lerp(MathUtils.Position(edgeGeometry.m_Start.m_Left, t1).y, MathUtils.Position(edgeGeometry.m_Start.m_Right, t1).y, 0.5f);
      }

      private Bezier4x3 CalculateConnectCurve(
        float3 startPos,
        float3 connectPos,
        Entity roadLane,
        float yOffset,
        float delta)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[roadLane];
        startPos.y += yOffset;
        connectPos.y += yOffset;
        float3 _a = MathUtils.Position(curve.m_Bezier, delta);
        return NetUtils.FitCurve(new Line3.Segment(startPos, connectPos), new Line3.Segment(_a, connectPos));
      }

      private Entity GetNodeObjectPrefab(Entity lanePrefab)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabUtilityLaneData[lanePrefab].m_NodeObjectPrefab;
      }

      private Entity GetLanePrefab(
        Entity roadLane,
        Entity consumerConnectionPrefab,
        int needConnection)
      {
        if (needConnection < 6)
          return consumerConnectionPrefab;
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_PrefabRefData[roadLane].m_Prefab;
        if (needConnection <= 6)
          return prefab;
        // ISSUE: reference to a compiler-generated field
        UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[prefab];
        return !(utilityLaneData.m_LocalConnectionPrefab != Entity.Null) ? prefab : utilityLaneData.m_LocalConnectionPrefab;
      }

      private PathNode GetPathNode(Entity roadLane, float delta)
      {
        // ISSUE: reference to a compiler-generated field
        return new PathNode(this.m_LaneData[roadLane].m_MiddleNode, delta);
      }

      private void DeleteLane(int jobIndex, Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, lane, in this.m_AppliedTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, lane);
      }

      private void UpdateLane(
        int jobIndex,
        Entity lane,
        Bezier4x3 curve,
        PathNode endNode,
        bool isTemp,
        Temp temp,
        Entity original)
      {
        Curve component1;
        component1.m_Bezier = curve;
        component1.m_Length = MathUtils.Length(curve);
        // ISSUE: reference to a compiler-generated field
        Lane component2 = this.m_LaneData[lane] with
        {
          m_EndNode = endNode
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Curve>(jobIndex, lane, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Lane>(jobIndex, lane, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, lane, new Updated());
        if (!isTemp)
          return;
        temp.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Temp>(jobIndex, lane, temp);
      }

      private void CreateLane(
        int jobIndex,
        Entity owner,
        int laneIndex,
        Entity prefab,
        Bezier4x3 curve,
        PathNode endNode,
        float3 connectPos,
        bool isTemp,
        Temp temp,
        Entity original)
      {
        // ISSUE: reference to a compiler-generated field
        NetLaneArchetypeData laneArchetypeData = this.m_PrefabNetLaneArchetypeData[prefab];
        Owner component1 = new Owner(owner);
        PrefabRef component2 = new PrefabRef(prefab);
        Game.Net.SecondaryLane component3 = new Game.Net.SecondaryLane();
        Lane component4;
        component4.m_StartNode = new PathNode(owner, (ushort) laneIndex);
        component4.m_MiddleNode = new PathNode(owner, (ushort) (laneIndex + 1));
        component4.m_EndNode = endNode;
        Curve component5;
        component5.m_Bezier = curve;
        component5.m_Length = MathUtils.Length(curve);
        Game.Net.Elevation component6;
        component6.m_Elevation.x = curve.a.y - connectPos.y;
        component6.m_Elevation.y = curve.d.y - connectPos.y;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, laneArchetypeData.m_LaneArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity, component4);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity, component5);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Game.Net.SecondaryLane>(jobIndex, entity, component3);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Game.Net.Elevation>(jobIndex, entity, component6);
        if (!isTemp)
          return;
        temp.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity, temp);
      }

      private void DeleteObject(int jobIndex, Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, lane, in this.m_AppliedTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, lane);
      }

      private void UpdateObject(
        int jobIndex,
        Entity obj,
        float3 position,
        bool isTemp,
        Temp temp,
        Entity original)
      {
        Transform component;
        component.m_Position = position;
        component.m_Rotation = quaternion.identity;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Transform>(jobIndex, obj, component);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, obj, new Updated());
        if (!isTemp)
          return;
        temp.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Temp>(jobIndex, obj, temp);
      }

      private void CreateObject(
        int jobIndex,
        Entity owner,
        Entity prefab,
        float3 position,
        float3 connectPos,
        bool isTemp,
        Temp temp,
        Entity original)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectData objectData = this.m_PrefabObjectData[prefab];
        Owner component1 = new Owner(owner);
        PrefabRef component2 = new PrefabRef(prefab);
        Secondary component3 = new Secondary();
        Transform component4;
        component4.m_Position = position;
        component4.m_Rotation = quaternion.identity;
        Game.Objects.Elevation component5;
        component5.m_Elevation = position.y - connectPos.y;
        component5.m_Flags = (ElevationFlags) 0;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, objectData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Transform>(jobIndex, entity, component4);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Secondary>(jobIndex, entity, component3);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Game.Objects.Elevation>(jobIndex, entity, component5);
        if (!isTemp)
          return;
        temp.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity, temp);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      public ComponentLookup<BackSide> __Game_Buildings_BackSide_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Created> __Game_Common_Created_RO_ComponentLookup;
      public ComponentLookup<Building> __Game_Buildings_Building_RW_ComponentLookup;
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.UtilityObject> __Game_Objects_UtilityObject_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Secondary> __Game_Objects_Secondary_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.UtilityObjectData> __Game_Prefabs_UtilityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> __Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BackSide_RW_ComponentLookup = state.GetComponentLookup<BackSide>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentLookup = state.GetComponentLookup<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RW_ComponentLookup = state.GetComponentLookup<Building>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RW_BufferLookup = state.GetBufferLookup<ConnectedBuilding>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UtilityObject_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.UtilityObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Secondary_RO_ComponentLookup = state.GetComponentLookup<Secondary>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.UtilityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup = state.GetComponentLookup<NetLaneArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RO_BufferLookup = state.GetBufferLookup<SpawnLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
      }
    }
  }
}
