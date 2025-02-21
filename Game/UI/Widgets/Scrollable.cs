// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.Scrollable
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public class Scrollable : LayoutContainer
  {
    public Direction direction { get; set; }

    public Scrollable() => this.flex = FlexLayout.Fill;

    public static Scrollable WithChildren(IList<IWidget> children)
    {
      Scrollable scrollable = new Scrollable();
      scrollable.children = children;
      return scrollable;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("direction");
      writer.Write((int) this.direction);
    }
  }
}
