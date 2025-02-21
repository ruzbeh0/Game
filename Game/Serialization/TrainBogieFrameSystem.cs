// Decompiled with JetBrains decompiler
// Type: Game.Serialization.TrainBogieFrameSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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
  public class TrainBogieFrameSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private TrainBogieFrameSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadWrite<TrainBogieFrame>(), ComponentType.ReadOnly<TrainCurrentLane>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrainBogieFrameSystem.BogieFrameJob jobData = new TrainBogieFrameSystem.BogieFrameJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_TrainCurrentLaneType = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle,
        m_TrainBogieFrameType = this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<TrainBogieFrameSystem.BogieFrameJob>(this.m_Query, this.Dependency);
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
    public TrainBogieFrameSystem()
    {
    }

    [BurstCompile]
    private struct BogieFrameJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<TrainCurrentLane> m_TrainCurrentLaneType;
      public BufferTypeHandle<TrainBogieFrame> m_TrainBogieFrameType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrainCurrentLane> nativeArray = chunk.GetNativeArray<TrainCurrentLane>(ref this.m_TrainCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TrainBogieFrame> bufferAccessor = chunk.GetBufferAccessor<TrainBogieFrame>(ref this.m_TrainBogieFrameType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          TrainCurrentLane trainCurrentLane = nativeArray[index1];
          DynamicBuffer<TrainBogieFrame> dynamicBuffer = bufferAccessor[index1];
          dynamicBuffer.ResizeUninitialized(4);
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            dynamicBuffer[index2] = new TrainBogieFrame()
            {
              m_FrontLane = trainCurrentLane.m_Front.m_Lane,
              m_RearLane = trainCurrentLane.m_Rear.m_Lane
            };
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
      public ComponentTypeHandle<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle;
      public BufferTypeHandle<TrainBogieFrame> __Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TrainBogieFrame>();
      }
    }
  }
}
