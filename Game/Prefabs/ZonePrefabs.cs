// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZonePrefabs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Zones;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ZonePrefabs
  {
    private NativeArray<Entity> m_ZonePrefabs;

    public ZonePrefabs(NativeArray<Entity> zonePrefabs) => this.m_ZonePrefabs = zonePrefabs;

    public Entity this[ZoneType type] => this.m_ZonePrefabs[(int) type.m_Index];
  }
}
