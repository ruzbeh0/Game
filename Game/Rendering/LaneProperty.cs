// Decompiled with JetBrains decompiler
// Type: Game.Rendering.LaneProperty
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public enum LaneProperty
  {
    [InstanceProperty("colossal_CurveMatrix", typeof (float4x4), (BatchFlags) 0, 0, false)] CurveMatrix,
    [InstanceProperty("colossal_CurveParams", typeof (float4), (BatchFlags) 0, 0, false)] CurveParams,
    [InstanceProperty("colossal_CurveScale", typeof (float4), (BatchFlags) 0, 0, false)] CurveScale,
    [InstanceProperty("colossal_NetInfoviewColor", typeof (float4), BatchFlags.InfoviewColor, 0, false)] InfoviewColor,
    [InstanceProperty("colossal_CurveDeterioration", typeof (float4), (BatchFlags) 0, 0, false)] CurveDeterioration,
    [InstanceProperty("_Outlines_Color", typeof (float4), BatchFlags.Outline, 0, true)] OutlineColors,
    [InstanceProperty("colossal_LodFade", typeof (float), BatchFlags.LodFade, 0, false)] LodFade0,
    [InstanceProperty("colossal_LodFade", typeof (float), BatchFlags.LodFade, 1, false)] LodFade1,
    [InstanceProperty("colossal_FlowMatrix", typeof (float4x4), BatchFlags.InfoviewFlow, 0, false)] FlowMatrix,
    [InstanceProperty("colossal_FlowOffset", typeof (float), BatchFlags.InfoviewFlow, 0, false)] FlowOffset,
    [InstanceProperty("colossal_HangingDistances", typeof (float4), BatchFlags.Hanging, 0, false)] HangingDistances,
    [InstanceProperty("colossal_ColorMask0", typeof (float4), BatchFlags.ColorMask, 0, false)] ColorMask1,
    [InstanceProperty("colossal_ColorMask1", typeof (float4), BatchFlags.ColorMask, 0, false)] ColorMask2,
    [InstanceProperty("colossal_ColorMask2", typeof (float4), BatchFlags.ColorMask, 0, false)] ColorMask3,
    Count,
  }
}
