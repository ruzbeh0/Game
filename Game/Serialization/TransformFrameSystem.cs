// Decompiled with JetBrains decompiler
// Type: Game.Serialization.TransformFrameSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Creatures;
using Game.Objects;
using Game.Rendering;
using Game.Simulation;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class TransformFrameSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_Query;
    private TransformFrameSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadWrite<TransformFrame>(), ComponentType.ReadWrite<InterpolatedTransform>(), ComponentType.ReadOnly<Transform>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalNavigation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanNavigation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      TransformFrameSystem.TransformFrameJob jobData = new TransformFrameSystem.TransformFrameJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
        m_HumanNavigationType = this.__TypeHandle.__Game_Creatures_HumanNavigation_RO_ComponentTypeHandle,
        m_AnimalNavigationType = this.__TypeHandle.__Game_Creatures_AnimalNavigation_RO_ComponentTypeHandle,
        m_TrainType = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentTypeHandle,
        m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<TransformFrameSystem.TransformFrameJob>(this.m_Query, this.Dependency);
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
    public TransformFrameSystem()
    {
    }

    [BurstCompile]
    private struct TransformFrameJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Moving> m_MovingType;
      [ReadOnly]
      public ComponentTypeHandle<HumanNavigation> m_HumanNavigationType;
      [ReadOnly]
      public ComponentTypeHandle<AnimalNavigation> m_AnimalNavigationType;
      [ReadOnly]
      public ComponentTypeHandle<Train> m_TrainType;
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      private const float SECONDS_PER_FRAME = 0.266666681f;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray1 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray2 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanNavigation> nativeArray3 = chunk.GetNativeArray<HumanNavigation>(ref this.m_HumanNavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalNavigation> nativeArray4 = chunk.GetNativeArray<AnimalNavigation>(ref this.m_AnimalNavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<InterpolatedTransform> nativeArray5 = chunk.GetNativeArray<InterpolatedTransform>(ref this.m_InterpolatedTransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Train> nativeArray6 = chunk.GetNativeArray<Train>(ref this.m_TrainType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        uint updateFrameOffset = (this.m_SimulationFrameIndex - chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index) / 16U;
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Transform transform = nativeArray1[index1];
          nativeArray5[index1] = new InterpolatedTransform(transform);
          DynamicBuffer<TransformFrame> transformFrames = bufferAccessor[index1];
          bool fullReset = transformFrames.Length != 4;
          if (fullReset)
            transformFrames.ResizeUninitialized(4);
          TransformFlags flags = (TransformFlags) 0;
          if (nativeArray6.Length != 0 && (nativeArray6[index1].m_Flags & TrainFlags.Pantograph) != (TrainFlags) 0)
            flags |= TransformFlags.Pantograph;
          if (nativeArray2.Length != 0)
          {
            Moving moving = nativeArray2[index1];
            if (nativeArray3.Length != 0)
            {
              HumanNavigation humanNavigation = nativeArray3[index1];
              // ISSUE: reference to a compiler-generated method
              this.InitTransformFrames(transform, moving, humanNavigation.m_TransformState, flags, humanNavigation.m_LastActivity, transformFrames, updateFrameOffset, fullReset);
            }
            else if (nativeArray4.Length != 0)
            {
              AnimalNavigation animalNavigation = nativeArray4[index1];
              // ISSUE: reference to a compiler-generated method
              this.InitTransformFrames(transform, moving, animalNavigation.m_TransformState, flags, animalNavigation.m_LastActivity, transformFrames, updateFrameOffset, fullReset);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.InitTransformFrames(transform, moving, TransformState.Default, flags, (byte) 0, transformFrames, updateFrameOffset, fullReset);
            }
          }
          else
          {
            for (int index2 = 0; index2 < transformFrames.Length; ++index2)
              transformFrames[index2] = new TransformFrame(transform)
              {
                m_Flags = flags
              };
          }
        }
      }

      private void InitTransformFrames(
        Transform transform,
        Moving moving,
        TransformState state,
        TransformFlags flags,
        byte activity,
        DynamicBuffer<TransformFrame> transformFrames,
        uint updateFrameOffset,
        bool fullReset)
      {
        for (int index = 0; index < transformFrames.Length; ++index)
        {
          TransformFrame transformFrame;
          if (fullReset)
          {
            transformFrame = new TransformFrame(transform, moving);
            transformFrame.m_State = state;
            transformFrame.m_Flags = flags;
            transformFrame.m_Activity = activity;
          }
          else
            transformFrame = transformFrames[index] with
            {
              m_Position = transform.m_Position,
              m_Velocity = moving.m_Velocity,
              m_Rotation = transform.m_Rotation
            };
          float num = (float) ((double) ((updateFrameOffset - (uint) index) % 4U) * 0.26666668057441711 + 0.13333334028720856);
          transformFrame.m_Position -= moving.m_Velocity * num;
          transformFrames[index] = transformFrame;
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HumanNavigation> __Game_Creatures_HumanNavigation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AnimalNavigation> __Game_Creatures_AnimalNavigation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Train> __Game_Vehicles_Train_RO_ComponentTypeHandle;
      public ComponentTypeHandle<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle;
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanNavigation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalNavigation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<InterpolatedTransform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>();
      }
    }
  }
}
