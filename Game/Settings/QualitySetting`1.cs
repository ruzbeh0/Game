// Decompiled with JetBrains decompiler
// Type: Game.Settings.QualitySetting`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Reflection;
using Game.Rendering;
using Game.UI.Menu;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Settings
{
  public abstract class QualitySetting<T> : QualitySetting where T : QualitySetting
  {
    private static readonly Dictionary<QualitySetting.Level, string> s_MockNames = new Dictionary<QualitySetting.Level, string>();
    protected static readonly Dictionary<QualitySetting.Level, T> s_SettingsMap = new Dictionary<QualitySetting.Level, T>();

    static QualitySetting()
    {
      QualitySetting<T>.s_SettingsMap = new Dictionary<QualitySetting.Level, T>();
    }

    internal override void AddToPageData(AutomaticSettings.SettingPageData pageData)
    {
      AutomaticSettings.SettingItemData settingItemData = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.AdvancedEnumDropdown, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(this.GetType(), typeof (QualitySetting.Level), "Level")
      {
        canRead = true,
        canWrite = true,
        getter = (Func<object, object>) (settings => (object) ((QualitySetting) settings).GetLevel()),
        setter = (Action<object, object>) ((settings, value) => ((QualitySetting) settings).SetLevel((QualitySetting.Level) value)),
        attributes = {
          (Attribute) new SettingsUIDropdownAttribute(typeof (QualitySetting), "GetQualityValues"),
          (Attribute) new SettingsUIPathAttribute(this.GetType().Name),
          (Attribute) new SettingsUIDisplayNameAttribute(this.GetType().Name),
          (Attribute) new SettingsUIDisableByConditionAttribute(typeof (QualitySetting), "IsOptionFullyDisabled")
        }
      }, pageData.prefix)
      {
        isAdvanced = false,
        simpleGroup = "Quality",
        advancedGroup = this.GetType().Name
      };
      pageData["General"].AddItem(settingItemData);
      base.AddToPageData(pageData);
    }

    public override string GetMockName(QualitySetting.Level level)
    {
      return QualitySetting<T>.MockName(level);
    }

    public static void RegisterMockName(QualitySetting.Level level, string name)
    {
      QualitySetting<T>.s_MockNames[level] = name;
    }

    public static string MockName(QualitySetting.Level level)
    {
      string str;
      return QualitySetting<T>.s_MockNames.TryGetValue(level, out str) ? str : level.ToString();
    }

    public static void RegisterSetting(QualitySetting.Level quality, T setting)
    {
      if (quality == QualitySetting.Level.Custom)
        Debug.LogWarning((object) "Can not register a default Custom quality setting. Ignoring.");
      else
        QualitySetting<T>.s_SettingsMap[quality] = setting;
    }

    public override QualitySetting.Level GetLevel()
    {
      foreach (KeyValuePair<QualitySetting.Level, T> settings in QualitySetting<T>.s_SettingsMap)
      {
        if (this.Equals((Setting) settings.Value))
          return settings.Key;
      }
      return QualitySetting.Level.Custom;
    }

    public override IEnumerable<QualitySetting.Level> EnumerateAvailableLevels()
    {
      foreach (QualitySetting.Level availableLevel in (IEnumerable<QualitySetting.Level>) QualitySetting<T>.GetAvailableLevels())
        yield return availableLevel;
    }

    public static IReadOnlyList<QualitySetting.Level> GetAvailableLevels()
    {
      List<QualitySetting.Level> availableLevels = new List<QualitySetting.Level>((IEnumerable<QualitySetting.Level>) QualitySetting<T>.s_SettingsMap.Keys);
      availableLevels.Add(QualitySetting.Level.Custom);
      availableLevels.Sort();
      return (IReadOnlyList<QualitySetting.Level>) availableLevels;
    }

    protected void ApplyState<PT>(VolumeParameter<PT> param, PT value, bool state = true)
    {
      param.overrideState = state;
      param.value = value;
    }

    protected void CreateVolumeComponent<PT>(VolumeProfile profile, ref PT component) where PT : VolumeComponent
    {
      VolumeHelper.GetOrCreateVolumeComponent<PT>(profile, ref component);
    }

    public override void SetLevel(QualitySetting.Level quality, bool apply = true)
    {
      T setting;
      if (QualitySetting<T>.s_SettingsMap.TryGetValue(quality, out setting))
      {
        this.TransferValuesFrom((QualitySetting) setting);
        if (!apply)
          return;
        this.ApplyAndSave();
      }
      else
        Setting.log.WarnFormat("Quality setting {0} doesn't exist for {1}", (object) quality, (object) this.GetType().Name);
    }

    public override void TransferValuesFrom(QualitySetting setting)
    {
      System.Type type = this.GetType();
      bool flag = true;
      PropertyInfo property1 = type.GetProperty("enabled", BindingFlags.Instance | BindingFlags.Public);
      if (property1 != (PropertyInfo) null)
      {
        flag = (bool) property1.GetValue((object) setting);
        property1.SetValue((object) this, (object) flag);
      }
      if (!flag && setting.GetLevel() == QualitySetting.Level.Disabled)
        return;
      foreach (PropertyInfo property2 in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (ReflectionUtils.GetAttribute<IgnoreEqualsAttribute>(property2.GetCustomAttributes(false)) == null)
          property2.SetValue((object) this, property2.GetValue((object) setting));
      }
    }
  }
}
