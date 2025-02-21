// Decompiled with JetBrains decompiler
// Type: Game.Effects.EffectControlData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Effects
{
  public struct EffectControlData
  {
    [ReadOnly]
    public ComponentLookup<Owner> m_Owners;
    [ReadOnly]
    public ComponentLookup<Transform> m_Transforms;
    [ReadOnly]
    public ComponentLookup<Hidden> m_Hidden;
    [ReadOnly]
    public ComponentLookup<EffectData> m_EffectDatas;
    [ReadOnly]
    public ComponentLookup<LightEffectData> m_LightEffectDatas;
    [ReadOnly]
    public ComponentLookup<Building> m_Buildings;
    [ReadOnly]
    public ComponentLookup<Signature> m_SignatureBuildings;
    [ReadOnly]
    public ComponentLookup<Vehicle> m_Vehicles;
    [ReadOnly]
    public ComponentLookup<Car> m_Cars;
    [ReadOnly]
    public ComponentLookup<Aircraft> m_Aircraft;
    [ReadOnly]
    public ComponentLookup<Watercraft> m_Watercraft;
    [ReadOnly]
    public ComponentLookup<ParkedCar> m_ParkedCars;
    [ReadOnly]
    public ComponentLookup<ParkedTrain> m_ParkedTrains;
    [ReadOnly]
    public ComponentLookup<PrefabRef> m_Prefabs;
    [ReadOnly]
    public ComponentLookup<OnFire> m_OnFires;
    [ReadOnly]
    public ComponentLookup<Game.Vehicles.FireEngine> m_FireEngines;
    [ReadOnly]
    public ComponentLookup<Temp> m_Temps;
    [ReadOnly]
    public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransports;
    [ReadOnly]
    public ComponentLookup<Game.Vehicles.Taxi> m_Taxis;
    [ReadOnly]
    public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransports;
    [ReadOnly]
    public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCars;
    [ReadOnly]
    public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgrades;
    [ReadOnly]
    public ComponentLookup<Extension> m_Extensions;
    [ReadOnly]
    public ComponentLookup<Game.Events.WeatherPhenomenon> m_WeatherPhenomenonData;
    [ReadOnly]
    public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeeds;
    [ReadOnly]
    public ComponentLookup<Destroyed> m_Destroyeds;
    [ReadOnly]
    public ComponentLookup<EarlyDisasterWarningDuration> m_EarlyDisasterWarningDurations;
    [ReadOnly]
    public ComponentLookup<Game.Buildings.WaterPumpingStation> m_WaterPumpingStations;
    [ReadOnly]
    public ComponentLookup<Game.Buildings.SewageOutlet> m_SewageOutlets;
    [ReadOnly]
    public ComponentLookup<Game.Buildings.WaterTower> m_WaterTowers;
    [ReadOnly]
    public ComponentLookup<StreetLight> m_StreetLights;
    [ReadOnly]
    public ComponentLookup<Stopped> m_Stoppeds;
    [ReadOnly]
    public ComponentLookup<Composition> m_Composition;
    [ReadOnly]
    public ComponentLookup<NetCompositionData> m_NetCompositionData;
    [ReadOnly]
    public ComponentLookup<Game.Buildings.ExtractorFacility> m_ExtractorData;
    [ReadOnly]
    public ComponentLookup<Attachment> m_AttachmentData;
    [ReadOnly]
    public BufferLookup<TransformFrame> m_TransformFrames;
    [ReadOnly]
    public BufferLookup<Renter> m_Renter;
    [ReadOnly]
    public EffectFlagSystem.EffectFlagData m_EffectFlagData;
    [ReadOnly]
    public uint m_SimulationFrame;
    [ReadOnly]
    public Entity m_Selected;

    public EffectControlData(SystemBase system)
    {
      this.m_Owners = system.GetComponentLookup<Owner>(true);
      this.m_Temps = system.GetComponentLookup<Temp>(true);
      this.m_Buildings = system.GetComponentLookup<Building>(true);
      this.m_EffectDatas = system.GetComponentLookup<EffectData>(true);
      this.m_LightEffectDatas = system.GetComponentLookup<LightEffectData>(true);
      this.m_Hidden = system.GetComponentLookup<Hidden>(true);
      this.m_ParkedCars = system.GetComponentLookup<ParkedCar>(true);
      this.m_ParkedTrains = system.GetComponentLookup<ParkedTrain>(true);
      this.m_Transforms = system.GetComponentLookup<Transform>(true);
      this.m_Vehicles = system.GetComponentLookup<Vehicle>(true);
      this.m_Cars = system.GetComponentLookup<Car>(true);
      this.m_Aircraft = system.GetComponentLookup<Aircraft>(true);
      this.m_Watercraft = system.GetComponentLookup<Watercraft>(true);
      this.m_Prefabs = system.GetComponentLookup<PrefabRef>(true);
      this.m_OnFires = system.GetComponentLookup<OnFire>(true);
      this.m_FireEngines = system.GetComponentLookup<Game.Vehicles.FireEngine>(true);
      this.m_PublicTransports = system.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
      this.m_ServiceUpgrades = system.GetComponentLookup<Game.Buildings.ServiceUpgrade>(true);
      this.m_Extensions = system.GetComponentLookup<Extension>(true);
      this.m_WeatherPhenomenonData = system.GetComponentLookup<Game.Events.WeatherPhenomenon>(true);
      this.m_PseudoRandomSeeds = system.GetComponentLookup<PseudoRandomSeed>(true);
      this.m_Destroyeds = system.GetComponentLookup<Destroyed>(true);
      this.m_CargoTransports = system.GetComponentLookup<Game.Vehicles.CargoTransport>(true);
      this.m_PersonalCars = system.GetComponentLookup<Game.Vehicles.PersonalCar>(true);
      this.m_Taxis = system.GetComponentLookup<Game.Vehicles.Taxi>(true);
      this.m_EarlyDisasterWarningDurations = system.GetComponentLookup<EarlyDisasterWarningDuration>(true);
      this.m_WaterPumpingStations = system.GetComponentLookup<Game.Buildings.WaterPumpingStation>(true);
      this.m_SewageOutlets = system.GetComponentLookup<Game.Buildings.SewageOutlet>(true);
      this.m_WaterTowers = system.GetComponentLookup<Game.Buildings.WaterTower>(true);
      this.m_StreetLights = system.GetComponentLookup<StreetLight>(true);
      this.m_Stoppeds = system.GetComponentLookup<Stopped>(true);
      this.m_Composition = system.GetComponentLookup<Composition>(true);
      this.m_NetCompositionData = system.GetComponentLookup<NetCompositionData>(true);
      this.m_TransformFrames = system.GetBufferLookup<TransformFrame>(true);
      this.m_Renter = system.GetBufferLookup<Renter>(true);
      this.m_SignatureBuildings = system.GetComponentLookup<Signature>(true);
      this.m_ExtractorData = system.GetComponentLookup<Game.Buildings.ExtractorFacility>(true);
      this.m_AttachmentData = system.GetComponentLookup<Attachment>(true);
      this.m_EffectFlagData = new EffectFlagSystem.EffectFlagData();
      this.m_SimulationFrame = 0U;
      this.m_Selected = new Entity();
    }

    public void Update(
      SystemBase system,
      EffectFlagSystem.EffectFlagData effectFlagData,
      uint simulationFrame,
      Entity selected)
    {
      this.m_Owners.Update(system);
      this.m_Temps.Update(system);
      this.m_Buildings.Update(system);
      this.m_EffectDatas.Update(system);
      this.m_LightEffectDatas.Update(system);
      this.m_Hidden.Update(system);
      this.m_ParkedCars.Update(system);
      this.m_ParkedTrains.Update(system);
      this.m_Transforms.Update(system);
      this.m_Vehicles.Update(system);
      this.m_Cars.Update(system);
      this.m_Aircraft.Update(system);
      this.m_Watercraft.Update(system);
      this.m_Prefabs.Update(system);
      this.m_OnFires.Update(system);
      this.m_FireEngines.Update(system);
      this.m_PublicTransports.Update(system);
      this.m_ServiceUpgrades.Update(system);
      this.m_Extensions.Update(system);
      this.m_WeatherPhenomenonData.Update(system);
      this.m_PseudoRandomSeeds.Update(system);
      this.m_Destroyeds.Update(system);
      this.m_CargoTransports.Update(system);
      this.m_PersonalCars.Update(system);
      this.m_Taxis.Update(system);
      this.m_EarlyDisasterWarningDurations.Update(system);
      this.m_WaterPumpingStations.Update(system);
      this.m_SewageOutlets.Update(system);
      this.m_WaterTowers.Update(system);
      this.m_StreetLights.Update(system);
      this.m_Stoppeds.Update(system);
      this.m_Composition.Update(system);
      this.m_NetCompositionData.Update(system);
      this.m_TransformFrames.Update(system);
      this.m_Renter.Update(system);
      this.m_SignatureBuildings.Update(system);
      this.m_ExtractorData.Update(system);
      this.m_AttachmentData.Update(system);
      this.m_EffectFlagData = effectFlagData;
      this.m_SimulationFrame = simulationFrame;
      this.m_Selected = selected;
    }

    private bool CheckTrigger(
      Entity owner,
      Entity buildingOwner,
      Entity topOwner,
      EffectConditionFlags flag,
      bool forbidden)
    {
      switch (flag)
      {
        case EffectConditionFlags.Emergency:
          Car componentData1;
          if (this.m_Cars.TryGetComponent(topOwner, out componentData1))
            return (componentData1.m_Flags & CarFlags.Emergency) > (CarFlags) 0;
          Aircraft componentData2;
          return this.m_Aircraft.TryGetComponent(topOwner, out componentData2) && (componentData2.m_Flags & AircraftFlags.Emergency) > (AircraftFlags) 0;
        case EffectConditionFlags.Parked:
          return !this.m_Vehicles.HasComponent(topOwner) || this.m_ParkedCars.HasComponent(topOwner) || this.m_ParkedTrains.HasComponent(topOwner);
        case EffectConditionFlags.Operational:
          Building componentData3;
          Extension componentData4;
          if (this.m_Buildings.TryGetComponent(buildingOwner, out componentData3) && (BuildingUtils.CheckOption(componentData3, BuildingOption.Inactive) || this.m_OnFires.HasComponent(buildingOwner)) || this.m_Extensions.TryGetComponent(owner, out componentData4) && (componentData4.m_Flags & ExtensionFlags.Disabled) != ExtensionFlags.None || this.m_Destroyeds.HasComponent(owner))
            return false;
          DynamicBuffer<Renter> bufferData1;
          Building componentData5;
          return this.m_SignatureBuildings.HasComponent(buildingOwner) ? this.m_Renter.TryGetBuffer(buildingOwner, out bufferData1) && bufferData1.Length > 0 : !this.m_Buildings.TryGetComponent(topOwner, out componentData5) || (componentData5.m_Flags & Game.Buildings.BuildingFlags.LowEfficiency) == Game.Buildings.BuildingFlags.None;
        case EffectConditionFlags.OnFire:
          return this.m_OnFires.HasComponent(buildingOwner);
        case EffectConditionFlags.Extinguishing:
          return this.m_FireEngines.HasComponent(topOwner) && (this.m_FireEngines[topOwner].m_State & FireEngineFlags.Extinguishing) > (FireEngineFlags) 0;
        case EffectConditionFlags.TakingOff:
          DynamicBuffer<TransformFrame> bufferData2;
          if (!this.m_TransformFrames.TryGetBuffer(topOwner, out bufferData2))
            return false;
          if (forbidden)
          {
            for (int index = 0; index < bufferData2.Length; ++index)
            {
              if ((bufferData2[index].m_Flags & TransformFlags.TakingOff) == (TransformFlags) 0)
                return false;
            }
            return true;
          }
          for (int index = 0; index < bufferData2.Length; ++index)
          {
            if ((bufferData2[index].m_Flags & TransformFlags.TakingOff) != (TransformFlags) 0)
              return true;
          }
          return false;
        case EffectConditionFlags.Landing:
          DynamicBuffer<TransformFrame> bufferData3;
          if (!this.m_TransformFrames.TryGetBuffer(topOwner, out bufferData3))
            return false;
          if (forbidden)
          {
            for (int index = 0; index < bufferData3.Length; ++index)
            {
              if ((bufferData3[index].m_Flags & TransformFlags.Landing) == (TransformFlags) 0)
                return false;
            }
            return true;
          }
          for (int index = 0; index < bufferData3.Length; ++index)
          {
            if ((bufferData3[index].m_Flags & TransformFlags.Landing) != (TransformFlags) 0)
              return true;
          }
          return false;
        case EffectConditionFlags.Flying:
          DynamicBuffer<TransformFrame> bufferData4;
          if (!this.m_TransformFrames.TryGetBuffer(topOwner, out bufferData4))
            return false;
          if (forbidden)
          {
            for (int index = 0; index < bufferData4.Length; ++index)
            {
              if ((bufferData4[index].m_Flags & TransformFlags.Flying) == (TransformFlags) 0)
                return false;
            }
            return true;
          }
          for (int index = 0; index < bufferData4.Length; ++index)
          {
            if ((bufferData4[index].m_Flags & TransformFlags.Flying) != (TransformFlags) 0)
              return true;
          }
          return false;
        case EffectConditionFlags.Stopped:
          return this.m_Stoppeds.HasComponent(topOwner);
        case EffectConditionFlags.Processing:
          bool flag1 = false;
          if (this.m_WaterPumpingStations.HasComponent(topOwner))
            flag1 = this.m_WaterPumpingStations[topOwner].m_LastProduction != 0;
          if (this.m_SewageOutlets.HasComponent(topOwner))
            flag1 = flag1 || this.m_SewageOutlets[topOwner].m_LastProcessed != 0;
          if (this.m_WaterTowers.HasComponent(topOwner))
            flag1 = flag1 || this.m_WaterTowers[topOwner].m_LastStoredWater != this.m_WaterTowers[topOwner].m_StoredWater;
          return flag1;
        case EffectConditionFlags.Boarding:
          if (this.m_PublicTransports.HasComponent(topOwner))
            return (this.m_PublicTransports[topOwner].m_State & PublicTransportFlags.Boarding) > (PublicTransportFlags) 0;
          if (this.m_Taxis.HasComponent(topOwner))
            return (this.m_Taxis[topOwner].m_State & TaxiFlags.Boarding) > (TaxiFlags) 0;
          if (this.m_CargoTransports.HasComponent(topOwner))
            return (this.m_CargoTransports[topOwner].m_State & CargoTransportFlags.Boarding) > (CargoTransportFlags) 0;
          return this.m_PersonalCars.HasComponent(topOwner) && (this.m_PersonalCars[topOwner].m_State & PersonalCarFlags.Boarding) > (PersonalCarFlags) 0;
        case EffectConditionFlags.Disaster:
          return this.m_EarlyDisasterWarningDurations.HasComponent(topOwner) && this.m_SimulationFrame < this.m_EarlyDisasterWarningDurations[topOwner].m_EndFrame;
        case EffectConditionFlags.Occurring:
          return this.m_WeatherPhenomenonData.HasComponent(topOwner) && (double) this.m_WeatherPhenomenonData[topOwner].m_Intensity != 0.0;
        case EffectConditionFlags.Night:
        case EffectConditionFlags.Cold:
          Random random = this.GetRandom(topOwner);
          return EffectFlagSystem.IsEnabled(flag, random, this.m_EffectFlagData, this.m_SimulationFrame);
        case EffectConditionFlags.LightsOff:
          StreetLight componentData6;
          if (this.m_StreetLights.TryGetComponent(topOwner, out componentData6))
            return (componentData6.m_State & StreetLightState.TurnedOff) != 0;
          Watercraft componentData7;
          return this.m_Watercraft.TryGetComponent(topOwner, out componentData7) && (componentData7.m_Flags & WatercraftFlags.LightsOff) > (WatercraftFlags) 0;
        case EffectConditionFlags.MainLights:
          DynamicBuffer<TransformFrame> bufferData5;
          if (!this.m_TransformFrames.TryGetBuffer(topOwner, out bufferData5))
            return false;
          if (forbidden)
          {
            for (int index = 0; index < bufferData5.Length; ++index)
            {
              if ((bufferData5[index].m_Flags & TransformFlags.MainLights) == (TransformFlags) 0)
                return false;
            }
            return true;
          }
          for (int index = 0; index < bufferData5.Length; ++index)
          {
            if ((bufferData5[index].m_Flags & TransformFlags.MainLights) != (TransformFlags) 0)
              return true;
          }
          return false;
        case EffectConditionFlags.ExtraLights:
          DynamicBuffer<TransformFrame> bufferData6;
          if (!this.m_TransformFrames.TryGetBuffer(topOwner, out bufferData6))
            return false;
          if (forbidden)
          {
            for (int index = 0; index < bufferData6.Length; ++index)
            {
              if ((bufferData6[index].m_Flags & TransformFlags.ExtraLights) == (TransformFlags) 0)
                return false;
            }
            return true;
          }
          for (int index = 0; index < bufferData6.Length; ++index)
          {
            if ((bufferData6[index].m_Flags & TransformFlags.ExtraLights) != (TransformFlags) 0)
              return true;
          }
          return false;
        case EffectConditionFlags.WarningLights:
          DynamicBuffer<TransformFrame> bufferData7;
          if (this.m_TransformFrames.TryGetBuffer(topOwner, out bufferData7))
          {
            if (forbidden)
            {
              for (int index = 0; index < bufferData7.Length; ++index)
              {
                if ((bufferData7[index].m_Flags & TransformFlags.WarningLights) == (TransformFlags) 0)
                  return false;
              }
              return true;
            }
            for (int index = 0; index < bufferData7.Length; ++index)
            {
              if ((bufferData7[index].m_Flags & TransformFlags.WarningLights) != (TransformFlags) 0)
                return true;
            }
            return false;
          }
          Car componentData8;
          return this.m_Cars.TryGetComponent(topOwner, out componentData8) && (componentData8.m_Flags & (CarFlags.Emergency | CarFlags.Warning)) > (CarFlags) 0;
        case EffectConditionFlags.WorkLights:
          DynamicBuffer<TransformFrame> bufferData8;
          if (this.m_TransformFrames.TryGetBuffer(topOwner, out bufferData8))
          {
            if (forbidden)
            {
              for (int index = 0; index < bufferData8.Length; ++index)
              {
                if ((bufferData8[index].m_Flags & TransformFlags.WorkLights) == (TransformFlags) 0)
                  return false;
              }
              return true;
            }
            for (int index = 0; index < bufferData8.Length; ++index)
            {
              if ((bufferData8[index].m_Flags & TransformFlags.WorkLights) != (TransformFlags) 0)
                return true;
            }
            return false;
          }
          Car componentData9;
          return this.m_Cars.TryGetComponent(topOwner, out componentData9) && (componentData9.m_Flags & (CarFlags.Sign | CarFlags.Working)) > (CarFlags) 0;
        case EffectConditionFlags.Spillway:
          Composition componentData10;
          NetCompositionData componentData11;
          return this.m_Composition.TryGetComponent(topOwner, out componentData10) && this.m_NetCompositionData.TryGetComponent(componentData10.m_Edge, out componentData11) && (componentData11.m_Flags.m_General & CompositionFlags.General.Spillway) > (CompositionFlags.General) 0;
        case EffectConditionFlags.Collapsing:
          Destroyed componentData12;
          return this.m_Destroyeds.TryGetComponent(owner, out componentData12) && (double) componentData12.m_Cleared < 0.0;
        default:
          return false;
      }
    }

    private bool CheckTriggers(
      Entity owner,
      Entity buildingOwner,
      Entity topOwner,
      EffectCondition condition)
    {
      EffectConditionFlags flag1 = EffectConditionFlags.Emergency;
      while (true)
      {
        bool flag2 = (condition.m_RequiredFlags & flag1) != 0;
        bool forbidden = (condition.m_ForbiddenFlags & flag1) != 0;
        if (flag2 | forbidden)
        {
          bool flag3 = this.CheckTrigger(owner, buildingOwner, topOwner, flag1, forbidden);
          if (flag2 && !flag3 || forbidden & flag3)
            break;
        }
        if (flag1 != EffectConditionFlags.Collapsing)
          flag1 = (EffectConditionFlags) ((int) flag1 << 1);
        else
          goto label_6;
      }
      return true;
label_6:
      return false;
    }

    private bool CheckConditions(
      Entity owner,
      Entity buildingOwner,
      Entity topOwner,
      Entity effect)
    {
      EffectData componentData;
      return !this.m_EffectDatas.TryGetComponent(effect, out componentData) || !this.CheckTriggers(owner, buildingOwner, topOwner, componentData.m_Flags);
    }

    private Random GetRandom(Entity owner)
    {
      PseudoRandomSeed componentData1;
      if (this.m_PseudoRandomSeeds.TryGetComponent(owner, out componentData1))
        return componentData1.GetRandom((uint) PseudoRandomSeed.kEffectCondition);
      Transform componentData2;
      return this.m_Transforms.TryGetComponent(owner, out componentData2) ? Random.CreateFromIndex((uint) math.dot(new float3(67f, 83f, 97f), componentData2.m_Position)) : Random.CreateFromIndex((uint) owner.Index);
    }

    public bool ShouldBeEnabled(
      Entity owner,
      Entity prefab,
      bool checkEnabled,
      bool isEditorContainer)
    {
      if (isEditorContainer)
      {
        if (this.m_Hidden.HasComponent(owner))
          return false;
        if (!this.m_LightEffectDatas.HasComponent(prefab))
        {
          Temp componentData1;
          if (this.m_Temps.TryGetComponent(owner, out componentData1))
          {
            Owner componentData2;
            Temp componentData3;
            if ((this.m_Selected == Entity.Null || componentData1.m_Original != this.m_Selected) && ((componentData1.m_Flags & TempFlags.Essential) == (TempFlags) 0 || this.m_Owners.TryGetComponent(owner, out componentData2) && this.m_Temps.TryGetComponent(componentData2.m_Owner, out componentData3) && (componentData3.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Duplicate)) != (TempFlags) 0))
              return false;
          }
          else if (owner != this.m_Selected)
            return false;
        }
      }
      else
      {
        if (this.m_LightEffectDatas.HasComponent(prefab))
        {
          if (this.m_Hidden.HasComponent(owner))
            return false;
        }
        else if (this.m_Temps.HasComponent(owner))
          return false;
        if (checkEnabled)
        {
          Entity buildingOwner;
          Entity realOwner = this.GetRealOwner(owner, out buildingOwner);
          if (!this.CheckConditions(owner, buildingOwner, realOwner, prefab))
            return false;
        }
      }
      return true;
    }

    private Entity GetRealOwner(Entity owner, out Entity buildingOwner)
    {
      Temp componentData1;
      if (this.m_Temps.TryGetComponent(owner, out componentData1))
      {
        buildingOwner = componentData1.m_Original != Entity.Null ? componentData1.m_Original : owner;
        return buildingOwner;
      }
      Owner componentData2;
      if (this.m_ServiceUpgrades.HasComponent(owner) && this.m_Owners.TryGetComponent(owner, out componentData2))
      {
        buildingOwner = this.m_Buildings.HasComponent(owner) ? owner : componentData2.m_Owner;
        return componentData2.m_Owner;
      }
      Owner componentData3;
      if (this.m_ExtractorData.HasComponent(owner) && this.m_Owners.TryGetComponent(owner, out componentData3))
      {
        Entity entity = componentData3.m_Owner;
        Owner componentData4;
        while (this.m_Owners.TryGetComponent(entity, out componentData4))
          entity = componentData4.m_Owner;
        Attachment componentData5;
        if (this.m_AttachmentData.TryGetComponent(entity, out componentData5))
          entity = componentData5.m_Attached;
        buildingOwner = entity;
        return buildingOwner;
      }
      buildingOwner = owner;
      return owner;
    }
  }
}
