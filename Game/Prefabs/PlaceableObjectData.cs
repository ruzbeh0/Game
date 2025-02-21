// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceableObjectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Game.Objects;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct PlaceableObjectData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_PlacementOffset;
    public uint m_ConstructionCost;
    public int m_XPReward;
    public byte m_DefaultProbability;
    public RotationSymmetry m_RotationSymmetry;
    public SubReplacementType m_SubReplacementType;
    public Game.Objects.PlacementFlags m_Flags;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_PlacementOffset);
      reader.Read(out this.m_ConstructionCost);
      reader.Read(out this.m_XPReward);
      uint num;
      reader.Read(out num);
      this.m_Flags = (Game.Objects.PlacementFlags) ((int) num & -32769);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PlacementOffset);
      writer.Write(this.m_ConstructionCost);
      writer.Write(this.m_XPReward);
      writer.Write((uint) this.m_Flags);
    }
  }
}
