// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.EducationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct EducationData : IJsonWritable
  {
    public int uneducated { get; }

    public int poorlyEducated { get; }

    public int educated { get; }

    public int wellEducated { get; }

    public int highlyEducated { get; }

    public int total { get; }

    public EducationData(
      int uneducated,
      int poorlyEducated,
      int educated,
      int wellEducated,
      int highlyEducated)
    {
      this.uneducated = uneducated;
      this.poorlyEducated = poorlyEducated;
      this.educated = educated;
      this.wellEducated = wellEducated;
      this.highlyEducated = highlyEducated;
      this.total = uneducated + poorlyEducated + educated + wellEducated + highlyEducated;
    }

    public static EducationData operator +(EducationData left, EducationData right)
    {
      return new EducationData(left.uneducated + right.uneducated, left.poorlyEducated + right.poorlyEducated, left.educated + right.educated, left.wellEducated + right.wellEducated, left.highlyEducated + right.highlyEducated);
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin("selectedInfo.ChartData");
      writer.PropertyName("values");
      writer.ArrayBegin(5U);
      writer.Write(this.uneducated);
      writer.Write(this.poorlyEducated);
      writer.Write(this.educated);
      writer.Write(this.wellEducated);
      writer.Write(this.highlyEducated);
      writer.ArrayEnd();
      writer.PropertyName("total");
      writer.Write(this.total);
      writer.TypeEnd();
    }
  }
}
