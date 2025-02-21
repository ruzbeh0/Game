// Decompiled with JetBrains decompiler
// Type: Game.Common.RaycastResult
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Common
{
  public struct RaycastResult : IAccumulable<RaycastResult>
  {
    public RaycastHit m_Hit;
    public Entity m_Owner;

    public void Accumulate(RaycastResult other)
    {
      if (!(this.m_Owner == Entity.Null) && (!(other.m_Owner != Entity.Null) || (double) other.m_Hit.m_NormalizedDistance >= (double) this.m_Hit.m_NormalizedDistance && ((double) other.m_Hit.m_NormalizedDistance != (double) this.m_Hit.m_NormalizedDistance || other.m_Hit.m_HitEntity.Index >= this.m_Hit.m_HitEntity.Index)))
        return;
      this = other;
    }
  }
}
