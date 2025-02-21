// Decompiled with JetBrains decompiler
// Type: Game.Input.CompositeInstance
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  public class CompositeInstance : ICustomComposite
  {
    private bool m_IsRebindable = true;
    private bool m_IsModifiersRebindable = true;
    private bool m_AllowModifiers = true;
    private bool m_CanBeEmpty = true;
    private bool m_DeveloperOnly;
    private Platform m_Platform = Platform.All;
    private bool m_BuiltIn = true;
    private bool m_IsDummy;
    private bool m_IsHidden;
    public OptionGroupOverride m_OptionGroupOverride;
    private Usages m_Usages = ICustomComposite.defaultUsages;

    public string typeName { get; internal set; }

    public bool isRebindable
    {
      get => this.isDummy || this.m_IsRebindable;
      set => this.m_IsRebindable = value;
    }

    public bool isModifiersRebindable
    {
      get
      {
        if (this.isDummy)
          return true;
        return this.m_IsModifiersRebindable && this.isRebindable && this.allowModifiers;
      }
      set => this.m_IsModifiersRebindable = value;
    }

    public bool allowModifiers
    {
      get => this.isDummy || this.m_AllowModifiers;
      set => this.m_AllowModifiers = value;
    }

    public bool canBeEmpty
    {
      get
      {
        if (this.isDummy)
          return true;
        return this.m_CanBeEmpty && this.isRebindable;
      }
      set => this.m_CanBeEmpty = value;
    }

    public bool developerOnly
    {
      get => this.m_DeveloperOnly;
      set => this.m_DeveloperOnly = value;
    }

    public Platform platform
    {
      get => this.m_Platform;
      set => this.m_Platform = value;
    }

    public bool builtIn
    {
      get => this.m_BuiltIn;
      set => this.m_BuiltIn = value;
    }

    public bool isDummy
    {
      get => this.m_IsDummy;
      set => this.m_IsDummy = value;
    }

    public bool isHidden
    {
      get => this.m_IsHidden;
      set => this.m_IsHidden = value;
    }

    public OptionGroupOverride optionGroupOverride
    {
      get => this.isHidden ? OptionGroupOverride.None : this.m_OptionGroupOverride;
      set => this.m_OptionGroupOverride = value;
    }

    public Usages usages
    {
      get => this.m_Usages.Copy();
      set => this.m_Usages.SetFrom(value);
    }

    public Guid linkedGuid { get; set; }

    public List<NameAndParameters> processors { get; } = new List<NameAndParameters>();

    public List<NameAndParameters> interactions { get; } = new List<NameAndParameters>();

    public Mode mode { get; set; }

    public InputManager.CompositeData compositeData
    {
      get
      {
        InputManager.CompositeData data;
        return !InputManager.TryGetCompositeData(this.typeName, out data) ? new InputManager.CompositeData() : data;
      }
    }

    public NameAndParameters parameters
    {
      get
      {
        long part1;
        long part2;
        CompositeUtility.SetGuid(this.linkedGuid, out part1, out part2);
        return new NameAndParameters()
        {
          name = this.typeName,
          parameters = new ReadOnlyArray<NamedValue>(new NamedValue[13]
          {
            NamedValue.From<bool>("m_IsRebindable", this.isRebindable),
            NamedValue.From<bool>("m_IsModifiersRebindable", this.isModifiersRebindable),
            NamedValue.From<bool>("m_AllowModifiers", this.allowModifiers),
            NamedValue.From<bool>("m_CanBeEmpty", this.canBeEmpty),
            NamedValue.From<bool>("m_DeveloperOnly", this.developerOnly),
            NamedValue.From<Platform>("m_Platform", this.platform),
            NamedValue.From<bool>("m_BuiltIn", this.builtIn),
            NamedValue.From<Mode>("m_Mode", this.mode),
            NamedValue.From<bool>("m_IsDummy", this.isDummy),
            NamedValue.From<bool>("m_IsHidden", this.isHidden),
            NamedValue.From<OptionGroupOverride>("m_OptionGroupOverride", this.optionGroupOverride),
            NamedValue.From<long>("m_LinkGuid1", part1),
            NamedValue.From<long>("m_LinkGuid2", part2)
          })
        };
      }
      set
      {
        long part1 = 0;
        long part2 = 0;
        foreach (NamedValue parameter in value.parameters)
        {
          switch (parameter.name)
          {
            case "m_AllowModifiers":
              this.allowModifiers = parameter.value.ToBoolean();
              continue;
            case "m_BuiltIn":
              this.builtIn = parameter.value.ToBoolean();
              continue;
            case "m_CanBeEmpty":
              this.canBeEmpty = parameter.value.ToBoolean();
              continue;
            case "m_DeveloperOnly":
              this.developerOnly = parameter.value.ToBoolean();
              continue;
            case "m_IsDummy":
              this.isDummy = parameter.value.ToBoolean();
              continue;
            case "m_IsHidden":
              this.isHidden = parameter.value.ToBoolean();
              continue;
            case "m_IsModifiersRebindable":
              this.isModifiersRebindable = parameter.value.ToBoolean();
              continue;
            case "m_IsRebindable":
              this.isRebindable = parameter.value.ToBoolean();
              continue;
            case "m_LinkGuid1":
              part1 = parameter.value.ToInt64();
              continue;
            case "m_LinkGuid2":
              part2 = parameter.value.ToInt64();
              continue;
            case "m_Mode":
              this.mode = (Mode) parameter.value.ToInt32();
              continue;
            case "m_OptionGroupOverride":
              this.optionGroupOverride = (OptionGroupOverride) parameter.value.ToInt32();
              continue;
            case "m_Platform":
              this.platform = (Platform) parameter.value.ToInt32();
              continue;
            case "m_Usages":
              this.m_Usages = new Usages((BuiltInUsages) parameter.value.ToInt32());
              continue;
            default:
              continue;
          }
        }
        this.linkedGuid = CompositeUtility.GetGuid(part1, part2);
      }
    }

    public CompositeInstance(string typeName) => this.typeName = typeName;

    public CompositeInstance(NameAndParameters parameters)
      : this(parameters.name)
    {
      this.m_Usages = ICustomComposite.defaultUsages;
      this.parameters = parameters;
      this.m_Usages.MakeReadOnly();
    }

    public CompositeInstance(NameAndParameters parameters, NameAndParameters usages)
      : this(parameters.name)
    {
      this.m_Usages = new Usages(0, false);
      this.parameters = parameters;
      this.m_Usages.parameters = usages;
      this.m_Usages.MakeReadOnly();
    }
  }
}
