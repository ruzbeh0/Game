// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UIPolicySlider
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Prefabs;
using System;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct UIPolicySlider : IEquatable<UIPolicySlider>, IJsonWritable
  {
    private readonly float m_Value;
    private readonly float m_Default;
    private readonly float m_Step;
    private readonly PolicySliderUnit m_Unit;

    public Bounds1 range { get; }

    public UIPolicySlider(float value, PolicySliderData sliderData)
    {
      this.m_Value = value;
      this.range = sliderData.m_Range;
      this.m_Default = sliderData.m_Default;
      this.m_Step = sliderData.m_Step;
      this.m_Unit = (PolicySliderUnit) sliderData.m_Unit;
    }

    public UIPolicySlider(PolicySliderData sliderData)
    {
      this.m_Value = sliderData.m_Default;
      this.range = sliderData.m_Range;
      this.m_Default = sliderData.m_Default;
      this.m_Step = sliderData.m_Step;
      this.m_Unit = (PolicySliderUnit) sliderData.m_Unit;
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(TypeNames.kPolicySlider);
      writer.PropertyName("range");
      writer.Write(this.range);
      writer.PropertyName("value");
      writer.Write(this.m_Value);
      writer.PropertyName("default");
      writer.Write(this.m_Default);
      writer.PropertyName("step");
      writer.Write(this.m_Step);
      writer.PropertyName("unit");
      writer.Write(Enum.GetName(typeof (PolicySliderUnit), (object) this.m_Unit));
      writer.TypeEnd();
    }

    public override int GetHashCode()
    {
      return (this.range, this.m_Value, this.m_Default, this.m_Step, this.m_Unit).GetHashCode();
    }

    public override bool Equals(object obj) => obj is UIPolicySlider other && this.Equals(other);

    public bool Equals(UIPolicySlider other)
    {
      Bounds1 range1 = this.range;
      float num1 = this.m_Value;
      float num2 = this.m_Default;
      float step1 = this.m_Step;
      PolicySliderUnit unit1 = this.m_Unit;
      Bounds1 range2 = other.range;
      float num3 = other.m_Value;
      float num4 = other.m_Default;
      float step2 = this.m_Step;
      PolicySliderUnit unit2 = this.m_Unit;
      Bounds1 bounds1 = range2;
      return range1 == bounds1 && (double) num1 == (double) num3 && (double) num2 == (double) num4 && (double) step1 == (double) step2 && unit1 == unit2;
    }

    public static bool operator ==(UIPolicySlider left, UIPolicySlider right) => left.Equals(right);

    public static bool operator !=(UIPolicySlider left, UIPolicySlider right)
    {
      return !left.Equals(right);
    }
  }
}
