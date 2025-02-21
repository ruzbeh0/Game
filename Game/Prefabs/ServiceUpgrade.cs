// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ServiceUpgrade
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class ServiceUpgrade : ComponentBase
  {
    public BuildingPrefab[] m_Buildings;
    public uint m_UpgradeCost = 100;
    public int m_XPReward;
    public int m_MaxPlacementOffset = -1;
    public float m_MaxPlacementDistance;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Buildings == null)
        return;
      for (int index = 0; index < this.m_Buildings.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Buildings[index]);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ServiceUpgradeData>());
      components.Add(ComponentType.ReadWrite<ServiceUpgradeBuilding>());
      if ((UnityEngine.Object) this.GetComponent<BuildingPrefab>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
      ServiceConsumption component;
      if (!this.prefab.TryGet<ServiceConsumption>(out component) || component.m_Upkeep <= 0)
        return;
      components.Add(ComponentType.ReadWrite<ServiceUpkeepData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.ServiceUpgrade>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<ServiceUpgradeData>(entity, new ServiceUpgradeData()
      {
        m_UpgradeCost = this.m_UpgradeCost,
        m_XPReward = this.m_XPReward,
        m_MaxPlacementOffset = this.m_MaxPlacementOffset,
        m_MaxPlacementDistance = this.m_MaxPlacementDistance
      });
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (this.m_Buildings == null)
        return;
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Buildings.Length; ++index)
      {
        BuildingPrefab building = this.m_Buildings[index];
        if (!((UnityEngine.Object) building == (UnityEngine.Object) null))
        {
          // ISSUE: reference to a compiler-generated method
          entityManager.GetBuffer<ServiceUpgradeBuilding>(entity).Add(new ServiceUpgradeBuilding(existingSystemManaged.GetEntity((PrefabBase) building)));
          building.AddUpgrade(entityManager, this);
        }
      }
    }
  }
}
