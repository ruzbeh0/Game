// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.OutsideConnectionTransferType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum OutsideConnectionTransferType
  {
    None = 0,
    Road = 1,
    Train = 2,
    Air = 4,
    Ship = 16, // 0x00000010
    Last = 32, // 0x00000020
  }
}
