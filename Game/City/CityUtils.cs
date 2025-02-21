// Decompiled with JetBrains decompiler
// Type: Game.City.CityUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.City
{
  public static class CityUtils
  {
    public static bool CheckOption(Game.City.City city, CityOption option)
    {
      return (city.m_OptionMask & 1U << (int) (option & (CityOption) 31)) > 0U;
    }

    public static void ApplyModifier(
      ref float value,
      DynamicBuffer<CityModifier> modifiers,
      CityModifierType type)
    {
      if ((CityModifierType) modifiers.Length <= type)
        return;
      float2 delta = modifiers[(int) type].m_Delta;
      value += delta.x;
      value += value * delta.y;
    }

    public static float2 GetModifier(DynamicBuffer<CityModifier> modifiers, CityModifierType type)
    {
      return (CityModifierType) modifiers.Length > type ? modifiers[(int) type].m_Delta : new float2();
    }

    public static bool HasOption(CityOptionData optionData, CityOption option)
    {
      return (optionData.m_OptionMask & 1U << (int) (option & (CityOption) 31)) > 0U;
    }

    public static int GetCityServiceWorkplaceMaxWorkers(
      Entity ownerEntity,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref BufferLookup<InstalledUpgrade> installedUpgrades,
      ref ComponentLookup<Deleted> deleteds,
      ref ComponentLookup<WorkplaceData> workplaceDatas,
      ref ComponentLookup<SchoolData> schoolDatas,
      ref BufferLookup<Student> studentBufs)
    {
      int workplaceMaxWorkers = 0;
      if (deleteds.HasComponent(ownerEntity))
        return workplaceMaxWorkers;
      Entity entity1 = (Entity) prefabRefs[ownerEntity];
      if (!workplaceDatas.HasComponent(entity1))
        return workplaceMaxWorkers;
      int b = workplaceDatas[entity1].m_MaxWorkers;
      if (!installedUpgrades.HasBuffer(ownerEntity))
        return b;
      int x = workplaceDatas[entity1].m_MinimumWorkersLimit == 0 ? b : workplaceDatas[entity1].m_MinimumWorkersLimit;
      foreach (InstalledUpgrade installedUpgrade in installedUpgrades[ownerEntity])
      {
        if (prefabRefs.HasComponent(installedUpgrade.m_Upgrade) && !deleteds.HasComponent(installedUpgrade.m_Upgrade))
        {
          Entity entity2 = (Entity) prefabRefs[installedUpgrade.m_Upgrade];
          if (workplaceDatas.HasComponent(entity2))
          {
            x += workplaceDatas[entity2].m_MinimumWorkersLimit;
            b += workplaceDatas[entity2].m_MaxWorkers;
          }
        }
      }
      if (schoolDatas.HasComponent(entity1))
      {
        int studentCapacity = schoolDatas[entity1].m_StudentCapacity;
        int length = studentBufs[ownerEntity].Length;
        b = math.max(x, (int) Mathf.Lerp(0.0f, (float) b, 1f * (float) length / (float) studentCapacity));
      }
      return b;
    }
  }
}
