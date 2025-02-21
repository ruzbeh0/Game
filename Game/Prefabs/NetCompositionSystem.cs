// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetCompositionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class NetCompositionSystem : GameSystemBase
  {
    private EntityQuery m_CompositionQuery;
    private NetCompositionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CompositionQuery = this.GetEntityQuery(ComponentType.ReadWrite<NetCompositionData>(), ComponentType.ReadWrite<NetCompositionPiece>(), ComponentType.ReadOnly<NetCompositionMeshRef>(), ComponentType.ReadOnly<Created>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompositionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPieceArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPieceObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPieceLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BridgeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetTerrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrafficSignData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StreetLightData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TaxiwayData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathwayData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterwayData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetVertexMatchData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCrosswalkData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPieceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionCarriageway_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionCrosswalk_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionArea_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionPiece_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TerrainComposition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TaxiwayComposition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathwayComposition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterwayComposition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackComposition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadComposition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      NetCompositionSystem.InitializeCompositionJob jobData = new NetCompositionSystem.InitializeCompositionJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NetCompositionType = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RW_ComponentTypeHandle,
        m_PlaceableNetCompositionType = this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RW_ComponentTypeHandle,
        m_RoadCompositionType = this.__TypeHandle.__Game_Prefabs_RoadComposition_RW_ComponentTypeHandle,
        m_TrackCompositionType = this.__TypeHandle.__Game_Prefabs_TrackComposition_RW_ComponentTypeHandle,
        m_WaterwayCompositionType = this.__TypeHandle.__Game_Prefabs_WaterwayComposition_RW_ComponentTypeHandle,
        m_PathwayCompositionType = this.__TypeHandle.__Game_Prefabs_PathwayComposition_RW_ComponentTypeHandle,
        m_TaxiwayCompositionType = this.__TypeHandle.__Game_Prefabs_TaxiwayComposition_RW_ComponentTypeHandle,
        m_TerrainCompositionType = this.__TypeHandle.__Game_Prefabs_TerrainComposition_RW_ComponentTypeHandle,
        m_NetCompositionPieceType = this.__TypeHandle.__Game_Prefabs_NetCompositionPiece_RW_BufferTypeHandle,
        m_NetCompositionLaneType = this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RW_BufferTypeHandle,
        m_NetCompositionObjectType = this.__TypeHandle.__Game_Prefabs_NetCompositionObject_RW_BufferTypeHandle,
        m_NetCompositionAreaType = this.__TypeHandle.__Game_Prefabs_NetCompositionArea_RW_BufferTypeHandle,
        m_NetCompositionCrosswalkType = this.__TypeHandle.__Game_Prefabs_NetCompositionCrosswalk_RW_BufferTypeHandle,
        m_NetCompositionCarriagewayType = this.__TypeHandle.__Game_Prefabs_NetCompositionCarriageway_RW_BufferTypeHandle,
        m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PlaceableNetPieceData = this.__TypeHandle.__Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup,
        m_NetPieceData = this.__TypeHandle.__Game_Prefabs_NetPieceData_RO_ComponentLookup,
        m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_NetCrosswalkData = this.__TypeHandle.__Game_Prefabs_NetCrosswalkData_RO_ComponentLookup,
        m_NetVertexMatchData = this.__TypeHandle.__Game_Prefabs_NetVertexMatchData_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
        m_TrackData = this.__TypeHandle.__Game_Prefabs_TrackData_RO_ComponentLookup,
        m_WaterwayData = this.__TypeHandle.__Game_Prefabs_WaterwayData_RO_ComponentLookup,
        m_PathwayData = this.__TypeHandle.__Game_Prefabs_PathwayData_RO_ComponentLookup,
        m_TaxiwayData = this.__TypeHandle.__Game_Prefabs_TaxiwayData_RO_ComponentLookup,
        m_StreetLightData = this.__TypeHandle.__Game_Prefabs_StreetLightData_RO_ComponentLookup,
        m_LaneDirectionData = this.__TypeHandle.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup,
        m_TrafficSignData = this.__TypeHandle.__Game_Prefabs_TrafficSignData_RO_ComponentLookup,
        m_UtilityObjectData = this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup,
        m_MeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_TerrainData = this.__TypeHandle.__Game_Prefabs_NetTerrainData_RO_ComponentLookup,
        m_BridgeData = this.__TypeHandle.__Game_Prefabs_BridgeData_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_NetPieceLanes = this.__TypeHandle.__Game_Prefabs_NetPieceLane_RO_BufferLookup,
        m_NetPieceObjects = this.__TypeHandle.__Game_Prefabs_NetPieceObject_RO_BufferLookup,
        m_NetPieceAreas = this.__TypeHandle.__Game_Prefabs_NetPieceArea_RO_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<NetCompositionSystem.InitializeCompositionJob>(this.m_CompositionQuery, this.Dependency);
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
    public NetCompositionSystem()
    {
    }

    [BurstCompile]
    private struct InitializeCompositionJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<NetCompositionData> m_NetCompositionType;
      public ComponentTypeHandle<PlaceableNetComposition> m_PlaceableNetCompositionType;
      public ComponentTypeHandle<RoadComposition> m_RoadCompositionType;
      public ComponentTypeHandle<TrackComposition> m_TrackCompositionType;
      public ComponentTypeHandle<WaterwayComposition> m_WaterwayCompositionType;
      public ComponentTypeHandle<PathwayComposition> m_PathwayCompositionType;
      public ComponentTypeHandle<TaxiwayComposition> m_TaxiwayCompositionType;
      public ComponentTypeHandle<TerrainComposition> m_TerrainCompositionType;
      public BufferTypeHandle<NetCompositionPiece> m_NetCompositionPieceType;
      public BufferTypeHandle<NetCompositionLane> m_NetCompositionLaneType;
      public BufferTypeHandle<NetCompositionObject> m_NetCompositionObjectType;
      public BufferTypeHandle<NetCompositionArea> m_NetCompositionAreaType;
      public BufferTypeHandle<NetCompositionCrosswalk> m_NetCompositionCrosswalkType;
      public BufferTypeHandle<NetCompositionCarriageway> m_NetCompositionCarriagewayType;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetPieceData> m_PlaceableNetPieceData;
      [ReadOnly]
      public ComponentLookup<NetPieceData> m_NetPieceData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<NetCrosswalkData> m_NetCrosswalkData;
      [ReadOnly]
      public ComponentLookup<NetVertexMatchData> m_NetVertexMatchData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_RoadData;
      [ReadOnly]
      public ComponentLookup<TrackData> m_TrackData;
      [ReadOnly]
      public ComponentLookup<WaterwayData> m_WaterwayData;
      [ReadOnly]
      public ComponentLookup<PathwayData> m_PathwayData;
      [ReadOnly]
      public ComponentLookup<TaxiwayData> m_TaxiwayData;
      [ReadOnly]
      public ComponentLookup<StreetLightData> m_StreetLightData;
      [ReadOnly]
      public ComponentLookup<LaneDirectionData> m_LaneDirectionData;
      [ReadOnly]
      public ComponentLookup<TrafficSignData> m_TrafficSignData;
      [ReadOnly]
      public ComponentLookup<UtilityObjectData> m_UtilityObjectData;
      [ReadOnly]
      public ComponentLookup<MeshData> m_MeshData;
      [ReadOnly]
      public ComponentLookup<NetTerrainData> m_TerrainData;
      [ReadOnly]
      public ComponentLookup<BridgeData> m_BridgeData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public BufferLookup<NetPieceLane> m_NetPieceLanes;
      [ReadOnly]
      public BufferLookup<NetPieceObject> m_NetPieceObjects;
      [ReadOnly]
      public BufferLookup<NetPieceArea> m_NetPieceAreas;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetCompositionData> nativeArray1 = chunk.GetNativeArray<NetCompositionData>(ref this.m_NetCompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PlaceableNetComposition> nativeArray3 = chunk.GetNativeArray<PlaceableNetComposition>(ref this.m_PlaceableNetCompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<RoadComposition> nativeArray4 = chunk.GetNativeArray<RoadComposition>(ref this.m_RoadCompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrackComposition> nativeArray5 = chunk.GetNativeArray<TrackComposition>(ref this.m_TrackCompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterwayComposition> nativeArray6 = chunk.GetNativeArray<WaterwayComposition>(ref this.m_WaterwayCompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathwayComposition> nativeArray7 = chunk.GetNativeArray<PathwayComposition>(ref this.m_PathwayCompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TaxiwayComposition> nativeArray8 = chunk.GetNativeArray<TaxiwayComposition>(ref this.m_TaxiwayCompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TerrainComposition> nativeArray9 = chunk.GetNativeArray<TerrainComposition>(ref this.m_TerrainCompositionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetCompositionPiece> bufferAccessor1 = chunk.GetBufferAccessor<NetCompositionPiece>(ref this.m_NetCompositionPieceType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetCompositionLane> bufferAccessor2 = chunk.GetBufferAccessor<NetCompositionLane>(ref this.m_NetCompositionLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetCompositionObject> bufferAccessor3 = chunk.GetBufferAccessor<NetCompositionObject>(ref this.m_NetCompositionObjectType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetCompositionArea> bufferAccessor4 = chunk.GetBufferAccessor<NetCompositionArea>(ref this.m_NetCompositionAreaType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetCompositionCrosswalk> bufferAccessor5 = chunk.GetBufferAccessor<NetCompositionCrosswalk>(ref this.m_NetCompositionCrosswalkType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetCompositionCarriageway> bufferAccessor6 = chunk.GetBufferAccessor<NetCompositionCarriageway>(ref this.m_NetCompositionCarriagewayType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          NetCompositionData compositionData = nativeArray1[index];
          DynamicBuffer<NetCompositionPiece> dynamicBuffer = bufferAccessor1[index];
          PrefabRef prefabRef = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.CalculateCompositionData(ref compositionData, dynamicBuffer.AsNativeArray(), this.m_NetPieceData, this.m_NetLaneData, this.m_NetVertexMatchData, this.m_NetPieceLanes);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabData.IsComponentEnabled(prefabRef.m_Prefab))
          {
            if ((compositionData.m_Flags.m_General & CompositionFlags.General.Elevated) != (CompositionFlags.General) 0)
            {
              compositionData.m_Width = netGeometryData.m_ElevatedWidth;
              compositionData.m_HeightRange = netGeometryData.m_ElevatedHeightRange;
            }
            else
            {
              compositionData.m_Width = netGeometryData.m_DefaultWidth;
              compositionData.m_HeightRange = netGeometryData.m_DefaultHeightRange;
            }
            compositionData.m_SurfaceHeight = netGeometryData.m_DefaultSurfaceHeight;
          }
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.CalculateMinLod(ref compositionData, dynamicBuffer.AsNativeArray(), this.m_MeshData);
          if ((netGeometryData.m_Flags & GeometryFlags.Marker) != (GeometryFlags) 0)
            compositionData.m_State |= CompositionState.Marker | CompositionState.NoSubCollisions;
          if ((netGeometryData.m_Flags & GeometryFlags.BlockZone) != (GeometryFlags) 0)
            compositionData.m_State |= CompositionState.BlockZone;
          nativeArray1[index] = compositionData;
        }
        if (bufferAccessor2.Length != 0)
        {
          NativeList<NetCompositionLane> netLanes = new NativeList<NetCompositionLane>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < bufferAccessor2.Length; ++index)
          {
            NetCompositionData compositionData = nativeArray1[index];
            DynamicBuffer<NetCompositionPiece> pieces = bufferAccessor1[index];
            DynamicBuffer<NetCompositionLane> dynamicBuffer = bufferAccessor2[index];
            PrefabRef prefabRef = nativeArray2[index];
            DynamicBuffer<NetCompositionCarriageway> carriageways = new DynamicBuffer<NetCompositionCarriageway>();
            if (bufferAccessor6.Length != 0)
              carriageways = bufferAccessor6[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetCompositionHelpers.AddCompositionLanes<DynamicBuffer<NetCompositionPiece>>(prefabRef.m_Prefab, ref compositionData, pieces, netLanes, carriageways, this.m_NetLaneData, this.m_NetPieceLanes);
            dynamicBuffer.CopyFrom(netLanes.AsArray());
            netLanes.Clear();
            nativeArray1[index] = compositionData;
          }
        }
        for (int index = 0; index < bufferAccessor3.Length; ++index)
        {
          NetCompositionData compositionData = nativeArray1[index];
          DynamicBuffer<NetCompositionPiece> pieces = bufferAccessor1[index];
          DynamicBuffer<NetCompositionObject> objects = bufferAccessor3[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          NetCompositionSystem.InitializeCompositionJob.AddCompositionObjects(nativeArray2[index].m_Prefab, compositionData, pieces, objects, this.m_StreetLightData, this.m_LaneDirectionData, this.m_TrafficSignData, this.m_UtilityObjectData, this.m_NetPieceObjects);
        }
        for (int index = 0; index < bufferAccessor4.Length; ++index)
        {
          NetCompositionData compositionData = nativeArray1[index];
          DynamicBuffer<NetCompositionPiece> pieces = bufferAccessor1[index];
          DynamicBuffer<NetCompositionArea> netAreas = bufferAccessor4[index];
          PrefabRef prefabRef = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          bool isBridge = this.m_BridgeData.HasComponent(prefabRef.m_Prefab);
          // ISSUE: reference to a compiler-generated method
          this.AddCompositionAreas(prefabRef.m_Prefab, compositionData, pieces, netAreas, isBridge);
        }
        for (int index = 0; index < bufferAccessor5.Length; ++index)
        {
          NetCompositionData compositionData = nativeArray1[index];
          DynamicBuffer<NetCompositionPiece> pieces = bufferAccessor1[index];
          DynamicBuffer<NetCompositionCrosswalk> crosswalks = bufferAccessor5[index];
          // ISSUE: reference to a compiler-generated method
          this.AddCompositionCrosswalks(nativeArray2[index].m_Prefab, compositionData, pieces, crosswalks);
        }
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          PlaceableNetComposition placeableData = nativeArray3[index];
          DynamicBuffer<NetCompositionPiece> dynamicBuffer = bufferAccessor1[index];
          // ISSUE: reference to a compiler-generated field
          NetCompositionHelpers.CalculatePlaceableData(ref placeableData, dynamicBuffer.AsNativeArray(), this.m_PlaceableNetPieceData);
          nativeArray3[index] = placeableData;
        }
        for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray2[index1];
          NetCompositionData netCompositionData = nativeArray1[index1];
          RoadComposition roadComposition = nativeArray4[index1];
          // ISSUE: reference to a compiler-generated field
          RoadData roadData = this.m_RoadData[prefabRef.m_Prefab];
          roadComposition.m_ZoneBlockPrefab = roadData.m_ZoneBlockPrefab;
          roadComposition.m_Flags = roadData.m_Flags;
          roadComposition.m_SpeedLimit = roadData.m_SpeedLimit;
          roadComposition.m_Priority = roadData.m_SpeedLimit;
          if ((netCompositionData.m_State & CompositionState.SeparatedCarriageways) != (CompositionState) 0)
            roadComposition.m_Flags |= RoadFlags.SeparatedCarriageways;
          if ((netCompositionData.m_Flags.m_General & CompositionFlags.General.Gravel) != (CompositionFlags.General) 0)
            roadComposition.m_Priority -= 1.25f;
          if (bufferAccessor2.Length != 0)
          {
            DynamicBuffer<NetCompositionLane> dynamicBuffer = bufferAccessor2[index1];
            int x = 0;
            int y = 0;
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              NetCompositionLane netCompositionLane = dynamicBuffer[index2];
              if ((netCompositionLane.m_Flags & (LaneFlags.Master | LaneFlags.Road)) == LaneFlags.Road)
              {
                if ((netCompositionLane.m_Flags & LaneFlags.Invert) != (LaneFlags) 0)
                  ++y;
                else
                  ++x;
              }
            }
            if ((roadData.m_Flags & RoadFlags.UseHighwayRules) != (RoadFlags) 0)
              roadComposition.m_Priority += (float) math.max(x, y);
            else
              roadComposition.m_Priority += (float) ((double) math.max(x, y) * 0.5 + (double) (x + y) * 0.25);
          }
          if (bufferAccessor3.Length != 0)
          {
            DynamicBuffer<NetCompositionObject> dynamicBuffer = bufferAccessor3[index1];
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_StreetLightData.HasComponent(dynamicBuffer[index3].m_Prefab))
              {
                roadComposition.m_Flags |= RoadFlags.HasStreetLights;
                break;
              }
            }
          }
          nativeArray4[index1] = roadComposition;
        }
        for (int index = 0; index < nativeArray5.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray2[index];
          TrackComposition trackComposition = nativeArray5[index];
          // ISSUE: reference to a compiler-generated field
          TrackData trackData = this.m_TrackData[prefabRef.m_Prefab];
          trackComposition.m_TrackType = trackData.m_TrackType;
          trackComposition.m_SpeedLimit = trackData.m_SpeedLimit;
          nativeArray5[index] = trackComposition;
        }
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray2[index];
          WaterwayComposition waterwayComposition = nativeArray6[index];
          // ISSUE: reference to a compiler-generated field
          WaterwayData waterwayData = this.m_WaterwayData[prefabRef.m_Prefab];
          waterwayComposition.m_SpeedLimit = waterwayData.m_SpeedLimit;
          nativeArray6[index] = waterwayComposition;
        }
        for (int index = 0; index < nativeArray7.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray2[index];
          PathwayComposition pathwayComposition = nativeArray7[index];
          // ISSUE: reference to a compiler-generated field
          PathwayData pathwayData = this.m_PathwayData[prefabRef.m_Prefab];
          pathwayComposition.m_SpeedLimit = pathwayData.m_SpeedLimit;
          nativeArray7[index] = pathwayComposition;
        }
        for (int index = 0; index < nativeArray8.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray2[index];
          TaxiwayComposition taxiwayComposition = nativeArray8[index];
          NetCompositionData netCompositionData = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          TaxiwayData taxiwayData = this.m_TaxiwayData[prefabRef.m_Prefab];
          taxiwayComposition.m_SpeedLimit = taxiwayData.m_SpeedLimit;
          taxiwayComposition.m_Flags = taxiwayData.m_Flags;
          if ((taxiwayData.m_Flags & TaxiwayFlags.Airspace) != (TaxiwayFlags) 0)
          {
            netCompositionData.m_State &= ~CompositionState.NoSubCollisions;
            netCompositionData.m_State |= CompositionState.Airspace;
          }
          nativeArray1[index] = netCompositionData;
          nativeArray8[index] = taxiwayComposition;
        }
        for (int index4 = 0; index4 < nativeArray9.Length; ++index4)
        {
          TerrainComposition terrainComposition = nativeArray9[index4];
          NetCompositionData netCompositionData = nativeArray1[index4];
          DynamicBuffer<NetCompositionPiece> dynamicBuffer = bufferAccessor1[index4];
          terrainComposition.m_ClipHeightOffset = new float2(float.MaxValue, float.MinValue);
          terrainComposition.m_MinHeightOffset = (float3) float.MaxValue;
          terrainComposition.m_MaxHeightOffset = (float3) float.MinValue;
          for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
          {
            NetCompositionPiece compositionPiece = dynamicBuffer[index5];
            NetTerrainData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TerrainData.TryGetComponent(compositionPiece.m_Piece, out componentData))
            {
              float2 float2 = netCompositionData.m_Width * 0.5f + new float2(compositionPiece.m_Offset.x, -compositionPiece.m_Offset.x) - compositionPiece.m_Size.x * 0.5f;
              if ((compositionPiece.m_SectionFlags & NetSectionFlags.Invert) != 0)
                componentData.m_WidthOffset = componentData.m_WidthOffset.yx;
              if ((double) componentData.m_WidthOffset.x != 0.0)
                terrainComposition.m_WidthOffset.x = math.max(terrainComposition.m_WidthOffset.x, float2.x + componentData.m_WidthOffset.x);
              if ((double) componentData.m_WidthOffset.y != 0.0)
                terrainComposition.m_WidthOffset.y = math.max(terrainComposition.m_WidthOffset.y, float2.y + componentData.m_WidthOffset.y);
              terrainComposition.m_ClipHeightOffset.x = math.min(terrainComposition.m_ClipHeightOffset.x, componentData.m_ClipHeightOffset.x);
              terrainComposition.m_ClipHeightOffset.y = math.max(terrainComposition.m_ClipHeightOffset.y, componentData.m_ClipHeightOffset.y);
              terrainComposition.m_MinHeightOffset = math.min(terrainComposition.m_MinHeightOffset, componentData.m_MinHeightOffset);
              terrainComposition.m_MaxHeightOffset = math.max(terrainComposition.m_MaxHeightOffset, componentData.m_MaxHeightOffset);
            }
          }
          terrainComposition.m_ClipHeightOffset = math.select(terrainComposition.m_ClipHeightOffset, (float2) 0.0f, terrainComposition.m_ClipHeightOffset == new float2(float.MaxValue, float.MinValue));
          terrainComposition.m_MinHeightOffset = math.select(terrainComposition.m_MinHeightOffset, (float3) 0.0f, terrainComposition.m_MinHeightOffset == float.MaxValue);
          terrainComposition.m_MaxHeightOffset = math.select(terrainComposition.m_MaxHeightOffset, (float3) 0.0f, terrainComposition.m_MaxHeightOffset == float.MinValue);
          nativeArray9[index4] = terrainComposition;
        }
      }

      private void AddCompositionAreas(
        Entity entity,
        NetCompositionData compositionData,
        DynamicBuffer<NetCompositionPiece> pieces,
        DynamicBuffer<NetCompositionArea> netAreas,
        bool isBridge)
      {
        for (int index = 0; index < pieces.Length; ++index)
        {
          NetCompositionPiece piece = pieces[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetPieceAreas.HasBuffer(piece.m_Piece))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetPieceArea> netPieceArea1 = this.m_NetPieceAreas[piece.m_Piece];
            bool c = (piece.m_SectionFlags & NetSectionFlags.Invert) != 0;
            for (int a = 0; a < netPieceArea1.Length; ++a)
            {
              NetPieceArea netPieceArea2 = netPieceArea1[math.select(a, netPieceArea1.Length - a - 1, c)];
              if (!isBridge || (netPieceArea2.m_Flags & NetAreaFlags.NoBridge) == (NetAreaFlags) 0)
              {
                if (c)
                {
                  netPieceArea2.m_Position.x = -netPieceArea2.m_Position.x;
                  netPieceArea2.m_SnapPosition.x = -netPieceArea2.m_SnapPosition.x;
                  netPieceArea2.m_Flags |= NetAreaFlags.Invert;
                }
                netAreas.Add(new NetCompositionArea()
                {
                  m_Flags = netPieceArea2.m_Flags,
                  m_Position = piece.m_Offset + netPieceArea2.m_Position,
                  m_SnapPosition = piece.m_Offset + netPieceArea2.m_SnapPosition,
                  m_Width = netPieceArea2.m_Width,
                  m_SnapWidth = netPieceArea2.m_SnapWidth
                });
              }
            }
          }
        }
      }

      public static void AddCompositionObjects(
        Entity entity,
        NetCompositionData compositionData,
        DynamicBuffer<NetCompositionPiece> pieces,
        DynamicBuffer<NetCompositionObject> objects,
        ComponentLookup<StreetLightData> streetLightDatas,
        ComponentLookup<LaneDirectionData> laneDirectionDatas,
        ComponentLookup<TrafficSignData> trafficSignDatas,
        ComponentLookup<UtilityObjectData> utilityObjectDatas,
        BufferLookup<NetPieceObject> netPieceObjects)
      {
        bool flag1 = (compositionData.m_Flags.m_General & CompositionFlags.General.Edge) > (CompositionFlags.General) 0;
        bool flag2 = (compositionData.m_Flags.m_General & CompositionFlags.General.Invert) > (CompositionFlags.General) 0;
        for (int index1 = 0; index1 < pieces.Length; ++index1)
        {
          NetCompositionPiece piece = pieces[index1];
          if (netPieceObjects.HasBuffer(piece.m_Piece))
          {
            DynamicBuffer<NetPieceObject> netPieceObject = netPieceObjects[piece.m_Piece];
            bool c = (piece.m_SectionFlags & NetSectionFlags.Invert) != 0;
            bool flag3 = (piece.m_SectionFlags & NetSectionFlags.FlipLanes) != 0;
            bool flag4 = (piece.m_SectionFlags & NetSectionFlags.Median) != 0;
            bool flag5 = (piece.m_PieceFlags & NetPieceFlags.PreserveShape) != 0;
            bool flag6 = flag1 & flag3;
            CompositionFlags compositionFlags;
            NetSectionFlags sectionFlags;
            if (c)
            {
              compositionFlags = NetCompositionHelpers.InvertCompositionFlags(compositionData.m_Flags);
              sectionFlags = NetCompositionHelpers.InvertSectionFlags(piece.m_SectionFlags);
            }
            else
            {
              compositionFlags = compositionData.m_Flags;
              sectionFlags = piece.m_SectionFlags;
            }
label_41:
            for (int a = 0; a < netPieceObject.Length; ++a)
            {
              NetPieceObject _object = netPieceObject[math.select(a, netPieceObject.Length - a - 1, c)];
              if (NetCompositionHelpers.TestObjectFlags(_object, compositionFlags, sectionFlags))
              {
                NetCompositionObject elem = new NetCompositionObject();
                bool flag7 = false;
                if (c)
                {
                  flag7 ^= (_object.m_Flags & SubObjectFlags.FlipInverted) != 0;
                  _object.m_Position.x = -_object.m_Position.x;
                }
                if (flag6)
                  flag7 = ((flag7 ? 1 : 0) ^ (laneDirectionDatas.HasComponent(_object.m_Prefab) ? 1 : (trafficSignDatas.HasComponent(_object.m_Prefab) ? 1 : 0))) != 0;
                if (flag7)
                {
                  _object.m_Rotation = math.mul(quaternion.RotateY(3.14159274f), _object.m_Rotation);
                  _object.m_CurveOffsetRange = 1f - _object.m_CurveOffsetRange;
                  if ((compositionData.m_Flags.m_General & CompositionFlags.General.Edge) != (CompositionFlags.General) 0)
                    _object.m_Position.z = -_object.m_Position.z;
                }
                float3 float3 = piece.m_Offset + _object.m_Position;
                elem.m_Prefab = _object.m_Prefab;
                elem.m_Position = float3.xz;
                elem.m_Offset = math.rotate(_object.m_Rotation, _object.m_Offset);
                elem.m_Offset.y += float3.y;
                elem.m_Rotation = _object.m_Rotation;
                elem.m_Flags = _object.m_Flags;
                elem.m_SpacingIgnore = _object.m_CompositionNone.m_General;
                elem.m_UseCurveRotation = _object.m_UseCurveRotation;
                elem.m_Probability = _object.m_Probability;
                elem.m_CurveOffsetRange = _object.m_CurveOffsetRange;
                elem.m_Spacing = _object.m_Spacing.z;
                elem.m_MinLength = _object.m_MinLength;
                if ((double) _object.m_Spacing.z > 0.10000000149011612)
                  elem.m_Flags |= SubObjectFlags.AllowCombine;
                if (flag4)
                  elem.m_Flags |= SubObjectFlags.OnMedian;
                if (flag5)
                  elem.m_Flags |= SubObjectFlags.PreserveShape;
                if ((double) _object.m_Spacing.x > 0.10000000149011612)
                {
                  StreetLightData streetLightData1 = new StreetLightData();
                  bool flag8 = false;
                  bool flag9 = utilityObjectDatas.HasComponent(_object.m_Prefab);
                  if (streetLightDatas.HasComponent(_object.m_Prefab))
                  {
                    streetLightData1 = streetLightDatas[_object.m_Prefab];
                    flag8 = true;
                  }
                  if (!flag8 || (compositionData.m_Flags.m_General & CompositionFlags.General.Intersection) == (CompositionFlags.General) 0)
                  {
                    for (int index2 = 0; index2 < objects.Length; ++index2)
                    {
                      NetCompositionObject compositionObject = objects[index2];
                      double num1 = (double) math.abs(elem.m_Position.x - compositionObject.m_Position.x);
                      bool flag10 = ((elem.m_Flags ^ compositionObject.m_Flags) & SubObjectFlags.SpacingOverride) != 0;
                      double x = (double) _object.m_Spacing.x;
                      if (num1 < x || flag10)
                      {
                        if (compositionObject.m_Prefab != elem.m_Prefab)
                        {
                          if (flag8 && streetLightDatas.HasComponent(compositionObject.m_Prefab))
                          {
                            StreetLightData streetLightData2 = streetLightDatas[compositionObject.m_Prefab];
                            if (streetLightData1.m_Layer != streetLightData2.m_Layer)
                              continue;
                          }
                          else
                          {
                            if (!flag8 && !flag9 && (double) elem.m_Spacing > 0.10000000149011612 && (double) compositionObject.m_Spacing > 0.10000000149011612 && ((elem.m_Flags ^ compositionObject.m_Flags) & SubObjectFlags.EvenSpacing) == (SubObjectFlags) 0)
                            {
                              elem.m_AvoidSpacing = compositionObject.m_Spacing;
                              continue;
                            }
                            continue;
                          }
                        }
                        if (flag10)
                        {
                          if ((elem.m_Flags & SubObjectFlags.SpacingOverride) != (SubObjectFlags) 0)
                          {
                            objects.RemoveAt(index2);
                            break;
                          }
                          goto label_41;
                        }
                        else
                        {
                          float num2 = math.abs(elem.m_Position.x) - math.abs(compositionObject.m_Position.x);
                          if ((double) num2 < 4.0 && (!flag2 || (double) num2 <= -4.0))
                          {
                            objects.RemoveAt(index2);
                            break;
                          }
                          goto label_41;
                        }
                      }
                    }
                  }
                }
                objects.Add(elem);
              }
            }
          }
        }
      }

      private void AddCompositionCrosswalks(
        Entity entity,
        NetCompositionData compositionData,
        DynamicBuffer<NetCompositionPiece> pieces,
        DynamicBuffer<NetCompositionCrosswalk> crosswalks)
      {
        NetCrosswalkData netCrosswalkData1 = new NetCrosswalkData();
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        for (int index1 = 0; index1 < pieces.Length; ++index1)
        {
          NetCompositionPiece piece = pieces[index1];
          if ((piece.m_PieceFlags & NetPieceFlags.Surface) != (NetPieceFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetCrosswalkData.HasComponent(piece.m_Piece))
            {
              // ISSUE: reference to a compiler-generated field
              NetCrosswalkData netCrosswalkData2 = this.m_NetCrosswalkData[piece.m_Piece];
              if (flag3)
              {
                if (!flag1)
                {
                  // ISSUE: reference to a compiler-generated field
                  NetLaneData netLaneData = this.m_NetLaneData[netCrosswalkData1.m_Lane];
                  NetCompositionCrosswalk elem = new NetCompositionCrosswalk();
                  elem.m_Lane = netCrosswalkData1.m_Lane;
                  elem.m_Start = netCrosswalkData1.m_Start;
                  elem.m_End = netCrosswalkData1.m_End;
                  elem.m_Flags = netLaneData.m_Flags;
                  if (flag4)
                    elem.m_Flags |= LaneFlags.CrossRoad;
                  crosswalks.Add(elem);
                }
                netCrosswalkData1 = new NetCrosswalkData();
                flag1 = flag2;
                flag2 = false;
                flag3 = false;
                flag4 = false;
              }
              // ISSUE: reference to a compiler-generated field
              if (!flag4 && this.m_NetPieceLanes.HasBuffer(piece.m_Piece))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<NetPieceLane> netPieceLane1 = this.m_NetPieceLanes[piece.m_Piece];
                for (int index2 = 0; index2 < netPieceLane1.Length; ++index2)
                {
                  NetPieceLane netPieceLane2 = netPieceLane1[index2];
                  // ISSUE: reference to a compiler-generated field
                  if ((double) netPieceLane2.m_Position.x >= (double) netCrosswalkData2.m_Start.x && (double) netPieceLane2.m_Position.x <= (double) netCrosswalkData2.m_End.x && (this.m_NetLaneData[netPieceLane2.m_Lane].m_Flags & LaneFlags.Road) != (LaneFlags) 0)
                  {
                    flag4 = true;
                    break;
                  }
                }
              }
              if ((piece.m_SectionFlags & NetSectionFlags.Invert) != (NetSectionFlags) 0)
              {
                float x = netCrosswalkData2.m_Start.x;
                netCrosswalkData2.m_Start.x = -netCrosswalkData2.m_End.x;
                netCrosswalkData2.m_End.x = -x;
              }
              if (netCrosswalkData1.m_Lane == Entity.Null)
              {
                netCrosswalkData1.m_Lane = netCrosswalkData2.m_Lane;
                netCrosswalkData1.m_Start = piece.m_Offset + netCrosswalkData2.m_Start;
                netCrosswalkData1.m_End = piece.m_Offset + netCrosswalkData2.m_End;
              }
              else
                netCrosswalkData1.m_End = piece.m_Offset + netCrosswalkData2.m_End;
            }
            else
            {
              if ((double) piece.m_Size.x > 0.0)
              {
                flag2 = false;
                if (netCrosswalkData1.m_Lane != Entity.Null)
                  flag3 = true;
              }
              if ((piece.m_PieceFlags & NetPieceFlags.BlockCrosswalk) != (NetPieceFlags) 0)
              {
                flag1 = true;
                flag2 = true;
              }
            }
          }
        }
        if (!(netCrosswalkData1.m_Lane != Entity.Null) || flag1)
          return;
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData1 = this.m_NetLaneData[netCrosswalkData1.m_Lane];
        NetCompositionCrosswalk elem1 = new NetCompositionCrosswalk();
        elem1.m_Lane = netCrosswalkData1.m_Lane;
        elem1.m_Start = netCrosswalkData1.m_Start;
        elem1.m_End = netCrosswalkData1.m_End;
        elem1.m_Flags = netLaneData1.m_Flags;
        if (flag4)
          elem1.m_Flags |= LaneFlags.CrossRoad;
        crosswalks.Add(elem1);
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<NetCompositionData> __Game_Prefabs_NetCompositionData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PlaceableNetComposition> __Game_Prefabs_PlaceableNetComposition_RW_ComponentTypeHandle;
      public ComponentTypeHandle<RoadComposition> __Game_Prefabs_RoadComposition_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TrackComposition> __Game_Prefabs_TrackComposition_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WaterwayComposition> __Game_Prefabs_WaterwayComposition_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathwayComposition> __Game_Prefabs_PathwayComposition_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TaxiwayComposition> __Game_Prefabs_TaxiwayComposition_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TerrainComposition> __Game_Prefabs_TerrainComposition_RW_ComponentTypeHandle;
      public BufferTypeHandle<NetCompositionPiece> __Game_Prefabs_NetCompositionPiece_RW_BufferTypeHandle;
      public BufferTypeHandle<NetCompositionLane> __Game_Prefabs_NetCompositionLane_RW_BufferTypeHandle;
      public BufferTypeHandle<NetCompositionObject> __Game_Prefabs_NetCompositionObject_RW_BufferTypeHandle;
      public BufferTypeHandle<NetCompositionArea> __Game_Prefabs_NetCompositionArea_RW_BufferTypeHandle;
      public BufferTypeHandle<NetCompositionCrosswalk> __Game_Prefabs_NetCompositionCrosswalk_RW_BufferTypeHandle;
      public BufferTypeHandle<NetCompositionCarriageway> __Game_Prefabs_NetCompositionCarriageway_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetPieceData> __Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetPieceData> __Game_Prefabs_NetPieceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCrosswalkData> __Game_Prefabs_NetCrosswalkData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetVertexMatchData> __Game_Prefabs_NetVertexMatchData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackData> __Game_Prefabs_TrackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterwayData> __Game_Prefabs_WaterwayData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathwayData> __Game_Prefabs_PathwayData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiwayData> __Game_Prefabs_TaxiwayData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StreetLightData> __Game_Prefabs_StreetLightData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneDirectionData> __Game_Prefabs_LaneDirectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficSignData> __Game_Prefabs_TrafficSignData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityObjectData> __Game_Prefabs_UtilityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetTerrainData> __Game_Prefabs_NetTerrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BridgeData> __Game_Prefabs_BridgeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<NetPieceLane> __Game_Prefabs_NetPieceLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetPieceObject> __Game_Prefabs_NetPieceObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetPieceArea> __Game_Prefabs_NetPieceArea_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetCompositionData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetComposition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PlaceableNetComposition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadComposition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RoadComposition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackComposition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TrackComposition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterwayComposition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterwayComposition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathwayComposition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathwayComposition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TaxiwayComposition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TaxiwayComposition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TerrainComposition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TerrainComposition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionPiece_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetCompositionPiece>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetCompositionLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetCompositionObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionArea_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetCompositionArea>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionCrosswalk_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetCompositionCrosswalk>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionCarriageway_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetCompositionCarriageway>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetPieceData_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetPieceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceData_RO_ComponentLookup = state.GetComponentLookup<NetPieceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCrosswalkData_RO_ComponentLookup = state.GetComponentLookup<NetCrosswalkData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetVertexMatchData_RO_ComponentLookup = state.GetComponentLookup<NetVertexMatchData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RO_ComponentLookup = state.GetComponentLookup<RoadData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackData_RO_ComponentLookup = state.GetComponentLookup<TrackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterwayData_RO_ComponentLookup = state.GetComponentLookup<WaterwayData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathwayData_RO_ComponentLookup = state.GetComponentLookup<PathwayData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TaxiwayData_RO_ComponentLookup = state.GetComponentLookup<TaxiwayData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StreetLightData_RO_ComponentLookup = state.GetComponentLookup<StreetLightData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup = state.GetComponentLookup<LaneDirectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrafficSignData_RO_ComponentLookup = state.GetComponentLookup<TrafficSignData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup = state.GetComponentLookup<UtilityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetTerrainData_RO_ComponentLookup = state.GetComponentLookup<NetTerrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BridgeData_RO_ComponentLookup = state.GetComponentLookup<BridgeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceLane_RO_BufferLookup = state.GetBufferLookup<NetPieceLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceObject_RO_BufferLookup = state.GetBufferLookup<NetPieceObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPieceArea_RO_BufferLookup = state.GetBufferLookup<NetPieceArea>(true);
      }
    }
  }
}
