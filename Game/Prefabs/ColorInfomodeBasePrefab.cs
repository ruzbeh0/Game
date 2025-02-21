// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ColorInfomodeBasePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public abstract class ColorInfomodeBasePrefab : InfomodeBasePrefab, IColorInfomode
  {
    public Color m_Color;

    public Color color => this.m_Color;

    public override void GetColors(
      out Color color0,
      out Color color1,
      out Color color2,
      out float steps,
      out float speed,
      out float tiling,
      out float fill)
    {
      color0 = this.m_Color;
      color1 = this.m_Color;
      color2 = this.m_Color;
      steps = 1f;
      speed = 0.0f;
      tiling = 0.0f;
      fill = 0.0f;
    }
  }
}
