// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Rendering;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class NetInitializeSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_LaneQuery;
    private EntityQuery m_PlaceholderQuery;
    private NativeValue<PathfindHeuristicData> m_PathfindHeuristicData;
    private JobHandle m_PathfindHeuristicDeps;
    private Layer m_InGameLayersOnce;
    private Layer m_InGameLayersTwice;
    private NetInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadWrite<NetData>(),
          ComponentType.ReadWrite<NetSectionData>(),
          ComponentType.ReadWrite<NetPieceData>(),
          ComponentType.ReadWrite<NetLaneData>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadWrite<NetLaneData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<NetLaneData>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceholderQuery = this.GetEntityQuery(ComponentType.ReadOnly<NetLaneData>(), ComponentType.ReadOnly<PlaceholderObjectElement>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindHeuristicData = new NativeValue<PathfindHeuristicData>(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindHeuristicData.Dispose();
      base.OnDestroy();
    }

    public PathfindHeuristicData GetHeuristicData()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindHeuristicDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      return this.m_PathfindHeuristicData.value;
    }

    public bool CanReplace(NetData netData, bool inGame)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !inGame || (netData.m_RequiredLayers & this.m_InGameLayersOnce & ~this.m_InGameLayersTwice) == Layer.None;
    }

    private void AddSections(
      PrefabBase prefab,
      NetSectionInfo[] source,
      DynamicBuffer<NetGeometrySection> target,
      NetSectionFlags flags)
    {
      int2 int2 = new int2(int.MaxValue, int.MinValue);
      for (int index = 0; index < source.Length; ++index)
      {
        if (source[index].m_Median)
        {
          int y = index << 1;
          int2.x = math.min(int2.x, y);
          int2.y = math.max(int2.y, y);
        }
      }
      if (int2.Equals(new int2(int.MaxValue, int.MinValue)))
      {
        int2 = (int2) (source.Length - 1);
        flags |= NetSectionFlags.AlignCenter;
      }
      for (int index = 0; index < source.Length; ++index)
      {
        NetSectionInfo netSectionInfo = source[index];
        NetGeometrySection elem = new NetGeometrySection();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        elem.m_Section = this.m_PrefabSystem.GetEntity((PrefabBase) netSectionInfo.m_Section);
        elem.m_Offset = netSectionInfo.m_Offset;
        elem.m_Flags = flags;
        NetSectionFlags sectionFlags1;
        NetCompositionHelpers.GetRequirementFlags(netSectionInfo.m_RequireAll, out elem.m_CompositionAll, out sectionFlags1);
        NetSectionFlags sectionFlags2;
        NetCompositionHelpers.GetRequirementFlags(netSectionInfo.m_RequireAny, out elem.m_CompositionAny, out sectionFlags2);
        NetSectionFlags sectionFlags3;
        NetCompositionHelpers.GetRequirementFlags(netSectionInfo.m_RequireNone, out elem.m_CompositionNone, out sectionFlags3);
        NetSectionFlags p3 = sectionFlags1 | sectionFlags2 | sectionFlags3;
        if (p3 != (NetSectionFlags) 0)
          COSystemBase.baseLog.ErrorFormat((Object) prefab, "Net section ({0}: {1}) cannot require section flags: {2}", (object) prefab.name, (object) netSectionInfo.m_Section.name, (object) p3);
        if (netSectionInfo.m_Invert)
          elem.m_Flags |= NetSectionFlags.Invert;
        if (netSectionInfo.m_Flip)
          elem.m_Flags |= NetSectionFlags.FlipLanes | NetSectionFlags.FlipMesh;
        NetPieceLayerMask netPieceLayerMask = NetPieceLayerMask.Surface | NetPieceLayerMask.Bottom | NetPieceLayerMask.Top | NetPieceLayerMask.Side;
        if ((netSectionInfo.m_HiddenLayers & netPieceLayerMask) == netPieceLayerMask)
          elem.m_Flags |= NetSectionFlags.Hidden;
        if ((netSectionInfo.m_HiddenLayers & NetPieceLayerMask.Surface) != (NetPieceLayerMask) 0)
          elem.m_Flags |= NetSectionFlags.HiddenSurface;
        if ((netSectionInfo.m_HiddenLayers & NetPieceLayerMask.Bottom) != (NetPieceLayerMask) 0)
          elem.m_Flags |= NetSectionFlags.HiddenBottom;
        if ((netSectionInfo.m_HiddenLayers & NetPieceLayerMask.Top) != (NetPieceLayerMask) 0)
          elem.m_Flags |= NetSectionFlags.HiddenTop;
        if ((netSectionInfo.m_HiddenLayers & NetPieceLayerMask.Side) != (NetPieceLayerMask) 0)
          elem.m_Flags |= NetSectionFlags.HiddenSide;
        int num = index << 1;
        if (num >= int2.x && num <= int2.y)
          elem.m_Flags |= NetSectionFlags.Median;
        else if (num > int2.y)
          elem.m_Flags |= NetSectionFlags.Right;
        else
          elem.m_Flags |= NetSectionFlags.Left;
        target.Add(elem);
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      bool flag1 = false;
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_NetData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetPieceData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetPieceData> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_NetPieceData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetGeometryData> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PlaceableNetData> componentTypeHandle6 = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MarkerNetData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<MarkerNetData> componentTypeHandle7 = this.__TypeHandle.__Game_Prefabs_MarkerNetData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_LocalConnectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<LocalConnectData> componentTypeHandle8 = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetLaneData> componentTypeHandle9 = this.__TypeHandle.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetLaneGeometryData> componentTypeHandle10 = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CarLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<CarLaneData> componentTypeHandle11 = this.__TypeHandle.__Game_Prefabs_CarLaneData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TrackLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<TrackLaneData> componentTypeHandle12 = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<UtilityLaneData> componentTypeHandle13 = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<ParkingLaneData> componentTypeHandle14 = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PedestrianLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PedestrianLaneData> componentTypeHandle15 = this.__TypeHandle.__Game_Prefabs_PedestrianLaneData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SecondaryLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<SecondaryLaneData> componentTypeHandle16 = this.__TypeHandle.__Game_Prefabs_SecondaryLaneData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetCrosswalkData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetCrosswalkData> componentTypeHandle17 = this.__TypeHandle.__Game_Prefabs_NetCrosswalkData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RoadData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<RoadData> componentTypeHandle18 = this.__TypeHandle.__Game_Prefabs_RoadData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TrackData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<TrackData> componentTypeHandle19 = this.__TypeHandle.__Game_Prefabs_TrackData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WaterwayData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<WaterwayData> componentTypeHandle20 = this.__TypeHandle.__Game_Prefabs_WaterwayData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathwayData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PathwayData> componentTypeHandle21 = this.__TypeHandle.__Game_Prefabs_PathwayData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TaxiwayData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<TaxiwayData> componentTypeHandle22 = this.__TypeHandle.__Game_Prefabs_TaxiwayData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PowerLineData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PowerLineData> componentTypeHandle23 = this.__TypeHandle.__Game_Prefabs_PowerLineData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PipelineData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PipelineData> componentTypeHandle24 = this.__TypeHandle.__Game_Prefabs_PipelineData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_FenceData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<FenceData> componentTypeHandle25 = this.__TypeHandle.__Game_Prefabs_FenceData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_EditorContainerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<EditorContainerData> componentTypeHandle26 = this.__TypeHandle.__Game_Prefabs_EditorContainerData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<ElectricityConnectionData> componentTypeHandle27 = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<WaterPipeConnectionData> componentTypeHandle28 = this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BridgeData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<BridgeData> componentTypeHandle29 = this.__TypeHandle.__Game_Prefabs_BridgeData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<SpawnableObjectData> componentTypeHandle30 = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetTerrainData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetTerrainData> componentTypeHandle31 = this.__TypeHandle.__Game_Prefabs_NetTerrainData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UIObjectData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<UIObjectData> componentTypeHandle32 = this.__TypeHandle.__Game_Prefabs_UIObjectData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetSubSection_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetSubSection> bufferTypeHandle1 = this.__TypeHandle.__Game_Prefabs_NetSubSection_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetSectionPiece_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetSectionPiece> bufferTypeHandle2 = this.__TypeHandle.__Game_Prefabs_NetSectionPiece_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetPieceLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetPieceLane> bufferTypeHandle3 = this.__TypeHandle.__Game_Prefabs_NetPieceLane_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetPieceArea_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetPieceArea> bufferTypeHandle4 = this.__TypeHandle.__Game_Prefabs_NetPieceArea_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetPieceObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetPieceObject> bufferTypeHandle5 = this.__TypeHandle.__Game_Prefabs_NetPieceObject_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetGeometrySection> bufferTypeHandle6 = this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryEdgeState_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetGeometryEdgeState> bufferTypeHandle7 = this.__TypeHandle.__Game_Prefabs_NetGeometryEdgeState_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryNodeState_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<NetGeometryNodeState> bufferTypeHandle8 = this.__TypeHandle.__Game_Prefabs_NetGeometryNodeState_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubObject> bufferTypeHandle9 = this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubMesh> bufferTypeHandle10 = this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_FixedNetElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<FixedNetElement> bufferTypeHandle11 = this.__TypeHandle.__Game_Prefabs_FixedNetElement_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AuxiliaryNetLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<AuxiliaryNetLane> bufferTypeHandle12 = this.__TypeHandle.__Game_Prefabs_AuxiliaryNetLane_RW_BufferTypeHandle;
        this.CompleteDependency();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
          {
            flag1 = archetypeChunk.Has<SpawnableObjectData>(ref componentTypeHandle30);
          }
          else
          {
            NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
            bool flag2 = archetypeChunk.Has<MarkerNetData>(ref componentTypeHandle7);
            bool flag3 = archetypeChunk.Has<BridgeData>(ref componentTypeHandle29);
            bool flag4 = archetypeChunk.Has<UIObjectData>(ref componentTypeHandle32);
            NativeArray<NetData> nativeArray3 = archetypeChunk.GetNativeArray<NetData>(ref componentTypeHandle3);
            NativeArray<NetGeometryData> nativeArray4 = archetypeChunk.GetNativeArray<NetGeometryData>(ref componentTypeHandle5);
            NativeArray<PlaceableNetData> nativeArray5 = archetypeChunk.GetNativeArray<PlaceableNetData>(ref componentTypeHandle6);
            if (nativeArray4.Length != 0)
            {
              BufferAccessor<NetGeometrySection> bufferAccessor1 = archetypeChunk.GetBufferAccessor<NetGeometrySection>(ref bufferTypeHandle6);
              for (int index2 = 0; index2 < nativeArray4.Length; ++index2)
              {
                Entity entity = nativeArray1[index2];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetGeometryPrefab prefab = this.m_PrefabSystem.GetPrefab<NetGeometryPrefab>(nativeArray2[index2]);
                NetGeometryData netGeometryData = nativeArray4[index2];
                DynamicBuffer<NetGeometrySection> target = bufferAccessor1[index2];
                netGeometryData.m_EdgeLengthRange.max = 200f;
                netGeometryData.m_ElevatedLength = 80f;
                netGeometryData.m_MaxSlopeSteepness = math.select(prefab.m_MaxSlopeSteepness, 0.0f, (double) prefab.m_MaxSlopeSteepness < 1.0 / 1000.0);
                netGeometryData.m_ElevationLimit = 4f;
                if ((Object) prefab.m_AggregateType != (Object) null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  netGeometryData.m_AggregateType = this.m_PrefabSystem.GetEntity((PrefabBase) prefab.m_AggregateType);
                }
                if (flag2)
                  netGeometryData.m_Flags |= GeometryFlags.Marker;
                // ISSUE: reference to a compiler-generated method
                this.AddSections((PrefabBase) prefab, prefab.m_Sections, target, (NetSectionFlags) 0);
                UndergroundNetSections component1 = prefab.GetComponent<UndergroundNetSections>();
                if ((Object) component1 != (Object) null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddSections((PrefabBase) prefab, component1.m_Sections, target, NetSectionFlags.Underground);
                }
                OverheadNetSections component2 = prefab.GetComponent<OverheadNetSections>();
                if ((Object) component2 != (Object) null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddSections((PrefabBase) prefab, component2.m_Sections, target, NetSectionFlags.Overhead);
                }
                switch (prefab.m_InvertMode)
                {
                  case CompositionInvertMode.InvertLefthandTraffic:
                    netGeometryData.m_Flags |= GeometryFlags.InvertCompositionHandedness;
                    break;
                  case CompositionInvertMode.FlipLefthandTraffic:
                    netGeometryData.m_Flags |= GeometryFlags.FlipCompositionHandedness;
                    break;
                  case CompositionInvertMode.InvertRighthandTraffic:
                    netGeometryData.m_Flags |= GeometryFlags.IsLefthanded | GeometryFlags.InvertCompositionHandedness;
                    break;
                  case CompositionInvertMode.FlipRighthandTraffic:
                    netGeometryData.m_Flags |= GeometryFlags.IsLefthanded | GeometryFlags.FlipCompositionHandedness;
                    break;
                }
                nativeArray4[index2] = netGeometryData;
              }
              BufferAccessor<NetGeometryEdgeState> bufferAccessor2 = archetypeChunk.GetBufferAccessor<NetGeometryEdgeState>(ref bufferTypeHandle7);
              BufferAccessor<NetGeometryNodeState> bufferAccessor3 = archetypeChunk.GetBufferAccessor<NetGeometryNodeState>(ref bufferTypeHandle8);
              for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetGeometryPrefab prefab = this.m_PrefabSystem.GetPrefab<NetGeometryPrefab>(nativeArray2[index3]);
                DynamicBuffer<NetGeometryEdgeState> dynamicBuffer1 = bufferAccessor2[index3];
                DynamicBuffer<NetGeometryNodeState> dynamicBuffer2 = bufferAccessor3[index3];
                if (prefab.m_EdgeStates != null)
                {
                  for (int index4 = 0; index4 < prefab.m_EdgeStates.Length; ++index4)
                  {
                    NetEdgeStateInfo edgeState = prefab.m_EdgeStates[index4];
                    NetGeometryEdgeState elem = new NetGeometryEdgeState();
                    NetSectionFlags sectionFlags1;
                    NetCompositionHelpers.GetRequirementFlags(edgeState.m_RequireAll, out elem.m_CompositionAll, out sectionFlags1);
                    NetSectionFlags sectionFlags2;
                    NetCompositionHelpers.GetRequirementFlags(edgeState.m_RequireAny, out elem.m_CompositionAny, out sectionFlags2);
                    NetSectionFlags sectionFlags3;
                    NetCompositionHelpers.GetRequirementFlags(edgeState.m_RequireNone, out elem.m_CompositionNone, out sectionFlags3);
                    NetSectionFlags sectionFlags4;
                    NetCompositionHelpers.GetRequirementFlags(edgeState.m_SetState, out elem.m_State, out sectionFlags4);
                    NetSectionFlags p2 = sectionFlags1 | sectionFlags2 | sectionFlags3 | sectionFlags4;
                    if (p2 != (NetSectionFlags) 0)
                      COSystemBase.baseLog.ErrorFormat((Object) prefab, "Net edge state ({0}) cannot require/set section flags: {1}", (object) prefab.name, (object) p2);
                    dynamicBuffer1.Add(elem);
                  }
                }
                if (prefab.m_NodeStates != null)
                {
                  for (int index5 = 0; index5 < prefab.m_NodeStates.Length; ++index5)
                  {
                    NetNodeStateInfo nodeState = prefab.m_NodeStates[index5];
                    NetGeometryNodeState elem = new NetGeometryNodeState();
                    NetSectionFlags sectionFlags5;
                    NetCompositionHelpers.GetRequirementFlags(nodeState.m_RequireAll, out elem.m_CompositionAll, out sectionFlags5);
                    NetSectionFlags sectionFlags6;
                    NetCompositionHelpers.GetRequirementFlags(nodeState.m_RequireAny, out elem.m_CompositionAny, out sectionFlags6);
                    NetSectionFlags sectionFlags7;
                    NetCompositionHelpers.GetRequirementFlags(nodeState.m_RequireNone, out elem.m_CompositionNone, out sectionFlags7);
                    NetSectionFlags sectionFlags8;
                    NetCompositionHelpers.GetRequirementFlags(nodeState.m_SetState, out elem.m_State, out sectionFlags8);
                    NetSectionFlags p2 = sectionFlags5 | sectionFlags6 | sectionFlags7 | sectionFlags8;
                    if (p2 != (NetSectionFlags) 0)
                      COSystemBase.baseLog.ErrorFormat((Object) prefab, "Net node state ({0}) cannot require/set section flags: {1}", (object) prefab.name, (object) p2);
                    elem.m_MatchType = nodeState.m_MatchType;
                    dynamicBuffer2.Add(elem);
                  }
                }
              }
            }
            for (int index6 = 0; index6 < nativeArray5.Length; ++index6)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              NetPrefab prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(nativeArray2[index6]);
              PlaceableNetData placeableNetData = nativeArray5[index6] with
              {
                m_SnapDistance = 8f
              };
              PlaceableNet component3 = prefab.GetComponent<PlaceableNet>();
              if ((Object) component3 != (Object) null)
              {
                placeableNetData.m_ElevationRange = component3.m_ElevationRange;
                placeableNetData.m_XPReward = component3.m_XPReward;
                if ((Object) component3.m_UndergroundPrefab != (Object) null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  placeableNetData.m_UndergroundPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) component3.m_UndergroundPrefab);
                }
                if (component3.m_AllowParallelMode)
                  placeableNetData.m_PlacementFlags |= PlacementFlags.AllowParallel;
              }
              NetUpgrade component4 = prefab.GetComponent<NetUpgrade>();
              if ((Object) component4 != (Object) null)
              {
                NetSectionFlags sectionFlags9;
                NetCompositionHelpers.GetRequirementFlags(component4.m_SetState, out placeableNetData.m_SetUpgradeFlags, out sectionFlags9);
                NetSectionFlags sectionFlags10;
                NetCompositionHelpers.GetRequirementFlags(component4.m_UnsetState, out placeableNetData.m_UnsetUpgradeFlags, out sectionFlags10);
                placeableNetData.m_PlacementFlags |= PlacementFlags.IsUpgrade;
                if (!component4.m_Standalone)
                  placeableNetData.m_PlacementFlags |= PlacementFlags.UpgradeOnly;
                if (component4.m_Underground)
                  placeableNetData.m_PlacementFlags |= PlacementFlags.UndergroundUpgrade;
                if (((placeableNetData.m_SetUpgradeFlags | placeableNetData.m_UnsetUpgradeFlags) & CompositionFlags.nodeMask) != new CompositionFlags())
                  placeableNetData.m_PlacementFlags |= PlacementFlags.NodeUpgrade;
                NetSectionFlags p2 = sectionFlags9 | sectionFlags10;
                if (p2 != (NetSectionFlags) 0)
                  COSystemBase.baseLog.ErrorFormat((Object) prefab, "PlaceableNet ({0}) cannot upgrade section flags: {1}", (object) prefab.name, (object) p2);
              }
              nativeArray5[index6] = placeableNetData;
            }
            BufferAccessor<SubObject> bufferAccessor4 = archetypeChunk.GetBufferAccessor<SubObject>(ref bufferTypeHandle9);
            EntityManager entityManager;
            for (int index7 = 0; index7 < bufferAccessor4.Length; ++index7)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              NetSubObjects component = this.m_PrefabSystem.GetPrefab<NetPrefab>(nativeArray2[index7]).GetComponent<NetSubObjects>();
              bool flag5 = false;
              NetGeometryData netGeometryData = new NetGeometryData();
              if (nativeArray4.Length != 0)
                netGeometryData = nativeArray4[index7];
              DynamicBuffer<SubObject> dynamicBuffer = bufferAccessor4[index7];
              for (int index8 = 0; index8 < component.m_SubObjects.Length; ++index8)
              {
                NetSubObjectInfo subObject = component.m_SubObjects[index8];
                ObjectPrefab prefab = subObject.m_Object;
                SubObject elem = new SubObject();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                elem.m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) prefab);
                elem.m_Position = subObject.m_Position;
                elem.m_Rotation = subObject.m_Rotation;
                elem.m_Probability = 100;
                switch (subObject.m_Placement)
                {
                  case NetObjectPlacement.EdgeEndsOrNode:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.AllowCombine;
                    break;
                  case NetObjectPlacement.EdgeMiddle:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.MiddlePlacement;
                    entityManager = this.EntityManager;
                    if (entityManager.HasComponent<PillarData>(elem.m_Prefab))
                    {
                      netGeometryData.m_Flags |= GeometryFlags.MiddlePillars;
                      break;
                    }
                    break;
                  case NetObjectPlacement.EdgeEnds:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement;
                    break;
                  case NetObjectPlacement.CourseStart:
                    elem.m_Flags |= SubObjectFlags.CoursePlacement | SubObjectFlags.StartPlacement;
                    if (!flag5)
                    {
                      elem.m_Flags |= SubObjectFlags.MakeOwner;
                      netGeometryData.m_Flags |= GeometryFlags.SubOwner;
                      flag5 = true;
                      break;
                    }
                    break;
                  case NetObjectPlacement.CourseEnd:
                    elem.m_Flags |= SubObjectFlags.CoursePlacement | SubObjectFlags.EndPlacement;
                    if (!flag5)
                    {
                      elem.m_Flags |= SubObjectFlags.MakeOwner;
                      netGeometryData.m_Flags |= GeometryFlags.SubOwner;
                      flag5 = true;
                      break;
                    }
                    break;
                  case NetObjectPlacement.NodeBeforeFixedSegment:
                    elem.m_Flags |= SubObjectFlags.StartPlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.NodeBetweenFixedSegment:
                    elem.m_Flags |= SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.NodeAfterFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EndPlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.EdgeMiddleFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.MiddlePlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    entityManager = this.EntityManager;
                    if (entityManager.HasComponent<PillarData>(elem.m_Prefab))
                    {
                      netGeometryData.m_Flags |= GeometryFlags.MiddlePillars;
                      break;
                    }
                    break;
                  case NetObjectPlacement.EdgeEndsFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.EdgeStartFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.StartPlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.EdgeEndFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.EndPlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.EdgeEndsOrNodeFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.AllowCombine | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.EdgeStartOrNodeFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.AllowCombine | SubObjectFlags.StartPlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                  case NetObjectPlacement.EdgeEndOrNodeFixedSegment:
                    elem.m_Flags |= SubObjectFlags.EdgePlacement | SubObjectFlags.AllowCombine | SubObjectFlags.EndPlacement | SubObjectFlags.FixedPlacement;
                    elem.m_ParentIndex = subObject.m_FixedIndex;
                    break;
                }
                if (subObject.m_AnchorTop)
                  elem.m_Flags |= SubObjectFlags.AnchorTop;
                if (subObject.m_AnchorCenter)
                  elem.m_Flags |= SubObjectFlags.AnchorCenter;
                if (subObject.m_RequireElevated)
                  elem.m_Flags |= SubObjectFlags.RequireElevated;
                if (subObject.m_RequireOutsideConnection)
                  elem.m_Flags |= SubObjectFlags.RequireOutsideConnection;
                if (subObject.m_RequireDeadEnd)
                  elem.m_Flags |= SubObjectFlags.RequireDeadEnd;
                if (subObject.m_RequireOrphan)
                  elem.m_Flags |= SubObjectFlags.RequireOrphan;
                dynamicBuffer.Add(elem);
              }
              if (nativeArray4.Length != 0)
                nativeArray4[index7] = netGeometryData;
            }
            BufferAccessor<NetSectionPiece> bufferAccessor5 = archetypeChunk.GetBufferAccessor<NetSectionPiece>(ref bufferTypeHandle2);
            if (bufferAccessor5.Length != 0)
            {
              BufferAccessor<NetSubSection> bufferAccessor6 = archetypeChunk.GetBufferAccessor<NetSubSection>(ref bufferTypeHandle1);
              for (int index9 = 0; index9 < bufferAccessor5.Length; ++index9)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetSectionPrefab prefab = this.m_PrefabSystem.GetPrefab<NetSectionPrefab>(nativeArray2[index9]);
                DynamicBuffer<NetSubSection> dynamicBuffer3 = bufferAccessor6[index9];
                DynamicBuffer<NetSectionPiece> dynamicBuffer4 = bufferAccessor5[index9];
                if (prefab.m_SubSections != null)
                {
                  for (int index10 = 0; index10 < prefab.m_SubSections.Length; ++index10)
                  {
                    NetSubSectionInfo subSection = prefab.m_SubSections[index10];
                    NetSubSection elem = new NetSubSection();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    elem.m_SubSection = this.m_PrefabSystem.GetEntity((PrefabBase) subSection.m_Section);
                    NetCompositionHelpers.GetRequirementFlags(subSection.m_RequireAll, out elem.m_CompositionAll, out elem.m_SectionAll);
                    NetCompositionHelpers.GetRequirementFlags(subSection.m_RequireAny, out elem.m_CompositionAny, out elem.m_SectionAny);
                    NetCompositionHelpers.GetRequirementFlags(subSection.m_RequireNone, out elem.m_CompositionNone, out elem.m_SectionNone);
                    dynamicBuffer3.Add(elem);
                  }
                }
                if (prefab.m_Pieces != null)
                {
                  for (int index11 = 0; index11 < prefab.m_Pieces.Length; ++index11)
                  {
                    NetPieceInfo piece = prefab.m_Pieces[index11];
                    NetSectionPiece elem = new NetSectionPiece();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    elem.m_Piece = this.m_PrefabSystem.GetEntity((PrefabBase) piece.m_Piece);
                    NetCompositionHelpers.GetRequirementFlags(piece.m_RequireAll, out elem.m_CompositionAll, out elem.m_SectionAll);
                    NetCompositionHelpers.GetRequirementFlags(piece.m_RequireAny, out elem.m_CompositionAny, out elem.m_SectionAny);
                    NetCompositionHelpers.GetRequirementFlags(piece.m_RequireNone, out elem.m_CompositionNone, out elem.m_SectionNone);
                    switch (piece.m_Piece.m_Layer)
                    {
                      case NetPieceLayer.Surface:
                        elem.m_Flags |= NetPieceFlags.Surface;
                        break;
                      case NetPieceLayer.Bottom:
                        elem.m_Flags |= NetPieceFlags.Bottom;
                        break;
                      case NetPieceLayer.Top:
                        elem.m_Flags |= NetPieceFlags.Top;
                        break;
                      case NetPieceLayer.Side:
                        elem.m_Flags |= NetPieceFlags.Side;
                        break;
                    }
                    if (piece.m_Piece.meshCount != 0)
                      elem.m_Flags |= NetPieceFlags.HasMesh;
                    NetDividerPiece component5 = piece.m_Piece.GetComponent<NetDividerPiece>();
                    if ((Object) component5 != (Object) null)
                    {
                      if (component5.m_PreserveShape)
                        elem.m_Flags |= NetPieceFlags.PreserveShape | NetPieceFlags.DisableTiling;
                      if (component5.m_BlockTraffic)
                        elem.m_Flags |= NetPieceFlags.BlockTraffic;
                      if (component5.m_BlockCrosswalk)
                        elem.m_Flags |= NetPieceFlags.BlockCrosswalk;
                    }
                    NetPieceTiling component6 = piece.m_Piece.GetComponent<NetPieceTiling>();
                    if ((Object) component6 != (Object) null && component6.m_DisableTextureTiling)
                      elem.m_Flags |= NetPieceFlags.DisableTiling;
                    MovePieceVertices component7 = piece.m_Piece.GetComponent<MovePieceVertices>();
                    if ((Object) component7 != (Object) null)
                    {
                      if (component7.m_LowerBottomToTerrain)
                        elem.m_Flags |= NetPieceFlags.LowerBottomToTerrain;
                      if (component7.m_RaiseTopToTerrain)
                        elem.m_Flags |= NetPieceFlags.RaiseTopToTerrain;
                      if (component7.m_SmoothTopNormal)
                        elem.m_Flags |= NetPieceFlags.SmoothTopNormal;
                    }
                    AsymmetricPieceMesh component8 = piece.m_Piece.GetComponent<AsymmetricPieceMesh>();
                    if ((Object) component8 != (Object) null)
                    {
                      if (component8.m_Sideways)
                        elem.m_Flags |= NetPieceFlags.AsymmetricMeshX;
                      if (component8.m_Lengthwise)
                        elem.m_Flags |= NetPieceFlags.AsymmetricMeshZ;
                    }
                    elem.m_Offset = piece.m_Offset;
                    dynamicBuffer4.Add(elem);
                  }
                }
              }
            }
            NativeArray<NetPieceData> nativeArray6 = archetypeChunk.GetNativeArray<NetPieceData>(ref componentTypeHandle4);
            if (nativeArray6.Length != 0)
            {
              BufferAccessor<NetPieceLane> bufferAccessor7 = archetypeChunk.GetBufferAccessor<NetPieceLane>(ref bufferTypeHandle3);
              BufferAccessor<NetPieceArea> bufferAccessor8 = archetypeChunk.GetBufferAccessor<NetPieceArea>(ref bufferTypeHandle4);
              BufferAccessor<NetPieceObject> bufferAccessor9 = archetypeChunk.GetBufferAccessor<NetPieceObject>(ref bufferTypeHandle5);
              NativeArray<NetCrosswalkData> nativeArray7 = archetypeChunk.GetNativeArray<NetCrosswalkData>(ref componentTypeHandle17);
              NativeArray<NetTerrainData> nativeArray8 = archetypeChunk.GetNativeArray<NetTerrainData>(ref componentTypeHandle31);
              for (int index12 = 0; index12 < nativeArray6.Length; ++index12)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetPiecePrefab prefab = this.m_PrefabSystem.GetPrefab<NetPiecePrefab>(nativeArray2[index12]);
                NetPieceData netPieceData = nativeArray6[index12] with
                {
                  m_HeightRange = prefab.m_HeightRange,
                  m_SurfaceHeights = prefab.m_SurfaceHeights,
                  m_Width = prefab.m_Width,
                  m_Length = prefab.m_Length,
                  m_WidthOffset = prefab.m_WidthOffset,
                  m_NodeOffset = prefab.m_NodeOffset
                };
                if (bufferAccessor7.Length != 0)
                {
                  NetPieceLanes component = prefab.GetComponent<NetPieceLanes>();
                  if (component.m_Lanes != null)
                  {
                    DynamicBuffer<NetPieceLane> dynamicBuffer = bufferAccessor7[index12];
                    for (int index13 = 0; index13 < component.m_Lanes.Length; ++index13)
                    {
                      NetLaneInfo lane = component.m_Lanes[index13];
                      NetPieceLane elem = new NetPieceLane();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      elem.m_Lane = this.m_PrefabSystem.GetEntity((PrefabBase) lane.m_Lane);
                      elem.m_Position = lane.m_Position;
                      if (lane.m_FindAnchor)
                        elem.m_ExtraFlags |= LaneFlags.FindAnchor;
                      dynamicBuffer.Add(elem);
                    }
                    if (dynamicBuffer.Length > 1)
                      dynamicBuffer.AsNativeArray().Sort<NetPieceLane>();
                  }
                }
                if (bufferAccessor8.Length != 0)
                {
                  DynamicBuffer<NetPieceArea> dynamicBuffer = bufferAccessor8[index12];
                  BuildableNetPiece component = prefab.GetComponent<BuildableNetPiece>();
                  if ((Object) component != (Object) null)
                    dynamicBuffer.Add(new NetPieceArea()
                    {
                      m_Flags = component.m_AllowOnBridge ? NetAreaFlags.Buildable : NetAreaFlags.Buildable | NetAreaFlags.NoBridge,
                      m_Position = component.m_Position,
                      m_Width = component.m_Width,
                      m_SnapPosition = component.m_SnapPosition,
                      m_SnapWidth = component.m_SnapWidth
                    });
                  if (dynamicBuffer.Length > 1)
                    dynamicBuffer.AsNativeArray().Sort<NetPieceArea>();
                }
                if (bufferAccessor9.Length != 0)
                {
                  DynamicBuffer<NetPieceObject> dynamicBuffer = bufferAccessor9[index12];
                  NetPieceObjects component = prefab.GetComponent<NetPieceObjects>();
                  if ((Object) component != (Object) null)
                  {
                    dynamicBuffer.ResizeUninitialized(component.m_PieceObjects.Length);
                    for (int index14 = 0; index14 < component.m_PieceObjects.Length; ++index14)
                    {
                      NetPieceObjectInfo pieceObject = component.m_PieceObjects[index14];
                      NetPieceObject netPieceObject = new NetPieceObject();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      netPieceObject.m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) pieceObject.m_Object);
                      netPieceObject.m_Position = pieceObject.m_Position;
                      netPieceObject.m_Offset = pieceObject.m_Offset;
                      netPieceObject.m_Spacing = pieceObject.m_Spacing;
                      netPieceObject.m_UseCurveRotation = pieceObject.m_UseCurveRotation;
                      netPieceObject.m_MinLength = pieceObject.m_MinLength;
                      netPieceObject.m_Probability = math.select(pieceObject.m_Probability, 100, pieceObject.m_Probability == 0);
                      netPieceObject.m_CurveOffsetRange = pieceObject.m_CurveOffsetRange;
                      netPieceObject.m_Rotation = pieceObject.m_Rotation;
                      NetCompositionHelpers.GetRequirementFlags(pieceObject.m_RequireAll, out netPieceObject.m_CompositionAll, out netPieceObject.m_SectionAll);
                      NetCompositionHelpers.GetRequirementFlags(pieceObject.m_RequireAny, out netPieceObject.m_CompositionAny, out netPieceObject.m_SectionAny);
                      NetCompositionHelpers.GetRequirementFlags(pieceObject.m_RequireNone, out netPieceObject.m_CompositionNone, out netPieceObject.m_SectionNone);
                      if (pieceObject.m_FlipWhenInverted)
                        netPieceObject.m_Flags |= SubObjectFlags.FlipInverted;
                      if (pieceObject.m_EvenSpacing)
                        netPieceObject.m_Flags |= SubObjectFlags.EvenSpacing;
                      if (pieceObject.m_SpacingOverride)
                        netPieceObject.m_Flags |= SubObjectFlags.SpacingOverride;
                      dynamicBuffer[index14] = netPieceObject;
                    }
                  }
                }
                if (nativeArray7.Length != 0)
                {
                  NetPieceCrosswalk component = prefab.GetComponent<NetPieceCrosswalk>();
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  nativeArray7[index12] = new NetCrosswalkData()
                  {
                    m_Lane = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_Lane),
                    m_Start = component.m_Start,
                    m_End = component.m_End
                  };
                }
                if (nativeArray8.Length != 0)
                {
                  NetTerrainPiece component = prefab.GetComponent<NetTerrainPiece>();
                  nativeArray8[index12] = new NetTerrainData()
                  {
                    m_WidthOffset = component.m_WidthOffset,
                    m_ClipHeightOffset = component.m_ClipHeightOffset,
                    m_MinHeightOffset = component.m_MinHeightOffset,
                    m_MaxHeightOffset = component.m_MaxHeightOffset
                  };
                }
                nativeArray6[index12] = netPieceData;
              }
            }
            NativeArray<NetLaneData> nativeArray9 = archetypeChunk.GetNativeArray<NetLaneData>(ref componentTypeHandle9);
            if (nativeArray9.Length != 0)
            {
              NativeArray<ParkingLaneData> nativeArray10 = archetypeChunk.GetNativeArray<ParkingLaneData>(ref componentTypeHandle14);
              NativeArray<CarLaneData> nativeArray11 = archetypeChunk.GetNativeArray<CarLaneData>(ref componentTypeHandle11);
              NativeArray<TrackLaneData> nativeArray12 = archetypeChunk.GetNativeArray<TrackLaneData>(ref componentTypeHandle12);
              NativeArray<UtilityLaneData> nativeArray13 = archetypeChunk.GetNativeArray<UtilityLaneData>(ref componentTypeHandle13);
              NativeArray<SecondaryLaneData> nativeArray14 = archetypeChunk.GetNativeArray<SecondaryLaneData>(ref componentTypeHandle16);
              BufferAccessor<AuxiliaryNetLane> bufferAccessor10 = archetypeChunk.GetBufferAccessor<AuxiliaryNetLane>(ref bufferTypeHandle12);
              bool flag6 = archetypeChunk.Has<PedestrianLaneData>(ref componentTypeHandle15);
              for (int index15 = 0; index15 < nativeArray9.Length; ++index15)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetLanePrefab prefab = this.m_PrefabSystem.GetPrefab<NetLanePrefab>(nativeArray2[index15]);
                NetLaneData netLaneData = nativeArray9[index15];
                if ((Object) prefab.m_PathfindPrefab != (Object) null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  netLaneData.m_PathfindPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) prefab.m_PathfindPrefab);
                }
                if (nativeArray11.Length != 0)
                {
                  CarLane component = prefab.GetComponent<CarLane>();
                  netLaneData.m_Flags |= LaneFlags.Road;
                  netLaneData.m_Width = component.m_Width;
                  if (component.m_StartingLane)
                    netLaneData.m_Flags |= LaneFlags.DisconnectedStart;
                  if (component.m_EndingLane)
                    netLaneData.m_Flags |= LaneFlags.DisconnectedEnd;
                  if (component.m_Twoway)
                    netLaneData.m_Flags |= LaneFlags.Twoway;
                  if (component.m_BusLane)
                    netLaneData.m_Flags |= LaneFlags.PublicOnly;
                  if (component.m_RoadType == RoadTypes.Watercraft)
                    netLaneData.m_Flags |= LaneFlags.OnWater;
                  CarLaneData carLaneData = nativeArray11[index15];
                  if ((Object) component.m_NotTrackLane != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    carLaneData.m_NotTrackLanePrefab = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_NotTrackLane);
                  }
                  if ((Object) component.m_NotBusLane != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    carLaneData.m_NotBusLanePrefab = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_NotBusLane);
                  }
                  carLaneData.m_RoadTypes = component.m_RoadType;
                  carLaneData.m_MaxSize = component.m_MaxSize;
                  nativeArray11[index15] = carLaneData;
                }
                if (nativeArray12.Length != 0)
                {
                  TrackLane component = prefab.GetComponent<TrackLane>();
                  netLaneData.m_Flags |= LaneFlags.Track;
                  netLaneData.m_Width = component.m_Width;
                  if (component.m_Twoway)
                    netLaneData.m_Flags |= LaneFlags.Twoway;
                  TrackLaneData trackLaneData = nativeArray12[index15];
                  if ((Object) component.m_FallbackLane != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    trackLaneData.m_FallbackPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_FallbackLane);
                  }
                  if ((Object) component.m_EndObject != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    trackLaneData.m_EndObjectPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_EndObject);
                  }
                  trackLaneData.m_TrackTypes = component.m_TrackType;
                  trackLaneData.m_MaxCurviness = math.radians(component.m_MaxCurviness);
                  nativeArray12[index15] = trackLaneData;
                }
                if (nativeArray13.Length != 0)
                {
                  UtilityLane component = prefab.GetComponent<UtilityLane>();
                  netLaneData.m_Flags |= LaneFlags.Utility;
                  netLaneData.m_Width = component.m_Width;
                  if (component.m_Underground)
                    netLaneData.m_Flags |= LaneFlags.Underground;
                  UtilityLaneData utilityLaneData = nativeArray13[index15];
                  if ((Object) component.m_LocalConnectionLane != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    utilityLaneData.m_LocalConnectionPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_LocalConnectionLane);
                  }
                  if ((Object) component.m_LocalConnectionLane2 != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    utilityLaneData.m_LocalConnectionPrefab2 = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_LocalConnectionLane2);
                  }
                  if ((Object) component.m_NodeObject != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    utilityLaneData.m_NodeObjectPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_NodeObject);
                  }
                  utilityLaneData.m_VisualCapacity = component.m_VisualCapacity;
                  utilityLaneData.m_Hanging = component.m_Hanging;
                  utilityLaneData.m_UtilityTypes = component.m_UtilityType;
                  nativeArray13[index15] = utilityLaneData;
                }
                if (nativeArray10.Length != 0)
                {
                  ParkingLane component = prefab.GetComponent<ParkingLane>();
                  netLaneData.m_Flags |= LaneFlags.Parking;
                  ParkingLaneData parkingLaneData = nativeArray10[index15] with
                  {
                    m_RoadTypes = component.m_RoadType,
                    m_SlotSize = math.select(component.m_SlotSize, (float2) 0.0f, component.m_SlotSize < 1f / 1000f),
                    m_SlotAngle = math.radians(math.clamp(component.m_SlotAngle, 0.0f, 90f))
                  };
                  parkingLaneData.m_MaxCarLength = math.select(0.0f, parkingLaneData.m_SlotSize.y + 0.4f, (double) parkingLaneData.m_SlotSize.y != 0.0);
                  float2 y = new float2(math.cos(parkingLaneData.m_SlotAngle), math.sin(parkingLaneData.m_SlotAngle));
                  if ((double) y.y < 1.0 / 1000.0)
                    parkingLaneData.m_SlotInterval = parkingLaneData.m_SlotSize.y;
                  else if ((double) y.x < 1.0 / 1000.0)
                  {
                    parkingLaneData.m_SlotInterval = parkingLaneData.m_SlotSize.x;
                    netLaneData.m_Flags |= LaneFlags.Twoway;
                  }
                  else
                  {
                    float2 a = parkingLaneData.m_SlotSize / y.yx;
                    float2 float2 = math.select(a, (float2) 0.0f, a < 1f / 1000f);
                    if ((double) float2.x < (double) float2.y)
                    {
                      parkingLaneData.m_SlotInterval = float2.x;
                    }
                    else
                    {
                      parkingLaneData.m_SlotInterval = float2.y;
                      parkingLaneData.m_MaxCarLength = math.max(0.0f, parkingLaneData.m_SlotSize.y - 1f);
                    }
                  }
                  netLaneData.m_Width = math.dot(parkingLaneData.m_SlotSize, y);
                  netLaneData.m_Width = math.select(netLaneData.m_Width, parkingLaneData.m_SlotSize.y, (double) parkingLaneData.m_SlotSize.y != 0.0 && (double) parkingLaneData.m_SlotSize.y < (double) netLaneData.m_Width);
                  if ((double) parkingLaneData.m_SlotSize.x == 0.0)
                    netLaneData.m_Flags |= LaneFlags.Virtual;
                  if (component.m_SpecialVehicles)
                    netLaneData.m_Flags |= LaneFlags.PublicOnly;
                  nativeArray10[index15] = parkingLaneData;
                }
                if (flag6)
                {
                  PedestrianLane component = prefab.GetComponent<PedestrianLane>();
                  netLaneData.m_Flags |= LaneFlags.Pedestrian | LaneFlags.Twoway;
                  netLaneData.m_Width = component.m_Width;
                  if (component.m_OnWater)
                    netLaneData.m_Flags |= LaneFlags.OnWater;
                }
                if (nativeArray14.Length != 0)
                {
                  Entity entity1 = nativeArray1[index15];
                  SecondaryLane component = prefab.GetComponent<SecondaryLane>();
                  netLaneData.m_Flags |= LaneFlags.Secondary;
                  bool flag7 = component.m_LeftLanes != null && component.m_LeftLanes.Length != 0;
                  bool flag8 = component.m_RightLanes != null && component.m_RightLanes.Length != 0;
                  bool flag9 = component.m_CrossingLanes != null && component.m_CrossingLanes.Length != 0;
                  SecondaryLaneData secondaryLaneData = nativeArray14[index15];
                  if (component.m_SkipSafePedestrianOverlap)
                    secondaryLaneData.m_Flags |= SecondaryLaneDataFlags.SkipSafePedestrianOverlap;
                  if (component.m_SkipSafeCarOverlap)
                    secondaryLaneData.m_Flags |= SecondaryLaneDataFlags.SkipSafeCarOverlap;
                  if (component.m_SkipUnsafeCarOverlap)
                    secondaryLaneData.m_Flags |= SecondaryLaneDataFlags.SkipUnsafeCarOverlap;
                  if (component.m_SkipTrackOverlap)
                    secondaryLaneData.m_Flags |= SecondaryLaneDataFlags.SkipTrackOverlap;
                  if (component.m_SkipMergeOverlap)
                    secondaryLaneData.m_Flags |= SecondaryLaneDataFlags.SkipMergeOverlap;
                  if (component.m_FitToParkingSpaces)
                    secondaryLaneData.m_Flags |= SecondaryLaneDataFlags.FitToParkingSpaces;
                  if (component.m_EvenSpacing)
                    secondaryLaneData.m_Flags |= SecondaryLaneDataFlags.EvenSpacing;
                  secondaryLaneData.m_PositionOffset = component.m_PositionOffset;
                  secondaryLaneData.m_LengthOffset = component.m_LengthOffset;
                  secondaryLaneData.m_CutMargin = component.m_CutMargin;
                  secondaryLaneData.m_CutOffset = component.m_CutOffset;
                  secondaryLaneData.m_CutOverlap = component.m_CutOverlap;
                  secondaryLaneData.m_Spacing = component.m_Spacing;
                  SecondaryNetLaneFlags secondaryNetLaneFlags1 = (SecondaryNetLaneFlags) 0;
                  if (component.m_CanFlipSides)
                    secondaryNetLaneFlags1 |= SecondaryNetLaneFlags.CanFlipSides;
                  if (component.m_DuplicateSides)
                    secondaryNetLaneFlags1 |= SecondaryNetLaneFlags.DuplicateSides;
                  if (component.m_RequireParallel)
                    secondaryNetLaneFlags1 |= SecondaryNetLaneFlags.RequireParallel;
                  if (component.m_RequireOpposite)
                    secondaryNetLaneFlags1 |= SecondaryNetLaneFlags.RequireOpposite;
                  SecondaryNetLane secondaryNetLane1;
                  if (flag7)
                  {
                    SecondaryNetLaneFlags secondaryNetLaneFlags2 = secondaryNetLaneFlags1 | SecondaryNetLaneFlags.Left;
                    if (!flag8)
                      secondaryNetLaneFlags2 |= SecondaryNetLaneFlags.OneSided;
                    for (int index16 = 0; index16 < component.m_LeftLanes.Length; ++index16)
                    {
                      SecondaryLaneInfo leftLane = component.m_LeftLanes[index16];
                      SecondaryNetLaneFlags secondaryNetLaneFlags3 = secondaryNetLaneFlags2 | leftLane.GetFlags();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      Entity entity2 = this.m_PrefabSystem.GetEntity((PrefabBase) leftLane.m_Lane);
                      entityManager = this.EntityManager;
                      DynamicBuffer<SecondaryNetLane> buffer = entityManager.GetBuffer<SecondaryNetLane>(entity2);
                      ref DynamicBuffer<SecondaryNetLane> local = ref buffer;
                      secondaryNetLane1 = new SecondaryNetLane();
                      secondaryNetLane1.m_Lane = entity1;
                      secondaryNetLane1.m_Flags = secondaryNetLaneFlags3;
                      SecondaryNetLane elem = secondaryNetLane1;
                      local.Add(elem);
                    }
                  }
                  if (flag8)
                  {
                    SecondaryNetLaneFlags secondaryNetLaneFlags4 = secondaryNetLaneFlags1 | SecondaryNetLaneFlags.Right;
                    if (!flag7)
                      secondaryNetLaneFlags4 |= SecondaryNetLaneFlags.OneSided;
label_262:
                    for (int index17 = 0; index17 < component.m_RightLanes.Length; ++index17)
                    {
                      SecondaryLaneInfo rightLane = component.m_RightLanes[index17];
                      SecondaryNetLaneFlags secondaryNetLaneFlags5 = secondaryNetLaneFlags4 | rightLane.GetFlags();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      Entity entity3 = this.m_PrefabSystem.GetEntity((PrefabBase) rightLane.m_Lane);
                      entityManager = this.EntityManager;
                      DynamicBuffer<SecondaryNetLane> buffer = entityManager.GetBuffer<SecondaryNetLane>(entity3);
                      for (int index18 = 0; index18 < buffer.Length; ++index18)
                      {
                        SecondaryNetLane secondaryNetLane2 = buffer[index18];
                        if (secondaryNetLane2.m_Lane == entity1 && ((secondaryNetLane2.m_Flags ^ secondaryNetLaneFlags5) & ~(SecondaryNetLaneFlags.Left | SecondaryNetLaneFlags.Right)) == (SecondaryNetLaneFlags) 0)
                        {
                          secondaryNetLane2.m_Flags |= secondaryNetLaneFlags5;
                          buffer[index18] = secondaryNetLane2;
                          goto label_262;
                        }
                      }
                      ref DynamicBuffer<SecondaryNetLane> local = ref buffer;
                      secondaryNetLane1 = new SecondaryNetLane();
                      secondaryNetLane1.m_Lane = entity1;
                      secondaryNetLane1.m_Flags = secondaryNetLaneFlags5;
                      SecondaryNetLane elem = secondaryNetLane1;
                      local.Add(elem);
                    }
                  }
                  if (flag9)
                  {
                    SecondaryNetLaneFlags secondaryNetLaneFlags6 = SecondaryNetLaneFlags.Crossing;
                    for (int index19 = 0; index19 < component.m_CrossingLanes.Length; ++index19)
                    {
                      SecondaryLaneInfo2 crossingLane = component.m_CrossingLanes[index19];
                      SecondaryNetLaneFlags secondaryNetLaneFlags7 = secondaryNetLaneFlags6 | crossingLane.GetFlags();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      Entity entity4 = this.m_PrefabSystem.GetEntity((PrefabBase) crossingLane.m_Lane);
                      entityManager = this.EntityManager;
                      DynamicBuffer<SecondaryNetLane> buffer = entityManager.GetBuffer<SecondaryNetLane>(entity4);
                      ref DynamicBuffer<SecondaryNetLane> local = ref buffer;
                      secondaryNetLane1 = new SecondaryNetLane();
                      secondaryNetLane1.m_Lane = entity1;
                      secondaryNetLane1.m_Flags = secondaryNetLaneFlags7;
                      SecondaryNetLane elem = secondaryNetLane1;
                      local.Add(elem);
                    }
                  }
                  nativeArray14[index15] = secondaryLaneData;
                }
                if (bufferAccessor10.Length != 0)
                {
                  DynamicBuffer<AuxiliaryNetLane> dynamicBuffer = bufferAccessor10[index15];
                  AuxiliaryLanes component = prefab.GetComponent<AuxiliaryLanes>();
                  if ((Object) component != (Object) null)
                  {
                    dynamicBuffer.ResizeUninitialized(component.m_AuxiliaryLanes.Length);
                    for (int index20 = 0; index20 < component.m_AuxiliaryLanes.Length; ++index20)
                    {
                      AuxiliaryLaneInfo auxiliaryLane = component.m_AuxiliaryLanes[index20];
                      AuxiliaryNetLane auxiliaryNetLane = new AuxiliaryNetLane();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      auxiliaryNetLane.m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) auxiliaryLane.m_Lane);
                      auxiliaryNetLane.m_Position = auxiliaryLane.m_Position;
                      auxiliaryNetLane.m_Spacing = auxiliaryLane.m_Spacing;
                      if (auxiliaryLane.m_EvenSpacing)
                        auxiliaryNetLane.m_Flags |= LaneFlags.EvenSpacing;
                      if (auxiliaryLane.m_FindAnchor)
                        auxiliaryNetLane.m_Flags |= LaneFlags.FindAnchor;
                      NetSectionFlags sectionFlags11;
                      NetCompositionHelpers.GetRequirementFlags(auxiliaryLane.m_RequireAll, out auxiliaryNetLane.m_CompositionAll, out sectionFlags11);
                      NetSectionFlags sectionFlags12;
                      NetCompositionHelpers.GetRequirementFlags(auxiliaryLane.m_RequireAny, out auxiliaryNetLane.m_CompositionAny, out sectionFlags12);
                      NetSectionFlags sectionFlags13;
                      NetCompositionHelpers.GetRequirementFlags(auxiliaryLane.m_RequireNone, out auxiliaryNetLane.m_CompositionNone, out sectionFlags13);
                      NetSectionFlags p3 = sectionFlags11 | sectionFlags12 | sectionFlags13;
                      if (p3 != (NetSectionFlags) 0)
                        COSystemBase.baseLog.ErrorFormat((Object) prefab, "Auxiliary net lane ({0}: {1}) cannot require section flags: {2}", (object) prefab.name, (object) auxiliaryLane.m_Lane.name, (object) p3);
                      dynamicBuffer[index20] = auxiliaryNetLane;
                      netLaneData.m_Flags |= LaneFlags.HasAuxiliary;
                    }
                  }
                }
                nativeArray9[index15] = netLaneData;
              }
              NativeArray<NetLaneGeometryData> nativeArray15 = archetypeChunk.GetNativeArray<NetLaneGeometryData>(ref componentTypeHandle10);
              BufferAccessor<SubMesh> bufferAccessor11 = archetypeChunk.GetBufferAccessor<SubMesh>(ref bufferTypeHandle10);
              for (int index21 = 0; index21 < nativeArray15.Length; ++index21)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetLaneGeometryPrefab prefab = this.m_PrefabSystem.GetPrefab<NetLaneGeometryPrefab>(nativeArray2[index21]);
                NetLaneData netLaneData = nativeArray9[index21];
                NetLaneGeometryData laneGeometryData = nativeArray15[index21];
                DynamicBuffer<SubMesh> dynamicBuffer = bufferAccessor11[index21];
                laneGeometryData.m_MinLod = (int) byte.MaxValue;
                laneGeometryData.m_GameLayers = (MeshLayer) 0;
                laneGeometryData.m_EditorLayers = (MeshLayer) 0;
                if (prefab.m_Meshes != null)
                {
                  for (int randomSeed = 0; randomSeed < prefab.m_Meshes.Length; ++randomSeed)
                  {
                    NetLaneMeshInfo mesh1 = prefab.m_Meshes[randomSeed];
                    RenderPrefab mesh2 = mesh1.m_Mesh;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) mesh2);
                    entityManager = this.EntityManager;
                    MeshData componentData = entityManager.GetComponentData<MeshData>(entity);
                    float3 y = MathUtils.Size(mesh2.bounds);
                    componentData.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(y.xy), componentData.m_LodBias);
                    componentData.m_ShadowLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetShadowRenderingSize(y.xy), componentData.m_ShadowBias);
                    laneGeometryData.m_Size = math.max(laneGeometryData.m_Size, y);
                    laneGeometryData.m_MinLod = math.min(laneGeometryData.m_MinLod, (int) componentData.m_MinLod);
                    SubMeshFlags flags = (SubMeshFlags) 0;
                    if (mesh1.m_RequireSafe)
                      flags |= SubMeshFlags.RequireSafe;
                    if (mesh1.m_RequireLevelCrossing)
                      flags |= SubMeshFlags.RequireLevelCrossing;
                    if (mesh1.m_RequireEditor)
                      flags |= SubMeshFlags.RequireEditor;
                    if (mesh1.m_RequireTrackCrossing)
                      flags |= SubMeshFlags.RequireTrack;
                    if (mesh1.m_RequireClear)
                      flags |= SubMeshFlags.RequireClear;
                    if (mesh1.m_RequireLeftHandTraffic)
                      flags |= SubMeshFlags.RequireLeftHandTraffic;
                    if (mesh1.m_RequireRightHandTraffic)
                      flags |= SubMeshFlags.RequireRightHandTraffic;
                    dynamicBuffer.Add(new SubMesh(entity, flags, (ushort) randomSeed));
                    MeshLayer meshLayer = componentData.m_DefaultLayers == (MeshLayer) 0 ? MeshLayer.Default : componentData.m_DefaultLayers;
                    if (!mesh1.m_RequireEditor)
                      laneGeometryData.m_GameLayers |= meshLayer;
                    laneGeometryData.m_EditorLayers |= meshLayer;
                    entityManager = this.EntityManager;
                    entityManager.SetComponentData<MeshData>(entity, componentData);
                    if (mesh2.Has<ColorProperties>())
                      netLaneData.m_Flags |= LaneFlags.PseudoRandom;
                  }
                }
                nativeArray9[index21] = netLaneData;
                nativeArray15[index21] = laneGeometryData;
              }
              NativeArray<SpawnableObjectData> nativeArray16 = archetypeChunk.GetNativeArray<SpawnableObjectData>(ref componentTypeHandle30);
              if (nativeArray16.Length != 0)
              {
                for (int index22 = 0; index22 < nativeArray16.Length; ++index22)
                {
                  Entity entity5 = nativeArray1[index22];
                  SpawnableObjectData spawnableObjectData = nativeArray16[index22];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  SpawnableLane component = this.m_PrefabSystem.GetPrefab<NetLanePrefab>(nativeArray2[index22]).GetComponent<SpawnableLane>();
                  for (int index23 = 0; index23 < component.m_Placeholders.Length; ++index23)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    Entity entity6 = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_Placeholders[index23]);
                    entityManager = this.EntityManager;
                    entityManager.GetBuffer<PlaceholderObjectElement>(entity6).Add(new PlaceholderObjectElement(entity5));
                  }
                  if ((Object) component.m_RandomizationGroup != (Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    spawnableObjectData.m_RandomizationGroup = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_RandomizationGroup);
                  }
                  spawnableObjectData.m_Probability = component.m_Probability;
                  nativeArray16[index22] = spawnableObjectData;
                }
              }
            }
            NativeArray<RoadData> nativeArray17 = archetypeChunk.GetNativeArray<RoadData>(ref componentTypeHandle18);
            if (nativeArray17.Length != 0)
            {
              for (int index24 = 0; index24 < nativeArray17.Length; ++index24)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                RoadPrefab prefab = this.m_PrefabSystem.GetPrefab<RoadPrefab>(nativeArray2[index24]);
                NetData netData = nativeArray3[index24];
                NetGeometryData netGeometryData = nativeArray4[index24];
                RoadData roadData = nativeArray17[index24];
                switch (prefab.m_RoadType)
                {
                  case RoadType.Normal:
                    netData.m_RequiredLayers |= Layer.Road;
                    break;
                  case RoadType.PublicTransport:
                    netData.m_RequiredLayers |= Layer.PublicTransportRoad;
                    break;
                }
                netData.m_ConnectLayers |= Layer.Road | Layer.TrainTrack | Layer.Pathway | Layer.TramTrack | Layer.Fence | Layer.PublicTransportRoad;
                netData.m_LocalConnectLayers |= Layer.Pathway | Layer.MarkerPathway;
                netData.m_NodePriority += 2000f;
                netGeometryData.m_MergeLayers |= Layer.Road | Layer.TramTrack | Layer.PublicTransportRoad;
                netGeometryData.m_IntersectLayers |= Layer.Road | Layer.TrainTrack | Layer.Pathway | Layer.TramTrack | Layer.PublicTransportRoad;
                netGeometryData.m_Flags |= GeometryFlags.SupportRoundabout | GeometryFlags.BlockZone | GeometryFlags.Directional | GeometryFlags.FlattenTerrain | GeometryFlags.ClipTerrain;
                roadData.m_SpeedLimit = prefab.m_SpeedLimit / 3.6f;
                if ((Object) prefab.m_ZoneBlock != (Object) null)
                {
                  netGeometryData.m_Flags |= GeometryFlags.SnapCellSize;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  roadData.m_ZoneBlockPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) prefab.m_ZoneBlock);
                  roadData.m_Flags |= RoadFlags.EnableZoning;
                }
                if (prefab.m_TrafficLights)
                  roadData.m_Flags |= RoadFlags.PreferTrafficLights;
                if (prefab.m_HighwayRules)
                {
                  roadData.m_Flags |= RoadFlags.UseHighwayRules;
                  netGeometryData.m_MinNodeOffset = math.max(netGeometryData.m_MinNodeOffset, 2f);
                  netGeometryData.m_Flags |= GeometryFlags.SmoothElevation;
                }
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index24];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.OnGround;
                  nativeArray5[index24] = placeableNetData;
                }
                nativeArray3[index24] = netData;
                nativeArray4[index24] = netGeometryData;
                nativeArray17[index24] = roadData;
              }
            }
            NativeArray<TrackData> nativeArray18 = archetypeChunk.GetNativeArray<TrackData>(ref componentTypeHandle19);
            if (nativeArray18.Length != 0)
            {
              for (int index25 = 0; index25 < nativeArray18.Length; ++index25)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                TrackPrefab prefab = this.m_PrefabSystem.GetPrefab<TrackPrefab>(nativeArray2[index25]);
                NetData netData = nativeArray3[index25];
                NetGeometryData netGeometryData = nativeArray4[index25];
                TrackData trackData = nativeArray18[index25];
                Layer layer1;
                Layer layer2;
                float num;
                float y;
                switch (prefab.m_TrackType)
                {
                  case TrackTypes.Train:
                    layer1 = Layer.TrainTrack;
                    layer2 = Layer.TrainTrack | Layer.Pathway;
                    num = 200f;
                    y = 10f;
                    netGeometryData.m_Flags |= GeometryFlags.SmoothElevation;
                    break;
                  case TrackTypes.Tram:
                    layer1 = Layer.TramTrack;
                    layer2 = Layer.TramTrack;
                    num = 0.0f;
                    y = 8f;
                    netGeometryData.m_Flags |= GeometryFlags.SupportRoundabout;
                    break;
                  case TrackTypes.Subway:
                    layer1 = Layer.SubwayTrack;
                    layer2 = Layer.SubwayTrack;
                    num = 200f;
                    y = 9f;
                    netGeometryData.m_Flags |= GeometryFlags.SmoothElevation;
                    break;
                  default:
                    layer1 = Layer.None;
                    layer2 = Layer.None;
                    num = 0.0f;
                    y = 0.0f;
                    break;
                }
                netData.m_RequiredLayers |= layer1;
                netData.m_ConnectLayers |= layer2;
                netData.m_LocalConnectLayers |= Layer.Pathway | Layer.MarkerPathway;
                netGeometryData.m_MergeLayers |= layer1;
                netGeometryData.m_IntersectLayers |= layer2;
                netGeometryData.m_EdgeLengthRange.max = math.max(netGeometryData.m_EdgeLengthRange.max, num * 1.5f);
                netGeometryData.m_MinNodeOffset = math.max(netGeometryData.m_MinNodeOffset, y);
                netGeometryData.m_Flags |= GeometryFlags.BlockZone | GeometryFlags.Directional | GeometryFlags.FlattenTerrain | GeometryFlags.ClipTerrain;
                trackData.m_TrackType = prefab.m_TrackType;
                trackData.m_SpeedLimit = prefab.m_SpeedLimit / 3.6f;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index25];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.OnGround;
                  nativeArray5[index25] = placeableNetData;
                }
                nativeArray3[index25] = netData;
                nativeArray4[index25] = netGeometryData;
                nativeArray18[index25] = trackData;
              }
            }
            NativeArray<WaterwayData> nativeArray19 = archetypeChunk.GetNativeArray<WaterwayData>(ref componentTypeHandle20);
            if (nativeArray19.Length != 0)
            {
              for (int index26 = 0; index26 < nativeArray19.Length; ++index26)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                WaterwayPrefab prefab = this.m_PrefabSystem.GetPrefab<WaterwayPrefab>(nativeArray2[index26]);
                NetData netData = nativeArray3[index26];
                NetGeometryData netGeometryData = nativeArray4[index26];
                WaterwayData waterwayData = nativeArray19[index26];
                netData.m_RequiredLayers |= Layer.Waterway;
                netData.m_ConnectLayers |= Layer.Waterway;
                netData.m_LocalConnectLayers |= Layer.Pathway | Layer.MarkerPathway;
                netGeometryData.m_MergeLayers |= Layer.Waterway;
                netGeometryData.m_IntersectLayers |= Layer.Waterway;
                netGeometryData.m_EdgeLengthRange.max = 1000f;
                netGeometryData.m_ElevatedLength = 1000f;
                netGeometryData.m_Flags |= GeometryFlags.BlockZone | GeometryFlags.Directional | GeometryFlags.FlattenTerrain | GeometryFlags.OnWater;
                waterwayData.m_SpeedLimit = prefab.m_SpeedLimit / 3.6f;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index26];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.Floating;
                  placeableNetData.m_SnapDistance = 16f;
                  nativeArray5[index26] = placeableNetData;
                }
                nativeArray3[index26] = netData;
                nativeArray4[index26] = netGeometryData;
                nativeArray19[index26] = waterwayData;
              }
            }
            NativeArray<PathwayData> nativeArray20 = archetypeChunk.GetNativeArray<PathwayData>(ref componentTypeHandle21);
            if (nativeArray20.Length != 0)
            {
              NativeArray<LocalConnectData> nativeArray21 = archetypeChunk.GetNativeArray<LocalConnectData>(ref componentTypeHandle8);
              for (int index27 = 0; index27 < nativeArray20.Length; ++index27)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                PathwayPrefab prefab = this.m_PrefabSystem.GetPrefab<PathwayPrefab>(nativeArray2[index27]);
                NetData netData = nativeArray3[index27];
                NetGeometryData netGeometryData = nativeArray4[index27];
                LocalConnectData localConnectData = nativeArray21[index27];
                PathwayData pathwayData = nativeArray20[index27];
                Layer layer = flag2 ? Layer.MarkerPathway : Layer.Pathway;
                netData.m_RequiredLayers |= layer;
                netData.m_ConnectLayers |= Layer.Pathway | Layer.MarkerPathway;
                netData.m_LocalConnectLayers |= Layer.Pathway | Layer.MarkerPathway;
                netGeometryData.m_MergeLayers |= layer;
                netGeometryData.m_IntersectLayers |= Layer.Pathway | Layer.MarkerPathway;
                netGeometryData.m_ElevationLimit = 2f;
                netGeometryData.m_Flags |= GeometryFlags.Directional;
                if (flag2)
                {
                  netGeometryData.m_ElevatedLength = netGeometryData.m_EdgeLengthRange.max;
                  netGeometryData.m_Flags |= GeometryFlags.LoweredIsTunnel | GeometryFlags.RaisedIsElevated;
                }
                else
                {
                  netGeometryData.m_ElevatedLength = 40f;
                  netGeometryData.m_Flags |= GeometryFlags.BlockZone | GeometryFlags.FlattenTerrain | GeometryFlags.ClipTerrain;
                }
                localConnectData.m_Flags |= LocalConnectFlags.KeepOpen | LocalConnectFlags.RequireDeadend | LocalConnectFlags.ChooseBest | LocalConnectFlags.ChooseSides;
                localConnectData.m_Layers |= Layer.Road | Layer.TrainTrack | Layer.Pathway | Layer.Waterway | Layer.TramTrack | Layer.SubwayTrack | Layer.MarkerPathway | Layer.PublicTransportRoad;
                localConnectData.m_HeightRange = new Bounds1(-8f, 8f);
                localConnectData.m_SearchDistance = 4f;
                pathwayData.m_SpeedLimit = prefab.m_SpeedLimit / 3.6f;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index27];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.OnGround;
                  placeableNetData.m_SnapDistance = flag2 ? 2f : 4f;
                  nativeArray5[index27] = placeableNetData;
                }
                nativeArray3[index27] = netData;
                nativeArray4[index27] = netGeometryData;
                nativeArray21[index27] = localConnectData;
                nativeArray20[index27] = pathwayData;
              }
            }
            NativeArray<TaxiwayData> nativeArray22 = archetypeChunk.GetNativeArray<TaxiwayData>(ref componentTypeHandle22);
            if (nativeArray22.Length != 0)
            {
              for (int index28 = 0; index28 < nativeArray22.Length; ++index28)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                TaxiwayPrefab prefab = this.m_PrefabSystem.GetPrefab<TaxiwayPrefab>(nativeArray2[index28]);
                NetData netData = nativeArray3[index28];
                NetGeometryData netGeometryData = nativeArray4[index28];
                TaxiwayData taxiwayData = nativeArray22[index28];
                Layer layer = flag2 ? Layer.MarkerTaxiway : Layer.Taxiway;
                netData.m_RequiredLayers |= layer;
                netData.m_ConnectLayers |= Layer.Pathway | Layer.Taxiway | Layer.MarkerPathway | Layer.MarkerTaxiway;
                netGeometryData.m_MergeLayers |= layer;
                netGeometryData.m_IntersectLayers |= Layer.Pathway | Layer.Taxiway | Layer.MarkerPathway | Layer.MarkerTaxiway;
                netGeometryData.m_EdgeLengthRange.max = 1000f;
                netGeometryData.m_ElevatedLength = 1000f;
                netGeometryData.m_Flags |= GeometryFlags.Directional;
                if (!flag2)
                  netGeometryData.m_Flags |= GeometryFlags.BlockZone | GeometryFlags.FlattenTerrain | GeometryFlags.ClipTerrain;
                taxiwayData.m_SpeedLimit = prefab.m_SpeedLimit / 3.6f;
                if (prefab.m_Airspace)
                {
                  if (prefab.m_Runway)
                    taxiwayData.m_Flags |= TaxiwayFlags.Runway;
                  else if (!prefab.m_Taxiway)
                    netGeometryData.m_Flags |= GeometryFlags.RaisedIsElevated | GeometryFlags.BlockZone | GeometryFlags.FlattenTerrain;
                  taxiwayData.m_Flags |= TaxiwayFlags.Airspace;
                }
                else if (prefab.m_Runway)
                  taxiwayData.m_Flags |= TaxiwayFlags.Runway;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index28];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.OnGround;
                  placeableNetData.m_SnapDistance = flag2 ? 4f : 8f;
                  nativeArray5[index28] = placeableNetData;
                }
                nativeArray3[index28] = netData;
                nativeArray4[index28] = netGeometryData;
                nativeArray22[index28] = taxiwayData;
              }
            }
            bool flag10 = archetypeChunk.Has<PowerLineData>(ref componentTypeHandle23);
            if (flag10)
            {
              for (int index29 = 0; index29 < nativeArray1.Length; ++index29)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                PowerLinePrefab prefab = this.m_PrefabSystem.GetPrefab<PowerLinePrefab>(nativeArray2[index29]);
                NetGeometryData netGeometryData = nativeArray4[index29];
                bool flag11 = false;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index29];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.OnGround;
                  flag11 = (double) placeableNetData.m_ElevationRange.max < 0.0;
                  nativeArray5[index29] = placeableNetData;
                }
                netGeometryData.m_EdgeLengthRange.max = prefab.m_MaxPylonDistance;
                netGeometryData.m_ElevatedLength = prefab.m_MaxPylonDistance;
                netGeometryData.m_Hanging = prefab.m_Hanging;
                netGeometryData.m_Flags |= GeometryFlags.StrictNodes | GeometryFlags.LoweredIsTunnel | GeometryFlags.RaisedIsElevated;
                if (!flag2)
                  netGeometryData.m_Flags |= GeometryFlags.FlattenTerrain;
                if (flag11)
                {
                  netGeometryData.m_IntersectLayers |= Layer.PowerlineLow | Layer.PowerlineHigh;
                  netGeometryData.m_MergeLayers |= Layer.PowerlineLow | Layer.PowerlineHigh;
                }
                else
                  netGeometryData.m_Flags |= GeometryFlags.StraightEdges | GeometryFlags.NoEdgeConnection | GeometryFlags.SnapToNetAreas | GeometryFlags.BlockZone | GeometryFlags.StandingNodes;
                nativeArray4[index29] = netGeometryData;
              }
            }
            bool flag12 = archetypeChunk.Has<PipelineData>(ref componentTypeHandle24);
            if (flag12)
            {
              for (int index30 = 0; index30 < nativeArray1.Length; ++index30)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_PrefabSystem.GetPrefab<PipelinePrefab>(nativeArray2[index30]);
                NetGeometryData netGeometryData = nativeArray4[index30];
                netGeometryData.m_ElevatedLength = netGeometryData.m_EdgeLengthRange.max;
                netGeometryData.m_Flags |= GeometryFlags.StrictNodes | GeometryFlags.LoweredIsTunnel | GeometryFlags.RaisedIsElevated;
                netGeometryData.m_IntersectLayers |= Layer.WaterPipe | Layer.SewagePipe | Layer.StormwaterPipe;
                netGeometryData.m_MergeLayers |= Layer.WaterPipe | Layer.SewagePipe | Layer.StormwaterPipe;
                if (!flag2)
                  netGeometryData.m_Flags |= GeometryFlags.FlattenTerrain;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index30];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.OnGround;
                  nativeArray5[index30] = placeableNetData;
                }
                nativeArray4[index30] = netGeometryData;
              }
            }
            if (archetypeChunk.Has<FenceData>(ref componentTypeHandle25))
            {
              for (int index31 = 0; index31 < nativeArray1.Length; ++index31)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_PrefabSystem.GetPrefab<FencePrefab>(nativeArray2[index31]);
                NetData netData = nativeArray3[index31];
                NetGeometryData netGeometryData = nativeArray4[index31];
                netData.m_RequiredLayers |= Layer.Fence;
                netData.m_ConnectLayers |= Layer.Fence;
                netGeometryData.m_ElevatedLength = netGeometryData.m_EdgeLengthRange.max;
                netGeometryData.m_Flags |= GeometryFlags.StrictNodes | GeometryFlags.BlockZone | GeometryFlags.FlattenTerrain;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index31];
                  placeableNetData.m_PlacementFlags |= PlacementFlags.OnGround;
                  placeableNetData.m_SnapDistance = 4f;
                  nativeArray5[index31] = placeableNetData;
                }
                nativeArray3[index31] = netData;
                nativeArray4[index31] = netGeometryData;
              }
            }
            if (archetypeChunk.Has<EditorContainerData>(ref componentTypeHandle26))
            {
              for (int index32 = 0; index32 < nativeArray3.Length; ++index32)
              {
                NetData netData = nativeArray3[index32];
                netData.m_RequiredLayers |= Layer.LaneEditor;
                netData.m_ConnectLayers |= Layer.LaneEditor;
                nativeArray3[index32] = netData;
              }
            }
            if (flag3)
            {
              BufferAccessor<FixedNetElement> bufferAccessor12 = archetypeChunk.GetBufferAccessor<FixedNetElement>(ref bufferTypeHandle11);
              for (int index33 = 0; index33 < nativeArray4.Length; ++index33)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetGeometryPrefab prefab = this.m_PrefabSystem.GetPrefab<NetGeometryPrefab>(nativeArray2[index33]);
                Bridge component = prefab.GetComponent<Bridge>();
                NetData netData = nativeArray3[index33];
                NetGeometryData netGeometryData = nativeArray4[index33];
                netData.m_NodePriority += 1000f;
                if ((double) component.m_SegmentLength > 0.10000000149011612)
                {
                  netGeometryData.m_EdgeLengthRange.min = component.m_SegmentLength * 0.6f;
                  netGeometryData.m_EdgeLengthRange.max = component.m_SegmentLength * 1.4f;
                }
                netGeometryData.m_ElevatedLength = netGeometryData.m_EdgeLengthRange.max;
                netGeometryData.m_Hanging = component.m_Hanging;
                netGeometryData.m_Flags |= GeometryFlags.StraightEdges | GeometryFlags.StraightEnds | GeometryFlags.RequireElevated | GeometryFlags.SymmetricalEdges | GeometryFlags.SmoothSlopes;
                if (nativeArray5.Length != 0)
                {
                  PlaceableNetData placeableNetData = nativeArray5[index33];
                  switch (component.m_WaterFlow)
                  {
                    case BridgeWaterFlow.Left:
                      placeableNetData.m_PlacementFlags |= PlacementFlags.FlowLeft;
                      break;
                    case BridgeWaterFlow.Right:
                      placeableNetData.m_PlacementFlags |= PlacementFlags.FlowRight;
                      break;
                  }
                  nativeArray5[index33] = placeableNetData;
                }
                if (bufferAccessor12.Length != 0)
                {
                  DynamicBuffer<FixedNetElement> dynamicBuffer = bufferAccessor12[index33];
                  dynamicBuffer.ResizeUninitialized(component.m_FixedSegments.Length);
                  int num = 0;
                  bool flag13 = false;
                  for (int index34 = 0; index34 < dynamicBuffer.Length; ++index34)
                  {
                    FixedNetSegmentInfo fixedSegment = component.m_FixedSegments[index34];
                    flag13 |= (double) fixedSegment.m_Length <= 0.10000000149011612;
                  }
                  for (int index35 = 0; index35 < dynamicBuffer.Length; ++index35)
                  {
                    FixedNetSegmentInfo fixedSegment = component.m_FixedSegments[index35];
                    FixedNetElement fixedNetElement;
                    fixedNetElement.m_Flags = (FixedNetFlags) 0;
                    if ((double) fixedSegment.m_Length > 0.10000000149011612)
                    {
                      if (flag13)
                      {
                        fixedNetElement.m_LengthRange.min = fixedSegment.m_Length;
                        fixedNetElement.m_LengthRange.max = fixedSegment.m_Length;
                      }
                      else
                      {
                        fixedNetElement.m_LengthRange.min = fixedSegment.m_Length * 0.6f;
                        fixedNetElement.m_LengthRange.max = fixedSegment.m_Length * 1.4f;
                      }
                    }
                    else
                      fixedNetElement.m_LengthRange = netGeometryData.m_EdgeLengthRange;
                    if (fixedSegment.m_CanCurve)
                    {
                      netGeometryData.m_Flags &= ~GeometryFlags.StraightEdges;
                      ++num;
                    }
                    else
                      fixedNetElement.m_Flags |= FixedNetFlags.Straight;
                    fixedNetElement.m_CountRange = fixedSegment.m_CountRange;
                    NetSectionFlags sectionFlags14;
                    NetCompositionHelpers.GetRequirementFlags(fixedSegment.m_SetState, out fixedNetElement.m_SetState, out sectionFlags14);
                    NetSectionFlags sectionFlags15;
                    NetCompositionHelpers.GetRequirementFlags(fixedSegment.m_UnsetState, out fixedNetElement.m_UnsetState, out sectionFlags15);
                    if ((sectionFlags14 | sectionFlags15) != (NetSectionFlags) 0)
                      COSystemBase.baseLog.ErrorFormat((Object) prefab, "Net segment state ({0}) cannot (un)set section flags: {1}", (object) prefab.name, (object) (sectionFlags14 | sectionFlags15));
                    dynamicBuffer[index35] = fixedNetElement;
                  }
                  if (num >= 2)
                    netGeometryData.m_Flags |= GeometryFlags.NoCurveSplit;
                }
                nativeArray3[index33] = netData;
                nativeArray4[index33] = netGeometryData;
              }
            }
            NativeArray<ElectricityConnectionData> nativeArray23 = archetypeChunk.GetNativeArray<ElectricityConnectionData>(ref componentTypeHandle27);
            if (nativeArray23.Length != 0)
            {
              NativeArray<LocalConnectData> nativeArray24 = archetypeChunk.GetNativeArray<LocalConnectData>(ref componentTypeHandle8);
              for (int index36 = 0; index36 < nativeArray23.Length; ++index36)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                NetPrefab prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(nativeArray2[index36]);
                ElectricityConnection component = prefab.GetComponent<ElectricityConnection>();
                NetData netData = nativeArray3[index36];
                ElectricityConnectionData electricityConnectionData = nativeArray23[index36];
                Layer layer3;
                Layer layer4;
                float num;
                switch (component.m_Voltage)
                {
                  case ElectricityConnection.Voltage.Low:
                    layer3 = Layer.PowerlineLow;
                    layer4 = Layer.Road | Layer.PowerlineLow;
                    num = 4f;
                    break;
                  case ElectricityConnection.Voltage.High:
                    layer3 = Layer.PowerlineHigh;
                    layer4 = Layer.PowerlineHigh;
                    num = 8f;
                    break;
                  default:
                    layer3 = Layer.None;
                    layer4 = Layer.None;
                    num = 8f;
                    break;
                }
                if (flag10)
                {
                  netData.m_RequiredLayers |= layer3;
                  netData.m_ConnectLayers |= layer3;
                  LocalConnectData localConnectData = nativeArray24[index36];
                  localConnectData.m_Flags |= LocalConnectFlags.ExplicitNodes | LocalConnectFlags.ChooseBest;
                  localConnectData.m_Layers |= layer4;
                  localConnectData.m_HeightRange = new Bounds1(-100f, 100f);
                  localConnectData.m_SearchDistance = 0.0f;
                  if (flag2)
                  {
                    localConnectData.m_Flags |= LocalConnectFlags.KeepOpen;
                    localConnectData.m_SearchDistance = 4f;
                  }
                  nativeArray24[index36] = localConnectData;
                  if (nativeArray5.Length != 0)
                  {
                    PlaceableNetData placeableNetData = nativeArray5[index36] with
                    {
                      m_SnapDistance = num
                    };
                    nativeArray5[index36] = placeableNetData;
                  }
                }
                netData.m_LocalConnectLayers |= layer3;
                electricityConnectionData.m_Direction = component.m_Direction;
                electricityConnectionData.m_Capacity = component.m_Capacity;
                electricityConnectionData.m_Voltage = component.m_Voltage;
                NetSectionFlags sectionFlags16;
                NetCompositionHelpers.GetRequirementFlags(component.m_RequireAll, out electricityConnectionData.m_CompositionAll, out sectionFlags16);
                NetSectionFlags sectionFlags17;
                NetCompositionHelpers.GetRequirementFlags(component.m_RequireAny, out electricityConnectionData.m_CompositionAny, out sectionFlags17);
                NetSectionFlags sectionFlags18;
                NetCompositionHelpers.GetRequirementFlags(component.m_RequireNone, out electricityConnectionData.m_CompositionNone, out sectionFlags18);
                NetSectionFlags p2 = sectionFlags16 | sectionFlags17 | sectionFlags18;
                if (p2 != (NetSectionFlags) 0)
                  COSystemBase.baseLog.ErrorFormat((Object) prefab, "Electricity connection ({0}) cannot require section flags: {1}", (object) prefab.name, (object) p2);
                nativeArray3[index36] = netData;
                nativeArray23[index36] = electricityConnectionData;
              }
            }
            NativeArray<WaterPipeConnectionData> nativeArray25 = archetypeChunk.GetNativeArray<WaterPipeConnectionData>(ref componentTypeHandle28);
            if (nativeArray25.Length != 0)
            {
              NativeArray<LocalConnectData> nativeArray26 = archetypeChunk.GetNativeArray<LocalConnectData>(ref componentTypeHandle8);
              for (int index37 = 0; index37 < nativeArray25.Length; ++index37)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                WaterPipeConnection component = this.m_PrefabSystem.GetPrefab<NetPrefab>(nativeArray2[index37]).GetComponent<WaterPipeConnection>();
                NetData netData = nativeArray3[index37];
                WaterPipeConnectionData pipeConnectionData = nativeArray25[index37];
                Layer layer = Layer.None;
                if (component.m_FreshCapacity != 0)
                  layer |= Layer.WaterPipe;
                if (component.m_SewageCapacity != 0)
                  layer |= Layer.SewagePipe;
                if (component.m_StormCapacity != 0)
                  layer |= Layer.StormwaterPipe;
                if (flag12)
                {
                  netData.m_RequiredLayers |= layer;
                  netData.m_ConnectLayers |= layer;
                  LocalConnectData localConnectData = nativeArray26[index37];
                  localConnectData.m_Flags |= LocalConnectFlags.ExplicitNodes | LocalConnectFlags.ChooseBest;
                  localConnectData.m_Layers |= Layer.Road | layer;
                  localConnectData.m_HeightRange = new Bounds1(-100f, 100f);
                  localConnectData.m_SearchDistance = 0.0f;
                  if (flag2)
                  {
                    localConnectData.m_Flags |= LocalConnectFlags.KeepOpen;
                    localConnectData.m_SearchDistance = 4f;
                  }
                  nativeArray26[index37] = localConnectData;
                  if (nativeArray5.Length != 0)
                  {
                    PlaceableNetData placeableNetData = nativeArray5[index37] with
                    {
                      m_SnapDistance = 4f
                    };
                    nativeArray5[index37] = placeableNetData;
                  }
                }
                netData.m_LocalConnectLayers |= layer;
                pipeConnectionData.m_FreshCapacity = component.m_FreshCapacity;
                pipeConnectionData.m_SewageCapacity = component.m_SewageCapacity;
                pipeConnectionData.m_StormCapacity = component.m_StormCapacity;
                nativeArray3[index37] = netData;
                nativeArray25[index37] = pipeConnectionData;
              }
            }
            if (flag4)
            {
              for (int index38 = 0; index38 < nativeArray3.Length; ++index38)
              {
                NetData netData = nativeArray3[index38];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_InGameLayersTwice |= this.m_InGameLayersOnce & netData.m_RequiredLayers;
                // ISSUE: reference to a compiler-generated field
                this.m_InGameLayersOnce |= netData.m_RequiredLayers;
              }
            }
          }
        }
      }
      catch
      {
        archetypeChunkArray.Dispose();
        throw;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindHeuristicData.value = new PathfindHeuristicData()
      {
        m_CarCosts = new PathfindCosts(1000000f, 1000000f, 1000000f, 1000000f),
        m_TrackCosts = new PathfindCosts(1000000f, 1000000f, 1000000f, 1000000f),
        m_PedestrianCosts = new PathfindCosts(1000000f, 1000000f, 1000000f, 1000000f),
        m_FlyingCosts = new PathfindCosts(1000000f, 1000000f, 1000000f, 1000000f),
        m_TaxiCosts = new PathfindCosts(1000000f, 1000000f, 1000000f, 1000000f),
        m_OffRoadCosts = new PathfindCosts(1000000f, 1000000f, 1000000f, 1000000f)
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPieceObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPieceLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetSectionPiece_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetSubSection_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetVertexMatchData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPieceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DefaultNetLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      NetInitializeSystem.InitializeNetDefaultsJob jobData1 = new NetInitializeSystem.InitializeNetDefaultsJob()
      {
        m_Chunks = archetypeChunkArray,
        m_NetGeometrySectionType = this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RO_BufferTypeHandle,
        m_NetType = this.__TypeHandle.__Game_Prefabs_NetData_RW_ComponentTypeHandle,
        m_NetGeometryType = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle,
        m_PlaceableNetType = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RW_ComponentTypeHandle,
        m_RoadType = this.__TypeHandle.__Game_Prefabs_RoadData_RW_ComponentTypeHandle,
        m_DefaultNetLaneType = this.__TypeHandle.__Game_Prefabs_DefaultNetLane_RW_BufferTypeHandle,
        m_NetPieceData = this.__TypeHandle.__Game_Prefabs_NetPieceData_RO_ComponentLookup,
        m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_NetVertexMatchData = this.__TypeHandle.__Game_Prefabs_NetVertexMatchData_RO_ComponentLookup,
        m_PlaceableNetPieceData = this.__TypeHandle.__Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup,
        m_PlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_NetSubSectionData = this.__TypeHandle.__Game_Prefabs_NetSubSection_RO_BufferLookup,
        m_NetSectionPieceData = this.__TypeHandle.__Game_Prefabs_NetSectionPiece_RO_BufferLookup,
        m_NetPieceLanes = this.__TypeHandle.__Game_Prefabs_NetPieceLane_RO_BufferLookup,
        m_NetPieceObjects = this.__TypeHandle.__Game_Prefabs_NetPieceObject_RO_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConnectionLaneData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      NetInitializeSystem.CollectPathfindDataJob jobData2 = new NetInitializeSystem.CollectPathfindDataJob()
      {
        m_NetLaneDataType = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentTypeHandle,
        m_ConnectionLaneDataType = this.__TypeHandle.__Game_Prefabs_ConnectionLaneData_RO_ComponentTypeHandle,
        m_PathfindCarData = this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup,
        m_PathfindTrackData = this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup,
        m_PathfindPedestrianData = this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup,
        m_PathfindTransportData = this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup,
        m_PathfindConnectionData = this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup,
        m_PathfindHeuristicData = this.m_PathfindHeuristicData
      };
      JobHandle job0_1 = jobData1.Schedule<NetInitializeSystem.InitializeNetDefaultsJob>(archetypeChunkArray.Length, 1, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery laneQuery = this.m_LaneQuery;
      JobHandle dependency = this.Dependency;
      JobHandle job1_1 = jobData2.Schedule<NetInitializeSystem.CollectPathfindDataJob>(laneQuery, dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindHeuristicDeps = job1_1;
      JobHandle job0_2 = JobHandle.CombineDependencies(job0_1, job1_1);
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        JobHandle job1_2 = new NetInitializeSystem.FixPlaceholdersJob()
        {
          m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_PlaceholderObjectElementType = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle
        }.ScheduleParallel<NetInitializeSystem.FixPlaceholdersJob>(this.m_PlaceholderQuery, this.Dependency);
        job0_2 = JobHandle.CombineDependencies(job0_2, job1_2);
      }
      this.Dependency = job0_2;
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
    public NetInitializeSystem()
    {
    }

    [BurstCompile]
    private struct FixPlaceholdersJob : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      public BufferTypeHandle<PlaceholderObjectElement> m_PlaceholderObjectElementType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PlaceholderObjectElement> bufferAccessor = chunk.GetBufferAccessor<PlaceholderObjectElement>(ref this.m_PlaceholderObjectElementType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<PlaceholderObjectElement> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeletedData.HasComponent(dynamicBuffer[index2].m_Object))
              dynamicBuffer.RemoveAtSwapBack(index2--);
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
    private struct InitializeNetDefaultsJob : IJobParallelFor
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public BufferTypeHandle<NetGeometrySection> m_NetGeometrySectionType;
      public ComponentTypeHandle<NetData> m_NetType;
      public ComponentTypeHandle<NetGeometryData> m_NetGeometryType;
      public ComponentTypeHandle<PlaceableNetData> m_PlaceableNetType;
      public ComponentTypeHandle<RoadData> m_RoadType;
      public BufferTypeHandle<DefaultNetLane> m_DefaultNetLaneType;
      [ReadOnly]
      public ComponentLookup<NetPieceData> m_NetPieceData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<NetVertexMatchData> m_NetVertexMatchData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetPieceData> m_PlaceableNetPieceData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PlaceableObjectData;
      [ReadOnly]
      public BufferLookup<NetSubSection> m_NetSubSectionData;
      [ReadOnly]
      public BufferLookup<NetSectionPiece> m_NetSectionPieceData;
      [ReadOnly]
      public BufferLookup<NetPieceLane> m_NetPieceLanes;
      [ReadOnly]
      public BufferLookup<NetPieceObject> m_NetPieceObjects;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetGeometryData> nativeArray1 = chunk.GetNativeArray<NetGeometryData>(ref this.m_NetGeometryType);
        if (nativeArray1.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetData> nativeArray2 = chunk.GetNativeArray<NetData>(ref this.m_NetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PlaceableNetData> nativeArray3 = chunk.GetNativeArray<PlaceableNetData>(ref this.m_PlaceableNetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<RoadData> nativeArray4 = chunk.GetNativeArray<RoadData>(ref this.m_RoadType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<DefaultNetLane> bufferAccessor1 = chunk.GetBufferAccessor<DefaultNetLane>(ref this.m_DefaultNetLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetGeometrySection> bufferAccessor2 = chunk.GetBufferAccessor<NetGeometrySection>(ref this.m_NetGeometrySectionType);
        NativeList<NetCompositionPiece> nativeList = new NativeList<NetCompositionPiece>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<NetCompositionLane> netLanes = new NativeList<NetCompositionLane>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        CompositionFlags flags1 = new CompositionFlags();
        CompositionFlags flags2 = new CompositionFlags(CompositionFlags.General.Elevated, (CompositionFlags.Side) 0, (CompositionFlags.Side) 0);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          DynamicBuffer<NetGeometrySection> geometrySections = bufferAccessor2[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.GetCompositionPieces(nativeList, geometrySections.AsNativeArray(), flags1, this.m_NetSubSectionData, this.m_NetSectionPieceData);
          NetCompositionData compositionData1 = new NetCompositionData();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.CalculateCompositionData(ref compositionData1, nativeList.AsArray(), this.m_NetPieceData, this.m_NetLaneData, this.m_NetVertexMatchData, this.m_NetPieceLanes);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.AddCompositionLanes<NativeList<NetCompositionPiece>>(Entity.Null, ref compositionData1, nativeList, netLanes, new DynamicBuffer<NetCompositionCarriageway>(), this.m_NetLaneData, this.m_NetPieceLanes);
          if (bufferAccessor1.Length != 0)
          {
            DynamicBuffer<DefaultNetLane> dynamicBuffer = bufferAccessor1[index1];
            dynamicBuffer.ResizeUninitialized(netLanes.Length);
            for (int index2 = 0; index2 < netLanes.Length; ++index2)
              dynamicBuffer[index2] = new DefaultNetLane(netLanes[index2]);
          }
          NetData netData = nativeArray2[index1];
          netData.m_NodePriority += compositionData1.m_Width;
          NetGeometryData netGeometryData = nativeArray1[index1] with
          {
            m_DefaultWidth = compositionData1.m_Width,
            m_DefaultHeightRange = compositionData1.m_HeightRange,
            m_DefaultSurfaceHeight = compositionData1.m_SurfaceHeight
          };
          // ISSUE: reference to a compiler-generated method
          this.UpdateFlagMasks(ref netData, geometrySections);
          if ((netData.m_RequiredLayers & (Layer.Road | Layer.TramTrack | Layer.PublicTransportRoad)) != Layer.None)
          {
            netData.m_GeneralFlagMask |= CompositionFlags.General.TrafficLights | CompositionFlags.General.RemoveTrafficLights;
            netData.m_SideFlagMask |= CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk;
          }
          if ((netData.m_RequiredLayers & (Layer.Road | Layer.PublicTransportRoad)) != Layer.None)
          {
            netData.m_GeneralFlagMask |= CompositionFlags.General.AllWayStop;
            netData.m_SideFlagMask |= CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.ForbidStraight;
          }
          if ((compositionData1.m_State & (CompositionState.HasForwardRoadLanes | CompositionState.HasForwardTrackLanes)) != 0 != ((compositionData1.m_State & (CompositionState.HasBackwardRoadLanes | CompositionState.HasBackwardTrackLanes)) != 0))
            netGeometryData.m_Flags |= GeometryFlags.FlipTrafficHandedness;
          if ((compositionData1.m_State & CompositionState.Asymmetric) != (CompositionState) 0)
            netGeometryData.m_Flags |= GeometryFlags.Asymmetric;
          if ((compositionData1.m_State & CompositionState.ExclusiveGround) != (CompositionState) 0)
            netGeometryData.m_Flags |= GeometryFlags.ExclusiveGround;
          if (nativeArray3.Length != 0 && (netGeometryData.m_Flags & GeometryFlags.RequireElevated) == (GeometryFlags) 0)
          {
            PlaceableNetComposition placeableNetComposition = new PlaceableNetComposition();
            // ISSUE: reference to a compiler-generated field
            NetCompositionHelpers.CalculatePlaceableData(ref placeableNetComposition, nativeList.AsArray(), this.m_PlaceableNetPieceData);
            // ISSUE: reference to a compiler-generated method
            this.AddObjectCosts(ref placeableNetComposition, nativeList);
            PlaceableNetData placeableNetData = nativeArray3[index1] with
            {
              m_DefaultConstructionCost = placeableNetComposition.m_ConstructionCost,
              m_DefaultUpkeepCost = placeableNetComposition.m_UpkeepCost
            };
            nativeArray3[index1] = placeableNetData;
          }
          if (nativeArray4.Length != 0)
          {
            RoadData roadData = nativeArray4[index1];
            if ((compositionData1.m_State & (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes)) == CompositionState.HasForwardRoadLanes)
              roadData.m_Flags |= RoadFlags.DefaultIsForward;
            else if ((compositionData1.m_State & (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes)) == CompositionState.HasBackwardRoadLanes)
              roadData.m_Flags |= RoadFlags.DefaultIsBackward;
            if ((roadData.m_Flags & RoadFlags.UseHighwayRules) != (RoadFlags) 0)
              netGeometryData.m_MinNodeOffset += netGeometryData.m_DefaultWidth * 0.5f;
            nativeArray4[index1] = roadData;
          }
          nativeList.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.GetCompositionPieces(nativeList, geometrySections.AsNativeArray(), flags2, this.m_NetSubSectionData, this.m_NetSectionPieceData);
          NetCompositionData compositionData2 = new NetCompositionData();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.CalculateCompositionData(ref compositionData2, nativeList.AsArray(), this.m_NetPieceData, this.m_NetLaneData, this.m_NetVertexMatchData, this.m_NetPieceLanes);
          netGeometryData.m_ElevatedWidth = compositionData2.m_Width;
          netGeometryData.m_ElevatedHeightRange = compositionData2.m_HeightRange;
          if (nativeArray3.Length != 0 && (netGeometryData.m_Flags & GeometryFlags.RequireElevated) != (GeometryFlags) 0)
          {
            PlaceableNetComposition placeableNetComposition = new PlaceableNetComposition();
            // ISSUE: reference to a compiler-generated field
            NetCompositionHelpers.CalculatePlaceableData(ref placeableNetComposition, nativeList.AsArray(), this.m_PlaceableNetPieceData);
            // ISSUE: reference to a compiler-generated method
            this.AddObjectCosts(ref placeableNetComposition, nativeList);
            PlaceableNetData placeableNetData = nativeArray3[index1] with
            {
              m_DefaultConstructionCost = placeableNetComposition.m_ConstructionCost,
              m_DefaultUpkeepCost = placeableNetComposition.m_UpkeepCost
            };
            nativeArray3[index1] = placeableNetData;
          }
          nativeArray2[index1] = netData;
          nativeArray1[index1] = netGeometryData;
          nativeList.Clear();
          netLanes.Clear();
        }
        nativeList.Dispose();
        netLanes.Dispose();
      }

      private void UpdateFlagMasks(
        ref NetData netData,
        DynamicBuffer<NetGeometrySection> geometrySections)
      {
        for (int index = 0; index < geometrySections.Length; ++index)
        {
          NetGeometrySection geometrySection = geometrySections[index];
          netData.m_GeneralFlagMask |= geometrySection.m_CompositionAll.m_General;
          netData.m_SideFlagMask |= geometrySection.m_CompositionAll.m_Left | geometrySection.m_CompositionAll.m_Right;
          netData.m_GeneralFlagMask |= geometrySection.m_CompositionAny.m_General;
          netData.m_SideFlagMask |= geometrySection.m_CompositionAny.m_Left | geometrySection.m_CompositionAny.m_Right;
          netData.m_GeneralFlagMask |= geometrySection.m_CompositionNone.m_General;
          netData.m_SideFlagMask |= geometrySection.m_CompositionNone.m_Left | geometrySection.m_CompositionNone.m_Right;
          // ISSUE: reference to a compiler-generated method
          this.UpdateFlagMasks(ref netData, geometrySection.m_Section);
        }
      }

      private void UpdateFlagMasks(ref NetData netData, Entity section)
      {
        DynamicBuffer<NetSubSection> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetSubSectionData.TryGetBuffer(section, out bufferData1))
        {
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            NetSubSection netSubSection = bufferData1[index];
            netData.m_GeneralFlagMask |= netSubSection.m_CompositionAll.m_General;
            netData.m_SideFlagMask |= netSubSection.m_CompositionAll.m_Left | netSubSection.m_CompositionAll.m_Right;
            netData.m_GeneralFlagMask |= netSubSection.m_CompositionAny.m_General;
            netData.m_SideFlagMask |= netSubSection.m_CompositionAny.m_Left | netSubSection.m_CompositionAny.m_Right;
            netData.m_GeneralFlagMask |= netSubSection.m_CompositionNone.m_General;
            netData.m_SideFlagMask |= netSubSection.m_CompositionNone.m_Left | netSubSection.m_CompositionNone.m_Right;
            // ISSUE: reference to a compiler-generated method
            this.UpdateFlagMasks(ref netData, netSubSection.m_SubSection);
          }
        }
        DynamicBuffer<NetSectionPiece> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_NetSectionPieceData.TryGetBuffer(section, out bufferData2))
          return;
        for (int index1 = 0; index1 < bufferData2.Length; ++index1)
        {
          NetSectionPiece netSectionPiece = bufferData2[index1];
          netData.m_GeneralFlagMask |= netSectionPiece.m_CompositionAll.m_General;
          netData.m_SideFlagMask |= netSectionPiece.m_CompositionAll.m_Left | netSectionPiece.m_CompositionAll.m_Right;
          netData.m_GeneralFlagMask |= netSectionPiece.m_CompositionAny.m_General;
          netData.m_SideFlagMask |= netSectionPiece.m_CompositionAny.m_Left | netSectionPiece.m_CompositionAny.m_Right;
          netData.m_GeneralFlagMask |= netSectionPiece.m_CompositionNone.m_General;
          netData.m_SideFlagMask |= netSectionPiece.m_CompositionNone.m_Left | netSectionPiece.m_CompositionNone.m_Right;
          DynamicBuffer<NetPieceObject> bufferData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetPieceObjects.TryGetBuffer(netSectionPiece.m_Piece, out bufferData3))
          {
            for (int index2 = 0; index2 < bufferData3.Length; ++index2)
            {
              NetPieceObject netPieceObject = bufferData3[index2];
              netData.m_GeneralFlagMask |= netPieceObject.m_CompositionAll.m_General;
              netData.m_SideFlagMask |= netPieceObject.m_CompositionAll.m_Left | netPieceObject.m_CompositionAll.m_Right;
              netData.m_GeneralFlagMask |= netPieceObject.m_CompositionAny.m_General;
              netData.m_SideFlagMask |= netPieceObject.m_CompositionAny.m_Left | netPieceObject.m_CompositionAny.m_Right;
              netData.m_GeneralFlagMask |= netPieceObject.m_CompositionNone.m_General;
              netData.m_SideFlagMask |= netPieceObject.m_CompositionNone.m_Left | netPieceObject.m_CompositionNone.m_Right;
            }
          }
        }
      }

      private void AddObjectCosts(
        ref PlaceableNetComposition placeableCompositionData,
        NativeList<NetCompositionPiece> pieceBuffer)
      {
        for (int index1 = 0; index1 < pieceBuffer.Length; ++index1)
        {
          NetCompositionPiece compositionPiece = pieceBuffer[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetPieceObjects.HasBuffer(compositionPiece.m_Piece))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetPieceObject> netPieceObject1 = this.m_NetPieceObjects[compositionPiece.m_Piece];
            for (int index2 = 0; index2 < netPieceObject1.Length; ++index2)
            {
              NetPieceObject netPieceObject2 = netPieceObject1[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_PlaceableObjectData.HasComponent(netPieceObject2.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                uint constructionCost = this.m_PlaceableObjectData[netPieceObject2.m_Prefab].m_ConstructionCost;
                if ((double) netPieceObject2.m_Spacing.z > 0.10000000149011612)
                  constructionCost = (uint) Mathf.RoundToInt((float) constructionCost * (8f / netPieceObject2.m_Spacing.z));
                placeableCompositionData.m_ConstructionCost += constructionCost;
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct CollectPathfindDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<NetLaneData> m_NetLaneDataType;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLaneData> m_ConnectionLaneDataType;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> m_PathfindCarData;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> m_PathfindPedestrianData;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> m_PathfindTrackData;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> m_PathfindTransportData;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> m_PathfindConnectionData;
      public NativeValue<PathfindHeuristicData> m_PathfindHeuristicData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetLaneData> nativeArray = chunk.GetNativeArray<NetLaneData>(ref this.m_NetLaneDataType);
        // ISSUE: reference to a compiler-generated field
        PathfindHeuristicData pathfindHeuristicData = this.m_PathfindHeuristicData.value;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<ConnectionLaneData>(ref this.m_ConnectionLaneDataType))
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            PathfindConnectionData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathfindConnectionData.TryGetComponent(nativeArray[index].m_PathfindPrefab, out componentData))
            {
              pathfindHeuristicData.m_FlyingCosts.m_Value = math.min(pathfindHeuristicData.m_FlyingCosts.m_Value, componentData.m_AirwayCost.m_Value);
              pathfindHeuristicData.m_OffRoadCosts.m_Value = math.min(pathfindHeuristicData.m_OffRoadCosts.m_Value, componentData.m_AreaCost.m_Value);
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            NetLaneData netLaneData = nativeArray[index];
            if ((netLaneData.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
            {
              PathfindCarData componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathfindCarData.TryGetComponent(netLaneData.m_PathfindPrefab, out componentData1))
                pathfindHeuristicData.m_CarCosts.m_Value = math.min(pathfindHeuristicData.m_CarCosts.m_Value, componentData1.m_DrivingCost.m_Value);
              PathfindTransportData componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathfindTransportData.TryGetComponent(netLaneData.m_PathfindPrefab, out componentData2))
                pathfindHeuristicData.m_TaxiCosts.m_Value = math.min(pathfindHeuristicData.m_TaxiCosts.m_Value, componentData2.m_TravelCost.m_Value);
            }
            PathfindTrackData componentData3;
            // ISSUE: reference to a compiler-generated field
            if ((netLaneData.m_Flags & LaneFlags.Track) != (LaneFlags) 0 && this.m_PathfindTrackData.TryGetComponent(netLaneData.m_PathfindPrefab, out componentData3))
              pathfindHeuristicData.m_TrackCosts.m_Value = math.min(pathfindHeuristicData.m_TrackCosts.m_Value, componentData3.m_DrivingCost.m_Value);
            PathfindPedestrianData componentData4;
            // ISSUE: reference to a compiler-generated field
            if ((netLaneData.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0 && this.m_PathfindPedestrianData.TryGetComponent(netLaneData.m_PathfindPrefab, out componentData4))
              pathfindHeuristicData.m_PedestrianCosts.m_Value = math.min(pathfindHeuristicData.m_PedestrianCosts.m_Value, componentData4.m_WalkingCost.m_Value);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindHeuristicData.value = pathfindHeuristicData;
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
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<NetData> __Game_Prefabs_NetData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetPieceData> __Game_Prefabs_NetPieceData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetGeometryData> __Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PlaceableNetData> __Game_Prefabs_PlaceableNetData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MarkerNetData> __Game_Prefabs_MarkerNetData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<LocalConnectData> __Game_Prefabs_LocalConnectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetLaneData> __Game_Prefabs_NetLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarLaneData> __Game_Prefabs_CarLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TrackLaneData> __Game_Prefabs_TrackLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PedestrianLaneData> __Game_Prefabs_PedestrianLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<SecondaryLaneData> __Game_Prefabs_SecondaryLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetCrosswalkData> __Game_Prefabs_NetCrosswalkData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<RoadData> __Game_Prefabs_RoadData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TrackData> __Game_Prefabs_TrackData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WaterwayData> __Game_Prefabs_WaterwayData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathwayData> __Game_Prefabs_PathwayData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TaxiwayData> __Game_Prefabs_TaxiwayData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PowerLineData> __Game_Prefabs_PowerLineData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PipelineData> __Game_Prefabs_PipelineData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<FenceData> __Game_Prefabs_FenceData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainerData> __Game_Prefabs_EditorContainerData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WaterPipeConnectionData> __Game_Prefabs_WaterPipeConnectionData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BridgeData> __Game_Prefabs_BridgeData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetTerrainData> __Game_Prefabs_NetTerrainData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UIObjectData> __Game_Prefabs_UIObjectData_RO_ComponentTypeHandle;
      public BufferTypeHandle<NetSubSection> __Game_Prefabs_NetSubSection_RW_BufferTypeHandle;
      public BufferTypeHandle<NetSectionPiece> __Game_Prefabs_NetSectionPiece_RW_BufferTypeHandle;
      public BufferTypeHandle<NetPieceLane> __Game_Prefabs_NetPieceLane_RW_BufferTypeHandle;
      public BufferTypeHandle<NetPieceArea> __Game_Prefabs_NetPieceArea_RW_BufferTypeHandle;
      public BufferTypeHandle<NetPieceObject> __Game_Prefabs_NetPieceObject_RW_BufferTypeHandle;
      public BufferTypeHandle<NetGeometrySection> __Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle;
      public BufferTypeHandle<NetGeometryEdgeState> __Game_Prefabs_NetGeometryEdgeState_RW_BufferTypeHandle;
      public BufferTypeHandle<NetGeometryNodeState> __Game_Prefabs_NetGeometryNodeState_RW_BufferTypeHandle;
      public BufferTypeHandle<SubObject> __Game_Prefabs_SubObject_RW_BufferTypeHandle;
      public BufferTypeHandle<SubMesh> __Game_Prefabs_SubMesh_RW_BufferTypeHandle;
      public BufferTypeHandle<FixedNetElement> __Game_Prefabs_FixedNetElement_RW_BufferTypeHandle;
      public BufferTypeHandle<AuxiliaryNetLane> __Game_Prefabs_AuxiliaryNetLane_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<NetGeometrySection> __Game_Prefabs_NetGeometrySection_RO_BufferTypeHandle;
      public BufferTypeHandle<DefaultNetLane> __Game_Prefabs_DefaultNetLane_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetPieceData> __Game_Prefabs_NetPieceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetVertexMatchData> __Game_Prefabs_NetVertexMatchData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetPieceData> __Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<NetSubSection> __Game_Prefabs_NetSubSection_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetSectionPiece> __Game_Prefabs_NetSectionPiece_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetPieceLane> __Game_Prefabs_NetPieceLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetPieceObject> __Game_Prefabs_NetPieceObject_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLaneData> __Game_Prefabs_ConnectionLaneData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> __Game_Prefabs_PathfindCarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> __Game_Prefabs_PathfindTrackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> __Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> __Game_Prefabs_PathfindTransportData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> __Game_Prefabs_PathfindConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      public BufferTypeHandle<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetPieceData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PlaceableNetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MarkerNetData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MarkerNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalConnectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetLaneGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TrackLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UtilityLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ParkingLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PedestrianLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PedestrianLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SecondaryLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SecondaryLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCrosswalkData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetCrosswalkData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RoadData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TrackData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterwayData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterwayData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathwayData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathwayData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TaxiwayData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TaxiwayData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PowerLineData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PowerLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PipelineData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PipelineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FenceData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FenceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EditorContainerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EditorContainerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConnectionData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPipeConnectionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeConnectionData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BridgeData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BridgeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetTerrainData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetTerrainData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UIObjectData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UIObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetSubSection_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetSubSection>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetSectionPiece_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetSectionPiece>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetPieceLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceArea_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetPieceArea>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetPieceObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetGeometrySection>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryEdgeState_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetGeometryEdgeState>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryNodeState_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetGeometryNodeState>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FixedNetElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<FixedNetElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AuxiliaryNetLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<AuxiliaryNetLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometrySection_RO_BufferTypeHandle = state.GetBufferTypeHandle<NetGeometrySection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DefaultNetLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<DefaultNetLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceData_RO_ComponentLookup = state.GetComponentLookup<NetPieceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetVertexMatchData_RO_ComponentLookup = state.GetComponentLookup<NetVertexMatchData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetPieceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetSubSection_RO_BufferLookup = state.GetBufferLookup<NetSubSection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetSectionPiece_RO_BufferLookup = state.GetBufferLookup<NetSectionPiece>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceLane_RO_BufferLookup = state.GetBufferLookup<NetPieceLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceObject_RO_BufferLookup = state.GetBufferLookup<NetPieceObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConnectionLaneData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ConnectionLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindCarData_RO_ComponentLookup = state.GetComponentLookup<PathfindCarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup = state.GetComponentLookup<PathfindTrackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup = state.GetComponentLookup<PathfindPedestrianData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup = state.GetComponentLookup<PathfindTransportData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup = state.GetComponentLookup<PathfindConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PlaceholderObjectElement>();
      }
    }
  }
}
