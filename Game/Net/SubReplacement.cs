// Decompiled with JetBrains decompiler
// Type: Game.Net.SubReplacement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Tools;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  [InternalBufferCapacity(2)]
  public struct SubReplacement : IBufferElementData, IEquatable<SubReplacement>, ISerializable
  {
    public Entity m_Prefab;
    public SubReplacementType m_Type;
    public SubReplacementSide m_Side;
    public AgeMask m_AgeMask;

    public bool Equals(SubReplacement other)
    {
      return this.m_Prefab == other.m_Prefab && this.m_Type == other.m_Type && this.m_Side == other.m_Side && this.m_AgeMask == other.m_AgeMask;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Prefab);
      writer.Write((byte) this.m_Type);
      writer.Write((sbyte) this.m_Side);
      writer.Write((byte) this.m_AgeMask);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Prefab);
      byte num1;
      reader.Read(out num1);
      sbyte num2;
      reader.Read(out num2);
      byte num3;
      reader.Read(out num3);
      this.m_Type = (SubReplacementType) num1;
      this.m_Side = (SubReplacementSide) num2;
      this.m_AgeMask = (AgeMask) num3;
    }
  }
}
