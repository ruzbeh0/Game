// Decompiled with JetBrains decompiler
// Type: Game.Net.ConnectionLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum ConnectionLaneFlags
  {
    Start = 1,
    Distance = 2,
    Outside = 4,
    SecondaryStart = 8,
    SecondaryEnd = 16, // 0x00000010
    Road = 32, // 0x00000020
    Track = 64, // 0x00000040
    Pedestrian = 128, // 0x00000080
    Parking = 256, // 0x00000100
    AllowMiddle = 512, // 0x00000200
    AllowCargo = 1024, // 0x00000400
    Airway = 2048, // 0x00000800
    Inside = 4096, // 0x00001000
    Area = 8192, // 0x00002000
    Disabled = 16384, // 0x00004000
    AllowEnter = 32768, // 0x00008000
    AllowExit = 65536, // 0x00010000
  }
}
