// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.Row
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  internal class Row : LayoutContainer
  {
    public bool wrap { get; set; }

    public static Row WithChildren(IList<IWidget> children)
    {
      Row row = new Row();
      row.children = children;
      return row;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("wrap");
      writer.Write(this.wrap);
    }
  }
}
