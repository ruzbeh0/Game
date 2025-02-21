// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ObjectInterpolateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Effects;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class ObjectInterpolateSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private PreCullingSystem m_PreCullingSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EffectControlSystem m_EffectControlSystem;
    private WindSystem m_WindSystem;
    private AnimatedSystem m_AnimatedSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private BatchDataSystem m_BatchDataSystem;
    private EntityQuery m_InterpolateQuery;
    private uint m_PrevFrameIndex;
    private ObjectInterpolateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlSystem = this.World.GetOrCreateSystemManaged<EffectControlSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedSystem = this.World.GetOrCreateSystemManaged<AnimatedSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchDataSystem = this.World.GetOrCreateSystemManaged<BatchDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InterpolateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadWrite<InterpolatedTransform>(),
          ComponentType.ReadWrite<Animated>(),
          ComponentType.ReadWrite<Bone>(),
          ComponentType.ReadWrite<LightState>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Relative>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<IconElement>(),
          ComponentType.ReadWrite<InterpolatedTransform>(),
          ComponentType.ReadOnly<UpdateFrame>(),
          ComponentType.ReadOnly<TransformFrame>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Relative>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> laneSearchTree = this.m_NetSearchSystem.GetLaneSearchTree(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<PreCullingData> cullingData = this.m_PreCullingSystem.GetCullingData(true, out dependencies2);
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<EnabledEffectData> enabledData = this.m_EffectControlSystem.GetEnabledData(true, out dependencies3);
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      AnimatedSystem.AnimationData animationData = this.m_AnimatedSystem.GetAnimationData(out dependencies4);
      float3 float3_1 = new float3();
      float3 float3_2 = new float3();
      float4 float4 = new float4();
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
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Momentum_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Swaying_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LightAnimation_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SwayingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ExtractorFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TrafficLight_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PointOfInterest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies5;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      ObjectInterpolateSystem.UpdateTransformDataJob jobData1 = new ObjectInterpolateSystem.UpdateTransformDataJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_PointOfInterestData = this.__TypeHandle.__Game_Common_PointOfInterest_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_TrafficLightData = this.__TypeHandle.__Game_Objects_TrafficLight_RO_ComponentLookup,
        m_BuildingEfficiencyData = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_BuildingElectricityConsumer = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_BuildingExtractorFacility = this.__TypeHandle.__Game_Buildings_ExtractorFacility_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_CarTrailerData = this.__TypeHandle.__Game_Vehicles_CarTrailer_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_HumanData = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_PrefabSwayingData = this.__TypeHandle.__Game_Prefabs_SwayingData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup,
        m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
        m_EffectInstances = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup,
        m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_AnimationMotions = this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_ProceduralLights = this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup,
        m_LightAnimations = this.__TypeHandle.__Game_Prefabs_LightAnimation_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_ActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup,
        m_SwayingData = this.__TypeHandle.__Game_Rendering_Swaying_RW_ComponentLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup,
        m_Emissives = this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup,
        m_Animateds = this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup,
        m_Momentums = this.__TypeHandle.__Game_Rendering_Momentum_RW_BufferLookup,
        m_Lights = this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup,
        m_PrevFrameIndex = this.m_PrevFrameIndex,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_FrameDelta = this.m_RenderingSystem.frameDelta,
        m_TimeOfDay = this.m_RenderingSystem.timeOfDay,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_RandomSeed = RandomSeed.Next(),
        m_WindData = this.m_WindSystem.GetData(true, out dependencies5),
        m_LaneSearchTree = laneSearchTree,
        m_CullingData = cullingData,
        m_EnabledData = enabledData,
        m_AnimationData = animationData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Momentum_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Swaying_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LightAnimation_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SwayingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PointOfInterest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      ObjectInterpolateSystem.UpdateTrailerTransformDataJob jobData2 = new ObjectInterpolateSystem.UpdateTrailerTransformDataJob()
      {
        m_PointOfInterestData = this.__TypeHandle.__Game_Common_PointOfInterest_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabSwayingData = this.__TypeHandle.__Game_Prefabs_SwayingData_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabCarTractorData = this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentLookup,
        m_PrefabCarTrailerData = this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_BogieFrames = this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RO_BufferLookup,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_ProceduralLights = this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup,
        m_LightAnimations = this.__TypeHandle.__Game_Prefabs_LightAnimation_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup,
        m_SwayingData = this.__TypeHandle.__Game_Rendering_Swaying_RW_ComponentLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup,
        m_Emissive = this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup,
        m_Momentums = this.__TypeHandle.__Game_Rendering_Momentum_RW_BufferLookup,
        m_LightStates = this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_FrameDelta = this.m_RenderingSystem.frameDelta,
        m_RandomSeed = RandomSeed.Next(),
        m_LaneSearchTree = laneSearchTree,
        m_CullingData = cullingData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Animation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Static_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ObjectInterpolateSystem.UpdateQueryTransformDataJob jobData3 = new ObjectInterpolateSystem.UpdateQueryTransformDataJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle,
        m_StaticType = this.__TypeHandle.__Game_Objects_Static_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_AnimationType = this.__TypeHandle.__Game_Tools_Animation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle,
        m_MeshGroupType = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle,
        m_EffectInstancesType = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferTypeHandle,
        m_IconElementType = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_ProceduralLights = this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup,
        m_Animateds = this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup,
        m_Emissives = this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup,
        m_Lights = this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_CullingData = cullingData,
        m_EnabledData = enabledData,
        m_AnimationData = animationData
      };
      JobHandle jobHandle1 = jobData1.Schedule<ObjectInterpolateSystem.UpdateTransformDataJob, PreCullingData>(cullingData, 16, JobUtils.CombineDependencies(this.Dependency, dependencies1, dependencies2, dependencies3, dependencies5, dependencies4));
      JobHandle jobHandle2 = jobData2.Schedule<ObjectInterpolateSystem.UpdateTrailerTransformDataJob, PreCullingData>(cullingData, 16, jobHandle1);
      // ISSUE: reference to a compiler-generated field
      EntityQuery interpolateQuery = this.m_InterpolateQuery;
      JobHandle dependsOn = jobHandle2;
      JobHandle jobHandle3 = jobData3.ScheduleParallel<ObjectInterpolateSystem.UpdateQueryTransformDataJob>(interpolateQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem.AddReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddLaneSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_EffectControlSystem.AddEnabledDataReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AnimatedSystem.AddAnimationWriter(jobHandle3);
      this.Dependency = jobHandle3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrevFrameIndex = this.m_RenderingSystem.frameIndex;
    }

    private static void UpdateSwaying(
      SwayingData swayingData,
      InterpolatedTransform oldTransform,
      ref InterpolatedTransform newTransform,
      ref Swaying swaying,
      float deltaTime,
      float speedDeltaFactor,
      out quaternion swayRotation,
      out float swayOffset)
    {
      if ((double) deltaTime != 0.0)
      {
        float3 float3_1 = oldTransform.m_Position - math.mul(oldTransform.m_Rotation, new float3(0.0f, swaying.m_SwayPosition.y, 0.0f));
        float3 float3_2 = (newTransform.m_Position - float3_1) * speedDeltaFactor;
        float3 float3_3 = math.mul(math.inverse(newTransform.m_Rotation), float3_2 - swaying.m_LastVelocity);
        swaying.m_SwayVelocity += float3_3 * swayingData.m_VelocityFactors - swaying.m_SwayPosition * swayingData.m_SpringFactors * deltaTime;
        swaying.m_SwayVelocity *= math.pow(swayingData.m_DampingFactors, (float3) deltaTime);
        swaying.m_SwayPosition += swaying.m_SwayVelocity * deltaTime;
        swaying.m_SwayVelocity = math.select(swaying.m_SwayVelocity, (float3) 0.0f, swaying.m_SwayPosition >= swayingData.m_MaxPosition & swaying.m_SwayVelocity >= 0.0f | swaying.m_SwayPosition <= -swayingData.m_MaxPosition & swaying.m_SwayVelocity <= 0.0f);
        swaying.m_SwayPosition = math.clamp(swaying.m_SwayPosition, -swayingData.m_MaxPosition, swayingData.m_MaxPosition);
        swaying.m_LastVelocity = float3_2;
      }
      float2 xz = swaying.m_SwayPosition.xz;
      if (MathUtils.TryNormalize(ref xz))
      {
        swayRotation = quaternion.AxisAngle(new float3(-xz.y, 0.0f, xz.x), math.length(swaying.m_SwayPosition.xz));
        newTransform.m_Rotation = math.mul(newTransform.m_Rotation, swayRotation);
        swayRotation = math.inverse(swayRotation);
      }
      else
        swayRotation = quaternion.identity;
      newTransform.m_Position += math.mul(newTransform.m_Rotation, new float3(0.0f, swaying.m_SwayPosition.y, 0.0f));
      swayOffset = -swaying.m_SwayPosition.y;
    }

    private static float CalculateSteeringRadius(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      InterpolatedTransform oldTransform,
      InterpolatedTransform newTransform,
      ref Skeleton skeleton,
      CarData carData)
    {
      float a1 = float.PositiveInfinity;
      float num1 = -1f;
      float num2 = 0.0f;
      for (int index1 = 0; index1 < proceduralBones.Length; ++index1)
      {
        ProceduralBone proceduralBone1 = proceduralBones[index1];
        int index2 = skeleton.m_BoneOffset + index1;
        ref Bone local = ref bones.ElementAt(index2);
        float3 world1;
        float3 world2;
        ProceduralBone proceduralBone2;
        float num3;
        switch (proceduralBone1.m_Type)
        {
          case BoneType.SteeringTire:
            world1 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
            world2 = ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
            proceduralBone2 = proceduralBone1;
            num3 = math.asin(math.mul(math.mul(math.inverse(proceduralBone1.m_Rotation), local.m_Rotation), math.left()).z);
            break;
          case BoneType.SteeringRotation:
            int childIndex1;
            // ISSUE: reference to a compiler-generated method
            if (ObjectInterpolateSystem.FindChildBone(proceduralBones, index1, out childIndex1))
            {
              proceduralBone2 = proceduralBones[childIndex1];
              int childIndex2;
              // ISSUE: reference to a compiler-generated method
              if (ObjectInterpolateSystem.FindChildBone(proceduralBones, childIndex1, out childIndex2))
                proceduralBone2 = proceduralBones[childIndex2];
              world1 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone2.m_ObjectPosition);
              world2 = ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone2.m_ObjectPosition);
              num3 = math.asin(math.mul(math.mul(math.inverse(proceduralBone1.m_Rotation), local.m_Rotation), math.right()).y);
              break;
            }
            continue;
          case BoneType.SteeringSuspension:
            int childIndex3;
            // ISSUE: reference to a compiler-generated method
            if (ObjectInterpolateSystem.FindChildBone(proceduralBones, index1, out childIndex3))
            {
              proceduralBone2 = proceduralBones[childIndex3];
              int childIndex4;
              // ISSUE: reference to a compiler-generated method
              if (ObjectInterpolateSystem.FindChildBone(proceduralBones, childIndex3, out childIndex4))
                proceduralBone2 = proceduralBones[childIndex4];
              world1 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone2.m_ObjectPosition);
              world2 = ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone2.m_ObjectPosition);
              num3 = math.asin(math.mul(math.mul(math.inverse(proceduralBone1.m_Rotation), local.m_Rotation), math.left()).z);
              break;
            }
            continue;
          default:
            continue;
        }
        float2 x1 = new float2(proceduralBone2.m_ObjectPosition.x, proceduralBone2.m_ObjectPosition.z - carData.m_PivotOffset);
        x1.y *= 0.5f;
        num2 = math.max(num2, math.csum(math.abs(x1)));
        float3 x2 = world2 - world1;
        float3 float3 = math.mul(newTransform.m_Rotation, math.right());
        float3 y1 = math.forward(newTransform.m_Rotation);
        float num4 = math.dot(x2, y1);
        float num5 = math.dot(x2, float3);
        float num6 = num4 + math.select(1f / 1000f, -1f / 1000f, (double) num4 < 0.0);
        float3 a2 = math.normalizesafe(y1 * num6 + float3 * num5);
        float3 y2 = math.select(a2, -a2, (double) num6 < 0.0);
        float num7 = math.abs(math.dot(x2, y2));
        if ((double) num7 > (double) num1)
        {
          num1 = num7;
          float num8 = math.asin(math.dot(float3, y2));
          float b = num7 / math.max(0.01f, proceduralBone2.m_ObjectPosition.y * 2f);
          float x3 = num3 + math.clamp(num8 - num3, -b, b);
          a1 = (proceduralBone2.m_ObjectPosition.z - carData.m_PivotOffset) / math.tan(x3) + proceduralBone2.m_ObjectPosition.x;
        }
      }
      float a3 = math.select(a1, num2, (double) a1 < (double) num2 & (double) a1 >= 0.0);
      return math.select(a3, -num2, (double) a3 > -(double) num2 & (double) a3 < 0.0);
    }

    private static void AnimateInterpolatedBone(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      DynamicBuffer<Momentum> momentums,
      InterpolatedTransform oldTransform,
      InterpolatedTransform newTransform,
      PrefabRef prefabRef,
      ref Skeleton skeleton,
      quaternion swayRotation,
      float swayOffset,
      float steeringRadius,
      float pivotOffset,
      int index,
      float deltaTime,
      Entity entity,
      bool instantReset,
      uint frameIndex,
      float frameTime,
      ref Random random,
      ref ComponentLookup<PointOfInterest> pointOfInterests,
      ref ComponentLookup<Curve> curveDatas,
      ref ComponentLookup<PrefabRef> prefabRefDatas,
      ref ComponentLookup<UtilityLaneData> prefabUtilityLaneDatas,
      ref ComponentLookup<ObjectGeometryData> prefabObjectGeometryDatas,
      ref NativeQuadTree<Entity, QuadTreeBoundsXZ> laneSearchTree)
    {
      ProceduralBone proceduralBone1 = proceduralBones[index];
      Momentum momentum = new Momentum();
      int index1 = skeleton.m_BoneOffset + index;
      ref Bone local1 = ref bones.ElementAt(index1);
      ref Momentum local2 = ref momentum;
      if (momentums.IsCreated)
        local2 = ref momentums.ElementAt(index1);
      switch (proceduralBone1.m_Type)
      {
        case BoneType.RollingTire:
          float3 world1 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
          double num1 = (double) math.dot(ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone1.m_ObjectPosition) - world1, math.forward(newTransform.m_Rotation)) / (double) math.max(0.01f, proceduralBone1.m_ObjectPosition.y);
          float2 yz1 = math.forward(math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation)).yz;
          double num2 = (double) math.atan2(yz1.x, yz1.y);
          float angle1 = (float) (num1 - num2);
          float3 rhs1 = math.mul(swayRotation, proceduralBone1.m_Position);
          rhs1.y += swayOffset;
          quaternion x1 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateX(angle1));
          skeleton.m_CurrentUpdated |= !local1.m_Position.Equals(rhs1) | !local1.m_Rotation.Equals(x1);
          local1.m_Position = rhs1;
          local1.m_Rotation = x1;
          break;
        case BoneType.SteeringTire:
          float3 world2 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
          float3 x2 = ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone1.m_ObjectPosition) - world2;
          float3 float3_1 = math.mul(newTransform.m_Rotation, math.right());
          float3 y1 = math.forward(newTransform.m_Rotation);
          float num3 = math.dot(x2, y1);
          float num4 = math.dot(x2, float3_1);
          float num5 = num3 + math.select(1f / 1000f, -1f / 1000f, (double) num3 < 0.0);
          float3 a1 = math.normalizesafe(y1 * num5 + float3_1 * num4);
          float3 y2 = math.select(a1, -a1, (double) num5 < 0.0);
          double num6 = (double) math.dot(x2, y2) / (double) math.max(0.01f, proceduralBone1.m_ObjectPosition.y);
          quaternion q = math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation);
          float3 float3_2 = math.forward(q);
          float a2 = math.length(float3_2.xz);
          float x3 = math.select(a2, -a2, (double) float3_2.z < 0.0);
          double num7 = (double) math.atan2(float3_2.y, x3);
          float angle2 = (float) (num6 - num7);
          float angle3;
          if ((double) steeringRadius == 0.0)
          {
            float num8 = math.asin(math.dot(float3_1, y2));
            float num9 = math.asin(math.mul(q, math.left()).z);
            float b = math.length(x2) / math.max(0.01f, proceduralBone1.m_ObjectPosition.y);
            angle3 = num9 + math.clamp(num8 - num9, -b, b);
          }
          else
            angle3 = math.atan((float) (((double) proceduralBone1.m_ObjectPosition.z - (double) pivotOffset) / ((double) steeringRadius - (double) proceduralBone1.m_ObjectPosition.x)));
          float3 rhs2 = math.mul(swayRotation, proceduralBone1.m_Position);
          rhs2.y += swayOffset;
          quaternion x4 = math.mul(proceduralBone1.m_Rotation, math.mul(quaternion.RotateY(angle3), quaternion.RotateX(angle2)));
          skeleton.m_CurrentUpdated |= !local1.m_Position.Equals(rhs2) | !local1.m_Rotation.Equals(x4);
          local1.m_Position = rhs2;
          local1.m_Rotation = x4;
          break;
        case BoneType.PoweredRotation:
        case BoneType.OperatingRotation:
          float speed = proceduralBone1.m_Speed;
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, speed, deltaTime, instantReset);
          break;
        case BoneType.SuspensionMovement:
          int childIndex1;
          // ISSUE: reference to a compiler-generated method
          if (!ObjectInterpolateSystem.FindChildBone(proceduralBones, index, out childIndex1))
            break;
          ProceduralBone proceduralBone2 = proceduralBones[childIndex1];
          float3 position1 = proceduralBone1.m_Position;
          position1.z += math.mul(swayRotation, proceduralBone2.m_ObjectPosition).y - proceduralBone2.m_ObjectPosition.y;
          position1.z += swayOffset;
          skeleton.m_CurrentUpdated |= !local1.m_Position.Equals(position1);
          local1.m_Position = position1;
          break;
        case BoneType.SteeringRotation:
          int childIndex2;
          // ISSUE: reference to a compiler-generated method
          if (!ObjectInterpolateSystem.FindChildBone(proceduralBones, index, out childIndex2))
            break;
          ProceduralBone proceduralBone3 = proceduralBones[childIndex2];
          int childIndex3;
          // ISSUE: reference to a compiler-generated method
          if (ObjectInterpolateSystem.FindChildBone(proceduralBones, childIndex2, out childIndex3))
            proceduralBone3 = proceduralBones[childIndex3];
          float angle4;
          if ((double) steeringRadius == 0.0)
          {
            float3 world3 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone3.m_ObjectPosition);
            float3 x5 = ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone3.m_ObjectPosition) - world3;
            float3 float3_3 = math.mul(newTransform.m_Rotation, math.right());
            float3 y3 = math.forward(newTransform.m_Rotation);
            float num10 = math.dot(x5, y3);
            float num11 = math.dot(x5, float3_3);
            float num12 = num10 + math.select(1f / 1000f, -1f / 1000f, (double) num10 < 0.0);
            float3 a3 = math.normalizesafe(y3 * num12 + float3_3 * num11);
            float3 y4 = math.select(a3, -a3, (double) num12 < 0.0);
            float num13 = math.asin(math.dot(float3_3, y4));
            float num14 = math.asin(math.mul(math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation), math.right()).y);
            float b = math.length(x5) / math.max(0.01f, proceduralBone3.m_ObjectPosition.y);
            angle4 = num14 + math.clamp(num13 - num14, -b, b);
          }
          else
            angle4 = math.atan((float) (((double) proceduralBone3.m_ObjectPosition.z - (double) pivotOffset) / ((double) steeringRadius - (double) proceduralBone3.m_ObjectPosition.x)));
          quaternion x6 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateZ(angle4));
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x6);
          local1.m_Rotation = x6;
          break;
        case BoneType.SuspensionRotation:
          int childIndex4;
          // ISSUE: reference to a compiler-generated method
          if (!ObjectInterpolateSystem.FindChildBone(proceduralBones, index, out childIndex4))
            break;
          ProceduralBone proceduralBone4 = proceduralBones[childIndex4];
          float angle5 = -math.atan((math.mul(swayRotation, proceduralBone4.m_ObjectPosition).y - proceduralBone4.m_ObjectPosition.y + swayOffset) / proceduralBone4.m_Position.z);
          quaternion x7 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateX(angle5));
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x7);
          local1.m_Rotation = x7;
          break;
        case BoneType.FixedRotation:
          ProceduralBone proceduralBone5 = proceduralBones[proceduralBone1.m_ParentIndex];
          Bone bone = bones.ElementAt(skeleton.m_BoneOffset + proceduralBone1.m_ParentIndex);
          // ISSUE: reference to a compiler-generated method
          quaternion x8 = math.mul(math.inverse(ObjectInterpolateSystem.LocalToObject(proceduralBones, bones, skeleton, proceduralBone5.m_ParentIndex, bone.m_Rotation)), proceduralBone1.m_ObjectRotation);
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x8);
          local1.m_Rotation = x8;
          break;
        case BoneType.FixedTire:
          float3 world4 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
          // ISSUE: reference to a compiler-generated method
          double num15 = (double) math.dot(ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone1.m_ObjectPosition) - world4, math.normalizesafe(math.cross(math.rotate(ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, newTransform.ToTransform(), skeleton, proceduralBone1.m_ParentIndex, proceduralBone1.m_Rotation), math.right()), math.rotate(newTransform.m_Rotation, math.up())))) / (double) math.max(0.01f, proceduralBone1.m_ObjectPosition.y);
          float2 yz2 = math.forward(math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation)).yz;
          double num16 = (double) math.atan2(yz2.x, yz2.y);
          float angle6 = (float) (num15 - num16);
          quaternion x9 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateX(angle6));
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x9);
          local1.m_Rotation = x9;
          break;
        case BoneType.DebugMovement:
          float3 position2 = proceduralBone1.m_Position;
          float x10 = (float) (((double) (frameIndex & (uint) byte.MaxValue) + (double) frameTime) * (3.0 / 128.0));
          if ((double) x10 < 1.0)
            position2.x += math.smoothstep(0.0f, 1f, x10);
          else if ((double) x10 < 2.0)
            position2.x += math.smoothstep(2f, 1f, x10);
          else if ((double) x10 < 3.0)
            position2.y += math.smoothstep(2f, 3f, x10);
          else if ((double) x10 < 4.0)
            position2.y += math.smoothstep(4f, 3f, x10);
          else if ((double) x10 < 5.0)
            position2.z += math.smoothstep(4f, 5f, x10);
          else
            position2.z += math.smoothstep(6f, 5f, x10);
          skeleton.m_CurrentUpdated |= !local1.m_Position.Equals(position2);
          local1.m_Position = position2;
          break;
        case BoneType.RollingRotation:
          float3 world5 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
          // ISSUE: reference to a compiler-generated method
          double num17 = (double) math.dot(ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone1.m_ObjectPosition) - world5, math.normalizesafe(math.cross(math.rotate(ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, newTransform.ToTransform(), skeleton, proceduralBone1.m_ParentIndex, proceduralBone1.m_Rotation), math.right()), math.rotate(newTransform.m_Rotation, math.up())))) * (double) proceduralBone1.m_Speed;
          float2 yz3 = math.forward(math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation)).yz;
          double num18 = (double) math.atan2(yz3.x, yz3.y);
          float angle7 = (float) (num17 - num18);
          quaternion x11 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateX(angle7));
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x11);
          local1.m_Rotation = x11;
          break;
        case BoneType.PropellerRotation:
          float3 world6 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
          // ISSUE: reference to a compiler-generated method
          double num19 = (double) math.dot(ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone1.m_ObjectPosition) - world6, math.rotate(ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, newTransform.ToTransform(), skeleton, proceduralBone1.m_ParentIndex, proceduralBone1.m_Rotation), math.up())) * (double) proceduralBone1.m_Speed;
          float2 xz = math.forward(math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation)).xz;
          double num20 = (double) math.atan2(xz.x, xz.y);
          float angle8 = (float) (num19 + num20);
          quaternion x12 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateY(angle8));
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x12);
          local1.m_Rotation = x12;
          break;
        case BoneType.LookAtRotation:
        case BoneType.LookAtRotationSide:
          PointOfInterest componentData1;
          if (pointOfInterests.TryGetComponent(entity, out componentData1) && componentData1.m_IsValid)
          {
            float3 position3 = proceduralBone1.m_Position;
            quaternion rotation = proceduralBone1.m_Rotation;
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, newTransform.ToTransform(), skeleton, proceduralBone1.m_ParentIndex, ref position3, ref rotation);
            float3 v = componentData1.m_Position - position3;
            float3 a4 = math.mul(math.inverse(rotation), v);
            a4.xz = math.select(a4.xz, MathUtils.Right(a4.xz), proceduralBone1.m_Type == BoneType.LookAtRotationSide);
            a4 = math.select(a4, -a4, (double) proceduralBone1.m_Speed < 0.0);
            float targetSpeed = math.abs(proceduralBone1.m_Speed);
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, a4.xz, targetSpeed, deltaTime, instantReset);
            break;
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, 0.0f, deltaTime, instantReset);
          break;
        case BoneType.LookAtAim:
          PointOfInterest componentData2;
          if (pointOfInterests.TryGetComponent(entity, out componentData2) && componentData2.m_IsValid)
          {
            float3 position4 = proceduralBone1.m_Position;
            quaternion rotation = proceduralBone1.m_Rotation;
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.LookAtLocalToWorld(proceduralBones, bones, newTransform.ToTransform(), skeleton, componentData2, proceduralBone1.m_ParentIndex, ref position4, ref rotation);
            float3 v = componentData2.m_Position - position4;
            float3 a5 = math.mul(math.inverse(rotation), v);
            float3 float3_4 = math.select(a5, -a5, (double) proceduralBone1.m_Speed < 0.0);
            float targetSpeed = math.abs(proceduralBone1.m_Speed);
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneX(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, float3_4.yz, targetSpeed, deltaTime, instantReset);
            break;
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateRotatingBoneX(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, 0.0f, deltaTime, instantReset);
          break;
        case BoneType.PropellerAngle:
          float3 world7 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone1.m_ObjectPosition);
          float3 x13 = ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone1.m_ObjectPosition) - world7;
          float3 float3_5 = math.mul(newTransform.m_Rotation, math.right());
          float3 float3_6 = math.forward(newTransform.m_Rotation);
          float num21 = math.dot(x13, float3_6);
          float num22 = math.dot(x13, float3_5);
          float num23 = num21 + math.select(1f / 1000f, -1f / 1000f, (double) num21 < 0.0);
          float3 y5 = math.normalizesafe(float3_6 * num23 + float3_5 * num22);
          double num24 = (double) math.atan2(math.dot(float3_5, y5), math.dot(float3_6, y5));
          float3 float3_7 = math.mul(local1.m_Rotation, math.forward());
          float num25 = math.atan2(float3_7.x, float3_7.z);
          float b1 = math.length(x13) * proceduralBone1.m_Speed;
          double num26 = (double) num25;
          float a6 = (float) (num24 - num26);
          float a7 = math.select(a6, a6 - 3.14159274f, (double) a6 > 3.1415927410125732);
          float x14 = math.select(a7, a7 + 3.14159274f, (double) a7 < -3.1415927410125732);
          quaternion x15 = math.mul(quaternion.RotateY(num25 + math.clamp(x14, -b1, b1)), proceduralBone1.m_Rotation);
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x15);
          local1.m_Rotation = x15;
          break;
        case BoneType.PantographRotation:
          bool active = (newTransform.m_Flags & TransformFlags.Pantograph) > (TransformFlags) 0;
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimatePantographBone(proceduralBones, bones, newTransform.ToTransform(), prefabRef, proceduralBone1, ref skeleton, ref local1, index, deltaTime, active, instantReset, ref curveDatas, ref prefabRefDatas, ref prefabUtilityLaneDatas, ref prefabObjectGeometryDatas, ref laneSearchTree);
          break;
        case BoneType.SteeringSuspension:
          int childIndex5;
          // ISSUE: reference to a compiler-generated method
          if (!ObjectInterpolateSystem.FindChildBone(proceduralBones, index, out childIndex5))
            break;
          ProceduralBone proceduralBone6 = proceduralBones[childIndex5];
          int childIndex6;
          // ISSUE: reference to a compiler-generated method
          if (ObjectInterpolateSystem.FindChildBone(proceduralBones, childIndex5, out childIndex6))
            proceduralBone6 = proceduralBones[childIndex6];
          float angle9;
          if ((double) steeringRadius == 0.0)
          {
            float3 world8 = ObjectUtils.LocalToWorld(oldTransform.ToTransform(), proceduralBone6.m_ObjectPosition);
            float3 x16 = ObjectUtils.LocalToWorld(newTransform.ToTransform(), proceduralBone6.m_ObjectPosition) - world8;
            float3 float3_8 = math.mul(newTransform.m_Rotation, math.right());
            float3 y6 = math.forward(newTransform.m_Rotation);
            float num27 = math.dot(x16, y6);
            float num28 = math.dot(x16, float3_8);
            float num29 = num27 + math.select(1f / 1000f, -1f / 1000f, (double) num27 < 0.0);
            float3 a8 = math.normalizesafe(y6 * num29 + float3_8 * num28);
            float3 y7 = math.select(a8, -a8, (double) num29 < 0.0);
            float num30 = math.asin(math.dot(float3_8, y7));
            float num31 = math.asin(math.mul(math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation), math.left()).z);
            float b2 = math.length(x16) / math.max(0.01f, proceduralBone6.m_ObjectPosition.y);
            angle9 = num31 + math.clamp(num30 - num31, -b2, b2);
          }
          else
            angle9 = math.atan((float) (((double) proceduralBone6.m_ObjectPosition.z - (double) pivotOffset) / ((double) steeringRadius - (double) proceduralBone6.m_ObjectPosition.x)));
          quaternion x17 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateY(angle9));
          float3 position5 = proceduralBone1.m_Position;
          position5.z += math.mul(swayRotation, proceduralBone6.m_ObjectPosition).y - proceduralBone6.m_ObjectPosition.y;
          position5.z += swayOffset;
          skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x17) | !local1.m_Position.Equals(position5);
          local1.m_Rotation = x17;
          local1.m_Position = position5;
          break;
      }
    }

    private static void AnimatePantographBone(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      Transform transform,
      PrefabRef prefabRef,
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      int index,
      float deltaTime,
      bool active,
      bool instantReset,
      ref ComponentLookup<Curve> curveDatas,
      ref ComponentLookup<PrefabRef> prefabRefDatas,
      ref ComponentLookup<UtilityLaneData> prefabUtilityLaneDatas,
      ref ComponentLookup<ObjectGeometryData> prefabObjectGeometryDatas,
      ref NativeQuadTree<Entity, QuadTreeBoundsXZ> laneSearchTree)
    {
      ProceduralBone proceduralBone1 = proceduralBones[proceduralBone.m_ParentIndex];
      quaternion x1;
      if (proceduralBone1.m_Type == BoneType.PantographRotation)
      {
        Bone bone1 = bones.ElementAt(skeleton.m_BoneOffset + proceduralBone.m_ParentIndex);
        quaternion quaternion = math.mul(math.inverse(proceduralBone1.m_Rotation), bone1.m_Rotation);
        quaternion.value.x = -quaternion.value.x;
        x1 = math.mul(math.mul(quaternion, quaternion), proceduralBone.m_Rotation);
      }
      else
      {
        int childIndex1;
        // ISSUE: reference to a compiler-generated method
        if (ObjectInterpolateSystem.FindChildBone(proceduralBones, index, out childIndex1))
        {
          float num1 = 0.0f;
          if (active)
          {
            ProceduralBone proceduralBone2 = proceduralBones[childIndex1];
            ObjectGeometryData objectGeometryData = prefabObjectGeometryDatas[prefabRef.m_Prefab];
            float3 objectPosition = proceduralBone.m_ObjectPosition with
            {
              y = objectGeometryData.m_Bounds.max.y
            };
            float3 world = ObjectUtils.LocalToWorld(transform, objectPosition);
            float x2 = math.length(proceduralBone2.m_Position.yz);
            int childIndex2;
            // ISSUE: reference to a compiler-generated method
            if (proceduralBone2.m_Type == BoneType.PantographRotation && ObjectInterpolateSystem.FindChildBone(proceduralBones, childIndex1, out childIndex2))
            {
              ProceduralBone proceduralBone3 = proceduralBones[childIndex2];
              x2 += math.length(proceduralBone3.m_Position.yz);
            }
            float defaultHeight = x2 * 0.382683426f;
            // ISSUE: reference to a compiler-generated method
            float a = math.asin(math.min(0.9f, ObjectInterpolateSystem.FindCatenaryHeight(world, transform.m_Rotation, defaultHeight, ref curveDatas, ref prefabRefDatas, ref prefabUtilityLaneDatas, ref laneSearchTree) / math.max(x2, 1f / 1000f)));
            num1 = math.select(a, -a, (double) proceduralBone2.m_Position.z > 0.0);
          }
          float2 yz = math.forward(math.mul(math.inverse(proceduralBone.m_Rotation), bone.m_Rotation)).yz;
          float num2 = -math.atan2(yz.x, yz.y);
          float num3 = proceduralBone.m_Speed * deltaTime;
          float angle = math.select(math.clamp(num1, num2 - num3, num2 + num3), num1, instantReset);
          x1 = math.mul(proceduralBone.m_Rotation, quaternion.RotateX(angle));
        }
        else
          x1 = bone.m_Rotation;
      }
      skeleton.m_CurrentUpdated |= !bone.m_Rotation.Equals(x1);
      bone.m_Rotation = x1;
    }

    private static float FindCatenaryHeight(
      float3 position,
      quaternion rotation,
      float defaultHeight,
      ref ComponentLookup<Curve> curveDatas,
      ref ComponentLookup<PrefabRef> prefabRefDatas,
      ref ComponentLookup<UtilityLaneData> prefabUtilityLaneDatas,
      ref NativeQuadTree<Entity, QuadTreeBoundsXZ> laneSearchTree)
    {
      Line3.Segment line = new Line3.Segment(position, position + math.mul(rotation, new float3(0.0f, defaultHeight * 2f, 0.0f)));
      float3 float3 = MathUtils.Position(line, 0.5f);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ObjectInterpolateSystem.CatenaryIterator iterator = new ObjectInterpolateSystem.CatenaryIterator()
      {
        m_Bounds = new Bounds3(float3 - defaultHeight, float3 + defaultHeight),
        m_Line = line,
        m_Result = (float3) 1000f,
        m_Default = defaultHeight,
        m_CurveData = curveDatas,
        m_PrefabRefData = prefabRefDatas,
        m_PrefabUtilityLaneData = prefabUtilityLaneDatas
      };
      laneSearchTree.Iterate<ObjectInterpolateSystem.CatenaryIterator>(ref iterator);
      // ISSUE: reference to a compiler-generated field
      curveDatas = iterator.m_CurveData;
      // ISSUE: reference to a compiler-generated field
      prefabRefDatas = iterator.m_PrefabRefData;
      // ISSUE: reference to a compiler-generated field
      prefabUtilityLaneDatas = iterator.m_PrefabUtilityLaneData;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return math.lerp(iterator.m_Result.x, defaultHeight, math.min(1f, iterator.m_Result.y / (defaultHeight * 0.5f)));
    }

    private static bool FindChildBone(
      DynamicBuffer<ProceduralBone> proceduralBones,
      int index,
      out int childIndex)
    {
      for (int index1 = 0; index1 < proceduralBones.Length; ++index1)
      {
        if (proceduralBones[index1].m_ParentIndex == index)
        {
          childIndex = index1;
          return true;
        }
      }
      childIndex = -1;
      return false;
    }

    private static void AnimateMovingBone(
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      ref Momentum momentum,
      float3 moveDirection,
      float targetOffset,
      float targetSpeed,
      float deltaTime,
      bool instantReset)
    {
      float3 position = proceduralBone.m_Position;
      float3 rhs;
      if (instantReset)
      {
        rhs = position + moveDirection * targetOffset;
        momentum.m_Momentum = 0.0f;
      }
      else
      {
        float num1 = math.dot(bone.m_Position - position, moveDirection);
        float num2 = targetOffset - num1;
        targetSpeed = math.select(targetSpeed, -targetSpeed, (double) num2 < 0.0);
        float b1 = math.sqrt(math.abs(num2 * proceduralBone.m_Acceleration));
        targetSpeed = math.clamp(targetSpeed, -b1, b1);
        float x = targetSpeed - momentum.m_Momentum;
        float b2 = math.abs(deltaTime * proceduralBone.m_Acceleration);
        momentum.m_Momentum += math.clamp(x, -b2, b2);
        rhs = position + moveDirection * (num1 + momentum.m_Momentum * deltaTime);
      }
      skeleton.m_CurrentUpdated |= !bone.m_Position.Equals(rhs);
      bone.m_Position = rhs;
    }

    private static void AnimateRotatingBoneY(
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      ref Momentum momentum,
      ref Random random,
      float2 targetDir,
      float targetSpeed,
      float deltaTime,
      bool instantReset)
    {
      float angle;
      if (instantReset)
      {
        angle = !MathUtils.TryNormalize(ref targetDir) ? random.NextFloat(-3.14159274f, 3.14159274f) : MathUtils.RotationAngleSignedRight(math.forward().xz, targetDir);
        momentum.m_Momentum = 0.0f;
      }
      else
      {
        float2 xz = math.forward(math.mul(math.inverse(proceduralBone.m_Rotation), bone.m_Rotation)).xz;
        if (MathUtils.TryNormalize(ref xz) && MathUtils.TryNormalize(ref targetDir))
        {
          float num = MathUtils.RotationAngleSignedRight(xz, targetDir);
          targetSpeed = math.select(targetSpeed, -targetSpeed, (double) num < 0.0);
          float b = math.sqrt(math.abs(num * proceduralBone.m_Acceleration));
          targetSpeed = math.clamp(targetSpeed, -b, b);
        }
        else
          targetSpeed = 0.0f;
        float x = targetSpeed - momentum.m_Momentum;
        float b1 = math.abs(deltaTime * proceduralBone.m_Acceleration);
        momentum.m_Momentum += math.clamp(x, -b1, b1);
        angle = math.atan2(xz.x, xz.y) + momentum.m_Momentum * deltaTime;
      }
      quaternion x1 = math.mul(proceduralBone.m_Rotation, quaternion.RotateY(angle));
      skeleton.m_CurrentUpdated |= !bone.m_Rotation.Equals(x1);
      bone.m_Rotation = x1;
    }

    private static void AnimateRotatingBoneZ(
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      ref Momentum momentum,
      ref Random random,
      float2 targetDir,
      float targetSpeed,
      float deltaTime,
      bool instantReset)
    {
      float angle;
      if (instantReset)
      {
        angle = !MathUtils.TryNormalize(ref targetDir) ? random.NextFloat(-3.14159274f, 3.14159274f) : MathUtils.RotationAngleSignedLeft(math.up().xy, targetDir);
        momentum.m_Momentum = 0.0f;
      }
      else
      {
        float2 xy = math.rotate(math.mul(math.inverse(proceduralBone.m_Rotation), bone.m_Rotation), math.up()).xy;
        if (MathUtils.TryNormalize(ref xy) && MathUtils.TryNormalize(ref targetDir))
        {
          float num = MathUtils.RotationAngleSignedLeft(xy, targetDir);
          targetSpeed = math.select(targetSpeed, -targetSpeed, (double) num < 0.0);
          float b = math.sqrt(math.abs(num * proceduralBone.m_Acceleration));
          targetSpeed = math.clamp(targetSpeed, -b, b);
        }
        else
          targetSpeed = 0.0f;
        float x = targetSpeed - momentum.m_Momentum;
        float b1 = math.abs(deltaTime * proceduralBone.m_Acceleration);
        momentum.m_Momentum += math.clamp(x, -b1, b1);
        angle = math.atan2(-xy.x, xy.y) + momentum.m_Momentum * deltaTime;
      }
      quaternion x1 = math.mul(proceduralBone.m_Rotation, quaternion.RotateZ(angle));
      skeleton.m_CurrentUpdated |= !bone.m_Rotation.Equals(x1);
      bone.m_Rotation = x1;
    }

    private static void AnimateRotatingBoneX(
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      ref Momentum momentum,
      ref Random random,
      float2 targetDir,
      float targetSpeed,
      float deltaTime,
      bool instantReset)
    {
      float angle;
      if (instantReset)
      {
        angle = !MathUtils.TryNormalize(ref targetDir) ? random.NextFloat(-3.14159274f, 3.14159274f) : MathUtils.RotationAngleSignedLeft(math.up().yz, targetDir);
        momentum.m_Momentum = 0.0f;
      }
      else
      {
        float2 yz = math.rotate(math.mul(math.inverse(proceduralBone.m_Rotation), bone.m_Rotation), math.up()).yz;
        if (MathUtils.TryNormalize(ref yz) && MathUtils.TryNormalize(ref targetDir))
        {
          float num = MathUtils.RotationAngleSignedLeft(yz, targetDir);
          targetSpeed = math.select(targetSpeed, -targetSpeed, (double) num < 0.0);
          float b = math.sqrt(math.abs(num * proceduralBone.m_Acceleration));
          targetSpeed = math.clamp(targetSpeed, -b, b);
        }
        else
          targetSpeed = 0.0f;
        float x = targetSpeed - momentum.m_Momentum;
        float b1 = math.abs(deltaTime * proceduralBone.m_Acceleration);
        momentum.m_Momentum += math.clamp(x, -b1, b1);
        angle = math.atan2(yz.y, yz.x) + momentum.m_Momentum * deltaTime;
      }
      quaternion x1 = math.mul(proceduralBone.m_Rotation, quaternion.RotateX(angle));
      skeleton.m_CurrentUpdated |= !bone.m_Rotation.Equals(x1);
      bone.m_Rotation = x1;
    }

    private static void AnimateRotatingBoneY(
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      ref Momentum momentum,
      ref Random random,
      float targetSpeed,
      float deltaTime,
      bool instantReset)
    {
      float angle;
      if (instantReset)
      {
        momentum.m_Momentum = targetSpeed;
        angle = random.NextFloat(-3.14159274f, 3.14159274f);
      }
      else
      {
        float x = targetSpeed - momentum.m_Momentum;
        float b = math.abs(deltaTime * proceduralBone.m_Acceleration);
        momentum.m_Momentum += math.clamp(x, -b, b);
        float2 xz = math.forward(math.mul(math.inverse(proceduralBone.m_Rotation), bone.m_Rotation)).xz;
        angle = math.atan2(xz.x, xz.y) + momentum.m_Momentum * deltaTime;
      }
      quaternion x1 = math.mul(proceduralBone.m_Rotation, quaternion.RotateY(angle));
      skeleton.m_CurrentUpdated |= !bone.m_Rotation.Equals(x1);
      bone.m_Rotation = x1;
    }

    private static void AnimateRotatingBoneZ(
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      ref Momentum momentum,
      ref Random random,
      float targetSpeed,
      float deltaTime,
      bool instantReset)
    {
      float angle;
      if (instantReset)
      {
        momentum.m_Momentum = targetSpeed;
        angle = random.NextFloat(-3.14159274f, 3.14159274f);
      }
      else
      {
        float x = targetSpeed - momentum.m_Momentum;
        float b = math.abs(deltaTime * proceduralBone.m_Acceleration);
        momentum.m_Momentum += math.clamp(x, -b, b);
        float2 xy = math.rotate(math.mul(math.inverse(proceduralBone.m_Rotation), bone.m_Rotation), math.up()).xy;
        angle = math.atan2(-xy.x, xy.y) + momentum.m_Momentum * deltaTime;
      }
      quaternion x1 = math.mul(proceduralBone.m_Rotation, quaternion.RotateZ(angle));
      skeleton.m_CurrentUpdated |= !bone.m_Rotation.Equals(x1);
      bone.m_Rotation = x1;
    }

    private static void AnimateRotatingBoneX(
      ProceduralBone proceduralBone,
      ref Skeleton skeleton,
      ref Bone bone,
      ref Momentum momentum,
      ref Random random,
      float targetSpeed,
      float deltaTime,
      bool instantReset)
    {
      float angle;
      if (instantReset)
      {
        momentum.m_Momentum = targetSpeed;
        angle = random.NextFloat(-3.14159274f, 3.14159274f);
      }
      else
      {
        float x = targetSpeed - momentum.m_Momentum;
        float b = math.abs(deltaTime * proceduralBone.m_Acceleration);
        momentum.m_Momentum += math.clamp(x, -b, b);
        float2 yz = math.rotate(math.mul(math.inverse(proceduralBone.m_Rotation), bone.m_Rotation), math.up()).yz;
        angle = math.atan2(yz.y, yz.x) + momentum.m_Momentum * deltaTime;
      }
      quaternion x1 = math.mul(proceduralBone.m_Rotation, quaternion.RotateX(angle));
      skeleton.m_CurrentUpdated |= !bone.m_Rotation.Equals(x1);
      bone.m_Rotation = x1;
    }

    private static void AnimateInterpolatedLight(
      DynamicBuffer<ProceduralLight> proceduralLights,
      DynamicBuffer<LightAnimation> lightAnimations,
      DynamicBuffer<LightState> lights,
      TransformFlags transformFlags,
      Random pseudoRandom,
      ref Emissive emissive,
      int index,
      uint frame,
      float frameTime,
      float deltaTime,
      bool instantReset)
    {
      ProceduralLight proceduralLight = proceduralLights[index];
      int index1 = emissive.m_LightOffset + index;
      ref LightState local = ref lights.ElementAt(index1);
      switch (proceduralLight.m_Purpose)
      {
        case EmissiveProperties.Purpose.DaytimeRunningLight:
        case EmissiveProperties.Purpose.DaytimeRunningLightAlt:
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, 1f, instantReset);
          break;
        case EmissiveProperties.Purpose.Headlight_HighBeam:
        case EmissiveProperties.Purpose.LandingLights:
          float targetIntensity1 = math.select(0.0f, 1f, (transformFlags & TransformFlags.ExtraLights) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity1, instantReset);
          break;
        case EmissiveProperties.Purpose.Headlight_LowBeam:
        case EmissiveProperties.Purpose.TaxiLights:
        case EmissiveProperties.Purpose.SearchLightsFront:
          float targetIntensity2 = math.select(0.0f, 1f, (transformFlags & TransformFlags.MainLights) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity2, instantReset);
          break;
        case EmissiveProperties.Purpose.TurnSignalLeft:
          float targetIntensity3 = 0.0f;
          if ((transformFlags & TransformFlags.TurningLeft) != (TransformFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            targetIntensity3 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, frame, frameTime, 1f);
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity3, instantReset);
          break;
        case EmissiveProperties.Purpose.TurnSignalRight:
          float targetIntensity4 = 0.0f;
          if ((transformFlags & TransformFlags.TurningRight) != (TransformFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            targetIntensity4 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, frame, frameTime, 1f);
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity4, instantReset);
          break;
        case EmissiveProperties.Purpose.RearLight:
          float targetIntensity5 = math.select(0.0f, 1f, (transformFlags & TransformFlags.RearLights) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity5, instantReset);
          break;
        case EmissiveProperties.Purpose.BrakeLight:
          float targetIntensity6 = math.select(0.0f, 1f, (transformFlags & TransformFlags.Braking) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity6, instantReset);
          break;
        case EmissiveProperties.Purpose.ReverseLight:
          float targetIntensity7 = math.select(0.0f, 1f, (transformFlags & TransformFlags.Reversing) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity7, instantReset);
          break;
        case EmissiveProperties.Purpose.Clearance:
        case EmissiveProperties.Purpose.Dashboard:
        case EmissiveProperties.Purpose.Clearance2:
        case EmissiveProperties.Purpose.MarkerLights:
        case EmissiveProperties.Purpose.WingInspectionLights:
        case EmissiveProperties.Purpose.LogoLights:
        case EmissiveProperties.Purpose.PositionLightLeft:
        case EmissiveProperties.Purpose.PositionLightRight:
        case EmissiveProperties.Purpose.PositionLights:
        case EmissiveProperties.Purpose.NumberLight:
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, 1f, instantReset);
          break;
        case EmissiveProperties.Purpose.DaytimeRunningLightLeft:
          float targetIntensity8 = math.select(1f, 0.0f, (transformFlags & TransformFlags.TurningLeft) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity8, instantReset);
          break;
        case EmissiveProperties.Purpose.DaytimeRunningLightRight:
          float targetIntensity9 = math.select(1f, 0.0f, (transformFlags & TransformFlags.TurningRight) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity9, instantReset);
          break;
        case EmissiveProperties.Purpose.SignalGroup1:
        case EmissiveProperties.Purpose.SignalGroup2:
        case EmissiveProperties.Purpose.SignalGroup3:
        case EmissiveProperties.Purpose.SignalGroup4:
        case EmissiveProperties.Purpose.SignalGroup5:
        case EmissiveProperties.Purpose.SignalGroup6:
        case EmissiveProperties.Purpose.SignalGroup7:
        case EmissiveProperties.Purpose.SignalGroup8:
        case EmissiveProperties.Purpose.SignalGroup9:
        case EmissiveProperties.Purpose.SignalGroup10:
        case EmissiveProperties.Purpose.SignalGroup11:
          SignalGroupMask signalGroupMask = (SignalGroupMask) (1U << (int) (proceduralLight.m_Purpose - 12 & EmissiveProperties.Purpose.Dashboard));
          float targetIntensity10 = 0.0f;
          if ((transformFlags & (TransformFlags.SignalAnimation1 | TransformFlags.SignalAnimation2)) != (TransformFlags) 0)
          {
            int signalAnimationIndex = (0 | ((transformFlags & TransformFlags.SignalAnimation1) != (TransformFlags) 0 ? 1 : 0) | ((transformFlags & TransformFlags.SignalAnimation2) != (TransformFlags) 0 ? 2 : 0)) - 1;
            // ISSUE: reference to a compiler-generated method
            targetIntensity10 = ObjectInterpolateSystem.AnimateIntensity(signalGroupMask, signalAnimationIndex, lightAnimations, pseudoRandom, frame, frameTime, 1f);
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity10, instantReset);
          break;
        case EmissiveProperties.Purpose.Interior1:
        case EmissiveProperties.Purpose.Interior2:
          float targetIntensity11 = math.select(0.0f, 3f / 1000f, (transformFlags & TransformFlags.InteriorLights) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity11, instantReset);
          break;
        case EmissiveProperties.Purpose.NeonSign:
        case EmissiveProperties.Purpose.DecorativeLight:
          // ISSUE: reference to a compiler-generated method
          float targetIntensity12 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, frame, frameTime, 1f);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity12, instantReset);
          break;
        case EmissiveProperties.Purpose.Emergency1:
        case EmissiveProperties.Purpose.Emergency2:
        case EmissiveProperties.Purpose.Emergency3:
        case EmissiveProperties.Purpose.Emergency4:
        case EmissiveProperties.Purpose.Emergency5:
        case EmissiveProperties.Purpose.Emergency6:
        case EmissiveProperties.Purpose.RearAlarmLights:
        case EmissiveProperties.Purpose.FrontAlarmLightsLeft:
        case EmissiveProperties.Purpose.FrontAlarmLightsRight:
        case EmissiveProperties.Purpose.Warning1:
        case EmissiveProperties.Purpose.Warning2:
        case EmissiveProperties.Purpose.Emergency7:
        case EmissiveProperties.Purpose.Emergency8:
        case EmissiveProperties.Purpose.Emergency9:
        case EmissiveProperties.Purpose.Emergency10:
        case EmissiveProperties.Purpose.AntiCollisionLightsRed:
        case EmissiveProperties.Purpose.AntiCollisionLightsWhite:
          float targetIntensity13 = 0.0f;
          if ((transformFlags & TransformFlags.WarningLights) != (TransformFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            targetIntensity13 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, frame, frameTime, 1f);
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity13, instantReset);
          break;
        case EmissiveProperties.Purpose.CollectionLights:
        case EmissiveProperties.Purpose.TaxiSign:
        case EmissiveProperties.Purpose.WorkLights:
        case EmissiveProperties.Purpose.SearchLights360:
          float targetIntensity14 = math.select(0.0f, 1f, (transformFlags & TransformFlags.WorkLights) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity14, instantReset);
          break;
        case EmissiveProperties.Purpose.BrakeAndTurnSignalLeft:
          float targetIntensity15 = math.select(0.0f, 1f, (transformFlags & TransformFlags.Braking) > (TransformFlags) 0);
          if ((transformFlags & TransformFlags.TurningLeft) != (TransformFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            targetIntensity15 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, frame, frameTime, 1f);
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity15, instantReset);
          break;
        case EmissiveProperties.Purpose.BrakeAndTurnSignalRight:
          float targetIntensity16 = math.select(0.0f, 1f, (transformFlags & TransformFlags.Braking) > (TransformFlags) 0);
          if ((transformFlags & TransformFlags.TurningRight) != (TransformFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            targetIntensity16 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, frame, frameTime, 1f);
          }
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity16, instantReset);
          break;
        case EmissiveProperties.Purpose.BoardingLightLeft:
          float y1 = math.select(1f, 0.0f, (transformFlags & TransformFlags.BoardingLeft) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, new float2(1f, y1), instantReset);
          break;
        case EmissiveProperties.Purpose.BoardingLightRight:
          float y2 = math.select(1f, 0.0f, (transformFlags & TransformFlags.BoardingRight) > (TransformFlags) 0);
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, new float2(1f, y2), instantReset);
          break;
      }
    }

    private static float AnimateIntensity(
      ProceduralLight proceduralLight,
      DynamicBuffer<LightAnimation> lightAnimations,
      Random pseudoRandom,
      uint frame,
      float frameTime,
      float intensity)
    {
      if (proceduralLight.m_AnimationIndex >= 0 && lightAnimations.IsCreated)
      {
        LightAnimation lightAnimation = lightAnimations[proceduralLight.m_AnimationIndex];
        float num = (float) ((frame + pseudoRandom.NextUInt(lightAnimation.m_DurationFrames)) % lightAnimation.m_DurationFrames) + frameTime;
        intensity *= lightAnimation.m_AnimationCurve.Evaluate(num / (float) lightAnimation.m_DurationFrames);
      }
      return intensity;
    }

    private static float AnimateIntensity(
      SignalGroupMask signalGroupMask,
      int signalAnimationIndex,
      DynamicBuffer<LightAnimation> lightAnimations,
      Random pseudoRandom,
      uint frame,
      float frameTime,
      float intensity)
    {
      if (signalAnimationIndex >= 0 && lightAnimations.IsCreated)
      {
        LightAnimation lightAnimation = lightAnimations[signalAnimationIndex];
        float num = (float) ((frame + pseudoRandom.NextUInt(lightAnimation.m_DurationFrames)) % lightAnimation.m_DurationFrames) + frameTime;
        intensity *= lightAnimation.m_SignalAnimation.Evaluate(signalGroupMask, num / (float) lightAnimation.m_DurationFrames);
      }
      return intensity;
    }

    public static void AnimateLight(
      ProceduralLight proceduralLight,
      ref Emissive emissive,
      ref LightState light,
      float deltaTime,
      float targetIntensity,
      bool instantReset)
    {
      float num1 = math.abs(deltaTime) * proceduralLight.m_ResponseSpeed;
      float num2 = math.select(math.clamp(targetIntensity, light.m_Intensity - num1, light.m_Intensity + num1), targetIntensity, instantReset);
      emissive.m_Updated |= (double) light.m_Intensity != (double) num2;
      light.m_Intensity = num2;
    }

    public static void AnimateLight(
      ProceduralLight proceduralLight,
      ref Emissive emissive,
      ref LightState light,
      float deltaTime,
      float2 target,
      bool instantReset)
    {
      float2 float2_1 = new float2(light.m_Intensity, light.m_Color);
      float num = math.abs(deltaTime) * proceduralLight.m_ResponseSpeed;
      float2 float2_2 = math.select(math.clamp(target, float2_1 - num, float2_1 + num), target, instantReset);
      emissive.m_Updated |= math.any(float2_2 != float2_1);
      light.m_Intensity = float2_2.x;
      light.m_Color = float2_2.y;
    }

    public static void UpdateInterpolatedAnimation(
      DynamicBuffer<AnimationClip> clips,
      InterpolatedTransform oldTransform,
      InterpolatedTransform newTransform,
      ref Animated animated,
      float stateTimer,
      TransformState state,
      ActivityType activity,
      float updateFrameToSeconds,
      float speedDeltaFactor)
    {
      AnimationClip clip = clips[(int) animated.m_ClipIndexBody0];
      float3 y = math.forward(newTransform.m_Rotation);
      float movementDelta = math.dot(newTransform.m_Position - oldTransform.m_Position, y);
      Game.Prefabs.AnimationType type;
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.GetClipType(clip, state, movementDelta, speedDeltaFactor, out type, ref activity);
      if (clip.m_Type != type || clip.m_Activity != activity || clip.m_Layer != AnimationLayer.Body)
      {
        int index;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.FindAnimationClip(clips, type, activity, AnimationLayer.Body, new AnimatedPropID(-1), (ActivityCondition) 0, out clip, out index);
        animated.m_ClipIndexBody0 = (short) index;
        animated.m_Time.x = 0.0f;
      }
      animated.m_PreviousTime = animated.m_Time.x;
      if ((double) clip.m_MovementSpeed != 0.0)
        animated.m_Time.x += movementDelta / clip.m_MovementSpeed;
      else
        animated.m_Time.x = stateTimer * updateFrameToSeconds;
    }

    public static void UpdateInterpolatedAnimationBody(
      Entity entity,
      in CharacterElement characterElement,
      DynamicBuffer<AnimationClip> clips,
      ref ComponentLookup<Human> humanLookup,
      ref ComponentLookup<CurrentVehicle> currentVehicleLookup,
      ref ComponentLookup<PrefabRef> prefabRefLookup,
      ref BufferLookup<ActivityLocationElement> activityLocationLookup,
      ref BufferLookup<AnimationMotion> motionLookup,
      InterpolatedTransform oldTransform,
      InterpolatedTransform newTransform,
      ref Animated animated,
      ref Random random,
      TransformFrame frame0,
      TransformFrame frame1,
      float framePosition,
      float updateFrameToSeconds,
      float speedDeltaFactor,
      float deltaTime,
      int updateFrameChanged,
      bool instantReset)
    {
      float3 y = math.forward(newTransform.m_Rotation);
      float movementDelta = math.dot(newTransform.m_Position - oldTransform.m_Position, y);
      if (instantReset)
      {
        AnimationClip clip = clips[(int) animated.m_ClipIndexBody0];
        ActivityType activity = (ActivityType) frame1.m_Activity;
        Game.Prefabs.AnimationType type;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.GetClipType(clip, frame1.m_State, movementDelta, speedDeltaFactor, out type, ref activity);
        // ISSUE: reference to a compiler-generated method
        AnimatedPropID propId = ObjectInterpolateSystem.GetPropID(entity, activity, ref currentVehicleLookup, ref prefabRefLookup, ref activityLocationLookup);
        // ISSUE: reference to a compiler-generated method
        ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
        int index;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.FindAnimationClip(clips, type, activity, AnimationLayer.Body, propId, activityConditions, out clip, out index);
        animated.m_ClipIndexBody0 = (short) index;
        animated.m_ClipIndexBody0I = (short) -1;
        animated.m_ClipIndexBody1 = (short) -1;
        animated.m_ClipIndexBody1I = (short) -1;
        // ISSUE: reference to a compiler-generated method
        animated.m_MovementSpeed = new float2(ObjectInterpolateSystem.GetMovementSpeed(in characterElement, in clip, ref motionLookup), 0.0f);
        animated.m_Time.xy = (float2) 0.0f;
        if ((double) animated.m_MovementSpeed.x != 0.0 || frame1.m_State == TransformState.Idle)
          animated.m_Time.x = random.NextFloat(clip.m_AnimationLength);
      }
      else if (updateFrameChanged > 0)
      {
        AnimationClip clip;
        if (animated.m_ClipIndexBody1 != (short) -1)
        {
          clip = clips[(int) animated.m_ClipIndexBody1];
          animated.m_ClipIndexBody0 = animated.m_ClipIndexBody1;
          animated.m_ClipIndexBody0I = animated.m_ClipIndexBody1I;
          animated.m_Time.x = animated.m_Time.y;
          animated.m_MovementSpeed.x = animated.m_MovementSpeed.y;
        }
        else
          clip = clips[(int) animated.m_ClipIndexBody0];
        ActivityType activity = (ActivityType) frame1.m_Activity;
        Game.Prefabs.AnimationType type;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.GetClipType(clip, frame1.m_State, movementDelta, speedDeltaFactor, out type, ref activity);
        // ISSUE: reference to a compiler-generated method
        AnimatedPropID propId = ObjectInterpolateSystem.GetPropID(entity, activity, ref currentVehicleLookup, ref prefabRefLookup, ref activityLocationLookup);
        animated.m_ClipIndexBody1 = (short) -1;
        animated.m_ClipIndexBody1I = (short) -1;
        animated.m_MovementSpeed.y = 0.0f;
        animated.m_Time.y = 0.0f;
        if (clip.m_Type != type || clip.m_Activity != activity || clip.m_PropID != propId)
        {
          // ISSUE: reference to a compiler-generated method
          ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
          float animationLength = clip.m_AnimationLength;
          int index;
          // ISSUE: reference to a compiler-generated method
          if (ObjectInterpolateSystem.FindAnimationClip(clips, type, activity, AnimationLayer.Body, propId, activityConditions, out clip, out index))
          {
            animated.m_ClipIndexBody1 = (short) index;
            animated.m_ClipIndexBody1I = (short) -1;
            // ISSUE: reference to a compiler-generated method
            animated.m_MovementSpeed.y = ObjectInterpolateSystem.GetMovementSpeed(in characterElement, in clip, ref motionLookup);
            // ISSUE: reference to a compiler-generated method
            animated.m_Time.y = ObjectInterpolateSystem.GetInitialTime(ref random, in clip, animated.m_MovementSpeed.y, animationLength, animated.m_MovementSpeed.x, animated.m_Time.x);
          }
        }
      }
      else if (updateFrameChanged < 0)
      {
        AnimationClip clip = clips[(int) animated.m_ClipIndexBody0];
        ActivityType activity = (ActivityType) frame0.m_Activity;
        Game.Prefabs.AnimationType type;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.GetClipType(clip, frame0.m_State, movementDelta, speedDeltaFactor, out type, ref activity);
        // ISSUE: reference to a compiler-generated method
        AnimatedPropID propId = ObjectInterpolateSystem.GetPropID(entity, activity, ref currentVehicleLookup, ref prefabRefLookup, ref activityLocationLookup);
        animated.m_ClipIndexBody1 = (short) -1;
        animated.m_ClipIndexBody1I = (short) -1;
        animated.m_Time.y = 0.0f;
        animated.m_MovementSpeed.y = 0.0f;
        if (clip.m_Type != type || clip.m_Activity != activity || clip.m_PropID != propId)
        {
          // ISSUE: reference to a compiler-generated method
          ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
          float animationLength = clip.m_AnimationLength;
          int index;
          // ISSUE: reference to a compiler-generated method
          if (ObjectInterpolateSystem.FindAnimationClip(clips, type, activity, AnimationLayer.Body, propId, activityConditions, out clip, out index))
          {
            animated.m_ClipIndexBody1 = animated.m_ClipIndexBody0;
            animated.m_ClipIndexBody1I = animated.m_ClipIndexBody0I;
            animated.m_MovementSpeed.y = animated.m_MovementSpeed.x;
            animated.m_Time.y = animated.m_Time.x;
            animated.m_ClipIndexBody0 = (short) index;
            animated.m_ClipIndexBody0I = (short) -1;
            // ISSUE: reference to a compiler-generated method
            animated.m_MovementSpeed.x = ObjectInterpolateSystem.GetMovementSpeed(in characterElement, in clip, ref motionLookup);
            // ISSUE: reference to a compiler-generated method
            animated.m_Time.x = ObjectInterpolateSystem.GetInitialTime(ref random, in clip, animated.m_MovementSpeed.x, animationLength, animated.m_MovementSpeed.y, animated.m_Time.y);
          }
        }
      }
      if (animated.m_ClipIndexBody1 != (short) -1)
      {
        if (math.all(animated.m_MovementSpeed != 0.0f))
        {
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.SynchronizeMovementTime(clips, ref animated, movementDelta, framePosition);
          return;
        }
        if ((double) animated.m_MovementSpeed.y != 0.0)
          animated.m_Time.y += movementDelta / animated.m_MovementSpeed.y;
        else if (clips[(int) animated.m_ClipIndexBody1].m_Type == Game.Prefabs.AnimationType.Idle)
          animated.m_Time.y += deltaTime;
        else
          animated.m_Time.y = (float) ((double) frame1.m_StateTimer + (double) framePosition - 1.0) * updateFrameToSeconds;
      }
      if ((double) animated.m_MovementSpeed.x != 0.0)
        animated.m_Time.x += movementDelta / animated.m_MovementSpeed.x;
      else if (clips[(int) animated.m_ClipIndexBody0].m_Type == Game.Prefabs.AnimationType.Idle)
        animated.m_Time.x += deltaTime;
      else
        animated.m_Time.x = ((float) frame0.m_StateTimer + framePosition) * updateFrameToSeconds;
    }

    public static float GetUpdateFrameTransition(float framePosition)
    {
      float num = framePosition * framePosition;
      return (float) (3.0 * (double) num - 2.0 * (double) num * (double) framePosition);
    }

    public static void SynchronizeMovementTime(
      DynamicBuffer<AnimationClip> clips,
      ref Animated animated,
      float movementDelta,
      float framePosition)
    {
      float2 float2_1 = new float2(clips[(int) animated.m_ClipIndexBody0].m_AnimationLength, clips[(int) animated.m_ClipIndexBody1].m_AnimationLength);
      float2 float2_2 = movementDelta / (animated.m_MovementSpeed * float2_1);
      animated.m_Time.xy += math.lerp(float2_2.x, float2_2.y, framePosition) * float2_1;
    }

    public static float GetInitialTime(
      ref Random random,
      in AnimationClip clip,
      float movementSpeed,
      float prevClipLength,
      float prevMovementSpeed,
      float prevTime)
    {
      if ((double) movementSpeed != 0.0 && (double) prevMovementSpeed != 0.0 && (double) prevClipLength != 0.0)
        return prevTime / prevClipLength * clip.m_AnimationLength;
      switch (clip.m_Playback)
      {
        case AnimationPlayback.RandomLoop:
          return random.NextFloat(clip.m_AnimationLength);
        case AnimationPlayback.HalfLoop:
          return math.select(0.0f, clip.m_AnimationLength * 0.5f, random.NextBool());
        default:
          return 0.0f;
      }
    }

    public static void UpdateInterpolatedAnimationFace(
      Entity entity,
      DynamicBuffer<AnimationClip> clips,
      ref ComponentLookup<Human> humanLookup,
      ref Animated animated,
      ref Random random,
      TransformState state,
      ActivityType activity,
      float deltaTime,
      int updateFrameChanged,
      bool instantReset)
    {
      if (instantReset)
      {
        // ISSUE: reference to a compiler-generated method
        ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
        AnimationClip clip;
        int index;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.FindAnimationClip(clips, Game.Prefabs.AnimationType.Idle, ActivityType.None, AnimationLayer.Facial, new AnimatedPropID(-1), activityConditions, out clip, out index);
        animated.m_ClipIndexFace0 = (short) index;
        animated.m_Time.z = random.NextFloat(clip.m_AnimationLength);
      }
      else if (updateFrameChanged > 0)
      {
        AnimationClip clip;
        if (animated.m_ClipIndexFace1 != (short) -1)
        {
          clip = clips[(int) animated.m_ClipIndexFace1];
          animated.m_ClipIndexFace0 = animated.m_ClipIndexFace1;
          animated.m_Time.z = animated.m_Time.w;
        }
        else
          clip = clips[(int) animated.m_ClipIndexFace0];
        // ISSUE: reference to a compiler-generated method
        ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
        animated.m_ClipIndexFace1 = (short) -1;
        animated.m_Time.w = 0.0f;
        int index;
        // ISSUE: reference to a compiler-generated method
        if (((clip.m_Conditions ^ activityConditions) & (ActivityCondition.Angry | ActivityCondition.Sad | ActivityCondition.Happy | ActivityCondition.Waiting)) != (ActivityCondition) 0 && ObjectInterpolateSystem.FindAnimationClip(clips, Game.Prefabs.AnimationType.Idle, ActivityType.None, AnimationLayer.Facial, new AnimatedPropID(-1), activityConditions, out clip, out index))
        {
          animated.m_ClipIndexFace1 = (short) index;
          animated.m_Time.w = random.NextFloat(clip.m_AnimationLength);
        }
      }
      else if (updateFrameChanged < 0)
      {
        AnimationClip clip = clips[(int) animated.m_ClipIndexFace0];
        // ISSUE: reference to a compiler-generated method
        ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
        animated.m_ClipIndexFace1 = (short) -1;
        animated.m_Time.w = 0.0f;
        int index;
        // ISSUE: reference to a compiler-generated method
        if (((clip.m_Conditions ^ activityConditions) & (ActivityCondition.Angry | ActivityCondition.Sad | ActivityCondition.Happy | ActivityCondition.Waiting)) != (ActivityCondition) 0 && ObjectInterpolateSystem.FindAnimationClip(clips, Game.Prefabs.AnimationType.Idle, ActivityType.None, AnimationLayer.Facial, new AnimatedPropID(-1), activityConditions, out clip, out index))
        {
          animated.m_ClipIndexFace1 = animated.m_ClipIndexFace0;
          animated.m_Time.w = animated.m_Time.z;
          animated.m_ClipIndexFace0 = (short) index;
          animated.m_Time.z = random.NextFloat(clip.m_AnimationLength);
        }
      }
      animated.m_Time.zw += deltaTime;
      animated.m_Time.w = 0.0f;
    }

    public static void CalculateUpdateFrames(
      uint simulationFrameIndex,
      float simulationFrameTime,
      uint updateFrameIndex,
      out uint updateFrame1,
      out uint updateFrame2,
      out float framePosition)
    {
      uint num = (uint) ((int) simulationFrameIndex - (int) updateFrameIndex - 32);
      updateFrame1 = num >> 4 & 3U;
      updateFrame2 = (uint) ((int) updateFrame1 + 1 & 3);
      framePosition = (float) (((double) (num & 15U) + (double) simulationFrameTime) * (1.0 / 16.0));
    }

    public static void CalculateUpdateFrames(
      uint simulationFrameIndex,
      uint prevSimulationFrameIndex,
      float simulationFrameTime,
      uint updateFrameIndex,
      out uint updateFrame1,
      out uint updateFrame2,
      out float framePosition,
      out int updateFrameChanged)
    {
      uint num1 = (uint) ((int) simulationFrameIndex - (int) updateFrameIndex - 32);
      int num2 = (int) prevSimulationFrameIndex - (int) updateFrameIndex - 32;
      updateFrame1 = num1 >> 4;
      uint num3 = (uint) (num2 >>> 4);
      updateFrameChanged = math.select(0, math.select(-1, 1, updateFrame1 > num3), (int) updateFrame1 != (int) num3);
      updateFrame1 &= 3U;
      updateFrame2 = (uint) ((int) updateFrame1 + 1 & 3);
      framePosition = (float) (((double) (num1 & 15U) + (double) simulationFrameTime) * (1.0 / 16.0));
    }

    public static InterpolatedTransform CalculateTransform(
      TransformFrame frame1,
      TransformFrame frame2,
      float framePosition)
    {
      Bezier4x3 curve = new Bezier4x3(frame1.m_Position, frame1.m_Position + frame1.m_Velocity * 0.08888889f, frame2.m_Position - frame2.m_Velocity * 0.08888889f, frame2.m_Position);
      InterpolatedTransform transform;
      transform.m_Position = MathUtils.Position(curve, framePosition);
      transform.m_Rotation = math.slerp(frame1.m_Rotation, frame2.m_Rotation, framePosition);
      transform.m_Flags = (double) framePosition >= 0.5 ? frame2.m_Flags : frame1.m_Flags;
      return transform;
    }

    private static quaternion LocalToWorld(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      Transform transform,
      Skeleton skeleton,
      int index,
      quaternion rotation)
    {
      for (; index >= 0; index = proceduralBones[index].m_ParentIndex)
        rotation = math.mul(bones[skeleton.m_BoneOffset + index].m_Rotation, rotation);
      return math.mul(transform.m_Rotation, rotation);
    }

    private static quaternion LocalToObject(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      Skeleton skeleton,
      int index,
      quaternion rotation)
    {
      for (; index >= 0; index = proceduralBones[index].m_ParentIndex)
        rotation = math.mul(bones[skeleton.m_BoneOffset + index].m_Rotation, rotation);
      return rotation;
    }

    private static float3 LocalToWorld(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      Transform transform,
      Skeleton skeleton,
      int index,
      float3 position)
    {
      for (; index >= 0; index = proceduralBones[index].m_ParentIndex)
      {
        Bone bone = bones[skeleton.m_BoneOffset + index];
        position = bone.m_Position + math.mul(bone.m_Rotation, position);
      }
      return transform.m_Position + math.mul(transform.m_Rotation, position);
    }

    private static void LocalToWorld(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      Transform transform,
      Skeleton skeleton,
      int index,
      ref float3 position,
      ref quaternion rotation)
    {
      for (; index >= 0; index = proceduralBones[index].m_ParentIndex)
      {
        Bone bone = bones[skeleton.m_BoneOffset + index];
        position = bone.m_Position + math.mul(bone.m_Rotation, position);
        rotation = math.mul(bone.m_Rotation, rotation);
      }
      position = transform.m_Position + math.mul(transform.m_Rotation, position);
      rotation = math.mul(transform.m_Rotation, rotation);
    }

    private static void LookAtLocalToWorld(
      DynamicBuffer<ProceduralBone> proceduralBones,
      DynamicBuffer<Bone> bones,
      Transform transform,
      Skeleton skeleton,
      PointOfInterest pointOfInterest,
      int parentIndex,
      ref float3 position,
      ref quaternion rotation)
    {
      ProceduralBone proceduralBone = proceduralBones[parentIndex];
      if (proceduralBone.m_Type == BoneType.LookAtRotation || proceduralBone.m_Type == BoneType.LookAtRotationSide)
      {
        float3 position1 = proceduralBone.m_Position;
        quaternion rotation1 = proceduralBone.m_Rotation;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, transform, skeleton, proceduralBone.m_ParentIndex, ref position1, ref rotation1);
        float3 v = pointOfInterest.m_Position - position1;
        float3 a = math.mul(math.inverse(rotation1), v);
        a.xz = math.select(a.xz, MathUtils.Right(a.xz), proceduralBone.m_Type == BoneType.LookAtRotationSide);
        a = math.select(a, -a, (double) proceduralBone.m_Speed < 0.0);
        float2 xz = a.xz;
        if (MathUtils.TryNormalize(ref xz))
        {
          float angle = MathUtils.RotationAngleSignedRight(math.forward().xz, xz);
          rotation1 = math.mul(rotation1, quaternion.RotateY(angle));
        }
        position = position1 + math.mul(rotation1, position);
        rotation = math.mul(rotation1, rotation);
      }
      else if (proceduralBone.m_Type == BoneType.LengthwiseLookAtRotation)
      {
        float3 position2 = proceduralBone.m_Position;
        quaternion rotation2 = proceduralBone.m_Rotation;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, transform, skeleton, proceduralBone.m_ParentIndex, ref position2, ref rotation2);
        float3 v = pointOfInterest.m_Position - position2;
        float3 a = math.mul(math.inverse(rotation2), v);
        float2 xy = math.select(a, -a, (double) proceduralBone.m_Speed < 0.0).xy;
        if (MathUtils.TryNormalize(ref xy))
        {
          float angle = MathUtils.RotationAngleSignedLeft(math.up().xy, xy);
          rotation2 = math.mul(rotation2, quaternion.RotateZ(angle));
        }
        position = position2 + math.mul(rotation2, position);
        rotation = math.mul(rotation2, rotation);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, transform, skeleton, parentIndex, ref position, ref rotation);
      }
    }

    public static bool FindAnimationClip(
      DynamicBuffer<AnimationClip> clips,
      Game.Prefabs.AnimationType type,
      ActivityType activity,
      AnimationLayer animationLayer,
      AnimatedPropID propID,
      ActivityCondition conditions,
      out AnimationClip clip,
      out int index)
    {
      int num1 = int.MaxValue;
      clip = clips[0];
      index = 0;
      for (int index1 = 0; index1 < clips.Length; ++index1)
      {
        AnimationClip clip1 = clips[index1];
        if (clip1.m_Type == type && clip1.m_Activity == activity && clip1.m_Layer == animationLayer && clip1.m_PropID == propID)
        {
          ActivityCondition x = clip1.m_Conditions ^ conditions;
          if (x == (ActivityCondition) 0)
          {
            clip = clip1;
            index = index1;
            return true;
          }
          int num2 = math.countbits((uint) x);
          if (num2 < num1)
          {
            num1 = num2;
            clip = clip1;
            index = index1;
          }
        }
      }
      return num1 != int.MaxValue;
    }

    public static float GetMovementSpeed(
      in CharacterElement characterElement,
      in AnimationClip clip,
      ref BufferLookup<AnimationMotion> motionLookup)
    {
      if (clip.m_Type != Game.Prefabs.AnimationType.Move || clip.m_MotionRange.y <= clip.m_MotionRange.x + 1)
        return clip.m_MovementSpeed;
      DynamicBuffer<AnimationMotion> motions = motionLookup[characterElement.m_Style];
      AnimationMotion motion = motions[clip.m_MotionRange.x];
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight0);
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight1);
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight2);
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight3);
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight4);
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight5);
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight6);
      // ISSUE: reference to a compiler-generated method
      ObjectInterpolateSystem.AddMotionOffset(ref motion, motions, clip.m_MotionRange, characterElement.m_ShapeWeights.m_Weight7);
      float num1 = clip.m_AnimationLength * clip.m_FrameRate;
      float num2 = clip.m_FrameRate / math.max(1f, num1 - 1f);
      return math.length(motion.m_EndOffset - motion.m_StartOffset) * num2;
    }

    private static void AddMotionOffset(
      ref AnimationMotion motion,
      DynamicBuffer<AnimationMotion> motions,
      int2 range,
      BlendWeight weight)
    {
      AnimationMotion motion1 = motions[range.x + weight.m_Index + 1];
      motion.m_StartOffset += motion1.m_StartOffset * weight.m_Weight;
      motion.m_EndOffset += motion1.m_EndOffset * weight.m_Weight;
    }

    public static AnimatedPropID GetPropID(
      Entity entity,
      ActivityType activity,
      ref ComponentLookup<CurrentVehicle> currentVehicleLookup,
      ref ComponentLookup<PrefabRef> prefabRefLookup,
      ref BufferLookup<ActivityLocationElement> activityLocationLookup)
    {
      AnimatedPropID propId = new AnimatedPropID(-1);
      CurrentVehicle componentData1;
      PrefabRef componentData2;
      DynamicBuffer<ActivityLocationElement> bufferData;
      if ((activity == ActivityType.Enter || activity == ActivityType.Exit) && currentVehicleLookup.TryGetComponent(entity, out componentData1) && prefabRefLookup.TryGetComponent(componentData1.m_Vehicle, out componentData2) && activityLocationLookup.TryGetBuffer(componentData2.m_Prefab, out bufferData) && bufferData.Length != 0)
        propId = bufferData[0].m_PropID;
      return propId;
    }

    public static ActivityCondition GetActivityConditions(
      Entity entity,
      ref ComponentLookup<Human> humanLookup)
    {
      Human componentData;
      return humanLookup.TryGetComponent(entity, out componentData) ? CreatureUtils.GetConditions(componentData) : (ActivityCondition) 0;
    }

    public static void GetClipType(
      AnimationClip clip,
      TransformState state,
      float movementDelta,
      float speedDeltaFactor,
      out Game.Prefabs.AnimationType type,
      ref ActivityType activity)
    {
      switch (state)
      {
        case TransformState.Move:
          type = Game.Prefabs.AnimationType.Move;
          if (activity != ActivityType.None)
            break;
          switch (clip.m_Activity)
          {
            case ActivityType.Walking:
              float num1 = math.abs(movementDelta * speedDeltaFactor);
              activity = (double) speedDeltaFactor == 0.0 || (double) num1 <= (double) clip.m_SpeedRange.max ? ActivityType.Walking : ActivityType.Running;
              return;
            case ActivityType.Running:
              float num2 = math.abs(movementDelta * speedDeltaFactor);
              activity = (double) speedDeltaFactor == 0.0 || (double) num2 >= (double) clip.m_SpeedRange.min ? ActivityType.Running : ActivityType.Walking;
              return;
            default:
              activity = ActivityType.Walking;
              return;
          }
        case TransformState.Start:
          type = Game.Prefabs.AnimationType.Start;
          break;
        case TransformState.End:
          type = Game.Prefabs.AnimationType.End;
          break;
        case TransformState.Action:
        case TransformState.Done:
          type = Game.Prefabs.AnimationType.Action;
          break;
        default:
          type = Game.Prefabs.AnimationType.Idle;
          if (activity != ActivityType.None)
            break;
          activity = ActivityType.Standing;
          break;
      }
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
    public ObjectInterpolateSystem()
    {
    }

    [BurstCompile]
    private struct UpdateTransformDataJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<PointOfInterest> m_PointOfInterestData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<TrafficLight> m_TrafficLightData;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencyData;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_BuildingElectricityConsumer;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ExtractorFacility> m_BuildingExtractorFacility;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<CarTrailer> m_CarTrailerData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Human> m_HumanData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<SwayingData> m_PrefabSwayingData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public BufferLookup<EnabledEffect> m_EffectInstances;
      [ReadOnly]
      public BufferLookup<AnimationClip> m_AnimationClips;
      [ReadOnly]
      public BufferLookup<AnimationMotion> m_AnimationMotions;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [ReadOnly]
      public BufferLookup<ProceduralLight> m_ProceduralLights;
      [ReadOnly]
      public BufferLookup<LightAnimation> m_LightAnimations;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<CharacterElement> m_CharacterElements;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocations;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Swaying> m_SwayingData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Skeleton> m_Skeletons;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Emissive> m_Emissives;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Animated> m_Animateds;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Bone> m_Bones;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Momentum> m_Momentums;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LightState> m_Lights;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public uint m_PrevFrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [ReadOnly]
      public float m_FrameDelta;
      [ReadOnly]
      public float m_TimeOfDay;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public CellMapData<Wind> m_WindData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public NativeList<EnabledEffectData> m_EnabledData;
      public AnimatedSystem.AnimationData m_AnimationData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData cullingData = this.m_CullingData[index];
        if ((cullingData.m_Flags & (PreCullingFlags.InterpolatedTransform | PreCullingFlags.Animated | PreCullingFlags.Skeleton | PreCullingFlags.Emissive)) == (PreCullingFlags) 0 || (cullingData.m_Flags & (PreCullingFlags.NearCamera | PreCullingFlags.Temp | PreCullingFlags.Relative)) != PreCullingFlags.NearCamera)
          return;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(index);
        DynamicBuffer<TransformFrame> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformFrames.TryGetBuffer(cullingData.m_Entity, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateInterpolatedTransforms(cullingData, bufferData, ref random);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateStaticAnimations(cullingData, ref random);
        }
      }

      private void UpdateStaticAnimations(PreCullingData cullingData, ref Random random)
      {
        // ISSUE: reference to a compiler-generated field
        float deltaTime = this.m_FrameDelta / 60f;
        bool instantReset = (cullingData.m_Flags & PreCullingFlags.NearCameraUpdated) > (PreCullingFlags) 0;
        // ISSUE: reference to a compiler-generated field
        Transform transform1 = this.m_TransformData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        if ((cullingData.m_Flags & PreCullingFlags.InterpolatedTransform) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          ref InterpolatedTransform local = ref this.m_InterpolatedTransformData.GetRefRW(cullingData.m_Entity).ValueRW;
          Destroyed componentData1;
          ObjectGeometryData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_DestroyedData.TryGetComponent(cullingData.m_Entity, out componentData1) && this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            // ISSUE: reference to a compiler-generated field
            quaternion q2 = this.m_PseudoRandomSeedData[cullingData.m_Entity].GetRandom((uint) PseudoRandomSeed.kCollapse).NextQuaternionRotation();
            float collapseTime = BuildingUtils.GetCollapseTime(componentData2.m_Size.y);
            float time = math.max(0.0f, (!instantReset ? BuildingUtils.GetCollapseTime(transform1.m_Position.y - local.m_Position.y) : collapseTime + componentData1.m_Cleared) + deltaTime);
            local.m_Position = transform1.m_Position;
            local.m_Position.y -= BuildingUtils.GetCollapseHeight(time);
            local.m_Rotation = math.slerp(transform1.m_Rotation, q2, time / (float) (10.0 + (double) collapseTime * 10.0));
          }
          else
            local = new InterpolatedTransform(transform1);
        }
        if ((cullingData.m_Flags & (PreCullingFlags.Skeleton | PreCullingFlags.Emissive)) == (PreCullingFlags) 0)
          return;
        Entity owner = Entity.Null;
        Owner componentData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(cullingData.m_Entity, out componentData3))
          owner = componentData3.m_Owner;
        float2 wind = (float2) float.NaN;
        float2 efficiency = (float2) -1f;
        float electricity;
        // ISSUE: reference to a compiler-generated field
        if (this.m_VehicleData.HasComponent(cullingData.m_Entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          efficiency = (float2) math.select(1f, 0.0f, this.m_ParkedCarData.HasComponent(cullingData.m_Entity) || this.m_ParkedTrainData.HasComponent(cullingData.m_Entity));
          // ISSUE: reference to a compiler-generated field
          electricity = math.select(1f, 0.0f, this.m_DestroyedData.HasComponent(cullingData.m_Entity));
        }
        else
        {
          DynamicBuffer<Efficiency> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingEfficiencyData.TryGetBuffer(cullingData.m_Entity, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            efficiency = this.GetEfficiency(bufferData);
          }
          ElectricityConsumer componentData4;
          // ISSUE: reference to a compiler-generated field
          electricity = !this.m_BuildingElectricityConsumer.TryGetComponent(cullingData.m_Entity, out componentData4) ? efficiency.x : math.select(0.0f, 1f, componentData4.electricityConnected);
        }
        float working = -1f;
        Game.Objects.TrafficLightState trafficLightState = Game.Objects.TrafficLightState.None;
        TrafficLight componentData5;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TrafficLightData.TryGetComponent(cullingData.m_Entity, out componentData5))
          trafficLightState = componentData5.m_State;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubMesh> subMesh1 = this.m_SubMeshes[prefabRef.m_Prefab];
        if ((cullingData.m_Flags & PreCullingFlags.Skeleton) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Bone> bone = this.m_Bones[cullingData.m_Entity];
          DynamicBuffer<Momentum> bufferData;
          // ISSUE: reference to a compiler-generated field
          this.m_Momentums.TryGetBuffer(cullingData.m_Entity, out bufferData);
          for (int index1 = 0; index1 < skeleton.Length; ++index1)
          {
            ref Skeleton local = ref skeleton.ElementAt(index1);
            if (!local.m_BufferAllocation.Empty)
            {
              SubMesh subMesh2 = subMesh1[index1];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ProceduralBone> proceduralBone = this.m_ProceduralBones[subMesh2.m_SubMesh];
              Transform transform2 = transform1;
              if ((subMesh2.m_Flags & SubMeshFlags.HasTransform) != (SubMeshFlags) 0)
                transform2 = ObjectUtils.LocalToWorld(transform1, subMesh2.m_Position, subMesh2.m_Rotation);
              for (int index2 = 0; index2 < proceduralBone.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated method
                this.AnimateStaticBone(proceduralBone, bone, bufferData, transform2, prefabRef, ref local, index2, deltaTime, cullingData.m_Entity, owner, instantReset, trafficLightState, ref random, ref wind, ref efficiency, ref electricity, ref working);
              }
            }
          }
        }
        if ((cullingData.m_Flags & PreCullingFlags.Emissive) == (PreCullingFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        PseudoRandomSeed pseudoRandomSeed = this.m_PseudoRandomSeedData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Emissive> emissive = this.m_Emissives[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LightState> light = this.m_Lights[cullingData.m_Entity];
        DynamicBuffer<EnabledEffect> bufferData1;
        // ISSUE: reference to a compiler-generated field
        this.m_EffectInstances.TryGetBuffer(cullingData.m_Entity, out bufferData1);
        CarFlags carFlags = (CarFlags) 0;
        Car componentData6;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarData.TryGetComponent(cullingData.m_Entity, out componentData6))
          carFlags = componentData6.m_Flags;
        Random random1 = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kLightState);
        for (int index3 = 0; index3 < emissive.Length; ++index3)
        {
          ref Emissive local = ref emissive.ElementAt(index3);
          if (!local.m_BufferAllocation.Empty)
          {
            SubMesh subMesh3 = subMesh1[index3];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ProceduralLight> proceduralLight = this.m_ProceduralLights[subMesh3.m_SubMesh];
            DynamicBuffer<LightAnimation> bufferData2;
            // ISSUE: reference to a compiler-generated field
            this.m_LightAnimations.TryGetBuffer(subMesh3.m_SubMesh, out bufferData2);
            for (int index4 = 0; index4 < proceduralLight.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated method
              this.AnimateStaticLight(proceduralLight, bufferData2, light, ref local, index4, deltaTime, owner, instantReset, random1, trafficLightState, carFlags, bufferData1, ref electricity);
            }
          }
        }
      }

      private void AnimateStaticBone(
        DynamicBuffer<ProceduralBone> proceduralBones,
        DynamicBuffer<Bone> bones,
        DynamicBuffer<Momentum> momentums,
        Transform transform,
        PrefabRef prefabRef,
        ref Skeleton skeleton,
        int index,
        float deltaTime,
        Entity entity,
        Entity owner,
        bool instantReset,
        Game.Objects.TrafficLightState trafficLightState,
        ref Random random,
        ref float2 wind,
        ref float2 efficiency,
        ref float electricity,
        ref float working)
      {
        ProceduralBone proceduralBone1 = proceduralBones[index];
        Momentum momentum = new Momentum();
        int index1 = skeleton.m_BoneOffset + index;
        ref Bone local1 = ref bones.ElementAt(index1);
        ref Momentum local2 = ref momentum;
        if (momentums.IsCreated)
          local2 = ref momentums.ElementAt(index1);
        switch (proceduralBone1.m_Type)
        {
          case BoneType.LookAtDirection:
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, owner);
            PointOfInterest componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PointOfInterestData.TryGetComponent(entity, out componentData1) && componentData1.m_IsValid)
            {
              // ISSUE: reference to a compiler-generated method
              quaternion world = ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, transform, skeleton, proceduralBone1.m_ParentIndex, proceduralBone1.m_Rotation);
              float3 v = componentData1.m_Position - transform.m_Position;
              float3 a = math.mul(math.inverse(world), v);
              float3 float3 = math.select(a, -a, (double) proceduralBone1.m_Speed < 0.0);
              float targetSpeed = math.abs(proceduralBone1.m_Speed) * efficiency.x;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, float3.xz, targetSpeed, deltaTime, instantReset);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, new float2(0.0f, 1f), 0.0f, deltaTime, instantReset);
            break;
          case BoneType.WindTurbineRotation:
            // ISSUE: reference to a compiler-generated method
            this.RequireWind(ref wind, transform);
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, owner);
            float targetSpeed1 = proceduralBone1.m_Speed * math.length(wind) * efficiency.y;
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, targetSpeed1, deltaTime, instantReset);
            break;
          case BoneType.WindSpeedRotation:
            // ISSUE: reference to a compiler-generated method
            this.RequireWind(ref wind, transform);
            float targetSpeed2 = proceduralBone1.m_Speed * math.length(wind);
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, targetSpeed2, deltaTime, instantReset);
            break;
          case BoneType.PoweredRotation:
            // ISSUE: reference to a compiler-generated method
            this.RequireElectricity(ref electricity, owner);
            float targetSpeed3 = proceduralBone1.m_Speed * electricity;
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, targetSpeed3, deltaTime, instantReset);
            break;
          case BoneType.TrafficBarrierDirection:
            float2 targetDir = math.select(new float2(math.select(-1f, 1f, (double) proceduralBone1.m_Speed < 0.0), 0.0f), new float2(0.0f, 1f), (trafficLightState & (Game.Objects.TrafficLightState.Red | Game.Objects.TrafficLightState.Yellow)) == Game.Objects.TrafficLightState.Red);
            float targetSpeed4 = math.abs(proceduralBone1.m_Speed);
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneZ(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, targetDir, targetSpeed4, deltaTime, instantReset);
            break;
          case BoneType.FixedRotation:
            ProceduralBone proceduralBone2 = proceduralBones[proceduralBone1.m_ParentIndex];
            Bone bone = bones.ElementAt(skeleton.m_BoneOffset + proceduralBone1.m_ParentIndex);
            // ISSUE: reference to a compiler-generated method
            quaternion x1 = math.mul(math.inverse(ObjectInterpolateSystem.LocalToObject(proceduralBones, bones, skeleton, proceduralBone2.m_ParentIndex, bone.m_Rotation)), proceduralBone1.m_ObjectRotation);
            skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x1);
            local1.m_Rotation = x1;
            break;
          case BoneType.LookAtRotation:
          case BoneType.LookAtRotationSide:
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, owner);
            PointOfInterest componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PointOfInterestData.TryGetComponent(entity, out componentData2) && componentData2.m_IsValid)
            {
              float3 position = proceduralBone1.m_Position;
              quaternion rotation = proceduralBone1.m_Rotation;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, transform, skeleton, proceduralBone1.m_ParentIndex, ref position, ref rotation);
              float3 v = componentData2.m_Position - position;
              float3 a = math.mul(math.inverse(rotation), v);
              a.xz = math.select(a.xz, MathUtils.Right(a.xz), proceduralBone1.m_Type == BoneType.LookAtRotationSide);
              a = math.select(a, -a, (double) proceduralBone1.m_Speed < 0.0);
              float targetSpeed5 = math.abs(proceduralBone1.m_Speed) * efficiency.x;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, a.xz, targetSpeed5, deltaTime, instantReset);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, math.forward().xz, 0.0f, deltaTime, instantReset);
            break;
          case BoneType.LookAtAim:
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, entity);
            PointOfInterest componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PointOfInterestData.TryGetComponent(entity, out componentData3) && componentData3.m_IsValid)
            {
              float3 position = proceduralBone1.m_Position;
              quaternion rotation = proceduralBone1.m_Rotation;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.LookAtLocalToWorld(proceduralBones, bones, transform, skeleton, componentData3, proceduralBone1.m_ParentIndex, ref position, ref rotation);
              float3 v = componentData3.m_Position - position;
              float3 a = math.mul(math.inverse(rotation), v);
              float3 float3 = math.select(a, -a, (double) proceduralBone1.m_Speed < 0.0);
              float targetSpeed6 = math.abs(proceduralBone1.m_Speed) * efficiency.x;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.AnimateRotatingBoneX(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, float3.yz, targetSpeed6, deltaTime, instantReset);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneX(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, math.up().yz, 0.0f, deltaTime, instantReset);
            break;
          case BoneType.PantographRotation:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimatePantographBone(proceduralBones, bones, transform, prefabRef, proceduralBone1, ref skeleton, ref local1, index, deltaTime, false, instantReset, ref this.m_CurveData, ref this.m_PrefabRefData, ref this.m_PrefabUtilityLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneSearchTree);
            break;
          case BoneType.LengthwiseLookAtRotation:
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, owner);
            PointOfInterest componentData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PointOfInterestData.TryGetComponent(entity, out componentData4) && componentData4.m_IsValid)
            {
              float3 position = proceduralBone1.m_Position;
              quaternion rotation = proceduralBone1.m_Rotation;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.LocalToWorld(proceduralBones, bones, transform, skeleton, proceduralBone1.m_ParentIndex, ref position, ref rotation);
              float3 v = componentData4.m_Position - position;
              float3 a = math.mul(math.inverse(rotation), v);
              float3 float3 = math.select(a, -a, (double) proceduralBone1.m_Speed < 0.0);
              float targetSpeed7 = math.abs(proceduralBone1.m_Speed) * efficiency.x;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.AnimateRotatingBoneZ(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, float3.xy, targetSpeed7, deltaTime, instantReset);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneZ(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, math.up().xy, 0.0f, deltaTime, instantReset);
            break;
          case BoneType.WorkingRotation:
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, owner);
            // ISSUE: reference to a compiler-generated method
            this.RequireWorking(ref working, entity);
            float targetSpeed8 = proceduralBone1.m_Speed * efficiency.x * working;
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneX(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, targetSpeed8, deltaTime, instantReset);
            break;
          case BoneType.OperatingRotation:
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, owner);
            float targetSpeed9 = proceduralBone1.m_Speed * efficiency.x;
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateRotatingBoneY(proceduralBone1, ref skeleton, ref local1, ref local2, ref random, targetSpeed9, deltaTime, instantReset);
            break;
          case BoneType.TimeRotation:
            // ISSUE: reference to a compiler-generated method
            this.RequireElectricity(ref electricity, owner);
            // ISSUE: reference to a compiler-generated field
            float num = this.m_TimeOfDay * proceduralBone1.m_Speed;
            float angle;
            if (instantReset)
            {
              angle = math.select(random.NextFloat(-3.14159274f, 3.14159274f), num, (double) electricity != 0.0);
            }
            else
            {
              float2 float2 = math.normalizesafe(math.mul(math.mul(math.inverse(proceduralBone1.m_Rotation), local1.m_Rotation), math.right()).xz);
              float fromAngle = math.atan2(-float2.y, float2.x);
              float b = math.abs(deltaTime) * electricity;
              angle = fromAngle + math.clamp(MathUtils.RotationAngle(fromAngle, num), -b, b);
            }
            quaternion x2 = math.mul(proceduralBone1.m_Rotation, quaternion.RotateY(angle));
            skeleton.m_CurrentUpdated |= !local1.m_Rotation.Equals(x2);
            local1.m_Rotation = x2;
            break;
          case BoneType.LookAtMovementX:
          case BoneType.LookAtMovementY:
          case BoneType.LookAtMovementZ:
            // ISSUE: reference to a compiler-generated method
            this.RequireEfficiency(ref efficiency, owner);
            float3 v1 = math.select((float3) 0.0f, (float3) 1f, new bool3(proceduralBone1.m_Type == BoneType.LookAtMovementX, proceduralBone1.m_Type == BoneType.LookAtMovementY, proceduralBone1.m_Type == BoneType.LookAtMovementZ));
            float3 moveDirection = math.rotate(proceduralBone1.m_Rotation, v1);
            PointOfInterest componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PointOfInterestData.TryGetComponent(entity, out componentData5) && componentData5.m_IsValid)
            {
              float3 position = proceduralBone1.m_Position;
              quaternion rotation = proceduralBone1.m_Rotation;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.LookAtLocalToWorld(proceduralBones, bones, transform, skeleton, componentData5, proceduralBone1.m_ParentIndex, ref position, ref rotation);
              float3 y = math.rotate(rotation, v1);
              float a = math.dot(componentData5.m_Position - position, y);
              float targetOffset = math.select(a, -a, (double) proceduralBone1.m_Speed < 0.0);
              float targetSpeed10 = math.abs(proceduralBone1.m_Speed) * efficiency.x;
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.AnimateMovingBone(proceduralBone1, ref skeleton, ref local1, ref local2, moveDirection, targetOffset, targetSpeed10, deltaTime, instantReset);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateMovingBone(proceduralBone1, ref skeleton, ref local1, ref local2, moveDirection, 0.0f, 0.0f, deltaTime, instantReset);
            break;
        }
      }

      private void AnimateStaticLight(
        DynamicBuffer<ProceduralLight> proceduralLights,
        DynamicBuffer<LightAnimation> lightAnimations,
        DynamicBuffer<LightState> lights,
        ref Emissive emissive,
        int index,
        float deltaTime,
        Entity owner,
        bool instantReset,
        Random pseudoRandom,
        Game.Objects.TrafficLightState trafficLightState,
        CarFlags carFlags,
        DynamicBuffer<EnabledEffect> effects,
        ref float electricity)
      {
        ProceduralLight proceduralLight = proceduralLights[index];
        int index1 = emissive.m_LightOffset + index;
        ref LightState local = ref lights.ElementAt(index1);
        switch (proceduralLight.m_Purpose)
        {
          case EmissiveProperties.Purpose.DaytimeRunningLight:
          case EmissiveProperties.Purpose.Headlight_HighBeam:
          case EmissiveProperties.Purpose.Headlight_LowBeam:
          case EmissiveProperties.Purpose.TurnSignalLeft:
          case EmissiveProperties.Purpose.TurnSignalRight:
          case EmissiveProperties.Purpose.RearLight:
          case EmissiveProperties.Purpose.BrakeLight:
          case EmissiveProperties.Purpose.ReverseLight:
          case EmissiveProperties.Purpose.Clearance:
          case EmissiveProperties.Purpose.DaytimeRunningLightLeft:
          case EmissiveProperties.Purpose.DaytimeRunningLightRight:
          case EmissiveProperties.Purpose.SignalGroup1:
          case EmissiveProperties.Purpose.SignalGroup2:
          case EmissiveProperties.Purpose.SignalGroup3:
          case EmissiveProperties.Purpose.SignalGroup4:
          case EmissiveProperties.Purpose.SignalGroup5:
          case EmissiveProperties.Purpose.SignalGroup6:
          case EmissiveProperties.Purpose.SignalGroup7:
          case EmissiveProperties.Purpose.SignalGroup8:
          case EmissiveProperties.Purpose.SignalGroup9:
          case EmissiveProperties.Purpose.SignalGroup10:
          case EmissiveProperties.Purpose.SignalGroup11:
          case EmissiveProperties.Purpose.Interior1:
          case EmissiveProperties.Purpose.DaytimeRunningLightAlt:
          case EmissiveProperties.Purpose.Dashboard:
          case EmissiveProperties.Purpose.Clearance2:
          case EmissiveProperties.Purpose.MarkerLights:
          case EmissiveProperties.Purpose.BrakeAndTurnSignalLeft:
          case EmissiveProperties.Purpose.BrakeAndTurnSignalRight:
          case EmissiveProperties.Purpose.TaxiLights:
          case EmissiveProperties.Purpose.LandingLights:
          case EmissiveProperties.Purpose.WingInspectionLights:
          case EmissiveProperties.Purpose.LogoLights:
          case EmissiveProperties.Purpose.PositionLightLeft:
          case EmissiveProperties.Purpose.PositionLightRight:
          case EmissiveProperties.Purpose.PositionLights:
          case EmissiveProperties.Purpose.SearchLightsFront:
          case EmissiveProperties.Purpose.SearchLights360:
          case EmissiveProperties.Purpose.NumberLight:
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, 0.0f, instantReset);
            break;
          case EmissiveProperties.Purpose.TrafficLight_Red:
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrafficLight(proceduralLight, lightAnimations, pseudoRandom, ref emissive, ref local, deltaTime, instantReset, (trafficLightState & Game.Objects.TrafficLightState.Red) != 0, (trafficLightState & Game.Objects.TrafficLightState.Flashing) != 0);
            break;
          case EmissiveProperties.Purpose.TrafficLight_Yellow:
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrafficLight(proceduralLight, lightAnimations, pseudoRandom, ref emissive, ref local, deltaTime, instantReset, (trafficLightState & Game.Objects.TrafficLightState.Yellow) != 0, (trafficLightState & Game.Objects.TrafficLightState.Flashing) != 0);
            break;
          case EmissiveProperties.Purpose.TrafficLight_Green:
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrafficLight(proceduralLight, lightAnimations, pseudoRandom, ref emissive, ref local, deltaTime, instantReset, (trafficLightState & Game.Objects.TrafficLightState.Green) != 0, (trafficLightState & Game.Objects.TrafficLightState.Flashing) != 0);
            break;
          case EmissiveProperties.Purpose.PedestrianLight_Stop:
            Game.Objects.TrafficLightState trafficLightState1 = (Game.Objects.TrafficLightState) ((uint) trafficLightState >> 4);
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrafficLight(proceduralLight, lightAnimations, pseudoRandom, ref emissive, ref local, deltaTime, instantReset, (trafficLightState1 & Game.Objects.TrafficLightState.Red) != 0, (trafficLightState1 & Game.Objects.TrafficLightState.Flashing) != 0);
            break;
          case EmissiveProperties.Purpose.PedestrianLight_Walk:
            Game.Objects.TrafficLightState trafficLightState2 = (Game.Objects.TrafficLightState) ((uint) trafficLightState >> 4);
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrafficLight(proceduralLight, lightAnimations, pseudoRandom, ref emissive, ref local, deltaTime, instantReset, (trafficLightState2 & Game.Objects.TrafficLightState.Green) != 0, (trafficLightState2 & Game.Objects.TrafficLightState.Flashing) != 0);
            break;
          case EmissiveProperties.Purpose.RailCrossing_Stop:
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrafficLight(proceduralLight, lightAnimations, pseudoRandom, ref emissive, ref local, deltaTime, instantReset, (trafficLightState & (Game.Objects.TrafficLightState.Red | Game.Objects.TrafficLightState.Yellow)) != 0, (trafficLightState & Game.Objects.TrafficLightState.Flashing) != 0);
            break;
          case EmissiveProperties.Purpose.NeonSign:
          case EmissiveProperties.Purpose.DecorativeLight:
            // ISSUE: reference to a compiler-generated method
            this.RequireElectricity(ref electricity, owner);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            float targetIntensity1 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, this.m_FrameIndex, this.m_FrameTime, electricity);
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity1, instantReset);
            break;
          case EmissiveProperties.Purpose.Emergency1:
          case EmissiveProperties.Purpose.Emergency2:
          case EmissiveProperties.Purpose.Emergency3:
          case EmissiveProperties.Purpose.Emergency4:
          case EmissiveProperties.Purpose.Emergency5:
          case EmissiveProperties.Purpose.Emergency6:
          case EmissiveProperties.Purpose.RearAlarmLights:
          case EmissiveProperties.Purpose.FrontAlarmLightsLeft:
          case EmissiveProperties.Purpose.FrontAlarmLightsRight:
          case EmissiveProperties.Purpose.Warning1:
          case EmissiveProperties.Purpose.Warning2:
          case EmissiveProperties.Purpose.Emergency7:
          case EmissiveProperties.Purpose.Emergency8:
          case EmissiveProperties.Purpose.Emergency9:
          case EmissiveProperties.Purpose.Emergency10:
          case EmissiveProperties.Purpose.AntiCollisionLightsRed:
          case EmissiveProperties.Purpose.AntiCollisionLightsWhite:
            float targetIntensity2 = 0.0f;
            if ((carFlags & (CarFlags.Emergency | CarFlags.Warning)) != (CarFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              targetIntensity2 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, this.m_FrameIndex, this.m_FrameTime, 1f);
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity2, instantReset);
            break;
          case EmissiveProperties.Purpose.CollectionLights:
          case EmissiveProperties.Purpose.WorkLights:
            float targetIntensity3 = math.select(0.0f, 1f, (carFlags & CarFlags.Sign) > (CarFlags) 0);
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity3, instantReset);
            break;
          case EmissiveProperties.Purpose.TaxiSign:
            float targetIntensity4 = 0.0f;
            if ((carFlags & CarFlags.Sign) != (CarFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              targetIntensity4 = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, this.m_FrameIndex, this.m_FrameTime, 1f);
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity4, instantReset);
            break;
          case EmissiveProperties.Purpose.EffectSource:
            if (!effects.IsCreated)
              break;
            float targetIntensity5 = 0.0f;
            int index2 = 0;
            if (effects.Length > index2)
            {
              // ISSUE: reference to a compiler-generated field
              targetIntensity5 = math.select(0.0f, 1f, (this.m_EnabledData[effects[index2].m_EnabledIndex].m_Flags & EnabledEffectFlags.IsEnabled) > (EnabledEffectFlags) 0);
            }
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref local, deltaTime, targetIntensity5, instantReset);
            break;
        }
      }

      private void AnimateTrafficLight(
        ProceduralLight proceduralLight,
        DynamicBuffer<LightAnimation> lightAnimations,
        Random pseudoRandom,
        ref Emissive emissive,
        ref LightState light,
        float deltaTime,
        bool instantReset,
        bool on,
        bool flashing)
      {
        float num = math.select(0.0f, 1f, on);
        if (flashing)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num = ObjectInterpolateSystem.AnimateIntensity(proceduralLight, lightAnimations, pseudoRandom, this.m_FrameIndex, this.m_FrameTime, num);
        }
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.AnimateLight(proceduralLight, ref emissive, ref light, deltaTime, num, instantReset);
      }

      private void RequireWind(ref float2 wind, Transform transform)
      {
        if (!math.isnan(wind.x))
          return;
        // ISSUE: reference to a compiler-generated field
        wind = Wind.SampleWind(this.m_WindData, transform.m_Position);
      }

      private void RequireEfficiency(ref float2 efficiency, Entity owner)
      {
        if ((double) efficiency.x >= 0.0)
          return;
        DynamicBuffer<Efficiency> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingEfficiencyData.TryGetBuffer(owner, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          efficiency = this.GetEfficiency(bufferData);
        }
        else
        {
          Owner componentData1;
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.TryGetComponent(owner, out componentData1))
          {
            owner = componentData1.m_Owner;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingEfficiencyData.TryGetBuffer(owner, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              efficiency = this.GetEfficiency(bufferData);
              return;
            }
          }
          Attachment componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachmentData.TryGetComponent(owner, out componentData2) && this.m_BuildingEfficiencyData.TryGetBuffer(componentData2.m_Attached, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            efficiency = this.GetEfficiency(bufferData);
          }
          else
            efficiency = (float2) 1f;
        }
      }

      private float2 GetEfficiency(DynamicBuffer<Efficiency> buffer)
      {
        float2 float2 = (float2) 1f;
        foreach (Efficiency efficiency in buffer)
        {
          float2 y;
          y.x = efficiency.m_Efficiency;
          y.y = math.select(1f, efficiency.m_Efficiency, efficiency.m_Factor != EfficiencyFactor.Fire);
          float2 *= math.max((float2) 0.0f, y);
        }
        return math.select((float2) 0.0f, math.max((float2) 0.01f, math.round(100f * float2) * 0.01f), float2 > 0.0f);
      }

      private void RequireElectricity(ref float electricity, Entity owner)
      {
        if ((double) electricity >= 0.0)
          return;
        electricity = 1f;
        ElectricityConsumer componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingElectricityConsumer.TryGetComponent(owner, out componentData1))
        {
          electricity = math.select(0.0f, 1f, componentData1.electricityConnected);
        }
        else
        {
          DynamicBuffer<Efficiency> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingEfficiencyData.TryGetBuffer(owner, out bufferData))
            electricity = BuildingUtils.GetEfficiency(bufferData);
          Owner componentData2;
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.TryGetComponent(owner, out componentData2))
          {
            owner = componentData2.m_Owner;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingElectricityConsumer.TryGetComponent(owner, out componentData1))
            {
              electricity = math.select(0.0f, 1f, componentData1.electricityConnected);
              return;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingEfficiencyData.TryGetBuffer(owner, out bufferData))
              electricity = BuildingUtils.GetEfficiency(bufferData);
          }
          Attachment componentData3;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_AttachmentData.TryGetComponent(owner, out componentData3))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingElectricityConsumer.TryGetComponent(componentData3.m_Attached, out componentData1))
          {
            electricity = math.select(0.0f, 1f, componentData1.electricityConnected);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_BuildingEfficiencyData.TryGetBuffer(componentData3.m_Attached, out bufferData))
              return;
            electricity = BuildingUtils.GetEfficiency(bufferData);
          }
        }
      }

      private void UpdateInterpolatedTransforms(
        PreCullingData cullingData,
        DynamicBuffer<TransformFrame> transformFrames,
        ref Random random)
      {
        uint updateFrame1;
        uint updateFrame2;
        float framePosition;
        int updateFrameChanged;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_PrevFrameIndex, this.m_FrameTime, (uint) cullingData.m_UpdateFrame, out updateFrame1, out updateFrame2, out framePosition, out updateFrameChanged);
        float updateFrameToSeconds = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        float deltaTime = this.m_FrameDelta / 60f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float speedDeltaFactor = math.select(60f / this.m_FrameDelta, 0.0f, (double) this.m_FrameDelta == 0.0);
        if ((cullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateInterpolatedAnimations(cullingData, transformFrames, ref random, updateFrame1, updateFrame2, framePosition, updateFrameToSeconds, deltaTime, speedDeltaFactor, updateFrameChanged);
        }
        else if ((cullingData.m_Flags & PreCullingFlags.Skeleton) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateInterpolatedAnimations(cullingData, transformFrames, updateFrame1, updateFrame2, framePosition, updateFrameToSeconds, deltaTime, speedDeltaFactor, ref random);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateInterpolatedTransforms(cullingData, transformFrames, updateFrame1, updateFrame2, framePosition, updateFrameToSeconds, deltaTime, speedDeltaFactor);
        }
      }

      private void RequireWorking(ref float working, Entity owner)
      {
        if ((double) working >= 0.0)
          return;
        Game.Buildings.ExtractorFacility componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingExtractorFacility.TryGetComponent(owner, out componentData))
          working = math.select(0.0f, 1f, (componentData.m_Flags & ExtractorFlags.Working) != 0);
        else
          working = 1f;
      }

      private void UpdateInterpolatedTransforms(
        PreCullingData cullingData,
        DynamicBuffer<TransformFrame> frames,
        uint updateFrame1,
        uint updateFrame2,
        float framePosition,
        float updateFrameToSeconds,
        float deltaTime,
        float speedDeltaFactor)
      {
        // ISSUE: reference to a compiler-generated field
        ref InterpolatedTransform local1 = ref this.m_InterpolatedTransformData.GetRefRW(cullingData.m_Entity).ValueRW;
        InterpolatedTransform oldTransform = local1;
        TransformFrame frame1 = frames[(int) updateFrame1];
        TransformFrame frame2 = frames[(int) updateFrame2];
        // ISSUE: reference to a compiler-generated method
        local1 = ObjectInterpolateSystem.CalculateTransform(frame1, frame2, framePosition);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SwayingData.HasComponent(cullingData.m_Entity))
          return;
        // ISSUE: reference to a compiler-generated field
        ref Swaying local2 = ref this.m_SwayingData.GetRefRW(cullingData.m_Entity).ValueRW;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        if ((cullingData.m_Flags & PreCullingFlags.NearCameraUpdated) > (PreCullingFlags) 0)
        {
          local2.m_LastVelocity = math.lerp(frame1.m_Velocity, frame2.m_Velocity, framePosition);
          local2.m_SwayVelocity = (float3) 0.0f;
          local2.m_SwayPosition = (float3) 0.0f;
        }
        else
        {
          SwayingData componentData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabSwayingData.TryGetComponent(prefabRef.m_Prefab, out componentData))
            return;
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.UpdateSwaying(componentData, oldTransform, ref local1, ref local2, deltaTime, speedDeltaFactor, out quaternion _, out float _);
        }
      }

      private void UpdateInterpolatedAnimations(
        PreCullingData cullingData,
        DynamicBuffer<TransformFrame> frames,
        uint updateFrame1,
        uint updateFrame2,
        float framePosition,
        float updateFrameToSeconds,
        float deltaTime,
        float speedDeltaFactor,
        ref Random random)
      {
        bool flag1 = false;
        bool flag2 = false;
        Controller componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControllerData.TryGetComponent(cullingData.m_Entity, out componentData1))
        {
          flag1 = componentData1.m_Controller != Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarTrailerData.HasComponent(cullingData.m_Entity))
          {
            CullingInfo componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CullingInfoData.TryGetComponent(componentData1.m_Controller, out componentData2) && componentData2.m_CullingIndex != 0 && (this.m_CullingData[componentData2.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) != (PreCullingFlags) 0)
              return;
            flag2 = true;
          }
        }
        bool instantReset = flag2 | (cullingData.m_Flags & PreCullingFlags.NearCameraUpdated) > (PreCullingFlags) 0;
        // ISSUE: reference to a compiler-generated field
        ref InterpolatedTransform local1 = ref this.m_InterpolatedTransformData.GetRefRW(cullingData.m_Entity).ValueRW;
        InterpolatedTransform interpolatedTransform = local1;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        TransformFrame frame1 = frames[(int) updateFrame1];
        TransformFrame frame2 = frames[(int) updateFrame2];
        // ISSUE: reference to a compiler-generated method
        local1 = ObjectInterpolateSystem.CalculateTransform(frame1, frame2, framePosition);
        quaternion swayRotation = quaternion.identity;
        float swayOffset = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SwayingData.HasComponent(cullingData.m_Entity))
        {
          // ISSUE: reference to a compiler-generated field
          ref Swaying local2 = ref this.m_SwayingData.GetRefRW(cullingData.m_Entity).ValueRW;
          if (instantReset)
          {
            local2.m_LastVelocity = math.lerp(frame1.m_Velocity, frame2.m_Velocity, framePosition);
            local2.m_SwayVelocity = (float3) 0.0f;
            local2.m_SwayPosition = (float3) 0.0f;
          }
          else
          {
            SwayingData componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSwayingData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
            {
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.UpdateSwaying(componentData3, interpolatedTransform, ref local1, ref local2, deltaTime, speedDeltaFactor, out swayRotation, out swayOffset);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubMesh> subMesh1 = this.m_SubMeshes[prefabRef.m_Prefab];
        if ((cullingData.m_Flags & PreCullingFlags.Skeleton) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Bone> bone = this.m_Bones[cullingData.m_Entity];
          DynamicBuffer<Momentum> bufferData;
          // ISSUE: reference to a compiler-generated field
          this.m_Momentums.TryGetBuffer(cullingData.m_Entity, out bufferData);
          for (int index1 = 0; index1 < skeleton.Length; ++index1)
          {
            ref Skeleton local3 = ref skeleton.ElementAt(index1);
            if (!local3.m_BufferAllocation.Empty)
            {
              SubMesh subMesh2 = subMesh1[index1];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ProceduralBone> proceduralBone = this.m_ProceduralBones[subMesh2.m_SubMesh];
              InterpolatedTransform oldTransform = interpolatedTransform;
              InterpolatedTransform newTransform = local1;
              if ((subMesh2.m_Flags & SubMeshFlags.HasTransform) != (SubMeshFlags) 0)
              {
                oldTransform = ObjectUtils.LocalToWorld(interpolatedTransform, subMesh2.m_Position, subMesh2.m_Rotation);
                newTransform = ObjectUtils.LocalToWorld(local1, subMesh2.m_Position, subMesh2.m_Rotation);
              }
              float steeringRadius = 0.0f;
              CarData componentData4;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabCarData.TryGetComponent(prefabRef.m_Prefab, out componentData4))
              {
                // ISSUE: reference to a compiler-generated method
                steeringRadius = ObjectInterpolateSystem.CalculateSteeringRadius(proceduralBone, bone, oldTransform, newTransform, ref local3, componentData4);
              }
              for (int index2 = 0; index2 < proceduralBone.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ObjectInterpolateSystem.AnimateInterpolatedBone(proceduralBone, bone, bufferData, oldTransform, newTransform, prefabRef, ref local3, swayRotation, swayOffset, steeringRadius, componentData4.m_PivotOffset, index2, deltaTime, cullingData.m_Entity, instantReset, this.m_FrameIndex, this.m_FrameTime, ref random, ref this.m_PointOfInterestData, ref this.m_CurveData, ref this.m_PrefabRefData, ref this.m_PrefabUtilityLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneSearchTree);
              }
            }
          }
        }
        if ((cullingData.m_Flags & PreCullingFlags.Emissive) == (PreCullingFlags) 0)
          return;
        PseudoRandomSeed componentData5;
        // ISSUE: reference to a compiler-generated field
        if (!flag1 || !this.m_PseudoRandomSeedData.TryGetComponent(componentData1.m_Controller, out componentData5))
        {
          // ISSUE: reference to a compiler-generated field
          componentData5 = this.m_PseudoRandomSeedData[cullingData.m_Entity];
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Emissive> emissive = this.m_Emissives[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LightState> light = this.m_Lights[cullingData.m_Entity];
        Random random1 = componentData5.GetRandom((uint) PseudoRandomSeed.kLightState);
        for (int index3 = 0; index3 < emissive.Length; ++index3)
        {
          ref Emissive local4 = ref emissive.ElementAt(index3);
          if (!local4.m_BufferAllocation.Empty)
          {
            SubMesh subMesh3 = subMesh1[index3];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ProceduralLight> proceduralLight = this.m_ProceduralLights[subMesh3.m_SubMesh];
            DynamicBuffer<LightAnimation> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_LightAnimations.TryGetBuffer(subMesh3.m_SubMesh, out bufferData);
            for (int index4 = 0; index4 < proceduralLight.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.AnimateInterpolatedLight(proceduralLight, bufferData, light, local1.m_Flags, random1, ref local4, index4, this.m_FrameIndex, this.m_FrameTime, deltaTime, instantReset);
            }
          }
        }
      }

      private void UpdateInterpolatedAnimations(
        PreCullingData cullingData,
        DynamicBuffer<TransformFrame> frames,
        ref Random random,
        uint updateFrame1,
        uint updateFrame2,
        float framePosition,
        float updateFrameToSeconds,
        float deltaTime,
        float speedDeltaFactor,
        int updateFrameChanged)
      {
        bool flag = (cullingData.m_Flags & PreCullingFlags.NearCameraUpdated) > (PreCullingFlags) 0;
        // ISSUE: reference to a compiler-generated field
        ref InterpolatedTransform local = ref this.m_InterpolatedTransformData.GetRefRW(cullingData.m_Entity).ValueRW;
        InterpolatedTransform oldTransform = local;
        TransformFrame frame1 = frames[(int) updateFrame1];
        TransformFrame frame2 = frames[(int) updateFrame2];
        if ((cullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Animated> animated1 = this.m_Animateds[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubMesh> subMesh = this.m_SubMeshes[prefabRef.m_Prefab];
          float stateTimer = 0.0f;
          TransformState state = TransformState.Default;
          ActivityType activity = ActivityType.None;
          DynamicBuffer<MeshGroup> bufferData1 = new DynamicBuffer<MeshGroup>();
          DynamicBuffer<CharacterElement> bufferData2 = new DynamicBuffer<CharacterElement>();
          int priority = 0;
          DynamicBuffer<SubMeshGroup> bufferData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData3))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_MeshGroups.TryGetBuffer(cullingData.m_Entity, out bufferData1);
            // ISSUE: reference to a compiler-generated field
            this.m_CharacterElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData2);
            // ISSUE: reference to a compiler-generated method
            local = ObjectInterpolateSystem.CalculateTransform(frame1, frame2, framePosition);
            // ISSUE: reference to a compiler-generated field
            CullingInfo cullingInfo = this.m_CullingInfoData[cullingData.m_Entity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance = (double) RenderingUtils.CalculateMinDistance(cullingInfo.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            priority = RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) - (int) cullingInfo.m_MinLod;
          }
          else
          {
            if ((double) framePosition >= 0.5)
            {
              stateTimer = (float) frame2.m_StateTimer + (framePosition - 0.5f);
              state = frame2.m_State;
              activity = (ActivityType) frame2.m_Activity;
            }
            else
            {
              stateTimer = (float) frame1.m_StateTimer + (framePosition + 0.5f);
              state = frame1.m_State;
              activity = (ActivityType) frame1.m_Activity;
            }
            switch (frame1.m_State)
            {
              case TransformState.Start:
              case TransformState.End:
              case TransformState.Action:
                if ((double) framePosition >= 0.5)
                {
                  local.m_Position = frame2.m_Position;
                  local.m_Rotation = frame2.m_Rotation;
                  local.m_Flags = frame2.m_Flags;
                  break;
                }
                local.m_Position = frame1.m_Position;
                local.m_Rotation = frame1.m_Rotation;
                local.m_Flags = frame1.m_Flags;
                break;
              default:
                // ISSUE: reference to a compiler-generated method
                local = ObjectInterpolateSystem.CalculateTransform(frame1, frame2, framePosition);
                break;
            }
          }
          for (int index1 = 0; index1 < animated1.Length; ++index1)
          {
            Animated animated2 = animated1[index1];
            if (animated2.m_ClipIndexBody0 != (short) -1)
            {
              if (bufferData2.IsCreated)
              {
                MeshGroup meshGroup;
                CollectionUtils.TryGet<MeshGroup>(bufferData1, index1, out meshGroup);
                CharacterElement characterElement = bufferData2[(int) meshGroup.m_SubMeshGroup];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<AnimationClip> animationClip = this.m_AnimationClips[characterElement.m_Style];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ObjectInterpolateSystem.UpdateInterpolatedAnimationBody(cullingData.m_Entity, in characterElement, animationClip, ref this.m_HumanData, ref this.m_CurrentVehicleData, ref this.m_PrefabRefData, ref this.m_ActivityLocations, ref this.m_AnimationMotions, oldTransform, local, ref animated2, ref random, frame1, frame2, framePosition, updateFrameToSeconds, speedDeltaFactor, deltaTime, updateFrameChanged, flag);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ObjectInterpolateSystem.UpdateInterpolatedAnimationFace(cullingData.m_Entity, animationClip, ref this.m_HumanData, ref animated2, ref random, state, activity, deltaTime, updateFrameChanged, flag);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                this.m_AnimationData.SetAnimationFrame(in characterElement, animationClip, in animated2, (float2) ObjectInterpolateSystem.GetUpdateFrameTransition(framePosition), priority, flag);
              }
              else
              {
                int index2 = index1;
                if (bufferData3.IsCreated)
                {
                  MeshGroup meshGroup;
                  CollectionUtils.TryGet<MeshGroup>(bufferData1, index1, out meshGroup);
                  index2 = bufferData3[(int) meshGroup.m_SubMeshGroup].m_SubMeshRange.x;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ObjectInterpolateSystem.UpdateInterpolatedAnimation(this.m_AnimationClips[subMesh[index2].m_SubMesh], oldTransform, local, ref animated2, stateTimer, state, activity, updateFrameToSeconds, speedDeltaFactor);
              }
            }
            animated1[index1] = animated2;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          local = ObjectInterpolateSystem.CalculateTransform(frame1, frame2, framePosition);
        }
      }
    }

    [BurstCompile]
    private struct UpdateTrailerTransformDataJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<PointOfInterest> m_PointOfInterestData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<SwayingData> m_PrefabSwayingData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<CarTractorData> m_PrefabCarTractorData;
      [ReadOnly]
      public ComponentLookup<CarTrailerData> m_PrefabCarTrailerData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<TrainBogieFrame> m_BogieFrames;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [ReadOnly]
      public BufferLookup<ProceduralLight> m_ProceduralLights;
      [ReadOnly]
      public BufferLookup<LightAnimation> m_LightAnimations;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Swaying> m_SwayingData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Skeleton> m_Skeletons;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Emissive> m_Emissive;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Bone> m_Bones;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Momentum> m_Momentums;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LightState> m_LightStates;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [ReadOnly]
      public float m_FrameDelta;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData cullingData = this.m_CullingData[index];
        if ((cullingData.m_Flags & (PreCullingFlags.NearCamera | PreCullingFlags.Temp | PreCullingFlags.VehicleLayout | PreCullingFlags.Relative)) != (PreCullingFlags.NearCamera | PreCullingFlags.VehicleLayout))
          return;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(index);
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformFrames.TryGetBuffer(cullingData.m_Entity, out DynamicBuffer<TransformFrame> _))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarData.HasComponent(cullingData.m_Entity))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateInterpolatedCarTrailers(cullingData, ref random);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateInterpolatedLayoutAnimations(cullingData);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ParkedTrainData.HasComponent(cullingData.m_Entity))
            return;
          // ISSUE: reference to a compiler-generated method
          this.UpdateStaticLayoutAnimations(cullingData);
        }
      }

      private void UpdateInterpolatedCarTrailers(PreCullingData cullingData, ref Random random)
      {
        uint updateFrame1;
        uint updateFrame2;
        float framePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_FrameTime, (uint) cullingData.m_UpdateFrame, out updateFrame1, out updateFrame2, out framePosition);
        // ISSUE: reference to a compiler-generated field
        float deltaTime = this.m_FrameDelta / 60f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float speedDeltaFactor = math.select(60f / this.m_FrameDelta, 0.0f, (double) this.m_FrameDelta == 0.0);
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LayoutElement> layoutElement = this.m_LayoutElements[cullingData.m_Entity];
        if (layoutElement.Length <= 1)
          return;
        // ISSUE: reference to a compiler-generated field
        InterpolatedTransform interpolatedTransform1 = this.m_InterpolatedTransformData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        PseudoRandomSeed pseudoRandomSeed = this.m_PseudoRandomSeedData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        CarTractorData carTractorData = this.m_PrefabCarTractorData[prefabRef1.m_Prefab];
        bool instantReset = (cullingData.m_Flags & PreCullingFlags.NearCameraUpdated) > (PreCullingFlags) 0;
        for (int index1 = 1; index1 < layoutElement.Length; ++index1)
        {
          Entity vehicle = layoutElement[index1].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          InterpolatedTransform interpolatedTransform2 = this.m_InterpolatedTransformData[vehicle];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef2 = this.m_PrefabRefData[vehicle];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<TransformFrame> transformFrame = this.m_TransformFrames[vehicle];
          // ISSUE: reference to a compiler-generated field
          CarData carData = this.m_PrefabCarData[prefabRef2.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          CarTrailerData carTrailerData = this.m_PrefabCarTrailerData[prefabRef2.m_Prefab];
          TransformFrame frame1 = transformFrame[(int) updateFrame1];
          TransformFrame frame2 = transformFrame[(int) updateFrame2];
          // ISSUE: reference to a compiler-generated method
          InterpolatedTransform transform = ObjectInterpolateSystem.CalculateTransform(frame1, frame2, framePosition);
          switch (carTrailerData.m_MovementType)
          {
            case TrailerMovementType.Free:
              float3 float3_1 = interpolatedTransform1.m_Position + math.rotate(interpolatedTransform1.m_Rotation, carTractorData.m_AttachPosition);
              float3 float3_2 = transform.m_Position + math.rotate(transform.m_Rotation, new float3(carTrailerData.m_AttachPosition.xy, carData.m_PivotOffset));
              transform.m_Rotation = interpolatedTransform1.m_Rotation;
              float3 forward = float3_1 - float3_2;
              if (MathUtils.TryNormalize(ref forward))
                transform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
              transform.m_Position = float3_1 - math.rotate(transform.m_Rotation, carTrailerData.m_AttachPosition);
              break;
            case TrailerMovementType.Locked:
              transform.m_Position = interpolatedTransform1.m_Position;
              transform.m_Position -= math.rotate(transform.m_Rotation, carTrailerData.m_AttachPosition);
              transform.m_Position += math.rotate(interpolatedTransform1.m_Rotation, carTractorData.m_AttachPosition);
              transform.m_Rotation = interpolatedTransform1.m_Rotation;
              break;
          }
          quaternion swayRotation = quaternion.identity;
          float swayOffset = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SwayingData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            Swaying swaying = this.m_SwayingData[vehicle];
            if (instantReset)
            {
              swaying.m_LastVelocity = math.lerp(frame1.m_Velocity, frame2.m_Velocity, framePosition);
              swaying.m_SwayVelocity = (float3) 0.0f;
              swaying.m_SwayPosition = (float3) 0.0f;
            }
            else
            {
              SwayingData componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabSwayingData.TryGetComponent(prefabRef2.m_Prefab, out componentData))
              {
                componentData.m_MaxPosition.z = 0.0f;
                // ISSUE: reference to a compiler-generated method
                ObjectInterpolateSystem.UpdateSwaying(componentData, interpolatedTransform2, ref transform, ref swaying, deltaTime, speedDeltaFactor, out swayRotation, out swayOffset);
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_SwayingData[vehicle] = swaying;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_Skeletons.HasBuffer(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[vehicle];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Bone> bone = this.m_Bones[vehicle];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubMesh> subMesh1 = this.m_SubMeshes[prefabRef2.m_Prefab];
            DynamicBuffer<Momentum> momentums = new DynamicBuffer<Momentum>();
            // ISSUE: reference to a compiler-generated field
            if (this.m_Momentums.HasBuffer(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              momentums = this.m_Momentums[vehicle];
            }
            for (int index2 = 0; index2 < skeleton.Length; ++index2)
            {
              ref Skeleton local = ref skeleton.ElementAt(index2);
              if (!local.m_BufferAllocation.Empty)
              {
                SubMesh subMesh2 = subMesh1[index2];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ProceduralBone> proceduralBone = this.m_ProceduralBones[subMesh2.m_SubMesh];
                InterpolatedTransform oldTransform = interpolatedTransform2;
                InterpolatedTransform newTransform = transform;
                if ((subMesh2.m_Flags & SubMeshFlags.HasTransform) != (SubMeshFlags) 0)
                {
                  oldTransform = ObjectUtils.LocalToWorld(interpolatedTransform2, subMesh2.m_Position, subMesh2.m_Rotation);
                  newTransform = ObjectUtils.LocalToWorld(transform, subMesh2.m_Position, subMesh2.m_Rotation);
                }
                float steeringRadius = 0.0f;
                CarData componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabCarData.TryGetComponent(prefabRef1.m_Prefab, out componentData))
                {
                  // ISSUE: reference to a compiler-generated method
                  steeringRadius = ObjectInterpolateSystem.CalculateSteeringRadius(proceduralBone, bone, oldTransform, newTransform, ref local, componentData);
                }
                for (int index3 = 0; index3 < proceduralBone.Length; ++index3)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  ObjectInterpolateSystem.AnimateInterpolatedBone(proceduralBone, bone, momentums, oldTransform, newTransform, prefabRef1, ref local, swayRotation, swayOffset, steeringRadius, componentData.m_PivotOffset, index3, deltaTime, vehicle, instantReset, this.m_FrameIndex, this.m_FrameTime, ref random, ref this.m_PointOfInterestData, ref this.m_CurveData, ref this.m_PrefabRefData, ref this.m_PrefabUtilityLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneSearchTree);
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_Emissive.HasBuffer(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Emissive> dynamicBuffer = this.m_Emissive[vehicle];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<LightState> lightState = this.m_LightStates[vehicle];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubMesh> subMesh3 = this.m_SubMeshes[prefabRef2.m_Prefab];
            Random random1 = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kLightState);
            for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
            {
              ref Emissive local = ref dynamicBuffer.ElementAt(index4);
              if (!local.m_BufferAllocation.Empty)
              {
                SubMesh subMesh4 = subMesh3[index4];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ProceduralLight> proceduralLight = this.m_ProceduralLights[subMesh4.m_SubMesh];
                DynamicBuffer<LightAnimation> bufferData;
                // ISSUE: reference to a compiler-generated field
                this.m_LightAnimations.TryGetBuffer(subMesh4.m_SubMesh, out bufferData);
                for (int index5 = 0; index5 < proceduralLight.Length; ++index5)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  ObjectInterpolateSystem.AnimateInterpolatedLight(proceduralLight, bufferData, lightState, transform.m_Flags, random1, ref local, index5, this.m_FrameIndex, this.m_FrameTime, deltaTime, instantReset);
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_InterpolatedTransformData[vehicle] = transform;
          if (index1 != layoutElement.Length - 1)
          {
            interpolatedTransform1 = transform;
            // ISSUE: reference to a compiler-generated field
            carTractorData = this.m_PrefabCarTractorData[prefabRef2.m_Prefab];
          }
        }
      }

      private void UpdateInterpolatedLayoutAnimations(PreCullingData cullingData)
      {
        uint updateFrame1;
        uint updateFrame2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_FrameTime, (uint) cullingData.m_UpdateFrame, out updateFrame1, out updateFrame2, out float _);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LayoutElement> layoutElement = this.m_LayoutElements[cullingData.m_Entity];
        if (layoutElement.Length == 0)
          return;
        Entity entity1 = layoutElement[0].m_Vehicle;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[entity1];
        InterpolatedTransform prevTransform = new InterpolatedTransform();
        // ISSUE: reference to a compiler-generated field
        InterpolatedTransform curTransform = this.m_InterpolatedTransformData[entity1];
        ObjectGeometryData prevGeometryData = new ObjectGeometryData();
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData curGeometryData = this.m_PrefabObjectGeometryData[prefabRef1.m_Prefab];
        bool prevReversed = false;
        bool curReversed = false;
        Train componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TrainData.TryGetComponent(entity1, out componentData))
          curReversed = (componentData.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0;
        for (int index1 = 0; index1 < layoutElement.Length; ++index1)
        {
          Entity entity2 = new Entity();
          PrefabRef prefabRef2 = new PrefabRef();
          InterpolatedTransform nextTransform = new InterpolatedTransform();
          ObjectGeometryData nextGeometryData = new ObjectGeometryData();
          bool nextReversed = false;
          if (index1 < layoutElement.Length - 1)
          {
            entity2 = layoutElement[index1 + 1].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            prefabRef2 = this.m_PrefabRefData[entity2];
            // ISSUE: reference to a compiler-generated field
            nextTransform = this.m_InterpolatedTransformData[entity2];
            // ISSUE: reference to a compiler-generated field
            nextGeometryData = this.m_PrefabObjectGeometryData[prefabRef2.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrainData.TryGetComponent(entity2, out componentData))
              nextReversed = (componentData.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_Skeletons.HasBuffer(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[entity1];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Bone> bone = this.m_Bones[entity1];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubMesh> subMesh = this.m_SubMeshes[prefabRef1.m_Prefab];
            TrainBogieFrame bogieFrame1 = new TrainBogieFrame();
            TrainBogieFrame bogieFrame2 = new TrainBogieFrame();
            DynamicBuffer<TrainBogieFrame> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BogieFrames.TryGetBuffer(entity1, out bufferData))
            {
              bogieFrame1 = bufferData[(int) updateFrame1];
              bogieFrame2 = bufferData[(int) updateFrame2];
            }
            for (int index2 = 0; index2 < skeleton.Length; ++index2)
            {
              ref Skeleton local = ref skeleton.ElementAt(index2);
              if (!local.m_BufferAllocation.Empty)
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ProceduralBone> proceduralBone = this.m_ProceduralBones[subMesh[index2].m_SubMesh];
                for (int index3 = 0; index3 < proceduralBone.Length; ++index3)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AnimateInterpolatedLayoutBone(proceduralBone, bone, prevTransform, curTransform, nextTransform, prevGeometryData, curGeometryData, nextGeometryData, bogieFrame1, bogieFrame2, prevReversed, curReversed, nextReversed, ref local, index3);
                }
              }
            }
          }
          prevTransform = curTransform;
          prevGeometryData = curGeometryData;
          prevReversed = curReversed;
          entity1 = entity2;
          prefabRef1 = prefabRef2;
          curTransform = nextTransform;
          curGeometryData = nextGeometryData;
          curReversed = nextReversed;
        }
      }

      private void UpdateStaticLayoutAnimations(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LayoutElement> layoutElement = this.m_LayoutElements[cullingData.m_Entity];
        if (layoutElement.Length == 0)
          return;
        Entity entity1 = layoutElement[0].m_Vehicle;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[entity1];
        Transform prevTransform = new Transform();
        // ISSUE: reference to a compiler-generated field
        Transform curTransform = this.m_TransformData[entity1];
        ObjectGeometryData prevGeometryData = new ObjectGeometryData();
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData curGeometryData = this.m_PrefabObjectGeometryData[prefabRef1.m_Prefab];
        bool prevReversed = false;
        bool curReversed = false;
        Train componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TrainData.TryGetComponent(entity1, out componentData))
          curReversed = (componentData.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0;
        for (int index1 = 0; index1 < layoutElement.Length; ++index1)
        {
          Entity entity2 = new Entity();
          PrefabRef prefabRef2 = new PrefabRef();
          Transform nextTransform = new Transform();
          ObjectGeometryData nextGeometryData = new ObjectGeometryData();
          bool nextReversed = false;
          if (index1 < layoutElement.Length - 1)
          {
            entity2 = layoutElement[index1 + 1].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            prefabRef2 = this.m_PrefabRefData[entity2];
            // ISSUE: reference to a compiler-generated field
            nextTransform = this.m_TransformData[entity2];
            // ISSUE: reference to a compiler-generated field
            nextGeometryData = this.m_PrefabObjectGeometryData[prefabRef2.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrainData.TryGetComponent(entity2, out componentData))
              nextReversed = (componentData.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_Skeletons.HasBuffer(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            ParkedTrain parkedTrain = this.m_ParkedTrainData[entity1];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[entity1];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Bone> bone = this.m_Bones[entity1];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubMesh> subMesh = this.m_SubMeshes[prefabRef1.m_Prefab];
            for (int index2 = 0; index2 < skeleton.Length; ++index2)
            {
              ref Skeleton local = ref skeleton.ElementAt(index2);
              if (!local.m_BufferAllocation.Empty)
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ProceduralBone> proceduralBone = this.m_ProceduralBones[subMesh[index2].m_SubMesh];
                for (int index3 = 0; index3 < proceduralBone.Length; ++index3)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AnimateStaticLayoutBone(proceduralBone, bone, prevTransform, curTransform, nextTransform, parkedTrain, prevGeometryData, curGeometryData, nextGeometryData, prevReversed, curReversed, nextReversed, ref local, index3);
                }
              }
            }
          }
          prevTransform = curTransform;
          prevGeometryData = curGeometryData;
          prevReversed = curReversed;
          entity1 = entity2;
          prefabRef1 = prefabRef2;
          curTransform = nextTransform;
          curGeometryData = nextGeometryData;
          curReversed = nextReversed;
        }
      }

      private void AnimateInterpolatedLayoutBone(
        DynamicBuffer<ProceduralBone> proceduralBones,
        DynamicBuffer<Bone> bones,
        InterpolatedTransform prevTransform,
        InterpolatedTransform curTransform,
        InterpolatedTransform nextTransform,
        ObjectGeometryData prevGeometryData,
        ObjectGeometryData curGeometryData,
        ObjectGeometryData nextGeometryData,
        TrainBogieFrame bogieFrame1,
        TrainBogieFrame bogieFrame2,
        bool prevReversed,
        bool curReversed,
        bool nextReversed,
        ref Skeleton skeleton,
        int index)
      {
        ProceduralBone proceduralBone = proceduralBones[index];
        int index1 = skeleton.m_BoneOffset + index;
        ref Bone local = ref bones.ElementAt(index1);
        switch (proceduralBone.m_Type)
        {
          case BoneType.VehicleConnection:
            // ISSUE: reference to a compiler-generated method
            this.AnimateVehicleConnectionBone(proceduralBone, ref skeleton, ref local, prevGeometryData, curGeometryData, nextGeometryData, prevTransform.ToTransform(), curTransform.ToTransform(), nextTransform.ToTransform(), prevReversed, curReversed, nextReversed);
            break;
          case BoneType.TrainBogie:
            float num1 = 2f;
            Entity entity1;
            Entity entity2;
            if ((double) proceduralBone.m_ObjectPosition.z >= 0.0 == curReversed)
            {
              entity1 = bogieFrame1.m_RearLane;
              entity2 = bogieFrame2.m_RearLane;
            }
            else
            {
              entity1 = bogieFrame1.m_FrontLane;
              entity2 = bogieFrame2.m_FrontLane;
            }
            float3 world = ObjectUtils.LocalToWorld(curTransform.ToTransform(), new float3(0.0f, 0.0f, proceduralBone.m_ObjectPosition.z));
            float3 position = new float3();
            float3 tangent = new float3();
            Curve componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.TryGetComponent(entity1, out componentData1))
            {
              float t;
              float num2 = MathUtils.Distance(componentData1.m_Bezier, world, out t);
              if ((double) num2 < (double) num1)
              {
                position = MathUtils.Position(componentData1.m_Bezier, t);
                tangent = MathUtils.Tangent(componentData1.m_Bezier, t);
                num1 = num2;
              }
            }
            Curve componentData2;
            // ISSUE: reference to a compiler-generated field
            if (entity1 != entity2 && this.m_CurveData.TryGetComponent(entity2, out componentData2))
            {
              float t;
              float num3 = MathUtils.Distance(componentData2.m_Bezier, world, out t);
              if ((double) num3 < (double) num1)
              {
                position = MathUtils.Position(componentData2.m_Bezier, t);
                tangent = MathUtils.Tangent(componentData2.m_Bezier, t);
                num1 = num3;
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrainBogieBone(curTransform.ToTransform(), proceduralBone, ref skeleton, ref local, position, tangent, (double) num1 != 2.0);
            break;
        }
      }

      private void AnimateStaticLayoutBone(
        DynamicBuffer<ProceduralBone> proceduralBones,
        DynamicBuffer<Bone> bones,
        Transform prevTransform,
        Transform curTransform,
        Transform nextTransform,
        ParkedTrain parkedTrain,
        ObjectGeometryData prevGeometryData,
        ObjectGeometryData curGeometryData,
        ObjectGeometryData nextGeometryData,
        bool prevReversed,
        bool curReversed,
        bool nextReversed,
        ref Skeleton skeleton,
        int index)
      {
        ProceduralBone proceduralBone = proceduralBones[index];
        int index1 = skeleton.m_BoneOffset + index;
        ref Bone local = ref bones.ElementAt(index1);
        switch (proceduralBone.m_Type)
        {
          case BoneType.VehicleConnection:
            // ISSUE: reference to a compiler-generated method
            this.AnimateVehicleConnectionBone(proceduralBone, ref skeleton, ref local, prevGeometryData, curGeometryData, nextGeometryData, prevTransform, curTransform, nextTransform, prevReversed, curReversed, nextReversed);
            break;
          case BoneType.TrainBogie:
            Entity entity;
            float t;
            if ((double) proceduralBone.m_ObjectPosition.z >= 0.0 == curReversed)
            {
              entity = parkedTrain.m_RearLane;
              t = parkedTrain.m_CurvePosition.y;
            }
            else
            {
              entity = parkedTrain.m_FrontLane;
              t = parkedTrain.m_CurvePosition.x;
            }
            float3 position = new float3();
            float3 tangent = new float3();
            Curve componentData;
            // ISSUE: reference to a compiler-generated field
            bool component = this.m_CurveData.TryGetComponent(entity, out componentData);
            if (component)
            {
              position = MathUtils.Position(componentData.m_Bezier, t);
              tangent = MathUtils.Tangent(componentData.m_Bezier, t);
            }
            // ISSUE: reference to a compiler-generated method
            this.AnimateTrainBogieBone(curTransform, proceduralBone, ref skeleton, ref local, position, tangent, component);
            break;
        }
      }

      private void AnimateVehicleConnectionBone(
        ProceduralBone proceduralBone,
        ref Skeleton skeleton,
        ref Bone bone,
        ObjectGeometryData prevGeometryData,
        ObjectGeometryData curGeometryData,
        ObjectGeometryData nextGeometryData,
        Transform prevTransform,
        Transform curTransform,
        Transform nextTransform,
        bool prevReversed,
        bool curReversed,
        bool nextReversed)
      {
        quaternion quaternion1 = math.inverse(curTransform.m_Rotation);
        float3 x1;
        float3 y;
        quaternion quaternion2;
        if ((double) proceduralBone.m_ObjectPosition.z >= 0.0 == curReversed)
        {
          if ((double) nextGeometryData.m_Bounds.max.z == (double) nextGeometryData.m_Bounds.min.z)
          {
            skeleton.m_CurrentUpdated |= !bone.m_Position.Equals(proceduralBone.m_Position) | !bone.m_Rotation.Equals(proceduralBone.m_Rotation);
            bone.m_Position = proceduralBone.m_Position;
            bone.m_Rotation = proceduralBone.m_Rotation;
            return;
          }
          x1 = new float3(proceduralBone.m_Position.xy, math.select(curGeometryData.m_Bounds.min.z, curGeometryData.m_Bounds.max.z, curReversed));
          float3 v = new float3(proceduralBone.m_Position.xy, math.select(nextGeometryData.m_Bounds.max.z, nextGeometryData.m_Bounds.min.z, nextReversed));
          float3 float3 = math.rotate(nextTransform.m_Rotation, v);
          y = math.rotate(quaternion1, float3 + (nextTransform.m_Position - curTransform.m_Position));
          quaternion2 = math.mul(quaternion1, nextTransform.m_Rotation);
          if (nextReversed != curReversed)
            quaternion2 = math.mul(quaternion2, quaternion.RotateY(3.14159274f));
        }
        else
        {
          if ((double) prevGeometryData.m_Bounds.max.z == (double) prevGeometryData.m_Bounds.min.z)
          {
            skeleton.m_CurrentUpdated |= !bone.m_Position.Equals(proceduralBone.m_Position) | !bone.m_Rotation.Equals(proceduralBone.m_Rotation);
            bone.m_Position = proceduralBone.m_Position;
            bone.m_Rotation = proceduralBone.m_Rotation;
            return;
          }
          x1 = new float3(proceduralBone.m_Position.xy, math.select(curGeometryData.m_Bounds.max.z, curGeometryData.m_Bounds.min.z, curReversed));
          float3 v = new float3(proceduralBone.m_Position.xy, math.select(prevGeometryData.m_Bounds.min.z, prevGeometryData.m_Bounds.max.z, prevReversed));
          float3 float3 = math.rotate(prevTransform.m_Rotation, v);
          y = math.rotate(quaternion1, float3 + (prevTransform.m_Position - curTransform.m_Position));
          quaternion2 = math.mul(quaternion1, prevTransform.m_Rotation);
          if (prevReversed != curReversed)
            quaternion2 = math.mul(quaternion2, quaternion.RotateY(3.14159274f));
        }
        float num = (float) ((double) math.sign(x1.z) * (double) math.distance(x1, y) * 0.25);
        x1.z += num;
        float3 float3_1 = y + math.rotate(quaternion2, new float3(0.0f, 0.0f, -num));
        float3 rhs = (x1 + float3_1) * 0.5f;
        quaternion x2 = math.slerp(quaternion.identity, quaternion2, 0.5f);
        skeleton.m_CurrentUpdated |= !bone.m_Position.Equals(rhs) | !bone.m_Rotation.Equals(x2);
        bone.m_Position = rhs;
        bone.m_Rotation = x2;
      }

      private void AnimateTrainBogieBone(
        Transform transform,
        ProceduralBone proceduralBone,
        ref Skeleton skeleton,
        ref Bone bone,
        float3 position,
        float3 tangent,
        bool positionValid)
      {
        float3 rhs = proceduralBone.m_Position;
        quaternion x = proceduralBone.m_Rotation;
        if (positionValid)
        {
          quaternion quaternion = math.inverse(transform.m_Rotation);
          rhs = math.rotate(quaternion, position - transform.m_Position);
          rhs.y += proceduralBone.m_Position.y;
          float3 y = math.forward(math.mul(transform.m_Rotation, proceduralBone.m_ObjectRotation));
          tangent = math.select(tangent, -tangent, (double) math.dot(tangent, y) < 0.0);
          x = math.mul(quaternion, quaternion.LookRotationSafe(tangent, math.up()));
        }
        skeleton.m_CurrentUpdated |= !bone.m_Position.Equals(rhs) | !bone.m_Rotation.Equals(x);
        bone.m_Position = rhs;
        bone.m_Rotation = x;
      }
    }

    [BurstCompile]
    private struct UpdateQueryTransformDataJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [ReadOnly]
      public ComponentTypeHandle<Static> m_StaticType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.Animation> m_AnimationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> m_MeshGroupType;
      [ReadOnly]
      public BufferTypeHandle<EnabledEffect> m_EffectInstancesType;
      [ReadOnly]
      public BufferTypeHandle<IconElement> m_IconElementType;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<ProceduralLight> m_ProceduralLights;
      [ReadOnly]
      public BufferLookup<CharacterElement> m_CharacterElements;
      [ReadOnly]
      public BufferLookup<AnimationClip> m_AnimationClips;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Transform> m_TransformData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Animated> m_Animateds;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Skeleton> m_Skeletons;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Emissive> m_Emissives;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Bone> m_Bones;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LightState> m_Lights;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public NativeList<EnabledEffectData> m_EnabledData;
      public AnimatedSystem.AnimationData m_AnimationData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CullingInfo> nativeArray3 = chunk.GetNativeArray<CullingInfo>(ref this.m_CullingInfoType);
        uint updateFrame1 = 0;
        uint updateFrame2 = 0;
        float framePosition = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<UpdateFrame>(this.m_UpdateFrameType))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_FrameTime, chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index, out updateFrame1, out updateFrame2, out framePosition);
        }
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Tools.Animation> nativeArray4 = chunk.GetNativeArray<Game.Tools.Animation>(ref this.m_AnimationType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<MeshGroup> bufferAccessor1 = chunk.GetBufferAccessor<MeshGroup>(ref this.m_MeshGroupType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<EnabledEffect> bufferAccessor2 = chunk.GetBufferAccessor<EnabledEffect>(ref this.m_EffectInstancesType);
          // ISSUE: reference to a compiler-generated field
          bool flag1 = chunk.Has<Static>(ref this.m_StaticType);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = chunk.Has<Stopped>(ref this.m_StoppedType);
          // ISSUE: reference to a compiler-generated field
          bool flag3 = chunk.Has<IconElement>(ref this.m_IconElementType);
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity entity = nativeArray1[index1];
            Temp temp = nativeArray2[index1];
            if (!flag3)
            {
              CullingInfo cullingInfo = nativeArray3[index1];
              // ISSUE: reference to a compiler-generated field
              if (cullingInfo.m_CullingIndex == 0 || (this.m_CullingData[cullingInfo.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
                continue;
            }
            Animated animated1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InterpolatedTransformData.HasComponent(entity))
            {
              if (flag1 | flag2 && (temp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0 || (temp.m_Flags & TempFlags.Dragging) != (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_InterpolatedTransformData[entity] = nativeArray4.Length == 0 ? new InterpolatedTransform(this.m_TransformData[entity]) : new InterpolatedTransform(nativeArray4[index1].ToTransform());
                // ISSUE: reference to a compiler-generated field
                if (this.m_Animateds.HasBuffer(entity))
                {
                  PrefabRef prefabRef = nativeArray5[index1];
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Animated> animated2 = this.m_Animateds[entity];
                  DynamicBuffer<MeshGroup> buffer;
                  CollectionUtils.TryGet<MeshGroup>(bufferAccessor1, index1, out buffer);
                  DynamicBuffer<CharacterElement> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CharacterElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData);
                  for (int index2 = 0; index2 < animated2.Length; ++index2)
                  {
                    Animated animated3 = animated2[index2];
                    if (animated3.m_ClipIndexBody0 != (short) -1)
                    {
                      animated3.m_ClipIndexBody0 = (short) 0;
                      animated3.m_Time = (float4) 0.0f;
                      animated3.m_MovementSpeed = (float2) 0.0f;
                      animated3.m_Interpolation = 0.0f;
                      animated3.m_PreviousTime = 0.0f;
                      animated2[index2] = animated3;
                    }
                    if (animated3.m_MetaIndex != 0 && bufferData.IsCreated)
                    {
                      MeshGroup meshGroup;
                      CollectionUtils.TryGet<MeshGroup>(buffer, index2, out meshGroup);
                      CharacterElement characterElement = bufferData[(int) meshGroup.m_SubMeshGroup];
                      animated1 = new Animated();
                      animated1.m_MetaIndex = animated3.m_MetaIndex;
                      animated1.m_ClipIndexBody0 = (short) -1;
                      animated1.m_ClipIndexBody0I = (short) -1;
                      animated1.m_ClipIndexBody1 = (short) -1;
                      animated1.m_ClipIndexBody1I = (short) -1;
                      animated1.m_ClipIndexFace0 = (short) -1;
                      animated1.m_ClipIndexFace1 = (short) -1;
                      Animated animated4 = animated1;
                      // ISSUE: reference to a compiler-generated field
                      DynamicBuffer<AnimationClip> animationClip = this.m_AnimationClips[characterElement.m_Style];
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      this.m_AnimationData.SetAnimationFrame(in characterElement, animationClip, in animated4, (float2) 0.0f, -1, true);
                    }
                  }
                  continue;
                }
                continue;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                Transform transform = this.m_TransformData[temp.m_Original];
                // ISSUE: reference to a compiler-generated field
                this.m_TransformData[entity] = transform;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_InterpolatedTransformData[entity] = !this.m_InterpolatedTransformData.HasComponent(temp.m_Original) ? new InterpolatedTransform(transform) : this.m_InterpolatedTransformData[temp.m_Original];
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_InterpolatedTransformData[entity] = new InterpolatedTransform(this.m_TransformData[entity]);
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_Animateds.HasBuffer(entity))
            {
              PrefabRef prefabRef = nativeArray5[index1];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Animated> animated5 = this.m_Animateds[entity];
              bool reset = false;
              CullingInfo cullingInfo = nativeArray3[index1];
              if (cullingInfo.m_CullingIndex != 0)
              {
                // ISSUE: reference to a compiler-generated field
                reset = (this.m_CullingData[cullingInfo.m_CullingIndex].m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated)) > (PreCullingFlags) 0;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double minDistance = (double) RenderingUtils.CalculateMinDistance(cullingInfo.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
              // ISSUE: reference to a compiler-generated field
              int priority = RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) - (int) cullingInfo.m_MinLod;
              DynamicBuffer<MeshGroup> buffer;
              CollectionUtils.TryGet<MeshGroup>(bufferAccessor1, index1, out buffer);
              DynamicBuffer<CharacterElement> bufferData1;
              // ISSUE: reference to a compiler-generated field
              this.m_CharacterElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData1);
              DynamicBuffer<Animated> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Animateds.TryGetBuffer(temp.m_Original, out bufferData2) && bufferData2.Length == animated5.Length)
              {
                for (int index3 = 0; index3 < animated5.Length; ++index3)
                {
                  Animated animated6 = animated5[index3];
                  Animated animated7 = bufferData2[index3];
                  animated6.m_ClipIndexBody0 = animated7.m_ClipIndexBody0;
                  animated6.m_ClipIndexBody0I = animated7.m_ClipIndexBody0I;
                  animated6.m_ClipIndexBody1 = animated7.m_ClipIndexBody1;
                  animated6.m_ClipIndexBody1I = animated7.m_ClipIndexBody1I;
                  animated6.m_ClipIndexFace0 = animated7.m_ClipIndexFace0;
                  animated6.m_ClipIndexFace1 = animated7.m_ClipIndexFace1;
                  animated6.m_Time = animated7.m_Time;
                  animated6.m_MovementSpeed = animated7.m_MovementSpeed;
                  animated6.m_Interpolation = animated7.m_Interpolation;
                  animated6.m_PreviousTime = animated7.m_PreviousTime;
                  animated5[index3] = animated6;
                  if (animated6.m_MetaIndex != 0 && bufferData1.IsCreated)
                  {
                    MeshGroup meshGroup;
                    CollectionUtils.TryGet<MeshGroup>(buffer, index3, out meshGroup);
                    CharacterElement characterElement = bufferData1[(int) meshGroup.m_SubMeshGroup];
                    float num = framePosition * framePosition;
                    float transition = (float) (3.0 * (double) num - 2.0 * (double) num * (double) framePosition);
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<AnimationClip> animationClip = this.m_AnimationClips[characterElement.m_Style];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.m_AnimationData.SetAnimationFrame(in characterElement, animationClip, in animated6, (float2) transition, priority, reset);
                  }
                }
              }
              else
              {
                for (int index4 = 0; index4 < animated5.Length; ++index4)
                {
                  Animated animated8 = animated5[index4];
                  if (animated8.m_ClipIndexBody0 != (short) -1)
                  {
                    animated8.m_ClipIndexBody0 = (short) 0;
                    animated8.m_Time = (float4) 0.0f;
                    animated8.m_MovementSpeed = (float2) 0.0f;
                    animated8.m_Interpolation = 0.0f;
                    animated8.m_PreviousTime = 0.0f;
                    animated5[index4] = animated8;
                  }
                  if (animated8.m_MetaIndex != 0 && bufferData1.IsCreated)
                  {
                    MeshGroup meshGroup;
                    CollectionUtils.TryGet<MeshGroup>(buffer, index4, out meshGroup);
                    CharacterElement characterElement = bufferData1[(int) meshGroup.m_SubMeshGroup];
                    animated1 = new Animated();
                    animated1.m_MetaIndex = animated8.m_MetaIndex;
                    animated1.m_ClipIndexBody0 = (short) -1;
                    animated1.m_ClipIndexBody0I = (short) -1;
                    animated1.m_ClipIndexBody1 = (short) -1;
                    animated1.m_ClipIndexBody1I = (short) -1;
                    animated1.m_ClipIndexFace0 = (short) -1;
                    animated1.m_ClipIndexFace1 = (short) -1;
                    Animated animated9 = animated1;
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<AnimationClip> animationClip = this.m_AnimationClips[characterElement.m_Style];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.m_AnimationData.SetAnimationFrame(in characterElement, animationClip, in animated9, (float2) 0.0f, -1, reset);
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_Bones.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[entity];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Bone> bone1 = this.m_Bones[entity];
              // ISSUE: reference to a compiler-generated field
              if (this.m_Bones.HasBuffer(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Bone> bone2 = this.m_Bones[temp.m_Original];
                if (bone1.Length == bone2.Length)
                {
                  for (int index5 = 0; index5 < skeleton.Length; ++index5)
                    skeleton.ElementAt(index5).m_CurrentUpdated = true;
                  for (int index6 = 0; index6 < bone1.Length; ++index6)
                    bone1[index6] = bone2[index6];
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_Lights.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Emissive> emissive = this.m_Emissives[entity];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LightState> light = this.m_Lights[entity];
              DynamicBuffer<EnabledEffect> dynamicBuffer1 = new DynamicBuffer<EnabledEffect>();
              if (bufferAccessor2.Length != 0)
                dynamicBuffer1 = bufferAccessor2[index1];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<SubMesh> subMesh = this.m_SubMeshes[nativeArray5[index1].m_Prefab];
              DynamicBuffer<LightState> dynamicBuffer2 = new DynamicBuffer<LightState>();
              bool flag4 = false;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Lights.HasBuffer(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer2 = this.m_Lights[temp.m_Original];
                flag4 = dynamicBuffer2.Length == light.Length;
              }
              for (int index7 = 0; index7 < emissive.Length; ++index7)
              {
                ref Emissive local1 = ref emissive.ElementAt(index7);
                if (!local1.m_BufferAllocation.Empty)
                {
                  local1.m_Updated = true;
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<ProceduralLight> proceduralLight1 = this.m_ProceduralLights[subMesh[index7].m_SubMesh];
                  for (int index8 = 0; index8 < proceduralLight1.Length; ++index8)
                  {
                    int index9 = local1.m_LightOffset + index8;
                    ProceduralLight proceduralLight2 = proceduralLight1[index8];
                    ref LightState local2 = ref light.ElementAt(index9);
                    if (proceduralLight2.m_Purpose == EmissiveProperties.Purpose.EffectSource)
                    {
                      if (dynamicBuffer1.IsCreated)
                      {
                        local2.m_Intensity = 0.0f;
                        int index10 = 0;
                        if (dynamicBuffer1.Length > index10)
                        {
                          // ISSUE: reference to a compiler-generated field
                          EnabledEffectData enabledEffectData = this.m_EnabledData[dynamicBuffer1[index10].m_EnabledIndex];
                          local2.m_Intensity = math.select(0.0f, 1f, (enabledEffectData.m_Flags & EnabledEffectFlags.IsEnabled) > (EnabledEffectFlags) 0);
                        }
                      }
                    }
                    else if (flag4)
                      local2 = dynamicBuffer2[index9];
                  }
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            CullingInfo cullingInfo = nativeArray3[index];
            // ISSUE: reference to a compiler-generated field
            if (cullingInfo.m_CullingIndex == 0 || (this.m_CullingData[cullingInfo.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
            {
              DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor[index];
              TransformFrame frame1 = dynamicBuffer[(int) updateFrame1];
              TransformFrame frame2 = dynamicBuffer[(int) updateFrame2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_InterpolatedTransformData[nativeArray1[index]] = ObjectInterpolateSystem.CalculateTransform(frame1, frame2, framePosition);
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

    private struct CatenaryIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Bounds3 m_Bounds;
      public Line3.Segment m_Line;
      public float3 m_Result;
      public float m_Default;
      public ComponentLookup<Curve> m_CurveData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        // ISSUE: reference to a compiler-generated field
        return MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
      {
        UtilityLaneData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds) || !this.m_PrefabUtilityLaneData.TryGetComponent(this.m_PrefabRefData[item].m_Prefab, out componentData) || (componentData.m_UtilityTypes & (UtilityTypes.LowVoltageLine | UtilityTypes.Catenary)) == UtilityTypes.None)
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[item];
        float t;
        // ISSUE: reference to a compiler-generated field
        double num = (double) MathUtils.Distance(curve.m_Bezier, MathUtils.Position(this.m_Line, 0.5f), out t);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float y = math.max(0.0f, MathUtils.Distance(this.m_Line, MathUtils.Position(curve.m_Bezier, t), out t) - this.m_Default * 0.5f);
        // ISSUE: reference to a compiler-generated field
        float x = (float) ((double) t * (double) this.m_Default * 2.0);
        float3 b = new float3(x, y, x + y);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Result = math.select(this.m_Result, b, (double) b.z < (double) this.m_Result.z);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PointOfInterest> __Game_Common_PointOfInterest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficLight> __Game_Objects_TrafficLight_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ExtractorFacility> __Game_Buildings_ExtractorFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Car> __Game_Vehicles_Car_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTrailer> __Game_Vehicles_CarTrailer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Human> __Game_Creatures_Human_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SwayingData> __Game_Prefabs_SwayingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<EnabledEffect> __Game_Effects_EnabledEffect_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AnimationClip> __Game_Prefabs_AnimationClip_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AnimationMotion> __Game_Prefabs_AnimationMotion_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralLight> __Game_Prefabs_ProceduralLight_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LightAnimation> __Game_Prefabs_LightAnimation_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CharacterElement> __Game_Prefabs_CharacterElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RW_ComponentLookup;
      public ComponentLookup<Swaying> __Game_Rendering_Swaying_RW_ComponentLookup;
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RW_BufferLookup;
      public BufferLookup<Emissive> __Game_Rendering_Emissive_RW_BufferLookup;
      public BufferLookup<Animated> __Game_Rendering_Animated_RW_BufferLookup;
      public BufferLookup<Bone> __Game_Rendering_Bone_RW_BufferLookup;
      public BufferLookup<Momentum> __Game_Rendering_Momentum_RW_BufferLookup;
      public BufferLookup<LightState> __Game_Rendering_LightState_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTractorData> __Game_Prefabs_CarTractorData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTrailerData> __Game_Prefabs_CarTrailerData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TrainBogieFrame> __Game_Vehicles_TrainBogieFrame_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Static> __Game_Objects_Static_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.Animation> __Game_Tools_Animation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<EnabledEffect> __Game_Effects_EnabledEffect_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<IconElement> __Game_Notifications_IconElement_RO_BufferTypeHandle;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PointOfInterest_RO_ComponentLookup = state.GetComponentLookup<PointOfInterest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TrafficLight_RO_ComponentLookup = state.GetComponentLookup<TrafficLight>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ExtractorFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ExtractorFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentLookup = state.GetComponentLookup<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailer_RO_ComponentLookup = state.GetComponentLookup<CarTrailer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentLookup = state.GetComponentLookup<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SwayingData_RO_ComponentLookup = state.GetComponentLookup<SwayingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferLookup = state.GetBufferLookup<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RO_BufferLookup = state.GetBufferLookup<EnabledEffect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationClip_RO_BufferLookup = state.GetBufferLookup<AnimationClip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationMotion_RO_BufferLookup = state.GetBufferLookup<AnimationMotion>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralLight_RO_BufferLookup = state.GetBufferLookup<ProceduralLight>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LightAnimation_RO_BufferLookup = state.GetBufferLookup<LightAnimation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterElement_RO_BufferLookup = state.GetBufferLookup<CharacterElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Swaying_RW_ComponentLookup = state.GetComponentLookup<Swaying>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RW_BufferLookup = state.GetBufferLookup<Skeleton>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Emissive_RW_BufferLookup = state.GetBufferLookup<Emissive>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Animated_RW_BufferLookup = state.GetBufferLookup<Animated>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Bone_RW_BufferLookup = state.GetBufferLookup<Bone>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Momentum_RW_BufferLookup = state.GetBufferLookup<Momentum>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_LightState_RW_BufferLookup = state.GetBufferLookup<LightState>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTractorData_RO_ComponentLookup = state.GetComponentLookup<CarTractorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTrailerData_RO_ComponentLookup = state.GetComponentLookup<CarTrailerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainBogieFrame_RO_BufferLookup = state.GetBufferLookup<TrainBogieFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Animation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Tools.Animation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferTypeHandle = state.GetBufferTypeHandle<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RO_BufferTypeHandle = state.GetBufferTypeHandle<EnabledEffect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<IconElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
      }
    }
  }
}
