// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TaxResourceInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.InGame
{
  public struct TaxResourceInfo : IJsonWritable
  {
    public string m_ID;
    public string m_Icon;

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin("taxation.TaxResourceInfo");
      writer.PropertyName("id");
      writer.Write(this.m_ID);
      writer.PropertyName("icon");
      writer.Write(this.m_Icon);
      writer.TypeEnd();
    }
  }
}
