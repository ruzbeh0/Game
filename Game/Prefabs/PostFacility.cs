// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PostFacility
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
  public class PostFacility : ComponentBase, IServiceUpgrade
  {
    public int m_PostVanCapacity = 10;
    public int m_PostTruckCapacity;
    public int m_MailStorageCapacity = 100000;
    public int m_MailBoxCapacity = 10000;
    public int m_SortingRate;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PostFacilityData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
      if (this.m_MailBoxCapacity <= 0)
        return;
      components.Add(ComponentType.ReadWrite<MailBoxData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.PostFacility>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
      {
        components.Add(ComponentType.ReadWrite<GuestVehicle>());
        components.Add(ComponentType.ReadWrite<Efficiency>());
      }
      if (this.m_PostTruckCapacity > 0)
      {
        components.Add(ComponentType.ReadWrite<ServiceDispatch>());
        components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      }
      if (this.m_PostVanCapacity > 0)
      {
        components.Add(ComponentType.ReadWrite<ServiceDispatch>());
        components.Add(ComponentType.ReadWrite<ServiceDistrict>());
        components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      }
      if (this.m_MailBoxCapacity <= 0)
        return;
      components.Add(ComponentType.ReadWrite<Game.Routes.MailBox>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.PostFacility>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      if (this.m_PostTruckCapacity > 0)
      {
        components.Add(ComponentType.ReadWrite<ServiceDispatch>());
        components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      }
      if (this.m_PostVanCapacity > 0)
      {
        components.Add(ComponentType.ReadWrite<ServiceDispatch>());
        components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      }
      if (this.m_MailBoxCapacity <= 0)
        return;
      components.Add(ComponentType.ReadWrite<Game.Routes.MailBox>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      PostFacilityData componentData1;
      componentData1.m_PostVanCapacity = this.m_PostVanCapacity;
      componentData1.m_PostTruckCapacity = this.m_PostTruckCapacity;
      componentData1.m_MailCapacity = this.m_MailStorageCapacity;
      componentData1.m_SortingRate = this.m_SortingRate;
      entityManager.SetComponentData<PostFacilityData>(entity, componentData1);
      if (this.m_MailBoxCapacity > 0)
      {
        MailBoxData componentData2;
        componentData2.m_MailCapacity = this.m_MailBoxCapacity;
        entityManager.SetComponentData<MailBoxData>(entity, componentData2);
      }
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(11));
    }
  }
}
