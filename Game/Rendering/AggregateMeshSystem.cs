// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AggregateMeshSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Serialization;
using Game.Tools;
using Game.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Burst;
using Unity.Burst.Intrinsics;
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
  public class AggregateMeshSystem : GameSystemBase, IPreDeserialize
  {
    private EntityQuery m_CreatedPrefabQuery;
    private EntityQuery m_UpdatedLabelQuery;
    private EntityQuery m_LabelQuery;
    private EntityQuery m_UpdatedArrowQuery;
    private EntityQuery m_ArrowQuery;
    private EntityQuery m_TempAggregatedQuery;
    private OverlayRenderSystem m_OverlayRenderSystem;
    private UndergroundViewSystem m_UndergroundViewSystem;
    private PrefabSystem m_PrefabSystem;
    private NameSystem m_NameSystem;
    private ToolSystem m_ToolSystem;
    private List<AggregateMeshSystem.MeshData> m_LabelData;
    private List<AggregateMeshSystem.MeshData> m_ArrowData;
    private Dictionary<Entity, string> m_CachedLabels;
    private int m_FaceColor;
    private bool m_TunnelSelectOn;
    private bool m_Loaded;
    private AggregateMeshSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem = this.World.GetOrCreateSystemManaged<OverlayRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UndergroundViewSystem = this.World.GetOrCreateSystemManaged<UndergroundViewSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedPrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<AggregateNetData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<NetNameData>(),
          ComponentType.ReadOnly<NetArrowData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedLabelQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Aggregate>(),
          ComponentType.ReadOnly<LabelMaterial>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LabelQuery = this.GetEntityQuery(ComponentType.ReadOnly<Aggregate>(), ComponentType.ReadOnly<LabelMaterial>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedArrowQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Aggregate>(),
          ComponentType.ReadOnly<ArrowMaterial>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ArrowQuery = this.GetEntityQuery(ComponentType.ReadOnly<Aggregate>(), ComponentType.ReadOnly<ArrowMaterial>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempAggregatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Aggregated>(), ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_FaceColor = Shader.PropertyToID("_FaceColor");
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.localizationManager.onActiveDictionaryChanged += (System.Action) (() => this.EntityManager.AddComponent<Updated>(this.m_LabelQuery));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DestroyMeshData(this.m_LabelData);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DestroyMeshData(this.m_ArrowData);
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.localizationManager.onActiveDictionaryChanged -= (System.Action) (() => this.EntityManager.AddComponent<Updated>(this.m_LabelQuery));
      base.OnDestroy();
    }

    private void DestroyMeshData(List<AggregateMeshSystem.MeshData> meshData)
    {
      if (meshData == null)
        return;
      for (int index1 = 0; index1 < meshData.Count; ++index1)
      {
        // ISSUE: variable of a compiler-generated type
        AggregateMeshSystem.MeshData meshData1 = meshData[index1];
        // ISSUE: reference to a compiler-generated field
        if (meshData1.m_Materials != null)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < meshData1.m_Materials.Count; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AggregateMeshSystem.MaterialData material = meshData1.m_Materials[index2];
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) material.m_Material != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              UnityEngine.Object.Destroy((UnityEngine.Object) material.m_Material);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) meshData1.m_Mesh != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) meshData1.m_Mesh);
        }
        // ISSUE: reference to a compiler-generated field
        if (meshData1.m_HasMeshData)
        {
          // ISSUE: reference to a compiler-generated field
          meshData1.m_MeshData.Dispose();
        }
      }
      meshData.Clear();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ClearMeshData(this.m_LabelData);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ClearMeshData(this.m_ArrowData);
      // ISSUE: reference to a compiler-generated field
      if (this.m_CachedLabels != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CachedLabels.Clear();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private void UpdateUndergroundState(
      List<AggregateMeshSystem.MeshData> meshData,
      bool undergroundOn)
    {
      if (meshData == null)
        return;
      for (int index1 = 0; index1 < meshData.Count; ++index1)
      {
        // ISSUE: variable of a compiler-generated type
        AggregateMeshSystem.MeshData meshData1 = meshData[index1];
        // ISSUE: reference to a compiler-generated field
        if (meshData1.m_Materials != null)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < meshData1.m_Materials.Count; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AggregateMeshSystem.MaterialData material = meshData1.m_Materials[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!material.m_IsUnderground && (UnityEngine.Object) material.m_Material != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              material.m_Material.SetColor(this.m_FaceColor, new Color(1f, 1f, 1f, undergroundOn ? 0.25f : 1f));
            }
          }
        }
      }
    }

    private void OnDictionaryChanged()
    {
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.AddComponent<Updated>(this.m_LabelQuery);
    }

    private void ClearMeshData(List<AggregateMeshSystem.MeshData> meshData)
    {
      if (meshData == null)
        return;
      for (int index1 = 0; index1 < meshData.Count; ++index1)
      {
        // ISSUE: variable of a compiler-generated type
        AggregateMeshSystem.MeshData meshData1 = meshData[index1];
        // ISSUE: reference to a compiler-generated field
        if (meshData1.m_Materials != null)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < meshData1.m_Materials.Count; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AggregateMeshSystem.MaterialData material = meshData1.m_Materials[index2] with
            {
              m_HasMesh = false
            };
            // ISSUE: reference to a compiler-generated field
            meshData1.m_Materials[index2] = material;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) meshData1.m_Mesh != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) meshData1.m_Mesh);
          // ISSUE: reference to a compiler-generated field
          meshData1.m_Mesh = (Mesh) null;
        }
        // ISSUE: reference to a compiler-generated field
        if (meshData1.m_HasMeshData)
        {
          // ISSUE: reference to a compiler-generated field
          meshData1.m_MeshData.Dispose();
          // ISSUE: reference to a compiler-generated field
          meshData1.m_HasMeshData = false;
        }
        // ISSUE: reference to a compiler-generated field
        meshData1.m_HasMesh = false;
      }
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
      if (!this.m_CreatedPrefabQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.InitializePrefabs();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery1 = loaded ? this.m_LabelQuery : this.m_UpdatedLabelQuery;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery2 = loaded ? this.m_ArrowQuery : this.m_UpdatedArrowQuery;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool undergroundOn = this.m_UndergroundViewSystem.undergroundOn && this.m_UndergroundViewSystem.tunnelsOn;
      bool flag1 = !entityQuery1.IsEmptyIgnoreFilter;
      bool flag2 = !entityQuery2.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      if (undergroundOn != this.m_TunnelSelectOn)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateUndergroundState(this.m_LabelData, undergroundOn);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateUndergroundState(this.m_ArrowData, undergroundOn);
        // ISSUE: reference to a compiler-generated field
        this.m_TunnelSelectOn = undergroundOn;
      }
      if (!(flag1 | flag2))
        return;
      JobHandle dependency = this.Dependency;
      JobHandle job0 = new JobHandle();
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateLabelVertices(loaded);
        // ISSUE: reference to a compiler-generated method
        JobHandle inputDeps = this.UpdateLabelPositions(dependency, loaded);
        // ISSUE: reference to a compiler-generated method
        job0 = JobHandle.CombineDependencies(job0, this.FillNameMeshData(inputDeps));
      }
      if (flag2)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateArrowMaterials(loaded);
        // ISSUE: reference to a compiler-generated method
        JobHandle inputDeps = this.UpdateArrowPositions(dependency, loaded);
        // ISSUE: reference to a compiler-generated method
        job0 = JobHandle.CombineDependencies(job0, this.FillArrowMeshData(inputDeps));
      }
      this.Dependency = job0;
    }

    private void InitializePrefabs()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_CreatedPrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_UndergroundViewSystem.undergroundOn && this.m_UndergroundViewSystem.tunnelsOn;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetNameData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetNameData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_NetNameData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetArrowData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetArrowData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_NetArrowData_RW_ComponentTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
          NativeArray<NetNameData> nativeArray2 = archetypeChunk.GetNativeArray<NetNameData>(ref componentTypeHandle2);
          NativeArray<NetArrowData> nativeArray3 = archetypeChunk.GetNativeArray<NetArrowData>(ref componentTypeHandle3);
label_24:
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            AggregateNetPrefab prefab = this.m_PrefabSystem.GetPrefab<AggregateNetPrefab>(nativeArray1[index2]);
            NetLabel component1 = prefab.GetComponent<NetLabel>();
            NetArrow component2 = prefab.GetComponent<NetArrow>();
            if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.m_NameMaterial != (UnityEngine.Object) null)
            {
              NetNameData netNameData = nativeArray2[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_LabelData != null)
              {
                // ISSUE: reference to a compiler-generated field
                for (int index3 = 0; index3 < this.m_LabelData.Count; ++index3)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((UnityEngine.Object) this.m_LabelData[index3].m_OriginalMaterial == (UnityEngine.Object) component1.m_NameMaterial)
                  {
                    netNameData.m_MaterialIndex = index3;
                    nativeArray2[index2] = netNameData;
                    goto label_13;
                  }
                }
              }
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AggregateMeshSystem.MeshData meshData = new AggregateMeshSystem.MeshData();
              // ISSUE: reference to a compiler-generated field
              meshData.m_OriginalMaterial = component1.m_NameMaterial;
              // ISSUE: reference to a compiler-generated field
              meshData.m_Materials = new List<AggregateMeshSystem.MaterialData>(2);
              // ISSUE: reference to a compiler-generated field
              if (this.m_LabelData == null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LabelData = new List<AggregateMeshSystem.MeshData>();
              }
              // ISSUE: reference to a compiler-generated field
              netNameData.m_MaterialIndex = this.m_LabelData.Count;
              nativeArray2[index2] = netNameData;
              // ISSUE: reference to a compiler-generated field
              this.m_LabelData.Add(meshData);
            }
label_13:
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2.m_ArrowMaterial != (UnityEngine.Object) null)
            {
              NetArrowData netArrowData = nativeArray3[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_ArrowData != null)
              {
                // ISSUE: reference to a compiler-generated field
                for (int index4 = 0; index4 < this.m_ArrowData.Count; ++index4)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((UnityEngine.Object) this.m_ArrowData[index4].m_OriginalMaterial == (UnityEngine.Object) component2.m_ArrowMaterial)
                  {
                    netArrowData.m_MaterialIndex = index4;
                    nativeArray3[index2] = netArrowData;
                    goto label_24;
                  }
                }
              }
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AggregateMeshSystem.MeshData meshData = new AggregateMeshSystem.MeshData();
              // ISSUE: reference to a compiler-generated field
              meshData.m_OriginalMaterial = component2.m_ArrowMaterial;
              // ISSUE: reference to a compiler-generated field
              meshData.m_Materials = new List<AggregateMeshSystem.MaterialData>(2);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AggregateMeshSystem.MaterialData materialData1 = new AggregateMeshSystem.MaterialData();
              // ISSUE: reference to a compiler-generated field
              materialData1.m_Material = new Material(component2.m_ArrowMaterial);
              // ISSUE: reference to a compiler-generated field
              materialData1.m_Material.name = "Aggregate arrows (" + prefab.name + ")";
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              materialData1.m_Material.SetColor(this.m_FaceColor, new Color(1f, 1f, 1f, flag ? 0.25f : 1f));
              // ISSUE: reference to a compiler-generated field
              meshData.m_Materials.Add(materialData1);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AggregateMeshSystem.MaterialData materialData2 = new AggregateMeshSystem.MaterialData();
              // ISSUE: reference to a compiler-generated field
              materialData2.m_Material = new Material(component2.m_ArrowMaterial);
              // ISSUE: reference to a compiler-generated field
              materialData2.m_Material.name = "Aggregate underground arrows (" + prefab.name + ")";
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              materialData2.m_Material.SetColor(this.m_FaceColor, new Color(1f, 1f, 1f, 1f));
              // ISSUE: reference to a compiler-generated field
              materialData2.m_IsUnderground = true;
              // ISSUE: reference to a compiler-generated field
              meshData.m_Materials.Add(materialData2);
              // ISSUE: reference to a compiler-generated field
              if (this.m_ArrowData == null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ArrowData = new List<AggregateMeshSystem.MeshData>();
              }
              // ISSUE: reference to a compiler-generated field
              netArrowData.m_MaterialIndex = this.m_ArrowData.Count;
              nativeArray3[index2] = netArrowData;
              // ISSUE: reference to a compiler-generated field
              this.m_ArrowData.Add(meshData);
            }
          }
        }
      }
    }

    private void UpdateLabelVertices(bool isLoaded)
    {
      List<AggregateMeshSystem.MaterialUpdate> materialUpdateList = (List<AggregateMeshSystem.MaterialUpdate>) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = (isLoaded ? this.m_LabelQuery : this.m_UpdatedLabelQuery).ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        TextMeshPro textMesh = this.m_OverlayRenderSystem.GetTextMesh();
        textMesh.rectTransform.sizeDelta = new Vector2(250f, 100f);
        textMesh.fontSize = 200f;
        textMesh.alignment = TextAlignmentOptions.Center;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_UndergroundViewSystem.undergroundOn && this.m_UndergroundViewSystem.tunnelsOn;
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
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LabelExtents_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<LabelExtents> componentTypeHandle5 = this.__TypeHandle.__Game_Net_LabelExtents_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LabelMaterial_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        SharedComponentTypeHandle<LabelMaterial> componentTypeHandle6 = this.__TypeHandle.__Game_Net_LabelMaterial_SharedComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LabelVertex_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<LabelVertex> bufferTypeHandle = this.__TypeHandle.__Game_Net_LabelVertex_RW_BufferTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          LabelMaterial sharedComponent = archetypeChunk.GetSharedComponent<LabelMaterial>(componentTypeHandle6, this.EntityManager);
          if (isLoaded || archetypeChunk.Has<Updated>(ref componentTypeHandle1) || archetypeChunk.Has<BatchesUpdated>(ref componentTypeHandle2))
          {
            NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<Temp> nativeArray2 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle3);
            NativeArray<PrefabRef> nativeArray3 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle4);
            NativeArray<LabelExtents> nativeArray4 = archetypeChunk.GetNativeArray<LabelExtents>(ref componentTypeHandle5);
            BufferAccessor<LabelVertex> bufferAccessor = archetypeChunk.GetBufferAccessor<LabelVertex>(ref bufferTypeHandle);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              PrefabRef prefabRef = nativeArray3[index2];
              DynamicBuffer<LabelVertex> dynamicBuffer = bufferAccessor[index2];
              NetNameData componentData = this.EntityManager.GetComponentData<NetNameData>(prefabRef.m_Prefab);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              AggregateMeshSystem.MeshData meshData = this.m_LabelData[componentData.m_MaterialIndex];
              // ISSUE: reference to a compiler-generated field
              meshData.m_MeshDirty = true;
              if (componentData.m_MaterialIndex != sharedComponent.m_Index)
              {
                if (materialUpdateList == null)
                  materialUpdateList = new List<AggregateMeshSystem.MaterialUpdate>();
                // ISSUE: object of a compiler-generated type is created
                materialUpdateList.Add(new AggregateMeshSystem.MaterialUpdate()
                {
                  m_Entity = entity,
                  m_Material = componentData.m_MaterialIndex
                });
              }
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
                  dynamicBuffer.Clear();
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
              dynamicBuffer.ResizeUninitialized(length);
              int num = 0;
              for (int index4 = 0; index4 < textInfo.meshInfo.Length; ++index4)
              {
                TMP_MeshInfo tmpMeshInfo = textInfo.meshInfo[index4];
                if (tmpMeshInfo.vertexCount != 0)
                {
                  Texture mainTexture = tmpMeshInfo.material.mainTexture;
                  int2 int2 = (int2) -1;
                  // ISSUE: reference to a compiler-generated field
                  for (int index5 = 0; index5 < meshData.m_Materials.Count; ++index5)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    AggregateMeshSystem.MaterialData material = meshData.m_Materials[index5];
                    // ISSUE: reference to a compiler-generated field
                    if ((UnityEngine.Object) material.m_Material.mainTexture == (UnityEngine.Object) mainTexture)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (material.m_IsUnderground)
                        int2.y = index5;
                      else
                        int2.x = index5;
                    }
                  }
                  if (int2.x == -1)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    AggregateMeshSystem.MaterialData materialData = new AggregateMeshSystem.MaterialData();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material = new Material(meshData.m_OriginalMaterial);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material.SetColor(this.m_FaceColor, new Color(1f, 1f, 1f, flag ? 0.25f : 1f));
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayRenderSystem.CopyFontAtlasParameters(tmpMeshInfo.material, materialData.m_Material);
                    // ISSUE: reference to a compiler-generated field
                    int2.x = meshData.m_Materials.Count;
                    // ISSUE: reference to a compiler-generated field
                    meshData.m_Materials.Add(materialData);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material.name = string.Format("Aggregate names {0} ({1})", (object) int2.x, (object) meshData.m_OriginalMaterial.name);
                  }
                  if (int2.y == -1)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    AggregateMeshSystem.MaterialData materialData = new AggregateMeshSystem.MaterialData();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material = new Material(meshData.m_OriginalMaterial);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material.SetColor(this.m_FaceColor, new Color(1f, 1f, 1f, 1f));
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayRenderSystem.CopyFontAtlasParameters(tmpMeshInfo.material, materialData.m_Material);
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_IsUnderground = true;
                    // ISSUE: reference to a compiler-generated field
                    int2.y = meshData.m_Materials.Count;
                    // ISSUE: reference to a compiler-generated field
                    meshData.m_Materials.Add(materialData);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    materialData.m_Material.name = string.Format("Aggregate underground names {0} ({1})", (object) int2.y, (object) meshData.m_OriginalMaterial.name);
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
                    labelVertex.m_Material = int2;
                    dynamicBuffer[num + index6] = labelVertex;
                  }
                  num += tmpMeshInfo.vertexCount;
                }
              }
              LabelExtents labelExtents = new LabelExtents();
              for (int index7 = 0; index7 < textInfo.lineCount; ++index7)
              {
                Extents lineExtents = textInfo.lineInfo[index7].lineExtents;
                labelExtents.m_Bounds |= new Bounds2((float2) lineExtents.min, (float2) lineExtents.max);
              }
              nativeArray4[index2] = labelExtents;
            }
          }
          else
          {
            NativeArray<Entity> nativeArray5 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<PrefabRef> nativeArray6 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle4);
            for (int index8 = 0; index8 < nativeArray5.Length; ++index8)
            {
              Entity key = nativeArray5[index8];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_LabelData[this.EntityManager.GetComponentData<NetNameData>(nativeArray6[index8].m_Prefab).m_MaterialIndex].m_MeshDirty = true;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CachedLabels != null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CachedLabels.Remove(key);
              }
            }
          }
        }
      }
      if (materialUpdateList == null)
        return;
      for (int index = 0; index < materialUpdateList.Count; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        AggregateMeshSystem.MaterialUpdate materialUpdate = materialUpdateList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.SetSharedComponent<LabelMaterial>(materialUpdate.m_Entity, new LabelMaterial()
        {
          m_Index = materialUpdate.m_Material
        });
      }
    }

    private void UpdateArrowMaterials(bool isLoaded)
    {
      List<AggregateMeshSystem.MaterialUpdate> materialUpdateList = (List<AggregateMeshSystem.MaterialUpdate>) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = (isLoaded ? this.m_ArrowQuery : this.m_UpdatedArrowQuery).ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
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
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ArrowMaterial_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        SharedComponentTypeHandle<ArrowMaterial> componentTypeHandle3 = this.__TypeHandle.__Game_Net_ArrowMaterial_SharedComponentTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          ArrowMaterial sharedComponent = archetypeChunk.GetSharedComponent<ArrowMaterial>(componentTypeHandle3, this.EntityManager);
          if (isLoaded || archetypeChunk.Has<Updated>(ref componentTypeHandle1))
          {
            NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              NetArrowData componentData = this.EntityManager.GetComponentData<NetArrowData>(nativeArray2[index2].m_Prefab);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ArrowData[componentData.m_MaterialIndex].m_MeshDirty = true;
              if (componentData.m_MaterialIndex != sharedComponent.m_Index)
              {
                if (materialUpdateList == null)
                  materialUpdateList = new List<AggregateMeshSystem.MaterialUpdate>();
                // ISSUE: object of a compiler-generated type is created
                materialUpdateList.Add(new AggregateMeshSystem.MaterialUpdate()
                {
                  m_Entity = entity,
                  m_Material = componentData.m_MaterialIndex
                });
              }
            }
          }
          else
          {
            NativeArray<Entity> nativeArray3 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<PrefabRef> nativeArray4 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
            for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
            {
              Entity entity = nativeArray3[index3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ArrowData[this.EntityManager.GetComponentData<NetArrowData>(nativeArray4[index3].m_Prefab).m_MaterialIndex].m_MeshDirty = true;
            }
          }
        }
      }
      if (materialUpdateList == null)
        return;
      for (int index = 0; index < materialUpdateList.Count; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        AggregateMeshSystem.MaterialUpdate materialUpdate = materialUpdateList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.SetSharedComponent<ArrowMaterial>(materialUpdate.m_Entity, new ArrowMaterial()
        {
          m_Index = materialUpdate.m_Material
        });
      }
    }

    private JobHandle UpdateLabelPositions(JobHandle inputDeps, bool isLoaded)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = isLoaded ? this.m_LabelQuery : this.m_UpdatedLabelQuery;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LabelPosition_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LabelExtents_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      return new AggregateMeshSystem.UpdateLabelPositionsJob()
      {
        m_LabelExtentsType = this.__TypeHandle.__Game_Net_LabelExtents_RO_ComponentTypeHandle,
        m_AggregateElementType = this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferTypeHandle,
        m_LabelPositionType = this.__TypeHandle.__Game_Net_LabelPosition_RW_BufferTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_NetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup
      }.ScheduleParallel<AggregateMeshSystem.UpdateLabelPositionsJob>(query, inputDeps);
    }

    private JobHandle UpdateArrowPositions(JobHandle inputDeps, bool isLoaded)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = isLoaded ? this.m_ArrowQuery : this.m_UpdatedArrowQuery;
      NativeParallelMultiHashMap<Entity, AggregateMeshSystem.TempValue> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, AggregateMeshSystem.TempValue>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AggregateMeshSystem.FillTempMapJob jobData1 = new AggregateMeshSystem.FillTempMapJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AggregatedType = this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TempMap = parallelMultiHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionCarriageway_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ArrowPosition_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AggregateMeshSystem.UpdateArrowPositionsJob jobData2 = new AggregateMeshSystem.UpdateArrowPositionsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AggregateElementType = this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferTypeHandle,
        m_ArrowPositionType = this.__TypeHandle.__Game_Net_ArrowPosition_RW_BufferTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_NetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_NetCompositionCarriageways = this.__TypeHandle.__Game_Prefabs_NetCompositionCarriageway_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_TempMap = parallelMultiHashMap
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.Schedule<AggregateMeshSystem.FillTempMapJob>(this.m_TempAggregatedQuery, inputDeps);
      EntityQuery query = entityQuery;
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps1 = jobData2.ScheduleParallel<AggregateMeshSystem.UpdateArrowPositionsJob>(query, dependsOn);
      parallelMultiHashMap.Dispose(inputDeps1);
      return inputDeps1;
    }

    private JobHandle FillNameMeshData(JobHandle inputDeps)
    {
      JobHandle job0 = inputDeps;
      // ISSUE: reference to a compiler-generated field
      if (this.m_LabelData != null)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_LabelData.Count; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AggregateMeshSystem.MeshData meshData = this.m_LabelData[index1];
          // ISSUE: reference to a compiler-generated field
          if (meshData.m_MeshDirty)
          {
            // ISSUE: reference to a compiler-generated field
            meshData.m_MeshDirty = false;
            // ISSUE: reference to a compiler-generated field
            this.m_LabelQuery.ResetFilter();
            // ISSUE: reference to a compiler-generated field
            this.m_LabelQuery.SetSharedComponentFilter<LabelMaterial>(new LabelMaterial()
            {
              m_Index = index1
            });
            // ISSUE: reference to a compiler-generated field
            if (this.m_LabelQuery.IsEmptyIgnoreFilter)
            {
              // ISSUE: reference to a compiler-generated field
              if (meshData.m_Materials != null)
              {
                // ISSUE: reference to a compiler-generated field
                for (int index2 = 0; index2 < meshData.m_Materials.Count; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  AggregateMeshSystem.MaterialData material = meshData.m_Materials[index2] with
                  {
                    m_HasMesh = false
                  };
                  // ISSUE: reference to a compiler-generated field
                  meshData.m_Materials[index2] = material;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if ((UnityEngine.Object) meshData.m_Mesh != (UnityEngine.Object) null)
              {
                // ISSUE: reference to a compiler-generated field
                UnityEngine.Object.Destroy((UnityEngine.Object) meshData.m_Mesh);
                // ISSUE: reference to a compiler-generated field
                meshData.m_Mesh = (Mesh) null;
              }
              // ISSUE: reference to a compiler-generated field
              if (meshData.m_HasMeshData)
              {
                // ISSUE: reference to a compiler-generated field
                meshData.m_HasMeshData = false;
                // ISSUE: reference to a compiler-generated field
                meshData.m_MeshData.Dispose();
              }
              // ISSUE: reference to a compiler-generated field
              meshData.m_HasMesh = false;
            }
            else
            {
              JobHandle outJobHandle;
              // ISSUE: reference to a compiler-generated field
              NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_LabelQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
              // ISSUE: reference to a compiler-generated field
              if (!meshData.m_HasMeshData)
              {
                // ISSUE: reference to a compiler-generated field
                meshData.m_HasMeshData = true;
                // ISSUE: reference to a compiler-generated field
                meshData.m_MeshData = Mesh.AllocateWritableMeshData(1);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Prefabs_NetNameData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Net_LabelVertex_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Net_LabelPosition_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
              this.__TypeHandle.__Game_Net_LabelExtents_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
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
              JobHandle jobHandle = new AggregateMeshSystem.FillNameDataJob()
              {
                m_LabelExtentsType = this.__TypeHandle.__Game_Net_LabelExtents_RO_ComponentTypeHandle,
                m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
                m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
                m_HiddenType = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle,
                m_LabelPositionType = this.__TypeHandle.__Game_Net_LabelPosition_RO_BufferTypeHandle,
                m_LabelVertexType = this.__TypeHandle.__Game_Net_LabelVertex_RO_BufferTypeHandle,
                m_NetNameData = this.__TypeHandle.__Game_Prefabs_NetNameData_RO_ComponentLookup,
                m_Chunks = archetypeChunkListAsync,
                m_SubMeshCount = meshData.m_Materials.Count,
                m_NameMeshData = meshData.m_MeshData
              }.Schedule<AggregateMeshSystem.FillNameDataJob>(JobHandle.CombineDependencies(outJobHandle, inputDeps));
              archetypeChunkListAsync.Dispose(jobHandle);
              // ISSUE: reference to a compiler-generated field
              meshData.m_DataDependencies = jobHandle;
              job0 = JobHandle.CombineDependencies(job0, jobHandle);
            }
          }
        }
      }
      return job0;
    }

    private JobHandle FillArrowMeshData(JobHandle inputDeps)
    {
      JobHandle job0 = inputDeps;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ArrowData != null)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_ArrowData.Count; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AggregateMeshSystem.MeshData meshData = this.m_ArrowData[index1];
          // ISSUE: reference to a compiler-generated field
          if (meshData.m_MeshDirty)
          {
            // ISSUE: reference to a compiler-generated field
            meshData.m_MeshDirty = false;
            // ISSUE: reference to a compiler-generated field
            this.m_ArrowQuery.ResetFilter();
            // ISSUE: reference to a compiler-generated field
            this.m_ArrowQuery.SetSharedComponentFilter<ArrowMaterial>(new ArrowMaterial()
            {
              m_Index = index1
            });
            // ISSUE: reference to a compiler-generated field
            if (this.m_ArrowQuery.IsEmptyIgnoreFilter)
            {
              // ISSUE: reference to a compiler-generated field
              if (meshData.m_Materials != null)
              {
                // ISSUE: reference to a compiler-generated field
                for (int index2 = 0; index2 < meshData.m_Materials.Count; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  AggregateMeshSystem.MaterialData material = meshData.m_Materials[index2] with
                  {
                    m_HasMesh = false
                  };
                  // ISSUE: reference to a compiler-generated field
                  meshData.m_Materials[index2] = material;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if ((UnityEngine.Object) meshData.m_Mesh != (UnityEngine.Object) null)
              {
                // ISSUE: reference to a compiler-generated field
                UnityEngine.Object.Destroy((UnityEngine.Object) meshData.m_Mesh);
                // ISSUE: reference to a compiler-generated field
                meshData.m_Mesh = (Mesh) null;
              }
              // ISSUE: reference to a compiler-generated field
              if (meshData.m_HasMeshData)
              {
                // ISSUE: reference to a compiler-generated field
                meshData.m_HasMeshData = false;
                // ISSUE: reference to a compiler-generated field
                meshData.m_MeshData.Dispose();
              }
              // ISSUE: reference to a compiler-generated field
              meshData.m_HasMesh = false;
            }
            else
            {
              JobHandle outJobHandle;
              // ISSUE: reference to a compiler-generated field
              NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_ArrowQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
              // ISSUE: reference to a compiler-generated field
              if (!meshData.m_HasMeshData)
              {
                // ISSUE: reference to a compiler-generated field
                meshData.m_HasMeshData = true;
                // ISSUE: reference to a compiler-generated field
                meshData.m_MeshData = Mesh.AllocateWritableMeshData(1);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Prefabs_NetArrowData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Net_ArrowPosition_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
              JobHandle jobHandle = new AggregateMeshSystem.FillArrowDataJob()
              {
                m_HiddenType = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle,
                m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
                m_ArrowPositionType = this.__TypeHandle.__Game_Net_ArrowPosition_RO_BufferTypeHandle,
                m_NetArrowData = this.__TypeHandle.__Game_Prefabs_NetArrowData_RO_ComponentLookup,
                m_Chunks = archetypeChunkListAsync,
                m_ArrowMeshData = meshData.m_MeshData
              }.Schedule<AggregateMeshSystem.FillArrowDataJob>(JobHandle.CombineDependencies(outJobHandle, inputDeps));
              archetypeChunkListAsync.Dispose(jobHandle);
              // ISSUE: reference to a compiler-generated field
              meshData.m_DataDependencies = jobHandle;
              job0 = JobHandle.CombineDependencies(job0, jobHandle);
            }
          }
        }
      }
      return job0;
    }

    public int GetNameMaterialCount() => this.m_LabelData != null ? this.m_LabelData.Count : 0;

    public int GetArrowMaterialCount() => this.m_ArrowData != null ? this.m_ArrowData.Count : 0;

    public bool GetNameMesh(int index, out Mesh mesh, out int subMeshCount)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.GetMeshData(this.m_LabelData, index, out mesh, out subMeshCount);
    }

    public bool GetNameMaterial(int index, int subMeshIndex, out Material material)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.GetMaterialData(this.m_LabelData, index, subMeshIndex, out material);
    }

    public bool GetArrowMesh(int index, out Mesh mesh, out int subMeshCount)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.GetMeshData(this.m_ArrowData, index, out mesh, out subMeshCount);
    }

    public bool GetArrowMaterial(int index, int subMeshIndex, out Material material)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.GetMaterialData(this.m_ArrowData, index, subMeshIndex, out material);
    }

    private bool GetMeshData(
      List<AggregateMeshSystem.MeshData> meshData,
      int index,
      out Mesh mesh,
      out int subMeshCount)
    {
      // ISSUE: variable of a compiler-generated type
      AggregateMeshSystem.MeshData meshData1 = meshData[index];
      // ISSUE: reference to a compiler-generated field
      subMeshCount = meshData1.m_Materials.Count;
      // ISSUE: reference to a compiler-generated field
      if (meshData1.m_HasMeshData)
      {
        // ISSUE: reference to a compiler-generated field
        meshData1.m_HasMeshData = false;
        // ISSUE: reference to a compiler-generated field
        meshData1.m_DataDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        meshData1.m_DataDependencies = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) meshData1.m_Mesh == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          meshData1.m_Mesh = new Mesh();
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) meshData1.m_OriginalMaterial != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            meshData1.m_Mesh.name = string.Format("Aggregates ({0})", (object) meshData1.m_OriginalMaterial);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Mesh.ApplyAndDisposeWritableMeshData(meshData1.m_MeshData, meshData1.m_Mesh, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
        Bounds bounds = new Bounds();
        // ISSUE: reference to a compiler-generated field
        meshData1.m_HasMesh = false;
        for (int index1 = 0; index1 < subMeshCount; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AggregateMeshSystem.MaterialData material = meshData1.m_Materials[index1];
          // ISSUE: reference to a compiler-generated field
          SubMeshDescriptor subMesh = meshData1.m_Mesh.GetSubMesh(index1);
          // ISSUE: reference to a compiler-generated field
          material.m_HasMesh = subMesh.vertexCount > 0;
          // ISSUE: reference to a compiler-generated field
          if (material.m_HasMesh)
          {
            // ISSUE: reference to a compiler-generated field
            if (meshData1.m_HasMesh)
            {
              bounds.Encapsulate(subMesh.bounds);
            }
            else
            {
              bounds = subMesh.bounds;
              // ISSUE: reference to a compiler-generated field
              meshData1.m_HasMesh = true;
            }
          }
          // ISSUE: reference to a compiler-generated field
          meshData1.m_Materials[index1] = material;
        }
        // ISSUE: reference to a compiler-generated field
        meshData1.m_Mesh.bounds = bounds;
      }
      // ISSUE: reference to a compiler-generated field
      mesh = meshData1.m_Mesh;
      // ISSUE: reference to a compiler-generated field
      return meshData1.m_HasMesh;
    }

    private bool GetMaterialData(
      List<AggregateMeshSystem.MeshData> meshData,
      int index,
      int subMeshIndex,
      out Material material)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AggregateMeshSystem.MaterialData material1 = meshData[index].m_Materials[subMeshIndex];
      // ISSUE: reference to a compiler-generated field
      material = material1.m_Material;
      // ISSUE: reference to a compiler-generated field
      return material1.m_HasMesh;
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
    public AggregateMeshSystem()
    {
    }

    private struct MaterialData
    {
      public Material m_Material;
      public bool m_IsUnderground;
      public bool m_HasMesh;
    }

    private class MeshData
    {
      public JobHandle m_DataDependencies;
      public Material m_OriginalMaterial;
      public List<AggregateMeshSystem.MaterialData> m_Materials;
      public Mesh m_Mesh;
      public Mesh.MeshDataArray m_MeshData;
      public bool m_MeshDirty;
      public bool m_HasMeshData;
      public bool m_HasMesh;
    }

    private struct MaterialUpdate
    {
      public Entity m_Entity;
      public int m_Material;
    }

    [BurstCompile]
    private struct UpdateLabelPositionsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<LabelExtents> m_LabelExtentsType;
      [ReadOnly]
      public BufferTypeHandle<AggregateElement> m_AggregateElementType;
      public BufferTypeHandle<LabelPosition> m_LabelPositionType;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<LabelExtents> nativeArray = chunk.GetNativeArray<LabelExtents>(ref this.m_LabelExtentsType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<AggregateElement> bufferAccessor1 = chunk.GetBufferAccessor<AggregateElement>(ref this.m_AggregateElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LabelPosition> bufferAccessor2 = chunk.GetBufferAccessor<LabelPosition>(ref this.m_LabelPositionType);
        NativeList<float> redundancyBuffer = new NativeList<float>();
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          LabelExtents labelExtents = nativeArray[index1];
          DynamicBuffer<AggregateElement> aggregateElements = bufferAccessor1[index1];
          DynamicBuffer<LabelPosition> labelPositions = bufferAccessor2[index1];
          labelPositions.Clear();
          if (aggregateElements.Length > 0)
          {
            if (!redundancyBuffer.IsCreated)
              redundancyBuffer = new NativeList<float>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            int startIndex = 0;
            int num1 = 1;
            // ISSUE: reference to a compiler-generated field
            Game.Net.Edge edge1 = this.m_EdgeData[aggregateElements[0].m_Edge];
            for (; num1 < aggregateElements.Length; ++num1)
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge2 = this.m_EdgeData[aggregateElements[num1].m_Edge];
              if (edge2.m_Start == edge1.m_End || edge2.m_Start == edge1.m_Start)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectedEdges[edge2.m_Start].Length > 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddLabels(startIndex, num1, labelExtents, redundancyBuffer, aggregateElements, labelPositions);
                  startIndex = num1;
                }
              }
              else if (edge2.m_End == edge1.m_End || edge2.m_End == edge1.m_Start)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectedEdges[edge2.m_End].Length > 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddLabels(startIndex, num1, labelExtents, redundancyBuffer, aggregateElements, labelPositions);
                  startIndex = num1;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.AddLabels(startIndex, num1, labelExtents, redundancyBuffer, aggregateElements, labelPositions);
                startIndex = num1;
              }
              edge1 = edge2;
            }
            // ISSUE: reference to a compiler-generated method
            this.AddLabels(startIndex, num1, labelExtents, redundancyBuffer, aggregateElements, labelPositions);
            float x = 0.0f;
            for (int index2 = 0; index2 < redundancyBuffer.Length; ++index2)
              x += redundancyBuffer[index2];
            float num2 = (float) (((double) math.max(1f, math.round(x)) - (double) x) * 0.5);
            float num3 = 0.0f;
            for (int index3 = 0; index3 < redundancyBuffer.Length; ++index3)
            {
              float num4 = redundancyBuffer[index3];
              num2 += num4;
              if ((double) num2 < 0.5)
              {
                LabelPosition labelPosition = labelPositions[index3];
                num3 += num4;
                if ((double) num3 < 0.25)
                {
                  labelPosition.m_MaxScale *= 0.25f;
                }
                else
                {
                  labelPosition.m_MaxScale *= 0.5f;
                  if ((double) num4 < 0.25)
                    num3 -= 0.5f;
                  else
                    num3 = 0.0f;
                }
                labelPositions[index3] = labelPosition;
              }
              else
              {
                --num2;
                num3 = 0.0f;
              }
            }
            redundancyBuffer.Clear();
          }
        }
        if (!redundancyBuffer.IsCreated)
          return;
        redundancyBuffer.Dispose();
      }

      private void AddLabels(
        int startIndex,
        int endIndex,
        LabelExtents labelExtents,
        NativeList<float> redundancyBuffer,
        DynamicBuffer<AggregateElement> aggregateElements,
        DynamicBuffer<LabelPosition> labelPositions)
      {
        float num1 = 0.0f;
        for (int index = startIndex; index < endIndex; ++index)
        {
          Entity edge = aggregateElements[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[edge];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num2 = math.sqrt(math.max(1f, this.m_NetCompositionData[this.m_CompositionData[edge].m_Edge].m_Width));
          num1 += curve.m_Length / num2;
        }
        float num3 = 100f;
        int num4 = math.clamp(Mathf.RoundToInt(num1 / num3), 1, endIndex - startIndex);
        float num5 = num1 / (float) num4;
        float num6 = num5 / 100f;
        float x = 0.0f;
        float num7 = 0.0f;
        int index1 = -1;
        for (int index2 = startIndex; index2 < endIndex; ++index2)
        {
          Entity edge1 = aggregateElements[index2].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Curve curve1 = this.m_CurveData[edge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num8 = math.sqrt(math.max(1f, this.m_NetCompositionData[this.m_CompositionData[edge1].m_Edge].m_Width));
          float y = x + curve1.m_Length / num8;
          if (index1 != -1 && (double) y - (double) num5 > (double) num5 - (double) x)
          {
            Entity edge2 = aggregateElements[index1].m_Edge;
            // ISSUE: reference to a compiler-generated field
            Curve curve2 = this.m_CurveData[edge2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData = this.m_NetCompositionData[this.m_CompositionData[edge2].m_Edge];
            LabelPosition elem;
            elem.m_Curve = curve2.m_Bezier;
            elem.m_ElementIndex = index2;
            elem.m_HalfLength = curve2.m_Length * 0.5f;
            elem.m_MaxScale = netCompositionData.m_Width * 0.5f / math.max(1f, math.max(-labelExtents.m_Bounds.min.y, labelExtents.m_Bounds.max.y));
            elem.m_IsUnderground = (netCompositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) > (CompositionFlags.General) 0;
            labelPositions.Add(elem);
            redundancyBuffer.Add(in num6);
            x -= num5;
            y -= num5;
            num7 = 0.0f;
            index1 = -1;
          }
          float num9 = math.lerp(x, y, 0.5f);
          float num10 = curve1.m_Length * num8 / math.max(1f, num5 + math.abs(num9 - num5 * 0.5f));
          x = y;
          if ((double) num10 > (double) num7)
          {
            num7 = num10;
            index1 = index2;
          }
        }
        if (index1 == -1)
          return;
        Entity edge3 = aggregateElements[index1].m_Edge;
        // ISSUE: reference to a compiler-generated field
        Curve curve3 = this.m_CurveData[edge3];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_NetCompositionData[this.m_CompositionData[edge3].m_Edge];
        LabelPosition elem1;
        elem1.m_Curve = curve3.m_Bezier;
        elem1.m_ElementIndex = index1;
        elem1.m_HalfLength = curve3.m_Length * 0.5f;
        elem1.m_MaxScale = netCompositionData1.m_Width * 0.5f / math.max(1f, math.max(-labelExtents.m_Bounds.min.y, labelExtents.m_Bounds.max.y));
        elem1.m_IsUnderground = (netCompositionData1.m_Flags.m_General & CompositionFlags.General.Tunnel) > (CompositionFlags.General) 0;
        labelPositions.Add(elem1);
        redundancyBuffer.Add(in num6);
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
    private struct FillTempMapJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Aggregated> m_AggregatedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      public NativeParallelMultiHashMap<Entity, AggregateMeshSystem.TempValue> m_TempMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Aggregated> nativeArray2 = chunk.GetNativeArray<Aggregated>(ref this.m_AggregatedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Aggregated aggregated = nativeArray2[index];
          Temp temp = nativeArray3[index];
          if (aggregated.m_Aggregate != Entity.Null && temp.m_Original != Entity.Null && (temp.m_Flags & (TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) != (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_TempMap.Add(aggregated.m_Aggregate, new AggregateMeshSystem.TempValue()
            {
              m_TempEntity = nativeArray1[index],
              m_OriginalEntity = temp.m_Original
            });
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

    private struct TempValue
    {
      public Entity m_TempEntity;
      public Entity m_OriginalEntity;
    }

    [BurstCompile]
    private struct UpdateArrowPositionsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<AggregateElement> m_AggregateElementType;
      public BufferTypeHandle<ArrowPosition> m_ArrowPositionType;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<NetCompositionCarriageway> m_NetCompositionCarriageways;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeParallelMultiHashMap<Entity, AggregateMeshSystem.TempValue> m_TempMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<AggregateElement> bufferAccessor1 = chunk.GetBufferAccessor<AggregateElement>(ref this.m_AggregateElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ArrowPosition> bufferAccessor2 = chunk.GetBufferAccessor<ArrowPosition>(ref this.m_ArrowPositionType);
        NativeParallelHashMap<Entity, Entity> edgeMap = new NativeParallelHashMap<Entity, Entity>();
        NativeList<AggregateElement> nativeList = new NativeList<AggregateElement>();
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          DynamicBuffer<AggregateElement> dynamicBuffer = bufferAccessor1[index1];
          DynamicBuffer<ArrowPosition> arrowPositions = bufferAccessor2[index1];
          arrowPositions.Clear();
          if (dynamicBuffer.Length > 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateEdgeMap(nativeArray[index1], ref edgeMap);
            // ISSUE: reference to a compiler-generated method
            this.ProcessElements(dynamicBuffer.AsNativeArray(), arrowPositions, edgeMap);
            if (edgeMap.IsCreated && !edgeMap.IsEmpty)
            {
              if (nativeList.IsCreated)
                nativeList.Clear();
              else
                nativeList = new NativeList<AggregateElement>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Entity edge;
                if (edgeMap.TryGetValue(dynamicBuffer[index2].m_Edge, out edge))
                  nativeList.Add(new AggregateElement(edge));
                else if (!nativeList.IsEmpty)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ProcessElements(nativeList.AsArray(), arrowPositions, new NativeParallelHashMap<Entity, Entity>());
                  nativeList.Clear();
                }
              }
              if (!nativeList.IsEmpty)
              {
                // ISSUE: reference to a compiler-generated method
                this.ProcessElements(nativeList.AsArray(), arrowPositions, new NativeParallelHashMap<Entity, Entity>());
              }
            }
          }
        }
        if (edgeMap.IsCreated)
          edgeMap.Dispose();
        if (!nativeList.IsCreated)
          return;
        nativeList.Dispose();
      }

      private void UpdateEdgeMap(
        Entity aggregate,
        ref NativeParallelHashMap<Entity, Entity> edgeMap)
      {
        // ISSUE: variable of a compiler-generated type
        AggregateMeshSystem.TempValue tempValue;
        NativeParallelMultiHashMapIterator<Entity> it;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempMap.TryGetFirstValue(aggregate, out tempValue, out it))
        {
          if (edgeMap.IsCreated)
            edgeMap.Clear();
          else
            edgeMap = new NativeParallelHashMap<Entity, Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          do
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            edgeMap.TryAdd(tempValue.m_OriginalEntity, tempValue.m_TempEntity);
          }
          while (this.m_TempMap.TryGetNextValue(out tempValue, ref it));
        }
        else
        {
          if (!edgeMap.IsCreated)
            return;
          edgeMap.Clear();
        }
      }

      private void ProcessElements(
        NativeArray<AggregateElement> aggregateElements,
        DynamicBuffer<ArrowPosition> arrowPositions,
        NativeParallelHashMap<Entity, Entity> edgeMap)
      {
        int startIndex = 0;
        int num = 1;
        // ISSUE: reference to a compiler-generated field
        Game.Net.Edge edge1 = this.m_EdgeData[aggregateElements[0].m_Edge];
        for (; num < aggregateElements.Length; ++num)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge2 = this.m_EdgeData[aggregateElements[num].m_Edge];
          if (edge2.m_Start == edge1.m_End || edge2.m_Start == edge1.m_Start)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.CompositionChange(this.m_ConnectedEdges[edge2.m_Start], edge2.m_Start == edge1.m_Start))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddArrows(startIndex, num, aggregateElements, arrowPositions, edgeMap);
              startIndex = num;
            }
          }
          else if (edge2.m_End == edge1.m_End || edge2.m_End == edge1.m_Start)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.CompositionChange(this.m_ConnectedEdges[edge2.m_End], edge2.m_End == edge1.m_End))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddArrows(startIndex, num, aggregateElements, arrowPositions, edgeMap);
              startIndex = num;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddArrows(startIndex, num, aggregateElements, arrowPositions, edgeMap);
            startIndex = num;
          }
          edge1 = edge2;
        }
        // ISSUE: reference to a compiler-generated method
        this.AddArrows(startIndex, num, aggregateElements, arrowPositions, edgeMap);
      }

      private bool CompositionChange(DynamicBuffer<ConnectedEdge> connectedEdges, bool invert)
      {
        if (connectedEdges.Length != 2)
          return true;
        Entity edge1 = connectedEdges[0].m_Edge;
        Entity edge2 = connectedEdges[1].m_Edge;
        // ISSUE: reference to a compiler-generated field
        Composition composition1 = this.m_CompositionData[edge1];
        // ISSUE: reference to a compiler-generated field
        Composition composition2 = this.m_CompositionData[edge2];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionCarriageway> compositionCarriageway1 = this.m_NetCompositionCarriageways[composition1.m_Edge];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionCarriageway> compositionCarriageway2 = this.m_NetCompositionCarriageways[composition2.m_Edge];
        if (compositionCarriageway1.Length != compositionCarriageway2.Length)
          return true;
        for (int index = 0; index < compositionCarriageway1.Length; ++index)
        {
          NetCompositionCarriageway compositionCarriageway3 = compositionCarriageway1[index];
          NetCompositionCarriageway compositionCarriageway4;
          if (invert)
          {
            compositionCarriageway4 = compositionCarriageway2[compositionCarriageway2.Length - index - 1];
            if ((compositionCarriageway4.m_Flags & LaneFlags.Twoway) == (LaneFlags) 0)
              compositionCarriageway4.m_Flags ^= LaneFlags.Invert;
          }
          else
            compositionCarriageway4 = compositionCarriageway2[index];
          if (((compositionCarriageway3.m_Flags ^ compositionCarriageway4.m_Flags) & (LaneFlags.Invert | LaneFlags.Road | LaneFlags.Track | LaneFlags.Twoway)) != (LaneFlags) 0)
            return true;
        }
        return false;
      }

      private void AddArrows(
        int startIndex,
        int endIndex,
        NativeArray<AggregateElement> aggregateElements,
        DynamicBuffer<ArrowPosition> arrowPositions,
        NativeParallelHashMap<Entity, Entity> edgeMap)
      {
        float num1 = 0.0f;
        for (int index1 = startIndex; index1 < endIndex; ++index1)
        {
          Entity edge = aggregateElements[index1].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[edge];
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionData[edge];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_NetCompositionData[composition.m_Edge];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<NetCompositionCarriageway> compositionCarriageway1 = this.m_NetCompositionCarriageways[composition.m_Edge];
          float x = 0.0f;
          int y = 0;
          // ISSUE: reference to a compiler-generated field
          if ((netCompositionData.m_State & CompositionState.Marker) == (CompositionState) 0 || this.m_EditorMode)
          {
            for (int index2 = 0; index2 < compositionCarriageway1.Length; ++index2)
            {
              NetCompositionCarriageway compositionCarriageway2 = compositionCarriageway1[index2];
              if ((compositionCarriageway2.m_Flags & LaneFlags.Twoway) == (LaneFlags) 0)
              {
                x = math.min(x, compositionCarriageway2.m_Width + 4f);
                ++y;
              }
            }
          }
          float num2 = math.sqrt(math.max(1f, math.max(x, netCompositionData.m_Width / (float) math.max(1, y))));
          num1 += curve.m_Length / num2;
        }
        float num3 = 25f;
        int num4 = math.max(Mathf.RoundToInt(num1 / num3), 1);
        float num5 = math.max(1f, num1 / (float) num4);
        float num6 = 20f;
        float num7 = num6 * 0.5f;
        float num8 = num5 * -0.5f;
        for (int index3 = startIndex; index3 < endIndex; ++index3)
        {
          Entity edge = aggregateElements[index3].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[edge];
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionData[edge];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_NetCompositionData[composition.m_Edge];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<NetCompositionCarriageway> compositionCarriageway3 = this.m_NetCompositionCarriageways[composition.m_Edge];
          float x = 0.0f;
          int y = 0;
          // ISSUE: reference to a compiler-generated field
          if ((netCompositionData.m_State & CompositionState.Marker) == (CompositionState) 0 || this.m_EditorMode)
          {
            for (int index4 = 0; index4 < compositionCarriageway3.Length; ++index4)
            {
              NetCompositionCarriageway compositionCarriageway4 = compositionCarriageway3[index4];
              if ((compositionCarriageway4.m_Flags & LaneFlags.Twoway) == (LaneFlags) 0)
              {
                x = math.min(x, compositionCarriageway4.m_Width + 4f);
                ++y;
              }
            }
          }
          float num9 = math.sqrt(math.max(1f, math.max(x, netCompositionData.m_Width / (float) math.max(1, y))));
          float num10 = num8 + curve.m_Length / num9;
          // ISSUE: reference to a compiler-generated method
          bool c = this.IsInverted(edge, index3, aggregateElements);
          float a1 = math.min(0.25f, num6 / math.max(1f, curve.m_Length));
          float num11 = 1f - a1;
          if (index3 > startIndex)
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve1 = this.m_CurveData[aggregateElements[index3 - 1].m_Edge];
            // ISSUE: reference to a compiler-generated method
            a1 = math.select(a1, 0.0f, this.IsContinuous(curve1, curve));
          }
          if (index3 < endIndex - 1)
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve2 = this.m_CurveData[aggregateElements[index3 + 1].m_Edge];
            // ISSUE: reference to a compiler-generated method
            num11 = math.select(num11, 1f, this.IsContinuous(curve, curve2));
          }
          bool flag = true;
          if (edgeMap.IsCreated && edgeMap.ContainsKey(edge))
            flag = false;
          for (; (double) num10 >= 0.0; num10 -= num5)
          {
            if (flag)
            {
              float a2 = math.clamp(-num8 / math.max(1f, num10 - num8), a1, num11);
              float t = math.select(a2, 1f - a2, c);
              float3 float3_1 = MathUtils.Position(curve.m_Bezier, t);
              float3 a3 = math.normalizesafe(MathUtils.Tangent(curve.m_Bezier, t));
              float3 float3_2 = math.normalizesafe(new float3(a3.z, 0.0f, -a3.x));
              // ISSUE: reference to a compiler-generated field
              if ((netCompositionData.m_State & CompositionState.Marker) == (CompositionState) 0 || this.m_EditorMode)
              {
                for (int index5 = 0; index5 < compositionCarriageway3.Length; ++index5)
                {
                  NetCompositionCarriageway compositionCarriageway5 = compositionCarriageway3[index5];
                  if ((compositionCarriageway5.m_Flags & LaneFlags.Twoway) == (LaneFlags) 0)
                  {
                    float num12 = math.max(compositionCarriageway5.m_Width + 4f, netCompositionData.m_Width / (float) math.max(1, y));
                    ArrowPosition elem;
                    elem.m_Direction = math.select(a3, -a3, (compositionCarriageway5.m_Flags & LaneFlags.Invert) != 0);
                    elem.m_Position = float3_1 + float3_2 * compositionCarriageway5.m_Position.x + elem.m_Direction;
                    elem.m_Position.y += compositionCarriageway5.m_Position.y;
                    elem.m_MaxScale = num12 * 0.5f / num7;
                    elem.m_IsTrack = (compositionCarriageway5.m_Flags & LaneFlags.Road) == (LaneFlags) 0;
                    elem.m_IsUnderground = (netCompositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) > (CompositionFlags.General) 0;
                    arrowPositions.Add(elem);
                  }
                }
              }
            }
            num8 -= num5;
          }
          num8 = num10;
        }
      }

      private bool IsContinuous(Curve curve1, Curve curve2)
      {
        float4 float4;
        float4.x = math.abs(math.distancesq(curve1.m_Bezier.a, curve2.m_Bezier.a));
        float4.y = math.abs(math.distancesq(curve1.m_Bezier.a, curve2.m_Bezier.d));
        float4.z = math.abs(math.distancesq(curve1.m_Bezier.d, curve2.m_Bezier.a));
        float4.w = math.abs(math.distancesq(curve1.m_Bezier.d, curve2.m_Bezier.d));
        if (math.all(float4 > 1f))
          return false;
        float3 x1 = !math.any(float4.xy < float4.zw & float4.xy < float4.wz) ? MathUtils.EndTangent(curve1.m_Bezier) : -MathUtils.StartTangent(curve1.m_Bezier);
        float3 x2 = !math.any(float4.xz < float4.yw & float4.xz < float4.wy) ? MathUtils.EndTangent(curve2.m_Bezier) : -MathUtils.StartTangent(curve2.m_Bezier);
        return (double) math.dot(math.normalizesafe(x1), math.normalizesafe(x2)) < -0.99000000953674316;
      }

      private bool IsInverted(
        Entity edge,
        int index,
        NativeArray<AggregateElement> aggregateElements)
      {
        if (index > 0)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge1 = this.m_EdgeData[aggregateElements[index - 1].m_Edge];
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge2 = this.m_EdgeData[edge];
          return edge2.m_End == edge1.m_Start || edge2.m_End == edge1.m_End;
        }
        if (index >= aggregateElements.Length - 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        Game.Net.Edge edge3 = this.m_EdgeData[edge];
        // ISSUE: reference to a compiler-generated field
        Game.Net.Edge edge4 = this.m_EdgeData[aggregateElements[index + 1].m_Edge];
        return edge3.m_Start == edge4.m_Start || edge3.m_Start == edge4.m_End;
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

    private struct LabelVertexData
    {
      public Vector3 m_Position;
      public Vector3 m_Normal;
      public Color32 m_Color;
      public Vector4 m_UV0;
      public Vector4 m_UV1;
      public Vector4 m_UV2;
      public Vector4 m_UV3;
    }

    private struct SubMeshData
    {
      public int m_VertexCount;
      public Bounds3 m_Bounds;
    }

    [BurstCompile]
    private struct FillNameDataJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<LabelExtents> m_LabelExtentsType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> m_HiddenType;
      [ReadOnly]
      public BufferTypeHandle<LabelPosition> m_LabelPositionType;
      [ReadOnly]
      public BufferTypeHandle<LabelVertex> m_LabelVertexType;
      [ReadOnly]
      public ComponentLookup<NetNameData> m_NetNameData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public int m_SubMeshCount;
      public Mesh.MeshDataArray m_NameMeshData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<AggregateMeshSystem.SubMeshData> array = new NativeArray<AggregateMeshSystem.SubMeshData>(this.m_SubMeshCount, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<LabelPosition> bufferAccessor1 = chunk.GetBufferAccessor<LabelPosition>(ref this.m_LabelPositionType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<LabelVertex> bufferAccessor2 = chunk.GetBufferAccessor<LabelVertex>(ref this.m_LabelVertexType);
            for (int index2 = 0; index2 < bufferAccessor2.Length; ++index2)
            {
              DynamicBuffer<LabelPosition> dynamicBuffer1 = bufferAccessor1[index2];
              DynamicBuffer<LabelVertex> dynamicBuffer2 = bufferAccessor2[index2];
              for (int index3 = 0; index3 < dynamicBuffer1.Length; ++index3)
              {
                LabelPosition labelPosition = dynamicBuffer1[index3];
                for (int index4 = 0; index4 < dynamicBuffer2.Length; index4 += 4)
                {
                  int2 material = dynamicBuffer2[index4].m_Material;
                  int index5 = math.select(material.x, material.y, labelPosition.m_IsUnderground);
                  // ISSUE: reference to a compiler-generated field
                  array.ElementAt<AggregateMeshSystem.SubMeshData>(index5).m_VertexCount += 4;
                }
              }
            }
          }
        }
        int vertexCount = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SubMeshCount; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          vertexCount += array[index].m_VertexCount;
        }
        // ISSUE: reference to a compiler-generated field
        Mesh.MeshData meshData = this.m_NameMeshData[0];
        NativeArray<VertexAttributeDescriptor> attributes = new NativeArray<VertexAttributeDescriptor>(7, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        attributes[0] = new VertexAttributeDescriptor();
        attributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal);
        attributes[2] = new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4);
        attributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 4);
        attributes[4] = new VertexAttributeDescriptor(VertexAttribute.TexCoord1, dimension: 4);
        attributes[5] = new VertexAttributeDescriptor(VertexAttribute.TexCoord2, dimension: 4);
        attributes[6] = new VertexAttributeDescriptor(VertexAttribute.TexCoord3, dimension: 4);
        meshData.SetVertexBufferParams(vertexCount, attributes);
        meshData.SetIndexBufferParams((vertexCount >> 2) * 6, IndexFormat.UInt32);
        attributes.Dispose();
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        meshData.subMeshCount = this.m_SubMeshCount;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SubMeshCount; ++index)
        {
          ref AggregateMeshSystem.SubMeshData local = ref array.ElementAt<AggregateMeshSystem.SubMeshData>(index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          meshData.SetSubMesh(index, new SubMeshDescriptor()
          {
            firstVertex = num1,
            indexStart = (num1 >> 2) * 6,
            vertexCount = local.m_VertexCount,
            indexCount = (local.m_VertexCount >> 2) * 6,
            topology = MeshTopology.Triangles
          }, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
          // ISSUE: reference to a compiler-generated field
          num1 += local.m_VertexCount;
          // ISSUE: reference to a compiler-generated field
          local.m_VertexCount = 0;
          // ISSUE: reference to a compiler-generated field
          local.m_Bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        }
        NativeArray<AggregateMeshSystem.LabelVertexData> vertexData = meshData.GetVertexData<AggregateMeshSystem.LabelVertexData>();
        NativeArray<uint> indexData = meshData.GetIndexData<uint>();
        // ISSUE: reference to a compiler-generated field
        for (int index6 = 0; index6 < this.m_Chunks.Length; ++index6)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index6];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<LabelExtents> nativeArray1 = chunk.GetNativeArray<LabelExtents>(ref this.m_LabelExtentsType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<LabelPosition> bufferAccessor3 = chunk.GetBufferAccessor<LabelPosition>(ref this.m_LabelPositionType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<LabelVertex> bufferAccessor4 = chunk.GetBufferAccessor<LabelVertex>(ref this.m_LabelVertexType);
            for (int index7 = 0; index7 < bufferAccessor4.Length; ++index7)
            {
              LabelExtents labelExtents = nativeArray1[index7];
              PrefabRef prefabRef = nativeArray2[index7];
              DynamicBuffer<LabelPosition> dynamicBuffer3 = bufferAccessor3[index7];
              DynamicBuffer<LabelVertex> dynamicBuffer4 = bufferAccessor4[index7];
              // ISSUE: reference to a compiler-generated field
              NetNameData netNameData = this.m_NetNameData[prefabRef.m_Prefab];
              Color32 color32 = netNameData.m_Color;
              if (nativeArray3.Length != 0 && (nativeArray3[index7].m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace)) != (TempFlags) 0)
                color32 = netNameData.m_SelectedColor;
              float num2 = math.length(math.max(-labelExtents.m_Bounds.min, labelExtents.m_Bounds.max));
              for (int index8 = 0; index8 < dynamicBuffer3.Length; ++index8)
              {
                LabelPosition labelPosition = dynamicBuffer3[index8];
                float3 float3_1 = new float3(labelPosition.m_HalfLength, 0.0f, 0.0f);
                float2 xy1 = labelPosition.m_Curve.a.xy;
                float2 xy2 = labelPosition.m_Curve.b.xy;
                float4 float4_1 = new float4(labelPosition.m_Curve.c, labelPosition.m_Curve.a.z);
                float4 float4_2 = new float4(labelPosition.m_Curve.d, labelPosition.m_Curve.b.z);
                float3 float3_2 = MathUtils.Position(labelPosition.m_Curve, 0.5f);
                float num3 = num2 * labelPosition.m_MaxScale;
                Bounds3 bounds3 = new Bounds3(float3_2 - num3, float3_2 + num3);
                SubMeshDescriptor subMeshDescriptor = new SubMeshDescriptor();
                int num4 = -1;
                for (int index9 = 0; index9 < dynamicBuffer4.Length; index9 += 4)
                {
                  int2 material = dynamicBuffer4[index9].m_Material;
                  int index10 = math.select(material.x, material.y, labelPosition.m_IsUnderground);
                  ref AggregateMeshSystem.SubMeshData local = ref array.ElementAt<AggregateMeshSystem.SubMeshData>(index10);
                  if (index10 != num4)
                  {
                    subMeshDescriptor = meshData.GetSubMesh(index10);
                    // ISSUE: reference to a compiler-generated field
                    local.m_Bounds |= bounds3;
                    num4 = index10;
                  }
                  // ISSUE: reference to a compiler-generated field
                  int num5 = subMeshDescriptor.firstVertex + local.m_VertexCount;
                  // ISSUE: reference to a compiler-generated field
                  int index11 = subMeshDescriptor.indexStart + (local.m_VertexCount >> 2) * 6;
                  // ISSUE: reference to a compiler-generated field
                  local.m_VertexCount += 4;
                  indexData[index11] = (uint) num5;
                  indexData[index11 + 1] = (uint) (num5 + 1);
                  indexData[index11 + 2] = (uint) (num5 + 2);
                  indexData[index11 + 3] = (uint) (num5 + 2);
                  indexData[index11 + 4] = (uint) (num5 + 3);
                  indexData[index11 + 5] = (uint) num5;
                  for (int index12 = 0; index12 < 4; ++index12)
                  {
                    LabelVertex labelVertex = dynamicBuffer4[index9 + index12];
                    // ISSUE: variable of a compiler-generated type
                    AggregateMeshSystem.LabelVertexData labelVertexData;
                    // ISSUE: reference to a compiler-generated field
                    labelVertexData.m_Position = (Vector3) labelVertex.m_Position;
                    // ISSUE: reference to a compiler-generated field
                    labelVertexData.m_Normal = (Vector3) float3_1;
                    // ISSUE: reference to a compiler-generated field
                    labelVertexData.m_Color = new Color32((byte) ((int) labelVertex.m_Color.r * (int) color32.r >> 8), (byte) ((int) labelVertex.m_Color.g * (int) color32.g >> 8), (byte) ((int) labelVertex.m_Color.b * (int) color32.b >> 8), (byte) ((int) labelVertex.m_Color.a * (int) color32.a >> 8));
                    // ISSUE: reference to a compiler-generated field
                    labelVertexData.m_UV0 = (Vector4) new float4(labelVertex.m_UV0, xy1);
                    // ISSUE: reference to a compiler-generated field
                    labelVertexData.m_UV1 = (Vector4) new float4(labelPosition.m_MaxScale, labelVertex.m_UV1.y, xy2);
                    // ISSUE: reference to a compiler-generated field
                    labelVertexData.m_UV2 = (Vector4) float4_1;
                    // ISSUE: reference to a compiler-generated field
                    labelVertexData.m_UV3 = (Vector4) float4_2;
                    vertexData[num5 + index12] = labelVertexData;
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SubMeshCount; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          SubMeshDescriptor subMesh = meshData.GetSubMesh(index) with
          {
            bounds = RenderingUtils.ToBounds(array[index].m_Bounds)
          };
          meshData.SetSubMesh(index, subMesh, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
        }
        array.Dispose();
      }
    }

    private struct ArrowVertexData
    {
      public Vector3 m_Position;
      public Color32 m_Color;
      public Vector2 m_UV0;
      public Vector4 m_UV1;
    }

    [BurstCompile]
    private struct FillArrowDataJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<Hidden> m_HiddenType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<ArrowPosition> m_ArrowPositionType;
      [ReadOnly]
      public ComponentLookup<NetArrowData> m_NetArrowData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      public Mesh.MeshDataArray m_ArrowMeshData;

      public void Execute()
      {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<ArrowPosition> bufferAccessor = chunk.GetBufferAccessor<ArrowPosition>(ref this.m_ArrowPositionType);
            for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
            {
              DynamicBuffer<ArrowPosition> dynamicBuffer = bufferAccessor[index2];
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                if (dynamicBuffer[index3].m_IsUnderground)
                {
                  num3 += 4;
                  num4 += 6;
                }
                else
                {
                  num1 += 4;
                  num2 += 6;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        Mesh.MeshData meshData = this.m_ArrowMeshData[0];
        NativeArray<VertexAttributeDescriptor> attributes = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        attributes[0] = new VertexAttributeDescriptor();
        attributes[1] = new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4);
        attributes[2] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 2);
        attributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord1, dimension: 4);
        meshData.SetVertexBufferParams(num1 + num3, attributes);
        meshData.SetIndexBufferParams(num2 + num4, IndexFormat.UInt32);
        attributes.Dispose();
        meshData.subMeshCount = 2;
        ref Mesh.MeshData local1 = ref meshData;
        SubMeshDescriptor subMeshDescriptor = new SubMeshDescriptor();
        subMeshDescriptor.vertexCount = num1;
        subMeshDescriptor.indexCount = num2;
        subMeshDescriptor.topology = MeshTopology.Triangles;
        SubMeshDescriptor desc1 = subMeshDescriptor;
        local1.SetSubMesh(0, desc1, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
        ref Mesh.MeshData local2 = ref meshData;
        subMeshDescriptor = new SubMeshDescriptor();
        subMeshDescriptor.firstVertex = num1;
        subMeshDescriptor.indexStart = num2;
        subMeshDescriptor.vertexCount = num3;
        subMeshDescriptor.indexCount = num4;
        subMeshDescriptor.topology = MeshTopology.Triangles;
        SubMeshDescriptor desc2 = subMeshDescriptor;
        local2.SetSubMesh(1, desc2, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
        NativeArray<AggregateMeshSystem.ArrowVertexData> vertexData = meshData.GetVertexData<AggregateMeshSystem.ArrowVertexData>();
        NativeArray<uint> indexData = meshData.GetIndexData<uint>();
        SubMeshDescriptor subMesh1 = meshData.GetSubMesh(0);
        SubMeshDescriptor subMesh2 = meshData.GetSubMesh(1);
        Bounds3 bounds1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        Bounds3 bounds2 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        int vertexIndex = 0;
        int indexIndex = 0;
        int firstVertex = subMesh2.firstVertex;
        int indexStart = subMesh2.indexStart;
        // ISSUE: reference to a compiler-generated field
        for (int index4 = 0; index4 < this.m_Chunks.Length; ++index4)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index4];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<ArrowPosition> bufferAccessor = chunk.GetBufferAccessor<ArrowPosition>(ref this.m_ArrowPositionType);
            for (int index5 = 0; index5 < bufferAccessor.Length; ++index5)
            {
              PrefabRef prefabRef = nativeArray[index5];
              DynamicBuffer<ArrowPosition> dynamicBuffer = bufferAccessor[index5];
              // ISSUE: reference to a compiler-generated field
              NetArrowData netArrowData = this.m_NetArrowData[prefabRef.m_Prefab];
              float num5 = 20f;
              float num6 = num5 * 0.5f;
              for (int index6 = 0; index6 < dynamicBuffer.Length; ++index6)
              {
                ArrowPosition arrowPosition = dynamicBuffer[index6];
                Color32 color = arrowPosition.m_IsTrack ? netArrowData.m_TrackColor : netArrowData.m_RoadColor;
                float4 uv1 = new float4(arrowPosition.m_Position, arrowPosition.m_MaxScale);
                float3 z = arrowPosition.m_Direction * num5;
                float3 x = math.normalizesafe(new float3(-arrowPosition.m_Direction.z, 0.0f, arrowPosition.m_Direction.x), math.right()) * num6;
                float num7 = num5 * arrowPosition.m_MaxScale;
                if (arrowPosition.m_IsUnderground)
                {
                  bounds2 |= new Bounds3(arrowPosition.m_Position - num7, arrowPosition.m_Position + num7);
                  // ISSUE: reference to a compiler-generated method
                  this.AddArrow(vertexData, indexData, color, uv1, z, x, ref firstVertex, ref indexStart);
                }
                else
                {
                  bounds1 |= new Bounds3(arrowPosition.m_Position - num7, arrowPosition.m_Position + num7);
                  // ISSUE: reference to a compiler-generated method
                  this.AddArrow(vertexData, indexData, color, uv1, z, x, ref vertexIndex, ref indexIndex);
                }
              }
            }
          }
        }
        subMesh1.bounds = RenderingUtils.ToBounds(bounds1);
        subMesh2.bounds = RenderingUtils.ToBounds(bounds2);
        meshData.SetSubMesh(0, subMesh1, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
        meshData.SetSubMesh(1, subMesh2, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
      }

      private void AddArrow(
        NativeArray<AggregateMeshSystem.ArrowVertexData> vertices,
        NativeArray<uint> indices,
        Color32 color,
        float4 uv1,
        float3 z,
        float3 x,
        ref int vertexIndex,
        ref int indexIndex)
      {
        indices[indexIndex++] = (uint) vertexIndex;
        indices[indexIndex++] = (uint) (vertexIndex + 1);
        indices[indexIndex++] = (uint) (vertexIndex + 2);
        indices[indexIndex++] = (uint) (vertexIndex + 2);
        indices[indexIndex++] = (uint) (vertexIndex + 3);
        indices[indexIndex++] = (uint) vertexIndex;
        // ISSUE: variable of a compiler-generated type
        AggregateMeshSystem.ArrowVertexData arrowVertexData;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Position = (Vector3) (-x - z);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Color = color;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV0 = (Vector2) new float2(0.0f, 0.0f);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV1 = (Vector4) uv1;
        vertices[vertexIndex++] = arrowVertexData;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Position = (Vector3) (x - z);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Color = color;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV0 = (Vector2) new float2(1f, 0.0f);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV1 = (Vector4) uv1;
        vertices[vertexIndex++] = arrowVertexData;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Position = (Vector3) (x + z);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Color = color;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV0 = (Vector2) new float2(1f, 1f);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV1 = (Vector4) uv1;
        vertices[vertexIndex++] = arrowVertexData;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Position = (Vector3) (z - x);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_Color = color;
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV0 = (Vector2) new float2(0.0f, 1f);
        // ISSUE: reference to a compiler-generated field
        arrowVertexData.m_UV1 = (Vector4) uv1;
        vertices[vertexIndex++] = arrowVertexData;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<NetNameData> __Game_Prefabs_NetNameData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetArrowData> __Game_Prefabs_NetArrowData_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Updated> __Game_Common_Updated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BatchesUpdated> __Game_Common_BatchesUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<LabelExtents> __Game_Net_LabelExtents_RW_ComponentTypeHandle;
      public SharedComponentTypeHandle<LabelMaterial> __Game_Net_LabelMaterial_SharedComponentTypeHandle;
      public BufferTypeHandle<LabelVertex> __Game_Net_LabelVertex_RW_BufferTypeHandle;
      public SharedComponentTypeHandle<ArrowMaterial> __Game_Net_ArrowMaterial_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LabelExtents> __Game_Net_LabelExtents_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<AggregateElement> __Game_Net_AggregateElement_RO_BufferTypeHandle;
      public BufferTypeHandle<LabelPosition> __Game_Net_LabelPosition_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Aggregated> __Game_Net_Aggregated_RO_ComponentTypeHandle;
      public BufferTypeHandle<ArrowPosition> __Game_Net_ArrowPosition_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<NetCompositionCarriageway> __Game_Prefabs_NetCompositionCarriageway_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> __Game_Tools_Hidden_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LabelPosition> __Game_Net_LabelPosition_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LabelVertex> __Game_Net_LabelVertex_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetNameData> __Game_Prefabs_NetNameData_RO_ComponentLookup;
      [ReadOnly]
      public BufferTypeHandle<ArrowPosition> __Game_Net_ArrowPosition_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetArrowData> __Game_Prefabs_NetArrowData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetNameData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetNameData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetArrowData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetArrowData>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BatchesUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelExtents_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LabelExtents>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelMaterial_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<LabelMaterial>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelVertex_RW_BufferTypeHandle = state.GetBufferTypeHandle<LabelVertex>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ArrowMaterial_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<ArrowMaterial>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelExtents_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LabelExtents>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_AggregateElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<AggregateElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelPosition_RW_BufferTypeHandle = state.GetBufferTypeHandle<LabelPosition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Aggregated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Aggregated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ArrowPosition_RW_BufferTypeHandle = state.GetBufferTypeHandle<ArrowPosition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionCarriageway_RO_BufferLookup = state.GetBufferLookup<NetCompositionCarriageway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelPosition_RO_BufferTypeHandle = state.GetBufferTypeHandle<LabelPosition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LabelVertex_RO_BufferTypeHandle = state.GetBufferTypeHandle<LabelVertex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetNameData_RO_ComponentLookup = state.GetComponentLookup<NetNameData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ArrowPosition_RO_BufferTypeHandle = state.GetBufferTypeHandle<ArrowPosition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetArrowData_RO_ComponentLookup = state.GetComponentLookup<NetArrowData>(true);
      }
    }
  }
}
