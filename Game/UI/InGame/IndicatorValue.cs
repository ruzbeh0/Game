// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.IndicatorValue
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct IndicatorValue : IEquatable<IndicatorValue>, IJsonWritable
  {
    public float min { get; }

    public float max { get; }

    public float current { get; }

    public IndicatorValue(float min, float max, float current)
    {
      this.min = min;
      this.max = max;
      this.current = math.clamp(current, min, max);
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().Name);
      writer.PropertyName("min");
      writer.Write(this.min);
      writer.PropertyName("max");
      writer.Write(this.max);
      writer.PropertyName("current");
      writer.Write(this.current);
      writer.TypeEnd();
    }

    public static IndicatorValue Calculate(
      float supply,
      float demand,
      float minRangeFactor = -1f,
      float maxRangeFactor = 1f)
    {
      float current = (double) supply > 1.4012984643248171E-45 ? math.clamp((supply - demand) / supply, minRangeFactor, maxRangeFactor) : minRangeFactor;
      return new IndicatorValue(minRangeFactor, maxRangeFactor, current);
    }

    public bool Equals(IndicatorValue other)
    {
      double min1 = (double) this.min;
      float max1 = this.max;
      float current1 = this.current;
      float min2 = other.min;
      float max2 = other.max;
      float current2 = other.current;
      double num = (double) min2;
      return min1 == num && (double) max1 == (double) max2 && (double) current1 == (double) current2;
    }

    public override bool Equals(object obj)
    {
      return obj is IndicatorValue indicatorValue && indicatorValue.Equals(this);
    }

    public override int GetHashCode() => (this.min, this.max, this.current).GetHashCode();
  }
}
