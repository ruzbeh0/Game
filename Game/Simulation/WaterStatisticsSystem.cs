// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterStatisticsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaterStatisticsSystem : 
    GameSystemBase,
    IWaterStatisticsSystem,
    IDefaultSerializable,
    ISerializable
  {
    private EntityQuery m_PumpGroup;
    private EntityQuery m_OutletGroup;
    private EntityQuery m_ConsumerGroup;
    private NativePerThreadSumInt m_FreshCapacity;
    private NativePerThreadSumInt m_SewageCapacity;
    private NativePerThreadSumInt m_Consumption;
    private NativePerThreadSumInt m_FulfilledFreshConsumption;
    private NativePerThreadSumInt m_FulfilledSewageConsumption;
    private int m_LastFreshCapacity;
    private int m_LastFreshConsumption;
    private int m_LastFulfilledFreshConsumption;
    private int m_LastSewageCapacity;
    private int m_LastSewageConsumption;
    private int m_LastFulfilledSewageConsumption;
    private WaterStatisticsSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 63;

    public int freshCapacity => this.m_LastFreshCapacity;

    public int freshConsumption => this.m_LastFreshConsumption;

    public int fulfilledFreshConsumption => this.m_LastFulfilledFreshConsumption;

    public int sewageCapacity => this.m_LastSewageCapacity;

    public int sewageConsumption => this.m_LastSewageConsumption;

    public int fulfilledSewageConsumption => this.m_LastFulfilledSewageConsumption;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PumpGroup = this.GetEntityQuery(ComponentType.ReadOnly<WaterPumpingStation>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutletGroup = this.GetEntityQuery(ComponentType.ReadOnly<SewageOutlet>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConsumerGroup = this.GetEntityQuery(ComponentType.ReadOnly<WaterConsumer>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_FreshCapacity = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SewageCapacity = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledFreshConsumption = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledSewageConsumption = new NativePerThreadSumInt(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_FreshCapacity.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SewageCapacity.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledFreshConsumption.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledSewageConsumption.Dispose();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFreshCapacity);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFreshConsumption);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFulfilledFreshConsumption);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastSewageCapacity);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastSewageConsumption);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFulfilledSewageConsumption);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastFreshCapacity);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastFreshConsumption);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastFulfilledFreshConsumption);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastSewageCapacity);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastSewageConsumption);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastFulfilledSewageConsumption);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshCapacity = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshConsumption = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastFulfilledFreshConsumption = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSewageCapacity = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSewageConsumption = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastFulfilledSewageConsumption = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshCapacity = this.m_FreshCapacity.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreshConsumption = this.m_Consumption.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFulfilledFreshConsumption = this.m_FulfilledFreshConsumption.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastSewageCapacity = this.m_SewageCapacity.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastSewageConsumption = this.m_Consumption.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFulfilledSewageConsumption = this.m_FulfilledSewageConsumption.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_FreshCapacity.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageCapacity.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledFreshConsumption.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledSewageConsumption.Count = 0;
      JobHandle job0 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PumpGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        job0 = new WaterStatisticsSystem.CountPumpCapacityJob()
        {
          m_PumpType = this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle,
          m_Capacity = this.m_FreshCapacity.ToConcurrent()
        }.ScheduleParallel<WaterStatisticsSystem.CountPumpCapacityJob>(this.m_PumpGroup, this.Dependency);
      }
      JobHandle job1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PumpGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        job1 = new WaterStatisticsSystem.CountOutletCapacityJob()
        {
          m_OutletType = this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle,
          m_Capacity = this.m_SewageCapacity.ToConcurrent()
        }.ScheduleParallel<WaterStatisticsSystem.CountOutletCapacityJob>(this.m_OutletGroup, this.Dependency);
      }
      JobHandle job2 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ConsumerGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        job2 = new WaterStatisticsSystem.CountWaterConsumptionJob()
        {
          m_ConsumerType = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle,
          m_Consumption = this.m_Consumption.ToConcurrent(),
          m_FulfilledFreshConsumption = this.m_FulfilledFreshConsumption.ToConcurrent(),
          m_FulfilledSewageConsumption = this.m_FulfilledSewageConsumption.ToConcurrent()
        }.ScheduleParallel<WaterStatisticsSystem.CountWaterConsumptionJob>(this.m_ConsumerGroup, this.Dependency);
      }
      this.Dependency = JobHandle.CombineDependencies(job0, job1, job2);
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
    public WaterStatisticsSystem()
    {
    }

    [BurstCompile]
    private struct CountPumpCapacityJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<WaterPumpingStation> m_PumpType;
      public NativePerThreadSumInt.Concurrent m_Capacity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPumpingStation> nativeArray = chunk.GetNativeArray<WaterPumpingStation>(ref this.m_PumpType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Capacity.Add(nativeArray[index].m_Capacity);
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
    private struct CountOutletCapacityJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<SewageOutlet> m_OutletType;
      public NativePerThreadSumInt.Concurrent m_Capacity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<SewageOutlet> nativeArray = chunk.GetNativeArray<SewageOutlet>(ref this.m_OutletType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Capacity.Add(nativeArray[index].m_Capacity);
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
    private struct CountWaterConsumptionJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> m_ConsumerType;
      public NativePerThreadSumInt.Concurrent m_Consumption;
      public NativePerThreadSumInt.Concurrent m_FulfilledFreshConsumption;
      public NativePerThreadSumInt.Concurrent m_FulfilledSewageConsumption;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterConsumer> nativeArray = chunk.GetNativeArray<WaterConsumer>(ref this.m_ConsumerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          WaterConsumer waterConsumer = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          this.m_Consumption.Add(waterConsumer.m_WantedConsumption);
          // ISSUE: reference to a compiler-generated field
          this.m_FulfilledFreshConsumption.Add(waterConsumer.m_FulfilledFresh);
          // ISSUE: reference to a compiler-generated field
          this.m_FulfilledSewageConsumption.Add(waterConsumer.m_FulfilledSewage);
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
      public ComponentTypeHandle<WaterPumpingStation> __Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SewageOutlet> __Game_Buildings_SewageOutlet_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPumpingStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SewageOutlet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterConsumer>(true);
      }
    }
  }
}
