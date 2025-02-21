// Decompiled with JetBrains decompiler
// Type: Game.Objects.SecondaryObjectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class SecondaryObjectSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ModificationBarrier4B m_ModificationBarrier;
    private EntityQuery m_ObjectQuery;
    private EntityQuery m_LaneQuery;
    private ComponentTypeSet m_AppliedTypes;
    private ComponentTypeSet m_SecondaryOwnerTypes;
    private ComponentTypeSet m_TempAnimationTypes;
    private SecondaryObjectSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<SubObject>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Area>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.UtilityLane>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Owner>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_SecondaryOwnerTypes = new ComponentTypeSet(ComponentType.ReadWrite<Secondary>(), ComponentType.ReadWrite<Owner>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempAnimationTypes = new ComponentTypeSet(ComponentType.ReadWrite<Temp>(), ComponentType.ReadWrite<Game.Tools.Animation>(), ComponentType.ReadWrite<InterpolatedTransform>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      bool flag1 = !this.m_ObjectQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      bool flag2 = !this.m_LaneQuery.IsEmptyIgnoreFilter;
      if (!flag1 && !flag2)
        return;
      NativeQueue<SecondaryObjectSystem.UpdateData> updateQueue = new NativeQueue<SecondaryObjectSystem.UpdateData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateObjects(updateQueue);
      }
      if (flag2)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateLanes(updateQueue);
      }
      updateQueue.Dispose(this.Dependency);
    }

    private void UpdateLanes(
      NativeQueue<SecondaryObjectSystem.UpdateData> updateQueue)
    {
      NativeParallelMultiHashMap<Entity, SecondaryObjectSystem.UpdateData> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, SecondaryObjectSystem.UpdateData>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryObjectSystem.FillUpdateMapJob jobData1 = new SecondaryObjectSystem.FillUpdateMapJob()
      {
        m_UpdateQueue = updateQueue,
        m_UpdateMap = parallelMultiHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryObjectSystem.SecondaryLaneAnchorJob jobData2 = new SecondaryObjectSystem.SecondaryLaneAnchorJob()
      {
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle,
        m_UtilityLaneType = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RW_ComponentTypeHandle,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_PrefabSubLanes = this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup,
        m_UpdateMap = parallelMultiHashMap
      };
      JobHandle jobHandle = jobData1.Schedule<SecondaryObjectSystem.FillUpdateMapJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery laneQuery = this.m_LaneQuery;
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.ScheduleParallel<SecondaryObjectSystem.SecondaryLaneAnchorJob>(laneQuery, dependsOn);
      parallelMultiHashMap.Dispose(inputDeps);
      this.Dependency = inputDeps;
    }

    private void UpdateObjects(
      NativeQueue<SecondaryObjectSystem.UpdateData> updateQueue)
    {
      NativeQueue<SecondaryObjectSystem.SubObjectOwnerData> nativeQueue = new NativeQueue<SecondaryObjectSystem.SubObjectOwnerData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<SecondaryObjectSystem.SubObjectOwnerData> nativeList = new NativeList<SecondaryObjectSystem.SubObjectOwnerData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryObjectSystem.CheckSubObjectOwnersJob jobData1 = new SecondaryObjectSystem.CheckSubObjectOwnersJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_SecondaryData = this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_AppliedTypes = this.m_AppliedTypes,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_OwnerQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryObjectSystem.CollectSubObjectOwnersJob jobData2 = new SecondaryObjectSystem.CollectSubObjectOwnersJob()
      {
        m_OwnerQueue = nativeQueue,
        m_OwnerList = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DefaultNetLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrafficLights_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_StreetLight_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TrafficLight_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ThemeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StreetLightData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrafficSignData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrafficLightData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      SecondaryObjectSystem.UpdateSubObjectsJob jobData3 = new SecondaryObjectSystem.UpdateSubObjectsJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabNetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabTrafficLightData = this.__TypeHandle.__Game_Prefabs_TrafficLightData_RO_ComponentLookup,
        m_PrefabTrafficSignData = this.__TypeHandle.__Game_Prefabs_TrafficSignData_RO_ComponentLookup,
        m_PrefabStreetLightData = this.__TypeHandle.__Game_Prefabs_StreetLightData_RO_ComponentLookup,
        m_PrefabLaneDirectionData = this.__TypeHandle.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabThemeData = this.__TypeHandle.__Game_Prefabs_ThemeData_RO_ComponentLookup,
        m_PrefabPlaceholderObjectData = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup,
        m_PrefabUtilityObjectData = this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup,
        m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_PrefabTreeData = this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_SecondaryData = this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup,
        m_TrafficLightData = this.__TypeHandle.__Game_Objects_TrafficLight_RO_ComponentLookup,
        m_StreetLightData = this.__TypeHandle.__Game_Objects_StreetLight_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_NetNodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_NetEdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NetCompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_UtilityLaneData = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_TrafficLightsData = this.__TypeHandle.__Game_Net_TrafficLights_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RW_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_NetCompositionObjects = this.__TypeHandle.__Game_Prefabs_NetCompositionObject_RO_BufferLookup,
        m_PlaceholderObjects = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_ObjectRequirements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup,
        m_DefaultNetLanes = this.__TypeHandle.__Game_Prefabs_DefaultNetLane_RO_BufferLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubReplacements = this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_RandomSeed = RandomSeed.Next(),
        m_DefaultTheme = this.m_CityConfigurationSystem.defaultTheme,
        m_AppliedTypes = this.m_AppliedTypes,
        m_SecondaryOwnerTypes = this.m_SecondaryOwnerTypes,
        m_TempAnimationTypes = this.m_TempAnimationTypes,
        m_OwnerList = nativeList.AsDeferredJobArray(),
        m_UpdateQueue = updateQueue.AsParallelWriter(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn1 = jobData1.ScheduleParallel<SecondaryObjectSystem.CheckSubObjectOwnersJob>(this.m_ObjectQuery, this.Dependency);
      JobHandle inputDeps = jobData2.Schedule<SecondaryObjectSystem.CollectSubObjectOwnersJob>(dependsOn1);
      NativeList<SecondaryObjectSystem.SubObjectOwnerData> list = nativeList;
      JobHandle dependsOn2 = inputDeps;
      JobHandle jobHandle = jobData3.Schedule<SecondaryObjectSystem.UpdateSubObjectsJob, SecondaryObjectSystem.SubObjectOwnerData>(list, 1, dependsOn2);
      nativeQueue.Dispose(inputDeps);
      nativeList.Dispose(jobHandle);
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
    public SecondaryObjectSystem()
    {
    }

    private struct UpdateData
    {
      public Entity m_Owner;
      public Entity m_Prefab;
      public Transform m_Transform;
    }

    private struct SubObjectOwnerData : IComparable<SecondaryObjectSystem.SubObjectOwnerData>
    {
      public Entity m_Owner;
      public Entity m_Original;
      public bool m_Temp;
      public bool m_Deleted;

      public SubObjectOwnerData(Entity owner, Entity original, bool temp, bool deleted)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        this.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        this.m_Temp = temp;
        // ISSUE: reference to a compiler-generated field
        this.m_Deleted = deleted;
      }

      public int CompareTo(SecondaryObjectSystem.SubObjectOwnerData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Owner.Index - other.m_Owner.Index;
      }
    }

    private struct TrafficSignNeeds
    {
      public uint m_SignTypeMask;
      public uint m_RemoveSignTypeMask;
      public uint m_SignTypeMask2;
      public uint m_RemoveSignTypeMask2;
      public ushort m_SpeedLimit;
      public ushort m_SpeedLimit2;
      public float m_VehicleLanesLeft;
      public float m_VehicleLanesRight;
      public ushort m_VehicleMask;
      public ushort m_CrossingLeftMask;
      public ushort m_CrossingRightMask;
      public LaneDirectionType m_Left;
      public LaneDirectionType m_Forward;
      public LaneDirectionType m_Right;
    }

    private struct TrafficSignData
    {
      public Transform m_ParentTransform;
      public Transform m_ObjectTransform;
      public Transform m_LocalTransform;
      public float2 m_ForwardDirection;
      public Entity m_Prefab;
      public int m_Probability;
      public SubObjectFlags m_Flags;
      public SecondaryObjectSystem.TrafficSignNeeds m_TrafficSignNeeds;
      public bool m_IsLowered;
    }

    private struct StreetLightData
    {
      public Transform m_ParentTransform;
      public Transform m_ObjectTransform;
      public Transform m_LocalTransform;
      public Entity m_Prefab;
      public int m_Probability;
      public SubObjectFlags m_Flags;
      public StreetLightLayer m_Layer;
      public float m_Spacing;
      public float m_Priority;
      public bool m_IsLowered;
    }

    private struct UtilityObjectData
    {
      public float3 m_UtilityPosition;
    }

    private struct UtilityNodeData
    {
      public Transform m_Transform;
      public Entity m_Prefab;
      public PathNode m_PathNode;
      public int m_Count;
      public float m_LanePriority;
      public float m_NodePriority;
      public float m_Elevation;
      public UtilityTypes m_UtilityTypes;
      public bool m_Unsure;
      public bool m_Vertical;
      public bool m_IsNew;
    }

    private struct TargetLaneData
    {
      public CarLaneFlags m_CarLaneFlags;
      public CarLaneFlags m_AndCarLaneFlags;
      public float2 m_SpeedLimit;
    }

    private struct PlaceholderKey : IEquatable<SecondaryObjectSystem.PlaceholderKey>
    {
      public Entity m_GroupPrefab;
      public int m_GroupIndex;

      public PlaceholderKey(Entity groupPrefab, int groupIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_GroupPrefab = groupPrefab;
        // ISSUE: reference to a compiler-generated field
        this.m_GroupIndex = groupIndex;
      }

      public bool Equals(SecondaryObjectSystem.PlaceholderKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_GroupPrefab.Equals(other.m_GroupPrefab) && this.m_GroupIndex == other.m_GroupIndex;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (17 * 31 + this.m_GroupPrefab.GetHashCode()) * 31 + this.m_GroupIndex.GetHashCode();
      }
    }

    private struct UpdateSecondaryObjectsData
    {
      public NativeParallelMultiHashMap<Entity, Entity> m_OldEntities;
      public NativeParallelMultiHashMap<Entity, Entity> m_OriginalEntities;
      public NativeParallelHashSet<Entity> m_PlaceholderRequirements;
      public NativeParallelHashMap<SecondaryObjectSystem.PlaceholderKey, Unity.Mathematics.Random> m_SelectedSpawnabled;
      public NativeParallelHashMap<PathNode, SecondaryObjectSystem.TargetLaneData> m_SourceLanes;
      public NativeParallelHashMap<PathNode, SecondaryObjectSystem.TargetLaneData> m_TargetLanes;
      public NativeList<SecondaryObjectSystem.TrafficSignData> m_TrafficSigns;
      public NativeList<SecondaryObjectSystem.StreetLightData> m_StreetLights;
      public NativeList<SecondaryObjectSystem.UtilityObjectData> m_UtilityObjects;
      public NativeList<SecondaryObjectSystem.UtilityNodeData> m_UtilityNodes;
      public bool m_RequirementsSearched;

      public void EnsureOldEntities(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldEntities.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_OldEntities = new NativeParallelMultiHashMap<Entity, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureOriginalEntities(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalEntities.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalEntities = new NativeParallelMultiHashMap<Entity, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsurePlaceholderRequirements(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceholderRequirements.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_PlaceholderRequirements = new NativeParallelHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureSelectedSpawnables(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedSpawnabled.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedSpawnabled = new NativeParallelHashMap<SecondaryObjectSystem.PlaceholderKey, Unity.Mathematics.Random>(10, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureSourceLanes(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SourceLanes.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_SourceLanes = new NativeParallelHashMap<PathNode, SecondaryObjectSystem.TargetLaneData>(16, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureTargetLanes(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetLanes.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TargetLanes = new NativeParallelHashMap<PathNode, SecondaryObjectSystem.TargetLaneData>(16, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureTrafficSigns(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TrafficSigns.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TrafficSigns = new NativeList<SecondaryObjectSystem.TrafficSignData>(16, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureStreetLights(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_StreetLights.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_StreetLights = new NativeList<SecondaryObjectSystem.StreetLightData>(16, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureUtilityObjects(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_UtilityObjects.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_UtilityObjects = new NativeList<SecondaryObjectSystem.UtilityObjectData>(16, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureUtilityNodes(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_UtilityNodes.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_UtilityNodes = new NativeList<SecondaryObjectSystem.UtilityNodeData>(16, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OldEntities.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OriginalEntities.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceholderRequirements.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PlaceholderRequirements.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedSpawnabled.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedSpawnabled.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SourceLanes.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SourceLanes.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetLanes.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TargetLanes.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TrafficSigns.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TrafficSigns.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_StreetLights.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_StreetLights.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UtilityObjects.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UtilityObjects.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UtilityNodes.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UtilityNodes.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_RequirementsSearched = false;
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OldEntities.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OriginalEntities.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceholderRequirements.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PlaceholderRequirements.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedSpawnabled.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedSpawnabled.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SourceLanes.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SourceLanes.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetLanes.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TargetLanes.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TrafficSigns.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TrafficSigns.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_StreetLights.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_StreetLights.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UtilityObjects.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UtilityObjects.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_UtilityNodes.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_UtilityNodes.Dispose();
      }
    }

    [BurstCompile]
    private struct FillUpdateMapJob : IJob
    {
      public NativeQueue<SecondaryObjectSystem.UpdateData> m_UpdateQueue;
      public NativeParallelMultiHashMap<Entity, SecondaryObjectSystem.UpdateData> m_UpdateMap;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        SecondaryObjectSystem.UpdateData updateData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_UpdateQueue.TryDequeue(out updateData))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateMap.Add(updateData.m_Owner, updateData);
        }
      }
    }

    [BurstCompile]
    private struct SecondaryLaneAnchorJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> m_UtilityLaneType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> m_PrefabSubLanes;
      [ReadOnly]
      public NativeParallelMultiHashMap<Entity, SecondaryObjectSystem.UpdateData> m_UpdateMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray1 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.UtilityLane> nativeArray2 = chunk.GetNativeArray<Game.Net.UtilityLane>(ref this.m_UtilityLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeLane> nativeArray3 = chunk.GetNativeArray<EdgeLane>(ref this.m_EdgeLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray4 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Game.Net.UtilityLane utilityLane = nativeArray2[index];
          if ((utilityLane.m_Flags & (UtilityLaneFlags.SecondaryStartAnchor | UtilityLaneFlags.SecondaryEndAnchor)) != (UtilityLaneFlags) 0)
          {
            Owner owner1 = nativeArray1[index];
            Curve curve = nativeArray4[index];
            PrefabRef prefabRef = nativeArray5[index];
            if ((utilityLane.m_Flags & UtilityLaneFlags.SecondaryStartAnchor) != (UtilityLaneFlags) 0)
            {
              Entity owner2 = owner1.m_Owner;
              float3 float3 = math.normalizesafe(-MathUtils.StartTangent(curve.m_Bezier) with
              {
                y = 0.0f
              });
              float requireDirection = float.MinValue;
              if (nativeArray3.Length != 0)
              {
                EdgeLane edgeLane = nativeArray3[index];
                if ((double) edgeLane.m_EdgeDelta.x == 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  owner2 = this.m_EdgeData[owner1.m_Owner].m_Start;
                }
                else if ((double) edgeLane.m_EdgeDelta.x == 1.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  owner2 = this.m_EdgeData[owner1.m_Owner].m_End;
                }
                if ((double) edgeLane.m_EdgeDelta.x == (double) edgeLane.m_EdgeDelta.y)
                  requireDirection = math.dot(float3, curve.m_Bezier.d - curve.m_Bezier.a);
              }
              // ISSUE: reference to a compiler-generated method
              curve.m_Bezier.a = this.FindAnchorPosition(owner2, prefabRef.m_Prefab, curve.m_Bezier.a, float3, requireDirection);
            }
            if ((utilityLane.m_Flags & UtilityLaneFlags.SecondaryEndAnchor) != (UtilityLaneFlags) 0)
            {
              Entity owner3 = owner1.m_Owner;
              float3 float3 = math.normalizesafe(MathUtils.EndTangent(curve.m_Bezier) with
              {
                y = 0.0f
              });
              float requireDirection = float.MinValue;
              if (nativeArray3.Length != 0)
              {
                EdgeLane edgeLane = nativeArray3[index];
                if ((double) edgeLane.m_EdgeDelta.y == 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  owner3 = this.m_EdgeData[owner1.m_Owner].m_Start;
                }
                else if ((double) edgeLane.m_EdgeDelta.y == 1.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  owner3 = this.m_EdgeData[owner1.m_Owner].m_End;
                }
                if ((double) edgeLane.m_EdgeDelta.x == (double) edgeLane.m_EdgeDelta.y)
                  requireDirection = math.dot(float3, curve.m_Bezier.a - curve.m_Bezier.d);
              }
              // ISSUE: reference to a compiler-generated method
              curve.m_Bezier.d = this.FindAnchorPosition(owner3, prefabRef.m_Prefab, curve.m_Bezier.d, float3, requireDirection);
            }
            // ISSUE: reference to a compiler-generated field
            UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[prefabRef.m_Prefab];
            if ((double) utilityLaneData.m_Hanging != 0.0)
            {
              curve.m_Bezier.b = math.lerp(curve.m_Bezier.a, curve.m_Bezier.d, 0.333333343f);
              curve.m_Bezier.c = math.lerp(curve.m_Bezier.a, curve.m_Bezier.d, 0.6666667f);
              float num = (float) ((double) math.distance(curve.m_Bezier.a.xz, curve.m_Bezier.d.xz) * (double) utilityLaneData.m_Hanging * 1.3333333730697632);
              curve.m_Bezier.b.y -= num;
              curve.m_Bezier.c.y -= num;
            }
            curve.m_Length = MathUtils.Length(curve.m_Bezier);
            nativeArray4[index] = curve;
          }
        }
      }

      private float3 FindAnchorPosition(
        Entity owner,
        Entity prefab,
        float3 position,
        float3 direction,
        float requireDirection)
      {
        float3 bestPosition = position;
        float maxValue = float.MaxValue;
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdatedData.HasComponent(owner))
        {
          // ISSUE: variable of a compiler-generated type
          SecondaryObjectSystem.UpdateData updateData;
          NativeParallelMultiHashMapIterator<Entity> it;
          // ISSUE: reference to a compiler-generated field
          if (this.m_UpdateMap.TryGetFirstValue(owner, out updateData, out it))
          {
            // ISSUE: reference to a compiler-generated field
            do
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.FindAnchorPosition(updateData.m_Prefab, prefab, updateData.m_Transform, position, direction, requireDirection, ref bestPosition, ref maxValue);
            }
            while (this.m_UpdateMap.TryGetNextValue(out updateData, ref it));
          }
        }
        else
        {
          DynamicBuffer<SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(owner, out bufferData))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              SubObject subObject = bufferData[index];
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[subObject.m_SubObject];
              // ISSUE: reference to a compiler-generated field
              Transform ownerTransform = this.m_TransformData[subObject.m_SubObject];
              // ISSUE: reference to a compiler-generated method
              this.FindAnchorPosition(prefabRef.m_Prefab, prefab, ownerTransform, position, direction, requireDirection, ref bestPosition, ref maxValue);
            }
          }
        }
        return bestPosition;
      }

      private void FindAnchorPosition(
        Entity ownerPrefab,
        Entity lanePrefab,
        Transform ownerTransform,
        float3 lanePosition,
        float3 laneDirection,
        float requireDirection,
        ref float3 bestPosition,
        ref float bestDistance)
      {
        DynamicBuffer<Game.Prefabs.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubLanes.TryGetBuffer(ownerPrefab, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Game.Prefabs.SubLane subLane = bufferData[index];
          if (!(subLane.m_Prefab != lanePrefab) && subLane.m_NodeIndex.x == subLane.m_NodeIndex.y)
          {
            float3 world = ObjectUtils.LocalToWorld(ownerTransform, subLane.m_Curve.a);
            float3 float3 = world - lanePosition;
            if ((double) requireDirection != -3.4028234663852886E+38)
            {
              if ((double) math.dot(laneDirection, float3) < (double) requireDirection)
                continue;
            }
            else
              float3 -= laneDirection * (math.dot(laneDirection, float3) * 0.75f);
            float num = math.lengthsq(float3);
            if ((double) num < (double) bestDistance)
            {
              bestPosition = world;
              bestDistance = num;
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
    private struct CheckSubObjectOwnersJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Secondary> m_SecondaryData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SecondaryObjectSystem.SubObjectOwnerData>.ParallelWriter m_OwnerQueue;

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
        BufferAccessor<SubObject> bufferAccessor = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity entity = nativeArray1[index1];
            DynamicBuffer<SubObject> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity subObject = dynamicBuffer[index2].m_SubObject;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_DeletedData.HasComponent(subObject) && this.m_SecondaryData.HasComponent(subObject) && this.m_OwnerData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == entity)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, subObject, in this.m_AppliedTypes);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, subObject, new Deleted());
                // ISSUE: reference to a compiler-generated field
                if (this.m_SubObjects.HasBuffer(subObject))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_OwnerQueue.Enqueue(new SecondaryObjectSystem.SubObjectOwnerData(subObject, Entity.Null, false, true));
                }
              }
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity owner = nativeArray1[index];
            if (nativeArray2.Length != 0)
            {
              Temp temp = nativeArray2[index];
              if ((temp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_OwnerQueue.Enqueue(new SecondaryObjectSystem.SubObjectOwnerData(owner, Entity.Null, true, false));
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_OwnerQueue.Enqueue(new SecondaryObjectSystem.SubObjectOwnerData(owner, temp.m_Original, true, false));
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_OwnerQueue.Enqueue(new SecondaryObjectSystem.SubObjectOwnerData(owner, Entity.Null, false, false));
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
    private struct CollectSubObjectOwnersJob : IJob
    {
      public NativeQueue<SecondaryObjectSystem.SubObjectOwnerData> m_OwnerQueue;
      public NativeList<SecondaryObjectSystem.SubObjectOwnerData> m_OwnerList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_OwnerQueue.Count;
        if (count == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerList.ResizeUninitialized(count);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OwnerList[index] = this.m_OwnerQueue.Dequeue();
        }
        // ISSUE: reference to a compiler-generated method
        this.MergeOwners();
      }

      private void MergeOwners()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerList.Sort<SecondaryObjectSystem.SubObjectOwnerData>();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SecondaryObjectSystem.SubObjectOwnerData subObjectOwnerData = new SecondaryObjectSystem.SubObjectOwnerData();
        int num = 0;
        int index = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_OwnerList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          SecondaryObjectSystem.SubObjectOwnerData owner = this.m_OwnerList[num++];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (owner.m_Owner != subObjectOwnerData.m_Owner)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (subObjectOwnerData.m_Owner != Entity.Null && !subObjectOwnerData.m_Deleted)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_OwnerList[index++] = subObjectOwnerData;
            }
            subObjectOwnerData = owner;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (owner.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              owner.m_Deleted |= subObjectOwnerData.m_Deleted;
              subObjectOwnerData = owner;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              subObjectOwnerData.m_Deleted |= owner.m_Deleted;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (subObjectOwnerData.m_Owner != Entity.Null && !subObjectOwnerData.m_Deleted)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OwnerList[index++] = subObjectOwnerData;
        }
        // ISSUE: reference to a compiler-generated field
        if (index >= this.m_OwnerList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerList.RemoveRange(index, this.m_OwnerList.Length - index);
      }
    }

    [BurstCompile]
    private struct UpdateSubObjectsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabNetCompositionData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<TrafficLightData> m_PrefabTrafficLightData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.TrafficSignData> m_PrefabTrafficSignData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.StreetLightData> m_PrefabStreetLightData;
      [ReadOnly]
      public ComponentLookup<LaneDirectionData> m_PrefabLaneDirectionData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<ThemeData> m_PrefabThemeData;
      [ReadOnly]
      public ComponentLookup<PlaceholderObjectData> m_PrefabPlaceholderObjectData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.UtilityObjectData> m_PrefabUtilityObjectData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<TreeData> m_PrefabTreeData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Secondary> m_SecondaryData;
      [ReadOnly]
      public ComponentLookup<TrafficLight> m_TrafficLightData;
      [ReadOnly]
      public ComponentLookup<StreetLight> m_StreetLightData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NetNodeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_NetEdgeData;
      [ReadOnly]
      public ComponentLookup<Composition> m_NetCompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;
      [ReadOnly]
      public ComponentLookup<Game.Net.SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.UtilityLane> m_UtilityLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<TrafficLights> m_TrafficLightsData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<NetCompositionObject> m_NetCompositionObjects;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PlaceholderObjects;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_ObjectRequirements;
      [ReadOnly]
      public BufferLookup<DefaultNetLane> m_DefaultNetLanes;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public BufferLookup<SubReplacement> m_SubReplacements;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Entity m_DefaultTheme;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      [ReadOnly]
      public ComponentTypeSet m_SecondaryOwnerTypes;
      [ReadOnly]
      public ComponentTypeSet m_TempAnimationTypes;
      [ReadOnly]
      public NativeArray<SecondaryObjectSystem.SubObjectOwnerData> m_OwnerList;
      public NativeQueue<SecondaryObjectSystem.UpdateData>.ParallelWriter m_UpdateQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        SecondaryObjectSystem.SubObjectOwnerData owner = this.m_OwnerList[index];
        bool flag1 = false;
        bool flag2 = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool isNative = this.m_NativeData.HasComponent(owner.m_Owner);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetNodeData.HasComponent(owner.m_Owner))
        {
          flag1 = true;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetEdgeData.HasComponent(owner.m_Owner))
            flag2 = true;
        }
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SecondaryObjectSystem.UpdateSecondaryObjectsData updateData = new SecondaryObjectSystem.UpdateSecondaryObjectsData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubObject> subObject1 = this.m_SubObjects[owner.m_Owner];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FillOldSubObjectsBuffer(owner.m_Owner, subObject1, ref updateData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (owner.m_Original != Entity.Null && this.m_SubObjects.HasBuffer(owner.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubObject> subObject2 = this.m_SubObjects[owner.m_Original];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FillOriginalSubObjectsBuffer(owner.m_Original, subObject2, ref updateData);
        }
        Temp ownerTemp = new Temp();
        // ISSUE: reference to a compiler-generated field
        if (owner.m_Temp)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ownerTemp = this.m_TempData[owner.m_Owner];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = !this.m_PseudoRandomSeedData.HasComponent(owner.m_Owner) ? this.m_RandomSeed.GetRandom(index) : this.m_PseudoRandomSeedData[owner.m_Owner].GetRandom((uint) PseudoRandomSeed.kSecondaryObject);
        bool hasStreetLights = false;
        bool alwaysLit = false;
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateSecondaryNodeObjects(index, ref random, owner.m_Owner, ref updateData, owner.m_Temp, isNative, ownerTemp, out hasStreetLights, out alwaysLit);
        }
        else if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateSecondaryEdgeObjects(index, ref random, owner.m_Owner, ref updateData, owner.m_Temp, isNative, ownerTemp, out hasStreetLights, out alwaysLit);
        }
        Road componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoadData.TryGetComponent(owner.m_Owner, out componentData))
        {
          if (hasStreetLights)
          {
            componentData.m_Flags |= Game.Net.RoadFlags.IsLit;
            if (alwaysLit)
              componentData.m_Flags |= Game.Net.RoadFlags.AlwaysLit;
            else
              componentData.m_Flags &= ~Game.Net.RoadFlags.AlwaysLit;
          }
          else
            componentData.m_Flags &= ~(Game.Net.RoadFlags.IsLit | Game.Net.RoadFlags.AlwaysLit);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_RoadData[owner.m_Owner] = componentData;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.RemoveUnusedOldSubObjects(index, owner.m_Owner, subObject1, ref updateData);
        // ISSUE: reference to a compiler-generated method
        updateData.Dispose();
      }

      private void FillOldSubObjectsBuffer(
        Entity owner,
        DynamicBuffer<SubObject> subObjects,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        if (subObjects.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated method
        updateData.EnsureOldEntities(Allocator.Temp);
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(subObject) && this.m_SecondaryData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == owner)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subObject];
            // ISSUE: reference to a compiler-generated field
            updateData.m_OldEntities.Add(prefabRef.m_Prefab, subObject);
          }
        }
      }

      private void FillOriginalSubObjectsBuffer(
        Entity owner,
        DynamicBuffer<SubObject> subObjects,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        if (subObjects.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated method
        updateData.EnsureOriginalEntities(Allocator.Temp);
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(subObject) && this.m_SecondaryData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == owner)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subObject];
            // ISSUE: reference to a compiler-generated field
            updateData.m_OriginalEntities.Add(prefabRef.m_Prefab, subObject);
          }
        }
      }

      private void RemoveUnusedOldSubObjects(
        int jobIndex,
        Entity owner,
        DynamicBuffer<SubObject> subObjects,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(subObject) && this.m_SecondaryData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == owner)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subObject];
            Entity e;
            NativeParallelMultiHashMapIterator<Entity> it;
            // ISSUE: reference to a compiler-generated field
            if (updateData.m_OldEntities.TryGetFirstValue(prefabRef.m_Prefab, out e, out it))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, e, in this.m_AppliedTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, e, new Deleted());
              // ISSUE: reference to a compiler-generated field
              updateData.m_OldEntities.Remove(it);
            }
          }
        }
      }

      private float Remap(float t, float4 syncOffsets, float4 syncTargets)
      {
        if ((double) t < (double) syncOffsets.x)
          return syncTargets.x * math.saturate(t / syncOffsets.x);
        if ((double) t < (double) syncOffsets.y)
          return math.lerp(syncTargets.x, syncTargets.y, math.saturate((float) (((double) t - (double) syncOffsets.x) / ((double) syncOffsets.y - (double) syncOffsets.x))));
        if ((double) t < (double) syncOffsets.z)
          return math.lerp(syncTargets.y, syncTargets.z, math.saturate((float) (((double) t - (double) syncOffsets.y) / ((double) syncOffsets.z - (double) syncOffsets.y))));
        return (double) t < (double) syncOffsets.w ? math.lerp(syncTargets.z, syncTargets.w, math.saturate((float) (((double) t - (double) syncOffsets.z) / ((double) syncOffsets.w - (double) syncOffsets.z)))) : math.lerp(syncTargets.w, 1f, math.saturate((float) (((double) t - (double) syncOffsets.w) / (1.0 - (double) syncOffsets.w))));
      }

      private Bezier4x3 LerpRemap(
        Bezier4x3 left,
        Bezier4x3 right,
        float t,
        float4 syncOffsets,
        float4 syncTargets)
      {
        // ISSUE: reference to a compiler-generated method
        float s1 = this.Remap(t, syncOffsets, syncTargets);
        float x = math.distance(left.a.xz, right.a.xz);
        float y = math.distance(left.d.xz, right.d.xz);
        float s2 = 0.5f;
        syncTargets = math.lerp(syncOffsets * x, syncTargets * y, s2) / math.max(1E-06f, math.lerp(x, y, s2));
        // ISSUE: reference to a compiler-generated method
        float s3 = this.Remap(t, syncOffsets, syncTargets);
        Bezier4x3 bezier4x3;
        bezier4x3.a = math.lerp(left.a, right.a, t);
        bezier4x3.b = math.lerp(left.b, right.b, t);
        bezier4x3.c = math.lerp(left.c, right.c, s3);
        bezier4x3.d = math.lerp(left.d, right.d, s1);
        return bezier4x3;
      }

      private Bezier4x3 LerpRemap2(
        Bezier4x3 left,
        Bezier4x3 right,
        float t,
        float4 syncOffsets,
        float4 syncTargets)
      {
        // ISSUE: reference to a compiler-generated method
        float s = this.Remap(t, syncOffsets, syncTargets);
        Bezier4x3 bezier4x3;
        bezier4x3.a = math.lerp(left.a, right.a, s);
        bezier4x3.b = math.lerp(left.b, right.b, s);
        bezier4x3.c = math.lerp(left.c, right.c, s);
        bezier4x3.d = math.lerp(left.d, right.d, s);
        return bezier4x3;
      }

      private void CreateSecondaryNodeObjects(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData,
        bool isTemp,
        bool isNative,
        Temp ownerTemp,
        out bool hasStreetLights,
        out bool alwaysLit)
      {
        alwaysLit = false;
        float num1 = 0.0f;
        float3 x1 = new float3();
        int num2 = 0;
        float ownerElevation1 = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetElevationData.HasComponent(owner))
        {
          // ISSUE: reference to a compiler-generated field
          ownerElevation1 = math.cmin(this.m_NetElevationData[owner].m_Elevation);
        }
        uint num3 = 0;
        uint num4 = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.EdgeIterator edgeIterator = new Game.Net.EdgeIterator(Entity.Null, owner, this.m_Edges, this.m_NetEdgeData, this.m_TempData, this.m_HiddenData, true);
        EdgeIteratorValue edgeIteratorValue;
        // ISSUE: variable of a compiler-generated type
        SecondaryObjectSystem.UtilityNodeData utilityNodeData1;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          if (!edgeIteratorValue.m_Middle)
          {
            // ISSUE: reference to a compiler-generated field
            Composition composition = this.m_NetCompositionData[edgeIteratorValue.m_Edge];
            // ISSUE: reference to a compiler-generated field
            EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edgeIteratorValue.m_Edge];
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData1 = this.m_PrefabNetCompositionData[composition.m_Edge];
            EdgeNodeGeometry geometry;
            NetCompositionData netCompositionData2;
            DynamicBuffer<NetCompositionObject> compositionObject1;
            if (edgeIteratorValue.m_End)
            {
              // ISSUE: reference to a compiler-generated field
              geometry = this.m_EndNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry;
              // ISSUE: reference to a compiler-generated field
              netCompositionData2 = this.m_PrefabNetCompositionData[composition.m_EndNode];
              // ISSUE: reference to a compiler-generated field
              compositionObject1 = this.m_NetCompositionObjects[composition.m_EndNode];
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              geometry = this.m_StartNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry;
              // ISSUE: reference to a compiler-generated field
              netCompositionData2 = this.m_PrefabNetCompositionData[composition.m_StartNode];
              // ISSUE: reference to a compiler-generated field
              compositionObject1 = this.m_NetCompositionObjects[composition.m_StartNode];
            }
            alwaysLit = ((alwaysLit ? 1 : 0) | ((netCompositionData2.m_Flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0 ? 0 : (((netCompositionData2.m_Flags.m_Left | netCompositionData2.m_Flags.m_Right) & (CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition)) == (CompositionFlags.Side) 0 ? 1 : 0))) != 0;
            bool flag = ((netCompositionData2.m_Flags.m_Left | netCompositionData2.m_Flags.m_Right) & CompositionFlags.Side.Lowered) > (CompositionFlags.Side) 0;
            float3 float3_1 = !edgeIteratorValue.m_End ? -(MathUtils.StartTangent(edgeGeometry.m_Start.m_Left) + MathUtils.StartTangent(edgeGeometry.m_Start.m_Right)) : MathUtils.EndTangent(edgeGeometry.m_End.m_Left) + MathUtils.StartTangent(edgeGeometry.m_End.m_Right);
            float2 x2;
            if ((double) geometry.m_MiddleRadius > 0.0)
            {
              num1 += math.max(math.distance(geometry.m_Middle.a, geometry.m_Middle.d) * 2f, geometry.m_Left.m_Length.x + geometry.m_Left.m_Length.y + geometry.m_Right.m_Length.x + geometry.m_Right.m_Length.y);
              x2 = MathUtils.StartTangent(geometry.m_Left.m_Left).xz + MathUtils.StartTangent(geometry.m_Left.m_Right).xz;
            }
            else
            {
              num1 += math.max(math.distance(geometry.m_Middle.a, geometry.m_Middle.d) * 2f, geometry.m_Left.m_Length.x + geometry.m_Right.m_Length.y);
              x2 = !(math.any(geometry.m_Left.m_Length > 0.05f) | math.any(geometry.m_Right.m_Length > 0.05f)) ? float3_1.xz : MathUtils.StartTangent(geometry.m_Left.m_Left).xz + MathUtils.StartTangent(geometry.m_Right.m_Right).xz;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[this.m_PrefabRefData[edgeIteratorValue.m_Edge].m_Prefab];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds1 = new SecondaryObjectSystem.TrafficSignNeeds();
            // ISSUE: reference to a compiler-generated field
            trafficSignNeeds1.m_Left = LaneDirectionType.None;
            // ISSUE: reference to a compiler-generated field
            trafficSignNeeds1.m_Forward = LaneDirectionType.None;
            // ISSUE: reference to a compiler-generated field
            trafficSignNeeds1.m_Right = LaneDirectionType.None;
            if ((netGeometryData.m_MergeLayers & (Layer.Road | Layer.TramTrack | Layer.PublicTransportRoad)) != Layer.None)
            {
              float2 float2 = math.normalizesafe(x2);
              uint num5 = (uint) (1 << Mathf.FloorToInt(math.atan2(float2.x, float2.y) * 5.092958f));
              if ((netCompositionData1.m_State & (edgeIteratorValue.m_End ? CompositionState.HasForwardRoadLanes : CompositionState.HasBackwardRoadLanes)) != (CompositionState) 0)
                num3 |= num5;
              if ((netCompositionData1.m_State & (edgeIteratorValue.m_End ? CompositionState.HasBackwardRoadLanes : CompositionState.HasForwardRoadLanes)) != (CompositionState) 0)
                num4 |= num5;
              if ((netCompositionData1.m_State & (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes)) == (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes))
              {
                // ISSUE: reference to a compiler-generated field
                trafficSignNeeds1.m_RemoveSignTypeMask2 = Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.DoNotEnter) | Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Oneway);
              }
              if ((netCompositionData2.m_Flags.m_General & CompositionFlags.General.Intersection) == (CompositionFlags.General) 0)
              {
                // ISSUE: reference to a compiler-generated field
                trafficSignNeeds1.m_RemoveSignTypeMask = Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Street) | Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoTurnLeft) | Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoTurnRight) | Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoUTurnLeft) | Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoUTurnRight);
              }
            }
            x1 = geometry.m_Middle.d;
            ++num2;
label_68:
            for (int index1 = 0; index1 < compositionObject1.Length; ++index1)
            {
              NetCompositionObject compositionObject2 = compositionObject1[index1];
              float num6 = (float) ((double) compositionObject2.m_Position.x / (double) math.max(1f, netCompositionData2.m_Width) + 0.5);
              float y1 = (float) ((double) netCompositionData2.m_MiddleOffset / (double) math.max(1f, netCompositionData2.m_Width) + 0.5);
              float2 float2 = new float2(math.cmin(compositionObject2.m_CurveOffsetRange), math.cmax(compositionObject2.m_CurveOffsetRange));
              if ((double) float2.y > 0.5)
                float2 = (double) float2.x <= 0.5 ? new float2((float) (((double) float2.x + 1.0 - (double) float2.y) * 0.5), 0.5f) : 1f - float2.yx;
              float num7 = math.lerp(float2.x, float2.y, random.NextFloat(1f));
              float num8;
              float3 position;
              float3 x3;
              float3 x4;
              if ((compositionObject2.m_Flags & (SubObjectFlags.OnMedian | SubObjectFlags.PreserveShape)) != (SubObjectFlags) 0)
              {
                Bezier4x3 middle = geometry.m_Middle;
                float3 x5;
                float3 y2;
                if ((double) geometry.m_MiddleRadius > 0.0)
                {
                  x5 = geometry.m_Left.m_Right.a - geometry.m_Left.m_Left.a;
                  y2 = geometry.m_Right.m_Right.d - geometry.m_Right.m_Left.d;
                }
                else
                {
                  x5 = geometry.m_Right.m_Right.a - geometry.m_Left.m_Left.a;
                  y2 = geometry.m_Right.m_Right.d - geometry.m_Left.m_Left.d;
                }
                if ((compositionObject2.m_Flags & SubObjectFlags.PreserveShape) != (SubObjectFlags) 0)
                  y2 = x5;
                num8 = MathUtils.Length(middle.xz);
                if ((double) num8 < 0.0099999997764825821)
                {
                  position = middle.a + x5 * (num6 - y1);
                  x3 = (double) geometry.m_MiddleRadius <= 0.0 ? MathUtils.StartTangent(geometry.m_Left.m_Left) + MathUtils.StartTangent(geometry.m_Right.m_Right) : MathUtils.StartTangent(geometry.m_Left.m_Left) + MathUtils.StartTangent(geometry.m_Left.m_Right);
                  x4 = x3;
                }
                else
                {
                  x3 = MathUtils.StartTangent(middle);
                  float length = num7 * 2f * num8 + compositionObject2.m_Position.y;
                  if ((double) length > 1.0 / 1000.0)
                  {
                    Bounds1 t = new Bounds1(0.0f, 1f);
                    MathUtils.ClampLength(middle.xz, ref t, length);
                    position = MathUtils.Position(middle, t.max) + math.lerp(x5, y2, t.max) * (num6 - y1);
                    x4 = MathUtils.Tangent(middle, t.max);
                  }
                  else
                  {
                    position = middle.a + x5 * (num6 - y1);
                    x4 = x3;
                  }
                }
                if ((double) geometry.m_MiddleRadius == 0.0 && math.all(geometry.m_Left.m_Length <= 0.05f) && math.all(geometry.m_Right.m_Length < 0.05f))
                {
                  x3 = float3_1;
                  x4 = float3_1;
                }
              }
              else if ((double) geometry.m_MiddleRadius > 0.0)
              {
                Bezier4x3 output1;
                Bezier4x3 output2;
                MathUtils.Divide(geometry.m_Middle, out output1, out output2, 0.99f);
                Bezier4x3 curve1;
                Bezier4x3 curve2;
                if ((double) num6 >= (double) y1)
                {
                  // ISSUE: reference to a compiler-generated method
                  curve1 = this.LerpRemap(output1, geometry.m_Left.m_Right, (num6 - y1) / math.max(1E-05f, 1f - y1), netCompositionData2.m_SyncVertexOffsetsRight, geometry.m_SyncVertexTargetsRight);
                  // ISSUE: reference to a compiler-generated method
                  curve2 = this.LerpRemap2(output2, geometry.m_Right.m_Right, (num6 - y1) / math.max(1E-05f, 1f - y1), netCompositionData2.m_SyncVertexOffsetsRight, geometry.m_SyncVertexTargetsRight);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  curve1 = this.LerpRemap(geometry.m_Left.m_Left, output1, num6 / math.max(1E-05f, y1), netCompositionData2.m_SyncVertexOffsetsLeft, geometry.m_SyncVertexTargetsLeft);
                  // ISSUE: reference to a compiler-generated method
                  curve2 = this.LerpRemap2(geometry.m_Right.m_Left, output2, num6 / math.max(1E-05f, y1), netCompositionData2.m_SyncVertexOffsetsLeft, geometry.m_SyncVertexTargetsLeft);
                }
                float num9 = MathUtils.Length(curve1.xz);
                float num10 = MathUtils.Length(curve2.xz);
                num8 = num9 + num10;
                x3 = MathUtils.StartTangent(curve1);
                float length = num7 * 2f * num8 + compositionObject2.m_Position.y;
                if ((double) length > (double) num9)
                {
                  Bounds1 t = new Bounds1(0.0f, 1f);
                  MathUtils.ClampLength(curve2.xz, ref t, length - num9);
                  position = MathUtils.Position(curve2, t.max);
                  x4 = MathUtils.Tangent(curve2, t.max);
                }
                else if ((double) length > 1.0 / 1000.0)
                {
                  Bounds1 t = new Bounds1(0.0f, 1f);
                  MathUtils.ClampLength(curve1.xz, ref t, length);
                  position = MathUtils.Position(curve1, t.max);
                  x4 = MathUtils.Tangent(curve1, t.max);
                }
                else
                {
                  position = curve1.a;
                  x4 = x3;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                Bezier4x3 curve = (double) num6 < (double) y1 ? this.LerpRemap(geometry.m_Left.m_Left, geometry.m_Left.m_Right, num6 / math.max(1E-05f, y1), netCompositionData2.m_SyncVertexOffsetsLeft, geometry.m_SyncVertexTargetsLeft) : this.LerpRemap(geometry.m_Right.m_Left, geometry.m_Right.m_Right, (num6 - y1) / math.max(1E-05f, 1f - y1), netCompositionData2.m_SyncVertexOffsetsRight, geometry.m_SyncVertexTargetsRight);
                num8 = MathUtils.Length(curve.xz);
                x3 = MathUtils.StartTangent(curve);
                float length = num7 * 2f * num8 + compositionObject2.m_Position.y;
                if ((double) length > 1.0 / 1000.0)
                {
                  Bounds1 t = new Bounds1(0.0f, 1f);
                  MathUtils.ClampLength(curve.xz, ref t, length);
                  position = MathUtils.Position(curve, t.max);
                  x4 = MathUtils.Tangent(curve, t.max);
                }
                else
                {
                  position = curve.a;
                  x4 = x3;
                }
                if (math.all(geometry.m_Left.m_Length <= 0.05f) && math.all(geometry.m_Right.m_Length < 0.05f))
                {
                  x3 = float3_1;
                  x4 = float3_1;
                }
              }
              x3.y = math.lerp(0.0f, x3.y, compositionObject2.m_UseCurveRotation.x);
              x4.y = math.lerp(0.0f, x4.y, compositionObject2.m_UseCurveRotation.x);
              float3 float3_2 = math.normalizesafe(x3, new float3(0.0f, 0.0f, 1f));
              float3 forward = math.normalizesafe(x4, float3_2);
              quaternion rotation = math.slerp(quaternion.LookRotationSafe(float3_2, math.up()), quaternion.LookRotationSafe(forward, math.up()), compositionObject2.m_UseCurveRotation.y);
              Transform parentTransform = new Transform(position, rotation);
              Transform transform = new Transform(compositionObject2.m_Offset, compositionObject2.m_Rotation);
              Transform world1 = ObjectUtils.LocalToWorld(parentTransform, transform);
              if (compositionObject2.m_Probability < 100)
                compositionObject2.m_Probability = math.clamp(Mathf.RoundToInt((float) compositionObject2.m_Probability * (num8 / netGeometryData.m_EdgeLengthRange.max)), 1, compositionObject2.m_Probability);
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabStreetLightData.HasComponent(compositionObject2.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Prefabs.StreetLightData streetLightData = this.m_PrefabStreetLightData[compositionObject2.m_Prefab];
                // ISSUE: reference to a compiler-generated method
                updateData.EnsureStreetLights(Allocator.Temp);
                // ISSUE: reference to a compiler-generated field
                for (int index2 = 0; index2 < updateData.m_StreetLights.Length; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) math.distancesq(updateData.m_StreetLights[index2].m_ObjectTransform.m_Position, world1.m_Position) < 1.0)
                    goto label_68;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                updateData.m_StreetLights.Add(new SecondaryObjectSystem.StreetLightData()
                {
                  m_ParentTransform = parentTransform,
                  m_ObjectTransform = world1,
                  m_LocalTransform = transform,
                  m_Prefab = compositionObject2.m_Prefab,
                  m_Probability = compositionObject2.m_Probability,
                  m_Flags = compositionObject2.m_Flags,
                  m_Layer = streetLightData.m_Layer,
                  m_Spacing = compositionObject2.m_Spacing,
                  m_Priority = num8,
                  m_IsLowered = flag
                });
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabTrafficSignData.HasComponent(compositionObject2.m_Prefab) || this.m_PrefabTrafficLightData.HasComponent(compositionObject2.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated method
                  updateData.EnsureTrafficSigns(Allocator.Temp);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  updateData.m_TrafficSigns.Add(new SecondaryObjectSystem.TrafficSignData()
                  {
                    m_ParentTransform = parentTransform,
                    m_ObjectTransform = world1,
                    m_LocalTransform = transform,
                    m_ForwardDirection = math.normalizesafe(math.forward(world1.m_Rotation).xz),
                    m_Prefab = compositionObject2.m_Prefab,
                    m_Probability = compositionObject2.m_Probability,
                    m_Flags = compositionObject2.m_Flags,
                    m_TrafficSignNeeds = trafficSignNeeds1,
                    m_IsLowered = flag
                  });
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabUtilityObjectData.HasComponent(compositionObject2.m_Prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Game.Prefabs.UtilityObjectData utilityObjectData = this.m_PrefabUtilityObjectData[compositionObject2.m_Prefab];
                    float3 world2 = ObjectUtils.LocalToWorld(world1, utilityObjectData.m_UtilityPosition);
                    // ISSUE: reference to a compiler-generated method
                    updateData.EnsureUtilityObjects(Allocator.Temp);
                    // ISSUE: reference to a compiler-generated field
                    for (int index3 = 0; index3 < updateData.m_UtilityObjects.Length; ++index3)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if ((double) math.distancesq(updateData.m_UtilityObjects[index3].m_UtilityPosition, world2) < 1.0)
                        goto label_68;
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: object of a compiler-generated type is created
                    updateData.m_UtilityObjects.Add(new SecondaryObjectSystem.UtilityObjectData()
                    {
                      m_UtilityPosition = world2
                    });
                  }
                  int jobIndex1 = jobIndex;
                  ref Unity.Mathematics.Random local1 = ref random;
                  Entity owner1 = owner;
                  int num11 = isTemp ? 1 : 0;
                  int num12 = flag ? 1 : 0;
                  int num13 = isNative ? 1 : 0;
                  Temp ownerTemp1 = ownerTemp;
                  double ownerElevation2 = (double) ownerElevation1;
                  Transform ownerTransform = parentTransform;
                  Transform transformData = world1;
                  Transform localTransformData = transform;
                  int flags = (int) compositionObject2.m_Flags;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds2 = new SecondaryObjectSystem.TrafficSignNeeds();
                  // ISSUE: variable of a compiler-generated type
                  SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds3 = trafficSignNeeds2;
                  ref SecondaryObjectSystem.UpdateSecondaryObjectsData local2 = ref updateData;
                  Entity prefab = compositionObject2.m_Prefab;
                  int probability = compositionObject2.m_Probability;
                  // ISSUE: reference to a compiler-generated method
                  this.CreateSecondaryObject(jobIndex1, ref local1, owner1, num11 != 0, false, num12 != 0, num13 != 0, (Game.Tools.AgeMask) 0, ownerTemp1, (float) ownerElevation2, ownerTransform, transformData, localTransformData, (SubObjectFlags) flags, trafficSignNeeds3, ref local2, prefab, 0, probability);
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[edgeIteratorValue.m_Edge];
          for (int index4 = 0; index4 < subLane1.Length; ++index4)
          {
            Entity subLane2 = subLane1[index4].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SecondaryLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_UtilityLaneData.HasComponent(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane2].m_Prefab];
                if (utilityLaneData.m_NodeObjectPrefab != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[subLane2];
                  // ISSUE: reference to a compiler-generated field
                  float a = math.length(MathUtils.Size(this.m_PrefabObjectGeometryData[utilityLaneData.m_NodeObjectPrefab].m_Bounds));
                  bool2 x6 = (bool2) false;
                  bool c = false;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_EdgeLaneData.HasComponent(subLane2))
                  {
                    if (!edgeIteratorValue.m_Middle)
                    {
                      // ISSUE: reference to a compiler-generated field
                      EdgeLane edgeLane = this.m_EdgeLaneData[subLane2];
                      x6 = !edgeIteratorValue.m_End & edgeLane.m_EdgeDelta == 0.0f | edgeIteratorValue.m_End & edgeLane.m_EdgeDelta == 1f;
                    }
                  }
                  else if (edgeIteratorValue.m_Middle)
                  {
                    x6 = new bool2(false, true);
                    c = true;
                  }
                  if (math.any(x6))
                  {
                    // ISSUE: reference to a compiler-generated method
                    updateData.EnsureUtilityNodes(Allocator.Temp);
                    // ISSUE: reference to a compiler-generated field
                    for (int index5 = 0; index5 < updateData.m_UtilityNodes.Length; ++index5)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: variable of a compiler-generated type
                      SecondaryObjectSystem.UtilityNodeData utilityNode = updateData.m_UtilityNodes[index5];
                      // ISSUE: reference to a compiler-generated field
                      if ((utilityNode.m_UtilityTypes & utilityLaneData.m_UtilityTypes) != UtilityTypes.None)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (x6.x && (double) math.distancesq(utilityNode.m_Transform.m_Position, curve.m_Bezier.a) < 0.0099999997764825821)
                        {
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Unsure &= c;
                          // ISSUE: reference to a compiler-generated field
                          if (!c && (double) a > (double) utilityNode.m_NodePriority)
                          {
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_NodePriority = a;
                          }
                          // ISSUE: reference to a compiler-generated field
                          if ((double) a > (double) utilityNode.m_LanePriority)
                          {
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_LanePriority = a;
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_Count = 1;
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_Vertical = c;
                          }
                          else
                          {
                            // ISSUE: reference to a compiler-generated field
                            if ((double) a == (double) utilityNode.m_LanePriority)
                            {
                              // ISSUE: reference to a compiler-generated field
                              ++utilityNode.m_Count;
                              // ISSUE: reference to a compiler-generated field
                              utilityNode.m_Vertical |= c;
                            }
                          }
                          // ISSUE: reference to a compiler-generated field
                          updateData.m_UtilityNodes[index5] = utilityNode;
                          x6.x = false;
                          if (!x6.y)
                            break;
                        }
                        // ISSUE: reference to a compiler-generated field
                        if (x6.y && (double) math.distancesq(utilityNode.m_Transform.m_Position, curve.m_Bezier.d) < 0.0099999997764825821)
                        {
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Unsure &= c;
                          // ISSUE: reference to a compiler-generated field
                          if (!c && (double) a > (double) utilityNode.m_NodePriority)
                          {
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_NodePriority = a;
                          }
                          // ISSUE: reference to a compiler-generated field
                          if ((double) a > (double) utilityNode.m_LanePriority)
                          {
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_LanePriority = a;
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_Count = 1;
                            // ISSUE: reference to a compiler-generated field
                            utilityNode.m_Vertical = c;
                          }
                          else
                          {
                            // ISSUE: reference to a compiler-generated field
                            if ((double) a == (double) utilityNode.m_LanePriority)
                            {
                              // ISSUE: reference to a compiler-generated field
                              ++utilityNode.m_Count;
                              // ISSUE: reference to a compiler-generated field
                              utilityNode.m_Vertical |= c;
                            }
                          }
                          // ISSUE: reference to a compiler-generated field
                          updateData.m_UtilityNodes[index5] = utilityNode;
                          x6.y = false;
                          if (!x6.x)
                            break;
                        }
                      }
                    }
                    if (x6.x)
                    {
                      // ISSUE: object of a compiler-generated type is created
                      utilityNodeData1 = new SecondaryObjectSystem.UtilityNodeData();
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Transform = new Transform(curve.m_Bezier.a, NetUtils.GetNodeRotation(MathUtils.StartTangent(curve.m_Bezier)));
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Count = 1;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_LanePriority = a;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_NodePriority = math.select(a, 0.0f, c);
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_UtilityTypes = utilityLaneData.m_UtilityTypes;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Unsure = c;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Vertical = c;
                      // ISSUE: variable of a compiler-generated type
                      SecondaryObjectSystem.UtilityNodeData utilityNodeData2 = utilityNodeData1;
                      // ISSUE: reference to a compiler-generated field
                      updateData.m_UtilityNodes.Add(in utilityNodeData2);
                    }
                    if (x6.y)
                    {
                      // ISSUE: object of a compiler-generated type is created
                      utilityNodeData1 = new SecondaryObjectSystem.UtilityNodeData();
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Transform = new Transform(curve.m_Bezier.d, NetUtils.GetNodeRotation(MathUtils.EndTangent(curve.m_Bezier)));
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Count = 1;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_LanePriority = a;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_NodePriority = math.select(a, 0.0f, c);
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_UtilityTypes = utilityLaneData.m_UtilityTypes;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Unsure = c;
                      // ISSUE: reference to a compiler-generated field
                      utilityNodeData1.m_Vertical = c;
                      // ISSUE: variable of a compiler-generated type
                      SecondaryObjectSystem.UtilityNodeData utilityNodeData3 = utilityNodeData1;
                      // ISSUE: reference to a compiler-generated field
                      updateData.m_UtilityNodes.Add(in utilityNodeData3);
                    }
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_CarLaneData.HasComponent(subLane2) && !this.m_MasterLaneData.HasComponent(subLane2) && this.m_EdgeLaneData.HasComponent(subLane2))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.CarLane carLane = this.m_CarLaneData[subLane2];
                  // ISSUE: reference to a compiler-generated field
                  bool2 x7 = this.m_EdgeLaneData[subLane2].m_EdgeDelta == math.select(0.0f, 1f, edgeIteratorValue.m_End);
                  if (math.any(x7))
                  {
                    // ISSUE: reference to a compiler-generated method
                    updateData.EnsureTargetLanes(Allocator.Temp);
                    // ISSUE: reference to a compiler-generated field
                    Lane lane = this.m_LaneData[subLane2];
                    CarLaneFlags flags = carLane.m_Flags;
                    SlaveLane componentData1;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_SlaveLaneData.TryGetComponent(subLane2, out componentData1))
                    {
                      for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= (int) componentData1.m_MaxIndex; ++minIndex)
                      {
                        Game.Net.CarLane componentData2;
                        // ISSUE: reference to a compiler-generated field
                        if (minIndex != index4 && this.m_CarLaneData.TryGetComponent(subLane1[minIndex].m_SubLane, out componentData2))
                          flags |= componentData2.m_Flags;
                      }
                    }
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TargetLaneData targetLaneData = new SecondaryObjectSystem.TargetLaneData()
                    {
                      m_CarLaneFlags = carLane.m_Flags,
                      m_AndCarLaneFlags = flags,
                      m_SpeedLimit = (float2) carLane.m_DefaultSpeedLimit
                    };
                    // ISSUE: reference to a compiler-generated field
                    if (!updateData.m_TargetLanes.TryAdd(x7.x ? lane.m_StartNode : lane.m_EndNode, targetLaneData))
                      UnityEngine.Debug.Log((object) string.Format("SecondaryObjectSystem: Duplicate node for lane {0}", (object) subLane2.Index));
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        hasStreetLights = updateData.m_StreetLights.IsCreated && updateData.m_StreetLights.Length != 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag1 = updateData.m_TrafficSigns.IsCreated && updateData.m_TrafficSigns.Length != 0;
        if (hasStreetLights)
        {
          // ISSUE: reference to a compiler-generated field
          while (updateData.m_StreetLights.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            StreetLightLayer layer = updateData.m_StreetLights[0].m_Layer;
            float num14 = -1f;
            int index6 = 0;
            float y = 0.0f;
            int x8 = 0;
            // ISSUE: reference to a compiler-generated field
            for (int index7 = 0; index7 < updateData.m_StreetLights.Length; ++index7)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              SecondaryObjectSystem.StreetLightData streetLight = updateData.m_StreetLights[index7];
              // ISSUE: reference to a compiler-generated field
              if (streetLight.m_Layer == layer)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num15 = math.distance(x1, streetLight.m_ObjectTransform.m_Position) + streetLight.m_Priority;
                if ((double) num15 > (double) num14)
                {
                  num14 = num15;
                  index6 = index7;
                }
                // ISSUE: reference to a compiler-generated field
                y += streetLight.m_Spacing;
                ++x8;
              }
            }
            if (index6 != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              SecondaryObjectSystem.StreetLightData streetLight = updateData.m_StreetLights[index6];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              updateData.m_StreetLights[index6] = updateData.m_StreetLights[0];
              // ISSUE: reference to a compiler-generated field
              updateData.m_StreetLights[0] = streetLight;
            }
            int a = math.min(x8, Mathf.RoundToInt(num1 * (float) x8 / math.max(1f, y)));
            int num16 = math.select(a, 2, a == 3 && num2 == 4);
            for (int index8 = 1; index8 < num16; ++index8)
            {
              float num17 = -1f;
              int index9 = index8;
              // ISSUE: reference to a compiler-generated field
              for (int index10 = index8; index10 < updateData.m_StreetLights.Length; ++index10)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                SecondaryObjectSystem.StreetLightData streetLight = updateData.m_StreetLights[index10];
                // ISSUE: reference to a compiler-generated field
                if (streetLight.m_Layer == layer)
                {
                  float x9 = float.MaxValue;
                  for (int index11 = 0; index11 < index8; ++index11)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    x9 = math.min(x9, math.distance(streetLight.m_ObjectTransform.m_Position, updateData.m_StreetLights[index11].m_ObjectTransform.m_Position));
                  }
                  // ISSUE: reference to a compiler-generated field
                  float num18 = x9 + streetLight.m_Priority;
                  if ((double) num18 > (double) num17)
                  {
                    num17 = num18;
                    index9 = index10;
                  }
                }
              }
              if (index9 != index8)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                SecondaryObjectSystem.StreetLightData streetLight = updateData.m_StreetLights[index9];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                updateData.m_StreetLights[index9] = updateData.m_StreetLights[index8];
                // ISSUE: reference to a compiler-generated field
                updateData.m_StreetLights[index8] = streetLight;
              }
            }
            for (int index12 = 0; index12 < num16; ++index12)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              SecondaryObjectSystem.StreetLightData streetLight = updateData.m_StreetLights[index12];
              int jobIndex2 = jobIndex;
              ref Unity.Mathematics.Random local3 = ref random;
              Entity owner2 = owner;
              int num19 = isTemp ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              int num20 = streetLight.m_IsLowered ? 1 : 0;
              int num21 = isNative ? 1 : 0;
              Temp ownerTemp2 = ownerTemp;
              double ownerElevation3 = (double) ownerElevation1;
              // ISSUE: reference to a compiler-generated field
              Transform parentTransform = streetLight.m_ParentTransform;
              // ISSUE: reference to a compiler-generated field
              Transform objectTransform = streetLight.m_ObjectTransform;
              // ISSUE: reference to a compiler-generated field
              Transform localTransform = streetLight.m_LocalTransform;
              // ISSUE: reference to a compiler-generated field
              int flags = (int) streetLight.m_Flags;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds4 = new SecondaryObjectSystem.TrafficSignNeeds();
              // ISSUE: variable of a compiler-generated type
              SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds5 = trafficSignNeeds4;
              ref SecondaryObjectSystem.UpdateSecondaryObjectsData local4 = ref updateData;
              // ISSUE: reference to a compiler-generated field
              Entity prefab = streetLight.m_Prefab;
              // ISSUE: reference to a compiler-generated field
              int probability = streetLight.m_Probability;
              // ISSUE: reference to a compiler-generated method
              this.CreateSecondaryObject(jobIndex2, ref local3, owner2, num19 != 0, false, num20 != 0, num21 != 0, (Game.Tools.AgeMask) 0, ownerTemp2, (float) ownerElevation3, parentTransform, objectTransform, localTransform, (SubObjectFlags) flags, trafficSignNeeds5, ref local4, prefab, 0, probability);
            }
            int index13 = 0;
            // ISSUE: reference to a compiler-generated field
            while (index13 < updateData.m_StreetLights.Length)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (updateData.m_StreetLights[index13].m_Layer != layer)
              {
                ++index13;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                updateData.m_StreetLights.RemoveAtSwapBack(index13);
              }
            }
          }
        }
        if (num2 == 0)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[owner];
          DynamicBuffer<DefaultNetLane> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_DefaultNetLanes.TryGetBuffer(prefabRef.m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NetNodeData[owner];
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[prefabRef.m_Prefab];
            for (int index = 0; index < bufferData.Length; ++index)
            {
              NetCompositionLane netCompositionLane = new NetCompositionLane(bufferData[index]);
              if ((netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Utility) != (Game.Prefabs.LaneFlags) 0 && (netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.FindAnchor) == (Game.Prefabs.LaneFlags) 0)
              {
                bool flag2 = (netCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Invert) != 0;
                if ((netCompositionLane.m_Flags & (flag2 ? Game.Prefabs.LaneFlags.DisconnectedEnd : Game.Prefabs.LaneFlags.DisconnectedStart)) == (Game.Prefabs.LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[netCompositionLane.m_Lane];
                  if (utilityLaneData.m_NodeObjectPrefab != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    float num22 = math.length(MathUtils.Size(this.m_PrefabObjectGeometryData[utilityLaneData.m_NodeObjectPrefab].m_Bounds));
                    netCompositionLane.m_Position.x = -netCompositionLane.m_Position.x;
                    float s = (float) ((double) netCompositionLane.m_Position.x / (double) math.max(1f, netGeometryData.m_DefaultWidth) + 0.5);
                    float3 y = node.m_Position + math.rotate(node.m_Rotation, new float3(netGeometryData.m_DefaultWidth * -0.5f, 0.0f, 0.0f));
                    float3 float3 = math.lerp(node.m_Position + math.rotate(node.m_Rotation, new float3(netGeometryData.m_DefaultWidth * 0.5f, 0.0f, 0.0f)), y, s);
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.UtilityNodeData utilityNodeData4 = new SecondaryObjectSystem.UtilityNodeData();
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_Transform.m_Position = float3;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_Transform.m_Position.y += netCompositionLane.m_Position.y;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_Count = 1;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_Elevation = netCompositionLane.m_Position.y;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_LanePriority = num22;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_NodePriority = num22;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData4.m_UtilityTypes = utilityLaneData.m_UtilityTypes;
                    // ISSUE: reference to a compiler-generated method
                    updateData.EnsureUtilityNodes(Allocator.Temp);
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_UtilityNodes.Add(in utilityNodeData4);
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane3 = this.m_SubLanes[owner];
        for (int index14 = 0; index14 < subLane3.Length; ++index14)
        {
          Entity subLane4 = subLane3[index14].m_SubLane;
          float3 float3;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SecondaryLaneData.HasComponent(subLane4))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_UtilityLaneData.HasComponent(subLane4))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane4].m_Prefab];
              if (utilityLaneData.m_NodeObjectPrefab != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane4];
                if ((double) curve.m_Length > 0.10000000149011612)
                {
                  // ISSUE: reference to a compiler-generated field
                  float num23 = math.length(MathUtils.Size(this.m_PrefabObjectGeometryData[utilityLaneData.m_NodeObjectPrefab].m_Bounds));
                  bool2 bool2 = (bool2) false;
                  // ISSUE: reference to a compiler-generated method
                  updateData.EnsureUtilityNodes(Allocator.Temp);
                  // ISSUE: reference to a compiler-generated field
                  for (int index15 = 0; index15 < updateData.m_UtilityNodes.Length; ++index15)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.UtilityNodeData utilityNode = updateData.m_UtilityNodes[index15];
                    // ISSUE: reference to a compiler-generated field
                    if ((utilityNode.m_UtilityTypes & utilityLaneData.m_UtilityTypes) != UtilityTypes.None)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (!bool2.x && (double) math.distancesq(utilityNode.m_Transform.m_Position, curve.m_Bezier.a) < 0.0099999997764825821)
                      {
                        // ISSUE: reference to a compiler-generated field
                        utilityNode.m_Unsure = false;
                        // ISSUE: reference to a compiler-generated field
                        if ((double) num23 > (double) utilityNode.m_NodePriority)
                        {
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_NodePriority = num23;
                        }
                        // ISSUE: reference to a compiler-generated field
                        if ((double) num23 > (double) utilityNode.m_LanePriority)
                        {
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_LanePriority = num23;
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Count = 1;
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Vertical = false;
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          if ((double) num23 == (double) utilityNode.m_LanePriority)
                          {
                            // ISSUE: reference to a compiler-generated field
                            ++utilityNode.m_Count;
                          }
                        }
                        // ISSUE: reference to a compiler-generated field
                        updateData.m_UtilityNodes[index15] = utilityNode;
                        bool2.x = true;
                        if (bool2.y)
                          break;
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (!bool2.y && (double) math.distancesq(utilityNode.m_Transform.m_Position, curve.m_Bezier.d) < 0.0099999997764825821)
                      {
                        // ISSUE: reference to a compiler-generated field
                        utilityNode.m_Unsure = false;
                        // ISSUE: reference to a compiler-generated field
                        if ((double) num23 > (double) utilityNode.m_NodePriority)
                        {
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_NodePriority = num23;
                        }
                        // ISSUE: reference to a compiler-generated field
                        if ((double) num23 > (double) utilityNode.m_LanePriority)
                        {
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_LanePriority = num23;
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Count = 1;
                          // ISSUE: reference to a compiler-generated field
                          utilityNode.m_Vertical = false;
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          if ((double) num23 == (double) utilityNode.m_LanePriority)
                          {
                            // ISSUE: reference to a compiler-generated field
                            ++utilityNode.m_Count;
                          }
                        }
                        // ISSUE: reference to a compiler-generated field
                        updateData.m_UtilityNodes[index15] = utilityNode;
                        bool2.y = true;
                        if (bool2.x)
                          break;
                      }
                    }
                  }
                  if (!bool2.x)
                  {
                    float num24 = 0.0f;
                    Game.Net.Elevation componentData;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_NetElevationData.TryGetComponent(subLane4, out componentData) && (double) componentData.m_Elevation.x != -3.4028234663852886E+38)
                      num24 = componentData.m_Elevation.x;
                    // ISSUE: object of a compiler-generated type is created
                    utilityNodeData1 = new SecondaryObjectSystem.UtilityNodeData();
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Transform = new Transform(curve.m_Bezier.a, NetUtils.GetNodeRotation(MathUtils.StartTangent(curve.m_Bezier)));
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Count = 1;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Elevation = num24;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_LanePriority = num23;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_NodePriority = num23;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_UtilityTypes = utilityLaneData.m_UtilityTypes;
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.UtilityNodeData utilityNodeData5 = utilityNodeData1;
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_UtilityNodes.Add(in utilityNodeData5);
                  }
                  if (!bool2.y)
                  {
                    float num25 = 0.0f;
                    Game.Net.Elevation componentData;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_NetElevationData.TryGetComponent(subLane4, out componentData) && (double) componentData.m_Elevation.y != -3.4028234663852886E+38)
                      num25 = componentData.m_Elevation.y;
                    // ISSUE: object of a compiler-generated type is created
                    utilityNodeData1 = new SecondaryObjectSystem.UtilityNodeData();
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Transform = new Transform(curve.m_Bezier.d, NetUtils.GetNodeRotation(MathUtils.EndTangent(curve.m_Bezier)));
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Count = 1;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_Elevation = num25;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_LanePriority = num23;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_NodePriority = num23;
                    // ISSUE: reference to a compiler-generated field
                    utilityNodeData1.m_UtilityTypes = utilityLaneData.m_UtilityTypes;
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.UtilityNodeData utilityNodeData6 = utilityNodeData1;
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_UtilityNodes.Add(in utilityNodeData6);
                  }
                }
              }
            }
            else if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarLaneData.HasComponent(subLane4))
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_MasterLaneData.HasComponent(subLane4))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.CarLane carLane = this.m_CarLaneData[subLane4];
                  // ISSUE: reference to a compiler-generated field
                  Lane lane = this.m_LaneData[subLane4];
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[subLane4];
                  if ((carLane.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                    carLane.m_Flags &= CarLaneFlags.Unsafe | CarLaneFlags.Highway | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit;
                  bool flag3 = (carLane.m_Flags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit)) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                  // ISSUE: reference to a compiler-generated field
                  bool flag4 = this.m_LaneSignalData.HasComponent(subLane4);
                  int index16 = -1;
                  int index17 = -1;
                  int index18 = -1;
                  int index19 = -1;
                  float num26 = float.MaxValue;
                  float num27 = float.MaxValue;
                  float num28 = float.MaxValue;
                  float num29 = float.MaxValue;
                  float3 = MathUtils.StartTangent(curve.m_Bezier);
                  float2 float2_1 = math.normalizesafe(float3.xz);
                  float3 = MathUtils.EndTangent(curve.m_Bezier);
                  float2 float2_2 = math.normalizesafe(float3.xz);
                  float3 a1 = curve.m_Bezier.a;
                  float3 a2 = curve.m_Bezier.a;
                  float3 d1 = curve.m_Bezier.d;
                  float3 d2 = curve.m_Bezier.d;
                  // ISSUE: reference to a compiler-generated field
                  a1.xz += MathUtils.Right(float2_1) * math.select(1.25f, -1.25f, this.m_LeftHandTraffic);
                  // ISSUE: reference to a compiler-generated field
                  a2.xz += MathUtils.Left(float2_1) * math.select(1.25f, -1.25f, this.m_LeftHandTraffic);
                  // ISSUE: reference to a compiler-generated field
                  d1.xz += MathUtils.Right(float2_2) * math.select(1.25f, -1.25f, this.m_LeftHandTraffic);
                  // ISSUE: reference to a compiler-generated field
                  d2.xz += MathUtils.Left(float2_2) * math.select(1.25f, -1.25f, this.m_LeftHandTraffic);
                  // ISSUE: reference to a compiler-generated field
                  for (int index20 = 0; index20 < updateData.m_TrafficSigns.Length; ++index20)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index20];
                    // ISSUE: reference to a compiler-generated field
                    double num30 = (double) math.dot(float2_1, trafficSign.m_ForwardDirection);
                    // ISSUE: reference to a compiler-generated field
                    float num31 = math.dot(float2_2, trafficSign.m_ForwardDirection);
                    if (num30 < -0.70710676908493042)
                    {
                      // ISSUE: reference to a compiler-generated field
                      float num32 = math.distance(a1, trafficSign.m_ObjectTransform.m_Position);
                      // ISSUE: reference to a compiler-generated field
                      float num33 = math.distance(a2, trafficSign.m_ObjectTransform.m_Position);
                      if ((double) num32 < (double) num26)
                      {
                        index16 = index20;
                        num26 = num32;
                      }
                      if ((double) num33 < (double) num27)
                      {
                        index17 = index20;
                        num27 = num33;
                      }
                    }
                    if ((double) num31 > 0.70710676908493042)
                    {
                      // ISSUE: reference to a compiler-generated field
                      float num34 = math.distance(d1, trafficSign.m_ObjectTransform.m_Position);
                      // ISSUE: reference to a compiler-generated field
                      float num35 = math.distance(d2, trafficSign.m_ObjectTransform.m_Position);
                      if ((double) num34 < (double) num28)
                      {
                        index18 = index20;
                        num28 = num34;
                      }
                      if ((double) num35 < (double) num29)
                      {
                        index19 = index20;
                        num29 = num35;
                      }
                    }
                  }
                  double num36 = (double) math.atan2(float2_1.x, float2_1.y);
                  float num37 = math.atan2(float2_2.x, float2_2.y);
                  int num38 = Mathf.FloorToInt((float) (num36 * 5.0929579734802246)) & 31;
                  int num39 = Mathf.FloorToInt(num37 * 5.092958f) + 16 & 31;
                  if (flag3 | flag4 && index16 != -1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index16];
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TargetLaneData targetLaneData;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (flag3 && updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_StartNode, out targetLaneData))
                    {
                      if ((carLane.m_Flags & CarLaneFlags.Stop) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Stop);
                      }
                      // ISSUE: reference to a compiler-generated field
                      if ((carLane.m_Flags & targetLaneData.m_AndCarLaneFlags & CarLaneFlags.Yield) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Yield);
                      }
                      if ((carLane.m_Flags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.GentleTurnLeft)) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_RemoveSignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoTurnLeft);
                      }
                      else if ((carLane.m_Flags & CarLaneFlags.LeftLimit) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        uint num40 = (uint) (2032 << num38) | math.select(2032U >> 32 - num38, 0U, num38 == 0);
                        if ((((int) num3 | (int) num4) & (int) num40) != 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoTurnLeft);
                        }
                      }
                      if ((carLane.m_Flags & CarLaneFlags.UTurnLeft) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_RemoveSignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoUTurnLeft);
                      }
                      else if ((carLane.m_Flags & CarLaneFlags.LeftLimit) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        uint num41 = (uint) (14 << num38) | math.select(14U >> 32 - num38, 0U, num38 == 0);
                        if ((((int) num3 | (int) num4) & (int) num41) != 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoUTurnLeft);
                        }
                      }
                      if ((carLane.m_Flags & (CarLaneFlags.TurnRight | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnRight)) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_RemoveSignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoTurnRight);
                      }
                      else if ((carLane.m_Flags & CarLaneFlags.RightLimit) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        uint num42 = (uint) (532676608 << num38) | math.select(532676608U >> 32 - num38, 0U, num38 == 0);
                        if ((((int) num3 | (int) num4) & (int) num42) != 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoTurnRight);
                        }
                      }
                      if ((carLane.m_Flags & CarLaneFlags.UTurnRight) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_RemoveSignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoUTurnRight);
                      }
                      else if ((carLane.m_Flags & CarLaneFlags.RightLimit) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        uint num43 = (uint) (-536870912 << num38) | math.select(3758096384U >> 32 - num38, 0U, num38 == 0);
                        if ((((int) num3 | (int) num4) & (int) num43) != 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.NoUTurnRight);
                        }
                      }
                    }
                    if (flag4)
                    {
                      // ISSUE: reference to a compiler-generated field
                      Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[subLane4];
                      // ISSUE: reference to a compiler-generated field
                      float y = math.dot(MathUtils.Right(float2_1), trafficSign.m_ObjectTransform.m_Position.xz - curve.m_Bezier.a.xz);
                      if ((double) y > 0.0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_VehicleLanesRight = math.max(trafficSign.m_TrafficSignNeeds.m_VehicleLanesRight, y);
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_VehicleLanesLeft = math.max(trafficSign.m_TrafficSignNeeds.m_VehicleLanesLeft, -y);
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      trafficSign.m_TrafficSignNeeds.m_VehicleMask |= laneSignal.m_GroupMask;
                    }
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_TrafficSigns[index16] = trafficSign;
                  }
                  if (flag3 && index17 != -1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index17];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (flag3 && updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_StartNode, out SecondaryObjectSystem.TargetLaneData _))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_LeftHandTraffic)
                      {
                        if ((carLane.m_Flags & CarLaneFlags.RightLimit) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                        {
                          uint num44 = (uint) (536870896 << num38) | math.select(536870896U >> 32 - num38, 0U, num38 == 0);
                          if (((int) num3 & (int) num44) != 0)
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.DoNotEnter);
                          }
                        }
                      }
                      else if ((carLane.m_Flags & CarLaneFlags.LeftLimit) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        uint num45 = (uint) (536870896 << num38) | math.select(536870896U >> 32 - num38, 0U, num38 == 0);
                        if (((int) num3 & (int) num45) != 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.DoNotEnter);
                        }
                      }
                    }
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_TrafficSigns[index17] = trafficSign;
                  }
                  if (index18 != -1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index18];
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TargetLaneData targetLaneData1;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_EndNode, out targetLaneData1))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_LeftHandTraffic)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if ((targetLaneData1.m_CarLaneFlags & (CarLaneFlags.Highway | CarLaneFlags.LeftLimit)) == CarLaneFlags.LeftLimit && (carLane.m_Flags & CarLaneFlags.TurnLeft) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Street);
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if ((targetLaneData1.m_CarLaneFlags & (CarLaneFlags.Highway | CarLaneFlags.RightLimit)) == CarLaneFlags.RightLimit && (carLane.m_Flags & CarLaneFlags.TurnRight) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Street);
                        }
                      }
                      uint num46 = (uint) (536870896 << num38) | math.select(536870896U >> 32 - num38, 0U, num38 == 0);
                      uint num47 = (uint) (536870896 << num39) | math.select(536870896U >> 32 - num39, 0U, num39 == 0);
                      if (((int) num3 & (int) num46) != 0 || ((int) num4 & (int) num47) != 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Oneway);
                      }
                      // ISSUE: reference to a compiler-generated field
                      if ((targetLaneData1.m_CarLaneFlags & ~carLane.m_Flags & CarLaneFlags.Highway) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Motorway);
                      }
                      // ISSUE: reference to a compiler-generated field
                      if ((double) math.abs(targetLaneData1.m_SpeedLimit.x - carLane.m_DefaultSpeedLimit) > 1.0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.SpeedLimit);
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        trafficSign.m_TrafficSignNeeds.m_SpeedLimit2 = (ushort) Mathf.RoundToInt(targetLaneData1.m_SpeedLimit.x * 3.6f);
                      }
                      // ISSUE: reference to a compiler-generated field
                      CarLaneFlags carLaneFlags = this.m_LeftHandTraffic ? CarLaneFlags.ParkingLeft : CarLaneFlags.ParkingRight;
                      // ISSUE: reference to a compiler-generated field
                      if ((targetLaneData1.m_CarLaneFlags & carLaneFlags) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: variable of a compiler-generated type
                        SecondaryObjectSystem.TargetLaneData targetLaneData2;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_StartNode, out targetLaneData2))
                        {
                          // ISSUE: reference to a compiler-generated field
                          if ((targetLaneData2.m_CarLaneFlags & carLaneFlags) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (carLane.m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Parking);
                          }
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Parking);
                        }
                      }
                    }
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_TrafficSigns[index18] = trafficSign;
                  }
                  if (index19 != -1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index19];
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TargetLaneData targetLaneData3;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_EndNode, out targetLaneData3))
                    {
                      // ISSUE: reference to a compiler-generated field
                      CarLaneFlags carLaneFlags = this.m_LeftHandTraffic ? CarLaneFlags.ParkingRight : CarLaneFlags.ParkingLeft;
                      // ISSUE: reference to a compiler-generated field
                      if ((targetLaneData3.m_CarLaneFlags & carLaneFlags) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: variable of a compiler-generated type
                        SecondaryObjectSystem.TargetLaneData targetLaneData4;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_StartNode, out targetLaneData4))
                        {
                          // ISSUE: reference to a compiler-generated field
                          if ((targetLaneData4.m_CarLaneFlags & carLaneFlags) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (carLane.m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Parking);
                          }
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.Parking);
                        }
                      }
                    }
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_TrafficSigns[index19] = trafficSign;
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PedestrianLaneData.HasComponent(subLane4) && (this.m_PedestrianLaneData[subLane4].m_Flags & PedestrianLaneFlags.Crosswalk) != (PedestrianLaneFlags) 0 && this.m_LaneSignalData.HasComponent(subLane4))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[subLane4];
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[subLane4];
                  int index21 = -1;
                  int index22 = -1;
                  float num48 = float.MaxValue;
                  float num49 = float.MaxValue;
                  float3 = MathUtils.StartTangent(curve.m_Bezier);
                  float2 float2_3 = math.normalizesafe(float3.xz);
                  float3 = MathUtils.EndTangent(curve.m_Bezier);
                  float2 float2_4 = math.normalizesafe(float3.xz);
                  // ISSUE: reference to a compiler-generated field
                  for (int index23 = 0; index23 < updateData.m_TrafficSigns.Length; ++index23)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index23];
                    // ISSUE: reference to a compiler-generated field
                    float num50 = 1f + math.distance(curve.m_Bezier.a, trafficSign.m_ObjectTransform.m_Position);
                    // ISSUE: reference to a compiler-generated field
                    float num51 = 1f + math.distance(curve.m_Bezier.d, trafficSign.m_ObjectTransform.m_Position);
                    // ISSUE: reference to a compiler-generated field
                    float num52 = num50 * (1f + math.abs(math.dot(float2_3, trafficSign.m_ForwardDirection)));
                    // ISSUE: reference to a compiler-generated field
                    float num53 = num51 * (1f + math.abs(math.dot(float2_4, trafficSign.m_ForwardDirection)));
                    if ((double) num52 < (double) num48)
                    {
                      index21 = index23;
                      num48 = num52;
                    }
                    if ((double) num53 < (double) num49)
                    {
                      index22 = index23;
                      num49 = num53;
                    }
                  }
                  if (index21 != -1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index21];
                    // ISSUE: reference to a compiler-generated field
                    if ((double) math.dot(MathUtils.Right(float2_3), trafficSign.m_ForwardDirection) > 0.0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      trafficSign.m_TrafficSignNeeds.m_CrossingLeftMask |= laneSignal.m_GroupMask;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      trafficSign.m_TrafficSignNeeds.m_CrossingRightMask |= laneSignal.m_GroupMask;
                    }
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_TrafficSigns[index21] = trafficSign;
                  }
                  if (index22 != -1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index22];
                    // ISSUE: reference to a compiler-generated field
                    if ((double) math.dot(MathUtils.Right(float2_4), trafficSign.m_ForwardDirection) > 0.0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      trafficSign.m_TrafficSignNeeds.m_CrossingRightMask |= laneSignal.m_GroupMask;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      trafficSign.m_TrafficSignNeeds.m_CrossingLeftMask |= laneSignal.m_GroupMask;
                    }
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_TrafficSigns[index22] = trafficSign;
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag5 = updateData.m_UtilityNodes.IsCreated && updateData.m_UtilityNodes.Length != 0;
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < updateData.m_TrafficSigns.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TrafficSignData trafficSign = updateData.m_TrafficSigns[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            trafficSign.m_TrafficSignNeeds.m_SignTypeMask &= ~trafficSign.m_TrafficSignNeeds.m_RemoveSignTypeMask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 &= ~trafficSign.m_TrafficSignNeeds.m_RemoveSignTypeMask2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (trafficSign.m_TrafficSignNeeds.m_SignTypeMask != 0U || trafficSign.m_TrafficSignNeeds.m_SignTypeMask2 != 0U || trafficSign.m_TrafficSignNeeds.m_VehicleMask != (ushort) 0 || trafficSign.m_TrafficSignNeeds.m_CrossingLeftMask != (ushort) 0 || trafficSign.m_TrafficSignNeeds.m_CrossingRightMask != (ushort) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateSecondaryObject(jobIndex, ref random, owner, isTemp, false, trafficSign.m_IsLowered, isNative, (Game.Tools.AgeMask) 0, ownerTemp, ownerElevation1, trafficSign.m_ParentTransform, trafficSign.m_ObjectTransform, trafficSign.m_LocalTransform, trafficSign.m_Flags, trafficSign.m_TrafficSignNeeds, ref updateData, trafficSign.m_Prefab, 0, trafficSign.m_Probability);
            }
          }
        }
        if (!flag5)
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < updateData.m_UtilityNodes.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          SecondaryObjectSystem.UtilityNodeData utilityNode = updateData.m_UtilityNodes[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!utilityNode.m_Unsure && (utilityNode.m_Count != 2 || utilityNode.m_Vertical))
          {
            Transform transform1 = new Transform();
            // ISSUE: reference to a compiler-generated field
            transform1.m_Position.y += utilityNode.m_Elevation;
            int jobIndex3 = jobIndex;
            ref Unity.Mathematics.Random local5 = ref random;
            Entity owner3 = owner;
            int num54 = isTemp ? 1 : 0;
            int num55 = isNative ? 1 : 0;
            Temp ownerTemp3 = ownerTemp;
            double ownerElevation4 = (double) ownerElevation1;
            // ISSUE: reference to a compiler-generated field
            Transform transform2 = utilityNode.m_Transform;
            // ISSUE: reference to a compiler-generated field
            Transform transform3 = utilityNode.m_Transform;
            Transform localTransformData = transform1;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds6 = new SecondaryObjectSystem.TrafficSignNeeds();
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds7 = trafficSignNeeds6;
            ref SecondaryObjectSystem.UpdateSecondaryObjectsData local6 = ref updateData;
            // ISSUE: reference to a compiler-generated field
            Entity prefab = utilityNode.m_Prefab;
            // ISSUE: reference to a compiler-generated method
            this.CreateSecondaryObject(jobIndex3, ref local5, owner3, num54 != 0, false, false, num55 != 0, (Game.Tools.AgeMask) 0, ownerTemp3, (float) ownerElevation4, transform2, transform3, localTransformData, SubObjectFlags.AnchorCenter, trafficSignNeeds7, ref local6, prefab, 0, 100);
          }
        }
      }

      private void AddNodeLanes(
        Entity node,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(node))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[node];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SecondaryLaneData.HasComponent(subLane2) && this.m_CarLaneData.HasComponent(subLane2) && !this.m_MasterLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.CarLane carLane = this.m_CarLaneData[subLane2];
            if ((carLane.m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
            {
              // ISSUE: reference to a compiler-generated field
              Lane lane = this.m_LaneData[subLane2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_SlaveLaneData.HasComponent(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                SlaveLane slaveLane = this.m_SlaveLaneData[subLane2];
                if ((int) slaveLane.m_MasterIndex < subLane1.Length)
                {
                  // ISSUE: reference to a compiler-generated field
                  lane = this.m_LaneData[subLane1[(int) slaveLane.m_MasterIndex].m_SubLane];
                }
              }
              // ISSUE: reference to a compiler-generated method
              updateData.EnsureSourceLanes(Allocator.Temp);
              // ISSUE: variable of a compiler-generated type
              SecondaryObjectSystem.TargetLaneData targetLaneData;
              // ISSUE: reference to a compiler-generated field
              if (updateData.m_SourceLanes.TryGetValue(lane.m_EndNode, out targetLaneData))
              {
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_CarLaneFlags |= carLane.m_Flags;
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_AndCarLaneFlags &= carLane.m_Flags;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_SpeedLimit.x = math.min(targetLaneData.m_SpeedLimit.x, carLane.m_DefaultSpeedLimit);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_SpeedLimit.y = math.max(targetLaneData.m_SpeedLimit.y, carLane.m_DefaultSpeedLimit);
                // ISSUE: reference to a compiler-generated field
                updateData.m_SourceLanes[lane.m_EndNode] = targetLaneData;
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                targetLaneData = new SecondaryObjectSystem.TargetLaneData()
                {
                  m_CarLaneFlags = carLane.m_Flags,
                  m_AndCarLaneFlags = carLane.m_Flags,
                  m_SpeedLimit = (float2) carLane.m_DefaultSpeedLimit
                };
                // ISSUE: reference to a compiler-generated field
                updateData.m_SourceLanes.Add(lane.m_EndNode, targetLaneData);
              }
            }
            else if ((carLane.m_Flags & CarLaneFlags.Forbidden) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
            {
              // ISSUE: reference to a compiler-generated field
              Lane lane = this.m_LaneData[subLane2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_SlaveLaneData.HasComponent(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                SlaveLane slaveLane = this.m_SlaveLaneData[subLane2];
                if ((int) slaveLane.m_MasterIndex < subLane1.Length)
                {
                  // ISSUE: reference to a compiler-generated field
                  lane = this.m_LaneData[subLane1[(int) slaveLane.m_MasterIndex].m_SubLane];
                }
              }
              // ISSUE: reference to a compiler-generated method
              updateData.EnsureTargetLanes(Allocator.Temp);
              // ISSUE: variable of a compiler-generated type
              SecondaryObjectSystem.TargetLaneData targetLaneData;
              // ISSUE: reference to a compiler-generated field
              if (updateData.m_TargetLanes.TryGetValue(lane.m_StartNode, out targetLaneData))
              {
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_CarLaneFlags |= carLane.m_Flags;
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_AndCarLaneFlags &= carLane.m_Flags;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_SpeedLimit.x = math.min(targetLaneData.m_SpeedLimit.x, carLane.m_DefaultSpeedLimit);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                targetLaneData.m_SpeedLimit.y = math.max(targetLaneData.m_SpeedLimit.y, carLane.m_DefaultSpeedLimit);
                // ISSUE: reference to a compiler-generated field
                updateData.m_TargetLanes[lane.m_StartNode] = targetLaneData;
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                targetLaneData = new SecondaryObjectSystem.TargetLaneData()
                {
                  m_CarLaneFlags = carLane.m_Flags,
                  m_AndCarLaneFlags = carLane.m_Flags,
                  m_SpeedLimit = (float2) carLane.m_DefaultSpeedLimit
                };
                // ISSUE: reference to a compiler-generated field
                updateData.m_TargetLanes.Add(lane.m_StartNode, targetLaneData);
              }
            }
          }
        }
      }

      private void AddEdgeLanes(
        Entity edge,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(edge))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[edge];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SecondaryLaneData.HasComponent(subLane2) && this.m_CarLaneData.HasComponent(subLane2) && !this.m_MasterLaneData.HasComponent(subLane2) && (this.m_CarLaneData[subLane2].m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
          {
            // ISSUE: reference to a compiler-generated field
            Lane lane = this.m_LaneData[subLane2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_SlaveLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              SlaveLane slaveLane = this.m_SlaveLaneData[subLane2];
              if ((int) slaveLane.m_MasterIndex < subLane1.Length)
              {
                // ISSUE: reference to a compiler-generated field
                lane = this.m_LaneData[subLane1[(int) slaveLane.m_MasterIndex].m_SubLane];
              }
            }
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TargetLaneData targetLaneData1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (updateData.m_SourceLanes.IsCreated && updateData.m_SourceLanes.TryGetValue(lane.m_StartNode, out targetLaneData1) && (targetLaneData1.m_CarLaneFlags & CarLaneFlags.Approach) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
            {
              // ISSUE: reference to a compiler-generated field
              targetLaneData1.m_CarLaneFlags |= CarLaneFlags.Approach;
              // ISSUE: reference to a compiler-generated field
              updateData.m_SourceLanes.TryAdd(lane.m_EndNode, targetLaneData1);
            }
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TargetLaneData targetLaneData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_EndNode, out targetLaneData2) && (targetLaneData2.m_CarLaneFlags & CarLaneFlags.Approach) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
            {
              // ISSUE: reference to a compiler-generated field
              targetLaneData2.m_CarLaneFlags |= CarLaneFlags.Approach;
              // ISSUE: reference to a compiler-generated field
              updateData.m_TargetLanes.TryAdd(lane.m_StartNode, targetLaneData2);
            }
          }
        }
      }

      private void CreateSecondaryEdgeObjects(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData,
        bool isTemp,
        bool isNative,
        Temp ownerTemp,
        out bool hasStreetLights,
        out bool alwaysLit)
      {
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_NetCompositionData[owner];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[owner];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[this.m_PrefabRefData[owner].m_Prefab];
        float ownerElevation1 = 0.0f;
        Game.Net.Elevation componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetElevationData.TryGetComponent(owner, out componentData1))
          ownerElevation1 = math.cmin(componentData1.m_Elevation);
        DynamicBuffer<SubReplacement> bufferData;
        // ISSUE: reference to a compiler-generated field
        this.m_SubReplacements.TryGetBuffer(owner, out bufferData);
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_PrefabNetCompositionData[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionObject> compositionObject1 = this.m_NetCompositionObjects[composition.m_Edge];
        bool flag1 = false;
        bool isLowered = ((netCompositionData1.m_Flags.m_Left | netCompositionData1.m_Flags.m_Right) & CompositionFlags.Side.Lowered) > (CompositionFlags.Side) 0;
        hasStreetLights = false;
        alwaysLit = (netCompositionData1.m_Flags.m_General & CompositionFlags.General.Tunnel) > (CompositionFlags.General) 0;
        for (int index1 = 0; index1 < compositionObject1.Length; ++index1)
        {
          NetCompositionObject compositionObject2 = compositionObject1[index1];
          float num1 = edgeGeometry.m_Start.middleLength + edgeGeometry.m_End.middleLength;
          float y = compositionObject2.m_Position.y;
          int num2 = 1;
          if ((compositionObject2.m_Flags & SubObjectFlags.EvenSpacing) != (SubObjectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData2 = this.m_PrefabNetCompositionData[composition.m_StartNode];
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData3 = this.m_PrefabNetCompositionData[composition.m_EndNode];
            if ((netCompositionData2.m_Flags.m_General & compositionObject2.m_SpacingIgnore) == (CompositionFlags.General) 0)
            {
              // ISSUE: reference to a compiler-generated field
              EdgeNodeGeometry geometry = this.m_StartNodeGeometryData[owner].m_Geometry;
              float num3 = math.min((float) (((double) geometry.m_Left.middleLength + (double) geometry.m_Right.middleLength) * 0.5), compositionObject2.m_Spacing * 0.333333343f);
              num1 += num3;
              y -= num3;
            }
            if ((netCompositionData3.m_Flags.m_General & compositionObject2.m_SpacingIgnore) == (CompositionFlags.General) 0)
            {
              // ISSUE: reference to a compiler-generated field
              EdgeNodeGeometry geometry = this.m_EndNodeGeometryData[owner].m_Geometry;
              float num4 = math.min((float) (((double) geometry.m_Left.middleLength + (double) geometry.m_Right.middleLength) * 0.5), compositionObject2.m_Spacing * 0.333333343f);
              num1 += num4;
            }
          }
          if ((double) num1 >= (double) compositionObject2.m_MinLength)
          {
            if ((double) compositionObject2.m_Spacing > 0.10000000149011612)
            {
              int a1 = Mathf.FloorToInt((float) ((double) num1 / (double) compositionObject2.m_Spacing + 0.5));
              num2 = (compositionObject2.m_Flags & SubObjectFlags.EvenSpacing) == (SubObjectFlags) 0 ? math.select(a1, 1, a1 == 0 & (double) num1 > (double) compositionObject2.m_Spacing * 0.10000000149011612) : a1 - 1;
              if ((double) compositionObject2.m_AvoidSpacing > 0.10000000149011612)
              {
                int a2 = Mathf.FloorToInt((float) ((double) num1 / (double) compositionObject2.m_AvoidSpacing + 0.5));
                int num5 = (compositionObject2.m_Flags & SubObjectFlags.EvenSpacing) == (SubObjectFlags) 0 ? math.select(a2, 1, a2 == 0 & (double) num1 > (double) compositionObject2.m_AvoidSpacing * 0.10000000149011612) : a2 - 1;
                if ((num2 & 1) == (num5 & 1))
                {
                  int2 int2 = num2 + new int2(-1, 1);
                  float2 float2 = math.abs((float2) int2 * compositionObject2.m_Spacing - num1);
                  num2 = math.select(int2.x, int2.y, (double) float2.y < (double) float2.x || int2.x == 0);
                }
              }
            }
            if (num2 > 0)
            {
              float t1 = (float) ((double) compositionObject2.m_Position.x / (double) math.max(1f, netCompositionData1.m_Width) + 0.5);
              Bezier4x3 curve1 = MathUtils.Lerp(edgeGeometry.m_Start.m_Left, edgeGeometry.m_Start.m_Right, t1);
              Bezier4x3 curve2 = MathUtils.Lerp(edgeGeometry.m_End.m_Left, edgeGeometry.m_End.m_Right, t1);
              float num6 = math.lerp(compositionObject2.m_CurveOffsetRange.x, compositionObject2.m_CurveOffsetRange.y, random.NextFloat(1f));
              float num7;
              if ((compositionObject2.m_Flags & SubObjectFlags.EvenSpacing) != (SubObjectFlags) 0)
              {
                num6 += 0.5f;
                num7 = num1 / (float) (num2 + 1);
              }
              else
                num7 = num1 / (float) num2;
              SubReplacementType subReplacementType = SubReplacementType.None;
              Game.Tools.AgeMask ageMask = (Game.Tools.AgeMask) 0;
              // ISSUE: reference to a compiler-generated field
              bool flag2 = this.m_PrefabTrafficSignData.HasComponent(compositionObject2.m_Prefab);
              // ISSUE: reference to a compiler-generated field
              bool flag3 = this.m_PrefabLaneDirectionData.HasComponent(compositionObject2.m_Prefab);
              if (!hasStreetLights)
              {
                // ISSUE: reference to a compiler-generated field
                hasStreetLights = this.m_PrefabStreetLightData.HasComponent(compositionObject2.m_Prefab);
              }
              PlaceableObjectData componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabPlaceableObjectData.TryGetComponent(compositionObject2.m_Prefab, out componentData2))
                subReplacementType = componentData2.m_SubReplacementType;
              if (subReplacementType != SubReplacementType.None && bufferData.IsCreated)
              {
                SubReplacementSide subReplacementSide = (compositionObject2.m_Flags & SubObjectFlags.OnMedian) == (SubObjectFlags) 0 ? ((double) compositionObject2.m_Position.x >= 0.0 ? SubReplacementSide.Right : SubReplacementSide.Left) : SubReplacementSide.Middle;
                for (int index2 = 0; index2 < bufferData.Length; ++index2)
                {
                  SubReplacement subReplacement = bufferData[index2];
                  if (subReplacement.m_Type == subReplacementType && subReplacement.m_Side == subReplacementSide)
                  {
                    compositionObject2.m_Prefab = subReplacement.m_Prefab;
                    ageMask = subReplacement.m_AgeMask;
                  }
                }
              }
              for (int index3 = 0; index3 < num2; ++index3)
              {
                float length = ((float) index3 + num6) * num7 + y;
                float3 position;
                float3 x;
                float3 float3_1;
                if ((double) length > (double) edgeGeometry.m_Start.middleLength)
                {
                  Bounds1 t2 = new Bounds1(0.0f, 1f);
                  MathUtils.ClampLength(MathUtils.Lerp(edgeGeometry.m_End.m_Left, edgeGeometry.m_End.m_Right, 0.5f).xz, ref t2, length - edgeGeometry.m_Start.middleLength);
                  position = MathUtils.Position(curve2, t2.max);
                  x = MathUtils.EndTangent(curve2);
                  float3_1 = MathUtils.Tangent(curve2, t2.max);
                }
                else
                {
                  Bounds1 t3 = new Bounds1(0.0f, 1f);
                  MathUtils.ClampLength(MathUtils.Lerp(edgeGeometry.m_Start.m_Left, edgeGeometry.m_Start.m_Right, 0.5f).xz, ref t3, length);
                  position = MathUtils.Position(curve1, t3.max);
                  x = MathUtils.StartTangent(curve1);
                  float3_1 = MathUtils.Tangent(curve1, t3.max);
                }
                x.y = math.lerp(0.0f, x.y, compositionObject2.m_UseCurveRotation.x);
                float3_1.y = math.lerp(0.0f, float3_1.y, compositionObject2.m_UseCurveRotation.x);
                float3 float3_2 = math.normalizesafe(x, new float3(0.0f, 0.0f, 1f));
                float3_1 = math.normalizesafe(float3_1, float3_2);
                quaternion rotation = math.slerp(quaternion.LookRotationSafe(float3_2, math.up()), quaternion.LookRotationSafe(float3_1, math.up()), compositionObject2.m_UseCurveRotation.y);
                Transform transform1 = new Transform(position, rotation);
                Transform transform2 = new Transform(compositionObject2.m_Offset, compositionObject2.m_Rotation);
                Transform world = ObjectUtils.LocalToWorld(transform1, transform2);
                if (compositionObject2.m_Probability < 100)
                  compositionObject2.m_Probability = math.clamp(Mathf.RoundToInt((float) compositionObject2.m_Probability * (num1 / netGeometryData.m_EdgeLengthRange.max)), 1, compositionObject2.m_Probability);
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds = new SecondaryObjectSystem.TrafficSignNeeds();
                // ISSUE: reference to a compiler-generated field
                trafficSignNeeds.m_Left = LaneDirectionType.None;
                // ISSUE: reference to a compiler-generated field
                trafficSignNeeds.m_Forward = LaneDirectionType.None;
                // ISSUE: reference to a compiler-generated field
                trafficSignNeeds.m_Right = LaneDirectionType.None;
                Entity result;
                bool hasTurningLanes;
                // ISSUE: reference to a compiler-generated method
                if (flag2 | flag3 && this.GetClosestCarLane(owner, world.m_Position, 1f, out result, out hasTurningLanes))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.CarLane carLane = this.m_CarLaneData[result];
                  // ISSUE: reference to a compiler-generated field
                  Lane lane = this.m_LaneData[result];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SlaveLaneData.HasComponent(result))
                  {
                    // ISSUE: reference to a compiler-generated field
                    SlaveLane slaveLane = this.m_SlaveLaneData[result];
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<Game.Net.SubLane> subLane = this.m_SubLanes[owner];
                    if ((int) slaveLane.m_MasterIndex < subLane.Length)
                    {
                      // ISSUE: reference to a compiler-generated field
                      lane = this.m_LaneData[subLane[(int) slaveLane.m_MasterIndex].m_SubLane];
                    }
                    if (flag3)
                    {
                      if ((slaveLane.m_Flags & SlaveLaneFlags.MergeLeft) != (SlaveLaneFlags) 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Left = LaneDirectionType.Merge;
                      }
                      if ((slaveLane.m_Flags & SlaveLaneFlags.MergeRight) != (SlaveLaneFlags) 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Right = LaneDirectionType.Merge;
                      }
                    }
                  }
                  if (!flag1)
                  {
                    flag1 = true;
                    // ISSUE: reference to a compiler-generated field
                    Game.Net.Edge edge = this.m_NetEdgeData[owner];
                    // ISSUE: reference to a compiler-generated method
                    this.AddNodeLanes(edge.m_Start, ref updateData);
                    // ISSUE: reference to a compiler-generated method
                    this.AddNodeLanes(edge.m_End, ref updateData);
                    // ISSUE: reference to a compiler-generated method
                    this.AddEdgeLanes(owner, ref updateData);
                  }
                  if (flag3)
                  {
                    bool flag4 = false;
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TargetLaneData targetLaneData;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (!hasTurningLanes && updateData.m_TargetLanes.IsCreated && updateData.m_TargetLanes.TryGetValue(lane.m_EndNode, out targetLaneData))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      flag4 = (targetLaneData.m_CarLaneFlags & CarLaneFlags.Forbidden) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (targetLaneData.m_CarLaneFlags & (CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight)) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                    }
                    if (hasTurningLanes | flag4)
                    {
                      if ((carLane.m_Flags & CarLaneFlags.UTurnLeft) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Left = LaneDirectionType.UTurn;
                      }
                      if ((carLane.m_Flags & CarLaneFlags.UTurnRight) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Right = LaneDirectionType.UTurn;
                      }
                      if ((carLane.m_Flags & CarLaneFlags.GentleTurnLeft) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Left = LaneDirectionType.Gentle;
                      }
                      if ((carLane.m_Flags & CarLaneFlags.GentleTurnRight) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Right = LaneDirectionType.Gentle;
                      }
                      if ((carLane.m_Flags & CarLaneFlags.TurnLeft) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Left = LaneDirectionType.Square;
                      }
                      if ((carLane.m_Flags & CarLaneFlags.TurnRight) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Right = LaneDirectionType.Square;
                      }
                      if ((carLane.m_Flags & CarLaneFlags.Forward) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_Forward = LaneDirectionType.Straight;
                      }
                    }
                  }
                  if (flag2)
                  {
                    // ISSUE: variable of a compiler-generated type
                    SecondaryObjectSystem.TargetLaneData targetLaneData;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (updateData.m_SourceLanes.IsCreated && updateData.m_SourceLanes.TryGetValue(lane.m_StartNode, out targetLaneData))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if ((carLane.m_Flags & ~targetLaneData.m_AndCarLaneFlags & CarLaneFlags.PublicOnly) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (targetLaneData.m_CarLaneFlags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.PublicOnly | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight)) != CarLaneFlags.PublicOnly)
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.BusOnly);
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.TaxiOnly);
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (math.any(math.abs(targetLaneData.m_SpeedLimit - carLane.m_DefaultSpeedLimit) > 1f))
                        {
                          // ISSUE: reference to a compiler-generated field
                          trafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.SpeedLimit);
                          // ISSUE: reference to a compiler-generated field
                          trafficSignNeeds.m_SpeedLimit = (ushort) Mathf.RoundToInt(carLane.m_DefaultSpeedLimit * 3.6f);
                        }
                      }
                    }
                    if ((carLane.m_Flags & CarLaneFlags.Roundabout) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_LeftHandTraffic)
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.RoundaboutClockwise);
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        trafficSignNeeds.m_SignTypeMask |= Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.RoundaboutCounterclockwise);
                      }
                    }
                  }
                }
                // ISSUE: reference to a compiler-generated method
                this.CreateSecondaryObject(jobIndex, ref random, owner, isTemp, false, isLowered, isNative, ageMask, ownerTemp, ownerElevation1, transform1, world, transform2, compositionObject2.m_Flags, trafficSignNeeds, ref updateData, compositionObject2.m_Prefab, 0, compositionObject2.m_Probability);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner];
        for (int index4 = 0; index4 < subLane1.Length; ++index4)
        {
          Entity subLane2 = subLane1[index4].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SecondaryLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_UtilityLaneData.HasComponent(subLane2) && !this.m_EdgeLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane2].m_Prefab];
              if (utilityLaneData.m_NodeObjectPrefab != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane2];
                // ISSUE: reference to a compiler-generated field
                Lane lane = this.m_LaneData[subLane2];
                bool flag5 = true;
                Temp componentData3;
                // ISSUE: reference to a compiler-generated field
                if (isTemp && this.m_TempData.TryGetComponent(subLane2, out componentData3))
                  flag5 = componentData3.m_Original == Entity.Null;
                // ISSUE: reference to a compiler-generated field
                float num8 = math.length(MathUtils.Size(this.m_PrefabObjectGeometryData[utilityLaneData.m_NodeObjectPrefab].m_Bounds));
                // ISSUE: reference to a compiler-generated method
                updateData.EnsureUtilityNodes(Allocator.Temp);
                bool flag6 = false;
                // ISSUE: reference to a compiler-generated field
                for (int index5 = 0; index5 < updateData.m_UtilityNodes.Length; ++index5)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  SecondaryObjectSystem.UtilityNodeData utilityNode = updateData.m_UtilityNodes[index5];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((utilityNode.m_UtilityTypes & utilityLaneData.m_UtilityTypes) != UtilityTypes.None && utilityNode.m_PathNode.Equals(lane.m_StartNode))
                  {
                    // ISSUE: reference to a compiler-generated field
                    if ((double) num8 > (double) utilityNode.m_LanePriority)
                    {
                      // ISSUE: reference to a compiler-generated field
                      utilityNode.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                      // ISSUE: reference to a compiler-generated field
                      utilityNode.m_LanePriority = num8;
                      // ISSUE: reference to a compiler-generated field
                      utilityNode.m_Count = 1;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      if ((double) num8 == (double) utilityNode.m_LanePriority)
                      {
                        // ISSUE: reference to a compiler-generated field
                        ++utilityNode.m_Count;
                      }
                    }
                    // ISSUE: reference to a compiler-generated field
                    utilityNode.m_IsNew &= flag5;
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_UtilityNodes[index5] = utilityNode;
                    flag6 = true;
                    break;
                  }
                }
                if (!flag6)
                {
                  float num9 = 0.0f;
                  Game.Net.Elevation componentData4;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_NetElevationData.TryGetComponent(subLane2, out componentData4) && (double) componentData4.m_Elevation.x != -3.4028234663852886E+38)
                    num9 = componentData4.m_Elevation.x;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  SecondaryObjectSystem.UtilityNodeData utilityNodeData = new SecondaryObjectSystem.UtilityNodeData()
                  {
                    m_Transform = new Transform(curve.m_Bezier.a, NetUtils.GetNodeRotation(MathUtils.StartTangent(curve.m_Bezier))),
                    m_Prefab = utilityLaneData.m_NodeObjectPrefab,
                    m_PathNode = lane.m_StartNode,
                    m_Count = 1,
                    m_Elevation = num9,
                    m_LanePriority = num8,
                    m_UtilityTypes = utilityLaneData.m_UtilityTypes,
                    m_Vertical = true,
                    m_IsNew = flag5
                  };
                  // ISSUE: reference to a compiler-generated field
                  updateData.m_UtilityNodes.Add(in utilityNodeData);
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TrackLaneData.HasComponent(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.TrackLane trackLane = this.m_TrackLaneData[subLane2];
                if ((trackLane.m_Flags & (TrackLaneFlags.StartingLane | TrackLaneFlags.EndingLane)) != (TrackLaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  TrackLaneData trackLaneData = this.m_PrefabTrackLaneData[this.m_PrefabRefData[subLane2].m_Prefab];
                  if (trackLaneData.m_EndObjectPrefab != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    Curve curve = this.m_CurveData[subLane2];
                    if ((trackLane.m_Flags & TrackLaneFlags.StartingLane) != (TrackLaneFlags) 0)
                    {
                      Transform transform;
                      transform.m_Position = curve.m_Bezier.a;
                      float3 forward = MathUtils.StartTangent(curve.m_Bezier);
                      transform.m_Rotation = !MathUtils.TryNormalize(ref forward) ? quaternion.identity : quaternion.LookRotation(forward, math.up());
                      transform.m_Position.y += netCompositionData1.m_SurfaceHeight.max;
                      int jobIndex1 = jobIndex;
                      ref Unity.Mathematics.Random local1 = ref random;
                      Entity owner1 = owner;
                      int num10 = isTemp ? 1 : 0;
                      int num11 = isLowered ? 1 : 0;
                      int num12 = isNative ? 1 : 0;
                      Temp ownerTemp1 = ownerTemp;
                      double ownerElevation2 = (double) ownerElevation1;
                      Transform ownerTransform = transform;
                      Transform transformData = transform;
                      Transform localTransformData = new Transform();
                      // ISSUE: object of a compiler-generated type is created
                      // ISSUE: variable of a compiler-generated type
                      SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds1 = new SecondaryObjectSystem.TrafficSignNeeds();
                      // ISSUE: variable of a compiler-generated type
                      SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds2 = trafficSignNeeds1;
                      ref SecondaryObjectSystem.UpdateSecondaryObjectsData local2 = ref updateData;
                      Entity endObjectPrefab = trackLaneData.m_EndObjectPrefab;
                      // ISSUE: reference to a compiler-generated method
                      this.CreateSecondaryObject(jobIndex1, ref local1, owner1, num10 != 0, false, num11 != 0, num12 != 0, (Game.Tools.AgeMask) 0, ownerTemp1, (float) ownerElevation2, ownerTransform, transformData, localTransformData, (SubObjectFlags) 0, trafficSignNeeds2, ref local2, endObjectPrefab, 0, 100);
                    }
                    if ((trackLane.m_Flags & TrackLaneFlags.EndingLane) != (TrackLaneFlags) 0)
                    {
                      Transform transform;
                      transform.m_Position = curve.m_Bezier.d;
                      float3 forward = -MathUtils.EndTangent(curve.m_Bezier);
                      transform.m_Rotation = !MathUtils.TryNormalize(ref forward) ? quaternion.identity : quaternion.LookRotation(forward, math.up());
                      transform.m_Position.y += netCompositionData1.m_SurfaceHeight.max;
                      int jobIndex2 = jobIndex;
                      ref Unity.Mathematics.Random local3 = ref random;
                      Entity owner2 = owner;
                      int num13 = isTemp ? 1 : 0;
                      int num14 = isLowered ? 1 : 0;
                      int num15 = isNative ? 1 : 0;
                      Temp ownerTemp2 = ownerTemp;
                      double ownerElevation3 = (double) ownerElevation1;
                      Transform ownerTransform = transform;
                      Transform transformData = transform;
                      Transform localTransformData = new Transform();
                      // ISSUE: object of a compiler-generated type is created
                      // ISSUE: variable of a compiler-generated type
                      SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds3 = new SecondaryObjectSystem.TrafficSignNeeds();
                      // ISSUE: variable of a compiler-generated type
                      SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds4 = trafficSignNeeds3;
                      ref SecondaryObjectSystem.UpdateSecondaryObjectsData local4 = ref updateData;
                      Entity endObjectPrefab = trackLaneData.m_EndObjectPrefab;
                      // ISSUE: reference to a compiler-generated method
                      this.CreateSecondaryObject(jobIndex2, ref local3, owner2, num13 != 0, false, num14 != 0, num15 != 0, (Game.Tools.AgeMask) 0, ownerTemp2, (float) ownerElevation3, ownerTransform, transformData, localTransformData, (SubObjectFlags) 0, trafficSignNeeds4, ref local4, endObjectPrefab, 0, 100);
                    }
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((!updateData.m_UtilityNodes.IsCreated ? 0 : (updateData.m_UtilityNodes.Length != 0 ? 1 : 0)) == 0)
          return;
        for (int index6 = 0; index6 < subLane1.Length; ++index6)
        {
          Entity subLane3 = subLane1[index6].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SecondaryLaneData.HasComponent(subLane3) && this.m_UtilityLaneData.HasComponent(subLane3) && this.m_EdgeLaneData.HasComponent(subLane3))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UtilityLaneData utilityLaneData = this.m_PrefabUtilityLaneData[this.m_PrefabRefData[subLane3].m_Prefab];
            if (utilityLaneData.m_NodeObjectPrefab != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              Lane lane = this.m_LaneData[subLane3];
              // ISSUE: reference to a compiler-generated field
              float num = math.length(MathUtils.Size(this.m_PrefabObjectGeometryData[utilityLaneData.m_NodeObjectPrefab].m_Bounds));
              // ISSUE: reference to a compiler-generated field
              for (int index7 = 0; index7 < updateData.m_UtilityNodes.Length; ++index7)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                SecondaryObjectSystem.UtilityNodeData utilityNode = updateData.m_UtilityNodes[index7];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((utilityNode.m_UtilityTypes & utilityLaneData.m_UtilityTypes) != UtilityTypes.None && utilityNode.m_PathNode.EqualsIgnoreCurvePos(lane.m_MiddleNode))
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((double) num > (double) utilityNode.m_LanePriority)
                  {
                    // ISSUE: reference to a compiler-generated field
                    utilityNode.m_LanePriority = num;
                    // ISSUE: reference to a compiler-generated field
                    utilityNode.m_Count = 2;
                    // ISSUE: reference to a compiler-generated field
                    utilityNode.m_Vertical = false;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if ((double) num == (double) utilityNode.m_LanePriority)
                    {
                      // ISSUE: reference to a compiler-generated field
                      utilityNode.m_Count += 2;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  utilityNode.m_Prefab = utilityLaneData.m_NodeObjectPrefab;
                  // ISSUE: reference to a compiler-generated field
                  updateData.m_UtilityNodes[index7] = utilityNode;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < updateData.m_UtilityNodes.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          SecondaryObjectSystem.UtilityNodeData utilityNode = updateData.m_UtilityNodes[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (utilityNode.m_Count != 2 || utilityNode.m_Vertical)
          {
            Transform transform3 = new Transform();
            // ISSUE: reference to a compiler-generated field
            transform3.m_Position.y += utilityNode.m_Elevation;
            int jobIndex3 = jobIndex;
            ref Unity.Mathematics.Random local5 = ref random;
            Entity owner3 = owner;
            int num16 = isTemp ? 1 : 0;
            // ISSUE: reference to a compiler-generated field
            int num17 = utilityNode.m_IsNew ? 1 : 0;
            int num18 = isNative ? 1 : 0;
            Temp ownerTemp3 = ownerTemp;
            double ownerElevation4 = (double) ownerElevation1;
            // ISSUE: reference to a compiler-generated field
            Transform transform4 = utilityNode.m_Transform;
            // ISSUE: reference to a compiler-generated field
            Transform transform5 = utilityNode.m_Transform;
            Transform localTransformData = transform3;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds5 = new SecondaryObjectSystem.TrafficSignNeeds();
            // ISSUE: variable of a compiler-generated type
            SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds6 = trafficSignNeeds5;
            ref SecondaryObjectSystem.UpdateSecondaryObjectsData local6 = ref updateData;
            // ISSUE: reference to a compiler-generated field
            Entity prefab = utilityNode.m_Prefab;
            // ISSUE: reference to a compiler-generated method
            this.CreateSecondaryObject(jobIndex3, ref local5, owner3, num16 != 0, num17 != 0, false, num18 != 0, (Game.Tools.AgeMask) 0, ownerTemp3, (float) ownerElevation4, transform4, transform5, localTransformData, SubObjectFlags.AnchorCenter, trafficSignNeeds6, ref local6, prefab, 0, 100);
          }
        }
      }

      private bool CheckRequirements(
        Entity owner,
        Entity prefab,
        bool isExplicit,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        DynamicBuffer<ObjectRequirementElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectRequirements.TryGetBuffer(prefab, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          this.EnsurePlaceholderRequirements(owner, ref updateData);
          int num = -1;
          bool flag = true;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            ObjectRequirementElement requirementElement = bufferData[index];
            if ((requirementElement.m_Type & ObjectRequirementType.SelectOnly) == (ObjectRequirementType) 0)
            {
              if ((int) requirementElement.m_Group != num)
              {
                if (flag)
                {
                  num = (int) requirementElement.m_Group;
                  flag = false;
                }
                else
                  break;
              }
              if (requirementElement.m_Requirement != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                flag = ((flag ? 1 : 0) | (updateData.m_PlaceholderRequirements.Contains(requirementElement.m_Requirement) ? 1 : (!isExplicit ? 0 : ((requirementElement.m_Type & ObjectRequirementType.IgnoreExplicit) != 0 ? 1 : 0)))) != 0;
              }
            }
          }
          if (!flag)
            return false;
        }
        return true;
      }

      private void CreateSecondaryObject(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        bool isTemp,
        bool isNew,
        bool isLowered,
        bool isNative,
        Game.Tools.AgeMask ageMask,
        Temp ownerTemp,
        float ownerElevation,
        Transform ownerTransform,
        Transform transformData,
        Transform localTransformData,
        SubObjectFlags flags,
        SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData,
        Entity prefab,
        int groupIndex,
        int probability)
      {
        PlaceholderObjectData componentData1;
        DynamicBuffer<PlaceholderObjectElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabPlaceholderObjectData.TryGetComponent(prefab, out componentData1) || !this.m_PlaceholderObjects.TryGetBuffer(prefab, out bufferData))
        {
          Entity groupPrefab = prefab;
          SpawnableObjectData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSpawnableObjectData.TryGetComponent(prefab, out componentData2) && componentData2.m_RandomizationGroup != Entity.Null)
            groupPrefab = componentData2.m_RandomizationGroup;
          // ISSUE: reference to a compiler-generated method
          if (!this.CheckRequirements(owner, prefab, true, ref updateData))
            return;
          Unity.Mathematics.Random random1 = random;
          random.NextInt();
          random.NextInt();
          Unity.Mathematics.Random random2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          if (updateData.m_SelectedSpawnabled.IsCreated && updateData.m_SelectedSpawnabled.TryGetValue(new SecondaryObjectSystem.PlaceholderKey(groupPrefab, groupIndex), out random2))
          {
            random1 = random2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            updateData.EnsureSelectedSpawnables(Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            updateData.m_SelectedSpawnabled.TryAdd(new SecondaryObjectSystem.PlaceholderKey(groupPrefab, groupIndex), random1);
          }
          if (random1.NextInt(100) >= probability)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CreateSecondaryObject(jobIndex, ref random1, owner, isTemp, isNew, isLowered, isNative, ageMask, ownerTemp, ownerElevation, Entity.Null, ownerTransform, transformData, localTransformData, flags, trafficSignNeeds, ref updateData, prefab, false, 0, groupIndex, probability);
        }
        else
        {
          if (componentData1.m_RandomizeGroupIndex)
            groupIndex = random.NextInt();
          float num1 = -1f;
          Entity prefab1 = Entity.Null;
          Entity groupPrefab1 = Entity.Null;
          SubObjectFlags flags1 = (SubObjectFlags) 0;
          Unity.Mathematics.Random random3 = new Unity.Mathematics.Random();
          bool flag1 = false;
          int max = 0;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity entity = bufferData[index].m_Object;
            // ISSUE: reference to a compiler-generated method
            if (this.CheckRequirements(owner, entity, false, ref updateData))
            {
              SubObjectFlags subObjectFlags = flags;
              float num2 = 0.0f;
              bool flag2 = false;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabTrafficSignData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Prefabs.TrafficSignData trafficSignData = this.m_PrefabTrafficSignData[entity];
                // ISSUE: reference to a compiler-generated field
                uint x = trafficSignData.m_TypeMask & trafficSignNeeds.m_SignTypeMask;
                // ISSUE: reference to a compiler-generated field
                int num3 = (int) trafficSignNeeds.m_SpeedLimit;
                if (x == 0U)
                {
                  // ISSUE: reference to a compiler-generated field
                  x = trafficSignData.m_TypeMask & trafficSignNeeds.m_SignTypeMask2;
                  // ISSUE: reference to a compiler-generated field
                  num3 = (int) trafficSignNeeds.m_SpeedLimit2;
                  if (x != 0U)
                    flag2 = true;
                  else
                    continue;
                }
                float num4 = 10f + math.log2((float) x);
                if (((int) x & (int) Game.Prefabs.TrafficSignData.GetTypeMask(TrafficSignType.SpeedLimit)) != 0)
                  num4 /= 1f + (float) math.abs(trafficSignData.m_SpeedLimit - num3);
                num2 += num4;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabTrafficLightData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                TrafficLightData trafficLightData = this.m_PrefabTrafficLightData[entity];
                int num5 = 0;
                // ISSUE: reference to a compiler-generated field
                if ((double) trafficSignNeeds.m_VehicleLanesLeft > 0.0)
                {
                  if ((trafficLightData.m_Type & TrafficLightType.VehicleLeft) != (TrafficLightType) 0)
                    num5 += 10;
                  else
                    continue;
                }
                else if ((trafficLightData.m_Type & TrafficLightType.VehicleLeft) != (TrafficLightType) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (trafficSignNeeds.m_VehicleMask != (ushort) 0)
                    --num5;
                  else
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                if ((double) trafficSignNeeds.m_VehicleLanesRight > 0.0)
                {
                  if ((trafficLightData.m_Type & TrafficLightType.VehicleRight) != (TrafficLightType) 0)
                    num5 += 10;
                  else
                    continue;
                }
                else if ((trafficLightData.m_Type & TrafficLightType.VehicleRight) != (TrafficLightType) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (trafficSignNeeds.m_VehicleMask != (ushort) 0)
                    --num5;
                  else
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (trafficSignNeeds.m_CrossingLeftMask != (ushort) 0 && trafficSignNeeds.m_CrossingRightMask != (ushort) 0)
                {
                  if ((trafficLightData.m_Type & (TrafficLightType.CrossingLeft | TrafficLightType.CrossingRight)) == (TrafficLightType.CrossingLeft | TrafficLightType.CrossingRight))
                    num5 += 10;
                  else
                    --num5;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (trafficSignNeeds.m_CrossingLeftMask != (ushort) 0)
                  {
                    if ((trafficLightData.m_Type & (TrafficLightType.CrossingLeft | TrafficLightType.CrossingRight)) == TrafficLightType.CrossingLeft)
                      num5 += 10;
                    else if ((trafficLightData.m_Type & (TrafficLightType.CrossingLeft | TrafficLightType.CrossingRight | TrafficLightType.AllowFlipped)) == (TrafficLightType.CrossingRight | TrafficLightType.AllowFlipped))
                    {
                      flag2 = true;
                      num5 += 9;
                    }
                    else if ((trafficLightData.m_Type & TrafficLightType.CrossingRight) == (TrafficLightType) 0)
                      --num5;
                    else
                      continue;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (trafficSignNeeds.m_CrossingRightMask != (ushort) 0)
                    {
                      if ((trafficLightData.m_Type & (TrafficLightType.CrossingLeft | TrafficLightType.CrossingRight)) == TrafficLightType.CrossingRight)
                        num5 += 10;
                      else if ((trafficLightData.m_Type & (TrafficLightType.CrossingLeft | TrafficLightType.CrossingRight | TrafficLightType.AllowFlipped)) == (TrafficLightType.CrossingLeft | TrafficLightType.AllowFlipped))
                      {
                        flag2 = true;
                        num5 += 9;
                      }
                      else if ((trafficLightData.m_Type & TrafficLightType.CrossingLeft) == (TrafficLightType) 0)
                        --num5;
                      else
                        continue;
                    }
                    else if ((trafficLightData.m_Type & (TrafficLightType.CrossingLeft | TrafficLightType.CrossingRight)) != (TrafficLightType) 0)
                      continue;
                  }
                }
                if (num5 > 0)
                {
                  num2 += (float) (50 * num5);
                  if ((trafficLightData.m_Type & TrafficLightType.VehicleLeft) != (TrafficLightType) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[entity];
                    Bounds1 bounds1_1 = trafficLightData.m_ReachOffset - objectGeometryData.m_Bounds.min.x;
                    // ISSUE: reference to a compiler-generated field
                    if ((double) bounds1_1.min <= (double) math.max(trafficSignNeeds.m_VehicleLanesLeft, 1f))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Bounds1 bounds1_2 = trafficSignNeeds.m_VehicleLanesLeft - bounds1_1;
                      num2 -= 50f * math.max(0.0f, math.max(-bounds1_2.min, bounds1_2.max));
                    }
                    else
                      continue;
                  }
                  if ((trafficLightData.m_Type & TrafficLightType.VehicleRight) != (TrafficLightType) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[entity];
                    Bounds1 bounds1_3 = trafficLightData.m_ReachOffset + objectGeometryData.m_Bounds.max.x;
                    // ISSUE: reference to a compiler-generated field
                    if ((double) bounds1_3.min <= (double) math.max(trafficSignNeeds.m_VehicleLanesRight, 1f))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Bounds1 bounds1_4 = trafficSignNeeds.m_VehicleLanesRight - bounds1_3;
                      num2 -= 50f * math.max(0.0f, math.max(-bounds1_4.min, bounds1_4.max));
                    }
                    else
                      continue;
                  }
                }
                else
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabLaneDirectionData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                LaneDirectionData laneDirectionData = this.m_PrefabLaneDirectionData[entity];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (trafficSignNeeds.m_Left != LaneDirectionType.None || trafficSignNeeds.m_Forward != LaneDirectionType.None || trafficSignNeeds.m_Right != LaneDirectionType.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int num6 = 0 + (180 - math.abs((int) (trafficSignNeeds.m_Left - laneDirectionData.m_Left))) + (180 - math.abs((int) (trafficSignNeeds.m_Forward - laneDirectionData.m_Forward))) + (180 - math.abs((int) (trafficSignNeeds.m_Right - laneDirectionData.m_Right)));
                  num2 += (float) num6 * 0.1f;
                }
                else
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              SpawnableObjectData spawnableObjectData = this.m_PrefabSpawnableObjectData[entity];
              Entity groupPrefab2 = spawnableObjectData.m_RandomizationGroup != Entity.Null ? spawnableObjectData.m_RandomizationGroup : entity;
              Unity.Mathematics.Random random4 = random;
              random.NextInt();
              random.NextInt();
              Unity.Mathematics.Random random5;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              if (updateData.m_SelectedSpawnabled.IsCreated && updateData.m_SelectedSpawnabled.TryGetValue(new SecondaryObjectSystem.PlaceholderKey(groupPrefab2, groupIndex), out random5))
              {
                num2 += 0.5f;
                random4 = random5;
              }
              if ((double) num2 > (double) num1)
              {
                num1 = num2;
                prefab1 = entity;
                groupPrefab1 = groupPrefab2;
                flags1 = subObjectFlags;
                random3 = random4;
                flag1 = flag2;
                // ISSUE: reference to a compiler-generated field
                max = this.m_PrefabSpawnableObjectData[entity].m_Probability;
              }
              else if ((double) num2 == (double) num1)
              {
                // ISSUE: reference to a compiler-generated field
                int probability1 = this.m_PrefabSpawnableObjectData[entity].m_Probability;
                max += probability1;
                if (random.NextInt(max) < probability1)
                {
                  prefab1 = entity;
                  groupPrefab1 = groupPrefab2;
                  flags1 = subObjectFlags;
                  random3 = random4;
                  flag1 = flag2;
                }
              }
            }
          }
          if (max <= 0)
            return;
          // ISSUE: reference to a compiler-generated method
          updateData.EnsureSelectedSpawnables(Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          updateData.m_SelectedSpawnabled.TryAdd(new SecondaryObjectSystem.PlaceholderKey(groupPrefab1, groupIndex), random3);
          if (random3.NextInt(100) >= probability)
            return;
          if (flag1)
            transformData.m_Rotation = math.mul(quaternion.RotateY(3.14159274f), transformData.m_Rotation);
          // ISSUE: reference to a compiler-generated method
          this.CreateSecondaryObject(jobIndex, ref random3, owner, isTemp, isNew, isLowered, isNative, ageMask, ownerTemp, ownerElevation, Entity.Null, ownerTransform, transformData, localTransformData, flags1, trafficSignNeeds, ref updateData, prefab1, false, 0, groupIndex, probability);
        }
      }

      private bool GetClosestCarLane(
        Entity owner,
        float3 position,
        float maxDistance,
        out Entity result,
        out bool hasTurningLanes)
      {
        result = Entity.Null;
        hasTurningLanes = false;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(owner))
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner];
        bool2 bool2 = (bool2) false;
        bool flag = false;
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.HasComponent(subLane2) && !this.m_MasterLaneData.HasComponent(subLane2) && !this.m_SecondaryLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.CarLane carLane = this.m_CarLaneData[subLane2];
            if ((carLane.m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
            {
              if ((carLane.m_Flags & CarLaneFlags.Invert) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                bool2.y |= (carLane.m_Flags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight)) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
              else
                bool2.x |= (carLane.m_Flags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight)) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
              // ISSUE: reference to a compiler-generated field
              float num = MathUtils.Distance(this.m_CurveData[subLane2].m_Bezier, position, out float _);
              if ((double) num < (double) maxDistance)
              {
                maxDistance = num;
                result = subLane2;
                flag = (carLane.m_Flags & CarLaneFlags.Invert) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
              }
            }
          }
        }
        hasTurningLanes = flag ? bool2.y : bool2.x;
        return result != Entity.Null;
      }

      private void CreateSecondaryObject(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        bool isTemp,
        bool isNew,
        bool isLowered,
        bool isNative,
        Game.Tools.AgeMask ageMask,
        Temp ownerTemp,
        float ownerElevation,
        Entity oldSecondaryObject,
        Transform ownerTransform,
        Transform transformData,
        Transform localTransformData,
        SubObjectFlags flags,
        SecondaryObjectSystem.TrafficSignNeeds trafficSignNeeds,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData,
        Entity prefab,
        bool cacheTransform,
        int parentMesh,
        int groupIndex,
        int probability)
      {
        // ISSUE: reference to a compiler-generated field
        bool flag1 = this.m_PrefabObjectGeometryData.HasComponent(prefab);
        PlaceableObjectData componentData1;
        // ISSUE: reference to a compiler-generated field
        int num1 = this.m_PrefabPlaceableObjectData.TryGetComponent(prefab, out componentData1) ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        bool flag2 = this.m_PrefabData.IsComponentEnabled(prefab);
        if ((flags & SubObjectFlags.AnchorTop) != (SubObjectFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
          objectGeometryData.m_Bounds.max.y -= componentData1.m_PlacementOffset.y;
          transformData.m_Position.y -= objectGeometryData.m_Bounds.max.y;
          localTransformData.m_Position.y -= objectGeometryData.m_Bounds.max.y;
        }
        else if ((flags & SubObjectFlags.AnchorCenter) != (SubObjectFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
          float num2 = (float) (((double) objectGeometryData.m_Bounds.max.y - (double) objectGeometryData.m_Bounds.min.y) * 0.5);
          transformData.m_Position.y -= num2;
          localTransformData.m_Position.y -= num2;
        }
        Elevation component1 = new Elevation(ownerElevation, math.abs(parentMesh) >= 1000 ? ElevationFlags.Stacked : (ElevationFlags) 0);
        if ((flags & SubObjectFlags.OnGround) == (SubObjectFlags) 0)
        {
          component1.m_Elevation += localTransformData.m_Position.y;
          if ((double) ownerElevation >= 0.0 && (double) component1.m_Elevation >= -0.5 && (double) component1.m_Elevation < 0.0)
            component1.m_Elevation = 0.0f;
          if (parentMesh < 0)
            component1.m_Flags |= ElevationFlags.OnGround;
          else if ((double) component1.m_Elevation < 0.0 & isLowered)
            component1.m_Flags |= ElevationFlags.Lowered;
        }
        else
        {
          if ((flags & (SubObjectFlags.AnchorTop | SubObjectFlags.AnchorCenter)) == (SubObjectFlags) 0)
          {
            transformData.m_Position.y = ownerTransform.m_Position.y - ownerElevation;
            localTransformData.m_Position.y = -ownerElevation;
          }
          component1.m_Elevation = 0.0f;
          component1.m_Flags |= ElevationFlags.OnGround;
        }
        if (oldSecondaryObject == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          oldSecondaryObject = this.FindOldSecondaryObject(prefab, transformData, ref updateData);
        }
        uint num3 = random.NextUInt(268435456U);
        PseudoRandomSeed component2 = new PseudoRandomSeed((ushort) (num3 >> 12));
        if (num1 != 0 && componentData1.m_RotationSymmetry != RotationSymmetry.None)
        {
          uint num4 = num3 & 4095U;
          if (componentData1.m_RotationSymmetry != RotationSymmetry.Any)
            num4 = (uint) ((int) num4 * (int) componentData1.m_RotationSymmetry & -4096) / (uint) componentData1.m_RotationSymmetry;
          float angle = (float) num4 * 0.00153398083f;
          transformData.m_Rotation = math.mul(quaternion.RotateY(angle), transformData.m_Rotation);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_UpdateQueue.Enqueue(new SecondaryObjectSystem.UpdateData()
        {
          m_Owner = owner,
          m_Prefab = prefab,
          m_Transform = transformData
        });
        if (oldSecondaryObject != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, oldSecondaryObject);
          Temp component3 = new Temp();
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(oldSecondaryObject))
            {
              // ISSUE: reference to a compiler-generated field
              component3 = this.m_TempData[oldSecondaryObject] with
              {
                m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
              };
              if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
                component3.m_Flags |= TempFlags.Modify;
              // ISSUE: reference to a compiler-generated method
              component3.m_Original = !isNew ? this.FindOriginalSecondaryObject(prefab, component3.m_Original, transformData, ref updateData) : Entity.Null;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Temp>(jobIndex, oldSecondaryObject, component3);
              Tree componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (component3.m_Original != Entity.Null && flag2 && this.m_PrefabTreeData.HasComponent(prefab) && this.m_TreeData.TryGetComponent(component3.m_Original, out componentData2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Tree>(jobIndex, oldSecondaryObject, componentData2);
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Transform>(jobIndex, oldSecondaryObject, transformData);
            // ISSUE: reference to a compiler-generated field
            if (!this.m_UpdatedData.HasComponent(oldSecondaryObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldSecondaryObject, new Updated());
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!transformData.Equals(this.m_TransformData[oldSecondaryObject]))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Transform>(jobIndex, oldSecondaryObject, transformData);
              // ISSUE: reference to a compiler-generated field
              if (!this.m_UpdatedData.HasComponent(oldSecondaryObject))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldSecondaryObject, new Updated());
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabStreetLightData.HasComponent(prefab))
          {
            StreetLight streetLight = new StreetLight();
            bool flag3 = false;
            StreetLight componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_StreetLightData.TryGetComponent(oldSecondaryObject, out componentData3))
            {
              streetLight = componentData3;
              flag3 = true;
            }
            Road componentData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoadData.TryGetComponent(owner, out componentData4))
            {
              // ISSUE: reference to a compiler-generated method
              StreetLightSystem.UpdateStreetLightState(ref streetLight, componentData4);
            }
            if (flag3)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<StreetLight>(jobIndex, oldSecondaryObject, streetLight);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<StreetLight>(jobIndex, oldSecondaryObject, streetLight);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTrafficLightData.HasComponent(prefab))
          {
            TrafficLight trafficLight = new TrafficLight();
            bool flag4 = false;
            TrafficLight componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrafficLightData.TryGetComponent(oldSecondaryObject, out componentData5))
            {
              trafficLight = componentData5;
              flag4 = true;
            }
            // ISSUE: reference to a compiler-generated field
            trafficLight.m_GroupMask0 = trafficSignNeeds.m_VehicleMask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            trafficLight.m_GroupMask1 = (ushort) ((uint) trafficSignNeeds.m_CrossingLeftMask | (uint) trafficSignNeeds.m_CrossingRightMask);
            TrafficLights componentData6;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrafficLightsData.TryGetComponent(owner, out componentData6))
            {
              // ISSUE: reference to a compiler-generated method
              TrafficLightSystem.UpdateTrafficLightState(componentData6, ref trafficLight);
            }
            if (flag4)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<TrafficLight>(jobIndex, oldSecondaryObject, trafficLight);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TrafficLight>(jobIndex, oldSecondaryObject, trafficLight);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (component3.m_Original == Entity.Null && this.m_PrefabTreeData.HasComponent(prefab))
          {
            Tree component4 = ObjectUtils.InitializeTreeState(ToolUtils.GetRandomAge(ref random, ageMask));
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Tree>(jobIndex, oldSecondaryObject, component4);
          }
          if (cacheTransform)
          {
            LocalTransformCache component5;
            component5.m_Position = localTransformData.m_Position;
            component5.m_Rotation = localTransformData.m_Rotation;
            component5.m_ParentMesh = parentMesh;
            component5.m_GroupIndex = groupIndex;
            component5.m_Probability = probability;
            component5.m_PrefabSubIndex = -1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.HasComponent(oldSecondaryObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<LocalTransformCache>(jobIndex, oldSecondaryObject, component5);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalTransformCache>(jobIndex, oldSecondaryObject, component5);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.HasComponent(oldSecondaryObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<LocalTransformCache>(jobIndex, oldSecondaryObject);
            }
          }
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PseudoRandomSeedData.HasComponent(component3.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              component2 = this.m_PseudoRandomSeedData[component3.m_Original];
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldSecondaryObject, component2);
          }
          if ((flags & SubObjectFlags.OnGround) == (SubObjectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.HasComponent(oldSecondaryObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Elevation>(jobIndex, oldSecondaryObject, component1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, oldSecondaryObject, component1);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_ElevationData.HasComponent(oldSecondaryObject))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Elevation>(jobIndex, oldSecondaryObject);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          ObjectData objectData = this.m_PrefabObjectData[prefab];
          if (!objectData.m_Archetype.Valid)
            return;
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, objectData.m_Archetype);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_SecondaryOwnerTypes);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity, new Owner(owner));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(prefab));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(jobIndex, entity, transformData);
          Temp component6 = new Temp();
          if (isTemp)
          {
            component6.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate);
            if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
              component6.m_Flags |= TempFlags.Modify;
            if (!isNew)
            {
              // ISSUE: reference to a compiler-generated method
              component6.m_Original = this.FindOriginalSecondaryObject(prefab, Entity.Null, transformData, ref updateData);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_TempAnimationTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity, component6);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity, component6);
            }
            Tree componentData7;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (component6.m_Original != Entity.Null && flag2 && this.m_PrefabTreeData.HasComponent(prefab) && this.m_TreeData.TryGetComponent(component6.m_Original, out componentData7))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Tree>(jobIndex, entity, componentData7);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabStreetLightData.HasComponent(prefab))
          {
            StreetLight streetLight = new StreetLight();
            StreetLight componentData8;
            // ISSUE: reference to a compiler-generated field
            if (this.m_StreetLightData.TryGetComponent(component6.m_Original, out componentData8))
              streetLight = componentData8;
            Road componentData9;
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoadData.TryGetComponent(owner, out componentData9))
            {
              // ISSUE: reference to a compiler-generated method
              StreetLightSystem.UpdateStreetLightState(ref streetLight, componentData9);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<StreetLight>(jobIndex, entity, streetLight);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTrafficLightData.HasComponent(prefab))
          {
            TrafficLight trafficLight = new TrafficLight();
            TrafficLight componentData10;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrafficLightData.TryGetComponent(component6.m_Original, out componentData10))
              trafficLight = componentData10;
            // ISSUE: reference to a compiler-generated field
            trafficLight.m_GroupMask0 = trafficSignNeeds.m_VehicleMask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            trafficLight.m_GroupMask1 = (ushort) ((uint) trafficSignNeeds.m_CrossingLeftMask | (uint) trafficSignNeeds.m_CrossingRightMask);
            TrafficLights componentData11;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrafficLightsData.TryGetComponent(owner, out componentData11))
            {
              // ISSUE: reference to a compiler-generated method
              TrafficLightSystem.UpdateTrafficLightState(componentData11, ref trafficLight);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<TrafficLight>(jobIndex, entity, trafficLight);
          }
          // ISSUE: reference to a compiler-generated field
          if (component6.m_Original == Entity.Null && this.m_PrefabTreeData.HasComponent(prefab))
          {
            Tree component7 = ObjectUtils.InitializeTreeState(ToolUtils.GetRandomAge(ref random, ageMask));
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Tree>(jobIndex, entity, component7);
          }
          if (isNative)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Native>(jobIndex, entity, new Native());
          }
          if (cacheTransform)
          {
            LocalTransformCache component8;
            component8.m_Position = localTransformData.m_Position;
            component8.m_Rotation = localTransformData.m_Rotation;
            component8.m_ParentMesh = parentMesh;
            component8.m_GroupIndex = groupIndex;
            component8.m_Probability = probability;
            component8.m_PrefabSubIndex = -1;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalTransformCache>(jobIndex, entity, component8);
          }
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PseudoRandomSeedData.HasComponent(component6.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              component2 = this.m_PseudoRandomSeedData[component6.m_Original];
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, component2);
          }
          if ((flags & SubObjectFlags.OnGround) != (SubObjectFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, entity, component1);
        }
      }

      private void EnsurePlaceholderRequirements(
        Entity owner,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_RequirementsSearched)
          return;
        // ISSUE: reference to a compiler-generated method
        updateData.EnsurePlaceholderRequirements(Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        if (this.m_DefaultTheme != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          updateData.m_PlaceholderRequirements.Add(this.m_DefaultTheme);
        }
        // ISSUE: reference to a compiler-generated field
        updateData.m_RequirementsSearched = true;
      }

      private Entity FindOldSecondaryObject(
        Entity prefab,
        Transform transform,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        Entity oldSecondaryObject = Entity.Null;
        Entity entity;
        NativeParallelMultiHashMapIterator<Entity> it1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_OldEntities.IsCreated && updateData.m_OldEntities.TryGetFirstValue(prefab, out entity, out it1))
        {
          oldSecondaryObject = entity;
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
          NativeParallelMultiHashMapIterator<Entity> it2 = it1;
          // ISSUE: reference to a compiler-generated field
          while (updateData.m_OldEntities.TryGetNextValue(out entity, ref it1))
          {
            // ISSUE: reference to a compiler-generated field
            float num2 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
            if ((double) num2 < (double) num1)
            {
              oldSecondaryObject = entity;
              num1 = num2;
              it2 = it1;
            }
          }
          // ISSUE: reference to a compiler-generated field
          updateData.m_OldEntities.Remove(it2);
        }
        return oldSecondaryObject;
      }

      private Entity FindOriginalSecondaryObject(
        Entity prefab,
        Entity original,
        Transform transform,
        ref SecondaryObjectSystem.UpdateSecondaryObjectsData updateData)
      {
        Entity originalSecondaryObject = Entity.Null;
        Entity entity;
        NativeParallelMultiHashMapIterator<Entity> it1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_OriginalEntities.IsCreated && updateData.m_OriginalEntities.TryGetFirstValue(prefab, out entity, out it1))
        {
          if (entity == original)
          {
            // ISSUE: reference to a compiler-generated field
            updateData.m_OriginalEntities.Remove(it1);
            return original;
          }
          originalSecondaryObject = entity;
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
          NativeParallelMultiHashMapIterator<Entity> it2 = it1;
          // ISSUE: reference to a compiler-generated field
          while (updateData.m_OriginalEntities.TryGetNextValue(out entity, ref it1))
          {
            if (entity == original)
            {
              // ISSUE: reference to a compiler-generated field
              updateData.m_OriginalEntities.Remove(it1);
              return original;
            }
            // ISSUE: reference to a compiler-generated field
            float num2 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
            if ((double) num2 < (double) num1)
            {
              originalSecondaryObject = entity;
              num1 = num2;
              it2 = it1;
            }
          }
          // ISSUE: reference to a compiler-generated field
          updateData.m_OriginalEntities.Remove(it2);
        }
        return originalSecondaryObject;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> __Game_Net_EdgeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> __Game_Prefabs_SubLane_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Secondary> __Game_Objects_Secondary_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficLightData> __Game_Prefabs_TrafficLightData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.TrafficSignData> __Game_Prefabs_TrafficSignData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.StreetLightData> __Game_Prefabs_StreetLightData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneDirectionData> __Game_Prefabs_LaneDirectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ThemeData> __Game_Prefabs_ThemeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderObjectData> __Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.UtilityObjectData> __Game_Prefabs_UtilityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficLight> __Game_Objects_TrafficLight_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StreetLight> __Game_Objects_StreetLight_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> __Game_Net_LaneSignal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficLights> __Game_Net_TrafficLights_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      public ComponentLookup<Road> __Game_Net_Road_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionObject> __Game_Prefabs_NetCompositionObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<DefaultNetLane> __Game_Prefabs_DefaultNetLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubReplacement> __Game_Net_SubReplacement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Secondary_RO_ComponentLookup = state.GetComponentLookup<Secondary>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrafficLightData_RO_ComponentLookup = state.GetComponentLookup<TrafficLightData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrafficSignData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.TrafficSignData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StreetLightData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.StreetLightData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup = state.GetComponentLookup<LaneDirectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ThemeData_RO_ComponentLookup = state.GetComponentLookup<ThemeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.UtilityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TrafficLight_RO_ComponentLookup = state.GetComponentLookup<TrafficLight>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_StreetLight_RO_ComponentLookup = state.GetComponentLookup<StreetLight>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneSignal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrafficLights_RO_ComponentLookup = state.GetComponentLookup<TrafficLights>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RW_ComponentLookup = state.GetComponentLookup<Road>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionObject_RO_BufferLookup = state.GetBufferLookup<NetCompositionObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DefaultNetLane_RO_BufferLookup = state.GetBufferLookup<DefaultNetLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubReplacement_RO_BufferLookup = state.GetBufferLookup<SubReplacement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
      }
    }
  }
}
