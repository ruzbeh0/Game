// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ColorSyncFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering
{
  [Flags]
  public enum ColorSyncFlags : byte
  {
    None = 0,
    SameGroup = 1,
    SameIndex = 2,
    DifferentGroup = 4,
    DifferentIndex = 8,
    SyncRangeVariation = 16, // 0x10
  }
}
