// Decompiled with JetBrains decompiler
// Type: Game.Settings.About
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using Colossal.PSI.Common;
using Game.UI.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
namespace Game.Settings
{
  public class About : Setting
  {
    public const string kName = "About";
    private const string kGameGroup = "Game";
    private const string kContentGroup = "Content";

    [SettingsUISection("kGameGroup")]
    public string gameVersion => Game.Version.current.fullVersion;

    [SettingsUISection("kGameGroup")]
    public string gameConfiguration => !Debug.isDebugBuild ? "Release" : "Development";

    public string coreVersion => Colossal.Core.Version.current.fullVersion;

    public string uiVersion => Colossal.UI.Version.current.fullVersion;

    public string unityVersion => Application.unityVersion;

    public string cohtmlVersion => Versioning.Build.ToString();

    public string atlVersion => ATL.Version.getVersion();

    public override void SetDefaults()
    {
    }

    public override AutomaticSettings.SettingPageData GetPageData(string id, bool addPrefix)
    {
      AutomaticSettings.SettingPageData pageData = base.GetPageData(id, addPrefix);
      foreach (IPlatformServiceIntegration serviceIntegration in (IEnumerable<IPlatformServiceIntegration>) PlatformManager.instance.platformServiceIntegrations)
      {
        StringBuilder b = new StringBuilder();
        serviceIntegration.LogVersion(b);
        string[] strArray = b.ToString().Split(Environment.NewLine, StringSplitOptions.None);
        int num = 0;
        foreach (string str in strArray)
        {
          string line = str;
          int sep = line.IndexOf(":", StringComparison.Ordinal);
          if (sep != -1)
          {
            AutomaticSettings.SettingItemData settingItemData = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.StringField, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (About), typeof (string), serviceIntegration.name)
            {
              canRead = true,
              canWrite = false,
              attributes = {
                (Attribute) new SettingsUIPathAttribute(string.Format("{0}{1}.{2}", (object) serviceIntegration.GetType().Name, (object) num++, (object) serviceIntegration.name)),
                (Attribute) new SettingsUIDisplayNameAttribute(overrideValue: line.Substring(0, sep))
              },
              getter = (Func<object, object>) (obj => (object) line.Substring(sep + 1))
            }, pageData.prefix);
            pageData["General"].AddItem(settingItemData);
          }
        }
      }
      pageData.AddGroup("Content");
      foreach (IDlc enumerateLocalDlC in PlatformManager.instance.EnumerateLocalDLCs())
      {
        IDlc dlc = enumerateLocalDlC;
        AutomaticSettings.SettingItemData settingItemData = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.StringField, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (About), typeof (string), dlc.internalName)
        {
          canRead = true,
          canWrite = false,
          attributes = {
            (Attribute) new SettingsUIPathAttribute(dlc.internalName),
            (Attribute) new SettingsUIDisplayNameAttribute(overrideValue: dlc.internalName),
            (Attribute) new SettingsUIDescriptionAttribute(overrideValue: dlc.version.fullVersion)
          },
          getter = (Func<object, object>) (obj => (object) dlc.version.fullVersion)
        }, pageData.prefix)
        {
          simpleGroup = "Content"
        };
        pageData["General"].AddItem(settingItemData);
      }
      return pageData;
    }
  }
}
