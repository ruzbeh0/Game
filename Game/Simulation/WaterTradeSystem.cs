// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterTradeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Serialization.Entities;
using Game.City;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaterTradeSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_TradeNodeGroup;
    private ServiceFeeSystem m_ServiceFeeSystem;
    private NativePerThreadSumInt m_FreshExport;
    private NativePerThreadSumInt m_PollutedExport;
    private NativePerThreadSumInt m_FreshImport;
    private NativePerThreadSumInt m_SewageExport;
    private int m_LastFreshExport;
    private int m_LastFreshImport;
    private int m_LastSewageExport;
    private WaterTradeSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1457460959_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 62;

    public int freshExport => this.m_LastFreshExport;

    public int freshImport => this.m_LastFreshImport;

    public int sewageExport => this.m_LastSewageExport;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TradeNodeGroup = this.GetEntityQuery(ComponentType.ReadOnly<TradeNode>(), ComponentType.ReadOnly<WaterPipeNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>());
      this.RequireForUpdate<OutsideTradeParameterData>();
      // ISSUE: reference to a compiler-generated field
      this.m_FreshExport = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_PollutedExport = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FreshImport = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SewageExport = new NativePerThreadSumInt(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_FreshExport.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_PollutedExport.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FreshImport.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SewageExport.Dispose();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFreshExport);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFreshImport);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastSewageExport);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastFreshExport);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastFreshImport);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastSewageExport);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshExport = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshImport = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSewageExport = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshExport = this.m_FreshExport.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshImport = this.m_FreshImport.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastSewageExport = this.m_SewageExport.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_FreshExport.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_PollutedExport.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_FreshImport.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageExport.Count = 0;
      // ISSUE: reference to a compiler-generated field
      if (this.m_TradeNodeGroup.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      OutsideTradeParameterData singleton = this.__query_1457460959_0.GetSingleton<OutsideTradeParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
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
      WaterTradeSystem.SumJob jobData1 = new WaterTradeSystem.SumJob()
      {
        m_FlowConnectionType = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup,
        m_FreshExport = this.m_FreshExport.ToConcurrent(),
        m_PollutedExport = this.m_PollutedExport.ToConcurrent(),
        m_FreshImport = this.m_FreshImport.ToConcurrent(),
        m_SewageExport = this.m_SewageExport.ToConcurrent(),
        m_OutsideTradeParameters = singleton,
        m_SourceNode = this.m_WaterPipeFlowSystem.sourceNode,
        m_SinkNode = this.m_WaterPipeFlowSystem.sinkNode
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<WaterTradeSystem.SumJob>(this.m_TradeNodeGroup, this.Dependency);
      JobHandle deps1;
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaterTradeSystem.WaterTradeJob jobData2 = new WaterTradeSystem.WaterTradeJob()
      {
        m_FreshExport = this.m_FreshExport,
        m_PollutedExport = this.m_PollutedExport,
        m_FreshImport = this.m_FreshImport,
        m_SewageExport = this.m_SewageExport,
        m_StatQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps1),
        m_FeeQueue = this.m_ServiceFeeSystem.GetFeeQueue(out deps2),
        m_OutsideTradeParameters = singleton
      };
      this.Dependency = jobData2.Schedule<WaterTradeSystem.WaterTradeJob>(JobHandle.CombineDependencies(this.Dependency, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ServiceFeeSystem.AddQueueWriter(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1457460959_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<OutsideTradeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public WaterTradeSystem()
    {
    }

    [BurstCompile]
    private struct SumJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> m_FlowConnectionType;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      public NativePerThreadSumInt.Concurrent m_FreshExport;
      public NativePerThreadSumInt.Concurrent m_PollutedExport;
      public NativePerThreadSumInt.Concurrent m_FreshImport;
      public NativePerThreadSumInt.Concurrent m_SewageExport;
      public OutsideTradeParameterData m_OutsideTradeParameters;
      public Entity m_SourceNode;
      public Entity m_SinkNode;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedFlowEdge> bufferAccessor = chunk.GetBufferAccessor<ConnectedFlowEdge>(ref this.m_FlowConnectionType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          DynamicBuffer<ConnectedFlowEdge> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            WaterPipeEdge flowEdge = this.m_FlowEdges[dynamicBuffer[index2].m_Edge];
            // ISSUE: reference to a compiler-generated field
            if (flowEdge.m_End == this.m_SinkNode)
            {
              Assert.IsTrue(flowEdge.m_FreshFlow >= 0);
              Assert.AreEqual(0, flowEdge.m_SewageFlow);
              // ISSUE: reference to a compiler-generated field
              this.m_FreshExport.Add(flowEdge.m_FreshFlow);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_PollutedExport.Add(math.min((int) math.round(flowEdge.m_FreshPollution / this.m_OutsideTradeParameters.m_WaterExportPollutionTolerance * (float) flowEdge.m_FreshFlow), flowEdge.m_FreshFlow));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (flowEdge.m_Start == this.m_SourceNode)
              {
                Assert.IsTrue(flowEdge.m_FreshFlow >= 0);
                Assert.IsTrue(flowEdge.m_SewageFlow >= 0);
                // ISSUE: reference to a compiler-generated field
                this.m_FreshImport.Add(flowEdge.m_FreshFlow);
                // ISSUE: reference to a compiler-generated field
                this.m_SewageExport.Add(flowEdge.m_SewageFlow);
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

    [BurstCompile]
    private struct WaterTradeJob : IJob
    {
      public NativePerThreadSumInt m_FreshExport;
      public NativePerThreadSumInt m_PollutedExport;
      public NativePerThreadSumInt m_FreshImport;
      public NativePerThreadSumInt m_SewageExport;
      public NativeQueue<StatisticsEvent> m_StatQueue;
      public NativeQueue<ServiceFeeSystem.FeeEvent> m_FeeQueue;
      public OutsideTradeParameterData m_OutsideTradeParameters;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = (float) this.m_FreshExport.Count / 2048f;
        // ISSUE: reference to a compiler-generated field
        float num2 = (float) this.m_PollutedExport.Count / 2048f;
        // ISSUE: reference to a compiler-generated field
        float num3 = (float) this.m_FreshImport.Count / 2048f;
        // ISSUE: reference to a compiler-generated field
        float num4 = (float) this.m_SewageExport.Count / 2048f;
        // ISSUE: reference to a compiler-generated field
        float num5 = (num1 - num2) * this.m_OutsideTradeParameters.m_WaterExportPrice;
        // ISSUE: reference to a compiler-generated field
        float num6 = num3 * this.m_OutsideTradeParameters.m_WaterImportPrice;
        // ISSUE: reference to a compiler-generated field
        float num7 = num4 * this.m_OutsideTradeParameters.m_SewageExportPrice;
        // ISSUE: variable of a compiler-generated type
        ServiceFeeSystem.FeeEvent feeEvent1;
        if ((double) num5 > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_StatQueue.Enqueue(new StatisticsEvent()
          {
            m_Statistic = StatisticType.Income,
            m_Change = num5,
            m_Parameter = 8
          });
          // ISSUE: reference to a compiler-generated field
          ref NativeQueue<ServiceFeeSystem.FeeEvent> local = ref this.m_FeeQueue;
          // ISSUE: object of a compiler-generated type is created
          feeEvent1 = new ServiceFeeSystem.FeeEvent();
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Resource = PlayerResource.Water;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Cost = num5;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Amount = num1;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Outside = true;
          // ISSUE: variable of a compiler-generated type
          ServiceFeeSystem.FeeEvent feeEvent2 = feeEvent1;
          local.Enqueue(feeEvent2);
        }
        if ((double) num6 > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_StatQueue.Enqueue(new StatisticsEvent()
          {
            m_Statistic = StatisticType.Expense,
            m_Change = num6,
            m_Parameter = 3
          });
          // ISSUE: reference to a compiler-generated field
          ref NativeQueue<ServiceFeeSystem.FeeEvent> local = ref this.m_FeeQueue;
          // ISSUE: object of a compiler-generated type is created
          feeEvent1 = new ServiceFeeSystem.FeeEvent();
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Resource = PlayerResource.Water;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Cost = num6;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Amount = -num3;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Outside = true;
          // ISSUE: variable of a compiler-generated type
          ServiceFeeSystem.FeeEvent feeEvent3 = feeEvent1;
          local.Enqueue(feeEvent3);
        }
        if ((double) num7 <= 0.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_StatQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Expense,
          m_Change = num7,
          m_Parameter = 4
        });
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<ServiceFeeSystem.FeeEvent> local1 = ref this.m_FeeQueue;
        // ISSUE: object of a compiler-generated type is created
        feeEvent1 = new ServiceFeeSystem.FeeEvent();
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Resource = PlayerResource.Sewage;
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Cost = num7;
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Amount = -num4;
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Outside = true;
        // ISSUE: variable of a compiler-generated type
        ServiceFeeSystem.FeeEvent feeEvent4 = feeEvent1;
        local1.Enqueue(feeEvent4);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
      }
    }
  }
}
