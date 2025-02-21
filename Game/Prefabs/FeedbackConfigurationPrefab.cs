// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FeedbackConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Common;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {})]
  public class FeedbackConfigurationPrefab : PrefabBase
  {
    public NotificationIconPrefab m_HappyFaceNotification;
    public NotificationIconPrefab m_SadFaceNotification;
    public float m_GarbageProducerGarbageFactor = 1f / 1000f;
    public float m_GarbageVehicleFactor = 0.1f;
    public float m_HospitalAmbulanceFactor = 0.01f;
    public float m_HospitalHelicopterFactor = 0.1f;
    public float m_HospitalCapacityFactor = 1f / 1000f;
    public float m_DeathcareHearseFactor = 0.1f;
    public float m_DeathcareCapacityFactor = 0.0005f;
    public float m_DeathcareProcessingFactor = 0.05f;
    public float m_ElectricityConsumptionFactor = 0.01f;
    public float m_ElectricityProductionFactor = 0.0001f;
    public float m_TransformerRadius = 500f;
    public float m_WaterConsumptionFactor = 0.01f;
    public float m_WaterCapacityFactor = 0.0001f;
    public float m_WaterConsumerSewageFactor = 1f / 500f;
    public float m_SewageCapacityFactor = 0.0001f;
    public float m_TransportVehicleCapacityFactor = 0.01f;
    public float m_TransportDispatchCenterFactor = 0.005f;
    public float m_TransportStationRange = 1000f;
    public float m_TransportStopRange = 250f;
    public float m_MailProducerMailFactor = 0.01f;
    public float m_PostFacilityVanFactor = 0.01f;
    public float m_PostFacilityTruckFactor = 0.1f;
    public float m_PostFacilityCapacityFactor = 0.0001f;
    public float m_PostFacilityProcessingFactor = 1f / 1000f;
    public float m_TelecomCapacityFactor = 0.0001f;
    public float m_ElementarySchoolCapacityFactor = 1f / 500f;
    public float m_HighSchoolCapacityFactor = 1f / 1000f;
    public float m_CollegeCapacityFactor = 0.0005f;
    public float m_UniversityCapacityFactor = 0.0002f;
    public float m_ParkingFacilityRange = 400f;
    public float m_MaintenanceVehicleFactor = 0.02f;
    public float m_FireStationEngineFactor = 0.02f;
    public float m_FireStationHelicopterFactor = 0.02f;
    public float m_CrimeProducerCrimeFactor = 0.0002f;
    public float m_PoliceStationCarFactor = 0.01f;
    public float m_PoliceStationHelicopterFactor = 0.1f;
    public float m_PoliceStationCapacityFactor = 0.005f;
    public float m_PrisonVehicleFactor = 0.02f;
    public float m_PrisonCapacityFactor = 0.0001f;
    public float m_GroundPollutionFactor = 0.01f;
    public float m_AirPollutionFactor = 0.01f;
    public float m_NoisePollutionFactor = 0.01f;
    public float m_GroundPollutionRadius = 150f;
    public float m_AirPollutionRadius = 1000f;
    public float m_NoisePollutionRadius = 200f;
    public float m_AttractivenessFactor = 0.005f;
    [EnumValue(typeof (LocalModifierType))]
    public float[] m_LocalModifierFactors;
    [EnumValue(typeof (CityModifierType))]
    public float[] m_CityModifierFactors;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_HappyFaceNotification);
      prefabs.Add((PrefabBase) this.m_SadFaceNotification);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<FeedbackConfigurationData>());
      components.Add(ComponentType.ReadWrite<FeedbackLocalEffectFactor>());
      components.Add(ComponentType.ReadWrite<FeedbackCityEffectFactor>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<FeedbackConfigurationData>(entity, new FeedbackConfigurationData()
      {
        m_HappyFaceNotification = systemManaged.GetEntity((PrefabBase) this.m_HappyFaceNotification),
        m_SadFaceNotification = systemManaged.GetEntity((PrefabBase) this.m_SadFaceNotification),
        m_GarbageProducerGarbageFactor = this.m_GarbageProducerGarbageFactor,
        m_GarbageVehicleFactor = this.m_GarbageVehicleFactor,
        m_HospitalAmbulanceFactor = this.m_HospitalAmbulanceFactor,
        m_HospitalHelicopterFactor = this.m_HospitalHelicopterFactor,
        m_HospitalCapacityFactor = this.m_HospitalCapacityFactor,
        m_DeathcareHearseFactor = this.m_DeathcareHearseFactor,
        m_DeathcareCapacityFactor = this.m_DeathcareCapacityFactor,
        m_DeathcareProcessingFactor = this.m_DeathcareProcessingFactor,
        m_ElectricityConsumptionFactor = this.m_ElectricityConsumptionFactor,
        m_ElectricityProductionFactor = this.m_ElectricityProductionFactor,
        m_TransformerRadius = this.m_TransformerRadius,
        m_WaterConsumptionFactor = this.m_WaterConsumptionFactor,
        m_WaterCapacityFactor = this.m_WaterCapacityFactor,
        m_WaterConsumerSewageFactor = this.m_WaterConsumerSewageFactor,
        m_SewageCapacityFactor = this.m_SewageCapacityFactor,
        m_TransportVehicleCapacityFactor = this.m_TransportVehicleCapacityFactor,
        m_TransportDispatchCenterFactor = this.m_TransportDispatchCenterFactor,
        m_TransportStationRange = this.m_TransportStationRange,
        m_TransportStopRange = this.m_TransportStopRange,
        m_MailProducerMailFactor = this.m_MailProducerMailFactor,
        m_PostFacilityVanFactor = this.m_PostFacilityVanFactor,
        m_PostFacilityTruckFactor = this.m_PostFacilityTruckFactor,
        m_PostFacilityCapacityFactor = this.m_PostFacilityCapacityFactor,
        m_PostFacilityProcessingFactor = this.m_PostFacilityProcessingFactor,
        m_TelecomCapacityFactor = this.m_TelecomCapacityFactor,
        m_ElementarySchoolCapacityFactor = this.m_ElementarySchoolCapacityFactor,
        m_HighSchoolCapacityFactor = this.m_HighSchoolCapacityFactor,
        m_CollegeCapacityFactor = this.m_CollegeCapacityFactor,
        m_UniversityCapacityFactor = this.m_UniversityCapacityFactor,
        m_ParkingFacilityRange = this.m_ParkingFacilityRange,
        m_MaintenanceVehicleFactor = this.m_MaintenanceVehicleFactor,
        m_FireStationEngineFactor = this.m_FireStationEngineFactor,
        m_FireStationHelicopterFactor = this.m_FireStationHelicopterFactor,
        m_CrimeProducerCrimeFactor = this.m_CrimeProducerCrimeFactor,
        m_PoliceStationCarFactor = this.m_PoliceStationCarFactor,
        m_PoliceStationHelicopterFactor = this.m_PoliceStationHelicopterFactor,
        m_PoliceStationCapacityFactor = this.m_PoliceStationCapacityFactor,
        m_PrisonVehicleFactor = this.m_PrisonVehicleFactor,
        m_PrisonCapacityFactor = this.m_PrisonCapacityFactor,
        m_GroundPollutionFactor = this.m_GroundPollutionFactor,
        m_AirPollutionFactor = this.m_AirPollutionFactor,
        m_NoisePollutionFactor = this.m_NoisePollutionFactor,
        m_GroundPollutionRadius = this.m_GroundPollutionRadius,
        m_AirPollutionRadius = this.m_AirPollutionRadius,
        m_NoisePollutionRadius = this.m_NoisePollutionRadius,
        m_AttractivenessFactor = this.m_AttractivenessFactor
      });
      if (this.m_LocalModifierFactors != null)
      {
        DynamicBuffer<FeedbackLocalEffectFactor> buffer = entityManager.GetBuffer<FeedbackLocalEffectFactor>(entity);
        buffer.ResizeUninitialized(this.m_LocalModifierFactors.Length);
        for (int index = 0; index < this.m_LocalModifierFactors.Length; ++index)
          buffer[index] = new FeedbackLocalEffectFactor()
          {
            m_Factor = this.m_LocalModifierFactors[index]
          };
      }
      if (this.m_CityModifierFactors == null)
        return;
      DynamicBuffer<FeedbackCityEffectFactor> buffer1 = entityManager.GetBuffer<FeedbackCityEffectFactor>(entity);
      buffer1.ResizeUninitialized(this.m_CityModifierFactors.Length);
      for (int index = 0; index < this.m_CityModifierFactors.Length; ++index)
        buffer1[index] = new FeedbackCityEffectFactor()
        {
          m_Factor = this.m_CityModifierFactors[index]
        };
    }
  }
}
