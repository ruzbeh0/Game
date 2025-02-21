// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZonePollution
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Zones/", new System.Type[] {typeof (ZonePrefab)})]
  public class ZonePollution : ComponentBase, IZoneBuildingComponent
  {
    [Min(0.0f)]
    public float m_GroundPollution;
    [Min(0.0f)]
    public float m_AirPollution;
    [Min(0.0f)]
    public float m_NoisePollution;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ZonePollutionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<ZonePollutionData>(entity, new ZonePollutionData()
      {
        m_GroundPollution = this.m_GroundPollution,
        m_AirPollution = this.m_AirPollution,
        m_NoisePollution = this.m_NoisePollution
      });
    }

    public void GetBuildingPrefabComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      components.Add(ComponentType.ReadWrite<PollutionData>());
    }

    public void GetBuildingArchetypeComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      this.GetBuildingPollutionData(buildingPrefab).AddArchetypeComponents(components);
    }

    public void InitializeBuilding(
      EntityManager entityManager,
      Entity entity,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      if (buildingPrefab.Has<Pollution>())
        return;
      entityManager.SetComponentData<PollutionData>(entity, this.GetBuildingPollutionData(buildingPrefab));
    }

    private PollutionData GetBuildingPollutionData(BuildingPrefab buildingPrefab)
    {
      int lotSize = buildingPrefab.lotSize;
      return new PollutionData()
      {
        m_GroundPollution = this.m_GroundPollution * (float) lotSize,
        m_AirPollution = this.m_AirPollution * (float) lotSize,
        m_NoisePollution = this.m_NoisePollution * (float) lotSize
      };
    }
  }
}
