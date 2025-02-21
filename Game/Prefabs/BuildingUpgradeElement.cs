// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingUpgradeElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct BuildingUpgradeElement : IBufferElementData, IEquatable<BuildingUpgradeElement>
  {
    public Entity m_Upgrade;

    public BuildingUpgradeElement(Entity upgrade) => this.m_Upgrade = upgrade;

    public bool Equals(BuildingUpgradeElement other) => this.m_Upgrade.Equals(other.m_Upgrade);

    public override int GetHashCode() => this.m_Upgrade.GetHashCode();
  }
}
