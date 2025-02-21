// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AnimatedPrefabSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Rendering;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class AnimatedPrefabSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private AnimatedSystem m_AnimatedSystem;
    private EntityQuery m_PrefabQuery;
    private AnimatedPrefabSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedSystem = this.World.GetOrCreateSystemManaged<AnimatedSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadWrite<CharacterStyleData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [Preserve]
    protected override unsafe void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CharacterStyleData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<CharacterStyleData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_CharacterStyleData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationClip_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<AnimationClip> bufferTypeHandle1 = this.__TypeHandle.__Game_Prefabs_AnimationClip_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationMotion_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<AnimationMotion> bufferTypeHandle2 = this.__TypeHandle.__Game_Prefabs_AnimationMotion_RW_BufferTypeHandle;
      this.CompleteDependency();
      Dictionary<(ActivityType, AnimationType, AnimatedPropID), int> dictionary = new Dictionary<(ActivityType, AnimationType, AnimatedPropID), int>();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
        NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
        NativeArray<CharacterStyleData> nativeArray3 = archetypeChunk.GetNativeArray<CharacterStyleData>(ref componentTypeHandle2);
        BufferAccessor<AnimationClip> bufferAccessor1 = archetypeChunk.GetBufferAccessor<AnimationClip>(ref bufferTypeHandle1);
        BufferAccessor<AnimationMotion> bufferAccessor2 = archetypeChunk.GetBufferAccessor<AnimationMotion>(ref bufferTypeHandle2);
        for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          CharacterStyle prefab = this.m_PrefabSystem.GetPrefab<CharacterStyle>(nativeArray2[index2]);
          ref CharacterStyleData local1 = ref nativeArray3.ElementAt<CharacterStyleData>(index2);
          DynamicBuffer<AnimationClip> dynamicBuffer1 = bufferAccessor1[index2];
          DynamicBuffer<AnimationMotion> dynamicBuffer2 = bufferAccessor2[index2];
          local1.m_ActivityMask = new ActivityMask();
          local1.m_RestPoseClipIndex = -1;
          int length1 = prefab.m_Animations.Length;
          int length2 = 0;
          dynamicBuffer1.ResizeUninitialized(length1);
          for (int index3 = 0; index3 < length1; ++index3)
          {
            CharacterStyle.AnimationInfo animation = prefab.m_Animations[index3];
            ref AnimationClip local2 = ref dynamicBuffer1.ElementAt(index3);
            *(AnimationClip*) ref local2 = new AnimationClip();
            local2.m_InfoIndex = -1;
            local2.m_RootMotionBone = animation.rootMotionBone;
            local2.m_PropClipIndex = -1;
            switch (animation.layer)
            {
              case Colossal.Animations.AnimationLayer.BodyLayer:
                local2.m_Layer = AnimationLayer.Body;
                break;
              case Colossal.Animations.AnimationLayer.PropLayer:
                local2.m_Layer = AnimationLayer.Prop;
                break;
              case Colossal.Animations.AnimationLayer.FacialLayer:
                local2.m_Layer = AnimationLayer.Facial;
                break;
              case Colossal.Animations.AnimationLayer.CorrectiveLayer:
                local2.m_Layer = AnimationLayer.Corrective;
                break;
              default:
                local2.m_Layer = AnimationLayer.None;
                break;
            }
            if (animation.rootMotion != null)
              length2 += animation.rootMotion.Length;
            if (animation.type == Colossal.Animations.AnimationType.RestPose && (Object) animation.target == (Object) null)
              local1.m_RestPoseClipIndex = index3;
            CharacterProperties component;
            if ((Object) animation.target != (Object) null && animation.target.TryGet<CharacterProperties>(out component))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              local2.m_PropID = this.m_AnimatedSystem.GetPropID(component.m_AnimatedPropName);
              if (animation.layer == Colossal.Animations.AnimationLayer.PropLayer)
                dictionary[(animation.activity, animation.state, local2.m_PropID)] = index3;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              local2.m_PropID = this.m_AnimatedSystem.GetPropID((string) null);
            }
          }
          dynamicBuffer2.ResizeUninitialized(length2);
          int num1 = 0;
          float num2 = float.MaxValue;
          float num3 = 0.0f;
          for (int index4 = 0; index4 < length1; ++index4)
          {
            CharacterStyle.AnimationInfo animation = prefab.m_Animations[index4];
            ref AnimationClip local3 = ref dynamicBuffer1.ElementAt(index4);
            local3.m_Type = animation.state;
            local3.m_Activity = animation.activity;
            local3.m_Conditions = animation.conditions;
            local3.m_Playback = animation.playback;
            local3.m_TargetValue = float.MinValue;
            if (local3.m_Playback == AnimationPlayback.RandomLoop || local3.m_Type == AnimationType.Move)
            {
              local3.m_AnimationLength = (float) animation.frameCount / (float) animation.frameRate;
              local3.m_FrameRate = (float) animation.frameRate;
            }
            else
            {
              float num4 = math.max(1f, math.round((float) (animation.frameCount - 1) * (60f / (float) animation.frameRate) / 16f)) * 16f;
              local3.m_AnimationLength = num4 * 0.0166666675f;
              local3.m_FrameRate = (float) math.max(1, animation.frameCount - 1) / local3.m_AnimationLength;
              local3.m_AnimationLength -= 1f / 1000f;
            }
            if (animation.rootMotion != null)
            {
              NativeArray<AnimationMotion> subArray = dynamicBuffer2.AsNativeArray().GetSubArray(num1, animation.rootMotion.Length);
              // ISSUE: reference to a compiler-generated method
              this.CleanUpRootMotion(animation.rootMotion, subArray);
              local3.m_MotionRange = new int2(num1, num1 + animation.rootMotion.Length);
              num1 += animation.rootMotion.Length;
              if (local3.m_Type == AnimationType.Move)
              {
                AnimationMotion animationMotion = subArray[0];
                local3.m_MovementSpeed = math.length(animationMotion.m_EndOffset - animationMotion.m_StartOffset) * local3.m_FrameRate / (float) math.max(1, animation.frameCount - 1);
                if (local3.m_Conditions == (ActivityCondition) 0)
                {
                  switch (local3.m_Activity)
                  {
                    case ActivityType.Walking:
                      num2 = local3.m_MovementSpeed;
                      break;
                    case ActivityType.Running:
                      num3 = local3.m_MovementSpeed;
                      break;
                  }
                }
              }
            }
            else
              local3.m_RootMotionBone = -1;
            int num5;
            if (animation.layer != Colossal.Animations.AnimationLayer.PropLayer && local3.m_PropID.isValid && dictionary.TryGetValue((animation.activity, animation.state, local3.m_PropID), out num5))
              local3.m_PropClipIndex = num5;
            local1.m_ActivityMask.m_Mask |= new ActivityMask(local3.m_Activity).m_Mask;
            local1.m_AnimationLayerMask.m_Mask |= new AnimationLayerMask(local3.m_Layer).m_Mask;
          }
          for (int index5 = 0; index5 < dynamicBuffer1.Length; ++index5)
          {
            ref AnimationClip local4 = ref dynamicBuffer1.ElementAt(index5);
            if (local4.m_Layer == AnimationLayer.Body && local4.m_Type == AnimationType.Move)
            {
              local4.m_SpeedRange = new Bounds1(0.0f, float.MaxValue);
              switch (local4.m_Activity)
              {
                case ActivityType.Walking:
                  local4.m_SpeedRange.max = math.select((float) (((double) num2 + (double) num3) * 0.5), float.MaxValue, (double) num3 <= (double) num2);
                  continue;
                case ActivityType.Running:
                  local4.m_SpeedRange.min = math.select((float) (((double) num2 + (double) num3) * 0.5), 0.0f, (double) num2 >= (double) num3);
                  continue;
                default:
                  continue;
              }
            }
          }
          local1.m_BoneCount = prefab.m_BoneCount;
          local1.m_ShapeCount = prefab.m_ShapeCount;
        }
      }
      archetypeChunkArray.Dispose();
    }

    private void CleanUpRootMotion(
      CharacterStyle.AnimationMotion[] source,
      NativeArray<AnimationMotion> target)
    {
      for (int index = 0; index < source.Length; ++index)
      {
        CharacterStyle.AnimationMotion animationMotion = source[index];
        ref AnimationMotion local1 = ref target.ElementAt<AnimationMotion>(index);
        local1.m_StartOffset = animationMotion.startOffset;
        local1.m_EndOffset = animationMotion.endOffset;
        local1.m_StartRotation = animationMotion.startRotation;
        local1.m_EndRotation = animationMotion.endRotation;
        if (index != 0)
        {
          ref AnimationMotion local2 = ref target.ElementAt<AnimationMotion>(0);
          local1.m_StartOffset -= local2.m_StartOffset;
          local1.m_StartRotation = math.mul(local1.m_StartRotation, math.inverse(local2.m_StartRotation));
          local1.m_EndOffset -= local2.m_EndOffset;
          local1.m_EndRotation = math.mul(local1.m_EndRotation, math.inverse(local2.m_EndRotation));
        }
        local1.m_StartOffset.y = 0.0f;
        local1.m_EndOffset.y = 0.0f;
        float3 forward1 = math.forward(local1.m_StartRotation);
        float3 forward2 = math.forward(local1.m_EndRotation);
        forward1.y = 0.0f;
        forward2.y = 0.0f;
        local1.m_StartRotation = quaternion.LookRotationSafe(forward1, math.up());
        local1.m_EndRotation = quaternion.LookRotationSafe(forward2, math.up());
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

    [Preserve]
    public AnimatedPrefabSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<CharacterStyleData> __Game_Prefabs_CharacterStyleData_RW_ComponentTypeHandle;
      public BufferTypeHandle<AnimationClip> __Game_Prefabs_AnimationClip_RW_BufferTypeHandle;
      public BufferTypeHandle<AnimationMotion> __Game_Prefabs_AnimationMotion_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterStyleData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CharacterStyleData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationClip_RW_BufferTypeHandle = state.GetBufferTypeHandle<AnimationClip>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationMotion_RW_BufferTypeHandle = state.GetBufferTypeHandle<AnimationMotion>();
      }
    }
  }
}
