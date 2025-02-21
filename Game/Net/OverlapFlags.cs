// Decompiled with JetBrains decompiler
// Type: Game.Net.OverlapFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum OverlapFlags : ushort
  {
    MergeStart = 1,
    MergeEnd = 2,
    OverlapLeft = 4,
    OverlapRight = 8,
    MergeMiddleStart = 16, // 0x0010
    MergeMiddleEnd = 32, // 0x0020
    Unsafe = 64, // 0x0040
    Road = 128, // 0x0080
    Track = 256, // 0x0100
    MergeFlip = 512, // 0x0200
  }
}
