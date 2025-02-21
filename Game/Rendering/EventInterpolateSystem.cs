// Decompiled with JetBrains decompiler
// Type: Game.Rendering.EventInterpolateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Events;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class EventInterpolateSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private EntityQuery m_EventQuery;
    private EventInterpolateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<WeatherPhenomenon>(), ComponentType.ReadOnly<HotspotFrame>(), ComponentType.ReadWrite<InterpolatedTransform>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_HotspotFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EventInterpolateSystem.UpdateTransformDataJob jobData = new EventInterpolateSystem.UpdateTransformDataJob()
      {
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_HotspotFrameType = this.__TypeHandle.__Game_Events_HotspotFrame_RO_BufferTypeHandle,
        m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<EventInterpolateSystem.UpdateTransformDataJob>(this.m_EventQuery, this.Dependency);
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
    public EventInterpolateSystem()
    {
    }

    [BurstCompile]
    private struct UpdateTransformDataJob : IJobChunk
    {
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [ReadOnly]
      public BufferTypeHandle<HotspotFrame> m_HotspotFrameType;
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        int num1 = (int) this.m_FrameIndex - 0 - 32;
        uint index1 = (uint) num1 / 16U % 4U;
        uint index2 = (index1 + 1U) % 4U;
        // ISSUE: reference to a compiler-generated field
        float t = (float) (((double) ((uint) num1 % 16U) + (double) this.m_FrameTime) / 16.0);
        // ISSUE: reference to a compiler-generated field
        NativeArray<InterpolatedTransform> nativeArray = chunk.GetNativeArray<InterpolatedTransform>(ref this.m_InterpolatedTransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HotspotFrame> bufferAccessor = chunk.GetBufferAccessor<HotspotFrame>(ref this.m_HotspotFrameType);
        for (int index3 = 0; index3 < chunk.Count; ++index3)
        {
          InterpolatedTransform interpolatedTransform = nativeArray[index3];
          DynamicBuffer<HotspotFrame> dynamicBuffer = bufferAccessor[index3];
          HotspotFrame hotspotFrame1 = dynamicBuffer[(int) index1];
          HotspotFrame hotspotFrame2 = dynamicBuffer[(int) index2];
          float num2 = 0.08888889f;
          Bezier4x3 curve = new Bezier4x3(hotspotFrame1.m_Position, hotspotFrame1.m_Position + hotspotFrame1.m_Velocity * num2, hotspotFrame2.m_Position - hotspotFrame2.m_Velocity * num2, hotspotFrame2.m_Position);
          interpolatedTransform.m_Position = MathUtils.Position(curve, t);
          interpolatedTransform.m_Rotation = quaternion.identity;
          nativeArray[index3] = interpolatedTransform;
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
      public BufferTypeHandle<HotspotFrame> __Game_Events_HotspotFrame_RO_BufferTypeHandle;
      public ComponentTypeHandle<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_HotspotFrame_RO_BufferTypeHandle = state.GetBufferTypeHandle<HotspotFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<InterpolatedTransform>();
      }
    }
  }
}
