// Decompiled with JetBrains decompiler
// Type: Game.Zones.ProcessEstimate
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Zones
{
  public struct ProcessEstimate : IBufferElementData
  {
    public float m_ProductionPerCell;
    public float m_BaseProfitabilityPerCell;
    public float m_WorkerProductionPerCell;
    public float m_LowEducationWeight;
    public Entity m_ProcessEntity;
  }
}
