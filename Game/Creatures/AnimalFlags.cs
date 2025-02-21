// Decompiled with JetBrains decompiler
// Type: Game.Creatures.AnimalFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Creatures
{
  [Flags]
  public enum AnimalFlags : uint
  {
    Roaming = 1,
    SwimmingTarget = 2,
    FlyingTarget = 4,
  }
}
