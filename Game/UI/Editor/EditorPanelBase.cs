// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorPanelBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor
{
  public abstract class EditorPanelBase : IEditorPanel
  {
    public LocalizedString title { get; set; }

    public IList<IWidget> children { get; set; } = (IList<IWidget>) Array.Empty<IWidget>();

    public virtual EditorPanelWidgetRenderer widgetRenderer => EditorPanelWidgetRenderer.Editor;

    public virtual void OnValueChanged(IWidget widget)
    {
    }

    public virtual bool OnCancel() => this.OnClose();

    public virtual bool OnClose() => true;
  }
}
