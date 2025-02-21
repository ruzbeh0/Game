// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NaturalResourceCell
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct NaturalResourceCell : IStrideSerializable, ISerializable
  {
    public NaturalResourceAmount m_Fertility;
    public NaturalResourceAmount m_Ore;
    public NaturalResourceAmount m_Oil;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write<NaturalResourceAmount>(this.m_Fertility);
      writer.Write<NaturalResourceAmount>(this.m_Ore);
      writer.Write<NaturalResourceAmount>(this.m_Oil);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.naturalResourceUsedAmounts)
      {
        reader.Read<NaturalResourceAmount>(out this.m_Fertility);
        reader.Read<NaturalResourceAmount>(out this.m_Ore);
        reader.Read<NaturalResourceAmount>(out this.m_Oil);
      }
      else
      {
        reader.Read(out this.m_Fertility.m_Base);
        reader.Read(out this.m_Ore.m_Base);
        reader.Read(out this.m_Oil.m_Base);
        this.m_Fertility.m_Base = (ushort) ((int) this.m_Fertility.m_Base * 10000 / (int) ushort.MaxValue);
        this.m_Ore.m_Base = (ushort) ((int) this.m_Ore.m_Base * 10000 / (int) ushort.MaxValue);
        this.m_Oil.m_Base = (ushort) ((int) this.m_Oil.m_Base * 10000 / (int) ushort.MaxValue);
      }
    }

    public int GetStride(Context context)
    {
      return this.m_Fertility.GetStride(context) + this.m_Ore.GetStride(context) + this.m_Oil.GetStride(context);
    }

    public float3 GetBaseResources()
    {
      return new float3((float) this.m_Fertility.m_Base, (float) this.m_Ore.m_Base, (float) this.m_Oil.m_Base);
    }

    public float3 GetUsedResources()
    {
      return new float3((float) this.m_Fertility.m_Used, (float) this.m_Ore.m_Used, (float) this.m_Oil.m_Used);
    }
  }
}
