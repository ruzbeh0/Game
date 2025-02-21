// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ColorVariation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(1)]
  public struct ColorVariation : IBufferElementData
  {
    public ColorSet m_ColorSet;
    public ColorGroupID m_GroupID;
    public ColorSyncFlags m_SyncFlags;
    public ColorSourceType m_ColorSourceType;
    public byte m_Probability;
    public sbyte m_ExternalChannel0;
    public sbyte m_ExternalChannel1;
    public sbyte m_ExternalChannel2;
    public byte m_HueRange;
    public byte m_SaturationRange;
    public byte m_ValueRange;
    public byte m_AlphaRange0;
    public byte m_AlphaRange1;
    public byte m_AlphaRange2;

    public bool hasExternalChannels
    {
      get
      {
        return this.m_ExternalChannel0 >= (sbyte) 0 | this.m_ExternalChannel1 >= (sbyte) 0 | this.m_ExternalChannel2 >= (sbyte) 0;
      }
    }

    public bool hasVariationRanges
    {
      get
      {
        return this.m_HueRange > (byte) 0 | this.m_SaturationRange > (byte) 0 | this.m_ValueRange > (byte) 0;
      }
    }

    public bool hasAlphaRanges
    {
      get
      {
        return this.m_AlphaRange0 > (byte) 0 | this.m_AlphaRange1 > (byte) 0 | this.m_AlphaRange2 > (byte) 0;
      }
    }

    public int GetExternalChannelIndex(int colorIndex)
    {
      switch (colorIndex)
      {
        case 0:
          return (int) this.m_ExternalChannel0;
        case 1:
          return (int) this.m_ExternalChannel1;
        case 2:
          return (int) this.m_ExternalChannel2;
        default:
          return -1;
      }
    }

    public void SetExternalChannelIndex(int colorIndex, int channelIndex)
    {
      switch (colorIndex)
      {
        case 0:
          this.m_ExternalChannel0 = (sbyte) channelIndex;
          break;
        case 1:
          this.m_ExternalChannel1 = (sbyte) channelIndex;
          break;
        case 2:
          this.m_ExternalChannel2 = (sbyte) channelIndex;
          break;
      }
    }
  }
}
