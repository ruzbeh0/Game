// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficSpawner
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab), typeof (MarkerObjectPrefab)})]
  public class TrafficSpawner : ComponentBase
  {
    public RoadTypes m_RoadType = RoadTypes.Car;
    public TrackTypes m_TrackType;
    public float m_SpawnRate = 0.5f;
    public bool m_NoSlowVehicles;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TrafficSpawnerData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TrafficSpawner>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<TrafficSpawnerData>(entity, new TrafficSpawnerData()
      {
        m_SpawnRate = this.m_SpawnRate,
        m_RoadType = this.m_RoadType,
        m_TrackType = this.m_TrackType,
        m_NoSlowVehicles = this.m_NoSlowVehicles
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(2));
    }
  }
}
