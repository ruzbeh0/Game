// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RoadPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Net;
using Game.Simulation;
using Game.Zones;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Prefab/", new System.Type[] {})]
  public class RoadPrefab : NetGeometryPrefab
  {
    public RoadType m_RoadType;
    public float m_SpeedLimit = 100f;
    public ZoneBlockPrefab m_ZoneBlock;
    public bool m_TrafficLights;
    public bool m_HighwayRules;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_ZoneBlock != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_ZoneBlock);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<RoadData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Game.Net.Edge>()))
      {
        components.Add(ComponentType.ReadWrite<Road>());
        components.Add(ComponentType.ReadWrite<UpdateFrame>());
        components.Add(ComponentType.ReadWrite<LandValue>());
        components.Add(ComponentType.ReadWrite<EdgeColor>());
        components.Add(ComponentType.ReadWrite<NetCondition>());
        components.Add(ComponentType.ReadWrite<MaintenanceConsumer>());
        components.Add(ComponentType.ReadWrite<BorderDistrict>());
        if ((UnityEngine.Object) this.m_ZoneBlock != (UnityEngine.Object) null)
        {
          components.Add(ComponentType.ReadWrite<SubBlock>());
          components.Add(ComponentType.ReadWrite<ConnectedBuilding>());
          components.Add(ComponentType.ReadWrite<Game.Net.ServiceCoverage>());
          components.Add(ComponentType.ReadWrite<ResourceAvailability>());
          components.Add(ComponentType.ReadWrite<Density>());
        }
        else
        {
          if (this.m_HighwayRules)
            return;
          components.Add(ComponentType.ReadWrite<ConnectedBuilding>());
        }
      }
      else if (components.Contains(ComponentType.ReadWrite<Game.Net.Node>()))
      {
        components.Add(ComponentType.ReadWrite<Road>());
        components.Add(ComponentType.ReadWrite<UpdateFrame>());
        components.Add(ComponentType.ReadWrite<LandValue>());
        components.Add(ComponentType.ReadWrite<NodeColor>());
        components.Add(ComponentType.ReadWrite<NetCondition>());
        components.Add(ComponentType.ReadWrite<Game.Objects.Surface>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<NetCompositionData>()))
          return;
        components.Add(ComponentType.ReadWrite<RoadComposition>());
      }
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        yield return "Roads";
      }
    }
  }
}
