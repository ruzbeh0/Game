// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchManagerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.IO.AssetDatabase;
using Colossal.IO.AssetDatabase.VirtualTexturing;
using Colossal.Mathematics;
using Colossal.Rendering;
using Game.Prefabs;
using Game.Serialization;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class BatchManagerSystem : GameSystemBase, IPreDeserialize
  {
    public const uint GPU_INSTANCE_MEMORY_DEFAULT = 67108864;
    public const uint GPU_INSTANCE_MEMORY_INCREMENT = 16777216;
    public const uint GPU_UPLOADER_CHUNK_SIZE = 2097152;
    public const uint GPU_UPLOADER_OPERATION_SIZE = 65536;
    public const int MAX_GROUP_BATCH_COUNT = 16;
    private RenderingSystem m_RenderingSystem;
    private ManagedBatchSystem m_ManagedBatchSystem;
    private BatchDataSystem m_BatchDataSystem;
    private TextureStreamingSystem m_TextureStreamingSystem;
    private PrefabSystem m_PrefabSystem;
    private NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
    private NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchInstances;
    private NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> m_NativeSubBatches;
    private ManagedBatches<OptionalProperties> m_ManagedBatches;
    private NativeList<PropertyData> m_MaterialProperties;
    private NativeList<PropertyData> m_ObjectProperties;
    private NativeList<PropertyData> m_NetProperties;
    private NativeList<PropertyData> m_LaneProperties;
    private NativeList<PropertyData> m_ZoneProperties;
    private NativeList<Entity> m_MergeMeshes;
    private NativeParallelMultiHashMap<Entity, int> m_MergeGroups;
    private EntityQuery m_MeshSettingsQuery;
    private bool m_LastMotionVectorsEnabled;
    private bool m_LastLodFadeEnabled;
    private bool m_PropertiesChanged;
    private bool m_MotionVectorsChanged;
    private bool m_LodFadeChanged;
    private bool m_VirtualTexturingChanged;
    private JobHandle m_NativeBatchGroupsReadDependencies;
    private JobHandle m_NativeBatchGroupsWriteDependencies;
    private JobHandle m_NativeBatchInstancesReadDependencies;
    private JobHandle m_NativeBatchInstancesWriteDependencies;
    private JobHandle m_NativeSubBatchesReadDependencies;
    private JobHandle m_NativeSubBatchesWriteDependencies;
    private JobHandle m_MergeDependencies;
    private BatchManagerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatchSystem = this.World.GetOrCreateSystemManaged<ManagedBatchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchDataSystem = this.World.GetOrCreateSystemManaged<BatchDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TextureStreamingSystem = this.World.GetOrCreateSystemManaged<TextureStreamingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroups = new NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData>(67108864U, 65536U, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstances = new NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>(this.m_NativeBatchGroups);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatches = new NativeSubBatches<CullingData, GroupData, BatchData, InstanceData>(this.m_NativeBatchGroups);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatches = ManagedBatches<OptionalProperties>.Create<CullingData, GroupData, BatchData, InstanceData>(this.m_NativeBatchInstances, (BatchRendererGroup.OnPerformCulling) ((rendererGroup, cullingContext, cullingOutput, userContext) =>
      {
        JobHandle dependencies1;
        // ISSUE: reference to a compiler-generated method
        NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.GetNativeBatchGroups(true, out dependencies1);
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated method
        NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances = this.GetNativeBatchInstances(true, out dependencies2);
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated method
        NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> nativeSubBatches = this.GetNativeSubBatches(true, out dependencies3);
        dependencies2.Complete();
        int activeGroupCount = nativeBatchInstances.GetActiveGroupCount();
        int num = (cullingContext.cullingSplits.Length << 1) - 1;
        NativeArray<BatchManagerSystem.ActiveGroupData> nativeArray = new NativeArray<BatchManagerSystem.ActiveGroupData>(activeGroupCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        NativeList<BatchManagerSystem.CullingSplitData> nativeList1 = new NativeList<BatchManagerSystem.CullingSplitData>(cullingContext.cullingSplits.Length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<FrustumPlanes.PlanePacket4> nativeList2 = new NativeList<FrustumPlanes.PlanePacket4>(FrustumPlanes.GetPacketCount(cullingContext.cullingPlanes.Length), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        BatchRenderFlags batchRenderFlags1 = BatchRenderFlags.IsEnabled;
        BatchRenderFlags batchRenderFlags2 = BatchRenderFlags.All;
        if (cullingContext.viewType == BatchCullingViewType.Light)
          batchRenderFlags1 |= BatchRenderFlags.CastShadows;
        // ISSUE: reference to a compiler-generated method
        if (!this.IsMotionVectorsEnabled())
          batchRenderFlags2 &= ~BatchRenderFlags.MotionVectors;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        BatchManagerSystem.AllocateCullingJob jobData1 = new BatchManagerSystem.AllocateCullingJob()
        {
          m_NativeBatchGroups = nativeBatchGroups,
          m_NativeBatchInstances = nativeBatchInstances,
          m_RequiredFlagMask = batchRenderFlags1,
          m_MaxSplitBatchCount = num,
          m_CullingOutput = cullingOutput,
          m_ActiveGroupData = nativeArray
        };
        bool flag = cullingContext.projectionType == BatchCullingProjectionType.Orthographic && cullingContext.viewType == BatchCullingViewType.Light && cullingContext.cullingSplits.Length == 4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        BatchManagerSystem.CullingPlanesJob jobData2 = new BatchManagerSystem.CullingPlanesJob()
        {
          m_CullingPlanes = cullingContext.cullingPlanes,
          m_CullingSplits = cullingContext.cullingSplits,
          m_ShadowCullingData = flag ? this.m_RenderingSystem.GetShadowCullingData() : float3.zero,
          m_SplitData = nativeList1,
          m_PlanePackets = nativeList2
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        BatchManagerSystem.BatchCullingJob jobData3 = new BatchManagerSystem.BatchCullingJob()
        {
          m_NativeBatchGroups = nativeBatchGroups,
          m_NativeBatchInstances = nativeBatchInstances,
          m_NativeSubBatches = nativeSubBatches,
          m_RequiredFlagMask = batchRenderFlags1,
          m_RenderFlagMask = batchRenderFlags2,
          m_MaxSplitBatchCount = num,
          m_IsShadowCulling = cullingContext.viewType == BatchCullingViewType.Light,
          m_ActiveGroupData = nativeArray,
          m_SplitData = nativeList1,
          m_CullingPlanePackets = nativeList2,
          m_CullingOutput = cullingOutput
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        BatchManagerSystem.FinalizeCullingJob jobData4 = new BatchManagerSystem.FinalizeCullingJob()
        {
          m_CullingOutput = cullingOutput
        };
        JobHandle job0 = jobData1.Schedule<BatchManagerSystem.AllocateCullingJob>(dependencies1);
        JobHandle job1 = jobData2.Schedule<BatchManagerSystem.CullingPlanesJob>();
        int arrayLength = activeGroupCount;
        JobHandle dependsOn = JobHandle.CombineDependencies(job0, job1, dependencies3);
        JobHandle jobHandle1 = jobData3.Schedule<BatchManagerSystem.BatchCullingJob>(arrayLength, 1, dependsOn);
        JobHandle jobHandle2 = jobData4.Schedule<BatchManagerSystem.FinalizeCullingJob>(jobHandle1);
        nativeList1.Dispose(jobHandle1);
        nativeList2.Dispose(jobHandle1);
        // ISSUE: reference to a compiler-generated method
        this.AddNativeBatchInstancesReader(jobHandle1);
        // ISSUE: reference to a compiler-generated method
        this.AddNativeBatchGroupsReader(jobHandle1);
        // ISSUE: reference to a compiler-generated method
        this.AddNativeSubBatchesReader(jobHandle1);
        return jobHandle2;
      }), 2097152U, new OptionalProperties(BatchFlags.MotionVectors, MeshType.Object));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.InitializeMaterialProperties<MaterialProperty>(out this.m_MaterialProperties);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.InitializeInstanceProperties<ObjectProperty>(out this.m_ObjectProperties, MeshType.Object);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.InitializeInstanceProperties<NetProperty>(out this.m_NetProperties, MeshType.Net);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.InitializeInstanceProperties<LaneProperty>(out this.m_LaneProperties, MeshType.Lane);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.InitializeInstanceProperties<ZoneProperty>(out this.m_ZoneProperties, MeshType.Zone);
      // ISSUE: reference to a compiler-generated field
      this.m_MergeMeshes = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MergeGroups = new NativeParallelMultiHashMap<Entity, int>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MeshSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<MeshSettingsData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastMotionVectorsEnabled = this.m_RenderingSystem.motionVectors;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastLodFadeEnabled = this.m_RenderingSystem.lodCrossFade;
    }

    private void InitializeMaterialProperties<T>(out NativeList<PropertyData> properties)
    {
      System.Array values = Enum.GetValues(typeof (T));
      properties = new NativeList<PropertyData>(values.Length, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      foreach (T obj in values)
      {
        object[] customAttributes = typeof (T).GetMember(obj.ToString())[0].GetCustomAttributes(typeof (MaterialPropertyAttribute), false);
        if (customAttributes.Length != 0)
        {
          MaterialPropertyAttribute propertyAttribute = (MaterialPropertyAttribute) customAttributes[0];
          properties.Add(new PropertyData()
          {
            m_NameID = Shader.PropertyToID(propertyAttribute.ShaderPropertyName)
          });
          // ISSUE: reference to a compiler-generated field
          this.m_ManagedBatches.RegisterMaterialPropertyType(propertyAttribute.ShaderPropertyName, propertyAttribute.DataType, false, isBuiltin: propertyAttribute.IsBuiltin);
        }
      }
    }

    private void InitializeInstanceProperties<T>(
      out NativeList<PropertyData> properties,
      MeshType meshType)
    {
      System.Array values = Enum.GetValues(typeof (T));
      properties = new NativeList<PropertyData>(values.Length, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      foreach (T obj in values)
      {
        object[] customAttributes = typeof (T).GetMember(obj.ToString())[0].GetCustomAttributes(typeof (InstancePropertyAttribute), false);
        if (customAttributes.Length != 0)
        {
          InstancePropertyAttribute propertyAttribute = (InstancePropertyAttribute) customAttributes[0];
          properties.Add(new PropertyData()
          {
            m_NameID = Shader.PropertyToID(propertyAttribute.ShaderPropertyName),
            m_DataIndex = propertyAttribute.DataIndex
          });
          // ISSUE: reference to a compiler-generated field
          this.m_ManagedBatches.RegisterMaterialPropertyType(propertyAttribute.ShaderPropertyName, propertyAttribute.DataType, true, isBuiltin: propertyAttribute.IsBuiltin, overrideRequirements: new OptionalProperties(propertyAttribute.RequiredFlags, meshType), propertyRequirements: new OptionalProperties((BatchFlags) 0, meshType));
        }
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroupsReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroupsWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstancesReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstancesWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatchesReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatchesWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_MergeDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatches.EndUpload<CullingData, GroupData, BatchData, InstanceData>(this.m_NativeBatchInstances);
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatches.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatches.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstances.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroups.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialProperties.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectProperties.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NetProperties.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneProperties.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneProperties.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MergeMeshes.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MergeGroups.Dispose();
      base.OnDestroy();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroupsReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroupsWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstancesReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstancesWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatchesReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatchesWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatches.EndUpload<CullingData, GroupData, BatchData, InstanceData>(this.m_NativeBatchInstances);
      // ISSUE: reference to a compiler-generated field
      int groupCount = this.m_NativeBatchGroups.GetGroupCount();
      for (int groupIndex = 0; groupIndex < groupCount; ++groupIndex)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NativeBatchGroups.IsValidGroup(groupIndex))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchInstances.RemoveInstances(groupIndex, this.m_NativeSubBatches);
        }
      }
    }

    public bool CheckPropertyUpdates()
    {
      // ISSUE: reference to a compiler-generated field
      bool motionVectors = this.m_RenderingSystem.motionVectors;
      // ISSUE: reference to a compiler-generated field
      if (motionVectors != this.m_LastMotionVectorsEnabled)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastMotionVectorsEnabled = motionVectors;
        // ISSUE: reference to a compiler-generated field
        this.m_MotionVectorsChanged = true;
      }
      // ISSUE: reference to a compiler-generated field
      bool lodCrossFade = this.m_RenderingSystem.lodCrossFade;
      // ISSUE: reference to a compiler-generated field
      if (lodCrossFade != this.m_LastLodFadeEnabled)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastLodFadeEnabled = lodCrossFade;
        // ISSUE: reference to a compiler-generated field
        this.m_LodFadeChanged = true;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_PropertiesChanged || this.m_MotionVectorsChanged || this.m_LodFadeChanged || this.m_VirtualTexturingChanged;
    }

    public void VirtualTexturingUpdated() => this.m_VirtualTexturingChanged = true;

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_PropertiesChanged || this.m_MotionVectorsChanged || this.m_LodFadeChanged || this.m_VirtualTexturingChanged)
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RefreshProperties(this.m_PropertiesChanged, this.m_MotionVectorsChanged, this.m_LodFadeChanged, this.m_VirtualTexturingChanged);
        }
        finally
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PropertiesChanged = false;
          // ISSUE: reference to a compiler-generated field
          this.m_MotionVectorsChanged = false;
          // ISSUE: reference to a compiler-generated field
          this.m_LodFadeChanged = false;
          // ISSUE: reference to a compiler-generated field
          this.m_VirtualTexturingChanged = false;
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_MergeDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      if (this.m_MergeMeshes.Length != 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.MergeGroups();
      }
      JobHandle dependencies1;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.AllocateBuffersJob jobData1 = new BatchManagerSystem.AllocateBuffersJob()
      {
        m_ObjectProperties = this.m_ObjectProperties,
        m_NetProperties = this.m_NetProperties,
        m_LaneProperties = this.m_LaneProperties,
        m_ZoneProperties = this.m_ZoneProperties,
        m_NativeBatchGroups = this.GetNativeBatchGroups(false, out dependencies1),
        m_NativeBatchInstances = this.GetNativeBatchInstances(false, out dependencies2)
      };
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.GenerateSubBatchesJob jobData2 = new BatchManagerSystem.GenerateSubBatchesJob()
      {
        m_NativeSubBatches = this.GetNativeSubBatches(false, out dependencies3)
      };
      JobHandle jobHandle1 = jobData1.Schedule<BatchManagerSystem.AllocateBuffersJob>(JobHandle.CombineDependencies(dependencies1, dependencies2));
      JobHandle dependsOn = dependencies3;
      JobHandle jobHandle2 = jobData2.Schedule<BatchManagerSystem.GenerateSubBatchesJob>(dependsOn);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeBatchGroupsWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeBatchInstancesWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeSubBatchesWriter(jobHandle2);
    }

    public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> GetNativeBatchGroups(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_NativeBatchGroupsWriteDependencies : JobHandle.CombineDependencies(this.m_NativeBatchGroupsReadDependencies, this.m_NativeBatchGroupsWriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_NativeBatchGroups;
    }

    public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> GetNativeBatchInstances(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_NativeBatchInstancesWriteDependencies : JobHandle.CombineDependencies(this.m_NativeBatchInstancesReadDependencies, this.m_NativeBatchInstancesWriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_NativeBatchInstances;
    }

    public NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> GetNativeSubBatches(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_NativeSubBatchesWriteDependencies : JobHandle.CombineDependencies(this.m_NativeSubBatchesReadDependencies, this.m_NativeSubBatchesWriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_NativeSubBatches;
    }

    public ManagedBatches<OptionalProperties> GetManagedBatches() => this.m_ManagedBatches;

    public bool IsMotionVectorsEnabled() => this.m_LastMotionVectorsEnabled;

    public bool IsLodFadeEnabled() => this.m_LastLodFadeEnabled;

    public void AddNativeBatchGroupsReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroupsReadDependencies = JobHandle.CombineDependencies(this.m_NativeBatchGroupsReadDependencies, jobHandle);
    }

    public void AddNativeBatchGroupsWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchGroupsWriteDependencies = jobHandle;
    }

    public void AddNativeBatchInstancesReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstancesReadDependencies = JobHandle.CombineDependencies(this.m_NativeBatchInstancesReadDependencies, jobHandle);
    }

    public void AddNativeBatchInstancesWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchInstancesWriteDependencies = jobHandle;
    }

    public void AddNativeSubBatchesReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatchesReadDependencies = JobHandle.CombineDependencies(this.m_NativeSubBatchesReadDependencies, jobHandle);
    }

    public void AddNativeSubBatchesWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NativeSubBatchesWriteDependencies = jobHandle;
    }

    public void MergeGroups(Entity meshEntity, int mergeIndex)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MergeDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_MergeGroups.ContainsKey(meshEntity))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MergeMeshes.Add(in meshEntity);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_MergeGroups.Add(meshEntity, mergeIndex);
    }

    public PropertyData GetPropertyData(MaterialProperty property)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_MaterialProperties[(int) property];
    }

    public PropertyData GetPropertyData(ObjectProperty property)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_ObjectProperties[(int) property];
    }

    public PropertyData GetPropertyData(NetProperty property)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_NetProperties[(int) property];
    }

    public PropertyData GetPropertyData(LaneProperty property)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_LaneProperties[(int) property];
    }

    public PropertyData GetPropertyData(ZoneProperty property)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_ZoneProperties[(int) property];
    }

    private void RefreshProperties(
      bool propertiesChanged,
      bool motionVectorsChanged,
      bool lodFadeChanged,
      bool virtualTexturingChanged)
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.GetNativeBatchGroups(false, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances = this.GetNativeBatchInstances(false, out dependencies2);
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated method
      NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> nativeSubBatches = this.GetNativeSubBatches(false, out dependencies3);
      dependencies1.Complete();
      dependencies2.Complete();
      dependencies3.Complete();
      int groupCount = nativeBatchGroups.GetGroupCount();
      bool flag1 = false;
      System.Collections.Generic.Dictionary<BatchPropertiesKey<OptionalProperties>, bool> dictionary = (System.Collections.Generic.Dictionary<BatchPropertiesKey<OptionalProperties>, bool>) null;
      if (propertiesChanged)
        dictionary = new System.Collections.Generic.Dictionary<BatchPropertiesKey<OptionalProperties>, bool>();
      MeshSettingsData meshSettingsData = new MeshSettingsData();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_MeshSettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        meshSettingsData = this.m_MeshSettingsQuery.GetSingleton<MeshSettingsData>();
      }
      for (int groupIndex = 0; groupIndex < groupCount; ++groupIndex)
      {
        if (nativeBatchGroups.IsValidGroup(groupIndex))
        {
          int batchCount = nativeBatchGroups.GetBatchCount(groupIndex);
          GroupData groupData = nativeBatchGroups.GetGroupData(groupIndex);
          for (int batchIndex = 0; batchIndex < batchCount; ++batchIndex)
          {
            int managedBatchIndex = nativeBatchGroups.GetManagedBatchIndex(groupIndex, batchIndex);
            if (managedBatchIndex >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              CustomBatch batch = (CustomBatch) this.m_ManagedBatches.GetBatch(managedBatchIndex);
              BatchFlags sourceFlags = batch.sourceFlags;
              // ISSUE: reference to a compiler-generated method
              if (!this.IsMotionVectorsEnabled())
                sourceFlags &= ~BatchFlags.MotionVectors;
              // ISSUE: reference to a compiler-generated method
              if (!this.IsLodFadeEnabled())
                sourceFlags &= ~BatchFlags.LodFade;
              OptionalProperties optionalProperties = new OptionalProperties(sourceFlags, batch.sourceType);
              bool flag2 = (batch.sourceFlags & BatchFlags.MotionVectors) != 0 & motionVectorsChanged || (batch.sourceFlags & BatchFlags.LodFade) != 0 & lodFadeChanged;
              if ((batch.sourceType & (MeshType.Net | MeshType.Zone)) == (MeshType) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                RenderPrefab meshPrefab = this.m_PrefabSystem.GetPrefab<RenderPrefab>(batch.sourceMeshEntity);
                if (virtualTexturingChanged)
                {
                  DecalProperties decalProperties = meshPrefab.GetComponent<DecalProperties>();
                  if ((UnityEngine.Object) decalProperties != (UnityEngine.Object) null && groupData.m_Layer == MeshLayer.Outline)
                    decalProperties = (DecalProperties) null;
                  VTAtlassingInfo[] vtAtlassingInfoArray = batch.sourceSurface.VTAtlassingInfos ?? batch.sourceSurface.PreReservedAtlassingInfos;
                  if (vtAtlassingInfoArray != null)
                  {
                    if ((UnityEngine.Object) decalProperties != (UnityEngine.Object) null || meshPrefab.manualVTRequired || meshPrefab.isImpostor)
                    {
                      BatchData batchData = nativeBatchGroups.GetBatchData(groupIndex, batchIndex);
                      Bounds2 bounds = MathUtils.Bounds(new float2(0.0f, 0.0f), new float2(1f, 1f));
                      batchData.m_VTIndex0 = -1;
                      batchData.m_VTIndex1 = -1;
                      if ((UnityEngine.Object) decalProperties != (UnityEngine.Object) null)
                        bounds = MathUtils.Bounds(decalProperties.m_TextureArea.min, decalProperties.m_TextureArea.max);
                      if (vtAtlassingInfoArray.Length >= 1 && vtAtlassingInfoArray[0].IndexInStack >= 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        batchData.m_VTIndex0 = this.m_ManagedBatchSystem.VTTextureRequester.RegisterTexture(0, vtAtlassingInfoArray[0].StackGlobalIndex, vtAtlassingInfoArray[0].IndexInStack, bounds);
                      }
                      if (vtAtlassingInfoArray.Length >= 2 && vtAtlassingInfoArray[1].IndexInStack >= 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        batchData.m_VTIndex1 = this.m_ManagedBatchSystem.VTTextureRequester.RegisterTexture(1, vtAtlassingInfoArray[1].StackGlobalIndex, vtAtlassingInfoArray[1].IndexInStack, bounds);
                      }
                      nativeBatchGroups.SetBatchData(groupIndex, batchIndex, batchData);
                    }
                    if (!meshPrefab.Has<DefaultMesh>())
                    {
                      for (int index = 0; index < 2; ++index)
                      {
                        if (vtAtlassingInfoArray.Length > index && vtAtlassingInfoArray[index].IndexInStack >= 0)
                        {
                          // ISSUE: reference to a compiler-generated method
                          PropertyData propertyData = this.GetPropertyData(index == 0 ? MaterialProperty.VTUVs0 : MaterialProperty.VTUVs1);
                          // ISSUE: reference to a compiler-generated field
                          batch.customProps.SetVector(propertyData.m_NameID, this.m_TextureStreamingSystem.GetShaderGraphUVs(vtAtlassingInfoArray[index]));
                          flag2 = true;
                        }
                      }
                    }
                  }
                }
                if (batch.generatedType == GeneratedType.ObjectBase)
                {
                  BaseProperties component = meshPrefab.GetComponent<BaseProperties>();
                  if ((UnityEngine.Object) component == (UnityEngine.Object) null && (batch.sourceFlags & BatchFlags.Lod) != (BatchFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    component = this.m_PrefabSystem.GetPrefab<RenderPrefab>(groupData.m_Mesh).GetComponent<BaseProperties>();
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  meshPrefab = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? this.m_PrefabSystem.GetPrefab<RenderPrefab>(meshSettingsData.m_DefaultBaseMesh) : component.m_BaseType;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_ManagedBatchSystem.SetupVT(meshPrefab, batch.material, batch.sourceSubMeshIndex);
              }
              if (propertiesChanged)
              {
                BatchPropertiesKey<OptionalProperties> key = new BatchPropertiesKey<OptionalProperties>(batch.material.shader, optionalProperties);
                bool flag3;
                if (!dictionary.TryGetValue(key, out flag3))
                {
                  // ISSUE: reference to a compiler-generated field
                  flag3 = this.m_ManagedBatches.RegenerateBatchProperties(batch.material.shader, optionalProperties);
                  dictionary.Add(key, flag3);
                }
                flag2 |= flag3;
              }
              if (flag2)
              {
                // ISSUE: reference to a compiler-generated field
                NativeBatchProperties batchProperties = this.m_ManagedBatches.GetBatchProperties(batch.material.shader, optionalProperties);
                nativeBatchGroups.SetBatchProperties(groupIndex, batchIndex, batchProperties);
                nativeSubBatches.RecreateRenderers(groupIndex, batchIndex);
                WriteableBatchDefaultsAccessor defaultsAccessor = nativeBatchGroups.GetBatchDefaultsAccessor(groupIndex, batchIndex);
                if ((AssetData) batch.sourceSurface != (IAssetData) null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.m_ManagedBatches.SetDefaults(ManagedBatchSystem.GetTemplate(batch.sourceSurface), batch.sourceSurface.floats, batch.sourceSurface.ints, batch.sourceSurface.vectors, batch.sourceSurface.colors, batch.customProps, batchProperties, defaultsAccessor);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ManagedBatches.SetDefaults(batch.sourceMaterial, batch.customProps, batchProperties, defaultsAccessor);
                }
                flag1 |= nativeBatchInstances.GetInstanceCount(groupIndex) != 0;
              }
            }
          }
        }
      }
      if (!flag1)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchDataSystem.InstancePropertiesUpdated();
      // ISSUE: reference to a compiler-generated method
      if (!this.IsLodFadeEnabled())
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddNativeBatchInstancesWriter(new BatchManagerSystem.InitializeLodFadeJob()
      {
        m_NativeBatchInstances = nativeBatchInstances.AsParallelInstanceWriter()
      }.Schedule<BatchManagerSystem.InitializeLodFadeJob>(nativeBatchInstances.GetActiveGroupCount(), 1));
    }

    private void MergeGroups()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.GetNativeBatchGroups(false, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances = this.GetNativeBatchInstances(false, out dependencies2);
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated method
      NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> nativeSubBatches = this.GetNativeSubBatches(false, out dependencies3);
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData>.GroupUpdater groupUpdater = nativeBatchGroups.BeginGroupUpdate(Allocator.TempJob);
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.InstanceUpdater instanceUpdater = nativeBatchInstances.BeginInstanceUpdate(Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BatchGroup_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.MergeGroupsJob jobData1 = new BatchManagerSystem.MergeGroupsJob()
      {
        m_MeshBatches = this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferLookup,
        m_BatchGroups = this.__TypeHandle.__Game_Prefabs_BatchGroup_RW_BufferLookup,
        m_MergeMeshes = this.m_MergeMeshes,
        m_MergeGroups = this.m_MergeGroups,
        m_BatchGroupUpdater = groupUpdater.AsParallel(int.MaxValue),
        m_BatchInstanceUpdater = instanceUpdater.AsParallel(int.MaxValue)
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.MergeCleanupJob jobData2 = new BatchManagerSystem.MergeCleanupJob()
      {
        m_MergeMeshes = this.m_MergeMeshes,
        m_MergeGroups = this.m_MergeGroups
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.Schedule<BatchManagerSystem.MergeGroupsJob>(this.m_MergeMeshes.Length, 1, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle2 = jobData2.Schedule<BatchManagerSystem.MergeCleanupJob>(dependsOn);
      JobHandle jobHandle3 = nativeBatchGroups.EndGroupUpdate(groupUpdater, jobHandle1);
      JobHandle jobHandle4 = nativeBatchInstances.EndInstanceUpdate(instanceUpdater, JobHandle.CombineDependencies(jobHandle1, dependencies3), nativeSubBatches);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeBatchGroupsWriter(jobHandle3);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeBatchInstancesWriter(jobHandle4);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeSubBatchesWriter(jobHandle4);
      this.Dependency = jobHandle1;
      // ISSUE: reference to a compiler-generated field
      this.m_MergeDependencies = jobHandle2;
    }

    private JobHandle OnPerformCulling(
      BatchRendererGroup rendererGroup,
      BatchCullingContext cullingContext,
      BatchCullingOutput cullingOutput,
      IntPtr userContext)
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.GetNativeBatchGroups(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances = this.GetNativeBatchInstances(true, out dependencies2);
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated method
      NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> nativeSubBatches = this.GetNativeSubBatches(true, out dependencies3);
      dependencies2.Complete();
      int activeGroupCount = nativeBatchInstances.GetActiveGroupCount();
      int num = (cullingContext.cullingSplits.Length << 1) - 1;
      NativeArray<BatchManagerSystem.ActiveGroupData> nativeArray = new NativeArray<BatchManagerSystem.ActiveGroupData>(activeGroupCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
      NativeList<BatchManagerSystem.CullingSplitData> nativeList1 = new NativeList<BatchManagerSystem.CullingSplitData>(cullingContext.cullingSplits.Length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<FrustumPlanes.PlanePacket4> nativeList2 = new NativeList<FrustumPlanes.PlanePacket4>(FrustumPlanes.GetPacketCount(cullingContext.cullingPlanes.Length), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      BatchRenderFlags batchRenderFlags1 = BatchRenderFlags.IsEnabled;
      BatchRenderFlags batchRenderFlags2 = BatchRenderFlags.All;
      if (cullingContext.viewType == BatchCullingViewType.Light)
        batchRenderFlags1 |= BatchRenderFlags.CastShadows;
      // ISSUE: reference to a compiler-generated method
      if (!this.IsMotionVectorsEnabled())
        batchRenderFlags2 &= ~BatchRenderFlags.MotionVectors;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.AllocateCullingJob jobData1 = new BatchManagerSystem.AllocateCullingJob()
      {
        m_NativeBatchGroups = nativeBatchGroups,
        m_NativeBatchInstances = nativeBatchInstances,
        m_RequiredFlagMask = batchRenderFlags1,
        m_MaxSplitBatchCount = num,
        m_CullingOutput = cullingOutput,
        m_ActiveGroupData = nativeArray
      };
      bool flag = cullingContext.projectionType == BatchCullingProjectionType.Orthographic && cullingContext.viewType == BatchCullingViewType.Light && cullingContext.cullingSplits.Length == 4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.CullingPlanesJob jobData2 = new BatchManagerSystem.CullingPlanesJob()
      {
        m_CullingPlanes = cullingContext.cullingPlanes,
        m_CullingSplits = cullingContext.cullingSplits,
        m_ShadowCullingData = flag ? this.m_RenderingSystem.GetShadowCullingData() : float3.zero,
        m_SplitData = nativeList1,
        m_PlanePackets = nativeList2
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.BatchCullingJob jobData3 = new BatchManagerSystem.BatchCullingJob()
      {
        m_NativeBatchGroups = nativeBatchGroups,
        m_NativeBatchInstances = nativeBatchInstances,
        m_NativeSubBatches = nativeSubBatches,
        m_RequiredFlagMask = batchRenderFlags1,
        m_RenderFlagMask = batchRenderFlags2,
        m_MaxSplitBatchCount = num,
        m_IsShadowCulling = cullingContext.viewType == BatchCullingViewType.Light,
        m_ActiveGroupData = nativeArray,
        m_SplitData = nativeList1,
        m_CullingPlanePackets = nativeList2,
        m_CullingOutput = cullingOutput
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchManagerSystem.FinalizeCullingJob jobData4 = new BatchManagerSystem.FinalizeCullingJob()
      {
        m_CullingOutput = cullingOutput
      };
      JobHandle job0 = jobData1.Schedule<BatchManagerSystem.AllocateCullingJob>(dependencies1);
      JobHandle job1 = jobData2.Schedule<BatchManagerSystem.CullingPlanesJob>();
      int arrayLength = activeGroupCount;
      JobHandle dependsOn = JobHandle.CombineDependencies(job0, job1, dependencies3);
      JobHandle jobHandle1 = jobData3.Schedule<BatchManagerSystem.BatchCullingJob>(arrayLength, 1, dependsOn);
      JobHandle jobHandle2 = jobData4.Schedule<BatchManagerSystem.FinalizeCullingJob>(jobHandle1);
      nativeList1.Dispose(jobHandle1);
      nativeList2.Dispose(jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeBatchInstancesReader(jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeBatchGroupsReader(jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.AddNativeSubBatchesReader(jobHandle1);
      return jobHandle2;
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
    public BatchManagerSystem()
    {
    }

    [BurstCompile]
    private struct MergeGroupsJob : IJobParallelFor
    {
      [NativeDisableParallelForRestriction]
      public BufferLookup<MeshBatch> m_MeshBatches;
      [NativeDisableParallelForRestriction]
      public BufferLookup<BatchGroup> m_BatchGroups;
      [ReadOnly]
      public NativeList<Entity> m_MergeMeshes;
      [ReadOnly]
      public NativeParallelMultiHashMap<Entity, int> m_MergeGroups;
      [NativeDisableParallelForRestriction]
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData>.ParallelGroupUpdater m_BatchGroupUpdater;
      [NativeDisableParallelForRestriction]
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.ParallelInstanceUpdater m_BatchInstanceUpdater;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity mergeMesh = this.m_MergeMeshes[index];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BatchGroup> batchGroup1 = this.m_BatchGroups[mergeMesh];
        int num;
        NativeParallelMultiHashMapIterator<Entity> it;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MergeGroups.TryGetFirstValue(mergeMesh, out num, out it))
          return;
        // ISSUE: reference to a compiler-generated field
        do
        {
          // ISSUE: reference to a compiler-generated field
          GroupUpdater<CullingData, GroupData, BatchData, InstanceData> groupUpdater1 = this.m_BatchGroupUpdater.BeginGroup(num);
          // ISSUE: reference to a compiler-generated field
          GroupInstanceUpdater<CullingData, GroupData, BatchData, InstanceData> groupInstanceUpdater1 = this.m_BatchInstanceUpdater.BeginGroup(num);
          GroupData groupData = groupUpdater1.GetGroupData();
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BatchGroup> batchGroup2 = this.m_BatchGroups[groupData.m_Mesh];
          // ISSUE: reference to a compiler-generated method
          int groupIndex = this.GetGroupIndex(batchGroup1, groupData);
          // ISSUE: reference to a compiler-generated field
          GroupUpdater<CullingData, GroupData, BatchData, InstanceData> groupUpdater2 = this.m_BatchGroupUpdater.BeginGroup(groupIndex);
          // ISSUE: reference to a compiler-generated field
          GroupInstanceUpdater<CullingData, GroupData, BatchData, InstanceData> groupInstanceUpdater2 = this.m_BatchInstanceUpdater.BeginGroup(groupIndex);
          int mergeIndex = groupUpdater2.MergeGroup(num, groupInstanceUpdater2);
          // ISSUE: reference to a compiler-generated method
          this.SetGroupIndex(batchGroup2, groupData, groupIndex, mergeIndex);
          for (int index1 = groupInstanceUpdater1.GetInstanceCount() - 1; index1 >= 0; --index1)
          {
            InstanceData instanceData = groupInstanceUpdater1.GetInstanceData(index1);
            CullingData cullingData = groupInstanceUpdater1.GetCullingData(index1);
            groupInstanceUpdater1.RemoveInstance(index1);
            int targetInstance = groupInstanceUpdater2.AddInstance(instanceData, cullingData, mergeIndex);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.SetInstanceIndex(this.m_MeshBatches[instanceData.m_Entity], num, index1, groupIndex, targetInstance);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_BatchInstanceUpdater.EndGroup(groupInstanceUpdater1);
          // ISSUE: reference to a compiler-generated field
          this.m_BatchGroupUpdater.EndGroup(groupUpdater1);
          // ISSUE: reference to a compiler-generated field
          this.m_BatchInstanceUpdater.EndGroup(groupInstanceUpdater2);
          // ISSUE: reference to a compiler-generated field
          this.m_BatchGroupUpdater.EndGroup(groupUpdater2);
        }
        while (this.m_MergeGroups.TryGetNextValue(out num, ref it));
      }

      private int GetGroupIndex(DynamicBuffer<BatchGroup> groups, GroupData groupData)
      {
        for (int index = 0; index < groups.Length; ++index)
        {
          ref BatchGroup local = ref groups.ElementAt(index);
          if (local.m_Layer == groupData.m_Layer && local.m_Type == groupData.m_MeshType && (int) local.m_Partition == (int) groupData.m_Partition)
            return local.m_GroupIndex;
        }
        return -1;
      }

      private void SetGroupIndex(
        DynamicBuffer<BatchGroup> groups,
        GroupData groupData,
        int groupIndex,
        int mergeIndex)
      {
        for (int index = 0; index < groups.Length; ++index)
        {
          ref BatchGroup local = ref groups.ElementAt(index);
          if (local.m_Layer == groupData.m_Layer && local.m_Type == groupData.m_MeshType && (int) local.m_Partition == (int) groupData.m_Partition)
          {
            local.m_GroupIndex = groupIndex;
            local.m_MergeIndex = mergeIndex;
            break;
          }
        }
      }

      private void SetInstanceIndex(
        DynamicBuffer<MeshBatch> meshBatches,
        int sourceGroup,
        int sourceInstance,
        int targetGroup,
        int targetInstance)
      {
        for (int index = 0; index < meshBatches.Length; ++index)
        {
          ref MeshBatch local = ref meshBatches.ElementAt(index);
          if (local.m_GroupIndex == sourceGroup && local.m_InstanceIndex == sourceInstance)
          {
            local.m_GroupIndex = targetGroup;
            local.m_InstanceIndex = targetInstance;
            break;
          }
        }
      }
    }

    [BurstCompile]
    private struct MergeCleanupJob : IJob
    {
      public NativeList<Entity> m_MergeMeshes;
      public NativeParallelMultiHashMap<Entity, int> m_MergeGroups;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MergeMeshes.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_MergeGroups.Clear();
      }
    }

    [BurstCompile]
    private struct InitializeLodFadeJob : IJobParallelFor
    {
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.ParallelInstanceWriter m_NativeBatchInstances;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        WriteableCullingAccessor<CullingData> cullingAccessor = this.m_NativeBatchInstances.GetCullingAccessor(index);
        for (int index1 = 0; index1 < cullingAccessor.Length; ++index1)
          cullingAccessor.Get(index1).lodFade = (int4) (int) byte.MaxValue;
      }
    }

    [BurstCompile]
    private struct AllocateBuffersJob : IJob
    {
      [ReadOnly]
      public NativeList<PropertyData> m_ObjectProperties;
      [ReadOnly]
      public NativeList<PropertyData> m_NetProperties;
      [ReadOnly]
      public NativeList<PropertyData> m_LaneProperties;
      [ReadOnly]
      public NativeList<PropertyData> m_ZoneProperties;
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchInstances;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        UpdatedPropertiesEnumerator updatedProperties = this.m_NativeBatchGroups.GetUpdatedProperties();
        int groupIndex1;
        while (updatedProperties.GetNextUpdatedGroup(out groupIndex1))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchGroups.AllocatePropertyBuffers(groupIndex1, 16777216U, this.m_NativeBatchInstances);
          // ISSUE: reference to a compiler-generated field
          GroupData groupData = this.m_NativeBatchGroups.GetGroupData(groupIndex1);
          switch (groupData.m_MeshType)
          {
            case MeshType.Object:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.SetPropertyIndices(this.m_ObjectProperties, groupIndex1, ref groupData);
              break;
            case MeshType.Net:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.SetPropertyIndices(this.m_NetProperties, groupIndex1, ref groupData);
              break;
            case MeshType.Lane:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.SetPropertyIndices(this.m_LaneProperties, groupIndex1, ref groupData);
              break;
            case MeshType.Zone:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.SetPropertyIndices(this.m_ZoneProperties, groupIndex1, ref groupData);
              break;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchGroups.SetGroupData(groupIndex1, groupData);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_NativeBatchGroups.ClearUpdatedProperties();
        // ISSUE: reference to a compiler-generated field
        UpdatedInstanceEnumerator updatedInstances = this.m_NativeBatchInstances.GetUpdatedInstances();
        int groupIndex2;
        while (updatedInstances.GetNextUpdatedGroup(out groupIndex2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchInstances.AllocateInstanceBuffers(groupIndex2, 16777216U, this.m_NativeBatchGroups);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_NativeBatchInstances.ClearUpdatedInstances();
      }

      private void SetPropertyIndices(
        NativeList<PropertyData> properties,
        int groupIndex,
        ref GroupData groupData)
      {
        // ISSUE: reference to a compiler-generated field
        NativeGroupPropertyAccessor propertyAccessor = this.m_NativeBatchGroups.GetGroupPropertyAccessor(groupIndex);
        for (int property = 0; property < properties.Length; ++property)
          groupData.SetPropertyIndex(property, -1);
        int num = propertyAccessor.PropertyCount;
        if (num > 30)
        {
          Debug.Log((object) string.Format("Too many group properties ({0})!", (object) num));
          num = 30;
        }
        for (int index1 = 0; index1 < num; ++index1)
        {
          int propertyName = propertyAccessor.GetPropertyName(index1);
          int dataIndex = propertyAccessor.GetDataIndex(index1);
          for (int index2 = 0; index2 < properties.Length; ++index2)
          {
            PropertyData property = properties[index2];
            if (propertyName == property.m_NameID && dataIndex == property.m_DataIndex)
              groupData.SetPropertyIndex(index2, index1);
          }
        }
      }
    }

    [BurstCompile]
    private struct GenerateSubBatchesJob : IJob
    {
      public NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> m_NativeSubBatches;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        UpdatedSubBatchEnumerator updatedSubBatches = this.m_NativeSubBatches.GetUpdatedSubBatches();
        int groupIndex;
        while (updatedSubBatches.GetNextUpdatedGroup(out groupIndex))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NativeSubBatches.GenerateSubBatches(groupIndex);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_NativeSubBatches.ClearUpdatedSubBatches();
      }
    }

    private struct ActiveGroupData
    {
      public int m_BatchOffset;
      public int m_InstanceOffset;
    }

    [BurstCompile]
    private struct AllocateCullingJob : IJob
    {
      [ReadOnly]
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      [ReadOnly]
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchInstances;
      [ReadOnly]
      public BatchRenderFlags m_RequiredFlagMask;
      [ReadOnly]
      public int m_MaxSplitBatchCount;
      public BatchCullingOutput m_CullingOutput;
      public NativeArray<BatchManagerSystem.ActiveGroupData> m_ActiveGroupData;

      public unsafe void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ref BatchCullingOutputDrawCommands local = ref this.m_CullingOutput.drawCommands.ElementAt<BatchCullingOutputDrawCommands>(0);
        *(BatchCullingOutputDrawCommands*) ref local = new BatchCullingOutputDrawCommands();
        // ISSUE: reference to a compiler-generated field
        int activeGroupCount = this.m_NativeBatchInstances.GetActiveGroupCount();
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < activeGroupCount; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          int groupIndex = this.m_NativeBatchInstances.GetGroupIndex(index);
          // ISSUE: reference to a compiler-generated field
          GroupData groupData = this.m_NativeBatchGroups.GetGroupData(groupIndex);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((groupData.m_RenderFlags & this.m_RequiredFlagMask) == this.m_RequiredFlagMask)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActiveGroupData[index] = new BatchManagerSystem.ActiveGroupData()
            {
              m_BatchOffset = num1,
              m_InstanceOffset = num2
            };
            // ISSUE: reference to a compiler-generated field
            int instanceCount = this.m_NativeBatchInstances.GetInstanceCount(groupIndex);
            // ISSUE: reference to a compiler-generated field
            num1 += this.m_NativeBatchGroups.GetBatchCount(groupIndex);
            num2 += instanceCount * (1 + (int) groupData.m_LodCount);
            if (instanceCount > 16777216)
              Debug.Log((object) string.Format("Too many batch instances: {0} > 16777216", (object) instanceCount));
          }
        }
        // ISSUE: reference to a compiler-generated field
        int num3 = num1 * this.m_MaxSplitBatchCount;
        local.drawCommandCount = num3;
        local.drawCommands = (BatchDrawCommand*) UnsafeUtility.Malloc((long) (UnsafeUtility.SizeOf<BatchDrawCommand>() * num3), UnsafeUtility.AlignOf<BatchDrawCommand>(), Allocator.TempJob);
        local.visibleInstanceCount = num2;
        local.visibleInstances = (int*) UnsafeUtility.Malloc((long) (UnsafeUtility.SizeOf<int>() * num2), UnsafeUtility.AlignOf<int>(), Allocator.TempJob);
        local.drawRangeCount = num1;
        local.drawRanges = (BatchDrawRange*) UnsafeUtility.Malloc((long) (UnsafeUtility.SizeOf<BatchDrawRange>() * num1), UnsafeUtility.AlignOf<BatchDrawRange>(), Allocator.TempJob);
      }
    }

    private struct CullingSplitData
    {
      public ulong m_PlaneMask;
      public int m_SplitMask;
      public float m_ShadowHeightThreshold;
      public float m_ShadowVolumeThreshold;
    }

    [BurstCompile]
    private struct CullingPlanesJob : IJob
    {
      [ReadOnly]
      public NativeArray<Plane> m_CullingPlanes;
      [ReadOnly]
      public NativeArray<CullingSplit> m_CullingSplits;
      [ReadOnly]
      public float3 m_ShadowCullingData;
      public NativeList<BatchManagerSystem.CullingSplitData> m_SplitData;
      public NativeList<FrustumPlanes.PlanePacket4> m_PlanePackets;
      private const float kGuardPixels = 5f;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Plane> cullingPlanes = new NativeArray<Plane>(this.m_CullingPlanes.Length, Allocator.Temp);
        int cullingPlaneCount = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_CullingSplits.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          CullingSplit cullingSplit = this.m_CullingSplits[index1];
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          BatchManagerSystem.CullingSplitData cullingSplitData = new BatchManagerSystem.CullingSplitData()
          {
            m_SplitMask = 1 << index1,
            m_ShadowHeightThreshold = 0.0f,
            m_ShadowVolumeThreshold = 0.0f
          };
          for (int index2 = 0; index2 < cullingSplit.cullingPlaneCount; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            Plane cullingPlane = this.m_CullingPlanes[cullingSplit.cullingPlaneOffset + index2];
            int index3 = -1;
            for (int index4 = 0; index4 < cullingPlaneCount; ++index4)
            {
              Plane plane = cullingPlanes[index4];
              if (math.all((float3) cullingPlane.normal == (float3) plane.normal) && (double) cullingPlane.distance == (double) plane.distance)
              {
                index3 = index4;
                break;
              }
            }
            if (index3 == -1)
            {
              index3 = cullingPlaneCount++;
              cullingPlanes[index3] = cullingPlane;
            }
            // ISSUE: reference to a compiler-generated field
            cullingSplitData.m_PlaneMask |= (ulong) (1L << index3);
          }
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_ShadowCullingData.x > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            float cascadePixelToMeters = this.CalculateCascadePixelToMeters(cullingSplit.sphereRadius, this.m_ShadowCullingData.x);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cullingSplitData.m_ShadowHeightThreshold = cascadePixelToMeters * this.m_ShadowCullingData.y;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cullingSplitData.m_ShadowVolumeThreshold = cascadePixelToMeters * cascadePixelToMeters * this.m_ShadowCullingData.z;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_SplitData.Add(in cullingSplitData);
        }
        // ISSUE: reference to a compiler-generated field
        FrustumPlanes.BuildSOAPlanePackets(cullingPlanes, cullingPlaneCount, this.m_PlanePackets);
        if (cullingPlaneCount > 64)
          Debug.Log((object) string.Format("Too many unique culling planes: {0} > 64", (object) cullingPlaneCount));
        // ISSUE: reference to a compiler-generated field
        if (this.m_CullingSplits.Length > 8)
        {
          // ISSUE: reference to a compiler-generated field
          Debug.Log((object) string.Format("Too many culling splits: {0} > 8", (object) this.m_CullingSplits.Length));
        }
        cullingPlanes.Dispose();
      }

      private float CalculateCascadePixelToMeters(float radius, float shadowResolution)
      {
        float num = radius * 2f;
        return (num + (float) (10.0 * ((double) num / (double) shadowResolution))) / shadowResolution;
      }
    }

    [BurstCompile]
    private struct FinalizeCullingJob : IJob
    {
      public BatchCullingOutput m_CullingOutput;

      public unsafe void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ref BatchCullingOutputDrawCommands local = ref this.m_CullingOutput.drawCommands.ElementAt<BatchCullingOutputDrawCommands>(0);
        BatchDrawRange batchDrawRange1 = new BatchDrawRange();
        int num1 = 0;
        int num2 = 0;
        for (int index1 = 0; index1 < local.drawRangeCount; ++index1)
        {
          BatchDrawRange batchDrawRange2 = local.drawRanges[index1];
          if (batchDrawRange2.drawCommandsCount != 0U)
          {
            int drawCommandsBegin = (int) batchDrawRange2.drawCommandsBegin;
            batchDrawRange2.drawCommandsBegin = (uint) num1;
            for (int index2 = 0; (long) index2 < (long) batchDrawRange2.drawCommandsCount; ++index2)
              local.drawCommands[num1++] = local.drawCommands[drawCommandsBegin + index2];
            if (UnsafeUtility.MemCmp((void*) &batchDrawRange1.filterSettings, (void*) &batchDrawRange2.filterSettings, (long) sizeof (BatchFilterSettings)) != 0)
            {
              if (batchDrawRange1.drawCommandsCount != 0U)
                local.drawRanges[num2++] = batchDrawRange1;
              batchDrawRange1 = batchDrawRange2;
            }
            else
              batchDrawRange1.drawCommandsCount += batchDrawRange2.drawCommandsCount;
          }
        }
        if (batchDrawRange1.drawCommandsCount != 0U)
          local.drawRanges[num2++] = batchDrawRange1;
        local.drawCommandCount = num1;
        local.drawRangeCount = num2;
      }
    }

    [BurstCompile]
    private struct BatchCullingJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      [ReadOnly]
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchInstances;
      [ReadOnly]
      public NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> m_NativeSubBatches;
      [ReadOnly]
      public BatchRenderFlags m_RequiredFlagMask;
      [ReadOnly]
      public BatchRenderFlags m_RenderFlagMask;
      [ReadOnly]
      public int m_MaxSplitBatchCount;
      [ReadOnly]
      public bool m_IsShadowCulling;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<BatchManagerSystem.ActiveGroupData> m_ActiveGroupData;
      [ReadOnly]
      public NativeList<BatchManagerSystem.CullingSplitData> m_SplitData;
      [ReadOnly]
      public NativeList<FrustumPlanes.PlanePacket4> m_CullingPlanePackets;
      [NativeDisableParallelForRestriction]
      public BatchCullingOutput m_CullingOutput;

      public unsafe void Execute(int index)
      {
        int num1 = index;
        // ISSUE: reference to a compiler-generated field
        int groupIndex = this.m_NativeBatchInstances.GetGroupIndex(num1);
        // ISSUE: reference to a compiler-generated field
        GroupData groupData = this.m_NativeBatchGroups.GetGroupData(groupIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((groupData.m_RenderFlags & this.m_RequiredFlagMask) != this.m_RequiredFlagMask)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        BatchManagerSystem.ActiveGroupData activeGroupData = this.m_ActiveGroupData[num1];
        // ISSUE: reference to a compiler-generated field
        NativeBatchAccessor<BatchData> batchAccessor = this.m_NativeBatchGroups.GetBatchAccessor(groupIndex);
        // ISSUE: reference to a compiler-generated field
        NativeCullingAccessor<CullingData> cullingAccessor = this.m_NativeBatchInstances.GetCullingAccessor(groupIndex);
        // ISSUE: reference to a compiler-generated field
        NativeSubBatchAccessor<BatchData> subBatchAccessor = this.m_NativeSubBatches.GetSubBatchAccessor(groupIndex);
        // ISSUE: reference to a compiler-generated field
        ref BatchCullingOutputDrawCommands local = ref this.m_CullingOutput.drawCommands.ElementAt<BatchCullingOutputDrawCommands>(0);
        float3 boundsCenter;
        float3 boundsExtents;
        // ISSUE: reference to a compiler-generated field
        if (this.m_IsShadowCulling)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchInstances.GetShadowBounds(groupIndex, out boundsCenter, out boundsExtents);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchInstances.GetBounds(groupIndex, out boundsCenter, out boundsExtents);
        }
        // ISSUE: reference to a compiler-generated field
        FrustumPlanes.PlanePacket4* unsafeReadOnlyPtr1 = this.m_CullingPlanePackets.GetUnsafeReadOnlyPtr<FrustumPlanes.PlanePacket4>();
        // ISSUE: reference to a compiler-generated field
        int length1 = this.m_CullingPlanePackets.Length;
        // ISSUE: reference to a compiler-generated field
        BatchManagerSystem.CullingSplitData* unsafeReadOnlyPtr2 = this.m_SplitData.GetUnsafeReadOnlyPtr<BatchManagerSystem.CullingSplitData>();
        // ISSUE: reference to a compiler-generated field
        int length2 = this.m_SplitData.Length;
        // ISSUE: reference to a compiler-generated field
        int batchCount = this.m_NativeBatchGroups.GetBatchCount(groupIndex);
        // ISSUE: reference to a compiler-generated field
        int instanceCount = this.m_NativeBatchInstances.GetInstanceCount(groupIndex);
        // ISSUE: reference to a compiler-generated field
        int* numPtr1 = local.visibleInstances + activeGroupData.m_InstanceOffset;
        // ISSUE: untyped stack allocation
        int** numPtr2 = (int**) __untypedstackalloc(checked (new IntPtr(16) * sizeof (int*)));
        // ISSUE: reference to a compiler-generated field
        bool flag = length2 == 4 && this.m_IsShadowCulling;
        if (length2 == 1)
        {
          // ISSUE: variable of a compiler-generated type
          BatchManagerSystem.CullingSplitData cullingSplitData = *unsafeReadOnlyPtr2;
          for (int index1 = 0; index1 <= (int) groupData.m_LodCount; ++index1)
          {
            int num2 = index1 * instanceCount;
            numPtr2[index1] = numPtr1 + num2;
          }
          switch (FrustumPlanes.CalculateIntersectResult(unsafeReadOnlyPtr1, length1, boundsCenter, boundsExtents))
          {
            case FrustumPlanes.IntersectResult.In:
              for (int index2 = 0; index2 < instanceCount; ++index2)
              {
                int4 lodRange = cullingAccessor.Get(index2).lodRange;
                // ISSUE: reference to a compiler-generated field
                lodRange.xy = math.select(lodRange.xy, lodRange.zw, this.m_IsShadowCulling);
                for (int x = lodRange.x; x < lodRange.y; ++x)
                {
                  int** numPtr3 = numPtr2 + x;
                  int* numPtr4 = *numPtr3;
                  *numPtr3 = numPtr4 + 1;
                  *numPtr4 = index2;
                }
              }
              break;
            case FrustumPlanes.IntersectResult.Partial:
              for (int index3 = 0; index3 < instanceCount; ++index3)
              {
                CullingData cullingData = cullingAccessor.Get(index3);
                float3 center = MathUtils.Center(cullingData.m_Bounds);
                float3 extents = MathUtils.Extents(cullingData.m_Bounds);
                if (FrustumPlanes.Intersect(unsafeReadOnlyPtr1, length1, center, extents))
                {
                  int4 lodRange = cullingData.lodRange;
                  // ISSUE: reference to a compiler-generated field
                  lodRange.xy = math.select(lodRange.xy, lodRange.zw, this.m_IsShadowCulling);
                  for (int x = lodRange.x; x < lodRange.y; ++x)
                  {
                    int** numPtr5 = numPtr2 + x;
                    int* numPtr6 = *numPtr5;
                    *numPtr5 = numPtr6 + 1;
                    *numPtr6 = index3;
                  }
                }
              }
              break;
          }
          int num3 = -1;
          int num4 = 0;
          int num5 = 0;
          for (int index4 = 0; index4 < batchCount; ++index4)
          {
            BatchData batchData = batchAccessor.GetBatchData(index4);
            // ISSUE: reference to a compiler-generated field
            int index5 = activeGroupData.m_BatchOffset + index4;
            if ((int) batchData.m_LodIndex != num3)
            {
              int index6 = (int) groupData.m_LodCount - (int) batchData.m_LodIndex;
              num3 = (int) batchData.m_LodIndex;
              int num6 = index6 * instanceCount;
              num5 = (int) (numPtr2[index6] - (numPtr1 + num6));
              // ISSUE: reference to a compiler-generated field
              num4 = num6 + activeGroupData.m_InstanceOffset;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (num5 != 0 && (batchData.m_RenderFlags & this.m_RequiredFlagMask) == this.m_RequiredFlagMask)
            {
              BatchMeshID meshID;
              BatchMaterialID materialID;
              int subMeshIndex;
              batchAccessor.GetRenderData(index4, out meshID, out materialID, out subMeshIndex);
              BatchID batchId = subBatchAccessor.GetBatchID(index4);
              // ISSUE: reference to a compiler-generated field
              int num7 = (int) (batchData.m_RenderFlags & this.m_RenderFlagMask);
              BatchDrawCommand batchDrawCommand = new BatchDrawCommand();
              batchDrawCommand.visibleOffset = (uint) num4;
              batchDrawCommand.visibleCount = (uint) num5;
              batchDrawCommand.batchID = batchId;
              batchDrawCommand.materialID = materialID;
              batchDrawCommand.meshID = meshID;
              batchDrawCommand.submeshIndex = (ushort) subMeshIndex;
              // ISSUE: reference to a compiler-generated field
              batchDrawCommand.splitVisibilityMask = (ushort) cullingSplitData.m_SplitMask;
              BatchFilterSettings batchFilterSettings = new BatchFilterSettings();
              batchFilterSettings.layer = batchData.m_Layer;
              batchFilterSettings.renderingLayerMask = uint.MaxValue;
              batchFilterSettings.motionMode = MotionVectorGenerationMode.ForceNoMotion;
              batchFilterSettings.receiveShadows = (batchData.m_RenderFlags & BatchRenderFlags.ReceiveShadows) != 0;
              batchFilterSettings.shadowCastingMode = (ShadowCastingMode) batchData.m_ShadowCastingMode;
              if ((num7 & 1) != 0)
              {
                batchDrawCommand.flags |= BatchDrawCommandFlags.HasMotion;
                batchFilterSettings.motionMode = MotionVectorGenerationMode.Object;
              }
              local.drawCommands[index5] = batchDrawCommand;
              *(local.drawRanges + index5) = new BatchDrawRange()
              {
                drawCommandsBegin = (uint) index5,
                drawCommandsCount = 1U,
                filterSettings = batchFilterSettings
              };
            }
            else
              local.drawRanges[index5] = new BatchDrawRange();
          }
        }
        else
        {
          for (int index7 = 0; index7 <= (int) groupData.m_LodCount; ++index7)
          {
            int num8 = index7 * instanceCount;
            numPtr2[index7] = numPtr1 + num8;
          }
          ulong inMask;
          ulong outMask1;
          FrustumPlanes.Intersect(unsafeReadOnlyPtr1, length1, boundsCenter, boundsExtents, out inMask, out outMask1);
          int num9 = 0;
          ulong num10 = 0;
          int x1 = length2;
          int num11 = 0;
          for (int y = 0; y < length2; ++y)
          {
            // ISSUE: variable of a compiler-generated type
            BatchManagerSystem.CullingSplitData cullingSplitData = unsafeReadOnlyPtr2[y];
            // ISSUE: reference to a compiler-generated field
            if (((long) cullingSplitData.m_PlaneMask & (long) outMask1) == 0L)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (((long) cullingSplitData.m_PlaneMask & (long) inMask) == (long) cullingSplitData.m_PlaneMask)
              {
                // ISSUE: reference to a compiler-generated field
                num9 |= cullingSplitData.m_SplitMask;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                num10 |= cullingSplitData.m_PlaneMask & ~inMask;
                x1 = math.min(x1, y);
                num11 = y;
              }
            }
          }
          if (num10 != 0UL)
          {
            int x2 = length1;
            int num12 = 0;
            for (int y = 0; y < length1; ++y)
            {
              if (((long) num10 & 15L << (y << 2)) != 0L)
              {
                x2 = math.min(x2, y);
                num12 = y;
              }
            }
            FrustumPlanes.PlanePacket4* cullingPlanePackets = unsafeReadOnlyPtr1 + x2;
            int length3 = num12 - x2 + 1;
            int num13 = x2 << 2;
            if (num12 < 8)
            {
              for (int index8 = 0; index8 < instanceCount; ++index8)
              {
                CullingData cullingData = cullingAccessor.Get(index8);
                int num14 = num9;
                float3 center = MathUtils.Center(cullingData.m_Bounds);
                float3 extents = MathUtils.Extents(cullingData.m_Bounds);
                uint outMask2;
                FrustumPlanes.Intersect(cullingPlanePackets, length3, center, extents, out outMask2);
                outMask2 <<= num13;
                for (int index9 = x1; index9 <= num11; ++index9)
                {
                  // ISSUE: variable of a compiler-generated type
                  BatchManagerSystem.CullingSplitData cullingSplitData = unsafeReadOnlyPtr2[index9];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num14 |= math.select(0, cullingSplitData.m_SplitMask, ((int) (uint) cullingSplitData.m_PlaneMask & (int) outMask2) == 0);
                }
                if (num14 != 0)
                {
                  int num15 = num14 << 24 | index8;
                  int4 lodRange = cullingData.lodRange;
                  // ISSUE: reference to a compiler-generated field
                  lodRange.xy = math.select(lodRange.xy, lodRange.zw, this.m_IsShadowCulling);
                  for (int x3 = lodRange.x; x3 < lodRange.y; ++x3)
                  {
                    int** numPtr7 = numPtr2 + x3;
                    int* numPtr8 = *numPtr7;
                    *numPtr7 = numPtr8 + 1;
                    *numPtr8 = num15;
                  }
                }
              }
            }
            else
            {
              for (int index10 = 0; index10 < instanceCount; ++index10)
              {
                CullingData cullingData = cullingAccessor.Get(index10);
                int num16 = num9;
                float3 center = MathUtils.Center(cullingData.m_Bounds);
                float3 extents = MathUtils.Extents(cullingData.m_Bounds);
                ulong outMask3;
                FrustumPlanes.Intersect(cullingPlanePackets, length3, center, extents, out outMask3);
                outMask3 <<= num13;
                for (int index11 = x1; index11 <= num11; ++index11)
                {
                  // ISSUE: variable of a compiler-generated type
                  BatchManagerSystem.CullingSplitData cullingSplitData = unsafeReadOnlyPtr2[index11];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num16 |= math.select(0, cullingSplitData.m_SplitMask, ((long) cullingSplitData.m_PlaneMask & (long) outMask3) == 0L);
                }
                if (num16 != 0)
                {
                  int num17 = num16 << 24 | index10;
                  int4 lodRange = cullingData.lodRange;
                  // ISSUE: reference to a compiler-generated field
                  lodRange.xy = math.select(lodRange.xy, lodRange.zw, this.m_IsShadowCulling);
                  for (int x4 = lodRange.x; x4 < lodRange.y; ++x4)
                  {
                    int** numPtr9 = numPtr2 + x4;
                    int* numPtr10 = *numPtr9;
                    *numPtr9 = numPtr10 + 1;
                    *numPtr10 = num17;
                  }
                }
              }
            }
          }
          else if (num9 != 0)
          {
            for (int index12 = 0; index12 < instanceCount; ++index12)
            {
              int4 lodRange = cullingAccessor.Get(index12).lodRange;
              // ISSUE: reference to a compiler-generated field
              lodRange.xy = math.select(lodRange.xy, lodRange.zw, this.m_IsShadowCulling);
              for (int x5 = lodRange.x; x5 < lodRange.y; ++x5)
              {
                int** numPtr11 = numPtr2 + x5;
                int* numPtr12 = *numPtr11;
                *numPtr11 = numPtr12 + 1;
                *numPtr12 = index12;
              }
            }
          }
          int num18 = -1;
          int length4 = 0;
          int index13 = 0;
          int* numPtr13 = stackalloc int[15];
          int* numPtr14 = stackalloc int[15];
          int* numPtr15 = stackalloc int[15];
          for (int index14 = 0; index14 < batchCount; ++index14)
          {
            BatchData batchData = batchAccessor.GetBatchData(index14);
            // ISSUE: reference to a compiler-generated field
            int index15 = activeGroupData.m_BatchOffset + index14;
            // ISSUE: reference to a compiler-generated field
            int num19 = index15 * this.m_MaxSplitBatchCount;
            if ((int) batchData.m_LodIndex != num18)
            {
              int index16 = (int) groupData.m_LodCount - (int) batchData.m_LodIndex;
              num18 = (int) batchData.m_LodIndex;
              int num20 = index16 * instanceCount;
              int* array = numPtr1 + num20;
              length4 = (int) (numPtr2[index16] - array);
              // ISSUE: reference to a compiler-generated field
              int num21 = num20 + activeGroupData.m_InstanceOffset;
              if (length4 != 0)
              {
                if (num10 != 0UL)
                {
                  if (length4 >= 3)
                    NativeSortExtension.Sort<int>(array, length4);
                  index13 = 0;
                  int num22 = 0;
                  while (num22 < length4)
                  {
                    int num23 = num22;
                    int* numPtr16 = array + num22++;
                    int num24 = *numPtr16 >>> 24;
                    int* numPtr17 = numPtr16;
                    int num25 = *numPtr17 & 16777215;
                    *numPtr17 = num25;
                    // ISSUE: reference to a compiler-generated field
                    if (index13 < this.m_MaxSplitBatchCount - 1)
                    {
                      for (; num22 < length4; ++num22)
                      {
                        int* numPtr18 = array + num22;
                        if (*numPtr18 >>> 24 == num24)
                        {
                          int* numPtr19 = numPtr18;
                          int num26 = *numPtr19 & 16777215;
                          *numPtr19 = num26;
                        }
                        else
                          break;
                      }
                    }
                    else
                    {
                      for (; num22 < length4; ++num22)
                      {
                        int* numPtr20 = array + num22;
                        num24 |= *numPtr20 >>> 24;
                        int* numPtr21 = numPtr20;
                        int num27 = *numPtr21 & 16777215;
                        *numPtr21 = num27;
                      }
                    }
                    numPtr13[index13] = num21 + num23;
                    numPtr14[index13] = num22 - num23;
                    numPtr15[index13] = num24;
                    ++index13;
                  }
                }
                else
                {
                  index13 = 1;
                  numPtr13[0] = num21;
                  numPtr14[0] = length4;
                  numPtr15[0] = num9;
                }
              }
            }
            // ISSUE: reference to a compiler-generated method
            int num28 = flag ? this.CalculateBatchMask(batchData, unsafeReadOnlyPtr2, length2) : -1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (length4 != 0 && (batchData.m_RenderFlags & this.m_RequiredFlagMask) == this.m_RequiredFlagMask)
            {
              BatchMeshID meshID;
              BatchMaterialID materialID;
              int subMeshIndex;
              batchAccessor.GetRenderData(index14, out meshID, out materialID, out subMeshIndex);
              BatchID batchId = subBatchAccessor.GetBatchID(index14);
              // ISSUE: reference to a compiler-generated field
              int num29 = (int) (batchData.m_RenderFlags & this.m_RenderFlagMask);
              BatchDrawCommand batchDrawCommand = new BatchDrawCommand();
              batchDrawCommand.batchID = batchId;
              batchDrawCommand.materialID = materialID;
              batchDrawCommand.meshID = meshID;
              batchDrawCommand.submeshIndex = (ushort) subMeshIndex;
              BatchFilterSettings batchFilterSettings = new BatchFilterSettings();
              batchFilterSettings.layer = batchData.m_Layer;
              batchFilterSettings.renderingLayerMask = uint.MaxValue;
              batchFilterSettings.motionMode = MotionVectorGenerationMode.ForceNoMotion;
              batchFilterSettings.receiveShadows = (batchData.m_RenderFlags & BatchRenderFlags.ReceiveShadows) != 0;
              batchFilterSettings.shadowCastingMode = (ShadowCastingMode) batchData.m_ShadowCastingMode;
              if ((num29 & 1) != 0)
              {
                batchDrawCommand.flags |= BatchDrawCommandFlags.HasMotion;
                batchFilterSettings.motionMode = MotionVectorGenerationMode.Object;
              }
              int num30 = 0;
              for (int index17 = 0; index17 < index13; ++index17)
              {
                batchDrawCommand.visibleOffset = (uint) numPtr13[index17];
                batchDrawCommand.visibleCount = (uint) numPtr14[index17];
                batchDrawCommand.splitVisibilityMask = (ushort) (numPtr15[index17] & num28);
                if (batchDrawCommand.splitVisibilityMask != (ushort) 0)
                {
                  local.drawCommands[num19 + num30] = batchDrawCommand;
                  ++num30;
                }
              }
              if (num30 > 0)
                *(local.drawRanges + index15) = new BatchDrawRange()
                {
                  drawCommandsBegin = (uint) num19,
                  drawCommandsCount = (uint) num30,
                  filterSettings = batchFilterSettings
                };
              else
                local.drawRanges[index15] = new BatchDrawRange();
            }
            else
              local.drawRanges[index15] = new BatchDrawRange();
          }
        }
      }

      private unsafe int CalculateBatchMask(
        BatchData batchData,
        BatchManagerSystem.CullingSplitData* cullSplitPtr,
        int length)
      {
        int batchMask = 0;
        for (int index = 0; index < length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          BatchManagerSystem.CullingSplitData cullingSplitData = cullSplitPtr[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          batchMask |= (double) batchData.m_ShadowArea < (double) cullingSplitData.m_ShadowVolumeThreshold || (double) batchData.m_ShadowHeight < (double) cullingSplitData.m_ShadowHeightThreshold ? 0 : 1 << index;
        }
        return batchMask;
      }
    }

    private struct TypeHandle
    {
      public BufferLookup<MeshBatch> __Game_Rendering_MeshBatch_RW_BufferLookup;
      public BufferLookup<BatchGroup> __Game_Prefabs_BatchGroup_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshBatch_RW_BufferLookup = state.GetBufferLookup<MeshBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BatchGroup_RW_BufferLookup = state.GetBufferLookup<BatchGroup>();
      }
    }
  }
}
