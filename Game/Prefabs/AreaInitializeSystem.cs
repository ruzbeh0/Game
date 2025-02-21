// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AreaInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class AreaInitializeSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_SubAreaQuery;
    private EntityQuery m_PlaceholderQuery;
    private AreaInitializeSystem.TypeHandle __TypeHandle;

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
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<AreaData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SubAreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadWrite<SubArea>(), ComponentType.ReadWrite<SubAreaNode>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceholderQuery = this.GetEntityQuery(ComponentType.ReadOnly<AreaData>(), ComponentType.ReadOnly<PlaceholderObjectElement>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_PrefabQuery, this.m_SubAreaQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PrefabQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.InitializeAreaPrefabs();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_SubAreaQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated method
      this.ValidateSubAreas();
    }

    private void InitializeAreaPrefabs()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      bool flag = false;
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
        this.__TypeHandle.__Game_Prefabs_AreaColorData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<AreaColorData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_AreaColorData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<AreaGeometryData> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<LotData> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_DistrictData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<DistrictData> componentTypeHandle6 = this.__TypeHandle.__Game_Prefabs_DistrictData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MapTileData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<MapTileData> componentTypeHandle7 = this.__TypeHandle.__Game_Prefabs_MapTileData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpaceData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<SpaceData> componentTypeHandle8 = this.__TypeHandle.__Game_Prefabs_SpaceData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SurfaceData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<SurfaceData> componentTypeHandle9 = this.__TypeHandle.__Game_Prefabs_SurfaceData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<StorageAreaData> componentTypeHandle10 = this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<ExtractorAreaData> componentTypeHandle11 = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<SpawnableObjectData> componentTypeHandle12 = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubObject> bufferTypeHandle1 = this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubArea> bufferTypeHandle2 = this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
          {
            flag = archetypeChunk.Has<SpawnableObjectData>(ref componentTypeHandle12);
          }
          else
          {
            NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
            NativeArray<AreaColorData> nativeArray2 = archetypeChunk.GetNativeArray<AreaColorData>(ref componentTypeHandle3);
            NativeArray<AreaGeometryData> nativeArray3 = archetypeChunk.GetNativeArray<AreaGeometryData>(ref componentTypeHandle4);
            NativeArray<SpawnableObjectData> nativeArray4 = archetypeChunk.GetNativeArray<SpawnableObjectData>(ref componentTypeHandle12);
            AreaType areaType = AreaType.None;
            GeometryFlags geometryFlags = (GeometryFlags) 0;
            NativeArray<ExtractorAreaData> nativeArray5 = new NativeArray<ExtractorAreaData>();
            if (archetypeChunk.Has<LotData>(ref componentTypeHandle5))
            {
              areaType = AreaType.Lot;
              geometryFlags = GeometryFlags.PhysicalGeometry | GeometryFlags.PseudoRandom;
              if (archetypeChunk.Has<StorageAreaData>(ref componentTypeHandle10))
                geometryFlags |= GeometryFlags.CanOverrideObjects;
              nativeArray5 = archetypeChunk.GetNativeArray<ExtractorAreaData>(ref componentTypeHandle11);
            }
            else if (archetypeChunk.Has<DistrictData>(ref componentTypeHandle6))
            {
              areaType = AreaType.District;
              geometryFlags = GeometryFlags.OnWaterSurface;
            }
            else if (archetypeChunk.Has<MapTileData>(ref componentTypeHandle7))
            {
              areaType = AreaType.MapTile;
              geometryFlags = GeometryFlags.ProtectedArea | GeometryFlags.OnWaterSurface;
            }
            else if (archetypeChunk.Has<SpaceData>(ref componentTypeHandle8))
              areaType = AreaType.Space;
            else if (archetypeChunk.Has<SurfaceData>(ref componentTypeHandle9))
              areaType = AreaType.Surface;
            float minNodeDistance = AreaUtils.GetMinNodeDistance(areaType);
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              AreaPrefab prefab = this.m_PrefabSystem.GetPrefab<AreaPrefab>(nativeArray1[index2]);
              AreaColorData areaColorData = nativeArray2[index2];
              AreaGeometryData areaGeometryData = nativeArray3[index2];
              if (prefab.Has<ClearArea>())
                geometryFlags |= GeometryFlags.ClearArea;
              if (prefab.Has<ClipArea>())
                geometryFlags |= GeometryFlags.ClipTerrain;
              if (nativeArray5.IsCreated && nativeArray5[index2].m_MapFeature != MapFeature.Forest)
                geometryFlags |= GeometryFlags.CanOverrideObjects;
              areaColorData.m_FillColor = (Color32) prefab.m_Color;
              areaColorData.m_EdgeColor = (Color32) prefab.m_EdgeColor;
              areaColorData.m_SelectionFillColor = (Color32) prefab.m_SelectionColor;
              areaColorData.m_SelectionEdgeColor = (Color32) prefab.m_SelectionEdgeColor;
              areaGeometryData.m_Type = areaType;
              areaGeometryData.m_Flags = geometryFlags;
              areaGeometryData.m_SnapDistance = minNodeDistance;
              RenderedArea component;
              if (prefab.TryGet<RenderedArea>(out component))
                areaGeometryData.m_LodBias = component.m_LodBias;
              nativeArray2[index2] = areaColorData;
              nativeArray3[index2] = areaGeometryData;
            }
            BufferAccessor<SubObject> bufferAccessor1 = archetypeChunk.GetBufferAccessor<SubObject>(ref bufferTypeHandle1);
            for (int index3 = 0; index3 < bufferAccessor1.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              AreaSubObjects component = this.m_PrefabSystem.GetPrefab<AreaPrefab>(nativeArray1[index3]).GetComponent<AreaSubObjects>();
              DynamicBuffer<SubObject> dynamicBuffer = bufferAccessor1[index3];
              for (int index4 = 0; index4 < component.m_SubObjects.Length; ++index4)
              {
                ObjectPrefab prefab = component.m_SubObjects[index4].m_Object;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                dynamicBuffer.Add(new SubObject()
                {
                  m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) prefab),
                  m_Position = new float3(),
                  m_Rotation = quaternion.identity
                });
              }
            }
            if (nativeArray4.Length != 0)
            {
              NativeArray<Entity> nativeArray6 = archetypeChunk.GetNativeArray(entityTypeHandle);
              for (int index5 = 0; index5 < nativeArray4.Length; ++index5)
              {
                Entity entity = nativeArray6[index5];
                SpawnableObjectData spawnableObjectData = nativeArray4[index5];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                SpawnableArea component = this.m_PrefabSystem.GetPrefab<AreaPrefab>(nativeArray1[index5]).GetComponent<SpawnableArea>();
                for (int index6 = 0; index6 < component.m_Placeholders.Length; ++index6)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.EntityManager.GetBuffer<PlaceholderObjectElement>(this.m_PrefabSystem.GetEntity((PrefabBase) component.m_Placeholders[index6])).Add(new PlaceholderObjectElement(entity));
                }
                spawnableObjectData.m_Probability = component.m_Probability;
                nativeArray4[index5] = spawnableObjectData;
              }
            }
            BufferAccessor<SubArea> bufferAccessor2 = archetypeChunk.GetBufferAccessor<SubArea>(ref bufferTypeHandle2);
            for (int index7 = 0; index7 < bufferAccessor2.Length; ++index7)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              MasterArea component = this.m_PrefabSystem.GetPrefab<AreaPrefab>(nativeArray1[index7]).GetComponent<MasterArea>();
              DynamicBuffer<SubArea> dynamicBuffer = bufferAccessor2[index7];
              for (int index8 = 0; index8 < component.m_SlaveAreas.Length; ++index8)
              {
                AreaPrefab area = component.m_SlaveAreas[index8].m_Area;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                dynamicBuffer.Add(new SubArea()
                {
                  m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) area),
                  m_NodeRange = (int2) -1
                });
              }
            }
          }
        }
        if (!flag)
          return;
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
        this.Dependency = new AreaInitializeSystem.FixPlaceholdersJob()
        {
          m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_PlaceholderObjectElementType = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle
        }.ScheduleParallel<AreaInitializeSystem.FixPlaceholdersJob>(this.m_PlaceholderQuery, this.Dependency);
      }
      finally
      {
        archetypeChunkArray.Dispose();
      }
    }

    private void ValidateSubAreas()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubAreaNode_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new AreaInitializeSystem.ValidateSubAreasJob()
      {
        m_AreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_SubAreaType = this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle,
        m_SubAreaNodeType = this.__TypeHandle.__Game_Prefabs_SubAreaNode_RW_BufferTypeHandle
      }.ScheduleParallel<AreaInitializeSystem.ValidateSubAreasJob>(this.m_SubAreaQuery, this.Dependency);
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
    public AreaInitializeSystem()
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
    private struct ValidateSubAreasJob : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_AreaGeometryData;
      public BufferTypeHandle<SubArea> m_SubAreaType;
      public BufferTypeHandle<SubAreaNode> m_SubAreaNodeType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubArea> bufferAccessor1 = chunk.GetBufferAccessor<SubArea>(ref this.m_SubAreaType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubAreaNode> bufferAccessor2 = chunk.GetBufferAccessor<SubAreaNode>(ref this.m_SubAreaNodeType);
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          DynamicBuffer<SubArea> dynamicBuffer1 = bufferAccessor1[index1];
          DynamicBuffer<SubAreaNode> dynamicBuffer2 = bufferAccessor2[index1];
          int index2 = 0;
          int index3 = 0;
          for (int index4 = 0; index4 < dynamicBuffer1.Length; ++index4)
          {
            SubArea subArea = dynamicBuffer1[index4];
            int2 nodeRange = subArea.m_NodeRange;
            subArea.m_NodeRange.x = index3;
            AreaGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            if (nodeRange.x != nodeRange.y && this.m_AreaGeometryData.TryGetComponent(subArea.m_Prefab, out componentData))
            {
              float minNodeDistance = AreaUtils.GetMinNodeDistance(componentData);
              SubAreaNode subAreaNode1 = dynamicBuffer2[nodeRange.x];
              dynamicBuffer2[index3++] = subAreaNode1;
              for (int index5 = nodeRange.x + 1; index5 < nodeRange.y; ++index5)
              {
                SubAreaNode subAreaNode2 = dynamicBuffer2[index5];
                if ((double) math.distance(subAreaNode1.m_Position.xz, subAreaNode2.m_Position.xz) >= (double) minNodeDistance)
                {
                  subAreaNode1 = subAreaNode2;
                  dynamicBuffer2[index3++] = subAreaNode2;
                }
              }
              subAreaNode1 = dynamicBuffer2[nodeRange.x];
              for (; index3 > subArea.m_NodeRange.x; --index3)
              {
                SubAreaNode subAreaNode3 = dynamicBuffer2[index3 - 1];
                if ((double) math.distance(subAreaNode1.m_Position.xz, subAreaNode3.m_Position.xz) >= (double) minNodeDistance)
                  break;
              }
            }
            else
            {
              for (int x = nodeRange.x; x < nodeRange.y; ++x)
                dynamicBuffer2[index3++] = dynamicBuffer2[x];
            }
            subArea.m_NodeRange.y = index3;
            int num1 = nodeRange.y - nodeRange.x;
            int num2 = subArea.m_NodeRange.y - subArea.m_NodeRange.x;
            if (num2 < num1)
              Debug.Log((object) string.Format("Invalid prefab sub-area nodes removed: {0} => {1}", (object) num1, (object) num2));
            if (num2 >= 3)
              dynamicBuffer1[index2++] = subArea;
            else
              index3 = subArea.m_NodeRange.x;
          }
          if (index2 < dynamicBuffer1.Length)
            dynamicBuffer1.RemoveRange(index2, dynamicBuffer1.Length - index2);
          if (index3 < dynamicBuffer2.Length)
            dynamicBuffer2.RemoveRange(index3, dynamicBuffer2.Length - index3);
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
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<AreaColorData> __Game_Prefabs_AreaColorData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LotData> __Game_Prefabs_LotData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<DistrictData> __Game_Prefabs_DistrictData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MapTileData> __Game_Prefabs_MapTileData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SpaceData> __Game_Prefabs_SpaceData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SurfaceData> __Game_Prefabs_SurfaceData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StorageAreaData> __Game_Prefabs_StorageAreaData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle;
      public BufferTypeHandle<SubObject> __Game_Prefabs_SubObject_RW_BufferTypeHandle;
      public BufferTypeHandle<SubArea> __Game_Prefabs_SubArea_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      public BufferTypeHandle<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      public BufferTypeHandle<SubAreaNode> __Game_Prefabs_SubAreaNode_RW_BufferTypeHandle;

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
        this.__Game_Prefabs_AreaColorData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AreaColorData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AreaGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LotData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DistrictData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DistrictData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MapTileData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MapTileData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpaceData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SurfaceData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SurfaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageAreaData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StorageAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubArea>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PlaceholderObjectElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubAreaNode_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubAreaNode>();
      }
    }
  }
}
