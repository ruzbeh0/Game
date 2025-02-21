// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CullingData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public struct CullingData
  {
    public Bounds3 m_Bounds;
    public uint m_LodData1;
    public uint m_LodData2;

    public int4 lodRange
    {
      get
      {
        return new int4((int) this.m_LodData1, (int) (this.m_LodData1 >> 5), (int) (this.m_LodData1 >> 10), (int) (this.m_LodData1 >> 15)) & 31;
      }
      set
      {
        this.m_LodData1 = (uint) ((int) this.m_LodData1 & -1048576 | value.x | value.y << 5 | value.z << 10 | value.w << 15);
      }
    }

    public int4 lodFade
    {
      get
      {
        return new int4((int) this.m_LodData2 & (int) byte.MaxValue, (int) (this.m_LodData2 >> 8) & (int) byte.MaxValue, (int) (this.m_LodData2 >> 16) & (int) byte.MaxValue, (int) (this.m_LodData2 >> 24));
      }
      set => this.m_LodData2 = (uint) (value.x | value.y << 8 | value.z << 16 | value.w << 24);
    }

    public int lodOffset
    {
      get => (int) this.m_LodData1 >> 23;
      set => this.m_LodData1 = (uint) ((int) this.m_LodData1 & 8388607 | value << 23);
    }

    public bool isHidden
    {
      get => (this.m_LodData1 & 1048576U) > 0U;
      set => this.m_LodData1 = value ? this.m_LodData1 | 1048576U : this.m_LodData1 & 4293918719U;
    }

    public bool isFading
    {
      get => (this.m_LodData1 & 2097152U) > 0U;
      set => this.m_LodData1 = value ? this.m_LodData1 | 2097152U : this.m_LodData1 & 4292870143U;
    }
  }
}
