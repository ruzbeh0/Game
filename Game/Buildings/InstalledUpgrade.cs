// Decompiled with JetBrains decompiler
// Type: Game.Buildings.InstalledUpgrade
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  [InternalBufferCapacity(0)]
  public struct InstalledUpgrade : 
    IBufferElementData,
    IEquatable<InstalledUpgrade>,
    IEmptySerializable
  {
    public Entity m_Upgrade;
    public uint m_OptionMask;

    public InstalledUpgrade(Entity upgrade, uint optionMask)
    {
      this.m_Upgrade = upgrade;
      this.m_OptionMask = optionMask;
    }

    public bool Equals(InstalledUpgrade other) => this.m_Upgrade.Equals(other.m_Upgrade);

    public override int GetHashCode() => this.m_Upgrade.GetHashCode();

    public static implicit operator Entity(InstalledUpgrade upgrade) => upgrade.m_Upgrade;
  }
}
