// Decompiled with JetBrains decompiler
// Type: Game.Rendering.RelativeObjectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Creatures;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
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
  public class RelativeObjectSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private PreCullingSystem m_PreCullingSystem;
    private AnimatedSystem m_AnimatedSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private BatchDataSystem m_BatchDataSystem;
    private EntityQuery m_RelativeQuery;
    private EntityQuery m_InterpolateQuery;
    private uint m_PrevFrameIndex;
    private RelativeObjectSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedSystem = this.World.GetOrCreateSystemManaged<AnimatedSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchDataSystem = this.World.GetOrCreateSystemManaged<BatchDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InterpolateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Relative>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadWrite<InterpolatedTransform>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<PreCullingData> cullingData = this.m_PreCullingSystem.GetCullingData(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      AnimatedSystem.AnimationData animationData = this.m_AnimatedSystem.GetAnimationData(out dependencies2);
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
      this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_BoneHistory_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      RelativeObjectSystem.UpdateRelativeTransformDataJob jobData1 = new RelativeObjectSystem.UpdateRelativeTransformDataJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_HumanData = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabVehicleData = this.__TypeHandle.__Game_Prefabs_VehicleData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup,
        m_BoneHistories = this.__TypeHandle.__Game_Rendering_BoneHistory_RO_BufferLookup,
        m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_AnimationMotions = this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup,
        m_ActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup,
        m_Animateds = this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup,
        m_PrevFrameIndex = this.m_PrevFrameIndex,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_FrameDelta = this.m_RenderingSystem.frameDelta,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_RandomSeed = RandomSeed.Next(),
        m_CullingData = cullingData,
        m_AnimationData = animationData
      };
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
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RelativeObjectSystem.UpdateQueryTransformDataJob jobData2 = new RelativeObjectSystem.UpdateQueryTransformDataJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle,
        m_StaticType = this.__TypeHandle.__Game_Objects_Static_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_AnimationType = this.__TypeHandle.__Game_Tools_Animation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MeshGroupType = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup,
        m_Animateds = this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_CullingData = cullingData,
        m_AnimationData = animationData
      };
      JobHandle jobHandle1 = jobData1.Schedule<RelativeObjectSystem.UpdateRelativeTransformDataJob, PreCullingData>(cullingData, 16, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated field
      EntityQuery interpolateQuery = this.m_InterpolateQuery;
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle2 = jobData2.ScheduleParallel<RelativeObjectSystem.UpdateQueryTransformDataJob>(interpolateQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AnimatedSystem.AddAnimationWriter(jobHandle2);
      this.Dependency = jobHandle2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrevFrameIndex = this.m_RenderingSystem.frameIndex;
    }

    public static void UpdateDrivingAnimationBody(
      Entity entity,
      in CharacterElement characterElement,
      DynamicBuffer<AnimationClip> clips,
      ref ComponentLookup<Human> humanLookup,
      ref BufferLookup<AnimationMotion> motionLookup,
      InterpolatedTransform oldTransform,
      InterpolatedTransform newTransform,
      ref Animated animated,
      ref Random random,
      float3 velocity,
      float steerAngle,
      AnimatedPropID propID,
      float updateFrameToSeconds,
      float speedDeltaFactor,
      float deltaTime,
      int updateFrameChanged,
      bool instantReset)
    {
      AnimationClip clip1 = new AnimationClip();
      AnimationClip clip2 = new AnimationClip();
      AnimationClip clip3 = new AnimationClip();
      AnimationClip clip4 = new AnimationClip();
      if (!instantReset)
      {
        clip1 = clips[(int) animated.m_ClipIndexBody0];
        if (animated.m_ClipIndexBody0I != (short) -1)
          clip2 = clips[(int) animated.m_ClipIndexBody0I];
        if (animated.m_ClipIndexBody1 != (short) -1)
          clip3 = clips[(int) animated.m_ClipIndexBody1];
        if (animated.m_ClipIndexBody1I != (short) -1)
          clip4 = clips[(int) animated.m_ClipIndexBody1I];
      }
      float3 y = math.forward(newTransform.m_Rotation);
      double num1 = (double) math.dot(velocity, y);
      float num2 = math.abs(steerAngle);
      float prev = math.radians(1f);
      ActivityType activityType = num1 >= 1.0 ? ActivityType.Driving : ActivityType.Standing;
      if (clip2.m_Activity == ActivityType.Driving)
      {
        // ISSUE: reference to a compiler-generated method
        float targetRotation = RelativeObjectSystem.GetTargetRotation(in clip2, math.radians(10f), prev);
        if ((double) num2 > (double) targetRotation)
          activityType = ActivityType.Driving;
      }
      else if (clip4.m_Activity == ActivityType.Driving)
      {
        // ISSUE: reference to a compiler-generated method
        float targetRotation = RelativeObjectSystem.GetTargetRotation(in clip4, math.radians(10f), prev);
        if ((double) num2 > (double) targetRotation)
          activityType = ActivityType.Driving;
      }
      if (clip1.m_Activity == ActivityType.Driving)
      {
        if (clip3.m_Activity == ActivityType.Driving)
        {
          if (activityType == ActivityType.Standing)
            clip3.m_Activity = ActivityType.Standing;
          else
            clip1.m_Activity = ActivityType.Standing;
        }
        else if (clip3.m_Activity == ActivityType.None && activityType == ActivityType.Standing && clip1.m_Type != AnimationType.Idle)
          clip1.m_Activity = ActivityType.Standing;
      }
      bool flag = updateFrameChanged > 0 && (clip1.m_Activity != ActivityType.None && (clip1.m_Activity != activityType || clip1.m_Type == AnimationType.Start || clip1.m_PropID != propID) || clip3.m_Activity != ActivityType.None && (clip3.m_Activity != activityType || clip3.m_Type == AnimationType.Start || clip3.m_PropID != propID));
      if (flag && clip3.m_Type != AnimationType.None)
      {
        animated.m_ClipIndexBody0 = animated.m_ClipIndexBody1;
        animated.m_ClipIndexBody0I = animated.m_ClipIndexBody1I;
        animated.m_ClipIndexBody1 = (short) -1;
        animated.m_ClipIndexBody1I = (short) -1;
        animated.m_Time.x = animated.m_Time.y;
        animated.m_Time.y = 0.0f;
        animated.m_MovementSpeed.x = animated.m_MovementSpeed.y;
        animated.m_MovementSpeed.y = 0.0f;
        clip1 = clip3;
        clip2 = clip4;
        clip3 = new AnimationClip();
        clip4 = new AnimationClip();
        flag &= clip1.m_Activity != activityType;
      }
      if (clip1.m_Activity == ActivityType.None || (clip1.m_Activity == ActivityType.Driving || clip1.m_Activity == ActivityType.Standing) && clip1.m_Type != AnimationType.Start && clip1.m_PropID == propID)
      {
        int num3 = clip1.m_Type == AnimationType.None ? 1 : 0;
        ActivityType targetActivity = num3 != 0 ? activityType : clip1.m_Activity;
        // ISSUE: reference to a compiler-generated method
        RelativeObjectSystem.UpdateDrivingClips(entity, ref clip1, ref clip2, ref animated.m_ClipIndexBody0, ref animated.m_ClipIndexBody0I, ref animated.m_MovementSpeed.x, ref animated.m_Interpolation, clips, ref humanLookup, steerAngle, propID, targetActivity);
        if (num3 != 0)
          animated.m_Time.x = random.NextFloat(clip1.m_AnimationLength);
      }
      if (flag || (clip3.m_Activity == ActivityType.Driving || clip3.m_Activity == ActivityType.Standing) && clip3.m_Type != AnimationType.Start && clip3.m_PropID == propID)
      {
        int num4 = clip3.m_Type == AnimationType.None ? 1 : 0;
        ActivityType targetActivity = num4 != 0 ? activityType : clip3.m_Activity;
        // ISSUE: reference to a compiler-generated method
        RelativeObjectSystem.UpdateDrivingClips(entity, ref clip3, ref clip4, ref animated.m_ClipIndexBody1, ref animated.m_ClipIndexBody1I, ref animated.m_MovementSpeed.y, ref animated.m_Interpolation, clips, ref humanLookup, steerAngle, propID, targetActivity);
        if (num4 != 0)
          animated.m_Time.y = clip1.m_Activity != ActivityType.Driving && clip1.m_Activity != ActivityType.Standing || clip1.m_Type == AnimationType.Start || !(clip1.m_PropID == propID) ? random.NextFloat(clip3.m_AnimationLength) : animated.m_Time.x;
      }
      if (animated.m_ClipIndexBody1 != (short) -1 && (double) animated.m_MovementSpeed.y == 0.0)
        animated.m_Time.y += deltaTime;
      if ((double) animated.m_MovementSpeed.x != 0.0)
        return;
      animated.m_Time.x += deltaTime;
    }

    public static void UpdateDrivingClips(
      Entity entity,
      ref AnimationClip clip,
      ref AnimationClip clipI,
      ref short clipIndex,
      ref short clipIndexI,
      ref float movementSpeed,
      ref float interpolation,
      DynamicBuffer<AnimationClip> clips,
      ref ComponentLookup<Human> humanLookup,
      float steerAngle,
      AnimatedPropID propID,
      ActivityType targetActivity)
    {
      float num = math.abs(steerAngle);
      float prev = math.radians(1f);
      if ((double) num > (double) prev)
      {
        AnimationType type1 = (double) steerAngle > 0.0 ? AnimationType.RightMin : AnimationType.LeftMin;
        if (clipI.m_Type != type1 || clipI.m_Activity != ActivityType.Driving || clipI.m_PropID != propID)
        {
          // ISSUE: reference to a compiler-generated method
          ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
          int index;
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.FindAnimationClip(clips, type1, ActivityType.Driving, AnimationLayer.Body, propID, activityConditions, out clipI, out index);
          clipIndexI = (short) index;
        }
        // ISSUE: reference to a compiler-generated method
        float targetRotation1 = RelativeObjectSystem.GetTargetRotation(in clipI, math.radians(10f), prev);
        AnimationType type2 = AnimationType.Idle;
        ActivityType activity = targetActivity;
        if ((double) num > (double) targetRotation1)
        {
          type2 = (double) steerAngle > 0.0 ? AnimationType.RightMax : AnimationType.LeftMax;
          activity = ActivityType.Driving;
        }
        if (clip.m_Type != type2 || clip.m_Activity != targetActivity || clip.m_PropID != propID)
        {
          // ISSUE: reference to a compiler-generated method
          ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
          int index;
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.FindAnimationClip(clips, type2, activity, AnimationLayer.Body, propID, activityConditions, out clip, out index);
          clipIndex = (short) index;
          movementSpeed = 0.0f;
        }
        if ((double) num > (double) targetRotation1)
        {
          // ISSUE: reference to a compiler-generated method
          float targetRotation2 = RelativeObjectSystem.GetTargetRotation(in clip, math.radians(45f), targetRotation1);
          interpolation = math.saturate((float) (1.0 - ((double) num - (double) targetRotation1) / ((double) targetRotation2 - (double) targetRotation1)));
        }
        else
          interpolation = math.saturate((float) (((double) num - (double) prev) / ((double) targetRotation1 - (double) prev)));
      }
      else
      {
        if (clip.m_Type != AnimationType.Idle || clip.m_Activity != targetActivity || clip.m_PropID != propID)
        {
          // ISSUE: reference to a compiler-generated method
          ActivityCondition activityConditions = ObjectInterpolateSystem.GetActivityConditions(entity, ref humanLookup);
          int index;
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.FindAnimationClip(clips, AnimationType.Idle, targetActivity, AnimationLayer.Body, propID, activityConditions, out clip, out index);
          clipIndex = (short) index;
          movementSpeed = 0.0f;
        }
        interpolation = 0.0f;
        clipIndexI = (short) -1;
        clipI = new AnimationClip();
      }
    }

    public static float GetTargetRotation(in AnimationClip clip, float def, float prev)
    {
      return math.max(math.select(clip.m_TargetValue, def, (double) clip.m_TargetValue == -3.4028234663852886E+38), prev + math.radians(1f));
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
    public RelativeObjectSystem()
    {
    }

    [BurstCompile]
    private struct UpdateRelativeTransformDataJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Human> m_HumanData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<VehicleData> m_PrefabVehicleData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Bone> m_Bones;
      [ReadOnly]
      public BufferLookup<BoneHistory> m_BoneHistories;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<CharacterElement> m_CharacterElements;
      [ReadOnly]
      public BufferLookup<AnimationClip> m_AnimationClips;
      [ReadOnly]
      public BufferLookup<AnimationMotion> m_AnimationMotions;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocations;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Animated> m_Animateds;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public uint m_PrevFrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [ReadOnly]
      public float m_FrameDelta;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      public AnimatedSystem.AnimationData m_AnimationData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData cullingData = this.m_CullingData[index];
        if ((cullingData.m_Flags & (PreCullingFlags.NearCamera | PreCullingFlags.Temp | PreCullingFlags.InterpolatedTransform | PreCullingFlags.Relative)) != (PreCullingFlags.NearCamera | PreCullingFlags.InterpolatedTransform | PreCullingFlags.Relative))
          return;
        CurrentVehicle componentData1;
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentVehicleData.TryGetComponent(cullingData.m_Entity, out componentData1))
        {
          entity = componentData1.m_Vehicle;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Owner owner = this.m_OwnerData[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_RelativeData.HasComponent(owner.m_Owner))
            return;
          entity = owner.m_Owner;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Transform relativeTransform = this.GetRelativeTransform(this.m_RelativeData[cullingData.m_Entity], entity);
        InterpolatedTransform componentData2;
        Transform componentData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Transform transform = !this.m_InterpolatedTransformData.TryGetComponent(entity, out componentData2) ? (!this.m_TransformData.TryGetComponent(entity, out componentData3) ? relativeTransform : ObjectUtils.LocalToWorld(componentData3, relativeTransform)) : ObjectUtils.LocalToWorld(componentData2.ToTransform(), relativeTransform);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(index);
        if ((cullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
        {
          float framePosition;
          int updateFrameChanged;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_PrevFrameIndex, this.m_FrameTime, (uint) cullingData.m_UpdateFrame, out uint _, out uint _, out framePosition, out updateFrameChanged);
          float updateFrameToSeconds = 0.266666681f;
          // ISSUE: reference to a compiler-generated field
          float deltaTime = this.m_FrameDelta / 60f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float speedDeltaFactor = math.select(60f / this.m_FrameDelta, 0.0f, (double) this.m_FrameDelta == 0.0);
          // ISSUE: reference to a compiler-generated method
          this.UpdateInterpolatedAnimations(cullingData, transform, entity, ref random, framePosition, updateFrameToSeconds, deltaTime, speedDeltaFactor, updateFrameChanged);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_InterpolatedTransformData[cullingData.m_Entity] = new InterpolatedTransform(transform);
        }
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(cullingData.m_Entity, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated method
        this.UpdateTransforms(transform, bufferData);
      }

      private float3 GetVelocity(Entity parent)
      {
        CullingInfo componentData;
        DynamicBuffer<TransformFrame> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CullingInfoData.TryGetComponent(parent, out componentData) || componentData.m_CullingIndex == 0 || !this.m_TransformFrames.TryGetBuffer(parent, out bufferData))
          return (float3) 0.0f;
        uint updateFrame1;
        uint updateFrame2;
        float framePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_PrevFrameIndex, this.m_FrameTime, (uint) this.m_CullingData[componentData.m_CullingIndex].m_UpdateFrame, out updateFrame1, out updateFrame2, out framePosition, out int _);
        return math.lerp(bufferData[(int) updateFrame1].m_Velocity, bufferData[(int) updateFrame2].m_Velocity, framePosition);
      }

      private void UpdateInterpolatedAnimations(
        PreCullingData cullingData,
        Transform transform,
        Entity parent,
        ref Random random,
        float framePosition,
        float updateFrameToSeconds,
        float deltaTime,
        float speedDeltaFactor,
        int updateFrameChanged)
      {
        bool flag = (cullingData.m_Flags & PreCullingFlags.NearCameraUpdated) > (PreCullingFlags) 0;
        // ISSUE: reference to a compiler-generated field
        ref InterpolatedTransform local = ref this.m_InterpolatedTransformData.GetRefRW(cullingData.m_Entity).ValueRW;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Animated> animated1 = this.m_Animateds[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubMesh> subMesh = this.m_SubMeshes[prefabRef.m_Prefab];
        float stateTimer = 0.0f;
        TransformState state = TransformState.Idle;
        ActivityType activity = ActivityType.Driving;
        AnimatedPropID propID = new AnimatedPropID(-1);
        float steerAngle = 0.0f;
        float3 velocity = new float3();
        PrefabRef componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.TryGetComponent(parent, out componentData1))
        {
          DynamicBuffer<ActivityLocationElement> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ActivityLocations.TryGetBuffer(componentData1.m_Prefab, out bufferData1) && bufferData1.Length != 0)
            propID = bufferData1[0].m_PropID;
          VehicleData componentData2;
          DynamicBuffer<Bone> bufferData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabVehicleData.TryGetComponent(componentData1.m_Prefab, out componentData2) && componentData2.m_SteeringBoneIndex != -1 && this.m_Bones.TryGetBuffer(parent, out bufferData2) && bufferData2.Length > componentData2.m_SteeringBoneIndex)
            steerAngle = math.asin(math.mul(bufferData2[componentData2.m_SteeringBoneIndex].m_Rotation, math.up()).x);
          // ISSUE: reference to a compiler-generated method
          velocity = this.GetVelocity(parent);
        }
        DynamicBuffer<MeshGroup> bufferData3 = new DynamicBuffer<MeshGroup>();
        DynamicBuffer<CharacterElement> bufferData4 = new DynamicBuffer<CharacterElement>();
        int priority = 0;
        DynamicBuffer<SubMeshGroup> bufferData5;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData5))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_MeshGroups.TryGetBuffer(cullingData.m_Entity, out bufferData3);
          // ISSUE: reference to a compiler-generated field
          this.m_CharacterElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData4);
          local = new InterpolatedTransform(transform);
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
          local = new InterpolatedTransform(transform);
        InterpolatedTransform oldTransform = local;
        for (int index1 = 0; index1 < animated1.Length; ++index1)
        {
          Animated animated2 = animated1[index1];
          if (animated2.m_ClipIndexBody0 != (short) -1)
          {
            if (bufferData4.IsCreated)
            {
              MeshGroup meshGroup;
              CollectionUtils.TryGet<MeshGroup>(bufferData3, index1, out meshGroup);
              CharacterElement characterElement = bufferData4[(int) meshGroup.m_SubMeshGroup];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<AnimationClip> animationClip = this.m_AnimationClips[characterElement.m_Style];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              RelativeObjectSystem.UpdateDrivingAnimationBody(cullingData.m_Entity, in characterElement, animationClip, ref this.m_HumanData, ref this.m_AnimationMotions, oldTransform, local, ref animated2, ref random, velocity, steerAngle, propID, updateFrameToSeconds, speedDeltaFactor, deltaTime, updateFrameChanged, flag);
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
              if (bufferData5.IsCreated)
              {
                MeshGroup meshGroup;
                CollectionUtils.TryGet<MeshGroup>(bufferData3, index1, out meshGroup);
                index2 = bufferData5[(int) meshGroup.m_SubMeshGroup].m_SubMeshRange.x;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ObjectInterpolateSystem.UpdateInterpolatedAnimation(this.m_AnimationClips[subMesh[index2].m_SubMesh], oldTransform, local, ref animated2, stateTimer, state, activity, updateFrameToSeconds, speedDeltaFactor);
            }
          }
          animated1[index1] = animated2;
        }
      }

      private void UpdateTransforms(Transform ownerTransform, DynamicBuffer<Game.Objects.SubObject> subObjects)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject1 = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (this.m_InterpolatedTransformData.HasComponent(subObject1))
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_RelativeData[subObject1].ToTransform();
            Transform world = ObjectUtils.LocalToWorld(ownerTransform, transform);
            // ISSUE: reference to a compiler-generated field
            this.m_InterpolatedTransformData[subObject1] = new InterpolatedTransform(world);
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.HasBuffer(subObject1))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Objects.SubObject> subObject2 = this.m_SubObjects[subObject1];
              // ISSUE: reference to a compiler-generated method
              this.UpdateTransforms(world, subObject2);
            }
          }
        }
      }

      private Transform GetRelativeTransform(Relative relative, Entity parent)
      {
        if (relative.m_BoneIndex.y >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BoneHistory> boneHistory = this.m_BoneHistories[parent];
          if (boneHistory.Length > relative.m_BoneIndex.y)
          {
            float4x4 matrix = boneHistory[relative.m_BoneIndex.y].m_Matrix;
            float3 float3 = math.transform(matrix, relative.m_Position);
            quaternion quaternion = quaternion.LookRotation(math.rotate(matrix, math.forward(relative.m_Rotation)), math.rotate(matrix, math.mul(relative.m_Rotation, math.up())));
            if (relative.m_BoneIndex.z >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              SubMesh subMesh = this.m_SubMeshes[this.m_PrefabRefData[parent].m_Prefab][relative.m_BoneIndex.z];
              float3 = subMesh.m_Position + math.rotate(subMesh.m_Rotation, float3);
              quaternion = math.mul(subMesh.m_Rotation, quaternion);
            }
            return new Transform(float3, quaternion);
          }
        }
        return new Transform(relative.m_Position, relative.m_Rotation);
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
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Animation> m_AnimationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> m_MeshGroupType;
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
        NativeArray<Animation> nativeArray3 = chunk.GetNativeArray<Animation>(ref this.m_AnimationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CullingInfo> nativeArray5 = chunk.GetNativeArray<CullingInfo>(ref this.m_CullingInfoType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MeshGroup> bufferAccessor = chunk.GetBufferAccessor<MeshGroup>(ref this.m_MeshGroupType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Static>(ref this.m_StaticType);
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
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Temp temp = nativeArray2[index1];
          CullingInfo cullingInfo = nativeArray5[index1];
          if (cullingInfo.m_CullingIndex != 0)
          {
            // ISSUE: reference to a compiler-generated field
            PreCullingData preCullingData = this.m_CullingData[cullingInfo.m_CullingIndex];
            if ((preCullingData.m_Flags & PreCullingFlags.NearCamera) != (PreCullingFlags) 0)
            {
              Animated animated1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_InterpolatedTransformData.HasComponent(entity))
              {
                if (flag && (temp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0 || (temp.m_Flags & TempFlags.Dragging) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_InterpolatedTransformData[entity] = nativeArray3.Length == 0 ? new InterpolatedTransform(this.m_TransformData[entity]) : new InterpolatedTransform(nativeArray3[index1].ToTransform());
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Animateds.HasBuffer(entity))
                  {
                    PrefabRef prefabRef = nativeArray4[index1];
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<Animated> animated2 = this.m_Animateds[entity];
                    DynamicBuffer<MeshGroup> buffer;
                    CollectionUtils.TryGet<MeshGroup>(bufferAccessor, index1, out buffer);
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
                PrefabRef prefabRef = nativeArray4[index1];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Animated> animated5 = this.m_Animateds[entity];
                bool reset = (preCullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated)) > (PreCullingFlags) 0;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                double minDistance = (double) RenderingUtils.CalculateMinDistance(cullingInfo.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
                // ISSUE: reference to a compiler-generated field
                int priority = RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) - (int) cullingInfo.m_MinLod;
                DynamicBuffer<MeshGroup> buffer;
                CollectionUtils.TryGet<MeshGroup>(bufferAccessor, index1, out buffer);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Human> __Game_Creatures_Human_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleData> __Game_Prefabs_VehicleData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Bone> __Game_Rendering_Bone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<BoneHistory> __Game_Rendering_BoneHistory_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CharacterElement> __Game_Prefabs_CharacterElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AnimationClip> __Game_Prefabs_AnimationClip_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AnimationMotion> __Game_Prefabs_AnimationMotion_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RW_ComponentLookup;
      public BufferLookup<Animated> __Game_Rendering_Animated_RW_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Static> __Game_Objects_Static_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Animation> __Game_Tools_Animation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferTypeHandle;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentLookup = state.GetComponentLookup<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleData_RO_ComponentLookup = state.GetComponentLookup<VehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Bone_RO_BufferLookup = state.GetBufferLookup<Bone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_BoneHistory_RO_BufferLookup = state.GetBufferLookup<BoneHistory>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferLookup = state.GetBufferLookup<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterElement_RO_BufferLookup = state.GetBufferLookup<CharacterElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationClip_RO_BufferLookup = state.GetBufferLookup<AnimationClip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationMotion_RO_BufferLookup = state.GetBufferLookup<AnimationMotion>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RW_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Animated_RW_BufferLookup = state.GetBufferLookup<Animated>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Animation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Animation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferTypeHandle = state.GetBufferTypeHandle<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
      }
    }
  }
}
