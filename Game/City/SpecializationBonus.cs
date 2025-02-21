// Decompiled with JetBrains decompiler
// Type: Game.City.SpecializationBonus
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.City
{
  public struct SpecializationBonus : IBufferElementData, IDefaultSerializable, ISerializable
  {
    public int m_Value;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Value);
    }

    public float GetBonus(float maxBonus, int coefficient)
    {
      return maxBonus * (float) this.m_Value / (float) (this.m_Value + coefficient);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Value);
    }

    public void SetDefaults(Context context) => this.m_Value = 0;
  }
}
