// Decompiled with JetBrains decompiler
// Type: Game.Tools.AnimationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class AnimationSystem : GameSystemBase
  {
    private EntityQuery m_AnimatedQuery;
    private AnimationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedQuery = this.GetEntityQuery(ComponentType.ReadWrite<Animation>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AnimatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Animation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new AnimationSystem.AnimateJob()
      {
        m_DeltaTime = UnityEngine.Time.deltaTime,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_AnimationType = this.__TypeHandle.__Game_Tools_Animation_RW_ComponentTypeHandle
      }.ScheduleParallel<AnimationSystem.AnimateJob>(this.m_AnimatedQuery, this.Dependency);
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
    public AnimationSystem()
    {
    }

    [BurstCompile]
    private struct AnimateJob : IJobChunk
    {
      [ReadOnly]
      public float m_DeltaTime;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      public ComponentTypeHandle<Animation> m_AnimationType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray1 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Animation> nativeArray2 = chunk.GetNativeArray<Animation>(ref this.m_AnimationType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Game.Objects.Transform transform = nativeArray1[index];
          Animation animation = nativeArray2[index];
          animation.m_SwayVelocity.zx += (animation.m_TargetPosition.xz - transform.m_Position.xz) * animation.m_PushFactor;
          animation.m_TargetPosition = transform.m_Position;
          animation.m_PushFactor = 0.0f;
          // ISSUE: reference to a compiler-generated field
          animation.m_SwayVelocity *= math.pow(0.0001f, this.m_DeltaTime);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          animation.m_SwayVelocity -= animation.m_SwayPosition * (float) ((double) this.m_DeltaTime * 100.0 / (1.0 + 100.0 * (double) this.m_DeltaTime * (double) this.m_DeltaTime));
          // ISSUE: reference to a compiler-generated field
          animation.m_SwayPosition += animation.m_SwayVelocity * this.m_DeltaTime;
          animation.m_SwayPosition = math.atan(animation.m_SwayPosition * 0.02f) * 50f;
          float3 xyz = animation.m_SwayPosition * 0.005f;
          xyz.z = -xyz.z;
          quaternion a = quaternion.EulerZXY(xyz);
          float3 swayPivot = animation.m_SwayPivot;
          animation.m_Rotation = math.mul(a, transform.m_Rotation);
          float3 x = transform.m_Position + math.mul(transform.m_Rotation, swayPivot) - math.mul(animation.m_Rotation, swayPivot);
          // ISSUE: reference to a compiler-generated field
          animation.m_Position = math.lerp(x, animation.m_Position, math.pow(1E-14f, this.m_DeltaTime));
          nativeArray2[index] = animation;
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
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Animation> __Game_Tools_Animation_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Animation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Animation>();
      }
    }
  }
}
