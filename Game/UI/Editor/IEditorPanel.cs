// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.IEditorPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor
{
  public interface IEditorPanel
  {
    [CanBeNull]
    LocalizedString title { get; }

    IList<IWidget> children { get; }

    void OnValueChanged(IWidget widget);

    bool OnCancel();

    bool OnClose();

    EditorPanelWidgetRenderer widgetRenderer { get; }
  }
}
