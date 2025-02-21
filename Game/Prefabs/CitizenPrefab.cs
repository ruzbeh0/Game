// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CitizenPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Citizens;
using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Citizens/", new Type[] {})]
  public class CitizenPrefab : ArchetypePrefab
  {
    public bool m_Male;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CitizenData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Citizen>());
      components.Add(ComponentType.ReadWrite<TripNeeded>());
      components.Add(ComponentType.ReadWrite<CrimeVictim>());
      components.Add(ComponentType.ReadWrite<MailSender>());
      components.Add(ComponentType.ReadWrite<Arrived>());
      components.Add(ComponentType.ReadWrite<CarKeeper>());
      components.Add(ComponentType.ReadWrite<HasJobSeeker>());
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<CitizenData>(entity, new CitizenData()
      {
        m_Male = this.m_Male
      });
    }
  }
}
