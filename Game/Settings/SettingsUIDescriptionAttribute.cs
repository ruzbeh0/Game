// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIDescriptionAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property, Inherited = true)]
  public class SettingsUIDescriptionAttribute : Attribute
  {
    public readonly string id;
    public readonly string value;
    public readonly Type getterType;
    public readonly string getterMethod;

    public SettingsUIDescriptionAttribute(string overrideId = null, string overrideValue = null)
    {
      this.id = overrideId;
      this.value = overrideValue;
    }

    public SettingsUIDescriptionAttribute(Type getterType, string getterMethod)
    {
      this.getterType = getterType;
      this.getterMethod = getterMethod;
    }
  }
}
