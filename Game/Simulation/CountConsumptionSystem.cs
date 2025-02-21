// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CountConsumptionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public class CountConsumptionSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    public static readonly int kUpdatesPerDay = 32;
    private NativeArray<int> m_Consumptions;
    private NativeArray<int> m_ConsumptionAccumulator;
    private JobHandle m_ReadDeps;
    private JobHandle m_WriteDeps;
    private JobHandle m_CopyDeps;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return 262144 / CountConsumptionSystem.kUpdatesPerDay;
    }

    public NativeArray<int> GetConsumptions(out JobHandle deps)
    {
      deps = this.m_CopyDeps;
      return this.m_Consumptions;
    }

    public NativeArray<int> GetConsumptionAccumulator(out JobHandle deps)
    {
      deps = this.m_WriteDeps;
      return this.m_ConsumptionAccumulator;
    }

    public void AddConsumptionReader(JobHandle deps)
    {
      this.m_ReadDeps = JobHandle.CombineDependencies(this.m_ReadDeps, deps);
    }

    public void AddConsumptionWriter(JobHandle deps)
    {
      this.m_WriteDeps = JobHandle.CombineDependencies(this.m_WriteDeps, deps);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_Consumptions = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Persistent);
      this.m_ConsumptionAccumulator = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      this.m_ConsumptionAccumulator.Dispose();
      this.m_Consumptions.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning() => base.OnStopRunning();

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      this.Dependency = new CountConsumptionSystem.CopyConsumptionJob()
      {
        m_Accumulator = this.m_ConsumptionAccumulator,
        m_Consumptions = this.m_Consumptions
      }.Schedule<CountConsumptionSystem.CopyConsumptionJob>(JobHandle.CombineDependencies(this.m_ReadDeps, this.m_WriteDeps));
      this.m_CopyDeps = this.Dependency;
      this.m_WriteDeps = this.Dependency;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      for (int index = 0; index < this.m_ConsumptionAccumulator.Length; ++index)
      {
        this.m_ConsumptionAccumulator[index] = 0;
        this.m_Consumptions[index] = 0;
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ConsumptionAccumulator);
      writer.Write(this.m_Consumptions);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(this.m_ConsumptionAccumulator);
      reader.Read(this.m_Consumptions);
    }

    [UnityEngine.Scripting.Preserve]
    public CountConsumptionSystem()
    {
    }

    [BurstCompile]
    private struct CopyConsumptionJob : IJob
    {
      public NativeArray<int> m_Accumulator;
      public NativeArray<int> m_Consumptions;

      public void Execute()
      {
        for (int index = 0; index < this.m_Accumulator.Length; ++index)
        {
          this.m_Consumptions[index] = this.m_Consumptions[index] == 0 ? this.m_Accumulator[index] : Mathf.RoundToInt((float) CountConsumptionSystem.kUpdatesPerDay * math.lerp((float) (this.m_Consumptions[index] / CountConsumptionSystem.kUpdatesPerDay), (float) this.m_Accumulator[index], 0.3f));
          this.m_Accumulator[index] = 0;
        }
      }
    }
  }
}
