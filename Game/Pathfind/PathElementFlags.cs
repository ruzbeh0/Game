// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathElementFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Pathfind
{
  [Flags]
  public enum PathElementFlags : byte
  {
    Secondary = 1,
    PathStart = 2,
    Action = 4,
    Return = 8,
    Reverse = 16, // 0x10
    WaitPosition = 32, // 0x20
    Leader = 64, // 0x40
  }
}
