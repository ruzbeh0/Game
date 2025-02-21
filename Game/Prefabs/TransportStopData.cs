// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportStopData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TransportStopData : IComponentData, IQueryTypeParameter
  {
    public float m_ComfortFactor;
    public float m_LoadingFactor;
    public float m_AccessDistance;
    public float m_BoardingTime;
    public TransportType m_TransportType;
    public bool m_PassengerTransport;
    public bool m_CargoTransport;
  }
}
