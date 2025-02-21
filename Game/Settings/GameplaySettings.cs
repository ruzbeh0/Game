// Decompiled with JetBrains decompiler
// Type: Game.Settings.GameplaySettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using UnityEngine;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  public class GameplaySettings : Setting
  {
    public const string kName = "Gameplay";
    private CameraController m_CameraController;

    public bool edgeScrolling { get; set; }

    public bool dayNightVisual { get; set; }

    public bool pausedAfterLoading { get; set; }

    public bool showTutorials { get; set; }

    [SettingsUIButton]
    [SettingsUIConfirmation(null, null)]
    public bool resetTutorials
    {
      set => SharedSettings.instance.userState.ResetTutorials();
    }

    public GameplaySettings() => this.SetDefaults();

    public override void SetDefaults()
    {
      this.edgeScrolling = true;
      this.dayNightVisual = true;
      this.pausedAfterLoading = false;
      this.showTutorials = true;
    }

    public override void Apply()
    {
      base.Apply();
      if ((Object) this.m_CameraController == (Object) null)
        this.TryGetGameplayCameraController(ref this.m_CameraController);
      if (!((Object) this.m_CameraController != (Object) null))
        return;
      this.m_CameraController.edgeScrolling = this.edgeScrolling;
    }
  }
}
