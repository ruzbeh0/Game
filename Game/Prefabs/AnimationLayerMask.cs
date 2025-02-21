// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AnimationLayerMask
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Prefabs
{
  public struct AnimationLayerMask
  {
    public uint m_Mask;

    public AnimationLayerMask(AnimationLayer layer)
    {
      if (layer == AnimationLayer.None)
        this.m_Mask = 0U;
      else
        this.m_Mask = 1U << (int) (layer - 1 & (AnimationLayer) 31);
    }
  }
}
