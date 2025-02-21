// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ColorField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class ColorField : Field<Color>
  {
    public bool hdr { get; set; }

    public bool showAlpha { get; set; }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("hdr");
      writer.Write(this.hdr);
      writer.PropertyName("showAlpha");
      writer.Write(this.showAlpha);
    }
  }
}
