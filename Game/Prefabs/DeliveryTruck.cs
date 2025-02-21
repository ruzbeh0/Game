// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DeliveryTruck
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using Game.Pathfind;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new Type[] {typeof (CarPrefab), typeof (CarTrailerPrefab)})]
  public class DeliveryTruck : ComponentBase
  {
    public int m_CargoCapacity = 10000;
    public int m_CostToDrive = 16;
    public ResourceInEditor[] m_TransportedResources;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<DeliveryTruckData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Vehicles.DeliveryTruck>());
      if (!(this.prefab is CarPrefab))
        return;
      components.Add(ComponentType.ReadWrite<PathInformation>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      DeliveryTruckData componentData = new DeliveryTruckData();
      componentData.m_CargoCapacity = this.m_CargoCapacity;
      componentData.m_CostToDrive = this.m_CostToDrive;
      if (this.m_TransportedResources != null)
      {
        for (int index = 0; index < this.m_TransportedResources.Length; ++index)
          componentData.m_TransportedResources |= EconomyUtils.GetResource(this.m_TransportedResources[index]);
      }
      entityManager.SetComponentData<DeliveryTruckData>(entity, componentData);
    }
  }
}
