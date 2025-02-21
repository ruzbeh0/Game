// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HealthProblemFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Citizens
{
  [Flags]
  public enum HealthProblemFlags : byte
  {
    None = 0,
    Sick = 1,
    Dead = 2,
    Injured = 4,
    RequireTransport = 8,
    InDanger = 16, // 0x10
    Trapped = 32, // 0x20
    NoHealthcare = 64, // 0x40
  }
}
