// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.GameScreenUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Input;
using Game.PSI;
using Game.SceneFlow;
using Game.Serialization;
using Game.Settings;
using Game.UI.Localization;
using System;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class GameScreenUISystem : UISystemBase, IPreDeserialize
  {
    private const string kSavingGameNotificationTitle = "SavingGame";
    private const string kGroup = "game";
    private ValueBinding<int> m_ActiveScreenBinding;
    private ValueBinding<bool> m_CanUseSaveSystem;

    public int activeScreen => this.m_ActiveScreenBinding.value;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.AddBinding((IBinding) (this.m_ActiveScreenBinding = new ValueBinding<int>("game", "activeScreen", 0)));
      this.AddBinding((IBinding) new TriggerBinding<int>("game", "setActiveScreen", new Action<int>(this.SetScreen)));
      this.AddBinding((IBinding) (this.m_CanUseSaveSystem = new ValueBinding<bool>("game", "canUseSaveSystem", true)));
      GameManager.instance.onGameSaveLoad += new GameManager.EventGameSaveLoad(this.SaveLoadInProgress);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      GameManager.instance.onGameSaveLoad -= new GameManager.EventGameSaveLoad(this.SaveLoadInProgress);
      base.OnDestroy();
    }

    private void SaveLoadInProgress(string name, bool start)
    {
      if (start)
      {
        string identifier = "SavingGame" + name;
        LocalizedString? nullable1 = new LocalizedString?(LocalizedString.Value(name));
        ProgressState? nullable2 = new ProgressState?(ProgressState.Indeterminate);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable1;
        ProgressState? progressState = nullable2;
        int? progress = new int?();
        NotificationSystem.Push(identifier, title, text, "SavingGame", progressState: progressState, progress: progress);
      }
      else
      {
        string identifier = "SavingGame" + name;
        LocalizedString? nullable3 = new LocalizedString?(LocalizedString.Value(name));
        ProgressState? nullable4 = new ProgressState?(ProgressState.Complete);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable3;
        ProgressState? progressState = nullable4;
        int? progress = new int?();
        NotificationSystem.Pop(identifier, 1f, title, text, "SavingGame", progressState: progressState, progress: progress);
      }
      this.m_CanUseSaveSystem.Update(!start);
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    public void PreDeserialize(Context context) => this.SetScreen(0);

    public void OpenScreen(GameScreenUISystem.GameScreen screen)
    {
      this.m_ActiveScreenBinding.Update((int) screen);
    }

    public bool isMenuActive => this.m_ActiveScreenBinding.value >= 10;

    public void SetScreen(int screen)
    {
      GameScreenUISystem.GameScreen gameScreen = (GameScreenUISystem.GameScreen) screen;
      InputManager.instance.hideCursor = gameScreen == GameScreenUISystem.GameScreen.FreeCamera;
      InputManager.instance.cursorLockMode = gameScreen == GameScreenUISystem.GameScreen.Main ? SharedSettings.instance.graphics.cursorMode.ToUnityCursorMode() : CursorLockMode.None;
      this.m_ActiveScreenBinding.Update(screen);
    }

    [Preserve]
    public GameScreenUISystem()
    {
    }

    public enum GameScreen
    {
      Main = 0,
      FreeCamera = 1,
      PauseMenu = 10, // 0x0000000A
      SaveGame = 11, // 0x0000000B
      NewGame = 12, // 0x0000000C
      LoadGame = 13, // 0x0000000D
      Options = 14, // 0x0000000E
    }
  }
}
