// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.AgeData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct AgeData : IJsonWritable
  {
    public int children { get; }

    public int teens { get; }

    public int adults { get; }

    public int elders { get; }

    public AgeData(int children, int teens, int adults, int elders)
    {
      this.children = children;
      this.teens = teens;
      this.adults = adults;
      this.elders = elders;
    }

    public static AgeData operator +(AgeData left, AgeData right)
    {
      return new AgeData(left.children + right.children, left.teens + right.teens, left.adults + right.adults, left.elders + right.elders);
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("values");
      writer.ArrayBegin(4U);
      writer.Write(this.children);
      writer.Write(this.teens);
      writer.Write(this.adults);
      writer.Write(this.elders);
      writer.ArrayEnd();
      writer.PropertyName("total");
      writer.Write(this.children + this.teens + this.adults + this.elders);
      writer.TypeEnd();
    }
  }
}
