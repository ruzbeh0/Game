// Decompiled with JetBrains decompiler
// Type: Game.Tools.CoursePosFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Tools
{
  [Flags]
  public enum CoursePosFlags : uint
  {
    IsFirst = 1,
    IsLast = 2,
    HalfAlign = 4,
    IsParallel = 8,
    IsRight = 16, // 0x00000010
    IsLeft = 32, // 0x00000020
    IsFixed = 64, // 0x00000040
    FreeHeight = 128, // 0x00000080
    LeftTransition = 256, // 0x00000100
    RightTransition = 512, // 0x00000200
    ForceElevatedNode = 1024, // 0x00000400
    ForceElevatedEdge = 2048, // 0x00000800
    DisableMerge = 4096, // 0x00001000
    IsGrid = 8192, // 0x00002000
    DontCreate = 16384, // 0x00004000
  }
}
