// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetPieceFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum NetPieceFlags
  {
    PreserveShape = 1,
    BlockTraffic = 2,
    BlockCrosswalk = 4,
    Surface = 8,
    DisableTiling = 16, // 0x00000010
    LowerBottomToTerrain = 32, // 0x00000020
    AsymmetricMeshX = 64, // 0x00000040
    AsymmetricMeshZ = 128, // 0x00000080
    HasMesh = 256, // 0x00000100
    Side = 512, // 0x00000200
    RaiseTopToTerrain = 1024, // 0x00000400
    SmoothTopNormal = 2048, // 0x00000800
    SkipBottomHalf = 4096, // 0x00001000
    Top = 8192, // 0x00002000
    Bottom = 16384, // 0x00004000
  }
}
