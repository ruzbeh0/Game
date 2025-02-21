// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SignalGroupMask
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum SignalGroupMask : ushort
  {
    SignalGroup1 = 1,
    SignalGroup2 = 2,
    SignalGroup3 = 4,
    SignalGroup4 = 8,
    SignalGroup5 = 16, // 0x0010
    SignalGroup6 = 32, // 0x0020
    SignalGroup7 = 64, // 0x0040
    SignalGroup8 = 128, // 0x0080
    SignalGroup9 = 256, // 0x0100
    SignalGroup10 = 512, // 0x0200
    SignalGroup11 = 1024, // 0x0400
  }
}
