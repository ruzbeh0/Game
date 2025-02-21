// Decompiled with JetBrains decompiler
// Type: ShowIfAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
public class ShowIfAttribute : PropertyAttribute
{
  public string ConditionName { get; private set; }

  public int EnumValue { get; private set; }

  public bool Inverse { get; private set; }

  public ShowIfAttribute(string conditionName, int enumValue, bool inverse = false)
  {
    this.ConditionName = conditionName;
    this.EnumValue = enumValue;
    this.Inverse = inverse;
  }
}
