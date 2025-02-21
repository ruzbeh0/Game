// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.BulldozeToolPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class BulldozeToolPanel : EditorPanelSystemBase
  {
    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.children = (IList<IWidget>) Array.Empty<IWidget>();
      this.title = (LocalizedString) "Editor.TOOL[BulldozeTool]";
    }

    [Preserve]
    public BulldozeToolPanel()
    {
    }
  }
}
