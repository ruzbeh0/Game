// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering
{
  [Flags]
  public enum BatchFlags
  {
    MotionVectors = 1,
    Animated = 2,
    Bones = 4,
    Emissive = 8,
    ColorMask = 16, // 0x00000010
    Outline = 32, // 0x00000020
    InfoviewColor = 64, // 0x00000040
    Lod = 128, // 0x00000080
    Node = 256, // 0x00000100
    Roundabout = 512, // 0x00000200
    Extended1 = 1024, // 0x00000400
    Extended2 = 2048, // 0x00000800
    Extended3 = 4096, // 0x00001000
    LodFade = 8192, // 0x00002000
    InfoviewFlow = 16384, // 0x00004000
    Hanging = 32768, // 0x00008000
    BlendWeights = 65536, // 0x00010000
    SurfaceState = 131072, // 0x00020000
  }
}
