// Decompiled with JetBrains decompiler
// Type: Game.Input.ICustomComposite
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  public interface ICustomComposite
  {
    const bool kDefaultIsRebindable = true;
    const bool kDefaultIsModifiersRebindable = true;
    const bool kDefaultAllowModifiers = true;
    const bool kDefaultCanBeEmpty = true;
    const bool kDefaultDeveloperOnly = false;
    const bool kDefaultBuiltIn = true;
    const bool kDefaultIsDummy = false;
    const bool kDefaultIsHidden = false;
    const Mode kDefaultMode = Mode.DigitalNormalized;
    const OptionGroupOverride kDefaultOptionGroupOverride = OptionGroupOverride.None;
    const Platform kDefaultPlatform = Platform.All;

    bool isRebindable { get; }

    bool isModifiersRebindable { get; }

    bool allowModifiers { get; }

    bool canBeEmpty { get; }

    bool developerOnly { get; }

    Platform platform { get; }

    bool builtIn { get; }

    bool isDummy { get; }

    bool isHidden { get; }

    Usages usages { get; }

    NameAndParameters parameters { get; }

    string typeName { get; }

    OptionGroupOverride optionGroupOverride { get; }

    Guid linkedGuid { get; }

    static Usages defaultUsages => Usages.defaultUsages.Copy(false);
  }
}
