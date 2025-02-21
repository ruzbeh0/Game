// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ZoneEvaluationUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using Game.Net;
using Game.Prefabs;
using Game.Zones;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public class ZoneEvaluationUtils
  {
    private static float GetFactor(
      DynamicBuffer<ResourceAvailability> availabilities,
      float curvePos,
      AvailableResource resource)
    {
      return math.min(20f, 0.2f / NetUtils.GetAvailability(availabilities, resource, curvePos));
    }

    public static void GetFactors(
      AreaType areaType,
      bool office,
      DynamicBuffer<ResourceAvailability> availabilities,
      float curvePos,
      ref ZonePreferenceData preferences,
      NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> results,
      NativeArray<int> resourceDemands,
      float pollution,
      float landvalue,
      DynamicBuffer<ProcessEstimate> estimates,
      NativeList<IndustrialProcessData> processes,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas)
    {
      switch (areaType)
      {
        case AreaType.Residential:
          float factor1 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Services);
          float factor2 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Workplaces);
          ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local1 = ref results;
          ZoneEvaluationUtils.ZoningEvaluationResult evaluationResult1 = new ZoneEvaluationUtils.ZoningEvaluationResult();
          evaluationResult1.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Services;
          evaluationResult1.m_Score = (float) (20.0 * (double) preferences.m_ResidentialSignificanceServices * (0.20000000298023224 - (double) factor1));
          ref ZoneEvaluationUtils.ZoningEvaluationResult local2 = ref evaluationResult1;
          local1.Add(in local2);
          ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local3 = ref results;
          evaluationResult1 = new ZoneEvaluationUtils.ZoningEvaluationResult();
          evaluationResult1.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Workplaces;
          evaluationResult1.m_Score = (float) (20.0 * (double) preferences.m_ResidentialSignificanceWorkplaces * (0.20000000298023224 - (double) factor2));
          ref ZoneEvaluationUtils.ZoningEvaluationResult local4 = ref evaluationResult1;
          local3.Add(in local4);
          ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local5 = ref results;
          evaluationResult1 = new ZoneEvaluationUtils.ZoningEvaluationResult();
          evaluationResult1.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Pollution;
          evaluationResult1.m_Score = (float) (20.0 + (double) preferences.m_ResidentialSignificancePollution * (double) pollution);
          ref ZoneEvaluationUtils.ZoningEvaluationResult local6 = ref evaluationResult1;
          local5.Add(in local6);
          ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local7 = ref results;
          evaluationResult1 = new ZoneEvaluationUtils.ZoningEvaluationResult();
          evaluationResult1.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.LandValue;
          evaluationResult1.m_Score = preferences.m_ResidentialSignificanceLandValue * (landvalue - preferences.m_ResidentialNeutralLandValue);
          ref ZoneEvaluationUtils.ZoningEvaluationResult local8 = ref evaluationResult1;
          local7.Add(in local8);
          break;
        case AreaType.Commercial:
          float factor3 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.UneducatedCitizens);
          float factor4 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.EducatedCitizens);
          float factor5 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Workplaces);
          ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local9 = ref results;
          ZoneEvaluationUtils.ZoningEvaluationResult evaluationResult2 = new ZoneEvaluationUtils.ZoningEvaluationResult();
          evaluationResult2.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Customers;
          evaluationResult2.m_Score = math.max(preferences.m_CommercialSignificanceConsumers * (2f - math.lerp(factor3, factor4, 0.67f)), preferences.m_CommercialSignificanceWorkplaces * (2f - factor5));
          ref ZoneEvaluationUtils.ZoningEvaluationResult local10 = ref evaluationResult2;
          local9.Add(in local10);
          float factor6 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Services);
          ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local11 = ref results;
          evaluationResult2 = new ZoneEvaluationUtils.ZoningEvaluationResult();
          evaluationResult2.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Competitors;
          evaluationResult2.m_Score = preferences.m_CommercialSignificanceCompetitors * (factor6 - 0.4f);
          ref ZoneEvaluationUtils.ZoningEvaluationResult local12 = ref evaluationResult2;
          local11.Add(in local12);
          ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local13 = ref results;
          evaluationResult2 = new ZoneEvaluationUtils.ZoningEvaluationResult();
          evaluationResult2.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.LandValue;
          evaluationResult2.m_Score = preferences.m_CommercialSignificanceLandValue * (landvalue - preferences.m_CommercialNeutralLandValue);
          ref ZoneEvaluationUtils.ZoningEvaluationResult local14 = ref evaluationResult2;
          local13.Add(in local14);
          break;
        case AreaType.Industrial:
          if (!office)
          {
            ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local15 = ref results;
            ZoneEvaluationUtils.ZoningEvaluationResult evaluationResult3 = new ZoneEvaluationUtils.ZoningEvaluationResult();
            evaluationResult3.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Inputs;
            evaluationResult3.m_Score = preferences.m_IndustrialSignificanceInput * ZoneEvaluationUtils.GetTransportScore(Resource.All, processes, availabilities, resourceDemands, curvePos, resourcePrefabs, ref resourceDatas);
            ref ZoneEvaluationUtils.ZoningEvaluationResult local16 = ref evaluationResult3;
            local15.Add(in local16);
            ref NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> local17 = ref results;
            evaluationResult3 = new ZoneEvaluationUtils.ZoningEvaluationResult();
            evaluationResult3.m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.LandValue;
            evaluationResult3.m_Score = preferences.m_IndustrialSignificanceLandValue * (landvalue - preferences.m_IndustrialNeutralLandValue);
            ref ZoneEvaluationUtils.ZoningEvaluationResult local18 = ref evaluationResult3;
            local17.Add(in local18);
            break;
          }
          float factor7 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.EducatedCitizens);
          float factor8 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Services);
          results.Add(new ZoneEvaluationUtils.ZoningEvaluationResult()
          {
            m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Employees,
            m_Score = preferences.m_OfficeSignificanceEmployees * (0.2f - factor7)
          });
          results.Add(new ZoneEvaluationUtils.ZoningEvaluationResult()
          {
            m_Factor = ZoneEvaluationUtils.ZoningEvaluationFactor.Services,
            m_Score = preferences.m_OfficeSignificanceServices * (0.2f - factor8)
          });
          break;
      }
    }

    private static float GetStorageScore(
      Resource resource,
      float price,
      DynamicBuffer<ResourceAvailability> availabilities,
      float curvePos)
    {
      float num1 = (float) (1.0 / ((double) price * (double) math.max(0.1f, NetUtils.GetAvailability(availabilities, EconomyUtils.GetAvailableResourceSupply(resource), curvePos))));
      float num2 = 0.0f;
      int num3 = 0;
      ResourceIterator iterator = ResourceIterator.GetIterator();
      while (iterator.Next())
      {
        if (EconomyUtils.IsProducedFrom(iterator.resource, resource) && EconomyUtils.GetAvailableResourceSupply(iterator.resource) != AvailableResource.Count)
        {
          num2 += (float) (1.0 / ((double) price * (double) math.max(0.1f, NetUtils.GetAvailability(availabilities, EconomyUtils.GetAvailableResourceSupply(iterator.resource), curvePos))));
          ++num3;
        }
      }
      if (num3 == 0)
      {
        num2 = (float) (1.0 / ((double) price * (double) math.max(0.1f, NetUtils.GetAvailability(availabilities, AvailableResource.ConvenienceFoodStore, curvePos))));
        num3 = 1;
      }
      return num1 + num2 / (float) num3;
    }

    private static float GetTransportScore(
      Resource allowedManufactured,
      NativeList<IndustrialProcessData> processes,
      DynamicBuffer<ResourceAvailability> availabilities,
      NativeArray<int> resourceDemands,
      float curvePos,
      ResourcePrefabs resourcePrefabs,
      ref ComponentLookup<ResourceData> resourceDatas)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      ResourceIterator iterator1 = ResourceIterator.GetIterator();
      while (iterator1.Next())
      {
        if ((allowedManufactured & iterator1.resource) != Resource.NoResource)
        {
          ResourceIterator iterator2 = ResourceIterator.GetIterator();
          while (iterator2.Next())
          {
            if (EconomyUtils.IsProducedFrom(iterator1.resource, iterator2.resource) && EconomyUtils.GetAvailableResourceSupply(iterator2.resource) != AvailableResource.Count)
            {
              EconomyUtils.GetResourceIndex(iterator2.resource);
              for (int index = 0; index < processes.Length; ++index)
              {
                IndustrialProcessData process = processes[index];
                if (process.m_Output.m_Resource == iterator1.resource && process.m_Input1.m_Resource != process.m_Output.m_Resource)
                {
                  int num3 = Mathf.Max(process.m_Input1.m_Resource == iterator2.resource ? process.m_Input1.m_Amount : 0, process.m_Input2.m_Resource == iterator2.resource ? process.m_Input2.m_Amount : 0);
                  int num4 = resourceDemands[EconomyUtils.GetResourceIndex(iterator1.resource)] + 1;
                  float num5 = math.max(0.1f, NetUtils.GetAvailability(availabilities, EconomyUtils.GetAvailableResourceSupply(iterator2.resource), curvePos));
                  if (num3 > 0 && num4 > 0 && (double) num5 > 0.0)
                  {
                    float num6 = (float) num3 / ((float) process.m_Output.m_Amount * num5);
                    float num7 = math.min(5f, EconomyUtils.GetMarketPrice(iterator2.resource, resourcePrefabs, ref resourceDatas));
                    if ((double) num6 < 0.30000001192092896 * (double) num7)
                    {
                      num1 += (float) num4 * (0.3f * num7 - num6);
                      num2 += (float) num4 * 0.3f * num7;
                    }
                  }
                }
              }
            }
          }
        }
      }
      return (double) num2 > 0.0 ? (float) ((double) num1 / (double) num2 - 0.5) : 0.0f;
    }

    public static float GetResidentialScore(
      DynamicBuffer<ResourceAvailability> availabilities,
      float curvePos,
      ref ZonePreferenceData preferences,
      float landValue,
      float pollution)
    {
      return 555f + ((float) (-(double) preferences.m_ResidentialSignificanceServices / (double) math.max(0.1f, NetUtils.GetAvailability(availabilities, AvailableResource.Services, curvePos)) - (double) preferences.m_ResidentialSignificanceWorkplaces / (double) math.max(0.1f, NetUtils.GetAvailability(availabilities, AvailableResource.Workplaces, curvePos))) + preferences.m_ResidentialSignificancePollution * pollution + preferences.m_ResidentialSignificanceLandValue * (landValue - preferences.m_ResidentialNeutralLandValue));
    }

    public static float GetCommercialScore(
      DynamicBuffer<ResourceAvailability> availabilities,
      float curvePos,
      ref ZonePreferenceData preferences,
      float landValue,
      bool lodging)
    {
      float num = 0.0f;
      float factor1 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.UneducatedCitizens);
      float factor2 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.EducatedCitizens);
      float factor3 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Workplaces);
      float factor4 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Services);
      return 555f + (num + math.max(preferences.m_CommercialSignificanceConsumers * (2f - math.lerp(factor1, factor2, 0.67f)), preferences.m_CommercialSignificanceWorkplaces * (2f - factor3)) + preferences.m_CommercialSignificanceCompetitors * (factor4 - 0.4f) + preferences.m_CommercialSignificanceLandValue * (landValue - preferences.m_CommercialNeutralLandValue));
    }

    public static float GetScore(
      AreaType areaType,
      bool office,
      DynamicBuffer<ResourceAvailability> availabilities,
      float curvePos,
      ref ZonePreferenceData preferences,
      bool storage,
      NativeArray<int> resourceDemands,
      BuildingPropertyData propertyData,
      float pollution,
      float landValue,
      DynamicBuffer<ProcessEstimate> estimates,
      NativeList<IndustrialProcessData> processes,
      ResourcePrefabs resourcePrefabs,
      ref ComponentLookup<ResourceData> resourceDatas)
    {
      switch (areaType)
      {
        case AreaType.Residential:
          return ZoneEvaluationUtils.GetResidentialScore(availabilities, curvePos, ref preferences, landValue, pollution);
        case AreaType.Commercial:
          return (float) (0.89999997615814209 * (double) ZoneEvaluationUtils.GetCommercialScore(availabilities, curvePos, ref preferences, landValue, false) + 0.10000000149011612 * (double) ZoneEvaluationUtils.GetCommercialScore(availabilities, curvePos, ref preferences, landValue, true));
        case AreaType.Industrial:
          if (storage)
          {
            Resource allowedStored = propertyData.m_AllowedStored;
            ResourceIterator iterator = ResourceIterator.GetIterator();
            float x = float.PositiveInfinity;
            while (iterator.Next())
            {
              if ((allowedStored & iterator.resource) != Resource.NoResource && resourceDemands[EconomyUtils.GetResourceIndex(iterator.resource)] != 0)
              {
                double weight = (double) EconomyUtils.GetWeight(iterator.resource, resourcePrefabs, ref resourceDatas);
                float marketPrice = EconomyUtils.GetMarketPrice(iterator.resource, resourcePrefabs, ref resourceDatas);
                x = math.min(math.min(x, ZoneEvaluationUtils.GetStorageScore(iterator.resource, marketPrice, availabilities, curvePos)), 0.05f / math.max(0.1f, NetUtils.GetAvailability(availabilities, EconomyUtils.GetAvailableResourceSupply(iterator.resource), curvePos)));
              }
            }
            return math.max(0.0f, (float) (555.0 - 10.0 * (double) x));
          }
          if (!office)
            return (float) (555.0 + (double) preferences.m_IndustrialSignificanceInput * (double) ZoneEvaluationUtils.GetTransportScore(propertyData.m_AllowedManufactured, processes, availabilities, resourceDemands, curvePos, resourcePrefabs, ref resourceDatas) + (double) preferences.m_IndustrialSignificanceLandValue * ((double) landValue - (double) preferences.m_IndustrialNeutralLandValue) - 0.5 * (double) landValue);
          float factor1 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.EducatedCitizens);
          float factor2 = ZoneEvaluationUtils.GetFactor(availabilities, curvePos, AvailableResource.Services);
          return (float) (555.0 + (double) preferences.m_OfficeSignificanceEmployees * (0.25 - 5.0 * (double) factor1) + (double) preferences.m_OfficeSignificanceServices * (0.25 - 2.0 * (double) factor2));
        default:
          return 0.0f;
      }
    }

    [Serializable]
    public enum ZoningEvaluationFactor
    {
      None,
      Workplaces,
      Services,
      Competitors,
      Customers,
      OutsideConnections,
      Inputs,
      Pollution,
      LandValue,
      Employees,
      Count,
    }

    public struct ZoningEvaluationResult : IComparable<ZoneEvaluationUtils.ZoningEvaluationResult>
    {
      public ZoneEvaluationUtils.ZoningEvaluationFactor m_Factor;
      public float m_Score;

      public int CompareTo(ZoneEvaluationUtils.ZoningEvaluationResult other)
      {
        int num = -Mathf.Abs(this.m_Score).CompareTo(Mathf.Abs(other.m_Score));
        return num != 0 ? num : -Mathf.Sign(this.m_Score).CompareTo(Mathf.Sign(other.m_Score));
      }
    }
  }
}
