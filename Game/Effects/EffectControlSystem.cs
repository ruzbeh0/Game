// Decompiled with JetBrains decompiler
// Type: Game.Effects.EffectControlSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Effects
{
  [CompilerGenerated]
  public class EffectControlSystem : GameSystemBase, IPostDeserialize
  {
    private VFXSystem m_VFXSystem;
    private SearchSystem m_SearchSystem;
    private EffectFlagSystem m_EffectFlagSystem;
    private SimulationSystem m_SimulationSystem;
    private PreCullingSystem m_PreCullingSystem;
    private ToolSystem m_ToolSystem;
    private RenderingSystem m_RenderingSystem;
    private BatchDataSystem m_BatchDataSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private EffectControlData m_EffectControlData;
    private NativeList<EnabledEffectData> m_EnabledData;
    private EntityQuery m_UpdatedEffectsQuery;
    private EntityQuery m_AllEffectsQuery;
    private JobHandle m_EnabledWriteDependencies;
    private JobHandle m_EnabledReadDependencies;
    private float3 m_PrevCameraPosition;
    private float3 m_PrevCameraDirection;
    private float4 m_PrevLodParameters;
    private bool m_Loaded;
    private bool m_ResetPrevious;
    private EffectControlSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_VFXSystem = this.World.GetOrCreateSystemManaged<VFXSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectFlagSystem = this.World.GetOrCreateSystemManaged<EffectFlagSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchDataSystem = this.World.GetOrCreateSystemManaged<BatchDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlData = new EffectControlData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledData = new NativeList<EnabledEffectData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedEffectsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EnabledEffect>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<EffectsUpdated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllEffectsQuery = this.GetEntityQuery(ComponentType.ReadOnly<EnabledEffect>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledData.Dispose();
      base.OnDestroy();
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledData.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ResetPrevious = true;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    public NativeList<EnabledEffectData> GetEnabledData(bool readOnly, out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_EnabledWriteDependencies : JobHandle.CombineDependencies(this.m_EnabledWriteDependencies, this.m_EnabledReadDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_EnabledData;
    }

    public void AddEnabledDataReader(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledReadDependencies = JobHandle.CombineDependencies(this.m_EnabledReadDependencies, dependencies);
    }

    public void AddEnabledDataWriter(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledWriteDependencies = dependencies;
    }

    public void GetLodParameters(
      out float4 lodParameters,
      out float3 cameraPosition,
      out float3 cameraDirection)
    {
      // ISSUE: reference to a compiler-generated field
      lodParameters = this.m_PrevLodParameters;
      // ISSUE: reference to a compiler-generated field
      cameraPosition = this.m_PrevCameraPosition;
      // ISSUE: reference to a compiler-generated field
      cameraDirection = this.m_PrevCameraDirection;
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
      int num = this.GetLoaded() ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = num != 0 ? this.m_AllEffectsQuery : this.m_UpdatedEffectsQuery;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlData.Update((SystemBase) this, this.m_EffectFlagSystem.GetData(), this.m_SimulationSystem.frameIndex, this.m_ToolSystem.selected);
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      int length = this.m_EnabledData.Length;
      NativeParallelQueue<EffectControlSystem.EnabledAction> nativeParallelQueue = new NativeParallelQueue<EffectControlSystem.EnabledAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<EffectControlSystem.OverflowAction> nativeQueue = new NativeQueue<EffectControlSystem.OverflowAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeReference<int> nativeReference = new NativeReference<int>(length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      float3 float3_1 = this.m_PrevCameraPosition;
      // ISSUE: reference to a compiler-generated field
      float3 float3_2 = this.m_PrevCameraDirection;
      // ISSUE: reference to a compiler-generated field
      float4 float4 = this.m_PrevLodParameters;
      LODParameters lodParameters;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_CameraUpdateSystem.TryGetLODParameters(out lodParameters))
      {
        float3_1 = (float3) lodParameters.cameraPosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float4 = RenderingUtils.CalculateLodParameters(this.m_BatchDataSystem.GetLevelOfDetail(this.m_RenderingSystem.frameLod, this.m_CameraUpdateSystem.activeCameraController), lodParameters);
        // ISSUE: reference to a compiler-generated field
        float3_2 = this.m_CameraUpdateSystem.activeViewer.forward;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ResetPrevious)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PrevCameraPosition = float3_1;
        // ISSUE: reference to a compiler-generated field
        this.m_PrevCameraDirection = float3_2;
        // ISSUE: reference to a compiler-generated field
        this.m_PrevLodParameters = float4;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LightEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Event_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Static_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle1 = new EffectControlSystem.EffectControlJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle,
        m_ObjectType = this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle,
        m_StaticType = this.__TypeHandle.__Game_Objects_Static_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_EventType = this.__TypeHandle.__Game_Events_Event_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_EffectOwnerType = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferTypeHandle,
        m_PrefabEffectData = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
        m_PrefabLightEffectData = this.__TypeHandle.__Game_Prefabs_LightEffectData_RO_ComponentLookup,
        m_PrefabAudioEffectData = this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup,
        m_PrefabEffects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_CullingData = this.m_PreCullingSystem.GetCullingData(true, out dependencies1),
        m_EnabledEffectData = this.m_EnabledData,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      }.ScheduleParallel<EffectControlSystem.EffectControlJob>(query, JobHandle.CombineDependencies(dependencies1, this.Dependency));
      if (num == 0)
      {
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeQuadTree<SourceInfo, QuadTreeBoundsXZ> searchTree = this.m_SearchSystem.GetSearchTree(true, out dependencies2);
        NativeArray<int> nativeArray1 = new NativeArray<int>(256, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        NativeArray<int> nativeArray2 = new NativeArray<int>(256, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EffectControlSystem.TreeCullingJob1 jobData1 = new EffectControlSystem.TreeCullingJob1()
        {
          m_EffectSearchTree = searchTree,
          m_LodParameters = float4,
          m_PrevLodParameters = this.m_PrevLodParameters,
          m_CameraPosition = float3_1,
          m_PrevCameraPosition = this.m_PrevCameraPosition,
          m_CameraDirection = float3_2,
          m_PrevCameraDirection = this.m_PrevCameraDirection,
          m_NodeBuffer = nativeArray1,
          m_SubDataBuffer = nativeArray2,
          m_ActionQueue = nativeParallelQueue.AsWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EffectControlSystem.TreeCullingJob2 jobData2 = new EffectControlSystem.TreeCullingJob2()
        {
          m_EffectSearchTree = searchTree,
          m_LodParameters = float4,
          m_PrevLodParameters = this.m_PrevLodParameters,
          m_CameraPosition = float3_1,
          m_PrevCameraPosition = this.m_PrevCameraPosition,
          m_CameraDirection = float3_2,
          m_PrevCameraDirection = this.m_PrevCameraDirection,
          m_NodeBuffer = nativeArray1,
          m_SubDataBuffer = nativeArray2,
          m_ActionQueue = nativeParallelQueue.AsWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EffectControlSystem.EffectCullingJob jobData3 = new EffectControlSystem.EffectCullingJob()
        {
          m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
          m_StaticData = this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabEffectData = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
          m_EffectOwners = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup,
          m_PrefabEffects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
          m_CullingData = this.m_PreCullingSystem.GetUpdatedData(true, out dependencies3),
          m_EnabledEffectData = this.m_EnabledData,
          m_ActionQueue = nativeParallelQueue.AsWriter()
        };
        JobHandle dependsOn = jobData1.Schedule<EffectControlSystem.TreeCullingJob1>(dependencies2);
        JobHandle jobHandle2 = jobData2.Schedule<EffectControlSystem.TreeCullingJob2>(nativeArray1.Length, 1, dependsOn);
        // ISSUE: reference to a compiler-generated field
        JobHandle job2 = jobData3.Schedule<EffectControlSystem.EffectCullingJob, PreCullingData>(jobData3.m_CullingData, 16, JobHandle.CombineDependencies(this.Dependency, dependencies3));
        nativeArray1.Dispose(jobHandle2);
        nativeArray2.Dispose(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SearchSystem.AddSearchTreeReader(jobHandle2);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle2, job2);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RandomTransformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VFXData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EffectControlSystem.EnabledActionJob jobData4 = new EffectControlSystem.EnabledActionJob()
      {
        m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_VFXDatas = this.__TypeHandle.__Game_Prefabs_VFXData_RO_ComponentLookup,
        m_RandomTransformDatas = this.__TypeHandle.__Game_Prefabs_RandomTransformData_RO_ComponentLookup,
        m_ObjectGeometryDatas = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_AudioSourceDatas = this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup,
        m_PrefabEffects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
        m_EffectOwners = this.__TypeHandle.__Game_Effects_EnabledEffect_RW_BufferLookup,
        m_CullingActions = nativeParallelQueue.AsReader(),
        m_OverflowActions = nativeQueue.AsParallelWriter(),
        m_VFXUpdateQueue = this.m_VFXSystem.GetSourceUpdateData().AsParallelWriter(),
        m_EnabledData = this.m_EnabledData,
        m_EnabledDataIndex = nativeReference,
        m_EffectControlData = this.m_EffectControlData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EffectControlSystem.ResizeEnabledDataJob jobData5 = new EffectControlSystem.ResizeEnabledDataJob()
      {
        m_EnabledDataIndex = nativeReference,
        m_EnabledData = this.m_EnabledData,
        m_OverflowActions = nativeQueue
      };
      JobHandle jobHandle3 = jobData4.Schedule<EffectControlSystem.EnabledActionJob>(nativeParallelQueue.HashRange, 1, jobHandle1);
      JobHandle dependsOn1 = jobHandle3;
      JobHandle inputDeps = jobData5.Schedule<EffectControlSystem.ResizeEnabledDataJob>(dependsOn1);
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledWriteDependencies = inputDeps;
      nativeParallelQueue.Dispose(jobHandle3);
      nativeQueue.Dispose(inputDeps);
      nativeReference.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_VFXSystem.AddSourceUpdateWriter(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_PrevCameraPosition = float3_1;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevCameraDirection = float3_2;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevLodParameters = float4;
      // ISSUE: reference to a compiler-generated field
      this.m_ResetPrevious = false;
      this.Dependency = jobHandle3;
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
    public EffectControlSystem()
    {
    }

    [BurstCompile]
    private struct EffectControlJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.EditorContainer> m_EditorContainerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Object> m_ObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Static> m_StaticType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Events.Event> m_EventType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<EnabledEffect> m_EffectOwnerType;
      [ReadOnly]
      public ComponentLookup<EffectData> m_PrefabEffectData;
      [ReadOnly]
      public ComponentLookup<LightEffectData> m_PrefabLightEffectData;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> m_PrefabAudioEffectData;
      [ReadOnly]
      public BufferLookup<Effect> m_PrefabEffects;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public NativeList<EnabledEffectData> m_EnabledEffectData;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<EnabledEffect> bufferAccessor = chunk.GetBufferAccessor<EnabledEffect>(ref this.m_EffectOwnerType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity entity = nativeArray1[index1];
            DynamicBuffer<EnabledEffect> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              EnabledEffect enabledEffect = dynamicBuffer[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_ActionQueue.Enqueue(new EffectControlSystem.EnabledAction()
              {
                m_Owner = entity,
                m_EffectIndex = enabledEffect.m_EffectIndex,
                m_Flags = EffectControlSystem.ActionFlags.Deleted
              });
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag1 = !chunk.Has<Temp>(ref this.m_TempType) && (chunk.Has<Static>(ref this.m_StaticType) || !chunk.Has<Game.Objects.Object>(ref this.m_ObjectType) && !chunk.Has<Game.Events.Event>(ref this.m_EventType));
          // ISSUE: reference to a compiler-generated field
          NativeArray<CullingInfo> nativeArray2 = chunk.GetNativeArray<CullingInfo>(ref this.m_CullingInfoType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Tools.EditorContainer> nativeArray3 = chunk.GetNativeArray<Game.Tools.EditorContainer>(ref this.m_EditorContainerType);
          NativeArray<Transform> transforms = new NativeArray<Transform>();
          NativeArray<Curve> curves = new NativeArray<Curve>();
          NativeArray<PrefabRef> nativeArray4 = new NativeArray<PrefabRef>();
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            transforms = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            curves = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          }
          if (nativeArray3.Length == 0)
          {
            // ISSUE: reference to a compiler-generated field
            nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          }
          for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
          {
            Entity entity = nativeArray1[index3];
            DynamicBuffer<EnabledEffect> dynamicBuffer = bufferAccessor[index3];
            Game.Tools.EditorContainer editorContainer;
            // ISSUE: variable of a compiler-generated type
            EffectControlSystem.EnabledAction enabledAction1;
            if (CollectionUtils.TryGet<Game.Tools.EditorContainer>(nativeArray3, index3, out editorContainer))
            {
              for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
              {
                EnabledEffect enabledEffect = dynamicBuffer[index4];
                // ISSUE: reference to a compiler-generated field
                EnabledEffectData enabledEffectData = this.m_EnabledEffectData[enabledEffect.m_EnabledIndex];
                if (editorContainer.m_Prefab != enabledEffectData.m_Prefab)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer local = ref this.m_ActionQueue;
                  // ISSUE: object of a compiler-generated type is created
                  enabledAction1 = new EffectControlSystem.EnabledAction();
                  // ISSUE: reference to a compiler-generated field
                  enabledAction1.m_Owner = entity;
                  // ISSUE: reference to a compiler-generated field
                  enabledAction1.m_EffectIndex = enabledEffect.m_EffectIndex;
                  // ISSUE: reference to a compiler-generated field
                  enabledAction1.m_Flags = EffectControlSystem.ActionFlags.WrongPrefab;
                  // ISSUE: variable of a compiler-generated type
                  EffectControlSystem.EnabledAction enabledAction2 = enabledAction1;
                  local.Enqueue(enabledAction2);
                }
              }
              EffectData componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabEffectData.TryGetComponent(editorContainer.m_Prefab, out componentData))
              {
                // ISSUE: variable of a compiler-generated type
                EffectControlSystem.ActionFlags actionFlags = flag1 ? EffectControlSystem.ActionFlags.CheckEnabled | EffectControlSystem.ActionFlags.IsStatic | EffectControlSystem.ActionFlags.OwnerUpdated : EffectControlSystem.ActionFlags.CheckEnabled | EffectControlSystem.ActionFlags.OwnerUpdated;
                if (componentData.m_OwnerCulling)
                {
                  // ISSUE: reference to a compiler-generated method
                  if (!this.IsNearCamera(nativeArray2, index3))
                    continue;
                }
                else if (flag1)
                {
                  Effect effect = new Effect()
                  {
                    m_Effect = editorContainer.m_Prefab
                  };
                  // ISSUE: reference to a compiler-generated method
                  if (!this.IsNearCamera(transforms, curves, index3, effect))
                    actionFlags = (EffectControlSystem.ActionFlags) 0;
                }
                // ISSUE: reference to a compiler-generated field
                ref NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer local = ref this.m_ActionQueue;
                // ISSUE: object of a compiler-generated type is created
                enabledAction1 = new EffectControlSystem.EnabledAction();
                // ISSUE: reference to a compiler-generated field
                enabledAction1.m_Owner = entity;
                // ISSUE: reference to a compiler-generated field
                enabledAction1.m_EffectIndex = 0;
                // ISSUE: reference to a compiler-generated field
                enabledAction1.m_Flags = actionFlags;
                // ISSUE: variable of a compiler-generated type
                EffectControlSystem.EnabledAction enabledAction3 = enabledAction1;
                local.Enqueue(enabledAction3);
              }
            }
            else
            {
              DynamicBuffer<Effect> bufferData;
              // ISSUE: reference to a compiler-generated field
              this.m_PrefabEffects.TryGetBuffer(nativeArray4[index3].m_Prefab, out bufferData);
              for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
              {
                EnabledEffect enabledEffect = dynamicBuffer[index5];
                // ISSUE: reference to a compiler-generated field
                EnabledEffectData enabledEffectData = this.m_EnabledEffectData[enabledEffect.m_EnabledIndex];
                if (!bufferData.IsCreated || bufferData.Length <= enabledEffect.m_EffectIndex || bufferData[enabledEffect.m_EffectIndex].m_Effect != enabledEffectData.m_Prefab)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer local = ref this.m_ActionQueue;
                  // ISSUE: object of a compiler-generated type is created
                  enabledAction1 = new EffectControlSystem.EnabledAction();
                  // ISSUE: reference to a compiler-generated field
                  enabledAction1.m_Owner = entity;
                  // ISSUE: reference to a compiler-generated field
                  enabledAction1.m_EffectIndex = enabledEffect.m_EffectIndex;
                  // ISSUE: reference to a compiler-generated field
                  enabledAction1.m_Flags = EffectControlSystem.ActionFlags.WrongPrefab;
                  // ISSUE: variable of a compiler-generated type
                  EffectControlSystem.EnabledAction enabledAction4 = enabledAction1;
                  local.Enqueue(enabledAction4);
                }
              }
              if (bufferData.IsCreated)
              {
                // ISSUE: reference to a compiler-generated method
                bool flag2 = this.IsNearCamera(nativeArray2, index3);
                for (int index6 = 0; index6 < bufferData.Length; ++index6)
                {
                  Effect effect = bufferData[index6];
                  EffectData componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabEffectData.TryGetComponent(effect.m_Effect, out componentData))
                  {
                    // ISSUE: variable of a compiler-generated type
                    EffectControlSystem.ActionFlags actionFlags = flag1 ? EffectControlSystem.ActionFlags.CheckEnabled | EffectControlSystem.ActionFlags.IsStatic | EffectControlSystem.ActionFlags.OwnerUpdated : EffectControlSystem.ActionFlags.CheckEnabled | EffectControlSystem.ActionFlags.OwnerUpdated;
                    if (componentData.m_OwnerCulling)
                    {
                      // ISSUE: reference to a compiler-generated field
                      bool flag3 = this.m_PrefabAudioEffectData.HasComponent(effect.m_Effect);
                      if (!flag2 && !flag3)
                        continue;
                    }
                    // ISSUE: reference to a compiler-generated method
                    if (flag1 && !this.IsNearCamera(transforms, curves, index3, effect))
                      actionFlags = (EffectControlSystem.ActionFlags) 0;
                    // ISSUE: reference to a compiler-generated field
                    ref NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer local = ref this.m_ActionQueue;
                    // ISSUE: object of a compiler-generated type is created
                    enabledAction1 = new EffectControlSystem.EnabledAction();
                    // ISSUE: reference to a compiler-generated field
                    enabledAction1.m_Owner = entity;
                    // ISSUE: reference to a compiler-generated field
                    enabledAction1.m_EffectIndex = index6;
                    // ISSUE: reference to a compiler-generated field
                    enabledAction1.m_Flags = actionFlags;
                    // ISSUE: variable of a compiler-generated type
                    EffectControlSystem.EnabledAction enabledAction5 = enabledAction1;
                    local.Enqueue(enabledAction5);
                  }
                }
              }
            }
          }
        }
      }

      private bool IsNearCamera(
        NativeArray<Transform> transforms,
        NativeArray<Curve> curves,
        int index,
        Effect effect)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        QuadTreeBoundsXZ bounds = SearchSystem.GetBounds(transforms, curves, index, effect, ref this.m_PrefabLightEffectData, ref this.m_PrefabAudioEffectData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        double minDistance = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
        // ISSUE: reference to a compiler-generated field
        return RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) >= (int) bounds.m_MinLod;
      }

      private bool IsNearCamera(NativeArray<CullingInfo> cullingInfos, int index)
      {
        CullingInfo cullingInfo;
        // ISSUE: reference to a compiler-generated field
        return CollectionUtils.TryGet<CullingInfo>(cullingInfos, index, out cullingInfo) && cullingInfo.m_CullingIndex != 0 && (this.m_CullingData[cullingInfo.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) > (PreCullingFlags) 0;
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
    private struct EffectCullingJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Static> m_StaticData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<EffectData> m_PrefabEffectData;
      [ReadOnly]
      public BufferLookup<EnabledEffect> m_EffectOwners;
      [ReadOnly]
      public BufferLookup<Effect> m_PrefabEffects;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public NativeList<EnabledEffectData> m_EnabledEffectData;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer m_ActionQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData preCullingData = this.m_CullingData[index];
        if ((preCullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Deleted | PreCullingFlags.Created | PreCullingFlags.EffectInstances)) != (PreCullingFlags.NearCameraUpdated | PreCullingFlags.EffectInstances))
          return;
        if ((preCullingData.m_Flags & PreCullingFlags.NearCamera) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[preCullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          bool flag = (preCullingData.m_Flags & PreCullingFlags.Temp) == (PreCullingFlags) 0 && (preCullingData.m_Flags & PreCullingFlags.Object) == (PreCullingFlags) 0 || this.m_StaticData.HasComponent(preCullingData.m_Entity);
          DynamicBuffer<Effect> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabEffects.TryGetBuffer(prefabRef.m_Prefab, out bufferData))
          {
            for (int index1 = 0; index1 < bufferData.Length; ++index1)
            {
              EffectData componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabEffectData.TryGetComponent(bufferData[index1].m_Effect, out componentData) && componentData.m_OwnerCulling)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_ActionQueue.Enqueue(new EffectControlSystem.EnabledAction()
                {
                  m_Owner = preCullingData.m_Entity,
                  m_EffectIndex = index1,
                  m_Flags = flag ? EffectControlSystem.ActionFlags.CheckEnabled | EffectControlSystem.ActionFlags.IsStatic : EffectControlSystem.ActionFlags.CheckEnabled
                });
              }
            }
          }
          else
          {
            Game.Tools.EditorContainer componentData1;
            EffectData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_EditorContainerData.TryGetComponent(preCullingData.m_Entity, out componentData1) || !this.m_PrefabEffectData.TryGetComponent(componentData1.m_Prefab, out componentData2) || !componentData2.m_OwnerCulling)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new EffectControlSystem.EnabledAction()
            {
              m_Owner = preCullingData.m_Entity,
              m_EffectIndex = 0,
              m_Flags = flag ? EffectControlSystem.ActionFlags.CheckEnabled | EffectControlSystem.ActionFlags.IsStatic : EffectControlSystem.ActionFlags.CheckEnabled
            });
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<EnabledEffect> effectOwner = this.m_EffectOwners[preCullingData.m_Entity];
          for (int index2 = 0; index2 < effectOwner.Length; ++index2)
          {
            EnabledEffect enabledEffect = effectOwner[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabEffectData[this.m_EnabledEffectData[enabledEffect.m_EnabledIndex].m_Prefab].m_OwnerCulling)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_ActionQueue.Enqueue(new EffectControlSystem.EnabledAction()
              {
                m_Owner = preCullingData.m_Entity,
                m_EffectIndex = enabledEffect.m_EffectIndex,
                m_Flags = (EffectControlSystem.ActionFlags) 0
              });
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct TreeCullingJob1 : IJob
    {
      [ReadOnly]
      public NativeQuadTree<SourceInfo, QuadTreeBoundsXZ> m_EffectSearchTree;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float4 m_PrevLodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_PrevCameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public float3 m_PrevCameraDirection;
      [NativeDisableParallelForRestriction]
      public NativeArray<int> m_NodeBuffer;
      [NativeDisableParallelForRestriction]
      public NativeArray<int> m_SubDataBuffer;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer m_ActionQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EffectControlSystem.TreeCullingIterator iterator = new EffectControlSystem.TreeCullingIterator()
        {
          m_LodParameters = this.m_LodParameters,
          m_PrevLodParameters = this.m_PrevLodParameters,
          m_CameraPosition = this.m_CameraPosition,
          m_PrevCameraPosition = this.m_PrevCameraPosition,
          m_CameraDirection = this.m_CameraDirection,
          m_PrevCameraDirection = this.m_PrevCameraDirection,
          m_ActionQueue = this.m_ActionQueue
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EffectSearchTree.Iterate<EffectControlSystem.TreeCullingIterator, int>(ref iterator, 3, this.m_NodeBuffer, this.m_SubDataBuffer);
      }
    }

    [BurstCompile]
    private struct TreeCullingJob2 : IJobParallelFor
    {
      [ReadOnly]
      public NativeQuadTree<SourceInfo, QuadTreeBoundsXZ> m_EffectSearchTree;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float4 m_PrevLodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_PrevCameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public float3 m_PrevCameraDirection;
      [ReadOnly]
      public NativeArray<int> m_NodeBuffer;
      [ReadOnly]
      public NativeArray<int> m_SubDataBuffer;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer m_ActionQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EffectControlSystem.TreeCullingIterator iterator = new EffectControlSystem.TreeCullingIterator()
        {
          m_LodParameters = this.m_LodParameters,
          m_PrevLodParameters = this.m_PrevLodParameters,
          m_CameraPosition = this.m_CameraPosition,
          m_PrevCameraPosition = this.m_PrevCameraPosition,
          m_CameraDirection = this.m_CameraDirection,
          m_PrevCameraDirection = this.m_PrevCameraDirection,
          m_ActionQueue = this.m_ActionQueue
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EffectSearchTree.Iterate<EffectControlSystem.TreeCullingIterator, int>(ref iterator, this.m_SubDataBuffer[index], this.m_NodeBuffer[index]);
      }
    }

    private struct TreeCullingIterator : 
      INativeQuadTreeIteratorWithSubData<SourceInfo, QuadTreeBoundsXZ, int>,
      IUnsafeQuadTreeIteratorWithSubData<SourceInfo, QuadTreeBoundsXZ, int>
    {
      public float4 m_LodParameters;
      public float3 m_CameraPosition;
      public float3 m_CameraDirection;
      public float3 m_PrevCameraPosition;
      public float4 m_PrevLodParameters;
      public float3 m_PrevCameraDirection;
      public NativeParallelQueue<EffectControlSystem.EnabledAction>.Writer m_ActionQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds, ref int subData)
      {
        switch (subData)
        {
          case 1:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance1 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod1 = RenderingUtils.CalculateLod((float) (minDistance1 * minDistance1), this.m_LodParameters);
            if (lod1 < (int) bounds.m_MinLod)
              return false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double maxDistance1 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod2 = RenderingUtils.CalculateLod((float) (maxDistance1 * maxDistance1), this.m_PrevLodParameters);
            return lod2 < (int) bounds.m_MaxLod && lod1 > lod2;
          case 2:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance2 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod3 = RenderingUtils.CalculateLod((float) (minDistance2 * minDistance2), this.m_PrevLodParameters);
            if (lod3 < (int) bounds.m_MinLod)
              return false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double maxDistance2 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod4 = RenderingUtils.CalculateLod((float) (maxDistance2 * maxDistance2), this.m_LodParameters);
            return lod4 < (int) bounds.m_MaxLod && lod3 > lod4;
          default:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float minDistance3 = RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance4 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod5 = RenderingUtils.CalculateLod(minDistance3 * minDistance3, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod6 = RenderingUtils.CalculateLod((float) (minDistance4 * minDistance4), this.m_PrevLodParameters);
            subData = 0;
            if (lod5 >= (int) bounds.m_MinLod)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double maxDistance3 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
              // ISSUE: reference to a compiler-generated field
              int lod7 = RenderingUtils.CalculateLod((float) (maxDistance3 * maxDistance3), this.m_PrevLodParameters);
              subData |= math.select(0, 1, lod7 < (int) bounds.m_MaxLod && lod5 > lod7);
            }
            if (lod6 >= (int) bounds.m_MinLod)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double maxDistance4 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
              // ISSUE: reference to a compiler-generated field
              int lod8 = RenderingUtils.CalculateLod((float) (maxDistance4 * maxDistance4), this.m_LodParameters);
              subData |= math.select(0, 2, lod8 < (int) bounds.m_MaxLod && lod6 > lod8);
            }
            return subData != 0;
        }
      }

      public void Iterate(QuadTreeBoundsXZ bounds, int subData, SourceInfo sourceInfo)
      {
        switch (subData)
        {
          case 1:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance1 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            if (RenderingUtils.CalculateLod((float) (minDistance1 * minDistance1), this.m_LodParameters) < (int) bounds.m_MinLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance2 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            if (RenderingUtils.CalculateLod((float) (minDistance2 * minDistance2), this.m_PrevLodParameters) >= (int) bounds.m_MaxLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new EffectControlSystem.EnabledAction()
            {
              m_Owner = sourceInfo.m_Entity,
              m_EffectIndex = sourceInfo.m_EffectIndex,
              m_Flags = EffectControlSystem.ActionFlags.SkipEnabled | EffectControlSystem.ActionFlags.IsStatic
            });
            break;
          case 2:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance3 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            if (RenderingUtils.CalculateLod((float) (minDistance3 * minDistance3), this.m_PrevLodParameters) < (int) bounds.m_MinLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance4 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            if (RenderingUtils.CalculateLod((float) (minDistance4 * minDistance4), this.m_LodParameters) >= (int) bounds.m_MaxLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new EffectControlSystem.EnabledAction()
            {
              m_Owner = sourceInfo.m_Entity,
              m_EffectIndex = sourceInfo.m_EffectIndex,
              m_Flags = (EffectControlSystem.ActionFlags) 0
            });
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float minDistance5 = RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance6 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod1 = RenderingUtils.CalculateLod(minDistance5 * minDistance5, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod2 = RenderingUtils.CalculateLod((float) (minDistance6 * minDistance6), this.m_PrevLodParameters);
            bool flag1 = lod1 >= (int) bounds.m_MinLod;
            int maxLod = (int) bounds.m_MaxLod;
            bool flag2 = lod2 >= maxLod;
            if (flag1 == flag2)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new EffectControlSystem.EnabledAction()
            {
              m_Owner = sourceInfo.m_Entity,
              m_EffectIndex = sourceInfo.m_EffectIndex,
              m_Flags = flag1 ? EffectControlSystem.ActionFlags.SkipEnabled | EffectControlSystem.ActionFlags.IsStatic : (EffectControlSystem.ActionFlags) 0
            });
            break;
        }
      }
    }

    [Flags]
    public enum ActionFlags : byte
    {
      CheckEnabled = 1,
      Deleted = 2,
      SkipEnabled = 4,
      IsStatic = 8,
      OwnerUpdated = 16, // 0x10
      WrongPrefab = 32, // 0x20
    }

    private struct EnabledAction
    {
      public Entity m_Owner;
      public int m_EffectIndex;
      public EffectControlSystem.ActionFlags m_Flags;

      public override int GetHashCode() => this.m_Owner.GetHashCode();
    }

    private struct OverflowAction
    {
      public Entity m_Owner;
      public Entity m_Prefab;
      public int m_DataIndex;
      public int m_EffectIndex;
      public EnabledEffectFlags m_Flags;
    }

    [BurstCompile]
    private struct EnabledActionJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<VFXData> m_VFXDatas;
      [ReadOnly]
      public ComponentLookup<RandomTransformData> m_RandomTransformDatas;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryDatas;
      [ReadOnly]
      public BufferLookup<AudioSourceData> m_AudioSourceDatas;
      [ReadOnly]
      public BufferLookup<Effect> m_PrefabEffects;
      [NativeDisableParallelForRestriction]
      public BufferLookup<EnabledEffect> m_EffectOwners;
      [ReadOnly]
      public NativeParallelQueue<EffectControlSystem.EnabledAction>.Reader m_CullingActions;
      public NativeQueue<EffectControlSystem.OverflowAction>.ParallelWriter m_OverflowActions;
      public NativeQueue<VFXUpdateInfo>.ParallelWriter m_VFXUpdateQueue;
      [NativeDisableParallelForRestriction]
      public NativeList<EnabledEffectData> m_EnabledData;
      [NativeDisableParallelForRestriction]
      public NativeReference<int> m_EnabledDataIndex;
      public EffectControlData m_EffectControlData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelQueue<EffectControlSystem.EnabledAction>.Enumerator enumerator = this.m_CullingActions.GetEnumerator(index);
        while (enumerator.MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          EffectControlSystem.EnabledAction current = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          if ((current.m_Flags & (EffectControlSystem.ActionFlags.CheckEnabled | EffectControlSystem.ActionFlags.SkipEnabled)) != (EffectControlSystem.ActionFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefab = this.m_EffectControlData.m_Prefabs[current.m_Owner];
            Entity entity = Entity.Null;
            bool isAnimated = false;
            bool isEditorContainer = false;
            DynamicBuffer<Effect> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabEffects.TryGetBuffer(prefab.m_Prefab, out bufferData))
            {
              // ISSUE: reference to a compiler-generated field
              Effect effect = bufferData[current.m_EffectIndex];
              entity = effect.m_Effect;
              isAnimated = effect.m_BoneIndex.x >= 0 || effect.m_AnimationIndex >= 0;
            }
            else
            {
              Game.Tools.EditorContainer componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorContainerData.TryGetComponent(current.m_Owner, out componentData))
              {
                entity = componentData.m_Prefab;
                isAnimated = componentData.m_GroupIndex >= 0;
                isEditorContainer = true;
              }
            }
            // ISSUE: reference to a compiler-generated field
            bool checkEnabled = (current.m_Flags & EffectControlSystem.ActionFlags.CheckEnabled) != 0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EffectControlData.ShouldBeEnabled(current.m_Owner, entity, checkEnabled, isEditorContainer))
            {
              // ISSUE: reference to a compiler-generated method
              this.Enable(current, entity, isAnimated, isEditorContainer);
              continue;
            }
          }
          // ISSUE: reference to a compiler-generated method
          this.Disable(current);
        }
        enumerator.Dispose();
      }

      private unsafe void Enable(
        EffectControlSystem.EnabledAction enabledAction,
        Entity effectPrefab,
        bool isAnimated,
        bool isEditorContainer)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<EnabledEffect> effectOwner = this.m_EffectOwners[enabledAction.m_Owner];
        for (int index = 0; index < effectOwner.Length; ++index)
        {
          ref EnabledEffect local1 = ref effectOwner.ElementAt(index);
          // ISSUE: reference to a compiler-generated field
          if (local1.m_EffectIndex == enabledAction.m_EffectIndex)
          {
            // ISSUE: reference to a compiler-generated field
            if (local1.m_EnabledIndex >= this.m_EnabledData.Length)
              return;
            // ISSUE: reference to a compiler-generated field
            ref EnabledEffectData local2 = ref UnsafeUtility.ArrayElementAsRef<EnabledEffectData>((void*) this.m_EnabledData.GetUnsafePtr<EnabledEffectData>(), local1.m_EnabledIndex);
            if (!(local2.m_Prefab != effectPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              if ((enabledAction.m_Flags & EffectControlSystem.ActionFlags.OwnerUpdated) != (EffectControlSystem.ActionFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((enabledAction.m_Flags & EffectControlSystem.ActionFlags.IsStatic) == (EffectControlSystem.ActionFlags) 0 | isAnimated || this.m_InterpolatedTransformData.HasComponent(enabledAction.m_Owner))
                  local2.m_Flags |= EnabledEffectFlags.DynamicTransform;
                else
                  local2.m_Flags &= ~EnabledEffectFlags.DynamicTransform;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.OwnerCollapsed(enabledAction.m_Owner))
                  local2.m_Flags |= EnabledEffectFlags.OwnerCollapsed;
                else
                  local2.m_Flags &= ~EnabledEffectFlags.OwnerCollapsed;
              }
              if ((local2.m_Flags & EnabledEffectFlags.IsEnabled) == (EnabledEffectFlags) 0)
              {
                local2.m_Flags |= EnabledEffectFlags.IsEnabled | EnabledEffectFlags.EnabledUpdated;
                if ((local2.m_Flags & EnabledEffectFlags.IsVFX) == (EnabledEffectFlags) 0)
                  return;
                // ISSUE: reference to a compiler-generated field
                this.m_VFXUpdateQueue.Enqueue(new VFXUpdateInfo()
                {
                  m_Type = VFXUpdateType.Add,
                  m_EnabledIndex = (int2) local1.m_EnabledIndex
                });
                return;
              }
              // ISSUE: reference to a compiler-generated field
              if ((enabledAction.m_Flags & EffectControlSystem.ActionFlags.OwnerUpdated) == (EffectControlSystem.ActionFlags) 0)
                return;
              local2.m_Flags |= EnabledEffectFlags.OwnerUpdated;
              return;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        int index1 = Interlocked.Increment(ref UnsafeUtility.AsRef<int>((void*) this.m_EnabledDataIndex.GetUnsafePtr<int>())) - 1;
        // ISSUE: reference to a compiler-generated field
        effectOwner.Add(new EnabledEffect()
        {
          m_EffectIndex = enabledAction.m_EffectIndex,
          m_EnabledIndex = index1
        });
        EnabledEffectFlags enabledEffectFlags = EnabledEffectFlags.IsEnabled | EnabledEffectFlags.EnabledUpdated;
        if (isEditorContainer)
          enabledEffectFlags |= EnabledEffectFlags.EditorContainer;
        // ISSUE: reference to a compiler-generated field
        if (this.m_EffectControlData.m_LightEffectDatas.HasComponent(effectPrefab))
          enabledEffectFlags |= EnabledEffectFlags.IsLight;
        // ISSUE: reference to a compiler-generated field
        if (this.m_VFXDatas.HasComponent(effectPrefab))
        {
          enabledEffectFlags |= EnabledEffectFlags.IsVFX;
          // ISSUE: reference to a compiler-generated field
          this.m_VFXUpdateQueue.Enqueue(new VFXUpdateInfo()
          {
            m_Type = VFXUpdateType.Add,
            m_EnabledIndex = (int2) index1
          });
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_AudioSourceDatas.HasBuffer(effectPrefab))
          enabledEffectFlags |= EnabledEffectFlags.IsAudio;
        // ISSUE: reference to a compiler-generated field
        if (this.m_RandomTransformDatas.HasComponent(effectPrefab))
          enabledEffectFlags |= EnabledEffectFlags.RandomTransform;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_EffectControlData.m_Temps.HasComponent(enabledAction.m_Owner))
          enabledEffectFlags |= EnabledEffectFlags.TempOwner;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((enabledAction.m_Flags & EffectControlSystem.ActionFlags.IsStatic) == (EffectControlSystem.ActionFlags) 0 | isAnimated || this.m_InterpolatedTransformData.HasComponent(enabledAction.m_Owner))
          enabledEffectFlags |= EnabledEffectFlags.DynamicTransform;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.OwnerCollapsed(enabledAction.m_Owner))
          enabledEffectFlags |= EnabledEffectFlags.OwnerCollapsed;
        // ISSUE: reference to a compiler-generated field
        if (index1 >= this.m_EnabledData.Capacity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_OverflowActions.Enqueue(new EffectControlSystem.OverflowAction()
          {
            m_Owner = enabledAction.m_Owner,
            m_Prefab = effectPrefab,
            m_DataIndex = index1,
            m_EffectIndex = enabledAction.m_EffectIndex,
            m_Flags = enabledEffectFlags
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          ref EnabledEffectData local = ref UnsafeUtility.ArrayElementAsRef<EnabledEffectData>((void*) this.m_EnabledData.GetUnsafePtr<EnabledEffectData>(), index1);
          *(EnabledEffectData*) ref local = new EnabledEffectData();
          // ISSUE: reference to a compiler-generated field
          local.m_Owner = enabledAction.m_Owner;
          local.m_Prefab = effectPrefab;
          // ISSUE: reference to a compiler-generated field
          local.m_EffectIndex = enabledAction.m_EffectIndex;
          local.m_Flags = enabledEffectFlags;
        }
      }

      private bool OwnerCollapsed(Entity owner)
      {
        PrefabRef componentData1;
        ObjectGeometryData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_DestroyedData.HasComponent(owner) && this.m_PrefabRefs.TryGetComponent(owner, out componentData1) && this.m_ObjectGeometryDatas.TryGetComponent(componentData1.m_Prefab, out componentData2) && (componentData2.m_Flags & (Game.Objects.GeometryFlags.Physical | Game.Objects.GeometryFlags.HasLot)) == (Game.Objects.GeometryFlags.Physical | Game.Objects.GeometryFlags.HasLot);
      }

      private unsafe void Disable(EffectControlSystem.EnabledAction enabledAction)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<EnabledEffect> effectOwner = this.m_EffectOwners[enabledAction.m_Owner];
        for (int index = 0; index < effectOwner.Length; ++index)
        {
          ref EnabledEffect local1 = ref effectOwner.ElementAt(index);
          // ISSUE: reference to a compiler-generated field
          if (local1.m_EffectIndex == enabledAction.m_EffectIndex)
          {
            // ISSUE: reference to a compiler-generated field
            if (local1.m_EnabledIndex >= this.m_EnabledData.Length)
              break;
            // ISSUE: reference to a compiler-generated field
            ref EnabledEffectData local2 = ref UnsafeUtility.ArrayElementAsRef<EnabledEffectData>((void*) this.m_EnabledData.GetUnsafePtr<EnabledEffectData>(), local1.m_EnabledIndex);
            if ((local2.m_Flags & EnabledEffectFlags.IsEnabled) != (EnabledEffectFlags) 0)
            {
              local2.m_Flags &= ~EnabledEffectFlags.IsEnabled;
              local2.m_Flags |= EnabledEffectFlags.EnabledUpdated;
              if ((local2.m_Flags & EnabledEffectFlags.IsVFX) != (EnabledEffectFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VFXUpdateQueue.Enqueue(new VFXUpdateInfo()
                {
                  m_Type = VFXUpdateType.Remove,
                  m_EnabledIndex = (int2) local1.m_EnabledIndex
                });
              }
            }
            // ISSUE: reference to a compiler-generated field
            if ((enabledAction.m_Flags & EffectControlSystem.ActionFlags.Deleted) != (EffectControlSystem.ActionFlags) 0)
              local2.m_Flags |= EnabledEffectFlags.Deleted;
            // ISSUE: reference to a compiler-generated field
            if ((enabledAction.m_Flags & EffectControlSystem.ActionFlags.WrongPrefab) == (EffectControlSystem.ActionFlags) 0)
              break;
            local2.m_Flags |= EnabledEffectFlags.WrongPrefab;
            break;
          }
        }
      }
    }

    [BurstCompile]
    private struct ResizeEnabledDataJob : IJob
    {
      [ReadOnly]
      public NativeReference<int> m_EnabledDataIndex;
      public NativeList<EnabledEffectData> m_EnabledData;
      public NativeQueue<EffectControlSystem.OverflowAction> m_OverflowActions;

      public unsafe void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EnabledData.Resize(math.min(this.m_EnabledDataIndex.Value, this.m_EnabledData.Capacity), NativeArrayOptions.UninitializedMemory);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EnabledData.Resize(this.m_EnabledDataIndex.Value, NativeArrayOptions.UninitializedMemory);
        // ISSUE: variable of a compiler-generated type
        EffectControlSystem.OverflowAction overflowAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OverflowActions.TryDequeue(out overflowAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref EnabledEffectData local = ref this.m_EnabledData.ElementAt(overflowAction.m_DataIndex);
          *(EnabledEffectData*) ref local = new EnabledEffectData();
          // ISSUE: reference to a compiler-generated field
          local.m_Owner = overflowAction.m_Owner;
          // ISSUE: reference to a compiler-generated field
          local.m_Prefab = overflowAction.m_Prefab;
          // ISSUE: reference to a compiler-generated field
          local.m_EffectIndex = overflowAction.m_EffectIndex;
          // ISSUE: reference to a compiler-generated field
          local.m_Flags = overflowAction.m_Flags;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Object> __Game_Objects_Object_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Static> __Game_Objects_Static_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Events.Event> __Game_Events_Event_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<EnabledEffect> __Game_Effects_EnabledEffect_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<EffectData> __Game_Prefabs_EffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LightEffectData> __Game_Prefabs_LightEffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> __Game_Prefabs_AudioEffectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Effect> __Game_Prefabs_Effect_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Static> __Game_Objects_Static_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<EnabledEffect> __Game_Effects_EnabledEffect_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VFXData> __Game_Prefabs_VFXData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RandomTransformData> __Game_Prefabs_RandomTransformData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<AudioSourceData> __Game_Prefabs_AudioSourceData_RO_BufferLookup;
      public BufferLookup<EnabledEffect> __Game_Effects_EnabledEffect_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Object_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Event_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Events.Event>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RO_BufferTypeHandle = state.GetBufferTypeHandle<EnabledEffect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectData_RO_ComponentLookup = state.GetComponentLookup<EffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LightEffectData_RO_ComponentLookup = state.GetComponentLookup<LightEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioEffectData_RO_ComponentLookup = state.GetComponentLookup<AudioEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RO_BufferLookup = state.GetBufferLookup<Effect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentLookup = state.GetComponentLookup<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RO_BufferLookup = state.GetBufferLookup<EnabledEffect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VFXData_RO_ComponentLookup = state.GetComponentLookup<VFXData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RandomTransformData_RO_ComponentLookup = state.GetComponentLookup<RandomTransformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioSourceData_RO_BufferLookup = state.GetBufferLookup<AudioSourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RW_BufferLookup = state.GetBufferLookup<EnabledEffect>();
      }
    }
  }
}
