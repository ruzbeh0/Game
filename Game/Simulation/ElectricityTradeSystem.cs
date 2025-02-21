// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityTradeSystem
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ElectricityTradeSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_TradeNodeGroup;
    private ServiceFeeSystem m_ServiceFeeSystem;
    private NativePerThreadSumInt m_Export;
    private NativePerThreadSumInt m_Import;
    private int m_LastExport;
    private int m_LastImport;
    private ElectricityTradeSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1233563293_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 126;

    public int export => this.m_LastExport;

    public int import => this.m_LastImport;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TradeNodeGroup = this.GetEntityQuery(ComponentType.ReadOnly<TradeNode>(), ComponentType.ReadOnly<ElectricityFlowNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>());
      this.RequireForUpdate<OutsideTradeParameterData>();
      // ISSUE: reference to a compiler-generated field
      this.m_Export = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Import = new NativePerThreadSumInt(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Export.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Import.Dispose();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastExport);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastImport);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastExport);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastImport);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastExport = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastImport = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastExport = this.m_Export.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastImport = this.m_Import.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_Export.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Import.Count = 0;
      // ISSUE: reference to a compiler-generated field
      if (this.m_TradeNodeGroup.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ElectricityTradeSystem.SumJob jobData1 = new ElectricityTradeSystem.SumJob()
      {
        m_FlowConnectionType = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_Export = this.m_Export.ToConcurrent(),
        m_Import = this.m_Import.ToConcurrent(),
        m_SourceNode = this.m_ElectricityFlowSystem.sourceNode,
        m_SinkNode = this.m_ElectricityFlowSystem.sinkNode
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<ElectricityTradeSystem.SumJob>(this.m_TradeNodeGroup, this.Dependency);
      JobHandle deps1;
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ElectricityTradeSystem.ElectricityTradeJob jobData2 = new ElectricityTradeSystem.ElectricityTradeJob()
      {
        m_Export = this.m_Export,
        m_Import = this.m_Import,
        m_StatQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps1),
        m_FeeQueue = this.m_ServiceFeeSystem.GetFeeQueue(out deps2),
        m_OutsideTradeParameters = this.__query_1233563293_0.GetSingleton<OutsideTradeParameterData>()
      };
      this.Dependency = jobData2.Schedule<ElectricityTradeSystem.ElectricityTradeJob>(JobHandle.CombineDependencies(this.Dependency, deps1, deps2));
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
      this.__query_1233563293_0 = state.GetEntityQuery(new EntityQueryDesc()
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
    public ElectricityTradeSystem()
    {
    }

    [BurstCompile]
    private struct SumJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> m_FlowConnectionType;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public NativePerThreadSumInt.Concurrent m_Export;
      public NativePerThreadSumInt.Concurrent m_Import;
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
            ElectricityFlowEdge flowEdge = this.m_FlowEdges[dynamicBuffer[index2].m_Edge];
            // ISSUE: reference to a compiler-generated field
            if (flowEdge.m_End == this.m_SinkNode)
            {
              Assert.IsTrue(flowEdge.m_Flow >= 0);
              // ISSUE: reference to a compiler-generated field
              this.m_Export.Add(flowEdge.m_Flow);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (flowEdge.m_Start == this.m_SourceNode)
              {
                Assert.IsTrue(flowEdge.m_Flow >= 0);
                // ISSUE: reference to a compiler-generated field
                this.m_Import.Add(flowEdge.m_Flow);
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
    private struct ElectricityTradeJob : IJob
    {
      public NativePerThreadSumInt m_Export;
      public NativePerThreadSumInt m_Import;
      public NativeQueue<StatisticsEvent> m_StatQueue;
      public NativeQueue<ServiceFeeSystem.FeeEvent> m_FeeQueue;
      public OutsideTradeParameterData m_OutsideTradeParameters;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = (float) this.m_Export.Count / 2048f;
        // ISSUE: reference to a compiler-generated field
        float num2 = (float) this.m_Import.Count / 2048f;
        // ISSUE: reference to a compiler-generated field
        float num3 = num1 * this.m_OutsideTradeParameters.m_ElectricityExportPrice;
        // ISSUE: reference to a compiler-generated field
        float num4 = num2 * this.m_OutsideTradeParameters.m_ElectricityImportPrice;
        // ISSUE: variable of a compiler-generated type
        ServiceFeeSystem.FeeEvent feeEvent1;
        if ((double) num3 > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_StatQueue.Enqueue(new StatisticsEvent()
          {
            m_Statistic = StatisticType.Income,
            m_Change = num3,
            m_Parameter = 7
          });
          // ISSUE: reference to a compiler-generated field
          ref NativeQueue<ServiceFeeSystem.FeeEvent> local = ref this.m_FeeQueue;
          // ISSUE: object of a compiler-generated type is created
          feeEvent1 = new ServiceFeeSystem.FeeEvent();
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Resource = PlayerResource.Electricity;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Cost = num3;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Amount = num1;
          // ISSUE: reference to a compiler-generated field
          feeEvent1.m_Outside = true;
          // ISSUE: variable of a compiler-generated type
          ServiceFeeSystem.FeeEvent feeEvent2 = feeEvent1;
          local.Enqueue(feeEvent2);
        }
        if ((double) num4 <= 0.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_StatQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Expense,
          m_Change = num4,
          m_Parameter = 2
        });
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<ServiceFeeSystem.FeeEvent> local1 = ref this.m_FeeQueue;
        // ISSUE: object of a compiler-generated type is created
        feeEvent1 = new ServiceFeeSystem.FeeEvent();
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Resource = PlayerResource.Electricity;
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Cost = num4;
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Amount = -num2;
        // ISSUE: reference to a compiler-generated field
        feeEvent1.m_Outside = true;
        // ISSUE: variable of a compiler-generated type
        ServiceFeeSystem.FeeEvent feeEvent3 = feeEvent1;
        local1.Enqueue(feeEvent3);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
      }
    }
  }
}
