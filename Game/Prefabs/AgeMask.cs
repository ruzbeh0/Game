// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AgeMask
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum AgeMask : byte
  {
    Child = 1,
    Teen = 2,
    Adult = 4,
    Elderly = 8,
    Any = Elderly | Adult | Teen | Child, // 0x0F
  }
}
