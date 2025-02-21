// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ExtractorParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ExtractorParameterData : IComponentData, IQueryTypeParameter
  {
    public float m_FertilityConsumption;
    public float m_OreConsumption;
    public float m_ForestConsumption;
    public float m_OilConsumption;
    public float m_FullFertility;
    public float m_FullOre;
    public float m_FullOil;
  }
}
