// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.VehicleUIUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Events;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using Game.Vehicles;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.InGame
{
  public static class VehicleUIUtils
  {
    public static int GetAvailableVehicles(Entity vehicleOwnerEntity, EntityManager entityManager)
    {
      float efficiency = 1f;
      DynamicBuffer<Efficiency> buffer;
      if (entityManager.TryGetBuffer<Efficiency>(vehicleOwnerEntity, true, out buffer))
        efficiency = Mathf.Min(BuildingUtils.GetEfficiency(buffer), 1f);
      int availableVehicles = 0;
      PrefabRef component1;
      if (entityManager.TryGetComponent<PrefabRef>(vehicleOwnerEntity, out component1))
      {
        GarbageFacilityData data1;
        if (UpgradeUtils.TryGetCombinedComponent<GarbageFacilityData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data1))
        {
          availableVehicles += BuildingUtils.GetVehicleCapacity(efficiency, data1.m_VehicleCapacity);
        }
        else
        {
          DeathcareFacilityData data2;
          if (UpgradeUtils.TryGetCombinedComponent<DeathcareFacilityData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data2))
          {
            availableVehicles += BuildingUtils.GetVehicleCapacity(efficiency, data2.m_HearseCapacity);
          }
          else
          {
            EmergencyShelterData data3;
            if (UpgradeUtils.TryGetCombinedComponent<EmergencyShelterData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data3))
            {
              availableVehicles += BuildingUtils.GetVehicleCapacity(efficiency, data3.m_VehicleCapacity);
            }
            else
            {
              FireStationData data4;
              if (UpgradeUtils.TryGetCombinedComponent<FireStationData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data4))
              {
                availableVehicles = availableVehicles + BuildingUtils.GetVehicleCapacity(efficiency, data4.m_FireEngineCapacity) + BuildingUtils.GetVehicleCapacity(efficiency, data4.m_FireHelicopterCapacity);
              }
              else
              {
                HospitalData data5;
                if (UpgradeUtils.TryGetCombinedComponent<HospitalData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data5))
                {
                  availableVehicles = availableVehicles + BuildingUtils.GetVehicleCapacity(efficiency, data5.m_AmbulanceCapacity) + BuildingUtils.GetVehicleCapacity(efficiency, data5.m_MedicalHelicopterCapacity);
                }
                else
                {
                  MaintenanceDepotData data6;
                  if (UpgradeUtils.TryGetCombinedComponent<MaintenanceDepotData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data6))
                  {
                    availableVehicles += BuildingUtils.GetVehicleCapacity(efficiency, data6.m_VehicleCapacity);
                  }
                  else
                  {
                    PoliceStationData data7;
                    if (UpgradeUtils.TryGetCombinedComponent<PoliceStationData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data7))
                    {
                      availableVehicles = availableVehicles + BuildingUtils.GetVehicleCapacity(efficiency, data7.m_PatrolCarCapacity) + BuildingUtils.GetVehicleCapacity(efficiency, data7.m_PoliceHelicopterCapacity);
                    }
                    else
                    {
                      PrisonData data8;
                      if (UpgradeUtils.TryGetCombinedComponent<PrisonData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data8))
                      {
                        availableVehicles += BuildingUtils.GetVehicleCapacity(efficiency, data8.m_PrisonVanCapacity);
                      }
                      else
                      {
                        TransportDepotData data9;
                        if (UpgradeUtils.TryGetCombinedComponent<TransportDepotData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data9))
                        {
                          availableVehicles += BuildingUtils.GetVehicleCapacity(efficiency, data9.m_VehicleCapacity);
                        }
                        else
                        {
                          PostFacilityData data10;
                          if (UpgradeUtils.TryGetCombinedComponent<PostFacilityData>(entityManager, vehicleOwnerEntity, component1.m_Prefab, out data10))
                          {
                            availableVehicles = availableVehicles + BuildingUtils.GetVehicleCapacity(efficiency, data10.m_PostVanCapacity) + BuildingUtils.GetVehicleCapacity(efficiency, data10.m_PostTruckCapacity);
                          }
                          else
                          {
                            TransportCompanyData component2;
                            if (entityManager.TryGetComponent<TransportCompanyData>(component1.m_Prefab, out component2))
                              availableVehicles += BuildingUtils.GetVehicleCapacity(efficiency, component2.m_MaxTransports);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      return availableVehicles;
    }

    public static Entity GetDestination(EntityManager entityManager, Entity vehicleEntity)
    {
      Entity entity = Entity.Null;
      Game.Common.Target component1;
      if (entityManager.TryGetComponent<Game.Common.Target>(vehicleEntity, out component1))
      {
        entity = component1.m_Target;
        Connected component2;
        if (entityManager.TryGetComponent<Connected>(entity, out component2))
          entity = component2.m_Connected;
        if (entityManager.HasComponent<Game.Objects.OutsideConnection>(entity) || entityManager.HasComponent<Vehicle>(entity))
          return entity;
        PropertyRenter component3;
        if (entityManager.HasComponent<CompanyData>(entity) && entityManager.TryGetComponent<PropertyRenter>(entity, out component3))
          return component3.m_Property;
        Owner component4;
        if (entityManager.TryGetComponent<Owner>(entity, out component4))
          entity = component4.m_Owner;
        Game.Creatures.Resident component5;
        if (entityManager.TryGetComponent<Game.Creatures.Resident>(entity, out component5))
          entity = component5.m_Citizen;
        Waypoint component6;
        DynamicBuffer<RouteWaypoint> buffer;
        if (!entityManager.HasComponent<Connected>(component1.m_Target) && entityManager.TryGetComponent<Waypoint>(component1.m_Target, out component6) && entityManager.TryGetBuffer<RouteWaypoint>(entity, true, out buffer))
        {
          int index1 = component6.m_Index + 1;
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            int a = index1 + index2;
            index1 = math.select(a, 0, a >= buffer.Length);
            if (entityManager.TryGetComponent<Connected>(buffer[index1].m_Waypoint, out component2))
            {
              entity = component2.m_Connected;
              break;
            }
          }
        }
      }
      return entity;
    }

    public static VehicleStateLocaleKey GetStateKey(Entity entity, EntityManager entityManager)
    {
      Game.Vehicles.PublicTransport component1;
      if (entityManager.TryGetComponent<Game.Vehicles.PublicTransport>(entity, out component1))
        return VehicleUIUtils.GetStateKey(entity, component1, entityManager);
      Game.Vehicles.PersonalCar component2;
      if (entityManager.TryGetComponent<Game.Vehicles.PersonalCar>(entity, out component2))
        return VehicleUIUtils.GetStateKey(entity, component2, entityManager);
      Game.Vehicles.PostVan component3;
      if (entityManager.TryGetComponent<Game.Vehicles.PostVan>(entity, out component3))
        return VehicleUIUtils.GetStateKey(entity, component3, entityManager);
      Game.Vehicles.PoliceCar component4;
      if (entityManager.TryGetComponent<Game.Vehicles.PoliceCar>(entity, out component4))
      {
        DynamicBuffer<ServiceDispatch> buffer;
        entityManager.TryGetBuffer<ServiceDispatch>(entity, true, out buffer);
        return VehicleUIUtils.GetStateKey(entity, component4, buffer, entityManager);
      }
      Game.Vehicles.MaintenanceVehicle component5;
      if (entityManager.TryGetComponent<Game.Vehicles.MaintenanceVehicle>(entity, out component5))
        return VehicleUIUtils.GetStateKey(entity, component5, entityManager);
      Game.Vehicles.Ambulance component6;
      if (entityManager.TryGetComponent<Game.Vehicles.Ambulance>(entity, out component6))
        return VehicleUIUtils.GetStateKey(entity, component6, entityManager);
      Game.Vehicles.GarbageTruck component7;
      if (entityManager.TryGetComponent<Game.Vehicles.GarbageTruck>(entity, out component7))
        return VehicleUIUtils.GetStateKey(entity, component7, entityManager);
      Game.Vehicles.FireEngine component8;
      if (entityManager.TryGetComponent<Game.Vehicles.FireEngine>(entity, out component8))
      {
        DynamicBuffer<ServiceDispatch> buffer;
        entityManager.TryGetBuffer<ServiceDispatch>(entity, true, out buffer);
        return VehicleUIUtils.GetStateKey(entity, component8, buffer, entityManager);
      }
      Game.Vehicles.DeliveryTruck component9;
      if (entityManager.TryGetComponent<Game.Vehicles.DeliveryTruck>(entity, out component9))
        return VehicleUIUtils.GetStateKey(entity, component9, entityManager);
      Game.Vehicles.Hearse component10;
      if (entityManager.TryGetComponent<Game.Vehicles.Hearse>(entity, out component10))
        return VehicleUIUtils.GetStateKey(entity, component10, entityManager);
      Game.Vehicles.CargoTransport component11;
      if (entityManager.TryGetComponent<Game.Vehicles.CargoTransport>(entity, out component11))
        return VehicleUIUtils.GetStateKey(entity, component11, entityManager);
      Game.Vehicles.Taxi component12;
      return entityManager.TryGetComponent<Game.Vehicles.Taxi>(entity, out component12) ? VehicleUIUtils.GetStateKey(entity, component12, entityManager) : VehicleStateLocaleKey.Unknown;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.PublicTransport publicTransportVehicle,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity) || entityManager.HasComponent<ParkedTrain>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((publicTransportVehicle.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
        return VehicleStateLocaleKey.Returning;
      if ((publicTransportVehicle.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        return VehicleStateLocaleKey.Boarding;
      return (publicTransportVehicle.m_State & PublicTransportFlags.Evacuating) == (PublicTransportFlags) 0 ? VehicleStateLocaleKey.EnRoute : VehicleStateLocaleKey.Evacuating;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.PersonalCar personalCar,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((personalCar.m_State & PersonalCarFlags.Boarding) != (PersonalCarFlags) 0)
        return VehicleStateLocaleKey.Boarding;
      if ((personalCar.m_State & PersonalCarFlags.Disembarking) != (PersonalCarFlags) 0)
        return VehicleStateLocaleKey.Disembarking;
      return (personalCar.m_State & PersonalCarFlags.Transporting) != (PersonalCarFlags) 0 ? VehicleStateLocaleKey.Transporting : VehicleStateLocaleKey.EnRoute;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.PostVan postVan,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((postVan.m_State & PostVanFlags.Delivering) != (PostVanFlags) 0)
        return VehicleStateLocaleKey.Delivering;
      if ((postVan.m_State & PostVanFlags.Collecting) != (PostVanFlags) 0)
        return VehicleStateLocaleKey.Collecting;
      return (postVan.m_State & PostVanFlags.Returning) == (PostVanFlags) 0 ? VehicleStateLocaleKey.Unknown : VehicleStateLocaleKey.Returning;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.PoliceCar policeCar,
      DynamicBuffer<ServiceDispatch> dispatches,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((policeCar.m_State & PoliceCarFlags.Returning) != (PoliceCarFlags) 0)
        return VehicleStateLocaleKey.Returning;
      if ((policeCar.m_State & PoliceCarFlags.AccidentTarget) == (PoliceCarFlags) 0 || policeCar.m_RequestCount <= 0 || !dispatches.IsCreated || dispatches.Length <= 0)
        return VehicleStateLocaleKey.Patrolling;
      if ((policeCar.m_State & PoliceCarFlags.AtTarget) != (PoliceCarFlags) 0)
      {
        PoliceEmergencyRequest component1;
        AccidentSite component2;
        if (entityManager.TryGetComponent<PoliceEmergencyRequest>(dispatches[0].m_Request, out component1) && entityManager.TryGetComponent<AccidentSite>(component1.m_Site, out component2))
        {
          if ((component2.m_Flags & AccidentSiteFlags.TrafficAccident) != (AccidentSiteFlags) 0)
            return VehicleStateLocaleKey.AccidentSite;
          if ((component2.m_Flags & AccidentSiteFlags.CrimeScene) != (AccidentSiteFlags) 0)
            return VehicleStateLocaleKey.CrimeScene;
        }
      }
      else if (entityManager.HasComponent<PoliceEmergencyRequest>(dispatches[0].m_Request))
        return VehicleStateLocaleKey.Dispatched;
      return VehicleStateLocaleKey.Unknown;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      Game.Common.Target component;
      return (maintenanceVehicle.m_State & MaintenanceVehicleFlags.TransformTarget) != (MaintenanceVehicleFlags) 0 ? (entityManager.TryGetComponent<Game.Common.Target>(entity, out component) && entityManager.HasComponent<InvolvedInAccident>(component.m_Target) ? VehicleStateLocaleKey.AccidentSite : VehicleStateLocaleKey.Dispatched) : ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0 ? VehicleStateLocaleKey.Working : VehicleStateLocaleKey.Returning);
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.Ambulance ambulance,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((ambulance.m_State & AmbulanceFlags.Returning) != (AmbulanceFlags) 0)
        return VehicleStateLocaleKey.Returning;
      return (ambulance.m_State & AmbulanceFlags.Transporting) == (AmbulanceFlags) 0 ? VehicleStateLocaleKey.Dispatched : VehicleStateLocaleKey.Transporting;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.GarbageTruck garbageTruck,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      return (garbageTruck.m_State & GarbageTruckFlags.Returning) == (GarbageTruckFlags) 0 ? VehicleStateLocaleKey.Collecting : VehicleStateLocaleKey.Returning;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.FireEngine fireEngine,
      DynamicBuffer<ServiceDispatch> dispatches,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if ((fireEngine.m_State & FireEngineFlags.Extinguishing) != (FireEngineFlags) 0)
        return VehicleStateLocaleKey.Extinguishing;
      if ((fireEngine.m_State & FireEngineFlags.Rescueing) != (FireEngineFlags) 0)
        return VehicleStateLocaleKey.Rescuing;
      if (fireEngine.m_RequestCount > 0 && dispatches.Length > 0)
        return VehicleStateLocaleKey.Dispatched;
      return !entityManager.HasComponent<ParkedCar>(entity) ? VehicleStateLocaleKey.Returning : VehicleStateLocaleKey.Parked;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.DeliveryTruck truck,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((truck.m_State & DeliveryTruckFlags.Returning) != (DeliveryTruckFlags) 0)
        return VehicleStateLocaleKey.Returning;
      if ((truck.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
        return VehicleStateLocaleKey.Buying;
      if ((truck.m_State & DeliveryTruckFlags.StorageTransfer) != (DeliveryTruckFlags) 0)
      {
        Owner component1;
        if (entityManager.TryGetComponent<Owner>(entity, out component1) && entityManager.HasComponent<Game.Objects.OutsideConnection>(component1.m_Owner))
          return VehicleStateLocaleKey.Importing;
        Game.Common.Target component2;
        if (entityManager.TryGetComponent<Game.Common.Target>(entity, out component2) && entityManager.HasComponent<Game.Objects.OutsideConnection>(component2.m_Target))
          return VehicleStateLocaleKey.Exporting;
      }
      return (truck.m_State & DeliveryTruckFlags.Delivering) == (DeliveryTruckFlags) 0 ? VehicleStateLocaleKey.Transporting : VehicleStateLocaleKey.Delivering;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.Hearse hearse,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((hearse.m_State & HearseFlags.Returning) != (HearseFlags) 0)
        return VehicleStateLocaleKey.Returning;
      return (hearse.m_State & HearseFlags.Transporting) == (HearseFlags) 0 ? VehicleStateLocaleKey.Gathering : VehicleStateLocaleKey.Conveying;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.CargoTransport cargoTransport,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity) || entityManager.HasComponent<ParkedTrain>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((cargoTransport.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0)
        return VehicleStateLocaleKey.Returning;
      return (cargoTransport.m_State & CargoTransportFlags.Boarding) == (CargoTransportFlags) 0 ? VehicleStateLocaleKey.EnRoute : VehicleStateLocaleKey.Loading;
    }

    public static VehicleStateLocaleKey GetStateKey(
      Entity entity,
      Game.Vehicles.Taxi taxi,
      EntityManager entityManager)
    {
      if (entityManager.HasComponent<InvolvedInAccident>(entity))
        return VehicleStateLocaleKey.InvolvedInAccident;
      if (entityManager.HasComponent<ParkedCar>(entity))
        return VehicleStateLocaleKey.Parked;
      if ((taxi.m_State & TaxiFlags.Returning) != (TaxiFlags) 0)
        return VehicleStateLocaleKey.Returning;
      if ((taxi.m_State & TaxiFlags.Dispatched) != (TaxiFlags) 0)
        return VehicleStateLocaleKey.Dispatched;
      if ((taxi.m_State & TaxiFlags.Boarding) != (TaxiFlags) 0)
        return VehicleStateLocaleKey.Boarding;
      return (taxi.m_State & TaxiFlags.Transporting) != (TaxiFlags) 0 ? VehicleStateLocaleKey.Transporting : VehicleStateLocaleKey.EnRoute;
    }

    public static VehicleLocaleKey GetPoliceVehicleLocaleKey(PolicePurpose purposeMask)
    {
      if ((purposeMask & PolicePurpose.Intelligence) != (PolicePurpose) 0)
        return VehicleLocaleKey.PoliceIntelligenceCar;
      return (purposeMask & PolicePurpose.Patrol) != (PolicePurpose) 0 ? VehicleLocaleKey.PolicePatrolCar : VehicleLocaleKey.Vehicle;
    }

    public readonly struct EntityWrapper
    {
      public Entity entity { get; }

      public EntityWrapper(Entity entity) => this.entity = entity;

      public void Write(IJsonWriter writer, NameSystem nameSystem)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("entity");
        if (this.entity == Entity.Null)
          writer.WriteNull();
        else
          writer.Write(this.entity);
        writer.PropertyName("name");
        if (this.entity == Entity.Null)
        {
          writer.WriteNull();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          nameSystem.BindName(writer, this.entity);
        }
        writer.TypeEnd();
      }
    }
  }
}
