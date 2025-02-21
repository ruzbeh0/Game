// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FlowUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Companies;
using Game.Prefabs;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public static class FlowUtils
  {
    public static int ConsumeFromTotal(int demand, ref int totalSupply, ref int totalDemand)
    {
      int num1 = 0;
      if (demand > 0)
      {
        int a = totalSupply - (totalDemand - demand);
        int b = totalSupply;
        int num2 = totalDemand * 100 / demand;
        num1 = math.clamp(totalSupply * 100 / num2, a, b);
        totalSupply -= num1;
        totalDemand -= demand;
      }
      return num1;
    }

    public static float GetRenterConsumptionMultiplier(
      Entity prefab,
      DynamicBuffer<Renter> renterBuffer,
      ref BufferLookup<HouseholdCitizen> householdCitizens,
      ref BufferLookup<Employee> employees,
      ref ComponentLookup<Citizen> citizens,
      ref ComponentLookup<SpawnableBuildingData> spawnableDatas)
    {
      int num1 = 0;
      float num2 = 0.0f;
      foreach (Renter renter in renterBuffer)
      {
        DynamicBuffer<HouseholdCitizen> bufferData1;
        if (householdCitizens.TryGetBuffer((Entity) renter, out bufferData1))
        {
          foreach (HouseholdCitizen householdCitizen in bufferData1)
          {
            Citizen componentData;
            if (citizens.TryGetComponent(householdCitizen.m_Citizen, out componentData))
            {
              num2 += (float) componentData.GetEducationLevel();
              ++num1;
            }
          }
        }
        else
        {
          DynamicBuffer<Employee> bufferData2;
          if (employees.TryGetBuffer((Entity) renter, out bufferData2))
          {
            foreach (Employee employee in bufferData2)
            {
              Citizen componentData;
              if (citizens.TryGetComponent(employee.m_Worker, out componentData))
              {
                num2 += (float) componentData.GetEducationLevel();
                ++num1;
              }
            }
          }
        }
      }
      if (num1 == 0)
        return 0.0f;
      SpawnableBuildingData componentData1;
      float num3 = spawnableDatas.TryGetComponent(prefab, out componentData1) ? (float) componentData1.m_Level : 5f;
      return (float) (5.0 * (double) num1 / ((double) num3 + 0.5 * ((double) num2 / (double) num1)));
    }
  }
}
