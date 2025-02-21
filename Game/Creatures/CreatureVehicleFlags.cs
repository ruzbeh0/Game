// Decompiled with JetBrains decompiler
// Type: Game.Creatures.CreatureVehicleFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Creatures
{
  [Flags]
  public enum CreatureVehicleFlags : uint
  {
    Ready = 1,
    Leader = 2,
    Driver = 4,
    Entering = 8,
    Exiting = 16, // 0x00000010
  }
}
