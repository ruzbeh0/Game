// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SubMeshFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum SubMeshFlags : uint
  {
    RequireSafe = 1,
    RequireLevelCrossing = 2,
    RequireChild = 4,
    RequireTeen = 8,
    RequireAdult = 16, // 0x00000010
    RequireElderly = 32, // 0x00000020
    RequireDead = 64, // 0x00000040
    RequireStump = 128, // 0x00000080
    RequireEmpty = 256, // 0x00000100
    RequireFull = 512, // 0x00000200
    RequireEditor = 1024, // 0x00000400
    RequireClear = 2048, // 0x00000800
    RequireTrack = 4096, // 0x00001000
    IsStackStart = 8192, // 0x00002000
    IsStackMiddle = 16384, // 0x00004000
    IsStackEnd = 32768, // 0x00008000
    RequireLeftHandTraffic = 65536, // 0x00010000
    RequireRightHandTraffic = 131072, // 0x00020000
    DefaultMissingMesh = 262144, // 0x00040000
    RequirePartial1 = 524288, // 0x00080000
    RequirePartial2 = 1048576, // 0x00100000
    HasTransform = 2097152, // 0x00200000
    RequireForward = 4194304, // 0x00400000
    RequireBackward = 8388608, // 0x00800000
    OutlineOnly = 16777216, // 0x01000000
  }
}
