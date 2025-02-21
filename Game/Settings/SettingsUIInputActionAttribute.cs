// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIInputActionAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Game.Settings
{
  public abstract class SettingsUIInputActionAttribute : Attribute
  {
    public readonly string name;
    public readonly InputManager.DeviceType device;
    public readonly ActionType type;
    public readonly bool allowModifiers;
    public readonly bool developerOnly;
    public readonly Game.Input.Mode mode;
    public readonly ReadOnlyCollection<string> interactions;
    public readonly ReadOnlyCollection<string> processors;
    private readonly string[] customUsages;

    public Usages usages
    {
      get
      {
        return this.customUsages != null && this.customUsages.Length != 0 ? new Usages(true, this.customUsages) : Usages.defaultUsages;
      }
    }

    protected SettingsUIInputActionAttribute(
      string name,
      InputManager.DeviceType device,
      ActionType type,
      bool allowModifiers,
      bool developerOnly,
      Game.Input.Mode mode,
      string[] customUsages,
      string[] interactions,
      string[] processors)
    {
      this.name = name;
      this.device = device;
      this.type = type;
      this.allowModifiers = allowModifiers;
      this.developerOnly = developerOnly;
      this.mode = mode;
      this.interactions = new ReadOnlyCollection<string>((IList<string>) (interactions ?? Array.Empty<string>()));
      this.processors = new ReadOnlyCollection<string>((IList<string>) (processors ?? Array.Empty<string>()));
      this.customUsages = customUsages ?? Array.Empty<string>();
    }

    protected SettingsUIInputActionAttribute(
      string name,
      InputManager.DeviceType device,
      ActionType type,
      Game.Input.Mode mode,
      string[] customUsages)
      : this(name, device, type, true, false, mode, customUsages, Array.Empty<string>(), Array.Empty<string>())
    {
    }
  }
}
