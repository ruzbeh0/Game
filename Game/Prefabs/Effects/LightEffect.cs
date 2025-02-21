// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Effects.LightEffect
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Game.Reflection;
using Game.Rendering;
using Game.UI;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

#nullable disable
namespace Game.Prefabs.Effects
{
  [ComponentMenu("Effects/", new System.Type[] {typeof (EffectPrefab)})]
  public class LightEffect : ComponentBase
  {
    public Game.Rendering.LightType m_Type;
    public Game.Rendering.SpotLightShape m_SpotShape;
    public Game.Rendering.AreaLightShape m_AreaShape;
    public float m_Range = 25f;
    [FormerlySerializedAs("m_LuxIntensity")]
    [HideInInspector]
    public float m_Intensity = 10f;
    public LightIntensity m_LightIntensity;
    [FormerlySerializedAs("m_LuxDistance")]
    public float m_LuxAtDistance = 5f;
    [HideInInspector]
    public Game.Rendering.LightUnit m_LightUnit = Game.Rendering.LightUnit.Lux;
    public bool m_EnableSpotReflector = true;
    [Range(1f, 179f)]
    public float m_SpotAngle = 150f;
    [Range(0.0f, 100f)]
    public float m_InnerSpotPercentage = 50f;
    public float m_ShapeRadius = 0.025f;
    [Range(0.05f, 20f)]
    public float m_AspectRatio = 1f;
    public float m_ShapeWidth = 0.5f;
    public float m_ShapeHeight = 0.5f;
    public bool m_UseColorTemperature = true;
    public Color m_Color = new Color(1f, 0.86f, 0.65f, 1f);
    [CustomField(typeof (LightEffect.ColorTemperatureSliderFieldFactory))]
    public float m_ColorTemperature = 6570f;
    public Texture m_Cookie;
    public bool m_AffectDiffuse = true;
    public bool m_AffectSpecular = true;
    public bool m_ApplyRangeAttenuation = true;
    [Range(0.0f, 16f)]
    public float m_LightDimmer = 1f;
    public float m_LodBias;
    [HideInInspector]
    public float m_BarnDoorAngle;
    [HideInInspector]
    public float m_BarnDoorLength;
    public bool m_UseVolumetric = true;
    [Range(0.0f, 16f)]
    public float m_VolumetricDimmer = 1f;
    public float m_VolumetricFadeDistance = 10000f;

    public void RecalculateIntensity(Game.Rendering.LightUnit oldUnit, Game.Rendering.LightUnit newUnit)
    {
      this.m_Intensity = Game.Rendering.LightUtils.ConvertLightIntensity(oldUnit, newUnit, this, this.m_Intensity);
      this.m_LightIntensity.m_Intensity = this.m_Intensity;
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<LightEffectData>());
      components.Add(ComponentType.ReadWrite<EffectColorData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(this.m_Range)), this.m_LodBias);
      float distanceFactor = RenderingUtils.CalculateDistanceFactor(lodLimit);
      float num = 1f / distanceFactor;
      if (this.m_LightIntensity != null)
      {
        if (this.m_LightUnit != this.m_LightIntensity.m_LightUnit)
          this.RecalculateIntensity(this.m_LightUnit, this.m_LightIntensity.m_LightUnit);
        this.m_Intensity = this.m_LightIntensity.m_Intensity;
        this.m_LightUnit = this.m_LightIntensity.m_LightUnit;
      }
      else
        this.m_LightIntensity = new LightIntensity()
        {
          m_Intensity = this.m_Intensity,
          m_LightUnit = this.m_LightUnit
        };
      LightEffectData componentData1 = new LightEffectData()
      {
        m_Range = this.m_Range,
        m_DistanceFactor = distanceFactor,
        m_InvDistanceFactor = num,
        m_MinLod = lodLimit
      };
      entityManager.SetComponentData<LightEffectData>(entity, componentData1);
      EffectColorData componentData2 = entityManager.GetComponentData<EffectColorData>(entity) with
      {
        m_Color = this.ComputeLightFinalColor()
      };
      entityManager.SetComponentData<EffectColorData>(entity, componentData2);
    }

    public HDLightTypeAndShape GetLightTypeAndShape()
    {
      switch (this.m_Type)
      {
        case Game.Rendering.LightType.Spot:
          switch (this.m_SpotShape)
          {
            case Game.Rendering.SpotLightShape.Cone:
              return HDLightTypeAndShape.ConeSpot;
            case Game.Rendering.SpotLightShape.Pyramid:
              return HDLightTypeAndShape.PyramidSpot;
            case Game.Rendering.SpotLightShape.Box:
              return HDLightTypeAndShape.BoxSpot;
            default:
              throw new NotImplementedException(string.Format("Spot shape not implemented {0}", (object) this.m_SpotShape));
          }
        case Game.Rendering.LightType.Point:
          return HDLightTypeAndShape.Point;
        case Game.Rendering.LightType.Area:
          switch (this.m_AreaShape)
          {
            case Game.Rendering.AreaLightShape.Rectangle:
              return HDLightTypeAndShape.RectangleArea;
            case Game.Rendering.AreaLightShape.Tube:
              return HDLightTypeAndShape.TubeArea;
            default:
              throw new NotImplementedException(string.Format("Area shape not implemented {0}", (object) this.m_AreaShape));
          }
        default:
          throw new NotImplementedException(string.Format("Light type not implemented {0}", (object) this.m_Type));
      }
    }

    public Color GetEmissionColor()
    {
      Color color = this.m_Color.linear * this.m_LightIntensity.m_Intensity;
      if (this.m_UseColorTemperature)
        color *= Mathf.CorrelatedColorTemperatureToRGB(this.m_ColorTemperature);
      return color * this.m_LightDimmer;
    }

    private float CalculateLightIntensityPunctual(float intensity)
    {
      switch (this.m_Type)
      {
        case Game.Rendering.LightType.Spot:
          if (this.m_LightUnit == Game.Rendering.LightUnit.Candela)
            return intensity;
          if (!this.m_EnableSpotReflector)
            return Game.Rendering.LightUtils.ConvertPointLightLumenToCandela(intensity);
          if (this.m_SpotShape == Game.Rendering.SpotLightShape.Cone)
            return Game.Rendering.LightUtils.ConvertSpotLightLumenToCandela(intensity, this.m_SpotAngle * ((float) Math.PI / 180f), true);
          if (this.m_SpotShape != Game.Rendering.SpotLightShape.Pyramid)
            return Game.Rendering.LightUtils.ConvertPointLightLumenToCandela(intensity);
          float angleA;
          float angleB;
          Game.Rendering.LightUtils.CalculateAnglesForPyramid(this.m_AspectRatio, this.m_SpotAngle * ((float) Math.PI / 180f), out angleA, out angleB);
          return Game.Rendering.LightUtils.ConvertFrustrumLightLumenToCandela(intensity, angleA, angleB);
        case Game.Rendering.LightType.Point:
          return this.m_LightUnit == Game.Rendering.LightUnit.Candela ? intensity : Game.Rendering.LightUtils.ConvertPointLightLumenToCandela(intensity);
        default:
          return intensity;
      }
    }

    private float ComputeLightIntensity()
    {
      if (this.m_LightUnit == Game.Rendering.LightUnit.Lumen)
        return this.m_Type == Game.Rendering.LightType.Spot || this.m_Type == Game.Rendering.LightType.Point ? this.CalculateLightIntensityPunctual(this.m_LightIntensity.m_Intensity) : Game.Rendering.LightUtils.ConvertAreaLightLumenToLuminance(this.m_AreaShape, this.m_LightIntensity.m_Intensity, this.m_ShapeWidth, this.m_ShapeHeight);
      if (this.m_LightUnit == Game.Rendering.LightUnit.Ev100)
        return Game.Rendering.LightUtils.ConvertEvToLuminance(this.m_LightIntensity.m_Intensity);
      return (this.m_Type == Game.Rendering.LightType.Spot || this.m_Type == Game.Rendering.LightType.Point) && this.m_LightUnit == Game.Rendering.LightUnit.Lux && (this.m_Type != Game.Rendering.LightType.Spot || this.m_SpotShape != Game.Rendering.SpotLightShape.Box) ? Game.Rendering.LightUtils.ConvertLuxToCandela(this.m_LightIntensity.m_Intensity, this.m_LuxAtDistance) : this.m_LightIntensity.m_Intensity;
    }

    public Color ComputeLightFinalColor()
    {
      Color color = this.m_Color.linear * this.ComputeLightIntensity();
      if (this.m_UseColorTemperature)
        color *= Mathf.CorrelatedColorTemperatureToRGB(this.m_ColorTemperature);
      return color * this.m_LightDimmer;
    }

    private class ColorTemperatureSliderFieldFactory : IFieldBuilderFactory
    {
      private IconValuePairs iconValuePairs;

      private string GetIconSource(float value) => this.iconValuePairs.GetIconFromValue(value);

      public FieldBuilder TryCreate(System.Type memberType, object[] attributes)
      {
        this.iconValuePairs = new IconValuePairs(new IconValuePairs.IconValuePair[7]
        {
          new IconValuePairs.IconValuePair("Media/Editor/Temperature01.svg", 2500f),
          new IconValuePairs.IconValuePair("Media/Editor/Temperature02.svg", 3500f),
          new IconValuePairs.IconValuePair("Media/Editor/Temperature03.svg", 4500f),
          new IconValuePairs.IconValuePair("Media/Editor/Temperature04.svg", 6000f),
          new IconValuePairs.IconValuePair("Media/Editor/Temperature05.svg", 7000f),
          new IconValuePairs.IconValuePair("Media/Editor/Temperature06.svg", 10000f),
          new IconValuePairs.IconValuePair("Media/Editor/Temperature07.svg", 20000f)
        });
        return (FieldBuilder) (accessor =>
        {
          return (IWidget) new GradientSliderField()
          {
            accessor = (ITypedValueAccessor<float>) new CastAccessor<float>(accessor),
            displayName = (LocalizedString) "Color temperature",
            gradient = (ColorGradient) ColorUtils.GetTemperatureGradient(),
            min = 1500f,
            max = 20000f,
            iconSrc = (Func<string>) (() => this.GetIconSource((float) accessor.GetValue()))
          };
        });
      }
    }
  }
}
