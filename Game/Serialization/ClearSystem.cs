// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ClearSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Effects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  public class ClearSystem : GameSystemBase
  {
    private EntityQuery m_ClearQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ClearQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[21]
        {
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<LoadedIndex>(),
          ComponentType.ReadOnly<ElectricityFlowNode>(),
          ComponentType.ReadOnly<ElectricityFlowEdge>(),
          ComponentType.ReadOnly<WaterPipeNode>(),
          ComponentType.ReadOnly<WaterPipeEdge>(),
          ComponentType.ReadOnly<ServiceRequest>(),
          ComponentType.ReadOnly<Game.Simulation.WaterSourceData>(),
          ComponentType.ReadOnly<Game.City.City>(),
          ComponentType.ReadOnly<SchoolSeeker>(),
          ComponentType.ReadOnly<JobSeeker>(),
          ComponentType.ReadOnly<CityStatistic>(),
          ComponentType.ReadOnly<ServiceBudgetData>(),
          ComponentType.ReadOnly<FloodCounterData>(),
          ComponentType.ReadOnly<CoordinatedMeeting>(),
          ComponentType.ReadOnly<LookingForPartner>(),
          ComponentType.ReadOnly<EffectInstance>(),
          ComponentType.ReadOnly<AtmosphereData>(),
          ComponentType.ReadOnly<BiomeData>(),
          ComponentType.ReadOnly<CreationDefinition>(),
          ComponentType.ReadOnly<TimeData>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<NetCompositionData>(),
          ComponentType.ReadOnly<PrefabData>()
        }
      });
    }

    [Preserve]
    protected override void OnUpdate() => this.EntityManager.DestroyEntity(this.m_ClearQuery);

    [Preserve]
    public ClearSystem()
    {
    }
  }
}
