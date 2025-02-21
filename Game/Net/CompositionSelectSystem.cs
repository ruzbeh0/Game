// Decompiled with JetBrains decompiler
// Type: Game.Net.CompositionSelectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.City;
using Game.Common;
using Game.Objects;
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
namespace Game.Net
{
  [CompilerGenerated]
  public class CompositionSelectSystem : GameSystemBase
  {
    private NetCompositionSystem m_NetCompositionSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ModificationBarrier3 m_ModificationBarrier;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_AllQuery;
    private bool m_Loaded;
    private CompositionSelectSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetCompositionSystem = this.World.GetOrCreateSystemManaged<NetCompositionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier3>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Composition>(),
          ComponentType.ReadWrite<Orphan>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Composition>(),
          ComponentType.ReadWrite<Orphan>()
        }
      });
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = this.GetLoaded() ? this.m_AllQuery : this.m_UpdatedQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      NativeQueue<CompositionSelectSystem.CompositionCreateInfo> nativeQueue = new NativeQueue<CompositionSelectSystem.CompositionCreateInfo>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FixedNetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryNodeState_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryEdgeState_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryComposition_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Fixed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CompositionSelectSystem.SelectCompositionJob jobData = new CompositionSelectSystem.SelectCompositionJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_UpgradedType = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle,
        m_FixedType = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RW_ComponentTypeHandle,
        m_OrphanType = this.__TypeHandle.__Game_Net_Orphan_RW_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
        m_FixedData = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabNetObjectData = this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup,
        m_PrefabRoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_PrefabGeometryCompositions = this.__TypeHandle.__Game_Prefabs_NetGeometryComposition_RO_BufferLookup,
        m_PrefabGeometryEdgeStates = this.__TypeHandle.__Game_Prefabs_NetGeometryEdgeState_RO_BufferLookup,
        m_PrefabGeometryNodeStates = this.__TypeHandle.__Game_Prefabs_NetGeometryNodeState_RO_BufferLookup,
        m_PrefabFixedNetElements = this.__TypeHandle.__Game_Prefabs_FixedNetElement_RO_BufferLookup,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_CompositionCreateQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetSectionPiece_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetSubSection_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetTerrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new CompositionSelectSystem.CreateCompositionJob()
      {
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabTerrainData = this.__TypeHandle.__Game_Prefabs_NetTerrainData_RO_ComponentLookup,
        m_SubReplacements = this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup,
        m_PrefabGeometrySections = this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RO_BufferLookup,
        m_PrefabSubSections = this.__TypeHandle.__Game_Prefabs_NetSubSection_RO_BufferLookup,
        m_PrefabSectionPieces = this.__TypeHandle.__Game_Prefabs_NetSectionPiece_RO_BufferLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RW_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RW_ComponentLookup,
        m_CompositionCreateQueue = nativeQueue,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<CompositionSelectSystem.CreateCompositionJob>(jobData.ScheduleParallel<CompositionSelectSystem.SelectCompositionJob>(query, this.Dependency));
      nativeQueue.Dispose(jobHandle);
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
    public CompositionSelectSystem()
    {
    }

    private struct CompositionCreateInfo
    {
      public Entity m_Entity;
      public Entity m_Prefab;
      public CompositionFlags m_EdgeFlags;
      public CompositionFlags m_StartFlags;
      public CompositionFlags m_EndFlags;
      public CompositionFlags m_ObsoleteEdgeFlags;
      public CompositionFlags m_ObsoleteStartFlags;
      public CompositionFlags m_ObsoleteEndFlags;
      public Composition m_CompositionData;
    }

    [BurstCompile]
    private struct SelectCompositionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> m_UpgradedType;
      [ReadOnly]
      public ComponentTypeHandle<Fixed> m_FixedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Composition> m_CompositionType;
      public ComponentTypeHandle<Orphan> m_OrphanType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<Fixed> m_FixedData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetObjectData> m_PrefabNetObjectData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_PrefabRoadData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<NetGeometryComposition> m_PrefabGeometryCompositions;
      [ReadOnly]
      public BufferLookup<NetGeometryEdgeState> m_PrefabGeometryEdgeStates;
      [ReadOnly]
      public BufferLookup<NetGeometryNodeState> m_PrefabGeometryNodeStates;
      [ReadOnly]
      public BufferLookup<FixedNetElement> m_PrefabFixedNetElements;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      public NativeQueue<CompositionSelectSystem.CompositionCreateInfo>.ParallelWriter m_CompositionCreateQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray2 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Upgraded> nativeArray4 = chunk.GetNativeArray<Upgraded>(ref this.m_UpgradedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Fixed> nativeArray5 = chunk.GetNativeArray<Fixed>(ref this.m_FixedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Composition> nativeArray7 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Edge edge = nativeArray2[index];
            Curve curve = nativeArray3[index];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CompositionSelectSystem.CompositionCreateInfo compositionCreateInfo = new CompositionSelectSystem.CompositionCreateInfo();
            // ISSUE: reference to a compiler-generated field
            compositionCreateInfo.m_Entity = entity;
            // ISSUE: reference to a compiler-generated field
            compositionCreateInfo.m_Prefab = nativeArray6[index].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData prefabGeometryData = this.m_PrefabGeometryData[compositionCreateInfo.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetGeometryComposition> geometryComposition = this.m_PrefabGeometryCompositions[compositionCreateInfo.m_Prefab];
            CompositionFlags upgradeFlags = new CompositionFlags();
            // ISSUE: reference to a compiler-generated method
            CompositionFlags subObjectFlags = this.GetSubObjectFlags(entity, curve, prefabGeometryData);
            // ISSUE: reference to a compiler-generated method
            CompositionFlags elevationFlags = this.GetElevationFlags(entity, edge.m_Start, edge.m_End, prefabGeometryData);
            if (nativeArray4.Length != 0)
              upgradeFlags = nativeArray4[index].m_Flags;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (nativeArray5.Length != 0 && this.m_PrefabFixedNetElements.HasBuffer(compositionCreateInfo.m_Prefab))
            {
              Fixed @fixed = nativeArray5[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<FixedNetElement> prefabFixedNetElement = this.m_PrefabFixedNetElements[compositionCreateInfo.m_Prefab];
              if (@fixed.m_Index >= 0 && @fixed.m_Index < prefabFixedNetElement.Length)
              {
                FixedNetElement fixedNetElement = prefabFixedNetElement[@fixed.m_Index];
                upgradeFlags |= fixedNetElement.m_SetState;
                elevationFlags &= ~fixedNetElement.m_UnsetState;
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_EdgeFlags = this.GetEdgeFlags(compositionCreateInfo.m_Entity, compositionCreateInfo.m_Prefab, prefabGeometryData, upgradeFlags, subObjectFlags, elevationFlags, out compositionCreateInfo.m_ObsoleteEdgeFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_StartFlags = this.GetNodeFlags(compositionCreateInfo.m_Entity, edge.m_Start, compositionCreateInfo.m_Prefab, prefabGeometryData, compositionCreateInfo.m_EdgeFlags, curve, true, out compositionCreateInfo.m_ObsoleteStartFlags, ref compositionCreateInfo.m_ObsoleteEdgeFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_EndFlags = this.GetNodeFlags(compositionCreateInfo.m_Entity, edge.m_End, compositionCreateInfo.m_Prefab, prefabGeometryData, compositionCreateInfo.m_EdgeFlags, curve, false, out compositionCreateInfo.m_ObsoleteEndFlags, ref compositionCreateInfo.m_ObsoleteEdgeFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_CompositionData.m_Edge = this.FindComposition(geometryComposition, compositionCreateInfo.m_EdgeFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_CompositionData.m_StartNode = this.FindComposition(geometryComposition, compositionCreateInfo.m_StartFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_CompositionData.m_EndNode = this.FindComposition(geometryComposition, compositionCreateInfo.m_EndFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (compositionCreateInfo.m_CompositionData.m_Edge == Entity.Null || compositionCreateInfo.m_ObsoleteEdgeFlags != new CompositionFlags() || compositionCreateInfo.m_CompositionData.m_StartNode == Entity.Null || compositionCreateInfo.m_ObsoleteStartFlags != new CompositionFlags() || compositionCreateInfo.m_CompositionData.m_EndNode == Entity.Null || compositionCreateInfo.m_ObsoleteEndFlags != new CompositionFlags())
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CompositionCreateQueue.Enqueue(compositionCreateInfo);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              nativeArray7[index] = compositionCreateInfo.m_CompositionData;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray8 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Orphan> nativeArray9 = chunk.GetNativeArray<Orphan>(ref this.m_OrphanType);
          for (int index = 0; index < nativeArray9.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CompositionSelectSystem.CompositionCreateInfo compositionCreateInfo = new CompositionSelectSystem.CompositionCreateInfo();
            // ISSUE: reference to a compiler-generated field
            compositionCreateInfo.m_Entity = entity;
            // ISSUE: reference to a compiler-generated field
            compositionCreateInfo.m_Prefab = nativeArray8[index].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData prefabGeometryData = this.m_PrefabGeometryData[compositionCreateInfo.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetGeometryComposition> geometryComposition = this.m_PrefabGeometryCompositions[compositionCreateInfo.m_Prefab];
            // ISSUE: reference to a compiler-generated method
            CompositionFlags elevationFlags = this.GetElevationFlags(Entity.Null, entity, entity, prefabGeometryData);
            CompositionFlags obsoleteEdgeFlags = new CompositionFlags();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_EdgeFlags = this.GetNodeFlags(Entity.Null, compositionCreateInfo.m_Entity, compositionCreateInfo.m_Prefab, prefabGeometryData, elevationFlags, new Curve(), true, out compositionCreateInfo.m_ObsoleteEdgeFlags, ref obsoleteEdgeFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_CompositionData.m_Edge = this.FindComposition(geometryComposition, compositionCreateInfo.m_EdgeFlags);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (compositionCreateInfo.m_CompositionData.m_Edge == Entity.Null || compositionCreateInfo.m_ObsoleteEdgeFlags != new CompositionFlags())
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CompositionCreateQueue.Enqueue(compositionCreateInfo);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              nativeArray9[index] = new Orphan()
              {
                m_Composition = compositionCreateInfo.m_CompositionData.m_Edge
              };
            }
          }
        }
      }

      private Entity FindComposition(
        DynamicBuffer<NetGeometryComposition> geometryCompositions,
        CompositionFlags flags)
      {
        for (int index = 0; index < geometryCompositions.Length; ++index)
        {
          NetGeometryComposition geometryComposition = geometryCompositions[index];
          if (geometryComposition.m_Mask == flags)
            return geometryComposition.m_Composition;
        }
        return Entity.Null;
      }

      private CompositionFlags GetSubObjectFlags(
        Entity entity,
        Curve curve,
        NetGeometryData prefabGeometryData)
      {
        CompositionFlags subObjectFlags = new CompositionFlags();
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[entity];
          for (int index = 0; index < subObject1.Length; ++index)
          {
            Entity subObject2 = subObject1[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subObject2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabNetObjectData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              NetObjectData netObjectData = this.m_PrefabNetObjectData[prefabRef.m_Prefab];
              netObjectData.m_CompositionFlags &= ~CompositionFlags.nodeMask;
              // ISSUE: reference to a compiler-generated field
              if (this.m_AttachedData.HasComponent(subObject2))
              {
                // ISSUE: reference to a compiler-generated field
                Attached attached = this.m_AttachedData[subObject2];
                if (!(attached.m_Parent != entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  Transform transform = this.m_TransformData[subObject2];
                  float3 float3 = MathUtils.Position(curve.m_Bezier, attached.m_CurvePosition);
                  float x = math.dot(MathUtils.Left(math.normalizesafe(MathUtils.Tangent(curve.m_Bezier, attached.m_CurvePosition)).xz), transform.m_Position.xz - float3.xz);
                  if ((double) x > 0.0)
                    netObjectData.m_CompositionFlags = NetCompositionHelpers.InvertCompositionFlags(netObjectData.m_CompositionFlags);
                  if ((netObjectData.m_CompositionFlags & ~CompositionFlags.optionMask) == new CompositionFlags() && netObjectData.m_CompositionFlags.m_General != (CompositionFlags.General) 0 && (netObjectData.m_CompositionFlags.m_Left != (CompositionFlags.Side) 0 || netObjectData.m_CompositionFlags.m_Right != (CompositionFlags.Side) 0))
                  {
                    if ((double) math.abs(x) < (double) prefabGeometryData.m_DefaultWidth * 0.1666666716337204)
                    {
                      netObjectData.m_CompositionFlags.m_Left = (CompositionFlags.Side) 0;
                      netObjectData.m_CompositionFlags.m_Right = (CompositionFlags.Side) 0;
                    }
                    else
                      netObjectData.m_CompositionFlags.m_General = (CompositionFlags.General) 0;
                  }
                }
                else
                  continue;
              }
              subObjectFlags |= netObjectData.m_CompositionFlags;
            }
          }
        }
        return subObjectFlags;
      }

      private CompositionFlags GetSubObjectFlags(Entity entity)
      {
        CompositionFlags subObjectFlags = new CompositionFlags();
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[entity];
          for (int index = 0; index < subObject1.Length; ++index)
          {
            Entity subObject2 = subObject1[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subObject2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabNetObjectData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              NetObjectData netObjectData = this.m_PrefabNetObjectData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_AttachedData.HasComponent(subObject2) || !(this.m_AttachedData[subObject2].m_Parent != entity))
                subObjectFlags |= netObjectData.m_CompositionFlags & CompositionFlags.nodeMask;
            }
          }
        }
        return subObjectFlags;
      }

      private CompositionFlags GetElevationFlags(
        Entity edge,
        Entity startNode,
        Entity endNode,
        NetGeometryData prefabGeometryData)
      {
        Elevation startElevation = new Elevation();
        Elevation middleElevation = new Elevation();
        Elevation endElevation = new Elevation();
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.HasComponent(startNode))
        {
          // ISSUE: reference to a compiler-generated field
          startElevation = this.m_ElevationData[startNode];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.HasComponent(endNode))
        {
          // ISSUE: reference to a compiler-generated field
          endElevation = this.m_ElevationData[endNode];
        }
        if (edge == Entity.Null)
        {
          middleElevation = startElevation;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.HasComponent(edge))
          {
            // ISSUE: reference to a compiler-generated field
            middleElevation = this.m_ElevationData[edge];
          }
        }
        return NetCompositionHelpers.GetElevationFlags(startElevation, middleElevation, endElevation, prefabGeometryData);
      }

      private CompositionFlags GetEdgeFlags(
        Entity edge,
        Entity prefab,
        NetGeometryData prefabGeometryData,
        CompositionFlags upgradeFlags,
        CompositionFlags subObjectFlags,
        CompositionFlags elevationFlags,
        out CompositionFlags obsoleteEdgeFlags)
      {
        CompositionFlags compositionFlags = upgradeFlags | subObjectFlags | elevationFlags;
        obsoleteEdgeFlags = new CompositionFlags();
        compositionFlags.m_General |= CompositionFlags.General.Edge;
        // ISSUE: reference to a compiler-generated method
        CompositionFlags edgeFlags1 = compositionFlags | this.GetHandednessFlags(prefabGeometryData);
        // ISSUE: reference to a compiler-generated method
        CompositionFlags edgeFlags2 = edgeFlags1 | this.GetEdgeStates(prefab, edgeFlags1);
        int num = (edgeFlags2.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) > (CompositionFlags.General) 0 ? 1 : 0;
        bool flag1 = (edgeFlags2.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) > (CompositionFlags.Side) 0;
        bool flag2 = (edgeFlags2.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) > (CompositionFlags.Side) 0;
        if (num != 0)
        {
          CompositionFlags.General general = upgradeFlags.m_General & (CompositionFlags.General.PrimaryMiddleBeautification | CompositionFlags.General.SecondaryMiddleBeautification);
          edgeFlags2.m_General &= ~general;
          obsoleteEdgeFlags.m_General |= general;
        }
        if ((num | (flag1 ? 1 : 0)) != 0)
        {
          CompositionFlags.Side side = upgradeFlags.m_Left & (CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk | CompositionFlags.Side.SoundBarrier);
          edgeFlags2.m_Left &= ~side;
          obsoleteEdgeFlags.m_Left |= side;
        }
        if ((num | (flag2 ? 1 : 0)) != 0)
        {
          CompositionFlags.Side side = upgradeFlags.m_Right & (CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk | CompositionFlags.Side.SoundBarrier);
          edgeFlags2.m_Right &= ~side;
          obsoleteEdgeFlags.m_Right |= side;
        }
        return edgeFlags2;
      }

      private CompositionFlags GetEdgeStates(Entity prefab, CompositionFlags edgeFlags)
      {
        CompositionFlags edgeStates = new CompositionFlags();
        DynamicBuffer<NetGeometryEdgeState> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabGeometryEdgeStates.TryGetBuffer(prefab, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            NetGeometryEdgeState edgeState = bufferData[index];
            if (NetCompositionHelpers.TestEdgeFlags(edgeState, edgeFlags))
              edgeStates |= edgeState.m_State;
          }
        }
        return edgeStates;
      }

      private CompositionFlags GetNodeStates(
        DynamicBuffer<NetGeometryNodeState> nodeStates,
        CompositionFlags edgeFlags1,
        CompositionFlags edgeFlags2,
        bool isLeft,
        bool isRight)
      {
        CompositionFlags nodeStates1 = new CompositionFlags();
        for (int index = 0; index < nodeStates.Length; ++index)
        {
          NetGeometryNodeState nodeState1 = nodeStates[index];
          NetGeometryNodeState nodeState2 = nodeState1;
          CompositionFlags compositionFlags1 = edgeFlags1;
          CompositionFlags compositionFlags2 = edgeFlags2;
          if (!isLeft)
          {
            nodeState2.m_CompositionNone.m_Left = (CompositionFlags.Side) 0;
            nodeState2.m_State.m_Left = (CompositionFlags.Side) 0;
            compositionFlags2.m_Right = (CompositionFlags.Side) 0;
            if (nodeState2.m_MatchType == NetEdgeMatchType.Exclusive)
              compositionFlags1.m_Left = (CompositionFlags.Side) 0;
          }
          if (!isRight)
          {
            nodeState2.m_CompositionNone.m_Right = (CompositionFlags.Side) 0;
            nodeState2.m_State.m_Right = (CompositionFlags.Side) 0;
            compositionFlags2.m_Left = (CompositionFlags.Side) 0;
            if (nodeState2.m_MatchType == NetEdgeMatchType.Exclusive)
              compositionFlags1.m_Right = (CompositionFlags.Side) 0;
          }
          bool2 match = new bool2(NetCompositionHelpers.TestEdgeFlags(nodeState1, compositionFlags1), NetCompositionHelpers.TestEdgeFlags(nodeState2, compositionFlags2));
          if (NetCompositionHelpers.TestEdgeMatch(nodeState2, match))
            nodeStates1 |= nodeState2.m_State;
        }
        return nodeStates1;
      }

      private CompositionFlags GetEdgeFlags(
        Entity edge,
        bool invert,
        Entity prefab,
        NetGeometryData prefabGeometryData,
        CompositionFlags elevationFlags)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[edge];
        CompositionFlags compositionFlags = new CompositionFlags();
        // ISSUE: reference to a compiler-generated method
        CompositionFlags subObjectFlags = this.GetSubObjectFlags(edge, curve, prefabGeometryData);
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpgradedData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          compositionFlags = this.m_UpgradedData[edge].m_Flags;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_FixedData.HasComponent(edge) && this.m_PrefabFixedNetElements.HasBuffer(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          Fixed @fixed = this.m_FixedData[edge];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<FixedNetElement> prefabFixedNetElement = this.m_PrefabFixedNetElements[prefab];
          if (@fixed.m_Index >= 0 && @fixed.m_Index < prefabFixedNetElement.Length)
          {
            FixedNetElement fixedNetElement = prefabFixedNetElement[@fixed.m_Index];
            compositionFlags |= fixedNetElement.m_SetState;
            elevationFlags &= ~fixedNetElement.m_UnsetState;
          }
        }
        CompositionFlags edgeFlags = compositionFlags | subObjectFlags | elevationFlags;
        edgeFlags.m_General |= CompositionFlags.General.Edge;
        // ISSUE: reference to a compiler-generated method
        edgeFlags |= this.GetHandednessFlags(prefabGeometryData);
        // ISSUE: reference to a compiler-generated method
        edgeFlags |= this.GetEdgeStates(prefab, edgeFlags);
        if (invert)
          edgeFlags = NetCompositionHelpers.InvertCompositionFlags(edgeFlags);
        return edgeFlags;
      }

      private CompositionFlags GetHandednessFlags(NetGeometryData prefabGeometryData)
      {
        CompositionFlags handednessFlags = new CompositionFlags();
        // ISSUE: reference to a compiler-generated field
        if ((prefabGeometryData.m_Flags & GeometryFlags.IsLefthanded) != 0 != this.m_LeftHandTraffic)
        {
          if ((prefabGeometryData.m_Flags & GeometryFlags.InvertCompositionHandedness) != (GeometryFlags) 0)
            handednessFlags.m_General |= CompositionFlags.General.Invert;
          if ((prefabGeometryData.m_Flags & GeometryFlags.FlipCompositionHandedness) != (GeometryFlags) 0)
            handednessFlags.m_General |= CompositionFlags.General.Flip;
        }
        return handednessFlags;
      }

      private CompositionFlags GetNodeFlags(
        Entity edge,
        Entity node,
        Entity prefab,
        NetGeometryData prefabGeometryData,
        CompositionFlags edgeFlags,
        Curve curve,
        bool isStart,
        out CompositionFlags obsoleteNodeFlags,
        ref CompositionFlags obsoleteEdgeFlags)
      {
        CompositionFlags compositionFlags1 = edgeFlags;
        edgeFlags.m_General &= CompositionFlags.General.Lighting | CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel | CompositionFlags.General.MiddlePlatform | CompositionFlags.General.WideMedian | CompositionFlags.General.PrimaryMiddleBeautification | CompositionFlags.General.SecondaryMiddleBeautification;
        edgeFlags.m_Left &= CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered | CompositionFlags.Side.PrimaryTrack | CompositionFlags.Side.SecondaryTrack | CompositionFlags.Side.TertiaryTrack | CompositionFlags.Side.QuaternaryTrack | CompositionFlags.Side.PrimaryStop | CompositionFlags.Side.SecondaryStop | CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk | CompositionFlags.Side.ParkingSpaces | CompositionFlags.Side.SoundBarrier | CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.ForbidStraight;
        edgeFlags.m_Right &= CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered | CompositionFlags.Side.PrimaryTrack | CompositionFlags.Side.SecondaryTrack | CompositionFlags.Side.TertiaryTrack | CompositionFlags.Side.QuaternaryTrack | CompositionFlags.Side.PrimaryStop | CompositionFlags.Side.SecondaryStop | CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk | CompositionFlags.Side.ParkingSpaces | CompositionFlags.Side.SoundBarrier | CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.ForbidStraight;
        // ISSUE: reference to a compiler-generated method
        CompositionFlags handednessFlags = this.GetHandednessFlags(prefabGeometryData);
        obsoleteNodeFlags = new CompositionFlags();
        handednessFlags.m_General |= CompositionFlags.General.Node;
        if (isStart)
        {
          handednessFlags.m_General ^= CompositionFlags.General.Invert;
          edgeFlags = NetCompositionHelpers.InvertCompositionFlags(edgeFlags);
        }
        // ISSUE: reference to a compiler-generated method
        CompositionFlags nodeFlags = handednessFlags | this.GetSubObjectFlags(node) | edgeFlags;
        if ((prefabGeometryData.m_Flags & GeometryFlags.SupportRoundabout) == (GeometryFlags) 0)
          nodeFlags.m_General &= ~CompositionFlags.General.Roundabout;
        int num1 = 0;
        int3 int3 = new int3();
        bool flag1 = true;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        bool flag6 = false;
        bool flag7 = false;
        bool flag8 = false;
        DynamicBuffer<NetGeometryNodeState> bufferData1;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabGeometryNodeStates.TryGetBuffer(prefab, out bufferData1);
        if (!isStart)
          curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
        Entity leftEdge;
        Entity rightEdge;
        // ISSUE: reference to a compiler-generated method
        this.FindFriendEdges(edge, node, prefabGeometryData.m_MergeLayers, curve, out leftEdge, out rightEdge);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge, node, this.m_ConnectedEdges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[edgeIteratorValue.m_Edge];
          // ISSUE: reference to a compiler-generated field
          NetGeometryData prefabGeometryData1 = this.m_PrefabGeometryData[prefabRef.m_Prefab];
          DynamicBuffer<NetGeometryNodeState> bufferData2;
          // ISSUE: reference to a compiler-generated field
          this.m_PrefabGeometryNodeStates.TryGetBuffer(prefabRef.m_Prefab, out bufferData2);
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRoadData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            RoadData roadData = this.m_PrefabRoadData[prefabRef.m_Prefab];
            flag3 |= (roadData.m_Flags & Game.Prefabs.RoadFlags.PreferTrafficLights) != 0;
            if ((roadData.m_Flags & (Game.Prefabs.RoadFlags.DefaultIsForward | Game.Prefabs.RoadFlags.DefaultIsBackward)) != (Game.Prefabs.RoadFlags) 0)
              int3 += math.select(new int3(0, 1, 1), new int3(1, 0, 1), edgeIteratorValue.m_End == ((roadData.m_Flags & Game.Prefabs.RoadFlags.DefaultIsForward) != 0));
            else
              ++int3;
          }
          if ((prefabGeometryData1.m_MergeLayers & prefabGeometryData.m_MergeLayers) == Layer.None)
          {
            Layer layer = prefabGeometryData1.m_MergeLayers | prefabGeometryData.m_MergeLayers;
            if ((layer & (Layer.Road | Layer.Pathway | Layer.MarkerPathway | Layer.PublicTransportRoad)) != Layer.None && (layer & (Layer.TrainTrack | Layer.SubwayTrack)) != Layer.None)
              nodeFlags.m_General |= CompositionFlags.General.LevelCrossing;
            if ((layer & Layer.Road) != Layer.None && (layer & (Layer.Pathway | Layer.MarkerPathway)) != Layer.None)
              flag2 = true;
          }
          else
          {
            bool flag9 = edgeIteratorValue.m_Edge == leftEdge;
            bool flag10 = edgeIteratorValue.m_Edge == rightEdge;
            // ISSUE: reference to a compiler-generated field
            Edge edge1 = this.m_EdgeData[edgeIteratorValue.m_Edge];
            // ISSUE: reference to a compiler-generated method
            CompositionFlags elevationFlags = this.GetElevationFlags(edgeIteratorValue.m_Edge, edge1.m_Start, edge1.m_End, prefabGeometryData1);
            CompositionFlags flags = elevationFlags;
            if (edgeIteratorValue.m_End)
              flags = NetCompositionHelpers.InvertCompositionFlags(flags);
            // ISSUE: reference to a compiler-generated method
            CompositionFlags edgeFlags1 = this.GetEdgeFlags(edgeIteratorValue.m_Edge, edgeIteratorValue.m_End, prefabRef.m_Prefab, prefabGeometryData1, elevationFlags);
            if ((edgeFlags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) != (CompositionFlags.General) 0 || (edgeFlags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered | CompositionFlags.Side.SoundBarrier)) != (CompositionFlags.Side) 0 || (edgeFlags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered | CompositionFlags.Side.SoundBarrier)) != (CompositionFlags.Side) 0)
            {
              if ((edgeFlags.m_General & CompositionFlags.General.Elevated) != (CompositionFlags.General) 0)
              {
                if ((flags.m_General & CompositionFlags.General.Elevated) == (CompositionFlags.General) 0)
                {
                  if (flag9)
                  {
                    if ((flags.m_Left & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
                      nodeFlags.m_Left |= CompositionFlags.Side.LowTransition;
                    else
                      nodeFlags.m_Left |= CompositionFlags.Side.HighTransition;
                  }
                  if (flag10)
                  {
                    if ((flags.m_Right & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
                      nodeFlags.m_Right |= CompositionFlags.Side.LowTransition;
                    else
                      nodeFlags.m_Right |= CompositionFlags.Side.HighTransition;
                  }
                }
              }
              else if ((edgeFlags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0)
              {
                if ((flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0)
                {
                  if (flag9)
                  {
                    if ((flags.m_Left & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
                      nodeFlags.m_Left |= CompositionFlags.Side.LowTransition;
                    else
                      nodeFlags.m_Left |= CompositionFlags.Side.HighTransition;
                  }
                  if (flag10)
                  {
                    if ((flags.m_Right & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
                      nodeFlags.m_Right |= CompositionFlags.Side.LowTransition;
                    else
                      nodeFlags.m_Right |= CompositionFlags.Side.HighTransition;
                  }
                }
              }
              else
              {
                if (flag9)
                {
                  if ((edgeFlags.m_Left & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
                  {
                    if ((flags.m_Left & CompositionFlags.Side.Raised) == (CompositionFlags.Side) 0 && (flags.m_General & CompositionFlags.General.Elevated) == (CompositionFlags.General) 0)
                      nodeFlags.m_Left |= CompositionFlags.Side.LowTransition;
                  }
                  else if ((edgeFlags.m_Left & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
                  {
                    if ((flags.m_Left & CompositionFlags.Side.Lowered) == (CompositionFlags.Side) 0 && (flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0)
                      nodeFlags.m_Left |= CompositionFlags.Side.LowTransition;
                  }
                  else if ((edgeFlags.m_Left & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0 && (edgeFlags1.m_Left & CompositionFlags.Side.SoundBarrier) == (CompositionFlags.Side) 0)
                    nodeFlags.m_Left |= CompositionFlags.Side.LowTransition;
                }
                if (flag10)
                {
                  if ((edgeFlags.m_Right & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
                  {
                    if ((flags.m_Right & CompositionFlags.Side.Raised) == (CompositionFlags.Side) 0 && (flags.m_General & CompositionFlags.General.Elevated) == (CompositionFlags.General) 0)
                      nodeFlags.m_Right |= CompositionFlags.Side.LowTransition;
                  }
                  else if ((edgeFlags.m_Right & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
                  {
                    if ((flags.m_Right & CompositionFlags.Side.Lowered) == (CompositionFlags.Side) 0 && (flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0)
                      nodeFlags.m_Right |= CompositionFlags.Side.LowTransition;
                  }
                  else if ((edgeFlags.m_Right & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0 && (edgeFlags1.m_Right & CompositionFlags.Side.SoundBarrier) == (CompositionFlags.Side) 0)
                    nodeFlags.m_Right |= CompositionFlags.Side.LowTransition;
                }
              }
            }
            if ((edgeFlags1.m_Left & CompositionFlags.Side.RemoveCrosswalk) != (CompositionFlags.Side) 0)
            {
              flag7 = true;
            }
            else
            {
              if (((edgeFlags1.m_Left | edgeFlags1.m_Right) & CompositionFlags.Side.Sidewalk) != (CompositionFlags.Side) 0 && (edgeFlags1.m_General & CompositionFlags.General.MiddlePlatform) != (CompositionFlags.General) 0)
                flag5 = true;
              if ((edgeFlags1.m_Left & CompositionFlags.Side.AddCrosswalk) != (CompositionFlags.Side) 0)
              {
                flag5 = true;
                flag8 = true;
              }
              flag6 = true;
            }
            if (((edgeFlags1.m_Left | edgeFlags1.m_Right) & CompositionFlags.Side.ParkingSpaces) != (CompositionFlags.Side) 0)
              flag4 = true;
            if (bufferData1.IsCreated && bufferData1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              nodeFlags |= this.GetNodeStates(bufferData1, compositionFlags1, edgeFlags1, flag9, flag10);
            }
            if (bufferData2.IsCreated && bufferData2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              nodeFlags |= NetCompositionHelpers.InvertCompositionFlags(this.GetNodeStates(bufferData2, edgeFlags1, compositionFlags1, flag10, flag9));
            }
            if (edgeIteratorValue.m_Edge != edge)
            {
              if (prefabRef.m_Prefab != prefab)
                flag1 = false;
              ++num1;
            }
          }
        }
        if ((nodeFlags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) != (CompositionFlags.General) 0)
        {
          int num2 = (nodeFlags.m_Left & (CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition)) > (CompositionFlags.Side) 0 ? 1 : 0;
          bool flag11 = (nodeFlags.m_Right & (CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition)) > (CompositionFlags.Side) 0;
          if ((num2 & (!flag11 ? 1 : 0)) != 0)
            nodeFlags.m_Right |= CompositionFlags.Side.LowTransition;
          if (num2 == 0 & flag11)
            nodeFlags.m_Left |= CompositionFlags.Side.LowTransition;
        }
        if (flag2 && (prefabGeometryData.m_MergeLayers & (Layer.Pathway | Layer.MarkerPathway)) != Layer.None)
          return nodeFlags;
        bool flag12 = flag1 & num1 == 1;
        bool flag13 = flag2 & num1 >= 1;
        bool flag14 = flag6 & flag13;
        if (num1 >= 2)
        {
          nodeFlags.m_General |= CompositionFlags.General.Intersection;
          if ((nodeFlags.m_Left & nodeFlags.m_Right & CompositionFlags.Side.Sidewalk) > (CompositionFlags.Side) 0 | flag13)
            nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
        }
        else if (num1 == 1)
        {
          if (flag12)
          {
            if (flag4 && ((compositionFlags1.m_Left | compositionFlags1.m_Right) & CompositionFlags.Side.ParkingSpaces) != (CompositionFlags.Side) 0 && (nodeFlags.m_General & CompositionFlags.General.LevelCrossing) == (CompositionFlags.General) 0)
              nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
          }
          else
            nodeFlags.m_General |= CompositionFlags.General.Intersection;
          if (flag13)
            nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
        }
        else if (num1 == 0)
        {
          nodeFlags.m_General |= CompositionFlags.General.DeadEnd;
          flag5 = false;
          if ((nodeFlags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
            obsoleteEdgeFlags.m_Left |= compositionFlags1.m_Left & (CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk);
          else
            obsoleteEdgeFlags.m_Right |= compositionFlags1.m_Right & (CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk);
        }
        if (num1 != 0)
        {
          if (((compositionFlags1.m_Left | compositionFlags1.m_Right) & CompositionFlags.Side.Sidewalk) != (CompositionFlags.Side) 0 && (compositionFlags1.m_General & CompositionFlags.General.MiddlePlatform) != (CompositionFlags.General) 0)
            nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
          if ((nodeFlags.m_General & (CompositionFlags.General.Intersection | CompositionFlags.General.MedianBreak)) == (CompositionFlags.General) 0)
          {
            if ((nodeFlags.m_General & CompositionFlags.General.Crosswalk) != (CompositionFlags.General) 0)
            {
              if ((nodeFlags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
              {
                if ((compositionFlags1.m_Left & CompositionFlags.Side.AddCrosswalk) > (CompositionFlags.Side) 0 | flag8)
                  obsoleteEdgeFlags.m_Left |= compositionFlags1.m_Left & (CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk);
                else if ((compositionFlags1.m_Left & CompositionFlags.Side.RemoveCrosswalk) > (CompositionFlags.Side) 0 | flag7)
                {
                  nodeFlags.m_General &= ~CompositionFlags.General.Crosswalk;
                  flag14 = false;
                }
              }
              else if ((compositionFlags1.m_Right & CompositionFlags.Side.AddCrosswalk) > (CompositionFlags.Side) 0 | flag8)
                obsoleteEdgeFlags.m_Right |= compositionFlags1.m_Right & (CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk);
              else if ((compositionFlags1.m_Right & CompositionFlags.Side.RemoveCrosswalk) > (CompositionFlags.Side) 0 | flag7)
              {
                nodeFlags.m_General &= ~CompositionFlags.General.Crosswalk;
                flag14 = false;
              }
            }
            else if ((nodeFlags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
            {
              if ((compositionFlags1.m_Left & CompositionFlags.Side.RemoveCrosswalk) > (CompositionFlags.Side) 0 | flag7)
                obsoleteEdgeFlags.m_Left |= compositionFlags1.m_Left & (CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk);
              else if ((compositionFlags1.m_Left & CompositionFlags.Side.AddCrosswalk) > (CompositionFlags.Side) 0 | flag8)
                nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
            }
            else if ((compositionFlags1.m_Right & CompositionFlags.Side.RemoveCrosswalk) > (CompositionFlags.Side) 0 | flag7)
              obsoleteEdgeFlags.m_Right |= compositionFlags1.m_Right & (CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk);
            else if ((compositionFlags1.m_Right & CompositionFlags.Side.AddCrosswalk) > (CompositionFlags.Side) 0 | flag8)
              nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
          }
          else if ((nodeFlags.m_General & CompositionFlags.General.Crosswalk) != (CompositionFlags.General) 0)
          {
            if ((nodeFlags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
            {
              if ((compositionFlags1.m_Left & CompositionFlags.Side.AddCrosswalk) != (CompositionFlags.Side) 0)
                obsoleteEdgeFlags.m_Left |= CompositionFlags.Side.AddCrosswalk;
              if ((compositionFlags1.m_Left & CompositionFlags.Side.RemoveCrosswalk) != (CompositionFlags.Side) 0)
                nodeFlags.m_General &= ~CompositionFlags.General.Crosswalk;
            }
            else
            {
              if ((compositionFlags1.m_Right & CompositionFlags.Side.AddCrosswalk) != (CompositionFlags.Side) 0)
                obsoleteEdgeFlags.m_Right |= CompositionFlags.Side.AddCrosswalk;
              if ((compositionFlags1.m_Right & CompositionFlags.Side.RemoveCrosswalk) != (CompositionFlags.Side) 0)
                nodeFlags.m_General &= ~CompositionFlags.General.Crosswalk;
            }
          }
          else if ((nodeFlags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
          {
            if ((compositionFlags1.m_Left & CompositionFlags.Side.RemoveCrosswalk) != (CompositionFlags.Side) 0)
              obsoleteEdgeFlags.m_Left |= CompositionFlags.Side.RemoveCrosswalk;
            if ((compositionFlags1.m_Left & CompositionFlags.Side.AddCrosswalk) != (CompositionFlags.Side) 0)
              nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
          }
          else
          {
            if ((compositionFlags1.m_Right & CompositionFlags.Side.RemoveCrosswalk) != (CompositionFlags.Side) 0)
              obsoleteEdgeFlags.m_Right |= CompositionFlags.Side.RemoveCrosswalk;
            if ((compositionFlags1.m_Right & CompositionFlags.Side.AddCrosswalk) != (CompositionFlags.Side) 0)
              nodeFlags.m_General |= CompositionFlags.General.Crosswalk;
          }
          if ((nodeFlags.m_General & CompositionFlags.General.MedianBreak) != (CompositionFlags.General) 0 && (nodeFlags.m_General & CompositionFlags.General.Crosswalk) > (CompositionFlags.General) 0 | flag5)
            nodeFlags.m_General |= CompositionFlags.General.Intersection;
        }
        CompositionFlags compositionFlags2 = new CompositionFlags();
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpgradedData.HasComponent(node))
        {
          // ISSUE: reference to a compiler-generated field
          Upgraded upgraded = this.m_UpgradedData[node];
          compositionFlags2 = upgraded.m_Flags;
          upgraded.m_Flags.m_General &= ~CompositionFlags.General.RemoveTrafficLights;
          nodeFlags |= upgraded.m_Flags;
        }
        if ((nodeFlags.m_General & (CompositionFlags.General.Roundabout | CompositionFlags.General.LevelCrossing)) == (CompositionFlags.General) 0 && (nodeFlags.m_General & (CompositionFlags.General.Intersection | CompositionFlags.General.Crosswalk)) > (CompositionFlags.General) 0 | flag13 | flag5)
        {
          if (flag3 && math.all(int3 >= new int3(1, 1, 2)) && ((math.all(int3 >= new int3(2, 1, 3)) ? 1 : ((nodeFlags.m_General & CompositionFlags.General.Crosswalk) > (CompositionFlags.General) 0 ? 1 : 0)) | (flag5 ? 1 : 0) | (flag14 ? 1 : 0)) != 0)
          {
            if ((compositionFlags2.m_General & (CompositionFlags.General.RemoveTrafficLights | CompositionFlags.General.AllWayStop)) != (CompositionFlags.General) 0)
              nodeFlags.m_General &= ~CompositionFlags.General.TrafficLights;
            else
              nodeFlags.m_General |= CompositionFlags.General.TrafficLights;
            obsoleteNodeFlags.m_General |= compositionFlags2.m_General & CompositionFlags.General.TrafficLights;
          }
          else
            obsoleteNodeFlags.m_General |= compositionFlags2.m_General & CompositionFlags.General.RemoveTrafficLights;
        }
        else
        {
          nodeFlags.m_General &= ~(CompositionFlags.General.TrafficLights | CompositionFlags.General.AllWayStop);
          obsoleteNodeFlags.m_General |= compositionFlags2.m_General & (CompositionFlags.General.TrafficLights | CompositionFlags.General.RemoveTrafficLights | CompositionFlags.General.AllWayStop);
        }
        if ((nodeFlags.m_General & (CompositionFlags.General.Intersection | CompositionFlags.General.Roundabout)) != CompositionFlags.General.Intersection)
        {
          nodeFlags.m_Left &= ~(CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.ForbidStraight);
          nodeFlags.m_Right &= ~(CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.ForbidStraight);
          if ((nodeFlags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
            obsoleteEdgeFlags.m_Left |= compositionFlags1.m_Left & (CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.ForbidStraight);
          else
            obsoleteEdgeFlags.m_Right |= compositionFlags1.m_Right & (CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.ForbidStraight);
        }
        return nodeFlags;
      }

      private void FindFriendEdges(
        Entity edge,
        Entity node,
        Layer mergeLayers,
        Curve curve,
        out Entity leftEdge,
        out Entity rightEdge)
      {
        float2 float2_1 = (float2) 0.0f;
        int num1 = 0;
        leftEdge = edge;
        rightEdge = edge;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge, node, this.m_ConnectedEdges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue1;
        while (edgeIterator.GetNext(out edgeIteratorValue1))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(edgeIteratorValue1.m_Edge == edge) && (this.m_PrefabGeometryData[this.m_PrefabRefData[edgeIteratorValue1.m_Edge].m_Prefab].m_MergeLayers & mergeLayers) != Layer.None)
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve1 = this.m_CurveData[edgeIteratorValue1.m_Edge];
            float2_1 += math.select(curve1.m_Bezier.a.xz, curve1.m_Bezier.d.xz, edgeIteratorValue1.m_End);
            ++num1;
            leftEdge = edgeIteratorValue1.m_Edge;
            rightEdge = edgeIteratorValue1.m_Edge;
          }
        }
        if (num1 <= 1)
          return;
        float2 float2_2 = float2_1 / (float) num1;
        float3 float3 = MathUtils.Position(curve.m_Bezier, 0.5f);
        float2 float2_3 = math.normalizesafe(float3.xz - float2_2);
        float2 x = MathUtils.Right(float2_3);
        float num2 = -2f;
        float num3 = 2f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        edgeIterator = new EdgeIterator(edge, node, this.m_ConnectedEdges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue2;
        while (edgeIterator.GetNext(out edgeIteratorValue2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(edgeIteratorValue2.m_Edge == edge) && (this.m_PrefabGeometryData[this.m_PrefabRefData[edgeIteratorValue2.m_Edge].m_Prefab].m_MergeLayers & mergeLayers) != Layer.None)
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve2 = this.m_CurveData[edgeIteratorValue2.m_Edge];
            if (edgeIteratorValue2.m_End)
              curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
            float3 = MathUtils.Position(curve2.m_Bezier, 0.5f);
            float2 y = math.normalizesafe(float3.xz - float2_2);
            float num4;
            if ((double) math.dot(float2_3, y) < 0.0)
            {
              num4 = math.dot(x, y) * 0.5f;
            }
            else
            {
              float num5 = math.dot(x, y);
              num4 = math.select(-1f, 1f, (double) num5 >= 0.0) - num5 * 0.5f;
            }
            if ((double) num4 > (double) num2)
            {
              num2 = num4;
              leftEdge = edgeIteratorValue2.m_Edge;
            }
            if ((double) num4 < (double) num3)
            {
              num3 = num4;
              rightEdge = edgeIteratorValue2.m_Edge;
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

    private struct CreatedCompositionKey : IEquatable<CompositionSelectSystem.CreatedCompositionKey>
    {
      public Entity m_Prefab;
      public CompositionFlags m_Flags;

      public bool Equals(
        CompositionSelectSystem.CreatedCompositionKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Prefab.Equals(other.m_Prefab) && this.m_Flags == other.m_Flags;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Prefab.GetHashCode() * 31 + this.m_Flags.GetHashCode();
      }
    }

    [BurstCompile]
    private struct CreateCompositionJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetTerrainData> m_PrefabTerrainData;
      [ReadOnly]
      public BufferLookup<SubReplacement> m_SubReplacements;
      [ReadOnly]
      public BufferLookup<NetGeometrySection> m_PrefabGeometrySections;
      [ReadOnly]
      public BufferLookup<NetSubSection> m_PrefabSubSections;
      [ReadOnly]
      public BufferLookup<NetSectionPiece> m_PrefabSectionPieces;
      public ComponentLookup<Upgraded> m_UpgradedData;
      public ComponentLookup<Temp> m_TempData;
      public NativeQueue<CompositionSelectSystem.CompositionCreateInfo> m_CompositionCreateQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_CompositionCreateQueue.Count;
        if (count == 0)
          return;
        NativeParallelHashMap<CompositionSelectSystem.CreatedCompositionKey, Entity> createdCompositions = new NativeParallelHashMap<CompositionSelectSystem.CreatedCompositionKey, Entity>();
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CompositionSelectSystem.CompositionCreateInfo compositionCreateInfo = this.m_CompositionCreateQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          if (compositionCreateInfo.m_CompositionData.m_Edge == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            compositionCreateInfo.m_CompositionData.m_Edge = this.GetOrCreateComposition(compositionCreateInfo.m_Prefab, compositionCreateInfo.m_EdgeFlags, ref createdCompositions);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeData.HasComponent(compositionCreateInfo.m_Entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_EdgeData[compositionCreateInfo.m_Entity];
            // ISSUE: reference to a compiler-generated field
            if (compositionCreateInfo.m_CompositionData.m_StartNode == Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              compositionCreateInfo.m_CompositionData.m_StartNode = this.GetOrCreateComposition(compositionCreateInfo.m_Prefab, compositionCreateInfo.m_StartFlags, ref createdCompositions);
            }
            // ISSUE: reference to a compiler-generated field
            if (compositionCreateInfo.m_CompositionData.m_EndNode == Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              compositionCreateInfo.m_CompositionData.m_EndNode = this.GetOrCreateComposition(compositionCreateInfo.m_Prefab, compositionCreateInfo.m_EndFlags, ref createdCompositions);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Composition>(compositionCreateInfo.m_Entity, compositionCreateInfo.m_CompositionData);
            // ISSUE: reference to a compiler-generated field
            if (compositionCreateInfo.m_ObsoleteStartFlags != new CompositionFlags())
            {
              // ISSUE: reference to a compiler-generated field
              Upgraded upgraded = this.m_UpgradedData[edge.m_Start];
              if (upgraded.m_Flags != new CompositionFlags())
              {
                // ISSUE: reference to a compiler-generated field
                upgraded.m_Flags &= ~compositionCreateInfo.m_ObsoleteStartFlags;
                // ISSUE: reference to a compiler-generated field
                this.m_UpgradedData[edge.m_Start] = upgraded;
                if (upgraded.m_Flags == new CompositionFlags())
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Upgraded>(edge.m_Start);
                }
                Temp componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.TryGetComponent(edge.m_Start, out componentData1) && componentData1.m_Original != Entity.Null && (componentData1.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent))
                {
                  Upgraded componentData2;
                  // ISSUE: reference to a compiler-generated field
                  this.m_UpgradedData.TryGetComponent(componentData1.m_Original, out componentData2);
                  if (upgraded.m_Flags == componentData2.m_Flags)
                  {
                    componentData1.m_Flags &= ~(TempFlags.Upgrade | TempFlags.Parent);
                    // ISSUE: reference to a compiler-generated field
                    this.m_TempData[edge.m_Start] = componentData1;
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (compositionCreateInfo.m_ObsoleteEndFlags != new CompositionFlags())
            {
              // ISSUE: reference to a compiler-generated field
              Upgraded upgraded = this.m_UpgradedData[edge.m_End];
              if (upgraded.m_Flags != new CompositionFlags())
              {
                // ISSUE: reference to a compiler-generated field
                upgraded.m_Flags &= ~compositionCreateInfo.m_ObsoleteEndFlags;
                // ISSUE: reference to a compiler-generated field
                this.m_UpgradedData[edge.m_End] = upgraded;
                if (upgraded.m_Flags == new CompositionFlags())
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Upgraded>(edge.m_End);
                }
                Temp componentData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.TryGetComponent(edge.m_End, out componentData3) && componentData3.m_Original != Entity.Null && (componentData3.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent))
                {
                  Upgraded componentData4;
                  // ISSUE: reference to a compiler-generated field
                  this.m_UpgradedData.TryGetComponent(componentData3.m_Original, out componentData4);
                  if (upgraded.m_Flags == componentData4.m_Flags)
                  {
                    componentData3.m_Flags &= ~(TempFlags.Upgrade | TempFlags.Parent);
                    // ISSUE: reference to a compiler-generated field
                    this.m_TempData[edge.m_End] = componentData3;
                  }
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Orphan>(compositionCreateInfo.m_Entity, new Orphan()
            {
              m_Composition = compositionCreateInfo.m_CompositionData.m_Edge
            });
          }
          // ISSUE: reference to a compiler-generated field
          if (compositionCreateInfo.m_ObsoleteEdgeFlags != new CompositionFlags())
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Upgraded upgraded = this.m_UpgradedData[compositionCreateInfo.m_Entity];
            if (upgraded.m_Flags != new CompositionFlags())
            {
              // ISSUE: reference to a compiler-generated field
              upgraded.m_Flags &= ~compositionCreateInfo.m_ObsoleteEdgeFlags;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_UpgradedData[compositionCreateInfo.m_Entity] = upgraded;
              if (upgraded.m_Flags == new CompositionFlags())
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Upgraded>(compositionCreateInfo.m_Entity);
              }
              Temp componentData5;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TempData.TryGetComponent(compositionCreateInfo.m_Entity, out componentData5) && componentData5.m_Original != Entity.Null && (componentData5.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent))
              {
                Upgraded componentData6;
                // ISSUE: reference to a compiler-generated field
                this.m_UpgradedData.TryGetComponent(componentData5.m_Original, out componentData6);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (upgraded.m_Flags == componentData6.m_Flags && this.EqualSubReplacements(compositionCreateInfo.m_Entity, componentData5.m_Original))
                {
                  componentData5.m_Flags &= ~(TempFlags.Upgrade | TempFlags.Parent);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_TempData[compositionCreateInfo.m_Entity] = componentData5;
                }
              }
            }
          }
        }
        if (!createdCompositions.IsCreated)
          return;
        createdCompositions.Dispose();
      }

      private bool EqualSubReplacements(Entity entity1, Entity entity2)
      {
        DynamicBuffer<SubReplacement> bufferData1;
        // ISSUE: reference to a compiler-generated field
        bool buffer1 = this.m_SubReplacements.TryGetBuffer(entity1, out bufferData1);
        DynamicBuffer<SubReplacement> bufferData2;
        // ISSUE: reference to a compiler-generated field
        bool buffer2 = this.m_SubReplacements.TryGetBuffer(entity2, out bufferData2);
        if (buffer1 != buffer2)
          return false;
        if (!buffer1)
          return true;
        if (bufferData1.Length != bufferData2.Length)
          return false;
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          SubReplacement subReplacement = bufferData1[index1];
          bool flag = false;
          for (int index2 = 0; index2 < bufferData2.Length; ++index2)
          {
            SubReplacement other = bufferData2[index2];
            if (subReplacement.Equals(other))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return false;
        }
        return true;
      }

      private Entity GetOrCreateComposition(
        Entity prefab,
        CompositionFlags flags,
        ref NativeParallelHashMap<CompositionSelectSystem.CreatedCompositionKey, Entity> createdCompositions)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CompositionSelectSystem.CreatedCompositionKey key = new CompositionSelectSystem.CreatedCompositionKey()
        {
          m_Prefab = prefab,
          m_Flags = flags
        };
        if (createdCompositions.IsCreated)
        {
          Entity composition;
          if (createdCompositions.TryGetValue(key, out composition))
            return composition;
        }
        else
          createdCompositions = new NativeParallelHashMap<CompositionSelectSystem.CreatedCompositionKey, Entity>(50, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        Entity composition1 = this.CreateComposition(prefab, flags);
        createdCompositions.Add(key, composition1);
        return composition1;
      }

      private Entity CreateComposition(Entity prefab, CompositionFlags mask)
      {
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabGeometryData[prefab];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetGeometrySection> prefabGeometrySection = this.m_PrefabGeometrySections[prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity e = (mask.m_General & CompositionFlags.General.Node) == (CompositionFlags.General) 0 ? this.m_CommandBuffer.CreateEntity(netGeometryData.m_EdgeCompositionArchetype) : this.m_CommandBuffer.CreateEntity(netGeometryData.m_NodeCompositionArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AppendToBuffer<NetGeometryComposition>(prefab, new NetGeometryComposition()
        {
          m_Composition = e,
          m_Mask = mask
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(e, new PrefabRef(prefab));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<NetCompositionData>(e, new NetCompositionData()
        {
          m_Flags = mask
        });
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionPiece> dynamicBuffer = this.m_CommandBuffer.SetBuffer<NetCompositionPiece>(e);
        NativeList<NetCompositionPiece> resultBuffer = new NativeList<NetCompositionPiece>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionHelpers.GetCompositionPieces(resultBuffer, prefabGeometrySection.AsNativeArray(), mask, this.m_PrefabSubSections, this.m_PrefabSectionPieces);
        dynamicBuffer.CopyFrom(resultBuffer.AsArray());
        for (int index = 0; index < resultBuffer.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTerrainData.HasComponent(resultBuffer[index].m_Piece))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<TerrainComposition>(e, new TerrainComposition());
            break;
          }
        }
        resultBuffer.Dispose();
        return e;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> __Game_Net_Upgraded_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Fixed> __Game_Net_Fixed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Orphan> __Game_Net_Orphan_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Fixed> __Game_Net_Fixed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetObjectData> __Game_Prefabs_NetObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetGeometryComposition> __Game_Prefabs_NetGeometryComposition_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetGeometryEdgeState> __Game_Prefabs_NetGeometryEdgeState_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetGeometryNodeState> __Game_Prefabs_NetGeometryNodeState_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<FixedNetElement> __Game_Prefabs_FixedNetElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<NetTerrainData> __Game_Prefabs_NetTerrainData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubReplacement> __Game_Net_SubReplacement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetGeometrySection> __Game_Prefabs_NetGeometrySection_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetSubSection> __Game_Prefabs_NetSubSection_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetSectionPiece> __Game_Prefabs_NetSectionPiece_RO_BufferLookup;
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RW_ComponentLookup;
      public ComponentLookup<Temp> __Game_Tools_Temp_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Orphan>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentLookup = state.GetComponentLookup<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetObjectData_RO_ComponentLookup = state.GetComponentLookup<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RO_ComponentLookup = state.GetComponentLookup<RoadData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryComposition_RO_BufferLookup = state.GetBufferLookup<NetGeometryComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryEdgeState_RO_BufferLookup = state.GetBufferLookup<NetGeometryEdgeState>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryNodeState_RO_BufferLookup = state.GetBufferLookup<NetGeometryNodeState>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FixedNetElement_RO_BufferLookup = state.GetBufferLookup<FixedNetElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetTerrainData_RO_ComponentLookup = state.GetComponentLookup<NetTerrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubReplacement_RO_BufferLookup = state.GetBufferLookup<SubReplacement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometrySection_RO_BufferLookup = state.GetBufferLookup<NetGeometrySection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetSubSection_RO_BufferLookup = state.GetBufferLookup<NetSubSection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetSectionPiece_RO_BufferLookup = state.GetBufferLookup<NetSectionPiece>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RW_ComponentLookup = state.GetComponentLookup<Upgraded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RW_ComponentLookup = state.GetComponentLookup<Temp>();
      }
    }
  }
}
