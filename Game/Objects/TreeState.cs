// Decompiled with JetBrains decompiler
// Type: Game.Objects.TreeState
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Objects
{
  [Flags]
  public enum TreeState : byte
  {
    Teen = 1,
    Adult = 2,
    Elderly = 4,
    Dead = 8,
    Stump = 16, // 0x10
    Collected = 32, // 0x20
  }
}
