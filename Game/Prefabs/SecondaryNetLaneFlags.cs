// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SecondaryNetLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum SecondaryNetLaneFlags
  {
    Left = 1,
    Right = 2,
    OneSided = 4,
    RequireSafe = 8,
    CanFlipSides = 16, // 0x00000010
    RequireParallel = 32, // 0x00000020
    RequireOpposite = 64, // 0x00000040
    RequireSingle = 128, // 0x00000080
    RequireMultiple = 256, // 0x00000100
    RequireAllowPassing = 512, // 0x00000200
    RequireForbidPassing = 1024, // 0x00000400
    RequireMerge = 2048, // 0x00000800
    RequireContinue = 4096, // 0x00001000
    RequireStop = 8192, // 0x00002000
    Crossing = 16384, // 0x00004000
    RequireUnsafe = 32768, // 0x00008000
    RequirePavement = 65536, // 0x00010000
    RequireYield = 131072, // 0x00020000
    DuplicateSides = 262144, // 0x00040000
    RequireSafeMaster = 524288, // 0x00080000
  }
}
