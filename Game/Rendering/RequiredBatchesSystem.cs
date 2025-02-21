// Decompiled with JetBrains decompiler
// Type: Game.Rendering.RequiredBatchesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Colossal.Rendering;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class RequiredBatchesSystem : GameSystemBase
  {
    private BatchManagerSystem m_BatchManagerSystem;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_AllQuery;
    private bool m_Loaded;
    private RequiredBatchesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<MeshBatch>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllQuery = this.GetEntityQuery(ComponentType.ReadOnly<MeshBatch>(), ComponentType.ReadOnly<PrefabRef>());
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BatchGroup_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneBlockData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshMaterial_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LodMesh_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Override_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new RequiredBatchesSystem.RequiredBatchesJob()
      {
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_ObjectType = this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle,
        m_ElevationType = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle,
        m_ObjectMarkerType = this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_OrphanType = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle,
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_UtilityLaneType = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle,
        m_NetMarkerType = this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle,
        m_BlockType = this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ErrorType = this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle,
        m_WarningType = this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle,
        m_OverrideType = this.__TypeHandle.__Game_Tools_Override_RO_ComponentTypeHandle,
        m_HighlightedType = this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MeshGroupType = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabCompositionMeshRef = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_PrefabSubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferLookup,
        m_PrefabSubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RW_BufferLookup,
        m_PrefabLodMeshes = this.__TypeHandle.__Game_Prefabs_LodMesh_RW_BufferLookup,
        m_PrefabMeshMaterials = this.__TypeHandle.__Game_Prefabs_MeshMaterial_RW_BufferLookup,
        m_PrefabMeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RW_ComponentLookup,
        m_PrefabCompositionMeshData = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RW_ComponentLookup,
        m_PrefabZoneBlockData = this.__TypeHandle.__Game_Prefabs_ZoneBlockData_RW_ComponentLookup,
        m_PrefabBatchGroups = this.__TypeHandle.__Game_Prefabs_BatchGroup_RW_BufferLookup,
        m_NativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies1),
        m_NativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(false, out dependencies2),
        m_NativeSubBatches = this.m_BatchManagerSystem.GetNativeSubBatches(false, out dependencies3)
      }.Schedule<RequiredBatchesSystem.RequiredBatchesJob>(query, JobHandle.CombineDependencies(this.Dependency, JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchInstancesWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchGroupsWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeSubBatchesWriter(jobHandle);
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
    public RequiredBatchesSystem()
    {
    }

    [BurstCompile]
    private struct RequiredBatchesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Object> m_ObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> m_ElevationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Marker> m_ObjectMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> m_OrphanType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> m_UtilityLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Marker> m_NetMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> m_BlockType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Error> m_ErrorType;
      [ReadOnly]
      public ComponentTypeHandle<Warning> m_WarningType;
      [ReadOnly]
      public ComponentTypeHandle<Override> m_OverrideType;
      [ReadOnly]
      public ComponentTypeHandle<Highlighted> m_HighlightedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> m_MeshGroupType;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<SubMesh> m_PrefabSubMeshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_PrefabSubMeshGroups;
      [ReadOnly]
      public BufferLookup<LodMesh> m_PrefabLodMeshes;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> m_PrefabCompositionMeshRef;
      public ComponentLookup<Game.Prefabs.MeshData> m_PrefabMeshData;
      [ReadOnly]
      public BufferLookup<MeshMaterial> m_PrefabMeshMaterials;
      public ComponentLookup<NetCompositionMeshData> m_PrefabCompositionMeshData;
      public ComponentLookup<ZoneBlockData> m_PrefabZoneBlockData;
      public BufferLookup<BatchGroup> m_PrefabBatchGroups;
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchInstances;
      public NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> m_NativeSubBatches;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Objects.Object>(ref this.m_ObjectType))
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateObjectBatches(chunk);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Composition>(ref this.m_CompositionType) || chunk.Has<Orphan>(ref this.m_OrphanType))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetBatches(chunk);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Lane>(ref this.m_LaneType))
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneBatches(chunk);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!chunk.Has<Game.Zones.Block>(ref this.m_BlockType))
                return;
              // ISSUE: reference to a compiler-generated method
              this.UpdateZoneBatches(chunk);
            }
          }
        }
      }

      private void UpdateObjectBatches(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray1 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Elevation> nativeArray2 = chunk.GetNativeArray<Game.Objects.Elevation>(ref this.m_ElevationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MeshGroup> bufferAccessor = chunk.GetBufferAccessor<MeshGroup>(ref this.m_MeshGroupType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<InterpolatedTransform>(ref this.m_InterpolatedTransformType) || chunk.Has<Stopped>(ref this.m_StoppedType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<Game.Objects.Marker>(ref this.m_ObjectMarkerType);
        MeshLayer meshLayer1 = MeshLayer.Default;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Error>(ref this.m_ErrorType) || chunk.Has<Warning>(ref this.m_WarningType) || chunk.Has<Override>(ref this.m_OverrideType) || chunk.Has<Highlighted>(ref this.m_HighlightedType))
          meshLayer1 |= MeshLayer.Outline;
        for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray4[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSubMeshes.HasBuffer(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubMesh> prefabSubMesh = this.m_PrefabSubMeshes[prefabRef.m_Prefab];
            MeshLayer meshLayer2 = meshLayer1;
            if (nativeArray3.Length != 0)
            {
              Temp temp = nativeArray3[index1];
              if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
              {
                if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent | TempFlags.SubDetail)) != (TempFlags) 0)
                  meshLayer2 |= MeshLayer.Outline;
              }
              else
                continue;
            }
            if (flag2)
              meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Marker;
            else if (flag1)
              meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Moving;
            else if (nativeArray2.Length != 0 && (double) nativeArray2[index1].m_Elevation < 0.0)
              meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Tunnel;
            DynamicBuffer<MeshGroup> buffer = new DynamicBuffer<MeshGroup>();
            int num = 1;
            DynamicBuffer<SubMeshGroup> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData) && CollectionUtils.TryGet<MeshGroup>(bufferAccessor, index1, out buffer))
              num = buffer.Length;
            for (int index2 = 0; index2 < num; ++index2)
            {
              SubMeshGroup subMeshGroup;
              if (bufferData.IsCreated)
              {
                MeshGroup meshGroup;
                CollectionUtils.TryGet<MeshGroup>(buffer, index2, out meshGroup);
                subMeshGroup = bufferData[(int) meshGroup.m_SubMeshGroup];
              }
              else
                subMeshGroup.m_SubMeshRange = new int2(0, prefabSubMesh.Length);
              for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
              {
                SubMesh subMesh = prefabSubMesh[x];
                // ISSUE: reference to a compiler-generated field
                Game.Prefabs.MeshData meshData = this.m_PrefabMeshData[subMesh.m_SubMesh];
                MeshLayer meshLayer3 = meshLayer2;
                if (meshData.m_DefaultLayers != (MeshLayer) 0 && (meshLayer2 & (MeshLayer.Moving | MeshLayer.Marker)) == (MeshLayer) 0 || (meshData.m_DefaultLayers & (MeshLayer.Pipeline | MeshLayer.SubPipeline)) != (MeshLayer) 0)
                {
                  MeshLayer meshLayer4 = meshLayer3 & ~(MeshLayer.Default | MeshLayer.Moving | MeshLayer.Tunnel | MeshLayer.Marker);
                  Owner owner;
                  CollectionUtils.TryGet<Owner>(nativeArray1, index1, out owner);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  meshLayer3 = meshLayer4 | Game.Net.SearchSystem.GetLayers(owner, new Game.Net.UtilityLane(), meshData.m_DefaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData);
                }
                MeshLayer newLayers = meshLayer3 & ~meshData.m_AvailableLayers;
                MeshType newTypes = MeshType.Object & ~meshData.m_AvailableTypes;
                if (newLayers != (MeshLayer) 0 || newTypes != (MeshType) 0)
                {
                  meshData.m_AvailableLayers |= newLayers;
                  meshData.m_AvailableTypes |= newTypes;
                  // ISSUE: reference to a compiler-generated field
                  this.m_PrefabMeshData[subMesh.m_SubMesh] = meshData;
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeBatchGroups(subMesh.m_SubMesh, newLayers, newTypes, meshData.m_AvailableLayers, meshData.m_AvailableTypes, false, (ushort) 0);
                }
              }
            }
          }
        }
      }

      private void UpdateNetBatches(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Composition> nativeArray1 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Orphan> nativeArray2 = chunk.GetNativeArray<Orphan>(ref this.m_OrphanType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Game.Net.Marker>(ref this.m_NetMarkerType);
        MeshLayer meshLayer = MeshLayer.Default;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Error>(ref this.m_ErrorType) || chunk.Has<Warning>(ref this.m_WarningType) || chunk.Has<Highlighted>(ref this.m_HighlightedType))
          meshLayer |= MeshLayer.Outline;
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          MeshLayer requiredLayers = meshLayer;
          if (nativeArray3.Length != 0)
          {
            Temp temp = nativeArray3[index];
            if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
            {
              if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent | TempFlags.SubDetail)) != (TempFlags) 0)
                requiredLayers |= MeshLayer.Outline;
            }
            else
              continue;
          }
          if (flag)
            requiredLayers = requiredLayers & ~MeshLayer.Default | MeshLayer.Marker;
          if (nativeArray1.Length != 0)
          {
            Composition composition = nativeArray1[index];
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetBatches(composition.m_Edge, requiredLayers);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetBatches(composition.m_StartNode, requiredLayers);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetBatches(composition.m_EndNode, requiredLayers);
          }
          else if (nativeArray2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetBatches(nativeArray2[index].m_Composition, requiredLayers);
          }
        }
      }

      private void UpdateNetBatches(Entity composition, MeshLayer requiredLayers)
      {
        NetCompositionMeshRef componentData1;
        NetCompositionMeshData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabCompositionMeshRef.TryGetComponent(composition, out componentData1) || !this.m_PrefabCompositionMeshData.TryGetComponent(componentData1.m_Mesh, out componentData2))
          return;
        MeshLayer meshLayer = requiredLayers;
        if (componentData2.m_DefaultLayers != (MeshLayer) 0 && (requiredLayers & MeshLayer.Marker) == (MeshLayer) 0)
          meshLayer = meshLayer & ~MeshLayer.Default | componentData2.m_DefaultLayers;
        MeshLayer newLayers = meshLayer & ~componentData2.m_AvailableLayers;
        if (newLayers == (MeshLayer) 0)
          return;
        componentData2.m_AvailableLayers |= newLayers;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabCompositionMeshData[componentData1.m_Mesh] = componentData2;
        // ISSUE: reference to a compiler-generated method
        this.InitializeBatchGroups(componentData1.m_Mesh, newLayers, (MeshType) 0, componentData2.m_AvailableLayers, MeshType.Net, false, (ushort) 0);
      }

      private void UpdateLaneBatches(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray1 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.UtilityLane> nativeArray2 = chunk.GetNativeArray<Game.Net.UtilityLane>(ref this.m_UtilityLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        MeshLayer meshLayer1 = MeshLayer.Default;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Error>(ref this.m_ErrorType) || chunk.Has<Warning>(ref this.m_WarningType) || chunk.Has<Highlighted>(ref this.m_HighlightedType))
          meshLayer1 |= MeshLayer.Outline;
        for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray4[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSubMeshes.HasBuffer(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubMesh> prefabSubMesh = this.m_PrefabSubMeshes[prefabRef.m_Prefab];
            MeshLayer meshLayer2 = meshLayer1;
            if (nativeArray3.Length != 0)
            {
              Temp temp = nativeArray3[index1];
              if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
              {
                if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent | TempFlags.SubDetail)) != (TempFlags) 0)
                  meshLayer2 |= MeshLayer.Outline;
              }
              else
                continue;
            }
            // ISSUE: reference to a compiler-generated method
            if (nativeArray1.Length != 0 && this.IsNetOwnerTunnel(nativeArray1[index1]))
              meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Tunnel;
            int partition = 256;
            UtilityLaneData componentData;
            // ISSUE: reference to a compiler-generated field
            if (nativeArray2.Length != 0 && this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData) && (componentData.m_UtilityTypes & (UtilityTypes.WaterPipe | UtilityTypes.SewagePipe | UtilityTypes.LowVoltageLine | UtilityTypes.HighVoltageLine)) != UtilityTypes.None)
              partition = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(componentData.m_VisualCapacity)));
            for (int index2 = 0; index2 < prefabSubMesh.Length; ++index2)
            {
              SubMesh subMesh = prefabSubMesh[index2];
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.MeshData meshData = this.m_PrefabMeshData[subMesh.m_SubMesh];
              MeshLayer meshLayer3 = meshLayer2;
              if ((subMesh.m_Flags & SubMeshFlags.RequireEditor) != (SubMeshFlags) 0)
                meshLayer3 = meshLayer3 & ~(MeshLayer.Default | MeshLayer.Tunnel) | MeshLayer.Marker;
              if (meshData.m_DefaultLayers != (MeshLayer) 0 && (meshLayer3 & MeshLayer.Marker) == (MeshLayer) 0 || (meshData.m_DefaultLayers & (MeshLayer.Pipeline | MeshLayer.SubPipeline)) != (MeshLayer) 0)
              {
                MeshLayer meshLayer4 = meshLayer3 & ~(MeshLayer.Default | MeshLayer.Tunnel | MeshLayer.Marker);
                Owner owner;
                CollectionUtils.TryGet<Owner>(nativeArray1, index1, out owner);
                Game.Net.UtilityLane utilityLane;
                CollectionUtils.TryGet<Game.Net.UtilityLane>(nativeArray2, index1, out utilityLane);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                meshLayer3 = meshLayer4 | Game.Net.SearchSystem.GetLayers(owner, utilityLane, meshData.m_DefaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData);
              }
              MeshLayer newLayers = meshLayer3 & ~meshData.m_AvailableLayers;
              MeshType newTypes = MeshType.Lane & ~meshData.m_AvailableTypes;
              if (newLayers != (MeshLayer) 0 || newTypes != (MeshType) 0)
              {
                meshData.m_AvailableLayers |= newLayers;
                meshData.m_AvailableTypes |= newTypes;
                // ISSUE: reference to a compiler-generated field
                this.m_PrefabMeshData[subMesh.m_SubMesh] = meshData;
                // ISSUE: reference to a compiler-generated method
                this.InitializeBatchGroups(subMesh.m_SubMesh, newLayers, newTypes, meshData.m_AvailableLayers, meshData.m_AvailableTypes, false, (ushort) meshData.m_MinLod);
                if (partition < (int) meshData.m_MinLod)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeBatchGroups(subMesh.m_SubMesh, newLayers, newTypes, meshData.m_AvailableLayers, meshData.m_AvailableTypes, false, (ushort) partition);
                }
              }
            }
          }
        }
      }

      private void UpdateZoneBatches(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Zones.Block> nativeArray1 = chunk.GetNativeArray<Game.Zones.Block>(ref this.m_BlockType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Game.Zones.Block block = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          ZoneBlockData zoneBlockData = this.m_PrefabZoneBlockData[prefabRef.m_Prefab];
          ushort partition = (ushort) math.clamp(block.m_Size.x * block.m_Size.y - 1 >> 4, 0, 3);
          MeshLayer newLayers = MeshLayer.Default & ~zoneBlockData.m_AvailableLayers;
          ushort num = (ushort) (1U << (int) partition & (uint) ~zoneBlockData.m_AvailablePartitions);
          if (newLayers != (MeshLayer) 0 || num != (ushort) 0)
          {
            zoneBlockData.m_AvailableLayers |= newLayers;
            zoneBlockData.m_AvailablePartitions |= num;
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabZoneBlockData[prefabRef.m_Prefab] = zoneBlockData;
            // ISSUE: reference to a compiler-generated method
            this.InitializeBatchGroups(prefabRef.m_Prefab, newLayers, (MeshType) 0, zoneBlockData.m_AvailableLayers, MeshType.Zone, num > (ushort) 0, partition);
          }
        }
      }

      private void InitializeBatchGroups(
        Entity mesh,
        MeshLayer newLayers,
        MeshType newTypes,
        MeshLayer allLayers,
        MeshType allTypes,
        bool isNewPartition,
        ushort partition)
      {
        if (newLayers != (MeshLayer) 0)
        {
          for (MeshLayer layer = MeshLayer.Default; layer <= MeshLayer.Marker; layer = (MeshLayer) ((uint) layer << 1))
          {
            if ((newLayers & layer) != (MeshLayer) 0)
            {
              for (MeshType type = MeshType.Object; type <= MeshType.Zone; type = (MeshType) ((uint) type << 1))
              {
                if ((allTypes & type) != (MeshType) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeBatchGroup(mesh, layer, type, partition);
                }
              }
            }
          }
          allLayers &= ~newLayers;
        }
        if (newTypes != (MeshType) 0)
        {
          for (MeshType type = MeshType.Object; type <= MeshType.Zone; type = (MeshType) ((uint) type << 1))
          {
            if ((newTypes & type) != (MeshType) 0)
            {
              for (MeshLayer layer = MeshLayer.Default; layer <= MeshLayer.Marker; layer = (MeshLayer) ((uint) layer << 1))
              {
                if ((allLayers & layer) != (MeshLayer) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeBatchGroup(mesh, layer, type, partition);
                }
              }
            }
          }
          allTypes &= ~newTypes;
        }
        if (!isNewPartition)
          return;
        for (MeshLayer layer = MeshLayer.Default; layer <= MeshLayer.Marker; layer = (MeshLayer) ((uint) layer << 1))
        {
          if ((allLayers & layer) != (MeshLayer) 0)
          {
            for (MeshType type = MeshType.Object; type <= MeshType.Zone; type = (MeshType) ((uint) type << 1))
            {
              if ((allTypes & type) != (MeshType) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.InitializeBatchGroup(mesh, layer, type, partition);
              }
            }
          }
        }
      }

      private void InitializeBatchGroup(
        Entity mesh,
        MeshLayer layer,
        MeshType type,
        ushort partition)
      {
        DynamicBuffer<LodMesh> dynamicBuffer;
        Bounds3 bounds1;
        MeshFlags meshFlags;
        int num1;
        float num2;
        int num3;
        int x1;
        int x2;
        float bias1;
        float bias2;
        switch (type)
        {
          case MeshType.Net:
            // ISSUE: reference to a compiler-generated field
            NetCompositionMeshData compositionMeshData1 = this.m_PrefabCompositionMeshData[mesh];
            dynamicBuffer = new DynamicBuffer<LodMesh>();
            bounds1 = new Bounds3(new float3(compositionMeshData1.m_Width * -0.5f, compositionMeshData1.m_HeightRange.min, 0.0f), new float3(compositionMeshData1.m_Width * 0.5f, compositionMeshData1.m_HeightRange.max, 0.0f));
            meshFlags = (MeshFlags) 0;
            // ISSUE: reference to a compiler-generated field
            num1 = this.m_PrefabMeshMaterials[mesh].Length;
            num2 = compositionMeshData1.m_IndexFactor;
            num3 = 0;
            x1 = 0;
            x2 = 0;
            bias1 = compositionMeshData1.m_LodBias;
            bias2 = compositionMeshData1.m_ShadowBias;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLodMeshes.HasBuffer(mesh))
            {
              // ISSUE: reference to a compiler-generated field
              dynamicBuffer = this.m_PrefabLodMeshes[mesh];
              num3 = dynamicBuffer.Length;
              break;
            }
            break;
          case MeshType.Zone:
            dynamicBuffer = new DynamicBuffer<LodMesh>();
            bounds1 = ZoneMeshHelpers.GetBounds(new int2(10, 6));
            meshFlags = (MeshFlags) 0;
            num1 = 1;
            num2 = (float) ZoneMeshHelpers.GetIndexCount(new int2(10, 6));
            num3 = 1;
            x1 = 0;
            x2 = 0;
            bias1 = 0.0f;
            bias2 = 0.0f;
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.MeshData meshData1 = this.m_PrefabMeshData[mesh];
            dynamicBuffer = new DynamicBuffer<LodMesh>();
            bounds1 = RenderingUtils.SafeBounds(meshData1.m_Bounds);
            meshFlags = meshData1.m_State;
            num1 = math.select(meshData1.m_SubMeshCount, meshData1.m_SubMeshCount + 1, (meshFlags & MeshFlags.Base) > (MeshFlags) 0);
            num2 = (float) meshData1.m_IndexCount;
            num3 = 0;
            x1 = type == MeshType.Lane ? (int) partition : (int) meshData1.m_MinLod;
            x2 = (int) meshData1.m_ShadowLod;
            bias1 = meshData1.m_LodBias;
            bias2 = meshData1.m_ShadowBias;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLodMeshes.HasBuffer(mesh))
            {
              // ISSUE: reference to a compiler-generated field
              dynamicBuffer = this.m_PrefabLodMeshes[mesh];
              num3 = dynamicBuffer.Length;
              break;
            }
            break;
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BatchGroup> prefabBatchGroup = this.m_PrefabBatchGroups[mesh];
        int batchCapacity = num1;
        if (dynamicBuffer.IsCreated)
        {
          for (int index = 0; index < dynamicBuffer.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.MeshData meshData2 = this.m_PrefabMeshData[dynamicBuffer[index].m_LodMesh];
            int a = batchCapacity + meshData2.m_SubMeshCount;
            batchCapacity = math.select(a, a + 1, (meshData2.m_State & MeshFlags.Base) > (MeshFlags) 0);
          }
        }
        float3 float3_1 = MathUtils.Center(bounds1);
        float3 float3_2 = MathUtils.Size(bounds1);
        GroupData groupData = new GroupData()
        {
          m_Mesh = mesh,
          m_SecondaryCenter = float3_1,
          m_SecondarySize = float3_2 * 0.4f,
          m_Layer = layer,
          m_MeshType = type,
          m_Partition = partition,
          m_LodCount = (byte) num3
        };
        for (int property = 0; property < 16; ++property)
          groupData.SetPropertyIndex(property, -1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int group = this.m_NativeBatchGroups.CreateGroup(groupData, batchCapacity, this.m_NativeBatchInstances, this.m_NativeSubBatches);
        prefabBatchGroup.Add(new BatchGroup()
        {
          m_GroupIndex = group,
          m_MergeIndex = -1,
          m_Layer = layer,
          m_Type = type,
          m_Partition = partition
        });
        StackDirection stackDirection = StackDirection.None;
        if ((meshFlags & MeshFlags.StackX) != (MeshFlags) 0)
          stackDirection = StackDirection.Right;
        if ((meshFlags & MeshFlags.StackY) != (MeshFlags) 0)
          stackDirection = StackDirection.Up;
        if ((meshFlags & MeshFlags.StackZ) != (MeshFlags) 0)
          stackDirection = StackDirection.Forward;
        float metersPerPixel = 0.0f;
        switch (type)
        {
          case MeshType.Object:
            metersPerPixel = RenderingUtils.GetRenderingSize(float3_2, stackDirection);
            break;
          case MeshType.Net:
            metersPerPixel = RenderingUtils.GetRenderingSize(float3_2.xy);
            x1 = RenderingUtils.CalculateLodLimit(metersPerPixel, bias1);
            x2 = RenderingUtils.CalculateLodLimit(RenderingUtils.GetShadowRenderingSize(float3_2.xy), bias2);
            break;
          case MeshType.Lane:
            metersPerPixel = RenderingUtils.GetRenderingSize(float3_2.xy);
            break;
          case MeshType.Zone:
            metersPerPixel = RenderingUtils.GetRenderingSize(float3_2);
            x1 = RenderingUtils.CalculateLodLimit(metersPerPixel, bias1);
            x2 = RenderingUtils.CalculateLodLimit(metersPerPixel, bias2);
            break;
        }
        int num4 = math.min(x1, (int) byte.MaxValue - num3);
        int x3 = math.clamp(x2, num4, (int) byte.MaxValue);
        BatchData batchData;
        for (int index1 = num3 - 1; index1 >= 0; --index1)
        {
          Entity lodMesh;
          Bounds3 bounds2;
          int num5;
          float indexCount;
          switch (type)
          {
            case MeshType.Net:
              lodMesh = dynamicBuffer[index1].m_LodMesh;
              // ISSUE: reference to a compiler-generated field
              NetCompositionMeshData compositionMeshData2 = this.m_PrefabCompositionMeshData[lodMesh];
              bounds2 = bounds1;
              // ISSUE: reference to a compiler-generated field
              num5 = this.m_PrefabMeshMaterials[lodMesh].Length;
              indexCount = compositionMeshData2.m_IndexFactor;
              break;
            case MeshType.Zone:
              lodMesh = Entity.Null;
              bounds2 = bounds1;
              num5 = 1;
              indexCount = (float) ZoneMeshHelpers.GetIndexCount(new int2(5, 3));
              break;
            default:
              lodMesh = dynamicBuffer[index1].m_LodMesh;
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.MeshData meshData3 = this.m_PrefabMeshData[lodMesh];
              bounds2 = meshData3.m_Bounds;
              num5 = math.select(meshData3.m_SubMeshCount, meshData3.m_SubMeshCount + 1, (meshData3.m_State & MeshFlags.Base) > (MeshFlags) 0);
              indexCount = math.select((float) meshData3.m_IndexCount, num2 * 0.25f, (meshData3.m_State & (MeshFlags.Decal | MeshFlags.Impostor)) > (MeshFlags) 0);
              break;
          }
          for (int index2 = 0; index2 < num5; ++index2)
          {
            batchData = new BatchData();
            batchData.m_LodMesh = lodMesh;
            batchData.m_VTIndex0 = -1;
            batchData.m_VTIndex1 = -1;
            batchData.m_SubMeshIndex = (byte) index2;
            batchData.m_MinLod = (byte) num4;
            batchData.m_ShadowLod = (byte) x3;
            batchData.m_LodIndex = (byte) (index1 + 1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NativeBatchGroups.CreateBatch(batchData, group, 16, this.m_NativeBatchInstances, this.m_NativeSubBatches) < 0)
            {
              UnityEngine.Debug.Log((object) string.Format("Too many batches in group (max: {0})", (object) 16));
              return;
            }
          }
          switch (type)
          {
            case MeshType.Object:
            case MeshType.Zone:
              float3 meshSize = MathUtils.Size(bounds2);
              metersPerPixel = RenderingUtils.GetRenderingSize(float3_2, meshSize, indexCount, stackDirection);
              break;
            case MeshType.Net:
              float indexFactor1 = indexCount;
              metersPerPixel = RenderingUtils.GetRenderingSize(float3_2.xy, indexFactor1);
              break;
            case MeshType.Lane:
              float indexFactor2 = indexCount / math.max(1f, MathUtils.Size(bounds2.z));
              metersPerPixel = RenderingUtils.GetRenderingSize(float3_2.xy, indexFactor2);
              break;
          }
          num4 = math.clamp(RenderingUtils.CalculateLodLimit(metersPerPixel, bias1) + 1, num4 + 1, (int) byte.MaxValue - index1);
          x3 = math.max(x3, num4);
        }
        for (int index = 0; index < num1; ++index)
        {
          batchData = new BatchData();
          batchData.m_VTIndex0 = -1;
          batchData.m_VTIndex1 = -1;
          batchData.m_SubMeshIndex = (byte) index;
          batchData.m_MinLod = (byte) num4;
          batchData.m_ShadowLod = (byte) x3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NativeBatchGroups.CreateBatch(batchData, group, 16, this.m_NativeBatchInstances, this.m_NativeSubBatches) < 0)
          {
            UnityEngine.Debug.Log((object) string.Format("Too many batches in group (max: {0})", (object) 16));
            break;
          }
        }
      }

      private bool IsNetOwnerTunnel(Owner owner)
      {
        Game.Net.Elevation componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetElevationData.TryGetComponent(owner.m_Owner, out componentData) && (double) math.cmin(componentData.m_Elevation) < 0.0)
          return true;
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.TryGetBuffer(owner.m_Owner, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetElevationData.TryGetComponent(bufferData[index].m_Edge, out componentData) && (double) math.cmin(componentData.m_Elevation) < 0.0)
              return true;
          }
        }
        return false;
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Object> __Game_Objects_Object_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Marker> __Game_Objects_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> __Game_Net_Orphan_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Marker> __Game_Net_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> __Game_Zones_Block_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Error> __Game_Tools_Error_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Warning> __Game_Tools_Warning_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Override> __Game_Tools_Override_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Highlighted> __Game_Tools_Highlighted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> __Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RW_BufferLookup;
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RW_BufferLookup;
      public BufferLookup<LodMesh> __Game_Prefabs_LodMesh_RW_BufferLookup;
      public BufferLookup<MeshMaterial> __Game_Prefabs_MeshMaterial_RW_BufferLookup;
      public ComponentLookup<Game.Prefabs.MeshData> __Game_Prefabs_MeshData_RW_ComponentLookup;
      public ComponentLookup<NetCompositionMeshData> __Game_Prefabs_NetCompositionMeshData_RW_ComponentLookup;
      public ComponentLookup<ZoneBlockData> __Game_Prefabs_ZoneBlockData_RW_ComponentLookup;
      public BufferLookup<BatchGroup> __Game_Prefabs_BatchGroup_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Object_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Error_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Error>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Warning_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Warning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Override_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Override>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Highlighted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Highlighted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferTypeHandle = state.GetBufferTypeHandle<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RW_BufferLookup = state.GetBufferLookup<SubMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RW_BufferLookup = state.GetBufferLookup<SubMeshGroup>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LodMesh_RW_BufferLookup = state.GetBufferLookup<LodMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshMaterial_RW_BufferLookup = state.GetBufferLookup<MeshMaterial>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RW_ComponentLookup = state.GetComponentLookup<Game.Prefabs.MeshData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshData_RW_ComponentLookup = state.GetComponentLookup<NetCompositionMeshData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneBlockData_RW_ComponentLookup = state.GetComponentLookup<ZoneBlockData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BatchGroup_RW_BufferLookup = state.GetBufferLookup<BatchGroup>();
      }
    }
  }
}
