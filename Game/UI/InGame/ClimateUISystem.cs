// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ClimateUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Simulation;
using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class ClimateUISystem : UISystemBase
  {
    public const string kGroup = "climate";
    private ClimateSystem m_ClimateSystem;
    private EntityQuery m_ClimateQuery;
    private EntityQuery m_ClimateSeasonQuery;
    private EntityQuery m_SeasonChangedQuery;
    private GetterValueBinding<float> m_TemperatureBinding;
    private GetterValueBinding<WeatherType> m_WeatherBinding;
    private GetterValueBinding<string> m_SeasonBinding;
    private Entity m_CurrentSeason;

    private float m_TemperatureBindingValue
    {
      get => MathUtils.Snap((float) this.m_ClimateSystem.temperature, 0.1f);
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      this.AddBinding((IBinding) (this.m_TemperatureBinding = new GetterValueBinding<float>("climate", "temperature", (Func<float>) (() => this.m_TemperatureBindingValue))));
      this.AddBinding((IBinding) (this.m_WeatherBinding = new GetterValueBinding<WeatherType>("climate", "weather", new Func<WeatherType>(this.GetWeather), (IWriter<WeatherType>) new DelegateWriter<WeatherType>(new WriterDelegate<WeatherType>(ClimateUISystem.WriteWeatherType)))));
      this.AddBinding((IBinding) (this.m_SeasonBinding = new GetterValueBinding<string>("climate", "seasonNameId", new Func<string>(this.GetCurrentSeasonNameID), (IWriter<string>) new StringWriter().Nullable<string>())));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_TemperatureBinding.Update();
      this.m_WeatherBinding.Update();
      if (!this.m_SeasonBinding.Update() && this.m_CurrentSeason != this.m_ClimateSystem.currentSeason)
        this.m_SeasonBinding.TriggerUpdate();
      this.m_CurrentSeason = this.m_ClimateSystem.currentSeason;
    }

    public WeatherType GetWeather()
    {
      if (!this.m_ClimateSystem.isPrecipitating)
        return ClimateUISystem.FromWeatherClassification(this.m_ClimateSystem.classification);
      if (this.m_ClimateSystem.isRaining)
        return WeatherType.Rain;
      return this.m_ClimateSystem.isSnowing ? WeatherType.Snow : WeatherType.Clear;
    }

    private static WeatherType FromWeatherClassification(
      ClimateSystem.WeatherClassification classification)
    {
      switch (classification)
      {
        case ClimateSystem.WeatherClassification.Clear:
          return WeatherType.Clear;
        case ClimateSystem.WeatherClassification.Few:
          return WeatherType.Few;
        case ClimateSystem.WeatherClassification.Scattered:
          return WeatherType.Scattered;
        case ClimateSystem.WeatherClassification.Broken:
          return WeatherType.Broken;
        case ClimateSystem.WeatherClassification.Overcast:
          return WeatherType.Overcast;
        case ClimateSystem.WeatherClassification.Stormy:
          return WeatherType.Storm;
        default:
          return WeatherType.Clear;
      }
    }

    private string GetCurrentSeasonNameID() => this.m_ClimateSystem.currentSeasonNameID;

    private static void WriteWeatherType(IJsonWriter writer, WeatherType type)
    {
      writer.Write((int) type);
    }

    [Preserve]
    public ClimateUISystem()
    {
    }
  }
}
