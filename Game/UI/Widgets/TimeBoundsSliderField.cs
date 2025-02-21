// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.TimeBoundsSliderField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Widgets
{
  public class TimeBoundsSliderField : TimeField<Bounds1>
  {
    public bool allowMinGreaterMax { get; set; }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("allowMinGreaterMax");
      writer.Write(this.allowMinGreaterMax);
    }
  }
}
