// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EffectConditionFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum EffectConditionFlags
  {
    None = 0,
    Emergency = 1,
    Parked = 2,
    Operational = 4,
    OnFire = 8,
    Extinguishing = 16, // 0x00000010
    TakingOff = 32, // 0x00000020
    Landing = 64, // 0x00000040
    Flying = 128, // 0x00000080
    Stopped = 256, // 0x00000100
    Processing = 512, // 0x00000200
    Boarding = 1024, // 0x00000400
    Disaster = 2048, // 0x00000800
    Occurring = 4096, // 0x00001000
    Night = 8192, // 0x00002000
    Cold = 16384, // 0x00004000
    LightsOff = 32768, // 0x00008000
    MainLights = 65536, // 0x00010000
    ExtraLights = 131072, // 0x00020000
    WarningLights = 262144, // 0x00040000
    WorkLights = 524288, // 0x00080000
    Spillway = 1048576, // 0x00100000
    Collapsing = 2097152, // 0x00200000
    Last = Collapsing, // 0x00200000
  }
}
