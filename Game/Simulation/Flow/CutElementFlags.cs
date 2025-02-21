// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.CutElementFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Simulation.Flow
{
  [Flags]
  public enum CutElementFlags
  {
    None = 0,
    Created = 1,
    Admissible = 2,
    Changed = 4,
    Deleted = 8,
  }
}
