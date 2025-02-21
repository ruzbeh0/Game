// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIBindingMimicAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Property)]
  public class SettingsUIBindingMimicAttribute : Attribute
  {
    public readonly string map;
    public readonly string action;

    public SettingsUIBindingMimicAttribute(string map, string action)
    {
      this.map = map;
      this.action = action;
    }
  }
}
