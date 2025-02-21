// Decompiled with JetBrains decompiler
// Type: Game.UI.AppBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Rendering.Utilities;
using Game.SceneFlow;
using Game.Settings;
using Game.UI.Debug;
using Game.UI.Menu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.UI
{
  public class AppBindings : CompositeBinding, IDisposable
  {
    private const string kGroup = "app";
    public const string kBodyClassNames = "";
    private ValueBinding<string> m_BackgroundProcessMessageBinding;
    private EventBinding<ConfirmationDialogBase> m_ConfirmationDialogBinding;
    private ValueBinding<HashSet<string>> m_ActiveUIModsLocation;
    private Action<int> m_ConfirmationDialogCallback;
    private Action<int, bool> m_DismissibleConfirmationDialogCallback;
    private DebugUISystem m_DebugUISystem;
    private static AppBindings.FrameTiming m_FrameTiming;

    private float GetFPS()
    {
      GeneralSettings general = SharedSettings.instance?.general;
      return general != null && general.fpsMode == GeneralSettings.FPSMode.Precise ? HDRenderPipeline.currentPipeline?.debugDisplaySettings?.debugFrameTiming?.m_FrameHistory?.SampleAverage.FramesPerSecond ?? 1f / Time.smoothDeltaTime : 1f / Time.smoothDeltaTime;
    }

    private float GetFullFrameTime()
    {
      return HDRenderPipeline.currentPipeline?.debugDisplaySettings?.debugFrameTiming?.m_FrameHistory?.SampleAverage.FullFrameTime.GetValueOrDefault();
    }

    private float GetCPUMainThreadTime()
    {
      return HDRenderPipeline.currentPipeline?.debugDisplaySettings?.debugFrameTiming?.m_FrameHistory?.SampleAverage.MainThreadCPUFrameTime.GetValueOrDefault();
    }

    private float GetCPURenderThreadTime()
    {
      return HDRenderPipeline.currentPipeline?.debugDisplaySettings?.debugFrameTiming?.m_FrameHistory?.SampleAverage.RenderThreadCPUFrameTime.GetValueOrDefault();
    }

    private float GetGPUTime()
    {
      return HDRenderPipeline.currentPipeline?.debugDisplaySettings?.debugFrameTiming?.m_FrameHistory?.SampleAverage.GPUFrameTime.GetValueOrDefault();
    }

    public bool ready { get; set; }

    public string activeUI { get; set; }

    public void SetMainMenuActive() => this.activeUI = "Menu";

    public void SetGameActive() => this.activeUI = "Game";

    public void SetEditorActive() => this.activeUI = "Editor";

    public void SetNoneActive() => this.activeUI = (string) null;

    public AppBindings()
    {
      ErrorDialogManager.Initialize();
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("app", nameof (ready), (Func<bool>) (() => this.ready)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<string>("app", nameof (activeUI), (Func<string>) (() => this.activeUI), (IWriter<string>) new StringWriter().Nullable<string>()));
      this.AddBinding((IBinding) new ValueBinding<string>("app", "bodyClassNames", ""));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("app", "fpsMode", (Func<int>) (() =>
      {
        SharedSettings instance = SharedSettings.instance;
        return instance == null ? 0 : (int) instance.general.fpsMode;
      })));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<AppBindings.FrameTiming>("app", "frameStats", (Func<AppBindings.FrameTiming>) (() => AppBindings.m_FrameTiming), (IWriter<AppBindings.FrameTiming>) new ValueWriter<AppBindings.FrameTiming>()));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<string>("app", "activeLocale", (Func<string>) (() => GameManager.instance.localizationManager.activeDictionary.localeID)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<ErrorDialog>("app", "currentError", (Func<ErrorDialog>) (() => ErrorDialogManager.currentErrorDialog), (IWriter<ErrorDialog>) new ValueWriter<ErrorDialog>().Nullable<ErrorDialog>()));
      this.AddBinding((IBinding) (this.m_BackgroundProcessMessageBinding = new ValueBinding<string>("app", "backgroundProcessMessage", (string) null, (IWriter<string>) new StringWriter().Nullable<string>())));
      this.AddBinding((IBinding) new TriggerBinding<string>("app", "setClipboard", new Action<string>(this.SetClipboard)));
      this.AddBinding((IBinding) new TriggerBinding("app", "exitApplication", new Action(this.ExitApplication)));
      this.AddBinding((IBinding) new TriggerBinding("app", "saveBackupAndExitApplication", new Action(this.SaveBackupAndExitApplication)));
      this.AddBinding((IBinding) new TriggerBinding("app", "saveBackup", new Action(this.SaveBackup)));
      this.AddBinding((IBinding) new TriggerBinding("app", "dismissCurrentError", new Action(this.DismissCurrentError)));
      this.AddBinding((IBinding) (this.m_ConfirmationDialogBinding = new EventBinding<ConfirmationDialogBase>("app", "confirmationDialog", (IWriter<ConfirmationDialogBase>) new ValueWriter<ConfirmationDialogBase>())));
      this.AddBinding((IBinding) new TriggerBinding<int>("app", "confirmationDialogCallback", new Action<int>(this.OnConfirmationDialogCallback)));
      this.AddBinding((IBinding) new TriggerBinding<int, bool>("app", "dismissibleConfirmationDialogCallback", new Action<int, bool>(this.OnDismissibleConfirmationDialogCallback)));
      this.AddBinding((IBinding) (this.m_ActiveUIModsLocation = new ValueBinding<HashSet<string>>("app", "activeUIModsLocation", new HashSet<string>(), (IWriter<HashSet<string>>) new CollectionWriter<string>())));
      this.AddBinding((IBinding) new GetterValueBinding<int>("app", "platform", (Func<int>) (() => (int) Application.platform.ToPlatform())));
    }

    public void UpdateActiveUIModsLocation(IList<string> locations)
    {
      this.m_ActiveUIModsLocation.Update(new HashSet<string>((IEnumerable<string>) locations));
    }

    public void AddActiveUIModLocation(IList<string> locations)
    {
      int count = this.m_ActiveUIModsLocation.value.Count;
      foreach (string location in (IEnumerable<string>) locations)
        this.m_ActiveUIModsLocation.value.Add(location);
      if (this.m_ActiveUIModsLocation.value.Count == count)
        return;
      this.m_ActiveUIModsLocation.TriggerUpdate();
    }

    public void RemoveActiveUIModLocation(IList<string> locations)
    {
      int count = this.m_ActiveUIModsLocation.value.Count;
      foreach (string location in (IEnumerable<string>) locations)
        this.m_ActiveUIModsLocation.value.Remove(location);
      if (this.m_ActiveUIModsLocation.value.Count == count)
        return;
      this.m_ActiveUIModsLocation.TriggerUpdate();
    }

    public void Dispose()
    {
      ErrorDialogManager.Dispose();
      AppBindings.m_FrameTiming.Dispose();
    }

    public override bool Update()
    {
      AppBindings.m_FrameTiming.Update();
      AdaptiveDynamicResolutionScale instance = AdaptiveDynamicResolutionScale.instance;
      DebugManager.instance.adaptiveDRSActive = instance.isEnabled && instance.isAdaptive;
      DebugFrameTiming debugFrameTiming = HDRenderPipeline.currentPipeline?.debugDisplaySettings?.debugFrameTiming;
      if (debugFrameTiming != null)
      {
        FrameTimeSample sample = debugFrameTiming.m_Sample;
        instance.UpdateDRS(sample.FullFrameTime, sample.MainThreadCPUFrameTime, sample.RenderThreadCPUFrameTime, sample.GPUFrameTime);
      }
      return base.Update();
    }

    private void ExitApplication() => GameManager.QuitGame();

    private async void SaveBackupAndExitApplication()
    {
      await this.SaveBackupImpl();
      GameManager.QuitGame();
    }

    private async void SaveBackup() => await this.SaveBackupImpl();

    private async Task SaveBackupImpl()
    {
      RenderTexture preview = ScreenCaptureHelper.CreateRenderTarget("PreviewSaveGame-Exit", 680, 383);
      ScreenCaptureHelper.CaptureScreenshot(Camera.main, preview, new MenuHelpers.SaveGamePreviewSettings());
      ScreenCaptureHelper.AsyncRequest request = new ScreenCaptureHelper.AsyncRequest((Texture) preview);
      // ISSUE: variable of a compiler-generated type
      MenuUISystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<MenuUISystem>();
      string saveName = "SaveRecovery" + DateTime.Now.ToString("dd-MMMM-HH-mm-ss");
      try
      {
        // ISSUE: reference to a compiler-generated method
        int num = await GameManager.instance.Save(saveName, existingSystemManaged.GetSaveInfo(false), Colossal.IO.AssetDatabase.AssetDatabase.user, request) ? 1 : 0;
      }
      catch (Exception ex)
      {
        CompositeBinding.log.Error(ex);
      }
      finally
      {
        await request.Dispose();
        CoreUtils.Destroy((UnityEngine.Object) preview);
      }
      preview = (RenderTexture) null;
      request = (ScreenCaptureHelper.AsyncRequest) null;
    }

    private void SetClipboard(string text) => GUIUtility.systemCopyBuffer = text;

    private void DismissCurrentError() => ErrorDialogManager.DismissCurrentErrorDialog();

    public void ShowConfirmationDialog([NotNull] ConfirmationDialog dialog, [NotNull] Action<int> callback)
    {
      this.m_ConfirmationDialogCallback = callback;
      this.m_ConfirmationDialogBinding.Trigger((ConfirmationDialogBase) dialog);
    }

    public void ShowMessageDialog([NotNull] MessageDialog dialog, Action<int> callback)
    {
      this.m_ConfirmationDialogCallback = callback;
      this.m_ConfirmationDialogBinding.Trigger((ConfirmationDialogBase) dialog);
    }

    public void ShowConfirmationDialog(
      [NotNull] DismissibleConfirmationDialog dialog,
      [NotNull] Action<int, bool> callback)
    {
      this.m_DismissibleConfirmationDialogCallback = callback;
      this.m_ConfirmationDialogBinding.Trigger((ConfirmationDialogBase) dialog);
    }

    private void OnConfirmationDialogCallback(int msg)
    {
      if (this.m_ConfirmationDialogCallback == null)
        return;
      Action<int> confirmationDialogCallback = this.m_ConfirmationDialogCallback;
      this.m_ConfirmationDialogCallback = (Action<int>) null;
      int num = msg;
      confirmationDialogCallback(num);
    }

    private void OnDismissibleConfirmationDialogCallback(int msg, bool dontShowAgain)
    {
      if (this.m_DismissibleConfirmationDialogCallback == null)
        return;
      Action<int, bool> confirmationDialogCallback = this.m_DismissibleConfirmationDialogCallback;
      this.m_DismissibleConfirmationDialogCallback = (Action<int, bool>) null;
      int num1 = msg;
      int num2 = dontShowAgain ? 1 : 0;
      confirmationDialogCallback(num1, num2 != 0);
    }

    private struct FrameTiming : IJsonWritable
    {
      private FrameTimeSampleHistory m_History;
      private GeneralSettings m_Settings;
      private DebugUISystem m_DebugUISystem;
      public float fps;
      public float fullFameTime;
      public float cpuMainThreadTime;
      public float cpuRenderThreadTime;
      public float gpuTime;

      public void Update()
      {
        if (this.m_Settings == null)
          this.m_Settings = SharedSettings.instance?.general;
        if (this.m_Settings != null)
        {
          switch (this.m_Settings.fpsMode)
          {
            case GeneralSettings.FPSMode.Simple:
              this.fps = math.max(1f / Time.smoothDeltaTime, 0.0f);
              break;
            case GeneralSettings.FPSMode.Advanced:
              this.fps = math.max(1f / Time.smoothDeltaTime, 0.0f);
              this.fullFameTime = 1000f / this.fps;
              break;
            case GeneralSettings.FPSMode.Precise:
              if (this.m_History == null)
                this.m_History = HDRenderPipeline.currentPipeline?.debugDisplaySettings?.debugFrameTiming?.m_FrameHistory;
              if (this.m_History != null)
              {
                this.fps = this.m_History.SampleAverage.FramesPerSecond;
                this.fullFameTime = this.m_History.SampleAverage.FullFrameTime;
                this.cpuMainThreadTime = this.m_History.SampleAverage.MainThreadCPUFrameTime;
                this.cpuRenderThreadTime = this.m_History.SampleAverage.RenderThreadCPUFrameTime;
                this.gpuTime = this.m_History.SampleAverage.GPUFrameTime;
                DebugManager.instance.externalDebugUIActive = true;
                break;
              }
              break;
          }
        }
        if (this.m_DebugUISystem == null)
          this.m_DebugUISystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<DebugUISystem>();
        DebugManager instance = DebugManager.instance;
        int num1 = instance.externalDebugUIActive ? 1 : 0;
        DebugUISystem debugUiSystem = this.m_DebugUISystem;
        int num2 = debugUiSystem == null ? 0 : (debugUiSystem.visible ? 1 : 0);
        instance.externalDebugUIActive = (num1 | num2) != 0;
      }

      public void Dispose()
      {
        this.m_History = (FrameTimeSampleHistory) null;
        this.m_Settings = (GeneralSettings) null;
        this.m_DebugUISystem = (DebugUISystem) null;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("fps");
        writer.Write(this.fps);
        writer.PropertyName("fullFameTime");
        writer.Write(this.fullFameTime);
        writer.PropertyName("cpuMainThreadTime");
        writer.Write(this.cpuMainThreadTime);
        writer.PropertyName("cpuRenderThreadTime");
        writer.Write(this.cpuRenderThreadTime);
        writer.PropertyName("gpuTime");
        writer.Write(this.gpuTime);
        writer.TypeEnd();
      }
    }
  }
}
