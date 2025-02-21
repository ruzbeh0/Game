// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorPanelSystemBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Logging;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public abstract class EditorPanelSystemBase : GameSystemBase, IEditorPanel
  {
    [CanBeNull]
    private IEditorPanel m_LastSubPanel;

    [CanBeNull]
    protected IEditorPanel activeSubPanel { get; set; }

    protected virtual LocalizedString title { get; set; }

    protected virtual IList<IWidget> children { get; set; } = (IList<IWidget>) Array.Empty<IWidget>();

    public virtual EditorPanelWidgetRenderer widgetRenderer => EditorPanelWidgetRenderer.Editor;

    protected ILog log { get; } = LogManager.GetLogger("Editor");

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.Enabled = false;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.activeSubPanel != this.m_LastSubPanel)
      {
        if (this.m_LastSubPanel is ComponentSystemBase lastSubPanel)
        {
          lastSubPanel.Enabled = false;
          lastSubPanel.Update();
        }
        this.m_LastSubPanel = this.activeSubPanel;
        if (this.activeSubPanel is ComponentSystemBase activeSubPanel)
          activeSubPanel.Enabled = true;
      }
      if (!(this.activeSubPanel is ComponentSystemBase activeSubPanel1))
        return;
      activeSubPanel1.Update();
    }

    protected virtual void OnValueChanged(IWidget widget)
    {
    }

    protected virtual bool OnCancel() => this.OnClose();

    protected virtual bool OnClose() => true;

    protected void CloseSubPanel() => this.activeSubPanel = (IEditorPanel) null;

    LocalizedString IEditorPanel.title
    {
      get => this.activeSubPanel == null ? this.title : this.activeSubPanel.title;
    }

    IList<IWidget> IEditorPanel.children
    {
      get => this.activeSubPanel == null ? this.children : this.activeSubPanel.children;
    }

    void IEditorPanel.OnValueChanged(IWidget widget)
    {
      if (this.activeSubPanel != null)
        this.activeSubPanel.OnValueChanged(widget);
      else
        this.OnValueChanged(widget);
    }

    bool IEditorPanel.OnCancel()
    {
      if (this.activeSubPanel == null)
        return this.OnCancel();
      if (this.activeSubPanel.OnCancel())
        this.activeSubPanel = (IEditorPanel) null;
      return false;
    }

    bool IEditorPanel.OnClose()
    {
      if (this.activeSubPanel == null)
        return this.OnClose();
      if (this.activeSubPanel.OnClose())
        this.activeSubPanel = (IEditorPanel) null;
      return false;
    }

    [Preserve]
    protected EditorPanelSystemBase()
    {
    }
  }
}
