// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportLineData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TransportLineData : IComponentData, IQueryTypeParameter
  {
    public Entity m_PathfindPrefab;
    public TransportType m_TransportType;
    public float m_DefaultVehicleInterval;
    public float m_DefaultUnbunchingFactor;
    public float m_StopDuration;
    public bool m_PassengerTransport;
    public bool m_CargoTransport;
    public Entity m_VehicleNotification;
  }
}
