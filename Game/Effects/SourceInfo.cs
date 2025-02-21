// Decompiled with JetBrains decompiler
// Type: Game.Effects.SourceInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;

#nullable disable
namespace Game.Effects
{
  public struct SourceInfo : IEquatable<SourceInfo>
  {
    public Entity m_Entity;
    public int m_EffectIndex;

    public SourceInfo(Entity entity, int effectIndex)
    {
      this.m_Entity = entity;
      this.m_EffectIndex = effectIndex;
    }

    public bool Equals(SourceInfo other)
    {
      return this.m_Entity == other.m_Entity && this.m_EffectIndex == other.m_EffectIndex;
    }

    public override int GetHashCode() => this.m_Entity.GetHashCode() ^ this.m_EffectIndex;
  }
}
