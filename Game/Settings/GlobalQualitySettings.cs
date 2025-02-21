// Decompiled with JetBrains decompiler
// Type: Game.Settings.GlobalQualitySettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.Settings
{
  public class GlobalQualitySettings : QualitySetting<GlobalQualitySettings>
  {
    protected static readonly Dictionary<Type, QualitySetting.Level> s_DefaultsMap = new Dictionary<Type, QualitySetting.Level>();
    private List<QualitySetting> m_QualitySettings = new List<QualitySetting>();

    [MergeByType]
    [IgnoreEquals]
    public List<QualitySetting> qualitySettings
    {
      get => this.m_QualitySettings;
      set
      {
        foreach (QualitySetting qualitySetting1 in this.m_QualitySettings)
        {
          QualitySetting qualitySetting = qualitySetting1;
          QualitySetting setting = value.Find((Predicate<QualitySetting>) (x => qualitySetting.GetType() == x.GetType()));
          if (setting != null)
            qualitySetting.TransferValuesFrom(setting);
        }
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj is GlobalQualitySettings globalQualitySettings))
        return false;
      bool flag = base.Equals(obj);
      for (int index = 0; index < this.m_QualitySettings.Count; ++index)
      {
        if (!this.m_QualitySettings[index].Equals((Setting) globalQualitySettings.m_QualitySettings[index]))
          return false;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int hashCode = base.GetHashCode();
      foreach (QualitySetting enumerateQualitySetting in this.EnumerateQualitySettings())
        hashCode = hashCode * 937 ^ enumerateQualitySetting.GetHashCode();
      return hashCode;
    }

    public T GetQualitySetting<T>() where T : QualitySetting
    {
      foreach (QualitySetting qualitySetting in this.m_QualitySettings)
      {
        if (qualitySetting.GetType() == typeof (T))
          return (T) qualitySetting;
      }
      return default (T);
    }

    public QualitySetting GetQualitySetting(Type type)
    {
      foreach (QualitySetting qualitySetting in this.m_QualitySettings)
      {
        if (qualitySetting.GetType() == type)
          return qualitySetting;
      }
      return (QualitySetting) null;
    }

    public override void SetDefaults()
    {
      foreach (QualitySetting enumerateQualitySetting in this.EnumerateQualitySettings())
        enumerateQualitySetting.SetLevel(GlobalQualitySettings.s_DefaultsMap[enumerateQualitySetting.GetType()], false);
    }

    public void AddQualitySetting<T>(T setting) where T : QualitySetting<T>
    {
      Type key1 = typeof (T);
      if ((object) this.GetQualitySetting<T>() != null)
        return;
      GlobalQualitySettings.s_DefaultsMap[key1] = setting.GetLevel();
      this.m_QualitySettings.Add((QualitySetting) setting);
      foreach (KeyValuePair<QualitySetting.Level, GlobalQualitySettings> settings in QualitySetting<GlobalQualitySettings>.s_SettingsMap)
      {
        if ((object) settings.Value.GetQualitySetting<T>() == null)
        {
          QualitySetting.Level key2 = settings.Key;
          T obj;
          if (!QualitySetting<T>.s_SettingsMap.TryGetValue(key2, out obj))
          {
            QualitySetting.Level key3 = key2 - 1;
            while (key3 >= QualitySetting.Level.Disabled && !QualitySetting<T>.s_SettingsMap.TryGetValue(key3, out obj))
              --key3;
            if ((object) obj == null)
            {
              QualitySetting.Level key4 = key2 + 1;
              while (key4 < QualitySetting.Level.Custom && !QualitySetting<T>.s_SettingsMap.TryGetValue(key4, out obj))
                ++key4;
            }
          }
          settings.Value.m_QualitySettings.Add((QualitySetting) obj);
        }
      }
    }

    public IEnumerable<QualitySetting> EnumerateQualitySettings()
    {
      foreach (QualitySetting qualitySetting in this.m_QualitySettings)
        yield return qualitySetting;
    }

    [Exclude]
    [IgnoreEquals]
    [SettingsUIHidden]
    public int countQualitySettings => this.m_QualitySettings.Count;

    public QualitySetting lastSetting
    {
      get
      {
        List<QualitySetting> qualitySettings = this.m_QualitySettings;
        return qualitySettings[qualitySettings.Count - 1];
      }
    }

    public override void SetLevel(QualitySetting.Level quality, bool apply = true)
    {
      if (quality >= QualitySetting.Level.Custom)
        return;
      for (int index = 0; index < this.m_QualitySettings.Count; ++index)
        this.m_QualitySettings[index].TransferValuesFrom(QualitySetting<GlobalQualitySettings>.s_SettingsMap[quality].m_QualitySettings[index]);
      if (!apply)
        return;
      this.ApplyAndSave();
    }
  }
}
