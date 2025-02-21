// Decompiled with JetBrains decompiler
// Type: Game.Areas.GeometryFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Areas
{
  [Flags]
  public enum GeometryFlags
  {
    PhysicalGeometry = 1,
    CanOverrideObjects = 2,
    ProtectedArea = 4,
    ClearArea = 8,
    ClipTerrain = 16, // 0x00000010
    ShiftTerrain = 32, // 0x00000020
    OnWaterSurface = 64, // 0x00000040
    PseudoRandom = 128, // 0x00000080
  }
}
