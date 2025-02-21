// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TilePurchaseErrorFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Simulation
{
  [Flags]
  public enum TilePurchaseErrorFlags
  {
    None = 0,
    NoCurrentlyAvailable = 1,
    NoAvailable = 2,
    NoSelection = 4,
    InsufficientFunds = 8,
    InsufficientPermits = 16, // 0x00000010
  }
}
