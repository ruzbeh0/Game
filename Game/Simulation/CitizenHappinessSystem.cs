// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CitizenHappinessSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Debug;
using Game.Economy;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CitizenHappinessSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    [DebugWatchValue]
    private DebugWatchDistribution m_DebugData;
    private NativeQueue<CitizenHappinessSystem.FactorItem> m_FactorQueue;
    private EntityQuery m_CitizenQuery;
    private EntityQuery m_HappinessFactorParameterQuery;
    private SimulationSystem m_SimulationSystem;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private LocalEffectSystem m_LocalEffectSystem;
    private CitySystem m_CitySystem;
    private TriggerSystem m_TriggerSystem;
    private TaxSystem m_TaxSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_HealthcareParameterQuery;
    private EntityQuery m_ParkParameterQuery;
    private EntityQuery m_EducationParameterQuery;
    private EntityQuery m_TelecomParameterQuery;
    private EntityQuery m_GarbageParameterQuery;
    private EntityQuery m_PoliceParameterQuery;
    private EntityQuery m_CitizenHappinessParameterQuery;
    private EntityQuery m_TimeSettingQuery;
    private EntityQuery m_TimeDataQuery;
    private NativeArray<int4> m_HappinessFactors;
    private JobHandle m_LastDeps;
    private CitizenHappinessSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_429327288_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    private static int GetFactorIndex(
      CitizenHappinessSystem.HappinessFactor factor,
      uint updateFrame)
    {
      return (int) (factor + 25 * (int) updateFrame);
    }

    public float3 GetHappinessFactor(
      CitizenHappinessSystem.HappinessFactor factor,
      DynamicBuffer<HappinessFactorParameterData> parameters,
      ref ComponentLookup<Locked> locked)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return CitizenHappinessSystem.GetHappinessFactor(factor, this.m_HappinessFactors, parameters, ref locked);
    }

    private static float3 GetHappinessFactor(
      CitizenHappinessSystem.HappinessFactor factor,
      NativeArray<int4> happinessFactors,
      DynamicBuffer<HappinessFactorParameterData> parameters,
      ref ComponentLookup<Locked> locked)
    {
      int4 int4 = (int4) 0;
      for (uint updateFrame = 0; updateFrame < 16U; ++updateFrame)
      {
        // ISSUE: reference to a compiler-generated method
        int4 += happinessFactors[CitizenHappinessSystem.GetFactorIndex(factor, updateFrame)];
      }
      Entity lockedEntity = parameters[(int) factor].m_LockedEntity;
      return lockedEntity != Entity.Null && locked.HasEnabledComponent<Locked>(lockedEntity) ? (float3) 0 : (int4.y > 0 ? new float3((float) int4.x / (2f * (float) int4.y), (float) (int4.z / int4.y), (float) (int4.w / int4.y)) : new float3()) - (float) parameters[(int) factor].m_BaseLevel;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalEffectSystem = this.World.GetOrCreateSystemManaged<LocalEffectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessFactors = new NativeArray<int4>(400, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FactorQueue = new NativeQueue<CitizenHappinessSystem.FactorItem>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ParkParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EducationParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EducationParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<TelecomParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<GarbageParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenHappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadWrite<Citizen>(),
          ComponentType.ReadOnly<HouseholdMember>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSettingQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessFactorParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<HappinessFactorParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DebugData = new DebugWatchDistribution();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenQuery);
      this.RequireForUpdate<ServiceFeeParameterData>();
    }

    public static float GetApartmentWellbeing(float sizePerResident, int level)
    {
      return (float) (0.800000011920929 * (4.0 * (double) (level - 1) + (24.555309295654297 + -70.209999084472656 / (double) math.pow(1f + math.pow(sizePerResident / 0.03690514f, 25.2376f), 0.01494523f))));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DebugData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessFactors.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FactorQueue.Dispose();
      base.OnDestroy();
    }

    public static float GetFreetimeWellbeingDifferential(int freetime) => 4f / (float) freetime;

    public static float GetFreetimeWellbeing(int freetime)
    {
      return (float) (4.0 * (double) math.log((float) math.max(1, freetime)) - 25.0);
    }

    public static int GetElectricityFeeHappinessEffect(
      float relativeFee,
      in CitizenHappinessParameterData data)
    {
      // ISSUE: reference to a compiler-generated method
      return (int) math.round((float) math.csum(CitizenHappinessSystem.GetElectricityFeeBonuses(relativeFee, in data)) / 2f);
    }

    public static int2 GetElectricityFeeBonuses(
      Entity building,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      float relativeFee,
      in CitizenHappinessParameterData data)
    {
      ElectricityConsumer componentData;
      // ISSUE: reference to a compiler-generated method
      return electricityConsumers.TryGetComponent(building, out componentData) && componentData.m_WantedConsumption > 0 ? CitizenHappinessSystem.GetElectricityFeeBonuses(relativeFee, in data) : new int2();
    }

    public static int2 GetElectricityFeeBonuses(
      float relativeFee,
      in CitizenHappinessParameterData data)
    {
      return new int2()
      {
        y = (int) math.round(data.m_ElectricityFeeWellbeingEffect.Evaluate(relativeFee))
      };
    }

    public static int2 GetElectricitySupplyBonuses(
      Entity building,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      in CitizenHappinessParameterData data)
    {
      ElectricityConsumer componentData;
      if (!electricityConsumers.TryGetComponent(building, out componentData))
        return new int2();
      float num = math.saturate((float) componentData.m_CooldownCounter / data.m_ElectricityPenaltyDelay);
      return new int2()
      {
        y = (int) math.round(-data.m_ElectricityWellbeingPenalty * num)
      };
    }

    public static int GetWaterFeeHappinessEffect(
      float relativeFee,
      in CitizenHappinessParameterData data)
    {
      // ISSUE: reference to a compiler-generated method
      return (int) math.round((float) math.csum(CitizenHappinessSystem.GetWaterFeeBonuses(relativeFee, in data)) / 2f);
    }

    public static int2 GetWaterFeeBonuses(
      Entity building,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      float relativeFee,
      in CitizenHappinessParameterData data)
    {
      WaterConsumer componentData;
      // ISSUE: reference to a compiler-generated method
      return waterConsumers.TryGetComponent(building, out componentData) && componentData.m_WantedConsumption > 0 ? CitizenHappinessSystem.GetWaterFeeBonuses(relativeFee, in data) : new int2();
    }

    public static int2 GetWaterFeeBonuses(float relativeFee, in CitizenHappinessParameterData data)
    {
      return new int2()
      {
        x = (int) math.round(data.m_WaterFeeHealthEffect.Evaluate(relativeFee)),
        y = (int) math.round(data.m_WaterFeeWellbeingEffect.Evaluate(relativeFee))
      };
    }

    public static int2 GetWaterSupplyBonuses(
      Entity building,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      in CitizenHappinessParameterData data)
    {
      WaterConsumer componentData;
      if (!waterConsumers.TryGetComponent(building, out componentData))
        return new int2();
      float num = math.saturate((float) componentData.m_FreshCooldownCounter / data.m_WaterPenaltyDelay);
      return new int2()
      {
        x = (int) math.round((float) -data.m_WaterHealthPenalty * num),
        y = (int) math.round((float) -data.m_WaterWellbeingPenalty * num)
      };
    }

    public static int2 GetWaterPollutionBonuses(
      Entity building,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      DynamicBuffer<CityModifier> cityModifiers,
      in CitizenHappinessParameterData data)
    {
      int2 pollutionBonuses = new int2();
      if (waterConsumers.HasComponent(building))
      {
        WaterConsumer waterConsumer = waterConsumers[building];
        if ((double) waterConsumer.m_Pollution > 0.0)
        {
          float num = 1f;
          CityUtils.ApplyModifier(ref num, cityModifiers, CityModifierType.PollutionHealthAffect);
          pollutionBonuses.x = Mathf.RoundToInt(num * data.m_WaterPollutionBonusMultiplier * math.min(1f, 10f * waterConsumer.m_Pollution));
        }
      }
      return pollutionBonuses;
    }

    public static int2 GetSewageBonuses(
      Entity building,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      in CitizenHappinessParameterData data)
    {
      WaterConsumer componentData;
      if (!waterConsumers.TryGetComponent(building, out componentData))
        return new int2();
      float num = math.saturate((float) componentData.m_SewageCooldownCounter / data.m_SewagePenaltyDelay);
      return new int2()
      {
        x = (int) math.round((float) -data.m_SewageHealthEffect * num),
        y = (int) math.round((float) -data.m_SewageWellbeingEffect * num)
      };
    }

    public static int2 GetHealthcareBonuses(
      float curvePosition,
      DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage,
      ref ComponentLookup<Locked> locked,
      Entity healthcareService,
      in CitizenHappinessParameterData data)
    {
      if (locked.HasEnabledComponent<Locked>(healthcareService))
        return new int2(0, 0);
      int2 healthcareBonuses = new int2();
      float serviceCoverage1 = NetUtils.GetServiceCoverage(serviceCoverage, CoverageService.Healthcare, curvePosition);
      healthcareBonuses.x = Mathf.RoundToInt(data.m_HealthCareHealthMultiplier * serviceCoverage1);
      healthcareBonuses.y = Mathf.RoundToInt(data.m_HealthCareWellbeingMultiplier * serviceCoverage1);
      return healthcareBonuses;
    }

    public static int2 GetEducationBonuses(
      float curvePosition,
      DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage,
      ref ComponentLookup<Locked> locked,
      Entity educationService,
      in CitizenHappinessParameterData data,
      int children)
    {
      if (locked.HasEnabledComponent<Locked>(educationService))
        return new int2(0, 0);
      int2 educationBonuses = new int2();
      float f = (float) ((double) math.sqrt((float) children) * (double) data.m_EducationWellbeingMultiplier * ((double) NetUtils.GetServiceCoverage(serviceCoverage, CoverageService.Education, curvePosition) - (double) data.m_NeutralEducation));
      educationBonuses.y = Mathf.RoundToInt(f);
      return educationBonuses;
    }

    public static int2 GetEntertainmentBonuses(
      float curvePosition,
      DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage,
      DynamicBuffer<CityModifier> cityModifiers,
      ref ComponentLookup<Locked> locked,
      Entity entertainmentService,
      in CitizenHappinessParameterData data)
    {
      if (locked.HasEnabledComponent<Locked>(entertainmentService))
        return new int2(0, 0);
      int2 entertainmentBonuses = new int2();
      float serviceCoverage1 = NetUtils.GetServiceCoverage(serviceCoverage, CoverageService.Park, curvePosition);
      CityUtils.ApplyModifier(ref serviceCoverage1, cityModifiers, CityModifierType.Entertainment);
      float f = data.m_EntertainmentWellbeingMultiplier * math.min(1f, math.sqrt(serviceCoverage1 / 1.5f));
      entertainmentBonuses.x = 0;
      entertainmentBonuses.y = Mathf.RoundToInt(f);
      return entertainmentBonuses;
    }

    public static int2 GetGroundPollutionBonuses(
      Entity building,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      NativeArray<GroundPollution> pollutionMap,
      DynamicBuffer<CityModifier> cityModifiers,
      in CitizenHappinessParameterData data)
    {
      int2 pollutionBonuses = new int2();
      if (transforms.HasComponent(building))
      {
        // ISSUE: reference to a compiler-generated method
        short y = (short) ((int) GroundPollutionSystem.GetPollution(transforms[building].m_Position, pollutionMap).m_Pollution / data.m_PollutionBonusDivisor);
        float num = 1f;
        CityUtils.ApplyModifier(ref num, cityModifiers, CityModifierType.PollutionHealthAffect);
        pollutionBonuses.x = (int) ((double) -math.min(data.m_MaxAirAndGroundPollutionBonus, (int) y) * (double) num);
      }
      return pollutionBonuses;
    }

    public static int2 GetAirPollutionBonuses(
      Entity building,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      NativeArray<AirPollution> airPollutionMap,
      DynamicBuffer<CityModifier> cityModifiers,
      in CitizenHappinessParameterData data)
    {
      int2 pollutionBonuses = new int2();
      if (transforms.HasComponent(building))
      {
        // ISSUE: reference to a compiler-generated method
        short y = (short) ((int) AirPollutionSystem.GetPollution(transforms[building].m_Position, airPollutionMap).m_Pollution / data.m_PollutionBonusDivisor);
        float num = 1f;
        CityUtils.ApplyModifier(ref num, cityModifiers, CityModifierType.PollutionHealthAffect);
        pollutionBonuses.x = (int) ((double) -math.min(data.m_MaxAirAndGroundPollutionBonus, (int) y) * (double) num);
      }
      return pollutionBonuses;
    }

    public static int2 GetNoiseBonuses(
      Entity building,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      NativeArray<NoisePollution> noiseMap,
      in CitizenHappinessParameterData data)
    {
      int2 noiseBonuses = new int2();
      if (transforms.HasComponent(building))
      {
        short y = (short) ((int) NoisePollutionSystem.GetPollution(transforms[building].m_Position, noiseMap).m_Pollution / data.m_PollutionBonusDivisor);
        noiseBonuses.y = -math.min(data.m_MaxNoisePollutionBonus, (int) y);
      }
      return noiseBonuses;
    }

    public static int2 GetGarbageBonuses(
      Entity building,
      ref ComponentLookup<GarbageProducer> garbages,
      ref ComponentLookup<Locked> locked,
      Entity garbageService,
      in GarbageParameterData data)
    {
      if (locked.HasEnabledComponent<Locked>(garbageService))
        return new int2(0, 0);
      int2 garbageBonuses = new int2();
      if (garbages.HasComponent(building))
      {
        int y = math.max(0, (garbages[building].m_Garbage - data.m_HappinessEffectBaseline) / data.m_HappinessEffectStep);
        garbageBonuses.x = -math.min(10, y);
        garbageBonuses.y = -math.min(10, y);
      }
      return garbageBonuses;
    }

    public static int2 GetCrimeBonuses(
      CrimeVictim crimeVictim,
      Entity building,
      ref ComponentLookup<CrimeProducer> crimes,
      ref ComponentLookup<Locked> locked,
      Entity policeService,
      in CitizenHappinessParameterData data)
    {
      if (locked.HasEnabledComponent<Locked>(policeService))
        return new int2(0, 0);
      int2 crimeBonuses = new int2();
      if (crimes.HasComponent(building))
      {
        int y = Mathf.RoundToInt(math.max(0.0f, (crimes[building].m_Crime - (float) data.m_NegligibleCrime) / data.m_CrimeMultiplier));
        crimeBonuses.x = 0;
        crimeBonuses.y = -math.min(data.m_MaxCrimePenalty, y);
      }
      crimeBonuses.y -= (int) crimeVictim.m_Effect;
      return crimeBonuses;
    }

    public static int2 GetMailBonuses(
      Entity building,
      ref ComponentLookup<MailProducer> mails,
      ref ComponentLookup<Locked> locked,
      Entity telecomService,
      in CitizenHappinessParameterData data)
    {
      if (locked.HasEnabledComponent<Locked>(telecomService))
        return new int2(0, 0);
      int2 mailBonuses = new int2();
      if (mails.HasComponent(building))
      {
        MailProducer mailProducer = mails[building];
        int num1 = math.max(0, math.max((int) mailProducer.m_SendingMail, mailProducer.receivingMail) - data.m_NegligibleMail);
        mailBonuses.x = 0;
        if (num1 < 25)
        {
          if (!mailProducer.mailDelivered)
            return mailBonuses;
          int num2 = 125;
          int num3 = 25 - num1;
          mailBonuses.y = (num3 * num3 + (num2 >> 1)) / num2;
        }
        else
        {
          int num4 = 250;
          int num5 = math.min(50, num1 - 25);
          mailBonuses.y = -((num5 * num5 + (num4 >> 1)) / num4);
        }
        mailBonuses.y *= Mathf.RoundToInt(data.m_MailMultiplier);
      }
      return mailBonuses;
    }

    public static int2 GetTelecomBonuses(
      Entity building,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      CellMapData<TelecomCoverage> telecomCoverage,
      ref ComponentLookup<Locked> locked,
      Entity telecomService,
      in CitizenHappinessParameterData data)
    {
      if (locked.HasEnabledComponent<Locked>(telecomService))
        return new int2();
      int2 telecomBonuses = new int2();
      if (transforms.HasComponent(building))
      {
        float3 position = transforms[building].m_Position;
        float num1 = TelecomCoverage.SampleNetworkQuality(telecomCoverage, position);
        float telecomBaseline = data.m_TelecomBaseline;
        if ((double) num1 >= (double) telecomBaseline)
        {
          float num2 = (float) (((double) num1 - (double) telecomBaseline) / (1.0 - (double) telecomBaseline));
          telecomBonuses.y = Mathf.RoundToInt(num2 * num2 * data.m_TelecomBonusMultiplier);
        }
        else
        {
          float num3 = (float) (1.0 - (double) num1 / (double) telecomBaseline);
          telecomBonuses.y = Mathf.RoundToInt((float) ((double) num3 * (double) num3 * -(double) data.m_TelecomPenaltyMultiplier));
        }
      }
      return telecomBonuses;
    }

    public static int2 GetTaxBonuses(
      int educationLevel,
      NativeArray<int> taxRates,
      in CitizenHappinessParameterData data)
    {
      // ISSUE: reference to a compiler-generated method
      int residentialTaxRate = TaxSystem.GetResidentialTaxRate(educationLevel, taxRates);
      float num = 0.0f;
      switch (educationLevel)
      {
        case 0:
          num = data.m_TaxUneducatedMultiplier;
          break;
        case 1:
          num = data.m_TaxPoorlyEducatedMultiplier;
          break;
        case 2:
          num = data.m_TaxEducatedMultiplier;
          break;
        case 3:
          num = data.m_TaxWellEducatedMultiplier;
          break;
        case 4:
          num = data.m_TaxHighlyEducatedMultiplier;
          break;
      }
      return new int2(0, Mathf.RoundToInt((float) (residentialTaxRate - 10) * num));
    }

    public static int2 GetWellfareBonuses(
      float curvePosition,
      DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage,
      in CitizenHappinessParameterData data,
      int currentHappiness)
    {
      int2 wellfareBonuses = new int2();
      float num = data.m_WelfareMultiplier * NetUtils.GetServiceCoverage(serviceCoverage, CoverageService.Welfare, curvePosition);
      wellfareBonuses.y = Mathf.RoundToInt(num * (float) math.max(0, (50 - currentHappiness) / 50));
      return wellfareBonuses;
    }

    public static float GetWelfareValue(
      float curvePosition,
      DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage,
      in CitizenHappinessParameterData data)
    {
      return data.m_WelfareMultiplier * NetUtils.GetServiceCoverage(serviceCoverage, CoverageService.Welfare, curvePosition);
    }

    public static int2 GetCachedWelfareBonuses(float cachedValue, int currentHappiness)
    {
      return new int2()
      {
        y = Mathf.RoundToInt(cachedValue * (float) math.max(0, (50 - currentHappiness) / 50))
      };
    }

    public static int2 GetSicknessBonuses(
      bool hasHealthProblem,
      in CitizenHappinessParameterData data)
    {
      return hasHealthProblem ? new int2(-data.m_HealthProblemHealthPenalty, 0) : new int2();
    }

    public static int2 GetHomelessBonuses(in CitizenHappinessParameterData data)
    {
      return new int2(data.m_HomelessHealthEffect, data.m_HomelessWellbeingEffect);
    }

    public static int2 GetDeathPenalty(
      DynamicBuffer<HouseholdCitizen> householdCitizens,
      ref ComponentLookup<HealthProblem> healthProblems,
      in CitizenHappinessParameterData data)
    {
      bool flag = false;
      foreach (HouseholdCitizen householdCitizen in householdCitizens)
      {
        if (CitizenUtils.IsDead(householdCitizen.m_Citizen, ref healthProblems))
        {
          flag = true;
          break;
        }
      }
      return flag ? new int2(-data.m_DeathHealthPenalty, -data.m_DeathWellbeingPenalty) : new int2();
    }

    public static float GetConsumptionHappinessDifferential(float dailyConsumption, int citizens)
    {
      if ((double) dailyConsumption <= 0.0)
        return 100f;
      float num = dailyConsumption / math.max(1f, (float) citizens);
      return (float) (8.0 / (1.0 + 0.20000000298023224 * (double) num) - 50000.0 * (double) math.pow((float) (2.0 * (double) num + 190.0), -2f));
    }

    public static int2 GetConsumptionBonuses(
      float dailyConsumption,
      int citizens,
      in CitizenHappinessParameterData data)
    {
      float num = dailyConsumption / math.max(1f, (float) citizens);
      return new int2(0, math.clamp(Mathf.RoundToInt((float) (20.0 * (double) math.log((float) (1.0 + 0.20000000298023224 * (double) num)) + 12500.0 / (2.0 * (double) num + 190.0) - 112.0)), -40, 40));
    }

    public static int2 GetLeisureBonuses(byte leisureValue)
    {
      return new int2(0, ((int) leisureValue - 128) / 16);
    }

    public static int GetMaxHealth(float ageInYears)
    {
      if ((double) ageInYears < 2.0)
        return 100;
      if ((double) ageInYears < 3.0)
        return 90;
      return (double) ageInYears < 6.0 ? 80 : 80 - 10 * Mathf.FloorToInt(ageInYears - 5f);
    }

    public static void GetBuildingHappinessFactors(
      Entity property,
      NativeArray<int> factors,
      ref ComponentLookup<PrefabRef> prefabs,
      ref ComponentLookup<SpawnableBuildingData> spawnableBuildings,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDatas,
      ref ComponentLookup<ConsumptionData> consumptionDatas,
      ref BufferLookup<CityModifier> cityModifiers,
      ref ComponentLookup<Building> buildings,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      ref BufferLookup<Game.Net.ServiceCoverage> serviceCoverages,
      ref ComponentLookup<Locked> locked,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      ref ComponentLookup<GarbageProducer> garbageProducers,
      ref ComponentLookup<CrimeProducer> crimeProducers,
      ref ComponentLookup<MailProducer> mailProducers,
      ref ComponentLookup<OfficeBuilding> officeBuildings,
      ref BufferLookup<Renter> renters,
      ref ComponentLookup<Citizen> citizenDatas,
      ref BufferLookup<HouseholdCitizen> householdCitizens,
      ref ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      ref ComponentLookup<CompanyData> companies,
      ref ComponentLookup<IndustrialProcessData> industrialProcessDatas,
      ref ComponentLookup<WorkProvider> workProviders,
      ref BufferLookup<Employee> employees,
      ref ComponentLookup<WorkplaceData> workplaceDatas,
      ref ComponentLookup<Citizen> citizens,
      ref ComponentLookup<HealthProblem> healthProblems,
      ref ComponentLookup<ServiceAvailable> serviceAvailables,
      ref ComponentLookup<ResourceData> resourceDatas,
      ref ComponentLookup<ZonePropertiesData> zonePropertiesDatas,
      ref BufferLookup<Efficiency> efficiencies,
      ref ComponentLookup<ServiceCompanyData> serviceCompanyDatas,
      ref BufferLookup<ResourceAvailability> availabilities,
      ref BufferLookup<TradeCost> tradeCosts,
      CitizenHappinessParameterData citizenHappinessParameters,
      GarbageParameterData garbageParameters,
      HealthcareParameterData healthcareParameters,
      ParkParameterData parkParameters,
      EducationParameterData educationParameters,
      TelecomParameterData telecomParameters,
      ref EconomyParameterData economyParameters,
      DynamicBuffer<HappinessFactorParameterData> happinessFactorParameters,
      NativeArray<GroundPollution> pollutionMap,
      NativeArray<NoisePollution> noisePollutionMap,
      NativeArray<AirPollution> airPollutionMap,
      CellMapData<TelecomCoverage> telecomCoverage,
      Entity city,
      NativeArray<int> taxRates,
      NativeArray<Entity> processes,
      ResourcePrefabs resourcePrefabs,
      float relativeElectricityFee,
      float relativeWaterFee)
    {
      for (int index = 0; index < factors.Length; ++index)
        factors[index] = 0;
      if (!prefabs.HasComponent(property))
        return;
      Entity prefab = prefabs[property].m_Prefab;
      if (!spawnableBuildings.HasComponent(prefab) || !buildingDatas.HasComponent(prefab))
        return;
      BuildingPropertyData buildingPropertyData = buildingPropertyDatas[prefab];
      DynamicBuffer<CityModifier> cityModifiers1 = cityModifiers[city];
      Game.Prefabs.BuildingData buildingData = buildingDatas[prefab];
      float num1 = (float) (buildingData.m_LotSize.x * buildingData.m_LotSize.y);
      Entity roadEdge = Entity.Null;
      float curvePosition = 0.0f;
      SpawnableBuildingData spawnableData = spawnableBuildings[prefab];
      int level = (int) spawnableData.m_Level;
      Building building = new Building();
      if (buildings.HasComponent(property))
      {
        building = buildings[property];
        roadEdge = building.m_RoadEdge;
        curvePosition = building.m_CurvePosition;
      }
      bool flag = false;
      Entity entity1 = new Entity();
      Entity entity2 = new Entity();
      IndustrialProcessData processData1 = new IndustrialProcessData();
      ServiceCompanyData serviceCompanyData = new ServiceCompanyData();
      Resource resource = buildingPropertyData.m_AllowedManufactured | buildingPropertyData.m_AllowedSold;
      if (resource != Resource.NoResource)
      {
        if (renters.HasBuffer(property))
        {
          DynamicBuffer<Renter> dynamicBuffer = renters[property];
          for (int index = 0; index < dynamicBuffer.Length; ++index)
          {
            entity1 = dynamicBuffer[index].m_Renter;
            if (companies.HasComponent(entity1) && prefabs.HasComponent(entity1))
            {
              entity2 = prefabs[entity1].m_Prefab;
              if (industrialProcessDatas.HasComponent(entity2))
              {
                if (serviceCompanyDatas.HasComponent(entity2))
                  serviceCompanyData = serviceCompanyDatas[entity2];
                processData1 = industrialProcessDatas[entity2];
                flag = true;
                break;
              }
            }
          }
        }
        int num2 = 0;
        if (flag)
        {
          // ISSUE: reference to a compiler-generated method
          CitizenHappinessSystem.AddCompanyHappinessFactors(factors, property, prefab, entity1, entity2, processData1, serviceCompanyData, buildingPropertyData.m_AllowedSold > Resource.NoResource, level, ref officeBuildings, ref workProviders, ref employees, ref workplaceDatas, ref serviceAvailables, ref resourceDatas, ref efficiencies, ref buildingPropertyDatas, ref availabilities, ref tradeCosts, taxRates, building, spawnableData, buildingData, resourcePrefabs, ref economyParameters);
          ++num2;
        }
        else
        {
          for (int index = 0; index < processes.Length; ++index)
          {
            IndustrialProcessData processData2 = industrialProcessDatas[processes[index]];
            int num3 = buildingPropertyData.m_AllowedSold > Resource.NoResource ? 1 : 0;
            if (num3 != 0 && serviceCompanyDatas.HasComponent(processes[index]))
              serviceCompanyData = serviceCompanyDatas[processes[index]];
            if ((num3 == 0 || serviceCompanyDatas.HasComponent(processes[index])) && (resource & processData2.m_Output.m_Resource) != Resource.NoResource)
            {
              // ISSUE: reference to a compiler-generated method
              CitizenHappinessSystem.AddCompanyHappinessFactors(factors, property, prefab, entity1, entity2, processData2, serviceCompanyData, buildingPropertyData.m_AllowedSold > Resource.NoResource, level, ref officeBuildings, ref workProviders, ref employees, ref workplaceDatas, ref serviceAvailables, ref resourceDatas, ref efficiencies, ref buildingPropertyDatas, ref availabilities, ref tradeCosts, taxRates, building, spawnableData, buildingData, resourcePrefabs, ref economyParameters);
              ++num2;
            }
          }
        }
        for (int index = 0; index < factors.Length; ++index)
          factors[index] /= num2;
      }
      if (buildingPropertyData.m_ResidentialProperties <= 0)
        return;
      for (int index = 0; index < factors.Length; ++index)
        factors[index] = Mathf.RoundToInt((float) factors[index] / (1f - economyParameters.m_MixedBuildingCompanyRentPercentage));
      float num4 = num1 / (float) buildingPropertyData.m_ResidentialProperties;
      float num5 = 1f;
      int currentHappiness = 50;
      int leisureValue = 128;
      float num6 = 0.3f;
      float num7 = 0.25f;
      float num8 = 0.25f;
      float num9 = 0.15f;
      float num10 = 0.05f;
      float num11 = 2f;
      if (renters.HasBuffer(property))
      {
        num6 = 0.0f;
        num7 = 0.0f;
        num8 = 0.0f;
        num9 = 0.0f;
        num10 = 0.0f;
        int2 int2_1 = new int2();
        int2 int2_2 = new int2();
        int num12 = 0;
        int num13 = 0;
        DynamicBuffer<Renter> dynamicBuffer1 = renters[property];
        for (int index1 = 0; index1 < dynamicBuffer1.Length; ++index1)
        {
          Entity renter = dynamicBuffer1[index1].m_Renter;
          if (householdCitizens.HasBuffer(renter))
          {
            ++num13;
            DynamicBuffer<HouseholdCitizen> dynamicBuffer2 = householdCitizens[renter];
            for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
            {
              Entity citizen1 = dynamicBuffer2[index2].m_Citizen;
              if (citizenDatas.HasComponent(citizen1))
              {
                Citizen citizen2 = citizenDatas[citizen1];
                int2_2.x += citizen2.Happiness;
                ++int2_2.y;
                num12 += (int) citizen2.m_LeisureCounter;
                switch (citizen2.GetEducationLevel())
                {
                  case 0:
                    ++num6;
                    break;
                  case 1:
                    ++num7;
                    break;
                  case 2:
                    ++num8;
                    break;
                  case 3:
                    ++num9;
                    break;
                  case 4:
                    ++num10;
                    break;
                }
                if (citizen2.GetAge() == CitizenAge.Child)
                  ++int2_1.x;
              }
            }
            ++int2_1.y;
          }
        }
        if (int2_1.y > 0)
          num5 = (float) (int2_1.x / int2_1.y);
        if (int2_2.y > 0)
        {
          currentHappiness = Mathf.RoundToInt((float) (int2_2.x / int2_2.y));
          leisureValue = Mathf.RoundToInt((float) (num12 / int2_2.y));
          num6 /= (float) int2_2.y;
          num7 /= (float) int2_2.y;
          num8 /= (float) int2_2.y;
          num9 /= (float) int2_2.y;
          num10 /= (float) int2_2.y;
          num11 = (float) int2_2.y / (float) num13;
        }
      }
      Entity healthcareServicePrefab = healthcareParameters.m_HealthcareServicePrefab;
      Entity parkServicePrefab = parkParameters.m_ParkServicePrefab;
      Entity educationServicePrefab = educationParameters.m_EducationServicePrefab;
      Entity telecomServicePrefab = telecomParameters.m_TelecomServicePrefab;
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[4].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 electricitySupplyBonuses = CitizenHappinessSystem.GetElectricitySupplyBonuses(property, ref electricityConsumers, in citizenHappinessParameters);
        factors[3] = (electricitySupplyBonuses.x + electricitySupplyBonuses.y) / 2 - happinessFactorParameters[4].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[23].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 electricityFeeBonuses = CitizenHappinessSystem.GetElectricityFeeBonuses(property, ref electricityConsumers, relativeElectricityFee, in citizenHappinessParameters);
        factors[26] = (electricityFeeBonuses.x + electricityFeeBonuses.y) / 2 - happinessFactorParameters[23].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[8].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 waterSupplyBonuses = CitizenHappinessSystem.GetWaterSupplyBonuses(property, ref waterConsumers, in citizenHappinessParameters);
        factors[7] = (waterSupplyBonuses.x + waterSupplyBonuses.y) / 2 - happinessFactorParameters[8].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[24].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 waterFeeBonuses = CitizenHappinessSystem.GetWaterFeeBonuses(property, ref waterConsumers, relativeWaterFee, in citizenHappinessParameters);
        factors[27] = (waterFeeBonuses.x + waterFeeBonuses.y) / 2 - happinessFactorParameters[24].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[9].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 pollutionBonuses = CitizenHappinessSystem.GetWaterPollutionBonuses(property, ref waterConsumers, cityModifiers1, in citizenHappinessParameters);
        factors[8] = (pollutionBonuses.x + pollutionBonuses.y) / 2 - happinessFactorParameters[9].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[10].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 sewageBonuses = CitizenHappinessSystem.GetSewageBonuses(property, ref waterConsumers, in citizenHappinessParameters);
        factors[9] = (sewageBonuses.x + sewageBonuses.y) / 2 - happinessFactorParameters[10].m_BaseLevel;
      }
      if (serviceCoverages.HasBuffer(roadEdge))
      {
        DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage = serviceCoverages[roadEdge];
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[5].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 healthcareBonuses = CitizenHappinessSystem.GetHealthcareBonuses(curvePosition, serviceCoverage, ref locked, healthcareServicePrefab, in citizenHappinessParameters);
          factors[4] = (healthcareBonuses.x + healthcareBonuses.y) / 2 - happinessFactorParameters[5].m_BaseLevel;
        }
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[12].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 entertainmentBonuses = CitizenHappinessSystem.GetEntertainmentBonuses(curvePosition, serviceCoverage, cityModifiers1, ref locked, parkServicePrefab, in citizenHappinessParameters);
          factors[11] = (entertainmentBonuses.x + entertainmentBonuses.y) / 2 - happinessFactorParameters[12].m_BaseLevel;
        }
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[13].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 educationBonuses = CitizenHappinessSystem.GetEducationBonuses(curvePosition, serviceCoverage, ref locked, educationServicePrefab, in citizenHappinessParameters, 1);
          factors[12] = Mathf.RoundToInt((float) ((double) num5 * (double) (educationBonuses.x + educationBonuses.y) / 2.0)) - happinessFactorParameters[13].m_BaseLevel;
        }
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[15].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 wellfareBonuses = CitizenHappinessSystem.GetWellfareBonuses(curvePosition, serviceCoverage, in citizenHappinessParameters, currentHappiness);
          factors[14] = (wellfareBonuses.x + wellfareBonuses.y) / 2 - happinessFactorParameters[15].m_BaseLevel;
        }
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[6].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 pollutionBonuses = CitizenHappinessSystem.GetGroundPollutionBonuses(property, ref transforms, pollutionMap, cityModifiers1, in citizenHappinessParameters);
        factors[5] = (pollutionBonuses.x + pollutionBonuses.y) / 2 - happinessFactorParameters[6].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[2].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 pollutionBonuses = CitizenHappinessSystem.GetAirPollutionBonuses(property, ref transforms, airPollutionMap, cityModifiers1, in citizenHappinessParameters);
        factors[2] = (pollutionBonuses.x + pollutionBonuses.y) / 2 - happinessFactorParameters[2].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[7].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 noiseBonuses = CitizenHappinessSystem.GetNoiseBonuses(property, ref transforms, noisePollutionMap, in citizenHappinessParameters);
        factors[6] = (noiseBonuses.x + noiseBonuses.y) / 2 - happinessFactorParameters[7].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[11].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 garbageBonuses = CitizenHappinessSystem.GetGarbageBonuses(property, ref garbageProducers, ref locked, happinessFactorParameters[11].m_LockedEntity, in garbageParameters);
        factors[10] = (garbageBonuses.x + garbageBonuses.y) / 2 - happinessFactorParameters[11].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[1].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 crimeBonuses = CitizenHappinessSystem.GetCrimeBonuses(new CrimeVictim(), property, ref crimeProducers, ref locked, happinessFactorParameters[1].m_LockedEntity, in citizenHappinessParameters);
        factors[1] = (crimeBonuses.x + crimeBonuses.y) / 2 - happinessFactorParameters[1].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[14].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 mailBonuses = CitizenHappinessSystem.GetMailBonuses(property, ref mailProducers, ref locked, telecomServicePrefab, in citizenHappinessParameters);
        factors[13] = (mailBonuses.x + mailBonuses.y) / 2 - happinessFactorParameters[14].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[0].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 telecomBonuses = CitizenHappinessSystem.GetTelecomBonuses(property, ref transforms, telecomCoverage, ref locked, telecomServicePrefab, in citizenHappinessParameters);
        factors[0] = (telecomBonuses.x + telecomBonuses.y) / 2 - happinessFactorParameters[0].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[16].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 leisureBonuses = CitizenHappinessSystem.GetLeisureBonuses((byte) leisureValue);
        factors[15] = (leisureBonuses.x + leisureBonuses.y) / 2 - happinessFactorParameters[16].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[17].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        float2 float2 = new float2(num6, num6) * (float2) CitizenHappinessSystem.GetTaxBonuses(0, taxRates, in citizenHappinessParameters) + new float2(num7, num7) * (float2) CitizenHappinessSystem.GetTaxBonuses(1, taxRates, in citizenHappinessParameters) + new float2(num8, num8) * (float2) CitizenHappinessSystem.GetTaxBonuses(2, taxRates, in citizenHappinessParameters) + new float2(num9, num9) * (float2) CitizenHappinessSystem.GetTaxBonuses(3, taxRates, in citizenHappinessParameters) + new float2(num10, num10) * (float2) CitizenHappinessSystem.GetTaxBonuses(4, taxRates, in citizenHappinessParameters);
        factors[16] = Mathf.RoundToInt(float2.x + float2.y) / 2 - happinessFactorParameters[17].m_BaseLevel;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[3].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        float2 apartmentWellbeing = (float2) CitizenHappinessSystem.GetApartmentWellbeing(buildingPropertyData.m_SpaceMultiplier * num4 / num11, level);
        factors[21] = Mathf.RoundToInt(apartmentWellbeing.x + apartmentWellbeing.y) / 2 - happinessFactorParameters[3].m_BaseLevel;
      }
      if (resource == Resource.NoResource)
        return;
      for (int index = 0; index < factors.Length; ++index)
        factors[index] = Mathf.RoundToInt((float) factors[index] * (1f - economyParameters.m_MixedBuildingCompanyRentPercentage));
    }

    private static void AddCompanyHappinessFactors(
      NativeArray<int> factors,
      Entity property,
      Entity prefab,
      Entity renter,
      Entity renterPrefab,
      IndustrialProcessData processData,
      ServiceCompanyData serviceCompanyData,
      bool commercial,
      int level,
      ref ComponentLookup<OfficeBuilding> officeBuildings,
      ref ComponentLookup<WorkProvider> workProviders,
      ref BufferLookup<Employee> employees,
      ref ComponentLookup<WorkplaceData> workplaceDatas,
      ref ComponentLookup<ServiceAvailable> serviceAvailables,
      ref ComponentLookup<ResourceData> resourceDatas,
      ref BufferLookup<Efficiency> efficiencies,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDatas,
      ref BufferLookup<ResourceAvailability> availabilities,
      ref BufferLookup<TradeCost> tradeCosts,
      NativeArray<int> taxRates,
      Building building,
      SpawnableBuildingData spawnableData,
      Game.Prefabs.BuildingData buildingData,
      ResourcePrefabs resourcePrefabs,
      ref EconomyParameterData economyParameters)
    {
    }

    private static int GetFactor(float profit, float defaultProfit)
    {
      return Mathf.RoundToInt((float) (10.0 * ((double) profit / (double) defaultProfit - 1.0)));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery.AddSharedComponentFilter<UpdateFrame>(new UpdateFrame(frameWithInterval));
      NativeQueue<int>.ParallelWriter parallelWriter = new NativeQueue<int>.ParallelWriter();
      // ISSUE: reference to a compiler-generated field
      if (this.m_DebugData.IsEnabled)
      {
        // ISSUE: reference to a compiler-generated field
        parallelWriter = this.m_DebugData.GetQueue(false, out JobHandle _).AsParallelWriter();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CrimeVictim_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
      JobHandle dependencies5;
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new CitizenHappinessSystem.CitizenHappinessJob()
      {
        m_DebugQueue = parallelWriter,
        m_DebugOn = this.m_DebugData.IsEnabled,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle,
        m_CrimeVictimType = this.__TypeHandle.__Game_Citizens_CrimeVictim_RO_ComponentTypeHandle,
        m_CriminalType = this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentTypeHandle,
        m_StudentType = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_HealthProblemType = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ElectricityConsumers = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_Properties = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_ServiceCoverages = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup,
        m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_WaterConsumers = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_Garbages = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Locked = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup,
        m_CrimeProducers = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup,
        m_MailProducers = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_SpawnableBuildings = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_ServiceFees = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_Prisons = this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup,
        m_Schools = this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup,
        m_HomelessHouseholds = this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup,
        m_PollutionMap = this.m_GroundPollutionSystem.GetMap(true, out dependencies1),
        m_AirPollutionMap = this.m_AirPollutionSystem.GetMap(true, out dependencies2),
        m_NoisePollutionMap = this.m_NoisePollutionSystem.GetMap(true, out dependencies3),
        m_TelecomCoverage = this.m_TelecomCoverageSystem.GetData(true, out dependencies4),
        m_LocalEffectData = this.m_LocalEffectSystem.GetReadData(out dependencies5),
        m_HealthcareParameters = this.m_HealthcareParameterQuery.GetSingleton<HealthcareParameterData>(),
        m_ParkParameters = this.m_ParkParameterQuery.GetSingleton<ParkParameterData>(),
        m_EducationParameters = this.m_EducationParameterQuery.GetSingleton<EducationParameterData>(),
        m_TelecomParameters = this.m_TelecomParameterQuery.GetSingleton<TelecomParameterData>(),
        m_GarbageParameters = this.m_GarbageParameterQuery.GetSingleton<GarbageParameterData>(),
        m_PoliceParameters = this.m_PoliceParameterQuery.GetSingleton<PoliceConfigurationData>(),
        m_CitizenHappinessParameters = this.m_CitizenHappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_TimeSettings = this.m_TimeSettingQuery.GetSingleton<TimeSettingsData>(),
        m_FeeParameters = this.__query_429327288_0.GetSingleton<ServiceFeeParameterData>(),
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_RawUpdateFrame = frameWithInterval,
        m_City = this.m_CitySystem.City,
        m_RandomSeed = RandomSeed.Next(),
        m_FactorQueue = this.m_FactorQueue.AsParallelWriter(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      }.ScheduleParallel<CitizenHappinessSystem.CitizenHappinessJob>(this.m_CitizenQuery, JobHandle.CombineDependencies(dependencies5, dependencies4, JobHandle.CombineDependencies(dependencies2, dependencies3, JobHandle.CombineDependencies(this.Dependency, dependencies1, deps))));
      // ISSUE: reference to a compiler-generated field
      if (this.m_DebugData.IsEnabled)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_DebugData.AddWriter(jobHandle);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LocalEffectSystem.AddLocalEffectReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CitizenHappinessSystem.HappinessFactorJob jobData = new CitizenHappinessSystem.HappinessFactorJob()
      {
        m_FactorQueue = this.m_FactorQueue,
        m_HappinessFactors = this.m_HappinessFactors,
        m_RawUpdateFrame = frameWithInterval,
        m_TriggerActionQueue = this.m_TriggerSystem.CreateActionBuffer(),
        m_ParameterEntity = this.m_HappinessFactorParameterQuery.GetSingletonEntity(),
        m_Parameters = this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RO_BufferLookup,
        m_Locked = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup
      };
      this.Dependency = jobData.Schedule<CitizenHappinessSystem.HappinessFactorJob>(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_LastDeps = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
    }

    private static TriggerType GetTriggerTypeForHappinessFactor(
      CitizenHappinessSystem.HappinessFactor factor)
    {
      switch (factor)
      {
        case CitizenHappinessSystem.HappinessFactor.Telecom:
          return TriggerType.TelecomHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Crime:
          return TriggerType.CrimeHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.AirPollution:
          return TriggerType.AirPollutionHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Apartment:
          return TriggerType.ApartmentHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Electricity:
          return TriggerType.ElectricityHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Healthcare:
          return TriggerType.HealthcareHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.GroundPollution:
          return TriggerType.GroundPollutionHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.NoisePollution:
          return TriggerType.NoisePollutionHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Water:
          return TriggerType.WaterHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.WaterPollution:
          return TriggerType.WaterPollutionHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Sewage:
          return TriggerType.SewageHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Garbage:
          return TriggerType.GarbageHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Entertainment:
          return TriggerType.EntertainmentHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Education:
          return TriggerType.EducationHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Mail:
          return TriggerType.MailHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Welfare:
          return TriggerType.WelfareHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Leisure:
          return TriggerType.LeisureHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Tax:
          return TriggerType.TaxHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Buildings:
          return TriggerType.BuildingsHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Consumption:
          return TriggerType.WealthHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.TrafficPenalty:
          return TriggerType.TrafficPenaltyHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.DeathPenalty:
          return TriggerType.DeathPenaltyHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.Homelessness:
          return TriggerType.HomelessnessHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.ElectricityFee:
          return TriggerType.ElectricityFeeHappinessFactor;
        case CitizenHappinessSystem.HappinessFactor.WaterFee:
          return TriggerType.WaterFeeHappinessFactor;
        default:
          UnityEngine.Debug.LogError((object) string.Format("Unknown trigger type for happiness factor: {0}", (object) factor));
          return TriggerType.NewNotification;
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(25);
      for (int index = 0; index < 400; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_HappinessFactors[index]);
      }
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version < Version.happinessFactorSerialization)
      {
        for (int index = 0; index < 352; ++index)
        {
          int4 int4;
          reader.Read(out int4);
          // ISSUE: reference to a compiler-generated field
          this.m_HappinessFactors[index] = int4;
        }
      }
      else
      {
        int num;
        reader.Read(out num);
        for (int index = 0; index < num * 16; ++index)
        {
          int4 int4;
          reader.Read(out int4);
          // ISSUE: reference to a compiler-generated field
          this.m_HappinessFactors[index] = int4;
        }
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      for (int index = 0; index < 400; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_HappinessFactors[index] = new int4();
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_429327288_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ServiceFeeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public CitizenHappinessSystem()
    {
    }

    public enum HappinessFactor
    {
      Telecom,
      Crime,
      AirPollution,
      Apartment,
      Electricity,
      Healthcare,
      GroundPollution,
      NoisePollution,
      Water,
      WaterPollution,
      Sewage,
      Garbage,
      Entertainment,
      Education,
      Mail,
      Welfare,
      Leisure,
      Tax,
      Buildings,
      Consumption,
      TrafficPenalty,
      DeathPenalty,
      Homelessness,
      ElectricityFee,
      WaterFee,
      Count,
    }

    private struct FactorItem
    {
      public CitizenHappinessSystem.HappinessFactor m_Factor;
      public int4 m_Value;
      public uint m_UpdateFrame;
    }

    [BurstCompile]
    private struct CitizenHappinessJob : IJobChunk
    {
      [NativeDisableContainerSafetyRestriction]
      public NativeQueue<int>.ParallelWriter m_DebugQueue;
      public bool m_DebugOn;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_HouseholdMemberType;
      [ReadOnly]
      public ComponentTypeHandle<CrimeVictim> m_CrimeVictimType;
      [ReadOnly]
      public ComponentTypeHandle<Criminal> m_CriminalType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> m_StudentType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_Properties;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumers;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumers;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_Transforms;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverages;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_Garbages;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<Locked> m_Locked;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> m_CrimeProducers;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducers;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildings;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public BufferLookup<ServiceFee> m_ServiceFees;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> m_Prisons;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> m_Schools;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> m_HomelessHouseholds;
      [ReadOnly]
      public NativeArray<GroundPollution> m_PollutionMap;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      [ReadOnly]
      public NativeArray<NoisePollution> m_NoisePollutionMap;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverage;
      [ReadOnly]
      public LocalEffectSystem.ReadData m_LocalEffectData;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      public HealthcareParameterData m_HealthcareParameters;
      public ParkParameterData m_ParkParameters;
      public EducationParameterData m_EducationParameters;
      public TelecomParameterData m_TelecomParameters;
      public GarbageParameterData m_GarbageParameters;
      public PoliceConfigurationData m_PoliceParameters;
      public CitizenHappinessParameterData m_CitizenHappinessParameters;
      public TimeSettingsData m_TimeSettings;
      public ServiceFeeParameterData m_FeeParameters;
      public TimeData m_TimeData;
      public Entity m_City;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public uint m_RawUpdateFrame;
      public NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter m_FactorQueue;
      public uint m_SimulationFrame;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

      private void AddData(float value)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DebugOn)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_DebugQueue.Enqueue(Mathf.RoundToInt(value));
      }

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray3 = chunk.GetNativeArray<HouseholdMember>(ref this.m_HouseholdMemberType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CrimeVictim> nativeArray4 = chunk.GetNativeArray<CrimeVictim>(ref this.m_CrimeVictimType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Criminal> nativeArray5 = chunk.GetNativeArray<Criminal>(ref this.m_CriminalType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Citizens.Student> nativeArray6 = chunk.GetNativeArray<Game.Citizens.Student>(ref this.m_StudentType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray7 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HealthProblem> nativeArray8 = chunk.GetNativeArray<HealthProblem>(ref this.m_HealthProblemType);
        // ISSUE: reference to a compiler-generated field
        EnabledMask enabledMask = chunk.GetEnabledMask<CrimeVictim>(ref this.m_CrimeVictimType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceFee> serviceFee = this.m_ServiceFees[this.m_City];
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        float relativeFee1 = ServiceFeeSystem.GetFee(PlayerResource.Electricity, serviceFee) / this.m_FeeParameters.m_ElectricityFee.m_Default;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        float relativeFee2 = ServiceFeeSystem.GetFee(PlayerResource.Water, serviceFee) / this.m_FeeParameters.m_WaterFee.m_Default;
        int4 int4_1 = new int4();
        int4 int4_2 = new int4();
        int4 int4_3 = new int4();
        int4 int4_4 = new int4();
        int4 int4_5 = new int4();
        int4 int4_6 = new int4();
        int4 int4_7 = new int4();
        int4 int4_8 = new int4();
        int4 int4_9 = new int4();
        int4 int4_10 = new int4();
        int4 int4_11 = new int4();
        int4 int4_12 = new int4();
        int4 int4_13 = new int4();
        int4 int4_14 = new int4();
        int4 int4_15 = new int4();
        int4 int4_16 = new int4();
        int4 int4_17 = new int4();
        int4 int4_18 = new int4();
        int4 int4_19 = new int4();
        int4 int4_20 = new int4();
        int4 int4_21 = new int4();
        int4 int4_22 = new int4();
        int4 int4_23 = new int4();
        int4 int4_24 = new int4();
        int4 int4_25 = new int4();
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        int num1 = 0;
        int num2 = 0;
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Entity household = nativeArray3[index1].m_Household;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Resources.HasBuffer(household))
            return;
          Citizen citizen = nativeArray2[index1];
          HealthProblem healthProblem;
          // ISSUE: reference to a compiler-generated field
          if ((!CollectionUtils.TryGet<HealthProblem>(nativeArray8, index1, out healthProblem) || !CitizenUtils.IsDead(healthProblem)) && ((this.m_Households[household].m_Flags & HouseholdFlags.MovedIn) != HouseholdFlags.None || (citizen.m_State & CitizenFlags.Tourist) != CitizenFlags.None))
          {
            Entity property1 = Entity.Null;
            Entity district = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Properties.HasComponent(household))
            {
              // ISSUE: reference to a compiler-generated field
              property1 = this.m_Properties[household].m_Property;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurrentDistrictData.HasComponent(property1))
              {
                // ISSUE: reference to a compiler-generated field
                district = this.m_CurrentDistrictData[property1].m_District;
              }
            }
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household];
            int children = 0;
            for (int index2 = 0; index2 < householdCitizen.Length; ++index2)
            {
              if (citizen.GetAge() == CitizenAge.Child)
                ++children;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int householdTotalWealth = EconomyUtils.GetHouseholdTotalWealth(this.m_Households[household], this.m_Resources[household]);
            int2 int2_1 = householdTotalWealth > 0 ? new int2(0, math.min(15, householdTotalWealth / 1000)) : new int2();
            int4_22.x += int2_1.x + int2_1.y;
            ++int4_22.y;
            int4_22.z += int2_1.x;
            int4_22.w += int2_1.y;
            int2 int2_2 = (int2) 0;
            Criminal criminal;
            CurrentBuilding currentBuilding;
            Game.Buildings.Prison componentData1;
            // ISSUE: reference to a compiler-generated field
            if (CollectionUtils.TryGet<Criminal>(nativeArray5, index1, out criminal) && (criminal.m_Flags & CriminalFlags.Prisoner) != (CriminalFlags) 0 && CollectionUtils.TryGet<CurrentBuilding>(nativeArray7, index1, out currentBuilding) && this.m_Prisons.TryGetComponent(currentBuilding.m_CurrentBuilding, out componentData1))
              int2_2 += new int2((int) componentData1.m_PrisonerHealth, (int) componentData1.m_PrisonerWellbeing);
            Game.Citizens.Student student;
            Game.Buildings.School componentData2;
            // ISSUE: reference to a compiler-generated field
            if (CollectionUtils.TryGet<Game.Citizens.Student>(nativeArray6, index1, out student) && this.m_Schools.TryGetComponent(student.m_School, out componentData2))
              int2_2 += new int2((int) componentData2.m_StudentHealth, (int) componentData2.m_StudentWellbeing);
            int4_21 += new int4(int2_2.x + int2_2.y, 1, int2_2.x, int2_2.y);
            int2 int2_3 = new int2(0, 0);
            int2 int2_4 = new int2();
            int2 int2_5 = new int2();
            int2 int2_6 = new int2();
            int2 int2_7 = new int2();
            int2 int2_8 = new int2();
            int2 int2_9 = new int2();
            int2 int2_10 = new int2();
            int2 int2_11 = new int2();
            int2 int2_12 = new int2();
            int2 int2_13 = new int2();
            int2 int2_14 = new int2();
            int2 int2_15 = new int2();
            int2 int2_16 = new int2();
            int2 int2_17 = new int2();
            int2 int2_18 = new int2();
            int2 int2_19 = new int2();
            int2 int2_20 = new int2();
            int2 int2_21 = new int2();
            int2 int2_22 = new int2();
            CrimeVictim crimeVictim = new CrimeVictim();
            if (enabledMask[index1])
              crimeVictim = nativeArray4[index1];
            PropertyRenter componentData3;
            PrefabRef componentData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Properties.TryGetComponent(household, out componentData3) && this.m_Prefabs.TryGetComponent(componentData3.m_Property, out componentData4))
            {
              Entity prefab = componentData4.m_Prefab;
              Entity property2 = componentData3.m_Property;
              // ISSUE: reference to a compiler-generated field
              Entity healthcareServicePrefab = this.m_HealthcareParameters.m_HealthcareServicePrefab;
              // ISSUE: reference to a compiler-generated field
              Entity parkServicePrefab = this.m_ParkParameters.m_ParkServicePrefab;
              // ISSUE: reference to a compiler-generated field
              Entity educationServicePrefab = this.m_EducationParameters.m_EducationServicePrefab;
              // ISSUE: reference to a compiler-generated field
              Entity telecomServicePrefab = this.m_TelecomParameters.m_TelecomServicePrefab;
              // ISSUE: reference to a compiler-generated field
              Entity garbageServicePrefab = this.m_GarbageParameters.m_GarbageServicePrefab;
              // ISSUE: reference to a compiler-generated field
              Entity policeServicePrefab = this.m_PoliceParameters.m_PoliceServicePrefab;
              Entity roadEdge = Entity.Null;
              float curvePosition = 0.0f;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Buildings.HasComponent(property2))
              {
                // ISSUE: reference to a compiler-generated field
                Building building = this.m_Buildings[property2];
                roadEdge = building.m_RoadEdge;
                curvePosition = building.m_CurvePosition;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_4 = CitizenHappinessSystem.GetElectricitySupplyBonuses(property2, ref this.m_ElectricityConsumers, in this.m_CitizenHappinessParameters);
              int4_5.x += int2_4.x + int2_4.y;
              int4_5.z += int2_4.x;
              int4_5.w += int2_4.y;
              ++int4_5.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_5 = CitizenHappinessSystem.GetElectricityFeeBonuses(property2, ref this.m_ElectricityConsumers, relativeFee1, in this.m_CitizenHappinessParameters);
              int4_6.x += int2_5.x + int2_5.y;
              int4_6.z += int2_5.x;
              int4_6.w += int2_5.y;
              ++int4_6.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_10 = CitizenHappinessSystem.GetWaterSupplyBonuses(property2, ref this.m_WaterConsumers, in this.m_CitizenHappinessParameters);
              int4_10.x += int2_10.x + int2_10.y;
              int4_10.z += int2_10.x;
              int4_10.w += int2_10.y;
              ++int4_10.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_11 = CitizenHappinessSystem.GetWaterFeeBonuses(property2, ref this.m_WaterConsumers, relativeFee2, in this.m_CitizenHappinessParameters);
              int4_11.x += int2_11.x + int2_11.y;
              int4_11.z += int2_11.x;
              int4_11.w += int2_11.y;
              ++int4_11.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_12 = CitizenHappinessSystem.GetWaterPollutionBonuses(property2, ref this.m_WaterConsumers, cityModifier, in this.m_CitizenHappinessParameters);
              int4_12.x += int2_12.x + int2_12.y;
              int4_12.z += int2_12.x;
              int4_12.w += int2_12.y;
              ++int4_12.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_13 = CitizenHappinessSystem.GetSewageBonuses(property2, ref this.m_WaterConsumers, in this.m_CitizenHappinessParameters);
              int4_13.x += int2_13.x + int2_13.y;
              int4_13.z += int2_13.x;
              int4_13.w += int2_13.y;
              ++int4_13.y;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ServiceCoverages.HasBuffer(roadEdge))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage = this.m_ServiceCoverages[roadEdge];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int2_6 = CitizenHappinessSystem.GetHealthcareBonuses(curvePosition, serviceCoverage, ref this.m_Locked, healthcareServicePrefab, in this.m_CitizenHappinessParameters);
                int4_7.x += int2_6.x + int2_6.y;
                int4_7.z += int2_6.x;
                int4_7.w += int2_6.y;
                ++int4_7.y;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int2_16 = CitizenHappinessSystem.GetEntertainmentBonuses(curvePosition, serviceCoverage, cityModifier, ref this.m_Locked, parkServicePrefab, in this.m_CitizenHappinessParameters);
                int4_15.x += int2_16.x + int2_16.y;
                int4_15.z += int2_16.x;
                int4_15.w += int2_16.y;
                ++int4_15.y;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int2_17 = CitizenHappinessSystem.GetEducationBonuses(curvePosition, serviceCoverage, ref this.m_Locked, educationServicePrefab, in this.m_CitizenHappinessParameters, children);
                int4_16.x += int2_17.x + int2_17.y;
                int4_16.z += int2_17.x;
                int4_16.w += int2_17.y;
                ++int4_16.y;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int2_20 = CitizenHappinessSystem.GetWellfareBonuses(curvePosition, serviceCoverage, in this.m_CitizenHappinessParameters, citizen.Happiness);
                int4_18.x += int2_20.x + int2_20.y;
                int4_18.z += int2_20.x;
                int4_18.w += int2_20.y;
                ++int4_18.y;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_7 = CitizenHappinessSystem.GetGroundPollutionBonuses(property2, ref this.m_Transforms, this.m_PollutionMap, cityModifier, in this.m_CitizenHappinessParameters);
              int4_8.x += int2_7.x + int2_7.y;
              int4_8.z += int2_7.x;
              int4_8.w += int2_7.y;
              ++int4_8.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_8 = CitizenHappinessSystem.GetAirPollutionBonuses(property2, ref this.m_Transforms, this.m_AirPollutionMap, cityModifier, in this.m_CitizenHappinessParameters);
              int4_3.x += int2_8.x + int2_8.y;
              int4_3.z += int2_8.x;
              int4_3.w += int2_8.y;
              ++int4_3.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_9 = CitizenHappinessSystem.GetNoiseBonuses(property2, ref this.m_Transforms, this.m_NoisePollutionMap, in this.m_CitizenHappinessParameters);
              int4_9.x += int2_9.x + int2_9.y;
              int4_9.z += int2_9.x;
              int4_9.w += int2_9.y;
              ++int4_9.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_14 = CitizenHappinessSystem.GetGarbageBonuses(property2, ref this.m_Garbages, ref this.m_Locked, garbageServicePrefab, in this.m_GarbageParameters);
              int4_14.x += int2_14.x + int2_14.y;
              int4_14.z += int2_14.x;
              int4_14.w += int2_14.y;
              ++int4_14.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_15 = CitizenHappinessSystem.GetCrimeBonuses(crimeVictim, property2, ref this.m_CrimeProducers, ref this.m_Locked, policeServicePrefab, in this.m_CitizenHappinessParameters);
              int4_1.x += int2_15.x + int2_15.y;
              int4_1.z += int2_15.x;
              int4_1.w += int2_15.y;
              ++int4_1.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_18 = CitizenHappinessSystem.GetMailBonuses(property2, ref this.m_MailProducers, ref this.m_Locked, telecomServicePrefab, in this.m_CitizenHappinessParameters);
              int4_17.x += int2_18.x + int2_18.y;
              int4_17.z += int2_18.x;
              int4_17.w += int2_18.y;
              ++int4_17.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_19 = CitizenHappinessSystem.GetTelecomBonuses(property2, ref this.m_Transforms, this.m_TelecomCoverage, ref this.m_Locked, telecomServicePrefab, in this.m_CitizenHappinessParameters);
              int4_2.x += int2_19.x + int2_19.y;
              int4_2.z += int2_19.x;
              int4_2.w += int2_19.y;
              ++int4_2.y;
              ++int4_25.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnableBuildings.HasComponent(prefab) && this.m_BuildingDatas.HasComponent(prefab) && this.m_BuildingPropertyDatas.HasComponent(prefab) && !this.m_HomelessHouseholds.HasComponent(household))
              {
                // ISSUE: reference to a compiler-generated field
                SpawnableBuildingData spawnableBuilding = this.m_SpawnableBuildings[prefab];
                // ISSUE: reference to a compiler-generated field
                Game.Prefabs.BuildingData buildingData = this.m_BuildingDatas[prefab];
                // ISSUE: reference to a compiler-generated field
                BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
                float sizePerResident = buildingPropertyData.m_SpaceMultiplier * (float) buildingData.m_LotSize.x * (float) buildingData.m_LotSize.y / (float) (householdCitizen.Length * buildingPropertyData.m_ResidentialProperties);
                // ISSUE: reference to a compiler-generated method
                int2_3.y = Mathf.RoundToInt(CitizenHappinessSystem.GetApartmentWellbeing(sizePerResident, (int) spawnableBuilding.m_Level));
                int4_4.x += int2_3.x + int2_3.y;
                int4_4.z += int2_3.x;
                int4_4.w += int2_3.y;
                ++int4_4.y;
                // ISSUE: reference to a compiler-generated method
                this.AddData(math.min(100f, 100f * sizePerResident));
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                int2_3.y = Mathf.RoundToInt(CitizenHappinessSystem.GetApartmentWellbeing(0.01f, 1));
                int4_4.x += int2_3.y;
                int4_4.w += int2_3.y;
                ++int4_4.y;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int2 homelessBonuses = CitizenHappinessSystem.GetHomelessBonuses(in this.m_CitizenHappinessParameters);
                int4_25.x += homelessBonuses.x + homelessBonuses.y;
                int4_25.z += homelessBonuses.x;
                int4_25.w += homelessBonuses.y;
              }
            }
            bool flag = (citizen.m_State & CitizenFlags.Tourist) != 0;
            if ((double) random.NextFloat() < 0.019999999552965164 * (flag ? 10.0 : 1.0))
              citizen.m_LeisureCounter = (byte) math.min((int) byte.MaxValue, math.max(0, (int) citizen.m_LeisureCounter - 1));
            citizen.m_PenaltyCounter = (byte) math.max(0, (int) citizen.m_PenaltyCounter - 1);
            // ISSUE: reference to a compiler-generated method
            int2 leisureBonuses = CitizenHappinessSystem.GetLeisureBonuses(citizen.m_LeisureCounter);
            int4_19.x += leisureBonuses.x + leisureBonuses.y;
            int4_19.z += leisureBonuses.x;
            int4_19.w += leisureBonuses.y;
            ++int4_19.y;
            if (!flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int2_21 = CitizenHappinessSystem.GetTaxBonuses(citizen.GetEducationLevel(), this.m_TaxRates, in this.m_CitizenHappinessParameters);
            }
            int4_20.x += int2_21.x + int2_21.y;
            int4_20.z += int2_21.x;
            int4_20.w += int2_21.y;
            ++int4_20.y;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int2 sicknessBonuses = CitizenHappinessSystem.GetSicknessBonuses(nativeArray8.Length != 0, in this.m_CitizenHappinessParameters);
            int4_7.x += sicknessBonuses.x + sicknessBonuses.y;
            int4_7.z += sicknessBonuses.x;
            int4_7.w += sicknessBonuses.y;
            ++int4_7.y;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int2 deathPenalty = CitizenHappinessSystem.GetDeathPenalty(householdCitizen, ref this.m_HealthProblems, in this.m_CitizenHappinessParameters);
            int4_24.x += deathPenalty.x + deathPenalty.y;
            int4_24.z += deathPenalty.x;
            int4_24.w += deathPenalty.y;
            ++int4_24.y;
            // ISSUE: reference to a compiler-generated field
            int penaltyEffect = citizen.m_PenaltyCounter > (byte) 0 ? this.m_CitizenHappinessParameters.m_PenaltyEffect : 0;
            int4_23.x += penaltyEffect;
            int4_23.w += penaltyEffect;
            ++int4_23.y;
            int num3 = math.max(0, 50 + penaltyEffect + deathPenalty.y + int2_1.y + int2_4.y + int2_5.y + int2_10.y + int2_11.y + int2_13.y + int2_6.y + leisureBonuses.y + int2_2.y + int2_12.y + int2_9.y + int2_14.y + int2_15.y + int2_16.y + int2_18.y + int2_17.y + int2_19.y + int2_3.y + int2_20.y + int2_21.y);
            int num4 = 50 + int2_6.x + sicknessBonuses.x + deathPenalty.x + int2_2.x + int2_7.x + int2_8.x + int2_4.x + int2_10.x + int2_13.x + int2_12.x + int2_14.x + int2_3.x + int2_20.x;
            float f1 = (float) num3;
            float f2 = (float) num4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Transforms.HasComponent(property1))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Objects.Transform transform = this.m_Transforms[property1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_LocalEffectData.ApplyModifier(ref f1, transform.m_Position, LocalModifierType.Wellbeing);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_LocalEffectData.ApplyModifier(ref f2, transform.m_Position, LocalModifierType.Health);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_DistrictModifiers.HasBuffer(district))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<DistrictModifier> districtModifier = this.m_DistrictModifiers[district];
              AreaUtils.ApplyModifier(ref f1, districtModifier, DistrictModifierType.Wellbeing);
            }
            int num5 = Mathf.RoundToInt(f1);
            int num6 = Mathf.RoundToInt(f2);
            int num7 = random.NextInt(100) > 50 + (int) citizen.m_WellBeing - num5 ? 1 : -1;
            citizen.m_WellBeing = (byte) math.max(0, math.min(100, (int) citizen.m_WellBeing + num7));
            int num8 = random.NextInt(100) > 50 + (int) citizen.m_Health - num6 ? 1 : -1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int maxHealth = CitizenHappinessSystem.GetMaxHealth(citizen.GetAgeInDays(this.m_SimulationFrame, this.m_TimeData) / (float) this.m_TimeSettings.m_DaysPerYear);
            citizen.m_Health = (byte) math.max(0, math.min(maxHealth, (int) citizen.m_Health + num8));
            // ISSUE: reference to a compiler-generated field
            if ((int) citizen.m_WellBeing < this.m_CitizenHappinessParameters.m_LowWellbeing)
              ++num1;
            // ISSUE: reference to a compiler-generated field
            if ((int) citizen.m_Health < this.m_CitizenHappinessParameters.m_LowHealth)
              ++num2;
            nativeArray2[index1] = citizen;
          }
        }
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local1 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Telecom;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_2;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem2 = factorItem1;
        local1.Enqueue(factorItem2);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local2 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Crime;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_1;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem3 = factorItem1;
        local2.Enqueue(factorItem3);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local3 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.AirPollution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_3;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem4 = factorItem1;
        local3.Enqueue(factorItem4);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local4 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Apartment;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_4;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem5 = factorItem1;
        local4.Enqueue(factorItem5);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local5 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Electricity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_5;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem6 = factorItem1;
        local5.Enqueue(factorItem6);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local6 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.ElectricityFee;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_6;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem7 = factorItem1;
        local6.Enqueue(factorItem7);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local7 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Healthcare;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_7;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem8 = factorItem1;
        local7.Enqueue(factorItem8);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local8 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.GroundPollution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_8;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem9 = factorItem1;
        local8.Enqueue(factorItem9);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local9 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.NoisePollution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_9;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem10 = factorItem1;
        local9.Enqueue(factorItem10);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local10 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Water;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_10;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem11 = factorItem1;
        local10.Enqueue(factorItem11);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local11 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.WaterFee;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_11;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem12 = factorItem1;
        local11.Enqueue(factorItem12);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local12 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.WaterPollution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_12;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem13 = factorItem1;
        local12.Enqueue(factorItem13);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local13 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Sewage;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_13;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem14 = factorItem1;
        local13.Enqueue(factorItem14);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local14 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Garbage;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_14;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem15 = factorItem1;
        local14.Enqueue(factorItem15);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local15 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Entertainment;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_15;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem16 = factorItem1;
        local15.Enqueue(factorItem16);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local16 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Education;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_16;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem17 = factorItem1;
        local16.Enqueue(factorItem17);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local17 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Mail;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_17;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem18 = factorItem1;
        local17.Enqueue(factorItem18);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local18 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Welfare;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_18;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem19 = factorItem1;
        local18.Enqueue(factorItem19);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local19 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Leisure;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_19;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem20 = factorItem1;
        local19.Enqueue(factorItem20);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local20 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Tax;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_20;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem21 = factorItem1;
        local20.Enqueue(factorItem21);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local21 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Buildings;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_21;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem22 = factorItem1;
        local21.Enqueue(factorItem22);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local22 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Consumption;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_22;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem23 = factorItem1;
        local22.Enqueue(factorItem23);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local23 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.TrafficPenalty;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_23;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem24 = factorItem1;
        local23.Enqueue(factorItem24);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local24 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.DeathPenalty;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_24;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem25 = factorItem1;
        local24.Enqueue(factorItem25);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<CitizenHappinessSystem.FactorItem>.ParallelWriter local25 = ref this.m_FactorQueue;
        // ISSUE: object of a compiler-generated type is created
        factorItem1 = new CitizenHappinessSystem.FactorItem();
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Factor = CitizenHappinessSystem.HappinessFactor.Homelessness;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_UpdateFrame = this.m_RawUpdateFrame;
        // ISSUE: reference to a compiler-generated field
        factorItem1.m_Value = int4_25;
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem26 = factorItem1;
        local25.Enqueue(factorItem26);
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.WellbeingLevel,
          m_Change = (float) num1
        });
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.HealthLevel,
          m_Change = (float) num2
        });
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct HappinessFactorJob : IJob
    {
      public NativeArray<int4> m_HappinessFactors;
      public NativeQueue<CitizenHappinessSystem.FactorItem> m_FactorQueue;
      public NativeQueue<TriggerAction> m_TriggerActionQueue;
      public uint m_RawUpdateFrame;
      public Entity m_ParameterEntity;
      [ReadOnly]
      public BufferLookup<HappinessFactorParameterData> m_Parameters;
      [ReadOnly]
      public ComponentLookup<Locked> m_Locked;

      public void Execute()
      {
        for (int factor = 0; factor < 25; ++factor)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_HappinessFactors[CitizenHappinessSystem.GetFactorIndex((CitizenHappinessSystem.HappinessFactor) factor, this.m_RawUpdateFrame)] = new int4();
        }
        // ISSUE: variable of a compiler-generated type
        CitizenHappinessSystem.FactorItem factorItem;
        // ISSUE: reference to a compiler-generated field
        while (this.m_FactorQueue.TryDequeue(out factorItem))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((int) factorItem.m_UpdateFrame != (int) this.m_RawUpdateFrame)
            UnityEngine.Debug.LogWarning((object) "Different updateframe in HappinessFactorJob than in its queue");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          this.m_HappinessFactors[CitizenHappinessSystem.GetFactorIndex(factorItem.m_Factor, factorItem.m_UpdateFrame)] += factorItem.m_Value;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HappinessFactorParameterData> parameter = this.m_Parameters[this.m_ParameterEntity];
        for (int factor = 0; factor < 25; ++factor)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TriggerActionQueue.Enqueue(new TriggerAction(CitizenHappinessSystem.GetTriggerTypeForHappinessFactor((CitizenHappinessSystem.HappinessFactor) factor), Entity.Null, CitizenHappinessSystem.GetHappinessFactor((CitizenHappinessSystem.HappinessFactor) factor, this.m_HappinessFactors, parameter, ref this.m_Locked).x));
        }
      }
    }

    private struct TypeHandle
    {
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CrimeVictim> __Game_Citizens_CrimeVictim_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Criminal> __Game_Citizens_Criminal_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceFee> __Game_City_ServiceFee_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> __Game_Buildings_Prison_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> __Game_Buildings_School_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> __Game_Citizens_HomelessHousehold_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HappinessFactorParameterData> __Game_Prefabs_HappinessFactorParameterData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CrimeVictim_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CrimeVictim>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Criminal_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Criminal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentLookup = state.GetComponentLookup<CrimeProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_ServiceFee_RO_BufferLookup = state.GetBufferLookup<ServiceFee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Prison_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Prison>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_School_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.School>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HomelessHousehold_RO_ComponentLookup = state.GetComponentLookup<HomelessHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HappinessFactorParameterData_RO_BufferLookup = state.GetBufferLookup<HappinessFactorParameterData>(true);
      }
    }
  }
}
