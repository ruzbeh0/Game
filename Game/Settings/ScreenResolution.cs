// Decompiled with JetBrains decompiler
// Type: Game.Settings.ScreenResolution
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.UI.Binding;
using System;
using UnityEngine;

#nullable disable
namespace Game.Settings
{
  public struct ScreenResolution : 
    IEquatable<ScreenResolution>,
    IComparable<ScreenResolution>,
    IJsonReadable,
    IJsonWritable
  {
    public int width;
    public int height;
    public RefreshRate refreshRate;

    private static void SupportValueTypesForAOT()
    {
      JSON.SupportTypeForAOT<ScreenResolution>();
      JSON.SupportTypeForAOT<RefreshRate>();
    }

    public double refreshRateDelta
    {
      get => Math.Abs(Math.Round(this.refreshRate.value) - this.refreshRate.value);
    }

    public ScreenResolution(Resolution resolution)
    {
      this.width = resolution.width;
      this.height = resolution.height;
      this.refreshRate = resolution.refreshRateRatio;
    }

    public bool Equals(ScreenResolution other)
    {
      int width1 = this.width;
      int height1 = this.height;
      uint numerator1 = this.refreshRate.numerator;
      uint denominator1 = this.refreshRate.denominator;
      int width2 = other.width;
      int height2 = other.height;
      uint numerator2 = other.refreshRate.numerator;
      uint denominator2 = other.refreshRate.denominator;
      int num = width2;
      return width1 == num && height1 == height2 && (int) numerator1 == (int) numerator2 && (int) denominator1 == (int) denominator2;
    }

    public override bool Equals(object obj) => obj is ScreenResolution other && this.Equals(other);

    public override int GetHashCode() => (this.width, this.height, this.refreshRate).GetHashCode();

    public static bool operator ==(ScreenResolution left, ScreenResolution right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ScreenResolution left, ScreenResolution right)
    {
      return !left.Equals(right);
    }

    public bool isValid
    {
      get
      {
        return this.width > 0 && this.height > 0 && this.refreshRate.numerator > 0U && this.refreshRate.denominator > 0U;
      }
    }

    public int CompareTo(ScreenResolution other)
    {
      int num1 = this.width.CompareTo(other.width);
      if (num1 != 0)
        return num1;
      int num2 = this.height.CompareTo(other.height);
      return num2 != 0 ? num2 : this.refreshRate.value.CompareTo(other.refreshRate.value);
    }

    public void Read(IJsonReader reader)
    {
      long num = (long) reader.ReadMapBegin();
      reader.ReadProperty("width");
      reader.Read(out this.width);
      reader.ReadProperty("height");
      reader.Read(out this.height);
      reader.ReadProperty("numerator");
      reader.Read(out this.refreshRate.numerator);
      reader.ReadProperty("denominator");
      reader.Read(out this.refreshRate.denominator);
      reader.ReadMapEnd();
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(typeof (ScreenResolution).FullName);
      writer.PropertyName("width");
      writer.Write(this.width);
      writer.PropertyName("height");
      writer.Write(this.height);
      writer.PropertyName("numerator");
      writer.Write(this.refreshRate.numerator);
      writer.PropertyName("denominator");
      writer.Write(this.refreshRate.denominator);
      writer.TypeEnd();
    }

    public override string ToString()
    {
      return string.Format("{0}x{1}x{2}Hz", (object) this.width, (object) this.height, (object) this.refreshRate.value);
    }
  }
}
