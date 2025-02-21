// Decompiled with JetBrains decompiler
// Type: Game.Modding.ModManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.Logging.Diagnostics;
using Colossal.PSI.Common;
using Colossal.Reflection;
using Colossal.UI;
using Game.PSI;
using Game.SceneFlow;
using Game.Serialization;
using Game.UI;
using Game.UI.Localization;
using Game.UI.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Unity.Entities;

#nullable disable
namespace Game.Modding
{
  public class ModManager : IEnumerable<ModManager.ModInfo>, IEnumerable, IDisposable
  {
    private const string kBurstSuffix = "_win_x86_64";
    private static ILog log = LogManager.GetLogger("Modding").SetShowsErrorsInUI(false);
    private readonly List<ModManager.ModInfo> m_ModsInfos = new List<ModManager.ModInfo>();
    private bool m_Disabled;
    private bool m_Initialized;
    private bool m_IsInProgress;

    public bool isInitialized => this.m_Initialized;

    public IEnumerator<ModManager.ModInfo> GetEnumerator()
    {
      return (IEnumerator<ModManager.ModInfo>) this.m_ModsInfos.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private ModManager()
    {
    }

    public static bool AreModsEnabled()
    {
      GameManager instance = GameManager.instance;
      if (instance == null)
        return false;
      int? length = instance.modManager?.ListModsEnabled().Length;
      int num = 0;
      return length.GetValueOrDefault() > num & length.HasValue;
    }

    public static string[] GetModsEnabled() => GameManager.instance?.modManager?.ListModsEnabled();

    public string[] ListModsEnabled()
    {
      return this.m_ModsInfos.Where<ModManager.ModInfo>((Func<ModManager.ModInfo, bool>) (x => x.isLoaded)).Select<ModManager.ModInfo, string>((Func<ModManager.ModInfo, string>) (x => x.name)).Concat<string>(Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<UIModuleAsset>(new SearchFilter<UIModuleAsset>()).Select<UIModuleAsset, string>((Func<UIModuleAsset, string>) (x => x.name))).ToArray<string>();
    }

    public ModManager(bool disabled)
    {
      this.m_Disabled = disabled;
      if (disabled)
        return;
      NotificationSystem.Push("ModLoadingStatus", titleId: "ModsLoading", textId: "ModsLoadingWaiting", progressState: new ProgressState?(ProgressState.Indeterminate));
    }

    public void Initialize(UpdateSystem updateSystem)
    {
      if (this.m_Disabled || this.m_Initialized)
        return;
      if (this.m_IsInProgress)
        return;
      try
      {
        this.m_IsInProgress = true;
        NotificationSystem.Push("ModLoadingStatus", text: new LocalizedString?((LocalizedString) "Initializing mods"), titleId: "ModsLoading", progressState: new ProgressState?(ProgressState.Indeterminate));
        this.RegisterMods();
        this.InitializeMods(updateSystem);
        this.m_Initialized = true;
        int num = 0;
        foreach (ModManager.ModInfo modsInfo in this.m_ModsInfos)
        {
          ModManager.ModInfo modInfo = modsInfo;
          if (modInfo.state >= ModManager.ModInfo.State.IsNotModWarning)
          {
            ++num;
            string id = modInfo.asset.GetHashCode().ToString();
            string str1 = string.IsNullOrEmpty(modInfo.asset.mod.thumbnailPath) ? (string) null : string.Format("{0}?width={1})", (object) modInfo.asset.mod.thumbnailPath, (object) NotificationUISystem.width);
            ProgressState progressState1;
            switch (modInfo.state)
            {
              case ModManager.ModInfo.State.IsNotModWarning:
                progressState1 = ProgressState.Warning;
                break;
              case ModManager.ModInfo.State.IsNotUniqueWarning:
                progressState1 = ProgressState.Warning;
                break;
              case ModManager.ModInfo.State.GeneralError:
                progressState1 = ProgressState.Failed;
                break;
              case ModManager.ModInfo.State.MissedDependenciesError:
                progressState1 = ProgressState.Failed;
                break;
              case ModManager.ModInfo.State.LoadAssemblyError:
                progressState1 = ProgressState.Failed;
                break;
              case ModManager.ModInfo.State.LoadAssemblyReferenceError:
                progressState1 = ProgressState.Failed;
                break;
              default:
                progressState1 = ProgressState.Failed;
                break;
            }
            ProgressState progressState = progressState1;
            string identifier = id;
            LocalizedString? title = new LocalizedString?((LocalizedString) modInfo.asset.mod.displayName);
            string str2 = str1;
            ProgressState? nullable = new ProgressState?(progressState);
            LocalizedString? text = new LocalizedString?();
            string thumbnail = str2;
            ProgressState? progressState2 = nullable;
            int? progress = new int?();
            Action onClicked = (Action) (() =>
            {
              string str3 = "Common.DIALOG_TITLE_MODDING[" + (progressState == ProgressState.Warning ? "ModLoadingWarning" : "ModLoadingError") + "]";
              LocalizedString message = new LocalizedString(string.Format("Common.DIALOG_MESSAGE_MODDING[{0}]", (object) modInfo.state), (string) null, (IReadOnlyDictionary<string, ILocElement>) new System.Collections.Generic.Dictionary<string, ILocElement>()
              {
                {
                  "MODNAME",
                  (ILocElement) LocalizedString.Value(modInfo.asset.mod.displayName)
                }
              });
              LocalizedString[] localizedStringArray1;
              if (!modInfo.asset.isLocal)
                localizedStringArray1 = new LocalizedString[2]
                {
                  LocalizedString.Id("Common.DIALOG_MESSAGE_MODDING[ModPage]"),
                  LocalizedString.Id("Common.DIALOG_MESSAGE_MODDING[Disable]")
                };
              else
                localizedStringArray1 = Array.Empty<LocalizedString>();
              LocalizedString[] localizedStringArray2 = localizedStringArray1;
              if (modInfo.loadError != null)
                GameManager.instance.userInterface.appBindings.ShowMessageDialog(new Game.UI.MessageDialog(new LocalizedString?((LocalizedString) str3), message, new LocalizedString?(LocalizedString.Value(modInfo.loadError.Replace("\\", "\\\\").Replace("*", "\\*"))), true, LocalizedString.Id("Common.OK"), localizedStringArray2), new Action<int>(Callback));
              else
                GameManager.instance.userInterface.appBindings.ShowMessageDialog(new Game.UI.MessageDialog(new LocalizedString?((LocalizedString) str3), message, LocalizedString.Id("Common.OK"), localizedStringArray2), new Action<int>(Callback));
            });
            NotificationSystem.Push(identifier, title, text, textId: "ModsLoadingFailed", thumbnail: thumbnail, progressState: progressState2, progress: progress, onClicked: onClicked);

            void Callback(int msg)
            {
              switch (msg)
              {
                case 0:
                  NotificationSystem.Pop(id);
                  break;
                case 2:
                  NotificationSystem.Pop(id);
                  modInfo.asset.mod.onClick();
                  break;
                case 3:
                  NotificationSystem.Pop(id);
                  modInfo.asset.mod.onEnable(false);
                  break;
              }
            }
          }
        }
        LocalizedString localizedString;
        if (this.m_ModsInfos.Count != 0)
        {
          // ISSUE: reference to a compiler-generated method
          localizedString = new LocalizedString(NotificationUISystem.GetText("ModsLoadingDone"), (string) null, (IReadOnlyDictionary<string, ILocElement>) new System.Collections.Generic.Dictionary<string, ILocElement>()
          {
            {
              "LOADED",
              (ILocElement) new LocalizedNumber<int>(this.m_ModsInfos.Count - num, "integer")
            },
            {
              "TOTAL",
              (ILocElement) new LocalizedNumber<int>(this.m_ModsInfos.Count, "integer")
            }
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          localizedString = LocalizedString.Id(NotificationUISystem.GetText("ModsLoadingDoneZero"));
        }
        NotificationSystem.Pop("ModLoadingStatus", 5f, text: new LocalizedString?(localizedString), titleId: "ModsLoading", progressState: new ProgressState?(ProgressState.Complete));
      }
      catch (Exception ex)
      {
        ModManager.log.Error(ex);
        // ISSUE: reference to a compiler-generated method
        NotificationSystem.Pop("ModLoadingStatus", 5f, text: new LocalizedString?(LocalizedString.Id(NotificationUISystem.GetText("ModsLoadingAllFailed"))), titleId: "ModsLoading", progressState: new ProgressState?(ProgressState.Failed));
      }
      finally
      {
        this.m_IsInProgress = false;
      }
    }

    private void RegisterMods()
    {
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => ModManager.log.InfoFormat("Mods registered in {0}ms", (object) t.TotalMilliseconds))))
      {
        foreach (ExecutableAsset modAsset in ExecutableAsset.GetModAssets(typeof (IMod)))
        {
          try
          {
            this.m_ModsInfos.Add(new ModManager.ModInfo(modAsset));
          }
          catch (Exception ex)
          {
            ModManager.log.ErrorFormat(ex, "Error registering mod {0}", (object) modAsset.fullName);
          }
        }
      }
    }

    private void InitializeMods(UpdateSystem updateSystem)
    {
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => ModManager.log.InfoFormat(string.Format("Mods initialized in {0}ms", (object) 0), (object) t.TotalMilliseconds))))
      {
        foreach (ModManager.ModInfo modsInfo in this.m_ModsInfos)
        {
          ModManager.ModInfo modInfo = modsInfo;
          try
          {
            using (PerformanceCounter.Start((Action<TimeSpan>) (t => ModManager.log.InfoFormat(string.Format("Loaded {{1}} in {0}ms", (object) 0), (object) t.TotalMilliseconds, (object) modInfo.name))))
              modInfo.Load(updateSystem);
          }
          catch (Exception ex)
          {
            ModManager.log.ErrorFormat(ex, "Error initializing mod {0} ({1})", (object) modInfo.name, (object) modInfo.assemblyFullName);
          }
        }
      }
      this.InitializeUIModules();
    }

    private void InitializeUIModules()
    {
      UIModuleAsset[] array = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<UIModuleAsset>(new SearchFilter<UIModuleAsset>()).ToArray<UIModuleAsset>();
      List<string> locations = new List<string>();
      foreach (UIModuleAsset p2 in array)
      {
        if (p2.isEnabled)
        {
          UIManager.defaultUISystem.AddHostLocation("ui-mods", Path.GetDirectoryName(p2.path), p2.isLocal);
          ModManager.log.InfoFormat("Registered UI Module {0} from {1}", (object) p2.moduleInfo, (object) p2);
          locations.Add(p2.couiPath);
        }
      }
      GameManager.instance.userInterface.appBindings.AddActiveUIModLocation((IList<string>) locations);
    }

    public void AddUIModule(UIModuleAsset uiModule)
    {
      if (!this.m_Initialized)
        return;
      UIManager.defaultUISystem.AddHostLocation("ui-mods", Path.GetDirectoryName(uiModule.path), uiModule.isLocal);
      GameManager.instance.userInterface.appBindings.AddActiveUIModLocation((IList<string>) new string[1]
      {
        uiModule.couiPath
      });
      ModManager.log.InfoFormat("Registered UI Module {0} from {1}", (object) uiModule.moduleInfo, (object) uiModule);
    }

    public void RemoveUIModule(UIModuleAsset uiModule)
    {
      if (!this.m_Initialized)
        return;
      UIManager.defaultUISystem.RemoveHostLocation("ui-mods", Path.GetDirectoryName(uiModule.path));
      GameManager.instance.userInterface.appBindings.RemoveActiveUIModLocation((IList<string>) new string[1]
      {
        uiModule.couiPath
      });
      ModManager.log.InfoFormat("Unregistered UI Module {0}", (object) uiModule.moduleInfo);
    }

    public void Dispose()
    {
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => ModManager.log.InfoFormat(string.Format("Mods disposed in {0}ms", (object) 0), (object) t.TotalMilliseconds))))
      {
        foreach (ModManager.ModInfo modsInfo in this.m_ModsInfos)
        {
          ModManager.ModInfo modInfo = modsInfo;
          try
          {
            using (PerformanceCounter.Start((Action<TimeSpan>) (t => ModManager.log.InfoFormat(string.Format("Disposed {{1}} in {0}ms", (object) 0), (object) t.TotalMilliseconds, (object) modInfo.name))))
              modInfo.Dispose();
          }
          catch (Exception ex)
          {
            ModManager.log.ErrorFormat(ex, "Error disposing mod {0} ({1})", (object) modInfo.name, (object) modInfo.assemblyFullName);
          }
        }
        this.m_ModsInfos.Clear();
      }
    }

    public bool restartRequired { get; private set; }

    public void RequireRestart()
    {
      if (!this.m_Initialized || this.restartRequired)
        return;
      this.restartRequired = true;
      ModManager.log.Info((object) "Restart required");
      NotificationSystem.Push("RestartRequired", titleId: "EnabledModsChanged", textId: "EnabledModsChanged", progressState: new ProgressState?(ProgressState.Warning), onClicked: (Action) (() => GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new Game.UI.ConfirmationDialog(new LocalizedString?((LocalizedString) "Common.DIALOG_TITLE[Warning]"), (LocalizedString) DialogMessage.GetId("EnabledModsChanged"), (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), Array.Empty<LocalizedString>()), (Action<int>) (msg =>
      {
        if (msg != 0)
          return;
        this.restartRequired = false;
        GameManager.QuitGame();
      }))));
    }

    public bool TryGetExecutableAsset(IMod mod, out ExecutableAsset asset)
    {
      foreach (ModManager.ModInfo modsInfo in this.m_ModsInfos)
      {
        foreach (IMod instance in (IEnumerable<IMod>) modsInfo.instances)
        {
          if (instance == mod)
          {
            asset = modsInfo.asset;
            return true;
          }
        }
      }
      asset = (ExecutableAsset) null;
      return false;
    }

    public bool TryGetExecutableAsset(Assembly assembly, out ExecutableAsset asset)
    {
      foreach (ModManager.ModInfo modsInfo in this.m_ModsInfos)
      {
        if (modsInfo.asset.assembly == assembly)
        {
          asset = modsInfo.asset;
          return true;
        }
      }
      asset = (ExecutableAsset) null;
      return false;
    }

    public class ModInfo
    {
      private readonly List<IMod> m_Instances = new List<IMod>();

      public IReadOnlyList<IMod> instances => (IReadOnlyList<IMod>) this.m_Instances;

      public ExecutableAsset asset { get; private set; }

      public bool isValid => this.asset.isMod && this.asset.isEnabled && this.asset.isUnique;

      public bool isLoaded => this.asset.isLoaded;

      public bool isBursted => this.asset.isBursted;

      public string name => this.asset.fullName;

      public string assemblyFullName => this.asset.assembly.FullName;

      public ModManager.ModInfo.State state { get; private set; }

      public string loadError { get; private set; }

      public ModInfo(ExecutableAsset asset) => this.asset = asset;

      public void Preload(Assembly[] assemblies)
      {
      }

      public void Load(UpdateSystem updateSystem)
      {
        try
        {
          if (!this.asset.isEnabled || !this.asset.isRequired)
            return;
          if (!this.asset.isMod && !this.asset.isReference)
            this.state = ModManager.ModInfo.State.IsNotModWarning;
          else if (this.asset.isMod && !this.asset.isUnique)
            this.state = ModManager.ModInfo.State.IsNotUniqueWarning;
          else if (this.asset.isMod && !this.asset.canBeLoaded)
          {
            this.state = ModManager.ModInfo.State.MissedDependenciesError;
            this.loadError = string.Join("\n", this.asset.references.Where<KeyValuePair<string, ExecutableAsset>>((Func<KeyValuePair<string, ExecutableAsset>, bool>) (r => (AssetData) r.Value == (IAssetData) null)).Select<KeyValuePair<string, ExecutableAsset>, string>((Func<KeyValuePair<string, ExecutableAsset>, string>) (r => r.Key)));
          }
          else
          {
            ExecutableAsset uniqueAsset;
            this.asset.LoadAssembly(new Action<Assembly>(ModManager.ModInfo.AfterLoadAssembly), out uniqueAsset);
            this.asset = uniqueAsset;
            foreach (Type type in this.asset.assembly.GetTypesDerivedFrom<IMod>())
              this.m_Instances.Add((IMod) FormatterServices.GetUninitializedObject(type));
            this.OnLoad(updateSystem);
            this.state = ModManager.ModInfo.State.Loaded;
          }
        }
        catch (ExecutableAsset.LoadExecutableException ex)
        {
          this.state = ModManager.ModInfo.State.LoadAssemblyError;
          this.loadError = StackTraceHelper.ExtractStackTraceFromException((Exception) ex);
          throw;
        }
        catch (ExecutableAsset.LoadExecutableReferenceException ex)
        {
          this.state = ModManager.ModInfo.State.LoadAssemblyReferenceError;
          this.loadError = StackTraceHelper.ExtractStackTraceFromException((Exception) ex);
          throw;
        }
        catch (Exception ex)
        {
          this.state = ModManager.ModInfo.State.GeneralError;
          this.loadError = StackTraceHelper.ExtractStackTraceFromException(ex);
          throw;
        }
      }

      private static void AfterLoadAssembly(Assembly assembly)
      {
        TypeManager.InitializeAdditionalComponentTypes(assembly);
        TypeManager.InitializeAdditionalSystemTypes(assembly);
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SerializerSystem>().SetDirty();
      }

      private void OnLoad(UpdateSystem updateSystem)
      {
        foreach (IMod instance in this.m_Instances)
          instance.OnLoad(updateSystem);
      }

      private void OnDispose()
      {
        foreach (IMod instance in this.m_Instances)
          instance.OnDispose();
        this.m_Instances.Clear();
      }

      public void Dispose()
      {
        this.OnDispose();
        this.state = ModManager.ModInfo.State.Disposed;
      }

      public enum State
      {
        Unknown,
        Loaded,
        Disposed,
        IsNotModWarning,
        IsNotUniqueWarning,
        GeneralError,
        MissedDependenciesError,
        LoadAssemblyError,
        LoadAssemblyReferenceError,
      }
    }
  }
}
