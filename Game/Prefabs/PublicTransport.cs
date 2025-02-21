// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PublicTransport
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using Game.Pathfind;
using Game.PSI;
using Game.Simulation;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ExcludeGeneratedModTag]
  [ComponentMenu("Vehicles/", new Type[] {typeof (VehiclePrefab)})]
  public class PublicTransport : ComponentBase
  {
    public TransportType m_TransportType;
    public int m_PassengerCapacity = 30;
    [EnumFlag]
    public PublicTransportPurpose m_Purposes = PublicTransportPurpose.TransportLine;
    public float m_MaintenanceRange;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PublicTransportVehicleData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Vehicles.PublicTransport>());
      components.Add(ComponentType.ReadWrite<Passenger>());
      components.Add(ComponentType.ReadWrite<Odometer>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()) || components.Contains(ComponentType.ReadWrite<Controller>()) && !components.Contains(ComponentType.ReadWrite<LayoutElement>()))
        return;
      components.Add(ComponentType.ReadWrite<PathInformation>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<PublicTransportVehicleData>(entity, new PublicTransportVehicleData(this.m_TransportType, this.m_PassengerCapacity, this.m_Purposes, this.m_MaintenanceRange * 1000f));
      if (!entityManager.HasComponent<CarData>(entity))
        return;
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(1));
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        if ((this.m_Purposes & PublicTransportPurpose.TransportLine) != (PublicTransportPurpose) 0)
        {
          yield return nameof (PublicTransport);
          yield return nameof (PublicTransport) + this.m_TransportType.ToString();
        }
      }
    }
  }
}
