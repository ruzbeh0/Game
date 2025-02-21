// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SpectatorEventData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct SpectatorEventData : IComponentData, IQueryTypeParameter
  {
    public EventTargetType m_RandomSiteType;
    public float m_PreparationDuration;
    public float m_ActiveDuration;
    public float m_TerminationDuration;
  }
}
