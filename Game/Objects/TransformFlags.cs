// Decompiled with JetBrains decompiler
// Type: Game.Objects.TransformFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Objects
{
  [Flags]
  public enum TransformFlags : uint
  {
    MainLights = 1,
    ExtraLights = 2,
    TurningLeft = 4,
    TurningRight = 8,
    Braking = 16, // 0x00000010
    RearLights = 32, // 0x00000020
    BoardingLeft = 64, // 0x00000040
    BoardingRight = 128, // 0x00000080
    InteriorLights = 256, // 0x00000100
    Pantograph = 512, // 0x00000200
    WarningLights = 1024, // 0x00000400
    Reversing = 2048, // 0x00000800
    WorkLights = 4096, // 0x00001000
    SignalAnimation1 = 8192, // 0x00002000
    SignalAnimation2 = 16384, // 0x00004000
    TakingOff = 32768, // 0x00008000
    Landing = 65536, // 0x00010000
    Flying = 131072, // 0x00020000
  }
}
