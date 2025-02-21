// Decompiled with JetBrains decompiler
// Type: Game.Settings.ModdingSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.PSI.Environment;
using Game.Modding.Toolchain;
using Game.Modding.Toolchain.Dependencies;
using Game.SceneFlow;
using Game.UI.Localization;
using Game.UI.Menu;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  [SettingsUIShowGroupName(new string[] {"Dependencies"})]
  [SettingsUIGroupOrder(new string[] {"Disclaimer", "Main", "Dependencies"})]
  [SettingsUIPageWarning(typeof (ModdingSettings), "showWarning")]
  public class ModdingSettings : Setting
  {
    public const string kName = "Modding";
    public const string kDisclaimer = "Disclaimer";
    public const string kMain = "Main";
    public const string kDependencies = "Dependencies";

    [SettingsUIHidden]
    public bool isInstalled { get; set; }

    [SettingsUISection("Main")]
    [SettingsUIButtonGroup("toolchainAction")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "canNotBeInstalled")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUISearchHidden]
    public bool installModdingToolchain
    {
      set
      {
        ToolchainDeployment.RunWithUI(DeploymentAction.Install, callback: (Action<bool>) (success =>
        {
          if (!success)
            return;
          this.isInstalled = true;
        }));
      }
    }

    [SettingsUISection("Main")]
    [SettingsUIButtonGroup("toolchainAction")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "canNotBeUninstalled")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUISearchHidden]
    public bool uninstallModdingToolchain
    {
      set
      {
        ToolchainDeployment.RunWithUI(DeploymentAction.Uninstall, callback: (Action<bool>) (success =>
        {
          if (!success)
            return;
          this.isInstalled = false;
        }));
      }
    }

    [SettingsUISection("Main")]
    [SettingsUIButtonGroup("toolchainAction")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "canNotBeRepaired")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUISearchHidden]
    public bool repairModdingToolchain
    {
      set
      {
        ToolchainDeployment.RunWithUI(DeploymentAction.Repair, callback: (Action<bool>) (success =>
        {
          if (!success)
            return;
          this.isInstalled = true;
        }));
      }
    }

    [SettingsUISection("Main")]
    [SettingsUIButtonGroup("toolchainAction")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "canNotBeUpdated")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUISearchHidden]
    public bool updateModdingToolchain
    {
      set
      {
        ToolchainDeployment.RunWithUI(DeploymentAction.Update, callback: (Action<bool>) (success =>
        {
          if (!success)
            return;
          this.isInstalled = true;
        }));
      }
    }

    [SettingsUIDeveloper]
    [SettingsUIButtonGroup("envSetVars")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "disableEnvVarUpdate")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUIDisplayName(null, "Show env vars")]
    [SettingsUIDescription(null, "Show environment variable currently set values")]
    public bool showEnvVars
    {
      set
      {
        List<string> values = new List<string>();
        foreach (string key in IToolchainDependency.envVars.Keys)
        {
          string environmentVariable = System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
          if (environmentVariable != null)
            values.Add("**" + key + "** = " + environmentVariable.Replace("\\", "\\\\"));
        }
        GameManager.instance.userInterface.appBindings.ShowMessageDialog(new Game.UI.MessageDialog(new LocalizedString?((LocalizedString) "envVars"), LocalizedString.Value("Values which set to variables"), new LocalizedString?(LocalizedString.Value(values.Count != 0 ? string.Join("\n", (IEnumerable<string>) values) : "Nothing set")), true, LocalizedString.Id("Common.OK"), Array.Empty<LocalizedString>()), (Action<int>) null);
      }
    }

    [SettingsUIDeveloper]
    [SettingsUIButtonGroup("envSetVars")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "disableEnvVarUpdate")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUIDisplayName(null, "Show actual values")]
    [SettingsUIDescription(null, "Show actual values which are supposed to be set.")]
    public bool showCurrentValues
    {
      set
      {
        List<string> list = IToolchainDependency.envVars.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (env => "**" + env.Key + "** = " + env.Value.Replace("\\", "\\\\"))).ToList<string>();
        GameManager.instance.userInterface.appBindings.ShowMessageDialog(new Game.UI.MessageDialog(new LocalizedString?((LocalizedString) "envVars"), LocalizedString.Value("Current values"), new LocalizedString?(LocalizedString.Value(string.Join("\n", (IEnumerable<string>) list))), true, LocalizedString.Id("Common.OK"), Array.Empty<LocalizedString>()), (Action<int>) null);
      }
    }

    [SettingsUIDeveloper]
    [SettingsUIButtonGroup("envVars")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "disableEnvVarUpdate")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUIDisplayName(null, "Update env vars")]
    [SettingsUIDescription(null, "Update environment variables with actual values.")]
    public bool updateEnvVars
    {
      set
      {
        ToolchainDependencyManager.UserEnvironmentVariableManager.SetEnvVars(IToolchainDependency.envVars.Keys.ToArray<string>());
        Task.Run<ToolchainDependencyManager.State>(new Func<Task<ToolchainDependencyManager.State>>(ToolchainDeployment.dependencyManager.GetCurrentState));
      }
    }

    [SettingsUIDeveloper]
    [SettingsUIButtonGroup("envVars")]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "disableEnvVarUpdate")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [SettingsUIDisplayName(null, "Remove env vars")]
    [SettingsUIDescription(null, "Remove environment variable values.")]
    public bool removeEnvVars
    {
      set
      {
        ToolchainDependencyManager.UserEnvironmentVariableManager.RemoveEnvVars();
        Task.Run<ToolchainDependencyManager.State>(new Func<Task<ToolchainDependencyManager.State>>(ToolchainDeployment.dependencyManager.GetCurrentState));
      }
    }

    private bool disableEnvVarUpdate => !this.isInstalled;

    [SettingsUISection("Main")]
    [SettingsUIDirectoryPicker]
    [SettingsUIHideByCondition(typeof (ModdingSettings), "noNeedDownloadPath")]
    [SettingsUIDisableByCondition(typeof (ModdingSettings), "isActionDisabled")]
    [Exclude]
    public string downloadDirectory { get; set; } = EnvPath.kTempDataPath;

    public bool isActionDisabled => ToolchainDeployment.dependencyManager.cachedState.m_Status != 0;

    public bool canNotBeInstalled
    {
      get
      {
        return (DeploymentState) ToolchainDeployment.dependencyManager.cachedState != DeploymentState.NotInstalled;
      }
    }

    public bool canNotBeUninstalled
    {
      get
      {
        return (DeploymentState) ToolchainDeployment.dependencyManager.cachedState == DeploymentState.NotInstalled;
      }
    }

    public bool canNotBeRepaired
    {
      get
      {
        return (DeploymentState) ToolchainDeployment.dependencyManager.cachedState != DeploymentState.Invalid;
      }
    }

    public bool canNotBeUpdated
    {
      get
      {
        return (DeploymentState) ToolchainDeployment.dependencyManager.cachedState != DeploymentState.Outdated;
      }
    }

    public bool noNeedDownloadPath
    {
      get
      {
        return (DeploymentState) ToolchainDeployment.dependencyManager.cachedState == DeploymentState.Installed;
      }
    }

    public bool showWarning
    {
      get
      {
        if (!this.isInstalled)
          return false;
        return (DeploymentState) ToolchainDeployment.dependencyManager.cachedState == DeploymentState.Invalid || (DeploymentState) ToolchainDeployment.dependencyManager.cachedState == DeploymentState.Outdated;
      }
    }

    public override void SetDefaults()
    {
    }

    public override AutomaticSettings.SettingPageData GetPageData(string id, bool addPrefix)
    {
      AutomaticSettings.SettingPageData pageData = base.GetPageData(id, addPrefix);
      pageData.AddGroup("Disclaimer");
      MultilineTextSettingItemData textSettingItemData1 = new MultilineTextSettingItemData((Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (ModdingSettings), typeof (string), "disclaimer")
      {
        canRead = true,
        canWrite = false,
        attributes = {
          (System.Attribute) new SettingsUIMultilineTextAttribute("Media/Misc/Warning.svg"),
          (System.Attribute) new SettingsUISearchHiddenAttribute()
        }
      }, pageData.prefix);
      textSettingItemData1.simpleGroup = "Disclaimer";
      MultilineTextSettingItemData textSettingItemData2 = textSettingItemData1;
      pageData["General"].AddItem((AutomaticSettings.SettingItemData) textSettingItemData2);
      ModdingToolchainSettingItem toolchainSettingItem1 = new ModdingToolchainSettingItem((Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (ModdingSettings), typeof (IToolchainDependency), "ToolchainDeployment")
      {
        canRead = true,
        canWrite = false,
        getter = (Func<object, object>) (_ => (object) ToolchainDependencyManager.m_MainDependency)
      }, pageData.prefix);
      toolchainSettingItem1.simpleGroup = "Main";
      toolchainSettingItem1.valueVersionAction = new Func<int>(((object) ToolchainDependencyManager.m_MainDependency).GetHashCode);
      ModdingToolchainSettingItem toolchainSettingItem2 = toolchainSettingItem1;
      pageData["General"].InsertItem((AutomaticSettings.SettingItemData) toolchainSettingItem2, 0);
      pageData.AddGroup("Dependencies");
      foreach (IToolchainDependency dependency in ToolchainDeployment.dependencyManager)
        pageData["General"].AddItem((AutomaticSettings.SettingItemData) this.GetItem(dependency, pageData));
      return pageData;
    }

    private ModdingToolchainSettingItem GetItem(
      IToolchainDependency dependency,
      AutomaticSettings.SettingPageData pageData)
    {
      ModdingToolchainSettingItem toolchainSettingItem1 = new ModdingToolchainSettingItem((Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (ModdingSettings), typeof (IToolchainDependency), dependency.GetType().Name)
      {
        canRead = true,
        canWrite = false,
        getter = (Func<object, object>) (obj => (object) dependency)
      }, pageData.prefix);
      toolchainSettingItem1.simpleGroup = "Dependencies";
      toolchainSettingItem1.valueVersionAction = new Func<int>(((object) dependency).GetHashCode);
      toolchainSettingItem1.description = dependency.description;
      ModdingToolchainSettingItem toolchainSettingItem2 = toolchainSettingItem1;
      if (dependency.canBeInstalled)
      {
        foreach (DeploymentAction deploymentAction in Enum.GetValues(typeof (DeploymentAction)))
        {
          DeploymentAction action = deploymentAction;
          if (action != DeploymentAction.None)
          {
            AutomaticSettings.SettingItemData settingItemData = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.BoolButton, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (ModdingSettings), typeof (bool), action.ToString().ToLower() + "ModdingToolchain")
            {
              canRead = false,
              canWrite = true,
              setter = (Action<object, object>) ((obj, value) => ToolchainDeployment.RunWithUI(action, new List<IToolchainDependency>()
              {
                dependency
              })),
              attributes = {
                (System.Attribute) new SettingsUIButtonGroupAttribute(dependency.name + "toolchainAction")
              }
            }, pageData.prefix)
            {
              hideAction = (Func<bool>) (() => (dependency.availableActions & action) == DeploymentAction.None),
              disableAction = (Func<bool>) (() => this.isActionDisabled)
            };
            toolchainSettingItem2.children.Add(settingItemData);
          }
        }
      }
      if (dependency.canBeInstalled && dependency.canChangeInstallationDirectory)
      {
        AutomaticSettings.SettingItemData settingItemData = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.DirectoryPicker, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (ModdingSettings), typeof (string), dependency.GetType().Name + ".InstallationDirectory")
        {
          canRead = true,
          canWrite = true,
          getter = (Func<object, object>) (obj => (object) dependency.installationDirectory),
          setter = (Action<object, object>) ((obj, value) => dependency.installationDirectory = (string) value),
          attributes = {
            (System.Attribute) new SettingsUIDisplayNameAttribute(typeof (ModdingSettings).Name + ".installationDirectory")
          }
        }, pageData.prefix)
        {
          hideAction = (Func<bool>) (() => this.isActionDisabled || dependency.state.m_State == DependencyState.Installed)
        };
        toolchainSettingItem2.children.Add(settingItemData);
      }
      if (dependency is CombinedDependency combinedDependency)
        toolchainSettingItem2.children.AddRange((IEnumerable<AutomaticSettings.SettingItemData>) combinedDependency.dependencies.Select<IToolchainDependency, ModdingToolchainSettingItem>((Func<IToolchainDependency, ModdingToolchainSettingItem>) (d => this.GetItem(d, pageData))));
      return toolchainSettingItem2;
    }
  }
}
