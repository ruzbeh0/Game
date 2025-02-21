// Decompiled with JetBrains decompiler
// Type: Game.Rendering.DecalLayers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering
{
  [Flags]
  public enum DecalLayers
  {
    Terrain = 1,
    Roads = 2,
    Buildings = 4,
    Vehicles = 8,
    Creatures = 16, // 0x00000010
    Other = 32, // 0x00000020
  }
}
