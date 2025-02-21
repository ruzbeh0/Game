// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GroundWaterPollutionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public class GroundWaterPollutionSystem : GameSystemBase
  {
    private GroundWaterSystem m_GroundWaterSystem;
    private GroundPollutionSystem m_GroundPollutionSystem;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      JobHandle dependencies2;
      this.Dependency = new GroundWaterPollutionSystem.PolluteGroundWaterJob()
      {
        m_GroundWaterMap = this.m_GroundWaterSystem.GetMap(false, out dependencies1),
        m_PollutionMap = this.m_GroundPollutionSystem.GetMap(true, out dependencies2)
      }.Schedule<GroundWaterPollutionSystem.PolluteGroundWaterJob>(JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      this.m_GroundWaterSystem.AddWriter(this.Dependency);
      this.m_GroundPollutionSystem.AddReader(this.Dependency);
    }

    [UnityEngine.Scripting.Preserve]
    public GroundWaterPollutionSystem()
    {
    }

    [BurstCompile]
    private struct PolluteGroundWaterJob : IJob
    {
      public NativeArray<GroundWater> m_GroundWaterMap;
      [ReadOnly]
      public NativeArray<GroundPollution> m_PollutionMap;

      public void Execute()
      {
        for (int index = 0; index < this.m_GroundWaterMap.Length; ++index)
        {
          GroundWater groundWater = this.m_GroundWaterMap[index];
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          GroundPollution pollution = GroundPollutionSystem.GetPollution(GroundWaterSystem.GetCellCenter(index), this.m_PollutionMap);
          if (pollution.m_Pollution > (short) 0)
          {
            groundWater.m_Polluted = (short) math.min((int) groundWater.m_Amount, (int) groundWater.m_Polluted + (int) pollution.m_Pollution / 200);
            this.m_GroundWaterMap[index] = groundWater;
          }
        }
      }
    }
  }
}
