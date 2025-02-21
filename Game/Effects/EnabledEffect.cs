// Decompiled with JetBrains decompiler
// Type: Game.Effects.EnabledEffect
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Effects
{
  [FormerlySerializedAs("Game.Effects.EffectOwner, Game")]
  [InternalBufferCapacity(2)]
  public struct EnabledEffect : IBufferElementData, IEmptySerializable
  {
    public int m_EffectIndex;
    public int m_EnabledIndex;
  }
}
