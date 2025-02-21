// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CompanyUIUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Companies;
using Game.Prefabs;
using Game.Zones;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public static class CompanyUIUtils
  {
    public static bool HasCompany(
      EntityManager entityManager,
      Entity entity,
      Entity prefab,
      out Entity company)
    {
      company = Entity.Null;
      BuildingPropertyData component;
      if (!entityManager.HasComponent<Renter>(entity) || !entityManager.TryGetComponent<BuildingPropertyData>(prefab, out component) || component.CountProperties(AreaType.Commercial) + component.CountProperties(AreaType.Industrial) <= 0)
        return false;
      DynamicBuffer<Renter> buffer;
      if (entityManager.TryGetBuffer<Renter>(entity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (entityManager.HasComponent<CompanyData>(buffer[index].m_Renter))
          {
            company = buffer[index].m_Renter;
            break;
          }
        }
      }
      return true;
    }

    public static bool HasCompany(
      Entity entity,
      Entity prefab,
      ref BufferLookup<Renter> renterFromEntity,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDataFromEntity,
      ref ComponentLookup<CompanyData> companyDataFromEntity,
      out Entity company)
    {
      company = Entity.Null;
      BuildingPropertyData componentData;
      if (!renterFromEntity.HasBuffer(entity) || !buildingPropertyDataFromEntity.TryGetComponent(prefab, out componentData) || componentData.CountProperties(AreaType.Commercial) + componentData.CountProperties(AreaType.Industrial) <= 0)
        return false;
      DynamicBuffer<Renter> bufferData;
      if (renterFromEntity.TryGetBuffer(entity, out bufferData))
      {
        for (int index = 0; index < bufferData.Length; ++index)
        {
          if (companyDataFromEntity.HasComponent(bufferData[index].m_Renter))
          {
            company = bufferData[index].m_Renter;
            break;
          }
        }
      }
      return true;
    }

    public static CompanyProfitabilityKey GetProfitabilityKey(int profit)
    {
      if (profit > 128)
        return CompanyProfitabilityKey.Profitable;
      if (profit > 32)
        return CompanyProfitabilityKey.GettingBy;
      if (profit > -64)
        return CompanyProfitabilityKey.BreakingEven;
      return profit > -182 ? CompanyProfitabilityKey.LosingMoney : CompanyProfitabilityKey.Bankrupt;
    }
  }
}
