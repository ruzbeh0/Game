// Decompiled with JetBrains decompiler
// Type: Game.Serialization.HotspotFrameSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Events;
using Game.Rendering;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class HotspotFrameSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private HotspotFrameSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadWrite<HotspotFrame>(), ComponentType.ReadWrite<InterpolatedTransform>(), ComponentType.ReadOnly<WeatherPhenomenon>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_HotspotFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WeatherPhenomenon_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HotspotFrameSystem.HotspotFrameJob jobData = new HotspotFrameSystem.HotspotFrameJob()
      {
        m_WeatherPhenomenonType = this.__TypeHandle.__Game_Events_WeatherPhenomenon_RO_ComponentTypeHandle,
        m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle,
        m_HotspotFrameType = this.__TypeHandle.__Game_Events_HotspotFrame_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<HotspotFrameSystem.HotspotFrameJob>(this.m_Query, this.Dependency);
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
    public HotspotFrameSystem()
    {
    }

    [BurstCompile]
    private struct HotspotFrameJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<WeatherPhenomenon> m_WeatherPhenomenonType;
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;
      public BufferTypeHandle<HotspotFrame> m_HotspotFrameType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<WeatherPhenomenon> nativeArray1 = chunk.GetNativeArray<WeatherPhenomenon>(ref this.m_WeatherPhenomenonType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<InterpolatedTransform> nativeArray2 = chunk.GetNativeArray<InterpolatedTransform>(ref this.m_InterpolatedTransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HotspotFrame> bufferAccessor = chunk.GetBufferAccessor<HotspotFrame>(ref this.m_HotspotFrameType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          WeatherPhenomenon weatherPhenomenon = nativeArray1[index1];
          nativeArray2[index1] = new InterpolatedTransform(weatherPhenomenon);
          DynamicBuffer<HotspotFrame> dynamicBuffer = bufferAccessor[index1];
          dynamicBuffer.ResizeUninitialized(4);
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            dynamicBuffer[index2] = new HotspotFrame(weatherPhenomenon);
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
      public ComponentTypeHandle<WeatherPhenomenon> __Game_Events_WeatherPhenomenon_RO_ComponentTypeHandle;
      public ComponentTypeHandle<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle;
      public BufferTypeHandle<HotspotFrame> __Game_Events_HotspotFrame_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WeatherPhenomenon_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WeatherPhenomenon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<InterpolatedTransform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_HotspotFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<HotspotFrame>();
      }
    }
  }
}
