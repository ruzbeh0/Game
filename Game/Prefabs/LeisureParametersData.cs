// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LeisureParametersData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct LeisureParametersData : IComponentData, IQueryTypeParameter
  {
    public Entity m_TravelingPrefab;
    public Entity m_AttractionPrefab;
    public Entity m_SightseeingPrefab;
    public int m_LeisureRandomFactor;
    public int m_TouristLodgingConsumePerDay;
    public int m_TouristServiceConsumePerDay;

    public Entity GetPrefab(LeisureType type)
    {
      switch (type)
      {
        case LeisureType.Travel:
          return this.m_TravelingPrefab;
        case LeisureType.Attractions:
          return this.m_AttractionPrefab;
        case LeisureType.Sightseeing:
          return this.m_SightseeingPrefab;
        default:
          return new Entity();
      }
    }
  }
}
