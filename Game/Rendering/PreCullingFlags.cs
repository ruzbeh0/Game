// Decompiled with JetBrains decompiler
// Type: Game.Rendering.PreCullingFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering
{
  [Flags]
  public enum PreCullingFlags : uint
  {
    PassedCulling = 1,
    NearCamera = 2,
    NearCameraUpdated = 4,
    Deleted = 8,
    Updated = 16, // 0x00000010
    Created = 32, // 0x00000020
    Applied = 64, // 0x00000040
    BatchesUpdated = 128, // 0x00000080
    Temp = 256, // 0x00000100
    FadeContainer = 512, // 0x00000200
    Object = 1024, // 0x00000400
    Net = 2048, // 0x00000800
    Lane = 4096, // 0x00001000
    Zone = 8192, // 0x00002000
    InfoviewColor = 16384, // 0x00004000
    BuildingState = 32768, // 0x00008000
    TreeGrowth = 65536, // 0x00010000
    LaneCondition = 131072, // 0x00020000
    InterpolatedTransform = 262144, // 0x00040000
    Animated = 524288, // 0x00080000
    Skeleton = 1048576, // 0x00100000
    Emissive = 2097152, // 0x00200000
    VehicleLayout = 4194304, // 0x00400000
    EffectInstances = 8388608, // 0x00800000
    Relative = 16777216, // 0x01000000
    SurfaceState = 33554432, // 0x02000000
    SurfaceDamage = 67108864, // 0x04000000
    ColorsUpdated = 134217728, // 0x08000000
    SmoothColor = 268435456, // 0x10000000
  }
}
