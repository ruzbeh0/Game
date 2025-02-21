// Decompiled with JetBrains decompiler
// Type: Game.Triggers.TargetType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Triggers
{
  [Flags]
  public enum TargetType
  {
    Nothing = 0,
    Building = 1,
    Citizen = 2,
    Policy = 4,
    Road = 8,
    ServiceBuilding = 16, // 0x00000010
  }
}
