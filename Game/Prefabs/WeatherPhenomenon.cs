// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WeatherPhenomenon
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Events;
using Game.Rendering;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new System.Type[] {typeof (EventPrefab)})]
  public class WeatherPhenomenon : ComponentBase
  {
    public float m_OccurrenceProbability = 10f;
    public Bounds1 m_OccurenceTemperature = new Bounds1(0.0f, 15f);
    public Bounds1 m_OccurenceRain = new Bounds1(0.0f, 1f);
    public Bounds1 m_Duration = new Bounds1(15f, 90f);
    public Bounds1 m_PhenomenonRadius = new Bounds1(500f, 1000f);
    public Bounds1 m_HotspotRadius = new Bounds1(0.8f, 0.9f);
    public Bounds1 m_LightningInterval = new Bounds1(0.0f, 0.0f);
    [Range(0.0f, 1f)]
    public float m_HotspotInstability = 0.1f;
    [Range(0.0f, 100f)]
    public float m_DamageSeverity = 10f;
    [Tooltip("How dangerous the disaster is for the cims in the city. Determines how likely cims will leave shelter while the disaster is ongoing")]
    [Range(0.0f, 1f)]
    public float m_DangerLevel = 1f;
    public bool m_Evacuate;
    public bool m_StayIndoors;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WeatherPhenomenonData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.WeatherPhenomenon>());
      components.Add(ComponentType.ReadWrite<HotspotFrame>());
      components.Add(ComponentType.ReadWrite<Duration>());
      components.Add(ComponentType.ReadWrite<DangerLevel>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
      components.Add(ComponentType.ReadWrite<InterpolatedTransform>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      WeatherPhenomenonData componentData;
      componentData.m_OccurenceProbability = this.m_OccurrenceProbability;
      componentData.m_HotspotInstability = this.m_HotspotInstability;
      componentData.m_DamageSeverity = this.m_DamageSeverity;
      componentData.m_DangerLevel = this.m_DangerLevel;
      componentData.m_PhenomenonRadius = this.m_PhenomenonRadius;
      componentData.m_HotspotRadius = this.m_HotspotRadius;
      componentData.m_LightningInterval = this.m_LightningInterval;
      componentData.m_Duration = this.m_Duration;
      componentData.m_OccurenceTemperature = this.m_OccurenceTemperature;
      componentData.m_OccurenceRain = this.m_OccurenceRain;
      componentData.m_DangerFlags = (DangerFlags) 0;
      if (this.m_Evacuate)
        componentData.m_DangerFlags = DangerFlags.Evacuate;
      if (this.m_StayIndoors)
        componentData.m_DangerFlags = DangerFlags.StayIndoors;
      entityManager.SetComponentData<WeatherPhenomenonData>(entity, componentData);
    }
  }
}
