// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LocalModifierData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct LocalModifierData : IBufferElementData, ISerializable
  {
    public LocalModifierType m_Type;
    public ModifierValueMode m_Mode;
    public ModifierRadiusCombineMode m_RadiusCombineMode;
    public Bounds1 m_Delta;
    public Bounds1 m_Radius;

    public LocalModifierData(
      LocalModifierType type,
      ModifierValueMode mode,
      ModifierRadiusCombineMode radiusMode,
      Bounds1 delta,
      Bounds1 radius)
    {
      this.m_Type = type;
      this.m_Mode = mode;
      this.m_RadiusCombineMode = radiusMode;
      this.m_Delta = delta;
      this.m_Radius = radius;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Delta.min);
      writer.Write(this.m_Delta.max);
      writer.Write(this.m_Radius.min);
      writer.Write(this.m_Radius.max);
      writer.Write((byte) this.m_Type);
      writer.Write((byte) this.m_Mode);
      writer.Write((byte) this.m_RadiusCombineMode);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Delta.min);
      reader.Read(out this.m_Delta.max);
      reader.Read(out this.m_Radius.min);
      reader.Read(out this.m_Radius.max);
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      byte num3;
      reader.Read(out num3);
      this.m_Type = (LocalModifierType) num1;
      this.m_Mode = (ModifierValueMode) num2;
      this.m_RadiusCombineMode = (ModifierRadiusCombineMode) num3;
    }
  }
}
