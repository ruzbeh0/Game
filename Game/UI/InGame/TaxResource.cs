// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TaxResource
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.InGame
{
  public struct TaxResource : IJsonReadable, IJsonWritable
  {
    public int m_Resource;
    public int m_AreaType;

    public void Read(IJsonReader reader)
    {
      long num = (long) reader.ReadMapBegin();
      reader.ReadProperty("resource");
      reader.Read(out this.m_Resource);
      reader.ReadProperty("area");
      reader.Read(out this.m_AreaType);
      reader.ReadMapEnd();
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("resource");
      writer.Write(this.m_Resource);
      writer.PropertyName("area");
      writer.Write(this.m_AreaType);
      writer.TypeEnd();
    }
  }
}
