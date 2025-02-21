// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityStatisticsSystem
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
  public class ElectricityStatisticsSystem : 
    GameSystemBase,
    IElectricityStatisticsSystem,
    IDefaultSerializable,
    ISerializable
  {
    private EntityQuery m_ProducerGroup;
    private EntityQuery m_ConsumerGroup;
    private EntityQuery m_BatteryGroup;
    private NativePerThreadSumInt m_Production;
    private NativePerThreadSumInt m_Consumption;
    private NativePerThreadSumInt m_FulfilledConsumption;
    private NativePerThreadSumInt m_BatteryCharge;
    private NativePerThreadSumInt m_BatteryCapacity;
    private int m_LastProduction;
    private int m_LastConsumption;
    private int m_LastFulfilledConsumption;
    private int m_LastBatteryCharge;
    private int m_LastBatteryCapacity;
    private ElectricityStatisticsSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => (int) sbyte.MaxValue;

    public int production => this.m_LastProduction;

    public int consumption => this.m_LastConsumption;

    public int fulfilledConsumption => this.m_LastFulfilledConsumption;

    public int batteryCharge => this.m_LastBatteryCharge;

    public int batteryCapacity => this.m_LastBatteryCapacity;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ProducerGroup = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityProducer>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConsumerGroup = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityConsumer>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryGroup = this.GetEntityQuery(ComponentType.ReadOnly<Battery>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_Production = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledConsumption = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCharge = new NativePerThreadSumInt(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCapacity = new NativePerThreadSumInt(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Production.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledConsumption.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCharge.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCapacity.Dispose();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastProduction);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastConsumption);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFulfilledConsumption);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastBatteryCharge);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastBatteryCapacity);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Game.Version.electricityStats)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastProduction);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastConsumption);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastFulfilledConsumption);
        if (!(reader.context.version >= Game.Version.batteryStats))
          return;
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastBatteryCharge);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastBatteryCapacity);
      }
      else
      {
        if (!(reader.context.version > Game.Version.seekerReferences))
          return;
        reader.Read(out float _);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastConsumption);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastProduction);
        reader.Read(out int _);
        if (!(reader.context.version > Game.Version.transmittedElectricity))
          return;
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastFulfilledConsumption);
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastProduction = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastConsumption = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastFulfilledConsumption = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastBatteryCharge = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastBatteryCapacity = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastProduction = this.m_Production.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastConsumption = this.m_Consumption.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFulfilledConsumption = this.m_FulfilledConsumption.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastBatteryCharge = this.m_BatteryCharge.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastBatteryCapacity = this.m_BatteryCapacity.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_Production.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_FulfilledConsumption.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCharge.Count = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCapacity.Count = 0;
      JobHandle job0 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ProducerGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        job0 = new ElectricityStatisticsSystem.CountElectricityProductionJob()
        {
          m_ProducerType = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle,
          m_Production = this.m_Production.ToConcurrent()
        }.ScheduleParallel<ElectricityStatisticsSystem.CountElectricityProductionJob>(this.m_ProducerGroup, this.Dependency);
      }
      JobHandle job1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ConsumerGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        job1 = new ElectricityStatisticsSystem.CountElectricityConsumptionJob()
        {
          m_ConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle,
          m_Consumption = this.m_Consumption.ToConcurrent(),
          m_FulfilledConsumption = this.m_FulfilledConsumption.ToConcurrent()
        }.ScheduleParallel<ElectricityStatisticsSystem.CountElectricityConsumptionJob>(this.m_ConsumerGroup, this.Dependency);
      }
      JobHandle job2 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ConsumerGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        job2 = new ElectricityStatisticsSystem.CountBatteryCapacityJob()
        {
          m_BatteryType = this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle,
          m_Charge = this.m_BatteryCharge.ToConcurrent(),
          m_Capacity = this.m_BatteryCapacity.ToConcurrent()
        }.ScheduleParallel<ElectricityStatisticsSystem.CountBatteryCapacityJob>(this.m_BatteryGroup, this.Dependency);
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
    public ElectricityStatisticsSystem()
    {
    }

    [BurstCompile]
    private struct CountElectricityProductionJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<ElectricityProducer> m_ProducerType;
      public NativePerThreadSumInt.Concurrent m_Production;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityProducer> nativeArray = chunk.GetNativeArray<ElectricityProducer>(ref this.m_ProducerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Production.Add(nativeArray[index].m_Capacity);
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
    private struct CountElectricityConsumptionJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> m_ConsumerType;
      public NativePerThreadSumInt.Concurrent m_Consumption;
      public NativePerThreadSumInt.Concurrent m_FulfilledConsumption;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityConsumer> nativeArray = chunk.GetNativeArray<ElectricityConsumer>(ref this.m_ConsumerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          ElectricityConsumer electricityConsumer = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          this.m_Consumption.Add(electricityConsumer.m_WantedConsumption);
          // ISSUE: reference to a compiler-generated field
          this.m_FulfilledConsumption.Add(electricityConsumer.m_FulfilledConsumption);
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
    private struct CountBatteryCapacityJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Battery> m_BatteryType;
      public NativePerThreadSumInt.Concurrent m_Charge;
      public NativePerThreadSumInt.Concurrent m_Capacity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Battery> nativeArray = chunk.GetNativeArray<Battery>(ref this.m_BatteryType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Battery battery = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          this.m_Charge.Add(battery.storedEnergyHours);
          // ISSUE: reference to a compiler-generated field
          this.m_Capacity.Add(battery.m_Capacity);
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
      public ComponentTypeHandle<ElectricityProducer> __Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Battery> __Game_Buildings_Battery_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Battery_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Battery>(true);
      }
    }
  }
}
