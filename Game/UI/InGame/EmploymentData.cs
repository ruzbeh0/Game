// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.EmploymentData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Companies;
using Game.Prefabs;
using Game.Simulation;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct EmploymentData : IJsonWritable
  {
    public int uneducated { get; }

    public int poorlyEducated { get; }

    public int educated { get; }

    public int wellEducated { get; }

    public int highlyEducated { get; }

    public int openPositions { get; }

    public int total { get; }

    public EmploymentData(
      int uneducated,
      int poorlyEducated,
      int educated,
      int wellEducated,
      int highlyEducated,
      int openPositions)
    {
      this.uneducated = uneducated;
      this.poorlyEducated = poorlyEducated;
      this.educated = educated;
      this.wellEducated = wellEducated;
      this.highlyEducated = highlyEducated;
      this.openPositions = openPositions;
      this.total = uneducated + poorlyEducated + educated + wellEducated + highlyEducated + openPositions;
    }

    public static EmploymentData operator +(EmploymentData left, EmploymentData right)
    {
      return new EmploymentData(left.uneducated + right.uneducated, left.poorlyEducated + right.poorlyEducated, left.educated + right.educated, left.wellEducated + right.wellEducated, left.highlyEducated + right.highlyEducated, left.openPositions + right.openPositions);
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin("selectedInfo.ChartData");
      writer.PropertyName("values");
      writer.ArrayBegin(6U);
      writer.Write(this.uneducated);
      writer.Write(this.poorlyEducated);
      writer.Write(this.educated);
      writer.Write(this.wellEducated);
      writer.Write(this.highlyEducated);
      writer.Write(this.openPositions);
      writer.ArrayEnd();
      writer.PropertyName("total");
      writer.Write(this.total);
      writer.TypeEnd();
    }

    public static EmploymentData GetWorkplacesData(
      int maxWorkers,
      int buildingLevel,
      WorkplaceComplexity complexity)
    {
      // ISSUE: reference to a compiler-generated method
      Workplaces numberOfWorkplaces = WorkProviderSystem.CalculateNumberOfWorkplaces(maxWorkers, complexity, buildingLevel);
      return new EmploymentData(numberOfWorkplaces.m_Uneducated, numberOfWorkplaces.m_PoorlyEducated, numberOfWorkplaces.m_Educated, numberOfWorkplaces.m_WellEducated, numberOfWorkplaces.m_HighlyEducated, 0);
    }

    public static EmploymentData GetEmployeesData(
      DynamicBuffer<Employee> employees,
      int openPositions)
    {
      int uneducated = 0;
      int poorlyEducated = 0;
      int educated = 0;
      int wellEducated = 0;
      int highlyEducated = 0;
      for (int index = 0; index < employees.Length; ++index)
      {
        switch (employees[index].m_Level)
        {
          case 0:
            ++uneducated;
            break;
          case 1:
            ++poorlyEducated;
            break;
          case 2:
            ++educated;
            break;
          case 3:
            ++wellEducated;
            break;
          case 4:
            ++highlyEducated;
            break;
        }
      }
      return new EmploymentData(uneducated, poorlyEducated, educated, wellEducated, highlyEducated, openPositions);
    }
  }
}
