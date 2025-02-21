// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.ClimatePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Simulation;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs.Climate
{
  [ComponentMenu("Weather/", new System.Type[] {})]
  public class ClimatePrefab : PrefabBase, IJsonWritable
  {
    [Range(-90f, 90f)]
    [EditorName("Editor.CLIMATE_LATITUDE")]
    public float m_Latitude = 61.49772f;
    [Range(-180f, 180f)]
    [EditorName("Editor.CLIMATE_LONGITUDE")]
    public float m_Longitude = 23.7670422f;
    [EditorName("Editor.CLIMATE_FREEZING_TEMPERATURE")]
    public float m_FreezingTemperature;
    public AnimationCurve m_Temperature;
    public AnimationCurve m_Precipitation;
    public AnimationCurve m_Cloudiness;
    public AnimationCurve m_Aurora;
    public AnimationCurve m_Fog;
    [EditorName("Editor.CLIMATE_DEFAULT_WEATHER")]
    public WeatherPrefab m_DefaultWeather;
    [EditorName("Editor.CLIMATE_DEFAULT_WEATHERS")]
    public WeatherPrefab[] m_DefaultWeathers;
    [EditorName("Editor.CLIMATE_SEASONS")]
    public ClimateSystem.SeasonInfo[] m_Seasons;
    [HideInInspector]
    public int m_RandomSeed = 1;
    public const int kYearDuration = 12;
    private int[] m_SeasonsOrder;
    private const float k90PercentileToStdDev = 0.780031264f;

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().Name);
      writer.PropertyName("latitude");
      writer.Write(this.m_Latitude);
      writer.PropertyName("longitude");
      writer.Write(this.m_Longitude);
      writer.PropertyName("freezingTemperature");
      writer.Write(this.m_FreezingTemperature);
      writer.PropertyName("seasons");
      writer.Write<ClimateSystem.SeasonInfo>((IList<ClimateSystem.SeasonInfo>) this.m_Seasons);
      writer.TypeEnd();
    }

    public void RebuildCurves()
    {
      this.EnsureSeasonsOrder(true);
      uint seed = (uint) this.m_RandomSeed;
      if (seed == 0U)
        seed = (uint) ((double) Time.realtimeSinceStartup * 10.0);
      this.RebuildTemperatureCurves(seed);
      this.RebuildPrecipitationCurves(seed);
      this.RebuildAuroraCurves(seed);
      this.RebuildFogCurves(seed);
    }

    internal void EnsureSeasonsOrder(bool force = false)
    {
      if (!force && this.m_SeasonsOrder != null && this.m_SeasonsOrder.Length == this.m_Seasons.Length)
        return;
      this.m_SeasonsOrder = Enumerable.Range(0, this.m_Seasons.Length).OrderBy<int, float>((Func<int, float>) (v => this.m_Seasons[v].m_StartTime)).ToArray<int>();
    }

    private static AnimationCurve GenCurveFromMinMax(
      int keyCount,
      AnimationCurve cmin,
      AnimationCurve cmax,
      uint seed,
      float minValue,
      float maxValue)
    {
      Unity.Mathematics.Random rng = new Unity.Mathematics.Random(seed);
      Keyframe[] keyframeArray = new Keyframe[keyCount];
      for (int index = 0; index < keyframeArray.Length; ++index)
      {
        float time = (float) ((double) index / (double) keyframeArray.Length * 12.0);
        float num1 = cmin.Evaluate(time);
        float num2 = cmax.Evaluate(time);
        float dev = (float) (((double) num2 - (double) num1) / 2.0 * 0.78003126382827759);
        keyframeArray[index].time = time;
        keyframeArray[index].value = ClimatePrefab.GaussianRandom((float) (((double) num1 + (double) num2) / 2.0), dev, ref rng);
      }
      AnimationCurve curve = new AnimationCurve(keyframeArray);
      ClimatePrefab.LoopCurve(curve, minValue, maxValue);
      return curve;
    }

    private void RebuildTemperatureCurves(uint seed)
    {
      ClimatePrefab.SeasonTempCurves temperatureCurves = this.CreateSeasonTemperatureCurves();
      AnimationCurve animationCurve1 = ClimatePrefab.GenCurveFromMinMax(12, temperatureCurves.nightMin, temperatureCurves.nightMax, seed + 10000U, -100f, 100f);
      AnimationCurve animationCurve2 = ClimatePrefab.GenCurveFromMinMax(12, temperatureCurves.dayMin, temperatureCurves.dayMax, seed + 11000U, -100f, 100f);
      Keyframe[] keyframeArray = new Keyframe[288];
      for (int index = 0; index < 288; ++index)
      {
        float time = (float) ((double) index / 288.0 * 12.0);
        keyframeArray[index].time = time;
        float x = animationCurve1.Evaluate(time);
        float y = animationCurve2.Evaluate(time);
        float s = (float) (-(double) math.cos((float) (((double) (index % 24) / 24.0 + (double) noise.cnoise(new float2(time * 4f, 0.0f)) / 24.0 * 4.0) * 3.1415927410125732 * 2.0)) * 0.5 + 0.5);
        keyframeArray[index].value = math.lerp(x, y, s);
      }
      this.m_Temperature = new AnimationCurve(keyframeArray);
      ClimatePrefab.LoopCurve(this.m_Temperature, -100f, 100f);
    }

    private void RebuildPrecipitationCurves(uint seed)
    {
      ClimatePrefab.SeasonPrecipCurves seasonPrecipCurves = this.CreateSeasonPrecipCurves();
      AnimationCurve animationCurve1 = ClimatePrefab.GenCurveFromMinMax(12, seasonPrecipCurves.cloudAmountMin, seasonPrecipCurves.cloudAmountMax, seed + 1000U, 0.0f, 1f);
      Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed + 2000U);
      Keyframe[] keyframeArray1 = new Keyframe[1728];
      float y1 = random.NextFloat(0.0f, 100f);
      float y2 = random.NextFloat(0.0f, 100f);
      for (int index = 0; index < keyframeArray1.Length; ++index)
      {
        float num1 = (float) ((double) index / (double) keyframeArray1.Length * 12.0);
        keyframeArray1[index].time = num1;
        float num2 = math.saturate(animationCurve1.Evaluate(num1));
        float num3 = math.saturate(seasonPrecipCurves.turbulence.Evaluate(num1));
        float num4 = ClimatePrefab.SmoothNoise(num1 * 4f, y2) * num3 * num2;
        float num5 = math.saturate(num2 + num4);
        float num6 = math.saturate(seasonPrecipCurves.cloudChance.Evaluate(num1));
        float num7 = math.saturate((float) (((double) ClimatePrefab.SmoothNoise(num1, y1) + (double) ClimatePrefab.SmoothNoise(num1 * 2f, y1 + 7f) * 0.5) * 0.5 + 0.5));
        if ((double) num7 > (double) num6)
          num5 *= 1f - math.saturate((float) (((double) num7 - (double) num6) * 2.0));
        keyframeArray1[index].value = num5;
      }
      this.m_Cloudiness = new AnimationCurve(keyframeArray1);
      ClimatePrefab.LoopCurve(this.m_Cloudiness);
      AnimationCurve animationCurve2 = ClimatePrefab.GenCurveFromMinMax(12, seasonPrecipCurves.precipAmountMin, seasonPrecipCurves.precipAmountMax, seed + 3000U, 0.0f, 1f);
      random = new Unity.Mathematics.Random(seed + 4000U);
      Keyframe[] keyframeArray2 = new Keyframe[1728];
      float y3 = random.NextFloat(0.0f, 100f);
      float y4 = random.NextFloat(0.0f, 100f);
      for (int index = 0; index < keyframeArray2.Length; ++index)
      {
        float num8 = (float) ((double) index / (double) keyframeArray2.Length * 12.0);
        keyframeArray2[index].time = num8;
        float num9 = math.saturate(animationCurve2.Evaluate(num8));
        float num10 = math.saturate(seasonPrecipCurves.turbulence.Evaluate(num8));
        float num11 = ClimatePrefab.SmoothNoise(num8 * 4f, y4) * num10 * num9;
        float num12 = math.saturate(num9 + num11);
        float num13 = math.saturate(seasonPrecipCurves.precipChance.Evaluate(num8));
        float num14 = math.saturate((float) (((double) ClimatePrefab.SmoothNoise(num8, y3) + (double) ClimatePrefab.SmoothNoise(num8 * 2f, y3 + 7f) * 0.5) * 0.5 + 0.5));
        if ((double) num14 > (double) num13)
          num12 *= 1f - math.saturate((float) (((double) num14 - (double) num13) * 2.0));
        float num15 = this.m_Cloudiness.Evaluate(num8);
        if ((double) num15 < 0.699999988079071)
          num12 *= num15 / 0.7f;
        if ((double) num15 < 0.40000000596046448)
          num12 *= num15 / 0.4f;
        if ((double) num15 < 0.20000000298023224)
          num12 = 0.0f;
        keyframeArray2[index].value = num12;
      }
      this.m_Precipitation = new AnimationCurve(keyframeArray2);
      ClimatePrefab.LoopCurve(this.m_Precipitation);
    }

    private static float SmoothNoise(float x, float y = 0.0f) => noise.snoise(new float2(x, y));

    private void RebuildAuroraCurves(uint seed)
    {
      ClimatePrefab.SeasonAuroraCurves seasonAuroraCurves = this.CreateSeasonAuroraCurves();
      Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed + 5000U);
      Keyframe[] keyframeArray = new Keyframe[288];
      float y1 = random.NextFloat(0.0f, 100f);
      float y2 = random.NextFloat(0.0f, 100f);
      for (int index = 0; index < keyframeArray.Length; ++index)
      {
        float num1 = (float) ((double) index / (double) keyframeArray.Length * 12.0);
        keyframeArray[index].time = num1;
        float num2 = math.max(0.0f, seasonAuroraCurves.amount.Evaluate(num1));
        float num3 = 0.1f;
        float num4 = ClimatePrefab.SmoothNoise(num1 * 4f, y2) * num3 * num2;
        float num5 = math.max(0.0f, num2 + num4);
        float num6 = math.saturate(seasonAuroraCurves.chance.Evaluate(num1));
        float num7 = math.saturate((float) (((double) ClimatePrefab.SmoothNoise(num1, y1) + (double) ClimatePrefab.SmoothNoise(num1 * 2f, y1 + 7f) * 0.5) * 0.5 + 0.5));
        if ((double) num7 > (double) num6)
          num5 *= 1f - math.saturate((float) (((double) num7 - (double) num6) * 8.0));
        keyframeArray[index].value = num5;
      }
      this.m_Aurora = new AnimationCurve(keyframeArray);
      ClimatePrefab.LoopCurve(this.m_Aurora, maxValue: 10f);
    }

    private void RebuildFogCurves(uint seed)
    {
      Keyframe[] keyframeArray = new Keyframe[288];
      float num1 = 0.0416666679f;
      float num2 = 2f;
      float num3 = 0.15f;
      float num4 = -1f;
      float num5 = 25f;
      float num6 = 0.5f;
      for (int index = 0; index < keyframeArray.Length; ++index)
      {
        float time = (float) ((double) index / (double) keyframeArray.Length * 12.0);
        keyframeArray[index].time = time;
        double num7 = (double) this.m_Cloudiness.Evaluate(time);
        float num8 = this.m_Precipitation.Evaluate(time);
        float num9 = this.m_Temperature.Evaluate(time);
        float x = 0.0f;
        double num10 = (double) num3;
        if (num7 > num10 && (double) num9 > (double) num4 && (double) num9 < (double) num5 && (double) num8 < (double) num6)
        {
          double num11 = (double) this.m_Temperature.Evaluate(time - 8f * num1);
          float num12 = this.m_Temperature.Evaluate(time - 7f * num1);
          float num13 = this.m_Temperature.Evaluate(time - 6f * num1);
          float num14 = this.m_Temperature.Evaluate(time - 5f * num1);
          float num15 = this.m_Temperature.Evaluate(time - 4f * num1);
          float num16 = this.m_Temperature.Evaluate(time - 3f * num1);
          float num17 = this.m_Temperature.Evaluate(time - 2f * num1);
          float num18 = this.m_Temperature.Evaluate(time - 1f * num1);
          if (num11 - (double) num12 > (double) num2)
            x += 0.19f;
          if ((double) num12 - (double) num13 > (double) num2)
            x += 0.17f;
          if ((double) num13 - (double) num14 > (double) num2)
            x += 0.12f;
          if ((double) num14 - (double) num15 > (double) num2)
            x += 0.09f;
          if ((double) num15 - (double) num16 > (double) num2)
            x += 0.11f;
          if ((double) num16 - (double) num17 > (double) num2)
            x += 0.13f;
          if ((double) num17 - (double) num18 > (double) num2)
            x += 0.14f;
          if ((double) num18 - (double) num9 > (double) num2)
            x += 0.15f;
          if (num11 - (double) num14 > (double) num2)
            x += 0.21f;
          if ((double) num12 - (double) num15 > (double) num2)
            x += 0.18f;
          if ((double) num13 - (double) num16 > (double) num2)
            x += 0.07f;
        }
        keyframeArray[index].value = math.saturate(x);
      }
      this.m_Fog = new AnimationCurve(keyframeArray);
      ClimatePrefab.LoopCurve(this.m_Aurora);
    }

    private static float GaussianRandom(float mean, float dev, ref Unity.Mathematics.Random rng)
    {
      int num1 = 0;
      float num2;
      do
      {
        float x = rng.NextFloat();
        float num3 = rng.NextFloat();
        float num4 = math.sqrt(-2f * math.log(x)) * math.sin(6.28318548f * num3);
        num2 = mean + dev * num4;
      }
      while ((double) math.abs(num2 - mean) > 2.0 * (double) dev && num1++ < 20);
      return num2;
    }

    public ClimatePrefab.SeasonTempCurves CreateSeasonTemperatureCurves()
    {
      ClimatePrefab.SeasonTempCurves temperatureCurves = new ClimatePrefab.SeasonTempCurves()
      {
        nightMin = new AnimationCurve(),
        nightMax = new AnimationCurve(),
        dayMin = new AnimationCurve(),
        dayMax = new AnimationCurve()
      };
      for (int index = 0; index < this.m_Seasons.Length; ++index)
      {
        (ClimateSystem.SeasonInfo seasonInfo, float time) = this.GetSeasonAndMidTime(index);
        float2 tempNightDay = seasonInfo.m_TempNightDay;
        float2 float2 = math.abs(seasonInfo.m_TempDeviationNightDay);
        temperatureCurves.nightMin.AddKey(time, tempNightDay.x - float2.x);
        temperatureCurves.nightMax.AddKey(time, tempNightDay.x + float2.x);
        temperatureCurves.dayMin.AddKey(time, tempNightDay.y - float2.y);
        temperatureCurves.dayMax.AddKey(time, tempNightDay.y + float2.y);
      }
      ClimatePrefab.LoopCurve(temperatureCurves.nightMin, -100f, 100f);
      ClimatePrefab.LoopCurve(temperatureCurves.nightMax, -100f, 100f);
      ClimatePrefab.LoopCurve(temperatureCurves.dayMin, -100f, 100f);
      ClimatePrefab.LoopCurve(temperatureCurves.dayMax, -100f, 100f);
      return temperatureCurves;
    }

    public (ClimateSystem.SeasonInfo, float) GetSeasonAndMidTime(int index)
    {
      this.EnsureSeasonsOrder();
      return (this.m_Seasons[this.m_SeasonsOrder[index]], this.GetSeasonMidTime(index));
    }

    public int CountElapsedSeasons(float startTime, float elapsedTime)
    {
      if (this.m_Seasons == null || this.m_Seasons.Length == 0)
        return 0;
      if (this.m_Seasons.Length == 1)
        return 1;
      int num = 0;
      for (int index = 0; index < this.m_SeasonsOrder.Length; ++index)
      {
        ClimateSystem.SeasonInfo season1 = this.m_Seasons[this.m_SeasonsOrder[index]];
        ClimateSystem.SeasonInfo season2 = this.m_Seasons[this.m_SeasonsOrder[(index + 1) % this.m_Seasons.Length]];
        float startTime1 = season1.m_StartTime;
        float startTime2 = season2.m_StartTime;
        if (this.Intersect(startTime, elapsedTime, startTime1, startTime2))
          ++num;
      }
      return num;
    }

    private bool Intersect(float startTime, float elapsedTime, float seasonStart, float seasonEnd)
    {
      if ((double) seasonEnd < (double) seasonStart)
      {
        if ((double) startTime < (double) seasonEnd)
          ++startTime;
        ++seasonEnd;
      }
      if ((double) startTime > (double) seasonEnd)
        --startTime;
      return (double) startTime < (double) seasonEnd && (double) startTime + (double) elapsedTime > (double) seasonStart;
    }

    public (ClimateSystem.SeasonInfo, float, float) FindSeasonByTime(float time)
    {
      if (this.m_Seasons == null || this.m_Seasons.Length == 0)
        return ((ClimateSystem.SeasonInfo) null, 0.0f, 1f);
      if (this.m_Seasons.Length == 1)
        return (this.m_Seasons[0], 0.0f, 1f);
      for (int index = 0; index < this.m_SeasonsOrder.Length; ++index)
      {
        ClimateSystem.SeasonInfo season1 = this.m_Seasons[this.m_SeasonsOrder[index]];
        ClimateSystem.SeasonInfo season2 = this.m_Seasons[this.m_SeasonsOrder[(index + 1) % this.m_Seasons.Length]];
        float startTime1 = season1.m_StartTime;
        float startTime2 = season2.m_StartTime;
        if ((double) startTime2 < (double) startTime1)
          ++startTime2;
        if ((double) time >= (double) startTime1 && (double) time < (double) startTime2)
          return (season1, startTime1, startTime2);
        if ((double) startTime2 > 1.0 && (double) time < (double) startTime2 - 1.0)
          return (season1, startTime1, startTime2);
      }
      return (this.m_Seasons[0], 0.0f, 1f);
    }

    private float GetSeasonMidTime(int index)
    {
      ClimateSystem.SeasonInfo season1 = this.m_Seasons[this.m_SeasonsOrder[index]];
      ClimateSystem.SeasonInfo season2 = this.m_Seasons[this.m_SeasonsOrder[(index + 1) % this.m_Seasons.Length]];
      float startTime1 = season1.m_StartTime;
      float startTime2 = season2.m_StartTime;
      if ((double) startTime2 < (double) startTime1)
        ++startTime2;
      return (float) (((double) startTime1 + (double) startTime2) * 0.5 * 12.0 % 12.0);
    }

    public ClimatePrefab.SeasonPrecipCurves CreateSeasonPrecipCurves()
    {
      ClimatePrefab.SeasonPrecipCurves seasonPrecipCurves = new ClimatePrefab.SeasonPrecipCurves()
      {
        cloudChance = new AnimationCurve(),
        cloudAmountMin = new AnimationCurve(),
        cloudAmountMax = new AnimationCurve(),
        precipChance = new AnimationCurve(),
        precipAmountMin = new AnimationCurve(),
        precipAmountMax = new AnimationCurve(),
        turbulence = new AnimationCurve()
      };
      for (int index = 0; index < this.m_Seasons.Length; ++index)
      {
        (ClimateSystem.SeasonInfo seasonInfo, float time) = this.GetSeasonAndMidTime(index);
        float num1 = seasonInfo.m_CloudAmount * 0.01f;
        float num2 = math.abs(seasonInfo.m_CloudAmountDeviation) * 0.01f;
        float num3 = seasonInfo.m_CloudChance * 0.01f;
        float num4 = seasonInfo.m_PrecipitationAmount * 0.01f;
        float num5 = math.abs(seasonInfo.m_PrecipitationAmountDeviation) * 0.01f;
        float num6 = seasonInfo.m_PrecipitationChance * 0.01f;
        seasonPrecipCurves.cloudAmountMin.AddKey(time, num1 - num2);
        seasonPrecipCurves.cloudAmountMax.AddKey(time, num1 + num2);
        seasonPrecipCurves.cloudChance.AddKey(time, num3);
        seasonPrecipCurves.precipAmountMin.AddKey(time, num4 - num5);
        seasonPrecipCurves.precipAmountMax.AddKey(time, num4 + num5);
        seasonPrecipCurves.precipChance.AddKey(time, num6);
        seasonPrecipCurves.turbulence.AddKey(time, seasonInfo.m_Turbulence);
      }
      ClimatePrefab.LoopCurve(seasonPrecipCurves.cloudChance);
      ClimatePrefab.LoopCurve(seasonPrecipCurves.cloudAmountMin);
      ClimatePrefab.LoopCurve(seasonPrecipCurves.cloudAmountMax);
      ClimatePrefab.LoopCurve(seasonPrecipCurves.precipChance);
      ClimatePrefab.LoopCurve(seasonPrecipCurves.precipAmountMin);
      ClimatePrefab.LoopCurve(seasonPrecipCurves.precipAmountMax);
      ClimatePrefab.LoopCurve(seasonPrecipCurves.turbulence);
      return seasonPrecipCurves;
    }

    private static void LoopCurve(AnimationCurve curve, float minValue = 0.0f, float maxValue = 1f)
    {
      curve.preWrapMode = curve.postWrapMode = WrapMode.Loop;
      for (int index = 0; index < curve.length; ++index)
        curve.SmoothTangents(index, 0.333333343f);
      Keyframe[] keys = curve.keys;
      bool flag = false;
      for (int index = 0; index < keys.Length; ++index)
      {
        Keyframe keyframe = keys[index];
        if ((double) keyframe.value <= (double) minValue)
        {
          keyframe.value = minValue;
          keyframe.inTangent = keyframe.outTangent = 0.0f;
          keys[index] = keyframe;
          flag = true;
        }
        if ((double) keyframe.value >= (double) maxValue)
        {
          keyframe.value = maxValue;
          keyframe.inTangent = keyframe.outTangent = 0.0f;
          keys[index] = keyframe;
          flag = true;
        }
      }
      if (flag)
        curve.keys = keys;
      Keyframe key = keys[0] with
      {
        inTangent = 0.0f,
        outTangent = 0.0f
      };
      curve.MoveKey(0, key);
      key.time += 12f;
      curve.AddKey(key);
    }

    public ClimatePrefab.SeasonAuroraCurves CreateSeasonAuroraCurves()
    {
      ClimatePrefab.SeasonAuroraCurves seasonAuroraCurves = new ClimatePrefab.SeasonAuroraCurves()
      {
        amount = new AnimationCurve(),
        chance = new AnimationCurve()
      };
      for (int index = 0; index < this.m_Seasons.Length; ++index)
      {
        (ClimateSystem.SeasonInfo seasonInfo, float time) = this.GetSeasonAndMidTime(index);
        seasonAuroraCurves.amount.AddKey(time, seasonInfo.m_AuroraAmount);
        seasonAuroraCurves.chance.AddKey(time, seasonInfo.m_AuroraChance * 0.01f);
      }
      ClimatePrefab.LoopCurve(seasonAuroraCurves.amount, maxValue: 10f);
      ClimatePrefab.LoopCurve(seasonAuroraCurves.chance);
      return seasonAuroraCurves;
    }

    public Bounds1 temperatureRange
    {
      get
      {
        Bounds1 temperatureRange = new Bounds1(float.MaxValue, float.MinValue);
        for (int index = 0; index < 288; ++index)
          temperatureRange |= this.m_Temperature.Evaluate((float) ((double) index / 288.0 * 12.0));
        return temperatureRange;
      }
    }

    public float averageCloudiness
    {
      get
      {
        float num = 0.0f;
        for (int index = 0; index < 288; ++index)
        {
          float time = (float) ((double) index / 288.0 * 12.0);
          num += this.m_Cloudiness.Evaluate(time);
        }
        return num / 288f;
      }
    }

    public float averagePrecipitation
    {
      get
      {
        float num = 0.0f;
        for (int index = 0; index < 288; ++index)
        {
          float time = (float) ((double) index / 288.0 * 12.0);
          num += this.m_Precipitation.Evaluate(time);
        }
        return num / 288f;
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ClimateData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Seasons != null)
      {
        foreach (ClimateSystem.SeasonInfo season in this.m_Seasons)
          prefabs.Add((PrefabBase) season.m_Prefab);
      }
      if ((UnityEngine.Object) this.m_DefaultWeather != (UnityEngine.Object) null)
        prefabs.Add((PrefabBase) this.m_DefaultWeather);
      if (this.m_DefaultWeathers == null)
        return;
      foreach (WeatherPrefab defaultWeather in this.m_DefaultWeathers)
      {
        if (defaultWeather.active)
          prefabs.Add((PrefabBase) defaultWeather);
      }
    }

    public struct SeasonTempCurves
    {
      public AnimationCurve nightMin;
      public AnimationCurve nightMax;
      public AnimationCurve dayMin;
      public AnimationCurve dayMax;
    }

    public struct SeasonPrecipCurves
    {
      public AnimationCurve cloudChance;
      public AnimationCurve cloudAmountMin;
      public AnimationCurve cloudAmountMax;
      public AnimationCurve precipChance;
      public AnimationCurve precipAmountMin;
      public AnimationCurve precipAmountMax;
      public AnimationCurve turbulence;
    }

    public struct SeasonAuroraCurves
    {
      public AnimationCurve amount;
      public AnimationCurve chance;
    }
  }
}
