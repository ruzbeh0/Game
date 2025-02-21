// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ManagedBatchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.IO.AssetDatabase.VirtualTexturing;
using Colossal.Mathematics;
using Colossal.Rendering;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class ManagedBatchSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private RenderingSystem m_RenderingSystem;
    private BatchMeshSystem m_BatchMeshSystem;
    private BatchManagerSystem m_BatchManagerSystem;
    private TextureStreamingSystem m_TextureStreamingSystem;
    private System.Collections.Generic.Dictionary<ManagedBatchSystem.MaterialKey, Material> m_Materials;
    private System.Collections.Generic.Dictionary<ManagedBatchSystem.GroupKey, Entity> m_Groups;
    private System.Collections.Generic.Dictionary<ManagedBatchSystem.MeshKey, Entity> m_Meshes;
    private List<ManagedBatchSystem.KeywordData> m_CachedKeywords;
    private List<ManagedBatchSystem.TextureData> m_CachedTextures;
    private ManagedBatchSystem.MaterialKey m_CachedMaterialKey;
    private ManagedBatchSystem.GroupKey m_CachedGroupKey;
    private VTTextureRequester m_VTTextureRequester;
    private JobHandle m_VTRequestDependencies;
    private EntityQuery m_MeshSettingsQuery;
    private bool m_VTRequestsUpdated;
    private int m_TunnelLayer;
    private int m_MovingLayer;
    private int m_PipelineLayer;
    private int m_SubPipelineLayer;
    private int m_WaterwayLayer;
    private int m_OutlineLayer;
    private int m_MarkerLayer;
    private int m_DecalLayerMask;
    private int m_AnimationTexture;
    private int m_UseStack1;
    private int m_ImpostorSize;
    private int m_ImpostorOffset;
    private int m_WorldspaceAlbedo;
    private int m_MaskMap;

    public int materialCount => this.m_Materials.Count;

    public int groupCount { get; private set; }

    public int batchCount { get; private set; }

    public IReadOnlyDictionary<ManagedBatchSystem.MaterialKey, Material> materials
    {
      get => (IReadOnlyDictionary<ManagedBatchSystem.MaterialKey, Material>) this.m_Materials;
    }

    public VTTextureRequester VTTextureRequester => this.m_VTTextureRequester;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchMeshSystem = this.World.GetOrCreateSystemManaged<BatchMeshSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TextureStreamingSystem = this.World.GetOrCreateSystemManaged<TextureStreamingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Materials = new System.Collections.Generic.Dictionary<ManagedBatchSystem.MaterialKey, Material>();
      // ISSUE: reference to a compiler-generated field
      this.m_Groups = new System.Collections.Generic.Dictionary<ManagedBatchSystem.GroupKey, Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_Meshes = new System.Collections.Generic.Dictionary<ManagedBatchSystem.MeshKey, Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_CachedKeywords = new List<ManagedBatchSystem.KeywordData>();
      // ISSUE: reference to a compiler-generated field
      this.m_CachedTextures = new List<ManagedBatchSystem.TextureData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_VTTextureRequester = new VTTextureRequester(this.m_TextureStreamingSystem);
      // ISSUE: reference to a compiler-generated field
      this.m_MeshSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<MeshSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TunnelLayer = LayerMask.NameToLayer("Tunnel");
      // ISSUE: reference to a compiler-generated field
      this.m_MovingLayer = LayerMask.NameToLayer("Moving");
      // ISSUE: reference to a compiler-generated field
      this.m_PipelineLayer = LayerMask.NameToLayer("Pipeline");
      // ISSUE: reference to a compiler-generated field
      this.m_SubPipelineLayer = LayerMask.NameToLayer("SubPipeline");
      // ISSUE: reference to a compiler-generated field
      this.m_WaterwayLayer = LayerMask.NameToLayer("Waterway");
      // ISSUE: reference to a compiler-generated field
      this.m_OutlineLayer = LayerMask.NameToLayer("Outline");
      // ISSUE: reference to a compiler-generated field
      this.m_MarkerLayer = LayerMask.NameToLayer("Marker");
      // ISSUE: reference to a compiler-generated field
      this.m_DecalLayerMask = Shader.PropertyToID("colossal_DecalLayerMask");
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationTexture = Shader.PropertyToID("_AnimationTexture");
      // ISSUE: reference to a compiler-generated field
      this.m_UseStack1 = Shader.PropertyToID("colossal_UseStack1");
      // ISSUE: reference to a compiler-generated field
      this.m_ImpostorSize = Shader.PropertyToID("_ImpostorSize");
      // ISSUE: reference to a compiler-generated field
      this.m_ImpostorOffset = Shader.PropertyToID("_ImpostorOffset");
      // ISSUE: reference to a compiler-generated field
      this.m_WorldspaceAlbedo = Shader.PropertyToID("_WorldspaceAlbedo");
      // ISSUE: reference to a compiler-generated field
      this.m_MaskMap = Shader.PropertyToID("_MaskMap");
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<ManagedBatchSystem.MaterialKey, Material> material in this.m_Materials)
        CoreUtils.Destroy((UnityEngine.Object) material.Value);
      // ISSUE: reference to a compiler-generated field
      this.m_VTRequestDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_VTTextureRequester.Dispose();
      base.OnDestroy();
    }

    public void RemoveMesh(Entity oldMesh, Entity newMesh = default (Entity))
    {
      List<ManagedBatchSystem.GroupKey> groupKeyList = new List<ManagedBatchSystem.GroupKey>();
      List<ManagedBatchSystem.MeshKey> meshKeyList = new List<ManagedBatchSystem.MeshKey>();
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<ManagedBatchSystem.GroupKey, Entity> group in this.m_Groups)
      {
        if (group.Key.mesh == oldMesh || group.Value == oldMesh)
          groupKeyList.Add(group.Key);
      }
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<ManagedBatchSystem.MeshKey, Entity> mesh in this.m_Meshes)
      {
        if (mesh.Value == oldMesh)
          meshKeyList.Add(mesh.Key);
      }
      bool flag = false;
      Game.Prefabs.MeshData component;
      RenderPrefab prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.EntityManager.TryGetComponent<Game.Prefabs.MeshData>(newMesh, out component) && this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(newMesh, out prefab) && (UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ManagedBatchSystem.MeshKey other = new ManagedBatchSystem.MeshKey(prefab, component);
        foreach (ManagedBatchSystem.MeshKey key in meshKeyList)
        {
          // ISSUE: reference to a compiler-generated method
          if (key.Equals(other))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Meshes[key] = newMesh;
            flag = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Meshes.Remove(key);
          }
        }
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_BatchMeshSystem.ReplaceMesh(oldMesh, newMesh);
        }
      }
      else
      {
        foreach (ManagedBatchSystem.MeshKey key in meshKeyList)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Meshes.Remove(key);
        }
      }
      foreach (ManagedBatchSystem.GroupKey key in groupKeyList)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Groups[key] == oldMesh)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Groups.Remove(key);
          --this.groupCount;
          this.batchCount -= key.batches.Count;
        }
      }
    }

    public void ResetSharedMeshes()
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies.Complete();
      int groupCount = nativeBatchGroups.GetGroupCount();
      for (int groupIndex = 0; groupIndex < groupCount; ++groupIndex)
      {
        if (nativeBatchGroups.IsValidGroup(groupIndex))
        {
          int batchCount = nativeBatchGroups.GetBatchCount(groupIndex);
          GroupData groupData = nativeBatchGroups.GetGroupData(groupIndex);
          // ISSUE: variable of a compiler-generated type
          ManagedBatchSystem.GroupKey groupKey = (ManagedBatchSystem.GroupKey) null;
          Entity mesh = groupData.m_Mesh;
          RenderPrefab prefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(groupData.m_Mesh, out prefab))
          {
            if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
            {
              try
              {
                Game.Prefabs.MeshData componentData = this.EntityManager.GetComponentData<Game.Prefabs.MeshData>(groupData.m_Mesh);
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                ManagedBatchSystem.MeshKey key = new ManagedBatchSystem.MeshKey(prefab, componentData);
                // ISSUE: reference to a compiler-generated field
                if (!this.m_Meshes.TryGetValue(key, out mesh))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_Meshes.Add(key, groupData.m_Mesh);
                  mesh = groupData.m_Mesh;
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_CachedGroupKey != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  groupKey = this.m_CachedGroupKey;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CachedGroupKey = (ManagedBatchSystem.GroupKey) null;
                }
                else
                {
                  // ISSUE: object of a compiler-generated type is created
                  groupKey = new ManagedBatchSystem.GroupKey();
                }
                // ISSUE: reference to a compiler-generated method
                groupKey.Initialize(mesh, groupData);
              }
              catch (Exception ex)
              {
                COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when initializing batches for {0}", (object) prefab.name);
              }
            }
          }
          for (int batchIndex = 0; batchIndex < batchCount; ++batchIndex)
          {
            int managedBatchIndex = nativeBatchGroups.GetManagedBatchIndex(groupIndex, batchIndex);
            if (managedBatchIndex >= 0)
            {
              // ISSUE: object of a compiler-generated type is created
              groupKey?.batches.Add(new ManagedBatchSystem.GroupKey.Batch((CustomBatch) managedBatches.GetBatch(managedBatchIndex)));
            }
            else
              goto label_19;
          }
          // ISSUE: reference to a compiler-generated field
          if (groupKey != null && this.m_Groups.TryAdd(groupKey, groupData.m_Mesh))
          {
            ++groupCount;
            this.batchCount += batchCount;
            continue;
          }
label_19:
          if (groupKey != null)
          {
            // ISSUE: reference to a compiler-generated method
            groupKey.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_CachedGroupKey = groupKey;
          }
        }
      }
    }

    [Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies.Complete();
      UpdatedManagedBatchEnumerator updatedManagedBatches = nativeBatchGroups.GetUpdatedManagedBatches();
      int groupIndex;
      while (updatedManagedBatches.GetNextUpdatedGroup(out groupIndex))
      {
        int batchCount = nativeBatchGroups.GetBatchCount(groupIndex);
        GroupData groupData = nativeBatchGroups.GetGroupData(groupIndex);
        // ISSUE: variable of a compiler-generated type
        ManagedBatchSystem.GroupKey key1 = (ManagedBatchSystem.GroupKey) null;
        Entity mesh = groupData.m_Mesh;
        RenderPrefab prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(groupData.m_Mesh, out prefab))
        {
          if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
          {
            try
            {
              Game.Prefabs.MeshData componentData = this.EntityManager.GetComponentData<Game.Prefabs.MeshData>(groupData.m_Mesh);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              ManagedBatchSystem.MeshKey key2 = new ManagedBatchSystem.MeshKey(prefab, componentData);
              // ISSUE: reference to a compiler-generated field
              if (!this.m_Meshes.TryGetValue(key2, out mesh))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Meshes.Add(key2, groupData.m_Mesh);
                mesh = groupData.m_Mesh;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_CachedGroupKey != null)
              {
                // ISSUE: reference to a compiler-generated field
                key1 = this.m_CachedGroupKey;
                // ISSUE: reference to a compiler-generated field
                this.m_CachedGroupKey = (ManagedBatchSystem.GroupKey) null;
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                key1 = new ManagedBatchSystem.GroupKey();
              }
              // ISSUE: reference to a compiler-generated method
              key1.Initialize(mesh, groupData);
            }
            catch (Exception ex)
            {
              COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when initializing batches for {0}", (object) prefab.name);
            }
          }
        }
        for (int index = 0; index < batchCount; ++index)
        {
          int batchIndex = nativeBatchGroups.GetManagedBatchIndex(groupIndex, index);
          if (batchIndex < 0)
          {
            try
            {
              BatchData batchData = nativeBatchGroups.GetBatchData(groupIndex, index);
              PropertyData lodFadeData;
              // ISSUE: reference to a compiler-generated method
              CustomBatch batch = this.CreateBatch(groupIndex, index, mesh, ref groupData, ref batchData, out lodFadeData);
              nativeBatchGroups.SetBatchData(groupIndex, index, batchData);
              BatchFlags sourceFlags = batch.sourceFlags;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (!this.m_BatchManagerSystem.IsMotionVectorsEnabled())
                sourceFlags &= ~BatchFlags.MotionVectors;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (!this.m_BatchManagerSystem.IsLodFadeEnabled())
                sourceFlags &= ~BatchFlags.LodFade;
              OptionalProperties optionalProperties = new OptionalProperties(sourceFlags, batch.sourceType);
              batchIndex = managedBatches.AddBatch<CullingData, GroupData, BatchData, InstanceData>((ManagedBatch) batch, index, nativeBatchGroups);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_BatchMeshSystem.AddBatch(batch, batchIndex);
              NativeBatchProperties batchProperties = managedBatches.GetBatchProperties(batch.material.shader, optionalProperties);
              nativeBatchGroups.SetBatchProperties(groupIndex, index, batchProperties);
              if (lodFadeData.m_DataIndex >= 0)
                nativeBatchGroups.SetBatchDataIndex(groupIndex, index, lodFadeData.m_NameID, lodFadeData.m_DataIndex);
              WriteableBatchDefaultsAccessor defaultsAccessor = nativeBatchGroups.GetBatchDefaultsAccessor(groupIndex, index);
              if ((AssetData) batch.sourceSurface != (IAssetData) null)
              {
                // ISSUE: reference to a compiler-generated method
                managedBatches.SetDefaults(ManagedBatchSystem.GetTemplate(batch.sourceSurface), batch.sourceSurface.floats, batch.sourceSurface.ints, batch.sourceSurface.vectors, batch.sourceSurface.colors, batch.customProps, batchProperties, defaultsAccessor);
              }
              else
                managedBatches.SetDefaults(batch.sourceMaterial, batch.customProps, batchProperties, defaultsAccessor);
            }
            catch (Exception ex)
            {
              if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
              {
                COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when initializing batch {0} for {1}", (object) index, (object) prefab.name);
                continue;
              }
              COSystemBase.baseLog.ErrorFormat(ex, "Error when initializing batch {0} for {1}", (object) index, (object) groupData.m_Mesh);
              continue;
            }
          }
          // ISSUE: object of a compiler-generated type is created
          key1?.batches.Add(new ManagedBatchSystem.GroupKey.Batch((CustomBatch) managedBatches.GetBatch(batchIndex)));
        }
        nativeBatchGroups.SetGroupData(groupIndex, groupData);
        if (key1 != null)
        {
          Entity meshEntity;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Groups.TryGetValue(key1, out meshEntity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_BatchManagerSystem.MergeGroups(meshEntity, groupIndex);
            // ISSUE: reference to a compiler-generated method
            key1.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_CachedGroupKey = key1;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Groups.Add(key1, groupData.m_Mesh);
            ++this.groupCount;
            this.batchCount += batchCount;
          }
        }
        else
        {
          ++this.groupCount;
          this.batchCount += batchCount;
        }
      }
      nativeBatchGroups.ClearUpdatedManagedBatches();
    }

    public void EnabledShadersUpdated()
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies.Complete();
      int groupCount = nativeBatchGroups.GetGroupCount();
      for (int groupIndex = 0; groupIndex < groupCount; ++groupIndex)
      {
        if (nativeBatchGroups.IsValidGroup(groupIndex))
        {
          int batchCount = nativeBatchGroups.GetBatchCount(groupIndex);
          GroupData groupData = nativeBatchGroups.GetGroupData(groupIndex);
          groupData.m_RenderFlags &= ~BatchRenderFlags.IsEnabled;
          for (int batchIndex = 0; batchIndex < batchCount; ++batchIndex)
          {
            int managedBatchIndex = nativeBatchGroups.GetManagedBatchIndex(groupIndex, batchIndex);
            if (managedBatchIndex >= 0)
            {
              CustomBatch batch = (CustomBatch) managedBatches.GetBatch(managedBatchIndex);
              BatchData batchData = nativeBatchGroups.GetBatchData(groupIndex, batchIndex);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_RenderingSystem.IsShaderEnabled(batch.loadedMaterial.shader))
              {
                groupData.m_RenderFlags |= BatchRenderFlags.IsEnabled;
                batchData.m_RenderFlags |= BatchRenderFlags.IsEnabled;
              }
              else
                batchData.m_RenderFlags &= ~BatchRenderFlags.IsEnabled;
              nativeBatchGroups.SetBatchData(groupIndex, batchIndex, batchData);
            }
          }
          nativeBatchGroups.SetGroupData(groupIndex, groupData);
        }
      }
    }

    public JobHandle GetVTRequestMaxPixels(
      out NativeList<float> maxPixels0,
      out NativeList<float> maxPixels1)
    {
      // ISSUE: reference to a compiler-generated field
      maxPixels0 = this.m_VTTextureRequester.TexturesMaxPixels[0];
      // ISSUE: reference to a compiler-generated field
      maxPixels1 = this.m_VTTextureRequester.TexturesMaxPixels[1];
      // ISSUE: reference to a compiler-generated field
      return this.m_VTRequestDependencies;
    }

    public void AddVTRequestWriter(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_VTRequestDependencies = dependencies;
      // ISSUE: reference to a compiler-generated field
      this.m_VTRequestsUpdated = true;
    }

    public void ResetVT(int desiredMipBias, UnityEngine.Rendering.VirtualTexturing.FilterMode filterMode)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TextureStreamingSystem.ShouldResetVT(desiredMipBias, filterMode))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_VTRequestDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_VTTextureRequester.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_TextureStreamingSystem.Initialize(desiredMipBias, filterMode);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.VirtualTexturingUpdated();
    }

    public void CompleteVTRequests()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_VTRequestsUpdated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_VTRequestDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_VTTextureRequester.UpdateTexturesVTRequests();
        // ISSUE: reference to a compiler-generated field
        this.m_VTRequestsUpdated = false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_TextureStreamingSystem.UpdateWorkingSetMipBias();
    }

    private CustomBatch CreateBatch(
      int groupIndex,
      int batchIndex,
      Entity sharedMesh,
      ref GroupData groupData,
      ref BatchData batchData,
      out PropertyData lodFadeData)
    {
      SurfaceAsset surfaceAsset = (SurfaceAsset) null;
      Material material = (Material) null;
      Material defaultMaterial = (Material) null;
      Material loadedMaterial = (Material) null;
      MaterialPropertyBlock customProps = (MaterialPropertyBlock) null;
      Entity entity1 = Entity.Null;
      ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;
      bool flag1 = true;
      BatchFlags flags = (BatchFlags) 0;
      GeneratedType generatedType = GeneratedType.None;
      int num1 = 0;
      int num2 = (int) batchData.m_SubMeshIndex;
      // ISSUE: variable of a compiler-generated type
      ManagedBatchSystem.MaterialKey materialKey;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CachedMaterialKey != null)
      {
        // ISSUE: reference to a compiler-generated field
        materialKey = this.m_CachedMaterialKey;
        // ISSUE: reference to a compiler-generated field
        this.m_CachedMaterialKey = (ManagedBatchSystem.MaterialKey) null;
      }
      else
      {
        // ISSUE: object of a compiler-generated type is created
        materialKey = new ManagedBatchSystem.MaterialKey();
      }
      lodFadeData = new PropertyData() { m_DataIndex = -1 };
      // ISSUE: reference to a compiler-generated field
      this.m_CachedKeywords.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_CachedTextures.Clear();
      Entity entity2;
      Entity mesh;
      if (batchData.m_LodMesh != Entity.Null)
      {
        entity2 = batchData.m_LodMesh;
        mesh = groupData.m_Mesh;
      }
      else
      {
        entity2 = groupData.m_Mesh;
        mesh = Entity.Null;
      }
      if (batchData.m_LodIndex > (byte) 0)
        flags |= BatchFlags.Lod;
      if (groupData.m_MeshType == MeshType.Zone)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        material = this.m_PrefabSystem.GetPrefab<ZoneBlockPrefab>(entity2).m_Material;
        // ISSUE: reference to a compiler-generated method
        materialKey.Initialize(material);
        if (groupData.m_Partition >= (ushort) 1)
          flags |= BatchFlags.Extended1;
        if (groupData.m_Partition >= (ushort) 2)
          flags |= BatchFlags.Extended2;
        if (groupData.m_Partition >= (ushort) 3)
          flags |= BatchFlags.Extended3;
        shadowCastingMode = ShadowCastingMode.Off;
        flag1 = false;
      }
      else if (groupData.m_MeshType == MeshType.Net)
      {
        EntityManager entityManager = this.EntityManager;
        NetCompositionMeshData componentData = entityManager.GetComponentData<NetCompositionMeshData>(entity2);
        entityManager = this.EntityManager;
        DynamicBuffer<MeshMaterial> buffer1 = entityManager.GetBuffer<MeshMaterial>(entity2, true);
        entityManager = this.EntityManager;
        DynamicBuffer<NetCompositionPiece> buffer2 = entityManager.GetBuffer<NetCompositionPiece>(entity2, true);
        int materialIndex = buffer1[num2].m_MaterialIndex;
        for (int index1 = 0; index1 < buffer2.Length; ++index1)
        {
          NetCompositionPiece compositionPiece = buffer2[index1];
          DynamicBuffer<MeshMaterial> buffer3;
          if (this.EntityManager.TryGetBuffer<MeshMaterial>(compositionPiece.m_Piece, true, out buffer3))
          {
            for (int index2 = 0; index2 < buffer3.Length; ++index2)
            {
              if (buffer3[index2].m_MaterialIndex == materialIndex)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                surfaceAsset = this.m_PrefabSystem.GetPrefab<RenderPrefab>(compositionPiece.m_Piece).GetSurfaceAsset(index2);
                surfaceAsset.LoadProperties(true);
                // ISSUE: reference to a compiler-generated method
                materialKey.Initialize(surfaceAsset);
                goto label_26;
              }
            }
          }
        }
label_26:
        materialKey.decalLayerMask = 2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        lodFadeData = this.m_BatchManagerSystem.GetPropertyData(((int) batchData.m_LodIndex & 1) == 0 ? NetProperty.LodFade0 : NetProperty.LodFade1);
        flags |= BatchFlags.InfoviewColor | BatchFlags.LodFade;
        generatedType = GeneratedType.NetComposition;
        if ((componentData.m_Flags.m_General & CompositionFlags.General.Node) != (CompositionFlags.General) 0)
          flags |= BatchFlags.Node;
        if ((componentData.m_Flags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
          flags |= BatchFlags.Roundabout;
        if ((componentData.m_State & MeshFlags.Default) != (MeshFlags) 0)
        {
          materialKey.textures.Clear();
          // ISSUE: reference to a compiler-generated field
          materialKey.textures.Add(this.m_WorldspaceAlbedo, (object) Texture2D.grayTexture);
        }
        batchData.m_ShadowArea = float.PositiveInfinity;
        batchData.m_ShadowHeight = 1f;
        if ((componentData.m_Flags.m_General & CompositionFlags.General.Elevated) != (CompositionFlags.General) 0 || (componentData.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered | CompositionFlags.Side.SoundBarrier)) != (CompositionFlags.Side) 0 || (componentData.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered | CompositionFlags.Side.SoundBarrier)) != (CompositionFlags.Side) 0 || (componentData.m_State & MeshFlags.Default) != (MeshFlags) 0)
          batchData.m_ShadowHeight = componentData.m_Width;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        RenderPrefab meshPrefab = this.m_PrefabSystem.GetPrefab<RenderPrefab>(entity2);
        Game.Prefabs.MeshData componentData1 = this.EntityManager.GetComponentData<Game.Prefabs.MeshData>(entity2);
        EntityManager entityManager = this.EntityManager;
        SharedMeshData componentData2 = entityManager.GetComponentData<SharedMeshData>(entity2);
        RenderPrefab renderPrefab = (RenderPrefab) null;
        if (mesh != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          renderPrefab = this.m_PrefabSystem.GetPrefab<RenderPrefab>(mesh);
        }
        if (batchData.m_LodIndex > (byte) 0)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ManagedBatchSystem.MeshKey key = new ManagedBatchSystem.MeshKey(meshPrefab, componentData1);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Meshes.TryGetValue(key, out sharedMesh))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Meshes.Add(key, entity2);
            sharedMesh = entity2;
          }
        }
        componentData2.m_Mesh = sharedMesh;
        entityManager = this.EntityManager;
        entityManager.SetComponentData<SharedMeshData>(entity2, componentData2);
        DecalProperties decalProperties = meshPrefab.GetComponent<DecalProperties>();
        AnimationProperties component1 = meshPrefab.GetComponent<AnimationProperties>();
        ProceduralAnimationProperties component2 = meshPrefab.GetComponent<ProceduralAnimationProperties>();
        EmissiveProperties component3 = meshPrefab.GetComponent<EmissiveProperties>();
        ColorProperties component4 = meshPrefab.GetComponent<ColorProperties>();
        CurveProperties component5 = meshPrefab.GetComponent<CurveProperties>();
        DilationProperties component6 = meshPrefab.GetComponent<DilationProperties>();
        OverlayProperties component7 = meshPrefab.GetComponent<OverlayProperties>();
        BaseProperties component8 = meshPrefab.GetComponent<BaseProperties>();
        if ((UnityEngine.Object) component3 == (UnityEngine.Object) null && (UnityEngine.Object) renderPrefab != (UnityEngine.Object) null)
          component3 = renderPrefab.GetComponent<EmissiveProperties>();
        if ((UnityEngine.Object) component4 == (UnityEngine.Object) null && (UnityEngine.Object) renderPrefab != (UnityEngine.Object) null)
          component4 = renderPrefab.GetComponent<ColorProperties>();
        if ((UnityEngine.Object) component8 == (UnityEngine.Object) null && (UnityEngine.Object) renderPrefab != (UnityEngine.Object) null)
          component8 = renderPrefab.GetComponent<BaseProperties>();
        if ((UnityEngine.Object) decalProperties != (UnityEngine.Object) null && groupData.m_Layer == MeshLayer.Outline)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          meshPrefab = this.m_PrefabSystem.GetPrefab<RenderPrefab>(this.m_MeshSettingsQuery.GetSingleton<MeshSettingsData>().m_MissingObjectMesh);
          num2 = 0;
          surfaceAsset = meshPrefab.GetSurfaceAsset(num2);
          decalProperties = (DecalProperties) null;
        }
        else if ((componentData1.m_State & MeshFlags.Base) != (MeshFlags) 0 && num2 == componentData1.m_SubMeshCount)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          meshPrefab = !((UnityEngine.Object) component8 != (UnityEngine.Object) null) ? this.m_PrefabSystem.GetPrefab<RenderPrefab>(this.m_MeshSettingsQuery.GetSingleton<MeshSettingsData>().m_DefaultBaseMesh) : component8.m_BaseType;
          num2 = 0;
          surfaceAsset = meshPrefab.GetSurfaceAsset(num2);
          generatedType = GeneratedType.ObjectBase;
        }
        else
          surfaceAsset = meshPrefab.GetSurfaceAsset(num2);
        if (groupData.m_MeshType == MeshType.Object)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          lodFadeData = this.m_BatchManagerSystem.GetPropertyData(((int) batchData.m_LodIndex & 1) == 0 ? ObjectProperty.LodFade0 : ObjectProperty.LodFade1);
        }
        else if (groupData.m_MeshType == MeshType.Lane)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          lodFadeData = this.m_BatchManagerSystem.GetPropertyData(((int) batchData.m_LodIndex & 1) == 0 ? LaneProperty.LodFade0 : LaneProperty.LodFade1);
        }
        surfaceAsset.LoadProperties(true);
        // ISSUE: reference to a compiler-generated method
        materialKey.Initialize(surfaceAsset);
        Bounds3 bounds1 = RenderingUtils.SafeBounds(componentData1.m_Bounds);
        float3 float3_1 = MathUtils.Center(bounds1);
        float3 float3_2 = MathUtils.Size(bounds1);
        float4 float4_1 = new float4(float3_2, float3_1.y);
        batchData.m_ShadowHeight = float3_2.y;
        batchData.m_ShadowArea = math.sqrt((float) ((double) float3_2.x * (double) float3_2.x + (double) float3_2.z * (double) float3_2.z)) * batchData.m_ShadowHeight;
        VTAtlassingInfo[] vtAtlassingInfoArray = surfaceAsset.VTAtlassingInfos ?? surfaceAsset.PreReservedAtlassingInfos;
        int lod = (int) componentData1.m_MinLod;
        if (groupData.m_MeshType == MeshType.Lane)
          lod = (int) groupData.m_Partition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PropertyData propertyData1 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.TextureArea);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PropertyData propertyData2 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.MeshSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PropertyData propertyData3 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.LodDistanceFactor);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PropertyData propertyData4 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.SingleLightsOffset);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PropertyData propertyData5 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.DilationParams);
        if ((UnityEngine.Object) decalProperties != (UnityEngine.Object) null)
        {
          materialKey.renderQueue = materialKey.template.shader.renderQueue + decalProperties.m_RendererPriority;
          if (customProps == null)
            customProps = new MaterialPropertyBlock();
          customProps.SetVector(propertyData1.m_NameID, (Vector4) new float4(decalProperties.m_TextureArea.min, decalProperties.m_TextureArea.max));
          customProps.SetVector(propertyData2.m_NameID, (Vector4) float4_1);
          customProps.SetFloat(propertyData3.m_NameID, RenderingUtils.CalculateDistanceFactor(lod));
          materialKey.decalLayerMask = (int) decalProperties.m_LayerMask;
          if (vtAtlassingInfoArray != null)
          {
            Bounds2 bounds2 = MathUtils.Bounds(decalProperties.m_TextureArea.min, decalProperties.m_TextureArea.max);
            // ISSUE: reference to a compiler-generated field
            this.m_VTRequestDependencies.Complete();
            if (vtAtlassingInfoArray.Length >= 1 && vtAtlassingInfoArray[0].IndexInStack >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              batchData.m_VTIndex0 = this.m_VTTextureRequester.RegisterTexture(0, vtAtlassingInfoArray[0].StackGlobalIndex, vtAtlassingInfoArray[0].IndexInStack, bounds2);
            }
            if (vtAtlassingInfoArray.Length >= 2 && vtAtlassingInfoArray[1].IndexInStack >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              batchData.m_VTIndex1 = this.m_VTTextureRequester.RegisterTexture(1, vtAtlassingInfoArray[1].StackGlobalIndex, vtAtlassingInfoArray[1].IndexInStack, bounds2);
            }
            batchData.m_VTSizeFactor = math.cmax(float3_2);
          }
          if (groupData.m_MeshType == MeshType.Lane)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            PropertyData propertyData6 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.SmoothingDistance);
            customProps.SetFloat(propertyData6.m_NameID, componentData1.m_SmoothingDistance);
          }
          if (decalProperties.m_EnableInfoviewColor)
            flags |= BatchFlags.InfoviewColor;
        }
        else
        {
          DecalLayers decalLayers = componentData1.m_DecalLayer != (DecalLayers) 0 ? componentData1.m_DecalLayer : DecalLayers.Other;
          materialKey.decalLayerMask = (int) decalLayers;
          flags |= BatchFlags.InfoviewColor | BatchFlags.LodFade | BatchFlags.SurfaceState;
        }
        bool flag2 = groupData.m_MeshType == MeshType.Object;
        bool flag3 = groupData.m_MeshType == MeshType.Lane;
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.m_Clips != null && component1.m_Clips.Length != 0)
        {
          AnimationProperties.BakedAnimationClip clip = component1.m_Clips[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.SetTexture(materialKey, this.m_AnimationTexture, (Texture) clip.animationTexture);
          // ISSUE: reference to a compiler-generated method
          this.DisableKeyword(materialKey, "_GPU_ANIMATION_TEXTURE");
          flags |= BatchFlags.MotionVectors | BatchFlags.Animated;
        }
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.m_Bones != null && component2.m_Bones.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PropertyData propertyData7 = this.m_BatchManagerSystem.GetPropertyData(ObjectProperty.BoneParameters);
          if (customProps == null)
            customProps = new MaterialPropertyBlock();
          customProps.SetVector(propertyData7.m_NameID, (Vector4) new Vector2(math.asfloat(0), math.asfloat(component2.m_Bones.Length)));
          // ISSUE: reference to a compiler-generated method
          this.EnableKeyword(materialKey, "_GPU_ANIMATION_PROCEDURAL");
          flags |= BatchFlags.MotionVectors | BatchFlags.Bones;
          flag2 = false;
        }
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && component3.hasAnyLights)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PropertyData propertyData8 = this.m_BatchManagerSystem.GetPropertyData(ObjectProperty.LightParameters);
          if (customProps == null)
            customProps = new MaterialPropertyBlock();
          customProps.SetVector(propertyData8.m_NameID, (Vector4) new Vector2(0.0f, (float) component3.lightsCount));
          customProps.SetFloat(propertyData4.m_NameID, (float) component3.GetSingleLightOffset((int) batchData.m_SubMeshIndex));
          if (!component3.IsSingleLightMaterialId((int) batchData.m_SubMeshIndex))
          {
            // ISSUE: reference to a compiler-generated method
            this.EnableKeyword(materialKey, "_EMISSIVE_PROCEDURAL");
          }
          flags |= BatchFlags.Emissive;
        }
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && component4.m_ColorVariations != null && component4.m_ColorVariations.Count != 0)
          flags |= BatchFlags.ColorMask;
        if ((componentData1.m_State & MeshFlags.Character) != (MeshFlags) 0)
        {
          if (customProps == null)
            customProps = new MaterialPropertyBlock();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_BatchMeshSystem.SetShapeParameters(customProps, sharedMesh, num2);
          flags |= BatchFlags.Bones | BatchFlags.BlendWeights;
        }
        if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
        {
          if (component5.m_GeometryTiling)
          {
            if (customProps == null)
              customProps = new MaterialPropertyBlock();
            float4 float4_2 = new float4(0.1f, 10f, 0.0f, 0.0f);
            if (componentData1.m_TilingCount != 0 && component5.m_StraightTiling)
            {
              float4_2.x = 1f / (float) componentData1.m_TilingCount;
              float4_2.y = 0.01f;
            }
            customProps.SetVector(propertyData5.m_NameID, (Vector4) float4_2);
            // ISSUE: reference to a compiler-generated method
            this.EnableKeyword(materialKey, "COLOSSAL_GEOMETRY_TILING");
            flag3 = false;
          }
          if (component5.m_SubFlow)
            flags |= BatchFlags.InfoviewFlow;
          if (component5.m_HangingSwaying)
          {
            flags |= BatchFlags.Hanging;
            float num3 = batchData.m_ShadowArea / batchData.m_ShadowHeight;
            batchData.m_ShadowHeight += 0.5f;
            batchData.m_ShadowArea = num3 * batchData.m_ShadowHeight;
          }
        }
        if ((UnityEngine.Object) component6 != (UnityEngine.Object) null)
        {
          if (customProps == null)
            customProps = new MaterialPropertyBlock();
          float4 float4_3 = new float4(component6.m_MinSize, 1f / math.max(1E-05f, math.max(float4_1.x, float4_1.y)), component6.m_InfoviewFactor * 2.5f, 2.5f);
          if (component6.m_InfoviewOnly)
          {
            float4_3.x = math.max(float4_1.x, float4_1.y);
            float4_3.w = 0.0f;
          }
          customProps.SetVector(propertyData5.m_NameID, (Vector4) float4_3);
          customProps.SetFloat(propertyData3.m_NameID, RenderingUtils.CalculateDistanceFactor(lod));
          // ISSUE: reference to a compiler-generated method
          this.EnableKeyword(materialKey, "COLOSSAL_GEOMETRY_DILATED");
          flag3 = false;
          if (surfaceAsset.keywords.Contains<string>("_SURFACE_TYPE_TRANSPARENT"))
            flags &= ~BatchFlags.LodFade;
        }
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated method
          this.EnableKeyword(materialKey, "_GPU_ANIMATION_OFF");
        }
        if (flag3)
        {
          // ISSUE: reference to a compiler-generated method
          this.EnableKeyword(materialKey, "COLOSSAL_GEOMETRY_DEFAULT");
        }
        if ((UnityEngine.Object) component7 != (UnityEngine.Object) null)
        {
          if (materialKey.template.renderQueue == 3000)
            materialKey.renderQueue = 3900;
          if (customProps == null)
            customProps = new MaterialPropertyBlock();
          customProps.SetVector(propertyData1.m_NameID, (Vector4) new float4(component7.m_TextureArea.min, component7.m_TextureArea.max));
          customProps.SetVector(propertyData2.m_NameID, (Vector4) float4_1);
          customProps.SetFloat(propertyData3.m_NameID, RenderingUtils.CalculateDistanceFactor(lod));
          flags &= ~BatchFlags.SurfaceState;
        }
        else if ((UnityEngine.Object) decalProperties == (UnityEngine.Object) null && materialKey.template.renderQueue >= 2000 && materialKey.template.renderQueue <= 2500 && math.any(MathUtils.Size(bounds1) < new float3(7f, 3f, 7f)))
          materialKey.renderQueue = materialKey.template.renderQueue + 1;
        if ((UnityEngine.Object) decalProperties != (UnityEngine.Object) null || (UnityEngine.Object) component7 != (UnityEngine.Object) null)
        {
          shadowCastingMode = ShadowCastingMode.Off;
          flag1 = false;
        }
        if (meshPrefab.isImpostor)
        {
          entityManager = this.EntityManager;
          ImpostorData componentData3 = entityManager.GetComponentData<ImpostorData>(entity2);
          Vector4 vector4;
          surfaceAsset.vectors.TryGetValue("_ImpostorOffset", out vector4);
          componentData3.m_Offset = ((float4) vector4).xyz;
          surfaceAsset.floats.TryGetValue("_ImpostorSize", out componentData3.m_Size);
          entityManager = this.EntityManager;
          entityManager.SetComponentData<ImpostorData>(entity2, componentData3);
          if ((int) batchData.m_LodIndex == (int) groupData.m_LodCount)
          {
            groupData.m_SecondarySize = float3_2 * float3_2 / (componentData3.m_Size * math.cmax(float3_2));
            groupData.m_SecondaryCenter = float3_1 - componentData3.m_Offset;
          }
        }
        if ((meshPrefab.manualVTRequired || meshPrefab.isImpostor) && (UnityEngine.Object) decalProperties == (UnityEngine.Object) null && vtAtlassingInfoArray != null)
        {
          Bounds2 bounds3 = MathUtils.Bounds(new float2(0.0f, 0.0f), new float2(1f, 1f));
          // ISSUE: reference to a compiler-generated field
          this.m_VTRequestDependencies.Complete();
          if (vtAtlassingInfoArray.Length >= 1 && vtAtlassingInfoArray[0].IndexInStack >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            batchData.m_VTIndex0 = this.m_VTTextureRequester.RegisterTexture(0, vtAtlassingInfoArray[0].StackGlobalIndex, vtAtlassingInfoArray[0].IndexInStack, bounds3);
          }
          if (vtAtlassingInfoArray.Length >= 2 && vtAtlassingInfoArray[1].IndexInStack >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            batchData.m_VTIndex1 = this.m_VTTextureRequester.RegisterTexture(1, vtAtlassingInfoArray[1].StackGlobalIndex, vtAtlassingInfoArray[1].IndexInStack, bounds3);
          }
          batchData.m_VTSizeFactor = math.cmax(float3_2) * 2f;
        }
        if ((componentData1.m_State & MeshFlags.Default) != (MeshFlags) 0)
        {
          batchData.m_ShadowArea = float.PositiveInfinity;
          batchData.m_ShadowHeight = float.PositiveInfinity;
          // ISSUE: reference to a compiler-generated method
          this.DisableKeyword(materialKey, "_TANGENTSPACE_OCTO");
          // ISSUE: reference to a compiler-generated field
          materialKey.textures.Add(this.m_MaskMap, (object) Texture2D.blackTexture);
        }
        if (vtAtlassingInfoArray != null)
        {
          if ((componentData1.m_State & MeshFlags.Default) != (MeshFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.DisableKeyword(materialKey, "ENABLE_VT");
          }
          else
          {
            for (int index = 0; index < 2; ++index)
            {
              if (vtAtlassingInfoArray.Length > index && vtAtlassingInfoArray[index].IndexInStack >= 0)
              {
                if (customProps == null)
                  customProps = new MaterialPropertyBlock();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                PropertyData propertyData9 = this.m_BatchManagerSystem.GetPropertyData(index == 0 ? MaterialProperty.VTUVs0 : MaterialProperty.VTUVs1);
                // ISSUE: reference to a compiler-generated field
                customProps.SetVector(propertyData9.m_NameID, this.m_TextureStreamingSystem.GetShaderGraphUVs(vtAtlassingInfoArray[index]));
                materialKey.vtStacks.Add(vtAtlassingInfoArray[index].StackGlobalIndex);
                // ISSUE: reference to a compiler-generated method
                this.EnableKeyword(materialKey, "ENABLE_VT");
              }
              else
                materialKey.vtStacks.Add(-1);
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Mesh defaultMesh = this.m_BatchMeshSystem.GetDefaultMesh(groupData.m_MeshType, flags, generatedType);
      if ((flags & BatchFlags.Animated) != (BatchFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Materials.TryGetValue(materialKey, out defaultMaterial))
        {
          // ISSUE: reference to a compiler-generated method
          defaultMaterial = this.CreateMaterial(surfaceAsset, material, materialKey);
          // ISSUE: reference to a compiler-generated field
          this.m_Materials.Add(materialKey, defaultMaterial);
          // ISSUE: object of a compiler-generated type is created
          materialKey = new ManagedBatchSystem.MaterialKey(materialKey);
        }
        // ISSUE: reference to a compiler-generated method
        this.DisableKeyword(materialKey, "_GPU_ANIMATION_OFF");
        // ISSUE: reference to a compiler-generated method
        this.EnableKeyword(materialKey, "_GPU_ANIMATION_TEXTURE");
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_Materials.TryGetValue(materialKey, out loadedMaterial))
      {
        // ISSUE: reference to a compiler-generated method
        materialKey.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_CachedMaterialKey = materialKey;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        loadedMaterial = this.CreateMaterial(surfaceAsset, material, materialKey);
        // ISSUE: reference to a compiler-generated field
        this.m_Materials.Add(materialKey, loadedMaterial);
      }
      if (loadedMaterial.IsKeywordEnabled("_TRANSPARENT_WRITES_MOTION_VEC"))
        flags |= BatchFlags.MotionVectors;
      if ((UnityEngine.Object) defaultMaterial == (UnityEngine.Object) null)
        defaultMaterial = loadedMaterial;
      MeshLayer layer = groupData.m_Layer;
      switch (layer)
      {
        case (MeshLayer) 0:
        case MeshLayer.Default:
        case MeshLayer.Default | MeshLayer.Moving:
        case MeshLayer.Default | MeshLayer.Tunnel:
        case MeshLayer.Moving | MeshLayer.Tunnel:
        case MeshLayer.Default | MeshLayer.Moving | MeshLayer.Tunnel:
label_164:
          if ((flags & BatchFlags.MotionVectors) != (BatchFlags) 0)
            batchData.m_RenderFlags |= BatchRenderFlags.MotionVectors;
          if (flag1)
            batchData.m_RenderFlags |= BatchRenderFlags.ReceiveShadows;
          if (shadowCastingMode != ShadowCastingMode.Off)
            batchData.m_RenderFlags |= BatchRenderFlags.CastShadows;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_RenderingSystem.IsShaderEnabled(loadedMaterial.shader))
            batchData.m_RenderFlags |= BatchRenderFlags.IsEnabled;
          batchData.m_ShadowCastingMode = (byte) shadowCastingMode;
          batchData.m_Layer = (byte) num1;
          groupData.m_RenderFlags |= batchData.m_RenderFlags;
          return new CustomBatch(groupIndex, batchIndex, surfaceAsset, material, defaultMaterial, loadedMaterial, defaultMesh, entity2, sharedMesh, flags, generatedType, groupData.m_MeshType, num2, customProps);
        case MeshLayer.Moving:
          // ISSUE: reference to a compiler-generated field
          num1 = this.m_MovingLayer;
          flags |= BatchFlags.MotionVectors;
          goto case (MeshLayer) 0;
        case MeshLayer.Tunnel:
          // ISSUE: reference to a compiler-generated field
          num1 = this.m_TunnelLayer;
          goto case (MeshLayer) 0;
        case MeshLayer.Pipeline:
          // ISSUE: reference to a compiler-generated field
          num1 = this.m_PipelineLayer;
          shadowCastingMode = ShadowCastingMode.Off;
          flag1 = false;
          goto case (MeshLayer) 0;
        default:
          if ((uint) layer <= 32U)
          {
            switch (layer)
            {
              case MeshLayer.SubPipeline:
                // ISSUE: reference to a compiler-generated field
                num1 = this.m_SubPipelineLayer;
                shadowCastingMode = ShadowCastingMode.Off;
                flag1 = false;
                goto label_164;
              case MeshLayer.Waterway:
                // ISSUE: reference to a compiler-generated field
                num1 = this.m_WaterwayLayer;
                shadowCastingMode = ShadowCastingMode.Off;
                flag1 = false;
                goto label_164;
              default:
                goto label_164;
            }
          }
          else
          {
            switch (layer)
            {
              case MeshLayer.Outline:
                // ISSUE: reference to a compiler-generated field
                num1 = this.m_OutlineLayer;
                flags = flags & ~(BatchFlags.MotionVectors | BatchFlags.Emissive | BatchFlags.ColorMask | BatchFlags.InfoviewColor | BatchFlags.LodFade | BatchFlags.InfoviewFlow | BatchFlags.SurfaceState) | BatchFlags.Outline;
                shadowCastingMode = ShadowCastingMode.Off;
                flag1 = false;
                goto label_164;
              case MeshLayer.Marker:
                // ISSUE: reference to a compiler-generated field
                num1 = this.m_MarkerLayer;
                shadowCastingMode = ShadowCastingMode.Off;
                goto label_164;
              default:
                goto label_164;
            }
          }
      }
    }

    public void SetupVT(RenderPrefab meshPrefab, Material material, int materialIndex)
    {
      SurfaceAsset surfaceAsset = meshPrefab.GetSurfaceAsset(materialIndex);
      VTAtlassingInfo[] vtAtlassingInfoArray = surfaceAsset.VTAtlassingInfos ?? surfaceAsset.PreReservedAtlassingInfos;
      if (vtAtlassingInfoArray == null || meshPrefab.Has<DefaultMesh>())
        return;
      for (int stackConfigIndex = 0; stackConfigIndex < 2; ++stackConfigIndex)
      {
        if (vtAtlassingInfoArray.Length > stackConfigIndex && vtAtlassingInfoArray[stackConfigIndex].IndexInStack >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TextureStreamingSystem.BindMaterial(material, vtAtlassingInfoArray[stackConfigIndex].StackGlobalIndex, stackConfigIndex, new float4(1f, 1f, 0.0f, 0.0f));
        }
      }
    }

    private Material CreateMaterial(
      SurfaceAsset sourceSurface,
      Material sourceMaterial,
      ManagedBatchSystem.MaterialKey materialKey)
    {
      Material material1;
      if ((AssetData) sourceSurface != (IAssetData) null)
      {
        Material material2 = new Material(materialKey.template);
        material2.name = "Batch (" + sourceSurface.name + ")";
        material2.hideFlags = HideFlags.HideAndDontSave;
        material1 = material2;
        foreach (KeyValuePair<string, float> keyValuePair in (IEnumerable<KeyValuePair<string, float>>) sourceSurface.floats)
          material1.SetFloat(keyValuePair.Key, keyValuePair.Value);
        foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) sourceSurface.ints)
          material1.SetInt(keyValuePair.Key, keyValuePair.Value);
        foreach (KeyValuePair<string, Vector4> vector in (IEnumerable<KeyValuePair<string, Vector4>>) sourceSurface.vectors)
          material1.SetVector(vector.Key, vector.Value);
        foreach (KeyValuePair<string, Color> color in (IEnumerable<KeyValuePair<string, Color>>) sourceSurface.colors)
          material1.SetColor(color.Key, color.Value);
        foreach (KeyValuePair<int, object> texture in materialKey.textures)
        {
          if (texture.Value is TextureAsset textureAsset)
            material1.SetTexture(texture.Key, textureAsset.Load(-1));
          else
            material1.SetTexture(texture.Key, (Texture) texture.Value);
        }
        foreach (string keyword in (IEnumerable<string>) sourceSurface.keywords)
          material1.EnableKeyword(keyword);
        HDMaterial.ValidateMaterial(material1);
      }
      else
      {
        Material material3 = new Material(sourceMaterial);
        material3.name = "Batch (" + sourceMaterial.name + ")";
        material3.hideFlags = HideFlags.HideAndDontSave;
        material1 = material3;
        // ISSUE: reference to a compiler-generated field
        foreach (ManagedBatchSystem.TextureData cachedTexture in this.m_CachedTextures)
          material1.SetTexture(cachedTexture.nameID, cachedTexture.texture);
      }
      if (materialKey.decalLayerMask != -1)
      {
        // ISSUE: reference to a compiler-generated field
        material1.SetFloat(this.m_DecalLayerMask, math.asfloat(materialKey.decalLayerMask));
      }
      if (materialKey.renderQueue != -1)
        material1.renderQueue = materialKey.renderQueue;
      // ISSUE: reference to a compiler-generated field
      foreach (ManagedBatchSystem.KeywordData cachedKeyword in this.m_CachedKeywords)
      {
        if (cachedKeyword.remove)
          material1.DisableKeyword(cachedKeyword.name);
        else
          material1.EnableKeyword(cachedKeyword.name);
      }
      for (int index = 0; index < materialKey.vtStacks.Count; ++index)
      {
        int vtStack = materialKey.vtStacks[index];
        if (vtStack >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TextureStreamingSystem.BindMaterial(material1, vtStack, index, new float4(1f, 1f, 0.0f, 0.0f));
        }
      }
      return material1;
    }

    private void EnableKeyword(ManagedBatchSystem.MaterialKey materialKey, string keyword)
    {
      if (!materialKey.keywords.Add(keyword))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_CachedKeywords.Add(new ManagedBatchSystem.KeywordData(keyword, false));
    }

    private void DisableKeyword(ManagedBatchSystem.MaterialKey materialKey, string keyword)
    {
      if (!materialKey.keywords.Remove(keyword))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_CachedKeywords.Add(new ManagedBatchSystem.KeywordData(keyword, true));
    }

    private void SetTexture(
      ManagedBatchSystem.MaterialKey materialKey,
      int nameID,
      Texture texture)
    {
      object obj;
      if (materialKey.textures.TryGetValue(nameID, out obj))
      {
        if (texture == obj)
          return;
        materialKey.textures[nameID] = (object) texture;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_CachedTextures.Add(new ManagedBatchSystem.TextureData(nameID, texture));
      }
      else
      {
        materialKey.textures.Add(nameID, (object) texture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_CachedTextures.Add(new ManagedBatchSystem.TextureData(nameID, texture));
      }
    }

    public static Material GetTemplate(SurfaceAsset surfaceAsset)
    {
      Material template = surfaceAsset.GetTemplateMaterial();
      if ((UnityEngine.Object) template == (UnityEngine.Object) null)
        template = SurfaceAsset.kDefaultMaterial;
      return template;
    }

    [Preserve]
    public ManagedBatchSystem()
    {
    }

    private struct KeywordData
    {
      public string name { get; private set; }

      public bool remove { get; private set; }

      public KeywordData(string name, bool remove)
      {
        this.name = name;
        this.remove = remove;
      }
    }

    private struct TextureData
    {
      public int nameID { get; private set; }

      public Texture texture { get; private set; }

      public TextureData(int nameID, Texture texture)
      {
        this.nameID = nameID;
        this.texture = texture;
      }
    }

    public class MaterialKey : IEquatable<ManagedBatchSystem.MaterialKey>
    {
      public Shader shader { get; set; }

      public Material template { get; set; }

      public int decalLayerMask { get; set; }

      public int renderQueue { get; set; }

      public HashSet<string> keywords { get; private set; }

      public List<int> vtStacks { get; private set; }

      public System.Collections.Generic.Dictionary<int, object> textures { get; private set; }

      public MaterialKey()
      {
        this.decalLayerMask = -1;
        this.renderQueue = -1;
        this.keywords = new HashSet<string>();
        this.vtStacks = new List<int>();
        this.textures = new System.Collections.Generic.Dictionary<int, object>();
      }

      public MaterialKey(ManagedBatchSystem.MaterialKey source)
      {
        this.shader = source.shader;
        this.template = source.template;
        this.decalLayerMask = source.decalLayerMask;
        this.renderQueue = source.renderQueue;
        this.keywords = new HashSet<string>((IEnumerable<string>) source.keywords);
        this.vtStacks = new List<int>((IEnumerable<int>) source.vtStacks);
        this.textures = new System.Collections.Generic.Dictionary<int, object>((IDictionary<int, object>) source.textures);
      }

      public void Initialize(SurfaceAsset surface)
      {
        // ISSUE: reference to a compiler-generated method
        this.template = ManagedBatchSystem.GetTemplate(surface);
        this.renderQueue = this.template.renderQueue;
        foreach (string keyword in (IEnumerable<string>) surface.keywords)
          this.keywords.Add(keyword);
        foreach (KeyValuePair<string, TextureAsset> texture in (IEnumerable<KeyValuePair<string, TextureAsset>>) surface.textures)
        {
          if (!surface.IsHandledByVirtualTexturing(texture))
            this.textures.Add(Shader.PropertyToID(texture.Key), (object) texture.Value);
        }
      }

      public void Initialize(Material material)
      {
        this.shader = material.shader;
        this.renderQueue = material.renderQueue;
        foreach (string shaderKeyword in material.shaderKeywords)
          this.keywords.Add(shaderKeyword);
        foreach (int texturePropertyNameId in material.GetTexturePropertyNameIDs())
          this.textures.Add(texturePropertyNameId, (object) material.GetTexture(texturePropertyNameId));
      }

      public void Clear()
      {
        this.shader = (Shader) null;
        this.template = (Material) null;
        this.decalLayerMask = -1;
        this.renderQueue = -1;
        this.keywords.Clear();
        this.vtStacks.Clear();
        this.textures.Clear();
      }

      public bool Equals(ManagedBatchSystem.MaterialKey other)
      {
        if ((UnityEngine.Object) this.shader != (UnityEngine.Object) other.shader || (UnityEngine.Object) this.template != (UnityEngine.Object) other.template || this.decalLayerMask != other.decalLayerMask || this.renderQueue != other.renderQueue || this.keywords.Count != other.keywords.Count || this.vtStacks.Count != other.vtStacks.Count || this.textures.Count != other.textures.Count)
          return false;
        foreach (string keyword in this.keywords)
        {
          if (!other.keywords.Contains(keyword))
            return false;
        }
        for (int index = 0; index < this.vtStacks.Count; ++index)
        {
          if (this.vtStacks[index] != other.vtStacks[index])
            return false;
        }
        foreach (KeyValuePair<int, object> texture in this.textures)
        {
          object obj;
          if (!other.textures.TryGetValue(texture.Key, out obj) || texture.Value != obj)
            return false;
        }
        return true;
      }

      public override int GetHashCode()
      {
        int num = this.decalLayerMask;
        int hashCode1 = num.GetHashCode();
        num = this.renderQueue;
        int hashCode2 = num.GetHashCode();
        int hashCode3 = hashCode1 ^ hashCode2;
        if ((UnityEngine.Object) this.shader != (UnityEngine.Object) null)
          hashCode3 ^= this.shader.GetHashCode();
        if ((UnityEngine.Object) this.template != (UnityEngine.Object) null)
          hashCode3 ^= this.template.GetHashCode();
        foreach (string keyword in this.keywords)
          hashCode3 ^= keyword.GetHashCode();
        int count = this.vtStacks.Count;
        foreach (int vtStack in this.vtStacks)
          hashCode3 ^= vtStack.GetHashCode() + count;
        foreach (KeyValuePair<int, object> texture in this.textures)
          hashCode3 ^= texture.Value != null ? texture.Value.GetHashCode() : texture.Key.GetHashCode();
        return hashCode3;
      }
    }

    private class GroupKey : IEquatable<ManagedBatchSystem.GroupKey>
    {
      public Entity mesh { get; set; }

      public ushort partition { get; set; }

      public MeshLayer layer { get; set; }

      public MeshType type { get; set; }

      public List<ManagedBatchSystem.GroupKey.Batch> batches { get; private set; }

      public GroupKey()
      {
        this.mesh = Entity.Null;
        this.partition = (ushort) 0;
        this.layer = (MeshLayer) 0;
        this.type = (MeshType) 0;
        this.batches = new List<ManagedBatchSystem.GroupKey.Batch>();
      }

      public void Initialize(Entity sharedMesh, GroupData groupData)
      {
        this.mesh = sharedMesh;
        this.partition = groupData.m_Partition;
        this.layer = groupData.m_Layer;
        this.type = groupData.m_MeshType;
      }

      public void Clear()
      {
        this.mesh = Entity.Null;
        this.partition = (ushort) 0;
        this.layer = (MeshLayer) 0;
        this.type = (MeshType) 0;
        this.batches.Clear();
      }

      public bool Equals(ManagedBatchSystem.GroupKey other)
      {
        if (this.mesh != other.mesh || (int) this.partition != (int) other.partition || this.layer != other.layer || this.type != other.type || this.batches.Count != other.batches.Count)
          return false;
        for (int index = 0; index < this.batches.Count; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          ManagedBatchSystem.GroupKey.Batch batch = this.batches[index];
          // ISSUE: reference to a compiler-generated method
          if (!batch.Equals(other.batches[index]))
            return false;
        }
        return true;
      }

      public override int GetHashCode()
      {
        int hashCode1 = this.mesh.GetHashCode() ^ this.partition.GetHashCode() ^ this.layer.GetHashCode() ^ this.type.GetHashCode();
        for (int index = 0; index < this.batches.Count; ++index)
        {
          int num = hashCode1;
          // ISSUE: variable of a compiler-generated type
          ManagedBatchSystem.GroupKey.Batch batch = this.batches[index];
          int hashCode2 = batch.GetHashCode();
          hashCode1 = num ^ hashCode2;
        }
        return hashCode1;
      }

      public struct Batch : IEquatable<ManagedBatchSystem.GroupKey.Batch>
      {
        public Material loadedMaterial { get; set; }

        public BatchFlags flags { get; set; }

        public Batch(CustomBatch batch)
        {
          this.loadedMaterial = batch.loadedMaterial;
          this.flags = batch.sourceFlags;
        }

        public bool Equals(ManagedBatchSystem.GroupKey.Batch other)
        {
          return (UnityEngine.Object) this.loadedMaterial == (UnityEngine.Object) other.loadedMaterial && this.flags == other.flags;
        }

        public override int GetHashCode()
        {
          return this.loadedMaterial.GetHashCode() ^ this.flags.GetHashCode();
        }
      }
    }

    private struct MeshKey : IEquatable<ManagedBatchSystem.MeshKey>
    {
      public Colossal.Hash128 meshGuid { get; set; }

      public MeshFlags flags { get; set; }

      public MeshKey(RenderPrefab meshPrefab, Game.Prefabs.MeshData meshData)
      {
        AssetData geometryAsset;
        this.meshGuid = !((geometryAsset = (AssetData) meshPrefab.geometryAsset) != (IAssetData) null) ? new Colossal.Hash128() : geometryAsset.guid;
        this.flags = meshData.m_State & MeshFlags.Base;
      }

      public bool Equals(ManagedBatchSystem.MeshKey other)
      {
        return this.meshGuid == other.meshGuid && this.flags == other.flags;
      }

      public override int GetHashCode() => this.meshGuid.GetHashCode() ^ this.flags.GetHashCode();
    }
  }
}
