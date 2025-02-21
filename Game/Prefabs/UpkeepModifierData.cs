// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UpkeepModifierData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct UpkeepModifierData : 
    IBufferElementData,
    ICombineBuffer<UpkeepModifierData>,
    ISerializable
  {
    public Resource m_Resource;
    public float m_Multiplier;

    public float Transform(float upkeep)
    {
      upkeep *= this.m_Multiplier;
      return upkeep;
    }

    public void Combine(NativeList<UpkeepModifierData> result)
    {
      for (int index = 0; index < result.Length; ++index)
      {
        ref UpkeepModifierData local = ref result.ElementAt(index);
        if (local.m_Resource == this.m_Resource)
        {
          local.m_Multiplier *= this.m_Multiplier;
          return;
        }
      }
      result.Add(in this);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Multiplier);
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Resource));
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Multiplier);
      if (reader.context.version < Version.upkeepModifierRelative)
        reader.Read(out float _);
      sbyte index;
      reader.Read(out index);
      this.m_Resource = EconomyUtils.GetResource((int) index);
    }
  }
}
