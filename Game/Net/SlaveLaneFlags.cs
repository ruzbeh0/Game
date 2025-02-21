// Decompiled with JetBrains decompiler
// Type: Game.Net.SlaveLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum SlaveLaneFlags
  {
    AllowChange = 1,
    StartingLane = 2,
    EndingLane = 4,
    MultipleLanes = 8,
    MergingLane = 16, // 0x00000010
    OpenStartLeft = 32, // 0x00000020
    OpenStartRight = 64, // 0x00000040
    OpenEndLeft = 128, // 0x00000080
    OpenEndRight = 256, // 0x00000100
    SplitLeft = 512, // 0x00000200
    SplitRight = 1024, // 0x00000400
    MiddleStart = 2048, // 0x00000800
    MiddleEnd = 4096, // 0x00001000
    MergeLeft = 8192, // 0x00002000
    MergeRight = 16384, // 0x00004000
  }
}
