// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorAssetUploadPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Game.UI.Localization;
using Game.UI.Menu;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorAssetUploadPanel : EditorPanelSystemBase
  {
    private AssetUploadPanelUISystem m_AssetUploadPanelSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_AssetUploadPanelSystem = this.World.GetOrCreateSystemManaged<AssetUploadPanelUISystem>();
      this.m_AssetUploadPanelSystem.Enabled = false;
      this.title = (LocalizedString) "Menu.ASSET_UPLOAD";
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      this.m_AssetUploadPanelSystem.onChildrenChange += new Action<IList<IWidget>>(this.OnChildrenChange);
      this.m_AssetUploadPanelSystem.Enabled = true;
      this.children = this.m_AssetUploadPanelSystem.children;
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      this.m_AssetUploadPanelSystem.Enabled = false;
      this.m_AssetUploadPanelSystem.onChildrenChange -= new Action<IList<IWidget>>(this.OnChildrenChange);
    }

    private void OnChildrenChange(IList<IWidget> _children) => this.children = _children;

    public void Show(AssetData mainAsset, bool allowManualFileCopy = true)
    {
      this.m_AssetUploadPanelSystem.Show(mainAsset, allowManualFileCopy);
    }

    protected override bool OnClose() => this.m_AssetUploadPanelSystem.Close();

    [Preserve]
    public EditorAssetUploadPanel()
    {
    }
  }
}
