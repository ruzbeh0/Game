// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LeisureProvider
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Economy;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab), typeof (CompanyPrefab)})]
  public class LeisureProvider : ComponentBase
  {
    public int m_Efficiency;
    [HideInInspector]
    public ResourceInEditor m_Resources;
    public LeisureType m_LeisureType;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<LeisureProviderData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (this.m_Efficiency <= 0)
        return;
      components.Add(ComponentType.ReadWrite<Game.Buildings.LeisureProvider>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<LeisureProviderData>(entity, new LeisureProviderData()
      {
        m_Efficiency = this.m_Efficiency,
        m_Resources = EconomyUtils.GetResource(this.m_Resources),
        m_LeisureType = this.m_LeisureType
      });
    }
  }
}
