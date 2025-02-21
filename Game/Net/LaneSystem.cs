// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class LaneSystem : GameSystemBase
  {
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ToolSystem m_ToolSystem;
    private TerrainSystem m_TerrainSystem;
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_OwnerQuery;
    private EntityQuery m_BuildingSettingsQuery;
    private ComponentTypeSet m_AppliedTypes;
    private ComponentTypeSet m_DeletedTempTypes;
    private ComponentTypeSet m_TempOwnerTypes;
    private LaneSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<SubLane>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<OutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Area>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedTempTypes = new ComponentTypeSet(ComponentType.ReadWrite<Deleted>(), ComponentType.ReadWrite<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempOwnerTypes = new ComponentTypeSet(ComponentType.ReadWrite<Temp>(), ComponentType.ReadWrite<Owner>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_OwnerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionPiece_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AuxiliaryNetLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DefaultNetLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TaxiwayComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathwayComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterwayComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new LaneSystem.UpdateLanesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
        m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_OrphanType = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_ElevationType = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle,
        m_UnderConstructionType = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_ConnectedNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_OverriddenData = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AreaClearData = this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup,
        m_TrackData = this.__TypeHandle.__Game_Prefabs_TrackComposition_RO_ComponentLookup,
        m_WaterwayData = this.__TypeHandle.__Game_Prefabs_WaterwayComposition_RO_ComponentLookup,
        m_PathwayData = this.__TypeHandle.__Game_Prefabs_PathwayComposition_RO_ComponentLookup,
        m_TaxiwayData = this.__TypeHandle.__Game_Prefabs_TaxiwayComposition_RO_ComponentLookup,
        m_PrefabLaneArchetypeData = this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup,
        m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_UtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabNetObjectData = this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_Nodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_CutRanges = this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_PrefabCompositionLanes = this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup,
        m_PrefabCompositionCrosswalks = this.__TypeHandle.__Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup,
        m_DefaultNetLanes = this.__TypeHandle.__Game_Prefabs_DefaultNetLane_RO_BufferLookup,
        m_PrefabSubLanes = this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup,
        m_PlaceholderObjects = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_ObjectRequirements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup,
        m_PrefabAuxiliaryLanes = this.__TypeHandle.__Game_Prefabs_AuxiliaryNetLane_RO_BufferLookup,
        m_PrefabCompositionPieces = this.__TypeHandle.__Game_Prefabs_NetCompositionPiece_RO_BufferLookup,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_RandomSeed = RandomSeed.Next(),
        m_DefaultTheme = this.m_CityConfigurationSystem.defaultTheme,
        m_AppliedTypes = this.m_AppliedTypes,
        m_DeletedTempTypes = this.m_DeletedTempTypes,
        m_TempOwnerTypes = this.m_TempOwnerTypes,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_BuildingConfigurationData = this.m_BuildingSettingsQuery.GetSingleton<BuildingConfigurationData>(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<LaneSystem.UpdateLanesJob>(this.m_OwnerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
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
    public LaneSystem()
    {
    }

    private struct LaneKey : IEquatable<LaneSystem.LaneKey>
    {
      private Lane m_Lane;
      private Entity m_Prefab;
      private Game.Prefabs.LaneFlags m_Flags;

      public LaneKey(Lane lane, Entity prefab, Game.Prefabs.LaneFlags flags)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = lane;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = prefab;
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = flags & (Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
      }

      public void ReplaceOwner(Entity oldOwner, Entity newOwner)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.m_StartNode.ReplaceOwner(oldOwner, newOwner);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.m_MiddleNode.ReplaceOwner(oldOwner, newOwner);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.m_EndNode.ReplaceOwner(oldOwner, newOwner);
      }

      public bool Equals(LaneSystem.LaneKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Lane.Equals(other.m_Lane) && this.m_Prefab.Equals(other.m_Prefab) && this.m_Flags == other.m_Flags;
      }

      public override int GetHashCode() => this.m_Lane.GetHashCode();
    }

    private struct ConnectionKey : IEquatable<LaneSystem.ConnectionKey>
    {
      private int4 m_Data;

      public ConnectionKey(
        LaneSystem.ConnectPosition sourcePosition,
        LaneSystem.ConnectPosition targetPosition)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Data.x = sourcePosition.m_Owner.Index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Data.y = (int) sourcePosition.m_LaneData.m_Index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Data.z = targetPosition.m_Owner.Index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Data.w = (int) targetPosition.m_LaneData.m_Index;
      }

      public bool Equals(LaneSystem.ConnectionKey other) => this.m_Data.Equals(other.m_Data);

      public override int GetHashCode() => this.m_Data.GetHashCode();
    }

    private struct ConnectPosition
    {
      public NetCompositionLane m_LaneData;
      public Entity m_Owner;
      public Entity m_NodeComposition;
      public Entity m_EdgeComposition;
      public float3 m_Position;
      public float3 m_Tangent;
      public float m_Order;
      public LaneSystem.CompositionData m_CompositionData;
      public float m_CurvePosition;
      public float m_BaseHeight;
      public float m_Elevation;
      public ushort m_GroupIndex;
      public byte m_SegmentIndex;
      public byte m_UnsafeCount;
      public byte m_ForbiddenCount;
      public TrackTypes m_TrackTypes;
      public UtilityTypes m_UtilityTypes;
      public bool m_IsEnd;
      public bool m_IsSideConnection;
    }

    private struct EdgeTarget
    {
      public Entity m_Edge;
      public Entity m_StartNode;
      public Entity m_EndNode;
      public float3 m_StartPos;
      public float3 m_StartTangent;
      public float3 m_EndPos;
      public float3 m_EndTangent;
    }

    private struct MiddleConnection
    {
      public LaneSystem.ConnectPosition m_ConnectPosition;
      public Entity m_SourceEdge;
      public Entity m_SourceNode;
      public Curve m_TargetCurve;
      public float m_TargetCurvePos;
      public float m_Distance;
      public LaneSystem.CompositionData m_TargetComposition;
      public Entity m_TargetLane;
      public Entity m_TargetOwner;
      public Game.Prefabs.LaneFlags m_TargetFlags;
      public uint m_TargetGroup;
      public int m_SortIndex;
      public ushort m_TargetIndex;
      public ushort m_TargetCarriageway;
      public bool m_IsSource;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct SourcePositionComparer : IComparer<LaneSystem.ConnectPosition>
    {
      public int Compare(LaneSystem.ConnectPosition x, LaneSystem.ConnectPosition y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int b = x.m_Owner.Index - y.m_Owner.Index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select((int) math.sign(x.m_Order - y.m_Order), b, b != 0);
      }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TargetPositionComparer : IComparer<LaneSystem.ConnectPosition>
    {
      public int Compare(LaneSystem.ConnectPosition x, LaneSystem.ConnectPosition y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (int) math.sign(x.m_Order - y.m_Order);
      }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct MiddleConnectionComparer : IComparer<LaneSystem.MiddleConnection>
    {
      public int Compare(LaneSystem.MiddleConnection x, LaneSystem.MiddleConnection y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(x.m_SortIndex - y.m_SortIndex, (int) (x.m_ConnectPosition.m_UtilityTypes - y.m_ConnectPosition.m_UtilityTypes), x.m_ConnectPosition.m_UtilityTypes != y.m_ConnectPosition.m_UtilityTypes);
      }
    }

    private struct LaneAnchor : IComparable<LaneSystem.LaneAnchor>
    {
      public Entity m_Prefab;
      public float m_Order;
      public float3 m_Position;
      public PathNode m_PathNode;

      public int CompareTo(LaneSystem.LaneAnchor other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(math.select(0, math.select(1, -1, (double) this.m_Order < (double) other.m_Order), (double) this.m_Order != (double) other.m_Order), this.m_Prefab.Index - other.m_Prefab.Index, this.m_Prefab.Index != other.m_Prefab.Index);
      }
    }

    private struct LaneBuffer
    {
      public NativeParallelHashMap<LaneSystem.LaneKey, Entity> m_OldLanes;
      public NativeParallelHashMap<LaneSystem.LaneKey, Entity> m_OriginalLanes;
      public NativeParallelHashMap<Entity, Unity.Mathematics.Random> m_SelectedSpawnables;
      public NativeList<Entity> m_Updates;

      public LaneBuffer(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OldLanes = new NativeParallelHashMap<LaneSystem.LaneKey, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalLanes = new NativeParallelHashMap<LaneSystem.LaneKey, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedSpawnables = new NativeParallelHashMap<Entity, Unity.Mathematics.Random>(10, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_Updates = new NativeList<Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OldLanes.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalLanes.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedSpawnables.Clear();
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OldLanes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalLanes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedSpawnables.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_Updates.Dispose();
      }
    }

    private struct CompositionData
    {
      public float m_SpeedLimit;
      public float m_Priority;
      public TaxiwayFlags m_TaxiwayFlags;
      public Game.Prefabs.RoadFlags m_RoadFlags;
    }

    [BurstCompile]
    private struct UpdateLanesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> m_NodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> m_OrphanType;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.EditorContainer> m_EditorContainerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> m_ElevationType;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> m_UnderConstructionType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedNode> m_ConnectedNodeType;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<LaneSignal> m_LaneSignalData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Overridden> m_OverriddenData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Clear> m_AreaClearData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<RoadComposition> m_RoadData;
      [ReadOnly]
      public ComponentLookup<TrackComposition> m_TrackData;
      [ReadOnly]
      public ComponentLookup<WaterwayComposition> m_WaterwayData;
      [ReadOnly]
      public ComponentLookup<PathwayComposition> m_PathwayData;
      [ReadOnly]
      public ComponentLookup<TaxiwayComposition> m_TaxiwayData;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> m_PrefabLaneArchetypeData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_UtilityLaneData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<NetObjectData> m_PrefabNetObjectData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_Nodes;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<CutRange> m_CutRanges;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> m_PrefabCompositionLanes;
      [ReadOnly]
      public BufferLookup<NetCompositionCrosswalk> m_PrefabCompositionCrosswalks;
      [ReadOnly]
      public BufferLookup<DefaultNetLane> m_DefaultNetLanes;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> m_PrefabSubLanes;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PlaceholderObjects;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_ObjectRequirements;
      [ReadOnly]
      public BufferLookup<AuxiliaryNetLane> m_PrefabAuxiliaryLanes;
      [ReadOnly]
      public BufferLookup<NetCompositionPiece> m_PrefabCompositionPieces;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Entity m_DefaultTheme;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      [ReadOnly]
      public ComponentTypeSet m_DeletedTempTypes;
      [ReadOnly]
      public ComponentTypeSet m_TempOwnerTypes;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          // ISSUE: reference to a compiler-generated method
          this.DeleteLanes(chunk, unfilteredChunkIndex);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateLanes(chunk, unfilteredChunkIndex);
        }
      }

      private void DeleteLanes(ArchetypeChunk chunk, int chunkIndex)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<SubLane> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity subLane = dynamicBuffer[index2].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SecondaryLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, subLane, new Deleted());
            }
          }
        }
      }

      private void UpdateLanes(ArchetypeChunk chunk, int chunkIndex)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneBuffer laneBuffer1 = new LaneSystem.LaneBuffer(Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray2 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray3 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray4 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray5 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor1 = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        if (nativeArray2.Length != 0)
        {
          NativeList<LaneSystem.ConnectPosition> nativeList1 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> nativeList2 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> tempBuffer1 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> tempBuffer2 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.EdgeTarget> edgeTargets = new NativeList<LaneSystem.EdgeTarget>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray6 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<EdgeGeometry> nativeArray7 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Composition> nativeArray8 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Tools.EditorContainer> nativeArray9 = chunk.GetNativeArray<Game.Tools.EditorContainer>(ref this.m_EditorContainerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray10 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<ConnectedNode> bufferAccessor2 = chunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNodeType);
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity owner1 = nativeArray1[index1];
            DynamicBuffer<SubLane> lanes = bufferAccessor1[index1];
            Temp ownerTemp1 = new Temp();
            if (nativeArray5.Length != 0)
            {
              ownerTemp1 = nativeArray5[index1];
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubLanes.HasBuffer(ownerTemp1.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.FillOldLaneBuffer(this.m_SubLanes[ownerTemp1.m_Original], laneBuffer1.m_OriginalLanes);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FillOldLaneBuffer(lanes, laneBuffer1.m_OldLanes);
            PseudoRandomSeed pseudoRandomSeed;
            if (nativeArray7.Length != 0)
            {
              Edge edge = nativeArray2[index1];
              EdgeGeometry geometryData = nativeArray7[index1];
              Curve curve = nativeArray6[index1];
              Composition composition = nativeArray8[index1];
              PrefabRef prefabRef = nativeArray10[index1];
              DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor2[index1];
              // ISSUE: reference to a compiler-generated field
              NetGeometryData prefabGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
              int maxValue = (int) ushort.MaxValue;
              int connectionIndex = 0;
              int groupIndex = 0;
              pseudoRandomSeed = nativeArray3[index1];
              Unity.Mathematics.Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kSubLane);
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                ConnectedNode connectedNode = dynamicBuffer[index2];
                // ISSUE: reference to a compiler-generated method
                this.GetMiddleConnectionCurves(connectedNode.m_Node, edgeTargets);
                // ISSUE: reference to a compiler-generated method
                this.GetNodeConnectPositions(connectedNode.m_Node, connectedNode.m_CurvePosition, nativeList1, nativeList2, true, ref groupIndex, out float _, out float _);
                // ISSUE: reference to a compiler-generated method
                this.FilterNodeConnectPositions(owner1, ownerTemp1.m_Original, nativeList1, edgeTargets);
                // ISSUE: reference to a compiler-generated method
                this.FilterNodeConnectPositions(owner1, ownerTemp1.m_Original, nativeList2, edgeTargets);
                // ISSUE: reference to a compiler-generated method
                this.CreateEdgeConnectionLanes(chunkIndex, ref maxValue, ref connectionIndex, ref random, owner1, laneBuffer1, nativeList1, nativeList2, tempBuffer1, tempBuffer2, composition.m_Edge, geometryData, prefabGeometryData, curve, nativeArray5.Length != 0, ownerTemp1);
                nativeList1.Clear();
                nativeList2.Clear();
              }
              // ISSUE: reference to a compiler-generated method
              this.CreateEdgeLanes(chunkIndex, ref random, owner1, laneBuffer1, composition, edge, geometryData, prefabGeometryData, nativeArray5.Length != 0, ownerTemp1);
            }
            else if (nativeArray9.Length != 0)
            {
              Game.Tools.EditorContainer editorContainer = nativeArray9[index1];
              Curve curve = nativeArray6[index1];
              Segment segment1 = new Segment()
              {
                m_Left = curve.m_Bezier,
                m_Right = curve.m_Bezier,
                m_Length = (float2) curve.m_Length
              };
              // ISSUE: reference to a compiler-generated field
              NetLaneData netLaneData = this.m_NetLaneData[editorContainer.m_Prefab];
              NetCompositionLane netCompositionLane = new NetCompositionLane()
              {
                m_Flags = netLaneData.m_Flags,
                m_Lane = editorContainer.m_Prefab
              };
              Unity.Mathematics.Random random;
              if (nativeArray3.Length != 0)
              {
                pseudoRandomSeed = nativeArray3[index1];
                random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kSubLane);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                random = this.m_RandomSeed.GetRandom(owner1.Index);
              }
              int jobIndex = chunkIndex;
              ref Unity.Mathematics.Random local = ref random;
              Entity owner2 = owner1;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.LaneBuffer laneBuffer2 = laneBuffer1;
              Segment segment2 = segment1;
              NetCompositionData prefabCompositionData = new NetCompositionData();
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              LaneSystem.CompositionData compositionData1 = new LaneSystem.CompositionData();
              // ISSUE: variable of a compiler-generated type
              LaneSystem.CompositionData compositionData2 = compositionData1;
              DynamicBuffer<NetCompositionLane> prefabCompositionLanes = new DynamicBuffer<NetCompositionLane>();
              NetCompositionLane prefabCompositionLaneData = netCompositionLane;
              int2 segmentIndex = new int2(0, 4);
              float2 edgeDelta = new float2(0.0f, 1f);
              NativeList<LaneSystem.LaneAnchor> startAnchors = new NativeList<LaneSystem.LaneAnchor>();
              NativeList<LaneSystem.LaneAnchor> endAnchors = new NativeList<LaneSystem.LaneAnchor>();
              bool2 canAnchor = (bool2) false;
              int num = nativeArray5.Length != 0 ? 1 : 0;
              Temp ownerTemp2 = ownerTemp1;
              // ISSUE: reference to a compiler-generated method
              this.CreateEdgeLane(jobIndex, ref local, owner2, laneBuffer2, segment2, prefabCompositionData, compositionData2, prefabCompositionLanes, prefabCompositionLaneData, segmentIndex, edgeDelta, startAnchors, endAnchors, canAnchor, num != 0, ownerTemp2);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.RemoveUnusedOldLanes(chunkIndex, lanes, laneBuffer1.m_OldLanes);
            // ISSUE: reference to a compiler-generated method
            laneBuffer1.Clear();
          }
          nativeList1.Dispose();
          nativeList2.Dispose();
          tempBuffer1.Dispose();
          tempBuffer2.Dispose();
          edgeTargets.Dispose();
        }
        else if (nativeArray4.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray11 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag1 = this.m_EditorMode && !chunk.Has<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = !chunk.Has<Game.Objects.Elevation>(ref this.m_ElevationType);
          // ISSUE: reference to a compiler-generated field
          bool flag3 = chunk.Has<UnderConstruction>(ref this.m_UnderConstructionType);
          // ISSUE: reference to a compiler-generated field
          bool flag4 = chunk.Has<Destroyed>(ref this.m_DestroyedType);
          NativeList<ClearAreaData> clearAreas = new NativeList<ClearAreaData>();
          for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
          {
            Entity owner = nativeArray1[index3];
            Game.Objects.Transform transform = nativeArray4[index3];
            PrefabRef prefabRef = nativeArray11[index3];
            DynamicBuffer<SubLane> lanes = bufferAccessor1[index3];
            Temp ownerTemp = new Temp();
            if (nativeArray5.Length != 0)
            {
              ownerTemp = nativeArray5[index3];
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubLanes.HasBuffer(ownerTemp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.FillOldLaneBuffer(this.m_SubLanes[ownerTemp.m_Original], laneBuffer1.m_OriginalLanes);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FillOldLaneBuffer(lanes, laneBuffer1.m_OldLanes);
            if (!flag1)
            {
              Entity entity = owner;
              if ((ownerTemp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) != (TempFlags) 0 || ownerTemp.m_Original != Entity.Null)
                entity = ownerTemp.m_Original;
              bool flag5 = (ownerTemp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) > (TempFlags) 0;
              DynamicBuffer<InstalledUpgrade> bufferData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_InstalledUpgrades.TryGetBuffer(entity, out bufferData1) && bufferData1.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                ClearAreaHelpers.FillClearAreas(bufferData1, Entity.Null, this.m_TransformData, this.m_AreaClearData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_SubAreas, this.m_AreaNodes, this.m_AreaTriangles, ref clearAreas);
                ClearAreaHelpers.InitClearAreas(clearAreas, transform);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_EditorMode && ownerTemp.m_Original != Entity.Null)
                  flag5 = true;
              }
              bool sampleTerrain = flag2;
              if (sampleTerrain)
              {
                ObjectGeometryData componentData;
                // ISSUE: reference to a compiler-generated field
                sampleTerrain = this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData) && ((componentData.m_Flags & Game.Objects.GeometryFlags.DeleteOverridden) != Game.Objects.GeometryFlags.None || (componentData.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.Marker)) == Game.Objects.GeometryFlags.None);
              }
              Unity.Mathematics.Random random = nativeArray3[index3].GetRandom((uint) PseudoRandomSeed.kSubLane);
              if (flag5)
              {
                DynamicBuffer<SubLane> bufferData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SubLanes.TryGetBuffer(ownerTemp.m_Original, out bufferData2))
                {
                  for (int index4 = 0; index4 < bufferData2.Length; ++index4)
                  {
                    Entity subLane = bufferData2[index4].m_SubLane;
                    // ISSUE: reference to a compiler-generated method
                    this.CreateObjectLane(chunkIndex, ref random, owner, subLane, laneBuffer1, transform, new Game.Prefabs.SubLane(), index4, sampleTerrain, false, nativeArray5.Length != 0, ownerTemp, clearAreas);
                  }
                }
              }
              else
              {
                int laneIndex = 0;
                int num1 = 0;
                DynamicBuffer<Game.Prefabs.SubLane> bufferData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabSubLanes.TryGetBuffer(prefabRef.m_Prefab, out bufferData3))
                {
                  for (int index5 = 0; index5 < bufferData3.Length; ++index5)
                  {
                    Game.Prefabs.SubLane prefabSubLane = bufferData3[index5];
                    if (prefabSubLane.m_NodeIndex.y != prefabSubLane.m_NodeIndex.x)
                    {
                      laneIndex = index5;
                      num1 = math.max(num1, math.cmax(prefabSubLane.m_NodeIndex));
                      // ISSUE: reference to a compiler-generated field
                      if ((!flag3 || (this.m_NetLaneData[prefabSubLane.m_Prefab].m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Parking | Game.Prefabs.LaneFlags.Track)) != (Game.Prefabs.LaneFlags) 0) && (!flag4 || !math.any(prefabSubLane.m_ParentMesh >= 0)))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CreateObjectLane(chunkIndex, ref random, owner, Entity.Null, laneBuffer1, transform, prefabSubLane, index5, sampleTerrain, false, nativeArray5.Length != 0, ownerTemp, clearAreas);
                      }
                    }
                  }
                }
                Game.Prefabs.BuildingData componentData1;
                NetLaneData componentData2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (flag3 && this.m_PrefabBuildingData.TryGetComponent(prefabRef.m_Prefab, out componentData1) && this.m_NetLaneData.TryGetComponent(this.m_BuildingConfigurationData.m_ConstructionBorder, out componentData2))
                {
                  float3 float3_1 = new float3((float) ((double) componentData1.m_LotSize.x * 4.0 - (double) componentData2.m_Width * 0.5), 0.0f, 0.0f);
                  float3 float3_2 = new float3(0.0f, 0.0f, (float) ((double) componentData1.m_LotSize.y * 4.0 - (double) componentData2.m_Width * 0.5));
                  Game.Prefabs.SubLane prefabSubLane;
                  // ISSUE: reference to a compiler-generated field
                  prefabSubLane.m_Prefab = this.m_BuildingConfigurationData.m_ConstructionBorder;
                  prefabSubLane.m_ParentMesh = (int2) -1;
                  prefabSubLane.m_NodeIndex = new int2(num1, num1 + 1);
                  prefabSubLane.m_Curve = NetUtils.StraightCurve(-float3_1 - float3_2, float3_1 - float3_2);
                  // ISSUE: reference to a compiler-generated method
                  this.CreateObjectLane(chunkIndex, ref random, owner, Entity.Null, laneBuffer1, transform, prefabSubLane, laneIndex, sampleTerrain, (componentData1.m_Flags & Game.Prefabs.BuildingFlags.BackAccess) > (Game.Prefabs.BuildingFlags) 0, nativeArray5.Length != 0, ownerTemp, clearAreas);
                  prefabSubLane.m_NodeIndex = new int2(num1 + 1, num1 + 2);
                  prefabSubLane.m_Curve = NetUtils.StraightCurve(float3_1 - float3_2, float3_1 + float3_2);
                  // ISSUE: reference to a compiler-generated method
                  this.CreateObjectLane(chunkIndex, ref random, owner, Entity.Null, laneBuffer1, transform, prefabSubLane, laneIndex + 1, sampleTerrain, (componentData1.m_Flags & Game.Prefabs.BuildingFlags.LeftAccess) > (Game.Prefabs.BuildingFlags) 0, nativeArray5.Length != 0, ownerTemp, clearAreas);
                  prefabSubLane.m_NodeIndex = new int2(num1 + 2, num1 + 3);
                  prefabSubLane.m_Curve = NetUtils.StraightCurve(float3_1 + float3_2, -float3_1 + float3_2);
                  // ISSUE: reference to a compiler-generated method
                  this.CreateObjectLane(chunkIndex, ref random, owner, Entity.Null, laneBuffer1, transform, prefabSubLane, laneIndex + 2, sampleTerrain, true, nativeArray5.Length != 0, ownerTemp, clearAreas);
                  prefabSubLane.m_NodeIndex = new int2(num1 + 3, num1);
                  prefabSubLane.m_Curve = NetUtils.StraightCurve(-float3_1 + float3_2, -float3_1 - float3_2);
                  // ISSUE: reference to a compiler-generated method
                  this.CreateObjectLane(chunkIndex, ref random, owner, Entity.Null, laneBuffer1, transform, prefabSubLane, laneIndex + 3, sampleTerrain, (componentData1.m_Flags & Game.Prefabs.BuildingFlags.RightAccess) > (Game.Prefabs.BuildingFlags) 0, nativeArray5.Length != 0, ownerTemp, clearAreas);
                  int num2 = laneIndex + 4;
                  int num3 = num1 + 4;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.RemoveUnusedOldLanes(chunkIndex, lanes, laneBuffer1.m_OldLanes);
            // ISSUE: reference to a compiler-generated method
            laneBuffer1.Clear();
            if (clearAreas.IsCreated)
              clearAreas.Clear();
          }
          if (clearAreas.IsCreated)
            clearAreas.Dispose();
        }
        else
        {
          NativeParallelHashSet<LaneSystem.ConnectionKey> createdConnections = new NativeParallelHashSet<LaneSystem.ConnectionKey>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> nativeList3 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> nativeList4 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> nativeList5 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> nativeList6 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> nativeList7 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.ConnectPosition> nativeList8 = new NativeList<LaneSystem.ConnectPosition>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.MiddleConnection> middleConnections = new NativeList<LaneSystem.MiddleConnection>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<LaneSystem.EdgeTarget> tempEdgeTargets = new NativeList<LaneSystem.EdgeTarget>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Orphan> nativeArray12 = chunk.GetNativeArray<Orphan>(ref this.m_OrphanType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<NodeGeometry> nativeArray13 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray14 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index6 = 0; index6 < nativeArray1.Length; ++index6)
          {
            Entity entity = nativeArray1[index6];
            DynamicBuffer<SubLane> lanes = bufferAccessor1[index6];
            Temp ownerTemp = new Temp();
            if (nativeArray5.Length != 0)
            {
              ownerTemp = nativeArray5[index6];
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubLanes.HasBuffer(ownerTemp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.FillOldLaneBuffer(this.m_SubLanes[ownerTemp.m_Original], laneBuffer1.m_OriginalLanes);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FillOldLaneBuffer(lanes, laneBuffer1.m_OldLanes);
            if (nativeArray13.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              float3 position = this.m_NodeData[entity].m_Position with
              {
                y = nativeArray13[index6].m_Position
              };
              int groupIndex = 1;
              float middleRadius;
              float roundaboutSize;
              // ISSUE: reference to a compiler-generated method
              this.GetNodeConnectPositions(entity, 0.0f, nativeList3, nativeList4, false, ref groupIndex, out middleRadius, out roundaboutSize);
              bool isDeadEnd = groupIndex <= 2;
              // ISSUE: reference to a compiler-generated method
              this.GetMiddleConnections(entity, ownerTemp.m_Original, middleConnections, tempEdgeTargets, nativeList7, nativeList8, ref groupIndex);
              // ISSUE: reference to a compiler-generated method
              this.FilterMainCarConnectPositions(nativeList3, nativeList5);
              // ISSUE: reference to a compiler-generated method
              this.FilterMainCarConnectPositions(nativeList4, nativeList6);
              int num4 = 0;
              Unity.Mathematics.Random random = nativeArray3[index6].GetRandom((uint) PseudoRandomSeed.kSubLane);
              RoadTypes roadPassThrough = RoadTypes.None;
              if ((double) middleRadius > 0.0)
              {
                // ISSUE: reference to a compiler-generated method
                roadPassThrough = this.GetRoundaboutRoadPassThrough(entity);
              }
              if ((double) middleRadius > 0.0 && roadPassThrough == RoadTypes.None | isDeadEnd)
              {
                if (nativeList5.Length != 0 || nativeList6.Length != 0)
                {
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition roundaboutLane1 = new LaneSystem.ConnectPosition();
                  int laneCount1 = 0;
                  uint laneGroup = 0;
                  float maxValue1 = float.MaxValue;
                  float spaceForLanes = float.MaxValue;
                  bool isPublicOnly = true;
                  int nextLaneIndex = num4;
                  int num5 = 0;
                  int num6 = 0;
                  for (int index7 = 0; index7 < nativeList5.Length; ++index7)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((nativeList5[index7].m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != (Game.Prefabs.RoadFlags) 0)
                      ++num5;
                    else
                      ++num6;
                  }
                  for (int index8 = 0; index8 < nativeList6.Length; ++index8)
                  {
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.ConnectPosition connectPosition = nativeList6[index8];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((connectPosition.m_CompositionData.m_RoadFlags & (Game.Prefabs.RoadFlags.DefaultIsForward | Game.Prefabs.RoadFlags.DefaultIsBackward)) != (Game.Prefabs.RoadFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if ((connectPosition.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != (Game.Prefabs.RoadFlags) 0)
                        ++num5;
                      else
                        ++num6;
                    }
                  }
                  bool preferHighway = num5 > num6 || num5 >= 2;
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition1;
                  bool flag6;
                  if (nativeList5.Length != 0)
                  {
                    int num7 = 0;
                    for (int index9 = 0; index9 < nativeList5.Length; ++index9)
                    {
                      // ISSUE: variable of a compiler-generated type
                      LaneSystem.ConnectPosition main = nativeList5[index9];
                      // ISSUE: reference to a compiler-generated method
                      this.FilterActualCarConnectPositions(main, nativeList3, nativeList7);
                      // ISSUE: reference to a compiler-generated method
                      bool roundaboutLane2 = this.GetRoundaboutLane(nativeList7, roundaboutSize, ref roundaboutLane1, ref laneCount1, ref maxValue1, ref isPublicOnly, ref spaceForLanes, true, preferHighway);
                      num7 = math.select(num7, index9, roundaboutLane2);
                      nativeList7.Clear();
                    }
                    // ISSUE: reference to a compiler-generated field
                    if (roundaboutLane1.m_LaneData.m_Lane == Entity.Null)
                    {
                      int laneCount2 = 0;
                      float maxValue2 = float.MaxValue;
                      for (int index10 = 0; index10 < nativeList6.Length; ++index10)
                      {
                        // ISSUE: variable of a compiler-generated type
                        LaneSystem.ConnectPosition main = nativeList6[index10];
                        // ISSUE: reference to a compiler-generated method
                        this.FilterActualCarConnectPositions(main, nativeList4, nativeList8);
                        // ISSUE: reference to a compiler-generated method
                        this.GetRoundaboutLane(nativeList8, roundaboutSize, ref roundaboutLane1, ref laneCount2, ref maxValue1, ref isPublicOnly, ref maxValue2, false, preferHighway);
                        nativeList8.Clear();
                      }
                      spaceForLanes = maxValue2 * (float) laneCount1;
                    }
                    connectPosition1 = nativeList5[num7];
                    flag6 = true;
                  }
                  else
                  {
                    int num8 = 0;
                    for (int index11 = 0; index11 < nativeList6.Length; ++index11)
                    {
                      // ISSUE: variable of a compiler-generated type
                      LaneSystem.ConnectPosition main = nativeList6[index11];
                      // ISSUE: reference to a compiler-generated method
                      this.FilterActualCarConnectPositions(main, nativeList4, nativeList8);
                      // ISSUE: reference to a compiler-generated method
                      bool roundaboutLane3 = this.GetRoundaboutLane(nativeList8, roundaboutSize, ref roundaboutLane1, ref laneCount1, ref maxValue1, ref isPublicOnly, ref spaceForLanes, false, preferHighway);
                      num8 = math.select(num8, index11, roundaboutLane3);
                      nativeList8.Clear();
                    }
                    connectPosition1 = nativeList6[num8];
                    flag6 = false;
                  }
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition nextPosition1;
                  bool nextIsSource1;
                  // ISSUE: reference to a compiler-generated method
                  this.ExtractNextConnectPosition(connectPosition1, position, nativeList5, nativeList6, out nextPosition1, out nextIsSource1);
                  if (flag6)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.FilterActualCarConnectPositions(connectPosition1, nativeList3, nativeList7);
                  }
                  if (!nextIsSource1)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.FilterActualCarConnectPositions(nextPosition1, nativeList4, nativeList8);
                  }
                  // ISSUE: reference to a compiler-generated method
                  int roundaboutLaneCount = this.GetRoundaboutLaneCount(connectPosition1, nextPosition1, nativeList7, nativeList8, nativeList4, position);
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition2 = nextPosition1;
                  bool flag7 = nextIsSource1;
                  nativeList7.Clear();
                  nativeList8.Clear();
                  int length = nativeList5.Length;
                  while (nativeList5.Length != 0 || nativeList6.Length != 0)
                  {
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.ConnectPosition nextPosition2;
                    bool nextIsSource2;
                    // ISSUE: reference to a compiler-generated method
                    this.ExtractNextConnectPosition(connectPosition2, position, nativeList5, nativeList6, out nextPosition2, out nextIsSource2);
                    if (flag7)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.FilterActualCarConnectPositions(connectPosition2, nativeList3, nativeList7);
                    }
                    if (!nextIsSource2)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.FilterActualCarConnectPositions(nextPosition2, nativeList4, nativeList8);
                    }
                    // ISSUE: reference to a compiler-generated method
                    this.CreateRoundaboutCarLanes(chunkIndex, ref random, entity, laneBuffer1, ref num4, -1, ref laneGroup, connectPosition2, nextPosition2, middleConnections, nativeList7, nativeList8, nativeList4, roundaboutLane1, position, middleRadius, ref roundaboutLaneCount, laneCount1, length, spaceForLanes, roadPassThrough, isDeadEnd, nativeArray5.Length != 0, ownerTemp);
                    connectPosition2 = nextPosition2;
                    flag7 = nextIsSource2;
                    nativeList7.Clear();
                    nativeList8.Clear();
                  }
                  if (flag7)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.FilterActualCarConnectPositions(connectPosition2, nativeList3, nativeList7);
                  }
                  if (!nextIsSource1)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.FilterActualCarConnectPositions(nextPosition1, nativeList4, nativeList8);
                  }
                  // ISSUE: reference to a compiler-generated method
                  this.CreateRoundaboutCarLanes(chunkIndex, ref random, entity, laneBuffer1, ref num4, nextLaneIndex, ref laneGroup, connectPosition2, nextPosition1, middleConnections, nativeList7, nativeList8, nativeList4, roundaboutLane1, position, middleRadius, ref roundaboutLaneCount, laneCount1, length, spaceForLanes, roadPassThrough, isDeadEnd, nativeArray5.Length != 0, ownerTemp);
                  nativeList7.Clear();
                  nativeList8.Clear();
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.FilterActualCarConnectPositions(nativeList4, nativeList8);
                for (int index12 = 0; index12 < nativeList5.Length; ++index12)
                {
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition3 = nativeList5[index12];
                  int nodeLaneIndex = num4;
                  // ISSUE: reference to a compiler-generated method
                  int yieldOffset = this.CalculateYieldOffset(connectPosition3, nativeList5, nativeList6);
                  // ISSUE: reference to a compiler-generated method
                  this.FilterActualCarConnectPositions(connectPosition3, nativeList3, nativeList7);
                  // ISSUE: reference to a compiler-generated method
                  this.ProcessCarConnectPositions(chunkIndex, ref nodeLaneIndex, ref random, entity, laneBuffer1, middleConnections, createdConnections, nativeList7, nativeList8, nativeList3, roadPassThrough, nativeArray5.Length != 0, ownerTemp, yieldOffset);
                  nativeList7.Clear();
                  for (int index13 = 0; index13 < nativeList6.Length; ++index13)
                  {
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.ConnectPosition targetPosition = nativeList6[index13];
                    bool isUnsafe = false;
                    bool isForbidden = false;
                    for (int index14 = 0; index14 < nativeList8.Length; ++index14)
                    {
                      // ISSUE: variable of a compiler-generated type
                      LaneSystem.ConnectPosition connectPosition4 = nativeList8[index14];
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (connectPosition4.m_Owner == targetPosition.m_Owner && (int) connectPosition4.m_LaneData.m_Group == (int) targetPosition.m_LaneData.m_Group)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (connectPosition4.m_ForbiddenCount != (byte) 0)
                        {
                          isUnsafe = true;
                          isForbidden = true;
                          // ISSUE: reference to a compiler-generated field
                          connectPosition4.m_UnsafeCount = (byte) 0;
                          // ISSUE: reference to a compiler-generated field
                          connectPosition4.m_ForbiddenCount = (byte) 0;
                          nativeList8[index14] = connectPosition4;
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          if (connectPosition4.m_UnsafeCount != (byte) 0)
                          {
                            isUnsafe = true;
                            // ISSUE: reference to a compiler-generated field
                            connectPosition4.m_UnsafeCount = (byte) 0;
                            nativeList8[index14] = connectPosition4;
                          }
                        }
                      }
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (((connectPosition3.m_LaneData.m_Flags | targetPosition.m_LaneData.m_Flags) & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      uint group = (uint) connectPosition3.m_GroupIndex | (uint) targetPosition.m_GroupIndex << 16;
                      bool right;
                      bool gentle;
                      bool uturn;
                      // ISSUE: reference to a compiler-generated method
                      bool isTurn = this.IsTurn(connectPosition3, targetPosition, out right, out gentle, out uturn);
                      float curviness = -1f;
                      // ISSUE: reference to a compiler-generated method
                      if (this.CreateNodeLane(chunkIndex, ref nodeLaneIndex, ref random, ref curviness, entity, laneBuffer1, middleConnections, connectPosition3, targetPosition, group, (ushort) 0, isUnsafe, isForbidden, nativeArray5.Length != 0, false, 0, ownerTemp, isTurn, right, gentle, uturn, false, false, false, false, false, false, RoadTypes.None))
                      {
                        // ISSUE: object of a compiler-generated type is created
                        createdConnections.Add(new LaneSystem.ConnectionKey(connectPosition3, targetPosition));
                      }
                    }
                  }
                  num4 += 256;
                }
                nativeList8.Clear();
              }
              nativeList5.Clear();
              nativeList6.Clear();
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              TrackTypes trackTypes = this.FilterTrackConnectPositions(nativeList3, nativeList5) & this.FilterTrackConnectPositions(nativeList4, nativeList6);
              TrackTypes trackType = TrackTypes.Train;
              while (trackTypes != TrackTypes.None)
              {
                if ((trackTypes & trackType) != TrackTypes.None)
                {
                  trackTypes &= ~trackType;
                  // ISSUE: reference to a compiler-generated method
                  this.FilterTrackConnectPositions(trackType, nativeList6, nativeList8);
                  if (nativeList8.Length != 0)
                  {
                    int index15 = 0;
                    while (index15 < nativeList5.Length)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.FilterTrackConnectPositions(ref index15, trackType, nativeList5, nativeList7);
                      if (nativeList7.Length != 0)
                      {
                        int nodeLaneIndex = num4;
                        // ISSUE: reference to a compiler-generated method
                        this.ProcessTrackConnectPositions(chunkIndex, ref nodeLaneIndex, ref random, entity, laneBuffer1, middleConnections, createdConnections, nativeList7, nativeList8, nativeArray5.Length != 0, ownerTemp);
                        nativeList7.Clear();
                        num4 += 256;
                      }
                    }
                    nativeList8.Clear();
                  }
                }
                trackType = (TrackTypes) ((uint) trackType << 1);
              }
              nativeList5.Clear();
              nativeList6.Clear();
              // ISSUE: reference to a compiler-generated method
              this.FilterPedestrianConnectPositions(nativeList4, nativeList5, middleConnections);
              // ISSUE: reference to a compiler-generated method
              this.CreateNodePedestrianLanes(chunkIndex, ref num4, ref random, entity, laneBuffer1, nativeList5, nativeList6, nativeList8, nativeArray5.Length != 0, ownerTemp, position, middleRadius, roundaboutSize);
              nativeList5.Clear();
              nativeList6.Clear();
              nativeList8.Clear();
              // ISSUE: reference to a compiler-generated method
              UtilityTypes utilityTypes1 = this.FilterUtilityConnectPositions(nativeList4, nativeList6);
              UtilityTypes utilityTypes2 = UtilityTypes.WaterPipe;
              while (utilityTypes1 != UtilityTypes.None)
              {
                if ((utilityTypes1 & utilityTypes2) != UtilityTypes.None)
                {
                  utilityTypes1 &= ~utilityTypes2;
                  // ISSUE: reference to a compiler-generated method
                  this.FilterUtilityConnectPositions(utilityTypes2, nativeList6, nativeList8);
                  // ISSUE: reference to a compiler-generated method
                  this.FilterUtilityConnectPositions(utilityTypes2, nativeList5, nativeList7);
                  if (nativeList8.Length != 0 || nativeList7.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CreateNodeUtilityLanes(chunkIndex, ref num4, ref random, entity, laneBuffer1, nativeList8, nativeList7, middleConnections, position, (double) middleRadius > 0.0, nativeArray5.Length != 0, ownerTemp);
                    nativeList8.Clear();
                    nativeList7.Clear();
                  }
                }
                utilityTypes2 = (UtilityTypes) ((uint) utilityTypes2 << 1);
              }
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeConnectionLanes(chunkIndex, ref num4, ref random, entity, laneBuffer1, middleConnections, nativeList8, (double) middleRadius > 0.0, nativeArray5.Length != 0, ownerTemp);
              createdConnections.Clear();
              nativeList3.Clear();
              nativeList4.Clear();
              nativeList5.Clear();
              nativeList6.Clear();
              middleConnections.Clear();
              if (nativeArray12.Length != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateOrphanLanes(chunkIndex, ref random, entity, laneBuffer1, nativeArray12[index6], position, nativeArray14[index6].m_Prefab, ref num4, nativeArray5.Length != 0, ownerTemp);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.RemoveUnusedOldLanes(chunkIndex, lanes, laneBuffer1.m_OldLanes);
            // ISSUE: reference to a compiler-generated method
            laneBuffer1.Clear();
          }
          createdConnections.Dispose();
          nativeList3.Dispose();
          nativeList4.Dispose();
          nativeList5.Dispose();
          nativeList6.Dispose();
          nativeList7.Dispose();
          nativeList8.Dispose();
          middleConnections.Dispose();
          tempEdgeTargets.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateLanes(chunkIndex, laneBuffer1.m_Updates);
        // ISSUE: reference to a compiler-generated method
        laneBuffer1.Dispose();
      }

      private void CreateOrphanLanes(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        Orphan orphan,
        float3 middlePosition,
        Entity prefab,
        ref int nodeLaneIndex,
        bool isTemp,
        Temp ownerTemp)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DefaultNetLanes.HasBuffer(prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<DefaultNetLane> defaultNetLane1 = this.m_DefaultNetLanes[prefab];
        int index1 = -1;
        int index2 = -1;
        for (int index3 = 0; index3 < defaultNetLane1.Length; ++index3)
        {
          if ((defaultNetLane1[index3].m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            if (index1 == -1)
              index1 = index3;
            else
              index2 = index3;
          }
        }
        if (index2 <= index1)
          return;
        DefaultNetLane defaultNetLane2 = defaultNetLane1[index1];
        DefaultNetLane defaultNetLane3 = defaultNetLane1[index2];
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition connectPosition1 = new LaneSystem.ConnectPosition();
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_LaneData.m_Lane = defaultNetLane2.m_Lane;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_LaneData.m_Flags = defaultNetLane2.m_Flags;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_NodeComposition = orphan.m_Composition;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_BaseHeight = middlePosition.y;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Position = middlePosition;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Position.xy += defaultNetLane2.m_Position.xy;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Tangent = new float3(0.0f, 0.0f, 1f);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition connectPosition2 = new LaneSystem.ConnectPosition();
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_LaneData.m_Lane = defaultNetLane3.m_Lane;
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_LaneData.m_Flags = defaultNetLane3.m_Flags;
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_NodeComposition = orphan.m_Composition;
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_BaseHeight = middlePosition.y;
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_Position = middlePosition;
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_Position.xy += defaultNetLane3.m_Position.xy;
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_Tangent = new float3(0.0f, 0.0f, 1f);
        PathNode pathNode = new PathNode(owner, (ushort) nodeLaneIndex++);
        Bezier4x3 curve;
        PathNode middleNode;
        PathNode endNode;
        // ISSUE: reference to a compiler-generated method
        this.CreateNodePedestrianLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition2, connectPosition1, pathNode, pathNode, false, true, isTemp, ownerTemp, false, false, out curve, out middleNode, out endNode);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Tangent = -connectPosition1.m_Tangent;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        connectPosition2.m_Tangent = -connectPosition2.m_Tangent;
        // ISSUE: reference to a compiler-generated method
        this.CreateNodePedestrianLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, connectPosition2, endNode, pathNode, false, true, isTemp, ownerTemp, false, false, out curve, out middleNode, out PathNode _);
      }

      private int GetRoundaboutLaneCount(
        LaneSystem.ConnectPosition prevMainPosition,
        LaneSystem.ConnectPosition nextMainPosition,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        NativeList<LaneSystem.ConnectPosition> allTargets,
        float3 middlePosition)
      {
        // ISSUE: reference to a compiler-generated field
        float2 fromVector = math.normalizesafe(prevMainPosition.m_Position.xz - middlePosition.xz);
        // ISSUE: reference to a compiler-generated field
        float2 toVector = math.normalizesafe(nextMainPosition.m_Position.xz - middlePosition.xz);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = math.max(1, Mathf.CeilToInt((float) ((!prevMainPosition.m_Position.Equals(nextMainPosition.m_Position) ? (this.m_LeftHandTraffic ? (double) MathUtils.RotationAngleRight(fromVector, toVector) : (double) MathUtils.RotationAngleLeft(fromVector, toVector)) : 6.2831854820251465) * 0.63661974668502808 - Math.PI / 1000.0)));
        int roundaboutLaneCount = math.max(1, sourceBuffer.Length);
        if (num1 == 1)
        {
          if (sourceBuffer.Length <= 0 || targetBuffer.Length <= 0)
            return roundaboutLaneCount;
          // ISSUE: reference to a compiler-generated method
          int num2 = math.min(1, math.clamp(sourceBuffer.Length + targetBuffer.Length - this.GetRoundaboutTargetLaneCount(sourceBuffer[sourceBuffer.Length - 1], allTargets), 0, math.min(targetBuffer.Length, sourceBuffer.Length - 1)));
          return roundaboutLaneCount - num2;
        }
        int num3 = targetBuffer.Length - math.select(1, 0, targetBuffer.Length <= 1);
        return math.max(1, roundaboutLaneCount - num3);
      }

      private int GetRoundaboutTargetLaneCount(
        LaneSystem.ConnectPosition sourcePosition,
        NativeList<LaneSystem.ConnectPosition> allTargets)
      {
        int roundaboutTargetLaneCount = 0;
        for (int index = 0; index < allTargets.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition allTarget = allTargets[index];
          // ISSUE: reference to a compiler-generated field
          if ((allTarget.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Road)) == Game.Prefabs.LaneFlags.Road)
          {
            bool uturn;
            // ISSUE: reference to a compiler-generated method
            this.IsTurn(sourcePosition, allTarget, out bool _, out bool _, out uturn);
            roundaboutTargetLaneCount += math.select(1, 0, uturn);
          }
        }
        return roundaboutTargetLaneCount;
      }

      private void CreateRoundaboutCarLanes(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        ref int prevLaneIndex,
        int nextLaneIndex,
        ref uint laneGroup,
        LaneSystem.ConnectPosition prevMainPosition,
        LaneSystem.ConnectPosition nextMainPosition,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        NativeList<LaneSystem.ConnectPosition> allTargets,
        LaneSystem.ConnectPosition lane,
        float3 middlePosition,
        float middleRadius,
        ref int laneCount,
        int totalLaneCount,
        int totalSourceCount,
        float spaceForLanes,
        RoadTypes roadPassThrough,
        bool isDeadEnd,
        bool isTemp,
        Temp ownerTemp)
      {
        // ISSUE: reference to a compiler-generated field
        float2 fromVector = math.normalizesafe(prevMainPosition.m_Position.xz - middlePosition.xz);
        // ISSUE: reference to a compiler-generated field
        float2 toVector = math.normalizesafe(nextMainPosition.m_Position.xz - middlePosition.xz);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num1 = !prevMainPosition.m_Position.Equals(nextMainPosition.m_Position) ? (this.m_LeftHandTraffic ? MathUtils.RotationAngleRight(fromVector, toVector) : MathUtils.RotationAngleLeft(fromVector, toVector)) : 6.28318548f;
        int num2 = math.max(1, Mathf.CeilToInt((float) ((double) num1 * 0.63661974668502808 - Math.PI / 1000.0)));
        float2 float2 = fromVector;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition sourcePosition1 = new LaneSystem.ConnectPosition();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_LaneData.m_Lane = lane.m_LaneData.m_Lane;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_NodeComposition = lane.m_NodeComposition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_EdgeComposition = lane.m_EdgeComposition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_CompositionData = lane.m_CompositionData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_BaseHeight = middlePosition.y + prevMainPosition.m_BaseHeight - prevMainPosition.m_Position.y;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_Tangent.xz = this.m_LeftHandTraffic ? MathUtils.Right(float2) : MathUtils.Left(float2);
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_SegmentIndex = (byte) (prevLaneIndex >> 8);
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        sourcePosition1.m_Position.y = middlePosition.y;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition targetPosition1 = new LaneSystem.ConnectPosition();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetPosition1.m_LaneData.m_Lane = lane.m_LaneData.m_Lane;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetPosition1.m_NodeComposition = lane.m_NodeComposition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetPosition1.m_EdgeComposition = lane.m_EdgeComposition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetPosition1.m_CompositionData = lane.m_CompositionData;
        // ISSUE: reference to a compiler-generated field
        targetPosition1.m_Owner = owner;
        if (sourceBuffer.Length >= 2)
        {
          NativeList<LaneSystem.ConnectPosition> list = sourceBuffer;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LaneSystem.TargetPositionComparer positionComparer = new LaneSystem.TargetPositionComparer();
          // ISSUE: variable of a compiler-generated type
          LaneSystem.TargetPositionComparer comp = positionComparer;
          list.Sort<LaneSystem.ConnectPosition, LaneSystem.TargetPositionComparer>(comp);
        }
        if (targetBuffer.Length >= 2)
        {
          NativeList<LaneSystem.ConnectPosition> list = targetBuffer;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LaneSystem.TargetPositionComparer positionComparer = new LaneSystem.TargetPositionComparer();
          // ISSUE: variable of a compiler-generated type
          LaneSystem.TargetPositionComparer comp = positionComparer;
          list.Sort<LaneSystem.ConnectPosition, LaneSystem.TargetPositionComparer>(comp);
        }
        int num3 = 0;
        int num4 = targetBuffer.Length - math.select(1, 0, targetBuffer.Length <= 1);
        if (num2 == 1 && sourceBuffer.Length > 0 && targetBuffer.Length > 0)
        {
          // ISSUE: reference to a compiler-generated method
          int y = math.clamp(sourceBuffer.Length + targetBuffer.Length - this.GetRoundaboutTargetLaneCount(sourceBuffer[sourceBuffer.Length - 1], allTargets), 0, math.min(targetBuffer.Length, sourceBuffer.Length - 1));
          if (y == 0 & sourceBuffer.Length < totalLaneCount)
            y = math.select(0, 1, math.max(1, laneCount - num4) + sourceBuffer.Length >= totalLaneCount);
          num3 = math.min(1, y);
        }
        for (int index1 = 1; index1 <= num2; ++index1)
        {
          int b1 = math.max(1, math.select(laneCount - num4, laneCount, index1 != num2));
          int num5 = math.select(math.min(totalLaneCount, b1 + sourceBuffer.Length) - num3, b1, index1 != 1);
          int nodeLaneIndex = prevLaneIndex + totalLaneCount + 2;
          prevLaneIndex += 256;
          float2 forward1;
          if (index1 == num2)
          {
            forward1 = toVector;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_NodeComposition = lane.m_NodeComposition;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_EdgeComposition = lane.m_EdgeComposition;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_CompositionData = lane.m_CompositionData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_BaseHeight = middlePosition.y + nextMainPosition.m_BaseHeight - nextMainPosition.m_Position.y;
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_SegmentIndex = (byte) (math.select(prevLaneIndex, nextLaneIndex, nextLaneIndex >= 0) >> 8);
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_Position.y = middlePosition.y;
          }
          else
          {
            float s = (float) index1 / (float) num2;
            // ISSUE: reference to a compiler-generated field
            forward1 = this.m_LeftHandTraffic ? MathUtils.RotateRight(float2, num1 * s) : MathUtils.RotateLeft(float2, num1 * s);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_CompositionData.m_SpeedLimit = math.lerp(prevMainPosition.m_CompositionData.m_SpeedLimit, nextMainPosition.m_CompositionData.m_SpeedLimit, s);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_BaseHeight = middlePosition.y + math.lerp(prevMainPosition.m_BaseHeight, nextMainPosition.m_BaseHeight, s) - math.lerp(prevMainPosition.m_Position.y, nextMainPosition.m_Position.y, s);
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_SegmentIndex = (byte) (prevLaneIndex >> 8);
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_Position.y = middlePosition.y;
          }
          // ISSUE: reference to a compiler-generated field
          float2 forward2 = this.m_LeftHandTraffic ? MathUtils.RotateRight(float2, num1 * ((float) index1 - 0.5f) / (float) num2) : MathUtils.RotateLeft(float2, num1 * ((float) index1 - 0.5f) / (float) num2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          targetPosition1.m_Tangent.xz = this.m_LeftHandTraffic ? MathUtils.Left(forward1) : MathUtils.Right(forward1);
          float3 centerTangent1 = new float3();
          // ISSUE: reference to a compiler-generated field
          centerTangent1.xz = this.m_LeftHandTraffic ? MathUtils.Right(forward2) : MathUtils.Left(forward2);
          float3 centerPosition1 = new float3();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          centerPosition1.y = math.lerp(sourcePosition1.m_Position.y, targetPosition1.m_Position.y, 0.5f);
          bool c1 = laneCount >= 2;
          bool c2 = num5 >= 2;
          bool flag1 = index1 == 1 && sourceBuffer.Length >= 1;
          bool flag2 = index1 == num2 && targetBuffer.Length >= 1;
          bool flag3 = num2 == 1 && sourceBuffer.Length >= 1 && targetBuffer.Length >= 1;
          float curviness = -1f;
          bool isUnsafe1 = roadPassThrough != RoadTypes.None || isDeadEnd && nextLaneIndex < 0 | flag1 | flag2;
          for (int a1 = 0; a1 < num5; ++a1)
          {
            // ISSUE: reference to a compiler-generated field
            int laneIndex = math.select(a1, num5 - a1 - 1, this.m_LeftHandTraffic);
            int num6 = math.max(0, a1 - num5 + b1);
            float num7 = middleRadius + ((float) num6 + 0.5f) * spaceForLanes / (float) totalLaneCount;
            float num8 = middleRadius + ((float) a1 + 0.5f) * spaceForLanes / (float) totalLaneCount;
            // ISSUE: reference to a compiler-generated field
            sourcePosition1.m_Position.xz = middlePosition.xz + fromVector * num7;
            // ISSUE: reference to a compiler-generated field
            sourcePosition1.m_LaneData.m_Index = (byte) num6;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            sourcePosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
            if (c1)
            {
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Slave;
            }
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_Position.xz = middlePosition.xz + forward1 * num8;
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_LaneData.m_Index = (byte) a1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
            if (c2)
            {
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Slave;
            }
            bool a2 = a1 == 0;
            bool b2 = a1 == num5 - 1 && index1 > 1 && index1 < num2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LeftHandTraffic)
              CommonUtils.Swap<bool>(ref a2, ref b2);
            if (num6 != a1)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition sourcePosition2 = sourcePosition1;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition2 = targetPosition1;
              // ISSUE: reference to a compiler-generated field
              float3 position = targetPosition2.m_Position with
              {
                xz = middlePosition.xz + forward1 * num7
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bezier4x3 bezier4x3 = NetUtils.FitCurve(sourcePosition2.m_Position, sourcePosition2.m_Tangent, -targetPosition2.m_Tangent, position);
              // ISSUE: reference to a compiler-generated field
              sourcePosition2.m_Tangent = bezier4x3.b - bezier4x3.a;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              targetPosition2.m_Tangent = MathUtils.Normalize(targetPosition2.m_Tangent, targetPosition2.m_Tangent.xz);
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition2, targetPosition2, laneGroup, (ushort) laneIndex, isUnsafe1, false, isTemp, false, 0, ownerTemp, false, false, false, false, true, a2, b2, false, false, true, roadPassThrough);
            }
            else
            {
              curviness = -1f;
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition1, targetPosition1, laneGroup, (ushort) laneIndex, isUnsafe1, false, isTemp, false, 0, ownerTemp, false, false, false, false, true, a2, b2, false, false, false, roadPassThrough);
            }
          }
          if (!isTemp && c1 | c2)
          {
            float num9 = middleRadius + (float) laneCount * 0.5f * spaceForLanes / (float) totalLaneCount;
            float num10 = middleRadius + (float) num5 * 0.5f * spaceForLanes / (float) totalLaneCount;
            // ISSUE: reference to a compiler-generated field
            sourcePosition1.m_Position.xz = middlePosition.xz + fromVector * num9;
            // ISSUE: reference to a compiler-generated field
            sourcePosition1.m_LaneData.m_Index = (byte) math.select(0, laneCount, c1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            sourcePosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
            // ISSUE: reference to a compiler-generated field
            sourcePosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Master;
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_Position.xz = middlePosition.xz + forward1 * num10;
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_LaneData.m_Index = (byte) math.select(0, num5, c2);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
            // ISSUE: reference to a compiler-generated field
            targetPosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Master;
            curviness = -1f;
            // ISSUE: reference to a compiler-generated method
            this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition1, targetPosition1, laneGroup, (ushort) 0, isUnsafe1, false, isTemp, false, 0, ownerTemp, false, false, false, false, true, false, false, false, false, false, roadPassThrough);
          }
          ++laneGroup;
          float num11 = 0.0f;
          Curve curve1;
          if (flag1)
          {
            bool flag4 = c2;
            int yield = math.select(0, 1, totalSourceCount >= 2);
            for (int a3 = 0; a3 < num5; ++a3)
            {
              // ISSUE: reference to a compiler-generated field
              int laneIndex = math.select(a3, num5 - a3 - 1, this.m_LeftHandTraffic);
              int a4 = math.max(0, a3 + math.min(0, sourceBuffer.Length - num5));
              // ISSUE: reference to a compiler-generated field
              int index2 = math.select(a4, sourceBuffer.Length - a4 - 1, this.m_LeftHandTraffic);
              float middleOffset = middleRadius + ((float) (a3 + totalLaneCount - math.max(sourceBuffer.Length, num5)) + 0.5f) * spaceForLanes / (float) totalLaneCount;
              centerPosition1.xz = middlePosition.xz + forward2 * middleOffset;
              float num12 = middleRadius + ((float) a3 + 0.5f) * spaceForLanes / (float) totalLaneCount;
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_Position.xz = middlePosition.xz + forward1 * num12;
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_LaneData.m_Index = (byte) a3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
              if (c2)
              {
                // ISSUE: reference to a compiler-generated field
                targetPosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Slave;
              }
              bool a5 = false;
              bool b3 = a3 == num5 - 1 && index1 < num2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                CommonUtils.Swap<bool>(ref a5, ref b3);
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition sourcePosition3 = sourceBuffer[index2];
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition3 = targetPosition1;
              // ISSUE: reference to a compiler-generated field
              flag4 |= (sourcePosition3.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != 0;
              // ISSUE: reference to a compiler-generated method
              this.PresetCurve(ref sourcePosition3, ref targetPosition3, middlePosition, centerPosition1, centerTangent1, middleOffset, 0.0f, num1 / (float) num2, 2f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bezier4x3 curve2 = new Bezier4x3(sourcePosition3.m_Position, sourcePosition3.m_Position + sourcePosition3.m_Tangent, targetPosition3.m_Position + targetPosition3.m_Tangent, targetPosition3.m_Position);
              num11 = math.max(num11, math.distance(MathUtils.Position(curve2, 0.5f).xz, middlePosition.xz));
              curve1 = new Curve();
              curve1.m_Bezier = curve2;
              curve1.m_Length = 1f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              curviness = NetUtils.CalculateStartCurviness(curve1, this.m_NetLaneData[sourcePosition3.m_LaneData.m_Lane].m_Width);
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition3, targetPosition3, laneGroup, (ushort) laneIndex, false, false, isTemp, false, yield, ownerTemp, false, false, false, false, true, a5, b3, false, false, true, roadPassThrough);
            }
            if (flag4)
            {
              float middleOffset = math.lerp(middleRadius + ((float) (totalLaneCount - math.max(sourceBuffer.Length, num5)) + 0.5f) * spaceForLanes / (float) totalLaneCount, middleRadius + ((float) (num5 - 1 + totalLaneCount - math.max(sourceBuffer.Length, num5)) + 0.5f) * spaceForLanes / (float) totalLaneCount, 0.5f);
              centerPosition1.xz = middlePosition.xz + forward2 * middleOffset;
              float num13 = middleRadius + (float) num5 * 0.5f * spaceForLanes / (float) totalLaneCount;
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_Position.xz = middlePosition.xz + forward1 * num13;
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_LaneData.m_Index = (byte) math.select(0, num5, c2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
              // ISSUE: reference to a compiler-generated field
              targetPosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Master;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition sourcePosition4 = prevMainPosition;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition4 = targetPosition1;
              // ISSUE: reference to a compiler-generated method
              this.PresetCurve(ref sourcePosition4, ref targetPosition4, middlePosition, centerPosition1, centerTangent1, middleOffset, 0.0f, num1 / (float) num2, 2f);
              curviness = -1f;
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition4, targetPosition4, laneGroup, (ushort) 0, false, false, isTemp, false, yield, ownerTemp, false, false, false, false, true, false, false, false, false, true, roadPassThrough);
            }
            ++laneGroup;
          }
          if (flag2)
          {
            bool flag5 = c1;
            int yield = math.select(0, -1, totalSourceCount >= 2);
            for (int a6 = 0; a6 < targetBuffer.Length; ++a6)
            {
              // ISSUE: reference to a compiler-generated field
              int laneIndex = math.select(a6, targetBuffer.Length - a6 - 1, this.m_LeftHandTraffic);
              int num14 = math.min(laneCount - 1, a6 + math.max(0, laneCount - targetBuffer.Length));
              float middleOffset = middleRadius + ((float) math.min(totalLaneCount - 1, a6 + math.max(0, totalLaneCount - targetBuffer.Length)) + 0.5f) * spaceForLanes / (float) totalLaneCount;
              centerPosition1.xz = middlePosition.xz + forward2 * middleOffset;
              float num15 = middleRadius + ((float) num14 + 0.5f) * spaceForLanes / (float) totalLaneCount;
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_Position.xz = middlePosition.xz + fromVector * num15;
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_LaneData.m_Index = (byte) num14;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
              if (c1)
              {
                // ISSUE: reference to a compiler-generated field
                sourcePosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Slave;
              }
              bool a7 = false;
              bool b4 = a6 == targetBuffer.Length - 1 && index1 > 1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                CommonUtils.Swap<bool>(ref a7, ref b4);
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition sourcePosition5 = sourcePosition1;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition5 = targetBuffer[targetBuffer.Length - 1 - laneIndex];
              // ISSUE: reference to a compiler-generated field
              flag5 |= (targetPosition5.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != 0;
              // ISSUE: reference to a compiler-generated method
              this.PresetCurve(ref sourcePosition5, ref targetPosition5, middlePosition, centerPosition1, centerTangent1, middleOffset, num1 / (float) num2, 0.0f, 2f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bezier4x3 curve3 = new Bezier4x3(sourcePosition5.m_Position, sourcePosition5.m_Position + sourcePosition5.m_Tangent, targetPosition5.m_Position + targetPosition5.m_Tangent, targetPosition5.m_Position);
              num11 = math.max(num11, math.distance(MathUtils.Position(curve3, 0.5f).xz, middlePosition.xz));
              bool isUnsafe2 = a6 >= laneCount;
              curve1 = new Curve();
              curve1.m_Bezier = curve3;
              curve1.m_Length = 1f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              curviness = NetUtils.CalculateEndCurviness(curve1, this.m_NetLaneData[targetPosition5.m_LaneData.m_Lane].m_Width);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition5, targetPosition5, laneGroup, (ushort) laneIndex, isUnsafe2, false, isTemp, false, yield, ownerTemp, true, !this.m_LeftHandTraffic, true, false, true, a7, b4, isUnsafe2 && this.m_LeftHandTraffic, isUnsafe2 && !this.m_LeftHandTraffic, true, roadPassThrough);
            }
            if (flag5)
            {
              float middleOffset = math.lerp(middleRadius + ((float) math.min(totalLaneCount - 1, math.max(0, totalLaneCount - targetBuffer.Length)) + 0.5f) * spaceForLanes / (float) totalLaneCount, middleRadius + ((float) math.min(totalLaneCount - 1, targetBuffer.Length - 1 + math.max(0, totalLaneCount - targetBuffer.Length)) + 0.5f) * spaceForLanes / (float) totalLaneCount, 0.5f);
              centerPosition1.xz = middlePosition.xz + forward2 * middleOffset;
              float num16 = middleRadius + (float) laneCount * 0.5f * spaceForLanes / (float) totalLaneCount;
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_Position.xz = middlePosition.xz + fromVector * num16;
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_LaneData.m_Index = (byte) math.select(0, laneCount, c1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
              // ISSUE: reference to a compiler-generated field
              sourcePosition1.m_LaneData.m_Flags |= Game.Prefabs.LaneFlags.Master;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition sourcePosition6 = sourcePosition1;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition6 = nextMainPosition;
              // ISSUE: reference to a compiler-generated method
              this.PresetCurve(ref sourcePosition6, ref targetPosition6, middlePosition, centerPosition1, centerTangent1, middleOffset, num1 / (float) num2, 0.0f, 2f);
              curviness = -1f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition6, targetPosition6, laneGroup, (ushort) 0, false, false, isTemp, false, yield, ownerTemp, true, !this.m_LeftHandTraffic, true, false, true, false, false, false, false, true, roadPassThrough);
            }
            ++laneGroup;
          }
          if (flag3)
          {
            bool flag6 = false;
            bool flag7 = false;
            int yield = math.select(0, 1, totalSourceCount >= 2);
            float x1 = middleRadius + ((float) totalLaneCount - 0.5f) * spaceForLanes / (float) totalLaneCount;
            float x2 = math.lerp(x1, math.max(x1, num11), 0.5f);
            // ISSUE: reference to a compiler-generated field
            float2 forward3 = this.m_LeftHandTraffic ? MathUtils.RotateRight(float2, num1 * ((float) index1 - 0.75f) / (float) num2) : MathUtils.RotateLeft(float2, num1 * ((float) index1 - 0.75f) / (float) num2);
            // ISSUE: reference to a compiler-generated field
            float2 forward4 = this.m_LeftHandTraffic ? MathUtils.RotateRight(float2, num1 * ((float) index1 - 0.25f) / (float) num2) : MathUtils.RotateLeft(float2, num1 * ((float) index1 - 0.25f) / (float) num2);
            float3 centerTangent2 = new float3();
            float3 centerTangent3 = new float3();
            // ISSUE: reference to a compiler-generated field
            centerTangent2.xz = this.m_LeftHandTraffic ? MathUtils.Right(forward3) : MathUtils.Left(forward3);
            // ISSUE: reference to a compiler-generated field
            centerTangent3.xz = this.m_LeftHandTraffic ? MathUtils.Right(forward4) : MathUtils.Left(forward4);
            int a8 = sourceBuffer.Length - 1;
            // ISSUE: reference to a compiler-generated field
            int index3 = math.select(a8, sourceBuffer.Length - a8 - 1, this.m_LeftHandTraffic);
            int a9 = 0;
            // ISSUE: reference to a compiler-generated field
            int index4 = math.select(a9, targetBuffer.Length - a9 - 1, this.m_LeftHandTraffic);
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition sourcePosition7 = sourceBuffer[index3];
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition1 = targetBuffer[index4];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float y = MathUtils.Distance(NetUtils.FitCurve(sourcePosition7.m_Position, sourcePosition7.m_Tangent, -connectPosition1.m_Tangent, connectPosition1.m_Position).xz, middlePosition.xz, out float _);
            float middleOffset = math.max(x2, y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition2 = new LaneSystem.ConnectPosition()
            {
              m_LaneData = {
                m_Lane = lane.m_LaneData.m_Lane
              },
              m_NodeComposition = lane.m_NodeComposition,
              m_EdgeComposition = lane.m_EdgeComposition,
              m_CompositionData = lane.m_CompositionData,
              m_Owner = owner
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_CompositionData.m_SpeedLimit = math.lerp(sourcePosition1.m_CompositionData.m_SpeedLimit, targetPosition1.m_CompositionData.m_SpeedLimit, 0.5f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_BaseHeight = math.lerp(sourcePosition1.m_BaseHeight, targetPosition1.m_BaseHeight, 0.5f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_SegmentIndex = targetPosition1.m_SegmentIndex;
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_Tangent = centerTangent1;
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_Position.y = centerPosition1.y;
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_LaneData.m_Index = (byte) (num5 + 1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_LaneData.m_Flags = lane.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.PublicOnly | Game.Prefabs.LaneFlags.HasAuxiliary);
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_Position.xz = middlePosition.xz + forward2 * middleOffset;
            // ISSUE: reference to a compiler-generated field
            float3 centerPosition2 = middlePosition with
            {
              y = math.lerp(sourcePosition1.m_Position.y, centerPosition1.y, 0.5f)
            };
            centerPosition2.xz += forward3 * middleOffset;
            // ISSUE: reference to a compiler-generated field
            float3 centerPosition3 = middlePosition with
            {
              y = math.lerp(centerPosition1.y, targetPosition1.m_Position.y, 0.5f)
            };
            centerPosition3.xz += forward4 * middleOffset;
            for (int a10 = 0; a10 < targetBuffer.Length; ++a10)
            {
              // ISSUE: reference to a compiler-generated field
              int laneIndex = math.select(a10, targetBuffer.Length - a10 - 1, this.m_LeftHandTraffic);
              int a11 = targetBuffer.Length - 1 - a10;
              // ISSUE: reference to a compiler-generated field
              int index5 = math.select(a11, targetBuffer.Length - a11 - 1, this.m_LeftHandTraffic);
              if (a10 == 0)
              {
                bool a12 = false;
                bool b5 = true;
                // ISSUE: reference to a compiler-generated field
                if (this.m_LeftHandTraffic)
                  CommonUtils.Swap<bool>(ref a12, ref b5);
                sourcePosition7 = sourceBuffer[index3];
                // ISSUE: variable of a compiler-generated type
                LaneSystem.ConnectPosition targetPosition7 = connectPosition2;
                // ISSUE: reference to a compiler-generated field
                flag6 |= (sourcePosition7.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != 0;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                targetPosition7.m_Tangent = -targetPosition7.m_Tangent;
                // ISSUE: reference to a compiler-generated method
                this.PresetCurve(ref sourcePosition7, ref targetPosition7, middlePosition, centerPosition2, centerTangent2, middleOffset, 0.0f, num1 * 0.5f / (float) num2, 2f);
                curviness = -1f;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition7, targetPosition7, laneGroup, (ushort) laneIndex, false, false, isTemp, false, yield, ownerTemp, true, !this.m_LeftHandTraffic, false, false, true, a12, b5, false, false, true, roadPassThrough);
              }
              bool a13 = false;
              bool b6 = a10 == targetBuffer.Length - 1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                CommonUtils.Swap<bool>(ref a13, ref b6);
              sourcePosition7 = connectPosition2;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition8 = targetBuffer[index5];
              // ISSUE: reference to a compiler-generated field
              flag7 |= (targetPosition8.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != 0;
              // ISSUE: reference to a compiler-generated method
              this.PresetCurve(ref sourcePosition7, ref targetPosition8, middlePosition, centerPosition3, centerTangent3, middleOffset, num1 * 0.5f / (float) num2, 0.0f, 2f);
              bool isUnsafe3 = a11 != 0;
              curviness = -1f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition7, targetPosition8, laneGroup + 1U, (ushort) laneIndex, isUnsafe3, false, isTemp, false, 0, ownerTemp, true, !this.m_LeftHandTraffic, false, false, true, a13, b6, isUnsafe3 && !this.m_LeftHandTraffic, isUnsafe3 && this.m_LeftHandTraffic, true, roadPassThrough);
            }
            if (flag6)
            {
              sourcePosition7 = prevMainPosition;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition9 = connectPosition2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              targetPosition9.m_Tangent = -targetPosition9.m_Tangent;
              // ISSUE: reference to a compiler-generated method
              this.PresetCurve(ref sourcePosition7, ref targetPosition9, middlePosition, centerPosition2, centerTangent2, middleOffset, 0.0f, num1 * 0.5f / (float) num2, 2f);
              curviness = -1f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition7, targetPosition9, laneGroup, (ushort) 0, false, false, isTemp, false, yield, ownerTemp, true, !this.m_LeftHandTraffic, false, false, true, false, false, false, false, true, roadPassThrough);
            }
            if (flag7)
            {
              sourcePosition7 = connectPosition2;
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition10 = nextMainPosition;
              // ISSUE: reference to a compiler-generated method
              this.PresetCurve(ref sourcePosition7, ref targetPosition10, middlePosition, centerPosition3, centerTangent3, middleOffset, num1 * 0.5f / (float) num2, 0.0f, 2f);
              curviness = -1f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition7, targetPosition10, laneGroup + 1U, (ushort) 0, false, false, isTemp, false, 0, ownerTemp, true, !this.m_LeftHandTraffic, false, false, true, false, false, false, false, true, roadPassThrough);
            }
            laneGroup += 2U;
          }
          fromVector = forward1;
          sourcePosition1 = targetPosition1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          sourcePosition1.m_Tangent = -sourcePosition1.m_Tangent;
          laneCount = num5;
        }
      }

      private void PresetCurve(
        ref LaneSystem.ConnectPosition sourcePosition,
        ref LaneSystem.ConnectPosition targetPosition,
        float3 middlePosition,
        float3 centerPosition,
        float3 centerTangent,
        float middleOffset,
        float startAngle,
        float endAngle,
        float smoothness)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Bezier4x3 bezier4x3_1 = NetUtils.FitCurve(sourcePosition.m_Position, sourcePosition.m_Tangent, centerTangent, centerPosition);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Bezier4x3 bezier4x3_2 = NetUtils.FitCurve(centerPosition, centerTangent, -targetPosition.m_Tangent, targetPosition.m_Position);
        float2 float2 = smoothness * new float2(0.425f, 0.075f);
        if ((double) startAngle > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(targetPosition.m_Position.xz, middlePosition.xz) - middleOffset;
          float num2 = math.max(math.distance(bezier4x3_1.a.xz, bezier4x3_1.b.xz) * 2f, (float) ((double) middleOffset * (double) math.tan(startAngle / 2f) - (double) num1 * (double) float2.y));
          float num3 = math.max(math.distance(bezier4x3_2.d.xz, bezier4x3_2.c.xz) * smoothness, num1 * float2.x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          sourcePosition.m_Tangent = MathUtils.Normalize(sourcePosition.m_Tangent, sourcePosition.m_Tangent.xz) * num2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          targetPosition.m_Tangent = MathUtils.Normalize(targetPosition.m_Tangent, targetPosition.m_Tangent.xz) * num3;
        }
        else
        {
          if ((double) endAngle <= 0.0)
            return;
          // ISSUE: reference to a compiler-generated field
          float num4 = math.distance(sourcePosition.m_Position.xz, middlePosition.xz) - middleOffset;
          float num5 = math.max(math.distance(bezier4x3_1.a.xz, bezier4x3_1.b.xz) * smoothness, num4 * float2.x);
          float num6 = math.max(math.distance(bezier4x3_2.d.xz, bezier4x3_2.c.xz) * 2f, (float) ((double) middleOffset * (double) math.tan(endAngle / 2f) - (double) num4 * (double) float2.y));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          sourcePosition.m_Tangent = MathUtils.Normalize(sourcePosition.m_Tangent, sourcePosition.m_Tangent.xz) * num5;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          targetPosition.m_Tangent = MathUtils.Normalize(targetPosition.m_Tangent, targetPosition.m_Tangent.xz) * num6;
        }
      }

      private void ExtractNextConnectPosition(
        LaneSystem.ConnectPosition prevPosition,
        float3 middlePosition,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        out LaneSystem.ConnectPosition nextPosition,
        out bool nextIsSource)
      {
        // ISSUE: reference to a compiler-generated field
        float2 fromVector = math.normalizesafe(prevPosition.m_Position.xz - middlePosition.xz);
        float num1 = float.MaxValue;
        // ISSUE: object of a compiler-generated type is created
        nextPosition = new LaneSystem.ConnectPosition();
        nextIsSource = false;
        int index1 = -1;
        int index2 = -1;
        if (sourceBuffer.Length + targetBuffer.Length == 1)
        {
          if (sourceBuffer.Length == 1)
          {
            nextPosition = sourceBuffer[0];
            index1 = 0;
          }
          else
          {
            nextPosition = targetBuffer[0];
            index2 = 0;
          }
        }
        else
        {
          for (int index3 = 0; index3 < targetBuffer.Length; ++index3)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = targetBuffer[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) connectPosition.m_GroupIndex != (int) prevPosition.m_GroupIndex)
            {
              // ISSUE: reference to a compiler-generated field
              float2 toVector = math.normalizesafe(connectPosition.m_Position.xz - middlePosition.xz);
              // ISSUE: reference to a compiler-generated field
              float num2 = this.m_LeftHandTraffic ? MathUtils.RotationAngleRight(fromVector, toVector) : MathUtils.RotationAngleLeft(fromVector, toVector);
              if ((double) num2 < (double) num1)
              {
                num1 = num2;
                nextPosition = connectPosition;
                index2 = index3;
              }
            }
          }
          for (int index4 = 0; index4 < sourceBuffer.Length; ++index4)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = sourceBuffer[index4];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) connectPosition.m_GroupIndex != (int) prevPosition.m_GroupIndex)
            {
              // ISSUE: reference to a compiler-generated field
              float2 toVector = math.normalizesafe(connectPosition.m_Position.xz - middlePosition.xz);
              // ISSUE: reference to a compiler-generated field
              float num3 = this.m_LeftHandTraffic ? MathUtils.RotationAngleRight(fromVector, toVector) : MathUtils.RotationAngleLeft(fromVector, toVector);
              if ((double) num3 < (double) num1)
              {
                num1 = num3;
                nextPosition = connectPosition;
                index1 = index4;
              }
            }
          }
        }
        if (index1 >= 0)
        {
          sourceBuffer.RemoveAtSwapBack(index1);
          nextIsSource = true;
        }
        else
        {
          if (index2 < 0)
            return;
          targetBuffer.RemoveAtSwapBack(index2);
          nextIsSource = false;
        }
      }

      private bool GetRoundaboutLane(
        NativeList<LaneSystem.ConnectPosition> buffer,
        float roundaboutSize,
        ref LaneSystem.ConnectPosition roundaboutLane,
        ref int laneCount,
        ref float laneWidth,
        ref bool isPublicOnly,
        ref float spaceForLanes,
        bool isSource,
        bool preferHighway)
      {
        bool roundaboutLane1 = false;
        if (buffer.Length > 0)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition1 = buffer[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[connectPosition1.m_NodeComposition];
          // ISSUE: reference to a compiler-generated field
          float num1 = connectPosition1.m_IsEnd ? netCompositionData.m_RoundaboutSize.y : netCompositionData.m_RoundaboutSize.x;
          float num2 = roundaboutSize - num1;
          for (int index = 0; index < buffer.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition2 = buffer[index];
            // ISSUE: reference to a compiler-generated field
            Entity entity = connectPosition2.m_LaneData.m_Lane;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((connectPosition2.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0 && this.m_CarLaneData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              CarLaneData carLaneData = this.m_CarLaneData[entity];
              if (carLaneData.m_NotTrackLanePrefab != Entity.Null)
                entity = carLaneData.m_NotTrackLanePrefab;
            }
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData = this.m_NetLaneData[entity];
            if (isSource)
              num2 += netLaneData.m_Width * 1.33333337f;
            else
              num2 = math.max(num2, netLaneData.m_Width * 1.33333337f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((connectPosition2.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0 == preferHighway)
            {
              bool flag = (netLaneData.m_Flags & Game.Prefabs.LaneFlags.PublicOnly) != 0;
              if (isPublicOnly & !flag | isPublicOnly == flag & (double) netLaneData.m_Width < (double) laneWidth)
              {
                laneWidth = netLaneData.m_Width;
                isPublicOnly = flag;
                roundaboutLane = connectPosition2;
              }
            }
          }
          int y = math.select(1, buffer.Length, isSource);
          roundaboutLane1 = y > laneCount;
          laneCount = math.max(laneCount, y);
          spaceForLanes = math.min(spaceForLanes, num2);
        }
        return roundaboutLane1;
      }

      private void FillOldLaneBuffer(
        DynamicBuffer<SubLane> lanes,
        NativeParallelHashMap<LaneSystem.LaneKey, Entity> laneBuffer)
      {
        for (int index = 0; index < lanes.Length; ++index)
        {
          Entity subLane = lanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SecondaryLaneData.HasComponent(subLane))
          {
            Game.Prefabs.LaneFlags flags = (Game.Prefabs.LaneFlags) 0;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MasterLaneData.HasComponent(subLane))
              flags |= Game.Prefabs.LaneFlags.Master;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SlaveLaneData.HasComponent(subLane))
              flags |= Game.Prefabs.LaneFlags.Slave;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LaneSystem.LaneKey key = new LaneSystem.LaneKey(this.m_LaneData[subLane], this.m_PrefabRefData[subLane].m_Prefab, flags);
            laneBuffer.TryAdd(key, subLane);
          }
        }
      }

      private void RemoveUnusedOldLanes(
        int jobIndex,
        DynamicBuffer<SubLane> lanes,
        NativeParallelHashMap<LaneSystem.LaneKey, Entity> laneBuffer)
      {
        StackList<Entity> stackList = (StackList<Entity>) stackalloc Entity[lanes.Length];
        for (int index = 0; index < lanes.Length; ++index)
        {
          Entity subLane = lanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SecondaryLaneData.HasComponent(subLane))
          {
            Game.Prefabs.LaneFlags flags = (Game.Prefabs.LaneFlags) 0;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MasterLaneData.HasComponent(subLane))
              flags |= Game.Prefabs.LaneFlags.Master;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SlaveLaneData.HasComponent(subLane))
              flags |= Game.Prefabs.LaneFlags.Slave;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LaneSystem.LaneKey key = new LaneSystem.LaneKey(this.m_LaneData[subLane], this.m_PrefabRefData[subLane].m_Prefab, flags);
            if (laneBuffer.TryGetValue(key, out Entity _))
            {
              stackList.AddNoResize(subLane);
              laneBuffer.Remove(key);
            }
          }
        }
        if (stackList.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, stackList.AsArray(), in this.m_AppliedTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, stackList.AsArray());
      }

      private void UpdateLanes(int jobIndex, NativeList<Entity> laneBuffer)
      {
        if (laneBuffer.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, laneBuffer.AsArray());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, laneBuffer.AsArray());
      }

      private void CreateEdgeConnectionLanes(
        int jobIndex,
        ref int edgeLaneIndex,
        ref int connectionIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        NativeList<LaneSystem.ConnectPosition> tempBuffer1,
        NativeList<LaneSystem.ConnectPosition> tempBuffer2,
        Entity composition,
        EdgeGeometry geometryData,
        NetGeometryData prefabGeometryData,
        Curve curve,
        bool isTemp,
        Temp ownerTemp)
      {
        // ISSUE: reference to a compiler-generated field
        NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition];
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        LaneSystem.CompositionData compositionData = this.GetCompositionData(composition);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionLane> prefabCompositionLane = this.m_PrefabCompositionLanes[composition];
        int num1 = -1;
        for (int index = 0; index < sourceBuffer.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = sourceBuffer[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            float3 float3_1 = MathUtils.Position(curve.m_Bezier, connectPosition.m_CurvePosition);
            // ISSUE: reference to a compiler-generated field
            float2 x = MathUtils.Right(MathUtils.Tangent(curve.m_Bezier, connectPosition.m_CurvePosition).xz);
            MathUtils.TryNormalize(ref x);
            // ISSUE: reference to a compiler-generated field
            float3 float3_2 = connectPosition.m_Position - float3_1;
            float3_2.x = math.dot(x, float3_2.xz);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float elevationOffset = connectPosition.m_LaneData.m_Position.y + connectPosition.m_Elevation;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.HasComponent(owner))
            {
              if ((double) float3_2.x > 0.0)
              {
                // ISSUE: reference to a compiler-generated field
                elevationOffset -= this.m_ElevationData[owner].m_Elevation.y;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                elevationOffset -= this.m_ElevationData[owner].m_Elevation.x;
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int bestConnectionLane = this.FindBestConnectionLane(prefabCompositionLane, float3_2.xy, elevationOffset, Game.Prefabs.LaneFlags.Road, connectPosition.m_LaneData.m_Flags);
            if (bestConnectionLane != -1)
            {
              // ISSUE: reference to a compiler-generated field
              if ((int) connectPosition.m_GroupIndex != num1)
              {
                // ISSUE: reference to a compiler-generated field
                num1 = (int) connectPosition.m_GroupIndex;
              }
              else
                --connectionIndex;
              // ISSUE: reference to a compiler-generated method
              this.CreateCarEdgeConnections(jobIndex, ref edgeLaneIndex, ref random, owner, laneBuffer, geometryData, prefabCompositionData, prefabGeometryData, compositionData, connectPosition, float3_2.xy, connectionIndex++, true, isTemp, ownerTemp, prefabCompositionLane, bestConnectionLane);
            }
          }
        }
        int num2 = -1;
        for (int index = 0; index < targetBuffer.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = targetBuffer[index];
          // ISSUE: reference to a compiler-generated field
          float3 float3_3 = MathUtils.Position(curve.m_Bezier, connectPosition.m_CurvePosition);
          // ISSUE: reference to a compiler-generated field
          float2 x = MathUtils.Right(MathUtils.Tangent(curve.m_Bezier, connectPosition.m_CurvePosition).xz);
          MathUtils.TryNormalize(ref x);
          // ISSUE: reference to a compiler-generated field
          float3 float3_4 = connectPosition.m_Position - float3_3;
          float3_4.x = math.dot(x, float3_4.xz);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float elevationOffset = connectPosition.m_LaneData.m_Position.y + connectPosition.m_Elevation;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.HasComponent(owner))
          {
            if ((double) float3_4.x > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              elevationOffset -= this.m_ElevationData[owner].m_Elevation.y;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              elevationOffset -= this.m_ElevationData[owner].m_Elevation.x;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int bestConnectionLane = this.FindBestConnectionLane(prefabCompositionLane, float3_4.xy, elevationOffset, Game.Prefabs.LaneFlags.Pedestrian, connectPosition.m_LaneData.m_Flags);
            if (bestConnectionLane != -1)
            {
              NetCompositionLane prefabCompositionLaneData = prefabCompositionLane[bestConnectionLane];
              // ISSUE: reference to a compiler-generated method
              this.CreateEdgeConnectionLane(jobIndex, ref edgeLaneIndex, ref random, owner, laneBuffer, geometryData.m_Start, geometryData.m_End, prefabCompositionData, prefabGeometryData, compositionData, prefabCompositionLaneData, connectPosition, connectionIndex++, false, false, isTemp, ownerTemp);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int bestConnectionLane = this.FindBestConnectionLane(prefabCompositionLane, float3_4.xy, elevationOffset, Game.Prefabs.LaneFlags.Road, connectPosition.m_LaneData.m_Flags);
            if (bestConnectionLane != -1)
            {
              // ISSUE: reference to a compiler-generated field
              if ((int) connectPosition.m_GroupIndex != num2)
              {
                // ISSUE: reference to a compiler-generated field
                num2 = (int) connectPosition.m_GroupIndex;
              }
              else
                --connectionIndex;
              // ISSUE: reference to a compiler-generated method
              this.CreateCarEdgeConnections(jobIndex, ref edgeLaneIndex, ref random, owner, laneBuffer, geometryData, prefabCompositionData, prefabGeometryData, compositionData, connectPosition, float3_4.xy, connectionIndex++, false, isTemp, ownerTemp, prefabCompositionLane, bestConnectionLane);
            }
          }
        }
        // ISSUE: reference to a compiler-generated method
        UtilityTypes utilityTypes1 = this.FilterUtilityConnectPositions(targetBuffer, tempBuffer1);
        UtilityTypes utilityTypes2 = UtilityTypes.WaterPipe;
        while (utilityTypes1 != UtilityTypes.None)
        {
          if ((utilityTypes1 & utilityTypes2) != UtilityTypes.None)
          {
            utilityTypes1 &= ~utilityTypes2;
            // ISSUE: reference to a compiler-generated method
            this.FilterUtilityConnectPositions(utilityTypes2, tempBuffer1, tempBuffer2);
            if (tempBuffer2.Length != 0)
            {
              int4 a = (int4) -1;
              int4 b = (int4) -1;
              for (int index = 0; index < tempBuffer2.Length; ++index)
              {
                // ISSUE: variable of a compiler-generated type
                LaneSystem.ConnectPosition connectPosition = tempBuffer2[index];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((this.m_NetLaneData[connectPosition.m_LaneData.m_Lane].m_Flags & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
                {
                  b.xy = math.select(b.xy, (int2) index, new bool2(b.x == -1, index > b.y));
                  if (a.x != -1)
                    break;
                }
                else
                {
                  a.xy = math.select(a.xy, (int2) index, new bool2(a.x == -1, index > a.y));
                  if (b.x != -1)
                    break;
                }
              }
              for (int index = 0; index < prefabCompositionLane.Length; ++index)
              {
                NetCompositionLane netCompositionLane = prefabCompositionLane[index];
                // ISSUE: reference to a compiler-generated field
                if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0 && (this.m_UtilityLaneData[netCompositionLane.m_Lane].m_UtilityTypes & utilityTypes2) != UtilityTypes.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_NetLaneData[netCompositionLane.m_Lane].m_Flags & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
                  {
                    b.zw = math.select(b.zw, (int2) index, new bool2(b.z == -1, index > b.w));
                    if (a.z != -1)
                      break;
                  }
                  else
                  {
                    a.zw = math.select(a.zw, (int2) index, new bool2(a.z == -1, index > a.w));
                    if (b.z != -1)
                      break;
                  }
                }
              }
              a = math.select(a, b, a == -1 | math.any(a == -1) & math.all(b != -1));
              if (math.all(a.xz != -1))
              {
                // ISSUE: variable of a compiler-generated type
                LaneSystem.ConnectPosition connectPosition = tempBuffer2[a.x];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData1 = this.m_NetLaneData[connectPosition.m_LaneData.m_Lane];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UtilityLaneData utilityLaneData1 = this.m_UtilityLaneData[connectPosition.m_LaneData.m_Lane];
                NetCompositionLane prefabCompositionLaneData = prefabCompositionLane[a.z];
                // ISSUE: reference to a compiler-generated field
                UtilityLaneData utilityLaneData2 = this.m_UtilityLaneData[prefabCompositionLaneData.m_Lane];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData2 = this.m_NetLaneData[prefabCompositionLaneData.m_Lane];
                bool useGroundPosition = false;
                if (((netLaneData1.m_Flags ^ netLaneData2.m_Flags) & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
                {
                  if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
                  {
                    useGroundPosition = true;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    connectPosition.m_LaneData.m_Lane = this.GetConnectionLanePrefab(connectPosition.m_LaneData.m_Lane, utilityLaneData1, (double) utilityLaneData2.m_VisualCapacity < (double) utilityLaneData1.m_VisualCapacity, false);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.GetGroundPosition(connectPosition.m_Owner, math.select(0.0f, 1f, connectPosition.m_IsEnd), ref connectPosition.m_Position);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    connectPosition.m_LaneData.m_Lane = this.GetConnectionLanePrefab(prefabCompositionLaneData.m_Lane, utilityLaneData2, (double) utilityLaneData1.m_VisualCapacity < (double) utilityLaneData2.m_VisualCapacity, (double) utilityLaneData1.m_VisualCapacity > (double) utilityLaneData2.m_VisualCapacity);
                  }
                }
                else
                {
                  ++a.y;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  connectPosition.m_Position = this.CalculateUtilityConnectPosition(tempBuffer2, a.xy);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  connectPosition.m_LaneData.m_Lane = (double) utilityLaneData1.m_VisualCapacity >= (double) utilityLaneData2.m_VisualCapacity ? this.GetConnectionLanePrefab(prefabCompositionLaneData.m_Lane, utilityLaneData2, false, (double) utilityLaneData1.m_VisualCapacity > (double) utilityLaneData2.m_VisualCapacity) : this.GetConnectionLanePrefab(connectPosition.m_LaneData.m_Lane, utilityLaneData1, false, false);
                }
                // ISSUE: reference to a compiler-generated method
                this.CreateEdgeConnectionLane(jobIndex, ref edgeLaneIndex, ref random, owner, laneBuffer, geometryData.m_Start, geometryData.m_End, prefabCompositionData, prefabGeometryData, compositionData, prefabCompositionLaneData, connectPosition, connectionIndex++, useGroundPosition, false, isTemp, ownerTemp);
              }
              tempBuffer2.Clear();
            }
          }
          utilityTypes2 = (UtilityTypes) ((uint) utilityTypes2 << 1);
        }
        tempBuffer1.Clear();
      }

      private void GetGroundPosition(Entity entity, float curvePosition, ref float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NodeData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          position = this.m_NodeData[entity].m_Position;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            position = MathUtils.Position(this.m_CurveData[entity].m_Bezier, curvePosition);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ElevationData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        position.y -= math.csum(this.m_ElevationData[entity].m_Elevation) * 0.5f;
      }

      private Entity GetConnectionLanePrefab(
        Entity lanePrefab,
        UtilityLaneData utilityLaneData,
        bool wantSmaller,
        bool wantLarger)
      {
        Entity connectionLanePrefab = lanePrefab;
        UtilityLaneData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_UtilityLaneData.TryGetComponent(utilityLaneData.m_LocalConnectionPrefab, out componentData))
        {
          if (wantSmaller && (double) componentData.m_VisualCapacity < (double) utilityLaneData.m_VisualCapacity || wantLarger && (double) componentData.m_VisualCapacity > (double) utilityLaneData.m_VisualCapacity || !wantSmaller && !wantLarger && (double) componentData.m_VisualCapacity == (double) utilityLaneData.m_VisualCapacity)
            return utilityLaneData.m_LocalConnectionPrefab;
          if ((double) componentData.m_VisualCapacity == (double) utilityLaneData.m_VisualCapacity)
            connectionLanePrefab = utilityLaneData.m_LocalConnectionPrefab;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UtilityLaneData.TryGetComponent(utilityLaneData.m_LocalConnectionPrefab2, out componentData))
        {
          if (wantSmaller && (double) componentData.m_VisualCapacity < (double) utilityLaneData.m_VisualCapacity || wantLarger && (double) componentData.m_VisualCapacity > (double) utilityLaneData.m_VisualCapacity || !wantSmaller && !wantLarger && (double) componentData.m_VisualCapacity == (double) utilityLaneData.m_VisualCapacity)
            return utilityLaneData.m_LocalConnectionPrefab2;
          if (connectionLanePrefab == lanePrefab && (double) componentData.m_VisualCapacity == (double) utilityLaneData.m_VisualCapacity)
            connectionLanePrefab = utilityLaneData.m_LocalConnectionPrefab2;
        }
        return connectionLanePrefab;
      }

      private void FindAnchors(Entity owner, NativeParallelHashSet<Entity> anchorPrefabs)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TransformData.HasComponent(owner))
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[owner];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubLanes.HasBuffer(prefabRef.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Prefabs.SubLane> prefabSubLane = this.m_PrefabSubLanes[prefabRef.m_Prefab];
        for (int index = 0; index < prefabSubLane.Length; ++index)
        {
          Game.Prefabs.SubLane subLane = prefabSubLane[index];
          anchorPrefabs.Add(subLane.m_Prefab);
        }
      }

      private void FindAnchors(
        Entity owner,
        float order,
        NativeList<LaneSystem.LaneAnchor> anchors)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TransformData.HasComponent(owner))
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[owner];
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform = this.m_TransformData[owner];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubLanes.HasBuffer(prefabRef.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Prefabs.SubLane> prefabSubLane = this.m_PrefabSubLanes[prefabRef.m_Prefab];
        for (int index = 0; index < prefabSubLane.Length; ++index)
        {
          Game.Prefabs.SubLane subLane = prefabSubLane[index];
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneAnchor laneAnchor;
          if (subLane.m_NodeIndex.x != subLane.m_NodeIndex.y)
          {
            ref NativeList<LaneSystem.LaneAnchor> local1 = ref anchors;
            // ISSUE: object of a compiler-generated type is created
            laneAnchor = new LaneSystem.LaneAnchor();
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Prefab = subLane.m_Prefab;
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Position = ObjectUtils.LocalToWorld(transform, subLane.m_Curve.a);
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Order = order + 1f;
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_PathNode = new PathNode(owner, (ushort) subLane.m_NodeIndex.x);
            ref LaneSystem.LaneAnchor local2 = ref laneAnchor;
            local1.Add(in local2);
            ref NativeList<LaneSystem.LaneAnchor> local3 = ref anchors;
            // ISSUE: object of a compiler-generated type is created
            laneAnchor = new LaneSystem.LaneAnchor();
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Prefab = subLane.m_Prefab;
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Position = ObjectUtils.LocalToWorld(transform, subLane.m_Curve.d);
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Order = order + 1f;
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_PathNode = new PathNode(owner, (ushort) subLane.m_NodeIndex.y);
            ref LaneSystem.LaneAnchor local4 = ref laneAnchor;
            local3.Add(in local4);
          }
          else
          {
            ref NativeList<LaneSystem.LaneAnchor> local5 = ref anchors;
            // ISSUE: object of a compiler-generated type is created
            laneAnchor = new LaneSystem.LaneAnchor();
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Prefab = subLane.m_Prefab;
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Position = ObjectUtils.LocalToWorld(transform, subLane.m_Curve.a);
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_Order = order;
            // ISSUE: reference to a compiler-generated field
            laneAnchor.m_PathNode = new PathNode(owner, (ushort) subLane.m_NodeIndex.x);
            ref LaneSystem.LaneAnchor local6 = ref laneAnchor;
            local5.Add(in local6);
          }
        }
      }

      private bool IsAnchored(
        Entity owner,
        ref NativeParallelHashSet<Entity> anchorPrefabs,
        Entity prefab)
      {
        if (!anchorPrefabs.IsCreated)
        {
          anchorPrefabs = new NativeParallelHashSet<Entity>(8, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.HasBuffer(owner))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Objects.SubObject> subObject = this.m_SubObjects[owner];
            for (int index = 0; index < subObject.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindAnchors(subObject[index].m_SubObject, anchorPrefabs);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(owner))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FindAnchors(this.m_OwnerData[owner].m_Owner, anchorPrefabs);
          }
        }
        return anchorPrefabs.Contains(prefab);
      }

      private void FindAnchors(
        Entity node,
        NativeList<LaneSystem.LaneAnchor> anchors,
        NativeList<LaneSystem.LaneAnchor> tempBuffer)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.HasBuffer(node))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Objects.SubObject> subObject = this.m_SubObjects[node];
          for (int index = 0; index < subObject.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindAnchors(subObject[index].m_SubObject, 0.0f, tempBuffer);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(node))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FindAnchors(this.m_OwnerData[node].m_Owner, 2f, tempBuffer);
        }
        if (tempBuffer.Length == 0)
          return;
        if (anchors.Length > 1)
          anchors.Sort<LaneSystem.LaneAnchor>();
        if (tempBuffer.Length > 1)
          tempBuffer.Sort<LaneSystem.LaneAnchor>();
        int index1 = 0;
        int index2;
        for (int index3 = 0; index1 < anchors.Length && index3 < tempBuffer.Length; index3 = index2)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneAnchor anchor1 = anchors[index1];
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneAnchor laneAnchor1 = tempBuffer[index3];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (anchor1.m_Prefab.Index != laneAnchor1.m_Prefab.Index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (anchor1.m_Prefab.Index < laneAnchor1.m_Prefab.Index)
            {
              if (++index1 < anchors.Length)
                anchor1 = anchors[index1];
              else
                goto label_51;
            }
            else if (++index3 < tempBuffer.Length)
              laneAnchor1 = tempBuffer[index3];
            else
              goto label_51;
          }
          // ISSUE: reference to a compiler-generated field
          float3 position1 = anchor1.m_Position;
          // ISSUE: reference to a compiler-generated field
          float3 position2 = anchor1.m_Position;
          int index4;
          for (index4 = index1 + 1; index4 < anchors.Length; ++index4)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.LaneAnchor anchor2 = anchors[index4];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(anchor2.m_Prefab != anchor1.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              position1 += anchor2.m_Position;
              // ISSUE: reference to a compiler-generated field
              position2 = anchor2.m_Position;
            }
            else
              break;
          }
          float3 y1 = position1 / (float) (index4 - index1);
          index2 = index3 + 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (index2 < tempBuffer.Length && !(tempBuffer[index2].m_Prefab != laneAnchor1.m_Prefab))
            ++index2;
          int num1 = index4 - index1;
          int length = index2 - index3;
          if (length > num1)
          {
            for (int index5 = index3; index5 < index2; ++index5)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.LaneAnchor laneAnchor2 = tempBuffer[index5];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              laneAnchor2.m_Order = laneAnchor2.m_Order * 10000f + math.distance(laneAnchor2.m_Position, y1);
              tempBuffer[index5] = laneAnchor2;
            }
            tempBuffer.AsArray().GetSubArray(index3, length).Sort<LaneSystem.LaneAnchor>();
            length = num1;
          }
          if (length > 1)
          {
            // ISSUE: reference to a compiler-generated field
            float3 y2 = position2 - anchor1.m_Position;
            int num2 = index3 + length;
            for (int index6 = index3; index6 < num2; ++index6)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.LaneAnchor laneAnchor3 = tempBuffer[index6];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              laneAnchor3.m_Order = math.dot(laneAnchor3.m_Position - y1, y2);
              tempBuffer[index6] = laneAnchor3;
            }
            tempBuffer.AsArray().GetSubArray(index3, length).Sort<LaneSystem.LaneAnchor>();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num3 = (float) (((double) tempBuffer[num2 - 1].m_Order - (double) tempBuffer[index3].m_Order) * 0.0099999997764825821);
            int index7;
            for (int index8 = index3; index8 < num2; index8 = index7)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.LaneAnchor laneAnchor4 = tempBuffer[index8];
              index7 = index8 + 1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              while (index7 < num2 && (double) tempBuffer[index7].m_Order - (double) laneAnchor4.m_Order < (double) num3)
                ++index7;
              if (index7 > index8 + 1)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float3 y3 = anchors[index1 + index7 - index3 - 1].m_Position - anchors[index1 + index8 - index3].m_Position;
                for (int index9 = index8; index9 < index7; ++index9)
                {
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.LaneAnchor laneAnchor5 = tempBuffer[index9];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  laneAnchor5.m_Order = math.dot(laneAnchor5.m_Position - y1, y3);
                  tempBuffer[index9] = laneAnchor5;
                }
                tempBuffer.AsArray().GetSubArray(index8, index7 - index8).Sort<LaneSystem.LaneAnchor>();
              }
            }
          }
          for (int index10 = 0; index10 < length; ++index10)
            anchors[index1 + index10] = tempBuffer[index3 + index10];
          index1 = index4;
        }
label_51:
        tempBuffer.Clear();
      }

      private void CreateEdgeLanes(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        Composition composition,
        Edge edge,
        EdgeGeometry geometryData,
        NetGeometryData prefabGeometryData,
        bool isTemp,
        Temp ownerTemp)
      {
        // ISSUE: reference to a compiler-generated field
        NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        LaneSystem.CompositionData compositionData1 = this.GetCompositionData(composition.m_Edge);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionLane> prefabCompositionLane = this.m_PrefabCompositionLanes[composition.m_Edge];
        NativeList<LaneSystem.LaneAnchor> nativeList1 = new NativeList<LaneSystem.LaneAnchor>();
        NativeList<LaneSystem.LaneAnchor> nativeList2 = new NativeList<LaneSystem.LaneAnchor>();
        NativeList<LaneSystem.LaneAnchor> tempBuffer = new NativeList<LaneSystem.LaneAnchor>();
        for (int index = 0; index < prefabCompositionLane.Length; ++index)
        {
          NetCompositionLane netCompositionLane = prefabCompositionLane[index];
          if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.FindAnchor) != (Game.Prefabs.LaneFlags) 0)
          {
            if (!nativeList1.IsCreated)
              nativeList1 = new NativeList<LaneSystem.LaneAnchor>(8, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            if (!nativeList2.IsCreated)
              nativeList2 = new NativeList<LaneSystem.LaneAnchor>(8, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            if (!tempBuffer.IsCreated)
              tempBuffer = new NativeList<LaneSystem.LaneAnchor>(8, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LaneSystem.LaneAnchor laneAnchor1 = new LaneSystem.LaneAnchor();
            // ISSUE: reference to a compiler-generated field
            laneAnchor1.m_Prefab = netCompositionLane.m_Lane;
            // ISSUE: reference to a compiler-generated field
            laneAnchor1.m_Order = (float) index;
            // ISSUE: reference to a compiler-generated field
            laneAnchor1.m_PathNode = new PathNode(owner, netCompositionLane.m_Index, (byte) 0);
            // ISSUE: variable of a compiler-generated type
            LaneSystem.LaneAnchor laneAnchor2 = laneAnchor1;
            // ISSUE: object of a compiler-generated type is created
            laneAnchor1 = new LaneSystem.LaneAnchor();
            // ISSUE: reference to a compiler-generated field
            laneAnchor1.m_Prefab = netCompositionLane.m_Lane;
            // ISSUE: reference to a compiler-generated field
            laneAnchor1.m_Order = (float) index;
            // ISSUE: reference to a compiler-generated field
            laneAnchor1.m_PathNode = new PathNode(owner, netCompositionLane.m_Index, (byte) 4);
            // ISSUE: variable of a compiler-generated type
            LaneSystem.LaneAnchor laneAnchor3 = laneAnchor1;
            float s = (float) ((double) netCompositionLane.m_Position.x / (double) math.max(1f, prefabCompositionData.m_Width) + 0.5);
            // ISSUE: reference to a compiler-generated field
            laneAnchor2.m_Position = math.lerp(geometryData.m_Start.m_Left.a, geometryData.m_Start.m_Right.a, s);
            // ISSUE: reference to a compiler-generated field
            laneAnchor3.m_Position = math.lerp(geometryData.m_End.m_Left.d, geometryData.m_End.m_Right.d, s);
            if ((double) netCompositionLane.m_Position.z > 1.0 / 1000.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float y = math.distance(laneAnchor2.m_Position, laneAnchor3.m_Position);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3 = (laneAnchor3.m_Position - laneAnchor2.m_Position) * (netCompositionLane.m_Position.z / math.max(netCompositionLane.m_Position.z * 4f, y));
              // ISSUE: reference to a compiler-generated field
              laneAnchor2.m_Position += float3;
              // ISSUE: reference to a compiler-generated field
              laneAnchor3.m_Position -= float3;
            }
            // ISSUE: reference to a compiler-generated field
            laneAnchor2.m_Position.y += netCompositionLane.m_Position.y;
            // ISSUE: reference to a compiler-generated field
            laneAnchor3.m_Position.y += netCompositionLane.m_Position.y;
            nativeList1.Add(in laneAnchor2);
            nativeList2.Add(in laneAnchor3);
          }
        }
        if (nativeList1.IsCreated)
        {
          // ISSUE: reference to a compiler-generated method
          this.FindAnchors(edge.m_Start, nativeList1, tempBuffer);
        }
        if (nativeList2.IsCreated)
        {
          // ISSUE: reference to a compiler-generated method
          this.FindAnchors(edge.m_End, nativeList2, tempBuffer);
        }
        bool flag1 = false;
        if ((prefabGeometryData.m_Flags & (GeometryFlags.StraightEdges | GeometryFlags.SmoothSlopes)) == GeometryFlags.StraightEdges)
        {
          Segment start = geometryData.m_Start with
          {
            m_Left = MathUtils.Join(geometryData.m_Start.m_Left, geometryData.m_End.m_Left),
            m_Right = MathUtils.Join(geometryData.m_Start.m_Right, geometryData.m_End.m_Right),
            m_Length = geometryData.m_Start.m_Length + geometryData.m_End.m_Length
          };
          for (int index = 0; index < prefabCompositionLane.Length; ++index)
          {
            NetCompositionLane prefabCompositionLaneData = prefabCompositionLane[index];
            flag1 |= (prefabCompositionLaneData.m_Flags & Game.Prefabs.LaneFlags.HasAuxiliary) != 0;
            // ISSUE: reference to a compiler-generated method
            this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, start, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(0, 4), new float2(0.0f, 1f), nativeList1, nativeList2, (bool2) true, isTemp, ownerTemp);
          }
        }
        else
        {
          for (int index = 0; index < prefabCompositionLane.Length; ++index)
          {
            NetCompositionLane prefabCompositionLaneData = prefabCompositionLane[index];
            flag1 |= (prefabCompositionLaneData.m_Flags & Game.Prefabs.LaneFlags.HasAuxiliary) != 0;
            // ISSUE: reference to a compiler-generated method
            this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, geometryData.m_Start, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(0, 2), new float2(0.0f, 0.5f), nativeList1, nativeList2, new bool2(true, false), isTemp, ownerTemp);
            // ISSUE: reference to a compiler-generated method
            this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, geometryData.m_End, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(2, 4), new float2(0.5f, 1f), nativeList1, nativeList2, new bool2(false, true), isTemp, ownerTemp);
          }
        }
        if (flag1)
        {
          for (int index1 = 0; index1 < prefabCompositionLane.Length; ++index1)
          {
            NetCompositionLane netCompositionLane1 = prefabCompositionLane[index1];
            if ((netCompositionLane1.m_Flags & Game.Prefabs.LaneFlags.HasAuxiliary) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<AuxiliaryNetLane> prefabAuxiliaryLane = this.m_PrefabAuxiliaryLanes[netCompositionLane1.m_Lane];
              int x1 = 5;
              for (int index2 = 0; index2 < prefabAuxiliaryLane.Length; ++index2)
              {
                AuxiliaryNetLane auxiliaryNetLane = prefabAuxiliaryLane[index2];
                if (NetCompositionHelpers.TestLaneFlags(auxiliaryNetLane, prefabCompositionData.m_Flags))
                {
                  bool flag2 = false;
                  bool c = false;
                  float3 float3_1 = (float3) 0.0f;
                  if ((double) auxiliaryNetLane.m_Spacing.x > 0.10000000149011612)
                  {
                    for (int index3 = 0; index3 < prefabCompositionLane.Length; ++index3)
                    {
                      NetCompositionLane netCompositionLane2 = prefabCompositionLane[index3];
                      if ((netCompositionLane2.m_Flags & Game.Prefabs.LaneFlags.HasAuxiliary) != (Game.Prefabs.LaneFlags) 0)
                      {
                        for (int index4 = 0; index4 < prefabAuxiliaryLane.Length; ++index4)
                        {
                          AuxiliaryNetLane lane = prefabAuxiliaryLane[index4];
                          if (NetCompositionHelpers.TestLaneFlags(lane, prefabCompositionData.m_Flags) && !(lane.m_Prefab != auxiliaryNetLane.m_Prefab) && (double) lane.m_Spacing.x > 0.10000000149011612 && (index3 != index1 || index4 != index2))
                          {
                            if (index3 < index1 || index3 == index1 && index4 < index2)
                            {
                              c = true;
                              float3_1 = netCompositionLane2.m_Position;
                              float3_1.xy += lane.m_Position.xy;
                            }
                            else
                              flag2 = true;
                          }
                        }
                      }
                    }
                  }
                  float num1 = geometryData.m_Start.middleLength + geometryData.m_End.middleLength;
                  float num2 = 0.0f;
                  int y = 0;
                  float3 float3_2 = new float3();
                  float3 float3_3 = new float3();
                  float3 float3_4 = new float3();
                  float3 float3_5 = new float3();
                  float3 float3_6 = new float3();
                  float3 float3_7 = new float3();
                  float3 float3_8 = new float3();
                  float3 float3_9 = new float3();
                  NetCompositionLane prefabCompositionLaneData = netCompositionLane1 with
                  {
                    m_Lane = auxiliaryNetLane.m_Prefab
                  };
                  prefabCompositionLaneData.m_Position.xy += auxiliaryNetLane.m_Position.xy;
                  // ISSUE: reference to a compiler-generated field
                  prefabCompositionLaneData.m_Flags = this.m_NetLaneData[auxiliaryNetLane.m_Prefab].m_Flags | auxiliaryNetLane.m_Flags;
                  if ((auxiliaryNetLane.m_Flags & Game.Prefabs.LaneFlags.EvenSpacing) != (Game.Prefabs.LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    NetCompositionData compositionData2 = this.m_PrefabCompositionData[composition.m_StartNode];
                    // ISSUE: reference to a compiler-generated field
                    NetCompositionData compositionData3 = this.m_PrefabCompositionData[composition.m_EndNode];
                    // ISSUE: reference to a compiler-generated field
                    EdgeNodeGeometry geometry1 = this.m_StartNodeGeometryData[owner].m_Geometry;
                    // ISSUE: reference to a compiler-generated field
                    EdgeNodeGeometry geometry2 = this.m_EndNodeGeometryData[owner].m_Geometry;
                    if (!NetCompositionHelpers.TestLaneFlags(auxiliaryNetLane, compositionData2.m_Flags))
                    {
                      float num3 = math.min((float) (((double) geometry1.m_Left.middleLength + (double) geometry1.m_Right.middleLength) * 0.5), auxiliaryNetLane.m_Spacing.z * 0.333333343f);
                      num1 += num3;
                      num2 -= num3;
                      float3_2 = geometry1.m_Right.m_Right.d - geometryData.m_Start.m_Left.a;
                      float3_3 = geometry1.m_Left.m_Left.d - geometryData.m_Start.m_Right.a;
                    }
                    else if ((double) auxiliaryNetLane.m_Position.z > 0.10000000149011612)
                    {
                      float t1 = (float) ((double) prefabCompositionLaneData.m_Position.x / (double) math.max(1f, prefabCompositionData.m_Width) + 0.5);
                      Bezier4x3 curve1 = MathUtils.Lerp(geometryData.m_Start.m_Left, geometryData.m_Start.m_Right, t1);
                      float3 tangent = -MathUtils.StartTangent(curve1);
                      tangent = MathUtils.Normalize(tangent, tangent.xz);
                      // ISSUE: reference to a compiler-generated method
                      float3_3 = float3_2 = this.CalculateAuxialryZOffset(curve1.a, tangent, geometry1, compositionData2, auxiliaryNetLane);
                      if (c)
                      {
                        float t2 = (float) ((double) float3_1.x / (double) math.max(1f, prefabCompositionData.m_Width) + 0.5);
                        Bezier4x3 curve2 = MathUtils.Lerp(geometryData.m_Start.m_Left, geometryData.m_Start.m_Right, t2);
                        tangent = -MathUtils.StartTangent(curve2);
                        tangent = MathUtils.Normalize(tangent, tangent.xz);
                        // ISSUE: reference to a compiler-generated method
                        float3_7 = float3_6 = this.CalculateAuxialryZOffset(curve2.a, tangent, geometry1, compositionData2, auxiliaryNetLane);
                      }
                    }
                    if (!NetCompositionHelpers.TestLaneFlags(auxiliaryNetLane, compositionData3.m_Flags))
                    {
                      float num4 = math.min((float) (((double) geometry2.m_Left.middleLength + (double) geometry2.m_Right.middleLength) * 0.5), auxiliaryNetLane.m_Spacing.z * 0.333333343f);
                      num1 += num4;
                      float3_4 = geometry2.m_Left.m_Left.d - geometryData.m_End.m_Left.d;
                      float3_5 = geometry2.m_Right.m_Right.d - geometryData.m_End.m_Right.d;
                    }
                    else if ((double) auxiliaryNetLane.m_Position.z > 0.10000000149011612)
                    {
                      float t3 = (float) ((double) prefabCompositionLaneData.m_Position.x / (double) math.max(1f, prefabCompositionData.m_Width) + 0.5);
                      Bezier4x3 curve3 = MathUtils.Lerp(geometryData.m_End.m_Left, geometryData.m_End.m_Right, t3);
                      float3 tangent = MathUtils.EndTangent(curve3);
                      tangent = MathUtils.Normalize(tangent, tangent.xz);
                      // ISSUE: reference to a compiler-generated method
                      float3_5 = float3_4 = this.CalculateAuxialryZOffset(curve3.d, tangent, geometry2, compositionData3, auxiliaryNetLane);
                      if (c)
                      {
                        float t4 = (float) ((double) float3_1.x / (double) math.max(1f, prefabCompositionData.m_Width) + 0.5);
                        Bezier4x3 curve4 = MathUtils.Lerp(geometryData.m_End.m_Left, geometryData.m_End.m_Right, t4);
                        tangent = MathUtils.EndTangent(curve4);
                        tangent = MathUtils.Normalize(tangent, tangent.xz);
                        // ISSUE: reference to a compiler-generated method
                        float3_9 = float3_8 = this.CalculateAuxialryZOffset(curve4.d, tangent, geometry2, compositionData3, auxiliaryNetLane);
                      }
                    }
                  }
                  if ((double) auxiliaryNetLane.m_Spacing.z > 0.10000000149011612)
                  {
                    int a = Mathf.FloorToInt((float) ((double) num1 / (double) auxiliaryNetLane.m_Spacing.z + 0.5));
                    y = (auxiliaryNetLane.m_Flags & Game.Prefabs.LaneFlags.EvenSpacing) == (Game.Prefabs.LaneFlags) 0 ? math.select(a, 1, a == 0 & (double) num1 > (double) auxiliaryNetLane.m_Spacing.z * 0.10000000149011612) : math.max(0, a - 1);
                  }
                  float num5;
                  float num6;
                  if ((auxiliaryNetLane.m_Flags & Game.Prefabs.LaneFlags.EvenSpacing) != (Game.Prefabs.LaneFlags) 0)
                  {
                    num5 = 1f;
                    num6 = num1 / (float) (y + 1);
                  }
                  else
                  {
                    num5 = 0.5f;
                    num6 = num1 / (float) y;
                  }
                  float length1 = (num5 - 1f) * num6 + num2;
                  float2 t5;
                  if ((double) length1 > (double) geometryData.m_Start.middleLength)
                  {
                    Bounds1 t6 = new Bounds1(0.0f, 1f);
                    MathUtils.ClampLength(MathUtils.Lerp(geometryData.m_End.m_Left, geometryData.m_End.m_Right, 0.5f).xz, ref t6, length1 - geometryData.m_Start.middleLength);
                    t5.x = 1f + t6.max;
                  }
                  else
                  {
                    Bounds1 t7 = new Bounds1(0.0f, 1f);
                    MathUtils.ClampLength(MathUtils.Lerp(geometryData.m_Start.m_Left, geometryData.m_Start.m_Right, 0.5f).xz, ref t7, length1);
                    t5.x = t7.max;
                  }
                  for (int index5 = 0; index5 <= y; ++index5)
                  {
                    float length2 = ((float) index5 + num5) * num6 + num2;
                    if ((double) length2 > (double) geometryData.m_Start.middleLength)
                    {
                      Bounds1 t8 = new Bounds1(0.0f, 1f);
                      MathUtils.ClampLength(MathUtils.Lerp(geometryData.m_End.m_Left, geometryData.m_End.m_Right, 0.5f).xz, ref t8, length2 - geometryData.m_Start.middleLength);
                      t5.y = 1f + t8.max;
                    }
                    else
                    {
                      Bounds1 t9 = new Bounds1(0.0f, 1f);
                      MathUtils.ClampLength(MathUtils.Lerp(geometryData.m_Start.m_Left, geometryData.m_Start.m_Right, 0.5f).xz, ref t9, length2);
                      t5.y = t9.max;
                    }
                    Segment segment1 = new Segment();
                    if ((double) t5.x >= 1.0)
                    {
                      segment1.m_Left = MathUtils.Cut(geometryData.m_End.m_Left, t5 - 1f);
                      segment1.m_Right = MathUtils.Cut(geometryData.m_End.m_Right, t5 - 1f);
                    }
                    else if ((double) t5.y <= 1.0)
                    {
                      segment1.m_Left = MathUtils.Cut(geometryData.m_Start.m_Left, t5);
                      segment1.m_Right = MathUtils.Cut(geometryData.m_Start.m_Right, t5);
                    }
                    else
                    {
                      float2 t10 = new float2(t5.x, 1f);
                      float2 t11 = new float2(0.0f, t5.y - 1f);
                      segment1.m_Left = MathUtils.Join(MathUtils.Cut(geometryData.m_Start.m_Left, t10), MathUtils.Cut(geometryData.m_End.m_Left, t11));
                      segment1.m_Right = MathUtils.Join(MathUtils.Cut(geometryData.m_Start.m_Right, t10), MathUtils.Cut(geometryData.m_End.m_Right, t11));
                    }
                    Segment segment2 = segment1;
                    if ((auxiliaryNetLane.m_Flags & Game.Prefabs.LaneFlags.EvenSpacing) != (Game.Prefabs.LaneFlags) 0)
                    {
                      if (index5 == 0)
                      {
                        segment1.m_Left.a += float3_2;
                        segment1.m_Right.a += float3_3;
                        segment2.m_Left.a += float3_6;
                        segment2.m_Right.a += float3_7;
                      }
                      if (index5 == y)
                      {
                        segment1.m_Left.d += float3_4;
                        segment1.m_Right.d += float3_5;
                        segment2.m_Left.d += float3_8;
                        segment2.m_Right.d += float3_9;
                      }
                    }
                    float2 edgeDelta = math.select(t5 * 0.5f, new float2(0.0f, 1f), index5 == new int2(0, y));
                    if ((double) auxiliaryNetLane.m_Spacing.x > 0.10000000149011612)
                    {
                      Segment segment3 = new Segment();
                      float3 float3_10 = (float3) netCompositionLane1.m_Position.x;
                      float3_10.xz += new float2(-auxiliaryNetLane.m_Spacing.x, auxiliaryNetLane.m_Spacing.x);
                      float3_10.x = math.select(float3_10.x, float3_1.x, c);
                      float3_10 = math.saturate(float3_10 / math.max(1f, prefabCompositionData.m_Width) + 0.5f);
                      if (index5 == 0)
                      {
                        float3 startPos = !c ? math.lerp(segment1.m_Left.a, segment1.m_Right.a, float3_10.x) : math.lerp(segment2.m_Left.a, segment2.m_Right.a, float3_10.x);
                        segment3.m_Left = NetUtils.StraightCurve(startPos, math.lerp(segment1.m_Left.a, segment1.m_Right.a, float3_10.y));
                        segment3.m_Right = segment3.m_Left;
                        // ISSUE: reference to a compiler-generated method
                        this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, segment3, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(x1, x1 + 2), edgeDelta.xx, nativeList1, nativeList2, new bool2(!c, false), isTemp, ownerTemp);
                        int x2 = x1 + 2;
                        if (!flag2)
                        {
                          segment3.m_Left = NetUtils.StraightCurve(segment3.m_Left.d, math.lerp(segment1.m_Left.a, segment1.m_Right.a, float3_10.z));
                          segment3.m_Right = segment3.m_Left;
                          // ISSUE: reference to a compiler-generated method
                          this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, segment3, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(x2, x2 + 2), edgeDelta.xx, nativeList1, nativeList2, new bool2(false, true), isTemp, ownerTemp);
                          x2 += 2;
                        }
                        x1 = x2 + 1;
                      }
                      float3 startPos1 = !c ? math.lerp(segment1.m_Left.d, segment1.m_Right.d, float3_10.x) : math.lerp(segment2.m_Left.d, segment2.m_Right.d, float3_10.x);
                      segment3.m_Left = NetUtils.StraightCurve(startPos1, math.lerp(segment1.m_Left.d, segment1.m_Right.d, float3_10.y));
                      segment3.m_Right = segment3.m_Left;
                      // ISSUE: reference to a compiler-generated method
                      this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, segment3, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(x1, x1 + 2), edgeDelta.yy, nativeList1, nativeList2, new bool2(!c, false), isTemp, ownerTemp);
                      int x3 = x1 + 2;
                      if (!flag2)
                      {
                        segment3.m_Left = NetUtils.StraightCurve(segment3.m_Left.d, math.lerp(segment1.m_Left.d, segment1.m_Right.d, float3_10.z));
                        segment3.m_Right = segment3.m_Left;
                        // ISSUE: reference to a compiler-generated method
                        this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, segment3, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(x3, x3 + 2), edgeDelta.yy, nativeList1, nativeList2, new bool2(false, true), isTemp, ownerTemp);
                        x3 += 2;
                      }
                      x1 = x3 + 1;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CreateEdgeLane(jobIndex, ref random, owner, laneBuffer, segment1, prefabCompositionData, compositionData1, prefabCompositionLane, prefabCompositionLaneData, new int2(x1, x1 + 2), edgeDelta, nativeList1, nativeList2, (bool2) true, isTemp, ownerTemp);
                      x1 += 2;
                    }
                    t5.x = t5.y;
                  }
                  if ((double) auxiliaryNetLane.m_Spacing.x <= 0.10000000149011612)
                    ++x1;
                }
              }
            }
          }
        }
        if (nativeList1.IsCreated)
          nativeList1.Dispose();
        if (nativeList2.IsCreated)
          nativeList2.Dispose();
        if (!tempBuffer.IsCreated)
          return;
        tempBuffer.Dispose();
      }

      private float3 CalculateAuxialryZOffset(
        float3 position,
        float3 tangent,
        EdgeNodeGeometry nodeGeometry,
        NetCompositionData compositionData,
        AuxiliaryNetLane auxiliaryLane)
      {
        float x = auxiliaryLane.m_Position.z;
        if ((compositionData.m_Flags.m_General & CompositionFlags.General.DeadEnd) != (CompositionFlags.General) 0)
          x = math.max(0.0f, math.min(x, compositionData.m_Width * 0.125f));
        else if ((double) nodeGeometry.m_MiddleRadius > 0.0)
        {
          float2 t;
          if (MathUtils.Intersect(new Line2(position.xz, position.xz + tangent.xz), new Line2(nodeGeometry.m_Right.m_Left.d.xz, nodeGeometry.m_Right.m_Right.d.xz), out t))
            x = math.max(0.0f, math.min(x, t.x * 0.5f));
        }
        else
        {
          float2 t1;
          if (MathUtils.Intersect(new Line2(position.xz, position.xz + tangent.xz), new Line2(nodeGeometry.m_Left.m_Left.d.xz, nodeGeometry.m_Left.m_Right.d.xz), out t1))
            x = math.max(0.0f, math.min(x, t1.x * 0.5f));
          float2 t2;
          if (MathUtils.Intersect(new Line2(position.xz, position.xz + tangent.xz), new Line2(nodeGeometry.m_Right.m_Left.d.xz, nodeGeometry.m_Right.m_Right.d.xz), out t2))
            x = math.max(0.0f, math.min(x, t2.x * 0.5f));
        }
        return tangent * x;
      }

      private int FindBestConnectionLane(
        DynamicBuffer<NetCompositionLane> prefabCompositionLanes,
        float2 offset,
        float elevationOffset,
        Game.Prefabs.LaneFlags laneType,
        Game.Prefabs.LaneFlags laneFlags)
      {
        float num1 = float.MaxValue;
        int bestConnectionLane = -1;
        for (int index = 0; index < prefabCompositionLanes.Length; ++index)
        {
          NetCompositionLane prefabCompositionLane = prefabCompositionLanes[index];
          if ((prefabCompositionLane.m_Flags & laneType) != (Game.Prefabs.LaneFlags) 0)
          {
            if ((laneFlags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
            {
              if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
                continue;
            }
            else if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
              continue;
            float2 x = new float2(offset.y, elevationOffset);
            x = math.abs(x - prefabCompositionLane.m_Position.yy);
            float num2 = math.lengthsq(new float2(prefabCompositionLane.m_Position.x - offset.x, math.cmin(x)));
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              bestConnectionLane = index;
            }
          }
        }
        return bestConnectionLane;
      }

      private void CreateCarEdgeConnections(
        int jobIndex,
        ref int edgeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        EdgeGeometry geometryData,
        NetCompositionData prefabCompositionData,
        NetGeometryData prefabGeometryData,
        LaneSystem.CompositionData compositionData,
        LaneSystem.ConnectPosition connectPosition,
        float2 offset,
        int connectionIndex,
        bool isSource,
        bool isTemp,
        Temp ownerTemp,
        DynamicBuffer<NetCompositionLane> prefabCompositionLanes,
        int bestIndex)
      {
        NetCompositionLane prefabCompositionLane1 = prefabCompositionLanes[bestIndex];
        int num1 = -1;
        int index1 = 0;
        float num2 = float.MaxValue;
        for (int index2 = 0; index2 < prefabCompositionLanes.Length; ++index2)
        {
          NetCompositionLane prefabCompositionLane2 = prefabCompositionLanes[index2];
          if ((prefabCompositionLane2.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0 && (int) prefabCompositionLane2.m_Carriageway == (int) prefabCompositionLane1.m_Carriageway)
          {
            // ISSUE: reference to a compiler-generated field
            if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
            {
              if ((prefabCompositionLane2.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
                continue;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0 && (prefabCompositionLane2.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarLaneData[prefabCompositionLane2.m_Lane].m_RoadTypes == RoadTypes.Car)
            {
              if (num1 != -1 && ((int) prefabCompositionLane2.m_Group != num1 || (prefabCompositionLane2.m_Flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0))
              {
                NetCompositionLane prefabCompositionLane3 = prefabCompositionLanes[index1];
                // ISSUE: reference to a compiler-generated method
                this.CreateEdgeConnectionLane(jobIndex, ref edgeLaneIndex, ref random, owner, laneBuffer, geometryData.m_Start, geometryData.m_End, prefabCompositionData, prefabGeometryData, compositionData, prefabCompositionLane3, connectPosition, connectionIndex, false, isSource, isTemp, ownerTemp);
                num1 = -1;
                num2 = float.MaxValue;
              }
              if ((prefabCompositionLane2.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
              {
                num1 = (int) prefabCompositionLane2.m_Group;
                float num3 = math.abs(prefabCompositionLane2.m_Position.x - offset.x);
                if ((double) num3 < (double) num2)
                {
                  num2 = num3;
                  index1 = index2;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateEdgeConnectionLane(jobIndex, ref edgeLaneIndex, ref random, owner, laneBuffer, geometryData.m_Start, geometryData.m_End, prefabCompositionData, prefabGeometryData, compositionData, prefabCompositionLane2, connectPosition, connectionIndex, false, isSource, isTemp, ownerTemp);
              }
            }
          }
        }
        if (num1 == -1)
          return;
        NetCompositionLane prefabCompositionLane4 = prefabCompositionLanes[index1];
        // ISSUE: reference to a compiler-generated method
        this.CreateEdgeConnectionLane(jobIndex, ref edgeLaneIndex, ref random, owner, laneBuffer, geometryData.m_Start, geometryData.m_End, prefabCompositionData, prefabGeometryData, compositionData, prefabCompositionLane4, connectPosition, connectionIndex, false, isSource, isTemp, ownerTemp);
      }

      private LaneSystem.CompositionData GetCompositionData(Entity composition)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.CompositionData compositionData = new LaneSystem.CompositionData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoadData.HasComponent(composition))
        {
          // ISSUE: reference to a compiler-generated field
          RoadComposition roadComposition = this.m_RoadData[composition];
          // ISSUE: reference to a compiler-generated field
          compositionData.m_SpeedLimit = roadComposition.m_SpeedLimit;
          // ISSUE: reference to a compiler-generated field
          compositionData.m_RoadFlags = roadComposition.m_Flags;
          // ISSUE: reference to a compiler-generated field
          compositionData.m_Priority = roadComposition.m_Priority;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TrackData.HasComponent(composition))
          {
            // ISSUE: reference to a compiler-generated field
            TrackComposition trackComposition = this.m_TrackData[composition];
            // ISSUE: reference to a compiler-generated field
            compositionData.m_SpeedLimit = trackComposition.m_SpeedLimit;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_WaterwayData.HasComponent(composition))
            {
              // ISSUE: reference to a compiler-generated field
              WaterwayComposition waterwayComposition = this.m_WaterwayData[composition];
              // ISSUE: reference to a compiler-generated field
              compositionData.m_SpeedLimit = waterwayComposition.m_SpeedLimit;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathwayData.HasComponent(composition))
              {
                // ISSUE: reference to a compiler-generated field
                PathwayComposition pathwayComposition = this.m_PathwayData[composition];
                // ISSUE: reference to a compiler-generated field
                compositionData.m_SpeedLimit = pathwayComposition.m_SpeedLimit;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TaxiwayData.HasComponent(composition))
                {
                  // ISSUE: reference to a compiler-generated field
                  TaxiwayComposition taxiwayComposition = this.m_TaxiwayData[composition];
                  // ISSUE: reference to a compiler-generated field
                  compositionData.m_SpeedLimit = taxiwayComposition.m_SpeedLimit;
                  // ISSUE: reference to a compiler-generated field
                  compositionData.m_TaxiwayFlags = taxiwayComposition.m_Flags;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  compositionData.m_SpeedLimit = 1f;
                }
              }
            }
          }
        }
        return compositionData;
      }

      private NetCompositionLane FindClosestLane(
        DynamicBuffer<NetCompositionLane> prefabCompositionLanes,
        Game.Prefabs.LaneFlags all,
        Game.Prefabs.LaneFlags none,
        float3 position,
        int carriageWay = -1)
      {
        float num1 = float.MaxValue;
        NetCompositionLane closestLane = new NetCompositionLane();
        if (!prefabCompositionLanes.IsCreated)
          return closestLane;
        for (int index = 0; index < prefabCompositionLanes.Length; ++index)
        {
          NetCompositionLane prefabCompositionLane = prefabCompositionLanes[index];
          if ((prefabCompositionLane.m_Flags & (all | none)) == all && (carriageWay == -1 || (int) prefabCompositionLane.m_Carriageway == carriageWay))
          {
            float num2 = math.lengthsq(prefabCompositionLane.m_Position - position);
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              closestLane = prefabCompositionLane;
            }
          }
        }
        return closestLane;
      }

      private static void Invert(ref Lane laneData, ref Curve curveData, ref EdgeLane edgeLaneData)
      {
        PathNode startNode = laneData.m_StartNode;
        laneData.m_StartNode = laneData.m_EndNode;
        laneData.m_EndNode = startNode;
        curveData.m_Bezier = MathUtils.Invert(curveData.m_Bezier);
        edgeLaneData.m_EdgeDelta = edgeLaneData.m_EdgeDelta.yx;
      }

      private void CreateNodeConnectionLanes(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        NativeList<LaneSystem.ConnectPosition> tempBuffer,
        bool isRoundabout,
        bool isTemp,
        Temp ownerTemp)
      {
        int index1 = 0;
        for (int index2 = 0; index2 < middleConnections.Length; ++index2)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.MiddleConnection middleConnection = middleConnections[index2];
          // ISSUE: reference to a compiler-generated field
          if (middleConnection.m_TargetLane != Entity.Null)
            middleConnections[index1++] = middleConnection;
        }
        middleConnections.RemoveRange(index1, middleConnections.Length - index1);
        if (middleConnections.Length >= 2)
        {
          NativeList<LaneSystem.MiddleConnection> list = middleConnections;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LaneSystem.MiddleConnectionComparer connectionComparer = new LaneSystem.MiddleConnectionComparer();
          // ISSUE: variable of a compiler-generated type
          LaneSystem.MiddleConnectionComparer comp = connectionComparer;
          list.Sort<LaneSystem.MiddleConnection, LaneSystem.MiddleConnectionComparer>(comp);
        }
        for (int index3 = 0; index3 < middleConnections.Length; ++index3)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.MiddleConnection middleConnection1 = middleConnections[index3];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection1.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            int4 a = (int4) -1;
            int4 b = (int4) -1;
            for (int index4 = index3; index4 < middleConnections.Length; ++index4)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.MiddleConnection middleConnection2 = middleConnections[index4];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((middleConnection2.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0 && (middleConnection2.m_ConnectPosition.m_UtilityTypes & middleConnection1.m_ConnectPosition.m_UtilityTypes) != UtilityTypes.None && !(middleConnection2.m_SourceNode != middleConnection1.m_SourceNode))
              {
                // ISSUE: reference to a compiler-generated field
                tempBuffer.Add(in middleConnection2.m_ConnectPosition);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((middleConnection2.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
                  b.xy = math.select(b.xy, (int2) index3, new bool2(b.x == -1, index3 > b.y));
                else
                  a.xy = math.select(a.xy, (int2) index3, new bool2(a.x == -1, index3 > a.y));
                // ISSUE: reference to a compiler-generated field
                if ((middleConnection2.m_TargetFlags & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
                  b.zw = math.select(b.zw, (int2) index3, new bool2(b.z == -1, index3 > b.w));
                else
                  a.zw = math.select(a.zw, (int2) index3, new bool2(a.z == -1, index3 > a.w));
              }
              else
                break;
            }
            index3 += math.max(0, tempBuffer.Length - 1);
            a = math.select(a, b, a == -1 | math.any(a == -1) & math.all(b != -1));
            if (math.all(a.xz != -1))
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.MiddleConnection middleConnection3 = middleConnections[a.x];
              // ISSUE: variable of a compiler-generated type
              LaneSystem.MiddleConnection middleConnection4 = middleConnections[a.z];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UtilityLaneData utilityLaneData1 = this.m_UtilityLaneData[middleConnection3.m_ConnectPosition.m_LaneData.m_Lane];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UtilityLaneData utilityLaneData2 = this.m_UtilityLaneData[middleConnection4.m_TargetLane];
              bool useGroundPosition = false;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (((middleConnection3.m_ConnectPosition.m_LaneData.m_Flags ^ middleConnection4.m_TargetFlags) & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((middleConnection3.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0)
                {
                  useGroundPosition = true;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  middleConnection3.m_ConnectPosition.m_LaneData.m_Lane = this.GetConnectionLanePrefab(middleConnection3.m_ConnectPosition.m_LaneData.m_Lane, utilityLaneData1, (double) utilityLaneData2.m_VisualCapacity < (double) utilityLaneData1.m_VisualCapacity, false);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.GetGroundPosition(middleConnection3.m_ConnectPosition.m_Owner, math.select(0.0f, 1f, middleConnection3.m_ConnectPosition.m_IsEnd), ref middleConnection3.m_ConnectPosition.m_Position);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  middleConnection3.m_ConnectPosition.m_LaneData.m_Lane = this.GetConnectionLanePrefab(middleConnection4.m_TargetLane, utilityLaneData2, (double) utilityLaneData1.m_VisualCapacity < (double) utilityLaneData2.m_VisualCapacity, (double) utilityLaneData1.m_VisualCapacity > (double) utilityLaneData2.m_VisualCapacity);
                }
              }
              else
              {
                ++a.y;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                middleConnection3.m_ConnectPosition.m_Position = this.CalculateUtilityConnectPosition(tempBuffer, new int2(0, a.y - a.x));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                middleConnection3.m_ConnectPosition.m_LaneData.m_Lane = (double) utilityLaneData1.m_VisualCapacity >= (double) utilityLaneData2.m_VisualCapacity ? this.GetConnectionLanePrefab(middleConnection4.m_TargetLane, utilityLaneData2, false, (double) utilityLaneData1.m_VisualCapacity > (double) utilityLaneData2.m_VisualCapacity) : this.GetConnectionLanePrefab(middleConnection3.m_ConnectPosition.m_LaneData.m_Lane, utilityLaneData1, false, false);
              }
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeConnectionLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, middleConnection3, useGroundPosition, isTemp, ownerTemp);
            }
            tempBuffer.Clear();
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((middleConnection1.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
            {
              float num1 = float.MaxValue;
              Entity targetOwner = Entity.Null;
              ushort num2 = 0;
              uint num3 = 0;
              int num4 = index3;
              for (; index3 < middleConnections.Length; ++index3)
              {
                // ISSUE: variable of a compiler-generated type
                LaneSystem.MiddleConnection middleConnection5 = middleConnections[index3];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((int) middleConnection5.m_ConnectPosition.m_GroupIndex == (int) middleConnection1.m_ConnectPosition.m_GroupIndex && middleConnection5.m_IsSource == middleConnection1.m_IsSource)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((middleConnection5.m_TargetFlags & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0 && (double) middleConnection5.m_Distance < (double) num1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    num1 = middleConnection5.m_Distance;
                    // ISSUE: reference to a compiler-generated field
                    targetOwner = middleConnection5.m_TargetOwner;
                    // ISSUE: reference to a compiler-generated field
                    num2 = middleConnection5.m_TargetCarriageway;
                    // ISSUE: reference to a compiler-generated field
                    num3 = middleConnection5.m_TargetGroup;
                  }
                }
                else
                  break;
              }
              --index3;
              for (int index5 = num4; index5 <= index3; ++index5)
              {
                // ISSUE: variable of a compiler-generated type
                LaneSystem.MiddleConnection middleConnection6 = middleConnections[index5];
                // ISSUE: reference to a compiler-generated field
                bool flag = (int) middleConnection6.m_TargetCarriageway == (int) num2;
                if (flag & isRoundabout)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  flag = middleConnection6.m_TargetOwner == targetOwner && (targetOwner != owner || (int) middleConnection6.m_TargetGroup == (int) num3);
                }
                // ISSUE: reference to a compiler-generated field
                if (flag && (middleConnection6.m_TargetFlags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
                {
                  flag = false;
                  for (int index6 = num4; index6 <= index3; ++index6)
                  {
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.MiddleConnection middleConnection7 = middleConnections[index6];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((middleConnection7.m_TargetFlags & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0 && (int) middleConnection7.m_TargetGroup == (int) middleConnection6.m_TargetGroup && (int) middleConnection7.m_TargetCarriageway == (int) num2 && (!isRoundabout || middleConnection7.m_TargetOwner == targetOwner && (targetOwner != owner || (int) middleConnection7.m_TargetGroup == (int) num3)))
                    {
                      flag = true;
                      break;
                    }
                  }
                }
                if (flag)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CreateNodeConnectionLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, middleConnection6, false, isTemp, ownerTemp);
                }
              }
            }
          }
        }
      }

      private void CreateNodeConnectionLane(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        LaneSystem.MiddleConnection middleConnection,
        bool useGroundPosition,
        bool isTemp,
        Temp ownerTemp)
      {
        Owner component1 = new Owner();
        component1.m_Owner = owner;
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PrefabRef component2 = new PrefabRef(middleConnection.m_ConnectPosition.m_LaneData.m_Lane);
        Unity.Mathematics.Random outRandom;
        // ISSUE: reference to a compiler-generated method
        this.CheckPrefab(ref component2.m_Prefab, ref random, out outRandom, laneBuffer);
        NodeLane component3 = new NodeLane();
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData1 = this.m_NetLaneData[component2.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(middleConnection.m_TargetLane))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData2 = this.m_NetLaneData[middleConnection.m_TargetLane];
            component3.m_WidthOffset.x = netLaneData2.m_Width - netLaneData1.m_Width;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(middleConnection.m_ConnectPosition.m_LaneData.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData3 = this.m_NetLaneData[middleConnection.m_ConnectPosition.m_LaneData.m_Lane];
            component3.m_WidthOffset.y = netLaneData3.m_Width - netLaneData1.m_Width;
          }
          // ISSUE: reference to a compiler-generated field
          if (middleConnection.m_IsSource)
            component3.m_WidthOffset = component3.m_WidthOffset.yx;
        }
        Curve curve = new Curve();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float length = math.sqrt(math.distance(middleConnection.m_ConnectPosition.m_Position, MathUtils.Position(middleConnection.m_TargetCurve.m_Bezier, middleConnection.m_TargetCurvePos)));
          // ISSUE: reference to a compiler-generated field
          if (middleConnection.m_IsSource)
          {
            // ISSUE: reference to a compiler-generated field
            Bounds1 t = new Bounds1(middleConnection.m_TargetCurvePos, 1f);
            // ISSUE: reference to a compiler-generated field
            MathUtils.ClampLength(middleConnection.m_TargetCurve.m_Bezier, ref t, ref length);
            // ISSUE: reference to a compiler-generated field
            middleConnection.m_TargetCurvePos = t.max;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Bounds1 t = new Bounds1(0.0f, middleConnection.m_TargetCurvePos);
            // ISSUE: reference to a compiler-generated field
            MathUtils.ClampLengthInverse(middleConnection.m_TargetCurve.m_Bezier, ref t, ref length);
            // ISSUE: reference to a compiler-generated field
            middleConnection.m_TargetCurvePos = t.min;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3 = MathUtils.Position(middleConnection.m_TargetCurve.m_Bezier, middleConnection.m_TargetCurvePos);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 startTangent = MathUtils.Tangent(middleConnection.m_TargetCurve.m_Bezier, middleConnection.m_TargetCurvePos);
          startTangent = MathUtils.Normalize(startTangent, startTangent.xz);
          // ISSUE: reference to a compiler-generated field
          if (middleConnection.m_IsSource)
            startTangent = -startTangent;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          curve.m_Bezier = (double) math.distance(middleConnection.m_ConnectPosition.m_Position, float3) < 0.0099999997764825821 ? NetUtils.StraightCurve(float3, middleConnection.m_ConnectPosition.m_Position) : NetUtils.FitCurve(float3, startTangent, -middleConnection.m_ConnectPosition.m_Tangent, middleConnection.m_ConnectPosition.m_Position);
          // ISSUE: reference to a compiler-generated field
          if (middleConnection.m_IsSource)
            curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
          curve.m_Length = MathUtils.Length(curve.m_Bezier);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 position = MathUtils.Position(middleConnection.m_TargetCurve.m_Bezier, middleConnection.m_TargetCurvePos);
            if (useGroundPosition)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.GetGroundPosition(owner, middleConnection.m_ConnectPosition.m_CurvePosition, ref position);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) math.abs(position.y - middleConnection.m_ConnectPosition.m_Position.y) >= 0.0099999997764825821)
            {
              curve.m_Bezier.a = position;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              curve.m_Bezier.b = math.lerp(position, middleConnection.m_ConnectPosition.m_Position, new float3(0.25f, 0.5f, 0.25f));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              curve.m_Bezier.c = math.lerp(position, middleConnection.m_ConnectPosition.m_Position, new float3(0.75f, 0.5f, 0.75f));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              curve.m_Bezier.d = middleConnection.m_ConnectPosition.m_Position;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              curve.m_Bezier = NetUtils.StraightCurve(position, middleConnection.m_ConnectPosition.m_Position);
            }
            // ISSUE: reference to a compiler-generated field
            if (middleConnection.m_IsSource)
              curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
            curve.m_Length = MathUtils.Length(curve.m_Bezier);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 float3 = MathUtils.Position(middleConnection.m_TargetCurve.m_Bezier, middleConnection.m_TargetCurvePos);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            curve.m_Bezier = NetUtils.StraightCurve(float3, middleConnection.m_ConnectPosition.m_Position);
            // ISSUE: reference to a compiler-generated field
            if (middleConnection.m_IsSource)
              curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            curve.m_Length = math.distance(float3, middleConnection.m_ConnectPosition.m_Position);
          }
        }
        Lane lane = new Lane();
        uint num;
        // ISSUE: reference to a compiler-generated field
        if (middleConnection.m_IsSource)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lane.m_StartNode = new PathNode(middleConnection.m_ConnectPosition.m_Owner, middleConnection.m_ConnectPosition.m_LaneData.m_Index, middleConnection.m_ConnectPosition.m_SegmentIndex);
          lane.m_MiddleNode = new PathNode(owner, (ushort) nodeLaneIndex++);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lane.m_EndNode = new PathNode(owner, middleConnection.m_TargetIndex, middleConnection.m_TargetCurvePos);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = (uint) middleConnection.m_ConnectPosition.m_GroupIndex | middleConnection.m_TargetGroup & 4294901760U;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lane.m_StartNode = new PathNode(owner, middleConnection.m_TargetIndex, middleConnection.m_TargetCurvePos);
          lane.m_MiddleNode = new PathNode(owner, (ushort) nodeLaneIndex++);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lane.m_EndNode = new PathNode(middleConnection.m_ConnectPosition.m_Owner, middleConnection.m_ConnectPosition.m_LaneData.m_Index, middleConnection.m_ConnectPosition.m_SegmentIndex);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = (uint) ((int) middleConnection.m_TargetGroup & (int) ushort.MaxValue | (int) middleConnection.m_ConnectPosition.m_GroupIndex << 16);
        }
        CarLane component4 = new CarLane();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component4.m_DefaultSpeedLimit = middleConnection.m_TargetComposition.m_SpeedLimit;
          component4.m_Curviness = NetUtils.CalculateCurviness(curve, netLaneData1.m_Width);
          // ISSUE: reference to a compiler-generated field
          component4.m_CarriagewayGroup = middleConnection.m_TargetCarriageway;
          component4.m_Flags |= CarLaneFlags.Unsafe | CarLaneFlags.SideConnection;
          // ISSUE: reference to a compiler-generated field
          if (middleConnection.m_IsSource)
            component4.m_Flags |= CarLaneFlags.Yield;
          float3 float3 = MathUtils.StartTangent(curve.m_Bezier);
          float2 x = MathUtils.Right(float3.xz);
          float3 = MathUtils.EndTangent(curve.m_Bezier);
          float2 xz = float3.xz;
          if ((double) math.dot(x, xz) >= 0.0)
            component4.m_Flags |= CarLaneFlags.TurnRight;
          else
            component4.m_Flags |= CarLaneFlags.TurnLeft;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_TargetComposition.m_TaxiwayFlags & TaxiwayFlags.Runway) != (TaxiwayFlags) 0)
            component4.m_Flags |= CarLaneFlags.Runway;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_TargetComposition.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != (Game.Prefabs.RoadFlags) 0)
            component4.m_Flags |= CarLaneFlags.Highway;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          middleConnection.m_ConnectPosition.m_LaneData.m_Flags &= ~(Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          middleConnection.m_ConnectPosition.m_LaneData.m_Flags |= middleConnection.m_TargetFlags & (Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          middleConnection.m_ConnectPosition.m_LaneData.m_Flags &= ~(Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
        }
        PedestrianLane component5 = new PedestrianLane();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          component5.m_Flags |= PedestrianLaneFlags.SideConnection;
        UtilityLane component6 = new UtilityLane();
        PrefabRef componentData1;
        NetData componentData2;
        NetGeometryData componentData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0 && this.m_PrefabRefData.TryGetComponent(middleConnection.m_ConnectPosition.m_Owner, out componentData1) && this.m_PrefabNetData.TryGetComponent(componentData1.m_Prefab, out componentData2) && this.m_PrefabGeometryData.TryGetComponent(componentData1.m_Prefab, out componentData3) && (componentData2.m_RequiredLayers & (Layer.PowerlineLow | Layer.PowerlineHigh | Layer.WaterPipe | Layer.SewagePipe)) != Layer.None && (componentData3.m_Flags & GeometryFlags.Marker) == (GeometryFlags) 0)
          component6.m_Flags |= UtilityLaneFlags.PipelineConnection;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey key = new LaneSystem.LaneKey(lane, component2.m_Prefab, middleConnection.m_ConnectPosition.m_LaneData.m_Flags);
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey laneKey = key;
        if (isTemp)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, middleConnection.m_ConnectPosition.m_Owner);
          // ISSUE: reference to a compiler-generated method
          this.GetOriginalLane(laneBuffer, laneKey, ref temp);
        }
        PseudoRandomSeed componentData4 = new PseudoRandomSeed();
        // ISSUE: reference to a compiler-generated field
        if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0 && !this.m_PseudoRandomSeedData.TryGetComponent(temp.m_Original, out componentData4))
          componentData4 = new PseudoRandomSeed(ref outRandom);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (laneBuffer.m_OldLanes.TryGetValue(key, out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_OldLanes.Remove(key);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity1, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, curve);
          if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PseudoRandomSeedData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, entity1, componentData4);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity1, componentData4);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity1, component4);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity1, component5);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity1, component6);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Updates.Add(in entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_Updates.Add(in entity1);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity1, new MasterLane()
            {
              m_Group = num
            });
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0)
            return;
          SlaveLane component7 = new SlaveLane();
          component7.m_Group = num;
          component7.m_MinIndex = (ushort) 0;
          component7.m_MaxIndex = (ushort) 0;
          component7.m_SubIndex = (ushort) 0;
          component7.m_Flags |= SlaveLaneFlags.AllowChange;
          // ISSUE: reference to a compiler-generated field
          component7.m_Flags |= middleConnection.m_IsSource ? SlaveLaneFlags.MiddleEnd : SlaveLaneFlags.MiddleStart;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity1, component7);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[component2.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EntityArchetype archetype = (middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0 ? ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0 ? laneArchetypeData.m_NodeLaneArchetype : laneArchetypeData.m_NodeMasterArchetype) : laneArchetypeData.m_NodeSlaveArchetype;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, archetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, lane);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity2, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, curve);
          if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, componentData4);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity2, component4);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity2, component5);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity2, component6);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity2, new MasterLane()
            {
              m_Group = num
            });
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
          {
            SlaveLane component8 = new SlaveLane();
            component8.m_Group = num;
            component8.m_MinIndex = (ushort) 0;
            component8.m_MaxIndex = (ushort) 0;
            component8.m_SubIndex = (ushort) 0;
            component8.m_Flags |= SlaveLaneFlags.AllowChange;
            // ISSUE: reference to a compiler-generated field
            component8.m_Flags |= middleConnection.m_IsSource ? SlaveLaneFlags.MiddleEnd : SlaveLaneFlags.MiddleStart;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity2, component8);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_TempOwnerTypes);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity2, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity2, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component1);
          }
        }
      }

      private void CreateEdgeConnectionLane(
        int jobIndex,
        ref int edgeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        Segment startSegment,
        Segment endSegment,
        NetCompositionData prefabCompositionData,
        NetGeometryData prefabGeometryData,
        LaneSystem.CompositionData compositionData,
        NetCompositionLane prefabCompositionLaneData,
        LaneSystem.ConnectPosition connectPosition,
        int connectionIndex,
        bool useGroundPosition,
        bool isSource,
        bool isTemp,
        Temp ownerTemp)
      {
        float t1 = (float) ((double) prefabCompositionLaneData.m_Position.x / (double) math.max(1f, prefabCompositionData.m_Width) + 0.5);
        Owner component1 = new Owner();
        component1.m_Owner = owner;
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        // ISSUE: reference to a compiler-generated field
        PrefabRef component2 = new PrefabRef(connectPosition.m_LaneData.m_Lane);
        Unity.Mathematics.Random outRandom;
        // ISSUE: reference to a compiler-generated method
        this.CheckPrefab(ref component2.m_Prefab, ref random, out outRandom, laneBuffer);
        bool flag1 = (prefabGeometryData.m_Flags & (GeometryFlags.StraightEdges | GeometryFlags.SmoothSlopes)) == GeometryFlags.StraightEdges;
        bool flag2 = false;
        Bezier4x3 curve1 = new Bezier4x3();
        Bezier4x3 curve2 = new Bezier4x3();
        Bezier4x3 curve3;
        byte segmentIndex;
        float t2;
        if (flag1)
        {
          curve3 = MathUtils.Lerp(MathUtils.Join(startSegment.m_Left, endSegment.m_Left), MathUtils.Join(startSegment.m_Right, endSegment.m_Right), t1);
          curve3.a.y += prefabCompositionLaneData.m_Position.y;
          curve3.b.y += prefabCompositionLaneData.m_Position.y;
          curve3.c.y += prefabCompositionLaneData.m_Position.y;
          curve3.d.y += prefabCompositionLaneData.m_Position.y;
          segmentIndex = (byte) 2;
          // ISSUE: reference to a compiler-generated field
          double num = (double) MathUtils.Distance(curve3, connectPosition.m_Position, out t2);
        }
        else
        {
          curve1 = MathUtils.Lerp(startSegment.m_Left, startSegment.m_Right, t1);
          curve2 = MathUtils.Lerp(endSegment.m_Left, endSegment.m_Right, t1);
          curve1.a.y += prefabCompositionLaneData.m_Position.y;
          curve1.b.y += prefabCompositionLaneData.m_Position.y;
          curve1.c.y += prefabCompositionLaneData.m_Position.y;
          curve1.d.y += prefabCompositionLaneData.m_Position.y;
          curve2.a.y += prefabCompositionLaneData.m_Position.y;
          curve2.b.y += prefabCompositionLaneData.m_Position.y;
          curve2.c.y += prefabCompositionLaneData.m_Position.y;
          curve2.d.y += prefabCompositionLaneData.m_Position.y;
          if ((prefabCompositionLaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Parking | Game.Prefabs.LaneFlags.Track)) != (Game.Prefabs.LaneFlags) 0 && (prefabCompositionLaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
          {
            curve1 = MathUtils.Invert(curve1);
            curve2 = MathUtils.Invert(curve2);
            flag2 = !flag2;
          }
          float t3;
          // ISSUE: reference to a compiler-generated field
          float num = MathUtils.Distance(curve1, connectPosition.m_Position, out t3);
          float t4;
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.Distance(curve2, connectPosition.m_Position, out t4) < (double) num)
          {
            curve3 = curve2;
            segmentIndex = (byte) 3;
            t2 = t4;
            flag2 = !flag2;
          }
          else
          {
            curve3 = curve1;
            segmentIndex = (byte) 1;
            t2 = t3;
          }
        }
        NodeLane component3 = new NodeLane();
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData1 = this.m_NetLaneData[component2.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(prefabCompositionLaneData.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData2 = this.m_NetLaneData[prefabCompositionLaneData.m_Lane];
            component3.m_WidthOffset.x = netLaneData2.m_Width - netLaneData1.m_Width;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(connectPosition.m_LaneData.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData3 = this.m_NetLaneData[connectPosition.m_LaneData.m_Lane];
            component3.m_WidthOffset.y = netLaneData3.m_Width - netLaneData1.m_Width;
          }
          if (isSource)
            component3.m_WidthOffset = component3.m_WidthOffset.yx;
        }
        Curve curve4 = new Curve();
        // ISSUE: reference to a compiler-generated field
        if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          float num = math.sqrt(math.distance(connectPosition.m_Position, MathUtils.Position(curve3, t2)));
          float length1 = num;
          if (isSource)
          {
            Bounds1 t5 = new Bounds1(t2, 1f);
            if (!MathUtils.ClampLength(curve3, ref t5, ref length1) && !flag1 && !flag2)
            {
              float length2 = num - length1;
              if (segmentIndex == (byte) 1)
              {
                curve3 = curve2;
                segmentIndex = (byte) 3;
              }
              else
              {
                curve3 = curve1;
                segmentIndex = (byte) 1;
              }
              t5 = new Bounds1(0.0f, 1f);
              MathUtils.ClampLength(curve3, ref t5, ref length2);
            }
            t2 = t5.max;
          }
          else
          {
            Bounds1 t6 = new Bounds1(0.0f, t2);
            if (!MathUtils.ClampLengthInverse(curve3, ref t6, ref length1) && !flag1 & flag2)
            {
              float length3 = num - length1;
              if (segmentIndex == (byte) 1)
              {
                curve3 = curve2;
                segmentIndex = (byte) 3;
              }
              else
              {
                curve3 = curve1;
                segmentIndex = (byte) 1;
              }
              t6 = new Bounds1(0.0f, 1f);
              MathUtils.ClampLengthInverse(curve3, ref t6, ref length3);
            }
            t2 = t6.min;
          }
          float3 float3_1 = MathUtils.Position(curve3, t2);
          float3 float3_2 = MathUtils.Tangent(curve3, t2);
          float3 startTangent = MathUtils.Normalize(float3_2, float3_2.xz);
          if (isSource)
            startTangent = -startTangent;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          curve4.m_Bezier = (double) math.distance(connectPosition.m_Position, float3_1) < 0.0099999997764825821 ? NetUtils.StraightCurve(float3_1, connectPosition.m_Position) : NetUtils.FitCurve(float3_1, startTangent, -connectPosition.m_Tangent, connectPosition.m_Position);
          if (isSource)
            curve4.m_Bezier = MathUtils.Invert(curve4.m_Bezier);
          curve4.m_Length = MathUtils.Length(curve4.m_Bezier);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            float3 position = MathUtils.Position(curve3, t2);
            if (useGroundPosition)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.GetGroundPosition(owner, connectPosition.m_CurvePosition, ref position);
            }
            // ISSUE: reference to a compiler-generated field
            if ((double) math.abs(position.y - connectPosition.m_Position.y) >= 0.0099999997764825821)
            {
              curve4.m_Bezier.a = position;
              // ISSUE: reference to a compiler-generated field
              curve4.m_Bezier.b = math.lerp(position, connectPosition.m_Position, new float3(0.25f, 0.5f, 0.25f));
              // ISSUE: reference to a compiler-generated field
              curve4.m_Bezier.c = math.lerp(position, connectPosition.m_Position, new float3(0.75f, 0.5f, 0.75f));
              // ISSUE: reference to a compiler-generated field
              curve4.m_Bezier.d = connectPosition.m_Position;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              curve4.m_Bezier = NetUtils.StraightCurve(position, connectPosition.m_Position);
            }
            if (isSource)
              curve4.m_Bezier = MathUtils.Invert(curve4.m_Bezier);
            curve4.m_Length = MathUtils.Length(curve4.m_Bezier);
          }
          else
          {
            float3 float3 = MathUtils.Position(curve3, t2);
            // ISSUE: reference to a compiler-generated field
            curve4.m_Bezier = NetUtils.StraightCurve(float3, connectPosition.m_Position);
            if (isSource)
              curve4.m_Bezier = MathUtils.Invert(curve4.m_Bezier);
            // ISSUE: reference to a compiler-generated field
            curve4.m_Length = math.distance(float3, connectPosition.m_Position);
          }
        }
        Lane lane = new Lane();
        if (isSource)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lane.m_StartNode = new PathNode(connectPosition.m_Owner, connectPosition.m_LaneData.m_Index, connectPosition.m_SegmentIndex);
          lane.m_MiddleNode = new PathNode(owner, (ushort) edgeLaneIndex--);
          lane.m_EndNode = new PathNode(owner, prefabCompositionLaneData.m_Index, segmentIndex, t2);
        }
        else
        {
          lane.m_StartNode = new PathNode(owner, prefabCompositionLaneData.m_Index, segmentIndex, t2);
          lane.m_MiddleNode = new PathNode(owner, (ushort) edgeLaneIndex--);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lane.m_EndNode = new PathNode(connectPosition.m_Owner, connectPosition.m_LaneData.m_Index, connectPosition.m_SegmentIndex);
        }
        CarLane component4 = new CarLane();
        // ISSUE: reference to a compiler-generated field
        if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          component4.m_DefaultSpeedLimit = compositionData.m_SpeedLimit;
          component4.m_Curviness = NetUtils.CalculateCurviness(curve4, netLaneData1.m_Width);
          component4.m_CarriagewayGroup = (ushort) ((uint) segmentIndex << 8 | (uint) prefabCompositionLaneData.m_Carriageway);
          component4.m_Flags |= CarLaneFlags.Unsafe | CarLaneFlags.SideConnection;
          if (isSource)
            component4.m_Flags |= CarLaneFlags.Yield;
          if ((double) math.dot(MathUtils.Right(MathUtils.StartTangent(curve4.m_Bezier).xz), MathUtils.EndTangent(curve4.m_Bezier).xz) >= 0.0)
            component4.m_Flags |= CarLaneFlags.TurnRight;
          else
            component4.m_Flags |= CarLaneFlags.TurnLeft;
          // ISSUE: reference to a compiler-generated field
          if ((compositionData.m_TaxiwayFlags & TaxiwayFlags.Runway) != (TaxiwayFlags) 0)
            component4.m_Flags |= CarLaneFlags.Runway;
          // ISSUE: reference to a compiler-generated field
          if ((compositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != (Game.Prefabs.RoadFlags) 0)
            component4.m_Flags |= CarLaneFlags.Highway;
          // ISSUE: reference to a compiler-generated field
          connectPosition.m_LaneData.m_Flags |= prefabCompositionLaneData.m_Flags & (Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          connectPosition.m_LaneData.m_Flags &= ~(Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
        }
        PedestrianLane component5 = new PedestrianLane();
        // ISSUE: reference to a compiler-generated field
        if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          component5.m_Flags |= PedestrianLaneFlags.SideConnection;
        UtilityLane component6 = new UtilityLane();
        PrefabRef componentData1;
        NetData componentData2;
        NetGeometryData componentData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0 && this.m_PrefabRefData.TryGetComponent(connectPosition.m_Owner, out componentData1) && this.m_PrefabNetData.TryGetComponent(componentData1.m_Prefab, out componentData2) && this.m_PrefabGeometryData.TryGetComponent(componentData1.m_Prefab, out componentData3) && (componentData2.m_RequiredLayers & (Layer.PowerlineLow | Layer.PowerlineHigh | Layer.WaterPipe | Layer.SewagePipe)) != Layer.None && (componentData3.m_Flags & GeometryFlags.Marker) == (GeometryFlags) 0)
          component6.m_Flags |= UtilityLaneFlags.PipelineConnection;
        uint num1 = (uint) prefabCompositionLaneData.m_Group | (uint) (connectionIndex + 4 << 8);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey key = new LaneSystem.LaneKey(lane, component2.m_Prefab, connectPosition.m_LaneData.m_Flags);
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey laneKey = key;
        if (isTemp)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, connectPosition.m_Owner);
          // ISSUE: reference to a compiler-generated method
          this.GetOriginalLane(laneBuffer, laneKey, ref temp);
        }
        PseudoRandomSeed componentData4 = new PseudoRandomSeed();
        // ISSUE: reference to a compiler-generated field
        if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0 && !this.m_PseudoRandomSeedData.TryGetComponent(temp.m_Original, out componentData4))
          componentData4 = new PseudoRandomSeed(ref outRandom);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (laneBuffer.m_OldLanes.TryGetValue(key, out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_OldLanes.Remove(key);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity1, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, curve4);
          if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PseudoRandomSeedData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, entity1, componentData4);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity1, componentData4);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity1, component4);
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity1, component5);
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity1, component6);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Updates.Add(in entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_Updates.Add(in entity1);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity1, new MasterLane()
            {
              m_Group = num1
            });
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0)
            return;
          SlaveLane component7 = new SlaveLane();
          component7.m_Group = num1;
          component7.m_MinIndex = (ushort) prefabCompositionLaneData.m_Index;
          component7.m_MaxIndex = (ushort) prefabCompositionLaneData.m_Index;
          component7.m_SubIndex = (ushort) prefabCompositionLaneData.m_Index;
          component7.m_Flags |= SlaveLaneFlags.AllowChange;
          component7.m_Flags |= isSource ? SlaveLaneFlags.MiddleEnd : SlaveLaneFlags.MiddleStart;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity1, component7);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[component2.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EntityArchetype archetype = (connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0 ? ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0 ? laneArchetypeData.m_NodeLaneArchetype : laneArchetypeData.m_NodeMasterArchetype) : laneArchetypeData.m_NodeSlaveArchetype;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, archetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, lane);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity2, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, curve4);
          if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, componentData4);
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity2, component4);
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity2, component5);
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity2, component6);
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity2, new MasterLane()
            {
              m_Group = num1
            });
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
          {
            SlaveLane component8 = new SlaveLane();
            component8.m_Group = num1;
            component8.m_MinIndex = (ushort) prefabCompositionLaneData.m_Index;
            component8.m_MaxIndex = (ushort) prefabCompositionLaneData.m_Index;
            component8.m_SubIndex = (ushort) prefabCompositionLaneData.m_Index;
            component8.m_Flags |= SlaveLaneFlags.AllowChange;
            component8.m_Flags |= isSource ? SlaveLaneFlags.MiddleEnd : SlaveLaneFlags.MiddleStart;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity2, component8);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_TempOwnerTypes);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity2, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity2, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component1);
          }
        }
      }

      private bool FindAnchor(
        ref float3 position,
        ref PathNode pathNode,
        Entity prefab,
        NativeList<LaneSystem.LaneAnchor> anchors)
      {
        if (!anchors.IsCreated)
          return false;
        for (int index = 0; index < anchors.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneAnchor anchor = anchors[index];
          // ISSUE: reference to a compiler-generated field
          if (anchor.m_Prefab == prefab)
          {
            // ISSUE: reference to a compiler-generated field
            int num = !anchor.m_PathNode.Equals(pathNode) ? 1 : 0;
            if (num != 0)
            {
              // ISSUE: reference to a compiler-generated field
              position = anchor.m_Position;
              // ISSUE: reference to a compiler-generated field
              pathNode = anchor.m_PathNode;
            }
            // ISSUE: reference to a compiler-generated field
            anchor.m_Prefab = Entity.Null;
            anchors[index] = anchor;
            return num != 0;
          }
        }
        return false;
      }

      private void CreateEdgeLane(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        Segment segment,
        NetCompositionData prefabCompositionData,
        LaneSystem.CompositionData compositionData,
        DynamicBuffer<NetCompositionLane> prefabCompositionLanes,
        NetCompositionLane prefabCompositionLaneData,
        int2 segmentIndex,
        float2 edgeDelta,
        NativeList<LaneSystem.LaneAnchor> startAnchors,
        NativeList<LaneSystem.LaneAnchor> endAnchors,
        bool2 canAnchor,
        bool isTemp,
        Temp ownerTemp)
      {
        Game.Prefabs.LaneFlags flags = prefabCompositionLaneData.m_Flags;
        float t = (float) ((double) prefabCompositionLaneData.m_Position.x / (double) math.max(1f, prefabCompositionData.m_Width) + 0.5);
        Owner component1 = new Owner();
        component1.m_Owner = owner;
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        Lane laneData = new Lane();
        int segmentIndex1 = math.csum(segmentIndex) / 2;
        laneData.m_StartNode = new PathNode(owner, prefabCompositionLaneData.m_Index, (byte) segmentIndex.x);
        laneData.m_MiddleNode = new PathNode(owner, prefabCompositionLaneData.m_Index, (byte) segmentIndex1);
        laneData.m_EndNode = new PathNode(owner, prefabCompositionLaneData.m_Index, (byte) segmentIndex.y);
        PrefabRef component2 = new PrefabRef(prefabCompositionLaneData.m_Lane);
        if ((flags & (Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track)) == (Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track))
        {
          flags &= ~Game.Prefabs.LaneFlags.Track;
          // ISSUE: reference to a compiler-generated field
          component2.m_Prefab = this.m_CarLaneData[component2.m_Prefab].m_NotTrackLanePrefab;
        }
        Unity.Mathematics.Random outRandom;
        // ISSUE: reference to a compiler-generated method
        this.CheckPrefab(ref component2.m_Prefab, ref random, out outRandom, laneBuffer);
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData = this.m_NetLaneData[component2.m_Prefab];
        EdgeLane edgeLaneData = new EdgeLane();
        edgeLaneData.m_EdgeDelta = edgeDelta;
        Curve curveData = new Curve();
        curveData.m_Bezier = MathUtils.Lerp(segment.m_Left, segment.m_Right, t);
        curveData.m_Bezier.a.y += prefabCompositionLaneData.m_Position.y;
        curveData.m_Bezier.b.y += prefabCompositionLaneData.m_Position.y;
        curveData.m_Bezier.c.y += prefabCompositionLaneData.m_Position.y;
        curveData.m_Bezier.d.y += prefabCompositionLaneData.m_Position.y;
        bool2 bool2 = (bool2) false;
        if ((flags & Game.Prefabs.LaneFlags.FindAnchor) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          bool2.x = canAnchor.x && !this.FindAnchor(ref curveData.m_Bezier.a, ref laneData.m_StartNode, prefabCompositionLaneData.m_Lane, startAnchors);
          // ISSUE: reference to a compiler-generated method
          bool2.y = canAnchor.y && !this.FindAnchor(ref curveData.m_Bezier.d, ref laneData.m_EndNode, prefabCompositionLaneData.m_Lane, endAnchors);
        }
        UtilityLane component3 = new UtilityLane();
        HangingLane component4 = new HangingLane();
        bool flag1 = false;
        if ((flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          UtilityLaneData utilityLaneData = this.m_UtilityLaneData[prefabCompositionLaneData.m_Lane];
          if ((double) utilityLaneData.m_Hanging != 0.0)
          {
            curveData.m_Bezier.b = math.lerp(curveData.m_Bezier.a, curveData.m_Bezier.d, 0.333333343f);
            curveData.m_Bezier.c = math.lerp(curveData.m_Bezier.a, curveData.m_Bezier.d, 0.6666667f);
            float num = (float) ((double) math.distance(curveData.m_Bezier.a.xz, curveData.m_Bezier.d.xz) * (double) utilityLaneData.m_Hanging * 1.3333333730697632);
            curveData.m_Bezier.b.y -= num;
            curveData.m_Bezier.c.y -= num;
            component4.m_Distances = (float2) 0.1f;
            if ((flags & Game.Prefabs.LaneFlags.FindAnchor) != (Game.Prefabs.LaneFlags) 0)
              component4.m_Distances = math.select(component4.m_Distances, (float2) 0.0f, canAnchor);
            flag1 = true;
          }
          if (bool2.x)
            component3.m_Flags |= UtilityLaneFlags.SecondaryStartAnchor;
          if (bool2.y)
            component3.m_Flags |= UtilityLaneFlags.SecondaryEndAnchor;
        }
        curveData.m_Length = MathUtils.Length(curveData.m_Bezier);
        if ((flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Parking | Game.Prefabs.LaneFlags.Track)) != (Game.Prefabs.LaneFlags) 0 && (flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          LaneSystem.UpdateLanesJob.Invert(ref laneData, ref curveData, ref edgeLaneData);
        }
        CarLane component5 = new CarLane();
        if ((flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          component5.m_DefaultSpeedLimit = compositionData.m_SpeedLimit;
          component5.m_Curviness = NetUtils.CalculateCurviness(curveData, netLaneData.m_Width);
          component5.m_CarriagewayGroup = (ushort) ((uint) (segmentIndex.x << 8) | (uint) prefabCompositionLaneData.m_Carriageway);
          if ((flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= CarLaneFlags.Invert;
          if ((flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= CarLaneFlags.Twoway;
          if ((flags & Game.Prefabs.LaneFlags.PublicOnly) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= CarLaneFlags.PublicOnly;
          // ISSUE: reference to a compiler-generated field
          if ((compositionData.m_TaxiwayFlags & TaxiwayFlags.Runway) != (TaxiwayFlags) 0)
            component5.m_Flags |= CarLaneFlags.Runway;
          // ISSUE: reference to a compiler-generated field
          if ((compositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != (Game.Prefabs.RoadFlags) 0)
            component5.m_Flags |= CarLaneFlags.Highway;
          if ((flags & Game.Prefabs.LaneFlags.LeftLimit) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= CarLaneFlags.LeftLimit;
          if ((flags & Game.Prefabs.LaneFlags.RightLimit) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= CarLaneFlags.RightLimit;
          if ((flags & Game.Prefabs.LaneFlags.ParkingLeft) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= CarLaneFlags.ParkingLeft;
          if ((flags & Game.Prefabs.LaneFlags.ParkingRight) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= CarLaneFlags.ParkingRight;
        }
        TrackLane component6 = new TrackLane();
        if ((flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          component6.m_SpeedLimit = compositionData.m_SpeedLimit;
          component6.m_Curviness = NetUtils.CalculateCurviness(curveData, netLaneData.m_Width);
          component6.m_Flags |= TrackLaneFlags.AllowMiddle;
          if ((flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
            component6.m_Flags |= TrackLaneFlags.Invert;
          if ((flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
            component6.m_Flags |= TrackLaneFlags.Twoway;
          if ((flags & Game.Prefabs.LaneFlags.Road) == (Game.Prefabs.LaneFlags) 0)
            component6.m_Flags |= TrackLaneFlags.Exclusive;
          if (((prefabCompositionData.m_Flags.m_Left | prefabCompositionData.m_Flags.m_Right) & (CompositionFlags.Side.PrimaryStop | CompositionFlags.Side.SecondaryStop)) != (CompositionFlags.Side) 0)
            component6.m_Flags |= TrackLaneFlags.Station;
        }
        ParkingLane component7 = new ParkingLane();
        if ((flags & Game.Prefabs.LaneFlags.Parking) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          NetCompositionLane closestLane1 = this.FindClosestLane(prefabCompositionLanes, Game.Prefabs.LaneFlags.Road, Game.Prefabs.LaneFlags.Slave, prefabCompositionLaneData.m_Position);
          // ISSUE: reference to a compiler-generated method
          NetCompositionLane closestLane2 = this.FindClosestLane(prefabCompositionLanes, Game.Prefabs.LaneFlags.Pedestrian, (Game.Prefabs.LaneFlags) 0, prefabCompositionLaneData.m_Position);
          if (closestLane1.m_Lane != Entity.Null)
          {
            laneData.m_StartNode = new PathNode(owner, closestLane1.m_Index, (byte) segmentIndex1, 0.5f);
            if ((flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              NetCompositionLane closestLane3 = this.FindClosestLane(prefabCompositionLanes, Game.Prefabs.LaneFlags.Road | ~closestLane1.m_Flags & Game.Prefabs.LaneFlags.Invert, Game.Prefabs.LaneFlags.Slave | closestLane1.m_Flags & Game.Prefabs.LaneFlags.Invert, closestLane1.m_Position, (int) closestLane1.m_Carriageway);
              if (closestLane3.m_Lane != Entity.Null)
              {
                component7.m_SecondaryStartNode = new PathNode(owner, closestLane3.m_Index, (byte) segmentIndex1, 0.5f);
                component7.m_Flags |= ParkingLaneFlags.SecondaryStart;
              }
            }
          }
          if (closestLane2.m_Lane != Entity.Null)
            laneData.m_EndNode = new PathNode(owner, closestLane2.m_Index, (byte) segmentIndex1, 0.5f);
          if ((flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
            component7.m_Flags |= ParkingLaneFlags.Invert;
          if ((flags & Game.Prefabs.LaneFlags.Virtual) != (Game.Prefabs.LaneFlags) 0)
            component7.m_Flags |= ParkingLaneFlags.VirtualLane;
          if ((flags & Game.Prefabs.LaneFlags.PublicOnly) != (Game.Prefabs.LaneFlags) 0)
            component7.m_Flags |= ParkingLaneFlags.SpecialVehicles;
          if ((double) edgeLaneData.m_EdgeDelta.x == 0.0 || (double) edgeLaneData.m_EdgeDelta.x == 1.0)
            component7.m_Flags |= ParkingLaneFlags.StartingLane;
          if ((double) edgeLaneData.m_EdgeDelta.y == 0.0 || (double) edgeLaneData.m_EdgeDelta.y == 1.0)
            component7.m_Flags |= ParkingLaneFlags.EndingLane;
          if ((double) prefabCompositionLaneData.m_Position.x >= 0.0)
            component7.m_Flags |= ParkingLaneFlags.RightSide;
          else
            component7.m_Flags |= ParkingLaneFlags.LeftSide;
          if ((flags & Game.Prefabs.LaneFlags.ParkingLeft) != (Game.Prefabs.LaneFlags) 0)
            component7.m_Flags |= ParkingLaneFlags.ParkingLeft;
          if ((flags & Game.Prefabs.LaneFlags.ParkingRight) != (Game.Prefabs.LaneFlags) 0)
            component7.m_Flags |= ParkingLaneFlags.ParkingRight;
        }
        PedestrianLane component8 = new PedestrianLane();
        if ((flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
        {
          component8.m_Flags |= PedestrianLaneFlags.AllowMiddle;
          if ((flags & Game.Prefabs.LaneFlags.OnWater) != (Game.Prefabs.LaneFlags) 0)
            component8.m_Flags |= PedestrianLaneFlags.OnWater;
        }
        uint num1 = (uint) prefabCompositionLaneData.m_Group | (uint) (segmentIndex.x << 8);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey key = new LaneSystem.LaneKey(laneData, component2.m_Prefab, flags);
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey laneKey = key;
        if (isTemp)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, owner);
          // ISSUE: reference to a compiler-generated method
          this.GetOriginalLane(laneBuffer, laneKey, ref temp);
        }
        PseudoRandomSeed componentData = new PseudoRandomSeed();
        // ISSUE: reference to a compiler-generated field
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0 && !this.m_PseudoRandomSeedData.TryGetComponent(temp.m_Original, out componentData))
          componentData = new PseudoRandomSeed(ref outRandom);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        bool flag2 = laneBuffer.m_OldLanes.TryGetValue(key, out entity1);
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[entity1];
          if ((double) math.dot(curve.m_Bezier.d - curve.m_Bezier.a, curveData.m_Bezier.d - curveData.m_Bezier.a) < 0.0)
            flag2 = false;
        }
        // ISSUE: reference to a compiler-generated field
        bool flag3 = this.m_PrefabData.IsComponentEnabled(component2.m_Prefab);
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_OldLanes.Remove(key);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity1, laneData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<EdgeLane>(jobIndex, entity1, edgeLaneData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, curveData);
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PseudoRandomSeedData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, entity1, componentData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity1, componentData);
            }
          }
          if (flag3)
          {
            if ((flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity1, component5);
            }
            if ((flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<TrackLane>(jobIndex, entity1, component6);
            }
            if ((flags & Game.Prefabs.LaneFlags.Parking) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<ParkingLane>(jobIndex, entity1, component7);
            }
            if ((flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity1, component8);
            }
            if ((flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity1, component3);
            }
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<HangingLane>(jobIndex, entity1, component4);
            }
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Updates.Add(in entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_Updates.Add(in entity1);
            }
          }
          if ((flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity1, new MasterLane()
            {
              m_Group = num1
            });
          }
          if ((flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0)
            return;
          SlaveLane component9 = new SlaveLane();
          component9.m_Group = num1;
          component9.m_MinIndex = (ushort) prefabCompositionLaneData.m_Index;
          component9.m_MaxIndex = (ushort) prefabCompositionLaneData.m_Index;
          component9.m_SubIndex = (ushort) prefabCompositionLaneData.m_Index;
          component9.m_Flags |= SlaveLaneFlags.AllowChange;
          if ((flags & Game.Prefabs.LaneFlags.DisconnectedStart) != (Game.Prefabs.LaneFlags) 0)
            component9.m_Flags |= SlaveLaneFlags.StartingLane;
          if ((flags & Game.Prefabs.LaneFlags.DisconnectedEnd) != (Game.Prefabs.LaneFlags) 0)
            component9.m_Flags |= SlaveLaneFlags.EndingLane;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity1, component9);
        }
        else
        {
          EntityArchetype entityArchetype = new EntityArchetype();
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[component2.m_Prefab];
          EntityArchetype archetype = (flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0 ? ((flags & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0 ? laneArchetypeData.m_EdgeLaneArchetype : laneArchetypeData.m_EdgeMasterArchetype) : laneArchetypeData.m_EdgeSlaveArchetype;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, archetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, laneData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<EdgeLane>(jobIndex, entity2, edgeLaneData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, curveData);
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, componentData);
          }
          if (flag3)
          {
            if ((flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity2, component5);
            }
            if ((flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<TrackLane>(jobIndex, entity2, component6);
            }
            if ((flags & Game.Prefabs.LaneFlags.Parking) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<ParkingLane>(jobIndex, entity2, component7);
            }
            if ((flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity2, component8);
            }
            if ((flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity2, component3);
            }
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HangingLane>(jobIndex, entity2, component4);
            }
          }
          if ((flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity2, new MasterLane()
            {
              m_Group = num1
            });
          }
          if ((flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
          {
            SlaveLane component10 = new SlaveLane();
            component10.m_Group = num1;
            component10.m_MinIndex = (ushort) prefabCompositionLaneData.m_Index;
            component10.m_MaxIndex = (ushort) prefabCompositionLaneData.m_Index;
            component10.m_SubIndex = (ushort) prefabCompositionLaneData.m_Index;
            component10.m_Flags |= SlaveLaneFlags.AllowChange;
            if ((flags & Game.Prefabs.LaneFlags.DisconnectedStart) != (Game.Prefabs.LaneFlags) 0)
              component10.m_Flags |= SlaveLaneFlags.StartingLane;
            if ((flags & Game.Prefabs.LaneFlags.DisconnectedEnd) != (Game.Prefabs.LaneFlags) 0)
              component10.m_Flags |= SlaveLaneFlags.EndingLane;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity2, component10);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_TempOwnerTypes);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity2, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity2, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component1);
          }
        }
      }

      private void CreateObjectLane(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        Entity original,
        LaneSystem.LaneBuffer laneBuffer,
        Game.Objects.Transform transform,
        Game.Prefabs.SubLane prefabSubLane,
        int laneIndex,
        bool sampleTerrain,
        bool cutForTraffic,
        bool isTemp,
        Temp ownerTemp,
        NativeList<ClearAreaData> clearAreas)
      {
        if (original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          prefabSubLane.m_Prefab = this.m_PrefabRefData[original].m_Prefab;
        }
        Owner component1 = new Owner();
        component1.m_Owner = owner;
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        Lane laneData = new Lane();
        if (original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          laneData = this.m_LaneData[original];
        }
        else
        {
          laneData.m_StartNode = new PathNode(owner, (ushort) prefabSubLane.m_NodeIndex.x);
          laneData.m_MiddleNode = new PathNode(owner, (ushort) ((int) ushort.MaxValue - laneIndex));
          laneData.m_EndNode = new PathNode(owner, (ushort) prefabSubLane.m_NodeIndex.y);
        }
        PrefabRef component2 = new PrefabRef(prefabSubLane.m_Prefab);
        Unity.Mathematics.Random outRandom = random;
        Curve curveData = new Curve();
        if (original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          curveData = this.m_CurveData[original];
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckPrefab(ref component2.m_Prefab, ref random, out outRandom, laneBuffer);
          curveData.m_Bezier.a = ObjectUtils.LocalToWorld(transform, prefabSubLane.m_Curve.a);
          curveData.m_Bezier.b = ObjectUtils.LocalToWorld(transform, prefabSubLane.m_Curve.b);
          curveData.m_Bezier.c = ObjectUtils.LocalToWorld(transform, prefabSubLane.m_Curve.c);
          curveData.m_Bezier.d = ObjectUtils.LocalToWorld(transform, prefabSubLane.m_Curve.d);
        }
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData = this.m_NetLaneData[component2.m_Prefab];
        UtilityLane component3 = new UtilityLane();
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          UtilityLaneData utilityLaneData = this.m_UtilityLaneData[prefabSubLane.m_Prefab];
          if (cutForTraffic)
            component3.m_Flags |= UtilityLaneFlags.CutForTraffic;
          if ((double) utilityLaneData.m_Hanging != 0.0)
          {
            curveData.m_Bezier.b = math.lerp(curveData.m_Bezier.a, curveData.m_Bezier.d, 0.333333343f);
            curveData.m_Bezier.c = math.lerp(curveData.m_Bezier.a, curveData.m_Bezier.d, 0.6666667f);
            float num = (float) ((double) math.distance(curveData.m_Bezier.a.xz, curveData.m_Bezier.d.xz) * (double) utilityLaneData.m_Hanging * 1.3333333730697632);
            curveData.m_Bezier.b.y -= num;
            curveData.m_Bezier.c.y -= num;
          }
        }
        bool2 bool2 = (bool2) false;
        float2 elevation = (float2) 0.0f;
        if (original != Entity.Null)
        {
          Elevation componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.TryGetComponent(original, out componentData))
          {
            bool2 = componentData.m_Elevation != float.MinValue;
            elevation = componentData.m_Elevation;
          }
        }
        else
        {
          bool2 = prefabSubLane.m_ParentMesh >= 0;
          elevation = math.select((float2) float.MinValue, new float2(prefabSubLane.m_Curve.a.y, prefabSubLane.m_Curve.d.y), bool2);
        }
        if (ClearAreaHelpers.ShouldClear(clearAreas, curveData.m_Bezier, !math.all(bool2)))
          return;
        if (sampleTerrain && !math.all(bool2))
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve = NetUtils.AdjustPosition(curveData, bool2.x, math.any(bool2), bool2.y, ref this.m_TerrainHeightData);
          Bezier4x1 y = curve.m_Bezier.y;
          float4 abcd1 = y.abcd;
          y = curveData.m_Bezier.y;
          float4 abcd2 = y.abcd;
          if (math.any(math.abs(abcd1 - abcd2) >= 0.01f))
            curveData = curve;
        }
        curveData.m_Length = MathUtils.Length(curveData.m_Bezier);
        if (original == Entity.Null && (netLaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Parking | Game.Prefabs.LaneFlags.Track)) != (Game.Prefabs.LaneFlags) 0 && (netLaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
        {
          EdgeLane edgeLaneData = new EdgeLane();
          // ISSUE: reference to a compiler-generated method
          LaneSystem.UpdateLanesJob.Invert(ref laneData, ref curveData, ref edgeLaneData);
        }
        CarLane component4 = new CarLane();
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
        {
          component4.m_DefaultSpeedLimit = 3f;
          component4.m_Curviness = NetUtils.CalculateCurviness(curveData, netLaneData.m_Width);
          component4.m_CarriagewayGroup = (ushort) laneIndex;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
            component4.m_Flags |= CarLaneFlags.Invert;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
            component4.m_Flags |= CarLaneFlags.Twoway;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PublicOnly) != (Game.Prefabs.LaneFlags) 0)
            component4.m_Flags |= CarLaneFlags.PublicOnly;
        }
        TrackLane component5 = new TrackLane();
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
        {
          component5.m_SpeedLimit = 3f;
          component5.m_Curviness = NetUtils.CalculateCurviness(curveData, netLaneData.m_Width);
          component5.m_Flags |= TrackLaneFlags.AllowMiddle | TrackLaneFlags.Station;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= TrackLaneFlags.Invert;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= TrackLaneFlags.Twoway;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Road) == (Game.Prefabs.LaneFlags) 0)
            component5.m_Flags |= TrackLaneFlags.Exclusive;
        }
        ParkingLane component6 = new ParkingLane();
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Parking) != (Game.Prefabs.LaneFlags) 0)
        {
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) != (Game.Prefabs.LaneFlags) 0)
            component6.m_Flags |= ParkingLaneFlags.Invert;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Virtual) != (Game.Prefabs.LaneFlags) 0)
            component6.m_Flags |= ParkingLaneFlags.VirtualLane;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PublicOnly) != (Game.Prefabs.LaneFlags) 0)
            component6.m_Flags |= ParkingLaneFlags.SpecialVehicles;
          component6.m_Flags |= ParkingLaneFlags.StartingLane | ParkingLaneFlags.EndingLane | ParkingLaneFlags.FindConnections;
        }
        PedestrianLane component7 = new PedestrianLane();
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
        {
          component7.m_Flags |= PedestrianLaneFlags.AllowMiddle;
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.OnWater) != (Game.Prefabs.LaneFlags) 0)
            component7.m_Flags |= PedestrianLaneFlags.OnWater;
        }
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey key = new LaneSystem.LaneKey(laneData, component2.m_Prefab, netLaneData.m_Flags);
        if (original != Entity.Null)
        {
          temp.m_Original = original;
        }
        else
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneKey laneKey = key;
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated method
            this.ReplaceTempOwner(ref laneKey, owner);
            // ISSUE: reference to a compiler-generated method
            this.GetOriginalLane(laneBuffer, laneKey, ref temp);
          }
        }
        PseudoRandomSeed componentData1 = new PseudoRandomSeed();
        // ISSUE: reference to a compiler-generated field
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0 && !this.m_PseudoRandomSeedData.TryGetComponent(temp.m_Original, out componentData1))
          componentData1 = new PseudoRandomSeed(ref outRandom);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (laneBuffer.m_OldLanes.TryGetValue(key, out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_OldLanes.Remove(key);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, curveData);
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PseudoRandomSeedData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, entity1, componentData1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity1, componentData1);
            }
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity1, component4);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<TrackLane>(jobIndex, entity1, component5);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Parking) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<ParkingLane>(jobIndex, entity1, component6);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity1, component7);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity1, component3);
          }
          if (math.any(bool2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, entity1, new Elevation(elevation));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Elevation>(jobIndex, entity1);
            }
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Updates.Add(in entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_Updates.Add(in entity1);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[component2.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, laneArchetypeData.m_LaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, laneData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, curveData);
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, componentData1);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity2, component4);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<TrackLane>(jobIndex, entity2, component5);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Parking) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<ParkingLane>(jobIndex, entity2, component6);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity2, component7);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity2, component3);
          }
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Secondary) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<SecondaryLane>(jobIndex, entity2);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_TempOwnerTypes);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity2, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity2, temp);
            if (original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_OverriddenData.HasComponent(original))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Overridden>(jobIndex, entity2, new Overridden());
              }
              DynamicBuffer<CutRange> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CutRanges.TryGetBuffer(original, out bufferData))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddBuffer<CutRange>(jobIndex, entity2).CopyFrom(bufferData);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component1);
          }
          if (!math.any(bool2))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, entity2, new Elevation(elevation));
        }
      }

      private void ReplaceTempOwner(ref LaneSystem.LaneKey laneKey, Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.HasComponent(owner))
          return;
        // ISSUE: reference to a compiler-generated field
        Temp temp = this.m_TempData[owner];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!(temp.m_Original != Entity.Null) || this.m_EdgeData.HasComponent(temp.m_Original) && !this.m_EdgeData.HasComponent(owner))
          return;
        // ISSUE: reference to a compiler-generated method
        laneKey.ReplaceOwner(owner, temp.m_Original);
      }

      private void GetOriginalLane(
        LaneSystem.LaneBuffer laneBuffer,
        LaneSystem.LaneKey laneKey,
        ref Temp temp)
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        if (!laneBuffer.m_OriginalLanes.TryGetValue(laneKey, out entity))
          return;
        temp.m_Original = entity;
        // ISSUE: reference to a compiler-generated field
        laneBuffer.m_OriginalLanes.Remove(laneKey);
      }

      private void GetMiddleConnectionCurves(
        Entity node,
        NativeList<LaneSystem.EdgeTarget> edgeTargets)
      {
        edgeTargets.Clear();
        // ISSUE: reference to a compiler-generated field
        float3 position = this.m_NodeData[node].m_Position;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, node, this.m_Edges, this.m_EdgeData, this.m_TempData, this.m_HiddenData, true);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          if (edgeIteratorValue.m_Middle)
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_EdgeData[edgeIteratorValue.m_Edge];
            // ISSUE: reference to a compiler-generated field
            EdgeNodeGeometry geometry1 = this.m_StartNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry;
            // ISSUE: reference to a compiler-generated field
            EdgeNodeGeometry geometry2 = this.m_EndNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry;
            // ISSUE: variable of a compiler-generated type
            LaneSystem.EdgeTarget edgeTarget;
            // ISSUE: reference to a compiler-generated field
            edgeTarget.m_Edge = edgeIteratorValue.m_Edge;
            // ISSUE: reference to a compiler-generated field
            edgeTarget.m_StartNode = math.any(geometry1.m_Left.m_Length > 0.05f) | math.any(geometry1.m_Right.m_Length > 0.05f) ? edge.m_Start : edgeIteratorValue.m_Edge;
            // ISSUE: reference to a compiler-generated field
            edgeTarget.m_EndNode = math.any(geometry2.m_Left.m_Length > 0.05f) | math.any(geometry2.m_Right.m_Length > 0.05f) ? edge.m_End : edgeIteratorValue.m_Edge;
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[edgeIteratorValue.m_Edge];
            // ISSUE: reference to a compiler-generated field
            EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edgeIteratorValue.m_Edge];
            float t;
            double num = (double) MathUtils.Distance(curve.m_Bezier.xz, position.xz, out t);
            float3 float3_1 = MathUtils.Position(curve.m_Bezier, t);
            float3 float3_2 = MathUtils.Tangent(curve.m_Bezier, t);
            if ((double) math.dot(position.xz - float3_1.xz, MathUtils.Right(float3_2.xz)) < 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_StartPos = edgeGeometry.m_Start.m_Left.a;
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_EndPos = edgeGeometry.m_End.m_Left.d;
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_StartTangent = math.normalizesafe(-MathUtils.StartTangent(edgeGeometry.m_Start.m_Left));
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_EndTangent = math.normalizesafe(MathUtils.EndTangent(edgeGeometry.m_End.m_Left));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_StartPos = edgeGeometry.m_Start.m_Right.a;
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_EndPos = edgeGeometry.m_End.m_Right.d;
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_StartTangent = math.normalizesafe(-MathUtils.StartTangent(edgeGeometry.m_Start.m_Right));
              // ISSUE: reference to a compiler-generated field
              edgeTarget.m_EndTangent = math.normalizesafe(MathUtils.EndTangent(edgeGeometry.m_End.m_Right));
            }
            edgeTargets.Add(in edgeTarget);
          }
        }
      }

      private void FilterNodeConnectPositions(
        Entity owner,
        Entity original,
        NativeList<LaneSystem.ConnectPosition> connectPositions,
        NativeList<LaneSystem.EdgeTarget> edgeTargets)
      {
        int index1 = 0;
        int num = -1;
        bool flag = false;
        for (int index2 = 0; index2 < connectPositions.Length; ++index2)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition1 = connectPositions[index2];
          // ISSUE: reference to a compiler-generated field
          if ((int) connectPosition1.m_GroupIndex != num)
          {
            // ISSUE: reference to a compiler-generated field
            num = (int) connectPosition1.m_GroupIndex;
            flag = false;
          }
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition1.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            if (!flag)
            {
              for (int index3 = index2 + 1; index3 < connectPositions.Length; ++index3)
              {
                // ISSUE: variable of a compiler-generated type
                LaneSystem.ConnectPosition connectPosition2 = connectPositions[index3];
                // ISSUE: reference to a compiler-generated field
                if ((int) connectPosition2.m_GroupIndex == num)
                {
                  // ISSUE: reference to a compiler-generated method
                  if (this.CheckConnectPosition(owner, original, connectPosition2, edgeTargets))
                  {
                    flag = true;
                    break;
                  }
                }
                else
                  break;
              }
              if (!flag)
                continue;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            if (this.CheckConnectPosition(owner, original, connectPosition1, edgeTargets))
              flag = true;
            else
              continue;
          }
          connectPositions[index1++] = connectPosition1;
        }
        connectPositions.RemoveRange(index1, connectPositions.Length - index1);
      }

      private bool CheckConnectPosition(
        Entity owner,
        Entity original,
        LaneSystem.ConnectPosition connectPosition,
        NativeList<LaneSystem.EdgeTarget> edgeTargets)
      {
        Entity entity1 = Entity.Null;
        float num1 = float.MaxValue;
        for (int index = 0; index < edgeTargets.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.EdgeTarget edgeTarget = edgeTargets[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 x1 = connectPosition.m_Position.xz - edgeTarget.m_StartPos.xz;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 x2 = connectPosition.m_Position.xz - edgeTarget.m_EndPos.xz;
          // ISSUE: reference to a compiler-generated field
          float num2 = math.dot(x1, edgeTarget.m_StartTangent.xz);
          // ISSUE: reference to a compiler-generated field
          float num3 = math.dot(x2, edgeTarget.m_EndTangent.xz);
          float num4 = math.length(x1);
          float num5 = math.length(x2);
          Entity entity2;
          float num6;
          if ((double) num2 > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            entity2 = edgeTarget.m_StartNode;
            num6 = num4 + num2;
          }
          else if ((double) num3 > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            entity2 = edgeTarget.m_EndNode;
            num6 = num5 + num3;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            entity2 = edgeTarget.m_Edge;
            num6 = math.select(num4 + num2, num5 + num3, (double) num5 < (double) num4);
          }
          if ((double) num6 < (double) num1)
          {
            entity1 = entity2;
            num1 = num6;
          }
        }
        return entity1 == owner || entity1 == original;
      }

      private void GetMiddleConnections(
        Entity owner,
        Entity original,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        NativeList<LaneSystem.EdgeTarget> tempEdgeTargets,
        NativeList<LaneSystem.ConnectPosition> tempBuffer1,
        NativeList<LaneSystem.ConnectPosition> tempBuffer2,
        ref int groupIndex)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, owner, this.m_Edges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedNode> node = this.m_Nodes[edgeIteratorValue.m_Edge];
          for (int index1 = 0; index1 < node.Length; ++index1)
          {
            ConnectedNode connectedNode = node[index1];
            // ISSUE: reference to a compiler-generated method
            this.GetMiddleConnectionCurves(connectedNode.m_Node, tempEdgeTargets);
            bool flag1 = false;
            for (int index2 = 0; index2 < tempEdgeTargets.Length; ++index2)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.EdgeTarget tempEdgeTarget = tempEdgeTargets[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (tempEdgeTarget.m_StartNode == owner || tempEdgeTarget.m_StartNode == original || tempEdgeTarget.m_EndNode == owner || tempEdgeTarget.m_EndNode == original)
              {
                flag1 = true;
                break;
              }
            }
            if (flag1)
            {
              int groupIndex1 = groupIndex;
              // ISSUE: reference to a compiler-generated method
              this.GetNodeConnectPositions(connectedNode.m_Node, connectedNode.m_CurvePosition, tempBuffer1, tempBuffer2, true, ref groupIndex1, out float _, out float _);
              // ISSUE: reference to a compiler-generated method
              this.FilterNodeConnectPositions(owner, original, tempBuffer1, tempEdgeTargets);
              // ISSUE: reference to a compiler-generated method
              this.FilterNodeConnectPositions(owner, original, tempBuffer2, tempEdgeTargets);
              Entity entity = new Entity();
              if (tempBuffer1.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                entity = tempBuffer1[0].m_Owner;
              }
              else if (tempBuffer2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                entity = tempBuffer2[0].m_Owner;
              }
              if (entity != Entity.Null)
              {
                bool flag2 = false;
                for (int index3 = 0; index3 < middleConnections.Length; ++index3)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (middleConnections[index3].m_ConnectPosition.m_Owner == entity)
                  {
                    flag2 = true;
                    break;
                  }
                }
                if (!flag2)
                {
                  groupIndex = groupIndex1;
                  for (int index4 = 0; index4 < tempBuffer1.Length; ++index4)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.MiddleConnection middleConnection = new LaneSystem.MiddleConnection()
                    {
                      m_ConnectPosition = tempBuffer1[index4]
                    };
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_ConnectPosition.m_IsSideConnection = true;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_SourceEdge = edgeIteratorValue.m_Edge;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_SourceNode = connectedNode.m_Node;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_SortIndex = middleConnections.Length;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_Distance = float.MaxValue;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_IsSource = true;
                    middleConnections.Add(in middleConnection);
                  }
                  for (int index5 = 0; index5 < tempBuffer2.Length; ++index5)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.MiddleConnection middleConnection = new LaneSystem.MiddleConnection()
                    {
                      m_ConnectPosition = tempBuffer2[index5]
                    };
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_ConnectPosition.m_IsSideConnection = true;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_SourceEdge = edgeIteratorValue.m_Edge;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_SourceNode = connectedNode.m_Node;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_SortIndex = middleConnections.Length;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_Distance = float.MaxValue;
                    // ISSUE: reference to a compiler-generated field
                    middleConnection.m_IsSource = false;
                    middleConnections.Add(in middleConnection);
                  }
                }
              }
              tempBuffer1.Clear();
              tempBuffer2.Clear();
            }
          }
        }
      }

      private RoadTypes GetRoundaboutRoadPassThrough(Entity owner)
      {
        RoadTypes roundaboutRoadPassThrough = RoadTypes.None;
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(owner, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            PrefabRef componentData1;
            NetObjectData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.TryGetComponent(bufferData[index].m_SubObject, out componentData1) && this.m_PrefabNetObjectData.TryGetComponent(componentData1.m_Prefab, out componentData2) && (componentData2.m_CompositionFlags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
              roundaboutRoadPassThrough |= componentData2.m_RoadPassThrough;
          }
        }
        return roundaboutRoadPassThrough;
      }

      private void GetNodeConnectPositions(
        Entity owner,
        float curvePosition,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        bool includeAnchored,
        ref int groupIndex,
        out float middleRadius,
        out float roundaboutSize)
      {
        middleRadius = 0.0f;
        roundaboutSize = 0.0f;
        bool flag1 = false;
        NativeParallelHashSet<Entity> anchorPrefabs = new NativeParallelHashSet<Entity>();
        float2 elevation = new float2();
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.HasComponent(owner))
        {
          // ISSUE: reference to a compiler-generated field
          elevation = this.m_ElevationData[owner].m_Elevation;
        }
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[owner];
        // ISSUE: reference to a compiler-generated field
        NetGeometryData prefabGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, owner, this.m_Edges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated method
          this.GetNodeConnectPositions(owner, edgeIteratorValue.m_Edge, edgeIteratorValue.m_End, groupIndex++, curvePosition, elevation, prefabGeometryData, sourceBuffer, targetBuffer, includeAnchored, ref middleRadius, ref roundaboutSize, ref anchorPrefabs);
          flag1 = true;
        }
        // ISSUE: reference to a compiler-generated field
        if (!flag1 && this.m_DefaultNetLanes.HasBuffer(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          Node node = this.m_NodeData[owner];
          NodeGeometry componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeGeometryData.TryGetComponent(owner, out componentData))
            node.m_Position.y = componentData.m_Position;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<DefaultNetLane> defaultNetLane = this.m_DefaultNetLanes[prefabRef.m_Prefab];
          Game.Prefabs.LaneFlags laneFlags = includeAnchored ? (Game.Prefabs.LaneFlags) 0 : Game.Prefabs.LaneFlags.FindAnchor;
          for (int index = 0; index < defaultNetLane.Length; ++index)
          {
            NetCompositionLane netCompositionLane = new NetCompositionLane(defaultNetLane[index]);
            // ISSUE: reference to a compiler-generated method
            if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0 && ((netCompositionLane.m_Flags & laneFlags) == (Game.Prefabs.LaneFlags) 0 || !this.IsAnchored(owner, ref anchorPrefabs, netCompositionLane.m_Lane)))
            {
              bool flag2 = (netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Invert) != 0;
              if ((netCompositionLane.m_Flags & (flag2 ? Game.Prefabs.LaneFlags.DisconnectedEnd : Game.Prefabs.LaneFlags.DisconnectedStart)) == (Game.Prefabs.LaneFlags) 0)
              {
                netCompositionLane.m_Position.x = -netCompositionLane.m_Position.x;
                float s = (float) ((double) netCompositionLane.m_Position.x / (double) math.max(1f, prefabGeometryData.m_DefaultWidth) + 0.5);
                float3 y = node.m_Position + math.rotate(node.m_Rotation, new float3(prefabGeometryData.m_DefaultWidth * -0.5f, 0.0f, 0.0f));
                float3 float3 = math.lerp(node.m_Position + math.rotate(node.m_Rotation, new float3(prefabGeometryData.m_DefaultWidth * 0.5f, 0.0f, 0.0f)), y, s);
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                LaneSystem.ConnectPosition connectPosition = new LaneSystem.ConnectPosition();
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_LaneData = netCompositionLane;
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Owner = owner;
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Position = float3;
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Position.y += netCompositionLane.m_Position.y;
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Tangent = math.forward(node.m_Rotation);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Tangent = -MathUtils.Normalize(connectPosition.m_Tangent, connectPosition.m_Tangent.xz);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Tangent.y = math.clamp(connectPosition.m_Tangent.y, -1f, 1f);
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_GroupIndex = (ushort) ((uint) netCompositionLane.m_Group | (uint) (groupIndex << 8));
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_CurvePosition = curvePosition;
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_BaseHeight = float3.y;
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Elevation = math.lerp(elevation.x, elevation.y, 0.5f);
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_Order = s;
                if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  TrackLaneData trackLaneData = this.m_TrackLaneData[netCompositionLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_TrackTypes = trackLaneData.m_TrackTypes;
                }
                if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  UtilityLaneData utilityLaneData = this.m_UtilityLaneData[netCompositionLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_UtilityTypes = utilityLaneData.m_UtilityTypes;
                }
                if ((netCompositionLane.m_Flags & (Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Utility)) != (Game.Prefabs.LaneFlags) 0)
                  targetBuffer.Add(in connectPosition);
                else if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
                {
                  targetBuffer.Add(in connectPosition);
                  sourceBuffer.Add(in connectPosition);
                }
                else if (!flag2)
                  targetBuffer.Add(in connectPosition);
                else
                  sourceBuffer.Add(in connectPosition);
              }
            }
          }
          ++groupIndex;
        }
        if (!anchorPrefabs.IsCreated)
          return;
        anchorPrefabs.Dispose();
      }

      private unsafe void GetNodeConnectPositions(
        Entity node,
        Entity edge,
        bool isEnd,
        int groupIndex,
        float curvePosition,
        float2 elevation,
        NetGeometryData prefabGeometryData,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        bool includeAnchored,
        ref float middleRadius,
        ref float roundaboutSize,
        ref NativeParallelHashSet<Entity> anchorPrefabs)
      {
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_CompositionData[edge];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[edge];
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        LaneSystem.CompositionData compositionData = this.GetCompositionData(composition.m_Edge);
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[isEnd ? composition.m_EndNode : composition.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabGeometryData[prefabRef1.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionLane> prefabCompositionLane = this.m_PrefabCompositionLanes[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edge];
        EdgeNodeGeometry geometry;
        if (isEnd)
        {
          edgeGeometry.m_Start.m_Left = MathUtils.Invert(edgeGeometry.m_End.m_Right);
          edgeGeometry.m_Start.m_Right = MathUtils.Invert(edgeGeometry.m_End.m_Left);
          // ISSUE: reference to a compiler-generated field
          geometry = this.m_EndNodeGeometryData[edge].m_Geometry;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          geometry = this.m_StartNodeGeometryData[edge].m_Geometry;
        }
        middleRadius = math.max(middleRadius, geometry.m_MiddleRadius);
        roundaboutSize = math.max(roundaboutSize, math.select(netCompositionData2.m_RoundaboutSize.x, netCompositionData2.m_RoundaboutSize.y, isEnd));
        bool flag1 = (netGeometryData.m_MergeLayers & prefabGeometryData.m_MergeLayers) == Layer.None && (prefabGeometryData.m_MergeLayers & Layer.Road) > Layer.None;
        Game.Prefabs.LaneFlags laneFlags1 = includeAnchored ? (Game.Prefabs.LaneFlags) 0 : Game.Prefabs.LaneFlags.FindAnchor;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_UpdatedData.HasComponent(edge) && this.m_SubLanes.HasBuffer(edge))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubLane> subLane1 = this.m_SubLanes[edge];
          float num1 = math.select(0.0f, 1f, isEnd);
          bool* flagPtr = stackalloc bool[prefabCompositionLane.Length];
          for (int index = 0; index < prefabCompositionLane.Length; ++index)
            flagPtr[index] = false;
          for (int index1 = 0; index1 < subLane1.Length; ++index1)
          {
            Entity subLane2 = subLane1[index1].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeLaneData.HasComponent(subLane2) && !this.m_SecondaryLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              bool2 x = this.m_EdgeLaneData[subLane2].m_EdgeDelta == num1;
              if (math.any(x))
              {
                bool y = x.y;
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane2];
                if (y)
                  curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
                int index2 = -1;
                float num2 = float.MaxValue;
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef2 = this.m_PrefabRefData[subLane2];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_NetLaneData[prefabRef2.m_Prefab];
                Game.Prefabs.LaneFlags laneFlags2 = y ? Game.Prefabs.LaneFlags.DisconnectedEnd : Game.Prefabs.LaneFlags.DisconnectedStart;
                Game.Prefabs.LaneFlags laneFlags3 = netLaneData.m_Flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Parking | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.Utility | Game.Prefabs.LaneFlags.Underground | Game.Prefabs.LaneFlags.OnWater);
                Game.Prefabs.LaneFlags laneFlags4 = Game.Prefabs.LaneFlags.Invert | Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Parking | Game.Prefabs.LaneFlags.Track | Game.Prefabs.LaneFlags.Utility | Game.Prefabs.LaneFlags.Underground | Game.Prefabs.LaneFlags.OnWater | laneFlags2;
                if (y != isEnd)
                  laneFlags3 |= Game.Prefabs.LaneFlags.Invert;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SlaveLaneData.HasComponent(subLane2))
                  laneFlags3 |= Game.Prefabs.LaneFlags.Slave;
                // ISSUE: reference to a compiler-generated field
                if (this.m_MasterLaneData.HasComponent(subLane2))
                {
                  laneFlags3 = (laneFlags3 | Game.Prefabs.LaneFlags.Master) & ~Game.Prefabs.LaneFlags.Track;
                  laneFlags4 &= ~Game.Prefabs.LaneFlags.Track;
                }
                else if ((netLaneData.m_Flags & laneFlags2) != (Game.Prefabs.LaneFlags) 0)
                  continue;
                TrackLaneData trackLaneData1 = new TrackLaneData();
                UtilityLaneData utilityLaneData1 = new UtilityLaneData();
                if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  trackLaneData1 = this.m_TrackLaneData[prefabRef2.m_Prefab];
                }
                if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  utilityLaneData1 = this.m_UtilityLaneData[prefabRef2.m_Prefab];
                }
                for (int index3 = 0; index3 < prefabCompositionLane.Length; ++index3)
                {
                  NetCompositionLane netCompositionLane = prefabCompositionLane[index3];
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((netCompositionLane.m_Flags & laneFlags4) == laneFlags3 && ((netCompositionLane.m_Flags & laneFlags1) == (Game.Prefabs.LaneFlags) 0 || !this.IsAnchored(node, ref anchorPrefabs, netCompositionLane.m_Lane)) && ((laneFlags3 & Game.Prefabs.LaneFlags.Track) == (Game.Prefabs.LaneFlags) 0 || this.m_TrackLaneData[netCompositionLane.m_Lane].m_TrackTypes == trackLaneData1.m_TrackTypes) && ((laneFlags3 & Game.Prefabs.LaneFlags.Utility) == (Game.Prefabs.LaneFlags) 0 || this.m_UtilityLaneData[netCompositionLane.m_Lane].m_UtilityTypes == utilityLaneData1.m_UtilityTypes))
                  {
                    netCompositionLane.m_Position.x = math.select(-netCompositionLane.m_Position.x, netCompositionLane.m_Position.x, isEnd);
                    float num3 = (float) ((double) netCompositionLane.m_Position.x / (double) math.max(1f, netCompositionData1.m_Width) + 0.5);
                    float2 t;
                    if (MathUtils.Intersect(new Line2(edgeGeometry.m_Start.m_Right.a.xz, edgeGeometry.m_Start.m_Left.a.xz), new Line2(curve.m_Bezier.a.xz, curve.m_Bezier.b.xz), out t))
                    {
                      float num4 = math.abs(num3 - t.x);
                      if ((double) num4 < (double) num2)
                      {
                        index2 = index3;
                        num2 = num4;
                      }
                    }
                  }
                }
                if (index2 != -1 && !flagPtr[index2])
                {
                  flagPtr[index2] = true;
                  NetCompositionLane netCompositionLane = prefabCompositionLane[index2];
                  netCompositionLane.m_Position.x = math.select(-netCompositionLane.m_Position.x, netCompositionLane.m_Position.x, isEnd);
                  float num5 = (float) ((double) netCompositionLane.m_Position.x / (double) math.max(1f, netCompositionData1.m_Width) + 0.5);
                  // ISSUE: reference to a compiler-generated field
                  Lane lane = this.m_LaneData[subLane2];
                  netCompositionLane.m_Index = !y ? (byte) ((uint) lane.m_StartNode.GetLaneIndex() & (uint) byte.MaxValue) : (byte) ((uint) lane.m_EndNode.GetLaneIndex() & (uint) byte.MaxValue);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition = new LaneSystem.ConnectPosition()
                  {
                    m_LaneData = netCompositionLane,
                    m_Owner = edge,
                    m_NodeComposition = isEnd ? composition.m_EndNode : composition.m_StartNode,
                    m_EdgeComposition = composition.m_Edge,
                    m_Position = curve.m_Bezier.a,
                    m_Tangent = MathUtils.StartTangent(curve.m_Bezier)
                  };
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_Tangent = -MathUtils.Normalize(connectPosition.m_Tangent, connectPosition.m_Tangent.xz);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_Tangent.y = math.clamp(connectPosition.m_Tangent.y, -1f, 1f);
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_SegmentIndex = (byte) math.select(0, 4, isEnd);
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_GroupIndex = (ushort) ((uint) netCompositionLane.m_Group | (uint) (groupIndex << 8));
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_CompositionData = compositionData;
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_CurvePosition = curvePosition;
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_BaseHeight = curve.m_Bezier.a.y;
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_BaseHeight -= netCompositionLane.m_Position.y;
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_Elevation = math.lerp(elevation.x, elevation.y, 0.5f);
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_IsEnd = isEnd;
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_Order = num5;
                  // ISSUE: reference to a compiler-generated field
                  connectPosition.m_IsSideConnection = flag1;
                  if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    TrackLaneData trackLaneData2 = this.m_TrackLaneData[netCompositionLane.m_Lane];
                    // ISSUE: reference to a compiler-generated field
                    connectPosition.m_TrackTypes = trackLaneData2.m_TrackTypes;
                  }
                  if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    UtilityLaneData utilityLaneData2 = this.m_UtilityLaneData[netCompositionLane.m_Lane];
                    // ISSUE: reference to a compiler-generated field
                    connectPosition.m_UtilityTypes = utilityLaneData2.m_UtilityTypes;
                  }
                  if ((netCompositionLane.m_Flags & (Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Utility)) != (Game.Prefabs.LaneFlags) 0)
                    targetBuffer.Add(in connectPosition);
                  else if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
                  {
                    targetBuffer.Add(in connectPosition);
                    sourceBuffer.Add(in connectPosition);
                  }
                  else if (!y)
                    targetBuffer.Add(in connectPosition);
                  else
                    sourceBuffer.Add(in connectPosition);
                }
              }
            }
          }
        }
        else
        {
          for (int index = 0; index < prefabCompositionLane.Length; ++index)
          {
            NetCompositionLane netCompositionLane = prefabCompositionLane[index];
            bool flag2 = isEnd == ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Invert) == (Game.Prefabs.LaneFlags) 0);
            // ISSUE: reference to a compiler-generated method
            if ((netCompositionLane.m_Flags & (flag2 ? Game.Prefabs.LaneFlags.DisconnectedEnd : Game.Prefabs.LaneFlags.DisconnectedStart)) == (Game.Prefabs.LaneFlags) 0 && ((netCompositionLane.m_Flags & laneFlags1) == (Game.Prefabs.LaneFlags) 0 || !this.IsAnchored(node, ref anchorPrefabs, netCompositionLane.m_Lane)))
            {
              netCompositionLane.m_Position.x = math.select(-netCompositionLane.m_Position.x, netCompositionLane.m_Position.x, isEnd);
              float t = (float) ((double) netCompositionLane.m_Position.x / (double) math.max(1f, netCompositionData1.m_Width) + 0.5);
              Bezier4x3 curve = MathUtils.Lerp(edgeGeometry.m_Start.m_Right, edgeGeometry.m_Start.m_Left, t);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition connectPosition = new LaneSystem.ConnectPosition();
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_LaneData = netCompositionLane;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Owner = edge;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_NodeComposition = isEnd ? composition.m_EndNode : composition.m_StartNode;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_EdgeComposition = composition.m_Edge;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Position = curve.a;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Position.y += netCompositionLane.m_Position.y;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Tangent = MathUtils.StartTangent(curve);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Tangent = -MathUtils.Normalize(connectPosition.m_Tangent, connectPosition.m_Tangent.xz);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Tangent.y = math.clamp(connectPosition.m_Tangent.y, -1f, 1f);
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_SegmentIndex = (byte) math.select(0, 4, isEnd);
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_GroupIndex = (ushort) ((uint) netCompositionLane.m_Group | (uint) (groupIndex << 8));
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_CompositionData = compositionData;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_CurvePosition = curvePosition;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_BaseHeight = curve.a.y;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Elevation = math.lerp(elevation.x, elevation.y, 0.5f);
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_IsEnd = isEnd;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_Order = t;
              // ISSUE: reference to a compiler-generated field
              connectPosition.m_IsSideConnection = flag1;
              if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                TrackLaneData trackLaneData = this.m_TrackLaneData[netCompositionLane.m_Lane];
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_TrackTypes = trackLaneData.m_TrackTypes;
              }
              if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                UtilityLaneData utilityLaneData = this.m_UtilityLaneData[netCompositionLane.m_Lane];
                // ISSUE: reference to a compiler-generated field
                connectPosition.m_UtilityTypes = utilityLaneData.m_UtilityTypes;
              }
              if ((netCompositionLane.m_Flags & (Game.Prefabs.LaneFlags.Pedestrian | Game.Prefabs.LaneFlags.Utility)) != (Game.Prefabs.LaneFlags) 0)
                targetBuffer.Add(in connectPosition);
              else if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
              {
                targetBuffer.Add(in connectPosition);
                sourceBuffer.Add(in connectPosition);
              }
              else if (!flag2)
                targetBuffer.Add(in connectPosition);
              else
                sourceBuffer.Add(in connectPosition);
            }
          }
        }
      }

      private void FilterMainCarConnectPositions(
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Road)) == Game.Prefabs.LaneFlags.Road)
            output.Add(in connectPosition);
        }
      }

      private void FilterActualCarConnectPositions(
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Road)) == Game.Prefabs.LaneFlags.Road)
            output.Add(in connectPosition);
        }
      }

      private void FilterActualCarConnectPositions(
        LaneSystem.ConnectPosition main,
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Road)) == Game.Prefabs.LaneFlags.Road && connectPosition.m_Owner == main.m_Owner && (int) connectPosition.m_LaneData.m_Group == (int) main.m_LaneData.m_Group)
            output.Add(in connectPosition);
        }
      }

      private TrackTypes FilterTrackConnectPositions(
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        TrackTypes trackTypes = TrackTypes.None;
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Track)) == Game.Prefabs.LaneFlags.Track)
          {
            output.Add(in connectPosition);
            // ISSUE: reference to a compiler-generated field
            trackTypes |= connectPosition.m_TrackTypes;
          }
        }
        return trackTypes;
      }

      private void FilterTrackConnectPositions(
        ref int index,
        TrackTypes trackType,
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition connectPosition1 = new LaneSystem.ConnectPosition();
        while (index < input.Length)
        {
          connectPosition1 = input[index++];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition1.m_TrackTypes & trackType) != TrackTypes.None)
          {
            output.Add(in connectPosition1);
            break;
          }
        }
        while (index < input.Length)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition2 = input[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (connectPosition2.m_Owner != connectPosition1.m_Owner)
            break;
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition2.m_TrackTypes & trackType) != TrackTypes.None)
            output.Add(in connectPosition2);
          ++index;
        }
      }

      private void FilterTrackConnectPositions(
        TrackTypes trackType,
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_TrackTypes & trackType) != TrackTypes.None)
            output.Add(in connectPosition);
        }
      }

      private void FilterPedestrianConnectPositions(
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output,
        NativeList<LaneSystem.MiddleConnection> middleConnections)
      {
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
            output.Add(in connectPosition);
        }
        for (int index = 0; index < middleConnections.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.MiddleConnection middleConnection = middleConnections[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Pedestrian) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            output.Add(in middleConnection.m_ConnectPosition);
          }
        }
      }

      private UtilityTypes FilterUtilityConnectPositions(
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        UtilityTypes utilityTypes = UtilityTypes.None;
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            output.Add(in connectPosition);
            // ISSUE: reference to a compiler-generated field
            utilityTypes |= connectPosition.m_UtilityTypes;
          }
        }
        return utilityTypes;
      }

      private void FilterUtilityConnectPositions(
        UtilityTypes utilityTypes,
        NativeList<LaneSystem.ConnectPosition> input,
        NativeList<LaneSystem.ConnectPosition> output)
      {
        for (int index = 0; index < input.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = input[index];
          // ISSUE: reference to a compiler-generated field
          if ((connectPosition.m_UtilityTypes & utilityTypes) != UtilityTypes.None)
            output.Add(in connectPosition);
        }
      }

      private int CalculateYieldOffset(
        LaneSystem.ConnectPosition source,
        NativeList<LaneSystem.ConnectPosition> sources,
        NativeList<LaneSystem.ConnectPosition> targets)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData = this.m_PrefabCompositionData[source.m_NodeComposition];
        if ((netCompositionData.m_Flags.m_General & CompositionFlags.General.AllWayStop) != (CompositionFlags.General) 0)
          return 2;
        if ((netCompositionData.m_Flags.m_General & (CompositionFlags.General.LevelCrossing | CompositionFlags.General.TrafficLights)) != (CompositionFlags.General) 0)
          return 0;
        Entity owner = Entity.Null;
        for (int index = 0; index < sources.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition source1 = sources[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (source1.m_Owner != source.m_Owner && source1.m_Owner != owner && (double) source1.m_CompositionData.m_Priority - (double) source.m_CompositionData.m_Priority > 0.99000000953674316)
          {
            if (owner != Entity.Null)
              return 1;
            // ISSUE: reference to a compiler-generated field
            owner = source1.m_Owner;
          }
        }
        if (owner == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((source.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) == (Game.Prefabs.RoadFlags) 0)
            return 0;
          int num = 0;
          for (int index1 = 0; index1 < sources.Length; ++index1)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition source2 = sources[index1];
            bool flag1 = false;
            for (int index2 = 0; index2 < targets.Length; ++index2)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition target = targets[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (target.m_Owner != source2.m_Owner)
              {
                bool gentle;
                // ISSUE: reference to a compiler-generated method
                bool flag2 = this.IsTurn(source2, target, out bool _, out gentle, out bool _);
                flag1 |= !flag2 | gentle;
              }
            }
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (source2.m_Owner == source.m_Owner)
                return 0;
              ++num;
            }
          }
          return math.select(0, 1, num >= 1);
        }
        for (int index = 0; index < targets.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition target = targets[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (target.m_Owner != source.m_Owner && target.m_Owner != owner && (double) target.m_CompositionData.m_Priority - (double) source.m_CompositionData.m_Priority > 0.99000000953674316)
            return 1;
        }
        return 0;
      }

      private void ProcessCarConnectPositions(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        NativeParallelHashSet<LaneSystem.ConnectionKey> createdConnections,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        NativeList<LaneSystem.ConnectPosition> allSources,
        RoadTypes roadPassThrough,
        bool isTemp,
        Temp ownerTemp,
        int yield)
      {
        if (sourceBuffer.Length < 1 || targetBuffer.Length < 1)
          return;
        NativeList<LaneSystem.ConnectPosition> list = sourceBuffer;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.SourcePositionComparer positionComparer = new LaneSystem.SourcePositionComparer();
        // ISSUE: variable of a compiler-generated type
        LaneSystem.SourcePositionComparer comp = positionComparer;
        list.Sort<LaneSystem.ConnectPosition, LaneSystem.SourcePositionComparer>(comp);
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition sourcePosition = sourceBuffer[0];
        // ISSUE: reference to a compiler-generated method
        this.SortTargets(sourcePosition, targetBuffer);
        // ISSUE: reference to a compiler-generated method
        this.CreateNodeCarLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, middleConnections, createdConnections, sourceBuffer, targetBuffer, allSources, roadPassThrough, isTemp, ownerTemp, yield);
      }

      private void SortTargets(
        LaneSystem.ConnectPosition sourcePosition,
        NativeList<LaneSystem.ConnectPosition> targetBuffer)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 x = new float2(sourcePosition.m_Tangent.z, -sourcePosition.m_Tangent.x);
        for (int index = 0; index < targetBuffer.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = targetBuffer[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 y = connectPosition.m_Position.xz - sourcePosition.m_Position.xz - connectPosition.m_Tangent.xz;
          MathUtils.TryNormalize(ref y);
          float num1;
          // ISSUE: reference to a compiler-generated field
          if ((double) math.dot(sourcePosition.m_Tangent.xz, y) > 0.0)
          {
            num1 = math.dot(x, y) * 0.5f;
          }
          else
          {
            float num2 = math.dot(x, y);
            num1 = math.select(-1f, 1f, (double) num2 >= 0.0) - num2 * 0.5f;
          }
          // ISSUE: reference to a compiler-generated field
          connectPosition.m_Order = num1;
          targetBuffer[index] = connectPosition;
        }
        NativeList<LaneSystem.ConnectPosition> list = targetBuffer;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.TargetPositionComparer positionComparer = new LaneSystem.TargetPositionComparer();
        // ISSUE: variable of a compiler-generated type
        LaneSystem.TargetPositionComparer comp = positionComparer;
        list.Sort<LaneSystem.ConnectPosition, LaneSystem.TargetPositionComparer>(comp);
      }

      private int2 CalculateSourcesBetween(
        LaneSystem.ConnectPosition source,
        LaneSystem.ConnectPosition target,
        NativeList<LaneSystem.ConnectPosition> allSources)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 x = MathUtils.Right(target.m_Position.xz - source.m_Position.xz);
        int2 sourcesBetween = (int2) 0;
        for (int index = 0; index < allSources.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition allSource = allSources[index];
          bool uturn;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if ((int) allSource.m_GroupIndex != (int) source.m_GroupIndex && (allSource.m_LaneData.m_Flags & (Game.Prefabs.LaneFlags.Master | Game.Prefabs.LaneFlags.Road)) == Game.Prefabs.LaneFlags.Road && !(this.IsTurn(allSource, target, out bool _, out bool _, out uturn) & uturn))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            sourcesBetween += math.select(new int2(0, 1), new int2(1, 0), (double) math.dot(x, target.m_Position.xz - allSource.m_Position.xz) > 0.0);
          }
        }
        return sourcesBetween;
      }

      private void CreateNodeCarLanes(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        NativeParallelHashSet<LaneSystem.ConnectionKey> createdConnections,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        NativeList<LaneSystem.ConnectPosition> allSources,
        RoadTypes roadPassThrough,
        bool isTemp,
        Temp ownerTemp,
        int yield)
      {
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition sourcePosition1 = sourceBuffer[0];
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition sourcePosition2 = sourceBuffer[sourceBuffer.Length - 1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData = this.m_PrefabCompositionData[sourcePosition1.m_NodeComposition];
        // ISSUE: reference to a compiler-generated field
        int num1 = (netCompositionData.m_Flags.m_General & CompositionFlags.General.Invert) > (CompositionFlags.General) 0 != ((sourcePosition1.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) != 0) ? (int) netCompositionData.m_Flags.m_Left : (int) netCompositionData.m_Flags.m_Right;
        bool flag1 = (num1 & 16777216) != 0;
        bool flag2 = (num1 & 33554432) != 0;
        bool c = (num1 & 268435456) != 0;
        int index1 = 0;
        int x1 = 0;
        int y1 = 0;
        while (index1 < targetBuffer.Length)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition = targetBuffer[index1];
          int index2;
          for (index2 = index1 + 1; index2 < targetBuffer.Length; ++index2)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = targetBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) connectPosition.m_GroupIndex == (int) targetPosition.m_GroupIndex)
              targetPosition = connectPosition;
            else
              break;
          }
          bool right;
          bool uturn;
          // ISSUE: reference to a compiler-generated method
          if (this.IsTurn(sourcePosition1, targetPosition, out right, out bool _, out uturn) && !(right | !uturn))
          {
            index1 = index2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (targetPosition.m_Owner == sourcePosition1.m_Owner && (int) targetPosition.m_LaneData.m_Carriageway == (int) sourcePosition1.m_LaneData.m_Carriageway)
              x1 = index2;
            if (flag1)
              y1 = index2;
          }
          else
            break;
        }
        int num2 = 0;
        int x2 = 0;
        int y2 = 0;
        while (num2 < targetBuffer.Length - index1)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition = targetBuffer[targetBuffer.Length - num2 - 1];
          int num3;
          for (num3 = num2 + 1; num3 < targetBuffer.Length - index1; ++num3)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = targetBuffer[targetBuffer.Length - num3 - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) connectPosition.m_GroupIndex == (int) targetPosition.m_GroupIndex)
              targetPosition = connectPosition;
            else
              break;
          }
          bool right;
          bool uturn;
          // ISSUE: reference to a compiler-generated method
          if (this.IsTurn(sourcePosition2, targetPosition, out right, out bool _, out uturn) && !(!right | !uturn))
          {
            num2 = num3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (((!(targetPosition.m_Owner == sourcePosition2.m_Owner) ? 0 : ((int) targetPosition.m_LaneData.m_Carriageway == (int) sourcePosition2.m_LaneData.m_Carriageway ? 1 : 0)) | (flag2 ? 1 : 0)) != 0)
              x2 = num3;
            if (flag2)
              y2 = num3;
          }
          else
            break;
        }
        int num4 = 0;
        int num5 = 0;
        while (index1 + num4 < targetBuffer.Length - num2)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition = targetBuffer[index1 + num4];
          int num6;
          for (num6 = num4 + 1; index1 + num6 < targetBuffer.Length - num2; ++num6)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = targetBuffer[index1 + num6];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) connectPosition.m_GroupIndex == (int) targetPosition.m_GroupIndex)
              targetPosition = connectPosition;
            else
              break;
          }
          bool right;
          // ISSUE: reference to a compiler-generated method
          if (this.IsTurn(sourcePosition1, targetPosition, out right, out bool _, out bool _) && !right)
          {
            num4 = num6;
            if (flag1)
              num5 = num6;
          }
          else
            break;
        }
        int num7 = 0;
        int num8 = 0;
        while (num2 + num7 < targetBuffer.Length - index1 - num4)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition = targetBuffer[targetBuffer.Length - num2 - num7 - 1];
          int num9;
          for (num9 = num7 + 1; num2 + num9 < targetBuffer.Length - index1 - num4; ++num9)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = targetBuffer[targetBuffer.Length - num2 - num9 - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) connectPosition.m_GroupIndex == (int) targetPosition.m_GroupIndex)
              targetPosition = connectPosition;
            else
              break;
          }
          bool right;
          // ISSUE: reference to a compiler-generated method
          if (this.IsTurn(sourcePosition2, targetPosition, out right, out bool _, out bool _) && right)
          {
            num7 = num9;
            if (flag2)
              num8 = num9;
          }
          else
            break;
        }
        int num10 = index1 + num2;
        int num11 = num4 + num7;
        int y3 = targetBuffer.Length - num10;
        int b1 = y3 - num11;
        int num12 = math.select(0, b1, c);
        int y4 = b1 - num12;
        int x3 = math.min(sourceBuffer.Length, y3);
        if (x1 + x2 == targetBuffer.Length)
        {
          y1 = math.max(0, y1 - x1);
          y2 = math.max(0, y2 - x2);
          x1 = 0;
          x2 = 0;
        }
        int x4 = num4 - num5;
        int num13 = num7 - num8;
        int y5 = x4 + num13;
        int x5 = index1 - math.max(x1, y1);
        int x6 = num2 - math.max(x2, y2);
        int y6 = x5 + x6;
        int num14 = sourceBuffer.Length - x3;
        int x7 = math.min(x5, math.max(0, num14 * x5 + y6 - 1) / math.max(1, y6));
        int x8 = math.min(x6, math.max(0, num14 * x6 + y6 - 1) / math.max(1, y6));
        if (x7 + x8 > num14)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LeftHandTraffic)
            x8 = num14 - x7;
          else
            x7 = num14 - x8;
        }
        int b2 = math.min(x3, y4);
        if (b2 >= 2 && y3 >= 4)
        {
          int num15 = math.max(0, math.max(x4, num13) - 1) * sourceBuffer.Length / (y3 - 1);
          b2 = math.clamp(sourceBuffer.Length - num15, 1, b2);
        }
        int num16 = x3 - b2;
        int num17 = math.min(x4, math.max(0, num16 * x4 + y5 - 1) / math.max(1, y5));
        int num18 = math.min(num13, math.max(0, num16 * num13 + y5 - 1) / math.max(1, y5));
        if (num17 + num18 > num16)
        {
          if (num13 > x4)
            num17 = num16 - num18;
          else if (x4 > num13)
          {
            num18 = num16 - num17;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LeftHandTraffic)
              num17 = num16 - num18;
            else
              num18 = num16 - num17;
          }
        }
        int num19 = sourceBuffer.Length - b2 - x7 - x8 - num17 - num18;
        if (num19 > 0)
        {
          if (y4 > 0)
            b2 += num19;
          else if (y5 > 0)
          {
            int num20 = (num19 * x4 + y5 - 1) / y5;
            int num21 = (num19 * num13 + y5 - 1) / y5;
            if (num20 + num21 > num19)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                num20 = num19 - num21;
              else
                num21 = num19 - num20;
            }
            num17 += num20;
            num18 += num21;
          }
          else if (y6 > 0)
          {
            int num22 = (num19 * x5 + y6 - 1) / y6;
            int num23 = (num19 * x6 + y6 - 1) / y6;
            if (num22 + num23 > num19)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                num22 = num19 - num23;
              else
                num23 = num19 - num22;
            }
            x7 += num22;
            x8 += num23;
          }
          else if (b1 > 0)
            b2 += num19;
          else if (num11 > 0)
          {
            int num24 = (num19 * num4 + num11 - 1) / num11;
            int num25 = (num19 * num7 + num11 - 1) / num11;
            if (num24 + num25 > num19)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                num24 = num19 - num25;
              else
                num25 = num19 - num24;
            }
            num17 += num24;
            num18 += num25;
          }
          else if (num10 > 0)
          {
            int num26 = (num19 * index1 + num10 - 1) / num10;
            int num27 = (num19 * num2 + num10 - 1) / num10;
            if (num26 + num27 > num19)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                num26 = num19 - num27;
              else
                num27 = num19 - num26;
            }
            x7 += num26;
            x8 += num27;
          }
          else
            b2 += num19;
        }
        int num28 = math.max(x7, math.select(0, 1, index1 != 0));
        int num29 = math.max(x8, math.select(0, 1, num2 != 0));
        int num30 = num17 + math.select(0, 1, num4 > num17 & sourceBuffer.Length > num17);
        int num31 = num18 + math.select(0, 1, num7 > num18 & sourceBuffer.Length > num18);
        if (b2 == 0 && num30 > num17 && num31 > num18)
        {
          if (num31 > num30)
            num31 = math.max(1, num31 - 1);
          else if (num30 > num31)
          {
            num30 = math.max(1, num30 - 1);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_LeftHandTraffic ? (num30 <= 1 ? 1 : 0) : (num31 > 1 ? 1 : 0)) != 0)
              num31 = math.max(1, num31 - 1);
            else
              num30 = math.max(1, num30 - 1);
          }
        }
        int index3;
        for (int index4 = 0; index4 < targetBuffer.Length; index4 = index3)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition1 = targetBuffer[index4];
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition2 = targetPosition1;
          for (index3 = index4 + 1; index3 < targetBuffer.Length; ++index3)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = targetBuffer[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) connectPosition.m_GroupIndex == (int) targetPosition1.m_GroupIndex)
              targetPosition2 = connectPosition;
            else
              break;
          }
          int y7 = index3 - index4;
          int num32 = targetBuffer.Length - index3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          uint group = (uint) sourcePosition1.m_GroupIndex | (uint) targetPosition1.m_GroupIndex << 16;
          bool isTurn = index4 < index1 + num4 | num32 < num2 + num7;
          bool isUTurn = index4 < index1 | num32 < num2;
          bool flag3 = index4 < x1 | num32 < x2;
          bool isForbidden = index4 < y1 + num5 | num32 < y2 + num8;
          bool isRight = num32 < num2 + num7;
          bool isGentle = false;
          int b3;
          int a;
          bool flag4;
          bool flag5;
          if (isTurn)
          {
            if (isUTurn)
            {
              if (isRight)
              {
                int num33 = num32;
                int num34 = (num33 * num29 + math.select(0, num2 - 1, num29 > num2)) / num2;
                b3 = ((num33 + y7) * num29 + num2 - 1) / num2 - 1;
                a = sourceBuffer.Length - num34 - 1;
                b3 = sourceBuffer.Length - b3 - 1;
                CommonUtils.Swap<int>(ref a, ref b3);
              }
              else
              {
                int num35 = index4;
                a = (num35 * num28 + math.select(0, index1 - 1, num28 > index1)) / index1;
                b3 = ((num35 + y7) * num28 + index1 - 1) / index1 - 1;
              }
            }
            else if (isRight)
            {
              int num36 = num32 - num2;
              int num37 = (num36 * num31 + math.select(0, num7 - 1, num31 > num7)) / num7;
              b3 = ((num36 + y7) * num31 + num7 - 1) / num7 - 1;
              a = sourceBuffer.Length - x8 - num37 - 1;
              b3 = sourceBuffer.Length - x8 - b3 - 1;
              CommonUtils.Swap<int>(ref a, ref b3);
              bool gentle1;
              // ISSUE: reference to a compiler-generated method
              this.IsTurn(sourceBuffer[a], targetPosition1, out flag4, out gentle1, out flag5);
              bool gentle2;
              // ISSUE: reference to a compiler-generated method
              this.IsTurn(sourceBuffer[b3], targetPosition2, out flag5, out gentle2, out flag4);
              isGentle = gentle1 & gentle2;
            }
            else
            {
              int num38 = index4 - index1;
              int num39 = (num38 * num30 + math.select(0, num4 - 1, num30 > num4)) / num4;
              b3 = ((num38 + y7) * num30 + num4 - 1) / num4 - 1;
              a = x7 + num39;
              b3 = x7 + b3;
              bool gentle3;
              // ISSUE: reference to a compiler-generated method
              this.IsTurn(sourceBuffer[a], targetPosition1, out flag4, out gentle3, out flag5);
              bool gentle4;
              // ISSUE: reference to a compiler-generated method
              this.IsTurn(sourceBuffer[b3], targetPosition2, out flag5, out gentle4, out flag4);
              isGentle = gentle3 & gentle4;
            }
          }
          else
          {
            int num40 = index4 - index1 - num4;
            if (b2 == 0)
            {
              // ISSUE: reference to a compiler-generated field
              a = !this.m_LeftHandTraffic ? math.min(x7 + num17, sourceBuffer.Length - 1) : math.max(x7 + num17 - 1, 0);
              b3 = a;
            }
            else
            {
              int num41 = (num40 * b2 + math.select(0, b1 - 1, b2 > b1)) / b1;
              b3 = ((num40 + y7) * b2 + b1 - 1) / b1 - 1;
              a = x7 + num17 + num41;
              b3 = x7 + num17 + b3;
            }
            if (num12 > 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_LeftHandTraffic)
                isForbidden |= num40 < num12;
              else
                isForbidden |= b1 - num40 - 1 < num12;
            }
          }
          int x9 = b3 - a + 1;
          int num42 = math.max(x9, y7);
          int num43 = math.min(x9, y7);
          int num44 = 0;
          int num45 = num42 - num43;
          int num46 = 0;
          int num47 = 0;
          float num48 = float.MaxValue;
          int2 x10 = (int2) 0;
          if (y7 > x9)
          {
            // ISSUE: reference to a compiler-generated method
            x10 = this.CalculateSourcesBetween(sourceBuffer[a], targetBuffer[index4], allSources);
            if (math.any(x10 >= 1))
            {
              int num49 = math.csum(x10);
              int x11;
              int x12;
              if (num49 > num45)
              {
                x11 = x10.x * num45 / num49;
                x12 = x10.y * num45 / num49;
                if (num45 >= 2 & math.all(x10 >= 1))
                {
                  x11 = math.max(x11, 1);
                  x12 = math.max(x12, 1);
                }
              }
              else
              {
                x11 = x10.x;
                x12 = x10.y;
              }
              num44 += x11;
              num45 -= x12;
            }
          }
          for (int index5 = num44; index5 <= num45; ++index5)
          {
            int num50 = math.max(index5 + x9 - num42, 0);
            int num51 = math.max(index5 + y7 - num42, 0);
            int index6 = num50 + a;
            int index7 = num51 + index4;
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition1 = sourceBuffer[index6];
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition2 = sourceBuffer[index6 + num43 - 1];
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition3 = targetBuffer[index7];
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition4 = targetBuffer[index7 + num43 - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num52 = math.max(0.0f, math.dot(connectPosition1.m_Tangent, connectPosition3.m_Tangent) * -0.5f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num53 = math.max(0.0f, math.dot(connectPosition2.m_Tangent, connectPosition4.m_Tangent) * -0.5f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num54 = num52 * math.distance(connectPosition1.m_Position.xz, connectPosition3.m_Position.xz);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num55 = num53 * math.distance(connectPosition2.m_Position.xz, connectPosition4.m_Position.xz);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition1.m_Position.xz += connectPosition1.m_Tangent.xz * num54;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition3.m_Position.xz += connectPosition3.m_Tangent.xz * num54;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_Position.xz += connectPosition2.m_Tangent.xz * num55;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition4.m_Position.xz += connectPosition4.m_Tangent.xz * num55;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num56 = math.max(math.distancesq(connectPosition1.m_Position.xz, connectPosition3.m_Position.xz), math.distancesq(connectPosition2.m_Position.xz, connectPosition4.m_Position.xz));
            if ((double) num56 < (double) num48)
            {
              num46 = math.min(num42 - y7 - index5, 0);
              num47 = math.min(num42 - x9 - index5, 0);
              num48 = num56;
            }
          }
          for (int laneIndex = 0; laneIndex < num42; ++laneIndex)
          {
            int num57 = math.clamp(laneIndex + num46, 0, x9 - 1);
            int num58 = math.clamp(laneIndex + num47, 0, y7 - 1);
            bool flag6 = laneIndex + num46 < 0;
            bool flag7 = laneIndex + num46 >= x9;
            bool flag8 = laneIndex + num47 < 0;
            bool flag9 = laneIndex + num47 >= y7;
            bool isMergeLeft = flag6 | flag8;
            bool isMergeRight = flag7 | flag9;
            bool isUnsafe = !isTurn ? isMergeLeft | isMergeRight | isForbidden : (!isRight ? isMergeLeft | isMergeRight | flag3 | isForbidden | isUTurn & x7 == 0 : isMergeLeft | isMergeRight | flag3 | isForbidden | isUTurn & x8 == 0);
            int index8 = num57 + a;
            int index9 = num58 + index4;
            bool isLeftLimit = index8 == 0 & index9 == 0;
            bool isRightLimit = index8 == sourceBuffer.Length - 1 & index9 == targetBuffer.Length - 1;
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition sourcePosition3 = sourceBuffer[index8];
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition targetPosition3 = targetBuffer[index9];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((sourcePosition3.m_CompositionData.m_RoadFlags & targetPosition3.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) == (Game.Prefabs.RoadFlags) 0 || !(flag6 & x10.x > 0 | flag7 & x10.y > 0))
            {
              float curviness = -1f;
              // ISSUE: reference to a compiler-generated method
              if (this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition3, targetPosition3, group, (ushort) laneIndex, isUnsafe, isForbidden, isTemp, false, yield, ownerTemp, isTurn, isRight, isGentle, isUTurn, false, isLeftLimit, isRightLimit, isMergeLeft, isMergeRight, false, roadPassThrough))
              {
                // ISSUE: object of a compiler-generated type is created
                createdConnections.Add(new LaneSystem.ConnectionKey(sourcePosition3, targetPosition3));
              }
              if (isForbidden)
              {
                // ISSUE: reference to a compiler-generated field
                ++targetPosition3.m_UnsafeCount;
                // ISSUE: reference to a compiler-generated field
                ++targetPosition3.m_ForbiddenCount;
                targetBuffer[index9] = targetPosition3;
              }
              else if (isUTurn & (isRight ? x8 : x7) == 0)
              {
                // ISSUE: reference to a compiler-generated field
                ++targetPosition3.m_UnsafeCount;
                targetBuffer[index9] = targetPosition3;
              }
            }
          }
        }
      }

      private void ProcessTrackConnectPositions(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        NativeParallelHashSet<LaneSystem.ConnectionKey> createdConnections,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        bool isTemp,
        Temp ownerTemp)
      {
        NativeList<LaneSystem.ConnectPosition> list = sourceBuffer;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.SourcePositionComparer positionComparer = new LaneSystem.SourcePositionComparer();
        // ISSUE: variable of a compiler-generated type
        LaneSystem.SourcePositionComparer comp = positionComparer;
        list.Sort<LaneSystem.ConnectPosition, LaneSystem.SourcePositionComparer>(comp);
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition sourcePosition = sourceBuffer[0];
        // ISSUE: reference to a compiler-generated method
        this.SortTargets(sourcePosition, targetBuffer);
        // ISSUE: reference to a compiler-generated method
        this.CreateNodeTrackLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, middleConnections, createdConnections, sourceBuffer, targetBuffer, isTemp, ownerTemp);
      }

      private void CreateNodeTrackLanes(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        NativeParallelHashSet<LaneSystem.ConnectionKey> createdConnections,
        NativeList<LaneSystem.ConnectPosition> sourceBuffer,
        NativeList<LaneSystem.ConnectPosition> targetBuffer,
        bool isTemp,
        Temp ownerTemp)
      {
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition connectPosition1 = sourceBuffer[0];
        for (int index = 1; index < sourceBuffer.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition2 = sourceBuffer[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          connectPosition1.m_Position += connectPosition2.m_Position;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          connectPosition1.m_Tangent += connectPosition2.m_Tangent;
        }
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Position /= (float) sourceBuffer.Length;
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Tangent.y = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        connectPosition1.m_Tangent = math.normalizesafe(connectPosition1.m_Tangent);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TrackLaneData trackLaneData = this.m_TrackLaneData[connectPosition1.m_LaneData.m_Lane];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData = this.m_PrefabCompositionData[connectPosition1.m_NodeComposition];
        int x1 = targetBuffer.Length;
        int x2 = 0;
        // ISSUE: variable of a compiler-generated type
        LaneSystem.ConnectPosition connectPosition3 = targetBuffer[0];
        int y1 = 0;
        for (int index = 1; index < targetBuffer.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition4 = targetBuffer[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (connectPosition4.m_Owner.Equals(connectPosition3.m_Owner))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition3.m_Position += connectPosition4.m_Position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition3.m_Tangent += connectPosition4.m_Tangent;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            connectPosition3.m_Position /= (float) (index - y1);
            // ISSUE: reference to a compiler-generated field
            connectPosition3.m_Tangent.y = 0.0f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition3.m_Tangent = math.normalizesafe(connectPosition3.m_Tangent);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!connectPosition3.m_Owner.Equals(connectPosition1.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float distance = math.max(1f, math.distance(connectPosition1.m_Position, connectPosition3.m_Position));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((double) NetUtils.CalculateCurviness(connectPosition1.m_Tangent, -connectPosition3.m_Tangent, distance) <= (double) trackLaneData.m_MaxCurviness)
              {
                x1 = math.min(x1, y1);
                x2 = math.max(x2, index - x1);
              }
            }
            connectPosition3 = connectPosition4;
            y1 = index;
          }
        }
        // ISSUE: reference to a compiler-generated field
        connectPosition3.m_Position /= (float) (targetBuffer.Length - y1);
        // ISSUE: reference to a compiler-generated field
        connectPosition3.m_Tangent.y = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        connectPosition3.m_Tangent = math.normalizesafe(connectPosition3.m_Tangent);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!connectPosition3.m_Owner.Equals(connectPosition1.m_Owner))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float distance = math.max(1f, math.distance(connectPosition1.m_Position, connectPosition3.m_Position));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) NetUtils.CalculateCurviness(connectPosition1.m_Tangent, -connectPosition3.m_Tangent, distance) <= (double) trackLaneData.m_MaxCurviness)
          {
            x1 = math.min(x1, y1);
            x2 = math.max(x2, targetBuffer.Length - x1);
          }
        }
        for (int index1 = 0; index1 < sourceBuffer.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition sourcePosition = sourceBuffer[index1];
          for (int index2 = 0; index2 < x2; ++index2)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition targetPosition = targetBuffer[x1 + index2];
            // ISSUE: object of a compiler-generated type is created
            if (!createdConnections.Contains(new LaneSystem.ConnectionKey(sourcePosition, targetPosition)))
            {
              if ((netCompositionData.m_Flags.m_General & CompositionFlags.General.Intersection) == (CompositionFlags.General) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num = math.distance(sourcePosition.m_Position, targetPosition.m_Position);
                if ((double) num > 1.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  float3 x3 = math.normalizesafe(sourcePosition.m_Tangent);
                  // ISSUE: reference to a compiler-generated field
                  float3 y2 = -math.normalizesafe(targetPosition.m_Tangent);
                  if ((double) math.dot(x3, y2) > 0.99000000953674316)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    float3 float3 = (targetPosition.m_Position - sourcePosition.m_Position) / num;
                    if ((double) math.min(math.dot(x3, float3), math.dot(float3, y2)) < 0.0099999997764825821)
                      continue;
                  }
                }
              }
              bool isLeftLimit = x1 + index2 == 0;
              bool isRightLimit = x1 + index2 == targetBuffer.Length - 1;
              bool right;
              bool gentle;
              bool uturn;
              // ISSUE: reference to a compiler-generated method
              bool isTurn = this.IsTurn(sourcePosition, targetPosition, out right, out gentle, out uturn);
              float curviness = -1f;
              // ISSUE: reference to a compiler-generated method
              this.CreateNodeLane(jobIndex, ref nodeLaneIndex, ref random, ref curviness, owner, laneBuffer, middleConnections, sourcePosition, targetPosition, 0U, (ushort) 0, false, false, isTemp, true, 0, ownerTemp, isTurn, right, gentle, uturn, false, isLeftLimit, isRightLimit, false, false, false, RoadTypes.None);
            }
          }
        }
      }

      private bool IsTurn(
        LaneSystem.ConnectPosition sourcePosition,
        LaneSystem.ConnectPosition targetPosition,
        out bool right,
        out bool gentle,
        out bool uturn)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return NetUtils.IsTurn(sourcePosition.m_Position.xz, sourcePosition.m_Tangent.xz, targetPosition.m_Position.xz, targetPosition.m_Tangent.xz, out right, out gentle, out uturn);
      }

      private void ModifyCurveHeight(
        ref Bezier4x3 curve,
        float startBaseHeight,
        float endBaseHeight,
        NetCompositionData startCompositionData,
        NetCompositionData endCompositionData)
      {
        float num1 = startBaseHeight + startCompositionData.m_SurfaceHeight.min;
        float num2 = endBaseHeight + endCompositionData.m_SurfaceHeight.min;
        float num3 = math.max(curve.a.y, curve.d.y);
        float num4 = math.min(curve.a.y, curve.d.y);
        if ((startCompositionData.m_Flags.m_General & (CompositionFlags.General.Roundabout | CompositionFlags.General.LevelCrossing)) != (CompositionFlags.General) 0)
          curve.b.y += (float) (((double) math.max(0.0f, num4 - curve.b.y) - (double) math.max(0.0f, curve.b.y - num3)) * 0.66666668653488159);
        if ((endCompositionData.m_Flags.m_General & (CompositionFlags.General.Roundabout | CompositionFlags.General.LevelCrossing)) != (CompositionFlags.General) 0)
          curve.c.y += (float) (((double) math.max(0.0f, num4 - curve.c.y) - (double) math.max(0.0f, curve.c.y - num3)) * 0.66666668653488159);
        curve.b.y += math.max(0.0f, num1 - math.max(curve.a.y, curve.b.y)) * 1.33333337f;
        curve.c.y += math.max(0.0f, num2 - math.max(curve.d.y, curve.c.y)) * 1.33333337f;
      }

      private bool CanConnectTrack(
        bool isUTurn,
        bool isRoundabout,
        LaneSystem.ConnectPosition sourcePosition,
        LaneSystem.ConnectPosition targetPosition,
        PrefabRef prefabRef)
      {
        TrackLaneData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (isUTurn | isRoundabout || ((sourcePosition.m_LaneData.m_Flags | targetPosition.m_LaneData.m_Flags) & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0 || (sourcePosition.m_LaneData.m_Flags & targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Track) == (Game.Prefabs.LaneFlags) 0 || !this.m_TrackLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData))
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float distance = math.max(1f, math.distance(sourcePosition.m_Position, targetPosition.m_Position));
        // ISSUE: reference to a compiler-generated field
        sourcePosition.m_Tangent.y = 0.0f;
        // ISSUE: reference to a compiler-generated field
        targetPosition.m_Tangent.y = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (double) NetUtils.CalculateCurviness(sourcePosition.m_Tangent, -targetPosition.m_Tangent, distance) <= (double) componentData.m_MaxCurviness && sourcePosition.m_TrackTypes == targetPosition.m_TrackTypes;
      }

      private bool CheckPrefab(
        ref Entity prefab,
        ref Unity.Mathematics.Random random,
        out Unity.Mathematics.Random outRandom,
        LaneSystem.LaneBuffer laneBuffer)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode)
        {
          DynamicBuffer<PlaceholderObjectElement> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceholderObjects.TryGetBuffer(prefab, out bufferData1))
          {
            float num1 = -1f;
            Entity entity1 = Entity.Null;
            Entity key1 = Entity.Null;
            Unity.Mathematics.Random random1 = new Unity.Mathematics.Random();
            int max = 0;
            for (int index1 = 0; index1 < bufferData1.Length; ++index1)
            {
              Entity entity2 = bufferData1[index1].m_Object;
              float num2 = 0.0f;
              DynamicBuffer<ObjectRequirementElement> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ObjectRequirements.TryGetBuffer(entity2, out bufferData2))
              {
                int num3 = -1;
                bool flag = true;
                for (int index2 = 0; index2 < bufferData2.Length; ++index2)
                {
                  ObjectRequirementElement requirementElement = bufferData2[index2];
                  if ((int) requirementElement.m_Group != num3)
                  {
                    if (flag)
                    {
                      num3 = (int) requirementElement.m_Group;
                      flag = false;
                    }
                    else
                      break;
                  }
                  // ISSUE: reference to a compiler-generated field
                  flag |= requirementElement.m_Requirement == this.m_DefaultTheme;
                }
                if (!flag)
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              SpawnableObjectData spawnableObjectData = this.m_PrefabSpawnableObjectData[entity2];
              Entity key2 = spawnableObjectData.m_RandomizationGroup != Entity.Null ? spawnableObjectData.m_RandomizationGroup : entity2;
              Unity.Mathematics.Random random2 = random;
              random.NextInt();
              random.NextInt();
              Unity.Mathematics.Random random3;
              // ISSUE: reference to a compiler-generated field
              if (laneBuffer.m_SelectedSpawnables.TryGetValue(key2, out random3))
              {
                num2 += 0.5f;
                random2 = random3;
              }
              if ((double) num2 > (double) num1)
              {
                num1 = num2;
                entity1 = entity2;
                key1 = key2;
                random1 = random2;
                max = spawnableObjectData.m_Probability;
              }
              else if ((double) num2 == (double) num1)
              {
                max += spawnableObjectData.m_Probability;
                if (random.NextInt(max) < spawnableObjectData.m_Probability)
                {
                  entity1 = entity2;
                  key1 = key2;
                  random1 = random2;
                }
              }
            }
            if (random.NextInt(100) < max)
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_SelectedSpawnables.TryAdd(key1, random1);
              prefab = entity1;
              outRandom = random1;
              return true;
            }
            outRandom = random;
            random.NextInt();
            random.NextInt();
            return false;
          }
          Entity key = prefab;
          SpawnableObjectData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSpawnableObjectData.TryGetComponent(prefab, out componentData) && componentData.m_RandomizationGroup != Entity.Null)
            key = componentData.m_RandomizationGroup;
          outRandom = random;
          random.NextInt();
          random.NextInt();
          Unity.Mathematics.Random random4;
          // ISSUE: reference to a compiler-generated field
          if (laneBuffer.m_SelectedSpawnables.TryGetValue(key, out random4))
          {
            outRandom = random4;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_SelectedSpawnables.TryAdd(key, outRandom);
          }
          return true;
        }
        outRandom = random;
        random.NextInt();
        random.NextInt();
        return true;
      }

      private bool CreateNodeLane(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        ref float curviness,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        LaneSystem.ConnectPosition sourcePosition,
        LaneSystem.ConnectPosition targetPosition,
        uint group,
        ushort laneIndex,
        bool isUnsafe,
        bool isForbidden,
        bool isTemp,
        bool trackOnly,
        int yield,
        Temp ownerTemp,
        bool isTurn,
        bool isRight,
        bool isGentle,
        bool isUTurn,
        bool isRoundabout,
        bool isLeftLimit,
        bool isRightLimit,
        bool isMergeLeft,
        bool isMergeRight,
        bool fixedTangents,
        RoadTypes roadPassThrough)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[sourcePosition.m_NodeComposition];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[targetPosition.m_NodeComposition];
        if (isUTurn && (netCompositionData1.m_State & CompositionState.BlockUTurn) != (CompositionState) 0)
          return false;
        Owner component1 = new Owner();
        component1.m_Owner = owner;
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        PrefabRef prefabRef = new PrefabRef();
        Game.Prefabs.LaneFlags flags;
        if (trackOnly)
        {
          // ISSUE: reference to a compiler-generated field
          flags = sourcePosition.m_LaneData.m_Flags & ~(Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
          // ISSUE: reference to a compiler-generated field
          prefabRef.m_Prefab = sourcePosition.m_LaneData.m_Lane;
          if ((flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track)) == (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track))
          {
            flags &= ~Game.Prefabs.LaneFlags.Road;
            // ISSUE: reference to a compiler-generated field
            prefabRef.m_Prefab = this.m_TrackLaneData[prefabRef.m_Prefab].m_FallbackPrefab;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((sourcePosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            flags = sourcePosition.m_LaneData.m_Flags;
            // ISSUE: reference to a compiler-generated field
            prefabRef.m_Prefab = sourcePosition.m_LaneData.m_Lane;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              flags = targetPosition.m_LaneData.m_Flags;
              // ISSUE: reference to a compiler-generated field
              prefabRef.m_Prefab = targetPosition.m_LaneData.m_Lane;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((sourcePosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                flags = sourcePosition.m_LaneData.m_Flags;
                // ISSUE: reference to a compiler-generated field
                prefabRef.m_Prefab = sourcePosition.m_LaneData.m_Lane;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if ((targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  flags = targetPosition.m_LaneData.m_Flags;
                  // ISSUE: reference to a compiler-generated field
                  prefabRef.m_Prefab = targetPosition.m_LaneData.m_Lane;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  flags = sourcePosition.m_LaneData.m_Flags;
                  // ISSUE: reference to a compiler-generated field
                  prefabRef.m_Prefab = sourcePosition.m_LaneData.m_Lane;
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int a1 = math.select(0, 1, (sourcePosition.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int a2 = math.select(0, 1, (targetPosition.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabCompositionData.HasComponent(sourcePosition.m_EdgeComposition))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData3 = this.m_PrefabCompositionData[sourcePosition.m_EdgeComposition];
            int a3 = math.select(a1, a1 + 2, (netCompositionData3.m_Flags.m_General & CompositionFlags.General.Tiles) > (CompositionFlags.General) 0);
            a1 = math.select(a3, a3 - 4, (netCompositionData3.m_Flags.m_General & CompositionFlags.General.Gravel) > (CompositionFlags.General) 0);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabCompositionData.HasComponent(targetPosition.m_EdgeComposition))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData4 = this.m_PrefabCompositionData[targetPosition.m_EdgeComposition];
            int a4 = math.select(a2, a2 + 2, (netCompositionData4.m_Flags.m_General & CompositionFlags.General.Tiles) > (CompositionFlags.General) 0);
            a2 = math.select(a4, a4 - 4, (netCompositionData4.m_Flags.m_General & CompositionFlags.General.Gravel) > (CompositionFlags.General) 0);
          }
          // ISSUE: reference to a compiler-generated field
          if (a1 > a2 && prefabRef.m_Prefab != sourcePosition.m_LaneData.m_Lane)
          {
            // ISSUE: reference to a compiler-generated field
            flags = flags & (Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master) | sourcePosition.m_LaneData.m_Flags & ~(Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
            // ISSUE: reference to a compiler-generated field
            prefabRef.m_Prefab = sourcePosition.m_LaneData.m_Lane;
          }
          // ISSUE: reference to a compiler-generated field
          if (a2 > a1 && prefabRef.m_Prefab != targetPosition.m_LaneData.m_Lane)
          {
            // ISSUE: reference to a compiler-generated field
            flags = flags & (Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master) | targetPosition.m_LaneData.m_Flags & ~(Game.Prefabs.LaneFlags.Slave | Game.Prefabs.LaneFlags.Master);
            // ISSUE: reference to a compiler-generated field
            prefabRef.m_Prefab = targetPosition.m_LaneData.m_Lane;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.PublicOnly)) == (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.PublicOnly) && (sourcePosition.m_LaneData.m_Flags & targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.PublicOnly) == (Game.Prefabs.LaneFlags) 0)
          {
            flags &= ~Game.Prefabs.LaneFlags.PublicOnly;
            // ISSUE: reference to a compiler-generated field
            prefabRef.m_Prefab = this.m_CarLaneData[prefabRef.m_Prefab].m_NotBusLanePrefab;
          }
          // ISSUE: reference to a compiler-generated method
          if ((flags & (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track)) == (Game.Prefabs.LaneFlags.Road | Game.Prefabs.LaneFlags.Track) && !this.CanConnectTrack(isUTurn, isRoundabout, sourcePosition, targetPosition, prefabRef))
          {
            flags &= ~Game.Prefabs.LaneFlags.Track;
            // ISSUE: reference to a compiler-generated field
            prefabRef.m_Prefab = this.m_CarLaneData[prefabRef.m_Prefab].m_NotTrackLanePrefab;
          }
        }
        Unity.Mathematics.Random outRandom;
        // ISSUE: reference to a compiler-generated method
        this.CheckPrefab(ref prefabRef.m_Prefab, ref random, out outRandom, laneBuffer);
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData1 = this.m_NetLaneData[prefabRef.m_Prefab];
        DynamicBuffer<AuxiliaryNetLane> dynamicBuffer = new DynamicBuffer<AuxiliaryNetLane>();
        int num1 = 0;
        if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.HasAuxiliary) != (Game.Prefabs.LaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          dynamicBuffer = this.m_PrefabAuxiliaryLanes[prefabRef.m_Prefab];
          num1 += dynamicBuffer.Length;
        }
        // ISSUE: reference to a compiler-generated field
        float3 position1 = sourcePosition.m_Position;
        // ISSUE: reference to a compiler-generated field
        float3 position2 = targetPosition.m_Position;
        for (int index1 = 0; index1 <= num1; ++index1)
        {
          if (index1 != 0)
          {
            AuxiliaryNetLane auxiliaryNetLane1 = dynamicBuffer[index1 - 1];
            if (NetCompositionHelpers.TestLaneFlags(auxiliaryNetLane1, netCompositionData1.m_Flags) && NetCompositionHelpers.TestLaneFlags(auxiliaryNetLane1, netCompositionData2.m_Flags) && (double) auxiliaryNetLane1.m_Spacing.x <= 0.10000000149011612)
            {
              // ISSUE: reference to a compiler-generated field
              sourcePosition.m_Position = position1;
              // ISSUE: reference to a compiler-generated field
              targetPosition.m_Position = position2;
              // ISSUE: reference to a compiler-generated field
              sourcePosition.m_Position.y += auxiliaryNetLane1.m_Position.y;
              // ISSUE: reference to a compiler-generated field
              targetPosition.m_Position.y += auxiliaryNetLane1.m_Position.y;
              prefabRef.m_Prefab = auxiliaryNetLane1.m_Prefab;
              // ISSUE: reference to a compiler-generated field
              netLaneData1 = this.m_NetLaneData[prefabRef.m_Prefab];
              flags = netLaneData1.m_Flags | auxiliaryNetLane1.m_Flags;
              if ((double) auxiliaryNetLane1.m_Position.z > 0.10000000149011612)
              {
                // ISSUE: reference to a compiler-generated field
                if (sourcePosition.m_Owner != owner)
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((sourcePosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.HasAuxiliary) != (Game.Prefabs.LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<AuxiliaryNetLane> prefabAuxiliaryLane = this.m_PrefabAuxiliaryLanes[sourcePosition.m_LaneData.m_Lane];
                    for (int index2 = 0; index2 < prefabAuxiliaryLane.Length; ++index2)
                    {
                      AuxiliaryNetLane auxiliaryNetLane2 = prefabAuxiliaryLane[index2];
                      if (auxiliaryNetLane2.m_Prefab == auxiliaryNetLane1.m_Prefab)
                      {
                        auxiliaryNetLane1 = auxiliaryNetLane2;
                        break;
                      }
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EdgeNodeGeometry nodeGeometry = !sourcePosition.m_IsEnd ? this.m_StartNodeGeometryData[sourcePosition.m_Owner].m_Geometry : this.m_EndNodeGeometryData[sourcePosition.m_Owner].m_Geometry;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  sourcePosition.m_Position += this.CalculateAuxialryZOffset(sourcePosition.m_Position, sourcePosition.m_Tangent, nodeGeometry, netCompositionData1, auxiliaryNetLane1);
                }
                // ISSUE: reference to a compiler-generated field
                if (targetPosition.m_Owner != owner)
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.HasAuxiliary) != (Game.Prefabs.LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<AuxiliaryNetLane> prefabAuxiliaryLane = this.m_PrefabAuxiliaryLanes[targetPosition.m_LaneData.m_Lane];
                    for (int index3 = 0; index3 < prefabAuxiliaryLane.Length; ++index3)
                    {
                      AuxiliaryNetLane auxiliaryNetLane3 = prefabAuxiliaryLane[index3];
                      if (auxiliaryNetLane3.m_Prefab == auxiliaryNetLane1.m_Prefab)
                      {
                        auxiliaryNetLane1 = auxiliaryNetLane3;
                        break;
                      }
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EdgeNodeGeometry nodeGeometry = !targetPosition.m_IsEnd ? this.m_StartNodeGeometryData[targetPosition.m_Owner].m_Geometry : this.m_EndNodeGeometryData[targetPosition.m_Owner].m_Geometry;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  targetPosition.m_Position += this.CalculateAuxialryZOffset(targetPosition.m_Position, targetPosition.m_Tangent, nodeGeometry, netCompositionData2, auxiliaryNetLane1);
                }
              }
            }
            else
              continue;
          }
          NodeLane component2 = new NodeLane();
          if ((flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetLaneData.HasComponent(sourcePosition.m_LaneData.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NetLaneData netLaneData2 = this.m_NetLaneData[sourcePosition.m_LaneData.m_Lane];
              component2.m_WidthOffset.x = netLaneData2.m_Width - netLaneData1.m_Width;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetLaneData.HasComponent(targetPosition.m_LaneData.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NetLaneData netLaneData3 = this.m_NetLaneData[targetPosition.m_LaneData.m_Lane];
              component2.m_WidthOffset.y = netLaneData3.m_Width - netLaneData1.m_Width;
            }
          }
          Curve curve = new Curve();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          curve.m_Bezier = (double) math.distance(sourcePosition.m_Position, targetPosition.m_Position) < 0.0099999997764825821 ? NetUtils.StraightCurve(sourcePosition.m_Position, targetPosition.m_Position) : (!fixedTangents ? NetUtils.FitCurve(sourcePosition.m_Position, sourcePosition.m_Tangent, -targetPosition.m_Tangent, targetPosition.m_Position) : new Bezier4x3(sourcePosition.m_Position, sourcePosition.m_Position + sourcePosition.m_Tangent, targetPosition.m_Position + targetPosition.m_Tangent, targetPosition.m_Position));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ModifyCurveHeight(ref curve.m_Bezier, sourcePosition.m_BaseHeight, targetPosition.m_BaseHeight, netCompositionData1, netCompositionData2);
          UtilityLane component3 = new UtilityLane();
          HangingLane component4 = new HangingLane();
          bool flag1 = false;
          if ((flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            UtilityLaneData utilityLaneData = this.m_UtilityLaneData[prefabRef.m_Prefab];
            if ((double) utilityLaneData.m_Hanging != 0.0)
            {
              curve.m_Bezier.b = math.lerp(curve.m_Bezier.a, curve.m_Bezier.d, 0.333333343f);
              curve.m_Bezier.c = math.lerp(curve.m_Bezier.a, curve.m_Bezier.d, 0.6666667f);
              float num2 = (float) ((double) math.distance(curve.m_Bezier.a.xz, curve.m_Bezier.d.xz) * (double) utilityLaneData.m_Hanging * 1.3333333730697632);
              curve.m_Bezier.b.y -= num2;
              curve.m_Bezier.c.y -= num2;
              component4.m_Distances = (float2) 0.1f;
              flag1 = true;
            }
            if ((flags & Game.Prefabs.LaneFlags.FindAnchor) != (Game.Prefabs.LaneFlags) 0)
            {
              component3.m_Flags |= UtilityLaneFlags.SecondaryStartAnchor | UtilityLaneFlags.SecondaryEndAnchor;
              component4.m_Distances = (float2) 0.0f;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((flags & Game.Prefabs.LaneFlags.Road) != 0 & isUTurn && sourcePosition.m_Owner == targetPosition.m_Owner && (int) sourcePosition.m_LaneData.m_Carriageway != (int) targetPosition.m_LaneData.m_Carriageway)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetCompositionPiece> compositionPiece1 = this.m_PrefabCompositionPieces[sourcePosition.m_NodeComposition];
            float2 float2_1 = new float2(math.distance(curve.m_Bezier.a.xz, curve.m_Bezier.b.xz), math.distance(curve.m_Bezier.d.xz, curve.m_Bezier.c.xz));
            float2 x1 = (float2) float.MinValue;
            for (int index4 = 0; index4 < compositionPiece1.Length; ++index4)
            {
              NetCompositionPiece compositionPiece2 = compositionPiece1[index4];
              if ((compositionPiece2.m_PieceFlags & NetPieceFlags.BlockTraffic) != (NetPieceFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float2 x2 = new float2(sourcePosition.m_LaneData.m_Position.x, targetPosition.m_LaneData.m_Position.x);
                x2 -= compositionPiece2.m_Offset.x;
                bool2 bool2 = x2 > 0.0f;
                if (bool2.x != bool2.y)
                {
                  x2 = math.abs(x2) - (netLaneData1.m_Width + component2.m_WidthOffset);
                  x2 = (compositionPiece2.m_Size.z + compositionPiece2.m_Size.x * 0.5f - x2) * 1.33333337f;
                  x2 += math.max((float2) 0.0f, x2 - math.max((float2) 0.0f, x2.yx));
                  x1 = math.max(x1, x2 - float2_1);
                }
              }
            }
            if (math.any(x1 > 0.0f))
            {
              float2 float2_2 = math.max(x1, math.max(math.min((float2) 0.0f, -x1.yx), float2_1 * -0.5f));
              // ISSUE: reference to a compiler-generated field
              curve.m_Bezier.b += sourcePosition.m_Tangent * float2_2.x;
              // ISSUE: reference to a compiler-generated field
              curve.m_Bezier.c += targetPosition.m_Tangent * float2_2.y;
            }
          }
          curve.m_Length = MathUtils.Length(curve.m_Bezier);
          bool flag2 = false;
          CarLane component5 = new CarLane();
          if ((flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component5.m_DefaultSpeedLimit = math.lerp(sourcePosition.m_CompositionData.m_SpeedLimit, targetPosition.m_CompositionData.m_SpeedLimit, 0.5f);
            if ((double) curviness < 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              curviness = NetUtils.CalculateCurviness(curve, this.m_NetLaneData[prefabRef.m_Prefab].m_Width);
            }
            component5.m_Curviness = curviness;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool c = (sourcePosition.m_CompositionData.m_RoadFlags & targetPosition.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0;
            if (isUnsafe)
              component5.m_Flags |= CarLaneFlags.Unsafe;
            if (isForbidden)
              component5.m_Flags |= CarLaneFlags.Forbidden;
            if (isRoundabout)
              component5.m_Flags |= CarLaneFlags.Roundabout;
            if (isLeftLimit)
              component5.m_Flags |= CarLaneFlags.LeftLimit;
            if (isRightLimit)
              component5.m_Flags |= CarLaneFlags.RightLimit;
            if (c)
              component5.m_Flags |= CarLaneFlags.Highway;
            if ((netCompositionData1.m_Flags.m_General & CompositionFlags.General.Intersection) > (CompositionFlags.General) 0 | isRoundabout | isUTurn)
            {
              component5.m_CarriagewayGroup = (ushort) 0;
              component5.m_Flags |= CarLaneFlags.ForbidPassing;
              if (isTurn)
              {
                if (isGentle)
                  component5.m_Flags |= isRight ? CarLaneFlags.GentleTurnRight : CarLaneFlags.GentleTurnLeft;
                else if (isUTurn)
                  component5.m_Flags |= isRight ? CarLaneFlags.UTurnRight : CarLaneFlags.UTurnLeft;
                else
                  component5.m_Flags |= isRight ? CarLaneFlags.TurnRight : CarLaneFlags.TurnLeft;
              }
              else
                component5.m_Flags |= CarLaneFlags.Forward;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component5.m_CarriagewayGroup = sourcePosition.m_Owner.Index < targetPosition.m_Owner.Index ? (ushort) sourcePosition.m_LaneData.m_Carriageway : (ushort) targetPosition.m_LaneData.m_Carriageway;
              if (c)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabCompositionData.HasComponent(sourcePosition.m_EdgeComposition) && (this.m_PrefabCompositionData[sourcePosition.m_EdgeComposition].m_State & (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes | CompositionState.Multilane)) == (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes | CompositionState.Multilane))
                  component5.m_Flags |= CarLaneFlags.ForbidPassing;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabCompositionData.HasComponent(targetPosition.m_EdgeComposition) && (this.m_PrefabCompositionData[targetPosition.m_EdgeComposition].m_State & (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes | CompositionState.Multilane)) == (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes | CompositionState.Multilane))
                  component5.m_Flags |= CarLaneFlags.ForbidPassing;
              }
              if ((double) component5.m_Curviness > (double) math.select((float) Math.PI / 180f, (float) Math.PI / 360f, c))
                component5.m_Flags |= CarLaneFlags.ForbidPassing;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((sourcePosition.m_LaneData.m_Flags & targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (sourcePosition.m_Owner.Index >= targetPosition.m_Owner.Index)
                return false;
              component5.m_Flags |= CarLaneFlags.Twoway;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((sourcePosition.m_LaneData.m_Flags & targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.PublicOnly) != (Game.Prefabs.LaneFlags) 0)
              component5.m_Flags |= CarLaneFlags.PublicOnly;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((sourcePosition.m_CompositionData.m_TaxiwayFlags & targetPosition.m_CompositionData.m_TaxiwayFlags & TaxiwayFlags.Runway) != (TaxiwayFlags) 0)
              component5.m_Flags |= CarLaneFlags.Runway;
            switch (yield)
            {
              case -1:
                component5.m_Flags |= CarLaneFlags.RightOfWay;
                break;
              case 1:
                component5.m_Flags |= CarLaneFlags.Yield;
                break;
              case 2:
                component5.m_Flags |= CarLaneFlags.Stop;
                break;
            }
            if ((netCompositionData1.m_Flags.m_General & CompositionFlags.General.TrafficLights) != (CompositionFlags.General) 0)
            {
              component5.m_Flags |= CarLaneFlags.TrafficLights;
              flag2 = true;
            }
            if ((netCompositionData1.m_Flags.m_General & CompositionFlags.General.LevelCrossing) != (CompositionFlags.General) 0)
            {
              component5.m_Flags |= CarLaneFlags.LevelCrossing;
              flag2 = true;
            }
          }
          TrackLane component6 = new TrackLane();
          if ((flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component6.m_SpeedLimit = math.lerp(sourcePosition.m_CompositionData.m_SpeedLimit, targetPosition.m_CompositionData.m_SpeedLimit, 0.5f);
            if ((double) curviness < 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              curviness = NetUtils.CalculateCurviness(curve, this.m_NetLaneData[prefabRef.m_Prefab].m_Width);
            }
            component6.m_Curviness = curviness;
            TrackLaneData componentData;
            // ISSUE: reference to a compiler-generated field
            if ((double) component6.m_Curviness > 9.9999999747524271E-07 && this.m_TrackLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData))
              component6.m_Curviness = math.min(component6.m_Curviness, componentData.m_MaxCurviness);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((sourcePosition.m_LaneData.m_Flags & targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int num3 = sourcePosition.m_IsEnd == ((sourcePosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) == (Game.Prefabs.LaneFlags) 0) ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bool flag3 = targetPosition.m_IsEnd == ((targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Invert) == (Game.Prefabs.LaneFlags) 0);
              int num4 = flag3 ? 1 : 0;
              if (num3 != num4)
              {
                if (flag3)
                  return false;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (sourcePosition.m_Owner.Index >= targetPosition.m_Owner.Index)
                  return false;
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (((sourcePosition.m_LaneData.m_Flags | targetPosition.m_LaneData.m_Flags) & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
              component6.m_Flags |= TrackLaneFlags.Twoway;
            if ((flags & Game.Prefabs.LaneFlags.Road) == (Game.Prefabs.LaneFlags) 0)
              component6.m_Flags |= TrackLaneFlags.Exclusive;
            if ((netCompositionData1.m_Flags.m_General & CompositionFlags.General.TrafficLights) != (CompositionFlags.General) 0)
              flag2 = true;
            if ((netCompositionData1.m_Flags.m_General & CompositionFlags.General.LevelCrossing) != (CompositionFlags.General) 0)
            {
              component6.m_Flags |= TrackLaneFlags.LevelCrossing;
              flag2 = true;
            }
            if (((netCompositionData1.m_Flags.m_Left | netCompositionData1.m_Flags.m_Right | netCompositionData2.m_Flags.m_Left | netCompositionData2.m_Flags.m_Right) & (CompositionFlags.Side.PrimaryStop | CompositionFlags.Side.SecondaryStop)) != (CompositionFlags.Side) 0)
              component6.m_Flags |= TrackLaneFlags.Station;
            if ((netCompositionData1.m_Flags.m_General & CompositionFlags.General.Intersection) > (CompositionFlags.General) 0 & isTurn)
              component6.m_Flags |= isRight ? TrackLaneFlags.TurnRight : TrackLaneFlags.TurnLeft;
          }
          Lane lane = new Lane();
          ushort laneIndex1;
          if (index1 != 0)
          {
            lane.m_StartNode = new PathNode(owner, (ushort) nodeLaneIndex++);
            laneIndex1 = (ushort) nodeLaneIndex++;
            lane.m_MiddleNode = new PathNode(owner, laneIndex1);
            lane.m_EndNode = new PathNode(owner, (ushort) nodeLaneIndex++);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            lane.m_StartNode = new PathNode(sourcePosition.m_Owner, sourcePosition.m_LaneData.m_Index, sourcePosition.m_SegmentIndex);
            laneIndex1 = (ushort) nodeLaneIndex++;
            lane.m_MiddleNode = new PathNode(owner, laneIndex1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            lane.m_EndNode = new PathNode(targetPosition.m_Owner, targetPosition.m_LaneData.m_Index, targetPosition.m_SegmentIndex);
          }
          if ((component5.m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
          {
            for (int index5 = 0; index5 < middleConnections.Length; ++index5)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.MiddleConnection middleConnection1 = middleConnections[index5];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((middleConnection1.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0 && (!isRoundabout || roadPassThrough == RoadTypes.None || (!(sourcePosition.m_Owner == owner) || middleConnection1.m_IsSource) && (!(targetPosition.m_Owner == owner) || !middleConnection1.m_IsSource)))
              {
                Game.Prefabs.LaneFlags laneFlags = flags;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((middleConnection1.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
                {
                  if ((flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0)
                    laneFlags |= Game.Prefabs.LaneFlags.Master;
                  else
                    continue;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((middleConnection1.m_ConnectPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
                  {
                    if ((flags & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0)
                      laneFlags |= Game.Prefabs.LaneFlags.Slave;
                    else
                      continue;
                  }
                }
                uint num5;
                uint num6;
                if (isRoundabout)
                {
                  num5 = group | group << 16;
                  num6 = uint.MaxValue;
                }
                else if ((flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
                {
                  num5 = group;
                  num6 = uint.MaxValue;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (middleConnection1.m_IsSource)
                  {
                    num5 = group;
                    num6 = 4294901760U;
                  }
                  else
                  {
                    num5 = group;
                    num6 = (uint) ushort.MaxValue;
                  }
                }
                int index6 = index5;
                // ISSUE: reference to a compiler-generated field
                if (middleConnection1.m_TargetLane != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_Distance = float.MaxValue;
                  index6 = -1;
                  for (; index5 < middleConnections.Length; ++index5)
                  {
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.MiddleConnection middleConnection2 = middleConnections[index5];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (middleConnection2.m_SortIndex == middleConnection1.m_SortIndex)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if ((((int) middleConnection2.m_TargetGroup ^ (int) num5) & (int) num6) == 0 && ((middleConnection2.m_TargetFlags ^ laneFlags) & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0)
                      {
                        middleConnection1 = middleConnection2;
                        index6 = index5;
                      }
                    }
                    else
                      break;
                  }
                  --index5;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num7 = math.length(MathUtils.Size(MathUtils.Bounds(curve.m_Bezier) | middleConnection1.m_ConnectPosition.m_Position));
                float2 t;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num8 = MathUtils.Distance(curve.m_Bezier, new Line3.Segment(middleConnection1.m_ConnectPosition.m_Position, middleConnection1.m_ConnectPosition.m_Position + middleConnection1.m_ConnectPosition.m_Tangent * num7), out t) + num7 * t.y;
                if (roadPassThrough != RoadTypes.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  t.x = !middleConnection1.m_IsSource ? math.lerp(0.0f, t.x, 0.5f) : math.lerp(t.x, 1f, 0.5f);
                }
                // ISSUE: reference to a compiler-generated field
                if ((double) num8 < (double) middleConnection1.m_Distance)
                {
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_Distance = num8;
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetLane = prefabRef.m_Prefab;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetOwner = middleConnection1.m_IsSource ? sourcePosition.m_Owner : targetPosition.m_Owner;
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetGroup = num5;
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetIndex = laneIndex1;
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetCarriageway = component5.m_CarriagewayGroup;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetComposition = middleConnection1.m_IsSource ? targetPosition.m_CompositionData : sourcePosition.m_CompositionData;
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetCurve = curve;
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetCurvePos = t.x;
                  // ISSUE: reference to a compiler-generated field
                  middleConnection1.m_TargetFlags = laneFlags;
                  if (index6 != -1)
                    middleConnections[index6] = middleConnection1;
                  else
                    CollectionUtils.Insert<LaneSystem.MiddleConnection>(middleConnections, index5 + 1, middleConnection1);
                }
              }
            }
          }
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneKey key = new LaneSystem.LaneKey(lane, prefabRef.m_Prefab, flags);
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneKey laneKey = key;
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated method
            this.ReplaceTempOwner(ref laneKey, owner);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.ReplaceTempOwner(ref laneKey, sourcePosition.m_Owner);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.ReplaceTempOwner(ref laneKey, targetPosition.m_Owner);
            // ISSUE: reference to a compiler-generated method
            this.GetOriginalLane(laneBuffer, laneKey, ref temp);
          }
          PseudoRandomSeed componentData1 = new PseudoRandomSeed();
          // ISSUE: reference to a compiler-generated field
          if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0 && !this.m_PseudoRandomSeedData.TryGetComponent(temp.m_Original, out componentData1))
            componentData1 = new PseudoRandomSeed(ref outRandom);
          Entity entity1;
          // ISSUE: reference to a compiler-generated field
          if (laneBuffer.m_OldLanes.TryGetValue(key, out entity1))
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_OldLanes.Remove(key);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity1, component2);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, curve);
            if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PseudoRandomSeedData.HasComponent(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, entity1, componentData1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity1, componentData1);
              }
            }
            if ((flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity1, component5);
            }
            if ((flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<TrackLane>(jobIndex, entity1, component6);
            }
            if ((flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity1, component3);
            }
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<HangingLane>(jobIndex, entity1, component4);
            }
            if (isTemp)
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_Updates.Add(in entity1);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TempData.HasComponent(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                laneBuffer.m_Updates.Add(in entity1);
              }
            }
            if ((flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity1, new MasterLane()
              {
                m_Group = group
              });
            }
            if ((flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
            {
              SlaveLane component7 = new SlaveLane();
              component7.m_Group = group;
              component7.m_MinIndex = laneIndex;
              component7.m_MaxIndex = laneIndex;
              component7.m_SubIndex = laneIndex;
              if (isMergeLeft)
                component7.m_Flags |= SlaveLaneFlags.MergingLane;
              if (isMergeRight)
                component7.m_Flags |= SlaveLaneFlags.MergingLane;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity1, component7);
            }
            if (flag2)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_LaneSignalData.HasComponent(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<LaneSignal>(jobIndex, entity1, new LaneSignal());
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneSignalData.HasComponent(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<LaneSignal>(jobIndex, entity1);
              }
            }
          }
          else
          {
            EntityArchetype entityArchetype = new EntityArchetype();
            // ISSUE: reference to a compiler-generated field
            NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[prefabRef.m_Prefab];
            EntityArchetype archetype = (flags & Game.Prefabs.LaneFlags.Slave) == (Game.Prefabs.LaneFlags) 0 ? ((flags & Game.Prefabs.LaneFlags.Master) == (Game.Prefabs.LaneFlags) 0 ? laneArchetypeData.m_NodeLaneArchetype : laneArchetypeData.m_NodeMasterArchetype) : laneArchetypeData.m_NodeSlaveArchetype;
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, archetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, prefabRef);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, lane);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity2, component2);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, curve);
            if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, componentData1);
            }
            if ((flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CarLane>(jobIndex, entity2, component5);
            }
            if ((flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<TrackLane>(jobIndex, entity2, component6);
            }
            if ((flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<UtilityLane>(jobIndex, entity2, component3);
            }
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HangingLane>(jobIndex, entity2, component4);
            }
            if ((flags & Game.Prefabs.LaneFlags.Master) != (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<MasterLane>(jobIndex, entity2, new MasterLane()
              {
                m_Group = group
              });
            }
            if ((flags & Game.Prefabs.LaneFlags.Slave) != (Game.Prefabs.LaneFlags) 0)
            {
              SlaveLane component8 = new SlaveLane();
              component8.m_Group = group;
              component8.m_MinIndex = laneIndex;
              component8.m_MaxIndex = laneIndex;
              component8.m_SubIndex = laneIndex;
              if (isMergeLeft)
                component8.m_Flags |= SlaveLaneFlags.MergingLane;
              if (isMergeRight)
                component8.m_Flags |= SlaveLaneFlags.MergingLane;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<SlaveLane>(jobIndex, entity2, component8);
            }
            if (isTemp)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_TempOwnerTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity2, component1);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity2, temp);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component1);
            }
            if (flag2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LaneSignal>(jobIndex, entity2, new LaneSignal());
            }
          }
        }
        return true;
      }

      private float3 CalculateUtilityConnectPosition(
        NativeList<LaneSystem.ConnectPosition> buffer,
        int2 bufferRange)
      {
        float4 float4_1 = new float4();
        for (int index = bufferRange.x + 1; index < bufferRange.y; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition1 = buffer[index];
          // ISSUE: reference to a compiler-generated field
          float3 tangent1 = connectPosition1.m_Tangent;
          if ((double) math.lengthsq(tangent1.xz) > 0.0099999997764825821)
          {
            float3 float3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.HasComponent(connectPosition1.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 = this.m_NodeData[connectPosition1.m_Owner].m_Position;
              NodeGeometry componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_NodeGeometryData.TryGetComponent(connectPosition1.m_Owner, out componentData))
                float3.y = componentData.m_Position;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 = connectPosition1.m_SegmentIndex == (byte) 0 ? this.m_StartNodeGeometryData[connectPosition1.m_Owner].m_Geometry.m_Middle.d : this.m_EndNodeGeometryData[connectPosition1.m_Owner].m_Geometry.m_Middle.d;
            }
            for (int x1 = bufferRange.x; x1 < index; ++x1)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition connectPosition2 = buffer[x1];
              // ISSUE: reference to a compiler-generated field
              float3 tangent2 = connectPosition2.m_Tangent;
              if ((double) math.lengthsq(tangent2.xz) > 0.0099999997764825821)
              {
                float x2 = math.dot(tangent1.xz, tangent2.xz);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float2 float2 = math.distance(connectPosition1.m_Position.xz, connectPosition2.m_Position.xz) * new float2(math.max(0.0f, x2 * 0.5f), (float) (1.0 - (double) math.abs(x2) * 0.5));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Line2.Segment segment1 = new Line2.Segment(connectPosition1.m_Position.xz + tangent1.xz * float2.x, connectPosition1.m_Position.xz + tangent1.xz * float2.y);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Line2.Segment segment2 = new Line2.Segment(connectPosition2.m_Position.xz + tangent2.xz * float2.x, connectPosition2.m_Position.xz + tangent2.xz * float2.y);
                float2 t;
                double num1 = (double) MathUtils.Distance(segment1, segment2, out t);
                float4 float4_2 = new float4();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float4_2.y = float3.y + connectPosition1.m_Position.y - connectPosition1.m_BaseHeight + float3.y + connectPosition2.m_Position.y - connectPosition2.m_BaseHeight;
                float4_2.xz = MathUtils.Position(segment1, t.x) + MathUtils.Position(segment2, t.y);
                float4_2.w = 2f;
                float num2 = 1.01f - math.abs(x2);
                float4_1 += float4_2 * num2;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        return (double) float4_1.w == 0.0 ? buffer[bufferRange.x].m_Position : (float4_1 / float4_1.w).xyz;
      }

      private void CreateNodeUtilityLanes(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.ConnectPosition> buffer1,
        NativeList<LaneSystem.ConnectPosition> buffer2,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        float3 middlePosition,
        bool isRoundabout,
        bool isTemp,
        Temp ownerTemp)
      {
        if (buffer1.Length >= 2)
        {
          NativeList<LaneSystem.ConnectPosition> list1 = buffer1;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LaneSystem.SourcePositionComparer positionComparer1 = new LaneSystem.SourcePositionComparer();
          // ISSUE: variable of a compiler-generated type
          LaneSystem.SourcePositionComparer comp1 = positionComparer1;
          list1.Sort<LaneSystem.ConnectPosition, LaneSystem.SourcePositionComparer>(comp1);
          Entity owner1 = Entity.Null;
          int num1 = 0;
          int num2 = 0;
          int num3 = 0;
          for (int index = 0; index < buffer1.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = buffer1[index];
            // ISSUE: reference to a compiler-generated field
            if (connectPosition.m_Owner != owner1)
            {
              if (index - num1 > num2)
              {
                num3 = num1;
                num2 = index - num1;
              }
              // ISSUE: reference to a compiler-generated field
              owner1 = connectPosition.m_Owner;
              num1 = index;
            }
          }
          if (buffer1.Length - num1 > num2)
          {
            num3 = num1;
            num2 = buffer1.Length - num1;
          }
          for (int index = 0; index < num2; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = buffer1[num3 + index] with
            {
              m_Order = (float) index
            };
            buffer1[num3 + index] = buffer1[index];
            buffer1[index] = connectPosition;
          }
          for (int index1 = num2; index1 < buffer1.Length; ++index1)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition1 = buffer1[index1];
            float num4 = float.MaxValue;
            int num5 = 0;
            for (int index2 = 0; index2 < num2; ++index2)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition connectPosition2 = buffer1[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float num6 = math.distancesq(connectPosition1.m_Position, connectPosition2.m_Position);
              if ((double) num6 < (double) num4)
              {
                num4 = num6;
                num5 = index2;
              }
            }
            // ISSUE: reference to a compiler-generated field
            connectPosition1.m_Order = (float) num5;
            buffer1[index1] = connectPosition1;
          }
          NativeList<LaneSystem.ConnectPosition> list2 = buffer1;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LaneSystem.TargetPositionComparer positionComparer2 = new LaneSystem.TargetPositionComparer();
          // ISSUE: variable of a compiler-generated type
          LaneSystem.TargetPositionComparer comp2 = positionComparer2;
          list2.Sort<LaneSystem.ConnectPosition, LaneSystem.TargetPositionComparer>(comp2);
          float num7 = -1f;
          int x = 0;
          for (int index = 0; index < buffer1.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition = buffer1[index];
            // ISSUE: reference to a compiler-generated field
            if ((double) connectPosition.m_Order != (double) num7)
            {
              if (index - x > 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateNodeUtilityLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, buffer1, buffer2, middleConnections, new int2(x, index), middlePosition, isRoundabout, isTemp, ownerTemp);
              }
              // ISSUE: reference to a compiler-generated field
              num7 = connectPosition.m_Order;
              x = index;
            }
          }
          if (buffer1.Length <= x)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CreateNodeUtilityLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, buffer1, buffer2, middleConnections, new int2(x, buffer1.Length), middlePosition, isRoundabout, isTemp, ownerTemp);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateNodeUtilityLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, buffer1, buffer2, middleConnections, new int2(0, buffer1.Length), middlePosition, isRoundabout, isTemp, ownerTemp);
        }
      }

      private void CreateNodeUtilityLanes(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.ConnectPosition> buffer1,
        NativeList<LaneSystem.ConnectPosition> buffer2,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        int2 bufferRange,
        float3 middlePosition,
        bool isRoundabout,
        bool isTemp,
        Temp ownerTemp)
      {
        int num = bufferRange.y - bufferRange.x;
        if (num == 2 && buffer2.Length == 0)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition1 = buffer1[bufferRange.x];
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition2 = buffer1[bufferRange.x + 1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (connectPosition1.m_LaneData.m_Lane == connectPosition2.m_LaneData.m_Lane)
          {
            // ISSUE: reference to a compiler-generated method
            this.CreateNodeUtilityLane(jobIndex, -1, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, connectPosition2, middleConnections, isTemp, ownerTemp);
            return;
          }
        }
        if (num < 2 && (num <= 0 || buffer2.Length <= 0) && !(num == 1 & isRoundabout))
          return;
        float3 float3;
        if (num >= 2)
        {
          // ISSUE: reference to a compiler-generated method
          float3 = this.CalculateUtilityConnectPosition(buffer1, bufferRange);
        }
        else if (isRoundabout)
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = buffer1[bufferRange.x].m_Position;
          // ISSUE: reference to a compiler-generated field
          float3 tangent = buffer1[bufferRange.x].m_Tangent;
          float3 = (position + tangent * math.dot(tangent, middlePosition - position)) with
          {
            y = middlePosition.y
          };
          // ISSUE: reference to a compiler-generated field
          float3.y = middlePosition.y + position.y - buffer1[bufferRange.x].m_BaseHeight;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          float3 = buffer1[bufferRange.x].m_Position;
        }
        int endNodeLaneIndex = nodeLaneIndex++;
        if (num >= 2 | isRoundabout)
        {
          for (int x = bufferRange.x; x < bufferRange.y; ++x)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition1 = buffer1[x];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: reference to a compiler-generated method
            this.CreateNodeUtilityLane(jobIndex, endNodeLaneIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, new LaneSystem.ConnectPosition()
            {
              m_Position = float3
            }, middleConnections, isTemp, ownerTemp);
          }
        }
        for (int index = 0; index < buffer2.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition1 = buffer2[index];
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated method
          this.CreateNodeUtilityLane(jobIndex, endNodeLaneIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, new LaneSystem.ConnectPosition()
          {
            m_Position = float3
          }, middleConnections, isTemp, ownerTemp);
        }
      }

      private void CreateNodeUtilityLane(
        int jobIndex,
        int endNodeLaneIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        LaneSystem.ConnectPosition connectPosition1,
        LaneSystem.ConnectPosition connectPosition2,
        NativeList<LaneSystem.MiddleConnection> middleConnections,
        bool isTemp,
        Temp ownerTemp)
      {
        Owner component1 = new Owner();
        component1.m_Owner = owner;
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        Lane lane = new Lane();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        lane.m_StartNode = !(connectPosition1.m_Owner != Entity.Null) ? new PathNode(owner, (ushort) nodeLaneIndex++) : new PathNode(connectPosition1.m_Owner, connectPosition1.m_LaneData.m_Index, connectPosition1.m_SegmentIndex);
        ushort laneIndex = (ushort) nodeLaneIndex++;
        lane.m_MiddleNode = new PathNode(owner, laneIndex);
        // ISSUE: reference to a compiler-generated field
        if (connectPosition2.m_Owner != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lane.m_EndNode = new PathNode(connectPosition2.m_Owner, connectPosition2.m_LaneData.m_Index, connectPosition2.m_SegmentIndex);
        }
        else
        {
          lane.m_EndNode = new PathNode(owner, (ushort) endNodeLaneIndex);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 n = connectPosition2.m_Position.xz - connectPosition1.m_Position.xz;
          if (MathUtils.TryNormalize(ref n))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connectPosition2.m_Tangent.xz = math.reflect(connectPosition1.m_Tangent.xz, n);
          }
        }
        PrefabRef component2 = new PrefabRef();
        // ISSUE: reference to a compiler-generated field
        component2.m_Prefab = connectPosition1.m_LaneData.m_Lane;
        Unity.Mathematics.Random outRandom;
        // ISSUE: reference to a compiler-generated method
        this.CheckPrefab(ref component2.m_Prefab, ref random, out outRandom, laneBuffer);
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData = this.m_NetLaneData[component2.m_Prefab];
        Curve component3 = new Curve();
        // ISSUE: reference to a compiler-generated field
        if (connectPosition1.m_Tangent.Equals(new float3()))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.abs(connectPosition1.m_Position.y - connectPosition2.m_Position.y) >= 0.0099999997764825821)
          {
            // ISSUE: reference to a compiler-generated field
            component3.m_Bezier.a = connectPosition1.m_Position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component3.m_Bezier.b = math.lerp(connectPosition1.m_Position, connectPosition2.m_Position, new float3(0.25f, 0.5f, 0.25f));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component3.m_Bezier.c = math.lerp(connectPosition1.m_Position, connectPosition2.m_Position, new float3(0.75f, 0.5f, 0.75f));
            // ISSUE: reference to a compiler-generated field
            component3.m_Bezier.d = connectPosition2.m_Position;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component3.m_Bezier = NetUtils.StraightCurve(connectPosition1.m_Position, connectPosition2.m_Position);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component3.m_Bezier = NetUtils.FitCurve(connectPosition1.m_Position, connectPosition1.m_Tangent, -connectPosition2.m_Tangent, connectPosition2.m_Position);
        }
        component3.m_Length = MathUtils.Length(component3.m_Bezier);
        for (int index = 0; index < middleConnections.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.MiddleConnection middleConnection = middleConnections[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((middleConnection.m_ConnectPosition.m_UtilityTypes & connectPosition1.m_UtilityTypes) != UtilityTypes.None && (middleConnection.m_SourceEdge == connectPosition1.m_Owner || middleConnection.m_SourceEdge == connectPosition2.m_Owner))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (middleConnection.m_TargetLane != Entity.Null && ((middleConnection.m_TargetFlags ^ middleConnection.m_ConnectPosition.m_LaneData.m_Flags) & Game.Prefabs.LaneFlags.Underground) != (Game.Prefabs.LaneFlags) 0 && ((connectPosition1.m_LaneData.m_Flags ^ middleConnection.m_ConnectPosition.m_LaneData.m_Flags) & Game.Prefabs.LaneFlags.Underground) == (Game.Prefabs.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              middleConnection.m_Distance = float.MaxValue;
            }
            float t;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num = MathUtils.Distance(component3.m_Bezier, middleConnection.m_ConnectPosition.m_Position, out t);
            // ISSUE: reference to a compiler-generated field
            if ((double) num < (double) middleConnection.m_Distance)
            {
              // ISSUE: reference to a compiler-generated field
              middleConnection.m_Distance = num;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              middleConnection.m_TargetLane = connectPosition1.m_LaneData.m_Lane;
              // ISSUE: reference to a compiler-generated field
              middleConnection.m_TargetIndex = laneIndex;
              // ISSUE: reference to a compiler-generated field
              middleConnection.m_TargetCurve = component3;
              // ISSUE: reference to a compiler-generated field
              middleConnection.m_TargetCurvePos = t;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              middleConnection.m_TargetFlags = connectPosition1.m_LaneData.m_Flags;
              middleConnections[index] = middleConnection;
            }
          }
        }
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey key = new LaneSystem.LaneKey(lane, component2.m_Prefab, (Game.Prefabs.LaneFlags) 0);
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey laneKey = key;
        if (isTemp)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, connectPosition1.m_Owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, connectPosition2.m_Owner);
          // ISSUE: reference to a compiler-generated method
          this.GetOriginalLane(laneBuffer, laneKey, ref temp);
        }
        PseudoRandomSeed componentData = new PseudoRandomSeed();
        // ISSUE: reference to a compiler-generated field
        if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0 && !this.m_PseudoRandomSeedData.TryGetComponent(temp.m_Original, out componentData))
          componentData = new PseudoRandomSeed(ref outRandom);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (laneBuffer.m_OldLanes.TryGetValue(key, out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_OldLanes.Remove(key);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, component3);
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PseudoRandomSeedData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, entity1, componentData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity1, componentData);
            }
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Updates.Add(in entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_Updates.Add(in entity1);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          EntityArchetype nodeLaneArchetype = this.m_PrefabLaneArchetypeData[component2.m_Prefab].m_NodeLaneArchetype;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, nodeLaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, lane);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, component3);
          if ((netLaneData.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, componentData);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_TempOwnerTypes);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity2, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity2, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component1);
          }
        }
      }

      private void CreateNodePedestrianLanes(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        NativeList<LaneSystem.ConnectPosition> buffer,
        NativeList<LaneSystem.ConnectPosition> tempBuffer,
        NativeList<LaneSystem.ConnectPosition> tempBuffer2,
        bool isTemp,
        Temp ownerTemp,
        float3 middlePosition,
        float middleRadius,
        float roundaboutSize)
      {
        if (buffer.Length >= 2)
        {
          NativeList<LaneSystem.ConnectPosition> list = buffer;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LaneSystem.SourcePositionComparer positionComparer = new LaneSystem.SourcePositionComparer();
          // ISSUE: variable of a compiler-generated type
          LaneSystem.SourcePositionComparer comp = positionComparer;
          list.Sort<LaneSystem.ConnectPosition, LaneSystem.SourcePositionComparer>(comp);
        }
        int num1 = 0;
        for (int index = 0; index < buffer.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = buffer[index];
          // ISSUE: reference to a compiler-generated field
          num1 += math.select(1, 0, connectPosition.m_IsSideConnection);
        }
        if (num1 == 0)
          return;
        StackList<int2> stackList = (StackList<int2>) stackalloc int2[num1];
        int laneIndex1 = -1;
        bool flag1 = false;
        int num2 = 0;
        while (num2 < buffer.Length)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition1 = buffer[num2];
          int index1 = num2 + 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (index1 < buffer.Length && !(buffer[index1].m_Owner != targetPosition1.m_Owner))
            ++index1;
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition1 = buffer[index1 - 1];
          // ISSUE: reference to a compiler-generated field
          if (connectPosition1.m_IsSideConnection)
          {
            num2 = index1;
          }
          else
          {
            int laneIndex2 = nodeLaneIndex;
            bool flag2 = index1 == num2 + 1;
            int2 result;
            // ISSUE: reference to a compiler-generated method
            if (this.FindNextRightLane(connectPosition1, buffer, out result))
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition2 = buffer[result.x];
              bool flag3 = result.y == result.x + 1;
              int num3 = 0;
              // ISSUE: reference to a compiler-generated field
              for (; targetPosition2.m_IsSideConnection; targetPosition2 = buffer[result.x])
              {
                if (++num3 > buffer.Length)
                {
                  tempBuffer2.Clear();
                  return;
                }
                for (int x = result.x; x < result.y; ++x)
                {
                  ref NativeList<LaneSystem.ConnectPosition> local1 = ref tempBuffer2;
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition2 = buffer[x];
                  ref LaneSystem.ConnectPosition local2 = ref connectPosition2;
                  local1.Add(in local2);
                }
                int index2 = result.y - 1;
                // ISSUE: reference to a compiler-generated method
                if (!this.FindNextRightLane(buffer[index2], buffer, out result))
                {
                  tempBuffer2.Clear();
                  return;
                }
              }
              if (num1 > 2)
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateNodePedestrianLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, targetPosition2, tempBuffer2, isTemp, ownerTemp, middlePosition, middleRadius, roundaboutSize);
              }
              else if (num1 == 2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (connectPosition1.m_Owner.Index == targetPosition2.m_Owner.Index)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CreateNodePedestrianLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, targetPosition2, tempBuffer2, isTemp, ownerTemp, middlePosition, middleRadius, roundaboutSize);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (connectPosition1.m_Owner.Index < targetPosition2.m_Owner.Index || flag2 & flag3 && (double) middleRadius > 0.0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CreateNodePedestrianLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, targetPosition2, tempBuffer2, isTemp, ownerTemp, middlePosition, middleRadius, roundaboutSize);
                  }
                }
              }
              if (flag2)
                stackList.AddNoResize(new int2(num2, result.x));
            }
            if (num1 == 1 && (double) middleRadius > 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateNodePedestrianLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, connectPosition1, targetPosition1, tempBuffer2, isTemp, ownerTemp, middlePosition, middleRadius, roundaboutSize);
            }
            tempBuffer2.Clear();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabCompositionCrosswalks.HasBuffer(targetPosition1.m_NodeComposition))
            {
              Segment left;
              // ISSUE: reference to a compiler-generated field
              if (targetPosition1.m_SegmentIndex != (byte) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[targetPosition1.m_Owner];
                if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
                {
                  left = endNodeGeometry.m_Geometry.m_Left;
                }
                else
                {
                  left.m_Left = endNodeGeometry.m_Geometry.m_Left.m_Left;
                  left.m_Right = endNodeGeometry.m_Geometry.m_Right.m_Right;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[targetPosition1.m_Owner];
                if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
                {
                  left = startNodeGeometry.m_Geometry.m_Left;
                }
                else
                {
                  left.m_Left = startNodeGeometry.m_Geometry.m_Left.m_Left;
                  left.m_Right = startNodeGeometry.m_Geometry.m_Right.m_Right;
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NetCompositionData netCompositionData = this.m_PrefabCompositionData[targetPosition1.m_NodeComposition];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<NetCompositionCrosswalk> compositionCrosswalk1 = this.m_PrefabCompositionCrosswalks[targetPosition1.m_NodeComposition];
              bool flag4 = (netCompositionData.m_Flags.m_General & (CompositionFlags.General.DeadEnd | CompositionFlags.General.Intersection | CompositionFlags.General.Crosswalk)) == CompositionFlags.General.Crosswalk;
              if (flag4 && laneIndex1 == -1)
              {
                laneIndex1 = laneIndex2;
                num2 = index1;
                continue;
              }
              if (compositionCrosswalk1.Length >= 1)
              {
                tempBuffer.ResizeUninitialized(compositionCrosswalk1.Length + 1);
                flag1 = (netCompositionData.m_Flags.m_General & CompositionFlags.General.Crosswalk) > (CompositionFlags.General) 0;
                for (int index3 = 0; index3 < tempBuffer.Length; ++index3)
                {
                  float3 float3 = index3 != 0 ? (index3 != tempBuffer.Length - 1 ? math.lerp(compositionCrosswalk1[index3 - 1].m_End, compositionCrosswalk1[index3].m_Start, 0.5f) : compositionCrosswalk1[index3 - 1].m_End) : compositionCrosswalk1[index3].m_Start;
                  float s = (float) ((double) float3.x / (double) math.max(1f, netCompositionData.m_Width) + 0.5);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition3 = new LaneSystem.ConnectPosition();
                  // ISSUE: reference to a compiler-generated field
                  connectPosition3.m_Position = math.lerp(left.m_Left.a, left.m_Right.a, s);
                  // ISSUE: reference to a compiler-generated field
                  connectPosition3.m_Position.y += float3.y;
                  tempBuffer[index3] = connectPosition3;
                }
                for (int index4 = num2; index4 < index1; ++index4)
                {
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition4 = buffer[index4];
                  float num4 = float.MaxValue;
                  int index5 = 0;
                  for (int index6 = 0; index6 < tempBuffer.Length; ++index6)
                  {
                    // ISSUE: variable of a compiler-generated type
                    LaneSystem.ConnectPosition connectPosition5 = tempBuffer[index6];
                    // ISSUE: reference to a compiler-generated field
                    if (connectPosition5.m_Owner == Entity.Null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      float num5 = math.lengthsq(connectPosition4.m_Position - connectPosition5.m_Position);
                      if ((double) num5 < (double) num4)
                      {
                        num4 = num5;
                        index5 = index6;
                      }
                    }
                  }
                  tempBuffer[index5] = connectPosition4;
                }
                for (int index7 = 0; index7 < tempBuffer.Length; ++index7)
                {
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition connectPosition6 = tempBuffer[index7];
                  // ISSUE: reference to a compiler-generated field
                  if (connectPosition6.m_Owner == Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    connectPosition6.m_Owner = owner;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    connectPosition6.m_NodeComposition = targetPosition1.m_NodeComposition;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    connectPosition6.m_EdgeComposition = targetPosition1.m_EdgeComposition;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    connectPosition6.m_Tangent = targetPosition1.m_Tangent;
                    tempBuffer[index7] = connectPosition6;
                  }
                }
                PathNode startPathNode = new PathNode();
                PathNode endPathNode = new PathNode();
                for (int index8 = 0; index8 < compositionCrosswalk1.Length; ++index8)
                {
                  NetCompositionCrosswalk compositionCrosswalk2 = compositionCrosswalk1[index8];
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition sourcePosition = tempBuffer[index8];
                  // ISSUE: variable of a compiler-generated type
                  LaneSystem.ConnectPosition targetPosition3 = tempBuffer[index8 + 1];
                  float s1 = (float) ((double) compositionCrosswalk2.m_Start.x / (double) math.max(1f, netCompositionData.m_Width) + 0.5);
                  float s2 = (float) ((double) compositionCrosswalk2.m_End.x / (double) math.max(1f, netCompositionData.m_Width) + 0.5);
                  if (flag4)
                  {
                    // ISSUE: reference to a compiler-generated field
                    sourcePosition.m_Position = math.lerp(left.m_Left.d, left.m_Right.d, s1);
                    // ISSUE: reference to a compiler-generated field
                    targetPosition3.m_Position = math.lerp(left.m_Left.d, left.m_Right.d, s2);
                    if (index8 == 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      sourcePosition.m_Owner = owner;
                      startPathNode = new PathNode(owner, (ushort) laneIndex1, 0.5f);
                      endPathNode = startPathNode;
                    }
                    if (index8 == compositionCrosswalk1.Length - 1)
                    {
                      // ISSUE: reference to a compiler-generated field
                      targetPosition3.m_Owner = owner;
                      endPathNode = new PathNode(owner, (ushort) laneIndex2, 0.5f);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    sourcePosition.m_Position = math.lerp(left.m_Left.a, left.m_Right.a, s1);
                    // ISSUE: reference to a compiler-generated field
                    targetPosition3.m_Position = math.lerp(left.m_Left.a, left.m_Right.a, s2);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    sourcePosition.m_Position += sourcePosition.m_Tangent * compositionCrosswalk2.m_Start.z;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    targetPosition3.m_Position += targetPosition3.m_Tangent * compositionCrosswalk2.m_End.z;
                    // ISSUE: reference to a compiler-generated field
                    if (index8 == 0 && sourcePosition.m_Owner == owner)
                    {
                      startPathNode = new PathNode(owner, (ushort) nodeLaneIndex++);
                      endPathNode = startPathNode;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  sourcePosition.m_Position.y += compositionCrosswalk2.m_Start.y;
                  // ISSUE: reference to a compiler-generated field
                  targetPosition3.m_Position.y += compositionCrosswalk2.m_End.y;
                  // ISSUE: reference to a compiler-generated field
                  sourcePosition.m_LaneData.m_Lane = compositionCrosswalk2.m_Lane;
                  // ISSUE: reference to a compiler-generated field
                  targetPosition3.m_LaneData.m_Lane = compositionCrosswalk2.m_Lane;
                  PathNode endNode;
                  // ISSUE: reference to a compiler-generated method
                  this.CreateNodePedestrianLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, sourcePosition, targetPosition3, startPathNode, endPathNode, true, false, isTemp, ownerTemp, false, (compositionCrosswalk2.m_Flags & Game.Prefabs.LaneFlags.CrossRoad) != 0, out Bezier4x3 _, out PathNode _, out endNode);
                  startPathNode = endNode;
                  endPathNode = endNode;
                }
              }
            }
            num2 = index1;
          }
        }
        if (flag1 || (double) middleRadius > 0.0)
          return;
        for (int index9 = 1; index9 < stackList.Length; ++index9)
        {
          int2 int2_1 = stackList[index9];
          for (int index10 = 0; index10 < index9; ++index10)
          {
            int2 int2_2 = stackList[index10];
            if (math.all(int2_1 != int2_2.yx))
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition sourcePosition = buffer[int2_1.x];
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition targetPosition = buffer[int2_2.x];
              // ISSUE: reference to a compiler-generated method
              this.CreateNodePedestrianLanes(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, sourcePosition, targetPosition, tempBuffer2, isTemp, ownerTemp, middlePosition, middleRadius, roundaboutSize);
            }
          }
        }
      }

      private void CreateNodePedestrianLanes(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        LaneSystem.ConnectPosition sourcePosition,
        LaneSystem.ConnectPosition targetPosition,
        NativeList<LaneSystem.ConnectPosition> sideConnections,
        bool isTemp,
        Temp ownerTemp,
        float3 middlePosition,
        float middleRadius,
        float roundaboutSize)
      {
        Bezier4x3 curve1;
        if ((double) middleRadius == 0.0)
        {
          int jobIndex1 = jobIndex;
          ref int local1 = ref nodeLaneIndex;
          ref Unity.Mathematics.Random local2 = ref random;
          Entity owner1 = owner;
          // ISSUE: variable of a compiler-generated type
          LaneSystem.LaneBuffer laneBuffer1 = laneBuffer;
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition sourcePosition1 = sourcePosition;
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition targetPosition1 = targetPosition;
          PathNode startPathNode = new PathNode();
          PathNode endNode = new PathNode();
          PathNode endPathNode = endNode;
          int num1 = isTemp ? 1 : 0;
          Temp ownerTemp1 = ownerTemp;
          Bezier4x3 curve2;
          ref Bezier4x3 local3 = ref curve2;
          PathNode pathNode1;
          ref PathNode local4 = ref pathNode1;
          PathNode middleNode;
          ref PathNode local5 = ref middleNode;
          // ISSUE: reference to a compiler-generated method
          this.CreateNodePedestrianLane(jobIndex1, ref local1, ref local2, owner1, laneBuffer1, sourcePosition1, targetPosition1, startPathNode, endPathNode, false, false, num1 != 0, ownerTemp1, false, true, out local3, out local4, out local5);
          for (int index = 0; index < sideConnections.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition sideConnection = sideConnections[index];
            // ISSUE: reference to a compiler-generated field
            float num2 = MathUtils.Distance(curve2, sideConnection.m_Position, out float _);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 position = sideConnection.m_Position + sideConnection.m_Tangent * (num2 * 0.5f);
            float t;
            double num3 = (double) MathUtils.Distance(curve2, position, out t);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition sourcePosition2 = new LaneSystem.ConnectPosition()
            {
              m_LaneData = {
                m_Lane = sideConnection.m_LaneData.m_Lane,
                m_Flags = sideConnection.m_LaneData.m_Flags
              },
              m_NodeComposition = sideConnection.m_NodeComposition,
              m_EdgeComposition = sideConnection.m_EdgeComposition,
              m_Owner = owner,
              m_BaseHeight = math.lerp(sourcePosition.m_BaseHeight, targetPosition.m_BaseHeight, t),
              m_Position = MathUtils.Position(curve2, t)
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            sourcePosition2.m_Tangent = position - sourcePosition2.m_Position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            sourcePosition2.m_Tangent = MathUtils.Normalize(sourcePosition2.m_Tangent, sourcePosition2.m_Tangent.xz);
            PathNode pathNode2 = new PathNode(pathNode1, t);
            // ISSUE: reference to a compiler-generated method
            this.CreateNodePedestrianLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, sourcePosition2, sideConnection, pathNode2, pathNode2, false, true, isTemp, ownerTemp, false, true, out curve1, out middleNode, out endNode);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          float2 float2 = math.normalizesafe(sourcePosition.m_Position.xz - middlePosition.xz);
          // ISSUE: reference to a compiler-generated field
          float2 toVector1 = math.normalizesafe(targetPosition.m_Position.xz - middlePosition.xz);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[sourcePosition.m_NodeComposition];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[targetPosition.m_NodeComposition];
          // ISSUE: reference to a compiler-generated field
          float x1 = netCompositionData1.m_Width * 0.5f - sourcePosition.m_LaneData.m_Position.x;
          // ISSUE: reference to a compiler-generated field
          float y1 = netCompositionData2.m_Width * 0.5f + targetPosition.m_LaneData.m_Position.x;
          float num4 = middleRadius + roundaboutSize - math.lerp(x1, y1, 0.5f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool c = sourcePosition.m_Position.Equals(targetPosition.m_Position) && sourcePosition.m_Owner == targetPosition.m_Owner && (int) sourcePosition.m_LaneData.m_Index == (int) targetPosition.m_LaneData.m_Index;
          float b = math.select(MathUtils.RotationAngleLeft(float2, toVector1), 6.28318548f, c);
          int num5 = 1 + math.max(1, Mathf.CeilToInt((float) ((double) b * 0.63661974668502808 - Math.PI / 1000.0)));
          float t1;
          if (num5 == 2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float y2 = MathUtils.Distance(NetUtils.FitCurve(sourcePosition.m_Position, sourcePosition.m_Tangent, -targetPosition.m_Tangent, targetPosition.m_Position).xz, middlePosition.xz, out t1);
            num4 = math.max(num4, y2);
          }
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition1 = sourcePosition;
          float x2 = 0.0f;
          PathNode pathNode3 = new PathNode();
          if (num5 >= 2)
          {
            for (int index = 0; index < sideConnections.Length; ++index)
            {
              ref LaneSystem.ConnectPosition local = ref sideConnections.ElementAt(index);
              // ISSUE: reference to a compiler-generated field
              float2 toVector2 = math.normalizesafe(local.m_Position.xz - middlePosition.xz);
              // ISSUE: reference to a compiler-generated field
              local.m_Order = MathUtils.RotationAngleLeft(float2, toVector2);
              // ISSUE: reference to a compiler-generated field
              if ((double) local.m_Order > (double) b)
              {
                float num6 = MathUtils.RotationAngleRight(float2, toVector2);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                local.m_Order = math.select(0.0f, b, (double) num6 > (double) local.m_Order - (double) b);
              }
            }
          }
          for (int index1 = 1; index1 <= num5; ++index1)
          {
            float num7 = math.saturate((float) (((double) index1 - 0.5) / ((double) num5 - 1.0)));
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition connectPosition2 = new LaneSystem.ConnectPosition();
            if (index1 == num5)
            {
              connectPosition2 = targetPosition;
            }
            else
            {
              float2 forward = MathUtils.RotateLeft(float2, b * num7);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_LaneData.m_Lane = sourcePosition.m_LaneData.m_Lane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_LaneData.m_Flags = sourcePosition.m_LaneData.m_Flags;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_NodeComposition = sourcePosition.m_NodeComposition;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_EdgeComposition = sourcePosition.m_EdgeComposition;
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_Owner = owner;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_BaseHeight = math.lerp(sourcePosition.m_BaseHeight, targetPosition.m_BaseHeight, num7);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_Position.y = math.lerp(sourcePosition.m_Position.y, targetPosition.m_Position.y, num7);
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_Position.xz = middlePosition.xz + forward * num4;
              // ISSUE: reference to a compiler-generated field
              connectPosition2.m_Tangent.xz = MathUtils.Right(forward);
            }
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition sourcePosition3 = connectPosition1;
            // ISSUE: variable of a compiler-generated type
            LaneSystem.ConnectPosition targetPosition2 = connectPosition2;
            Bezier4x3 curve3;
            PathNode middleNode;
            if (index1 > 1 && index1 < num5)
            {
              PathNode endNode;
              // ISSUE: reference to a compiler-generated method
              this.CreateNodePedestrianLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, sourcePosition3, targetPosition2, pathNode3, pathNode3, false, false, isTemp, ownerTemp, false, true, out curve3, out middleNode, out endNode);
              pathNode3 = endNode;
            }
            else
            {
              float num8 = math.lerp(x2, num7, 0.5f);
              float2 forward = MathUtils.RotateLeft(float2, b * num8);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 centerPosition = middlePosition with
              {
                y = math.lerp(connectPosition1.m_Position.y, connectPosition2.m_Position.y, 0.5f)
              };
              centerPosition.xz += forward * num4;
              float3 centerTangent = new float3();
              centerTangent.xz = MathUtils.Left(forward);
              if (index1 == 1)
              {
                // ISSUE: reference to a compiler-generated method
                this.PresetCurve(ref sourcePosition3, ref targetPosition2, middlePosition, centerPosition, centerTangent, num4, 0.0f, b / (float) num5, 2f);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.PresetCurve(ref sourcePosition3, ref targetPosition2, middlePosition, centerPosition, centerTangent, num4, b / (float) num5, 0.0f, 2f);
              }
              PathNode endNode;
              // ISSUE: reference to a compiler-generated method
              this.CreateNodePedestrianLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, sourcePosition3, targetPosition2, pathNode3, pathNode3, false, false, isTemp, ownerTemp, true, true, out curve3, out middleNode, out endNode);
              pathNode3 = endNode;
            }
            for (int index2 = 0; index2 < sideConnections.Length; ++index2)
            {
              // ISSUE: variable of a compiler-generated type
              LaneSystem.ConnectPosition sideConnection = sideConnections[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (((double) sideConnection.m_Order >= (double) b * (double) x2 || index1 == 1) && ((double) sideConnection.m_Order < (double) b * (double) num7 || index1 == num5))
              {
                // ISSUE: reference to a compiler-generated field
                float num9 = MathUtils.Distance(curve3, sideConnection.m_Position, out t1);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float3 position = sideConnection.m_Position + sideConnection.m_Tangent * (num9 * 0.5f);
                float t2;
                double num10 = (double) MathUtils.Distance(curve3, position, out t2);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                LaneSystem.ConnectPosition sourcePosition4 = new LaneSystem.ConnectPosition()
                {
                  m_LaneData = {
                    m_Lane = sideConnection.m_LaneData.m_Lane,
                    m_Flags = sideConnection.m_LaneData.m_Flags
                  },
                  m_NodeComposition = sideConnection.m_NodeComposition,
                  m_EdgeComposition = sideConnection.m_EdgeComposition,
                  m_Owner = owner,
                  m_BaseHeight = math.lerp(sourcePosition.m_BaseHeight, targetPosition.m_BaseHeight, t2),
                  m_Position = MathUtils.Position(curve3, t2)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                sourcePosition4.m_Tangent = position - sourcePosition4.m_Position;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                sourcePosition4.m_Tangent = MathUtils.Normalize(sourcePosition4.m_Tangent, sourcePosition4.m_Tangent.xz);
                PathNode pathNode4 = new PathNode(middleNode, t2);
                // ISSUE: reference to a compiler-generated method
                this.CreateNodePedestrianLane(jobIndex, ref nodeLaneIndex, ref random, owner, laneBuffer, sourcePosition4, sideConnection, pathNode4, pathNode4, false, true, isTemp, ownerTemp, false, true, out curve1, out PathNode _, out PathNode _);
              }
            }
            // ISSUE: reference to a compiler-generated field
            connectPosition1 = connectPosition2 with
            {
              m_Tangent = -connectPosition2.m_Tangent
            };
            x2 = num7;
          }
        }
      }

      private bool FindNextRightLane(
        LaneSystem.ConnectPosition position,
        NativeList<LaneSystem.ConnectPosition> buffer,
        out int2 result)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 x = new float2(position.m_Tangent.z, -position.m_Tangent.x);
        float num1 = -1f;
        result = new int2();
        int num2;
        for (int index = 0; index < buffer.Length; index = num2)
        {
          // ISSUE: variable of a compiler-generated type
          LaneSystem.ConnectPosition connectPosition = buffer[index];
          num2 = index + 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (num2 < buffer.Length && !(buffer[num2].m_Owner != connectPosition.m_Owner))
            ++num2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!connectPosition.m_Owner.Equals(position.m_Owner) || num2 != index + 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2 y = connectPosition.m_Position.xz - position.m_Position.xz;
            // ISSUE: reference to a compiler-generated field
            y -= connectPosition.m_Tangent.xz;
            MathUtils.TryNormalize(ref y);
            float num3;
            // ISSUE: reference to a compiler-generated field
            if ((double) math.dot(position.m_Tangent.xz, y) > 0.0)
            {
              num3 = math.dot(x, y) * 0.5f;
            }
            else
            {
              float num4 = math.dot(x, y);
              num3 = math.select(-1f, 1f, (double) num4 >= 0.0) - num4 * 0.5f;
            }
            if ((double) num3 > (double) num1)
            {
              num1 = num3;
              result = new int2(index, num2);
            }
          }
        }
        return (double) num1 != -1.0;
      }

      private void CreateNodePedestrianLane(
        int jobIndex,
        ref int nodeLaneIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        LaneSystem.LaneBuffer laneBuffer,
        LaneSystem.ConnectPosition sourcePosition,
        LaneSystem.ConnectPosition targetPosition,
        PathNode startPathNode,
        PathNode endPathNode,
        bool isCrosswalk,
        bool isSideConnection,
        bool isTemp,
        Temp ownerTemp,
        bool fixedTangents,
        bool hasSignals,
        out Bezier4x3 curve,
        out PathNode middleNode,
        out PathNode endNode)
      {
        curve = new Bezier4x3();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData startCompositionData = this.m_PrefabCompositionData[sourcePosition.m_NodeComposition];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData endCompositionData = this.m_PrefabCompositionData[targetPosition.m_NodeComposition];
        Owner component1 = new Owner();
        component1.m_Owner = owner;
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        Lane lane = new Lane();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        lane.m_StartNode = !(sourcePosition.m_Owner == owner) ? new PathNode(sourcePosition.m_Owner, sourcePosition.m_LaneData.m_Index, sourcePosition.m_SegmentIndex) : startPathNode;
        lane.m_MiddleNode = new PathNode(owner, (ushort) nodeLaneIndex++);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        lane.m_EndNode = !(targetPosition.m_Owner == owner) ? new PathNode(targetPosition.m_Owner, targetPosition.m_LaneData.m_Index, targetPosition.m_SegmentIndex) : (endPathNode.Equals(startPathNode) ? new PathNode(owner, (ushort) nodeLaneIndex++) : endPathNode);
        middleNode = lane.m_MiddleNode;
        endNode = lane.m_EndNode;
        PrefabRef component2 = new PrefabRef();
        // ISSUE: reference to a compiler-generated field
        component2.m_Prefab = sourcePosition.m_LaneData.m_Lane;
        if (!isCrosswalk)
        {
          // ISSUE: reference to a compiler-generated field
          int num = (double) sourcePosition.m_LaneData.m_Position.x > 0.0 ? 1 : 0;
          // ISSUE: reference to a compiler-generated field
          bool flag = (double) targetPosition.m_LaneData.m_Position.x > 0.0;
          CompositionFlags.Side side1 = num != 0 ? startCompositionData.m_Flags.m_Right : startCompositionData.m_Flags.m_Left;
          CompositionFlags.Side side2 = flag ? endCompositionData.m_Flags.m_Right : endCompositionData.m_Flags.m_Left;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int a1 = math.select(0, 1, (sourcePosition.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int a2 = math.select(0, 1, (targetPosition.m_CompositionData.m_RoadFlags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0);
          int a3 = math.select(a1, 1 - a1, (side1 & CompositionFlags.Side.Sidewalk) > (CompositionFlags.Side) 0);
          int a4 = math.select(a2, 1 - a2, (side2 & CompositionFlags.Side.Sidewalk) > (CompositionFlags.Side) 0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabCompositionData.HasComponent(sourcePosition.m_EdgeComposition))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[sourcePosition.m_EdgeComposition];
            int a5 = math.select(a3, a3 + 2, (netCompositionData.m_Flags.m_General & CompositionFlags.General.Tiles) > (CompositionFlags.General) 0);
            a3 = math.select(a5, a5 - 4, (netCompositionData.m_Flags.m_General & CompositionFlags.General.Gravel) > (CompositionFlags.General) 0);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabCompositionData.HasComponent(targetPosition.m_EdgeComposition))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[targetPosition.m_EdgeComposition];
            int a6 = math.select(a4, a4 + 2, (netCompositionData.m_Flags.m_General & CompositionFlags.General.Tiles) > (CompositionFlags.General) 0);
            a4 = math.select(a6, a6 - 4, (netCompositionData.m_Flags.m_General & CompositionFlags.General.Gravel) > (CompositionFlags.General) 0);
          }
          // ISSUE: reference to a compiler-generated field
          if (a3 > a4 && component2.m_Prefab != sourcePosition.m_LaneData.m_Lane)
          {
            // ISSUE: reference to a compiler-generated field
            component2.m_Prefab = sourcePosition.m_LaneData.m_Lane;
          }
          // ISSUE: reference to a compiler-generated field
          if (a4 > a3 && component2.m_Prefab != targetPosition.m_LaneData.m_Lane)
          {
            // ISSUE: reference to a compiler-generated field
            component2.m_Prefab = targetPosition.m_LaneData.m_Lane;
          }
        }
        Unity.Mathematics.Random outRandom;
        // ISSUE: reference to a compiler-generated method
        this.CheckPrefab(ref component2.m_Prefab, ref random, out outRandom, laneBuffer);
        PedestrianLane component3 = new PedestrianLane();
        NodeLane component4 = new NodeLane();
        Curve component5 = new Curve();
        float2 x = (float2) 0.0f;
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData1 = this.m_NetLaneData[component2.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetLaneData.HasComponent(sourcePosition.m_LaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetLaneData netLaneData2 = this.m_NetLaneData[sourcePosition.m_LaneData.m_Lane];
          component4.m_WidthOffset.x = netLaneData2.m_Width - netLaneData1.m_Width;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetLaneData.HasComponent(targetPosition.m_LaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetLaneData netLaneData3 = this.m_NetLaneData[targetPosition.m_LaneData.m_Lane];
          component4.m_WidthOffset.y = netLaneData3.m_Width - netLaneData1.m_Width;
        }
        if (isCrosswalk)
        {
          component3.m_Flags |= PedestrianLaneFlags.Crosswalk;
          if ((startCompositionData.m_Flags.m_General & CompositionFlags.General.Crosswalk) == (CompositionFlags.General) 0)
          {
            if ((startCompositionData.m_Flags.m_General & CompositionFlags.General.DeadEnd) != (CompositionFlags.General) 0)
              return;
            component3.m_Flags |= PedestrianLaneFlags.Unsafe;
            hasSignals = false;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component5.m_Bezier = NetUtils.StraightCurve(sourcePosition.m_Position, targetPosition.m_Position);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component5.m_Length = math.distance(sourcePosition.m_Position, targetPosition.m_Position);
          hasSignals &= (startCompositionData.m_Flags.m_General & CompositionFlags.General.TrafficLights) > (CompositionFlags.General) 0;
          if ((double) component5.m_Length >= 0.10000000149011612)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2 y = math.normalizesafe(targetPosition.m_Position.xz - sourcePosition.m_Position.xz);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            x = new float2(math.dot(sourcePosition.m_Tangent.xz, y), math.dot(targetPosition.m_Tangent.xz, y));
            float2 float2 = math.tan(1.57079637f - math.acos(math.saturate(math.abs(x)))) * (netLaneData1.m_Width + component4.m_WidthOffset) * 0.5f;
            x = math.select(math.saturate(float2 / component5.m_Length), (float2) 0.0f, float2 < 0.01f);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (sourcePosition.m_Owner == targetPosition.m_Owner && (startCompositionData.m_Flags.m_General & (CompositionFlags.General.DeadEnd | CompositionFlags.General.Roundabout)) == (CompositionFlags.General) 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component5.m_Bezier = (double) math.distance(sourcePosition.m_Position, targetPosition.m_Position) < 0.0099999997764825821 ? NetUtils.StraightCurve(sourcePosition.m_Position, targetPosition.m_Position) : (!fixedTangents ? NetUtils.FitCurve(sourcePosition.m_Position, sourcePosition.m_Tangent, -targetPosition.m_Tangent, targetPosition.m_Position) : new Bezier4x3(sourcePosition.m_Position, sourcePosition.m_Position + sourcePosition.m_Tangent, targetPosition.m_Position + targetPosition.m_Tangent, targetPosition.m_Position));
          if (isSideConnection)
          {
            component3.m_Flags |= PedestrianLaneFlags.SideConnection;
          }
          else
          {
            component3.m_Flags |= PedestrianLaneFlags.AllowMiddle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.ModifyCurveHeight(ref component5.m_Bezier, sourcePosition.m_BaseHeight, targetPosition.m_BaseHeight, startCompositionData, endCompositionData);
          }
          component5.m_Length = MathUtils.Length(component5.m_Bezier);
          hasSignals &= (startCompositionData.m_Flags.m_General & CompositionFlags.General.LevelCrossing) > (CompositionFlags.General) 0;
        }
        curve = component5.m_Bezier;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((sourcePosition.m_LaneData.m_Flags & targetPosition.m_LaneData.m_Flags & Game.Prefabs.LaneFlags.OnWater) != (Game.Prefabs.LaneFlags) 0)
          component3.m_Flags |= PedestrianLaneFlags.OnWater;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey key = new LaneSystem.LaneKey(lane, component2.m_Prefab, (Game.Prefabs.LaneFlags) 0);
        // ISSUE: variable of a compiler-generated type
        LaneSystem.LaneKey laneKey = key;
        if (isTemp)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, sourcePosition.m_Owner);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, targetPosition.m_Owner);
          // ISSUE: reference to a compiler-generated method
          this.GetOriginalLane(laneBuffer, laneKey, ref temp);
        }
        PseudoRandomSeed componentData = new PseudoRandomSeed();
        // ISSUE: reference to a compiler-generated field
        if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0 && !this.m_PseudoRandomSeedData.TryGetComponent(temp.m_Original, out componentData))
          componentData = new PseudoRandomSeed(ref outRandom);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (laneBuffer.m_OldLanes.TryGetValue(key, out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_OldLanes.Remove(key);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity1, component4);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, component5);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity1, component3);
          if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PseudoRandomSeedData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, entity1, componentData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity1, componentData);
            }
          }
          if (hasSignals)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_LaneSignalData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LaneSignal>(jobIndex, entity1, new LaneSignal());
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneSignalData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<LaneSignal>(jobIndex, entity1);
            }
          }
          if (math.any(x != 0.0f))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CutRange> dynamicBuffer = !this.m_CutRanges.HasBuffer(entity1) ? this.m_CommandBuffer.AddBuffer<CutRange>(jobIndex, entity1) : this.m_CommandBuffer.SetBuffer<CutRange>(jobIndex, entity1);
            if ((double) x.x != 0.0)
              dynamicBuffer.Add(new CutRange()
              {
                m_CurveDelta = new Bounds1(0.0f, x.x)
              });
            if ((double) x.y != 0.0)
              dynamicBuffer.Add(new CutRange()
              {
                m_CurveDelta = new Bounds1(1f - x.y, 1f)
              });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<CutRange>(jobIndex, entity1);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Updates.Add(in entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              laneBuffer.m_Updates.Add(in entity1);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          EntityArchetype nodeLaneArchetype = this.m_PrefabLaneArchetypeData[component2.m_Prefab].m_NodeLaneArchetype;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, nodeLaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, lane);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NodeLane>(jobIndex, entity2, component4);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, component5);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PedestrianLane>(jobIndex, entity2, component3);
          if ((netLaneData1.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, componentData);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_TempOwnerTypes);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity2, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity2, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component1);
          }
          if (hasSignals)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LaneSignal>(jobIndex, entity2, new LaneSignal());
          }
          if (!math.any(x != 0.0f))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CutRange> dynamicBuffer = this.m_CommandBuffer.AddBuffer<CutRange>(jobIndex, entity2);
          if ((double) x.x != 0.0)
            dynamicBuffer.Add(new CutRange()
            {
              m_CurveDelta = new Bounds1(0.0f, x.x)
            });
          if ((double) x.y == 0.0)
            return;
          dynamicBuffer.Add(new CutRange()
          {
            m_CurveDelta = new Bounds1(1f - x.y, 1f)
          });
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> __Game_Net_Orphan_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneSignal> __Game_Net_LaneSignal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Overridden> __Game_Common_Overridden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Clear> __Game_Areas_Clear_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadComposition> __Game_Prefabs_RoadComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackComposition> __Game_Prefabs_TrackComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterwayComposition> __Game_Prefabs_WaterwayComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathwayComposition> __Game_Prefabs_PathwayComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiwayComposition> __Game_Prefabs_TaxiwayComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> __Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetObjectData> __Game_Prefabs_NetObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CutRange> __Game_Net_CutRange_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> __Game_Prefabs_NetCompositionLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionCrosswalk> __Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<DefaultNetLane> __Game_Prefabs_DefaultNetLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> __Game_Prefabs_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AuxiliaryNetLane> __Game_Prefabs_AuxiliaryNetLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionPiece> __Game_Prefabs_NetCompositionPiece_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RO_ComponentLookup = state.GetComponentLookup<LaneSignal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentLookup = state.GetComponentLookup<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Clear_RO_ComponentLookup = state.GetComponentLookup<Clear>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadComposition_RO_ComponentLookup = state.GetComponentLookup<RoadComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackComposition_RO_ComponentLookup = state.GetComponentLookup<TrackComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterwayComposition_RO_ComponentLookup = state.GetComponentLookup<WaterwayComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathwayComposition_RO_ComponentLookup = state.GetComponentLookup<PathwayComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TaxiwayComposition_RO_ComponentLookup = state.GetComponentLookup<TaxiwayComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup = state.GetComponentLookup<NetLaneArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetObjectData_RO_ComponentLookup = state.GetComponentLookup<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CutRange_RO_BufferLookup = state.GetBufferLookup<CutRange>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionLane_RO_BufferLookup = state.GetBufferLookup<NetCompositionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup = state.GetBufferLookup<NetCompositionCrosswalk>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DefaultNetLane_RO_BufferLookup = state.GetBufferLookup<DefaultNetLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AuxiliaryNetLane_RO_BufferLookup = state.GetBufferLookup<AuxiliaryNetLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionPiece_RO_BufferLookup = state.GetBufferLookup<NetCompositionPiece>(true);
      }
    }
  }
}
