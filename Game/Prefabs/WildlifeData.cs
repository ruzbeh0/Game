// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WildlifeData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct WildlifeData : IComponentData, IQueryTypeParameter
  {
    public Bounds1 m_TripLength;
    public Bounds1 m_IdleTime;
    public int2 m_GroupMemberCount;
  }
}
