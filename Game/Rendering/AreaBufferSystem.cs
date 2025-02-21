// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AreaBufferSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Serialization;
using Game.Tools;
using Game.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class AreaBufferSystem : GameSystemBase, IPreDeserialize
  {
    private EntityQuery m_SettingsQuery;
    private RenderingSystem m_RenderingSystem;
    private OverlayRenderSystem m_OverlayRenderSystem;
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private NameSystem m_NameSystem;
    private AreaBufferSystem.AreaTypeData[] m_AreaTypeData;
    private AreaType m_LastSelectionAreaType;
    private EntityQuery m_SelectionQuery;
    private bool m_Loaded;
    private int m_AreaParameters;
    private Dictionary<Entity, string> m_CachedLabels;
    private AreaBufferSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem = this.World.GetOrCreateSystemManaged<OverlayRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTypeData = new AreaBufferSystem.AreaTypeData[5];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaTypeData[0] = this.InitializeAreaData<Lot>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaTypeData[1] = this.InitializeAreaData<District>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaTypeData[2] = this.InitializeAreaData<MapTile>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaTypeData[3] = this.InitializeAreaData<Game.Areas.Space>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaTypeData[4] = this.InitializeAreaData<Surface>();
      // ISSUE: reference to a compiler-generated field
      this.m_SettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<Game.Prefabs.AreaTypeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_SelectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<SelectionInfo>(), ComponentType.ReadOnly<SelectionElement>());
      // ISSUE: reference to a compiler-generated field
      this.m_AreaParameters = Shader.PropertyToID("colossal_AreaParameters");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.localizationManager.onActiveDictionaryChanged += (System.Action) (() => this.EntityManager.AddComponent<Updated>(this.m_AreaTypeData[1].m_AreaQuery));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_AreaTypeData.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaBufferSystem.AreaTypeData areaTypeData = this.m_AreaTypeData[index1];
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_NameMaterials != null)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < areaTypeData.m_NameMaterials.Count; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AreaBufferSystem.MaterialData nameMaterial = areaTypeData.m_NameMaterials[index2];
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) nameMaterial.m_Material != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              UnityEngine.Object.Destroy((UnityEngine.Object) nameMaterial.m_Material);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_BufferData.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_BufferData.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_Bounds.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_Bounds.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) areaTypeData.m_Material != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) areaTypeData.m_Material);
        }
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) areaTypeData.m_NameMesh != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) areaTypeData.m_NameMesh);
        }
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_Buffer != null)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_Buffer.Release();
        }
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_HasNameMeshData)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMeshData.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.localizationManager.onActiveDictionaryChanged -= (System.Action) (() => this.EntityManager.AddComponent<Updated>(this.m_AreaTypeData[1].m_AreaQuery));
      base.OnDestroy();
    }

    private void OnDictionaryChanged()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.AddComponent<Updated>(this.m_AreaTypeData[1].m_AreaQuery);
    }

    private AreaBufferSystem.AreaTypeData InitializeAreaData<T>() where T : struct, IComponentData
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBufferSystem.AreaTypeData areaTypeData = new AreaBufferSystem.AreaTypeData();
      // ISSUE: reference to a compiler-generated field
      areaTypeData.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Area>(),
          ComponentType.ReadOnly<T>(),
          ComponentType.ReadOnly<Game.Areas.Node>(),
          ComponentType.ReadOnly<Triangle>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      areaTypeData.m_AreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<T>(), ComponentType.ReadOnly<Game.Areas.Node>(), ComponentType.ReadOnly<Triangle>(), ComponentType.Exclude<Deleted>());
      return areaTypeData;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_AreaTypeData.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaBufferSystem.AreaTypeData areaTypeData = this.m_AreaTypeData[index];
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_BufferData.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_BufferData.Dispose();
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_BufferData = new NativeList<AreaBufferSystem.AreaTriangleData>();
        }
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_Buffer != null)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_Buffer.Release();
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_Buffer = (ComputeBuffer) null;
        }
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) areaTypeData.m_NameMesh != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) areaTypeData.m_NameMesh);
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMesh = (Mesh) null;
        }
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_HasNameMeshData)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMeshData.Dispose();
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_HasNameMeshData = false;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CachedLabels != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CachedLabels.Clear();
      }
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
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_SettingsQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          NativeArray<PrefabData> nativeArray = archetypeChunkArray[index1].GetNativeArray<PrefabData>(ref componentTypeHandle);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            AreaTypePrefab prefab = this.m_PrefabSystem.GetPrefab<AreaTypePrefab>(nativeArray[index2]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AreaBufferSystem.AreaTypeData areaTypeData = this.m_AreaTypeData[(int) prefab.m_Type];
            float minNodeDistance = AreaUtils.GetMinNodeDistance(prefab.m_Type);
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) areaTypeData.m_Material != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              UnityEngine.Object.Destroy((UnityEngine.Object) areaTypeData.m_Material);
            }
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_Material = new Material(prefab.m_Material);
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_Material.name = "Area buffer (" + prefab.m_Material.name + ")";
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_Material.SetVector(this.m_AreaParameters, new Vector4(minNodeDistance * (1f / 32f), minNodeDistance * 0.25f, minNodeDistance * 2f, 0.0f));
            // ISSUE: reference to a compiler-generated field
            if (areaTypeData.m_NameMaterials != null)
            {
              // ISSUE: reference to a compiler-generated field
              for (int index3 = 0; index3 < areaTypeData.m_NameMaterials.Count; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                AreaBufferSystem.MaterialData nameMaterial = areaTypeData.m_NameMaterials[index3];
                // ISSUE: reference to a compiler-generated field
                if ((UnityEngine.Object) nameMaterial.m_Material != (UnityEngine.Object) null)
                {
                  // ISSUE: reference to a compiler-generated field
                  UnityEngine.Object.Destroy((UnityEngine.Object) nameMaterial.m_Material);
                }
              }
              // ISSUE: reference to a compiler-generated field
              areaTypeData.m_NameMaterials = (List<AreaBufferSystem.MaterialData>) null;
            }
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_OriginalNameMaterial = prefab.m_NameMaterial;
            if ((UnityEngine.Object) prefab.m_NameMaterial != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              areaTypeData.m_NameMaterials = new List<AreaBufferSystem.MaterialData>(1);
            }
          }
        }
        archetypeChunkArray.Dispose();
      }
      JobHandle job0 = new JobHandle();
      AreaType index4 = AreaType.None;
      Entity singletonEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SelectionQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        singletonEntity = this.m_SelectionQuery.GetSingletonEntity();
        index4 = this.EntityManager.GetComponentData<SelectionInfo>(singletonEntity).m_AreaType;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastSelectionAreaType != AreaType.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTypeData[(int) this.m_LastSelectionAreaType].m_BufferDataDirty = true;
      }
      if (index4 != AreaType.None)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTypeData[(int) index4].m_BufferDataDirty = true;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectionAreaType = index4;
      // ISSUE: reference to a compiler-generated field
      for (int index5 = 0; index5 < this.m_AreaTypeData.Length; ++index5)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaBufferSystem.AreaTypeData data = this.m_AreaTypeData[index5];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityQuery entityQuery = loaded ? data.m_AreaQuery : data.m_UpdatedQuery;
        // ISSUE: reference to a compiler-generated field
        if (data.m_BufferDataDirty || !entityQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated field
          if (data.m_AreaQuery.IsEmptyIgnoreFilter)
          {
            // ISSUE: reference to a compiler-generated field
            data.m_BufferDataDirty = false;
            // ISSUE: reference to a compiler-generated field
            data.m_BufferDirty = false;
            // ISSUE: reference to a compiler-generated field
            if (data.m_Buffer != null)
            {
              // ISSUE: reference to a compiler-generated field
              data.m_Buffer.Release();
              // ISSUE: reference to a compiler-generated field
              data.m_Buffer = (ComputeBuffer) null;
            }
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) data.m_NameMesh != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              UnityEngine.Object.Destroy((UnityEngine.Object) data.m_NameMesh);
              // ISSUE: reference to a compiler-generated field
              data.m_NameMesh = (Mesh) null;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            data.m_BufferDataDirty = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (data.m_NameMaterials != null && !entityQuery.IsEmptyIgnoreFilter)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateLabelVertices(data, loaded);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RenderingSystem.hideOverlay)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index6 = 0; index6 < this.m_AreaTypeData.Length; ++index6)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AreaBufferSystem.AreaTypeData areaTypeData = this.m_AreaTypeData[index6];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (areaTypeData.m_BufferDataDirty && (areaTypeData.m_NameMaterials != null || this.m_ToolSystem.activeTool != null && (this.m_ToolSystem.activeTool.requireAreas & (AreaTypeMask) (1 << index6)) != AreaTypeMask.None))
          {
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_BufferDataDirty = false;
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_BufferDirty = true;
            JobHandle outJobHandle;
            // ISSUE: reference to a compiler-generated field
            NativeList<ArchetypeChunk> archetypeChunkListAsync = areaTypeData.m_AreaQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
            NativeList<AreaBufferSystem.ChunkData> nativeList = new NativeList<AreaBufferSystem.ChunkData>(0, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
            // ISSUE: reference to a compiler-generated field
            if (!areaTypeData.m_BufferData.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              areaTypeData.m_BufferData = new NativeList<AreaBufferSystem.AreaTriangleData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
            }
            // ISSUE: reference to a compiler-generated field
            if (!areaTypeData.m_Bounds.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              areaTypeData.m_Bounds = new NativeValue<Bounds3>(Allocator.Persistent);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
            AreaBufferSystem.ResetChunkDataJob jobData1 = new AreaBufferSystem.ResetChunkDataJob()
            {
              m_Chunks = archetypeChunkListAsync,
              m_AreaType = this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle,
              m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
              m_HiddenType = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle,
              m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
              m_ChunkData = nativeList,
              m_AreaTriangleData = areaTypeData.m_BufferData
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_SelectionElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_AreaColorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            AreaBufferSystem.FillMeshDataJob jobData2 = new AreaBufferSystem.FillMeshDataJob()
            {
              m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
              m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
              m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
              m_HiddenType = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle,
              m_AreaType = this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle,
              m_NativeType = this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle,
              m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
              m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
              m_GeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
              m_ColorData = this.__TypeHandle.__Game_Prefabs_AreaColorData_RO_ComponentLookup,
              m_SelectionElements = this.__TypeHandle.__Game_Tools_SelectionElement_RO_BufferLookup,
              m_SelectionEntity = singletonEntity,
              m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
              m_Chunks = archetypeChunkListAsync.AsDeferredJobArray(),
              m_ChunkData = nativeList,
              m_AreaTriangleData = areaTypeData.m_BufferData
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            AreaBufferSystem.CalculateBoundsJob jobData3 = new AreaBufferSystem.CalculateBoundsJob()
            {
              m_ChunkData = nativeList,
              m_Bounds = areaTypeData.m_Bounds
            };
            JobHandle dependsOn1 = JobHandle.CombineDependencies(this.Dependency, outJobHandle);
            JobHandle dependsOn2 = jobData1.Schedule<AreaBufferSystem.ResetChunkDataJob>(dependsOn1);
            JobHandle jobHandle1 = jobData2.Schedule<AreaBufferSystem.FillMeshDataJob, ArchetypeChunk>(archetypeChunkListAsync, 1, dependsOn2);
            JobHandle dependsOn3 = jobHandle1;
            JobHandle jobHandle2 = jobData3.Schedule<AreaBufferSystem.CalculateBoundsJob>(dependsOn3);
            nativeList.Dispose(jobHandle2);
            // ISSUE: reference to a compiler-generated field
            if (areaTypeData.m_NameMaterials != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (!areaTypeData.m_HasNameMeshData)
              {
                // ISSUE: reference to a compiler-generated field
                areaTypeData.m_HasNameMeshData = true;
                // ISSUE: reference to a compiler-generated field
                areaTypeData.m_NameMeshData = Mesh.AllocateWritableMeshData(1);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Prefabs_AreaNameData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Areas_LabelVertex_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
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
              JobHandle job1 = new AreaBufferSystem.FillNameDataJob()
              {
                m_GeometryType = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle,
                m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
                m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
                m_HiddenType = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle,
                m_LabelVertexType = this.__TypeHandle.__Game_Areas_LabelVertex_RO_BufferTypeHandle,
                m_AreaNameData = this.__TypeHandle.__Game_Prefabs_AreaNameData_RO_ComponentLookup,
                m_Chunks = archetypeChunkListAsync,
                m_SubMeshCount = areaTypeData.m_NameMaterials.Count,
                m_NameMeshData = areaTypeData.m_NameMeshData
              }.Schedule<AreaBufferSystem.FillNameDataJob>(dependsOn1);
              archetypeChunkListAsync.Dispose(JobHandle.CombineDependencies(jobHandle1, job1));
              // ISSUE: reference to a compiler-generated field
              areaTypeData.m_DataDependencies = JobHandle.CombineDependencies(jobHandle2, job1);
            }
            else
            {
              archetypeChunkListAsync.Dispose(jobHandle1);
              // ISSUE: reference to a compiler-generated field
              areaTypeData.m_DataDependencies = jobHandle2;
            }
            // ISSUE: reference to a compiler-generated field
            job0 = JobHandle.CombineDependencies(job0, areaTypeData.m_DataDependencies);
          }
        }
      }
      this.Dependency = job0;
    }

    public bool GetAreaBuffer(
      AreaType type,
      out ComputeBuffer buffer,
      out Material material,
      out Bounds bounds)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AreaBufferSystem.AreaTypeData areaTypeData = this.m_AreaTypeData[(int) type];
      // ISSUE: reference to a compiler-generated field
      if (areaTypeData.m_BufferDirty)
      {
        // ISSUE: reference to a compiler-generated field
        areaTypeData.m_BufferDirty = false;
        // ISSUE: reference to a compiler-generated field
        areaTypeData.m_DataDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        areaTypeData.m_DataDependencies = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_BufferData.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (areaTypeData.m_Buffer != null && areaTypeData.m_Buffer.count != areaTypeData.m_BufferData.Length)
          {
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_Buffer.Release();
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_Buffer = (ComputeBuffer) null;
          }
          // ISSUE: reference to a compiler-generated field
          if (areaTypeData.m_BufferData.Length > 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (areaTypeData.m_Buffer == null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              areaTypeData.m_Buffer = new ComputeBuffer(areaTypeData.m_BufferData.Length, sizeof (AreaBufferSystem.AreaTriangleData));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            areaTypeData.m_Buffer.SetData<AreaBufferSystem.AreaTriangleData>(areaTypeData.m_BufferData.AsArray());
          }
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_BufferData.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      buffer = areaTypeData.m_Buffer;
      // ISSUE: reference to a compiler-generated field
      material = areaTypeData.m_Material;
      // ISSUE: reference to a compiler-generated field
      if (areaTypeData.m_Bounds.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        bounds = RenderingUtils.ToBounds(areaTypeData.m_Bounds.value);
      }
      else
        bounds = new Bounds();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return areaTypeData.m_Buffer != null && areaTypeData.m_Buffer.count != 0;
    }

    private void UpdateLabelVertices(AreaBufferSystem.AreaTypeData data, bool isLoaded)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = (isLoaded ? data.m_AreaQuery : data.m_UpdatedQuery).ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        TextMeshPro textMesh = this.m_OverlayRenderSystem.GetTextMesh();
        textMesh.rectTransform.sizeDelta = new Vector2(250f, 100f);
        textMesh.fontSize = 200f;
        textMesh.alignment = TextAlignmentOptions.Center;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Updated> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<BatchesUpdated> componentTypeHandle2 = this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle3 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_LabelExtents_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<LabelExtents> bufferTypeHandle1 = this.__TypeHandle.__Game_Areas_LabelExtents_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_LabelVertex_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<LabelVertex> bufferTypeHandle2 = this.__TypeHandle.__Game_Areas_LabelVertex_RW_BufferTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          if (isLoaded || archetypeChunk.Has<Updated>(ref componentTypeHandle1) || archetypeChunk.Has<BatchesUpdated>(ref componentTypeHandle2))
          {
            NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<Temp> nativeArray2 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle3);
            BufferAccessor<LabelExtents> bufferAccessor1 = archetypeChunk.GetBufferAccessor<LabelExtents>(ref bufferTypeHandle1);
            BufferAccessor<LabelVertex> bufferAccessor2 = archetypeChunk.GetBufferAccessor<LabelVertex>(ref bufferTypeHandle2);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              DynamicBuffer<LabelExtents> dynamicBuffer1 = bufferAccessor1[index2];
              DynamicBuffer<LabelVertex> dynamicBuffer2 = bufferAccessor2[index2];
              string renderedLabelName;
              if (nativeArray2.Length != 0)
              {
                Temp temp = nativeArray2[index2];
                if (temp.m_Original != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  renderedLabelName = this.m_NameSystem.GetRenderedLabelName(temp.m_Original);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CachedLabels != null && this.m_CachedLabels.ContainsKey(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CachedLabels.Remove(entity);
                  }
                  dynamicBuffer2.Clear();
                  continue;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                renderedLabelName = this.m_NameSystem.GetRenderedLabelName(entity);
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_CachedLabels != null)
              {
                string str;
                // ISSUE: reference to a compiler-generated field
                if (this.m_CachedLabels.TryGetValue(entity, out str))
                {
                  if (!(str == renderedLabelName))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CachedLabels[entity] = renderedLabelName;
                  }
                  else
                    continue;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CachedLabels.Add(entity, renderedLabelName);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CachedLabels = new Dictionary<Entity, string>();
                // ISSUE: reference to a compiler-generated field
                this.m_CachedLabels.Add(entity, renderedLabelName);
              }
              TMP_TextInfo textInfo = textMesh.GetTextInfo(renderedLabelName);
              int length = 0;
              for (int index3 = 0; index3 < textInfo.meshInfo.Length; ++index3)
              {
                TMP_MeshInfo tmpMeshInfo = textInfo.meshInfo[index3];
                length += tmpMeshInfo.vertexCount;
              }
              dynamicBuffer2.ResizeUninitialized(length);
              int num1 = 0;
              for (int index4 = 0; index4 < textInfo.meshInfo.Length; ++index4)
              {
                TMP_MeshInfo tmpMeshInfo = textInfo.meshInfo[index4];
                if (tmpMeshInfo.vertexCount != 0)
                {
                  Texture mainTexture = tmpMeshInfo.material.mainTexture;
                  int num2 = -1;
                  // ISSUE: reference to a compiler-generated field
                  for (int index5 = 0; index5 < data.m_NameMaterials.Count; ++index5)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((UnityEngine.Object) data.m_NameMaterials[index5].m_Material.mainTexture == (UnityEngine.Object) mainTexture)
                    {
                      num2 = index5;
                      break;
                    }
                  }
                  if (num2 == -1)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    AreaBufferSystem.MaterialData materialData = new AreaBufferSystem.MaterialData();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material = new Material(data.m_OriginalNameMaterial);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayRenderSystem.CopyFontAtlasParameters(tmpMeshInfo.material, materialData.m_Material);
                    // ISSUE: reference to a compiler-generated field
                    num2 = data.m_NameMaterials.Count;
                    // ISSUE: reference to a compiler-generated field
                    data.m_NameMaterials.Add(materialData);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material.name = string.Format("Area names {0} ({1})", (object) num2, (object) data.m_OriginalNameMaterial.name);
                  }
                  Vector3[] vertices = tmpMeshInfo.vertices;
                  Vector2[] uvs0 = tmpMeshInfo.uvs0;
                  Vector2[] uvs2 = tmpMeshInfo.uvs2;
                  Color32[] colors32 = tmpMeshInfo.colors32;
                  for (int index6 = 0; index6 < tmpMeshInfo.vertexCount; ++index6)
                  {
                    LabelVertex labelVertex;
                    labelVertex.m_Position = (float3) vertices[index6];
                    labelVertex.m_Color = colors32[index6];
                    labelVertex.m_UV0 = (float2) uvs0[index6];
                    labelVertex.m_UV1 = (float2) uvs2[index6];
                    labelVertex.m_Material = num2;
                    dynamicBuffer2[num1 + index6] = labelVertex;
                  }
                  num1 += tmpMeshInfo.vertexCount;
                }
              }
              dynamicBuffer1.ResizeUninitialized(textInfo.lineCount);
              for (int index7 = 0; index7 < textInfo.lineCount; ++index7)
              {
                Extents lineExtents = textInfo.lineInfo[index7].lineExtents;
                dynamicBuffer1[index7] = new LabelExtents((float2) lineExtents.min, (float2) lineExtents.max);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CachedLabels != null)
            {
              NativeArray<Entity> nativeArray = archetypeChunk.GetNativeArray(entityTypeHandle);
              for (int index8 = 0; index8 < nativeArray.Length; ++index8)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CachedLabels.Remove(nativeArray[index8]);
              }
            }
          }
        }
      }
    }

    public bool GetNameMesh(AreaType type, out Mesh mesh, out int subMeshCount)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AreaBufferSystem.AreaTypeData areaTypeData = this.m_AreaTypeData[(int) type];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      subMeshCount = areaTypeData.m_NameMaterials == null ? 0 : areaTypeData.m_NameMaterials.Count;
      // ISSUE: reference to a compiler-generated field
      if (areaTypeData.m_HasNameMeshData)
      {
        // ISSUE: reference to a compiler-generated field
        areaTypeData.m_HasNameMeshData = false;
        // ISSUE: reference to a compiler-generated field
        areaTypeData.m_DataDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        areaTypeData.m_DataDependencies = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) areaTypeData.m_NameMesh == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMesh = new Mesh();
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMesh.name = string.Format("Area names ({0})", (object) type);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Mesh.ApplyAndDisposeWritableMeshData(areaTypeData.m_NameMeshData, areaTypeData.m_NameMesh, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (areaTypeData.m_Bounds.IsCreated && math.all(areaTypeData.m_Bounds.value.max >= areaTypeData.m_Bounds.value.min))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMesh.bounds = RenderingUtils.ToBounds(areaTypeData.m_Bounds.value);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMesh.RecalculateBounds();
        }
        // ISSUE: reference to a compiler-generated field
        areaTypeData.m_HasNameMesh = false;
        for (int index = 0; index < subMeshCount; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AreaBufferSystem.MaterialData nameMaterial = areaTypeData.m_NameMaterials[index];
          // ISSUE: reference to a compiler-generated field
          SubMeshDescriptor subMesh = areaTypeData.m_NameMesh.GetSubMesh(index);
          // ISSUE: reference to a compiler-generated field
          nameMaterial.m_HasMesh = subMesh.vertexCount > 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_HasNameMesh |= nameMaterial.m_HasMesh;
          // ISSUE: reference to a compiler-generated field
          areaTypeData.m_NameMaterials[index] = nameMaterial;
        }
      }
      // ISSUE: reference to a compiler-generated field
      mesh = areaTypeData.m_NameMesh;
      // ISSUE: reference to a compiler-generated field
      return areaTypeData.m_HasNameMesh;
    }

    public bool GetNameMaterial(AreaType type, int subMeshIndex, out Material material)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AreaBufferSystem.MaterialData nameMaterial = this.m_AreaTypeData[(int) type].m_NameMaterials[subMeshIndex];
      // ISSUE: reference to a compiler-generated field
      material = nameMaterial.m_Material;
      // ISSUE: reference to a compiler-generated field
      return nameMaterial.m_HasMesh;
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
    public AreaBufferSystem()
    {
    }

    private struct AreaTriangleData
    {
      public Vector3 m_APos;
      public Vector3 m_BPos;
      public Vector3 m_CPos;
      public Vector2 m_APrevXZ;
      public Vector2 m_BPrevXZ;
      public Vector2 m_CPrevXZ;
      public Vector2 m_ANextXZ;
      public Vector2 m_BNextXZ;
      public Vector2 m_CNextXZ;
      public Vector2 m_YMinMax;
      public Vector4 m_FillColor;
      public Vector4 m_EdgeColor;
    }

    private struct MaterialData
    {
      public Material m_Material;
      public bool m_HasMesh;
    }

    private class AreaTypeData
    {
      public EntityQuery m_UpdatedQuery;
      public EntityQuery m_AreaQuery;
      public NativeList<AreaBufferSystem.AreaTriangleData> m_BufferData;
      public NativeValue<Bounds3> m_Bounds;
      public JobHandle m_DataDependencies;
      public ComputeBuffer m_Buffer;
      public Material m_Material;
      public Material m_OriginalNameMaterial;
      public List<AreaBufferSystem.MaterialData> m_NameMaterials;
      public Mesh m_NameMesh;
      public Mesh.MeshDataArray m_NameMeshData;
      public bool m_BufferDataDirty;
      public bool m_BufferDirty;
      public bool m_HasNameMeshData;
      public bool m_HasNameMesh;
    }

    private struct ChunkData
    {
      public int m_TriangleOffset;
      public Bounds3 m_Bounds;
    }

    [BurstCompile]
    private struct ResetChunkDataJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Area> m_AreaType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> m_HiddenType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      public NativeList<AreaBufferSystem.ChunkData> m_ChunkData;
      public NativeList<AreaBufferSystem.AreaTriangleData> m_AreaTriangleData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ChunkData.ResizeUninitialized(this.m_Chunks.Length);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaBufferSystem.ChunkData chunkData = new AreaBufferSystem.ChunkData();
        // ISSUE: reference to a compiler-generated field
        chunkData.m_Bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ChunkData[index1] = chunkData;
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Area> nativeArray1 = chunk.GetNativeArray<Area>(ref this.m_AreaType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Triangle> bufferAccessor = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
            for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
            {
              if ((nativeArray1[index2].m_Flags & AreaFlags.Slave) == (AreaFlags) 0 && (nativeArray2.Length == 0 || (nativeArray2[index2].m_Flags & TempFlags.Hidden) == (TempFlags) 0))
              {
                DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor[index2];
                // ISSUE: reference to a compiler-generated field
                chunkData.m_TriangleOffset += dynamicBuffer.Length;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTriangleData.ResizeUninitialized(chunkData.m_TriangleOffset);
      }
    }

    [BurstCompile]
    private struct FillMeshDataJob : IJobParallelForDefer
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> m_HiddenType;
      [ReadOnly]
      public ComponentTypeHandle<Area> m_AreaType;
      [ReadOnly]
      public ComponentTypeHandle<Native> m_NativeType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_GeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.AreaColorData> m_ColorData;
      [ReadOnly]
      public BufferLookup<SelectionElement> m_SelectionElements;
      [ReadOnly]
      public Entity m_SelectionEntity;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [NativeDisableParallelForRestriction]
      public NativeList<AreaBufferSystem.ChunkData> m_ChunkData;
      [NativeDisableParallelForRestriction]
      public NativeList<AreaBufferSystem.AreaTriangleData> m_AreaTriangleData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Hidden>(ref this.m_HiddenType))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaBufferSystem.ChunkData chunkData = this.m_ChunkData[index];
        // ISSUE: reference to a compiler-generated field
        NativeArray<Area> nativeArray1 = chunk.GetNativeArray<Area>(ref this.m_AreaType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_EditorMode || chunk.Has<Native>(ref this.m_NativeType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectionElements.HasBuffer(this.m_SelectionEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SelectionElement> selectionElement = this.m_SelectionElements[this.m_SelectionEntity];
          NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(selectionElement.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index1 = 0; index1 < selectionElement.Length; ++index1)
            nativeParallelHashSet.Add(selectionElement[index1].m_Entity);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray4 = chunk.GetNativeArray(this.m_EntityType);
          for (int index2 = 0; index2 < nativeArray4.Length; ++index2)
          {
            if ((nativeArray1[index2].m_Flags & AreaFlags.Slave) == (AreaFlags) 0)
            {
              PrefabRef prefabRef = nativeArray3[index2];
              DynamicBuffer<Game.Areas.Node> nodes = bufferAccessor1[index2];
              DynamicBuffer<Triangle> triangles = bufferAccessor2[index2];
              // ISSUE: reference to a compiler-generated field
              AreaGeometryData geometryData = this.m_GeometryData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.AreaColorData areaColorData = this.m_ColorData[prefabRef.m_Prefab];
              Entity original;
              if (nativeArray2.Length != 0)
              {
                Temp temp = nativeArray2[index2];
                if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
                  original = temp.m_Original;
                else
                  continue;
              }
              else
                original = nativeArray4[index2];
              Color color1;
              Color color2;
              Color color3;
              if (nativeParallelHashSet.Contains(original))
              {
                color1 = (Color) areaColorData.m_SelectionFillColor;
                color2 = color1.linear;
                color1 = (Color) areaColorData.m_SelectionEdgeColor;
                color3 = color1.linear;
              }
              else
              {
                color1 = (Color) areaColorData.m_FillColor;
                color2 = color1.linear;
                color1 = (Color) areaColorData.m_EdgeColor;
                color3 = color1.linear;
              }
              if (!flag)
              {
                // ISSUE: reference to a compiler-generated method
                color2 = AreaBufferSystem.FillMeshDataJob.GetDisabledColor(color2);
                // ISSUE: reference to a compiler-generated method
                color3 = AreaBufferSystem.FillMeshDataJob.GetDisabledColor(color3);
              }
              // ISSUE: reference to a compiler-generated method
              this.AddTriangles(nodes, triangles, (Vector4) color2, (Vector4) color3, geometryData, ref chunkData);
            }
          }
          nativeParallelHashSet.Dispose();
        }
        else
        {
          for (int index3 = 0; index3 < bufferAccessor1.Length; ++index3)
          {
            if ((nativeArray1[index3].m_Flags & AreaFlags.Slave) == (AreaFlags) 0 && (nativeArray2.Length == 0 || (nativeArray2[index3].m_Flags & TempFlags.Hidden) == (TempFlags) 0))
            {
              PrefabRef prefabRef = nativeArray3[index3];
              DynamicBuffer<Game.Areas.Node> nodes = bufferAccessor1[index3];
              DynamicBuffer<Triangle> triangles = bufferAccessor2[index3];
              // ISSUE: reference to a compiler-generated field
              AreaGeometryData geometryData = this.m_GeometryData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.AreaColorData areaColorData = this.m_ColorData[prefabRef.m_Prefab];
              Color color4 = ((Color) areaColorData.m_FillColor).linear;
              Color color5 = ((Color) areaColorData.m_EdgeColor).linear;
              if (!flag)
              {
                // ISSUE: reference to a compiler-generated method
                color4 = AreaBufferSystem.FillMeshDataJob.GetDisabledColor(color4);
                // ISSUE: reference to a compiler-generated method
                color5 = AreaBufferSystem.FillMeshDataJob.GetDisabledColor(color5);
              }
              // ISSUE: reference to a compiler-generated method
              this.AddTriangles(nodes, triangles, (Vector4) color4, (Vector4) color5, geometryData, ref chunkData);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ChunkData[index] = chunkData;
      }

      private static Color GetDisabledColor(Color color)
      {
        color.a *= 0.25f;
        return color;
      }

      private void AddTriangles(
        DynamicBuffer<Game.Areas.Node> nodes,
        DynamicBuffer<Triangle> triangles,
        Vector4 fillColor,
        Vector4 edgeColor,
        AreaGeometryData geometryData,
        ref AreaBufferSystem.ChunkData chunkData)
      {
        for (int index = 0; index < triangles.Length; ++index)
        {
          Triangle triangle = triangles[index];
          Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangle);
          Bounds3 bounds3 = MathUtils.Bounds(triangle3);
          int3 int3_1 = math.select(triangle.m_Indices - 1, (int3) (nodes.Length - 1), triangle.m_Indices == 0);
          int3 int3_2 = math.select(triangle.m_Indices + 1, (int3) 0, triangle.m_Indices == nodes.Length - 1);
          bounds3.min.y += triangle.m_HeightRange.min - geometryData.m_SnapDistance * 2f;
          bounds3.max.y += triangle.m_HeightRange.max + geometryData.m_SnapDistance * 2f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_AreaTriangleData[chunkData.m_TriangleOffset++] = new AreaBufferSystem.AreaTriangleData()
          {
            m_APos = (Vector3) triangle3.a,
            m_BPos = (Vector3) triangle3.b,
            m_CPos = (Vector3) triangle3.c,
            m_APrevXZ = (Vector2) nodes[int3_1.x].m_Position.xz,
            m_BPrevXZ = (Vector2) nodes[int3_1.y].m_Position.xz,
            m_CPrevXZ = (Vector2) nodes[int3_1.z].m_Position.xz,
            m_ANextXZ = (Vector2) nodes[int3_2.x].m_Position.xz,
            m_BNextXZ = (Vector2) nodes[int3_2.y].m_Position.xz,
            m_CNextXZ = (Vector2) nodes[int3_2.z].m_Position.xz,
            m_YMinMax = {
              x = bounds3.min.y,
              y = bounds3.max.y
            },
            m_FillColor = fillColor,
            m_EdgeColor = edgeColor
          };
          // ISSUE: reference to a compiler-generated field
          chunkData.m_Bounds |= bounds3;
        }
      }
    }

    [BurstCompile]
    private struct CalculateBoundsJob : IJob
    {
      [ReadOnly]
      public NativeList<AreaBufferSystem.ChunkData> m_ChunkData;
      public NativeValue<Bounds3> m_Bounds;

      public void Execute()
      {
        Bounds3 bounds3 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ChunkData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bounds3 |= this.m_ChunkData[index].m_Bounds;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds.value = bounds3;
      }
    }

    private struct LabelVertexData
    {
      public Vector3 m_Position;
      public Color32 m_Color;
      public Vector2 m_UV0;
      public Vector2 m_UV1;
      public Vector3 m_UV2;
    }

    [BurstCompile]
    private struct FillNameDataJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<Geometry> m_GeometryType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> m_HiddenType;
      [ReadOnly]
      public BufferTypeHandle<LabelVertex> m_LabelVertexType;
      [ReadOnly]
      public ComponentLookup<AreaNameData> m_AreaNameData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public int m_SubMeshCount;
      public Mesh.MeshDataArray m_NameMeshData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<int> array = new NativeArray<int>(this.m_SubMeshCount, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<LabelVertex> bufferAccessor = chunk.GetBufferAccessor<LabelVertex>(ref this.m_LabelVertexType);
            for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
            {
              DynamicBuffer<LabelVertex> dynamicBuffer = bufferAccessor[index2];
              for (int index3 = 0; index3 < dynamicBuffer.Length; index3 += 4)
              {
                int material = dynamicBuffer[index3].m_Material;
                array.ElementAt<int>(material) += 4;
              }
            }
          }
        }
        int vertexCount = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SubMeshCount; ++index)
          vertexCount += array[index];
        // ISSUE: reference to a compiler-generated field
        Mesh.MeshData meshData = this.m_NameMeshData[0];
        NativeArray<VertexAttributeDescriptor> attributes = new NativeArray<VertexAttributeDescriptor>(5, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        attributes[0] = new VertexAttributeDescriptor();
        attributes[1] = new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4);
        attributes[2] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 2);
        attributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord1, dimension: 2);
        attributes[4] = new VertexAttributeDescriptor(VertexAttribute.TexCoord2);
        meshData.SetVertexBufferParams(vertexCount, attributes);
        meshData.SetIndexBufferParams((vertexCount >> 2) * 6, IndexFormat.UInt32);
        attributes.Dispose();
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        meshData.subMeshCount = this.m_SubMeshCount;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SubMeshCount; ++index)
        {
          ref int local = ref array.ElementAt<int>(index);
          meshData.SetSubMesh(index, new SubMeshDescriptor()
          {
            firstVertex = num1,
            indexStart = (num1 >> 2) * 6,
            vertexCount = local,
            indexCount = (local >> 2) * 6,
            topology = MeshTopology.Triangles
          }, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
          num1 += local;
          local = 0;
        }
        NativeArray<AreaBufferSystem.LabelVertexData> vertexData = meshData.GetVertexData<AreaBufferSystem.LabelVertexData>();
        NativeArray<uint> indexData = meshData.GetIndexData<uint>();
        // ISSUE: reference to a compiler-generated field
        for (int index4 = 0; index4 < this.m_Chunks.Length; ++index4)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index4];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Geometry> nativeArray1 = chunk.GetNativeArray<Geometry>(ref this.m_GeometryType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<LabelVertex> bufferAccessor = chunk.GetBufferAccessor<LabelVertex>(ref this.m_LabelVertexType);
            for (int index5 = 0; index5 < bufferAccessor.Length; ++index5)
            {
              Geometry geometry = nativeArray1[index5];
              PrefabRef prefabRef = nativeArray2[index5];
              DynamicBuffer<LabelVertex> dynamicBuffer = bufferAccessor[index5];
              // ISSUE: reference to a compiler-generated field
              AreaNameData areaNameData = this.m_AreaNameData[prefabRef.m_Prefab];
              float3 labelPosition = AreaUtils.CalculateLabelPosition(geometry);
              Color32 color32 = areaNameData.m_Color;
              if (nativeArray3.Length != 0 && (nativeArray3[index5].m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace)) != (TempFlags) 0)
                color32 = areaNameData.m_SelectedColor;
              SubMeshDescriptor subMeshDescriptor = new SubMeshDescriptor();
              int num2 = -1;
              for (int index6 = 0; index6 < dynamicBuffer.Length; index6 += 4)
              {
                int material = dynamicBuffer[index6].m_Material;
                ref int local = ref array.ElementAt<int>(material);
                if (material != num2)
                {
                  subMeshDescriptor = meshData.GetSubMesh(material);
                  num2 = material;
                }
                int num3 = subMeshDescriptor.firstVertex + local;
                int index7 = subMeshDescriptor.indexStart + (local >> 2) * 6;
                local += 4;
                indexData[index7] = (uint) num3;
                indexData[index7 + 1] = (uint) (num3 + 1);
                indexData[index7 + 2] = (uint) (num3 + 2);
                indexData[index7 + 3] = (uint) (num3 + 2);
                indexData[index7 + 4] = (uint) (num3 + 3);
                indexData[index7 + 5] = (uint) num3;
                for (int index8 = 0; index8 < 4; ++index8)
                {
                  LabelVertex labelVertex = dynamicBuffer[index6 + index8];
                  // ISSUE: variable of a compiler-generated type
                  AreaBufferSystem.LabelVertexData labelVertexData;
                  // ISSUE: reference to a compiler-generated field
                  labelVertexData.m_Position = (Vector3) labelVertex.m_Position;
                  // ISSUE: reference to a compiler-generated field
                  labelVertexData.m_Color = new Color32((byte) ((int) labelVertex.m_Color.r * (int) color32.r >> 8), (byte) ((int) labelVertex.m_Color.g * (int) color32.g >> 8), (byte) ((int) labelVertex.m_Color.b * (int) color32.b >> 8), (byte) ((int) labelVertex.m_Color.a * (int) color32.a >> 8));
                  // ISSUE: reference to a compiler-generated field
                  labelVertexData.m_UV0 = (Vector2) labelVertex.m_UV0;
                  // ISSUE: reference to a compiler-generated field
                  labelVertexData.m_UV1 = (Vector2) labelVertex.m_UV1;
                  // ISSUE: reference to a compiler-generated field
                  labelVertexData.m_UV2 = (Vector3) labelPosition;
                  vertexData[num3 + index8] = labelVertexData;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SubMeshCount; ++index)
        {
          SubMeshDescriptor subMesh = meshData.GetSubMesh(index);
          meshData.SetSubMesh(index, subMesh);
        }
        array.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Area> __Game_Areas_Area_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> __Game_Tools_Hidden_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Triangle> __Game_Areas_Triangle_RO_BufferTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Native> __Game_Common_Native_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.AreaColorData> __Game_Prefabs_AreaColorData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SelectionElement> __Game_Tools_SelectionElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Geometry> __Game_Areas_Geometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LabelVertex> __Game_Areas_LabelVertex_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<AreaNameData> __Game_Prefabs_AreaNameData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Updated> __Game_Common_Updated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BatchesUpdated> __Game_Common_BatchesUpdated_RO_ComponentTypeHandle;
      public BufferTypeHandle<LabelExtents> __Game_Areas_LabelExtents_RW_BufferTypeHandle;
      public BufferTypeHandle<LabelVertex> __Game_Areas_LabelVertex_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferTypeHandle = state.GetBufferTypeHandle<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaColorData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.AreaColorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_SelectionElement_RO_BufferLookup = state.GetBufferLookup<SelectionElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_LabelVertex_RO_BufferTypeHandle = state.GetBufferTypeHandle<LabelVertex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaNameData_RO_ComponentLookup = state.GetComponentLookup<AreaNameData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BatchesUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_LabelExtents_RW_BufferTypeHandle = state.GetBufferTypeHandle<LabelExtents>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_LabelVertex_RW_BufferTypeHandle = state.GetBufferTypeHandle<LabelVertex>();
      }
    }
  }
}
