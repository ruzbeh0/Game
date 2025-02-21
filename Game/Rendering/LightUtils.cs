// Decompiled with JetBrains decompiler
// Type: Game.Rendering.LightUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs.Effects;
using System;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  public static class LightUtils
  {
    public static float ConvertSpotLightLumenToCandela(float intensity, float angle, bool exact)
    {
      return !exact ? intensity / 3.14159274f : intensity / (float) (2.0 * (1.0 - (double) Mathf.Cos(angle / 2f)) * 3.1415927410125732);
    }

    public static float ConvertFrustrumLightLumenToCandela(
      float intensity,
      float angleA,
      float angleB)
    {
      return intensity / (4f * Mathf.Asin(Mathf.Sin(angleA / 2f) * Mathf.Sin(angleB / 2f)));
    }

    public static float ConvertPunctualLightLumenToCandela(
      LightType lightType,
      float lumen,
      float initialIntensity,
      bool enableSpotReflector)
    {
      return lightType == LightType.Spot & enableSpotReflector ? initialIntensity : LightUtils.ConvertPointLightLumenToCandela(lumen);
    }

    public static float ConvertPointLightLumenToCandela(float intensity) => intensity / 12.566371f;

    public static float ConvertPunctualLightLumenToLux(
      LightType lightType,
      float lumen,
      float initialIntensity,
      bool enableSpotReflector,
      float distance)
    {
      return LightUtils.ConvertCandelaToLux(LightUtils.ConvertPunctualLightLumenToCandela(lightType, lumen, initialIntensity, enableSpotReflector), distance);
    }

    public static float ConvertCandelaToLux(float candela, float distance)
    {
      return candela / (distance * distance);
    }

    public static float ConvertPunctualLightLumenToEv(
      LightType lightType,
      float lumen,
      float initialIntensity,
      bool enableSpotReflector)
    {
      return LightUtils.ConvertCandelaToEv(LightUtils.ConvertPunctualLightLumenToCandela(lightType, lumen, initialIntensity, enableSpotReflector));
    }

    public static float ConvertCandelaToEv(float candela)
    {
      return LightUtils.ConvertLuminanceToEv(candela);
    }

    private static float s_LuminanceToEvFactor
    {
      get => Mathf.Log(100f / ColorUtils.s_LightMeterCalibrationConstant, 2f);
    }

    public static float ConvertLuminanceToEv(float luminance)
    {
      return Mathf.Log(luminance, 2f) + LightUtils.s_LuminanceToEvFactor;
    }

    public static float ConvertPunctualLightCandelaToLumen(
      LightType lightType,
      SpotLightShape spotLightShape,
      float candela,
      bool enableSpotReflector,
      float spotAngle,
      float aspectRatio)
    {
      if (!(lightType == LightType.Spot & enableSpotReflector))
        return LightUtils.ConvertPointLightCandelaToLumen(candela);
      if (spotLightShape == SpotLightShape.Cone)
        return LightUtils.ConvertSpotLightCandelaToLumen(candela, spotAngle * ((float) Math.PI / 180f), true);
      if (spotLightShape != SpotLightShape.Pyramid)
        return LightUtils.ConvertPointLightCandelaToLumen(candela);
      float angleA;
      float angleB;
      LightUtils.CalculateAnglesForPyramid(aspectRatio, spotAngle * ((float) Math.PI / 180f), out angleA, out angleB);
      return LightUtils.ConvertFrustrumLightCandelaToLumen(candela, angleA, angleB);
    }

    public static float ConvertSpotLightCandelaToLumen(float intensity, float angle, bool exact)
    {
      return !exact ? intensity * 3.14159274f : intensity * (float) (2.0 * (1.0 - (double) Mathf.Cos(angle / 2f)) * 3.1415927410125732);
    }

    public static float ConvertFrustrumLightCandelaToLumen(
      float intensity,
      float angleA,
      float angleB)
    {
      return intensity * (4f * Mathf.Asin(Mathf.Sin(angleA / 2f) * Mathf.Sin(angleB / 2f)));
    }

    public static float ConvertPointLightCandelaToLumen(float intensity) => intensity * 12.566371f;

    public static void CalculateAnglesForPyramid(
      float aspectRatio,
      float spotAngle,
      out float angleA,
      out float angleB)
    {
      if ((double) aspectRatio < 1.0)
        aspectRatio = 1f / aspectRatio;
      angleA = spotAngle;
      float num = Mathf.Atan(Mathf.Tan(angleA * 0.5f) * aspectRatio);
      angleB = num * 2f;
    }

    public static float ConvertPunctualLightLuxToLumen(
      LightType lightType,
      SpotLightShape spotLightShape,
      float lux,
      bool enableSpotReflector,
      float spotAngle,
      float aspectRatio,
      float distance)
    {
      float candela = LightUtils.ConvertLuxToCandela(lux, distance);
      return LightUtils.ConvertPunctualLightCandelaToLumen(lightType, spotLightShape, candela, enableSpotReflector, spotAngle, aspectRatio);
    }

    public static float ConvertLuxToCandela(float lux, float distance) => lux * distance * distance;

    public static float ConvertLuxToEv(float lux, float distance)
    {
      return LightUtils.ConvertLuminanceToEv(LightUtils.ConvertLuxToCandela(lux, distance));
    }

    public static float ConvertPunctualLightEvToLumen(
      LightType lightType,
      SpotLightShape spotLightShape,
      float ev,
      bool enableSpotReflector,
      float spotAngle,
      float aspectRatio)
    {
      float candela = LightUtils.ConvertEvToCandela(ev);
      return LightUtils.ConvertPunctualLightCandelaToLumen(lightType, spotLightShape, candela, enableSpotReflector, spotAngle, aspectRatio);
    }

    public static float ConvertEvToCandela(float ev) => LightUtils.ConvertEvToLuminance(ev);

    private static float s_EvToLuminanceFactor
    {
      get => -Mathf.Log(100f / ColorUtils.s_LightMeterCalibrationConstant, 2f);
    }

    public static float ConvertEvToLuminance(float ev)
    {
      return Mathf.Pow(2f, ev + LightUtils.s_EvToLuminanceFactor);
    }

    public static float ConvertEvToLux(float ev, float distance)
    {
      return LightUtils.ConvertCandelaToLux(LightUtils.ConvertEvToLuminance(ev), distance);
    }

    public static float ConvertAreaLightLumenToLuminance(
      AreaLightShape areaLightShape,
      float lumen,
      float width,
      float height = 0.0f)
    {
      if (areaLightShape == AreaLightShape.Rectangle)
        return LightUtils.ConvertRectLightLumenToLuminance(lumen, width, height);
      return areaLightShape == AreaLightShape.Tube ? LightUtils.CalculateLineLightLumenToLuminance(lumen, width) : lumen;
    }

    public static float ConvertRectLightLumenToLuminance(
      float intensity,
      float width,
      float height)
    {
      return intensity / (float) ((double) width * (double) height * 3.1415927410125732);
    }

    public static float CalculateLineLightLumenToLuminance(float intensity, float lineWidth)
    {
      return intensity / (12.566371f * lineWidth);
    }

    public static float ConvertAreaLightLuminanceToLumen(
      AreaLightShape areaLightShape,
      float luminance,
      float width,
      float height = 0.0f)
    {
      if (areaLightShape == AreaLightShape.Rectangle)
        return LightUtils.ConvertRectLightLuminanceToLumen(luminance, width, height);
      return areaLightShape == AreaLightShape.Tube ? LightUtils.CalculateLineLightLuminanceToLumen(luminance, width) : luminance;
    }

    public static float CalculateLineLightLuminanceToLumen(float intensity, float lineWidth)
    {
      return intensity * (12.566371f * lineWidth);
    }

    public static float ConvertRectLightLuminanceToLumen(
      float intensity,
      float width,
      float height)
    {
      return intensity * (float) ((double) width * (double) height * 3.1415927410125732);
    }

    public static float ConvertAreaLightEvToLumen(
      AreaLightShape AreaLightShape,
      float ev,
      float width,
      float height)
    {
      float luminance = LightUtils.ConvertEvToLuminance(ev);
      return LightUtils.ConvertAreaLightLuminanceToLumen(AreaLightShape, luminance, width, height);
    }

    public static float ConvertAreaLightLumenToEv(
      AreaLightShape AreaLightShape,
      float lumen,
      float width,
      float height)
    {
      return LightUtils.ConvertLuminanceToEv(LightUtils.ConvertAreaLightLumenToLuminance(AreaLightShape, lumen, width, height));
    }

    public static float ConvertLightIntensity(
      LightUnit oldLightUnit,
      LightUnit newLightUnit,
      LightEffect editor,
      float intensity)
    {
      LightType type = editor.m_Type;
      switch (type)
      {
        case LightType.Spot:
        case LightType.Point:
          if (oldLightUnit == LightUnit.Lumen && newLightUnit == LightUnit.Candela)
          {
            intensity = LightUtils.ConvertPunctualLightLumenToCandela(type, intensity, intensity, editor.m_EnableSpotReflector);
            break;
          }
          if (oldLightUnit == LightUnit.Lumen && newLightUnit == LightUnit.Lux)
          {
            intensity = LightUtils.ConvertPunctualLightLumenToLux(type, intensity, intensity, editor.m_EnableSpotReflector, editor.m_LuxAtDistance);
            break;
          }
          if (oldLightUnit == LightUnit.Lumen && newLightUnit == LightUnit.Ev100)
          {
            intensity = LightUtils.ConvertPunctualLightLumenToEv(type, intensity, intensity, editor.m_EnableSpotReflector);
            break;
          }
          if (oldLightUnit == LightUnit.Candela && newLightUnit == LightUnit.Lumen)
          {
            intensity = LightUtils.ConvertPunctualLightCandelaToLumen(type, editor.m_SpotShape, intensity, editor.m_EnableSpotReflector, editor.m_SpotAngle, editor.m_AspectRatio);
            break;
          }
          if (oldLightUnit == LightUnit.Candela && newLightUnit == LightUnit.Lux)
          {
            intensity = LightUtils.ConvertCandelaToLux(intensity, editor.m_LuxAtDistance);
            break;
          }
          if (oldLightUnit == LightUnit.Candela && newLightUnit == LightUnit.Ev100)
          {
            intensity = LightUtils.ConvertCandelaToEv(intensity);
            break;
          }
          if (oldLightUnit == LightUnit.Lux && newLightUnit == LightUnit.Lumen)
          {
            intensity = LightUtils.ConvertPunctualLightLuxToLumen(type, editor.m_SpotShape, intensity, editor.m_EnableSpotReflector, editor.m_SpotAngle, editor.m_AspectRatio, editor.m_LuxAtDistance);
            break;
          }
          if (oldLightUnit == LightUnit.Lux && newLightUnit == LightUnit.Candela)
          {
            intensity = LightUtils.ConvertLuxToCandela(intensity, editor.m_LuxAtDistance);
            break;
          }
          if (oldLightUnit == LightUnit.Lux && newLightUnit == LightUnit.Ev100)
          {
            intensity = LightUtils.ConvertLuxToEv(intensity, editor.m_LuxAtDistance);
            break;
          }
          if (oldLightUnit == LightUnit.Ev100 && newLightUnit == LightUnit.Lumen)
          {
            intensity = LightUtils.ConvertPunctualLightEvToLumen(type, editor.m_SpotShape, intensity, editor.m_EnableSpotReflector, editor.m_SpotAngle, editor.m_AspectRatio);
            break;
          }
          if (oldLightUnit == LightUnit.Ev100 && newLightUnit == LightUnit.Candela)
          {
            intensity = LightUtils.ConvertEvToCandela(intensity);
            break;
          }
          if (oldLightUnit == LightUnit.Ev100 && newLightUnit == LightUnit.Lux)
          {
            intensity = LightUtils.ConvertEvToLux(intensity, editor.m_LuxAtDistance);
            break;
          }
          break;
        case LightType.Area:
          if (oldLightUnit == LightUnit.Lumen && newLightUnit == LightUnit.Nits)
            intensity = LightUtils.ConvertAreaLightLumenToLuminance(editor.m_AreaShape, intensity, editor.m_ShapeWidth, editor.m_ShapeHeight);
          if (oldLightUnit == LightUnit.Nits && newLightUnit == LightUnit.Lumen)
            intensity = LightUtils.ConvertAreaLightLuminanceToLumen(editor.m_AreaShape, intensity, editor.m_ShapeWidth, editor.m_ShapeHeight);
          if (oldLightUnit == LightUnit.Nits && newLightUnit == LightUnit.Ev100)
            intensity = LightUtils.ConvertLuminanceToEv(intensity);
          if (oldLightUnit == LightUnit.Ev100 && newLightUnit == LightUnit.Nits)
            intensity = LightUtils.ConvertEvToLuminance(intensity);
          if (oldLightUnit == LightUnit.Ev100 && newLightUnit == LightUnit.Lumen)
            intensity = LightUtils.ConvertAreaLightEvToLumen(editor.m_AreaShape, intensity, editor.m_ShapeWidth, editor.m_ShapeHeight);
          if (oldLightUnit == LightUnit.Lumen && newLightUnit == LightUnit.Ev100)
          {
            intensity = LightUtils.ConvertAreaLightLumenToEv(editor.m_AreaShape, intensity, editor.m_ShapeWidth, editor.m_ShapeHeight);
            break;
          }
          break;
      }
      return intensity;
    }
  }
}
