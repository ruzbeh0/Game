// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ObjectProperty
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public enum ObjectProperty
  {
    [InstanceProperty("_TextureCoordinate", typeof (float3), BatchFlags.Animated, 0, false)] AnimationCoordinate,
    [InstanceProperty("colossal_BoneParameters", typeof (float2), BatchFlags.Bones, 0, false)] BoneParameters,
    [InstanceProperty("colossal_LightParameters", typeof (float2), BatchFlags.Emissive, 0, false)] LightParameters,
    [InstanceProperty("colossal_ColorMask0", typeof (float4), BatchFlags.ColorMask, 0, false)] ColorMask1,
    [InstanceProperty("colossal_ColorMask1", typeof (float4), BatchFlags.ColorMask, 0, false)] ColorMask2,
    [InstanceProperty("colossal_ColorMask2", typeof (float4), BatchFlags.ColorMask, 0, false)] ColorMask3,
    [InstanceProperty("colossal_InfoviewColor", typeof (float2), BatchFlags.InfoviewColor, 0, false)] InfoviewColor,
    [InstanceProperty("colossal_BuildingState", typeof (float4), (BatchFlags) 0, 0, false)] BuildingState,
    [InstanceProperty("_Outlines_Color", typeof (float4), BatchFlags.Outline, 0, true)] OutlineColors,
    [InstanceProperty("colossal_LodFade", typeof (float), BatchFlags.LodFade, 0, false)] LodFade0,
    [InstanceProperty("colossal_LodFade", typeof (float), BatchFlags.LodFade, 1, false)] LodFade1,
    [InstanceProperty("colossal_MetaParameters", typeof (float), BatchFlags.BlendWeights, 0, false)] MetaParameters,
    [InstanceProperty("colossal_Wetness", typeof (float4), BatchFlags.SurfaceState, 0, false)] SurfaceWetness,
    [InstanceProperty("colossal_Damage", typeof (float4), BatchFlags.SurfaceState, 0, false)] SurfaceDamage,
    Count,
  }
}
