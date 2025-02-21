// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TrafficInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Common;
using Game.Net;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TrafficInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "trafficInfo";
    private EntityQuery m_AggregateQuery;
    private RawValueBinding m_TrafficFlow;
    private NativeArray<float> m_Results;
    private float[] m_Flow;
    private TrafficInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AggregateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Aggregated>(), ComponentType.ReadOnly<Edge>(), ComponentType.ReadOnly<Road>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Native>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TrafficFlow = new RawValueBinding("trafficInfo", "trafficFlow", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = math.select((int) this.m_Results[4], 1, (int) this.m_Results[4] == 0);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Flow[0] = this.m_Results[0] / (float) num;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Flow[1] = this.m_Results[1] / (float) num;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Flow[2] = this.m_Results[2] / (float) num;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Flow[3] = this.m_Results[3] / (float) num;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Flow[4] = this.m_Flow[0];
        // ISSUE: reference to a compiler-generated field
        writer.ArrayBegin(this.m_Flow.Length);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Flow.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_Flow[index]);
        }
        writer.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.m_Flow = new float[5];
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<float>(5, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override bool Active => base.Active || this.m_TrafficFlow.active;

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.Reset();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new TrafficInfoviewUISystem.UpdateFlowJob()
      {
        m_RoadHandle = this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle,
        m_Results = this.m_Results
      }.Schedule<TrafficInfoviewUISystem.UpdateFlowJob>(this.m_AggregateQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficFlow.Update();
    }

    private void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0.0f;
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Flow.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Flow[index] = 0.0f;
      }
    }

    private void UpdateTrafficFlowBinding(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num = math.select((int) this.m_Results[4], 1, (int) this.m_Results[4] == 0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[0] = this.m_Results[0] / (float) num;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[1] = this.m_Results[1] / (float) num;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[2] = this.m_Results[2] / (float) num;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[3] = this.m_Results[3] / (float) num;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[4] = this.m_Flow[0];
      // ISSUE: reference to a compiler-generated field
      writer.ArrayBegin(this.m_Flow.Length);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Flow.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_Flow[index]);
      }
      writer.ArrayEnd();
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
    public TrafficInfoviewUISystem()
    {
    }

    [BurstCompile]
    private struct UpdateFlowJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Road> m_RoadHandle;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Road> nativeArray = chunk.GetNativeArray<Road>(ref this.m_RoadHandle);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          float4 float4 = NetUtils.GetTrafficFlowSpeed(nativeArray[index]) * 100f;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[0] += float4.x;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[1] += float4.y;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[2] += float4.z;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[3] += float4.w;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[4] += (float) nativeArray.Length;
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
      public ComponentTypeHandle<Road> __Game_Net_Road_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Road>(true);
      }
    }
  }
}
