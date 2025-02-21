// Decompiled with JetBrains decompiler
// Type: Game.Net.PedestrianLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum PedestrianLaneFlags
  {
    Unsafe = 1,
    Crosswalk = 2,
    AllowMiddle = 4,
    AllowEnter = 8,
    SideConnection = 16, // 0x00000010
    ForbidTransitTraffic = 32, // 0x00000020
    OnWater = 64, // 0x00000040
  }
}
