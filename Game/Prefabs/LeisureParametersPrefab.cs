// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LeisureParametersPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class LeisureParametersPrefab : PrefabBase
  {
    public EventPrefab m_TravelingEvent;
    public EventPrefab m_AttractionPrefab;
    public EventPrefab m_SightseeingPrefab;
    public int m_LeisureRandomFactor = 512;
    [Tooltip("The lodging resource consuming speed of tourist.")]
    public int m_TouristLodgingConsumePerDay = 100;
    [Tooltip("The service available consuming speed of tourist")]
    public int m_TouristServiceConsumePerDay = 100;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_TravelingEvent);
      prefabs.Add((PrefabBase) this.m_AttractionPrefab);
      prefabs.Add((PrefabBase) this.m_SightseeingPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<LeisureParametersData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      LeisureParametersData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_TravelingPrefab = systemManaged.GetEntity((PrefabBase) this.m_TravelingEvent);
      // ISSUE: reference to a compiler-generated method
      componentData.m_AttractionPrefab = systemManaged.GetEntity((PrefabBase) this.m_AttractionPrefab);
      // ISSUE: reference to a compiler-generated method
      componentData.m_SightseeingPrefab = systemManaged.GetEntity((PrefabBase) this.m_SightseeingPrefab);
      componentData.m_LeisureRandomFactor = this.m_LeisureRandomFactor;
      componentData.m_TouristLodgingConsumePerDay = this.m_TouristLodgingConsumePerDay;
      componentData.m_TouristServiceConsumePerDay = this.m_TouristServiceConsumePerDay;
      entityManager.SetComponentData<LeisureParametersData>(entity, componentData);
    }
  }
}
