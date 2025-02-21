// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PostVanRequestFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Simulation
{
  [Flags]
  public enum PostVanRequestFlags : byte
  {
    Deliver = 1,
    Collect = 2,
    BuildingTarget = 4,
    MailBoxTarget = 8,
  }
}
