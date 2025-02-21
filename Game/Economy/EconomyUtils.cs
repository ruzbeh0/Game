// Decompiled with JetBrains decompiler
// Type: Game.Economy.EconomyUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Companies;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Vehicles;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Economy
{
  public class EconomyUtils
  {
    public static readonly int kCompanyUpdatesPerDay = 256;
    public static readonly int ResourceCount = EconomyUtils.GetResourceIndex(Resource.Last);

    public static int GetResourceIndex(Resource r)
    {
      switch (r)
      {
        case Resource.NoResource:
        case Resource.Money:
        case Resource.Grain:
        case Resource.Money | Resource.Grain:
        case Resource.ConvenienceFood:
        case Resource.Money | Resource.ConvenienceFood:
        case Resource.Grain | Resource.ConvenienceFood:
        case Resource.Money | Resource.Grain | Resource.ConvenienceFood:
        case Resource.Food:
          switch (r - 1UL)
          {
            case Resource.NoResource:
              return 0;
            case Resource.Money:
              return 1;
            case Resource.Grain:
              break;
            case Resource.Money | Resource.Grain:
              return 2;
            default:
              if (r == Resource.Food)
                return 3;
              break;
          }
          break;
        case Resource.Vegetables:
          return 4;
        case Resource.Meals:
          return 5;
        case Resource.Wood:
          return 6;
        case Resource.Timber:
          return 7;
        case Resource.Paper:
          return 8;
        case Resource.Furniture:
          return 9;
        case Resource.Vehicles:
          return 10;
        case Resource.Lodging:
          return 11;
        case Resource.UnsortedMail:
          return 12;
        case Resource.LocalMail:
          return 13;
        case Resource.OutgoingMail:
          return 14;
        case Resource.Oil:
          return 15;
        case Resource.Petrochemicals:
          return 16;
        case Resource.Ore:
          return 17;
        case Resource.Plastics:
          return 18;
        case Resource.Metals:
          return 19;
        case Resource.Electronics:
          return 20;
        case Resource.Software:
          return 21;
        case Resource.Coal:
          return 22;
        case Resource.Stone:
          return 23;
        case Resource.Livestock:
          return 24;
        case Resource.Cotton:
          return 25;
        case Resource.Steel:
          return 26;
        case Resource.Minerals:
          return 27;
        case Resource.Concrete:
          return 28;
        case Resource.Machinery:
          return 29;
        case Resource.Chemicals:
          return 30;
        case Resource.Pharmaceuticals:
          return 31;
        case Resource.Beverages:
          return 32;
        case Resource.Textiles:
          return 33;
        case Resource.Telecom:
          return 34;
        case Resource.Financial:
          return 35;
        case Resource.Media:
          return 36;
        case Resource.Entertainment:
          return 37;
        case Resource.Recreation:
          return 38;
        case Resource.Garbage:
          return 39;
        case Resource.Last:
          return 40;
      }
      return -1;
    }

    public static float3 BuildPseudoTradeCost(
      float distance,
      IndustrialProcessData process,
      ComponentLookup<ResourceData> resourceDatas,
      ResourcePrefabs resourcePrefabs)
    {
      float3 float3 = new float3();
      if (process.m_Input1.m_Resource != Resource.NoResource)
        float3.y = (float) EconomyUtils.GetTransportCost(distance, process.m_Input1.m_Resource, 20000, resourceDatas[resourcePrefabs[process.m_Input1.m_Resource]].m_Weight);
      if (process.m_Input2.m_Resource != Resource.NoResource)
        float3.z = (float) EconomyUtils.GetTransportCost(distance, process.m_Input2.m_Resource, 20000, resourceDatas[resourcePrefabs[process.m_Input2.m_Resource]].m_Weight);
      float3.x = (float) EconomyUtils.GetTransportCost(distance, process.m_Output.m_Resource, 20000, resourceDatas[resourcePrefabs[process.m_Output.m_Resource]].m_Weight) - (float) ((double) float3.y * (double) process.m_Input1.m_Amount - (double) float3.z * (double) process.m_Input2.m_Amount) / (float) process.m_Output.m_Amount;
      return float3 / 20000f;
    }

    public static Resource GetAllResources()
    {
      Resource allResources = Resource.NoResource;
      ResourceIterator iterator = ResourceIterator.GetIterator();
      while (iterator.Next())
        allResources |= iterator.resource;
      return allResources;
    }

    public static bool IsProducedFrom(Resource product, Resource material)
    {
      switch (material)
      {
        case Resource.Grain:
          return (product & (Resource.ConvenienceFood | Resource.Food | Resource.Petrochemicals | Resource.Beverages)) > Resource.NoResource;
        case Resource.Food:
          return (product & (Resource.Meals | Resource.Lodging)) > Resource.NoResource;
        case Resource.Vegetables:
          return (product & (Resource.Food | Resource.Beverages)) > Resource.NoResource;
        case Resource.Wood:
          return (product & Resource.Timber) > Resource.NoResource;
        case Resource.Timber:
          return (product & Resource.Furniture) > Resource.NoResource;
        case Resource.Oil:
          return (product & Resource.Petrochemicals) > Resource.NoResource;
        case Resource.Petrochemicals:
          return (product & (Resource.Plastics | Resource.Chemicals)) > Resource.NoResource;
        case Resource.Ore:
          return (product & Resource.Metals) > Resource.NoResource;
        case Resource.Plastics:
          return (product & (Resource.Vehicles | Resource.Electronics)) > Resource.NoResource;
        case Resource.Metals:
          return (product & (Resource.Vehicles | Resource.Machinery)) > Resource.NoResource;
        case Resource.Electronics:
          return (product & (Resource.Software | Resource.Telecom)) > Resource.NoResource;
        case Resource.Software:
          return (product & (Resource.Telecom | Resource.Financial | Resource.Media)) > Resource.NoResource;
        case Resource.Coal:
          return (product & Resource.Steel) > Resource.NoResource;
        case Resource.Stone:
          return (product & (Resource.Minerals | Resource.Concrete)) > Resource.NoResource;
        case Resource.Livestock:
          return (product & (Resource.ConvenienceFood | Resource.Food | Resource.Textiles)) > Resource.NoResource;
        case Resource.Cotton:
          return (product & Resource.Textiles) > Resource.NoResource;
        case Resource.Steel:
          return (product & Resource.Machinery) > Resource.NoResource;
        case Resource.Minerals:
          return (product & (Resource.Electronics | Resource.Chemicals)) > Resource.NoResource;
        case Resource.Chemicals:
          return (product & (Resource.Paper | Resource.Pharmaceuticals)) > Resource.NoResource;
        case Resource.Beverages:
          return (product & (Resource.Meals | Resource.Lodging | Resource.Entertainment)) > Resource.NoResource;
        default:
          return false;
      }
    }

    public static Resource GetResources([CanBeNull] ResourceInEditor[] resources)
    {
      Resource resources1 = Resource.NoResource;
      if (resources != null)
      {
        foreach (ResourceInEditor resource in resources)
          resources1 |= EconomyUtils.GetResource(resource);
      }
      return resources1;
    }

    public static Resource GetResource(ResourceInEditor resource)
    {
      return EconomyUtils.GetResource((int) (resource - 1));
    }

    public static Resource GetResource(int index)
    {
      return index < 0 ? Resource.NoResource : (Resource) (1L << index);
    }

    public static int CountResources(Resource resource)
    {
      int num = 0;
      ResourceIterator iterator = ResourceIterator.GetIterator();
      while (iterator.Next())
      {
        if ((resource & iterator.resource) != Resource.NoResource)
          ++num;
      }
      return num;
    }

    public static TradeCost GetTradeCost(Resource resource, DynamicBuffer<TradeCost> costs)
    {
      for (int index = 0; index < costs.Length; ++index)
      {
        TradeCost cost = costs[index];
        if (cost.m_Resource == resource)
          return cost;
      }
      return new TradeCost() { m_Resource = resource };
    }

    public static long GetLastTradeRequestTime(DynamicBuffer<TradeCost> costs)
    {
      long tradeRequestTime = 0;
      for (int index = 0; index < costs.Length; ++index)
      {
        TradeCost cost = costs[index];
        if (cost.m_LastTransferRequestTime > tradeRequestTime)
          tradeRequestTime = cost.m_LastTransferRequestTime;
      }
      return tradeRequestTime;
    }

    public static void SetTradeCost(
      Resource resource,
      TradeCost newcost,
      DynamicBuffer<TradeCost> costs,
      bool keepLastTime,
      float buyLerp = 1f,
      float sellLerp = 1f)
    {
      Assert.IsTrue(!float.IsNaN(newcost.m_SellCost));
      Assert.IsTrue(!float.IsNaN(newcost.m_BuyCost));
      for (int index = 0; index < costs.Length; ++index)
      {
        TradeCost cost = costs[index];
        if (cost.m_Resource == resource)
        {
          if (keepLastTime)
            newcost.m_LastTransferRequestTime = cost.m_LastTransferRequestTime;
          newcost.m_BuyCost = (double) cost.m_BuyCost == 0.0 ? newcost.m_BuyCost : math.lerp(cost.m_BuyCost, newcost.m_BuyCost, buyLerp);
          newcost.m_SellCost = (double) cost.m_SellCost == 0.0 ? newcost.m_SellCost : math.lerp(cost.m_SellCost, newcost.m_SellCost, sellLerp);
          costs[index] = newcost;
          return;
        }
      }
      costs.Add(newcost);
    }

    public static void SetResources(
      Resource resource,
      DynamicBuffer<Resources> resources,
      int amount)
    {
      for (int index = 0; index < resources.Length; ++index)
      {
        Resources resource1 = resources[index];
        if (resource1.m_Resource == resource)
        {
          resource1.m_Amount = amount;
          resources[index] = resource1;
          return;
        }
      }
      resources.Add(new Resources()
      {
        m_Resource = resource,
        m_Amount = amount
      });
    }

    public static int GetResources(Resource resource, DynamicBuffer<Resources> resources)
    {
      for (int index = 0; index < resources.Length; ++index)
      {
        Resources resource1 = resources[index];
        if (resource1.m_Resource == resource)
          return resource1.m_Amount;
      }
      return 0;
    }

    public static int AddResources(
      Resource resource,
      int amount,
      DynamicBuffer<Resources> resources)
    {
      for (int index = 0; index < resources.Length; ++index)
      {
        Resources resource1 = resources[index];
        if (resource1.m_Resource == resource)
        {
          resource1.m_Amount = (int) math.clamp((long) resource1.m_Amount + (long) amount, (long) int.MinValue, (long) int.MaxValue);
          resources[index] = resource1;
          return resource1.m_Amount;
        }
      }
      resources.Add(new Resources()
      {
        m_Resource = resource,
        m_Amount = amount
      });
      return amount;
    }

    public static Resource GetResource(AvailableResource available)
    {
      switch (available)
      {
        case AvailableResource.GrainSupply:
          return Resource.Grain;
        case AvailableResource.VegetableSupply:
          return Resource.Vegetables;
        case AvailableResource.WoodSupply:
          return Resource.Wood;
        case AvailableResource.TextilesSupply:
          return Resource.Textiles;
        case AvailableResource.ConvenienceFoodSupply:
          return Resource.ConvenienceFood;
        case AvailableResource.PaperSupply:
          return Resource.Paper;
        case AvailableResource.VehiclesSupply:
          return Resource.Vehicles;
        case AvailableResource.OilSupply:
          return Resource.Oil;
        case AvailableResource.PetrochemicalsSupply:
          return Resource.Petrochemicals;
        case AvailableResource.OreSupply:
          return Resource.Ore;
        case AvailableResource.MetalsSupply:
          return Resource.Metals;
        case AvailableResource.ElectronicsSupply:
          return Resource.Electronics;
        case AvailableResource.PlasticsSupply:
          return Resource.Plastics;
        case AvailableResource.CoalSupply:
          return Resource.Coal;
        case AvailableResource.StoneSupply:
          return Resource.Stone;
        case AvailableResource.LivestockSupply:
          return Resource.Livestock;
        case AvailableResource.CottonSupply:
          return Resource.Cotton;
        case AvailableResource.SteelSupply:
          return Resource.Steel;
        case AvailableResource.MineralSupply:
          return Resource.Minerals;
        case AvailableResource.ChemicalSupply:
          return Resource.Chemicals;
        case AvailableResource.MachinerySupply:
          return Resource.Machinery;
        case AvailableResource.BeveragesSupply:
          return Resource.Beverages;
        case AvailableResource.TimberSupply:
          return Resource.Timber;
        default:
          return Resource.NoResource;
      }
    }

    public static AvailableResource GetAvailableResourceSupply(Resource resource)
    {
      switch (resource)
      {
        case Resource.Grain:
          return AvailableResource.GrainSupply;
        case Resource.ConvenienceFood:
          return AvailableResource.ConvenienceFoodSupply;
        case Resource.Vegetables:
          return AvailableResource.VegetableSupply;
        case Resource.Wood:
          return AvailableResource.WoodSupply;
        case Resource.Timber:
          return AvailableResource.TimberSupply;
        case Resource.Paper:
          return AvailableResource.PaperSupply;
        case Resource.Vehicles:
          return AvailableResource.VehiclesSupply;
        case Resource.Oil:
          return AvailableResource.OilSupply;
        case Resource.Petrochemicals:
          return AvailableResource.PetrochemicalsSupply;
        case Resource.Ore:
          return AvailableResource.OreSupply;
        case Resource.Plastics:
          return AvailableResource.PlasticsSupply;
        case Resource.Metals:
          return AvailableResource.MetalsSupply;
        case Resource.Electronics:
          return AvailableResource.ElectronicsSupply;
        case Resource.Coal:
          return AvailableResource.CoalSupply;
        case Resource.Stone:
          return AvailableResource.StoneSupply;
        case Resource.Livestock:
          return AvailableResource.LivestockSupply;
        case Resource.Cotton:
          return AvailableResource.CottonSupply;
        case Resource.Steel:
          return AvailableResource.SteelSupply;
        case Resource.Minerals:
          return AvailableResource.MineralSupply;
        case Resource.Machinery:
          return AvailableResource.MachinerySupply;
        case Resource.Chemicals:
          return AvailableResource.ChemicalSupply;
        case Resource.Beverages:
          return AvailableResource.BeveragesSupply;
        case Resource.Textiles:
          return AvailableResource.TextilesSupply;
        default:
          return AvailableResource.Count;
      }
    }

    public static int GetHouseholdTotalWealth(
      Household householdData,
      DynamicBuffer<Resources> resources)
    {
      int resources1 = EconomyUtils.GetResources(Resource.Money, resources);
      return (int) math.min((long) int.MaxValue, (long) householdData.m_Resources + (long) resources1);
    }

    public static int GetHouseholdSpendableMoney(
      Household householdData,
      DynamicBuffer<Resources> resources,
      ref BufferLookup<Renter> m_RenterBufs,
      ref ComponentLookup<ConsumptionData> consumptionDatas,
      ref ComponentLookup<PrefabRef> prefabRefs,
      PropertyRenter propertyRenter)
    {
      int householdSpendableMoney = EconomyUtils.GetResources(Resource.Money, resources);
      if (propertyRenter.m_Property != Entity.Null && m_RenterBufs.HasBuffer(propertyRenter.m_Property))
      {
        int length = m_RenterBufs[propertyRenter.m_Property].Length;
        int num1 = householdSpendableMoney - propertyRenter.m_Rent;
        Entity prefab = prefabRefs[propertyRenter.m_Property].m_Prefab;
        if (length == 0)
          Debug.LogWarning((object) string.Format("Property:{0} has 0 renter", (object) propertyRenter.m_Property.Index));
        int num2 = consumptionDatas[prefab].m_Upkeep / (length + 1);
        householdSpendableMoney = num1 - num2;
      }
      return householdSpendableMoney;
    }

    public static int GetHouseholdIncome(
      DynamicBuffer<HouseholdCitizen> citizens,
      ref ComponentLookup<Worker> workers,
      ref ComponentLookup<Citizen> citizenDatas,
      ref ComponentLookup<HealthProblem> healthProblems,
      ref EconomyParameterData economyParameters,
      NativeArray<int> taxRates)
    {
      int f = 0;
      for (int index = 0; index < citizens.Length; ++index)
      {
        Entity citizen = citizens[index].m_Citizen;
        if (!CitizenUtils.IsDead(citizen, ref healthProblems))
        {
          CitizenAge age = citizenDatas[citizen].GetAge();
          if (workers.HasComponent(citizen))
          {
            int level = (int) workers[citizen].m_Level;
            int wage = economyParameters.GetWage(level);
            f += wage;
            int num = wage - economyParameters.m_ResidentialMinimumEarnings;
            if (num > 0)
            {
              // ISSUE: reference to a compiler-generated method
              f -= Mathf.RoundToInt((float) num * ((float) TaxSystem.GetResidentialTaxRate(level, taxRates) / 100f));
            }
          }
          else
          {
            switch (age)
            {
              case CitizenAge.Child:
              case CitizenAge.Teen:
                f += economyParameters.m_FamilyAllowance;
                continue;
              case CitizenAge.Elderly:
                f += economyParameters.m_Pension;
                continue;
              default:
                // ISSUE: reference to a compiler-generated field
                if ((double) citizenDatas[citizen].m_UnemploymentCounter < (double) economyParameters.m_UnemploymentAllowanceMaxDays * (double) PayWageSystem.kUpdatesPerDay)
                {
                  f += economyParameters.m_UnemploymentBenefit;
                  continue;
                }
                continue;
            }
          }
        }
      }
      return Mathf.RoundToInt((float) f);
    }

    public static int GetCompanyTotalWorth(
      DynamicBuffer<Resources> resources,
      DynamicBuffer<OwnedVehicle> vehicles,
      BufferLookup<LayoutElement> layouts,
      ComponentLookup<Game.Vehicles.DeliveryTruck> trucks,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas)
    {
      int companyTotalWorth = EconomyUtils.GetCompanyTotalWorth(resources, resourcePrefabs, resourceDatas);
      for (int index1 = 0; index1 < vehicles.Length; ++index1)
      {
        Entity vehicle1 = vehicles[index1].m_Vehicle;
        if (trucks.HasComponent(vehicle1))
        {
          DynamicBuffer<LayoutElement> dynamicBuffer = new DynamicBuffer<LayoutElement>();
          if (layouts.HasBuffer(vehicle1))
            dynamicBuffer = layouts[vehicle1];
          if (dynamicBuffer.IsCreated && dynamicBuffer.Length != 0)
          {
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity vehicle2 = dynamicBuffer[index2].m_Vehicle;
              if (trucks.HasComponent(vehicle2))
              {
                Game.Vehicles.DeliveryTruck truck = trucks[vehicle2];
                int num = Mathf.RoundToInt((float) truck.m_Amount * EconomyUtils.GetMarketPrice(truck.m_Resource, resourcePrefabs, ref resourceDatas));
                companyTotalWorth += num;
              }
            }
          }
          else
          {
            Game.Vehicles.DeliveryTruck truck = trucks[vehicle1];
            int num = Mathf.RoundToInt((float) truck.m_Amount * EconomyUtils.GetMarketPrice(truck.m_Resource, resourcePrefabs, ref resourceDatas));
            companyTotalWorth += num;
          }
        }
      }
      return companyTotalWorth;
    }

    public static int GetCompanyTotalWorth(
      DynamicBuffer<Resources> resources,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas)
    {
      int companyTotalWorth = 0;
      for (int index = 0; index < resources.Length; ++index)
        companyTotalWorth += Mathf.RoundToInt((float) resources[index].m_Amount * EconomyUtils.GetMarketPrice(resources[index].m_Resource, resourcePrefabs, ref resourceDatas));
      return companyTotalWorth;
    }

    public static void AddResources(DynamicBuffer<Resources> from, DynamicBuffer<Resources> to)
    {
      for (int index = 0; index < from.Length; ++index)
        EconomyUtils.AddResources(from[index].m_Resource, from[index].m_Amount, to);
    }

    public static int GetTotalStorageUsed(DynamicBuffer<Resources> resources)
    {
      int totalStorageUsed = 0;
      for (int index = 0; index < resources.Length; ++index)
      {
        Resources resource = resources[index];
        if (resource.m_Resource != Resource.Money)
          totalStorageUsed += resource.m_Amount;
      }
      return totalStorageUsed;
    }

    public static float GetServicePriceMultiplier(float serviceAvailable, int maxServiceAvailable)
    {
      return math.lerp(0.7f, 1.3f, math.saturate((float) (1.0 - (double) serviceAvailable / (double) maxServiceAvailable)));
    }

    public static int GetTransportCost(
      float distance,
      Resource resource,
      int amount,
      float weight)
    {
      return resource != Resource.NoResource ? Mathf.RoundToInt(distance * 0.03f * weight * (float) (1 + Mathf.FloorToInt((float) (amount / 1000)))) : 0;
    }

    public static float GetTransportCost(
      float distance,
      int amount,
      float weight,
      StorageTransferFlags flags)
    {
      if ((flags & StorageTransferFlags.Car) != (StorageTransferFlags) 0)
        return (float) ((double) distance * 0.019999999552965164 * (10.0 + (double) weight * (double) amount / 1000.0));
      return (flags & StorageTransferFlags.Transport) != (StorageTransferFlags) 0 ? distance * (1f / 500f) * weight * (float) (10 + amount / 10000) : (float) ((double) distance * (1.0 / 500.0) * (10.0 + (double) weight * (double) amount / 1000.0));
    }

    public static string GetNames(Resource r)
    {
      string names = "";
      for (int index = 0; index < EconomyUtils.ResourceCount; ++index)
      {
        if ((r & EconomyUtils.GetResource(index)) != Resource.NoResource)
          names = names + EconomyUtils.GetName(EconomyUtils.GetResource(index)) + "|";
      }
      return names;
    }

    public static string GetName(Resource r)
    {
      if (r <= Resource.Electronics)
      {
        if (r <= Resource.Lodging)
        {
          if (r <= Resource.Wood)
          {
            if (r <= Resource.Vegetables)
            {
              switch (r)
              {
                case Resource.NoResource:
                  return "none";
                case Resource.Money:
                  return "money";
                case Resource.Grain:
                  return "grain";
                case Resource.Money | Resource.Grain:
                case Resource.Money | Resource.ConvenienceFood:
                case Resource.Grain | Resource.ConvenienceFood:
                case Resource.Money | Resource.Grain | Resource.ConvenienceFood:
                  break;
                case Resource.ConvenienceFood:
                  return "conv.food";
                case Resource.Food:
                  return "food";
                default:
                  if (r == Resource.Vegetables)
                    return "vegetables";
                  break;
              }
            }
            else
            {
              if (r == Resource.Meals)
                return "meals";
              if (r == Resource.Wood)
                return "wood";
            }
          }
          else if (r <= Resource.Paper)
          {
            if (r == Resource.Timber)
              return "timber";
            if (r == Resource.Paper)
              return "paper";
          }
          else
          {
            if (r == Resource.Furniture)
              return "furniture";
            if (r == Resource.Vehicles)
              return "vehicles";
            if (r == Resource.Lodging)
              return "lodging";
          }
        }
        else if (r <= Resource.Oil)
        {
          if (r <= Resource.LocalMail)
          {
            if (r == Resource.UnsortedMail)
              return "unsorted mail";
            if (r == Resource.LocalMail)
              return "local mail";
          }
          else
          {
            if (r == Resource.OutgoingMail)
              return "outgoing mail";
            if (r == Resource.Oil)
              return "oil";
          }
        }
        else if (r <= Resource.Ore)
        {
          if (r == Resource.Petrochemicals)
            return "petrochemicals";
          if (r == Resource.Ore)
            return "ore";
        }
        else
        {
          if (r == Resource.Plastics)
            return "plastics";
          if (r == Resource.Metals)
            return "metals";
          if (r == Resource.Electronics)
            return "electronics";
        }
      }
      else if (r <= Resource.Machinery)
      {
        if (r <= Resource.Livestock)
        {
          if (r <= Resource.Coal)
          {
            if (r == Resource.Software)
              return "software";
            if (r == Resource.Coal)
              return "coal";
          }
          else
          {
            if (r == Resource.Stone)
              return "stone";
            if (r == Resource.Livestock)
              return "livestock";
          }
        }
        else if (r <= Resource.Steel)
        {
          if (r == Resource.Cotton)
            return "cotton";
          if (r == Resource.Steel)
            return "steel";
        }
        else
        {
          if (r == Resource.Minerals)
            return "minerals";
          if (r == Resource.Concrete)
            return "concrete";
          if (r == Resource.Machinery)
            return "machinery";
        }
      }
      else if (r <= Resource.Telecom)
      {
        if (r <= Resource.Pharmaceuticals)
        {
          if (r == Resource.Chemicals)
            return "chemicals";
          if (r == Resource.Pharmaceuticals)
            return "pharmaceuticals";
        }
        else
        {
          if (r == Resource.Beverages)
            return "beverages";
          if (r == Resource.Textiles)
            return "textiles";
          if (r == Resource.Telecom)
            return "telecom";
        }
      }
      else if (r <= Resource.Media)
      {
        if (r == Resource.Financial)
          return "financial";
        if (r == Resource.Media)
          return "media";
      }
      else
      {
        if (r == Resource.Entertainment)
          return "entertainment";
        if (r == Resource.Recreation)
          return "recreation";
        if (r == Resource.Garbage)
          return "garbage";
      }
      return "none";
    }

    public static FixedString32Bytes GetNameFixed(Resource r)
    {
      if (r <= Resource.Electronics)
      {
        if (r <= Resource.Lodging)
        {
          if (r <= Resource.Wood)
          {
            if (r <= Resource.Vegetables)
            {
              switch (r)
              {
                case Resource.NoResource:
                  return (FixedString32Bytes) "none";
                case Resource.Money:
                  return (FixedString32Bytes) "money";
                case Resource.Grain:
                  return (FixedString32Bytes) "grain";
                case Resource.Money | Resource.Grain:
                case Resource.Money | Resource.ConvenienceFood:
                case Resource.Grain | Resource.ConvenienceFood:
                case Resource.Money | Resource.Grain | Resource.ConvenienceFood:
                  break;
                case Resource.ConvenienceFood:
                  return (FixedString32Bytes) "conv.food";
                case Resource.Food:
                  return (FixedString32Bytes) "food";
                default:
                  if (r == Resource.Vegetables)
                    return (FixedString32Bytes) "vegetables";
                  break;
              }
            }
            else
            {
              if (r == Resource.Meals)
                return (FixedString32Bytes) "meals";
              if (r == Resource.Wood)
                return (FixedString32Bytes) "wood";
            }
          }
          else if (r <= Resource.Paper)
          {
            if (r == Resource.Timber)
              return (FixedString32Bytes) "timber";
            if (r == Resource.Paper)
              return (FixedString32Bytes) "paper";
          }
          else
          {
            if (r == Resource.Furniture)
              return (FixedString32Bytes) "furniture";
            if (r == Resource.Vehicles)
              return (FixedString32Bytes) "vehicles";
            if (r == Resource.Lodging)
              return (FixedString32Bytes) "lodging";
          }
        }
        else if (r <= Resource.Oil)
        {
          if (r <= Resource.LocalMail)
          {
            if (r == Resource.UnsortedMail)
              return (FixedString32Bytes) "unsorted mail";
            if (r == Resource.LocalMail)
              return (FixedString32Bytes) "local mail";
          }
          else
          {
            if (r == Resource.OutgoingMail)
              return (FixedString32Bytes) "outgoing mail";
            if (r == Resource.Oil)
              return (FixedString32Bytes) "oil";
          }
        }
        else if (r <= Resource.Ore)
        {
          if (r == Resource.Petrochemicals)
            return (FixedString32Bytes) "petrochemicals";
          if (r == Resource.Ore)
            return (FixedString32Bytes) "ore";
        }
        else
        {
          if (r == Resource.Plastics)
            return (FixedString32Bytes) "plastics";
          if (r == Resource.Metals)
            return (FixedString32Bytes) "metals";
          if (r == Resource.Electronics)
            return (FixedString32Bytes) "electronics";
        }
      }
      else if (r <= Resource.Machinery)
      {
        if (r <= Resource.Livestock)
        {
          if (r <= Resource.Coal)
          {
            if (r == Resource.Software)
              return (FixedString32Bytes) "software";
            if (r == Resource.Coal)
              return (FixedString32Bytes) "coal";
          }
          else
          {
            if (r == Resource.Stone)
              return (FixedString32Bytes) "stone";
            if (r == Resource.Livestock)
              return (FixedString32Bytes) "livestock";
          }
        }
        else if (r <= Resource.Steel)
        {
          if (r == Resource.Cotton)
            return (FixedString32Bytes) "cotton";
          if (r == Resource.Steel)
            return (FixedString32Bytes) "steel";
        }
        else
        {
          if (r == Resource.Minerals)
            return (FixedString32Bytes) "minerals";
          if (r == Resource.Concrete)
            return (FixedString32Bytes) "concrete";
          if (r == Resource.Machinery)
            return (FixedString32Bytes) "machinery";
        }
      }
      else if (r <= Resource.Telecom)
      {
        if (r <= Resource.Pharmaceuticals)
        {
          if (r == Resource.Chemicals)
            return (FixedString32Bytes) "chemicals";
          if (r == Resource.Pharmaceuticals)
            return (FixedString32Bytes) "pharmaceuticals";
        }
        else
        {
          if (r == Resource.Beverages)
            return (FixedString32Bytes) "beverages";
          if (r == Resource.Textiles)
            return (FixedString32Bytes) "textiles";
          if (r == Resource.Telecom)
            return (FixedString32Bytes) "telecom";
        }
      }
      else if (r <= Resource.Media)
      {
        if (r == Resource.Financial)
          return (FixedString32Bytes) "financial";
        if (r == Resource.Media)
          return (FixedString32Bytes) "media";
      }
      else
      {
        if (r == Resource.Entertainment)
          return (FixedString32Bytes) "entertainment";
        if (r == Resource.Recreation)
          return (FixedString32Bytes) "recreation";
        if (r == Resource.Garbage)
          return (FixedString32Bytes) "garbage";
      }
      return (FixedString32Bytes) "none";
    }

    public static Color GetResourceColor(Resource r)
    {
      if (r <= Resource.Electronics)
      {
        if (r <= Resource.Lodging)
        {
          if (r <= Resource.Wood)
          {
            if (r <= Resource.Vegetables)
            {
              switch (r)
              {
                case Resource.NoResource:
                  return Color.white;
                case Resource.Money:
                  return new Color()
                  {
                    r = 0.305882365f,
                    g = 0.46274513f,
                    b = 0.3372549f,
                    a = 1f
                  };
                case Resource.Grain:
                  return new Color()
                  {
                    r = 0.596078455f,
                    g = 0.5647059f,
                    b = 0.427451f,
                    a = 1f
                  };
                case Resource.Money | Resource.Grain:
                case Resource.Money | Resource.ConvenienceFood:
                case Resource.Grain | Resource.ConvenienceFood:
                case Resource.Money | Resource.Grain | Resource.ConvenienceFood:
                  break;
                case Resource.ConvenienceFood:
                  return new Color()
                  {
                    r = 0.7607843f,
                    g = 0.5480962f,
                    b = 0.352941215f,
                    a = 1f
                  };
                case Resource.Food:
                  return new Color()
                  {
                    r = 0.596078455f,
                    g = 0.1607843f,
                    b = 0.260291934f,
                    a = 1f
                  };
                default:
                  if (r == Resource.Vegetables)
                    return new Color()
                    {
                      r = 0.552749455f,
                      g = 0.8207547f,
                      b = 0.376824439f,
                      a = 1f
                    };
                  break;
              }
            }
            else if (r != Resource.Meals)
            {
              if (r == Resource.Wood)
                return new Color()
                {
                  r = 0.4039216f,
                  g = 0.3529412f,
                  b = 0.298039228f,
                  a = 1f
                };
            }
            else
              return new Color()
              {
                r = 0.3137255f,
                g = 0.654902f,
                b = 0.5686275f,
                a = 1f
              };
          }
          else if (r <= Resource.Paper)
          {
            if (r != Resource.Timber)
            {
              if (r == Resource.Paper)
                return new Color()
                {
                  r = 0.8000001f,
                  g = 0.7960785f,
                  b = 0.6313726f,
                  a = 1f
                };
            }
            else
              return new Color()
              {
                r = 0.4039216f,
                g = 0.286274523f,
                b = 0.152941182f,
                a = 1f
              };
          }
          else if (r != Resource.Furniture)
          {
            if (r != Resource.Vehicles)
            {
              if (r == Resource.Lodging)
                return new Color()
                {
                  r = 0.321568638f,
                  g = 0.3803922f,
                  b = 0.5921569f,
                  a = 1f
                };
            }
            else
              return new Color()
              {
                r = 0.3137255f,
                g = 0.388235331f,
                b = 0.470588267f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.450980425f,
              g = 0.356862754f,
              b = 0.200000018f,
              a = 1f
            };
        }
        else if (r <= Resource.Oil)
        {
          if (r <= Resource.LocalMail)
          {
            if (r != Resource.UnsortedMail)
            {
              if (r == Resource.LocalMail)
                return new Color()
                {
                  r = 0.7254902f,
                  g = 0.7176471f,
                  b = 0.360784322f,
                  a = 1f
                };
            }
            else
              return new Color()
              {
                r = 1f,
                g = 0.9921569f,
                b = 0.5058824f,
                a = 1f
              };
          }
          else if (r != Resource.OutgoingMail)
          {
            if (r == Resource.Oil)
              return new Color()
              {
                r = 0.168627456f,
                g = 0.13333334f,
                b = 0.0784313753f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.5372549f,
              g = 0.533333361f,
              b = 0.282352954f,
              a = 1f
            };
        }
        else if (r <= Resource.Ore)
        {
          if (r != Resource.Petrochemicals)
          {
            if (r == Resource.Ore)
              return new Color()
              {
                r = 0.261436433f,
                g = 0.436397761f,
                b = 0.4433962f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.190325737f,
              g = 0.214659408f,
              b = 0.4433962f,
              a = 1f
            };
        }
        else if (r != Resource.Plastics)
        {
          if (r != Resource.Metals)
          {
            if (r == Resource.Electronics)
              return new Color()
              {
                r = 0.7411765f,
                g = 1f,
                b = 0.474509835f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.48627454f,
              g = 0.48627454f,
              b = 0.48627454f,
              a = 1f
            };
        }
        else
          return new Color()
          {
            r = 0.298039228f,
            g = 0.4901961f,
            b = 0.866666734f,
            a = 1f
          };
      }
      else if (r <= Resource.Machinery)
      {
        if (r <= Resource.Livestock)
        {
          if (r <= Resource.Coal)
          {
            if (r != Resource.Software)
            {
              if (r == Resource.Coal)
                return new Color()
                {
                  r = 0.1137255f,
                  g = 0.1137255f,
                  b = 0.149019614f,
                  a = 1f
                };
            }
            else
              return new Color()
              {
                r = 0.7843138f,
                g = 0.6745098f,
                b = 0.7725491f,
                a = 1f
              };
          }
          else if (r != Resource.Stone)
          {
            if (r == Resource.Livestock)
              return new Color()
              {
                r = 0.7725491f,
                g = 0.7725491f,
                b = 0.7725491f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.227451f,
              g = 0.2509804f,
              b = 0.3019608f,
              a = 1f
            };
        }
        else if (r <= Resource.Steel)
        {
          if (r != Resource.Cotton)
          {
            if (r == Resource.Steel)
              return new Color()
              {
                r = 0.549019635f,
                g = 0.5921569f,
                b = 0.6039216f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.9568628f,
              g = 0.9568628f,
              b = 0.9568628f,
              a = 1f
            };
        }
        else if (r != Resource.Minerals)
        {
          if (r != Resource.Concrete)
          {
            if (r == Resource.Machinery)
              return new Color()
              {
                r = 0.31764707f,
                g = 0.360784322f,
                b = 0.9490197f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.4039216f,
              g = 0.423529446f,
              b = 0.5176471f,
              a = 1f
            };
        }
        else
          return new Color()
          {
            r = 0.5411765f,
            g = 0.4431373f,
            b = 0.6313726f,
            a = 1f
          };
      }
      else if (r <= Resource.Telecom)
      {
        if (r <= Resource.Pharmaceuticals)
        {
          if (r != Resource.Chemicals)
          {
            if (r == Resource.Pharmaceuticals)
              return new Color()
              {
                r = 0.788235366f,
                g = 0.5529412f,
                b = 0.6039216f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.393734336f,
              g = 0.7924528f,
              b = 0.492530644f,
              a = 1f
            };
        }
        else if (r != Resource.Beverages)
        {
          if (r != Resource.Textiles)
          {
            if (r == Resource.Telecom)
              return new Color()
              {
                r = 0.498039246f,
                g = 0.7960785f,
                b = 0.7058824f,
                a = 1f
              };
          }
          else
            return new Color()
            {
              r = 0.9803922f,
              g = 0.549019635f,
              b = 0.9490197f,
              a = 1f
            };
        }
        else
          return new Color()
          {
            r = 0.7843138f,
            g = 0.8000001f,
            b = 0.549019635f,
            a = 1f
          };
      }
      else if (r <= Resource.Media)
      {
        if (r != Resource.Financial)
        {
          if (r == Resource.Media)
            return new Color()
            {
              r = 0.2509804f,
              g = 0.80392164f,
              b = 0.839215755f,
              a = 1f
            };
        }
        else
          return new Color()
          {
            r = 0.212649778f,
            g = 0.5283019f,
            b = 0.219511822f,
            a = 1f
          };
      }
      else if (r != Resource.Entertainment)
      {
        if (r != Resource.Recreation)
        {
          if (r == Resource.Garbage)
            return new Color()
            {
              r = 0.3019608f,
              g = 0.211764708f,
              b = 0.211764708f,
              a = 1f
            };
        }
        else
          return new Color()
          {
            r = 0.839215755f,
            g = 0.1764706f,
            b = 0.447058856f,
            a = 1f
          };
      }
      else
        return new Color()
        {
          r = 0.913725555f,
          g = 0.3254902f,
          b = 0.3254902f,
          a = 1f
        };
      return Color.black;
    }

    public static float GetIndustrialPrice(
      Resource r,
      ResourcePrefabs prefabs,
      ref ComponentLookup<ResourceData> resourceDatas)
    {
      Entity prefab = prefabs[r];
      return resourceDatas.HasComponent(prefab) ? resourceDatas[prefab].m_Price.x : 0.0f;
    }

    public static float GetServicePrice(
      Resource r,
      ResourcePrefabs prefabs,
      ref ComponentLookup<ResourceData> resourceDatas)
    {
      Entity prefab = prefabs[r];
      return resourceDatas.HasComponent(prefab) ? resourceDatas[prefab].m_Price.y : 0.0f;
    }

    public static float GetMarketPrice(
      Resource r,
      ResourcePrefabs prefabs,
      ref ComponentLookup<ResourceData> resourceDatas)
    {
      Entity prefab = prefabs[r];
      return resourceDatas.HasComponent(prefab) ? resourceDatas[prefab].m_Price.x + resourceDatas[prefab].m_Price.y : 0.0f;
    }

    public static float GetMarketPrice(
      Resource r,
      ResourcePrefabs prefabs,
      EntityManager entityManager)
    {
      Entity prefab = prefabs[r];
      ResourceData component;
      return entityManager.TryGetComponent<ResourceData>(prefab, out component) ? component.m_Price.x + component.m_Price.y : 0.0f;
    }

    public static float GetMarketPrice(ResourceData data) => data.m_Price.x + data.m_Price.y;

    public static float GetWeight(EntityManager entityManager, Resource r, ResourcePrefabs prefabs)
    {
      ResourceData component;
      return !entityManager.TryGetComponent<ResourceData>(prefabs[r], out component) ? 1f : component.m_Weight;
    }

    public static float GetWeight(
      Resource r,
      ResourcePrefabs prefabs,
      ref ComponentLookup<ResourceData> datas)
    {
      Entity prefab = prefabs[r];
      ResourceData componentData;
      return !datas.TryGetComponent(prefab, out componentData) ? 1f : componentData.m_Weight;
    }

    public static bool IsMaterial(
      Resource r,
      ResourcePrefabs prefabs,
      ref ComponentLookup<ResourceData> datas)
    {
      return (double) EconomyUtils.GetWeight(r, prefabs, ref datas) > 0.0;
    }

    public static bool IsOfficeResource(Resource resource)
    {
      return (resource & (Resource.Software | Resource.Telecom | Resource.Financial | Resource.Media)) > Resource.NoResource;
    }

    public static bool IsCommercialResource(Resource resource)
    {
      return (resource & (Resource.ConvenienceFood | Resource.Food | Resource.Meals | Resource.Paper | Resource.Furniture | Resource.Vehicles | Resource.Lodging | Resource.Petrochemicals | Resource.Plastics | Resource.Electronics | Resource.Chemicals | Resource.Pharmaceuticals | Resource.Beverages | Resource.Textiles | Resource.Entertainment | Resource.Recreation)) > Resource.NoResource;
    }

    public static bool IsExtractorResource(Resource resource)
    {
      return (resource & (Resource.Grain | Resource.Vegetables | Resource.Wood | Resource.Oil | Resource.Ore | Resource.Coal | Resource.Stone | Resource.Livestock | Resource.Cotton)) > Resource.NoResource;
    }

    public static bool IsIndustrialResource(
      ResourceData resourceData,
      bool includeMaterial,
      bool includeOffice)
    {
      return resourceData.m_IsProduceable && includeMaterial == resourceData.m_IsMaterial && includeOffice == ((double) resourceData.m_Weight == 0.0);
    }

    public static bool GetProcessComplexity(
      NativeList<ArchetypeChunk> m_IndustrialProcessDataChunks,
      ComponentLookup<WorkplaceData> workplaceDatas,
      Resource r,
      EntityTypeHandle entityType,
      ComponentTypeHandle<IndustrialProcessData> processType,
      out WorkplaceComplexity complexity)
    {
      for (int index1 = 0; index1 < m_IndustrialProcessDataChunks.Length; ++index1)
      {
        ArchetypeChunk processDataChunk = m_IndustrialProcessDataChunks[index1];
        NativeArray<Entity> nativeArray1 = processDataChunk.GetNativeArray(entityType);
        NativeArray<IndustrialProcessData> nativeArray2 = processDataChunk.GetNativeArray<IndustrialProcessData>(ref processType);
        for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
        {
          if (nativeArray2[index2].m_Output.m_Resource == r)
          {
            Entity entity = nativeArray1[index2];
            if (workplaceDatas.HasComponent(entity))
            {
              complexity = workplaceDatas[entity].m_Complexity;
              return true;
            }
          }
        }
      }
      complexity = WorkplaceComplexity.Simple;
      return false;
    }

    public static int GetCompanyProfitPerDay(
      float buildingEfficiency,
      bool isIndustrial,
      DynamicBuffer<Employee> employees,
      IndustrialProcessData processData,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas,
      ComponentLookup<Citizen> citizens,
      ref EconomyParameterData economyParameters)
    {
      // ISSUE: reference to a compiler-generated method
      int totalWage = WorkProviderSystem.CalculateTotalWage(employees, ref economyParameters);
      int productionPerDay = EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, isIndustrial, employees, processData, resourcePrefabs, resourceDatas, citizens, ref economyParameters);
      return Mathf.RoundToInt((float) ((isIndustrial ? (double) EconomyUtils.GetIndustrialPrice(processData.m_Output.m_Resource, resourcePrefabs, ref resourceDatas) : (double) EconomyUtils.GetMarketPrice(processData.m_Output.m_Resource, resourcePrefabs, ref resourceDatas)) * (double) processData.m_Output.m_Amount - (double) processData.m_Input1.m_Amount * (double) EconomyUtils.GetIndustrialPrice(processData.m_Input1.m_Resource, resourcePrefabs, ref resourceDatas) - (double) processData.m_Input2.m_Amount * (double) EconomyUtils.GetIndustrialPrice(processData.m_Input2.m_Resource, resourcePrefabs, ref resourceDatas)) / (float) processData.m_Output.m_Amount * (float) productionPerDay - (float) totalWage);
    }

    public static int GetCompanyProductionPerDay(
      float buildingEfficiency,
      bool isIndustrial,
      DynamicBuffer<Employee> employees,
      IndustrialProcessData processData,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas,
      ComponentLookup<Citizen> citizens,
      ref EconomyParameterData economyParameters)
    {
      ResourceData resourceData = resourceDatas[resourcePrefabs[processData.m_Output.m_Resource]];
      return EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, isIndustrial, employees, processData, resourceData, citizens, ref economyParameters);
    }

    public static int GetCompanyProductionPerDay(
      float buildingEfficiency,
      bool isIndustrial,
      DynamicBuffer<Employee> employees,
      IndustrialProcessData processData,
      ResourceData resourceData,
      ComponentLookup<Citizen> citizens,
      ref EconomyParameterData economyParameters)
    {
      float num1 = EconomyUtils.IsExtractorResource(processData.m_Output.m_Resource) ? economyParameters.m_ExtractorProductionEfficiency : (isIndustrial ? economyParameters.m_IndustrialEfficiency : economyParameters.m_CommercialEfficiency);
      // ISSUE: reference to a compiler-generated method
      float num2 = buildingEfficiency * num1 * WorkProviderSystem.GetWorkforce(employees, citizens) * (float) EconomyUtils.kCompanyUpdatesPerDay;
      float num3 = isIndustrial ? (float) resourceData.m_NeededWorkPerUnit.x : (float) resourceData.m_NeededWorkPerUnit.y;
      return (int) math.ceil((float) processData.m_Output.m_Amount * (num2 / num3));
    }

    public static int GetCompanyProductionPerDay(
      float buildingEfficiency,
      int workerAmount,
      int level,
      bool isIndustrial,
      WorkplaceData workplaceData,
      IndustrialProcessData processData,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas,
      ref EconomyParameterData economyParameters)
    {
      ResourceData resourceData = resourceDatas[resourcePrefabs[processData.m_Output.m_Resource]];
      return EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, workerAmount, level, isIndustrial, workplaceData, processData, resourceData, ref economyParameters);
    }

    public static int GetCompanyProductionPerDay(
      float buildingEfficiency,
      int workerAmount,
      int level,
      bool isIndustrial,
      WorkplaceData workplaceData,
      IndustrialProcessData processData,
      ResourceData resourceData,
      ref EconomyParameterData economyParameters)
    {
      float num1 = EconomyUtils.IsExtractorResource(processData.m_Output.m_Resource) ? economyParameters.m_ExtractorProductionEfficiency : (isIndustrial ? economyParameters.m_IndustrialEfficiency : economyParameters.m_CommercialEfficiency);
      // ISSUE: reference to a compiler-generated method
      float num2 = buildingEfficiency * num1 * WorkProviderSystem.GetAverageWorkforce(workerAmount, workplaceData.m_Complexity, level) * (float) EconomyUtils.kCompanyUpdatesPerDay;
      float num3 = isIndustrial ? (float) resourceData.m_NeededWorkPerUnit.x : (float) resourceData.m_NeededWorkPerUnit.y;
      return (int) math.ceil((float) processData.m_Output.m_Amount * (num2 / num3));
    }

    public static IncomeSource GetIncomeSource(PlayerResource resource)
    {
      switch (resource)
      {
        case PlayerResource.Electricity:
          return IncomeSource.FeeElectricity;
        case PlayerResource.Healthcare:
          return IncomeSource.FeeHealthcare;
        case PlayerResource.BasicEducation:
        case PlayerResource.SecondaryEducation:
        case PlayerResource.HigherEducation:
          return IncomeSource.FeeEducation;
        case PlayerResource.Garbage:
          return IncomeSource.FeeGarbage;
        case PlayerResource.Water:
        case PlayerResource.Sewage:
          return IncomeSource.FeeWater;
        case PlayerResource.PublicTransport:
          return IncomeSource.FeePublicTransport;
        case PlayerResource.Parking:
          return IncomeSource.FeeParking;
        default:
          return IncomeSource.Count;
      }
    }
  }
}
