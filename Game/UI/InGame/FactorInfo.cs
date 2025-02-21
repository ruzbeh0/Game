// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.FactorInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Simulation;
using System;
using Unity.Collections;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct FactorInfo : IComparable<FactorInfo>
  {
    public int factor { get; }

    public int weight { get; }

    public FactorInfo(int factor, int weight)
    {
      this.factor = factor;
      this.weight = weight;
    }

    public int CompareTo(FactorInfo other)
    {
      int num = math.abs(other.weight).CompareTo(math.abs(this.weight));
      return num == 0 ? other.factor.CompareTo(this.factor) : num;
    }

    public void WriteDemandFactor(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("factor");
      writer.Write(Enum.GetName(typeof (DemandFactor), (object) this.factor));
      writer.PropertyName("weight");
      writer.Write(this.weight);
      writer.TypeEnd();
    }

    public void WriteHappinessFactor(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("factor");
      writer.Write(Enum.GetName(typeof (CitizenHappinessSystem.HappinessFactor), (object) this.factor));
      writer.PropertyName("weight");
      writer.Write(this.weight);
      writer.TypeEnd();
    }

    public void WriteBuildingHappinessFactor(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("factor");
      writer.Write(Enum.GetName(typeof (BuildingHappinessFactor), (object) this.factor));
      writer.PropertyName("weight");
      writer.Write(this.weight);
      writer.TypeEnd();
    }

    public static NativeList<FactorInfo> FromFactorArray(
      NativeArray<int> factors,
      Allocator allocator)
    {
      NativeList<FactorInfo> list = new NativeList<FactorInfo>(factors.Length, (AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < factors.Length; ++index)
      {
        if (factors[index] != 0)
          list.Add(new FactorInfo(index, factors[index]));
      }
      list.Sort<FactorInfo>();
      return list;
    }
  }
}
