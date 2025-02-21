// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.AutomaticSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.OdinSerializer.Utilities;
using Colossal.Reflection;
using Colossal.UI.Binding;
using Game.Input;
using Game.Reflection;
using Game.SceneFlow;
using Game.Settings;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.UI.Menu
{
  public static class AutomaticSettings
  {
    private static readonly MethodInfo s_CustomDropdownMethodInfo = typeof (AutomaticSettings).GetMethod("AddCustomDropdownPropertyGeneric", BindingFlags.Static | BindingFlags.Public);
    private static readonly Dictionary<string, ButtonRow> s_ButtonGroups = new Dictionary<string, ButtonRow>();

    private static bool GetButtonsGroup(string groupName, out ButtonRow buttons, Button item)
    {
      if (AutomaticSettings.s_ButtonGroups.TryGetValue(groupName, out buttons))
      {
        buttons.children = new List<Button>((IEnumerable<Button>) buttons.children)
        {
          item
        }.ToArray();
        return false;
      }
      buttons = new ButtonRow()
      {
        children = new Button[1]{ item }
      };
      AutomaticSettings.s_ButtonGroups.Add(groupName, buttons);
      return true;
    }

    private static bool IsHidden(AutomaticSettings.IProxyProperty property)
    {
      return property.GetAttribute<SettingsUIHiddenAttribute>() != null;
    }

    private static bool IsSupportedOnPlatform(AutomaticSettings.IProxyProperty property)
    {
      SettingsUIPlatformAttribute attribute = property.GetAttribute<SettingsUIPlatformAttribute>();
      return attribute == null || attribute.IsPlatformSet(Application.platform);
    }

    private static bool IsDeveloperOnly(AutomaticSettings.IProxyProperty property)
    {
      return property.GetAttribute<SettingsUIDeveloperAttribute>() != null && !GameManager.instance.configuration.developerMode;
    }

    private static Dictionary<string, AutomaticSettings.SectionInfo> GetSections(
      AutomaticSettings.IProxyProperty property)
    {
      Dictionary<string, AutomaticSettings.SectionInfo> sections = new Dictionary<string, AutomaticSettings.SectionInfo>();
      foreach (SettingsUISectionAttribute attribute in property.GetAttributes<SettingsUISectionAttribute>())
        sections[attribute.tab] = new AutomaticSettings.SectionInfo()
        {
          m_Tab = attribute.tab,
          m_SimpleGroup = attribute.simpleGroup,
          m_AdvancedGroup = attribute.advancedGroup
        };
      if (sections.Count != 0)
        return sections;
      foreach (SettingsUISectionAttribute attribute in ReflectionUtils.GetAttributes<SettingsUISectionAttribute>(property.declaringType.GetCustomAttributes(false)))
        sections[attribute.tab] = new AutomaticSettings.SectionInfo()
        {
          m_Tab = attribute.tab,
          m_SimpleGroup = attribute.simpleGroup,
          m_AdvancedGroup = attribute.advancedGroup
        };
      if (sections.Count != 0)
        return sections;
      sections["General"] = new AutomaticSettings.SectionInfo()
      {
        m_Tab = "General",
        m_SimpleGroup = string.Empty,
        m_AdvancedGroup = string.Empty
      };
      return sections;
    }

    public static bool TryGetAction<T>(
      Setting setting,
      System.Type type,
      string name,
      out Func<T> action)
    {
      try
      {
        if (type != (System.Type) null)
        {
          if (!string.IsNullOrEmpty(name))
          {
            BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            if (type.IsInstanceOfType((object) setting))
              bindingAttr |= BindingFlags.Instance;
            foreach (MethodInfo method in type.GetMethods(bindingAttr))
            {
              if (!(method.Name != name) && !(method.ReturnType != typeof (T)) && method.GetParameters().Length == 0)
              {
                action = (Func<T>) method.CreateDelegate(typeof (Func<T>), method.IsStatic ? (object) (Setting) null : (object) setting);
                return true;
              }
            }
            foreach (PropertyInfo property in type.GetProperties(bindingAttr))
            {
              if (!(property.Name != name) && property.CanRead && !(property.PropertyType != typeof (T)))
              {
                MethodInfo getMethod = property.GetGetMethod(true);
                action = (Func<T>) getMethod.CreateDelegate(typeof (Func<T>), getMethod.IsStatic ? (object) (Setting) null : (object) setting);
                return true;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("TryGetAction error with {0} in {1}", (object) name, (object) type), ex);
      }
      action = (Func<T>) null;
      return false;
    }

    private static AutomaticSettings.DropdownItemsAccessor<DropdownItem<T>[]> GetDropdownItemAccessor<T>(
      AutomaticSettings.IProxyProperty property,
      Setting setting)
    {
      SettingsUIDropdownAttribute attribute = property.GetAttribute<SettingsUIDropdownAttribute>();
      Func<DropdownItem<T>[]> action;
      return attribute != null && AutomaticSettings.TryGetAction<DropdownItem<T>[]>(setting, attribute.itemsGetterType, attribute.itemsGetterMethod, out action) ? new AutomaticSettings.DropdownItemsAccessor<DropdownItem<T>[]>(action) : (AutomaticSettings.DropdownItemsAccessor<DropdownItem<T>[]>) null;
    }

    private static DelegateAccessor<EnumMember[]> GetEnumMemberAccessor(
      AutomaticSettings.IProxyProperty property,
      Setting setting,
      string prefix)
    {
      SettingsUIDropdownAttribute attribute = property.GetAttribute<SettingsUIDropdownAttribute>();
      Func<EnumMember[]> action;
      if (attribute != null && AutomaticSettings.TryGetAction<EnumMember[]>(setting, attribute.itemsGetterType, attribute.itemsGetterMethod, out action))
        return new DelegateAccessor<EnumMember[]>(action);
      prefix = string.IsNullOrEmpty(prefix) ? "Options" : "Options." + prefix;
      return new DelegateAccessor<EnumMember[]>((Func<EnumMember[]>) (() => AutomaticSettings.GetEnumValues(property.propertyType, prefix)));
    }

    public static EnumMember[] GetEnumValues(System.Type underlyingType, string prefix)
    {
      List<EnumMember> enumMemberList = new List<EnumMember>();
      string[] names = Enum.GetNames(underlyingType);
      Array values = Enum.GetValues(underlyingType);
      for (int index = 0; index < names.Length; ++index)
      {
        ulong num = (ulong) (int) values.GetValue(index);
        if (underlyingType.GetField(names[index]).GetCustomAttributes(typeof (SettingsUIHiddenAttribute), false).Length == 0)
        {
          string displayName = underlyingType.Name.ToUpperInvariant() + "[" + names[index] + "]";
          if (!string.IsNullOrEmpty(prefix))
            displayName = prefix + "." + displayName;
          enumMemberList.Add(new EnumMember(num, (LocalizedString) displayName));
        }
      }
      return enumMemberList.ToArray();
    }

    private static bool IsShowGroupName(
      Setting setting,
      out bool showAll,
      out ReadOnlyCollection<string> groups)
    {
      SettingsUIShowGroupNameAttribute attribute = ReflectionUtils.GetAttribute<SettingsUIShowGroupNameAttribute>(setting.GetType().GetCustomAttributes(false));
      if (attribute != null)
      {
        showAll = attribute.showAll;
        groups = attribute.groups;
        return true;
      }
      showAll = false;
      groups = (ReadOnlyCollection<string>) null;
      return false;
    }

    private static Func<bool> GetWarningGetter(Setting setting)
    {
      SettingsUIPageWarningAttribute attribute = ReflectionUtils.GetAttribute<SettingsUIPageWarningAttribute>(setting.GetType().GetCustomAttributes(false));
      Func<bool> action;
      return attribute != null && AutomaticSettings.TryGetAction<bool>(setting, attribute.checkType, attribute.checkMethod, out action) ? action : (Func<bool>) null;
    }

    private static Dictionary<string, Func<bool>> GetTabWarningGetters(Setting setting)
    {
      Dictionary<string, Func<bool>> tabWarningGetters = new Dictionary<string, Func<bool>>();
      foreach (SettingsUITabWarningAttribute attribute in ReflectionUtils.GetAttributes<SettingsUITabWarningAttribute>(setting.GetType().GetCustomAttributes(false)))
      {
        Func<bool> action;
        if (!string.IsNullOrEmpty(attribute.tab) && AutomaticSettings.TryGetAction<bool>(setting, attribute.checkType, attribute.checkMethod, out action))
          tabWarningGetters.TryAdd(attribute.tab, action);
      }
      return tabWarningGetters;
    }

    public static AutomaticSettings.SettingPageData FillSettingsPage(
      Setting setting,
      string id,
      bool addPrefix)
    {
      AutomaticSettings.s_ButtonGroups.Clear();
      if (setting == null)
        return (AutomaticSettings.SettingPageData) null;
      AutomaticSettings.SettingPageData pageData = new AutomaticSettings.SettingPageData(id, addPrefix);
      bool showAll;
      ReadOnlyCollection<string> groups;
      if (AutomaticSettings.IsShowGroupName(setting, out showAll, out groups))
      {
        if (showAll)
        {
          pageData.showAllGroupNames = true;
        }
        else
        {
          foreach (string group in groups)
            pageData.AddGroupToShowName(group);
        }
      }
      pageData.warningGetter = AutomaticSettings.GetWarningGetter(setting);
      pageData.tabWarningGetters = AutomaticSettings.GetTabWarningGetters(setting);
      AutomaticSettings.FillSettingsPage(pageData, setting);
      AutomaticSettings.s_ButtonGroups.Clear();
      return pageData;
    }

    public static void FillSettingsPage(AutomaticSettings.SettingPageData pageData, Setting setting)
    {
      SettingsUITabOrderAttribute attribute1;
      if (setting.GetType().TryGetAttribute<SettingsUITabOrderAttribute>(out attribute1, false))
      {
        Func<string[]> action;
        if (AutomaticSettings.TryGetAction<string[]>(setting, attribute1.checkType, attribute1.checkMethod, out action))
        {
          foreach (string tab in action())
            pageData.AddTab(tab);
        }
        else
        {
          foreach (string tab in attribute1.tabs)
            pageData.AddTab(tab);
        }
      }
      SettingsUIGroupOrderAttribute attribute2;
      if (setting.GetType().TryGetAttribute<SettingsUIGroupOrderAttribute>(out attribute2, false))
      {
        Func<string[]> action;
        if (AutomaticSettings.TryGetAction<string[]>(setting, attribute2.checkType, attribute2.checkMethod, out action))
        {
          foreach (string group in action())
            pageData.AddGroup(group);
        }
        else
        {
          foreach (string group in attribute2.groups)
            pageData.AddGroup(group);
        }
      }
      foreach (PropertyInfo property1 in setting.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        AutomaticSettings.ProxyProperty property2 = new AutomaticSettings.ProxyProperty(property1);
        if (AutomaticSettings.IsSupportedOnPlatform((AutomaticSettings.IProxyProperty) property2) && !AutomaticSettings.IsHidden((AutomaticSettings.IProxyProperty) property2) && !AutomaticSettings.IsDeveloperOnly((AutomaticSettings.IProxyProperty) property2))
        {
          AutomaticSettings.WidgetType widgetType = AutomaticSettings.GetWidgetType((AutomaticSettings.IProxyProperty) property2);
          if (widgetType != AutomaticSettings.WidgetType.None)
          {
            foreach (AutomaticSettings.SectionInfo sectionInfo in AutomaticSettings.GetSections((AutomaticSettings.IProxyProperty) property2).Values)
            {
              AutomaticSettings.SettingItemData settingItemData = widgetType != AutomaticSettings.WidgetType.MultilineText ? new AutomaticSettings.SettingItemData(widgetType, setting, (AutomaticSettings.IProxyProperty) property2, pageData.prefix) : (AutomaticSettings.SettingItemData) new MultilineTextSettingItemData(setting, (AutomaticSettings.IProxyProperty) property2, pageData.prefix);
              settingItemData.simpleGroup = sectionInfo.m_SimpleGroup;
              settingItemData.advancedGroup = sectionInfo.m_AdvancedGroup;
              pageData[sectionInfo.m_Tab].AddItem(settingItemData);
              pageData.AddGroup(settingItemData.simpleGroup);
              pageData.AddGroup(settingItemData.advancedGroup);
            }
          }
        }
      }
    }

    public static AutomaticSettings.WidgetType GetWidgetType(
      AutomaticSettings.IProxyProperty property)
    {
      if (property.propertyType == typeof (bool))
      {
        if (property.HasAttribute<SettingsUIButtonAttribute>())
          return property.HasAttribute<SettingsUIConfirmationAttribute>() ? AutomaticSettings.WidgetType.BoolButtonWithConfirmation : AutomaticSettings.WidgetType.BoolButton;
        if (property.canRead && property.canWrite)
          return AutomaticSettings.WidgetType.BoolToggle;
        return !property.canRead && property.canWrite ? AutomaticSettings.WidgetType.BoolButton : AutomaticSettings.WidgetType.None;
      }
      if (property.propertyType == typeof (int))
      {
        if (property.HasAttribute<SettingsUIDropdownAttribute>())
          return AutomaticSettings.WidgetType.IntDropdown;
        return property.HasAttribute<SettingsUISliderAttribute>() ? AutomaticSettings.WidgetType.IntSlider : AutomaticSettings.WidgetType.None;
      }
      if (property.propertyType == typeof (float))
        return property.HasAttribute<SettingsUISliderAttribute>() ? AutomaticSettings.WidgetType.FloatSlider : AutomaticSettings.WidgetType.None;
      if (property.propertyType == typeof (string))
      {
        if (property.canRead && property.canWrite)
        {
          if (property.HasAttribute<SettingsUITextInputAttribute>())
            return AutomaticSettings.WidgetType.StringTextInput;
          if (property.HasAttribute<SettingsUIDropdownAttribute>())
            return AutomaticSettings.WidgetType.StringDropdown;
          return property.HasAttribute<SettingsUIDirectoryPickerAttribute>() ? AutomaticSettings.WidgetType.DirectoryPicker : AutomaticSettings.WidgetType.None;
        }
        if (!property.canRead || property.canWrite)
          return AutomaticSettings.WidgetType.None;
        return property.HasAttribute<SettingsUIMultilineTextAttribute>() ? AutomaticSettings.WidgetType.MultilineText : AutomaticSettings.WidgetType.StringField;
      }
      if (property.propertyType == typeof (LocalizedString))
        return property.canRead && !property.canWrite ? AutomaticSettings.WidgetType.LocalizedStringField : AutomaticSettings.WidgetType.None;
      if (property.propertyType == typeof (ProxyBinding))
        return AutomaticSettings.WidgetType.KeyBinding;
      return property.propertyType.IsEnum ? (property.HasAttribute<SettingsUIDropdownAttribute>() ? AutomaticSettings.WidgetType.AdvancedEnumDropdown : AutomaticSettings.WidgetType.EnumDropdown) : (property.HasAttribute<SettingsUIDropdownAttribute>() ? AutomaticSettings.WidgetType.CustomDropdown : AutomaticSettings.WidgetType.None);
    }

    public static LocalizedString GetConfirmationMessage(AutomaticSettings.SettingItemData itemData)
    {
      SettingsUIConfirmationAttribute attribute = itemData.property.GetAttribute<SettingsUIConfirmationAttribute>();
      if (attribute != null)
      {
        if (!string.IsNullOrEmpty(attribute.confirmMessageId))
          return LocalizedString.IdWithFallback("Options.WARNING[" + attribute.confirmMessageId + "]", attribute.confirmMessageValue);
        if (!string.IsNullOrEmpty(attribute.confirmMessageValue))
          return LocalizedString.Value(attribute.confirmMessageValue);
      }
      string str = itemData.property.declaringType.Name + "." + itemData.property.name;
      if (!string.IsNullOrEmpty(itemData.prefix))
        str = itemData.prefix + "." + str;
      return LocalizedString.Id("Options.WARNING[" + str + "]");
    }

    public static IWidget AddBoolButtonWithConfirmationProperty(
      AutomaticSettings.SettingItemData itemData)
    {
      ButtonWithConfirmation withConfirmation1 = new ButtonWithConfirmation();
      withConfirmation1.path = (PathSegment) itemData.path;
      withConfirmation1.displayName = itemData.displayName;
      withConfirmation1.description = itemData.description;
      withConfirmation1.displayNameAction = itemData.dispayNameAction;
      withConfirmation1.descriptionAction = itemData.descriptionAction;
      withConfirmation1.action = (Action) (() => itemData.property.SetValue((object) itemData.setting, (object) true));
      withConfirmation1.disabled = itemData.disableAction;
      withConfirmation1.hidden = itemData.hideAction;
      withConfirmation1.confirmationMessage = new LocalizedString?(AutomaticSettings.GetConfirmationMessage(itemData));
      ButtonWithConfirmation withConfirmation2 = withConfirmation1;
      ButtonRow buttons;
      return AutomaticSettings.GetButtonsGroup(itemData.property.GetAttribute<SettingsUIButtonGroupAttribute>()?.name ?? itemData.property.declaringType.Name + "." + itemData.property.name + "_ButtonGroup", out buttons, (Button) withConfirmation2) ? (IWidget) buttons : (IWidget) null;
    }

    public static IWidget AddBoolToggleProperty(AutomaticSettings.SettingItemData itemData)
    {
      if (!itemData.property.canRead || !itemData.property.canWrite)
        return (IWidget) null;
      Action<bool> setterAction = itemData.setterAction as Action<bool>;
      ToggleField toggleField = new ToggleField();
      toggleField.path = (PathSegment) itemData.path;
      toggleField.displayName = itemData.displayName;
      toggleField.description = itemData.description;
      toggleField.displayNameAction = itemData.dispayNameAction;
      toggleField.descriptionAction = itemData.descriptionAction;
      toggleField.warningAction = itemData.warningAction;
      toggleField.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => (bool) itemData.property.GetValue((object) itemData.setting)), (Action<bool>) (value =>
      {
        Action<bool> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) value);
        itemData.setting.ApplyAndSave();
      }));
      toggleField.disabled = itemData.disableAction;
      toggleField.hidden = itemData.hideAction;
      return (IWidget) toggleField;
    }

    public static IWidget AddBoolButtonProperty(AutomaticSettings.SettingItemData itemData)
    {
      if (itemData.property.canRead || !itemData.property.canWrite)
        return (IWidget) null;
      Button button1 = new Button();
      button1.path = (PathSegment) itemData.path;
      button1.displayName = itemData.displayName;
      button1.description = itemData.description;
      button1.displayNameAction = itemData.dispayNameAction;
      button1.descriptionAction = itemData.descriptionAction;
      button1.action = (Action) (() => itemData.property.SetValue((object) itemData.setting, (object) true));
      button1.disabled = itemData.disableAction;
      button1.hidden = itemData.hideAction;
      Button button2 = button1;
      ButtonRow buttons;
      return AutomaticSettings.GetButtonsGroup(itemData.property.GetAttribute<SettingsUIButtonGroupAttribute>()?.name ?? itemData.property.declaringType.Name + "." + itemData.property.name + "_ButtonGroup", out buttons, button2) ? (IWidget) buttons : (IWidget) null;
    }

    public static IWidget AddIntDropdownProperty(AutomaticSettings.SettingItemData itemData)
    {
      if (itemData.property.GetAttribute<SettingsUIDropdownAttribute>() == null)
        return (IWidget) null;
      Action<int> setterAction = itemData.setterAction as Action<int>;
      DropdownField<int> dropdownField = new DropdownField<int>();
      dropdownField.path = (PathSegment) itemData.path;
      dropdownField.displayName = itemData.displayName;
      dropdownField.description = itemData.description;
      dropdownField.displayNameAction = itemData.dispayNameAction;
      dropdownField.descriptionAction = itemData.descriptionAction;
      dropdownField.warningAction = itemData.warningAction;
      dropdownField.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => (int) itemData.property.GetValue((object) itemData.setting)), (Action<int>) (value =>
      {
        Action<int> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) value);
        itemData.setting.ApplyAndSave();
      }));
      dropdownField.itemsAccessor = (ITypedValueAccessor<DropdownItem<int>[]>) AutomaticSettings.GetDropdownItemAccessor<int>(itemData.property, itemData.setting);
      dropdownField.itemsVersion = itemData.valueVersionAction;
      dropdownField.disabled = itemData.disableAction;
      dropdownField.hidden = itemData.hideAction;
      return (IWidget) dropdownField;
    }

    public static IWidget AddIntSliderProperty(AutomaticSettings.SettingItemData itemData)
    {
      SettingsUISliderAttribute sliderAttribute = itemData.property.GetAttribute<SettingsUISliderAttribute>();
      if (sliderAttribute == null)
        return (IWidget) null;
      Action<int> setterAction = itemData.setterAction as Action<int>;
      IntSliderField intSliderField1 = new IntSliderField();
      intSliderField1.path = (PathSegment) itemData.path;
      intSliderField1.displayName = itemData.displayName;
      intSliderField1.description = itemData.description;
      intSliderField1.displayNameAction = itemData.dispayNameAction;
      intSliderField1.descriptionAction = itemData.descriptionAction;
      intSliderField1.warningAction = itemData.warningAction;
      intSliderField1.min = (int) sliderAttribute.min;
      intSliderField1.max = (int) sliderAttribute.max;
      intSliderField1.step = (int) sliderAttribute.step;
      intSliderField1.unit = sliderAttribute.unit;
      intSliderField1.scaleDragVolume = sliderAttribute.scaleDragVolume;
      intSliderField1.updateOnDragEnd = sliderAttribute.updateOnDragEnd;
      intSliderField1.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => (int) itemData.property.GetValue((object) itemData.setting) * (int) sliderAttribute.scalarMultiplier), (Action<int>) (value =>
      {
        Action<int> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) Mathf.RoundToInt((float) value / sliderAttribute.scalarMultiplier));
        itemData.setting.ApplyAndSave();
      }));
      intSliderField1.disabled = itemData.disableAction;
      intSliderField1.hidden = itemData.hideAction;
      IntSliderField intSliderField2 = intSliderField1;
      SettingsUICustomFormatAttribute attribute = itemData.property.GetAttribute<SettingsUICustomFormatAttribute>();
      if (attribute != null)
      {
        intSliderField2.unit = "custom";
        intSliderField2.separateThousands = attribute.separateThousands;
        intSliderField2.signed = attribute.signed;
      }
      return (IWidget) intSliderField2;
    }

    public static IWidget AddFloatSliderProperty(AutomaticSettings.SettingItemData itemData)
    {
      SettingsUISliderAttribute attribute = itemData.property.GetAttribute<SettingsUISliderAttribute>();
      Action<float> setterAction = itemData.setterAction as Action<float>;
      FloatSliderField floatSliderField1 = new FloatSliderField();
      floatSliderField1.path = (PathSegment) itemData.path;
      floatSliderField1.displayName = itemData.displayName;
      floatSliderField1.description = itemData.description;
      floatSliderField1.displayNameAction = itemData.dispayNameAction;
      floatSliderField1.descriptionAction = itemData.descriptionAction;
      floatSliderField1.warningAction = itemData.warningAction;
      floatSliderField1.min = (double) attribute.min;
      floatSliderField1.max = (double) attribute.max;
      floatSliderField1.step = (double) attribute.step;
      floatSliderField1.unit = attribute.unit;
      floatSliderField1.scaleDragVolume = attribute.scaleDragVolume;
      floatSliderField1.updateOnDragEnd = attribute.updateOnDragEnd;
      floatSliderField1.accessor = (ITypedValueAccessor<double>) new DelegateAccessor<double>((Func<double>) (() => (double) (float) itemData.property.GetValue((object) itemData.setting) * (double) (int) attribute.scalarMultiplier), (Action<double>) (value =>
      {
        Action<float> action = setterAction;
        if (action != null)
          action((float) value);
        itemData.property.SetValue((object) itemData.setting, (object) ((float) value / attribute.scalarMultiplier));
        itemData.setting.ApplyAndSave();
      }));
      floatSliderField1.disabled = itemData.disableAction;
      floatSliderField1.hidden = itemData.hideAction;
      FloatSliderField floatSliderField2 = floatSliderField1;
      SettingsUICustomFormatAttribute attribute1 = itemData.property.GetAttribute<SettingsUICustomFormatAttribute>();
      if (attribute1 != null)
      {
        floatSliderField2.unit = "custom";
        floatSliderField2.fractionDigits = Math.Max(attribute1.fractionDigits, 0);
        floatSliderField2.separateThousands = attribute1.separateThousands;
        floatSliderField2.maxValueWithFraction = (double) attribute1.maxValueWithFraction;
        floatSliderField2.signed = attribute1.signed;
      }
      return (IWidget) floatSliderField2;
    }

    public static IWidget AddStringTextInputProperty(AutomaticSettings.SettingItemData itemData)
    {
      Action<string> setterAction = itemData.setterAction as Action<string>;
      StringInputField stringInputField = new StringInputField();
      stringInputField.path = (PathSegment) itemData.path;
      stringInputField.displayName = itemData.displayName;
      stringInputField.description = itemData.description;
      stringInputField.displayNameAction = itemData.dispayNameAction;
      stringInputField.descriptionAction = itemData.descriptionAction;
      stringInputField.warningAction = itemData.warningAction;
      stringInputField.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => (string) itemData.property.GetValue((object) itemData.setting)), (Action<string>) (value =>
      {
        Action<string> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) value);
        itemData.setting.ApplyAndSave();
      }));
      stringInputField.disabled = itemData.disableAction;
      stringInputField.hidden = itemData.hideAction;
      return (IWidget) stringInputField;
    }

    public static IWidget AddStringDropdownProperty(AutomaticSettings.SettingItemData itemData)
    {
      if (itemData.property.GetAttribute<SettingsUIDropdownAttribute>() == null)
        return (IWidget) null;
      Action<string> setterAction = itemData.setterAction as Action<string>;
      DropdownField<string> dropdownField = new DropdownField<string>();
      dropdownField.path = (PathSegment) itemData.path;
      dropdownField.displayName = itemData.displayName;
      dropdownField.description = itemData.description;
      dropdownField.displayNameAction = itemData.dispayNameAction;
      dropdownField.descriptionAction = itemData.descriptionAction;
      dropdownField.warningAction = itemData.warningAction;
      dropdownField.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => (string) itemData.property.GetValue((object) itemData.setting)), (Action<string>) (value =>
      {
        Action<string> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) value);
        itemData.setting.ApplyAndSave();
      }));
      dropdownField.itemsAccessor = (ITypedValueAccessor<DropdownItem<string>[]>) AutomaticSettings.GetDropdownItemAccessor<string>(itemData.property, itemData.setting);
      dropdownField.itemsVersion = itemData.valueVersionAction;
      dropdownField.disabled = itemData.disableAction;
      dropdownField.hidden = itemData.hideAction;
      return (IWidget) dropdownField;
    }

    public static IWidget AddStringFieldProperty(AutomaticSettings.SettingItemData itemData)
    {
      LocalizedValueField localizedValueField = new LocalizedValueField();
      localizedValueField.path = (PathSegment) itemData.path;
      localizedValueField.displayName = itemData.displayName;
      localizedValueField.description = itemData.description;
      localizedValueField.displayNameAction = itemData.dispayNameAction;
      localizedValueField.descriptionAction = itemData.descriptionAction;
      localizedValueField.warningAction = itemData.warningAction;
      localizedValueField.accessor = (ITypedValueAccessor<LocalizedString>) new DelegateAccessor<LocalizedString>((Func<LocalizedString>) (() => LocalizedString.Value((string) itemData.property.GetValue((object) itemData.setting))), (Action<LocalizedString>) (value => { }));
      localizedValueField.valueVersion = itemData.valueVersionAction;
      localizedValueField.disabled = itemData.disableAction;
      localizedValueField.hidden = itemData.hideAction;
      return (IWidget) localizedValueField;
    }

    public static IWidget AddLocalizedStringFieldProperty(AutomaticSettings.SettingItemData itemData)
    {
      LocalizedValueField localizedValueField = new LocalizedValueField();
      localizedValueField.path = (PathSegment) itemData.path;
      localizedValueField.displayName = itemData.displayName;
      localizedValueField.description = itemData.description;
      localizedValueField.displayNameAction = itemData.dispayNameAction;
      localizedValueField.descriptionAction = itemData.descriptionAction;
      localizedValueField.warningAction = itemData.warningAction;
      localizedValueField.accessor = (ITypedValueAccessor<LocalizedString>) new DelegateAccessor<LocalizedString>((Func<LocalizedString>) (() => (LocalizedString) itemData.property.GetValue((object) itemData.setting)), (Action<LocalizedString>) (value => { }));
      localizedValueField.valueVersion = itemData.valueVersionAction;
      localizedValueField.disabled = itemData.disableAction;
      localizedValueField.hidden = itemData.hideAction;
      return (IWidget) localizedValueField;
    }

    public static IWidget AddEnumDropdownProperty(AutomaticSettings.SettingItemData itemData)
    {
      Action<int> setterAction = itemData.setterAction as Action<int>;
      DropdownField<int> dropdownField = new DropdownField<int>();
      dropdownField.path = (PathSegment) itemData.path;
      dropdownField.displayName = itemData.displayName;
      dropdownField.description = itemData.description;
      dropdownField.displayNameAction = itemData.dispayNameAction;
      dropdownField.descriptionAction = itemData.descriptionAction;
      dropdownField.warningAction = itemData.warningAction;
      dropdownField.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => (int) itemData.property.GetValue((object) itemData.setting)), (Action<int>) (value =>
      {
        Action<int> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) value);
        itemData.setting.ApplyAndSave();
      }));
      dropdownField.itemsAccessor = (ITypedValueAccessor<DropdownItem<int>[]>) AutomaticSettings.GetDropdownItemAccessor<int>(itemData.property, itemData.setting);
      dropdownField.itemsVersion = itemData.valueVersionAction;
      dropdownField.disabled = itemData.disableAction;
      dropdownField.hidden = itemData.hideAction;
      return (IWidget) dropdownField;
    }

    public static IWidget AddEnumSimpleProperty(AutomaticSettings.SettingItemData itemData)
    {
      Action<int> setterAction = itemData.setterAction as Action<int>;
      EnumField enumField = new EnumField();
      enumField.path = (PathSegment) itemData.path;
      enumField.displayName = itemData.displayName;
      enumField.description = itemData.description;
      enumField.displayNameAction = itemData.dispayNameAction;
      enumField.descriptionAction = itemData.descriptionAction;
      enumField.warningAction = itemData.warningAction;
      enumField.accessor = (ITypedValueAccessor<ulong>) new DelegateAccessor<ulong>((Func<ulong>) (() => (ulong) (int) itemData.property.GetValue((object) itemData.setting)), (Action<ulong>) (value =>
      {
        Action<int> action = setterAction;
        if (action != null)
          action((int) value);
        itemData.property.SetValue((object) itemData.setting, (object) (int) value);
        itemData.setting.ApplyAndSave();
      }));
      enumField.itemsAccessor = (ITypedValueAccessor<EnumMember[]>) AutomaticSettings.GetEnumMemberAccessor(itemData.property, itemData.setting, itemData.prefix);
      enumField.itemsVersion = itemData.valueVersionAction;
      enumField.disabled = itemData.disableAction;
      enumField.hidden = itemData.hideAction;
      return (IWidget) enumField;
    }

    public static IWidget AddKeyBindingProperty(AutomaticSettings.SettingItemData itemData)
    {
      Action<ProxyBinding> setterAction = itemData.setterAction as Action<ProxyBinding>;
      InputBindingField inputBindingField = new InputBindingField();
      inputBindingField.path = (PathSegment) itemData.path;
      inputBindingField.displayName = itemData.displayName;
      inputBindingField.description = itemData.description;
      inputBindingField.displayNameAction = itemData.dispayNameAction;
      inputBindingField.descriptionAction = itemData.descriptionAction;
      inputBindingField.accessor = (ITypedValueAccessor<ProxyBinding>) new DelegateAccessor<ProxyBinding>((Func<ProxyBinding>) (() =>
      {
        ProxyBinding binding = (ProxyBinding) itemData.property.GetValue((object) itemData.setting);
        return InputManager.instance.GetOrCreateBindingWatcher(binding).binding with
        {
          alies = binding.alies
        };
      }), (Action<ProxyBinding>) (value =>
      {
        ProxyBinding result;
        if (!InputManager.instance.SetBinding(value, out result))
          return;
        Action<ProxyBinding> action = setterAction;
        if (action != null)
          action(result);
        itemData.property.SetValue((object) itemData.setting, (object) result);
        itemData.setting.ApplyAndSave();
      }));
      inputBindingField.valueVersion = itemData.valueVersionAction ?? new Func<int>(GetValueVersion);
      inputBindingField.disabled = itemData.disableAction;
      inputBindingField.hidden = itemData.hideAction;
      return (IWidget) inputBindingField;

      static int GetValueVersion() => InputManager.instance.actionVersion;
    }

    public static IWidget AddDirectoryPickerBindingProperty(
      AutomaticSettings.SettingItemData itemData)
    {
      Action<string> setterAction = itemData.setterAction as Action<string>;
      DirectoryPickerField directoryPickerField = new DirectoryPickerField();
      directoryPickerField.path = (PathSegment) itemData.path;
      directoryPickerField.displayName = itemData.displayName;
      directoryPickerField.displayNameAction = itemData.dispayNameAction;
      directoryPickerField.warningAction = itemData.warningAction;
      directoryPickerField.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => (string) itemData.property.GetValue((object) itemData.setting)), (Action<string>) (value =>
      {
        Action<string> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) value);
        itemData.setting.ApplyAndSave();
      }));
      directoryPickerField.disabled = itemData.disableAction;
      directoryPickerField.hidden = itemData.hideAction;
      directoryPickerField.action = (Action) (() =>
      {
        string root = (string) itemData.property.GetValue((object) itemData.setting);
        // ISSUE: reference to a compiler-generated method
        World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OptionsUISystem>().OpenDirectoryBrowser(root, (Action<string>) (value =>
        {
          Action<string> action = setterAction;
          if (action != null)
            action(value);
          itemData.property.SetValue((object) itemData.setting, (object) value);
          itemData.setting.ApplyAndSave();
        }));
      });
      return (IWidget) directoryPickerField;
    }

    public static IWidget AddCustomDropdownProperty(AutomaticSettings.SettingItemData itemData)
    {
      if (itemData.property.GetAttribute<SettingsUIDropdownAttribute>() == null)
        return (IWidget) null;
      if (!typeof (IJsonWritable).IsAssignableFrom(itemData.property.propertyType))
        return (IWidget) null;
      if (!typeof (IJsonReadable).IsAssignableFrom(itemData.property.propertyType))
        return (IWidget) null;
      if (!itemData.property.propertyType.IsValueType && itemData.property.propertyType.GetConstructor(System.Type.EmptyTypes) == (ConstructorInfo) null)
        return (IWidget) null;
      return (IWidget) AutomaticSettings.s_CustomDropdownMethodInfo.MakeGenericMethod(itemData.property.propertyType).Invoke((object) null, new object[1]
      {
        (object) itemData
      });
    }

    public static IWidget AddCustomDropdownPropertyGeneric<T>(
      AutomaticSettings.SettingItemData itemData)
      where T : IJsonWritable, IJsonReadable, new()
    {
      Action<T> setterAction = itemData.setterAction as Action<T>;
      DropdownField<T> dropdownField = new DropdownField<T>();
      dropdownField.path = (PathSegment) itemData.path;
      dropdownField.displayName = itemData.displayName;
      dropdownField.description = itemData.description;
      dropdownField.displayNameAction = itemData.dispayNameAction;
      dropdownField.descriptionAction = itemData.descriptionAction;
      dropdownField.warningAction = itemData.warningAction;
      dropdownField.accessor = (ITypedValueAccessor<T>) new DelegateAccessor<T>((Func<T>) (() => (T) itemData.property.GetValue((object) itemData.setting)), (Action<T>) (value =>
      {
        Action<T> action = setterAction;
        if (action != null)
          action(value);
        itemData.property.SetValue((object) itemData.setting, (object) value);
        itemData.setting.ApplyAndSave();
      }));
      dropdownField.valueWriter = (IWriter<T>) new ValueWriter<T>();
      dropdownField.valueReader = (IReader<T>) new ValueReader<T>();
      dropdownField.itemsVersion = itemData.valueVersionAction;
      dropdownField.itemsAccessor = (ITypedValueAccessor<DropdownItem<T>[]>) AutomaticSettings.GetDropdownItemAccessor<T>(itemData.property, itemData.setting);
      dropdownField.disabled = itemData.disableAction;
      dropdownField.hidden = itemData.hideAction;
      return (IWidget) dropdownField;
    }

    private struct SectionInfo
    {
      public string m_Tab;
      public string m_SimpleGroup;
      public string m_AdvancedGroup;
    }

    public enum WidgetType
    {
      None,
      BoolButton,
      BoolButtonWithConfirmation,
      BoolToggle,
      IntDropdown,
      IntSlider,
      FloatSlider,
      StringDropdown,
      StringField,
      StringTextInput,
      LocalizedStringField,
      AdvancedEnumDropdown,
      EnumDropdown,
      KeyBinding,
      DirectoryPicker,
      MultilineText,
      CustomDropdown,
    }

    public class SettingPageData
    {
      private Dictionary<string, int> m_TabOrder = new Dictionary<string, int>();
      private Dictionary<string, int> m_GroupOrder = new Dictionary<string, int>();
      private List<AutomaticSettings.SettingTabData> m_Tabs = new List<AutomaticSettings.SettingTabData>();

      public string id { get; }

      public bool addPrefix { get; }

      public string prefix => !this.addPrefix ? string.Empty : this.id;

      public bool showAllGroupNames { get; set; }

      private HashSet<string> m_GroupToShowName { get; } = new HashSet<string>();

      public List<AutomaticSettings.SettingTabData> tabs => this.m_Tabs;

      public IEnumerable<string> groupNames
      {
        get
        {
          return this.m_GroupOrder.OrderBy<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>) (g => g.Value)).Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (g => g.Key));
        }
      }

      public IEnumerable<string> groupToShowName
      {
        get
        {
          return !this.showAllGroupNames ? (IEnumerable<string>) this.m_GroupToShowName : this.groupNames;
        }
      }

      public Func<bool> warningGetter { get; set; }

      public Dictionary<string, Func<bool>> tabWarningGetters { get; set; }

      public AutomaticSettings.SettingTabData this[string name]
      {
        get
        {
          int index = this.m_Tabs.FindIndex((Predicate<AutomaticSettings.SettingTabData>) (s => s.id == name));
          if (index != -1)
            return this.m_Tabs[index];
          AutomaticSettings.SettingTabData settingTabData = new AutomaticSettings.SettingTabData(name, this);
          this.m_Tabs.Add(settingTabData);
          return settingTabData;
        }
      }

      public SettingPageData(string id, bool addPrefix)
      {
        this.id = id;
        this.addPrefix = addPrefix;
      }

      public void SortTabs()
      {
        this.m_Tabs.Sort((Comparison<AutomaticSettings.SettingTabData>) ((a, b) =>
        {
          int maxValue1;
          if (!this.m_TabOrder.TryGetValue(a.id, out maxValue1))
            maxValue1 = int.MaxValue;
          int maxValue2;
          if (!this.m_TabOrder.TryGetValue(b.id, out maxValue2))
            maxValue2 = int.MaxValue;
          return maxValue1.CompareTo(maxValue2);
        }));
      }

      public OptionsUISystem.Page BuildPage()
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OptionsUISystem.Page page = new OptionsUISystem.Page()
        {
          id = this.id,
          beta = BetaFilter.options.Contains<string>(this.id),
          warningGetter = this.warningGetter
        };
        this.SortTabs();
        foreach (AutomaticSettings.SettingTabData tab in this.m_Tabs)
          page.sections.Add(tab.BuildTab(page));
        return page;
      }

      public void AddTab(string tab) => this.m_TabOrder.TryAdd(tab, this.m_TabOrder.Count);

      public void AddGroup(string group)
      {
        this.m_GroupOrder.TryAdd(group, this.m_GroupOrder.Count);
      }

      public bool TryGetTabOrder(string tabName, out int index)
      {
        return this.m_TabOrder.TryGetValue(tabName, out index);
      }

      public bool TryGetGroupOrder(string groupName, out int index)
      {
        return this.m_GroupOrder.TryGetValue(groupName, out index);
      }

      public void AddGroupToShowName(string group) => this.m_GroupToShowName.Add(group);
    }

    public class SettingTabData
    {
      private readonly List<AutomaticSettings.SettingItemData> m_Items = new List<AutomaticSettings.SettingItemData>();

      public string id { get; }

      public AutomaticSettings.SettingPageData pageData { get; }

      public IEnumerable<AutomaticSettings.SettingItemData> items
      {
        get => (IEnumerable<AutomaticSettings.SettingItemData>) this.m_Items;
      }

      public SettingTabData(string id, AutomaticSettings.SettingPageData pageData)
      {
        this.id = id;
        this.pageData = pageData;
      }

      public void AddItem(AutomaticSettings.SettingItemData item) => this.m_Items.Add(item);

      public void InsertItem(AutomaticSettings.SettingItemData item, int index)
      {
        if (index > this.m_Items.Count)
          this.AddItem(item);
        else
          this.m_Items.Insert(index, item);
      }

      public OptionsUISystem.Section BuildTab(OptionsUISystem.Page page)
      {
        Func<bool> func;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OptionsUISystem.Section section = new OptionsUISystem.Section(this.pageData.addPrefix ? this.pageData.id + "." + this.id : this.id, page, this.pageData)
        {
          warningGetter = this.pageData.tabWarningGetters.TryGetValue(this.id, out func) ? func : (Func<bool>) null
        };
        foreach (AutomaticSettings.SettingItemData settingItemData in this.m_Items)
        {
          IWidget widget = settingItemData.widget;
          if (widget != null)
          {
            int num = (int) widget.Update();
            int index1;
            int index2;
            // ISSUE: object of a compiler-generated type is created
            section.options.Add(new OptionsUISystem.Option()
            {
              widget = widget,
              isAdvanced = settingItemData.isAdvanced,
              simpleGroupIndex = this.pageData.TryGetGroupOrder(settingItemData.simpleGroup ?? string.Empty, out index1) ? index1 : int.MaxValue,
              advancedGroupIndex = this.pageData.TryGetGroupOrder(settingItemData.advancedGroup ?? settingItemData.simpleGroup ?? string.Empty, out index2) ? index2 : int.MaxValue,
              searchHidden = settingItemData.isSearchHidden
            });
          }
        }
        return section;
      }
    }

    public class SettingItemData
    {
      private IWidget m_Widget;

      public AutomaticSettings.WidgetType widgetType { get; }

      public Setting setting { get; }

      public AutomaticSettings.IProxyProperty property { get; }

      public string prefix { get; }

      public string path { get; }

      public LocalizedString displayName { get; set; }

      public LocalizedString description { get; set; }

      public bool isAdvanced { get; set; }

      public string simpleGroup { get; set; }

      public string advancedGroup { get; set; }

      public bool isSearchHidden { get; set; }

      public Delegate setterAction { get; set; }

      public Func<bool> disableAction { get; set; }

      public Func<bool> hideAction { get; set; }

      public Func<int> valueVersionAction { get; set; }

      public Func<LocalizedString> dispayNameAction { get; set; }

      public Func<LocalizedString> descriptionAction { get; set; }

      public Func<bool> warningAction { get; set; }

      public IWidget widget => this.m_Widget ?? (this.m_Widget = this.GetWidget());

      protected virtual IWidget GetWidget()
      {
        IWidget widget;
        switch (this.widgetType)
        {
          case AutomaticSettings.WidgetType.BoolButton:
            widget = AutomaticSettings.AddBoolButtonProperty(this);
            break;
          case AutomaticSettings.WidgetType.BoolButtonWithConfirmation:
            widget = AutomaticSettings.AddBoolButtonWithConfirmationProperty(this);
            break;
          case AutomaticSettings.WidgetType.BoolToggle:
            widget = AutomaticSettings.AddBoolToggleProperty(this);
            break;
          case AutomaticSettings.WidgetType.IntDropdown:
            widget = AutomaticSettings.AddIntDropdownProperty(this);
            break;
          case AutomaticSettings.WidgetType.IntSlider:
            widget = AutomaticSettings.AddIntSliderProperty(this);
            break;
          case AutomaticSettings.WidgetType.FloatSlider:
            widget = AutomaticSettings.AddFloatSliderProperty(this);
            break;
          case AutomaticSettings.WidgetType.StringDropdown:
            widget = AutomaticSettings.AddStringDropdownProperty(this);
            break;
          case AutomaticSettings.WidgetType.StringField:
            widget = AutomaticSettings.AddStringFieldProperty(this);
            break;
          case AutomaticSettings.WidgetType.StringTextInput:
            widget = AutomaticSettings.AddStringTextInputProperty(this);
            break;
          case AutomaticSettings.WidgetType.LocalizedStringField:
            widget = AutomaticSettings.AddLocalizedStringFieldProperty(this);
            break;
          case AutomaticSettings.WidgetType.AdvancedEnumDropdown:
            widget = AutomaticSettings.AddEnumDropdownProperty(this);
            break;
          case AutomaticSettings.WidgetType.EnumDropdown:
            widget = AutomaticSettings.AddEnumSimpleProperty(this);
            break;
          case AutomaticSettings.WidgetType.KeyBinding:
            widget = AutomaticSettings.AddKeyBindingProperty(this);
            break;
          case AutomaticSettings.WidgetType.DirectoryPicker:
            widget = AutomaticSettings.AddDirectoryPickerBindingProperty(this);
            break;
          case AutomaticSettings.WidgetType.CustomDropdown:
            widget = AutomaticSettings.AddCustomDropdownProperty(this);
            break;
          default:
            widget = (IWidget) null;
            break;
        }
        return widget;
      }

      public SettingItemData(
        AutomaticSettings.WidgetType widgetType,
        Setting setting,
        AutomaticSettings.IProxyProperty property,
        string prefix)
      {
        this.widgetType = widgetType;
        this.setting = setting;
        this.property = property;
        this.prefix = prefix;
        this.path = this.GetPath(property, prefix);
        this.displayName = this.GetDisplayName(property, this.path);
        this.description = this.GetDescription(property, this.path);
        this.dispayNameAction = this.GetDisplayNameAction(property, setting);
        this.descriptionAction = this.GetDescriptionAction(property, setting);
        this.setterAction = this.GetSetterAction(property, setting);
        this.disableAction = this.GetDisableAction(property, setting);
        this.hideAction = this.GetHideAction(property, setting);
        this.valueVersionAction = this.GetValueVersionAction(property, setting);
        this.isAdvanced = this.IsAdvanced(property);
        this.isSearchHidden = this.IsSearchHidden(property);
        this.warningAction = this.GetWarningAction(property, setting);
      }

      private string GetPath(AutomaticSettings.IProxyProperty property, string prefix)
      {
        string path = property.GetAttribute<SettingsUIPathAttribute>()?.path;
        if (string.IsNullOrEmpty(path))
        {
          path = property.declaringType.Name + "." + property.name;
          if (!string.IsNullOrEmpty(prefix))
            path = prefix + "." + path;
        }
        return path;
      }

      private LocalizedString GetDisplayName(AutomaticSettings.IProxyProperty property, string path)
      {
        SettingsUIDisplayNameAttribute attribute = property.GetAttribute<SettingsUIDisplayNameAttribute>();
        if (attribute != null)
        {
          if (!string.IsNullOrEmpty(attribute.id))
            return LocalizedString.IdWithFallback("Options.OPTION[" + attribute.id + "]", attribute.value);
          if (!string.IsNullOrEmpty(attribute.value))
            return LocalizedString.Value(attribute.value);
        }
        return LocalizedString.Id("Options.OPTION[" + path + "]");
      }

      private LocalizedString GetDescription(AutomaticSettings.IProxyProperty property, string path)
      {
        SettingsUIDescriptionAttribute attribute = property.GetAttribute<SettingsUIDescriptionAttribute>();
        if (attribute != null)
        {
          if (!string.IsNullOrEmpty(attribute.id))
            return LocalizedString.IdWithFallback("Options.OPTION_DESCRIPTION[" + attribute.id + "]", attribute.value);
          if (!string.IsNullOrEmpty(attribute.value))
            return LocalizedString.Value(attribute.value);
        }
        return LocalizedString.Id("Options.OPTION_DESCRIPTION[" + path + "]");
      }

      private Func<LocalizedString> GetDisplayNameAction(
        AutomaticSettings.IProxyProperty property,
        Setting setting)
      {
        SettingsUIDisplayNameAttribute attribute = property.GetAttribute<SettingsUIDisplayNameAttribute>();
        Func<LocalizedString> action;
        return attribute != null && AutomaticSettings.TryGetAction<LocalizedString>(setting, attribute.getterType, attribute.getterMethod, out action) ? action : (Func<LocalizedString>) null;
      }

      private Func<LocalizedString> GetDescriptionAction(
        AutomaticSettings.IProxyProperty property,
        Setting setting)
      {
        SettingsUIDescriptionAttribute attribute = property.GetAttribute<SettingsUIDescriptionAttribute>();
        Func<LocalizedString> action;
        return attribute != null && AutomaticSettings.TryGetAction<LocalizedString>(setting, attribute.getterType, attribute.getterMethod, out action) ? action : (Func<LocalizedString>) null;
      }

      private Func<bool> GetWarningAction(
        AutomaticSettings.IProxyProperty property,
        Setting setting)
      {
        SettingsUIWarningAttribute attribute = property.GetAttribute<SettingsUIWarningAttribute>();
        Func<bool> action;
        return attribute != null && AutomaticSettings.TryGetAction<bool>(setting, attribute.checkType, attribute.checkMethod, out action) ? action : (Func<bool>) null;
      }

      private bool IsAdvanced(AutomaticSettings.IProxyProperty property)
      {
        return property.GetAttribute<SettingsUIAdvancedAttribute>() != null || ReflectionUtils.GetAttribute<SettingsUIAdvancedAttribute>(property.declaringType.GetCustomAttributes(false)) != null;
      }

      private bool IsSearchHidden(AutomaticSettings.IProxyProperty property)
      {
        return property.GetAttribute<SettingsUISearchHiddenAttribute>() != null || ReflectionUtils.GetAttribute<SettingsUISearchHiddenAttribute>(property.declaringType.GetCustomAttributes(false)) != null;
      }

      private Delegate GetSetterAction(AutomaticSettings.IProxyProperty property, Setting setting)
      {
        SettingsUISetterAttribute attribute = property.GetAttribute<SettingsUISetterAttribute>();
        if (attribute == null || attribute.setterType == (System.Type) null || string.IsNullOrEmpty(attribute.setterMethod))
          return (Delegate) null;
        BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        if (attribute.setterType.IsInstanceOfType((object) setting))
          bindingAttr |= BindingFlags.Instance;
        foreach (MethodInfo method in attribute.setterType.GetMethods(bindingAttr))
        {
          if (!(method.Name != attribute.setterMethod) && !(method.ReturnType != typeof (void)))
          {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length == 1 && !(parameters[0].ParameterType != property.propertyType))
            {
              System.Type delegateType = typeof (Action<>).MakeGenericType(property.propertyType);
              return method.CreateDelegate(delegateType, method.IsStatic ? (object) (Setting) null : (object) setting);
            }
          }
        }
        return (Delegate) null;
      }

      private Func<bool> GetDisableAction(
        AutomaticSettings.IProxyProperty property,
        Setting setting)
      {
        SettingsUIDisableByConditionAttribute attribute1 = property.GetAttribute<SettingsUIDisableByConditionAttribute>();
        Func<bool> action;
        if (attribute1 != null && AutomaticSettings.TryGetAction<bool>(setting, attribute1.checkType, attribute1.checkMethod, out action))
          return !attribute1.invert ? action : (Func<bool>) (() => !action());
        SettingsUIDisableByConditionAttribute attribute2 = ReflectionUtils.GetAttribute<SettingsUIDisableByConditionAttribute>(property.declaringType.GetCustomAttributes(false));
        if (attribute2 == null || !AutomaticSettings.TryGetAction<bool>(setting, attribute2.checkType, attribute2.checkMethod, out action))
          return (Func<bool>) null;
        return !attribute2.invert ? action : (Func<bool>) (() => !action());
      }

      private Func<bool> GetHideAction(AutomaticSettings.IProxyProperty property, Setting setting)
      {
        SettingsUIHideByConditionAttribute attribute1 = property.GetAttribute<SettingsUIHideByConditionAttribute>();
        Func<bool> action;
        if (attribute1 != null && AutomaticSettings.TryGetAction<bool>(setting, attribute1.checkType, attribute1.checkMethod, out action))
          return !attribute1.invert ? action : (Func<bool>) (() => !action());
        SettingsUIHideByConditionAttribute attribute2 = ReflectionUtils.GetAttribute<SettingsUIHideByConditionAttribute>(property.declaringType.GetCustomAttributes(false));
        if (attribute2 == null || !AutomaticSettings.TryGetAction<bool>(setting, attribute2.checkType, attribute2.checkMethod, out action))
          return (Func<bool>) null;
        return !attribute2.invert ? action : (Func<bool>) (() => !action());
      }

      private Func<int> GetValueVersionAction(
        AutomaticSettings.IProxyProperty property,
        Setting setting)
      {
        SettingsUIValueVersionAttribute attribute = property.GetAttribute<SettingsUIValueVersionAttribute>();
        Func<int> action;
        return attribute != null && AutomaticSettings.TryGetAction<int>(setting, attribute.versionGetterType, attribute.versionGetterMethod, out action) ? action : (Func<int>) null;
      }
    }

    public interface IProxyProperty
    {
      bool canRead { get; }

      bool canWrite { get; }

      string name { get; }

      System.Type propertyType { get; }

      System.Type declaringType { get; }

      void SetValue(object obj, object value);

      object GetValue(object obj);

      bool HasAttribute<T>(bool inherit = false) where T : Attribute;

      T GetAttribute<T>(bool inherit = false) where T : Attribute;

      IEnumerable<T> GetAttributes<T>(bool inherit = false) where T : Attribute;

      bool TryGetAttribute<T>(out T attribute, bool inherit = false) where T : Attribute;
    }

    public class ProxyProperty : AutomaticSettings.IProxyProperty
    {
      public PropertyInfo property { get; }

      public ProxyProperty(PropertyInfo property) => this.property = property;

      public bool canRead => this.property.CanRead;

      public bool canWrite => this.property.CanWrite;

      public string name => this.property.Name;

      public System.Type propertyType => this.property.PropertyType;

      public System.Type declaringType => this.property.DeclaringType;

      public void SetValue(object obj, object value) => this.property.SetValue(obj, value);

      public object GetValue(object obj) => this.property.GetValue(obj);

      public bool HasAttribute<T>(bool inherit = false) where T : Attribute
      {
        return this.property.HasAttribute<T>(inherit);
      }

      public T GetAttribute<T>(bool inherit = false) where T : Attribute
      {
        return this.property.GetAttribute<T>(inherit);
      }

      public IEnumerable<T> GetAttributes<T>(bool inherit = false) where T : Attribute
      {
        return this.property.GetAttributes<T>(inherit);
      }

      public bool TryGetAttribute<T>(out T attribute, bool inherit = false) where T : Attribute
      {
        return this.property.TryGetAttribute<T>(out attribute, inherit);
      }
    }

    public class ManualProperty : AutomaticSettings.IProxyProperty
    {
      public bool canRead { get; set; }

      public bool canWrite { get; set; }

      public string name { get; set; }

      public System.Type propertyType { get; set; }

      public System.Type declaringType { get; set; }

      public Action<object, object> setter { get; set; }

      public Func<object, object> getter { get; set; }

      public List<Attribute> attributes { get; } = new List<Attribute>();

      public ManualProperty(System.Type declaringType, System.Type propertyType, string name)
      {
        this.declaringType = declaringType;
        this.propertyType = propertyType;
        this.name = name;
      }

      public void SetValue(object obj, object value)
      {
        Action<object, object> setter = this.setter;
        if (setter == null)
          return;
        setter(obj, value);
      }

      public object GetValue(object obj)
      {
        Func<object, object> getter = this.getter;
        return getter == null ? (object) null : getter(obj);
      }

      public bool HasAttribute<T>(bool inherit = false) where T : Attribute
      {
        return this.attributes.AnyOfType<Attribute>(typeof (T));
      }

      public T GetAttribute<T>(bool inherit = false) where T : Attribute
      {
        return this.attributes.OfType<T>().FirstOrDefault<T>();
      }

      public IEnumerable<T> GetAttributes<T>(bool inherit = false) where T : Attribute
      {
        return this.attributes.OfType<T>();
      }

      public bool TryGetAttribute<T>(out T attribute, bool inherit = false) where T : Attribute
      {
        attribute = this.GetAttribute<T>(false);
        return (object) attribute != null;
      }
    }

    public class DropdownItemsAccessor<T> : ITypedValueAccessor<T>, IValueAccessor
    {
      public System.Type valueType => typeof (T);

      public Func<T> del { get; }

      public DropdownItemsAccessor(System.Type valueType, MethodInfo getterMethod, Setting setting)
      {
        System.Type type = typeof (DropdownItem<>).MakeGenericType(valueType).MakeArrayType();
        if (getterMethod.ReturnType != type)
          throw new ArgumentException(string.Format("method's return type must be {0}", (object) type), nameof (getterMethod));
        this.del = getterMethod.GetParameters().Length == 0 ? (Func<T>) getterMethod.CreateDelegate(typeof (Func<T>), (object) setting) : throw new ArgumentException("method must take 0 arg", nameof (getterMethod));
      }

      public DropdownItemsAccessor(Func<T> del) => this.del = del;

      public object GetValue() => this.del != null ? (object) this.del() : (object) null;

      public T GetTypedValue() => (T) this.GetValue();

      public void SetValue(object value)
      {
        throw new InvalidOperationException("DropdownItemsAccessor is readonly");
      }

      public void SetTypedValue(T value)
      {
        throw new InvalidOperationException("DropdownItemsAccessor is readonly");
      }
    }
  }
}
