// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIValueVersionAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Property, Inherited = true)]
  public class SettingsUIValueVersionAttribute : Attribute
  {
    public readonly Type versionGetterType;
    public readonly string versionGetterMethod;

    public SettingsUIValueVersionAttribute(Type versionGetterType, string versionGetterMethod)
    {
      this.versionGetterType = versionGetterType;
      this.versionGetterMethod = versionGetterMethod;
    }
  }
}
