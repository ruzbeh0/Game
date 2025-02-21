// Decompiled with JetBrains decompiler
// Type: Game.City.Tourism
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.City
{
  public struct Tourism : IComponentData, IQueryTypeParameter, IDefaultSerializable, ISerializable
  {
    public int m_CurrentTourists;
    public int m_AverageTourists;
    public int m_Attractiveness;
    public int2 m_Lodging;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_CurrentTourists);
      reader.Read(out this.m_AverageTourists);
      reader.Read(out this.m_Attractiveness);
      reader.Read(out this.m_Lodging);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_CurrentTourists);
      writer.Write(this.m_AverageTourists);
      writer.Write(this.m_Attractiveness);
      writer.Write(this.m_Lodging);
    }

    public void SetDefaults(Context context)
    {
      this.m_CurrentTourists = 0;
      this.m_AverageTourists = 0;
      this.m_Attractiveness = 0;
      this.m_Lodging = new int2();
    }
  }
}
