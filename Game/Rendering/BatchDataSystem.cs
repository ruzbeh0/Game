// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchDataSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Rendering;
using Game.Buildings;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using Game.Zones;
using System.Runtime.CompilerServices;
using System.Threading;
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
  public class BatchDataSystem : GameSystemBase, IPostDeserialize
  {
    public const float LOD_FADE_DURATION = 0.25f;
    public const float DEBUG_FADE_DURATION = 2.5f;
    private RenderingSystem m_RenderingSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private BatchManagerSystem m_BatchManagerSystem;
    private BatchMeshSystem m_BatchMeshSystem;
    private ManagedBatchSystem m_ManagedBatchSystem;
    private PreCullingSystem m_PreCullingSystem;
    private LightingSystem m_LightingSystem;
    private MeshColorSystem m_MeshColorSystem;
    private SimulationSystem m_SimulationSystem;
    private CitizenPresenceSystem m_CitizenPresenceSystem;
    private TreeGrowthSystem m_TreeGrowthSystem;
    private WetnessSystem m_WetnessSystem;
    private WindSystem m_WindSystem;
    private DirtynessSystem m_DirtynessSystem;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_RenderingSettingsQuery;
    private NativeAccumulator<BatchDataSystem.SmoothingNeeded> m_SmoothingNeeded;
    private int m_SHCoefficients;
    private int m_LodParameters;
    private bool m_UpdateAll;
    private float m_LastLightFactor;
    private float m_LodFadeTimer;
    private float4 m_LastBuildingStateOverride;
    private uint m_LastCitizenPresenceVersion;
    private uint m_LastTreeGrowthVersion;
    private uint m_LastWetnessVersion;
    private uint m_LastDirtynessVersion;
    private uint m_LastFireDamageVersion;
    private uint m_LastWaterDamageVersion;
    private uint m_LastWeatherDamageVersion;
    private uint m_LastLaneConditionFrame;
    private uint m_LastDamagedFrame;
    private BatchDataSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchMeshSystem = this.World.GetOrCreateSystemManaged<BatchMeshSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatchSystem = this.World.GetOrCreateSystemManaged<ManagedBatchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LightingSystem = this.World.GetOrCreateSystemManaged<LightingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MeshColorSystem = this.World.GetOrCreateSystemManaged<MeshColorSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenPresenceSystem = this.World.GetOrCreateSystemManaged<CitizenPresenceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TreeGrowthSystem = this.World.GetOrCreateSystemManaged<TreeGrowthSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WetnessSystem = this.World.GetOrCreateSystemManaged<WetnessSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DirtynessSystem = this.World.GetOrCreateSystemManaged<DirtynessSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<RenderingSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_SmoothingNeeded = new NativeAccumulator<BatchDataSystem.SmoothingNeeded>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SHCoefficients = Shader.PropertyToID("unity_SHCoefficients");
      // ISSUE: reference to a compiler-generated field
      this.m_LodParameters = Shader.PropertyToID("colossal_LodParameters");
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SmoothingNeeded.Dispose();
      base.OnDestroy();
    }

    public void InstancePropertiesUpdated() => this.m_UpdateAll = true;

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastLaneConditionFrame = this.m_SimulationSystem.frameIndex;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastDamagedFrame = this.m_SimulationSystem.frameIndex;
    }

    public float GetLevelOfDetail(float levelOfDetail, IGameCameraController cameraController)
    {
      if (cameraController != null)
        levelOfDetail *= (float) (1.0 - 1.0 / (2.0 + 0.0099999997764825821 * (double) cameraController.zoom));
      return levelOfDetail;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      float4 float4 = (float4) 1f;
      float3 float3_1 = (float3) 0.0f;
      float3 float3_2 = (float3) 0.0f;
      float num1 = 1f;
      LODParameters lodParameters;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_CameraUpdateSystem.TryGetLODParameters(out lodParameters))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float4 = RenderingUtils.CalculateLodParameters(this.GetLevelOfDetail(this.m_RenderingSystem.frameLod, this.m_CameraUpdateSystem.activeCameraController), lodParameters);
        float3_1 = (float3) lodParameters.cameraPosition;
        // ISSUE: reference to a compiler-generated field
        float3_2 = this.m_CameraUpdateSystem.activeViewer.forward;
        num1 = (float) lodParameters.cameraPixelHeight / math.radians(lodParameters.fieldOfView);
      }
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVector(this.m_LodParameters, new Vector4(float4.x, float4.y, 0.0f, 0.0f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      bool flag = this.m_BatchManagerSystem.IsLodFadeEnabled();
      int num2 = 0;
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LodFadeTimer += UnityEngine.Time.deltaTime * (this.m_RenderingSystem.debugCrossFade ? 102f : 1020f);
        // ISSUE: reference to a compiler-generated field
        int x = Mathf.FloorToInt(this.m_LodFadeTimer);
        // ISSUE: reference to a compiler-generated field
        this.m_LodFadeTimer -= (float) x;
        num2 = math.clamp(x, 0, (int) byte.MaxValue);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchMeshSystem.UpdateMeshes();
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(false, out dependencies2);
      PreCullingFlags cullingFlags;
      // ISSUE: variable of a compiler-generated type
      BatchDataSystem.UpdateMasks updateMasks;
      // ISSUE: reference to a compiler-generated method
      this.GetDataQuery(out cullingFlags, out updateMasks);
      dependencies2.Complete();
      // ISSUE: reference to a compiler-generated method
      this.UpdateGlobalValues(nativeBatchInstances);
      int activeGroupCount = nativeBatchInstances.GetActiveGroupCount();
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.CullingWriter cullingWriter = nativeBatchInstances.BeginCulling(Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubFlow_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_HangingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CitizenPresence_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Emissive_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Animated_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_FadeBatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshBatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Override_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Warning_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Error_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies3;
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      BatchDataSystem.BatchDataJob jobData1 = new BatchDataSystem.BatchDataJob()
      {
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ErrorData = this.__TypeHandle.__Game_Tools_Error_RO_ComponentLookup,
        m_WarningData = this.__TypeHandle.__Game_Tools_Warning_RO_ComponentLookup,
        m_OverrideData = this.__TypeHandle.__Game_Tools_Override_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_MeshBatches = this.__TypeHandle.__Game_Rendering_MeshBatch_RO_BufferLookup,
        m_FadeBatches = this.__TypeHandle.__Game_Rendering_FadeBatch_RO_BufferLookup,
        m_MeshColors = this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup,
        m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
        m_Animateds = this.__TypeHandle.__Game_Rendering_Animated_RO_BufferLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup,
        m_Emissives = this.__TypeHandle.__Game_Rendering_Emissive_RO_BufferLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_StackData = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup,
        m_ObjectTransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ObjectColorData = this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup,
        m_ObjectElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_ObjectSurfaceData = this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup,
        m_ObjectDamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_CitizenPresenceData = this.__TypeHandle.__Game_Buildings_CitizenPresence_RO_ComponentLookup,
        m_BuildingAbandonedData = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_NodeLaneData = this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_LaneColorData = this.__TypeHandle.__Game_Net_LaneColor_RO_ComponentLookup,
        m_LaneConditionData = this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup,
        m_HangingLaneData = this.__TypeHandle.__Game_Net_HangingLane_RO_ComponentLookup,
        m_NetEdgeColorData = this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup,
        m_NetNodeColorData = this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup,
        m_SubFlows = this.__TypeHandle.__Game_Net_SubFlow_RO_BufferLookup,
        m_CutRanges = this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup,
        m_ZoneBlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_ZoneCells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGrowthScaleData = this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabPublicTransportVehicleData = this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup,
        m_PrefabMeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabSubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_PrefabSubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_PrefabAnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_LightFactor = this.m_LastLightFactor,
        m_FrameDelta = this.m_RenderingSystem.frameDelta,
        m_SmoothnessDelta = this.m_RenderingSystem.frameDelta * (1f / 600f),
        m_BuildingStateOverride = this.m_LastBuildingStateOverride,
        m_CullingFlags = cullingFlags,
        m_UpdateMasks = updateMasks,
        m_NativeBatchGroups = nativeBatchGroups,
        m_CullingData = this.m_PreCullingSystem.GetCullingData(true, out dependencies3),
        m_WindData = this.m_WindSystem.GetData(true, out dependencies4),
        m_NativeBatchInstances = nativeBatchInstances.AsParallelInstanceWriter(),
        m_SmoothingNeeded = this.m_SmoothingNeeded.AsParallelWriter()
      };
      JobHandle dependencies5;
      JobHandle dependencies6;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchDataSystem.BatchLodJob jobData2 = new BatchDataSystem.BatchLodJob()
      {
        m_DisableLods = this.m_RenderingSystem.disableLodModels,
        m_UseLodFade = flag,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_PixelSizeFactor = num1,
        m_LodFadeDelta = num2,
        m_LoadingState = this.m_BatchMeshSystem.GetLoadingState(out dependencies5),
        m_NativeBatchGroups = nativeBatchGroups,
        m_NativeBatchInstances = cullingWriter.AsParallel(),
        m_BatchPriority = this.m_BatchMeshSystem.GetBatchPriority(out dependencies6)
      };
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RenderingSettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData1.m_RenderingSettingsData = this.m_RenderingSettingsQuery.GetSingleton<RenderingSettingsData>();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle requestMaxPixels = this.m_ManagedBatchSystem.GetVTRequestMaxPixels(out jobData2.m_VTRequestsMaxPixels0, out jobData2.m_VTRequestsMaxPixels1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.Schedule<BatchDataSystem.BatchDataJob, PreCullingData>(jobData1.m_CullingData, 16, JobUtils.CombineDependencies(this.Dependency, dependencies3, dependencies4, dependencies1));
      JobHandle jobHandle2 = jobData2.Schedule<BatchDataSystem.BatchLodJob>(activeGroupCount, 1, JobUtils.CombineDependencies(jobHandle1, requestMaxPixels, dependencies6, dependencies5));
      JobHandle jobHandle3 = nativeBatchInstances.EndCulling(cullingWriter, jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchGroupsReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchInstancesWriter(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchMeshSystem.AddBatchPriorityWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchMeshSystem.AddLoadingStateReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ManagedBatchSystem.AddVTRequestWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem.AddReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchMeshSystem.UpdateBatchPriorities();
      this.Dependency = jobHandle1;
    }

    private void GetDataQuery(
      out PreCullingFlags cullingFlags,
      out BatchDataSystem.UpdateMasks updateMasks)
    {
      cullingFlags = PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated | PreCullingFlags.FadeContainer | PreCullingFlags.InterpolatedTransform | PreCullingFlags.Animated | PreCullingFlags.ColorsUpdated;
      // ISSUE: object of a compiler-generated type is created
      updateMasks = new BatchDataSystem.UpdateMasks();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RenderingSystem.editorBuildingStateOverride.Equals(this.m_LastBuildingStateOverride))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastBuildingStateOverride = this.m_RenderingSystem.editorBuildingStateOverride;
        // ISSUE: reference to a compiler-generated field
        --this.m_LastCitizenPresenceVersion;
      }
      // ISSUE: reference to a compiler-generated field
      uint lastSystemVersion1 = this.m_CitizenPresenceSystem.LastSystemVersion;
      // ISSUE: reference to a compiler-generated field
      uint lastSystemVersion2 = this.m_TreeGrowthSystem.LastSystemVersion;
      // ISSUE: reference to a compiler-generated field
      uint lastSystemVersion3 = this.m_WetnessSystem.LastSystemVersion;
      // ISSUE: reference to a compiler-generated field
      uint lastSystemVersion4 = this.m_DirtynessSystem.LastSystemVersion;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_RenderingSystem.frameIndex >= this.m_LastLaneConditionFrame + 128U ? this.m_RenderingSystem.frameIndex : this.m_LastLaneConditionFrame;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint num2 = this.m_RenderingSystem.frameIndex >= this.m_LastDamagedFrame + 128U ? this.m_RenderingSystem.frameIndex : this.m_LastDamagedFrame;
      // ISSUE: reference to a compiler-generated method
      float lightFactor = this.CalculateLightFactor();
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdateAll)
      {
        cullingFlags |= PreCullingFlags.NearCamera | PreCullingFlags.InfoviewColor | PreCullingFlags.BuildingState | PreCullingFlags.TreeGrowth | PreCullingFlags.LaneCondition | PreCullingFlags.SurfaceState | PreCullingFlags.SurfaceDamage | PreCullingFlags.SmoothColor;
        // ISSUE: reference to a compiler-generated method
        updateMasks.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateAll = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        BatchDataSystem.SmoothingNeeded result = this.m_SmoothingNeeded.GetResult();
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_ToolSystem.activeInfoview != (UnityEngine.Object) null)
        {
          cullingFlags |= PreCullingFlags.InfoviewColor;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.InfoviewColor);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_NetMask.UpdateProperty(NetProperty.InfoviewColor);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_LaneMask.UpdateProperty(LaneProperty.InfoviewColor);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_LaneMask.UpdateProperty(LaneProperty.FlowMatrix);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) lastSystemVersion1 != (int) this.m_LastCitizenPresenceVersion || (double) lightFactor != (double) this.m_LastLightFactor)
        {
          cullingFlags |= PreCullingFlags.BuildingState;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.BuildingState);
        }
        // ISSUE: reference to a compiler-generated field
        if ((int) lastSystemVersion2 != (int) this.m_LastTreeGrowthVersion)
          cullingFlags |= PreCullingFlags.TreeGrowth;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((int) lastSystemVersion3 != (int) this.m_LastWetnessVersion || result.IsNeeded(BatchDataSystem.SmoothingType.SurfaceWetness))
        {
          cullingFlags |= PreCullingFlags.SurfaceState;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.SurfaceWetness);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((int) lastSystemVersion4 != (int) this.m_LastDirtynessVersion || result.IsNeeded(BatchDataSystem.SmoothingType.SurfaceDirtyness))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.SurfaceDamage);
        }
        // ISSUE: reference to a compiler-generated field
        if ((int) num1 != (int) this.m_LastLaneConditionFrame)
        {
          cullingFlags |= PreCullingFlags.LaneCondition;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_LaneMask.UpdateProperty(LaneProperty.CurveDeterioration);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((int) num2 != (int) this.m_LastDamagedFrame || result.IsNeeded(BatchDataSystem.SmoothingType.SurfaceDamage))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.SurfaceDamage);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_MeshColorSystem.smoothColorsUpdated || result.IsNeeded(BatchDataSystem.SmoothingType.ColorMask))
        {
          cullingFlags |= PreCullingFlags.SmoothColor;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.ColorMask1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.ColorMask2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_ObjectMask.UpdateProperty(ObjectProperty.ColorMask3);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_LaneMask.UpdateProperty(LaneProperty.ColorMask1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_LaneMask.UpdateProperty(LaneProperty.ColorMask2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          updateMasks.m_LaneMask.UpdateProperty(LaneProperty.ColorMask3);
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SmoothingNeeded.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LastCitizenPresenceVersion = lastSystemVersion1;
      // ISSUE: reference to a compiler-generated field
      this.m_LastTreeGrowthVersion = lastSystemVersion2;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWetnessVersion = lastSystemVersion3;
      // ISSUE: reference to a compiler-generated field
      this.m_LastDirtynessVersion = lastSystemVersion4;
      // ISSUE: reference to a compiler-generated field
      this.m_LastLightFactor = lightFactor;
      // ISSUE: reference to a compiler-generated field
      this.m_LastLaneConditionFrame = num1;
      // ISSUE: reference to a compiler-generated field
      this.m_LastDamagedFrame = num2;
    }

    private float CalculateLightFactor()
    {
      // ISSUE: reference to a compiler-generated field
      return math.saturate((float) (1.0 - (double) math.round(this.m_LightingSystem.dayLightBrightness * 100f) * 0.0099999997764825821));
    }

    private void UpdateGlobalValues(
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances)
    {
      SHCoefficients shCoefficients = new SHCoefficients(RenderSettings.ambientProbe);
      // ISSUE: reference to a compiler-generated field
      nativeBatchInstances.SetGlobalValue<SHCoefficients>(shCoefficients, this.m_SHCoefficients);
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
    public BatchDataSystem()
    {
    }

    private struct UpdateMask
    {
      private uint m_Mask;

      public void UpdateAll() => this.m_Mask = uint.MaxValue;

      public void UpdateTransform() => this.m_Mask |= 1U;

      public bool ShouldUpdateAll() => this.m_Mask == uint.MaxValue;

      public bool ShouldUpdateNothing() => this.m_Mask == 0U;

      public bool ShouldUpdateTransform() => (this.m_Mask & 1U) > 0U;

      public void UpdateProperty(ObjectProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mask |= 2U << (int) (property & (ObjectProperty) 31);
      }

      public bool ShouldUpdateProperty(ObjectProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        return (this.m_Mask & 2U << (int) (property & (ObjectProperty) 31)) > 0U;
      }

      public bool ShouldUpdateProperty(
        ObjectProperty property,
        in GroupData groupData,
        out int index)
      {
        index = -1;
        // ISSUE: reference to a compiler-generated field
        return ((int) this.m_Mask & 2 << (int) (property & (ObjectProperty) 31)) != 0 && groupData.GetPropertyIndex((int) property, out index);
      }

      public void UpdateProperty(NetProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mask |= 2U << (int) (property & (NetProperty.LodFade1 | NetProperty.Count));
      }

      public bool ShouldUpdateProperty(NetProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        return (this.m_Mask & 2U << (int) (property & (NetProperty.LodFade1 | NetProperty.Count))) > 0U;
      }

      public bool ShouldUpdateProperty(NetProperty property, in GroupData groupData, out int index)
      {
        index = -1;
        // ISSUE: reference to a compiler-generated field
        return ((int) this.m_Mask & 2 << (int) (property & (NetProperty.LodFade1 | NetProperty.Count))) != 0 && groupData.GetPropertyIndex((int) property, out index);
      }

      public void UpdateProperty(LaneProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mask |= 2U << (int) (property & (LaneProperty) 31);
      }

      public bool ShouldUpdateProperty(
        LaneProperty property,
        in GroupData groupData,
        out int index)
      {
        index = -1;
        // ISSUE: reference to a compiler-generated field
        return ((int) this.m_Mask & 2 << (int) (property & (LaneProperty) 31)) != 0 && groupData.GetPropertyIndex((int) property, out index);
      }

      public bool ShouldUpdateProperty(LaneProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        return (this.m_Mask & 2U << (int) (property & (LaneProperty) 31)) > 0U;
      }

      public void UpdateProperty(ZoneProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mask |= 2U << (int) (property & (ZoneProperty) 31);
      }

      public bool ShouldUpdateProperty(
        ZoneProperty property,
        in GroupData groupData,
        out int index)
      {
        index = -1;
        // ISSUE: reference to a compiler-generated field
        return ((int) this.m_Mask & 2 << (int) (property & (ZoneProperty) 31)) != 0 && groupData.GetPropertyIndex((int) property, out index);
      }

      public bool ShouldUpdateProperty(ZoneProperty property)
      {
        // ISSUE: reference to a compiler-generated field
        return (this.m_Mask & 2U << (int) (property & (ZoneProperty) 31)) > 0U;
      }
    }

    private struct UpdateMasks
    {
      public BatchDataSystem.UpdateMask m_ObjectMask;
      public BatchDataSystem.UpdateMask m_NetMask;
      public BatchDataSystem.UpdateMask m_LaneMask;
      public BatchDataSystem.UpdateMask m_ZoneMask;

      public void UpdateAll()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectMask.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetMask.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_LaneMask.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ZoneMask.UpdateAll();
      }

      public bool ShouldUpdateAll() => this.m_ObjectMask.ShouldUpdateAll();
    }

    private enum SmoothingType
    {
      SurfaceWetness,
      SurfaceDamage,
      SurfaceDirtyness,
      ColorMask,
    }

    private struct SmoothingNeeded : IAccumulable<BatchDataSystem.SmoothingNeeded>
    {
      private uint m_Value;

      public SmoothingNeeded(BatchDataSystem.SmoothingType type)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Value = 1U << (int) (type & (BatchDataSystem.SmoothingType) 31);
      }

      public bool IsNeeded(BatchDataSystem.SmoothingType type)
      {
        // ISSUE: reference to a compiler-generated field
        return (this.m_Value & 1U << (int) (type & (BatchDataSystem.SmoothingType) 31)) > 0U;
      }

      public void Accumulate(BatchDataSystem.SmoothingNeeded other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Value |= other.m_Value;
      }
    }

    private struct CellTypes
    {
      public float4x4 m_CellTypes0;
      public float4x4 m_CellTypes1;
      public float4x4 m_CellTypes2;
      public float4x4 m_CellTypes3;
    }

    [BurstCompile]
    private struct BatchDataJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Error> m_ErrorData;
      [ReadOnly]
      public ComponentLookup<Warning> m_WarningData;
      [ReadOnly]
      public ComponentLookup<Override> m_OverrideData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public BufferLookup<MeshBatch> m_MeshBatches;
      [ReadOnly]
      public BufferLookup<FadeBatch> m_FadeBatches;
      [ReadOnly]
      public BufferLookup<MeshColor> m_MeshColors;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public BufferLookup<Animated> m_Animateds;
      [ReadOnly]
      public BufferLookup<Skeleton> m_Skeletons;
      [ReadOnly]
      public BufferLookup<Emissive> m_Emissives;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Stack> m_StackData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_ObjectTransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> m_ObjectColorData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ObjectElevationData;
      [ReadOnly]
      public ComponentLookup<Surface> m_ObjectSurfaceData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_ObjectDamagedData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<CitizenPresence> m_CitizenPresenceData;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_BuildingAbandonedData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireData;
      [ReadOnly]
      public BufferLookup<Passenger> m_Passengers;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<NodeLane> m_NodeLaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<EdgeColor> m_NetEdgeColorData;
      [ReadOnly]
      public ComponentLookup<NodeColor> m_NetNodeColorData;
      [ReadOnly]
      public ComponentLookup<LaneColor> m_LaneColorData;
      [ReadOnly]
      public ComponentLookup<LaneCondition> m_LaneConditionData;
      [ReadOnly]
      public ComponentLookup<HangingLane> m_HangingLaneData;
      [ReadOnly]
      public BufferLookup<SubFlow> m_SubFlows;
      [ReadOnly]
      public BufferLookup<CutRange> m_CutRanges;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_ZoneBlockData;
      [ReadOnly]
      public BufferLookup<Game.Zones.Cell> m_ZoneCells;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> m_PrefabGrowthScaleData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> m_PrefabPublicTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_PrefabMeshData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public BufferLookup<SubMesh> m_PrefabSubMeshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_PrefabSubMeshGroups;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.AnimationClip> m_PrefabAnimationClips;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public float m_LightFactor;
      [ReadOnly]
      public float m_FrameDelta;
      [ReadOnly]
      public float4 m_BuildingStateOverride;
      [ReadOnly]
      public PreCullingFlags m_CullingFlags;
      [ReadOnly]
      public float m_SmoothnessDelta;
      [ReadOnly]
      public BatchDataSystem.UpdateMasks m_UpdateMasks;
      [ReadOnly]
      public RenderingSettingsData m_RenderingSettingsData;
      [ReadOnly]
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public CellMapData<Wind> m_WindData;
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.ParallelInstanceWriter m_NativeBatchInstances;
      public NativeAccumulator<BatchDataSystem.SmoothingNeeded>.ParallelWriter m_SmoothingNeeded;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData preCullingData = this.m_CullingData[index];
        // ISSUE: reference to a compiler-generated field
        if ((preCullingData.m_Flags & this.m_CullingFlags) == (PreCullingFlags) 0 || (preCullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        BatchDataSystem.UpdateMasks updateMasks = this.m_UpdateMasks;
        // ISSUE: reference to a compiler-generated method
        if (!updateMasks.ShouldUpdateAll() && (preCullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated)) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          updateMasks.UpdateAll();
        }
        if ((preCullingData.m_Flags & PreCullingFlags.Object) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateObjectData(preCullingData, updateMasks.m_ObjectMask);
        }
        else if ((preCullingData.m_Flags & PreCullingFlags.Net) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateNetData(preCullingData, updateMasks.m_NetMask);
        }
        else if ((preCullingData.m_Flags & PreCullingFlags.Lane) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateLaneData(preCullingData, updateMasks.m_LaneMask);
        }
        else if ((preCullingData.m_Flags & PreCullingFlags.Zone) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateZoneData(preCullingData, updateMasks.m_ZoneMask);
        }
        else
        {
          if ((preCullingData.m_Flags & PreCullingFlags.FadeContainer) == (PreCullingFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.UpdateFadeData(preCullingData);
        }
      }

      private void UpdateObjectData(
        PreCullingData preCullingData,
        BatchDataSystem.UpdateMask updateMask)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (!updateMask.ShouldUpdateTransform() && (this.m_CullingFlags & preCullingData.m_Flags & (PreCullingFlags.TreeGrowth | PreCullingFlags.InterpolatedTransform)) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateTransform();
        }
        // ISSUE: reference to a compiler-generated method
        if (!updateMask.ShouldUpdateProperty(ObjectProperty.AnimationCoordinate) && (preCullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateProperty(ObjectProperty.AnimationCoordinate);
        }
        // ISSUE: reference to a compiler-generated method
        if (!updateMask.ShouldUpdateProperty(ObjectProperty.ColorMask1) && (preCullingData.m_Flags & PreCullingFlags.ColorsUpdated) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateProperty(ObjectProperty.ColorMask1);
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateProperty(ObjectProperty.ColorMask2);
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateProperty(ObjectProperty.ColorMask3);
        }
        DynamicBuffer<MeshBatch> bufferData1;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (updateMask.ShouldUpdateNothing() || !this.m_MeshBatches.TryGetBuffer(preCullingData.m_Entity, out bufferData1))
          return;
        DynamicBuffer<MeshGroup> bufferData2;
        // ISSUE: reference to a compiler-generated field
        this.m_MeshGroups.TryGetBuffer(preCullingData.m_Entity, out bufferData2);
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          MeshBatch meshBatch1 = bufferData1[index1];
          // ISSUE: reference to a compiler-generated field
          GroupData groupData = this.m_NativeBatchGroups.GetGroupData(meshBatch1.m_GroupIndex);
          DynamicBuffer<MeshBatch> bufferData3;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (updateMask.ShouldUpdateAll() && (preCullingData.m_Flags & PreCullingFlags.Temp) != (PreCullingFlags) 0 && this.m_MeshBatches.TryGetBuffer(this.m_TempData[preCullingData.m_Entity].m_Original, out bufferData3))
          {
            for (int index2 = 0; index2 < bufferData3.Length; ++index2)
            {
              MeshBatch meshBatch2 = bufferData3[index2];
              if ((int) meshBatch2.m_MeshGroup == (int) meshBatch1.m_MeshGroup && (int) meshBatch2.m_MeshIndex == (int) meshBatch1.m_MeshIndex && (int) meshBatch2.m_TileIndex == (int) meshBatch1.m_TileIndex)
              {
                // ISSUE: reference to a compiler-generated field
                ref CullingData local = ref this.m_NativeBatchInstances.AccessCullingData(meshBatch2.m_GroupIndex, meshBatch2.m_InstanceIndex);
                // ISSUE: reference to a compiler-generated field
                this.m_NativeBatchInstances.AccessCullingData(meshBatch1.m_GroupIndex, meshBatch1.m_InstanceIndex).lodFade = local.lodFade;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NativeBatchInstances.InitializeTransform(meshBatch1.m_GroupIndex, meshBatch1.m_InstanceIndex, meshBatch2.m_GroupIndex, meshBatch2.m_InstanceIndex))
                  break;
              }
            }
          }
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateTransform())
          {
            // ISSUE: reference to a compiler-generated field
            CullingInfo cullingInfo = this.m_CullingInfoData[preCullingData.m_Entity];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[preCullingData.m_Entity];
            int meshIndex = (int) meshBatch1.m_MeshIndex;
            MeshGroup meshGroup;
            DynamicBuffer<SubMeshGroup> bufferData4;
            // ISSUE: reference to a compiler-generated field
            if (CollectionUtils.TryGet<MeshGroup>(bufferData2, (int) meshBatch1.m_MeshGroup, out meshGroup) && this.m_PrefabSubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData4))
              meshIndex += bufferData4[(int) meshGroup.m_SubMeshGroup].m_SubMeshRange.x;
            // ISSUE: reference to a compiler-generated field
            SubMesh subMesh = this.m_PrefabSubMeshes[prefabRef.m_Prefab][meshIndex];
            float3 subMeshScale = (float3) 1f;
            GrowthScaleData componentData1;
            // ISSUE: reference to a compiler-generated field
            if ((preCullingData.m_Flags & PreCullingFlags.TreeGrowth) != (PreCullingFlags) 0 && this.m_PrefabGrowthScaleData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
            {
              float3 scale;
              // ISSUE: reference to a compiler-generated field
              int treeSubMeshData = (int) BatchDataHelpers.CalculateTreeSubMeshData(this.m_TreeData[preCullingData.m_Entity], componentData1, out scale);
              subMeshScale *= scale;
            }
            Stack componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((subMesh.m_Flags & (SubMeshFlags.IsStackStart | SubMeshFlags.IsStackMiddle | SubMeshFlags.IsStackEnd)) != (SubMeshFlags) 0 && this.m_StackData.TryGetComponent(preCullingData.m_Entity, out componentData2) && this.m_PrefabStackData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              StackData stackData = this.m_PrefabStackData[prefabRef.m_Prefab];
              float3 offsets;
              float3 scale;
              int stackSubMeshData = (int) BatchDataHelpers.CalculateStackSubMeshData(componentData2, stackData, out int3 _, out offsets, out scale);
              BatchDataHelpers.CalculateStackSubMeshData(stackData, offsets, scale, (int) meshBatch1.m_TileIndex, subMesh.m_Flags, ref subMesh.m_Position, ref subMeshScale);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform = (preCullingData.m_Flags & PreCullingFlags.InterpolatedTransform) == (PreCullingFlags) 0 ? this.m_ObjectTransformData[preCullingData.m_Entity] : this.m_InterpolatedTransformData[preCullingData.m_Entity].ToTransform();
            float3 translation1;
            quaternion quaternion;
            if ((subMesh.m_Flags & (SubMeshFlags.IsStackStart | SubMeshFlags.IsStackMiddle | SubMeshFlags.IsStackEnd | SubMeshFlags.HasTransform)) != (SubMeshFlags) 0)
            {
              translation1 = transform.m_Position + math.rotate(transform.m_Rotation, subMesh.m_Position);
              quaternion = math.mul(transform.m_Rotation, subMesh.m_Rotation);
            }
            else
            {
              translation1 = transform.m_Position;
              quaternion = transform.m_Rotation;
            }
            int num = 0;
            float3x4 float3x4;
            float3x4 secondaryValue;
            if ((subMesh.m_Flags & SubMeshFlags.DefaultMissingMesh) != (SubMeshFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.MeshData meshData = this.m_PrefabMeshData[subMesh.m_SubMesh];
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
              if ((subMesh.m_Flags & SubMeshFlags.IsStackMiddle) != (SubMeshFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                switch (this.m_PrefabStackData[prefabRef.m_Prefab].m_Direction)
                {
                  case StackDirection.Right:
                    objectGeometryData.m_Bounds.x = new Bounds1(-1f, 1f);
                    break;
                  case StackDirection.Up:
                    objectGeometryData.m_Bounds.y = new Bounds1(-1f, 1f);
                    break;
                  case StackDirection.Forward:
                    objectGeometryData.m_Bounds.z = new Bounds1(-1f, 1f);
                    break;
                }
              }
              float3 translation2 = translation1 + math.rotate(quaternion, subMeshScale * MathUtils.Center(objectGeometryData.m_Bounds));
              float3 float3 = subMeshScale * MathUtils.Size(objectGeometryData.m_Bounds) * 0.5f;
              quaternion rotation = quaternion;
              float3 scale = float3;
              float3x4 = TransformHelper.TRS(translation2, rotation, scale);
              secondaryValue = float3x4;
              num = objectGeometryData.m_MinLod - (int) meshData.m_MinLod;
            }
            else
            {
              float3 translation3 = translation1 + math.rotate(quaternion, subMeshScale * groupData.m_SecondaryCenter);
              float3 float3 = subMeshScale * groupData.m_SecondarySize;
              float3x4 = TransformHelper.TRS(translation1, quaternion, subMeshScale);
              quaternion rotation = quaternion;
              float3 scale = float3;
              secondaryValue = TransformHelper.TRS(translation3, rotation, scale);
            }
            // ISSUE: reference to a compiler-generated field
            ref CullingData local = ref this.m_NativeBatchInstances.SetTransformValue(float3x4, secondaryValue, meshBatch1.m_GroupIndex, meshBatch1.m_InstanceIndex);
            local.m_Bounds = cullingInfo.m_Bounds;
            // ISSUE: reference to a compiler-generated field
            local.isHidden = this.m_HiddenData.HasComponent(preCullingData.m_Entity);
            local.lodOffset = num;
          }
          int index3;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.AnimationCoordinate, in groupData, out index3))
          {
            float3 float3 = float3.zero;
            if ((preCullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Animated> animated1 = this.m_Animateds[preCullingData.m_Entity];
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[preCullingData.m_Entity];
              int index4 = (int) meshBatch1.m_MeshIndex;
              int meshIndex = (int) meshBatch1.m_MeshIndex;
              MeshGroup meshGroup;
              DynamicBuffer<SubMeshGroup> bufferData5;
              // ISSUE: reference to a compiler-generated field
              if (CollectionUtils.TryGet<MeshGroup>(bufferData2, (int) meshBatch1.m_MeshGroup, out meshGroup) && this.m_PrefabSubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData5))
              {
                meshIndex += bufferData5[(int) meshGroup.m_SubMeshGroup].m_SubMeshRange.x;
                index4 = (int) meshBatch1.m_MeshGroup;
              }
              Animated animated2 = animated1[index4];
              if (animated2.m_ClipIndexBody0 != (short) -1)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float3 = BatchDataHelpers.GetAnimationCoordinate(this.m_PrefabAnimationClips[this.m_PrefabSubMeshes[prefabRef.m_Prefab][meshIndex].m_SubMesh][(int) animated2.m_ClipIndexBody0], animated2.m_Time.x, animated2.m_PreviousTime);
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float3>(float3, meshBatch1.m_GroupIndex, index3, meshBatch1.m_InstanceIndex);
          }
          int index5;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.InfoviewColor, in groupData, out index5))
          {
            float2 float2 = new float2();
            if ((preCullingData.m_Flags & PreCullingFlags.InfoviewColor) != (PreCullingFlags) 0)
            {
              Game.Objects.Color componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ObjectColorData.TryGetComponent(preCullingData.m_Entity, out componentData3))
              {
                float2 = new float2((float) componentData3.m_Index + 0.5f, (float) componentData3.m_Value * 0.003921569f);
              }
              else
              {
                Owner componentData4;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.TryGetComponent(preCullingData.m_Entity, out componentData4))
                {
                  Game.Objects.Elevation componentData5;
                  // ISSUE: reference to a compiler-generated field
                  bool flag = this.m_ObjectElevationData.TryGetComponent(preCullingData.m_Entity, out componentData5) && (componentData5.m_Flags & ElevationFlags.OnGround) == (ElevationFlags) 0;
                  Game.Objects.Color componentData6;
                  Owner componentData7;
                  // ISSUE: reference to a compiler-generated field
                  for (; !this.m_ObjectColorData.TryGetComponent(componentData4.m_Owner, out componentData6); componentData4 = componentData7)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_OwnerData.TryGetComponent(componentData4.m_Owner, out componentData7))
                    {
                      // ISSUE: reference to a compiler-generated field
                      flag = ((flag ? 1 : 0) & (!this.m_ObjectElevationData.TryGetComponent(componentData4.m_Owner, out componentData5) ? 0 : ((componentData5.m_Flags & ElevationFlags.OnGround) == (ElevationFlags) 0 ? 1 : 0))) != 0;
                    }
                    else
                      goto label_53;
                  }
                  if (flag || componentData6.m_SubColor)
                    float2 = new float2((float) componentData6.m_Index + 0.5f, (float) componentData6.m_Value * 0.003921569f);
                }
              }
            }
label_53:
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float2>(float2, meshBatch1.m_GroupIndex, index5, meshBatch1.m_InstanceIndex);
          }
          int index6;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.BuildingState, in groupData, out index6))
          {
            float4 float4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
            {
              // ISSUE: reference to a compiler-generated field
              float4 = this.m_BuildingStateOverride;
            }
            else
            {
              Entity entity = preCullingData.m_Entity;
              if ((preCullingData.m_Flags & PreCullingFlags.Temp) != (PreCullingFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                Temp temp = this.m_TempData[preCullingData.m_Entity];
                if (temp.m_Original != Entity.Null)
                  entity = temp.m_Original;
              }
              PseudoRandomSeed componentData8;
              // ISSUE: reference to a compiler-generated field
              this.m_PseudoRandomSeedData.TryGetComponent(entity, out componentData8);
              // ISSUE: reference to a compiler-generated field
              bool destroyed = this.m_DestroyedData.HasComponent(entity);
              // ISSUE: reference to a compiler-generated field
              if (this.m_VehicleData.HasComponent(entity))
              {
                int passengerCapacity = 0;
                int passengersCount = 0;
                Game.Vehicles.PublicTransport componentData9;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PublicTransportData.TryGetComponent(entity, out componentData9))
                {
                  PublicTransportVehicleData componentData10;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabPublicTransportVehicleData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData10))
                    passengerCapacity = componentData10.m_PassengerCapacity;
                  if ((componentData9.m_State & PublicTransportFlags.DummyTraffic) != (PublicTransportFlags) 0)
                  {
                    passengersCount = componentData8.GetRandom((uint) PseudoRandomSeed.kDummyPassengers).NextInt(0, passengerCapacity + 1);
                  }
                  else
                  {
                    DynamicBuffer<Passenger> bufferData6;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Passengers.TryGetBuffer(entity, out bufferData6))
                      passengersCount = bufferData6.Length;
                  }
                }
                else
                {
                  Unity.Mathematics.Random random = componentData8.GetRandom((uint) PseudoRandomSeed.kDummyPassengers);
                  passengerCapacity = 1000;
                  passengersCount = random.NextInt(0, passengerCapacity + 1);
                }
                // ISSUE: reference to a compiler-generated field
                float4 = BatchDataHelpers.GetBuildingState(componentData8, passengersCount, passengerCapacity, this.m_LightFactor, destroyed);
              }
              else
              {
                Owner componentData11;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.TryGetComponent(entity, out componentData11))
                  entity = componentData11.m_Owner;
                CitizenPresence componentData12;
                // ISSUE: reference to a compiler-generated field
                this.m_CitizenPresenceData.TryGetComponent(entity, out componentData12);
                // ISSUE: reference to a compiler-generated field
                bool flag = this.m_BuildingAbandonedData.HasComponent(entity);
                bool electricity = true;
                Building componentData13;
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingData.TryGetComponent(entity, out componentData13))
                  electricity = (componentData13.m_Flags & Game.Buildings.BuildingFlags.Illuminated) != 0;
                // ISSUE: reference to a compiler-generated field
                float4 = BatchDataHelpers.GetBuildingState(componentData8, componentData12, this.m_LightFactor, flag | destroyed, electricity);
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float4>(float4, meshBatch1.m_GroupIndex, index6, meshBatch1.m_InstanceIndex);
          }
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.ColorMask1))
          {
            DynamicBuffer<MeshColor> bufferData7;
            UnityEngine.Color color1;
            UnityEngine.Color color2;
            UnityEngine.Color color3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshColors.TryGetBuffer(preCullingData.m_Entity, out bufferData7))
            {
              int meshIndex = (int) meshBatch1.m_MeshIndex;
              MeshGroup meshGroup;
              if (CollectionUtils.TryGet<MeshGroup>(bufferData2, (int) meshBatch1.m_MeshGroup, out meshGroup))
                meshIndex += (int) meshGroup.m_ColorOffset;
              MeshColor meshColor = bufferData7[meshIndex];
              color1 = meshColor.m_ColorSet.m_Channel0.linear;
              color2 = meshColor.m_ColorSet.m_Channel1.linear;
              color3 = meshColor.m_ColorSet.m_Channel2.linear;
            }
            else
            {
              color1 = UnityEngine.Color.white;
              color2 = UnityEngine.Color.white;
              color3 = UnityEngine.Color.white;
            }
            bool smooth = (preCullingData.m_Flags & PreCullingFlags.SmoothColor) > (PreCullingFlags) 0;
            // ISSUE: reference to a compiler-generated method
            this.UpdateColorMask(in groupData, in meshBatch1, updateMask, ObjectProperty.ColorMask1, color1, smooth);
            // ISSUE: reference to a compiler-generated method
            this.UpdateColorMask(in groupData, in meshBatch1, updateMask, ObjectProperty.ColorMask2, color2, smooth);
            // ISSUE: reference to a compiler-generated method
            this.UpdateColorMask(in groupData, in meshBatch1, updateMask, ObjectProperty.ColorMask3, color3, smooth);
          }
          int index7;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.BoneParameters, in groupData, out index7))
          {
            DynamicBuffer<Skeleton> bufferData8;
            float2 float2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Skeletons.TryGetBuffer(preCullingData.m_Entity, out bufferData8))
            {
              int meshIndex = (int) meshBatch1.m_MeshIndex;
              MeshGroup meshGroup;
              if (CollectionUtils.TryGet<MeshGroup>(bufferData2, (int) meshBatch1.m_MeshGroup, out meshGroup))
                meshIndex += (int) meshGroup.m_MeshOffset;
              float2 = BatchDataHelpers.GetBoneParameters(bufferData8[meshIndex]);
            }
            else if ((preCullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Animated> animated = this.m_Animateds[preCullingData.m_Entity];
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[preCullingData.m_Entity];
              int index8 = (int) meshBatch1.m_MeshIndex;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabSubMeshGroups.HasBuffer(prefabRef.m_Prefab))
                index8 = !bufferData2.IsCreated || bufferData2.Length == 0 ? 0 : (int) meshBatch1.m_MeshGroup;
              float2 = BatchDataHelpers.GetBoneParameters(animated[index8]);
            }
            else
              float2 = float2.zero;
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float2>(float2, meshBatch1.m_GroupIndex, index7, meshBatch1.m_InstanceIndex);
          }
          int index9;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.LightParameters, in groupData, out index9))
          {
            int meshIndex = (int) meshBatch1.m_MeshIndex;
            MeshGroup meshGroup;
            if (CollectionUtils.TryGet<MeshGroup>(bufferData2, (int) meshBatch1.m_MeshGroup, out meshGroup))
              meshIndex += (int) meshGroup.m_MeshOffset;
            DynamicBuffer<Emissive> bufferData9;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float2>(!this.m_Emissives.TryGetBuffer(preCullingData.m_Entity, out bufferData9) ? float2.zero : BatchDataHelpers.GetLightParameters(bufferData9[meshIndex]), meshBatch1.m_GroupIndex, index9, meshBatch1.m_InstanceIndex);
          }
          int index10;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.OutlineColors, in groupData, out index10))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UnityEngine.Color color = !this.m_ErrorData.HasComponent(preCullingData.m_Entity) ? (!this.m_WarningData.HasComponent(preCullingData.m_Entity) ? (!this.m_OverrideData.HasComponent(preCullingData.m_Entity) ? ((preCullingData.m_Flags & PreCullingFlags.Temp) == (PreCullingFlags) 0 ? this.m_RenderingSettingsData.m_HoveredColor : ((this.m_TempData[preCullingData.m_Entity].m_Flags & TempFlags.Parent) == (TempFlags) 0 ? this.m_RenderingSettingsData.m_HoveredColor : this.m_RenderingSettingsData.m_OwnerColor)) : this.m_RenderingSettingsData.m_OverrideColor) : this.m_RenderingSettingsData.m_WarningColor) : this.m_RenderingSettingsData.m_ErrorColor;
            color = color.linear;
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<UnityEngine.Color>(color, meshBatch1.m_GroupIndex, index10, meshBatch1.m_InstanceIndex);
          }
          int index11;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.MetaParameters, in groupData, out index11))
          {
            float num = 0.0f;
            if ((preCullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Animated> animated = this.m_Animateds[preCullingData.m_Entity];
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[preCullingData.m_Entity];
              int index12 = (int) meshBatch1.m_MeshIndex;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabSubMeshGroups.HasBuffer(prefabRef.m_Prefab))
                index12 = !bufferData2.IsCreated || bufferData2.Length == 0 ? 0 : (int) meshBatch1.m_MeshGroup;
              num = math.asfloat(animated[index12].m_MetaIndex);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float>(num, meshBatch1.m_GroupIndex, index11, meshBatch1.m_InstanceIndex);
          }
          int index13;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ObjectProperty.SurfaceWetness, in groupData, out index13))
          {
            float4 float4 = new float4();
            if ((preCullingData.m_Flags & PreCullingFlags.SurfaceState) != (PreCullingFlags) 0)
            {
              Surface componentData14;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ObjectSurfaceData.TryGetComponent(preCullingData.m_Entity, out componentData14))
              {
                float4 = BatchDataHelpers.GetWetness(componentData14);
              }
              else
              {
                Owner componentData15;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.TryGetComponent(preCullingData.m_Entity, out componentData15))
                {
                  Surface componentData16;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (; !this.m_ObjectSurfaceData.TryGetComponent(componentData15.m_Owner, out componentData16); componentData15 = this.m_OwnerData[componentData15.m_Owner])
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_OwnerData.HasComponent(componentData15.m_Owner))
                      goto label_115;
                  }
                  float4 = BatchDataHelpers.GetWetness(componentData16);
                }
              }
            }
label_115:
            float4 x;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NativeBatchInstances.GetPropertyValue<float4>(out x, meshBatch1.m_GroupIndex, index13, meshBatch1.m_InstanceIndex))
            {
              if (math.any(x != float4))
              {
                bool4 c = float4 < x;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                x += math.select((float4) this.m_SmoothnessDelta, (float4) -this.m_SmoothnessDelta, c);
                x = math.clamp(x, math.select((float4) 0.0f, float4, c), math.select(float4, (float4) 1f, c));
                // ISSUE: reference to a compiler-generated field
                this.m_NativeBatchInstances.SetPropertyValue<float4>(x, meshBatch1.m_GroupIndex, index13, meshBatch1.m_InstanceIndex);
                if (math.any(x != float4))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_SmoothingNeeded.Accumulate(new BatchDataSystem.SmoothingNeeded(BatchDataSystem.SmoothingType.SurfaceWetness));
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4>(float4, meshBatch1.m_GroupIndex, index13, meshBatch1.m_InstanceIndex);
            }
          }
        }
      }

      private void UpdateColorMask(
        in GroupData groupData,
        in MeshBatch meshBatch,
        BatchDataSystem.UpdateMask updateMask,
        ObjectProperty property,
        UnityEngine.Color color,
        bool smooth)
      {
        int index;
        // ISSUE: reference to a compiler-generated method
        if (!updateMask.ShouldUpdateProperty(property, in groupData, out index))
          return;
        // ISSUE: reference to a compiler-generated method
        this.UpdateColorMask(in meshBatch, color, index, smooth);
      }

      private void UpdateColorMask(
        in GroupData groupData,
        in MeshBatch meshBatch,
        BatchDataSystem.UpdateMask updateMask,
        LaneProperty property,
        UnityEngine.Color color,
        bool smooth)
      {
        int index;
        // ISSUE: reference to a compiler-generated method
        if (!updateMask.ShouldUpdateProperty(property, in groupData, out index))
          return;
        // ISSUE: reference to a compiler-generated method
        this.UpdateColorMask(in meshBatch, color, index, smooth);
      }

      private void UpdateColorMask(
        in MeshBatch meshBatch,
        UnityEngine.Color color,
        int colorMask,
        bool smooth)
      {
        UnityEngine.Color color1;
        // ISSUE: reference to a compiler-generated field
        if (smooth && this.m_NativeBatchInstances.GetPropertyValue<UnityEngine.Color>(out color1, meshBatch.m_GroupIndex, colorMask, meshBatch.m_InstanceIndex))
        {
          float4 float4_1 = (float4) (Vector4) color;
          float4 float4_2 = (float4) (Vector4) color1;
          float4 x = float4_1 - float4_2;
          if (!math.any(x != 0.0f))
            return;
          bool4 c = x < 0.0f;
          // ISSUE: reference to a compiler-generated field
          float4 float4_3 = math.clamp(float4_2 + x * (this.m_SmoothnessDelta / math.cmax(math.abs(x))), math.select((float4) 0.0f, float4_1, c), math.select(float4_1, (float4) 1f, c));
          color1 = (UnityEngine.Color) (Vector4) float4_3;
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchInstances.SetPropertyValue<UnityEngine.Color>(color1, meshBatch.m_GroupIndex, colorMask, meshBatch.m_InstanceIndex);
          if (!math.any(float4_3 != float4_1))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_SmoothingNeeded.Accumulate(new BatchDataSystem.SmoothingNeeded(BatchDataSystem.SmoothingType.ColorMask));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchInstances.SetPropertyValue<UnityEngine.Color>(color, meshBatch.m_GroupIndex, colorMask, meshBatch.m_InstanceIndex);
        }
      }

      private void UpdateNetData(
        PreCullingData preCullingData,
        BatchDataSystem.UpdateMask updateMask)
      {
        DynamicBuffer<MeshBatch> bufferData;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (updateMask.ShouldUpdateNothing() || !this.m_MeshBatches.TryGetBuffer(preCullingData.m_Entity, out bufferData))
          return;
        for (int index1 = 0; index1 < bufferData.Length; ++index1)
        {
          MeshBatch meshBatch = bufferData[index1];
          NetSubMesh meshIndex = (NetSubMesh) meshBatch.m_MeshIndex;
          // ISSUE: reference to a compiler-generated field
          GroupData groupData = this.m_NativeBatchGroups.GetGroupData(meshBatch.m_GroupIndex);
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateTransform())
          {
            // ISSUE: reference to a compiler-generated field
            CullingInfo cullingInfo = this.m_CullingInfoData[preCullingData.m_Entity];
            BatchDataHelpers.CompositionParameters compositionParameters = new BatchDataHelpers.CompositionParameters();
            switch (meshIndex)
            {
              case NetSubMesh.Edge:
              case NetSubMesh.RotatedEdge:
                // ISSUE: reference to a compiler-generated field
                BatchDataHelpers.CalculateEdgeParameters(this.m_EdgeGeometryData[preCullingData.m_Entity], meshIndex == NetSubMesh.RotatedEdge, out compositionParameters);
                break;
              case NetSubMesh.StartNode:
              case NetSubMesh.SubStartNode:
                // ISSUE: reference to a compiler-generated field
                Composition composition1 = this.m_CompositionData[preCullingData.m_Entity];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                BatchDataHelpers.CalculateNodeParameters(this.m_StartNodeGeometryData[preCullingData.m_Entity].m_Geometry, this.m_PrefabCompositionData[composition1.m_StartNode], out compositionParameters);
                break;
              case NetSubMesh.EndNode:
              case NetSubMesh.SubEndNode:
                // ISSUE: reference to a compiler-generated field
                Composition composition2 = this.m_CompositionData[preCullingData.m_Entity];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                BatchDataHelpers.CalculateNodeParameters(this.m_EndNodeGeometryData[preCullingData.m_Entity].m_Geometry, this.m_PrefabCompositionData[composition2.m_EndNode], out compositionParameters);
                break;
              case NetSubMesh.Orphan1:
              case NetSubMesh.Orphan2:
                // ISSUE: reference to a compiler-generated field
                Game.Net.Node node = this.m_NodeData[preCullingData.m_Entity];
                // ISSUE: reference to a compiler-generated field
                Orphan orphan = this.m_OrphanData[preCullingData.m_Entity];
                // ISSUE: reference to a compiler-generated field
                NodeGeometry nodeGeometry1 = this.m_NodeGeometryData[preCullingData.m_Entity];
                // ISSUE: reference to a compiler-generated field
                NetCompositionData netCompositionData = this.m_PrefabCompositionData[orphan.m_Composition];
                NodeGeometry nodeGeometry2 = nodeGeometry1;
                NetCompositionData prefabCompositionData = netCompositionData;
                int num = meshIndex == NetSubMesh.Orphan1 ? 1 : 0;
                ref BatchDataHelpers.CompositionParameters local1 = ref compositionParameters;
                BatchDataHelpers.CalculateOrphanParameters(node, nodeGeometry2, prefabCompositionData, num != 0, out local1);
                break;
            }
            // ISSUE: reference to a compiler-generated field
            ref CullingData local2 = ref this.m_NativeBatchInstances.SetTransformValue(compositionParameters.m_TransformMatrix, compositionParameters.m_TransformMatrix, meshBatch.m_GroupIndex, meshBatch.m_InstanceIndex);
            local2.m_Bounds = cullingInfo.m_Bounds;
            // ISSUE: reference to a compiler-generated field
            local2.isHidden = this.m_HiddenData.HasComponent(preCullingData.m_Entity);
            int index2;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix0, in groupData, out index2))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix0, meshBatch.m_GroupIndex, index2, meshBatch.m_InstanceIndex);
            }
            int index3;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix1, in groupData, out index3))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix1, meshBatch.m_GroupIndex, index3, meshBatch.m_InstanceIndex);
            }
            int index4;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix2, in groupData, out index4))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix2, meshBatch.m_GroupIndex, index4, meshBatch.m_InstanceIndex);
            }
            int index5;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix3, in groupData, out index5))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix3, meshBatch.m_GroupIndex, index5, meshBatch.m_InstanceIndex);
            }
            int index6;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix4, in groupData, out index6))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix4, meshBatch.m_GroupIndex, index6, meshBatch.m_InstanceIndex);
            }
            int index7;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix5, in groupData, out index7))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix5, meshBatch.m_GroupIndex, index7, meshBatch.m_InstanceIndex);
            }
            int index8;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix6, in groupData, out index8))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix6, meshBatch.m_GroupIndex, index8, meshBatch.m_InstanceIndex);
            }
            int index9;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionMatrix7, in groupData, out index9))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(compositionParameters.m_CompositionMatrix7, meshBatch.m_GroupIndex, index9, meshBatch.m_InstanceIndex);
            }
            int index10;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionSync0, in groupData, out index10))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4>(compositionParameters.m_CompositionSync0, meshBatch.m_GroupIndex, index10, meshBatch.m_InstanceIndex);
            }
            int index11;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionSync1, in groupData, out index11))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4>(compositionParameters.m_CompositionSync1, meshBatch.m_GroupIndex, index11, meshBatch.m_InstanceIndex);
            }
            int index12;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionSync2, in groupData, out index12))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4>(compositionParameters.m_CompositionSync2, meshBatch.m_GroupIndex, index12, meshBatch.m_InstanceIndex);
            }
            int index13;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(NetProperty.CompositionSync3, in groupData, out index13))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4>(compositionParameters.m_CompositionSync3, meshBatch.m_GroupIndex, index13, meshBatch.m_InstanceIndex);
            }
          }
          int index14;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(NetProperty.InfoviewColor, in groupData, out index14))
          {
            float4 float4 = new float4();
            if ((preCullingData.m_Flags & PreCullingFlags.InfoviewColor) != (PreCullingFlags) 0)
            {
              EdgeColor componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_NetEdgeColorData.TryGetComponent(preCullingData.m_Entity, out componentData1))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.Edge edge = this.m_EdgeData[preCullingData.m_Entity];
                float2 float2_1 = new float2((float) componentData1.m_Index + 0.5f, (float) componentData1.m_Value0 * 0.003921569f);
                float2 float2_2 = new float2((float) componentData1.m_Index + 0.5f, (float) componentData1.m_Value1 * 0.003921569f);
                float2 zw1 = float2_1;
                float2 zw2 = float2_2;
                NodeColor componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NetNodeColorData.TryGetComponent(edge.m_Start, out componentData2))
                  zw1 = new float2((float) componentData2.m_Index + 0.5f, (float) componentData2.m_Value * 0.003921569f);
                NodeColor componentData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NetNodeColorData.TryGetComponent(edge.m_End, out componentData3))
                  zw2 = new float2((float) componentData3.m_Index + 0.5f, (float) componentData3.m_Value * 0.003921569f);
                switch (meshIndex)
                {
                  case NetSubMesh.Edge:
                  case NetSubMesh.SubStartNode:
                  case NetSubMesh.SubEndNode:
                    float4 = new float4(float2_1, float2_2);
                    break;
                  case NetSubMesh.RotatedEdge:
                    float4 = new float4(float2_2, float2_1);
                    break;
                  case NetSubMesh.StartNode:
                    float4 = new float4(float2_1, zw1);
                    break;
                  case NetSubMesh.EndNode:
                    float4 = new float4(float2_2, zw2);
                    break;
                }
              }
              else
              {
                NodeColor componentData4;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NetNodeColorData.TryGetComponent(preCullingData.m_Entity, out componentData4))
                  float4 = new float4((float) componentData4.m_Index + 0.5f, (float) componentData4.m_Value * 0.003921569f, (float) componentData4.m_Index + 0.5f, (float) componentData4.m_Value * 0.003921569f);
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float4>(float4, meshBatch.m_GroupIndex, index14, meshBatch.m_InstanceIndex);
          }
          int index15;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(NetProperty.OutlineColors, in groupData, out index15))
          {
            UnityEngine.Color color;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ErrorData.HasComponent(preCullingData.m_Entity))
            {
              // ISSUE: reference to a compiler-generated field
              color = this.m_RenderingSettingsData.m_ErrorColor;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_WarningData.HasComponent(preCullingData.m_Entity))
              {
                // ISSUE: reference to a compiler-generated field
                color = this.m_RenderingSettingsData.m_WarningColor;
              }
              else if ((preCullingData.m_Flags & PreCullingFlags.Temp) != (PreCullingFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                Temp temp = this.m_TempData[preCullingData.m_Entity];
                // ISSUE: reference to a compiler-generated field
                color = this.m_RenderingSettingsData.m_HoveredColor;
                if ((temp.m_Flags & TempFlags.Parent) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  color = this.m_RenderingSettingsData.m_OwnerColor;
                }
                if ((temp.m_Flags & TempFlags.SubDetail) != (TempFlags) 0)
                {
                  switch (meshIndex)
                  {
                    case NetSubMesh.StartNode:
                    case NetSubMesh.SubStartNode:
                      Temp componentData5;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_TempData.TryGetComponent(this.m_EdgeData[preCullingData.m_Entity].m_Start, out componentData5) && (componentData5.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent))
                      {
                        // ISSUE: reference to a compiler-generated field
                        color = this.m_RenderingSettingsData.m_OwnerColor;
                        break;
                      }
                      break;
                    case NetSubMesh.EndNode:
                    case NetSubMesh.SubEndNode:
                      Temp componentData6;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_TempData.TryGetComponent(this.m_EdgeData[preCullingData.m_Entity].m_End, out componentData6) && (componentData6.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent))
                      {
                        // ISSUE: reference to a compiler-generated field
                        color = this.m_RenderingSettingsData.m_OwnerColor;
                        break;
                      }
                      break;
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                color = this.m_RenderingSettingsData.m_HoveredColor;
              }
            }
            color = color.linear;
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<UnityEngine.Color>(color, meshBatch.m_GroupIndex, index15, meshBatch.m_InstanceIndex);
          }
        }
      }

      private void UpdateLaneData(
        PreCullingData preCullingData,
        BatchDataSystem.UpdateMask updateMask)
      {
        // ISSUE: reference to a compiler-generated method
        if (!updateMask.ShouldUpdateProperty(LaneProperty.ColorMask1) && (preCullingData.m_Flags & PreCullingFlags.ColorsUpdated) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateProperty(LaneProperty.ColorMask1);
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateProperty(LaneProperty.ColorMask2);
          // ISSUE: reference to a compiler-generated method
          updateMask.UpdateProperty(LaneProperty.ColorMask3);
        }
        DynamicBuffer<MeshBatch> bufferData1;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (updateMask.ShouldUpdateNothing() || !this.m_MeshBatches.TryGetBuffer(preCullingData.m_Entity, out bufferData1))
          return;
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          MeshBatch meshBatch = bufferData1[index1];
          // ISSUE: reference to a compiler-generated field
          GroupData groupData = this.m_NativeBatchGroups.GetGroupData(meshBatch.m_GroupIndex);
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateTransform())
          {
            // ISSUE: reference to a compiler-generated field
            CullingInfo cullingInfo = this.m_CullingInfoData[preCullingData.m_Entity];
            // ISSUE: reference to a compiler-generated field
            Curve curve1 = this.m_CurveData[preCullingData.m_Entity];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[preCullingData.m_Entity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.MeshData meshData = this.m_PrefabMeshData[this.m_PrefabSubMeshes[prefabRef.m_Prefab][(int) meshBatch.m_MeshIndex].m_SubMesh];
            float4 size = new float4(MathUtils.Size(meshData.m_Bounds), MathUtils.Center(meshData.m_Bounds).y);
            bool geometryTiling = (meshData.m_State & MeshFlags.Tiling) > (MeshFlags) 0;
            int num1 = 1;
            int clipCount = 0;
            int tileIndex = (int) meshBatch.m_TileIndex;
            DynamicBuffer<CutRange> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CutRanges.TryGetBuffer(preCullingData.m_Entity, out bufferData2))
            {
              float _min = 0.0f;
              for (int index2 = 0; index2 <= bufferData2.Length; ++index2)
              {
                float _max;
                float num2;
                if (index2 < bufferData2.Length)
                {
                  CutRange cutRange = bufferData2[index2];
                  _max = cutRange.m_CurveDelta.min;
                  num2 = cutRange.m_CurveDelta.max;
                }
                else
                {
                  _max = 1f;
                  num2 = 1f;
                }
                if ((double) _max >= (double) _min)
                {
                  Curve curve2 = new Curve()
                  {
                    m_Length = curve1.m_Length * (_max - _min)
                  };
                  if ((double) curve2.m_Length > 0.10000000149011612)
                  {
                    num1 = BatchDataHelpers.GetTileCount(curve2, size.z, meshData.m_TilingCount, geometryTiling, out clipCount);
                    if (num1 > tileIndex)
                    {
                      curve2.m_Bezier = MathUtils.Cut(curve1.m_Bezier, new Bounds1(_min, _max));
                      curve1 = curve2;
                      break;
                    }
                    tileIndex -= num1;
                  }
                }
                _min = num2;
              }
            }
            else if (geometryTiling)
              num1 = BatchDataHelpers.GetTileCount(curve1, size.z, meshData.m_TilingCount, true, out clipCount);
            if ((meshData.m_State & MeshFlags.Invert) != (MeshFlags) 0)
              curve1.m_Bezier = MathUtils.Invert(curve1.m_Bezier);
            if (num1 > 1)
            {
              int2 int2 = new int2(tileIndex, tileIndex + 1) * clipCount / num1;
              float2 float2 = curve1.m_Length * (float2) int2 / (float) clipCount;
              float2 t1 = new float2(0.0f, 1f);
              if (int2.x != 0)
              {
                Bounds1 t2 = new Bounds1(0.0f, 1f);
                MathUtils.ClampLength(curve1.m_Bezier, ref t2, float2.x);
                t1.x = t2.max;
              }
              if (int2.y != clipCount)
              {
                Bounds1 t3 = new Bounds1(0.0f, 1f);
                MathUtils.ClampLength(curve1.m_Bezier, ref t3, float2.y);
                t1.y = t3.max;
              }
              curve1.m_Bezier = MathUtils.Cut(curve1.m_Bezier, t1);
              curve1.m_Length = float2.y - float2.x;
            }
            NodeLane componentData1;
            // ISSUE: reference to a compiler-generated field
            bool component = this.m_NodeLaneData.TryGetComponent(preCullingData.m_Entity, out componentData1);
            float4 curveScale;
            if (component)
            {
              // ISSUE: reference to a compiler-generated field
              NetLaneData netLaneData = this.m_PrefabNetLaneData[prefabRef.m_Prefab];
              curveScale = BatchDataHelpers.BuildCurveScale(componentData1, netLaneData);
            }
            else
              curveScale = BatchDataHelpers.BuildCurveScale();
            bool isDecal = (meshData.m_State & MeshFlags.Decal) > (MeshFlags) 0;
            float3x4 transformMatrix = TransformHelper.Convert(BatchDataHelpers.BuildTransformMatrix(curve1, size, curveScale, meshData.m_SmoothingDistance, isDecal, true));
            float3x4 secondaryValue = TransformHelper.Convert(BatchDataHelpers.BuildTransformMatrix(curve1, size, curveScale, meshData.m_SmoothingDistance, isDecal, false));
            float4x4 float4x4 = BatchDataHelpers.BuildCurveMatrix(curve1, transformMatrix, size, meshData.m_TilingCount);
            // ISSUE: reference to a compiler-generated field
            ref CullingData local = ref this.m_NativeBatchInstances.SetTransformValue(transformMatrix, secondaryValue, meshBatch.m_GroupIndex, meshBatch.m_InstanceIndex);
            local.m_Bounds = cullingInfo.m_Bounds;
            // ISSUE: reference to a compiler-generated field
            local.isHidden = this.m_HiddenData.HasComponent(preCullingData.m_Entity);
            int index3;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(LaneProperty.CurveMatrix, in groupData, out index3))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(float4x4, meshBatch.m_GroupIndex, index3, meshBatch.m_InstanceIndex);
            }
            int index4;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(LaneProperty.CurveParams, in groupData, out index4))
            {
              EdgeLane componentData2;
              Game.Net.Elevation componentData3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4>(!component ? (!this.m_EdgeLaneData.TryGetComponent(preCullingData.m_Entity, out componentData2) ? (!this.m_NetElevationData.TryGetComponent(preCullingData.m_Entity, out componentData3) ? BatchDataHelpers.BuildCurveParams(size) : BatchDataHelpers.BuildCurveParams(size, componentData3)) : BatchDataHelpers.BuildCurveParams(size, componentData2)) : BatchDataHelpers.BuildCurveParams(size, componentData1), meshBatch.m_GroupIndex, index4, meshBatch.m_InstanceIndex);
            }
            int index5;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(LaneProperty.CurveScale, in groupData, out index5))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4>(curveScale, meshBatch.m_GroupIndex, index5, meshBatch.m_InstanceIndex);
            }
          }
          int index6;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(LaneProperty.InfoviewColor, in groupData, out index6))
          {
            float4 float4 = new float4();
            if ((preCullingData.m_Flags & PreCullingFlags.InfoviewColor) != (PreCullingFlags) 0)
            {
              LaneColor componentData4;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneColorData.TryGetComponent(preCullingData.m_Entity, out componentData4))
              {
                float4 = new float4((float) componentData4.m_Index + 0.5f, (float) componentData4.m_Value0 * 0.003921569f, (float) componentData4.m_Index + 0.5f, (float) componentData4.m_Value1 * 0.003921569f);
              }
              else
              {
                Owner componentData5;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.TryGetComponent(preCullingData.m_Entity, out componentData5))
                {
                  EdgeLane componentData6;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_EdgeLaneData.TryGetComponent(preCullingData.m_Entity, out componentData6))
                  {
                    EdgeColor componentData7;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_NetEdgeColorData.TryGetComponent(componentData5.m_Owner, out componentData7))
                      float4 = math.lerp(new float2((float) componentData7.m_Index + 0.5f, (float) componentData7.m_Value0 * 0.003921569f).xyxy, new float2((float) componentData7.m_Index + 0.5f, (float) componentData7.m_Value1 * 0.003921569f).xyxy, componentData6.m_EdgeDelta.xxyy);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_NodeLaneData.HasComponent(preCullingData.m_Entity))
                    {
                      NodeColor componentData8;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_NetNodeColorData.TryGetComponent(componentData5.m_Owner, out componentData8))
                        float4 = new float4((float) componentData8.m_Index + 0.5f, (float) componentData8.m_Value * 0.003921569f, (float) componentData8.m_Index + 0.5f, (float) componentData8.m_Value * 0.003921569f);
                    }
                    else
                    {
                      Game.Objects.Color componentData9;
                      Owner componentData10;
                      // ISSUE: reference to a compiler-generated field
                      for (; !this.m_ObjectColorData.TryGetComponent(componentData5.m_Owner, out componentData9); componentData5 = componentData10)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (!this.m_OwnerData.TryGetComponent(componentData5.m_Owner, out componentData10))
                          goto label_54;
                      }
                      if (componentData9.m_SubColor)
                        float4 = new float4((float) componentData9.m_Index + 0.5f, (float) componentData9.m_Value * 0.003921569f, (float) componentData9.m_Index + 0.5f, (float) componentData9.m_Value * 0.003921569f);
                    }
                  }
                }
              }
            }
label_54:
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float4>(float4, meshBatch.m_GroupIndex, index6, meshBatch.m_InstanceIndex);
          }
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(LaneProperty.ColorMask1))
          {
            DynamicBuffer<MeshColor> bufferData3;
            UnityEngine.Color color1;
            UnityEngine.Color color2;
            UnityEngine.Color color3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshColors.TryGetBuffer(preCullingData.m_Entity, out bufferData3))
            {
              MeshColor meshColor = bufferData3[(int) meshBatch.m_MeshIndex];
              color1 = meshColor.m_ColorSet.m_Channel0.linear;
              color2 = meshColor.m_ColorSet.m_Channel1.linear;
              color3 = meshColor.m_ColorSet.m_Channel2.linear;
            }
            else
            {
              color1 = UnityEngine.Color.white;
              color2 = UnityEngine.Color.white;
              color3 = UnityEngine.Color.white;
            }
            bool smooth = (preCullingData.m_Flags & PreCullingFlags.SmoothColor) > (PreCullingFlags) 0;
            // ISSUE: reference to a compiler-generated method
            this.UpdateColorMask(in groupData, in meshBatch, updateMask, LaneProperty.ColorMask1, color1, smooth);
            // ISSUE: reference to a compiler-generated method
            this.UpdateColorMask(in groupData, in meshBatch, updateMask, LaneProperty.ColorMask2, color2, smooth);
            // ISSUE: reference to a compiler-generated method
            this.UpdateColorMask(in groupData, in meshBatch, updateMask, LaneProperty.ColorMask3, color3, smooth);
          }
          int index7;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(LaneProperty.FlowMatrix, in groupData, out index7))
          {
            float4x4 float4x4 = new float4x4();
            DynamicBuffer<SubFlow> bufferData4;
            // ISSUE: reference to a compiler-generated field
            if ((preCullingData.m_Flags & PreCullingFlags.InfoviewColor) != (PreCullingFlags) 0 && this.m_SubFlows.TryGetBuffer(preCullingData.m_Entity, out bufferData4) && bufferData4.Length == 16)
            {
              float4x4.c0 = new float4((float) bufferData4[0].m_Value, (float) bufferData4[4].m_Value, (float) bufferData4[8].m_Value, (float) bufferData4[12].m_Value) * (1f / (float) sbyte.MaxValue);
              float4x4.c1 = new float4((float) bufferData4[1].m_Value, (float) bufferData4[5].m_Value, (float) bufferData4[9].m_Value, (float) bufferData4[13].m_Value) * (1f / (float) sbyte.MaxValue);
              float4x4.c2 = new float4((float) bufferData4[2].m_Value, (float) bufferData4[6].m_Value, (float) bufferData4[10].m_Value, (float) bufferData4[14].m_Value) * (1f / (float) sbyte.MaxValue);
              float4x4.c3 = new float4((float) bufferData4[3].m_Value, (float) bufferData4[7].m_Value, (float) bufferData4[11].m_Value, (float) bufferData4[15].m_Value) * (1f / (float) sbyte.MaxValue);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float4x4>(float4x4, meshBatch.m_GroupIndex, index7, meshBatch.m_InstanceIndex);
          }
          int index8;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(LaneProperty.FlowOffset, in groupData, out index8))
          {
            float num = 0.0f;
            Owner componentData11;
            PseudoRandomSeed componentData12;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((preCullingData.m_Flags & PreCullingFlags.InfoviewColor) != (PreCullingFlags) 0 && this.m_OwnerData.TryGetComponent(preCullingData.m_Entity, out componentData11) && this.m_PseudoRandomSeedData.TryGetComponent(componentData11.m_Owner, out componentData12))
              num = componentData12.GetRandom((uint) PseudoRandomSeed.kFlowOffset).NextFloat(1f);
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float>(num, meshBatch.m_GroupIndex, index8, meshBatch.m_InstanceIndex);
          }
          int index9;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(LaneProperty.CurveDeterioration, in groupData, out index9))
          {
            float4 float4 = (float4) 0.0f;
            LaneCondition componentData;
            // ISSUE: reference to a compiler-generated field
            if ((preCullingData.m_Flags & PreCullingFlags.LaneCondition) != (PreCullingFlags) 0 && this.m_LaneConditionData.TryGetComponent(preCullingData.m_Entity, out componentData))
            {
              float num = componentData.m_Wear / 10f;
              float4 = new float4(num, num, num, 0.0f);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float4>(float4, meshBatch.m_GroupIndex, index9, meshBatch.m_InstanceIndex);
          }
          int index10;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(LaneProperty.OutlineColors, in groupData, out index10))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UnityEngine.Color color = !this.m_ErrorData.HasComponent(preCullingData.m_Entity) ? (!this.m_WarningData.HasComponent(preCullingData.m_Entity) ? ((preCullingData.m_Flags & PreCullingFlags.Temp) == (PreCullingFlags) 0 ? this.m_RenderingSettingsData.m_HoveredColor : ((this.m_TempData[preCullingData.m_Entity].m_Flags & TempFlags.Parent) == (TempFlags) 0 ? this.m_RenderingSettingsData.m_HoveredColor : this.m_RenderingSettingsData.m_OwnerColor)) : this.m_RenderingSettingsData.m_WarningColor) : this.m_RenderingSettingsData.m_ErrorColor;
            color = color.linear;
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<UnityEngine.Color>(color, meshBatch.m_GroupIndex, index10, meshBatch.m_InstanceIndex);
          }
          int index11;
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(LaneProperty.HangingDistances, in groupData, out index11))
          {
            float4 float4 = (float4) 0.0f;
            PrefabRef componentData13;
            Curve componentData14;
            UtilityLaneData componentData15;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.TryGetComponent(preCullingData.m_Entity, out componentData13) && this.m_CurveData.TryGetComponent(preCullingData.m_Entity, out componentData14) && this.m_PrefabUtilityLaneData.TryGetComponent(componentData13.m_Prefab, out componentData15))
            {
              HangingLane componentData16;
              // ISSUE: reference to a compiler-generated field
              this.m_HangingLaneData.TryGetComponent(preCullingData.m_Entity, out componentData16);
              float4.xw = componentData16.m_Distances.xy;
              float4.yz = (componentData16.m_Distances.xy + componentData15.m_Hanging * componentData14.m_Length) * 0.6666667f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float4 x = new float4(math.lengthsq(Wind.SampleWind(this.m_WindData, componentData14.m_Bezier.a)), math.lengthsq(Wind.SampleWind(this.m_WindData, componentData14.m_Bezier.b)), math.lengthsq(Wind.SampleWind(this.m_WindData, componentData14.m_Bezier.c)), math.lengthsq(Wind.SampleWind(this.m_WindData, componentData14.m_Bezier.d)));
              float4 *= math.sqrt(x);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetPropertyValue<float4>(float4, meshBatch.m_GroupIndex, index11, meshBatch.m_InstanceIndex);
          }
        }
      }

      private unsafe void UpdateZoneData(
        PreCullingData preCullingData,
        BatchDataSystem.UpdateMask updateMask)
      {
        DynamicBuffer<MeshBatch> bufferData;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (updateMask.ShouldUpdateNothing() || !this.m_MeshBatches.TryGetBuffer(preCullingData.m_Entity, out bufferData))
          return;
        for (int index1 = 0; index1 < bufferData.Length; ++index1)
        {
          MeshBatch meshBatch = bufferData[index1];
          // ISSUE: reference to a compiler-generated field
          GroupData groupData = this.m_NativeBatchGroups.GetGroupData(meshBatch.m_GroupIndex);
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateTransform())
          {
            // ISSUE: reference to a compiler-generated field
            CullingInfo cullingInfo = this.m_CullingInfoData[preCullingData.m_Entity];
            // ISSUE: reference to a compiler-generated field
            Game.Zones.Block block = this.m_ZoneBlockData[preCullingData.m_Entity];
            float3 position = block.m_Position;
            float2 float2 = (float2) (block.m_Size - new int2(10, 6)) * 4f;
            position.xz += block.m_Direction * float2.y;
            position.xz += MathUtils.Right(block.m_Direction) * float2.x;
            float3x4 secondaryValue = TransformHelper.TRS(position, ZoneUtils.GetRotation(block), new float3(1f, 1f, 1f));
            // ISSUE: reference to a compiler-generated field
            ref CullingData local = ref this.m_NativeBatchInstances.SetTransformValue(secondaryValue, secondaryValue, meshBatch.m_GroupIndex, meshBatch.m_InstanceIndex);
            local.m_Bounds = cullingInfo.m_Bounds;
            // ISSUE: reference to a compiler-generated field
            local.isHidden = this.m_HiddenData.HasComponent(preCullingData.m_Entity);
          }
          // ISSUE: reference to a compiler-generated method
          if (updateMask.ShouldUpdateProperty(ZoneProperty.CellType0))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Zones.Block block = this.m_ZoneBlockData[preCullingData.m_Entity];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Zones.Cell> zoneCell = this.m_ZoneCells[preCullingData.m_Entity];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            BatchDataSystem.CellTypes cellTypes = new BatchDataSystem.CellTypes();
            void* destination = (void*) &cellTypes;
            for (int index2 = 0; index2 < block.m_Size.y; ++index2)
            {
              for (int index3 = 0; index3 < block.m_Size.x; ++index3)
              {
                Game.Zones.Cell cell = zoneCell[index2 * block.m_Size.x + index3];
                int colorIndex = ZoneUtils.GetColorIndex(cell.m_State, cell.m_Zone);
                int index4 = index2 + index3 * 6;
                UnsafeUtility.WriteArrayElement<float>(destination, index4, (float) colorIndex);
              }
            }
            int index5;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(ZoneProperty.CellType0, in groupData, out index5))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(cellTypes.m_CellTypes0, meshBatch.m_GroupIndex, index5, meshBatch.m_InstanceIndex);
            }
            int index6;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(ZoneProperty.CellType1, in groupData, out index6))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(cellTypes.m_CellTypes1, meshBatch.m_GroupIndex, index6, meshBatch.m_InstanceIndex);
            }
            int index7;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(ZoneProperty.CellType2, in groupData, out index7))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(cellTypes.m_CellTypes2, meshBatch.m_GroupIndex, index7, meshBatch.m_InstanceIndex);
            }
            int index8;
            // ISSUE: reference to a compiler-generated method
            if (updateMask.ShouldUpdateProperty(ZoneProperty.CellType3, in groupData, out index8))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchInstances.SetPropertyValue<float4x4>(cellTypes.m_CellTypes3, meshBatch.m_GroupIndex, index8, meshBatch.m_InstanceIndex);
            }
          }
        }
      }

      private void UpdateFadeData(PreCullingData preCullingData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<MeshBatch> meshBatch1 = this.m_MeshBatches[preCullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<FadeBatch> fadeBatch1 = this.m_FadeBatches[preCullingData.m_Entity];
        for (int index = 0; index < meshBatch1.Length; ++index)
        {
          MeshBatch meshBatch2 = meshBatch1[index];
          FadeBatch fadeBatch2 = fadeBatch1[index];
          float3x4 float3x4;
          float3x4 secondaryValue;
          // ISSUE: reference to a compiler-generated field
          if (!fadeBatch2.m_Velocity.Equals(new float3()) && this.m_NativeBatchInstances.GetTransformValue(meshBatch2.m_GroupIndex, meshBatch2.m_InstanceIndex, out float3x4, out secondaryValue))
          {
            // ISSUE: reference to a compiler-generated field
            float3 float3 = fadeBatch2.m_Velocity * (this.m_FrameDelta * 0.0166666675f);
            float3x4.c3 += float3;
            secondaryValue.c3 += float3;
            // ISSUE: reference to a compiler-generated field
            this.m_NativeBatchInstances.SetTransformValue(float3x4, secondaryValue, meshBatch2.m_GroupIndex, meshBatch2.m_InstanceIndex).m_Bounds += float3;
          }
        }
      }
    }

    [BurstCompile]
    private struct BatchLodJob : IJobParallelFor
    {
      [ReadOnly]
      public bool m_DisableLods;
      [ReadOnly]
      public bool m_UseLodFade;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public float m_PixelSizeFactor;
      [ReadOnly]
      public int m_LodFadeDelta;
      [ReadOnly]
      public NativeList<MeshLoadingState> m_LoadingState;
      [ReadOnly]
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.ParallelCullingWriter m_NativeBatchInstances;
      [NativeDisableParallelForRestriction]
      public NativeList<int> m_BatchPriority;
      [NativeDisableParallelForRestriction]
      public NativeList<float> m_VTRequestsMaxPixels0;
      [NativeDisableParallelForRestriction]
      public NativeList<float> m_VTRequestsMaxPixels1;

      public void Execute(int index)
      {
        int activeGroup = index;
        // ISSUE: reference to a compiler-generated field
        int groupIndex = this.m_NativeBatchInstances.GetGroupIndex(activeGroup);
        // ISSUE: reference to a compiler-generated field
        GroupData groupData = this.m_NativeBatchGroups.GetGroupData(groupIndex);
        // ISSUE: reference to a compiler-generated field
        if (groupData.m_LodCount == (byte) 0 | this.m_DisableLods)
        {
          // ISSUE: reference to a compiler-generated method
          this.SingleLod(activeGroup, groupIndex, in groupData);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.MultiLod(activeGroup, groupIndex, in groupData);
        }
      }

      private bool GetLodPropertyIndex(in GroupData groupData, int dataIndex, out int index)
      {
        switch (groupData.m_MeshType)
        {
          case MeshType.Object:
            return groupData.GetPropertyIndex(9 + dataIndex, out index);
          case MeshType.Net:
            return groupData.GetPropertyIndex(14 + dataIndex, out index);
          case MeshType.Lane:
            return groupData.GetPropertyIndex(6 + dataIndex, out index);
          default:
            index = -1;
            return false;
        }
      }

      private unsafe void SingleLod(int activeGroup, int groupIndex, in GroupData groupData)
      {
        // ISSUE: reference to a compiler-generated field
        NativeBatchAccessor<BatchData> batchAccessor = this.m_NativeBatchGroups.GetBatchAccessor(groupIndex);
        // ISSUE: reference to a compiler-generated field
        WriteableCullingAccessor<CullingData> cullingAccessor = this.m_NativeBatchInstances.GetCullingAccessor(activeGroup);
        // ISSUE: reference to a compiler-generated field
        MergeIndexAccessor mergeIndexAccessor = this.m_NativeBatchInstances.GetMergeIndexAccessor(groupIndex);
        WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData> propertyAccessor1 = new WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData>();
        WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData> propertyAccessor2 = new WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData>();
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_UseLodFade)
        {
          int index1;
          // ISSUE: reference to a compiler-generated method
          if (this.GetLodPropertyIndex(in groupData, 0, out index1))
          {
            // ISSUE: reference to a compiler-generated field
            propertyAccessor1 = this.m_NativeBatchInstances.GetPropertyAccessor(activeGroup, index1);
            flag1 = true;
          }
          int index2;
          // ISSUE: reference to a compiler-generated method
          if (this.GetLodPropertyIndex(in groupData, 1, out index2))
          {
            // ISSUE: reference to a compiler-generated field
            propertyAccessor2 = this.m_NativeBatchInstances.GetPropertyAccessor(activeGroup, index2);
            flag1 = true;
          }
        }
        int length = batchAccessor.Length;
        Bounds3 bounds1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        Bounds3 bounds2 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        bool useSecondaryMatrix = false;
        BatchData batchData1 = batchAccessor.GetBatchData(0);
        int2 int2_1 = new int2((int) batchData1.m_MinLod, (int) batchData1.m_ShadowLod);
        int num1 = -1000000;
        int num2 = 0;
        int index3 = 0;
        int num3 = int2_1.x;
        bool flag2 = batchData1.m_VTIndex0 >= 0 | batchData1.m_VTIndex1 >= 0;
        for (int index4 = 1; index4 < length; ++index4)
        {
          BatchData batchData2 = batchAccessor.GetBatchData(index4);
          flag2 |= batchData2.m_VTIndex0 >= 0 | batchData2.m_VTIndex1 >= 0;
          if ((int) batchData2.m_MinLod != num3)
          {
            num3 = (int) batchData2.m_MinLod;
            index3 = index4;
            ++num2;
          }
        }
        int num4 = 1;
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          num4 += this.m_NativeBatchGroups.GetMergedGroupCount(groupIndex);
        }
        float* numPtr = stackalloc float[num4];
        for (int index5 = 0; index5 < num4; ++index5)
          numPtr[index5] = float.MaxValue;
        int managedBatchIndex1 = batchAccessor.GetManagedBatchIndex(index3);
        if (managedBatchIndex1 >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          useSecondaryMatrix = this.m_LoadingState[managedBatchIndex1] < MeshLoadingState.Complete;
        }
        for (int index6 = 0; index6 < cullingAccessor.Length; ++index6)
        {
          ref CullingData local = ref cullingAccessor.Get(index6);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float minDistance = RenderingUtils.CalculateMinDistance(local.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
          // ISSUE: reference to a compiler-generated field
          int lod = RenderingUtils.CalculateLod(minDistance * minDistance, this.m_LodParameters);
          int2 int2_2 = int2_1 + local.lodOffset;
          bool flag3 = !local.isHidden;
          bool2 c = lod >= int2_2 & !local.isFading;
          int4 int4_1 = new int4(num2, num2, math.select(new int2(0), new int2((int) byte.MaxValue), c));
          if (flag1)
          {
            int4 lodFade = local.lodFade;
            if (math.any(lodFade != int4_1))
            {
              lodFade.xy = (int2) num2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              lodFade.zw = math.clamp(lodFade.zw + math.select((int2) -this.m_LodFadeDelta, (int2) this.m_LodFadeDelta, c), (int2) 0, (int2) (int) byte.MaxValue);
              c = lodFade.zw != 0;
              if (c.x)
              {
                int2 int2_3 = math.select(lodFade.zw, lodFade.zw - (int) byte.MaxValue, local.isFading);
                int4 int4_2 = math.select(new int4(int2_3, -int2_3), new int4(-int2_3, int2_3), (((int) groupData.m_LodCount - lodFade.xy & 1) != 0).xyxy);
                num1 = math.max(num1, 0);
                int4_2.xz = 1065353471 - int4_2.xz | (int) byte.MaxValue - int4_2.yw << 11;
                if (propertyAccessor1.Length != 0)
                  propertyAccessor1.SetPropertyValue<int>(int4_2.x, index6);
                if (propertyAccessor2.Length != 0)
                  propertyAccessor2.SetPropertyValue<int>(int4_2.z, index6);
              }
              local.lodFade = lodFade;
            }
          }
          else
            local.lodFade = int4_1;
          int4 int4_3 = (int4) 0;
          bool2 bool2 = c & flag3;
          if (bool2.x)
          {
            int4_3.xy = new int2(num2, num2 + 1);
            bounds1 |= local.m_Bounds;
            if (bool2.y)
            {
              int4_3.zw = new int2(num2, num2 + 1);
              bounds2 |= local.m_Bounds;
            }
            if (flag2)
            {
              int index7 = num4 != 1 ? 1 + mergeIndexAccessor.Get(index6) : 0;
              numPtr[index7] = math.min(numPtr[index7], minDistance);
            }
          }
          local.lodRange = int4_3;
          num1 = math.max(num1, lod - int2_2.x);
        }
        for (int index8 = index3; index8 < length; ++index8)
        {
          if (flag2)
          {
            BatchData batchData3 = batchAccessor.GetBatchData(index8);
            if (batchData3.m_VTIndex0 >= 0 | batchData3.m_VTIndex1 >= 0)
            {
              if ((double) numPtr[0] != 3.4028234663852886E+38)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddVTRequests(in batchData3, numPtr[0]);
              }
              for (int index9 = 1; index9 < num4; ++index9)
              {
                if ((double) numPtr[index9] != 3.4028234663852886E+38)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.AddVTRequests(this.m_NativeBatchGroups.GetBatchData(this.m_NativeBatchGroups.GetMergedGroupIndex(groupIndex, index9 - 1), index8), numPtr[index9]);
                }
              }
            }
          }
          int managedBatchIndex2 = batchAccessor.GetManagedBatchIndex(index8);
          if (managedBatchIndex2 >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            ref int local = ref this.m_BatchPriority.ElementAt(managedBatchIndex2);
            local = math.max(local, num1);
          }
        }
        float3 boundsCenter;
        float3 boundsExtents;
        if ((double) bounds1.min.x != 3.4028234663852886E+38)
        {
          boundsCenter = MathUtils.Center(bounds1);
          boundsExtents = MathUtils.Extents(bounds1);
        }
        else
        {
          boundsCenter = new float3();
          boundsExtents = (float3) float.MinValue;
        }
        float3 shadowBoundsCenter;
        float3 shadowBoundsExtents;
        if ((double) bounds2.min.x != 3.4028234663852886E+38)
        {
          shadowBoundsCenter = MathUtils.Center(bounds2);
          shadowBoundsExtents = MathUtils.Extents(bounds2);
        }
        else
        {
          shadowBoundsCenter = new float3();
          shadowBoundsExtents = (float3) float.MinValue;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_NativeBatchInstances.UpdateCulling(activeGroup, boundsCenter, boundsExtents, shadowBoundsCenter, shadowBoundsExtents, useSecondaryMatrix);
      }

      private unsafe void MultiLod(int activeGroup, int groupIndex, in GroupData groupData)
      {
        // ISSUE: reference to a compiler-generated field
        NativeBatchAccessor<BatchData> batchAccessor = this.m_NativeBatchGroups.GetBatchAccessor(groupIndex);
        // ISSUE: reference to a compiler-generated field
        WriteableCullingAccessor<CullingData> cullingAccessor = this.m_NativeBatchInstances.GetCullingAccessor(activeGroup);
        // ISSUE: reference to a compiler-generated field
        MergeIndexAccessor mergeIndexAccessor = this.m_NativeBatchInstances.GetMergeIndexAccessor(groupIndex);
        WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData> propertyAccessor1 = new WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData>();
        WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData> propertyAccessor2 = new WriteablePropertyAccessor<CullingData, GroupData, BatchData, InstanceData>();
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_UseLodFade)
        {
          int index1;
          // ISSUE: reference to a compiler-generated method
          if (this.GetLodPropertyIndex(in groupData, 0, out index1))
          {
            // ISSUE: reference to a compiler-generated field
            propertyAccessor1 = this.m_NativeBatchInstances.GetPropertyAccessor(activeGroup, index1);
            flag1 = true;
          }
          int index2;
          // ISSUE: reference to a compiler-generated method
          if (this.GetLodPropertyIndex(in groupData, 1, out index2))
          {
            // ISSUE: reference to a compiler-generated field
            propertyAccessor2 = this.m_NativeBatchInstances.GetPropertyAccessor(activeGroup, index2);
            flag1 = true;
          }
        }
        int length = batchAccessor.Length;
        int num1 = 0;
        Bounds3 bounds1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        Bounds3 bounds2 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        bool useSecondaryMatrix = false;
        bool flag2 = false;
        BatchDataSystem.BatchLodJob.LodData* lodDataPtr = stackalloc BatchDataSystem.BatchLodJob.LodData[(int) groupData.m_LodCount + 1];
        ref BatchDataSystem.BatchLodJob.LodData local1 = ref lodDataPtr[0];
        // ISSUE: reference to a compiler-generated field
        local1.m_MinLod = (int2) -1;
        for (int index = 0; index < length; ++index)
        {
          BatchData batchData = batchAccessor.GetBatchData(index);
          flag2 |= batchData.m_VTIndex0 >= 0 | batchData.m_VTIndex1 >= 0;
          // ISSUE: reference to a compiler-generated field
          if ((int) batchData.m_MinLod != local1.m_MinLod.x)
          {
            // ISSUE: explicit reference operation
            local1 = @lodDataPtr[num1++];
            // ISSUE: reference to a compiler-generated field
            local1.m_MinLod = new int2((int) batchData.m_MinLod, (int) batchData.m_ShadowLod);
            // ISSUE: reference to a compiler-generated field
            local1.m_MaxPriority = -1000000;
            // ISSUE: reference to a compiler-generated field
            local1.m_SelectLod = num1 - 1;
          }
          if (num1 != 1)
          {
            int managedBatchIndex = batchAccessor.GetManagedBatchIndex(index);
            // ISSUE: reference to a compiler-generated field
            int selectLod = lodDataPtr[num1 - 2].m_SelectLod;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            local1.m_SelectLod = math.select(num1 - 1, selectLod, local1.m_SelectLod == selectLod || managedBatchIndex >= 0 && this.m_LoadingState[managedBatchIndex] < MeshLoadingState.Complete);
          }
        }
        int num2 = 1;
        int num3 = 1;
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          num2 += this.m_NativeBatchGroups.GetMergedGroupCount(groupIndex);
          num3 = num2 * num1;
        }
        float* numPtr = stackalloc float[num3];
        for (int index = 0; index < num3; ++index)
          numPtr[index] = float.MaxValue;
        int managedBatchIndex1 = batchAccessor.GetManagedBatchIndex(0);
        if (managedBatchIndex1 >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          useSecondaryMatrix = this.m_LoadingState[managedBatchIndex1] < MeshLoadingState.Complete;
        }
        for (int index3 = 0; index3 < cullingAccessor.Length; ++index3)
        {
          ref CullingData local2 = ref cullingAccessor.Get(index3);
          ref BatchDataSystem.BatchLodJob.LodData local3 = ref lodDataPtr[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float minDistance = RenderingUtils.CalculateMinDistance(local2.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
          // ISSUE: reference to a compiler-generated field
          int lod = RenderingUtils.CalculateLod(minDistance * minDistance, this.m_LodParameters);
          bool flag3 = !local2.isHidden;
          // ISSUE: reference to a compiler-generated field
          bool2 c1 = lod >= local3.m_MinLod;
          // ISSUE: reference to a compiler-generated field
          int2 int2_1 = math.select((int2) -1, (int2) local3.m_SelectLod, c1);
          int2 int2_2 = (int2) 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          local3.m_MaxPriority = math.max(local3.m_MaxPriority, lod - local3.m_MinLod.x);
          bool2 c2 = c1 & !local2.isFading;
          for (int index4 = 1; index4 < num1; ++index4)
          {
            // ISSUE: explicit reference operation
            ref BatchDataSystem.BatchLodJob.LodData local4 = @lodDataPtr[index4];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2_1 = math.select(int2_1, (int2) local4.m_SelectLod, lod >= local4.m_MinLod);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            local4.m_MaxPriority = math.max(local4.m_MaxPriority, lod - local4.m_MinLod.x);
          }
          if (flag1)
          {
            int4 int4_1 = new int4(int2_1, math.select(new int2(0), new int2((int) byte.MaxValue), c2));
            int4 int4_2 = local2.lodFade;
            if (math.any(int4_2 != int4_1))
            {
              bool2 bool2;
              if (local2.isFading)
              {
                // ISSUE: reference to a compiler-generated field
                int4_2.zw -= this.m_LodFadeDelta;
                int4_1.xy = math.select((int2) 0, int2_1, int2_1 >= 0);
                int4 a1 = int4_2;
                int4 b1 = int4_1;
                bool2 = int4_2.xy == (int) byte.MaxValue | int4_2.zw <= 0;
                bool4 xyxy1 = bool2.xyxy;
                int4_2 = math.select(a1, b1, xyxy1);
                c2 = int4_2.zw != 0;
                if (c2.x)
                {
                  // ISSUE: explicit reference operation
                  ref BatchDataSystem.BatchLodJob.LodData local5 = @lodDataPtr[int4_2.x];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  local5.m_MaxPriority = math.max(local5.m_MaxPriority, 0);
                  // ISSUE: reference to a compiler-generated field
                  int2_1.x = local5.m_SelectLod;
                  int2_2.x = 1;
                  if (c2.y)
                  {
                    // ISSUE: explicit reference operation
                    ref BatchDataSystem.BatchLodJob.LodData local6 = @lodDataPtr[int4_2.y];
                    // ISSUE: reference to a compiler-generated field
                    int2_1.y = local6.m_SelectLod;
                    int2_2.y = 1;
                  }
                  int2 int2_3 = int4_2.zw - (int) byte.MaxValue;
                  int4 a2 = new int4(int2_3, -int2_3);
                  int4 b2 = new int4(-int2_3, int2_3);
                  bool2 = ((int) groupData.m_LodCount - int4_2.xy & 1) != 0;
                  bool4 xyxy2 = bool2.xyxy;
                  int4 int4_3 = math.select(a2, b2, xyxy2);
                  int4_3.xz = 1065353471 - int4_3.xz | (int) byte.MaxValue - int4_3.yw << 11;
                  if (propertyAccessor1.Length != 0)
                    propertyAccessor1.SetPropertyValue<int>(int4_3.x, index3);
                  if (propertyAccessor2.Length != 0)
                    propertyAccessor2.SetPropertyValue<int>(int4_3.z, index3);
                }
              }
              else
              {
                if (int2_1.x >= int4_2.x)
                {
                  // ISSUE: reference to a compiler-generated field
                  int4_2.z += this.m_LodFadeDelta;
                  int4_2.xz += math.select((int2) 0, new int2(1, -255), int4_2.z >= 256);
                  int4_2.xz = math.select(int4_2.xz, new int2(int2_1.x, (int) byte.MaxValue), int4_2.x > int2_1.x);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  int4_2.z -= this.m_LodFadeDelta;
                  int4_2.xz += math.select((int2) 0, new int2(-1, (int) byte.MaxValue), int4_2.z <= 0);
                  int4_1.xz = math.select((int2) 0, new int2(int2_1.x, (int) byte.MaxValue), c2.x);
                  int4_2.xz = math.select(int4_2.xz, int4_1.xz, int4_2.x == int2_1.x | int4_2.x >= 254);
                }
                if (int2_1.y >= int4_2.y)
                {
                  // ISSUE: reference to a compiler-generated field
                  int4_2.w += this.m_LodFadeDelta;
                  int4_2.yw += math.select((int2) 0, new int2(1, -255), int4_2.w >= 256);
                  int4_2.yw = math.select(int4_2.yw, new int2(int2_1.y, (int) byte.MaxValue), int4_2.y > int2_1.y);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  int4_2.w -= this.m_LodFadeDelta;
                  int4_2.yw += math.select((int2) 0, new int2(-1, (int) byte.MaxValue), int4_2.w <= 0);
                  int4_1.yw = math.select((int2) 0, new int2(int2_1.y, (int) byte.MaxValue), c2.y);
                  int4_2.yw = math.select(int4_2.yw, int4_1.yw, int4_2.y == int2_1.y | int4_2.y >= 254);
                }
                c2 = int4_2.zw != 0;
                if (c2.x)
                {
                  int index5 = int4_2.x - math.select(0, 1, int4_2.z != (int) byte.MaxValue & int4_2.x != 0);
                  // ISSUE: explicit reference operation
                  ref BatchDataSystem.BatchLodJob.LodData local7 = @lodDataPtr[index5];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  local7.m_MaxPriority = math.max(local7.m_MaxPriority, 0);
                  // ISSUE: reference to a compiler-generated field
                  int2_1.x = local7.m_SelectLod;
                  // ISSUE: explicit reference operation
                  ref BatchDataSystem.BatchLodJob.LodData local8 = @lodDataPtr[int4_2.x];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  local8.m_MaxPriority = math.max(local8.m_MaxPriority, 0);
                  // ISSUE: reference to a compiler-generated field
                  int2_2.x = local8.m_SelectLod - int2_1.x + 1;
                  if (c2.y)
                  {
                    int index6 = int4_2.y - math.select(0, 1, int4_2.w != (int) byte.MaxValue & int4_2.y != 0);
                    // ISSUE: explicit reference operation
                    ref BatchDataSystem.BatchLodJob.LodData local9 = @lodDataPtr[index6];
                    // ISSUE: reference to a compiler-generated field
                    int2_1.y = local9.m_SelectLod;
                    // ISSUE: explicit reference operation
                    ref BatchDataSystem.BatchLodJob.LodData local10 = @lodDataPtr[int4_2.y];
                    // ISSUE: reference to a compiler-generated field
                    int2_2.y = local10.m_SelectLod - int2_1.y + 1;
                  }
                  int2 zw = int4_2.zw;
                  int4 a = new int4(zw, -zw);
                  int4 b = new int4(-zw, zw);
                  bool2 = ((int) groupData.m_LodCount - int4_2.xy & 1) != 0;
                  bool4 xyxy = bool2.xyxy;
                  int4 int4_4 = math.select(a, b, xyxy);
                  int4_4.xz = 1065353471 - int4_4.xz | (int) byte.MaxValue - int4_4.yw << 11;
                  if (propertyAccessor1.Length != 0)
                    propertyAccessor1.SetPropertyValue<int>(int4_4.x, index3);
                  if (propertyAccessor2.Length != 0)
                    propertyAccessor2.SetPropertyValue<int>(int4_4.z, index3);
                }
              }
              local2.lodFade = int4_2;
            }
          }
          else
            local2.lodFade = math.select((int4) 0, new int4(int2_1, (int) byte.MaxValue, (int) byte.MaxValue), c2.xyxy);
          int4 int4 = (int4) 0;
          bool2 bool2_1 = c2 & flag3;
          if (bool2_1.x)
          {
            int4.xy = new int2(int2_1.x, int2_1.x + int2_2.x);
            bounds1 |= local2.m_Bounds;
            if (bool2_1.y)
            {
              int4.zw = new int2(int2_1.y, int2_1.y + int2_2.y);
              bounds2 |= local2.m_Bounds;
            }
            if (flag2)
            {
              int num4 = num2 != 1 ? 1 + mergeIndexAccessor.Get(index3) : 0;
              int index7 = int2_1.x * num2 + num4;
              numPtr[index7] = math.min(numPtr[index7], minDistance);
            }
          }
          local2.lodRange = int4;
        }
        int num5 = 1;
        ref BatchDataSystem.BatchLodJob.LodData local11 = ref lodDataPtr[0];
        int index8 = 0;
        for (int index9 = 0; index9 < length; ++index9)
        {
          BatchData batchData = batchAccessor.GetBatchData(index9);
          // ISSUE: reference to a compiler-generated field
          if ((int) batchData.m_MinLod != local11.m_MinLod.x)
          {
            // ISSUE: explicit reference operation
            local11 = @lodDataPtr[num5++];
            index8 += num2;
          }
          if (batchData.m_VTIndex0 >= 0 | batchData.m_VTIndex1 >= 0)
          {
            if ((double) numPtr[index8] != 3.4028234663852886E+38)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddVTRequests(in batchData, numPtr[index8]);
            }
            for (int index10 = 1; index10 < num2; ++index10)
            {
              if ((double) numPtr[index8 + index10] != 3.4028234663852886E+38)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.AddVTRequests(this.m_NativeBatchGroups.GetBatchData(this.m_NativeBatchGroups.GetMergedGroupIndex(groupIndex, index10 - 1), index9), numPtr[index8 + index10]);
              }
            }
          }
          int managedBatchIndex2 = batchAccessor.GetManagedBatchIndex(index9);
          if (managedBatchIndex2 >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            ref int local12 = ref this.m_BatchPriority.ElementAt(managedBatchIndex2);
            // ISSUE: reference to a compiler-generated field
            local12 = math.max(local12, local11.m_MaxPriority);
          }
        }
        float3 boundsCenter;
        float3 boundsExtents;
        if ((double) bounds1.min.x != 3.4028234663852886E+38)
        {
          boundsCenter = MathUtils.Center(bounds1);
          boundsExtents = MathUtils.Extents(bounds1);
        }
        else
        {
          boundsCenter = new float3();
          boundsExtents = (float3) float.MinValue;
        }
        float3 shadowBoundsCenter;
        float3 shadowBoundsExtents;
        if ((double) bounds2.min.x != 3.4028234663852886E+38)
        {
          shadowBoundsCenter = MathUtils.Center(bounds2);
          shadowBoundsExtents = MathUtils.Extents(bounds2);
        }
        else
        {
          shadowBoundsCenter = new float3();
          shadowBoundsExtents = (float3) float.MinValue;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_NativeBatchInstances.UpdateCulling(activeGroup, boundsCenter, boundsExtents, shadowBoundsCenter, shadowBoundsExtents, useSecondaryMatrix);
      }

      private void AddVTRequests(in BatchData batchData, float minDistance)
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = math.atan(batchData.m_VTSizeFactor / minDistance) * this.m_PixelSizeFactor;
        if (batchData.m_VTIndex0 >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          ref float local = ref this.m_VTRequestsMaxPixels0.ElementAt(batchData.m_VTIndex0);
          if ((double) num1 > (double) local)
          {
            float num2 = num1;
            float num3;
            do
            {
              num3 = num2;
              num2 = Interlocked.Exchange(ref local, num3);
            }
            while ((double) num2 > (double) num3);
          }
        }
        if (batchData.m_VTIndex1 < 0)
          return;
        // ISSUE: reference to a compiler-generated field
        ref float local1 = ref this.m_VTRequestsMaxPixels1.ElementAt(batchData.m_VTIndex1);
        if ((double) num1 <= (double) local1)
          return;
        float num4 = num1;
        float num5;
        do
        {
          num5 = num4;
          num4 = Interlocked.Exchange(ref local1, num5);
        }
        while ((double) num4 > (double) num5);
      }

      private struct LodData
      {
        public int2 m_MinLod;
        public int m_MaxPriority;
        public int m_SelectLod;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Error> __Game_Tools_Error_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Warning> __Game_Tools_Warning_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Override> __Game_Tools_Override_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<MeshBatch> __Game_Rendering_MeshBatch_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<FadeBatch> __Game_Rendering_FadeBatch_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshColor> __Game_Rendering_MeshColor_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Animated> __Game_Rendering_Animated_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Emissive> __Game_Rendering_Emissive_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stack> __Game_Objects_Stack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> __Game_Objects_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Surface> __Game_Objects_Surface_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CitizenPresence> __Game_Buildings_CitizenPresence_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeLane> __Game_Net_NodeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneColor> __Game_Net_LaneColor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneCondition> __Game_Net_LaneCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HangingLane> __Game_Net_HangingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeColor> __Game_Net_EdgeColor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeColor> __Game_Net_NodeColor_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubFlow> __Game_Net_SubFlow_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CutRange> __Game_Net_CutRange_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Zones.Cell> __Game_Zones_Cell_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> __Game_Prefabs_GrowthScaleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> __Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.AnimationClip> __Game_Prefabs_AnimationClip_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Error_RO_ComponentLookup = state.GetComponentLookup<Error>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Warning_RO_ComponentLookup = state.GetComponentLookup<Warning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Override_RO_ComponentLookup = state.GetComponentLookup<Override>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshBatch_RO_BufferLookup = state.GetBufferLookup<MeshBatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_FadeBatch_RO_BufferLookup = state.GetBufferLookup<FadeBatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshColor_RO_BufferLookup = state.GetBufferLookup<MeshColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Animated_RO_BufferLookup = state.GetBufferLookup<Animated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RO_BufferLookup = state.GetBufferLookup<Skeleton>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Emissive_RO_BufferLookup = state.GetBufferLookup<Emissive>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentLookup = state.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Surface_RO_ComponentLookup = state.GetComponentLookup<Surface>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CitizenPresence_RO_ComponentLookup = state.GetComponentLookup<CitizenPresence>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeLane_RO_ComponentLookup = state.GetComponentLookup<NodeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneColor_RO_ComponentLookup = state.GetComponentLookup<LaneColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RO_ComponentLookup = state.GetComponentLookup<LaneCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_HangingLane_RO_ComponentLookup = state.GetComponentLookup<HangingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeColor_RO_ComponentLookup = state.GetComponentLookup<EdgeColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeColor_RO_ComponentLookup = state.GetComponentLookup<NodeColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubFlow_RO_BufferLookup = state.GetBufferLookup<SubFlow>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CutRange_RO_BufferLookup = state.GetBufferLookup<CutRange>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Game.Zones.Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup = state.GetComponentLookup<GrowthScaleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationClip_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.AnimationClip>(true);
      }
    }
  }
}
