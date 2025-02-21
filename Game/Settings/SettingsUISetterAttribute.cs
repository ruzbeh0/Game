// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUISetterAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Property, Inherited = true)]
  public class SettingsUISetterAttribute : Attribute
  {
    public readonly Type setterType;
    public readonly string setterMethod;

    public SettingsUISetterAttribute(Type setterType, string setterMethod)
    {
      this.setterType = setterType;
      this.setterMethod = setterMethod;
    }
  }
}
