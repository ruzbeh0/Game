// Decompiled with JetBrains decompiler
// Type: Game.AutoSaveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;
using Game.Assets;
using Game.SceneFlow;
using Game.Settings;
using Game.UI;
using Game.UI.Menu;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game
{
  [CompilerGenerated]
  public class AutoSaveSystem : GameSystemBase
  {
    private float m_LastAutoSaveCheck = -1f;

    private float timeSinceStartup => UnityEngine.Time.realtimeSinceStartup;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      SharedSettings.instance.general.onSettingsApplied += (OnSettingsAppliedHandler) (setting =>
      {
        if (!GameManager.instance.gameMode.IsGame() || !(setting is GeneralSettings settings2))
          return;
        if (settings2.autoSave)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_LastAutoSaveCheck >= 0.0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.PruneAutoSaves(settings2);
          // ISSUE: reference to a compiler-generated field
          this.m_LastAutoSaveCheck = this.timeSinceStartup;
          COSystemBase.baseLog.Debug((object) "Auto-save watch active!");
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_LastAutoSaveCheck < 0.0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.PruneAutoSaves(settings2);
          // ISSUE: reference to a compiler-generated field
          this.m_LastAutoSaveCheck = -1f;
          COSystemBase.baseLog.Debug((object) "Auto-save watch inactive!");
        }
      });
    }

    private void OnSettingsChanged(Setting setting)
    {
      if (!GameManager.instance.gameMode.IsGame() || !(setting is GeneralSettings settings))
        return;
      if (settings.autoSave)
      {
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_LastAutoSaveCheck >= 0.0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.PruneAutoSaves(settings);
        // ISSUE: reference to a compiler-generated field
        this.m_LastAutoSaveCheck = this.timeSinceStartup;
        COSystemBase.baseLog.Debug((object) "Auto-save watch active!");
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_LastAutoSaveCheck < 0.0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.PruneAutoSaves(settings);
        // ISSUE: reference to a compiler-generated field
        this.m_LastAutoSaveCheck = -1f;
        COSystemBase.baseLog.Debug((object) "Auto-save watch inactive!");
      }
    }

    [Preserve]
    protected override void OnDestroy()
    {
      SharedSettings.instance.general.onSettingsApplied -= (OnSettingsAppliedHandler) (setting =>
      {
        if (!GameManager.instance.gameMode.IsGame() || !(setting is GeneralSettings settings2))
          return;
        if (settings2.autoSave)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_LastAutoSaveCheck >= 0.0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.PruneAutoSaves(settings2);
          // ISSUE: reference to a compiler-generated field
          this.m_LastAutoSaveCheck = this.timeSinceStartup;
          COSystemBase.baseLog.Debug((object) "Auto-save watch active!");
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_LastAutoSaveCheck < 0.0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.PruneAutoSaves(settings2);
          // ISSUE: reference to a compiler-generated field
          this.m_LastAutoSaveCheck = -1f;
          COSystemBase.baseLog.Debug((object) "Auto-save watch inactive!");
        }
      });
      base.OnDestroy();
    }

    protected override void OnGamePreload(Purpose purpose, GameMode mode)
    {
      if (!SharedSettings.instance.general.autoSave)
        return;
      COSystemBase.baseLog.Debug((object) "Auto-save watch inactive!");
      // ISSUE: reference to a compiler-generated field
      this.m_LastAutoSaveCheck = -1f;
    }

    protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
    {
      if (purpose != Purpose.LoadGame && purpose != Purpose.NewGame || !SharedSettings.instance.general.autoSave)
        return;
      COSystemBase.baseLog.Debug((object) "Auto-save watch active!");
      // ISSUE: reference to a compiler-generated field
      this.m_LastAutoSaveCheck = this.timeSinceStartup;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_LastAutoSaveCheck < 0.0 || !GameManager.instance.gameMode.IsGame())
        return;
      GeneralSettings general = SharedSettings.instance.general;
      if (!general.autoSave)
        return;
      // ISSUE: reference to a compiler-generated method
      this.CheckAutoSave(general);
    }

    private async void CheckAutoSave(GeneralSettings settings)
    {
      // ISSUE: reference to a compiler-generated field
      if ((double) this.timeSinceStartup - (double) this.m_LastAutoSaveCheck <= (double) settings.autoSaveInterval)
        return;
      // ISSUE: reference to a compiler-generated field
      COSystemBase.baseLog.DebugFormat("Auto-save triggered after {0}s", (object) this.m_LastAutoSaveCheck);
      // ISSUE: reference to a compiler-generated field
      this.m_LastAutoSaveCheck = this.timeSinceStartup;
      // ISSUE: reference to a compiler-generated method
      await this.PerformAutoSave(settings);
    }

    private void PruneAutoSaves(GeneralSettings settings)
    {
      if (settings.autoSaveCount == GeneralSettings.AutoSaveCount.Unlimited)
        return;
      try
      {
        // ISSUE: reference to a compiler-generated method
        foreach (SaveGameMetadata saveGameMetadata in AutoSaveSystem.GetAutoSaveDatabaseTarget().GetAssets<SaveGameMetadata>().Where<SaveGameMetadata>((Func<SaveGameMetadata, bool>) (s => s.target.autoSave)).OrderByDescending<SaveGameMetadata, DateTime>((Func<SaveGameMetadata, DateTime>) (s => s.target.lastModified)).ToList<SaveGameMetadata>().Skip<SaveGameMetadata>((int) settings.autoSaveCount))
          SaveHelpers.DeleteSaveGame(saveGameMetadata);
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex, (object) "An error occurred while pruning auto-saves");
      }
    }

    public async Task PerformAutoSave(GeneralSettings settings)
    {
      // ISSUE: reference to a compiler-generated method
      await AutoSaveSystem.SafeAutoSave();
      // ISSUE: reference to a compiler-generated method
      this.PruneAutoSaves(settings);
    }

    private static Task SafeAutoSave()
    {
      return TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (async () =>
      {
        RenderTexture preview = ScreenCaptureHelper.CreateRenderTarget("PreviewSaveGame-Auto", 680, 383);
        ScreenCaptureHelper.CaptureScreenshot(Camera.main, preview, new MenuHelpers.SaveGamePreviewSettings());
        // ISSUE: variable of a compiler-generated type
        MenuUISystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<MenuUISystem>();
        string str = string.Format("AutoSave{0:dd-MMMM-HH-mm-ss}", (object) DateTime.Now);
        COSystemBase.baseLog.InfoFormat("Auto-saving {0}...", (object) str);
        try
        {
          // ISSUE: reference to a compiler-generated method
          ILocalAssetDatabase saveDatabaseTarget = AutoSaveSystem.GetAutoSaveDatabaseTarget();
          PackageAsset asset;
          if (saveDatabaseTarget.Exists<PackageAsset>(SaveHelpers.GetAssetDataPath<SaveGameMetadata>(saveDatabaseTarget, str), out asset))
            saveDatabaseTarget.DeleteAsset<PackageAsset>(asset);
          // ISSUE: reference to a compiler-generated method
          int num = await GameManager.instance.Save(str, existingSystemManaged.GetSaveInfo(true), saveDatabaseTarget, (Texture) preview) ? 1 : 0;
          preview = (RenderTexture) null;
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
          preview = (RenderTexture) null;
        }
        finally
        {
          CoreUtils.Destroy((UnityEngine.Object) preview);
        }
      }), 1);
    }

    private static async Task AutoSave()
    {
      RenderTexture preview = ScreenCaptureHelper.CreateRenderTarget("PreviewSaveGame-Auto", 680, 383);
      ScreenCaptureHelper.CaptureScreenshot(Camera.main, preview, new MenuHelpers.SaveGamePreviewSettings());
      // ISSUE: variable of a compiler-generated type
      MenuUISystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<MenuUISystem>();
      string str = string.Format("AutoSave{0:dd-MMMM-HH-mm-ss}", (object) DateTime.Now);
      COSystemBase.baseLog.InfoFormat("Auto-saving {0}...", (object) str);
      try
      {
        // ISSUE: reference to a compiler-generated method
        ILocalAssetDatabase saveDatabaseTarget = AutoSaveSystem.GetAutoSaveDatabaseTarget();
        PackageAsset asset;
        if (saveDatabaseTarget.Exists<PackageAsset>(SaveHelpers.GetAssetDataPath<SaveGameMetadata>(saveDatabaseTarget, str), out asset))
          saveDatabaseTarget.DeleteAsset<PackageAsset>(asset);
        // ISSUE: reference to a compiler-generated method
        int num = await GameManager.instance.Save(str, existingSystemManaged.GetSaveInfo(true), saveDatabaseTarget, (Texture) preview) ? 1 : 0;
        preview = (RenderTexture) null;
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
        preview = (RenderTexture) null;
      }
      finally
      {
        CoreUtils.Destroy((UnityEngine.Object) preview);
      }
    }

    private static ILocalAssetDatabase GetAutoSaveDatabaseTarget() => Colossal.IO.AssetDatabase.AssetDatabase.user;

    [Preserve]
    public AutoSaveSystem()
    {
    }
  }
}
