// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.RuleFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Pathfind
{
  [Flags]
  public enum RuleFlags : byte
  {
    HasBlockage = 1,
    ForbidCombustionEngines = 2,
    ForbidTransitTraffic = 4,
    ForbidHeavyTraffic = 8,
    ForbidPrivateTraffic = 16, // 0x10
    ForbidSlowTraffic = 32, // 0x20
  }
}
