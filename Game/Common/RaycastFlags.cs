// Decompiled with JetBrains decompiler
// Type: Game.Common.RaycastFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Common
{
  [Flags]
  public enum RaycastFlags : uint
  {
    DebugDisable = 1,
    UIDisable = 2,
    ToolDisable = 4,
    FreeCameraDisable = 8,
    ElevateOffset = 16, // 0x00000010
    SubElements = 32, // 0x00000020
    Placeholders = 64, // 0x00000040
    Markers = 128, // 0x00000080
    NoMainElements = 256, // 0x00000100
    UpgradeIsMain = 512, // 0x00000200
    OutsideConnections = 1024, // 0x00000400
    Outside = 2048, // 0x00000800
    Cargo = 4096, // 0x00001000
    Passenger = 8192, // 0x00002000
    Decals = 16384, // 0x00004000
    EditorContainers = 32768, // 0x00008000
    SubBuildings = 65536, // 0x00010000
    PartialSurface = 131072, // 0x00020000
    BuildingLots = 262144, // 0x00040000
    IgnoreSecondary = 524288, // 0x00080000
  }
}
