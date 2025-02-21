// Decompiled with JetBrains decompiler
// Type: Game.Rendering.NetColorSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class NetColorSystem : GameSystemBase
  {
    private EntityQuery m_ZonePreferenceParameterGroup;
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_NodeQuery;
    private EntityQuery m_LaneQuery;
    private EntityQuery m_InfomodeQuery;
    private EntityQuery m_ProcessQuery;
    private ToolSystem m_ToolSystem;
    private ZoneToolSystem m_ZoneToolSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private PrefabSystem m_PrefabSystem;
    private ResourceSystem m_ResourceSystem;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private NetColorSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1733354667_0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneToolSystem = this.World.GetOrCreateSystemManaged<ZoneToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZonePreferenceParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<ZonePreferenceData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadWrite<EdgeColor>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_NodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Node>(), ComponentType.ReadWrite<NodeColor>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Lane>(), ComponentType.ReadWrite<LaneColor>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<InfomodeActive>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<InfoviewCoverageData>(),
          ComponentType.ReadOnly<InfoviewAvailabilityData>(),
          ComponentType.ReadOnly<InfoviewNetGeometryData>(),
          ComponentType.ReadOnly<InfoviewNetStatusData>()
        },
        None = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_ToolSystem.activeInfoview == (UnityEngine.Object) null || this.m_EdgeQuery.IsEmptyIgnoreFilter && this.m_NodeQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ZonePreferenceData zonePreferenceData = this.m_ZonePreferenceParameterGroup.CalculateEntityCount() > 0 ? this.m_ZonePreferenceParameterGroup.GetSingleton<ZonePreferenceData>() : new ZonePreferenceData();
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_InfomodeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1);
      Entity entity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_ZoneToolSystem && (UnityEngine.Object) this.m_ZoneToolSystem.prefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        entity = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_ZoneToolSystem.prefab);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem && (UnityEngine.Object) this.m_ObjectToolSystem.prefab != (UnityEngine.Object) null)
        {
          SignatureBuilding component1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectToolSystem.prefab.TryGet<SignatureBuilding>(out component1) && (UnityEngine.Object) component1.m_ZoneType != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            entity = this.m_PrefabSystem.GetEntity((PrefabBase) component1.m_ZoneType);
          }
          else
          {
            PlaceholderBuilding component2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectToolSystem.prefab.TryGet<PlaceholderBuilding>(out component2) && (UnityEngine.Object) component2.m_ZoneType != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              entity = this.m_PrefabSystem.GetEntity((PrefabBase) component2.m_ZoneType);
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeColor_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ProcessEstimate_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Pollution_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubwayTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Waterway_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      JobHandle deps1;
      JobHandle deps2;
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetColorSystem.UpdateEdgeColorsJob jobData1 = new NetColorSystem.UpdateEdgeColorsJob()
      {
        m_InfomodeChunks = archetypeChunkListAsync,
        m_InfomodeActiveType = this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle,
        m_InfoviewCoverageType = this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle,
        m_InfoviewAvailabilityType = this.__TypeHandle.__Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle,
        m_InfoviewNetGeometryType = this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle,
        m_InfoviewNetStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle,
        m_TrainTrackType = this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentTypeHandle,
        m_TramTrackType = this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentTypeHandle,
        m_WaterwayType = this.__TypeHandle.__Game_Net_Waterway_RO_ComponentTypeHandle,
        m_SubwayTrackType = this.__TypeHandle.__Game_Net_SubwayTrack_RO_ComponentTypeHandle,
        m_NetConditionType = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentTypeHandle,
        m_RoadType = this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle,
        m_PollutionType = this.__TypeHandle.__Game_Net_Pollution_RO_ComponentTypeHandle,
        m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
        m_ServiceCoverageType = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferTypeHandle,
        m_ResourceAvailabilityType = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferTypeHandle,
        m_LandValues = this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_Temps = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ZonePropertiesDatas = this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup,
        m_ProcessEstimates = this.__TypeHandle.__Game_Zones_ProcessEstimate_RO_BufferLookup,
        m_ServiceCoverageData = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup,
        m_ResourceAvailabilityData = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ColorType = this.__TypeHandle.__Game_Net_EdgeColor_RW_ComponentTypeHandle,
        m_ZonePrefab = entity,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_PollutionMap = this.m_GroundPollutionSystem.GetMap(true, out dependencies),
        m_IndustrialDemands = this.m_IndustrialDemandSystem.GetBuildingDemands(out deps1),
        m_StorageDemands = this.m_IndustrialDemandSystem.GetStorageBuildingDemands(out deps2),
        m_Processes = this.m_ProcessQuery.ToComponentDataListAsync<IndustrialProcessData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_ZonePreferences = zonePreferenceData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeColor_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Pollution_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubwayTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Waterway_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      NetColorSystem.UpdateNodeColorsJob jobData2 = new NetColorSystem.UpdateNodeColorsJob()
      {
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_ColorData = this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_InfomodeChunks = archetypeChunkListAsync,
        m_InfomodeActiveType = this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle,
        m_InfoviewNetGeometryType = this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle,
        m_InfoviewNetStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle,
        m_TrainTrackType = this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentTypeHandle,
        m_TramTrackType = this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentTypeHandle,
        m_WaterwayType = this.__TypeHandle.__Game_Net_Waterway_RO_ComponentTypeHandle,
        m_SubwayTrackType = this.__TypeHandle.__Game_Net_SubwayTrack_RO_ComponentTypeHandle,
        m_NetConditionType = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentTypeHandle,
        m_RoadType = this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle,
        m_PollutionType = this.__TypeHandle.__Game_Net_Pollution_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ConnectedEdgeType = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle,
        m_ColorType = this.__TypeHandle.__Game_Net_NodeColor_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeColor_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
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
      NetColorSystem.UpdateEdgeColors2Job jobData3 = new NetColorSystem.UpdateEdgeColors2Job()
      {
        m_ColorData = this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_StartNodeGeometryType = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle,
        m_EndNodeGeometryType = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle,
        m_ColorType = this.__TypeHandle.__Game_Net_EdgeColor_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubFlow_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneColor_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeMapping_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      NetColorSystem.LaneColorJob jobData4 = new NetColorSystem.LaneColorJob()
      {
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle,
        m_NodeLaneType = this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentTypeHandle,
        m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle,
        m_UtilityLaneType = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle,
        m_SecondaryLaneType = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentTypeHandle,
        m_EdgeMappingType = this.__TypeHandle.__Game_Net_EdgeMapping_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ColorType = this.__TypeHandle.__Game_Net_LaneColor_RW_ComponentTypeHandle,
        m_SubFlowType = this.__TypeHandle.__Game_Net_SubFlow_RW_BufferTypeHandle,
        m_InfomodeChunks = archetypeChunkListAsync,
        m_InfomodeActiveType = this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle,
        m_InfoviewNetGeometryType = this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle,
        m_InfoviewNetStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_NodeColorData = this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup,
        m_EdgeColorData = this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup,
        m_ObjectColorData = this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup,
        m_ElectricityNodeConnectionData = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_ElectricityFlowEdgeData = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_ElectricityBuildingConnectionData = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup,
        m_WaterPipeNodeConnectionData = this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup,
        m_WaterPipeEdgeData = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup,
        m_WaterPipeBuildingConnectionData = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ElectricityConsumerData = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_WaterConsumerData = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_ConnectedFlowEdges = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_ElectricitySinkNode = this.m_ElectricityFlowSystem.sinkNode,
        m_WaterSinkNode = this.m_WaterPipeFlowSystem.sinkNode,
        m_WaterPipeParameters = this.__query_1733354667_0.GetSingleton<WaterPipeParameterData>()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<NetColorSystem.UpdateEdgeColorsJob>(this.m_EdgeQuery, JobUtils.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2, dependencies, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn1 = jobData2.ScheduleParallel<NetColorSystem.UpdateNodeColorsJob>(this.m_NodeQuery, jobHandle1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData3.ScheduleParallel<NetColorSystem.UpdateEdgeColors2Job>(this.m_EdgeQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      EntityQuery laneQuery = this.m_LaneQuery;
      JobHandle dependsOn2 = jobHandle2;
      JobHandle inputDeps = jobData4.ScheduleParallel<NetColorSystem.LaneColorJob>(laneQuery, dependsOn2);
      archetypeChunkListAsync.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem.AddReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IndustrialDemandSystem.AddReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(jobHandle1);
      this.Dependency = inputDeps;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1733354667_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<WaterPipeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public NetColorSystem()
    {
    }

    [BurstCompile]
    private struct UpdateEdgeColorsJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfomodeChunks;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> m_InfomodeActiveType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewCoverageData> m_InfoviewCoverageType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewAvailabilityData> m_InfoviewAvailabilityType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetGeometryData> m_InfoviewNetGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> m_InfoviewNetStatusType;
      [ReadOnly]
      public ComponentTypeHandle<TrainTrack> m_TrainTrackType;
      [ReadOnly]
      public ComponentTypeHandle<TramTrack> m_TramTrackType;
      [ReadOnly]
      public ComponentTypeHandle<Waterway> m_WaterwayType;
      [ReadOnly]
      public ComponentTypeHandle<SubwayTrack> m_SubwayTrackType;
      [ReadOnly]
      public ComponentTypeHandle<NetCondition> m_NetConditionType;
      [ReadOnly]
      public ComponentTypeHandle<Road> m_RoadType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Pollution> m_PollutionType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.ServiceCoverage> m_ServiceCoverageType;
      [ReadOnly]
      public BufferTypeHandle<ResourceAvailability> m_ResourceAvailabilityType;
      [ReadOnly]
      public ComponentLookup<LandValue> m_LandValues;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_Edges;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_Nodes;
      [ReadOnly]
      public ComponentLookup<Temp> m_Temps;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> m_ZonePropertiesDatas;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverageData;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_ResourceAvailabilityData;
      [ReadOnly]
      public BufferLookup<ProcessEstimate> m_ProcessEstimates;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      public ComponentTypeHandle<EdgeColor> m_ColorType;
      [ReadOnly]
      public Entity m_ZonePrefab;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public NativeArray<GroundPollution> m_PollutionMap;
      [ReadOnly]
      public NativeArray<int> m_IndustrialDemands;
      [ReadOnly]
      public NativeArray<int> m_StorageDemands;
      [ReadOnly]
      public NativeList<IndustrialProcessData> m_Processes;
      [ReadOnly]
      public ZonePreferenceData m_ZonePreferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeColor> nativeArray1 = chunk.GetNativeArray<EdgeColor>(ref this.m_ColorType);
        InfoviewCoverageData coverageData;
        InfomodeActive activeData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (chunk.Has<Game.Net.ServiceCoverage>(ref this.m_ServiceCoverageType) && this.GetServiceCoverageData(chunk, out coverageData, out activeData1))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Net.ServiceCoverage> bufferAccessor = chunk.GetBufferAccessor<Game.Net.ServiceCoverage>(ref this.m_ServiceCoverageType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            DynamicBuffer<Game.Net.ServiceCoverage> dynamicBuffer = bufferAccessor[index];
            Temp temp;
            DynamicBuffer<Game.Net.ServiceCoverage> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (CollectionUtils.TryGet<Temp>(nativeArray2, index, out temp) && this.m_ServiceCoverageData.TryGetBuffer(temp.m_Original, out bufferData))
              dynamicBuffer = bufferData;
            if (dynamicBuffer.Length == 0)
            {
              nativeArray1[index] = new EdgeColor();
            }
            else
            {
              Game.Net.ServiceCoverage serviceCoverage = dynamicBuffer[(int) coverageData.m_Service];
              EdgeColor edgeColor;
              edgeColor.m_Index = (byte) activeData1.m_Index;
              edgeColor.m_Value0 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(coverageData, serviceCoverage.m_Coverage.x) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              edgeColor.m_Value1 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(coverageData, serviceCoverage.m_Coverage.y) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              nativeArray1[index] = edgeColor;
            }
          }
        }
        else
        {
          InfoviewAvailabilityData availabilityData;
          InfomodeActive activeData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (chunk.Has<ResourceAvailability>(ref this.m_ResourceAvailabilityType) && this.GetResourceAvailabilityData(chunk, out availabilityData, out activeData2))
          {
            // ISSUE: reference to a compiler-generated field
            ZonePreferenceData zonePreferences = this.m_ZonePreferences;
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.Edge> nativeArray3 = chunk.GetNativeArray<Game.Net.Edge>(ref this.m_EdgeType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<ResourceAvailability> bufferAccessor = chunk.GetBufferAccessor<ResourceAvailability>(ref this.m_ResourceAvailabilityType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Game.Net.Edge edge = nativeArray3[index];
              DynamicBuffer<ResourceAvailability> availabilityBuffer = bufferAccessor[index];
              Temp temp;
              float landValue1;
              float landValue2;
              if (CollectionUtils.TryGet<Temp>(nativeArray4, index, out temp))
              {
                Game.Net.Edge componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Edges.TryGetComponent(temp.m_Original, out componentData1))
                {
                  edge = componentData1;
                  // ISSUE: reference to a compiler-generated field
                  landValue1 = this.m_LandValues[componentData1.m_Start].m_LandValue;
                  // ISSUE: reference to a compiler-generated field
                  landValue2 = this.m_LandValues[componentData1.m_End].m_LandValue;
                  DynamicBuffer<ResourceAvailability> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ResourceAvailabilityData.TryGetBuffer(temp.m_Original, out bufferData))
                    availabilityBuffer = bufferData;
                }
                else
                {
                  Temp componentData2;
                  LandValue componentData3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  landValue1 = !this.m_Temps.TryGetComponent(edge.m_Start, out componentData2) || !this.m_LandValues.TryGetComponent(componentData2.m_Original, out componentData3) ? this.m_LandValues[edge.m_Start].m_LandValue : componentData3.m_LandValue;
                  Temp componentData4;
                  LandValue componentData5;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  landValue2 = !this.m_Temps.TryGetComponent(edge.m_End, out componentData4) || !this.m_LandValues.TryGetComponent(componentData4.m_Original, out componentData5) ? this.m_LandValues[edge.m_End].m_LandValue : componentData5.m_LandValue;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                landValue1 = this.m_LandValues[edge.m_Start].m_LandValue;
                // ISSUE: reference to a compiler-generated field
                landValue2 = this.m_LandValues[edge.m_End].m_LandValue;
              }
              if (availabilityBuffer.Length == 0)
              {
                nativeArray1[index] = new EdgeColor();
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                float3 position1 = this.m_Nodes[edge.m_Start].m_Position;
                // ISSUE: reference to a compiler-generated field
                float3 position2 = this.m_Nodes[edge.m_End].m_Position;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                GroundPollution pollution1 = GroundPollutionSystem.GetPollution(position1, this.m_PollutionMap);
                // ISSUE: reference to a compiler-generated field
                NativeArray<GroundPollution> pollutionMap = this.m_PollutionMap;
                // ISSUE: reference to a compiler-generated method
                GroundPollution pollution2 = GroundPollutionSystem.GetPollution(position2, pollutionMap);
                float pollution3 = (float) pollution1.m_Pollution;
                float pollution4 = (float) pollution2.m_Pollution;
                DynamicBuffer<ProcessEstimate> bufferData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_ProcessEstimates.TryGetBuffer(this.m_ZonePrefab, out bufferData);
                ZonePropertiesData componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ZonePropertiesDatas.TryGetComponent(this.m_ZonePrefab, out componentData))
                {
                  float num = availabilityData.m_AreaType != AreaType.Residential ? componentData.m_SpaceMultiplier : (componentData.m_ScaleResidentials ? componentData.m_ResidentialProperties : componentData.m_ResidentialProperties / 8f);
                  landValue1 /= num;
                  landValue2 /= num;
                }
                EdgeColor edgeColor;
                edgeColor.m_Index = (byte) activeData2.m_Index;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                edgeColor.m_Value0 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(availabilityData, availabilityBuffer, 0.0f, ref zonePreferences, this.m_IndustrialDemands, this.m_StorageDemands, pollution3, landValue1, bufferData, this.m_Processes, this.m_ResourcePrefabs, this.m_ResourceDatas) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                edgeColor.m_Value1 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(availabilityData, availabilityBuffer, 1f, ref zonePreferences, this.m_IndustrialDemands, this.m_StorageDemands, pollution4, landValue2, bufferData, this.m_Processes, this.m_ResourcePrefabs, this.m_ResourceDatas) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
                nativeArray1[index] = edgeColor;
              }
            }
          }
          else
          {
            InfoviewNetStatusData statusData;
            InfomodeActive activeData3;
            // ISSUE: reference to a compiler-generated method
            if (this.GetNetStatusType(chunk, out statusData, out activeData3))
            {
              // ISSUE: reference to a compiler-generated method
              this.GetNetStatusColors(nativeArray1, chunk, statusData, activeData3);
            }
            else
            {
              int index1;
              // ISSUE: reference to a compiler-generated method
              if (this.GetNetGeometryColor(chunk, out index1))
              {
                for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
                  nativeArray1[index2] = new EdgeColor((byte) index1, (byte) 0, (byte) 0);
              }
              else
              {
                for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
                  nativeArray1[index3] = new EdgeColor((byte) 0, byte.MaxValue, byte.MaxValue);
              }
            }
          }
        }
      }

      private bool GetServiceCoverageData(
        ArchetypeChunk chunk,
        out InfoviewCoverageData coverageData,
        out InfomodeActive activeData)
      {
        coverageData = new InfoviewCoverageData();
        activeData = new InfomodeActive();
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewCoverageData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewCoverageData>(ref this.m_InfoviewCoverageType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              if (priority < num)
              {
                coverageData = nativeArray1[index2];
                coverageData.m_Service = CoverageService.Count;
                activeData = infomodeActive;
                num = priority;
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool GetResourceAvailabilityData(
        ArchetypeChunk chunk,
        out InfoviewAvailabilityData availabilityData,
        out InfomodeActive activeData)
      {
        availabilityData = new InfoviewAvailabilityData();
        activeData = new InfomodeActive();
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewAvailabilityData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewAvailabilityData>(ref this.m_InfoviewAvailabilityType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              if (priority < num)
              {
                availabilityData = nativeArray1[index2];
                activeData = infomodeActive;
                num = priority;
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool GetNetStatusType(
        ArchetypeChunk chunk,
        out InfoviewNetStatusData statusData,
        out InfomodeActive activeData)
      {
        statusData = new InfoviewNetStatusData();
        activeData = new InfomodeActive();
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewNetStatusData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewNetStatusData>(ref this.m_InfoviewNetStatusType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              if (priority < num)
              {
                InfoviewNetStatusData infoviewNetStatusData = nativeArray1[index2];
                // ISSUE: reference to a compiler-generated method
                if (this.HasNetStatus(nativeArray1[index2], chunk))
                {
                  statusData = infoviewNetStatusData;
                  activeData = infomodeActive;
                  num = priority;
                }
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasNetStatus(InfoviewNetStatusData infoviewNetStatusData, ArchetypeChunk chunk)
      {
        switch (infoviewNetStatusData.m_Type)
        {
          case NetStatusType.Wear:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<NetCondition>(ref this.m_NetConditionType);
          case NetStatusType.TrafficFlow:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Road>(ref this.m_RoadType);
          case NetStatusType.NoisePollutionSource:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Net.Pollution>(ref this.m_PollutionType);
          case NetStatusType.AirPollutionSource:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Net.Pollution>(ref this.m_PollutionType);
          case NetStatusType.TrafficVolume:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Road>(ref this.m_RoadType);
          default:
            return false;
        }
      }

      private void GetNetStatusColors(
        NativeArray<EdgeColor> results,
        ArchetypeChunk chunk,
        InfoviewNetStatusData statusData,
        InfomodeActive activeData)
      {
        switch (statusData.m_Type)
        {
          case NetStatusType.Wear:
            // ISSUE: reference to a compiler-generated field
            NativeArray<NetCondition> nativeArray1 = chunk.GetNativeArray<NetCondition>(ref this.m_NetConditionType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              NetCondition netCondition = nativeArray1[index];
              EdgeColor edgeColor;
              edgeColor.m_Index = (byte) activeData.m_Index;
              edgeColor.m_Value0 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, netCondition.m_Wear.x / 10f) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              edgeColor.m_Value1 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, netCondition.m_Wear.y / 10f) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = edgeColor;
            }
            break;
          case NetStatusType.TrafficFlow:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Road> nativeArray2 = chunk.GetNativeArray<Road>(ref this.m_RoadType);
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Road road = nativeArray2[index];
              float4 trafficFlowSpeed1 = NetUtils.GetTrafficFlowSpeed(road.m_TrafficFlowDuration0, road.m_TrafficFlowDistance0);
              float4 trafficFlowSpeed2 = NetUtils.GetTrafficFlowSpeed(road.m_TrafficFlowDuration1, road.m_TrafficFlowDistance1);
              EdgeColor edgeColor;
              edgeColor.m_Index = (byte) activeData.m_Index;
              edgeColor.m_Value0 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, (float) ((double) math.csum(trafficFlowSpeed1) * 0.125 + (double) math.cmin(trafficFlowSpeed1) * 0.5)) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              edgeColor.m_Value1 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, (float) ((double) math.csum(trafficFlowSpeed2) * 0.125 + (double) math.cmin(trafficFlowSpeed2) * 0.5)) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = edgeColor;
            }
            break;
          case NetStatusType.NoisePollutionSource:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.Pollution> nativeArray3 = chunk.GetNativeArray<Game.Net.Pollution>(ref this.m_PollutionType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<EdgeGeometry> nativeArray4 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
            for (int index = 0; index < nativeArray3.Length; ++index)
            {
              double x = (double) nativeArray3[index].m_Accumulation.x;
              EdgeGeometry edgeGeometry = nativeArray4[index];
              double middleLength1 = (double) edgeGeometry.m_Start.middleLength;
              edgeGeometry = nativeArray4[index];
              double middleLength2 = (double) edgeGeometry.m_End.middleLength;
              double num = (double) math.max(0.1f, (float) (middleLength1 + middleLength2));
              float status = (float) (x / num);
              EdgeColor edgeColor;
              edgeColor.m_Index = (byte) activeData.m_Index;
              edgeColor.m_Value0 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              edgeColor.m_Value1 = edgeColor.m_Value0;
              results[index] = edgeColor;
            }
            break;
          case NetStatusType.AirPollutionSource:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.Pollution> nativeArray5 = chunk.GetNativeArray<Game.Net.Pollution>(ref this.m_PollutionType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<EdgeGeometry> nativeArray6 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
            for (int index = 0; index < nativeArray5.Length; ++index)
            {
              double y = (double) nativeArray5[index].m_Accumulation.y;
              EdgeGeometry edgeGeometry = nativeArray6[index];
              double middleLength3 = (double) edgeGeometry.m_Start.middleLength;
              edgeGeometry = nativeArray6[index];
              double middleLength4 = (double) edgeGeometry.m_End.middleLength;
              double num = (double) math.max(0.1f, (float) (middleLength3 + middleLength4));
              float status = (float) (y / num);
              EdgeColor edgeColor;
              edgeColor.m_Index = (byte) activeData.m_Index;
              edgeColor.m_Value0 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              edgeColor.m_Value1 = edgeColor.m_Value0;
              results[index] = edgeColor;
            }
            break;
          case NetStatusType.TrafficVolume:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Road> nativeArray7 = chunk.GetNativeArray<Road>(ref this.m_RoadType);
            for (int index = 0; index < nativeArray7.Length; ++index)
            {
              Road road = nativeArray7[index];
              float4 x1 = math.sqrt(road.m_TrafficFlowDistance0 * 5.33333349f);
              float4 x2 = math.sqrt(road.m_TrafficFlowDistance1 * 5.33333349f);
              EdgeColor edgeColor;
              edgeColor.m_Index = (byte) activeData.m_Index;
              edgeColor.m_Value0 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, math.csum(x1) * 0.25f) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              edgeColor.m_Value1 = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, math.csum(x2) * 0.25f) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = edgeColor;
            }
            break;
        }
      }

      private bool GetNetGeometryColor(ArchetypeChunk chunk, out int index)
      {
        index = 0;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewNetGeometryData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewNetGeometryData>(ref this.m_InfoviewNetGeometryType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              // ISSUE: reference to a compiler-generated method
              if (priority < num && this.HasNetGeometryColor(nativeArray1[index2], chunk))
              {
                index = infomodeActive.m_Index;
                num = priority;
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasNetGeometryColor(
        InfoviewNetGeometryData infoviewNetGeometryData,
        ArchetypeChunk chunk)
      {
        switch (infoviewNetGeometryData.m_Type)
        {
          case NetType.Road:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Road>(ref this.m_RoadType);
          case NetType.TrainTrack:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TrainTrack>(ref this.m_TrainTrackType);
          case NetType.TramTrack:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TramTrack>(ref this.m_TramTrackType);
          case NetType.Waterway:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Waterway>(ref this.m_WaterwayType);
          case NetType.SubwayTrack:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<SubwayTrack>(ref this.m_SubwayTrackType);
          default:
            return false;
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
    private struct UpdateNodeColorsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<EdgeColor> m_ColorData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> m_ConnectedEdgeType;
      public ComponentTypeHandle<NodeColor> m_ColorType;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfomodeChunks;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> m_InfomodeActiveType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetGeometryData> m_InfoviewNetGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> m_InfoviewNetStatusType;
      [ReadOnly]
      public ComponentTypeHandle<TrainTrack> m_TrainTrackType;
      [ReadOnly]
      public ComponentTypeHandle<TramTrack> m_TramTrackType;
      [ReadOnly]
      public ComponentTypeHandle<Waterway> m_WaterwayType;
      [ReadOnly]
      public ComponentTypeHandle<SubwayTrack> m_SubwayTrackType;
      [ReadOnly]
      public ComponentTypeHandle<NetCondition> m_NetConditionType;
      [ReadOnly]
      public ComponentTypeHandle<Road> m_RoadType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Pollution> m_PollutionType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NodeColor> nativeArray2 = chunk.GetNativeArray<NodeColor>(ref this.m_ColorType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedEdge> bufferAccessor = chunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
        bool flag1 = false;
        InfoviewNetStatusData statusData;
        InfomodeActive activeData;
        // ISSUE: reference to a compiler-generated method
        if (this.GetNetStatusType(chunk, out statusData, out activeData))
        {
          // ISSUE: reference to a compiler-generated method
          this.GetNetStatusColors(nativeArray2, chunk, statusData, activeData);
          flag1 = true;
        }
        else
        {
          int index1;
          // ISSUE: reference to a compiler-generated method
          if (this.GetNetGeometryColor(chunk, out index1))
          {
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
              nativeArray2[index2] = new NodeColor((byte) index1, (byte) 0);
            flag1 = true;
          }
        }
        for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
        {
          Entity original = nativeArray1[index3];
          DynamicBuffer<ConnectedEdge> dynamicBuffer = bufferAccessor[index3];
          Temp temp;
          DynamicBuffer<ConnectedEdge> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (CollectionUtils.TryGet<Temp>(nativeArray3, index3, out temp) && this.m_ConnectedEdges.TryGetBuffer(temp.m_Original, out bufferData))
          {
            original = temp.m_Original;
            dynamicBuffer = bufferData;
          }
          int3 int3_1 = new int3();
          bool flag2 = flag1;
          for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
          {
            Entity edge1 = dynamicBuffer[index4].m_Edge;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ColorData.HasComponent(edge1))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge2 = this.m_EdgeData[edge1];
              bool2 x = new bool2(edge2.m_Start == original, edge2.m_End == original);
              if (math.any(x))
              {
                if (flag2)
                {
                  if (x.x)
                  {
                    StartNodeGeometry componentData;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_StartNodeGeometryData.TryGetComponent(edge1, out componentData))
                      flag2 = math.any(componentData.m_Geometry.m_Left.m_Length > 0.05f) | math.any(componentData.m_Geometry.m_Right.m_Length > 0.05f);
                  }
                  else
                  {
                    EndNodeGeometry componentData;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_EndNodeGeometryData.TryGetComponent(edge1, out componentData))
                      flag2 = math.any(componentData.m_Geometry.m_Left.m_Length > 0.05f) | math.any(componentData.m_Geometry.m_Right.m_Length > 0.05f);
                  }
                }
                // ISSUE: reference to a compiler-generated field
                EdgeColor edgeColor = this.m_ColorData[edge1];
                if (edgeColor.m_Index != (byte) 0)
                {
                  int3 int3_2;
                  int3_2.x = (int) edgeColor.m_Index;
                  int3_2.y = x.x ? (int) edgeColor.m_Value0 : (int) edgeColor.m_Value1;
                  int3_2.z = 1;
                  if (int3_2.x == int3_1.x | int3_1.z == 0)
                  {
                    int3_1.x = int3_2.x;
                    int3_1.yz += int3_2.yz;
                  }
                  else
                    int3_1.z = -1;
                }
              }
            }
          }
          if (!flag2)
          {
            if (int3_1.z > 0)
            {
              int3_1.y /= int3_1.z;
              nativeArray2[index3] = new NodeColor((byte) int3_1.x, (byte) int3_1.y);
            }
            else
              nativeArray2[index3] = new NodeColor((byte) 0, byte.MaxValue);
          }
        }
      }

      private bool GetNetStatusType(
        ArchetypeChunk chunk,
        out InfoviewNetStatusData statusData,
        out InfomodeActive activeData)
      {
        statusData = new InfoviewNetStatusData();
        activeData = new InfomodeActive();
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewNetStatusData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewNetStatusData>(ref this.m_InfoviewNetStatusType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              if (priority < num)
              {
                InfoviewNetStatusData infoviewNetStatusData = nativeArray1[index2];
                // ISSUE: reference to a compiler-generated method
                if (this.HasNetStatus(nativeArray1[index2], chunk))
                {
                  statusData = infoviewNetStatusData;
                  activeData = infomodeActive;
                  num = priority;
                }
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasNetStatus(InfoviewNetStatusData infoviewNetStatusData, ArchetypeChunk chunk)
      {
        switch (infoviewNetStatusData.m_Type)
        {
          case NetStatusType.Wear:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<NetCondition>(ref this.m_NetConditionType);
          case NetStatusType.TrafficFlow:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Road>(ref this.m_RoadType);
          case NetStatusType.NoisePollutionSource:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Net.Pollution>(ref this.m_PollutionType);
          case NetStatusType.AirPollutionSource:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Net.Pollution>(ref this.m_PollutionType);
          case NetStatusType.TrafficVolume:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Road>(ref this.m_RoadType);
          default:
            return false;
        }
      }

      private float GetRelativeLength(Entity entity, DynamicBuffer<ConnectedEdge> edges)
      {
        float relativeLength = 0.0f;
        for (int index = 0; index < edges.Length; ++index)
        {
          Entity edge1 = edges[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge2 = this.m_EdgeData[edge1];
          bool2 x = new bool2(edge2.m_Start == entity, edge2.m_End == entity);
          if (math.any(x))
          {
            EdgeNodeGeometry edgeNodeGeometry = new EdgeNodeGeometry();
            if (x.x)
            {
              StartNodeGeometry componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_StartNodeGeometryData.TryGetComponent(edge1, out componentData))
                edgeNodeGeometry = componentData.m_Geometry;
            }
            else
            {
              EndNodeGeometry componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EndNodeGeometryData.TryGetComponent(edge1, out componentData))
                edgeNodeGeometry = componentData.m_Geometry;
            }
            if ((double) edgeNodeGeometry.m_MiddleRadius > 0.0)
              relativeLength += edgeNodeGeometry.m_Left.middleLength + edgeNodeGeometry.m_Right.middleLength;
            else
              relativeLength += (float) (((double) edgeNodeGeometry.m_Left.middleLength + (double) edgeNodeGeometry.m_Right.middleLength) * 0.5);
          }
        }
        return relativeLength;
      }

      private void GetNetStatusColors(
        NativeArray<NodeColor> results,
        ArchetypeChunk chunk,
        InfoviewNetStatusData statusData,
        InfomodeActive activeData)
      {
        switch (statusData.m_Type)
        {
          case NetStatusType.Wear:
            // ISSUE: reference to a compiler-generated field
            NativeArray<NetCondition> nativeArray1 = chunk.GetNativeArray<NetCondition>(ref this.m_NetConditionType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              NodeColor nodeColor;
              nodeColor.m_Index = (byte) activeData.m_Index;
              nodeColor.m_Value = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, math.cmax(nativeArray1[index].m_Wear) / 10f) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = nodeColor;
            }
            break;
          case NetStatusType.TrafficFlow:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Road> nativeArray2 = chunk.GetNativeArray<Road>(ref this.m_RoadType);
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              float4 trafficFlowSpeed = NetUtils.GetTrafficFlowSpeed(nativeArray2[index]);
              NodeColor nodeColor;
              nodeColor.m_Index = (byte) activeData.m_Index;
              nodeColor.m_Value = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, (float) ((double) math.csum(trafficFlowSpeed) * 0.125 + (double) math.cmin(trafficFlowSpeed) * 0.5)) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = nodeColor;
            }
            break;
          case NetStatusType.NoisePollutionSource:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.Pollution> nativeArray4 = chunk.GetNativeArray<Game.Net.Pollution>(ref this.m_PollutionType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<ConnectedEdge> bufferAccessor1 = chunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
            for (int index = 0; index < nativeArray4.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              float status = nativeArray4[index].m_Accumulation.x / math.max(0.1f, this.GetRelativeLength(nativeArray3[index], bufferAccessor1[index]));
              NodeColor nodeColor;
              nodeColor.m_Index = (byte) activeData.m_Index;
              nodeColor.m_Value = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = nodeColor;
            }
            break;
          case NetStatusType.AirPollutionSource:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray5 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.Pollution> nativeArray6 = chunk.GetNativeArray<Game.Net.Pollution>(ref this.m_PollutionType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<ConnectedEdge> bufferAccessor2 = chunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
            for (int index = 0; index < nativeArray6.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              float status = nativeArray6[index].m_Accumulation.y / math.max(0.1f, this.GetRelativeLength(nativeArray5[index], bufferAccessor2[index]));
              NodeColor nodeColor;
              nodeColor.m_Index = (byte) activeData.m_Index;
              nodeColor.m_Value = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = nodeColor;
            }
            break;
          case NetStatusType.TrafficVolume:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Road> nativeArray7 = chunk.GetNativeArray<Road>(ref this.m_RoadType);
            for (int index = 0; index < nativeArray7.Length; ++index)
            {
              Road road = nativeArray7[index];
              float4 x = math.sqrt((road.m_TrafficFlowDistance0 + road.m_TrafficFlowDistance1) * 2.66666675f);
              NodeColor nodeColor;
              nodeColor.m_Index = (byte) activeData.m_Index;
              nodeColor.m_Value = (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, math.csum(x) * 0.25f) * (float) byte.MaxValue), 0, (int) byte.MaxValue);
              results[index] = nodeColor;
            }
            break;
        }
      }

      private bool GetNetGeometryColor(ArchetypeChunk chunk, out int index)
      {
        index = 0;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewNetGeometryData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewNetGeometryData>(ref this.m_InfoviewNetGeometryType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              // ISSUE: reference to a compiler-generated method
              if (priority < num && this.HasNetGeometryColor(nativeArray1[index2], chunk))
              {
                index = infomodeActive.m_Index;
                num = priority;
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasNetGeometryColor(
        InfoviewNetGeometryData infoviewNetGeometryData,
        ArchetypeChunk chunk)
      {
        switch (infoviewNetGeometryData.m_Type)
        {
          case NetType.Road:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Road>(ref this.m_RoadType);
          case NetType.TrainTrack:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TrainTrack>(ref this.m_TrainTrackType);
          case NetType.TramTrack:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TramTrack>(ref this.m_TramTrackType);
          case NetType.Waterway:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Waterway>(ref this.m_WaterwayType);
          case NetType.SubwayTrack:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<SubwayTrack>(ref this.m_SubwayTrackType);
          default:
            return false;
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
    private struct UpdateEdgeColors2Job : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<NodeColor> m_ColorData;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> m_StartNodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> m_EndNodeGeometryType;
      public ComponentTypeHandle<EdgeColor> m_ColorType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Edge> nativeArray1 = chunk.GetNativeArray<Game.Net.Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<StartNodeGeometry> nativeArray2 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartNodeGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EndNodeGeometry> nativeArray3 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndNodeGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeColor> nativeArray4 = chunk.GetNativeArray<EdgeColor>(ref this.m_ColorType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Game.Net.Edge edge = nativeArray1[index];
          EdgeColor edgeColor = nativeArray4[index];
          bool2 bool2 = (bool2) false;
          if (nativeArray2.Length != 0)
          {
            StartNodeGeometry startNodeGeometry = nativeArray2[index];
            if (math.any(startNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(startNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
              bool2.x = true;
          }
          if (nativeArray3.Length != 0)
          {
            EndNodeGeometry endNodeGeometry = nativeArray3[index];
            if (math.any(endNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(endNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
              bool2.y = true;
          }
          NodeColor componentData1;
          // ISSUE: reference to a compiler-generated field
          if (!bool2.x && this.m_ColorData.TryGetComponent(edge.m_Start, out componentData1) && (int) componentData1.m_Index == (int) edgeColor.m_Index)
            edgeColor.m_Value0 = componentData1.m_Value;
          NodeColor componentData2;
          // ISSUE: reference to a compiler-generated field
          if (!bool2.y && this.m_ColorData.TryGetComponent(edge.m_End, out componentData2) && (int) componentData2.m_Index == (int) edgeColor.m_Index)
            edgeColor.m_Value1 = componentData2.m_Value;
          nativeArray4[index] = edgeColor;
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
    private struct LaneColorJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<NodeLane> m_NodeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.TrackLane> m_TrackLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> m_UtilityLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.SecondaryLane> m_SecondaryLaneType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeMapping> m_EdgeMappingType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<LaneColor> m_ColorType;
      public BufferTypeHandle<SubFlow> m_SubFlowType;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfomodeChunks;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> m_InfomodeActiveType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetGeometryData> m_InfoviewNetGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> m_InfoviewNetStatusType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> m_ObjectColorData;
      [ReadOnly]
      public ComponentLookup<NodeColor> m_NodeColorData;
      [ReadOnly]
      public ComponentLookup<EdgeColor> m_EdgeColorData;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnectionData;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_ElectricityFlowEdgeData;
      [ReadOnly]
      public ComponentLookup<ElectricityBuildingConnection> m_ElectricityBuildingConnectionData;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> m_WaterPipeNodeConnectionData;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> m_WaterPipeEdgeData;
      [ReadOnly]
      public ComponentLookup<WaterPipeBuildingConnection> m_WaterPipeBuildingConnectionData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumerData;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;
      public Entity m_ElectricitySinkNode;
      public Entity m_WaterSinkNode;
      public WaterPipeParameterData m_WaterPipeParameters;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<LaneColor> nativeArray1 = chunk.GetNativeArray<LaneColor>(ref this.m_ColorType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeLane> nativeArray4 = chunk.GetNativeArray<EdgeLane>(ref this.m_EdgeLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NodeLane> nativeArray5 = chunk.GetNativeArray<NodeLane>(ref this.m_NodeLaneType);
        int index1 = 0;
        int index2 = 0;
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        float num5 = 0.0f;
        float num6 = 0.0f;
        float num7 = 0.0f;
        float num8 = 0.0f;
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Game.Net.TrackLane>(ref this.m_TrackLaneType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<Game.Net.UtilityLane>(ref this.m_UtilityLaneType);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = chunk.Has<Game.Net.SecondaryLane>(ref this.m_SecondaryLaneType);
        // ISSUE: reference to a compiler-generated field
        bool flag4 = chunk.Has<Temp>(ref this.m_TempType);
        NativeArray<EdgeMapping> nativeArray6 = new NativeArray<EdgeMapping>();
        NativeArray<PrefabRef> nativeArray7 = new NativeArray<PrefabRef>();
        BufferAccessor<SubFlow> bufferAccessor = new BufferAccessor<SubFlow>();
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          int num9 = int.MaxValue;
          int num10 = int.MaxValue;
          // ISSUE: reference to a compiler-generated field
          for (int index3 = 0; index3 < this.m_InfomodeChunks.Length; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index3];
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewNetGeometryData> nativeArray8 = infomodeChunk.GetNativeArray<InfoviewNetGeometryData>(ref this.m_InfoviewNetGeometryType);
            if (nativeArray8.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<InfomodeActive> nativeArray9 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
              for (int index4 = 0; index4 < nativeArray8.Length; ++index4)
              {
                InfoviewNetGeometryData infoviewNetGeometryData = nativeArray8[index4];
                InfomodeActive infomodeActive = nativeArray9[index4];
                int priority = infomodeActive.m_Priority;
                switch (infoviewNetGeometryData.m_Type)
                {
                  case NetType.TrainTrack:
                    if (priority < num9)
                    {
                      index1 = infomodeActive.m_Index;
                      num9 = priority;
                      break;
                    }
                    break;
                  case NetType.TramTrack:
                    if (priority < num10)
                    {
                      index2 = infomodeActive.m_Index;
                      num10 = priority;
                      break;
                    }
                    break;
                }
              }
            }
          }
        }
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          nativeArray6 = chunk.GetNativeArray<EdgeMapping>(ref this.m_EdgeMappingType);
          // ISSUE: reference to a compiler-generated field
          nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          bufferAccessor = chunk.GetBufferAccessor<SubFlow>(ref this.m_SubFlowType);
          int num11 = int.MaxValue;
          int num12 = int.MaxValue;
          int num13 = int.MaxValue;
          int num14 = int.MaxValue;
          // ISSUE: reference to a compiler-generated field
          for (int index5 = 0; index5 < this.m_InfomodeChunks.Length; ++index5)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index5];
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewNetStatusData> nativeArray10 = infomodeChunk.GetNativeArray<InfoviewNetStatusData>(ref this.m_InfoviewNetStatusType);
            if (nativeArray10.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<InfomodeActive> nativeArray11 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
              for (int index6 = 0; index6 < nativeArray10.Length; ++index6)
              {
                InfoviewNetStatusData infoviewNetStatusData = nativeArray10[index6];
                InfomodeActive infomodeActive = nativeArray11[index6];
                int priority = infomodeActive.m_Priority;
                switch (infoviewNetStatusData.m_Type)
                {
                  case NetStatusType.LowVoltageFlow:
                    if (priority < num11)
                    {
                      num1 = infomodeActive.m_Index;
                      num5 = infoviewNetStatusData.m_Tiling;
                      num11 = priority;
                      break;
                    }
                    break;
                  case NetStatusType.HighVoltageFlow:
                    if (priority < num12)
                    {
                      num2 = infomodeActive.m_Index;
                      num6 = infoviewNetStatusData.m_Tiling;
                      num12 = priority;
                      break;
                    }
                    break;
                  case NetStatusType.PipeWaterFlow:
                    if (priority < num13)
                    {
                      num3 = infomodeActive.m_Index;
                      num7 = infoviewNetStatusData.m_Tiling;
                      num13 = priority;
                      break;
                    }
                    break;
                  case NetStatusType.PipeSewageFlow:
                    if (priority < num14)
                    {
                      num4 = infomodeActive.m_Index;
                      num8 = infoviewNetStatusData.m_Tiling;
                      num14 = priority;
                      break;
                    }
                    break;
                }
              }
            }
          }
        }
        bool flag5 = flag1 && (index1 != 0 || index2 != 0);
        bool flag6 = flag2 && bufferAccessor.Length != 0 && (num1 != 0 || num2 != 0 || num3 != 0 || num4 != 0);
        for (int index7 = 0; index7 < nativeArray1.Length; ++index7)
        {
          TrackLaneData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (flag5 && this.m_PrefabTrackLaneData.TryGetComponent(nativeArray7[index7].m_Prefab, out componentData1))
          {
            if ((componentData1.m_TrackTypes & TrackTypes.Train) != 0 & index1 != 0)
            {
              nativeArray1[index7] = new LaneColor((byte) index1, (byte) 0, (byte) 0);
              continue;
            }
            if ((componentData1.m_TrackTypes & TrackTypes.Tram) != 0 & index2 != 0)
            {
              nativeArray1[index7] = new LaneColor((byte) index2, (byte) 0, (byte) 0);
              continue;
            }
          }
          UtilityLaneData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (flag6 && this.m_PrefabUtilityLaneData.TryGetComponent(nativeArray7[index7].m_Prefab, out componentData2))
          {
            int index8 = 0;
            float num15 = 0.0f;
            if ((componentData2.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None && num1 != 0)
            {
              index8 = num1;
              num15 = num5;
            }
            else if ((componentData2.m_UtilityTypes & UtilityTypes.HighVoltageLine) != UtilityTypes.None && num2 != 0)
            {
              index8 = num2;
              num15 = num6;
            }
            else if ((componentData2.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None && num3 != 0)
            {
              index8 = num3;
              num15 = num7;
            }
            else if ((componentData2.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None && num4 != 0)
            {
              index8 = num4;
              num15 = num8;
            }
            if (index8 != 0)
            {
              Curve curve = nativeArray3[index7];
              EdgeMapping edgeMapping = nativeArray6[index7];
              DynamicBuffer<SubFlow> dynamicBuffer = bufferAccessor[index7];
              Owner owner = new Owner();
              if (nativeArray2.Length != 0)
                owner = nativeArray2[index7];
              if (dynamicBuffer.Length != 16)
                dynamicBuffer.ResizeUninitialized(16);
              NativeArray<SubFlow> nativeArray12 = dynamicBuffer.AsNativeArray();
              float warning = 0.0f;
              if (edgeMapping.m_Parent1 != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_EdgeData.HasComponent(edgeMapping.m_Parent1))
                {
                  if (flag4)
                  {
                    if (edgeMapping.m_Parent2 != Entity.Null)
                    {
                      Bezier4x3 output1;
                      Bezier4x3 output2;
                      MathUtils.Divide(curve.m_Bezier, out output1, out output2, 0.5f);
                      // ISSUE: reference to a compiler-generated method
                      this.GetOriginalEdge(output1, ref edgeMapping.m_Parent1, ref edgeMapping.m_CurveDelta1);
                      // ISSUE: reference to a compiler-generated method
                      this.GetOriginalEdge(output2, ref edgeMapping.m_Parent2, ref edgeMapping.m_CurveDelta2);
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GetOriginalEdge(curve.m_Bezier, ref edgeMapping.m_Parent1, ref edgeMapping.m_CurveDelta1);
                    }
                  }
                  if (index8 == num1 || index8 == num2)
                  {
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    this.FillEdgeFlow<NetColorSystem.LaneColorJob.ElectricityFlow>(this.GetElectricityFlow(), nativeArray12, edgeMapping, out warning);
                  }
                  else if (index8 == num3)
                  {
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    this.FillEdgeFlow<NetColorSystem.LaneColorJob.WaterFlow>(this.GetWaterFlow(), nativeArray12, edgeMapping, out warning);
                  }
                  else if (index8 == num4)
                  {
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    this.FillEdgeFlow<NetColorSystem.LaneColorJob.SewageFlow>(this.GetSewageFlow(), nativeArray12, edgeMapping, out warning);
                  }
                  else
                    nativeArray12.Fill<SubFlow>(new SubFlow());
                }
                else
                {
                  if (flag4)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GetOriginalNode(ref edgeMapping.m_Parent1);
                    // ISSUE: reference to a compiler-generated method
                    this.GetOriginalEdge(curve.m_Bezier, ref edgeMapping.m_Parent2, ref edgeMapping.m_CurveDelta2);
                  }
                  if (index8 == num1 || index8 == num2)
                  {
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    this.FillNodeFlow<NetColorSystem.LaneColorJob.ElectricityFlow>(this.GetElectricityFlow(), nativeArray12, edgeMapping, out warning);
                  }
                  else if (index8 == num3)
                  {
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    this.FillNodeFlow<NetColorSystem.LaneColorJob.WaterFlow>(this.GetWaterFlow(), nativeArray12, edgeMapping, out warning);
                  }
                  else if (index8 == num4)
                  {
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    this.FillNodeFlow<NetColorSystem.LaneColorJob.SewageFlow>(this.GetSewageFlow(), nativeArray12, edgeMapping, out warning);
                  }
                  else
                    nativeArray12.Fill<SubFlow>(new SubFlow());
                }
              }
              else if (flag3)
              {
                if (index8 == num1 || index8 == num2)
                {
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  this.FillBuildingFlow<NetColorSystem.LaneColorJob.ElectricityFlow>(this.GetElectricityFlow(), nativeArray12, owner.m_Owner, out warning);
                }
                else if (index8 == num3)
                {
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  this.FillBuildingFlow<NetColorSystem.LaneColorJob.WaterFlow>(this.GetWaterFlow(), nativeArray12, owner.m_Owner, out warning);
                }
                else if (index8 == num4)
                {
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  this.FillBuildingFlow<NetColorSystem.LaneColorJob.SewageFlow>(this.GetSewageFlow(), nativeArray12, owner.m_Owner, out warning);
                }
                else
                  nativeArray12.Fill<SubFlow>(new SubFlow());
              }
              else
                index8 = 0;
              if (index8 != 0)
              {
                int2 int2 = new int2((int) dynamicBuffer[0].m_Value, (int) dynamicBuffer[15].m_Value);
                bool flag7 = ((int2.x ^ int2.y) & 128) != 0 & math.all(int2 != 0);
                int a = math.clamp(Mathf.RoundToInt(curve.m_Length * num15), 1, (int) byte.MaxValue);
                int num16 = math.clamp(Mathf.RoundToInt(warning * (float) byte.MaxValue), 0, (int) byte.MaxValue);
                int num17 = math.select(a, 2, a == 1 & flag7);
                nativeArray1[index7] = new LaneColor((byte) index8, (byte) num17, (byte) num16);
                continue;
              }
            }
          }
          if (nativeArray2.Length != 0)
          {
            Owner owner = nativeArray2[index7];
            if (nativeArray4.Length != 0)
            {
              EdgeColor componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EdgeColorData.TryGetComponent(owner.m_Owner, out componentData3))
              {
                float2 float2 = math.lerp((float2) (float) componentData3.m_Value0, (float2) (float) componentData3.m_Value1, nativeArray4[index7].m_EdgeDelta);
                nativeArray1[index7] = new LaneColor(componentData3.m_Index, (byte) Mathf.RoundToInt(float2.x), (byte) Mathf.RoundToInt(float2.y));
                continue;
              }
            }
            else if (nativeArray5.Length != 0)
            {
              NodeColor componentData4;
              // ISSUE: reference to a compiler-generated field
              if (this.m_NodeColorData.TryGetComponent(owner.m_Owner, out componentData4))
              {
                nativeArray1[index7] = new LaneColor(componentData4.m_Index, componentData4.m_Value, componentData4.m_Value);
                continue;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_PrefabNetLaneData[nativeArray7[index7].m_Prefab].m_Flags & Game.Prefabs.LaneFlags.Underground) == (Game.Prefabs.LaneFlags) 0)
              {
                Game.Objects.Color componentData5;
                Owner componentData6;
                // ISSUE: reference to a compiler-generated field
                for (; !this.m_ObjectColorData.TryGetComponent(owner.m_Owner, out componentData5); owner = componentData6)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData6))
                    goto label_97;
                }
                if (componentData5.m_SubColor)
                {
                  nativeArray1[index7] = new LaneColor(componentData5.m_Index, componentData5.m_Value, componentData5.m_Value);
                  continue;
                }
              }
            }
          }
label_97:
          nativeArray1[index7] = new LaneColor();
        }
      }

      private void GetOriginalEdge(Bezier4x3 laneCurve, ref Entity parent, ref float2 curveMapping)
      {
        Temp componentData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.TryGetComponent(parent, out componentData1))
          return;
        if (componentData1.m_Original != Entity.Null)
        {
          parent = componentData1.m_Original;
        }
        else
        {
          Game.Net.Edge componentData2;
          Temp componentData3;
          Temp componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EdgeData.TryGetComponent(parent, out componentData2) || !this.m_TempData.TryGetComponent(componentData2.m_Start, out componentData3) || !this.m_TempData.TryGetComponent(componentData2.m_End, out componentData4) || !(componentData3.m_Original != Entity.Null) || !(componentData4.m_Original != Entity.Null))
            return;
          Curve componentData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.TryGetComponent(componentData3.m_Original, out componentData5))
          {
            parent = componentData3.m_Original;
            double num1 = (double) MathUtils.Distance(componentData5.m_Bezier.xz, laneCurve.a.xz, out curveMapping.x);
            double num2 = (double) MathUtils.Distance(componentData5.m_Bezier.xz, laneCurve.d.xz, out curveMapping.y);
          }
          else
          {
            Curve componentData6;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CurveData.TryGetComponent(componentData4.m_Original, out componentData6))
              return;
            parent = componentData4.m_Original;
            double num3 = (double) MathUtils.Distance(componentData6.m_Bezier.xz, laneCurve.a.xz, out curveMapping.x);
            double num4 = (double) MathUtils.Distance(componentData6.m_Bezier.xz, laneCurve.d.xz, out curveMapping.y);
          }
        }
      }

      private void GetOriginalNode(ref Entity parent)
      {
        Temp componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.TryGetComponent(parent, out componentData))
          return;
        parent = componentData.m_Original;
      }

      private void FillEdgeFlow<T>(
        T impl,
        NativeArray<SubFlow> flowArray,
        EdgeMapping edgeMapping,
        out float warning)
        where T : struct, NetColorSystem.LaneColorJob.IFlowImplementation
      {
        if (edgeMapping.m_Parent2 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.FillEdgeFlow<T>(impl, flowArray.GetSubArray(0, 8), edgeMapping.m_Parent1, edgeMapping.m_CurveDelta1, out warning);
          // ISSUE: reference to a compiler-generated method
          this.FillEdgeFlow<T>(impl, flowArray.GetSubArray(8, 8), edgeMapping.m_Parent2, edgeMapping.m_CurveDelta2, out warning);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.FillEdgeFlow<T>(impl, flowArray, edgeMapping.m_Parent1, edgeMapping.m_CurveDelta1, out warning);
        }
      }

      private unsafe void FillEdgeFlow<T>(
        T impl,
        NativeArray<SubFlow> flows,
        Entity edge,
        float2 curveMapping,
        out float warning)
        where T : struct, NetColorSystem.LaneColorJob.IFlowImplementation
      {
        Game.Net.Edge componentData;
        Entity flowNode1;
        Entity flowNode2;
        Entity flowNode3;
        int flow1;
        int capacity1;
        float warning1;
        int flow2;
        int num1;
        float warning2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (this.m_EdgeData.TryGetComponent(edge, out componentData) && impl.TryGetFlowNode(edge, out flowNode1) && impl.TryGetFlowNode(componentData.m_Start, out flowNode2) && impl.TryGetFlowNode(componentData.m_End, out flowNode3) && impl.TryGetFlowEdge(flowNode2, flowNode1, out flow1, out capacity1, out warning1) && impl.TryGetFlowEdge(flowNode1, flowNode3, out flow2, out num1, out warning2))
        {
          int capacity2 = math.max(1, capacity1);
          if ((double) curveMapping.y < (double) curveMapping.x)
          {
            num1 = -flow2;
            int num2 = -flow1;
            flow1 = num1;
            flow2 = num2;
          }
          int* tempFlows = stackalloc int[flows.Length];
          DynamicBuffer<ConnectedNode> bufferData1;
          float warning3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedNodes.TryGetBuffer(edge, out bufferData1))
          {
            foreach (ConnectedNode connectedNode in bufferData1)
            {
              Entity flowNode4;
              int flow3;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (impl.TryGetFlowNode(connectedNode.m_Node, out flowNode4) && impl.TryGetFlowEdge(flowNode4, flowNode1, out flow3, out num1, out warning3))
              {
                // ISSUE: reference to a compiler-generated method
                NetColorSystem.LaneColorJob.AddTempFlow(flow3, connectedNode.m_CurvePosition, tempFlows, flows.Length, curveMapping);
              }
            }
          }
          int flow4;
          DynamicBuffer<ConnectedBuilding> bufferData2;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (impl.TryGetFlowEdge(flowNode1, impl.sinkNode, out flow4, out num1, out warning3) && this.m_ConnectedBuildings.TryGetBuffer(edge, out bufferData2))
          {
            int totalDemand = 0;
            foreach (ConnectedBuilding connectedBuilding in bufferData2)
            {
              int wantedConsumption;
              // ISSUE: reference to a compiler-generated method
              impl.GetConsumption(connectedBuilding.m_Building, out wantedConsumption, out num1, out warning3);
              totalDemand += wantedConsumption;
            }
            foreach (ConnectedBuilding connectedBuilding in bufferData2)
            {
              int wantedConsumption;
              // ISSUE: reference to a compiler-generated method
              impl.GetConsumption(connectedBuilding.m_Building, out wantedConsumption, out num1, out warning3);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              NetColorSystem.LaneColorJob.AddTempFlow(-FlowUtils.ConsumeFromTotal(wantedConsumption, ref flow4, ref totalDemand), this.m_BuildingData[connectedBuilding.m_Building].m_CurvePosition, tempFlows, flows.Length, curveMapping);
            }
          }
          int num3 = flow1;
          for (int index = 0; index < flows.Length; ++index)
          {
            num3 += tempFlows[index];
            // ISSUE: reference to a compiler-generated method
            flows[index] = NetColorSystem.LaneColorJob.GetSubFlow(impl.multiplier * num3, capacity2);
          }
          if ((double) MathUtils.Max(curveMapping) == 1.0)
          {
            // ISSUE: reference to a compiler-generated method
            flows[flows.Length - 1] = NetColorSystem.LaneColorJob.GetSubFlow(impl.multiplier * flow2, capacity2);
          }
          warning = math.max(warning1, warning2);
        }
        else
        {
          flows.Fill<SubFlow>(new SubFlow());
          warning = 0.0f;
        }
      }

      private static unsafe void AddTempFlow(
        int flow,
        float curvePosition,
        int* tempFlows,
        int length,
        float2 curveMapping)
      {
        float num1 = curveMapping.y - curveMapping.x;
        if ((double) num1 != 0.0)
        {
          float num2 = (curvePosition - curveMapping.x) / num1;
          if ((double) num2 < 0.0)
          {
            int* numPtr = tempFlows;
            int num3 = *numPtr + flow;
            *numPtr = num3;
          }
          else
          {
            if ((double) num2 >= 1.0)
              return;
            int num4 = math.clamp(Mathf.RoundToInt(num2 * (float) (length - 1)), 1, length - 1);
            int* numPtr = tempFlows + num4;
            *numPtr = *numPtr + flow;
          }
        }
        else
        {
          if ((double) curvePosition >= (double) curveMapping.x)
            return;
          int* numPtr = tempFlows;
          int num5 = *numPtr + flow;
          *numPtr = num5;
        }
      }

      private static SubFlow GetSubFlow(int flow, int capacity)
      {
        int x = (int) sbyte.MaxValue * flow / capacity;
        return new SubFlow()
        {
          m_Value = x != 0 ? (sbyte) math.clamp(x, -127, (int) sbyte.MaxValue) : (sbyte) math.clamp(flow, -1, 1)
        };
      }

      private void FillNodeFlow<T>(
        T impl,
        NativeArray<SubFlow> flows,
        EdgeMapping edgeMapping,
        out float warning)
        where T : struct, NetColorSystem.LaneColorJob.IFlowImplementation
      {
        // ISSUE: reference to a compiler-generated method
        this.FillNodeFlow<T>(impl, flows, edgeMapping.m_Parent1, edgeMapping.m_Parent2, edgeMapping.m_CurveDelta1, out warning);
      }

      private void FillNodeFlow<T>(
        T impl,
        NativeArray<SubFlow> flows,
        Entity node,
        Entity edge,
        float2 curveMapping,
        out float warning)
        where T : struct, NetColorSystem.LaneColorJob.IFlowImplementation
      {
        float a = 0.0f;
        Entity flowNode1;
        Entity flowNode2;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (impl.TryGetFlowNode(node, out flowNode1) && impl.TryGetFlowNode(edge, out flowNode2))
        {
          int flow;
          int capacity;
          // ISSUE: reference to a compiler-generated method
          if (impl.TryGetFlowEdge(flowNode1, flowNode2, out flow, out capacity, out warning))
          {
            a = (float) flow / (float) capacity;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            if (impl.TryGetFlowEdge(flowNode2, flowNode1, out flow, out capacity, out warning))
              a = (float) -flow / (float) capacity;
          }
        }
        else
          warning = 0.0f;
        float num = math.select(a, -a, (double) curveMapping.y < (double) curveMapping.x);
        // ISSUE: reference to a compiler-generated method
        SubFlow subFlow = this.GetSubFlow((float) impl.multiplier * num);
        flows.Fill<SubFlow>(subFlow);
      }

      private void FillBuildingFlow<T>(
        T impl,
        NativeArray<SubFlow> flows,
        Entity building,
        out float warning)
        where T : struct, NetColorSystem.LaneColorJob.IFlowImplementation
      {
        int fulfilledConsumption;
        // ISSUE: reference to a compiler-generated method
        impl.GetConsumption(building, out int _, out fulfilledConsumption, out warning);
        float num = (float) -(double) fulfilledConsumption / (float) (10000 + math.abs(fulfilledConsumption));
        // ISSUE: reference to a compiler-generated method
        SubFlow subFlow = this.GetSubFlow((float) impl.multiplier * num);
        flows.Fill<SubFlow>(subFlow);
      }

      private SubFlow GetSubFlow(float value)
      {
        int a1 = math.clamp(Mathf.RoundToInt(value * (float) sbyte.MaxValue), -127, (int) sbyte.MaxValue);
        int a2 = math.select(a1, 1, a1 == 0 & (double) value > 0.0);
        int num = math.select(a2, -1, a2 == 0 & (double) value < 0.0);
        return new SubFlow() { m_Value = (sbyte) num };
      }

      private NetColorSystem.LaneColorJob.ElectricityFlow GetElectricityFlow()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new NetColorSystem.LaneColorJob.ElectricityFlow()
        {
          sinkNode = this.m_ElectricitySinkNode,
          m_NodeConnectionData = this.m_ElectricityNodeConnectionData,
          m_FlowEdgeData = this.m_ElectricityFlowEdgeData,
          m_BuildingConnectionData = this.m_ElectricityBuildingConnectionData,
          m_ConsumerData = this.m_ElectricityConsumerData,
          m_ConnectedFlowEdges = this.m_ConnectedFlowEdges
        };
      }

      private NetColorSystem.LaneColorJob.WaterFlow GetWaterFlow()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new NetColorSystem.LaneColorJob.WaterFlow()
        {
          sinkNode = this.m_WaterSinkNode,
          m_NodeConnectionData = this.m_WaterPipeNodeConnectionData,
          m_FlowEdgeData = this.m_WaterPipeEdgeData,
          m_BuildingConnectionData = this.m_WaterPipeBuildingConnectionData,
          m_ConsumerData = this.m_WaterConsumerData,
          m_ConnectedFlowEdges = this.m_ConnectedFlowEdges,
          m_MaxToleratedPollution = this.m_WaterPipeParameters.m_MaxToleratedPollution
        };
      }

      private NetColorSystem.LaneColorJob.SewageFlow GetSewageFlow()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new NetColorSystem.LaneColorJob.SewageFlow()
        {
          sinkNode = this.m_WaterSinkNode,
          m_NodeConnectionData = this.m_WaterPipeNodeConnectionData,
          m_FlowEdgeData = this.m_WaterPipeEdgeData,
          m_BuildingConnectionData = this.m_WaterPipeBuildingConnectionData,
          m_ConsumerData = this.m_WaterConsumerData,
          m_ConnectedFlowEdges = this.m_ConnectedFlowEdges
        };
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

      private interface IFlowImplementation
      {
        Entity sinkNode { get; }

        int multiplier { get; }

        bool TryGetFlowNode(Entity entity, out Entity flowNode);

        bool TryGetFlowEdge(
          Entity startNode,
          Entity endNode,
          out int flow,
          out int capacity,
          out float warning);

        void GetConsumption(
          Entity building,
          out int wantedConsumption,
          out int fulfilledConsumption,
          out float warning);
      }

      private struct ElectricityFlow : NetColorSystem.LaneColorJob.IFlowImplementation
      {
        [ReadOnly]
        public ComponentLookup<ElectricityNodeConnection> m_NodeConnectionData;
        [ReadOnly]
        public ComponentLookup<ElectricityFlowEdge> m_FlowEdgeData;
        [ReadOnly]
        public ComponentLookup<ElectricityBuildingConnection> m_BuildingConnectionData;
        [ReadOnly]
        public ComponentLookup<ElectricityConsumer> m_ConsumerData;
        [ReadOnly]
        public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;

        public Entity sinkNode { get; set; }

        public int multiplier => 1;

        public bool TryGetFlowNode(Entity entity, out Entity flowNode)
        {
          ElectricityNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeConnectionData.TryGetComponent(entity, out componentData))
          {
            flowNode = componentData.m_ElectricityNode;
            return true;
          }
          flowNode = new Entity();
          return false;
        }

        public bool TryGetFlowEdge(
          Entity startNode,
          Entity endNode,
          out int flow,
          out int capacity,
          out float warning)
        {
          ElectricityFlowEdge edge;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (ElectricityGraphUtils.TryGetFlowEdge(startNode, endNode, ref this.m_ConnectedFlowEdges, ref this.m_FlowEdgeData, out edge))
          {
            flow = edge.m_Flow;
            capacity = edge.m_Capacity;
            warning = math.select(0.0f, 0.75f, (edge.m_Flags & ElectricityFlowEdgeFlags.BeyondBottleneck) != 0);
            warning = math.select(warning, 1f, (edge.m_Flags & ElectricityFlowEdgeFlags.Bottleneck) != 0);
            return true;
          }
          flow = capacity = 0;
          warning = 0.0f;
          return false;
        }

        public void GetConsumption(
          Entity building,
          out int wantedConsumption,
          out int fulfilledConsumption,
          out float warning)
        {
          ElectricityConsumer componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConsumerData.TryGetComponent(building, out componentData) && !this.m_BuildingConnectionData.HasComponent(building))
          {
            wantedConsumption = componentData.m_WantedConsumption;
            fulfilledConsumption = componentData.m_FulfilledConsumption;
            warning = math.select(0.0f, 0.75f, (componentData.m_Flags & ElectricityConsumerFlags.BottleneckWarning) != 0);
          }
          else
          {
            wantedConsumption = fulfilledConsumption = 0;
            warning = 0.0f;
          }
        }
      }

      private struct WaterFlow : NetColorSystem.LaneColorJob.IFlowImplementation
      {
        [ReadOnly]
        public ComponentLookup<WaterPipeNodeConnection> m_NodeConnectionData;
        [ReadOnly]
        public ComponentLookup<WaterPipeEdge> m_FlowEdgeData;
        [ReadOnly]
        public ComponentLookup<WaterPipeBuildingConnection> m_BuildingConnectionData;
        [ReadOnly]
        public ComponentLookup<WaterConsumer> m_ConsumerData;
        [ReadOnly]
        public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;
        public float m_MaxToleratedPollution;

        public Entity sinkNode { get; set; }

        public int multiplier => 1;

        public bool TryGetFlowNode(Entity entity, out Entity flowNode)
        {
          WaterPipeNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeConnectionData.TryGetComponent(entity, out componentData))
          {
            flowNode = componentData.m_WaterPipeNode;
            return true;
          }
          flowNode = new Entity();
          return false;
        }

        public bool TryGetFlowEdge(
          Entity startNode,
          Entity endNode,
          out int flow,
          out int capacity,
          out float warning)
        {
          WaterPipeEdge edge;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (WaterPipeGraphUtils.TryGetFlowEdge(startNode, endNode, ref this.m_ConnectedFlowEdges, ref this.m_FlowEdgeData, out edge))
          {
            flow = edge.m_FreshFlow;
            capacity = 10000;
            // ISSUE: reference to a compiler-generated field
            warning = math.saturate(edge.m_FreshPollution / this.m_MaxToleratedPollution);
            return true;
          }
          flow = capacity = 0;
          warning = 0.0f;
          return false;
        }

        public void GetConsumption(
          Entity building,
          out int wantedConsumption,
          out int fulfilledConsumption,
          out float warning)
        {
          WaterConsumer componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConsumerData.TryGetComponent(building, out componentData) && !this.m_BuildingConnectionData.HasComponent(building))
          {
            wantedConsumption = componentData.m_WantedConsumption;
            fulfilledConsumption = componentData.m_FulfilledFresh;
            warning = math.select(0.0f, 1f, (double) componentData.m_Pollution > 0.0);
          }
          else
          {
            wantedConsumption = fulfilledConsumption = 0;
            warning = 0.0f;
          }
        }
      }

      private struct SewageFlow : NetColorSystem.LaneColorJob.IFlowImplementation
      {
        [ReadOnly]
        public ComponentLookup<WaterPipeNodeConnection> m_NodeConnectionData;
        [ReadOnly]
        public ComponentLookup<WaterPipeEdge> m_FlowEdgeData;
        [ReadOnly]
        public ComponentLookup<WaterPipeBuildingConnection> m_BuildingConnectionData;
        [ReadOnly]
        public ComponentLookup<WaterConsumer> m_ConsumerData;
        [ReadOnly]
        public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;

        public Entity sinkNode { get; set; }

        public int multiplier => -1;

        public bool TryGetFlowNode(Entity entity, out Entity flowNode)
        {
          WaterPipeNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeConnectionData.TryGetComponent(entity, out componentData))
          {
            flowNode = componentData.m_WaterPipeNode;
            return true;
          }
          flowNode = new Entity();
          return false;
        }

        public bool TryGetFlowEdge(
          Entity startNode,
          Entity endNode,
          out int flow,
          out int capacity,
          out float warning)
        {
          WaterPipeEdge edge;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (WaterPipeGraphUtils.TryGetFlowEdge(startNode, endNode, ref this.m_ConnectedFlowEdges, ref this.m_FlowEdgeData, out edge))
          {
            flow = edge.m_SewageFlow;
            capacity = 10000;
            warning = 0.0f;
            return true;
          }
          flow = capacity = 0;
          warning = 0.0f;
          return false;
        }

        public void GetConsumption(
          Entity building,
          out int wantedConsumption,
          out int fulfilledConsumption,
          out float warning)
        {
          WaterConsumer componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConsumerData.TryGetComponent(building, out componentData) && !this.m_BuildingConnectionData.HasComponent(building))
          {
            wantedConsumption = componentData.m_WantedConsumption;
            fulfilledConsumption = componentData.m_FulfilledSewage;
          }
          else
            wantedConsumption = fulfilledConsumption = 0;
          warning = 0.0f;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> __Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewCoverageData> __Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewAvailabilityData> __Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetGeometryData> __Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> __Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrainTrack> __Game_Net_TrainTrack_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TramTrack> __Game_Net_TramTrack_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Waterway> __Game_Net_Waterway_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SubwayTrack> __Game_Net_SubwayTrack_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCondition> __Game_Net_NetCondition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Road> __Game_Net_Road_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Pollution> __Game_Net_Pollution_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> __Game_Prefabs_ZonePropertiesData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ProcessEstimate> __Game_Zones_ProcessEstimate_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      public ComponentTypeHandle<EdgeColor> __Game_Net_EdgeColor_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<EdgeColor> __Game_Net_EdgeColor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferTypeHandle;
      public ComponentTypeHandle<NodeColor> __Game_Net_NodeColor_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<NodeColor> __Game_Net_NodeColor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> __Game_Net_EdgeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NodeLane> __Game_Net_NodeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeMapping> __Game_Net_EdgeMapping_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<LaneColor> __Game_Net_LaneColor_RW_ComponentTypeHandle;
      public BufferTypeHandle<SubFlow> __Game_Net_SubFlow_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> __Game_Objects_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> __Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfomodeActive>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewCoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewAvailabilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewNetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewNetStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrainTrack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TramTrack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TramTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Waterway_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Waterway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubwayTrack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SubwayTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Pollution_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Pollution>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferTypeHandle = state.GetBufferTypeHandle<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup = state.GetComponentLookup<ZonePropertiesData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_ProcessEstimate_RO_BufferLookup = state.GetBufferLookup<ProcessEstimate>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeColor_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeColor>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeColor_RO_ComponentLookup = state.GetComponentLookup<EdgeColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeColor_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NodeColor>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeColor_RO_ComponentLookup = state.GetComponentLookup<NodeColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NodeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeMapping_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeMapping>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneColor_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LaneColor>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubFlow_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubFlow>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
      }
    }
  }
}
