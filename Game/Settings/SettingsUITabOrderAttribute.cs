// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUITabOrderAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class SettingsUITabOrderAttribute : Attribute
  {
    public readonly ReadOnlyCollection<string> tabs;
    public readonly Type checkType;
    public readonly string checkMethod;

    public SettingsUITabOrderAttribute(params string[] tabs)
    {
      this.tabs = new ReadOnlyCollection<string>((IList<string>) tabs);
    }

    public SettingsUITabOrderAttribute(Type checkType, string checkMethod)
    {
      this.checkType = checkType;
      this.checkMethod = checkMethod;
    }
  }
}
