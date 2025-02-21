// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Effects;
using Game.Objects;
using Game.Policies;
using Game.Simulation;
using Game.UI.Editor;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new Type[] {})]
  public class BuildingPrefab : StaticObjectPrefab
  {
    public BuildingAccessType m_AccessType;
    [CustomField(typeof (BuildingLotWidthField))]
    public int m_LotWidth = 4;
    [CustomField(typeof (BuildingLotDepthField))]
    public int m_LotDepth = 4;

    public int lotSize => this.m_LotWidth * this.m_LotDepth;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<BuildingData>());
      components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
      components.Add(ComponentType.ReadWrite<BuildingTerraformData>());
      components.Add(ComponentType.ReadWrite<Effect>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Building>());
      components.Add(ComponentType.ReadWrite<CitizenPresence>());
      components.Add(ComponentType.ReadWrite<SpawnLocationElement>());
      components.Add(ComponentType.ReadWrite<CurrentDistrict>());
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
      components.Add(ComponentType.ReadWrite<Color>());
      components.Add(ComponentType.ReadWrite<Game.Objects.Surface>());
      components.Add(ComponentType.ReadWrite<BuildingModifier>());
      components.Add(ComponentType.ReadWrite<Policy>());
      components.Add(ComponentType.ReadWrite<Game.Net.SubLane>());
      components.Add(ComponentType.ReadWrite<Game.Objects.SubObject>());
      components.Add(ComponentType.ReadWrite<Game.Buildings.Lot>());
      components.Add(ComponentType.ReadWrite<EnabledEffect>());
    }

    protected override void RefreshArchetype(EntityManager entityManager, Entity entity)
    {
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
      if (entityManager.HasComponent<BuildingUpgradeElement>(entity))
        componentTypeSet.Add(ComponentType.ReadWrite<InstalledUpgrade>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      entityManager.SetComponentData<ObjectData>(entity, new ObjectData()
      {
        m_Archetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet))
      });
    }

    public void AddUpgrade(EntityManager entityManager, ServiceUpgrade upgrade)
    {
      Entity entity;
      // ISSUE: reference to a compiler-generated method
      if (!entityManager.World.GetExistingSystemManaged<PrefabSystem>().TryGetEntity((PrefabBase) this, out entity))
        throw new Exception("Building prefab entity not found for upgrade!");
      if (entityManager.HasComponent<BuildingUpgradeElement>(entity))
        return;
      entityManager.AddBuffer<BuildingUpgradeElement>(entity);
      if (!entityManager.GetComponentData<ObjectData>(entity).m_Archetype.Valid)
        return;
      this.RefreshArchetype(entityManager, entity);
    }
  }
}
