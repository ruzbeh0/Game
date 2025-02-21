// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUITabWarningAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class SettingsUITabWarningAttribute : Attribute
  {
    public readonly string tab;
    public readonly Type checkType;
    public readonly string checkMethod;

    public SettingsUITabWarningAttribute(string tab, Type checkType, string checkMethod)
    {
      this.tab = tab;
      this.checkType = checkType;
      this.checkMethod = checkMethod;
    }
  }
}
