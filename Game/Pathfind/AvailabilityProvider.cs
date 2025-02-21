// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.AvailabilityProvider
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct AvailabilityProvider
  {
    public Entity m_Provider;
    public float m_Capacity;
    public float m_Cost;

    public AvailabilityProvider(Entity provider, float capacity, float cost)
    {
      this.m_Provider = provider;
      this.m_Capacity = capacity;
      this.m_Cost = cost;
    }
  }
}
