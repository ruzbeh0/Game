// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Prison
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Simulation;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class Prison : ComponentBase, IServiceUpgrade
  {
    public int m_PrisonVanCapacity = 10;
    public int m_PrisonerCapacity = 500;
    public sbyte m_PrisonerWellbeing;
    public sbyte m_PrisonerHealth;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PrisonData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.Prison>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      if ((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<ServiceDistrict>());
      if (this.m_PrisonerCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Occupant>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.Prison>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      if (this.m_PrisonerCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Occupant>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      PrisonData componentData;
      componentData.m_PrisonVanCapacity = this.m_PrisonVanCapacity;
      componentData.m_PrisonerCapacity = this.m_PrisonerCapacity;
      componentData.m_PrisonerWellbeing = this.m_PrisonerWellbeing;
      componentData.m_PrisonerHealth = this.m_PrisonerHealth;
      entityManager.SetComponentData<PrisonData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(3));
    }
  }
}
