// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshLayer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum MeshLayer : ushort
  {
    Default = 1,
    Moving = 2,
    Tunnel = 4,
    Pipeline = 8,
    SubPipeline = 16, // 0x0010
    Waterway = 32, // 0x0020
    Outline = 64, // 0x0040
    Marker = 128, // 0x0080
    First = Default, // 0x0001
    Last = Marker, // 0x0080
  }
}
