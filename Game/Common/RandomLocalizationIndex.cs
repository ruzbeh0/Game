// Decompiled with JetBrains decompiler
// Type: Game.Common.RandomLocalizationIndex
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Common
{
  [InternalBufferCapacity(1)]
  public struct RandomLocalizationIndex : IBufferElementData, ISerializable
  {
    public static readonly RandomLocalizationIndex kNone = new RandomLocalizationIndex(-1);
    public int m_Index;

    public RandomLocalizationIndex(int index) => this.m_Index = index;

    public static void GenerateRandomIndices(
      DynamicBuffer<RandomLocalizationIndex> indices,
      DynamicBuffer<LocalizationCount> counts,
      ref Random random)
    {
      indices.ResizeUninitialized(counts.Length);
      for (int index = 0; index < counts.Length; ++index)
      {
        int count = counts[index].m_Count;
        indices[index] = new RandomLocalizationIndex(count > 0 ? random.NextInt(count) : -1);
      }
    }

    public static void EnsureValidRandomIndices(
      DynamicBuffer<RandomLocalizationIndex> indices,
      DynamicBuffer<LocalizationCount> counts,
      ref Random random)
    {
      if (indices.Length == counts.Length)
      {
        for (int index = 0; index < counts.Length; ++index)
        {
          int count = counts[index].m_Count;
          if (indices[index].m_Index < 0 || indices[index].m_Index >= count)
            indices[index] = new RandomLocalizationIndex(count > 0 ? random.NextInt(count) : -1);
        }
      }
      else
        RandomLocalizationIndex.GenerateRandomIndices(indices, counts, ref random);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Index);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Index);
    }
  }
}
