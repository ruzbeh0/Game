// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Climate.WeatherPropertiesStack
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Prefabs.Climate;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering.Climate
{
  internal class WeatherPropertiesStack : IDisposable
  {
    private Volume m_Volume;
    public readonly Dictionary<System.Type, WeatherPropertiesStack.InterpolatedProperties> components = new Dictionary<System.Type, WeatherPropertiesStack.InterpolatedProperties>();

    public WeatherPropertiesStack(Volume volume = null)
    {
      this.m_Volume = volume;
      this.CreateInterpolatedRepresentation();
    }

    private void CreateInterpolatedRepresentation()
    {
      foreach (System.Type type in CoreUtils.GetAllTypesDerivedFrom<OverrideablePropertiesComponent>().Where<System.Type>((Func<System.Type, bool>) (t => !t.IsAbstract)))
      {
        OverrideablePropertiesComponent instance1 = (OverrideablePropertiesComponent) ScriptableObject.CreateInstance(type);
        OverrideablePropertiesComponent instance2 = (OverrideablePropertiesComponent) ScriptableObject.CreateInstance(type);
        OverrideablePropertiesComponent instance3 = (OverrideablePropertiesComponent) ScriptableObject.CreateInstance(type);
        OverrideablePropertiesComponent instance4 = (OverrideablePropertiesComponent) ScriptableObject.CreateInstance(type);
        OverrideablePropertiesComponent instance5 = (OverrideablePropertiesComponent) ScriptableObject.CreateInstance(type);
        instance1.Bind(this.m_Volume);
        this.components.Add(type, new WeatherPropertiesStack.InterpolatedProperties(instance1, instance2, instance3, instance4, instance5));
      }
    }

    public void SetTarget(System.Type type, OverrideablePropertiesComponent target)
    {
      WeatherPropertiesStack.InterpolatedProperties interpolatedProperties;
      this.components.TryGetValue(type, out interpolatedProperties);
      interpolatedProperties.SetTarget(target);
      interpolatedProperties.SetPrevious(interpolatedProperties.current);
    }

    public void SetTo(
      System.Type type,
      OverrideablePropertiesComponent to,
      bool setLimits,
      Bounds1 limits)
    {
      WeatherPropertiesStack.InterpolatedProperties interpolatedProperties;
      this.components.TryGetValue(type, out interpolatedProperties);
      if (setLimits)
        interpolatedProperties.remapLimits = limits;
      interpolatedProperties.SetTo(to);
      interpolatedProperties.SetPrevious(interpolatedProperties.current);
    }

    public void SetFrom(System.Type type, OverrideablePropertiesComponent from)
    {
      WeatherPropertiesStack.InterpolatedProperties interpolatedProperties;
      this.components.TryGetValue(type, out interpolatedProperties);
      interpolatedProperties.SetFrom(from);
      interpolatedProperties.SetPrevious(interpolatedProperties.current);
    }

    public void InterpolateOverrideData(
      float deltaTime,
      float renderingDeltaTime,
      ClimateSystem.ClimateSample sample,
      bool editMode)
    {
      foreach (KeyValuePair<System.Type, WeatherPropertiesStack.InterpolatedProperties> component in this.components)
      {
        if (component.Value.target.active)
        {
          if (!component.Value.to.hasTimeBasedInterpolation)
          {
            float lerp = component.Value.GetLerp(sample);
            component.Value.target.Override(component.Value.from, component.Value.to, lerp);
          }
          if (editMode)
            component.Value.source.Override(component.Value.target);
          component.Value.current.Override(component.Value.previous, component.Value.target, component.Value.time);
          component.Value.Advance(deltaTime, renderingDeltaTime);
        }
      }
    }

    public void Dispose()
    {
      foreach (KeyValuePair<System.Type, WeatherPropertiesStack.InterpolatedProperties> component in this.components)
      {
        CoreUtils.Destroy((UnityEngine.Object) component.Value.current);
        CoreUtils.Destroy((UnityEngine.Object) component.Value.previous);
        CoreUtils.Destroy((UnityEngine.Object) component.Value.target);
        CoreUtils.Destroy((UnityEngine.Object) component.Value.from);
        CoreUtils.Destroy((UnityEngine.Object) component.Value.to);
      }
      this.components.Clear();
    }

    public class InterpolatedProperties
    {
      public float time;
      public Bounds1 remapLimits;
      public readonly OverrideablePropertiesComponent current;
      public readonly OverrideablePropertiesComponent previous;
      public readonly OverrideablePropertiesComponent target;
      public readonly OverrideablePropertiesComponent from;
      public readonly OverrideablePropertiesComponent to;
      internal OverrideablePropertiesComponent source;

      public InterpolatedProperties(
        OverrideablePropertiesComponent current,
        OverrideablePropertiesComponent previous,
        OverrideablePropertiesComponent target,
        OverrideablePropertiesComponent from,
        OverrideablePropertiesComponent to)
      {
        this.current = current;
        this.previous = previous;
        this.target = target;
        this.from = from;
        this.to = to;
      }

      private static float Remap(float value, float from1, float to1, float from2, float to2)
      {
        return math.saturate((float) (((double) value - (double) from1) / ((double) to1 - (double) from1) * ((double) to2 - (double) from2)) + from2);
      }

      public void SetTarget(OverrideablePropertiesComponent newTarget)
      {
        newTarget.Override(this.target);
        this.source = newTarget;
        this.target.m_InterpolationMode = this.source.m_InterpolationMode;
        this.target.m_InterpolationTime = this.source.m_InterpolationTime;
        this.time = 0.0f;
      }

      public void SetPrevious(OverrideablePropertiesComponent newSource)
      {
        newSource.Override(this.previous);
        this.time = 0.0f;
      }

      public void SetTo(OverrideablePropertiesComponent newTo)
      {
        newTo.Override(this.to);
        this.source = newTo;
        this.to.m_InterpolationMode = this.source.m_InterpolationMode;
        this.to.m_InterpolationTime = this.source.m_InterpolationTime;
        this.time = 0.0f;
      }

      public void SetFrom(OverrideablePropertiesComponent newTo)
      {
        newTo.Override(this.from);
        this.time = 0.0f;
      }

      public void Advance(float deltaTime, float renderingDeltaTime)
      {
        switch (this.target.m_InterpolationMode)
        {
          case OverrideablePropertiesComponent.InterpolationMode.RenderingTime:
            this.time = math.saturate(this.time + renderingDeltaTime / this.target.m_InterpolationTime);
            break;
          default:
            this.time = math.saturate(this.time + deltaTime / this.target.m_InterpolationTime);
            break;
        }
      }

      public float GetLerp(ClimateSystem.ClimateSample sample)
      {
        switch (this.to.m_InterpolationMode)
        {
          case OverrideablePropertiesComponent.InterpolationMode.Cloudiness:
            return WeatherPropertiesStack.InterpolatedProperties.Remap(sample.cloudiness, this.remapLimits.min, this.remapLimits.max, 0.0f, 1f);
          case OverrideablePropertiesComponent.InterpolationMode.Precipitation:
            return sample.precipitation;
          case OverrideablePropertiesComponent.InterpolationMode.Aurora:
            return sample.aurora;
          default:
            return 1f;
        }
      }
    }
  }
}
