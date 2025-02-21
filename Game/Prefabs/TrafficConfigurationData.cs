// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficConfigurationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TrafficConfigurationData : IComponentData, IQueryTypeParameter
  {
    public Entity m_BottleneckNotification;
    public Entity m_DeadEndNotification;
    public Entity m_RoadConnectionNotification;
    public Entity m_TrackConnectionNotification;
    public Entity m_CarConnectionNotification;
    public Entity m_ShipConnectionNotification;
    public Entity m_TrainConnectionNotification;
    public Entity m_PedestrianConnectionNotification;
  }
}
