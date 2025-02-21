// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LaneDirectionType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Prefabs
{
  public enum LaneDirectionType : short
  {
    None = -180, // 0xFF4C
    Straight = 0,
    Merge = 10, // 0x000A
    Gentle = 45, // 0x002D
    Square = 90, // 0x005A
    UTurn = 180, // 0x00B4
  }
}
