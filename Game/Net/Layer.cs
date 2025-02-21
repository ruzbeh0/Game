// Decompiled with JetBrains decompiler
// Type: Game.Net.Layer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum Layer : uint
  {
    Road = 1,
    PowerlineLow = 2,
    PowerlineHigh = 4,
    WaterPipe = 8,
    SewagePipe = 16, // 0x00000010
    StormwaterPipe = 32, // 0x00000020
    TrainTrack = 64, // 0x00000040
    Pathway = 128, // 0x00000080
    Waterway = 256, // 0x00000100
    Taxiway = 512, // 0x00000200
    TramTrack = 1024, // 0x00000400
    SubwayTrack = 2048, // 0x00000800
    Fence = 4096, // 0x00001000
    MarkerPathway = 8192, // 0x00002000
    MarkerTaxiway = 16384, // 0x00004000
    PublicTransportRoad = 32768, // 0x00008000
    LaneEditor = 65536, // 0x00010000
    None = 0,
    All = 4294967295, // 0xFFFFFFFF
  }
}
