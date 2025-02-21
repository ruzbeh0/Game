// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TelecomFacility
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class TelecomFacility : ComponentBase, IServiceUpgrade
  {
    public float m_Range = 1000f;
    public float m_NetworkCapacity = 10000f;
    public bool m_PenetrateTerrain;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TelecomFacilityData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TelecomFacility>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Efficiency>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TelecomFacility>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<TelecomFacilityData>(entity, new TelecomFacilityData()
      {
        m_Range = this.m_Range,
        m_NetworkCapacity = this.m_NetworkCapacity,
        m_PenetrateTerrain = this.m_PenetrateTerrain
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(13));
    }
  }
}
