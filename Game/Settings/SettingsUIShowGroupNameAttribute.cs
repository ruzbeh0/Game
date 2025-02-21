// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIShowGroupNameAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
  public class SettingsUIShowGroupNameAttribute : Attribute
  {
    public readonly bool showAll;
    public readonly ReadOnlyCollection<string> groups;

    public SettingsUIShowGroupNameAttribute() => this.showAll = true;

    public SettingsUIShowGroupNameAttribute(params string[] groups)
    {
      this.groups = new ReadOnlyCollection<string>((IList<string>) groups);
    }
  }
}
