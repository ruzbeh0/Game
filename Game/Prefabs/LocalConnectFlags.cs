// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LocalConnectFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum LocalConnectFlags : uint
  {
    ExplicitNodes = 1,
    KeepOpen = 2,
    RequireDeadend = 4,
    ChooseBest = 8,
    ChooseSides = 16, // 0x00000010
  }
}
