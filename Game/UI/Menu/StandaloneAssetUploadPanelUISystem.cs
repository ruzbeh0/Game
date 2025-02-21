// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.StandaloneAssetUploadPanelUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.UI.Binding;
using Game.UI.Editor;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Menu
{
  public class StandaloneAssetUploadPanelUISystem : UISystemBase
  {
    private static readonly string kGroup = "assetUploadPanel";
    private AssetUploadPanelUISystem m_AssetUploadPanelUISystem;
    private WidgetBindings m_WidgetBindings;
    private ValueBinding<bool> m_Visible;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_AssetUploadPanelUISystem = this.World.GetOrCreateSystemManaged<AssetUploadPanelUISystem>();
      this.m_AssetUploadPanelUISystem.Enabled = false;
      this.AddUpdateBinding((IUpdateBinding) (this.m_WidgetBindings = new WidgetBindings(StandaloneAssetUploadPanelUISystem.kGroup)));
      EditorPanelUISystem.AddEditorWidgetBindings(this.m_WidgetBindings);
      this.AddBinding((IBinding) (this.m_Visible = new ValueBinding<bool>(StandaloneAssetUploadPanelUISystem.kGroup, "visible", false)));
      this.AddBinding((IBinding) new TriggerBinding(StandaloneAssetUploadPanelUISystem.kGroup, "close", new Action(this.OnClose)));
    }

    public void Show(AssetData mainAsset, bool allowManualFileCopy = true)
    {
      this.m_AssetUploadPanelUISystem.Show(mainAsset, allowManualFileCopy);
      this.m_AssetUploadPanelUISystem.onChildrenChange -= new Action<IList<IWidget>>(this.OnChildrenChange);
      this.m_AssetUploadPanelUISystem.onChildrenChange += new Action<IList<IWidget>>(this.OnChildrenChange);
      this.m_AssetUploadPanelUISystem.Enabled = true;
      this.m_WidgetBindings.children = this.m_AssetUploadPanelUISystem.children;
      this.m_Visible.Update(true);
    }

    private void OnClose()
    {
      if (!this.m_AssetUploadPanelUISystem.Close())
        return;
      this.m_AssetUploadPanelUISystem.onChildrenChange -= new Action<IList<IWidget>>(this.OnChildrenChange);
      this.m_AssetUploadPanelUISystem.Enabled = false;
      this.m_WidgetBindings.children = (IList<IWidget>) Array.Empty<IWidget>();
      this.m_Visible.Update(false);
    }

    private void OnChildrenChange(IList<IWidget> children)
    {
      this.m_WidgetBindings.children = children;
    }

    [Preserve]
    public StandaloneAssetUploadPanelUISystem()
    {
    }
  }
}
