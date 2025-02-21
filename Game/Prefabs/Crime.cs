// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Crime
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Events;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new Type[] {typeof (EventPrefab)})]
  public class Crime : ComponentBase
  {
    public EventTargetType m_RandomTargetType = EventTargetType.Citizen;
    public CrimeType m_CrimeType;
    public Bounds1 m_OccurenceProbability = new Bounds1(0.0f, 50f);
    public Bounds1 m_RecurrenceProbability = new Bounds1(0.0f, 100f);
    public Bounds1 m_AlarmDelay = new Bounds1(5f, 10f);
    public Bounds1 m_CrimeDuration = new Bounds1(20f, 60f);
    public Bounds1 m_CrimeIncomeAbsolute = new Bounds1(100f, 1000f);
    public Bounds1 m_CrimeIncomeRelative = new Bounds1(0.0f, 0.25f);
    public Bounds1 m_JailTimeRange = new Bounds1(0.125f, 1f);
    public Bounds1 m_PrisonTimeRange = new Bounds1(1f, 100f);
    public float m_PrisonProbability = 50f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CrimeData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.Crime>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      CrimeData componentData;
      componentData.m_RandomTargetType = this.m_RandomTargetType;
      componentData.m_CrimeType = this.m_CrimeType;
      componentData.m_OccurenceProbability = this.m_OccurenceProbability;
      componentData.m_RecurrenceProbability = this.m_RecurrenceProbability;
      componentData.m_AlarmDelay = this.m_AlarmDelay;
      componentData.m_CrimeDuration = this.m_CrimeDuration;
      componentData.m_CrimeIncomeAbsolute = this.m_CrimeIncomeAbsolute;
      componentData.m_CrimeIncomeRelative = this.m_CrimeIncomeRelative;
      componentData.m_JailTimeRange = this.m_JailTimeRange;
      componentData.m_PrisonTimeRange = this.m_PrisonTimeRange;
      componentData.m_PrisonProbability = this.m_PrisonProbability;
      entityManager.SetComponentData<CrimeData>(entity, componentData);
    }
  }
}
