// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Park
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class Park : ComponentBase
  {
    public short m_MaintenancePool;
    public bool m_AllowHomeless = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ParkData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.Park>());
      components.Add(ComponentType.ReadWrite<Renter>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<MaintenanceConsumer>());
      components.Add(ComponentType.ReadWrite<ModifiedServiceCoverage>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<ParkData>(entity, new ParkData()
      {
        m_MaintenancePool = this.m_MaintenancePool,
        m_AllowHomeless = this.m_AllowHomeless
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(9));
    }
  }
}
