// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EffectAnimation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct EffectAnimation : IBufferElementData
  {
    public uint m_DurationFrames;
    public AnimationCurve1 m_AnimationCurve;
  }
}
