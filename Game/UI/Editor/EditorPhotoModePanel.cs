// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorPhotoModePanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.InGame;
using Game.UI.Localization;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorPhotoModePanel : EditorPanelSystemBase
  {
    private PhotoModeUISystem m_PhotoModeUISystem;

    public override EditorPanelWidgetRenderer widgetRenderer => EditorPanelWidgetRenderer.PhotoMode;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.title = (LocalizedString) "PhotoMode.TITLE";
      this.m_PhotoModeUISystem = this.World.GetOrCreateSystemManaged<PhotoModeUISystem>();
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      this.m_PhotoModeUISystem.Activate(true);
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      this.m_PhotoModeUISystem.Activate(false);
    }

    [Preserve]
    public EditorPhotoModePanel()
    {
    }
  }
}
