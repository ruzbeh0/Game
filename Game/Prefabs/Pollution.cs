// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Pollution
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class Pollution : ComponentBase, IServiceUpgrade
  {
    [Min(0.0f)]
    public int m_GroundPollution;
    [Min(0.0f)]
    public int m_AirPollution;
    [Min(0.0f)]
    public int m_NoisePollution;
    [Tooltip("Disable this if you don't want the pollution to be multiplied with renters(household members/employees)")]
    public bool m_ScaleWithRenters = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PollutionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (this.prefab.Has<ServiceUpgrade>() || this.prefab.Has<PlaceholderBuilding>())
        return;
      this.GetPollutionData().AddArchetypeComponents(components);
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      this.GetPollutionData().AddArchetypeComponents(components);
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<PollutionData>(entity, this.GetPollutionData());
    }

    private PollutionData GetPollutionData()
    {
      return new PollutionData()
      {
        m_GroundPollution = (float) this.m_GroundPollution,
        m_AirPollution = (float) this.m_AirPollution,
        m_NoisePollution = (float) this.m_NoisePollution,
        m_ScaleWithRenters = this.m_ScaleWithRenters
      };
    }
  }
}
