// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetSectionFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum NetSectionFlags
  {
    Invert = 1,
    Median = 2,
    Left = 4,
    Right = 8,
    Underground = 16, // 0x00000010
    Overhead = 32, // 0x00000020
    FlipLanes = 64, // 0x00000040
    AlignCenter = 128, // 0x00000080
    FlipMesh = 256, // 0x00000100
    Hidden = 512, // 0x00000200
    HiddenSurface = 1024, // 0x00000400
    HiddenSide = 2048, // 0x00000800
    HiddenTop = 4096, // 0x00001000
    HiddenBottom = 8192, // 0x00002000
  }
}
