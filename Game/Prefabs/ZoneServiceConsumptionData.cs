// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneServiceConsumptionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ZoneServiceConsumptionData : IComponentData, IQueryTypeParameter
  {
    public float m_Upkeep;
    public float m_ElectricityConsumption;
    public float m_WaterConsumption;
    public float m_GarbageAccumulation;
    public float m_TelecomNeed;
  }
}
