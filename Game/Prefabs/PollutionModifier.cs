// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PollutionModifier
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
  public class PollutionModifier : ComponentBase
  {
    [Tooltip("Factor to increase (+) or decrease (-) ground pollution")]
    [Range(-1f, 1f)]
    public float m_GroundPollutionMultiplier;
    [Tooltip("Factor to increase (+) or decrease (-) air pollution")]
    [Range(-1f, 1f)]
    public float m_AirPollutionMultiplier;
    [Tooltip("Factor to increase (+) or decrease (-) noise pollution")]
    [Range(-1f, 1f)]
    public float m_NoisePollutionMultiplier;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PollutionModifierData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      if (!this.prefab.Has<ServiceUpgrade>())
        ComponentBase.baseLog.ErrorFormat((UnityEngine.Object) this.prefab, "PollutionModifier should only be added to service upgrades: {0}", (object) this.prefab.name);
      entityManager.SetComponentData<PollutionModifierData>(entity, new PollutionModifierData()
      {
        m_GroundPollutionMultiplier = this.m_GroundPollutionMultiplier,
        m_AirPollutionMultiplier = this.m_AirPollutionMultiplier,
        m_NoisePollutionMultiplier = this.m_NoisePollutionMultiplier
      });
    }
  }
}
