// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ZoneProperty
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public enum ZoneProperty
  {
    [InstanceProperty("colossal_CellType0", typeof (float4x4), (BatchFlags) 0, 0, false)] CellType0,
    [InstanceProperty("colossal_CellType1", typeof (float4x4), BatchFlags.Extended1, 0, false)] CellType1,
    [InstanceProperty("colossal_CellType2", typeof (float4x4), BatchFlags.Extended2, 0, false)] CellType2,
    [InstanceProperty("colossal_CellType3", typeof (float4x4), BatchFlags.Extended3, 0, false)] CellType3,
    Count,
  }
}
