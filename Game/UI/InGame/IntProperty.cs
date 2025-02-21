// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.IntProperty
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.InGame
{
  public struct IntProperty : IJsonWritable
  {
    public string labelId;
    public int value;
    public string unit;
    public bool signed;
    public string icon;
    public string valueIcon;

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin("Game.UI.Common.NumberProperty");
      writer.PropertyName("labelId");
      writer.Write(this.labelId);
      writer.PropertyName("value");
      writer.Write(this.value);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.PropertyName("signed");
      writer.Write(this.signed);
      writer.PropertyName("icon");
      writer.Write(this.icon);
      writer.PropertyName("valueIcon");
      writer.Write(this.valueIcon);
      writer.TypeEnd();
    }
  }
}
