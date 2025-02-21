// Decompiled with JetBrains decompiler
// Type: Game.Modding.ModSetting
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.Reflection;
using Game.Input;
using Game.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Modding
{
  public abstract class ModSetting : Setting
  {
    private PropertyInfo[] m_keyBindingProperties;

    internal static Dictionary<string, ModSetting> instances { get; } = new Dictionary<string, ModSetting>();

    internal IMod mod { get; }

    protected internal override sealed bool builtIn => false;

    [SettingsUIHidden]
    public string id { get; }

    [SettingsUIHidden]
    public string name { get; }

    [SettingsUIHidden]
    public bool keyBindingRegistered { get; private set; }

    private PropertyInfo[] keyBindingProperties
    {
      get
      {
        return this.m_keyBindingProperties ?? (this.m_keyBindingProperties = ((IEnumerable<PropertyInfo>) this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.CanRead && p.CanWrite && p.PropertyType == typeof (ProxyBinding))).ToArray<PropertyInfo>());
      }
    }

    public ModSetting(IMod mod)
    {
      Type type = mod.GetType();
      this.id = type.Assembly.GetName().Name + "." + type.Namespace + "." + type.Name;
      this.name = this.GetType().Name;
      this.mod = mod;
      ModSetting.instances[this.id] = this;
      this.InitializeKeyBindings();
    }

    public void RegisterInOptionsUI() => this.RegisterInOptionsUI(this.id, true);

    public void UnregisterInOptionsUI() => Setting.UnregisterInOptionsUI(this.id);

    private void InitializeKeyBindings()
    {
      foreach (PropertyInfo keyBindingProperty in this.keyBindingProperties)
      {
        ProxyBinding binding = this.GenerateBinding(keyBindingProperty);
        keyBindingProperty.SetValue((object) this, (object) binding);
      }
    }

    private ProxyBinding GenerateBinding(PropertyInfo property)
    {
      SettingsUIKeyboardBindingAttribute attribute1;
      string actionName;
      InputManager.DeviceType device;
      ActionType type;
      ActionComponent component;
      string control;
      IEnumerable<string> modifierControls;
      if (property.TryGetAttribute<SettingsUIKeyboardBindingAttribute>(out attribute1))
      {
        actionName = attribute1.actionName ?? property.Name;
        device = attribute1.device;
        type = attribute1.type;
        component = attribute1.component;
        control = attribute1.control;
        modifierControls = attribute1.modifierControls;
      }
      else
      {
        SettingsUIGamepadBindingAttribute attribute2;
        if (property.TryGetAttribute<SettingsUIGamepadBindingAttribute>(out attribute2))
        {
          actionName = attribute2.actionName ?? property.Name;
          device = attribute2.device;
          type = attribute2.type;
          component = attribute2.component;
          control = attribute2.control;
          modifierControls = attribute2.modifierControls;
        }
        else
        {
          SettingsUIMouseBindingAttribute attribute3;
          if (property.TryGetAttribute<SettingsUIMouseBindingAttribute>(out attribute3))
          {
            actionName = attribute3.actionName ?? property.Name;
            device = attribute3.device;
            type = attribute3.type;
            component = attribute3.component;
            control = attribute3.control;
            modifierControls = attribute3.modifierControls;
          }
          else
          {
            actionName = property.Name;
            device = InputManager.DeviceType.Keyboard;
            type = ActionType.Button;
            component = ActionComponent.Press;
            control = string.Empty;
            modifierControls = (IEnumerable<string>) Array.Empty<string>();
          }
        }
      }
      ProxyBinding sourceBinding;
      return !this.TryGetSourceBindingForMimic(property, device, component, out sourceBinding) ? this.CreateBinding(device, actionName, type, component, control, modifierControls) : this.CreateMimicBinding(device, actionName, type, component, sourceBinding);
    }

    private bool TryGetSourceBindingForMimic(
      PropertyInfo property,
      InputManager.DeviceType device,
      ActionComponent component,
      out ProxyBinding sourceBinding)
    {
      sourceBinding = new ProxyBinding();
      SettingsUIBindingMimicAttribute attribute;
      ProxyAction action;
      ProxyComposite composite;
      return property.TryGetAttribute<SettingsUIBindingMimicAttribute>(out attribute) && InputManager.instance.TryFindAction(attribute.map, attribute.action, out action) && action.isBuiltIn && action.TryGetComposite(device, out composite) && composite.TryGetBinding(component, out sourceBinding);
    }

    public void RegisterKeyBindings()
    {
      if (this.keyBindingRegistered)
        return;
      PropertyInfo[] bindingProperties = this.keyBindingProperties;
      Dictionary<string, (ProxyAction.Info, List<PropertyInfo>)> dictionary1 = new Dictionary<string, (ProxyAction.Info, List<PropertyInfo>)>();
      Dictionary<(string, InputManager.DeviceType), SettingsUIInputActionAttribute> dictionary2 = new Dictionary<(string, InputManager.DeviceType), SettingsUIInputActionAttribute>();
      foreach (SettingsUIInputActionAttribute attribute in ReflectionUtils.GetAttributes<SettingsUIInputActionAttribute>(this.GetType().GetCustomAttributes(false)))
        dictionary2.TryAdd((attribute.name, attribute.device), attribute);
      for (int index = 0; index < bindingProperties.Length; ++index)
      {
        PropertyInfo propertyInfo = bindingProperties[index];
        ProxyBinding binding = (ProxyBinding) propertyInfo.GetValue((object) this);
        (ProxyAction.Info, List<PropertyInfo>) valueTuple;
        if (!dictionary1.TryGetValue(binding.actionName, out valueTuple))
          valueTuple = (new ProxyAction.Info()
          {
            m_Map = binding.mapName,
            m_Name = binding.actionName,
            m_Type = binding.component.GetActionType(),
            m_Composites = new List<ProxyComposite.Info>()
          }, new List<PropertyInfo>());
        if (binding.component.GetActionType() == valueTuple.Item1.m_Type)
        {
          valueTuple.Item2.Add(propertyInfo);
          ProxyComposite.Info info1 = valueTuple.Item1.m_Composites.FirstOrDefault<ProxyComposite.Info>((Func<ProxyComposite.Info, bool>) (info => info.m_Device == binding.device));
          if (info1.m_Source == null)
          {
            InputManager.CompositeData data;
            if (InputManager.TryGetCompositeData(binding.component.GetActionType(), out data))
            {
              CompositeInstance compositeInstance = new CompositeInstance(data.m_Type)
              {
                builtIn = false
              };
              SettingsUIInputActionAttribute inputActionAttribute;
              if (dictionary2.TryGetValue((binding.actionName, binding.device), out inputActionAttribute))
              {
                compositeInstance.allowModifiers = inputActionAttribute.allowModifiers;
                compositeInstance.developerOnly = inputActionAttribute.developerOnly;
                compositeInstance.mode = inputActionAttribute.mode;
                compositeInstance.usages = inputActionAttribute.usages;
                compositeInstance.interactions.AddRange(inputActionAttribute.interactions.Select<string, NameAndParameters>(new Func<string, NameAndParameters>(NameAndParameters.Parse)));
                compositeInstance.processors.AddRange(inputActionAttribute.processors.Select<string, NameAndParameters>(new Func<string, NameAndParameters>(NameAndParameters.Parse)));
              }
              else
                compositeInstance.allowModifiers = true;
              info1 = new ProxyComposite.Info()
              {
                m_Device = binding.device,
                m_Source = compositeInstance,
                m_Bindings = new List<ProxyBinding>()
              };
              valueTuple.Item1.m_Composites.Add(info1);
            }
            else
              continue;
          }
          info1.m_Bindings.Add(binding);
          dictionary1[binding.actionName] = valueTuple;
        }
      }
      InputManager.instance.AddActions(dictionary1.Values.Select<(ProxyAction.Info, List<PropertyInfo>), ProxyAction.Info>((Func<(ProxyAction.Info, List<PropertyInfo>), ProxyAction.Info>) (d => d.actionInfo)).ToArray<ProxyAction.Info>());
      foreach (PropertyInfo propertyInfo in bindingProperties)
      {
        PropertyInfo property = propertyInfo;
        ProxyBinding binding = (ProxyBinding) property.GetValue((object) this);
        binding.CreateWatcher((Action<ProxyBinding>) (newBinding => property.SetValue((object) this, (object) newBinding)));
        ProxyBinding sourceBinding;
        if (this.TryGetSourceBindingForMimic(property, binding.device, binding.component, out sourceBinding))
          sourceBinding.CreateWatcher((Action<ProxyBinding>) (newSourceBinding => InputManager.instance.SetBinding(binding.Copy() with
          {
            path = newSourceBinding.path,
            modifiers = newSourceBinding.modifiers
          }, out ProxyBinding _)));
      }
      this.keyBindingRegistered = true;
    }

    private ProxyBinding CreateBinding(
      InputManager.DeviceType device,
      string actionName,
      ActionType type,
      ActionComponent component,
      string control,
      IEnumerable<string> modifierControls)
    {
      InputManager.CompositeData data;
      InputManager.CompositeComponentData componentData;
      if (!InputManager.TryGetCompositeData(type, out data) || !data.TryGetData(component, out componentData))
        componentData = InputManager.CompositeComponentData.defaultData;
      ProxyModifier[] array = modifierControls.Select<string, ProxyModifier>((Func<string, ProxyModifier>) (modifierControl => new ProxyModifier()
      {
        m_Component = component,
        m_Name = componentData.m_ModifierName,
        m_Path = modifierControl
      })).ToArray<ProxyModifier>();
      return new ProxyBinding(this.id, actionName, component, componentData.m_BindingName, new CompositeInstance(device.ToString()))
      {
        device = device,
        path = control,
        originalPath = control,
        modifiers = (IReadOnlyList<ProxyModifier>) array,
        originalModifiers = (IReadOnlyList<ProxyModifier>) array
      };
    }

    private ProxyBinding CreateMimicBinding(
      InputManager.DeviceType device,
      string actionName,
      ActionType type,
      ActionComponent component,
      ProxyBinding sourceBinding)
    {
      InputManager.CompositeData data1;
      InputManager.CompositeComponentData data2;
      if (!InputManager.TryGetCompositeData(type, out data1) || !data1.TryGetData(component, out data2))
        data2 = InputManager.CompositeComponentData.defaultData;
      return new ProxyBinding(this.id, actionName, component, data2.m_BindingName, new CompositeInstance(device.ToString()))
      {
        device = device,
        path = sourceBinding.path,
        originalPath = sourceBinding.originalPath,
        modifiers = sourceBinding.modifiers,
        originalModifiers = sourceBinding.modifiers
      };
    }

    [AfterDecode]
    protected internal void ApplyKeyBindings()
    {
      if (!this.keyBindingRegistered)
        return;
      InputManager.instance.SetBindings((IEnumerable<ProxyBinding>) ((IEnumerable<PropertyInfo>) this.keyBindingProperties).Select<PropertyInfo, ProxyBinding>((Func<PropertyInfo, ProxyBinding>) (p => (ProxyBinding) p.GetValue((object) this))).ToArray<ProxyBinding>(), out List<ProxyBinding> _);
    }

    protected void ResetKeyBindings()
    {
      if (!this.keyBindingRegistered)
        return;
      InputManager.instance.SetBindings((IEnumerable<ProxyBinding>) ((IEnumerable<PropertyInfo>) this.keyBindingProperties).Select<PropertyInfo, ProxyBinding>(new Func<PropertyInfo, ProxyBinding>(this.GenerateBinding)).ToArray<ProxyBinding>(), out List<ProxyBinding> _);
      this.ApplyAndSave();
    }

    public ProxyAction GetAction(string name) => InputManager.instance.FindAction(this.id, name);

    public IEnumerable<ProxyAction> GetActions()
    {
      ProxyActionMap map;
      return !InputManager.instance.TryFindActionMap(this.id, out map) ? (IEnumerable<ProxyAction>) Array.Empty<ProxyAction>() : map.actions.Values;
    }

    public string GetSettingsLocaleID() => "Options.SECTION[" + this.id + "]";

    public string GetOptionLabelLocaleID(string optionName)
    {
      return "Options.OPTION[" + this.id + "." + this.name + "." + optionName + "]";
    }

    public string GetOptionDescLocaleID(string optionName)
    {
      return "Options.OPTION_DESCRIPTION[" + this.id + "." + this.name + "." + optionName + "]";
    }

    public string GetOptionWarningLocaleID(string optionName)
    {
      return "Options.WARNING[" + this.id + "." + this.name + "." + optionName + "]";
    }

    public string GetOptionTabLocaleID(string tabName)
    {
      return "Options.TAB[" + this.id + "." + tabName + "]";
    }

    public string GetOptionGroupLocaleID(string groupName)
    {
      return "Options.GROUP[" + this.id + "." + groupName + "]";
    }

    public string GetEnumValueLocaleID<T>(T value) where T : Enum
    {
      return string.Format("Options.{0}.{1}[{2}]", (object) this.id, (object) typeof (T).Name.ToUpper(), (object) value);
    }

    public string GetOptionFormatLocaleID(string optionName)
    {
      return "Options.FORMAT[" + this.id + "." + this.name + "." + optionName + "]";
    }

    public string GetBindingKeyLocaleID(string actionName)
    {
      return this.GetBindingKeyLocaleID(actionName, InputManager.GetBindingName(ActionComponent.Press));
    }

    public string GetBindingKeyLocaleID(string actionName, AxisComponent component)
    {
      return this.GetBindingKeyLocaleID(actionName, InputManager.GetBindingName((ActionComponent) component));
    }

    public string GetBindingKeyLocaleID(string actionName, Vector2Component component)
    {
      return this.GetBindingKeyLocaleID(actionName, InputManager.GetBindingName((ActionComponent) component));
    }

    private string GetBindingKeyLocaleID(string actionName, string componentName)
    {
      return "Options.OPTION[" + this.id + "/" + actionName + "/" + componentName + "]";
    }

    public string GetBindingKeyHintLocaleID(string actionName)
    {
      return "Common.ACTION[" + this.id + "/" + actionName + "]";
    }

    public string GetBindingMapLocaleID() => "Options.INPUT_MAP[" + this.id + "]";
  }
}
