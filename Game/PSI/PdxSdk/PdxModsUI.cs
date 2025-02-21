// Decompiled with JetBrains decompiler
// Type: Game.PSI.PdxSdk.PdxModsUI
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using Colossal.Logging;
using Colossal.PSI.Common;
using Colossal.PSI.Environment;
using Colossal.PSI.PdxSdk;
using Colossal.UI;
using Game.Input;
using Game.SceneFlow;
using Game.Settings;
using PDX.ModsUI;
using PDX.ModsUI.Adapters;
using PDX.ModsUI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.PSI.PdxSdk
{
  public class PdxModsUI : IPdxModsUI, IDisposable
  {
    private static ILog log = LogManager.GetLogger(nameof (PdxModsUI));
    private const string kModsUIHost = "ModsUI";
    private const string kModsUIUri = "coui://ModsUI/index.html";
    private PdxSdkPlatform m_PdxPlatform;

    public PdxSdkPlatform platform => this.m_PdxPlatform;

    public PdxModsUI()
    {
      GameManager.instance.inputManager.EventActiveDeviceChanged += new Game.Input.InputManager.ActiveDeviceChanged(this.OnActiveDeviceChanged);
      GameManager.instance.localizationManager.onActiveDictionaryChanged += new Action(this.UpdateLocale);
      this.m_PdxPlatform = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
      this.m_PdxPlatform?.SetPdxModsUI((IPdxModsUI) this);
      PlatformManager.instance.onPlatformRegistered += (PlatformRegisteredHandler) (psi =>
      {
        if (!(psi is PdxSdkPlatform pdxSdkPlatform2))
          return;
        this.m_PdxPlatform = pdxSdkPlatform2;
        this.m_PdxPlatform.SetPdxModsUI((IPdxModsUI) this);
      });
    }

    public string locale
    {
      get
      {
        return GameManager.instance.localizationManager.activeLocaleId.ToPdxLanguage().ToString().Replace('_', '-');
      }
    }

    public ICohtmlViewAdapter uiViewAdapter
    {
      get => (ICohtmlViewAdapter) new PdxModsUI.ColossalUIViewAdapter();
    }

    public ILogService logger => (ILogService) new PdxModsUI.ModsUILogger();

    public void Show() => this.m_PdxPlatform?.ShowModsUI();

    public bool isActive => this.m_PdxPlatform != null && this.m_PdxPlatform.isModsUIActive;

    public void Destroy() => this.m_PdxPlatform?.DestroyModsUI();

    private void OnActiveDeviceChanged(
      InputDevice newDevice,
      InputDevice oldDevice,
      bool schemeChanged)
    {
      if (!schemeChanged && Game.Input.InputManager.instance.activeControlScheme != Game.Input.InputManager.ControlScheme.Gamepad)
        return;
      this.m_PdxPlatform?.UpdateInputMode();
    }

    private void UpdateLocale() => this.m_PdxPlatform?.ChangeModsUILanguage(this.locale);

    public void Dispose()
    {
      GameManager.instance.inputManager.EventActiveDeviceChanged -= new Game.Input.InputManager.ActiveDeviceChanged(this.OnActiveDeviceChanged);
      GameManager.instance.localizationManager.onActiveDictionaryChanged -= new Action(this.UpdateLocale);
    }

    public InputMode GetInputMode()
    {
      Game.Input.InputManager.ControlScheme activeControlScheme = GameManager.instance.inputManager.activeControlScheme;
      Game.Input.InputManager.GamepadType finalInputHintsType = SharedSettings.instance.userInterface.GetFinalInputHintsType();
      switch (activeControlScheme)
      {
        case Game.Input.InputManager.ControlScheme.KeyboardAndMouse:
          return InputMode.KeyboardAndMouse;
        case Game.Input.InputManager.ControlScheme.Gamepad:
          if (finalInputHintsType == Game.Input.InputManager.GamepadType.Xbox)
            return InputMode.XboxSeriesXS;
          if (finalInputHintsType == Game.Input.InputManager.GamepadType.PS)
            return InputMode.PS5;
          throw new Exception(string.Format("Unknown control scheme {0} with gamepad {1}", (object) activeControlScheme, (object) finalInputHintsType));
        default:
          throw new Exception(string.Format("Unknown control scheme {0}", (object) activeControlScheme));
      }
    }

    private class ColossalUIViewAdapter : ICohtmlViewAdapter, IDisposable
    {
      private readonly InputBarrier m_InputBarrier;
      private UIView m_View;

      public ColossalUIViewAdapter()
      {
        this.m_InputBarrier = GameManager.instance.inputManager.CreateGlobalBarrier(nameof (ColossalUIViewAdapter));
        UIView.Settings settings = UIView.Settings.New with
        {
          textInputHandler = (TextInputHandler) GameManager.instance.userInterface.virtualKeyboard
        };
        UIManager.defaultUISystem.AddHostLocation("ModsUI".ToLowerInvariant(), EnvPath.kContentPath + "/Game/~ModsUI~");
        this.m_View = UIManager.defaultUISystem.CreateView("coui://ModsUI/index.html", settings);
        this.m_View.Listener.ReadyForBindings += new Action(this.OnReadyForBindings);
        this.m_View.Listener.TextInputTypeChanged += new Action<ControlType>(this.OnTextInputTypeChanged);
        this.m_View.Listener.CaretRectChanged += new Action<int, int, uint, uint>(this.OnCaretRectChanged);
      }

      private bool isAvailable
      {
        get => this.m_View != null && this.m_View.enabled && this.m_View.View.IsReadyForBindings();
      }

      public event Action ReadyForBindings;

      public bool IsActiveAndEnabled
      {
        get
        {
          UIView view = this.m_View;
          return view != null && view.enabled;
        }
      }

      public void Disable()
      {
        this.m_View.enabled = false;
        this.m_InputBarrier.blocked = false;
      }

      public void Enable()
      {
        this.m_InputBarrier.blocked = true;
        this.m_View.enabled = true;
      }

      public void Reload() => this.m_View.View.Reload();

      public BoundEventHandle BindCall(string callName, Delegate handler)
      {
        if (!this.isAvailable)
          throw new Exception("Not ready for bindings");
        return this.m_View.View.BindCall(callName, handler);
      }

      public BoundEventHandle RegisterForEvent(string callName, Delegate handler)
      {
        if (!this.isAvailable)
          throw new Exception("Not ready for bindings");
        return this.m_View.View.RegisterForEvent(callName, handler);
      }

      public void UnbindCall(BoundEventHandle boundEventHandle)
      {
        if (!this.isAvailable)
          return;
        this.m_View.View.UnbindCall(boundEventHandle);
      }

      public void UnregisterFromEvent(BoundEventHandle boundEventHandle)
      {
        if (!this.isAvailable)
          return;
        this.m_View.View.UnregisterFromEvent(boundEventHandle);
      }

      public void TriggerEvent<T>(string eventName, T message)
      {
        if (!this.isAvailable)
          return;
        this.m_View.View.TriggerEvent<T>(eventName, message);
      }

      public void AddHostLocation(string key, List<string> value)
      {
        this.m_View.uiSystem.AddHostLocation(key, new HashSet<string>((IEnumerable<string>) value), false);
      }

      public void RemoveHostLocation(string key) => this.m_View.uiSystem.RemoveHostLocation(key);

      public void Dispose()
      {
        this.m_View.Listener.ReadyForBindings -= new Action(this.OnReadyForBindings);
        this.m_View.Listener.TextInputTypeChanged -= new Action<ControlType>(this.OnTextInputTypeChanged);
        this.m_View.Listener.CaretRectChanged -= new Action<int, int, uint, uint>(this.OnCaretRectChanged);
        this.m_View.uiSystem.RemoveHostLocation("ModsUI".ToLowerInvariant());
        this.m_View.uiSystem.DestroyView(this.m_View);
        this.m_View = (UIView) null;
        this.m_InputBarrier.Dispose();
      }

      private void OnReadyForBindings()
      {
        Action readyForBindings = this.ReadyForBindings;
        if (readyForBindings == null)
          return;
        readyForBindings();
      }

      private void OnTextInputTypeChanged(ControlType type)
      {
        GameManager.instance.inputManager.hasInputFieldFocus = type == ControlType.TextInput;
      }

      private void OnCaretRectChanged(int x, int y, uint width, uint height)
      {
        GameManager.instance.inputManager.caretRect = (new Vector2((float) x, (float) y), new Vector2((float) width, (float) height));
      }
    }

    private class ModsUILogger : LogService
    {
      public override void WriteLogEntry(
        string message,
        PDX.SDK.Contracts.Enums.LogLevel logLevel,
        string source = null,
        string callerFilePath = null)
      {
        if (logLevel < this.LogLevel || logLevel == PDX.SDK.Contracts.Enums.LogLevel.L9_None)
          return;
        if (source == null && callerFilePath != null)
          source = Path.GetFileNameWithoutExtension(callerFilePath);
        string message1 = source == null ? message : source + ": " + message;
        if (logLevel == PDX.SDK.Contracts.Enums.LogLevel.L2_Warning)
          PdxModsUI.log.Warn((object) message1);
        else if (logLevel == PDX.SDK.Contracts.Enums.LogLevel.L3_Error)
          PdxModsUI.log.Error((object) message1);
        else
          PdxModsUI.log.Info((object) message1);
      }
    }
  }
}
