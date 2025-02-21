// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.VehiclesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Citizens;
using Game.Companies;
using Game.Prefabs;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class VehiclesSection : InfoSectionBase
  {
    private DynamicBuffer<OwnedVehicle> m_Buffer;
    private Entity m_CompanyEntity;

    protected override string group => nameof (VehiclesSection);

    private VehicleLocaleKey vehicleKey { get; set; }

    private int vehicleCount { get; set; }

    private int availableVehicleCount { get; set; }

    private int vehicleCapacity { get; set; }

    private NativeList<VehiclesSection.UIVehicle> vehicleList { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.vehicleList = new NativeList<VehiclesSection.UIVehicle>(50, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.vehicleList.Dispose();
      base.OnDestroy();
    }

    protected override void Reset()
    {
      this.vehicleCount = 0;
      this.vehicleCapacity = 0;
      this.vehicleList.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Buffer = new DynamicBuffer<OwnedVehicle>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyEntity = Entity.Null;
    }

    private bool Visible()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.TryGetBuffer<OwnedVehicle>(this.selectedEntity, true, out this.m_Buffer))
        return true;
      PrefabRef component;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return CompanyUIUtils.HasCompany(this.EntityManager, this.selectedEntity, this.selectedPrefab, out this.m_CompanyEntity) && this.EntityManager.TryGetComponent<PrefabRef>(this.m_CompanyEntity, out component) && this.EntityManager.HasComponent<TransportCompanyData>(component.m_Prefab) && this.EntityManager.TryGetBuffer<OwnedVehicle>(this.m_CompanyEntity, true, out this.m_Buffer);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      this.vehicleKey = VehicleLocaleKey.Vehicle;
      string str = VehicleLocaleKey.Vehicle.ToString();
      Entity entity1 = this.selectedEntity;
      Entity entity2 = this.selectedPrefab;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CompanyEntity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        entity1 = this.m_CompanyEntity;
        PrefabRef component;
        // ISSUE: reference to a compiler-generated field
        entity2 = this.EntityManager.TryGetComponent<PrefabRef>(this.m_CompanyEntity, out component) ? component.m_Prefab : Entity.Null;
      }
      HospitalData data1;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<HospitalData>(entity1, entity2, out data1))
        this.vehicleCapacity += data1.m_AmbulanceCapacity + data1.m_MedicalHelicopterCapacity;
      PoliceStationData data2;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<PoliceStationData>(entity1, entity2, out data2))
        this.vehicleCapacity += data2.m_PatrolCarCapacity + data2.m_PoliceHelicopterCapacity;
      FireStationData data3;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<FireStationData>(entity1, entity2, out data3))
        this.vehicleCapacity += data3.m_FireEngineCapacity + data3.m_FireHelicopterCapacity + data3.m_DisasterResponseCapacity;
      PostFacilityData data4;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<PostFacilityData>(entity1, entity2, out data4))
        this.vehicleCapacity += data4.m_PostVanCapacity + data4.m_PostTruckCapacity;
      MaintenanceDepotData data5;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<MaintenanceDepotData>(entity1, entity2, out data5))
        this.vehicleCapacity += data5.m_VehicleCapacity;
      TransportDepotData data6;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<TransportDepotData>(entity1, entity2, out data6))
      {
        this.vehicleCapacity += data6.m_VehicleCapacity;
        str = VehicleLocaleKey.PublicTransportVehicle.ToString();
      }
      DeathcareFacilityData data7;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<DeathcareFacilityData>(entity1, entity2, out data7))
        this.vehicleCapacity += data7.m_HearseCapacity;
      GarbageFacilityData data8;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<GarbageFacilityData>(entity1, entity2, out data8))
        this.vehicleCapacity += data8.m_VehicleCapacity;
      PrisonData data9;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<PrisonData>(entity1, entity2, out data9))
        this.vehicleCapacity += data9.m_PrisonVanCapacity;
      EmergencyShelterData data10;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<EmergencyShelterData>(entity1, entity2, out data10))
        this.vehicleCapacity += data10.m_VehicleCapacity;
      TransportCompanyData component1;
      if (this.EntityManager.TryGetComponent<TransportCompanyData>(entity2, out component1))
        this.vehicleCapacity += component1.m_MaxTransports;
      if (this.EntityManager.HasComponent<Household>(this.selectedEntity))
        str = VehicleLocaleKey.HouseholdVehicle.ToString();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity vehicle = this.m_Buffer[index].m_Vehicle;
        if (!this.EntityManager.HasComponent<ParkedCar>(vehicle) && !this.EntityManager.HasComponent<ParkedTrain>(vehicle))
        {
          // ISSUE: reference to a compiler-generated method
          VehiclesSection.AddVehicle(this.EntityManager, vehicle, this.vehicleList);
        }
      }
      this.tooltipKeys.Add(str);
      this.vehicleCount = this.vehicleList.Length;
      this.availableVehicleCount = VehicleUIUtils.GetAvailableVehicles(entity1, this.EntityManager);
      this.vehicleList.Sort<VehiclesSection.UIVehicle>();
    }

    public static void AddVehicle(
      EntityManager entityManager,
      Entity vehicle,
      NativeList<VehiclesSection.UIVehicle> vehicleList)
    {
      VehicleStateLocaleKey stateKey = VehicleUIUtils.GetStateKey(vehicle, entityManager);
      PrefabRef componentData = entityManager.GetComponentData<PrefabRef>(vehicle);
      VehicleLocaleKey vehicleKey = VehicleLocaleKey.Vehicle;
      if (entityManager.HasComponent<Car>(vehicle))
      {
        if (entityManager.HasComponent<Game.Vehicles.Ambulance>(vehicle))
          vehicleKey = VehicleLocaleKey.Ambulance;
        PoliceCarData component1;
        if (entityManager.TryGetComponent<PoliceCarData>(componentData.m_Prefab, out component1))
          vehicleKey = VehicleUIUtils.GetPoliceVehicleLocaleKey(component1.m_PurposeMask);
        if (entityManager.HasComponent<Game.Vehicles.FireEngine>(vehicle))
          vehicleKey = VehicleLocaleKey.FireEngine;
        if (entityManager.HasComponent<Game.Vehicles.PostVan>(vehicle))
          vehicleKey = VehicleLocaleKey.PostVan;
        if (entityManager.HasComponent<Game.Vehicles.DeliveryTruck>(vehicle))
          vehicleKey = VehicleLocaleKey.DeliveryTruck;
        if (entityManager.HasComponent<Game.Vehicles.Hearse>(vehicle))
          vehicleKey = VehicleLocaleKey.Hearse;
        if (entityManager.HasComponent<Game.Vehicles.GarbageTruck>(vehicle))
          vehicleKey = VehicleLocaleKey.GarbageTruck;
        if (entityManager.HasComponent<Game.Vehicles.Taxi>(vehicle))
          vehicleKey = VehicleLocaleKey.Taxi;
        if (entityManager.HasComponent<Game.Vehicles.MaintenanceVehicle>(vehicle))
          vehicleKey = VehicleLocaleKey.MaintenanceVehicle;
        PublicTransportVehicleData component2;
        if (entityManager.HasComponent<Game.Vehicles.PublicTransport>(vehicle) && entityManager.TryGetComponent<PublicTransportVehicleData>(componentData.m_Prefab, out component2))
          vehicleKey = (component2.m_PurposeMask & PublicTransportPurpose.PrisonerTransport) != (PublicTransportPurpose) 0 ? VehicleLocaleKey.PrisonVan : ((component2.m_PurposeMask & PublicTransportPurpose.Evacuation) != (PublicTransportPurpose) 0 ? VehicleLocaleKey.EvacuationBus : VehicleLocaleKey.PublicTransportVehicle);
      }
      if (entityManager.HasComponent<Helicopter>(vehicle))
      {
        if (entityManager.HasComponent<Game.Vehicles.Ambulance>(vehicle))
          vehicleKey = VehicleLocaleKey.MedicalHelicopter;
        if (entityManager.HasComponent<Game.Vehicles.PoliceCar>(vehicle))
          vehicleKey = VehicleLocaleKey.PoliceHelicopter;
        if (entityManager.HasComponent<Game.Vehicles.FireEngine>(vehicle))
          vehicleKey = VehicleLocaleKey.FireHelicopter;
      }
      ref NativeList<VehiclesSection.UIVehicle> local1 = ref vehicleList;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      VehiclesSection.UIVehicle uiVehicle = new VehiclesSection.UIVehicle(vehicle, vehicleKey, stateKey);
      ref VehiclesSection.UIVehicle local2 = ref uiVehicle;
      local1.Add(in local2);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("vehicleKey");
      writer.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) this.vehicleKey));
      writer.PropertyName("vehicleCount");
      writer.Write(this.vehicleCount);
      writer.PropertyName("availableVehicleCount");
      writer.Write(this.availableVehicleCount);
      writer.PropertyName("vehicleCapacity");
      writer.Write(this.vehicleCapacity);
      this.vehicleList.Sort<VehiclesSection.UIVehicle>();
      writer.PropertyName("vehicleList");
      writer.ArrayBegin(this.vehicleList.Length);
      int index = 0;
      while (true)
      {
        int num = index;
        NativeList<VehiclesSection.UIVehicle> vehicleList = this.vehicleList;
        int length = vehicleList.Length;
        if (num < length)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          NameSystem nameSystem = this.m_NameSystem;
          IJsonWriter binder = writer;
          vehicleList = this.vehicleList;
          // ISSUE: variable of a compiler-generated type
          VehiclesSection.UIVehicle vehicle = vehicleList[index];
          // ISSUE: reference to a compiler-generated method
          VehiclesSection.BindVehicle(nameSystem, binder, vehicle);
          ++index;
        }
        else
          break;
      }
      writer.ArrayEnd();
    }

    public static void BindVehicle(
      NameSystem nameSystem,
      IJsonWriter binder,
      VehiclesSection.UIVehicle vehicle)
    {
      binder.TypeBegin("Game.UI.InGame.VehiclesSection.Vehicle");
      binder.PropertyName("entity");
      binder.Write(vehicle.entity);
      binder.PropertyName("name");
      // ISSUE: reference to a compiler-generated method
      nameSystem.BindName(binder, vehicle.entity);
      binder.PropertyName("vehicleKey");
      binder.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) vehicle.vehicleKey));
      binder.PropertyName("stateKey");
      binder.Write(Enum.GetName(typeof (VehicleStateLocaleKey), (object) vehicle.stateKey));
      binder.TypeEnd();
    }

    [Preserve]
    public VehiclesSection()
    {
    }

    public readonly struct UIVehicle : IComparable<VehiclesSection.UIVehicle>
    {
      public Entity entity { get; }

      public VehicleLocaleKey vehicleKey { get; }

      public VehicleStateLocaleKey stateKey { get; }

      public UIVehicle(Entity entity, VehicleLocaleKey vehicleKey, VehicleStateLocaleKey stateKey)
      {
        this.entity = entity;
        this.vehicleKey = vehicleKey;
        this.stateKey = stateKey;
      }

      public int CompareTo(VehiclesSection.UIVehicle other)
      {
        int num = this.vehicleKey - other.vehicleKey;
        return num == 0 ? this.stateKey - other.stateKey : num;
      }
    }
  }
}
