// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectRequirementFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum ObjectRequirementFlags : ushort
  {
    Renter = 1,
    Children = 2,
    Snow = 4,
    Teens = 8,
    GoodWealth = 16, // 0x0010
    Dogs = 32, // 0x0020
    Homeless = 64, // 0x0040
  }
}
