// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneBuiltRequirementData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Zones;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ZoneBuiltRequirementData : IComponentData, IQueryTypeParameter
  {
    public Entity m_RequiredTheme;
    public Entity m_RequiredZone;
    public int m_MinimumSquares;
    public int m_MinimumCount;
    public AreaType m_RequiredType;
    public byte m_MinimumLevel;
  }
}
