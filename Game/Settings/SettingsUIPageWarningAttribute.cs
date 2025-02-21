// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIPageWarningAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class SettingsUIPageWarningAttribute : Attribute
  {
    public readonly Type checkType;
    public readonly string checkMethod;

    public SettingsUIPageWarningAttribute(Type checkType, string checkMethod)
    {
      this.checkType = checkType;
      this.checkMethod = checkMethod;
    }
  }
}
