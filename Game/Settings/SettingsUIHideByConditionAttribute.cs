// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIHideByConditionAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true)]
  public class SettingsUIHideByConditionAttribute : Attribute
  {
    public readonly Type checkType;
    public readonly string checkMethod;
    public readonly bool invert;

    public SettingsUIHideByConditionAttribute(Type checkType, string checkMethod)
    {
      this.checkType = checkType;
      this.checkMethod = checkMethod;
    }

    public SettingsUIHideByConditionAttribute(Type checkType, string checkMethod, bool invert)
    {
      this.checkType = checkType;
      this.checkMethod = checkMethod;
      this.invert = invert;
    }
  }
}
