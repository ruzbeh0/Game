// Decompiled with JetBrains decompiler
// Type: Game.Rendering.MaterialProperty
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public enum MaterialProperty
  {
    [MaterialProperty("_VTUVs0", typeof (float4), false)] VTUVs0,
    [MaterialProperty("_VTUVs1", typeof (float4), false)] VTUVs1,
    [MaterialProperty("_AlbedoAffectEmissive", typeof (float), false)] AlbedoAffectEmissive,
    [MaterialProperty("colossal_SingleLightsOffset", typeof (float), false)] SingleLightsOffset,
    [MaterialProperty("colossal_TextureArea", typeof (float4), false)] TextureArea,
    [MaterialProperty("colossal_MeshSize", typeof (float4), false)] MeshSize,
    [MaterialProperty("colossal_LodDistanceFactor", typeof (float), false)] LodDistanceFactor,
    [MaterialProperty("_BaseColor", typeof (float4), false)] BaseColor,
    [MaterialProperty("colossal_DilationParams", typeof (float4), false)] DilationParams,
    [MaterialProperty("_ImpostorFrames", typeof (float), false)] ImpostorFrames,
    [MaterialProperty("_ImpostorSize", typeof (float), false)] ImpostorSize,
    [MaterialProperty("_ImpostorOffset", typeof (float3), false)] ImpostorOffset,
    [MaterialProperty("_TextureScaleFactor", typeof (float), false)] TextureScaleFactor,
    [MaterialProperty("_SmoothingDistance", typeof (float), false)] SmoothingDistance,
    [MaterialProperty("_WindRangeLvlB", typeof (float), false)] WindRangeLvlB,
    [MaterialProperty("_WindElasticityLvlB", typeof (float), false)] WindElasticityLvlB,
    [MaterialProperty("colossal_ShapeParameters1", typeof (float4), false)] ShapeParameters1,
    [MaterialProperty("colossal_ShapeParameters2", typeof (float4), false)] ShapeParameters2,
    Count,
  }
}
