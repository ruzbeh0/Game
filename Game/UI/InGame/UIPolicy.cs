// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UIPolicy
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct UIPolicy : IEquatable<UIPolicy>, IComparable<UIPolicy>
  {
    private readonly string m_Id;
    private readonly string m_LocalizedName;
    private readonly int m_Priority;
    private readonly string m_Icon;
    private readonly Entity m_Entity;
    private readonly bool m_Locked;
    private readonly string m_UITag;
    private readonly int m_Milestone;
    private readonly bool m_Active;
    private readonly bool m_Slider;
    private readonly UIPolicySlider m_Data;

    public UIPolicy(
      string id,
      string localizedName,
      int priority,
      string icon,
      Entity entity,
      bool active,
      bool locked,
      string uiTag,
      int milestone,
      bool slider,
      UIPolicySlider data)
    {
      this.m_Id = id;
      this.m_LocalizedName = localizedName;
      this.m_Priority = priority;
      this.m_Icon = icon;
      this.m_Entity = entity;
      this.m_Active = active;
      this.m_Locked = locked;
      this.m_UITag = uiTag;
      this.m_Milestone = milestone;
      this.m_Slider = slider;
      this.m_Data = data;
    }

    public void Write(PrefabUISystem prefabUISystem, IJsonWriter writer)
    {
      writer.TypeBegin(TypeNames.kPolicy);
      writer.PropertyName("id");
      writer.Write(this.m_Id);
      writer.PropertyName("icon");
      writer.Write(this.m_Icon);
      writer.PropertyName("entity");
      if (this.m_Entity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.m_Entity);
      writer.PropertyName("active");
      writer.Write(this.m_Active);
      writer.PropertyName("locked");
      writer.Write(this.m_Locked);
      writer.PropertyName("uiTag");
      writer.Write(this.m_UITag);
      writer.PropertyName("requirements");
      // ISSUE: reference to a compiler-generated method
      prefabUISystem.BindPrefabRequirements(writer, this.m_Entity);
      writer.PropertyName("data");
      if (this.m_Slider)
        writer.Write<UIPolicySlider>(this.m_Data);
      else
        writer.WriteNull();
      writer.TypeEnd();
    }

    public override int GetHashCode()
    {
      return (this.m_Id, this.m_Icon, this.m_Entity, this.m_Active, this.m_Slider, this.m_Data).GetHashCode();
    }

    public int CompareTo(UIPolicy other)
    {
      int num1 = this.m_Milestone.CompareTo(other.m_Milestone);
      int num2 = this.m_Priority.CompareTo(other.m_Priority);
      if (num1 != 0)
        return num1;
      return num2 == 0 ? string.Compare(this.m_LocalizedName, other.m_LocalizedName, StringComparison.Ordinal) : num2;
    }

    public override bool Equals(object obj) => obj is UIPolicy other && this.Equals(other);

    public bool Equals(UIPolicy other) => this.m_Entity == other.m_Entity;

    public static bool operator ==(UIPolicy left, UIPolicy right) => left.Equals(right);

    public static bool operator !=(UIPolicy left, UIPolicy right) => !left.Equals(right);
  }
}
