// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PersonalCar
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new System.Type[] {typeof (CarPrefab), typeof (CarTrailerPrefab)})]
  public class PersonalCar : ComponentBase
  {
    public int m_PassengerCapacity = 5;
    public int m_BaggageCapacity = 5;
    public int m_CostToDrive = 8;
    [Range(0.0f, 100f)]
    public int m_Probability = 100;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PersonalCarData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Vehicles.PersonalCar>());
      components.Add(ComponentType.ReadWrite<Passenger>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<PersonalCarData>(entity, new PersonalCarData()
      {
        m_PassengerCapacity = this.m_PassengerCapacity,
        m_BaggageCapacity = this.m_BaggageCapacity,
        m_CostToDrive = this.m_CostToDrive,
        m_Probability = this.m_Probability
      });
    }
  }
}
