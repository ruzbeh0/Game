// Decompiled with JetBrains decompiler
// Type: Game.Assets.SimulationDateTime
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.Assets
{
  public struct SimulationDateTime : IEquatable<SimulationDateTime>, IJsonReadable, IJsonWritable
  {
    public int year;
    public int month;
    public int hour;
    public int minute;

    private static void SupportValueTypesForAOT() => JSON.SupportTypeForAOT<SimulationDateTime>();

    public SimulationDateTime(int year, int month, int hour, int minute)
    {
      this.year = year;
      this.month = month;
      this.hour = hour;
      this.minute = minute;
    }

    public bool Equals(SimulationDateTime other)
    {
      int year1 = this.year;
      int month1 = this.month;
      int hour1 = this.hour;
      int minute1 = this.minute;
      int year2 = other.year;
      int month2 = other.month;
      int hour2 = other.hour;
      int minute2 = other.minute;
      int num = year2;
      return year1 == num && month1 == month2 && hour1 == hour2 && minute1 == minute2;
    }

    public override bool Equals(object obj)
    {
      return obj is SimulationDateTime other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return (this.year, this.month, this.hour, this.minute).GetHashCode();
    }

    public static bool operator ==(SimulationDateTime left, SimulationDateTime right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(SimulationDateTime left, SimulationDateTime right)
    {
      return !left.Equals(right);
    }

    public void Read(IJsonReader reader)
    {
      long num = (long) reader.ReadMapBegin();
      reader.ReadProperty("year");
      reader.Read(out this.year);
      reader.ReadProperty("month");
      reader.Read(out this.month);
      reader.ReadProperty("hour");
      reader.Read(out this.hour);
      reader.ReadProperty("minute");
      reader.Read(out this.minute);
      reader.ReadMapEnd();
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("year");
      writer.Write(this.year);
      writer.PropertyName("month");
      writer.Write(this.month);
      writer.PropertyName("hour");
      writer.Write(this.hour);
      writer.PropertyName("minute");
      writer.Write(this.minute);
      writer.TypeEnd();
    }
  }
}
