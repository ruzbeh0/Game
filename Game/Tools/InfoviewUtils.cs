// Decompiled with JetBrains decompiler
// Type: Game.Tools.InfoviewUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Zones;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public static class InfoviewUtils
  {
    public static float GetColor(InfoviewCoverageData data, float coverage)
    {
      return math.saturate((float) (((double) coverage - (double) data.m_Range.min) / ((double) data.m_Range.max - (double) data.m_Range.min)));
    }

    public static float GetColor(
      InfoviewAvailabilityData data,
      DynamicBuffer<ResourceAvailability> availabilityBuffer,
      float curvePosition,
      ref ZonePreferenceData preferences,
      NativeArray<int> industrialDemands,
      NativeArray<int> storageDemands,
      float pollution,
      float landValue,
      DynamicBuffer<ProcessEstimate> estimates,
      NativeList<IndustrialProcessData> processes,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas)
    {
      Resource allResources = EconomyUtils.GetAllResources();
      BuildingPropertyData propertyData = new BuildingPropertyData()
      {
        m_AllowedManufactured = allResources,
        m_AllowedSold = allResources,
        m_AllowedStored = allResources,
        m_ResidentialProperties = 1
      };
      float x = ZoneEvaluationUtils.GetScore(data.m_AreaType, data.m_Office, availabilityBuffer, curvePosition, ref preferences, false, industrialDemands, propertyData, pollution, landValue, estimates, processes, resourcePrefabs, ref resourceDatas) / 256f;
      if (data.m_AreaType == AreaType.Industrial && !data.m_Office)
        x = x * 0.875f + ZoneEvaluationUtils.GetScore(data.m_AreaType, false, availabilityBuffer, curvePosition, ref preferences, true, storageDemands, propertyData, pollution, landValue, estimates, processes, resourcePrefabs, ref resourceDatas) / 2048f;
      return math.saturate(x);
    }

    public static float GetColor(InfoviewNetStatusData data, float status)
    {
      return math.saturate((float) (((double) status - (double) data.m_Range.min) / ((double) data.m_Range.max - (double) data.m_Range.min)));
    }

    public static float GetColor(InfoviewBuildingStatusData data, float status)
    {
      return math.saturate((float) (((double) status - (double) data.m_Range.min) / ((double) data.m_Range.max - (double) data.m_Range.min)));
    }

    public static float GetColor(InfoviewObjectStatusData data, float status)
    {
      return math.saturate((float) (((double) status - (double) data.m_Range.min) / ((double) data.m_Range.max - (double) data.m_Range.min)));
    }
  }
}
