// Decompiled with JetBrains decompiler
// Type: Game.Settings.SharedSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Localization;
using Colossal.PSI.Common;
using Game.PSI.PdxSdk;
using Game.SceneFlow;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Settings
{
  public class SharedSettings
  {
    private readonly List<Setting> m_Settings = new List<Setting>();

    public static SharedSettings instance => GameManager.instance?.settings;

    public GeneralSettings general { get; private set; }

    public AudioSettings audio { get; private set; }

    public GameplaySettings gameplay { get; private set; }

    public RadioSettings radio { get; private set; }

    public GraphicsSettings graphics { get; private set; }

    public EditorSettings editor { get; private set; }

    public InterfaceSettings userInterface { get; private set; }

    public InputSettings input { get; private set; }

    public KeybindingSettings keybinding { get; private set; }

    public ModdingSettings modding { get; private set; }

    public UserState userState { get; private set; }

    public SharedSettings(LocalizationManager localizationManager)
    {
      this.m_Settings.Add((Setting) (this.general = new GeneralSettings()));
      this.m_Settings.Add((Setting) (this.audio = new AudioSettings()));
      this.m_Settings.Add((Setting) (this.gameplay = new GameplaySettings()));
      this.m_Settings.Add((Setting) (this.radio = new RadioSettings()));
      this.m_Settings.Add((Setting) (this.graphics = new GraphicsSettings()));
      this.m_Settings.Add((Setting) (this.editor = new EditorSettings()));
      this.m_Settings.Add((Setting) (this.userInterface = new InterfaceSettings()));
      this.m_Settings.Add((Setting) (this.input = new InputSettings()));
      this.m_Settings.Add((Setting) (this.userState = new UserState()));
      this.m_Settings.Add((Setting) (this.keybinding = new KeybindingSettings()));
      this.m_Settings.Add((Setting) (this.modding = new ModdingSettings()));
      this.LoadSettings();
      LauncherSettings.LoadSettings(localizationManager, this);
      localizationManager.SetActiveLocale(this.userInterface.locale);
    }

    public void RegisterInOptionsUI()
    {
      this.general.RegisterInOptionsUI("General");
      this.graphics.RegisterInOptionsUI("Graphics");
      this.gameplay.RegisterInOptionsUI("Gameplay");
      this.userInterface.RegisterInOptionsUI("Interface");
      this.audio.RegisterInOptionsUI("Audio");
      this.input.RegisterInOptionsUI("Input");
      this.modding.RegisterInOptionsUI("Modding");
      if (GameManager.instance.configuration.developerMode)
      {
        new About().RegisterInOptionsUI("About");
        PlatformManager.instance.onStatusChanged += (OnStatusChangedEventHandler) (psi => new About().RegisterInOptionsUI("About"));
      }
      // ISSUE: method pointer
      UnityEngine.InputSystem.InputSystem.onDeviceChange += new Action<InputDevice, InputDeviceChange>((object) this, __methodptr(\u003CRegisterInOptionsUI\u003Eg__OnDeviceChange\u007C48_1));
      // ISSUE: method pointer
      Game.Input.InputManager.instance.EventControlSchemeChanged += new Action<Game.Input.InputManager.ControlScheme>((object) this, __methodptr(\u003CRegisterInOptionsUI\u003Eg__OnControlSchemeChanged\u007C48_2));
    }

    public void LoadSettings()
    {
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("General Settings", (object) this.general, (object) new GeneralSettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Audio Settings", (object) this.audio, (object) new AudioSettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Gameplay Settings", (object) this.gameplay, (object) new GameplaySettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Radio Settings", (object) this.radio, (object) new RadioSettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Graphics Settings", (object) this.graphics, (object) new GraphicsSettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Editor Settings", (object) this.editor, (object) new EditorSettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Interface Settings", (object) this.userInterface, (object) new InterfaceSettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Input Settings", (object) this.input, (object) new InputSettings());
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Keybinding Settings", (object) this.keybinding);
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Modding Settings", (object) this.modding, (object) new ModdingSettings());
    }

    public void LoadUserSettings()
    {
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("User Settings", (object) this.userState, (object) new UserState());
    }

    public void Reset()
    {
      Launcher.DeleteLastSaveMetadata();
      foreach (Setting setting in this.m_Settings)
      {
        setting.SetDefaults();
        setting.ApplyAndSave();
      }
    }

    public void Apply()
    {
      foreach (Setting setting in this.m_Settings)
        setting.Apply();
    }
  }
}
