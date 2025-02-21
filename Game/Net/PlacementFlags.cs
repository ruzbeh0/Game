// Decompiled with JetBrains decompiler
// Type: Game.Net.PlacementFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum PlacementFlags
  {
    None = 0,
    OnGround = 1,
    Floating = 2,
    IsUpgrade = 4,
    UpgradeOnly = 8,
    AllowParallel = 16, // 0x00000010
    NodeUpgrade = 32, // 0x00000020
    FlowLeft = 64, // 0x00000040
    FlowRight = 128, // 0x00000080
    UndergroundUpgrade = 256, // 0x00000100
  }
}
