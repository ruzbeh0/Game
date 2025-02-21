// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrainEngineData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct TrainEngineData : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public int2 m_Count;

    public TrainEngineData(int minCount, int maxCount)
    {
      this.m_Count = new int2(minCount, maxCount);
    }
  }
}
