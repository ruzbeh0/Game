// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.MultilineText
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Widgets
{
  public class MultilineText : NamedWidget
  {
    public string icon { get; set; }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("icon");
      writer.Write(this.icon);
    }
  }
}
