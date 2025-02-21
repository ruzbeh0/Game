// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ClimateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Effects;
using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Rendering;
using Game.Serialization;
using Game.Triggers;
using System;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  public class ClimateSystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPreSerialize,
    IPostDeserialize
  {
    public OverridableProperty<float> thunder = new OverridableProperty<float>();
    private TriggerSystem m_TriggerSystem;
    private PrefabSystem m_PrefabSystem;
    private TimeSystem m_TimeSystem;
    private ClimateRenderSystem m_ClimateRenderSystem;
    private EntityQuery m_ClimateQuery;
    private OverridableProperty<float> m_Date;
    private Entity m_CurrentClimate;
    private NativeList<Entity> m_CurrentWeatherEffects;
    private NativeList<Entity> m_NextWeatherEffects;
    private float m_TemperatureBaseHeight;
    private ClimateSystem.SeasonInfo m_CurrentSeason;
    private static readonly int[,] kLut = new int[12, 5]
    {
      {
        33,
        15,
        32,
        10,
        10
      },
      {
        31,
        18,
        31,
        10,
        10
      },
      {
        31,
        21,
        28,
        10,
        10
      },
      {
        23,
        18,
        30,
        10,
        19
      },
      {
        22,
        20,
        23,
        10,
        25
      },
      {
        21,
        19,
        24,
        10,
        26
      },
      {
        19,
        18,
        26,
        10,
        27
      },
      {
        18,
        22,
        23,
        10,
        27
      },
      {
        25,
        23,
        24,
        10,
        18
      },
      {
        29,
        19,
        32,
        10,
        10
      },
      {
        30,
        16,
        34,
        10,
        10
      },
      {
        34,
        15,
        31,
        10,
        10
      }
    };
    private static readonly int[] kSampleTimes = new int[3]
    {
      7,
      13,
      19
    };

    public float2 wind { get; private set; } = new float2(0.0275f, 0.0275f);

    public float hail { get; set; }

    public float rainbow { get; set; }

    public float aerosolDensity { get; private set; }

    public float seasonTemperature { get; private set; }

    public float seasonPrecipitation { get; private set; }

    public float seasonCloudiness { get; private set; }

    public OverridableProperty<float> currentDate => this.m_Date;

    public OverridableProperty<float> precipitation { get; } = new OverridableProperty<float>();

    public OverridableProperty<float> temperature { get; } = new OverridableProperty<float>();

    public OverridableProperty<float> cloudiness { get; } = new OverridableProperty<float>();

    public OverridableProperty<float> aurora { get; } = new OverridableProperty<float>();

    public OverridableProperty<float> fog { get; } = new OverridableProperty<float>();

    public Entity currentClimate
    {
      get => this.m_CurrentClimate;
      set
      {
        Assert.AreNotEqual<Entity>(Entity.Null, value);
        this.m_CurrentClimate = value;
        ClimatePrefab prefab = this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_CurrentClimate);
        prefab.EnsureSeasonsOrder(true);
        this.averageTemperature = this.CalculateTemperatureAverage(prefab);
        this.UpdateSeason(prefab, (float) this.m_Date);
        if (this.m_CurrentWeatherEffects.Length != 0 || this.m_NextWeatherEffects.Length != 0)
          return;
        this.UpdateWeather(prefab);
      }
    }

    public float temperatureBaseHeight => this.m_TemperatureBaseHeight;

    public float snowTemperatureHeightScale => 0.01f;

    public float averageTemperature { get; private set; }

    public float freezingTemperature { get; private set; }

    public bool isRaining
    {
      get
      {
        return (double) (float) this.precipitation > 0.0 && (double) (float) this.temperature > (double) this.freezingTemperature;
      }
    }

    public bool isSnowing
    {
      get
      {
        return (double) (float) this.precipitation > 0.0 && (double) (float) this.temperature <= (double) this.freezingTemperature;
      }
    }

    public bool isPrecipitating => (double) (float) this.precipitation > 0.0;

    public ClimateSystem.WeatherClassification classification { get; private set; }

    public Entity currentSeason
    {
      get
      {
        return this.m_CurrentSeason == null ? Entity.Null : this.m_PrefabSystem.GetEntity((PrefabBase) this.m_CurrentSeason.m_Prefab);
      }
    }

    public string currentSeasonNameID => this.m_CurrentSeason?.m_NameID;

    public void PatchReference(ref PrefabReferences references)
    {
      this.m_CurrentClimate = references.Check(this.EntityManager, this.m_CurrentClimate);
      for (int index = 0; index < this.m_CurrentWeatherEffects.Length; ++index)
        this.m_CurrentWeatherEffects[index] = references.Check(this.EntityManager, this.m_CurrentWeatherEffects[index]);
      for (int index = 0; index < this.m_NextWeatherEffects.Length; ++index)
        this.m_NextWeatherEffects[index] = references.Check(this.EntityManager, this.m_NextWeatherEffects[index]);
    }

    private float CalculateMeanTemperatureStandard(
      ClimatePrefab prefab,
      int resolutionPerDay,
      out float meanMin,
      out float meanMax)
    {
      if (prefab.m_Seasons != null)
      {
        int length = prefab.m_Temperature.length;
        int num = this.m_TimeSystem.daysPerYear * resolutionPerDay;
        float2 zero1 = float2.zero;
        for (int index1 = 0; index1 < this.m_TimeSystem.daysPerYear; ++index1)
        {
          float2 zero2 = float2.zero;
          for (int index2 = 0; index2 < resolutionPerDay; ++index2)
          {
            float time = (float) (index1 + index2) / (float) num * (float) length;
            float y = prefab.m_Temperature.Evaluate(time);
            zero2.x = math.min(zero2.x, y);
            zero2.y = math.max(zero2.y, y);
          }
          zero1 += zero2;
        }
        float2 float2 = zero1 / (float) this.m_TimeSystem.daysPerYear;
        meanMin = float2.x;
        meanMax = float2.y;
        return (float) (((double) float2.x + (double) float2.y) * 0.5);
      }
      meanMin = 0.0f;
      meanMax = 0.0f;
      return 0.0f;
    }

    private float CalculateMeanTemperatureEkholmModen(ClimatePrefab prefab, int resolutionPerDay)
    {
      Assert.AreEqual(12, this.m_TimeSystem.daysPerYear);
      if (prefab.m_Seasons == null)
        return 0.0f;
      int daysPerYear = this.m_TimeSystem.daysPerYear;
      int num1 = daysPerYear + (ClimateSystem.kSampleTimes.Length + 2);
      float num2 = 0.0f;
      for (int index1 = 0; index1 < daysPerYear; ++index1)
      {
        float num3 = 0.0f;
        for (int index2 = 0; index2 < ClimateSystem.kSampleTimes.Length; ++index2)
        {
          float time = (float) (ClimateSystem.kSampleTimes[index2] + index1) / (float) num1 * (float) daysPerYear;
          float num4 = prefab.m_Temperature.Evaluate(time);
          num3 += num4 * (float) ClimateSystem.kLut[index1, index2];
        }
        float2 zero = float2.zero;
        for (int index3 = 0; index3 < resolutionPerDay; ++index3)
        {
          float time = (float) (index1 + index3 - 5) / (float) num1 * (float) daysPerYear;
          float y = prefab.m_Temperature.Evaluate(time);
          zero.x = math.min(zero.x, y);
          zero.y = math.max(zero.y, y);
        }
        float num5 = num3 + zero.x * (float) ClimateSystem.kLut[index1, 3] + zero.y * (float) ClimateSystem.kLut[index1, 3];
        num2 += num5 / 100f;
      }
      return num2 / (float) this.m_TimeSystem.daysPerYear;
    }

    private float CalculateMeanPrecipitation(
      ClimatePrefab prefab,
      int resolutionPerDay = 48,
      float startRange = 0.0f,
      float endRange = 1f)
    {
      int daysPerYear = this.m_TimeSystem.daysPerYear;
      float x = startRange * (float) daysPerYear;
      float y = endRange * (float) daysPerYear;
      int num1 = (int) math.round((y - x) * (float) resolutionPerDay);
      float num2 = 0.0f;
      for (int index = 0; index < num1; ++index)
      {
        float s = (float) index / (float) num1;
        num2 += prefab.m_Precipitation.Evaluate(math.lerp(x, y, s));
      }
      return num2 / (float) num1;
    }

    private float CalculateMeanTemperature(
      ClimatePrefab prefab,
      int resolutionPerDay = 48,
      float startRange = 0.0f,
      float endRange = 1f)
    {
      int daysPerYear = this.m_TimeSystem.daysPerYear;
      float x = startRange * (float) daysPerYear;
      float y1 = endRange * (float) daysPerYear;
      int num = (int) math.round((y1 - x) * (float) resolutionPerDay);
      float2 zero = float2.zero;
      for (int index = 0; index < num; ++index)
      {
        float s = (float) index / (float) num;
        float y2 = prefab.m_Temperature.Evaluate(math.lerp(x, y1, s));
        zero.x = math.min(zero.x, y2);
        zero.y = math.max(zero.y, y2);
      }
      return (float) (((double) zero.x + (double) zero.y) * 0.5);
    }

    private float CalculateMeanCloudiness(
      ClimatePrefab prefab,
      int resolutionPerDay = 48,
      float startRange = 0.0f,
      float endRange = 1f)
    {
      int daysPerYear = this.m_TimeSystem.daysPerYear;
      float x = startRange * (float) daysPerYear;
      float y = endRange * (float) daysPerYear;
      int num1 = (int) math.round((y - x) * (float) resolutionPerDay);
      float num2 = 0.0f;
      for (int index = 0; index < num1; ++index)
      {
        float s = (float) index / (float) num1;
        num2 += prefab.m_Cloudiness.Evaluate(math.lerp(x, y, s));
      }
      return num2 / (float) num1;
    }

    private float CalculateTemperatureAverage(ClimatePrefab prefab, int resolutionPerDay = 48)
    {
      this.freezingTemperature = prefab.m_FreezingTemperature;
      return this.m_TimeSystem.daysPerYear == 12 ? this.CalculateMeanTemperatureEkholmModen(prefab, resolutionPerDay) : this.CalculateMeanTemperatureStandard(prefab, resolutionPerDay, out float _, out float _);
    }

    private float CalculateTemperatureBaseHeight()
    {
      // ISSUE: variable of a compiler-generated type
      TerrainSystem systemManaged1 = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: variable of a compiler-generated type
      WaterSystem systemManaged2 = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: variable of a compiler-generated type
      MapTileSystem systemManaged3 = this.World.GetOrCreateSystemManaged<MapTileSystem>();
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = systemManaged1.GetHeightData();
      JobHandle deps;
      // ISSUE: reference to a compiler-generated method
      WaterSurfaceData surfaceData = systemManaged2.GetSurfaceData(out deps);
      deps.Complete();
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> startTiles = systemManaged3.GetStartTiles();
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index1 = 0; index1 < startTiles.Length; ++index1)
      {
        DynamicBuffer<Node> buffer;
        if (this.EntityManager.TryGetBuffer<Node>(startTiles[index1], true, out buffer))
        {
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            num1 += WaterUtils.SampleHeight(ref surfaceData, ref heightData, buffer[index2].m_Position);
            ++num2;
          }
        }
      }
      return (double) num2 <= 0.0 ? 0.0f : num1 / num2;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      this.m_ClimateRenderSystem = this.World.GetOrCreateSystemManaged<ClimateRenderSystem>();
      this.m_CurrentWeatherEffects = new NativeList<Entity>(0, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_NextWeatherEffects = new NativeList<Entity>(0, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_ClimateQuery = this.GetEntityQuery(ComponentType.ReadOnly<ClimateData>());
      this.m_Date = new OverridableProperty<float>((Func<float>) (() => this.m_TimeSystem.normalizedDate));
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.m_CurrentWeatherEffects.Dispose();
      this.m_NextWeatherEffects.Dispose();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.currentClimate != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        ClimatePrefab prefab = this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.currentClimate);
        ClimateSystem.ClimateSample climateSample = this.SampleClimate(prefab, (float) this.m_Date);
        this.temperature.value = climateSample.temperature;
        this.precipitation.value = climateSample.precipitation;
        this.cloudiness.value = climateSample.cloudiness;
        this.aurora.value = climateSample.aurora;
        this.fog.value = climateSample.fog;
        this.UpdateSeason(prefab, (float) this.m_Date);
        this.UpdateWeather(prefab);
      }
      if (!this.m_TriggerSystem.Enabled)
        return;
      this.HandleTriggers();
    }

    private void HandleTriggers()
    {
      // ISSUE: reference to a compiler-generated method
      NativeQueue<TriggerAction> actionBuffer = this.m_TriggerSystem.CreateActionBuffer();
      actionBuffer.Enqueue(new TriggerAction(TriggerType.Temperature, Entity.Null, (float) this.temperature));
      bool flag1 = (double) this.hail > 1.0 / 1000.0;
      bool flag2 = this.classification == ClimateSystem.WeatherClassification.Overcast;
      bool flag3 = (double) this.m_TimeSystem.normalizedTime >= (double) EffectFlagSystem.kDayBegin && (double) this.m_TimeSystem.normalizedTime < (double) EffectFlagSystem.kNightBegin;
      bool flag4 = this.classification == ClimateSystem.WeatherClassification.Stormy;
      if (flag1 | flag4)
        actionBuffer.Enqueue(new TriggerAction(TriggerType.WeatherStormy, Entity.Null, 0.0f));
      else if (!flag1 && this.isRaining)
        actionBuffer.Enqueue(new TriggerAction((double) (float) this.temperature > 0.0 ? TriggerType.WeatherRainy : TriggerType.WeatherSnowy, Entity.Null, 0.0f));
      else if (((flag1 || this.isRaining ? 0 : (!flag2 ? 1 : 0)) & (flag3 ? 1 : 0)) != 0)
        actionBuffer.Enqueue(new TriggerAction((double) (float) this.temperature > 15.0 ? TriggerType.WeatherSunny : TriggerType.WeatherClear, Entity.Null, 0.0f));
      else if (flag2)
        actionBuffer.Enqueue(new TriggerAction(TriggerType.WeatherCloudy, Entity.Null, 0.0f));
      if (flag3)
        return;
      actionBuffer.Enqueue(new TriggerAction(TriggerType.AuroraBorealis, Entity.Null, (float) this.aurora));
    }

    public void PreSerialize(Colossal.Serialization.Entities.Context context)
    {
      if (context.purpose != Colossal.Serialization.Entities.Purpose.SaveMap)
        return;
      this.m_TemperatureBaseHeight = this.CalculateTemperatureBaseHeight();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_CurrentClimate);
      writer.Write(this.m_CurrentWeatherEffects);
      writer.Write(this.m_NextWeatherEffects);
      writer.Write(this.m_TemperatureBaseHeight);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      this.m_CurrentClimate = Entity.Null;
      this.m_CurrentWeatherEffects.ResizeUninitialized(0);
      this.m_NextWeatherEffects.ResizeUninitialized(0);
      this.m_TemperatureBaseHeight = 0.0f;
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_CurrentClimate);
      reader.Read(this.m_CurrentWeatherEffects);
      reader.Read(this.m_NextWeatherEffects);
      reader.Read(out this.m_TemperatureBaseHeight);
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.m_CurrentClimate == Entity.Null || !this.m_PrefabSystem.TryGetPrefab<ClimatePrefab>(this.m_CurrentClimate, out ClimatePrefab _))
      {
        if (this.m_CurrentClimate != Entity.Null)
          COSystemBase.baseLog.Error((object) "Missing climate prefab, reverting to default climate");
        using (NativeArray<Entity> entityArray = this.m_ClimateQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          if (entityArray.Length > 0)
            this.m_CurrentClimate = entityArray[0];
        }
      }
      if (!(this.m_CurrentClimate != Entity.Null))
        return;
      // ISSUE: reference to a compiler-generated method
      ClimatePrefab prefab = this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_CurrentClimate);
      prefab.EnsureSeasonsOrder(true);
      this.averageTemperature = this.CalculateTemperatureAverage(prefab);
      this.UpdateSeason(prefab, (float) this.m_Date);
      if (this.AreEffectsInvalid(this.m_CurrentWeatherEffects) || this.m_CurrentWeatherEffects.Length == 0 || this.AreEffectsInvalid(this.m_NextWeatherEffects) || this.m_NextWeatherEffects.Length == 0)
        this.UpdateWeather(prefab);
      else
        this.ApplyWeatherEffects();
    }

    private bool AreEffectsInvalid(NativeList<Entity> list)
    {
      for (int index = 0; index < list.Length; ++index)
      {
        if (list[index] == Entity.Null)
        {
          list.ResizeUninitialized(0);
          return false;
        }
      }
      return true;
    }

    public ClimateSystem.ClimateSample SampleClimate(ClimatePrefab prefab, float t)
    {
      float time = t * (float) this.m_TimeSystem.daysPerYear;
      float num1 = prefab.m_Temperature.Evaluate(time);
      float num2 = prefab.m_Precipitation.Evaluate(time);
      float num3 = prefab.m_Cloudiness.Evaluate(time);
      float num4 = prefab.m_Aurora.Evaluate(time);
      float num5 = prefab.m_Aurora.Evaluate(time);
      return new ClimateSystem.ClimateSample()
      {
        temperature = num1,
        precipitation = num2,
        cloudiness = num3,
        aurora = num4,
        fog = num5
      };
    }

    public ClimateSystem.ClimateSample SampleClimate(float t)
    {
      if (!(this.m_CurrentClimate != Entity.Null))
        return new ClimateSystem.ClimateSample();
      // ISSUE: reference to a compiler-generated method
      ClimateSystem.ClimateSample climateSample = this.SampleClimate(this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_CurrentClimate), t);
      if (this.temperature.overrideState)
        climateSample.temperature = this.temperature.overrideValue;
      if (this.precipitation.overrideState)
        climateSample.precipitation = this.precipitation.overrideValue;
      if (this.cloudiness.overrideState)
        climateSample.cloudiness = this.cloudiness.overrideValue;
      if (this.aurora.overrideState)
        climateSample.aurora = this.aurora.overrideValue;
      if (this.fog.overrideState)
        climateSample.fog = this.fog.overrideValue;
      return climateSample;
    }

    private void UpdateSeason(ClimatePrefab prefab, float normalizedDate)
    {
      ClimateSystem.SeasonInfo currentSeason1 = this.m_CurrentSeason;
      float startRange;
      float endRange;
      (this.m_CurrentSeason, startRange, endRange) = prefab.FindSeasonByTime(normalizedDate);
      ClimateSystem.SeasonInfo currentSeason2 = this.m_CurrentSeason;
      if (currentSeason1 == currentSeason2)
        return;
      this.seasonTemperature = this.CalculateMeanTemperature(prefab, startRange: startRange, endRange: endRange);
      this.seasonPrecipitation = this.CalculateMeanPrecipitation(prefab, startRange: startRange, endRange: endRange);
      this.seasonCloudiness = this.CalculateMeanCloudiness(prefab, startRange: startRange, endRange: endRange);
    }

    private bool SelectDefaultWeather(
      ClimatePrefab prefab,
      ref NativeList<ClimateSystem.WeatherTempData> currentWeathers,
      ref NativeList<ClimateSystem.WeatherTempData> nextWeathers)
    {
      if (!((UnityEngine.Object) prefab.m_DefaultWeather != (UnityEngine.Object) null))
        return false;
      // ISSUE: reference to a compiler-generated method
      ClimateSystem.WeatherTempData weatherTempData = new ClimateSystem.WeatherTempData()
      {
        m_Entity = this.m_PrefabSystem.GetEntity((PrefabBase) prefab.m_DefaultWeather),
        m_Priority = -1001f
      };
      currentWeathers.Add(in weatherTempData);
      nextWeathers.Add(in weatherTempData);
      return true;
    }

    private bool SelectWeatherPlaceholder(
      ClimatePrefab prefab,
      out WeatherPrefab current,
      out WeatherPrefab next)
    {
      if (prefab.m_DefaultWeathers != null)
      {
        float num1 = float.MaxValue;
        int index1 = 0;
        for (int index2 = 0; index2 < prefab.m_DefaultWeathers.Length; ++index2)
        {
          WeatherPrefab defaultWeather = prefab.m_DefaultWeathers[index2];
          float num2 = math.max(defaultWeather.m_CloudinessRange.x - (float) this.cloudiness, (float) this.cloudiness - defaultWeather.m_CloudinessRange.y);
          if ((double) num2 < (double) num1)
          {
            index1 = index2;
            num1 = num2;
          }
        }
        current = prefab.m_DefaultWeathers[math.max(index1 - 1, 0)];
        next = prefab.m_DefaultWeathers[index1];
        return true;
      }
      current = (WeatherPrefab) null;
      next = (WeatherPrefab) null;
      return false;
    }

    private void SelectRandomWeather(
      WeatherPrefab weather,
      ref NativeList<ClimateSystem.WeatherTempData> weathers)
    {
      ClimateSystem.WeatherTempData weatherTempData1;
      // ISSUE: reference to a compiler-generated method
      weatherTempData1.m_Entity = this.m_PrefabSystem.GetEntity((PrefabBase) weather);
      weatherTempData1.m_Priority = -1000f;
      weathers.Add(in weatherTempData1);
      DynamicBuffer<PlaceholderObjectElement> buffer1;
      if (!this.EntityManager.TryGetBuffer<PlaceholderObjectElement>(weatherTempData1.m_Entity, true, out buffer1))
        return;
      for (int index1 = 0; index1 < buffer1.Length; ++index1)
      {
        ClimateSystem.WeatherTempData weatherTempData2;
        weatherTempData2.m_Entity = buffer1[index1].m_Object;
        weatherTempData2.m_Priority = 0.0f;
        DynamicBuffer<ObjectRequirementElement> buffer2;
        if (this.EntityManager.TryGetBuffer<ObjectRequirementElement>(weatherTempData2.m_Entity, true, out buffer2))
        {
          int num = -1;
          bool flag = true;
          for (int index2 = 0; index2 < buffer2.Length; ++index2)
          {
            ObjectRequirementElement requirementElement = buffer2[index2];
            if ((requirementElement.m_Type & ObjectRequirementType.SelectOnly) == (ObjectRequirementType) 0)
            {
              if ((int) requirementElement.m_Group != num)
              {
                if (flag)
                {
                  num = (int) requirementElement.m_Group;
                  flag = false;
                }
                else
                  break;
              }
              flag |= requirementElement.m_Requirement == this.currentSeason;
              weatherTempData2.m_Priority = 1000f;
            }
          }
          if (!flag)
            continue;
        }
        // ISSUE: reference to a compiler-generated method
        WeatherPrefab prefab = this.m_PrefabSystem.GetPrefab<WeatherPrefab>(weatherTempData2.m_Entity);
        if ((double) (float) this.aurora > 0.0 && prefab.m_RandomizationLayer == WeatherPrefab.RandomizationLayer.Aurora)
        {
          weatherTempData2.m_Priority = 500f;
          weathers.Add(in weatherTempData2);
        }
        else if (prefab.m_RandomizationLayer == WeatherPrefab.RandomizationLayer.Cloudiness)
        {
          weatherTempData2.m_Priority = 250f;
          weathers.Add(in weatherTempData2);
        }
        else if (prefab.m_RandomizationLayer == WeatherPrefab.RandomizationLayer.Season)
        {
          weatherTempData2.m_Priority = 300f;
          weathers.Add(in weatherTempData2);
        }
      }
    }

    private bool ResetWeatherEffects(ref NativeList<Entity> weatherEffects)
    {
      int num = weatherEffects.Length != 0 ? 1 : 0;
      weatherEffects.Clear();
      return num != 0;
    }

    private bool SortAndCheckUpdate(
      ref NativeList<ClimateSystem.WeatherTempData> weatherEffects,
      ref NativeList<Entity> reference)
    {
      bool flag = false;
      weatherEffects.Sort<ClimateSystem.WeatherTempData>();
      if (weatherEffects.Length != reference.Length)
      {
        flag = true;
        reference.ResizeUninitialized(weatherEffects.Length);
        for (int index = 0; index < weatherEffects.Length; ++index)
          reference[index] = weatherEffects[index].m_Entity;
      }
      else
      {
        for (int index = 0; index < weatherEffects.Length; ++index)
        {
          flag |= reference[index] != weatherEffects[index].m_Entity;
          reference[index] = weatherEffects[index].m_Entity;
        }
      }
      return flag;
    }

    private void UpdateWeather(ClimatePrefab prefab)
    {
      bool flag = false;
      NativeList<ClimateSystem.WeatherTempData> nativeList1 = new NativeList<ClimateSystem.WeatherTempData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      NativeList<ClimateSystem.WeatherTempData> nativeList2 = new NativeList<ClimateSystem.WeatherTempData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (this.SelectDefaultWeather(prefab, ref nativeList1, ref nativeList2))
      {
        WeatherPrefab current;
        WeatherPrefab next;
        if (this.SelectWeatherPlaceholder(prefab, out current, out next))
        {
          this.SelectRandomWeather(current, ref nativeList1);
          this.SelectRandomWeather(next, ref nativeList2);
          flag = flag | this.SortAndCheckUpdate(ref nativeList1, ref this.m_CurrentWeatherEffects) | this.SortAndCheckUpdate(ref nativeList2, ref this.m_NextWeatherEffects);
        }
      }
      else
        flag = flag | this.ResetWeatherEffects(ref this.m_CurrentWeatherEffects) | this.ResetWeatherEffects(ref this.m_NextWeatherEffects);
      nativeList1.Dispose();
      nativeList2.Dispose();
      if (!flag)
        return;
      this.ApplyWeatherEffects();
    }

    private void ApplyWeatherEffects()
    {
      // ISSUE: reference to a compiler-generated method
      this.m_ClimateRenderSystem.Clear();
      for (int index = 0; index < this.m_CurrentWeatherEffects.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        WeatherPrefab prefab = this.m_PrefabSystem.GetPrefab<WeatherPrefab>(this.m_CurrentWeatherEffects[index]);
        if (prefab.m_Classification != ClimateSystem.WeatherClassification.Irrelevant)
          this.classification = prefab.m_Classification;
        // ISSUE: reference to a compiler-generated method
        this.m_ClimateRenderSystem.ScheduleFrom(prefab);
      }
      for (int index = 0; index < this.m_NextWeatherEffects.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        WeatherPrefab prefab = this.m_PrefabSystem.GetPrefab<WeatherPrefab>(this.m_NextWeatherEffects[index]);
        if (prefab.m_Classification != ClimateSystem.WeatherClassification.Irrelevant)
          this.classification = prefab.m_Classification;
        // ISSUE: reference to a compiler-generated method
        this.m_ClimateRenderSystem.ScheduleTo(prefab);
      }
    }

    [Preserve]
    public ClimateSystem()
    {
    }

    [Serializable]
    public class SeasonInfo : IJsonWritable, IJsonReadable
    {
      public SeasonPrefab m_Prefab;
      public string m_NameID;
      public string m_IconPath;
      public float m_StartTime;
      public float2 m_TempNightDay = new float2(5f, 20f);
      public float2 m_TempDeviationNightDay = new float2(4f, 7f);
      public float m_CloudChance = 50f;
      public float m_CloudAmount = 40f;
      public float m_CloudAmountDeviation = 20f;
      public float m_PrecipitationChance = 30f;
      public float m_PrecipitationAmount = 40f;
      public float m_PrecipitationAmountDeviation = 30f;
      public float m_Turbulence = 0.2f;
      public float m_AuroraAmount = 1f;
      public float m_AuroraChance = 10f;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("Season");
        writer.PropertyName("name");
        writer.Write(this.m_NameID);
        writer.PropertyName("startTime");
        writer.Write(this.m_StartTime);
        writer.PropertyName("tempNightDay");
        writer.Write(this.m_TempNightDay);
        writer.PropertyName("tempDeviationNightDay");
        writer.Write(this.m_TempDeviationNightDay);
        writer.PropertyName("cloudChance");
        writer.Write(this.m_CloudChance);
        writer.PropertyName("cloudAmount");
        writer.Write(this.m_CloudAmount);
        writer.PropertyName("cloudAmountDeviation");
        writer.Write(this.m_CloudAmountDeviation);
        writer.PropertyName("precipitationChance");
        writer.Write(this.m_PrecipitationChance);
        writer.PropertyName("precipitationAmount");
        writer.Write(this.m_PrecipitationAmount);
        writer.PropertyName("precipitationAmountDeviation");
        writer.Write(this.m_PrecipitationAmountDeviation);
        writer.PropertyName("turbulence");
        writer.Write(this.m_Turbulence);
        writer.PropertyName("auroraAmount");
        writer.Write(this.m_AuroraAmount);
        writer.PropertyName("auroraChance");
        writer.Write(this.m_AuroraChance);
        writer.TypeEnd();
      }

      public void Read(IJsonReader reader)
      {
        long num = (long) reader.ReadMapBegin();
        reader.ReadProperty("name");
        reader.Read(out this.m_NameID);
        reader.ReadProperty("startTime");
        reader.Read(out this.m_StartTime);
        reader.ReadProperty("tempNightDay");
        reader.Read(out this.m_TempNightDay);
        reader.ReadProperty("tempDeviationNightDay");
        reader.Read(out this.m_TempDeviationNightDay);
        reader.ReadProperty("cloudChance");
        reader.Read(out this.m_CloudChance);
        reader.ReadProperty("cloudAmount");
        reader.Read(out this.m_CloudAmount);
        reader.ReadProperty("cloudAmountDeviation");
        reader.Read(out this.m_CloudAmountDeviation);
        reader.ReadProperty("precipitationChance");
        reader.Read(out this.m_PrecipitationChance);
        reader.ReadProperty("precipitationAmount");
        reader.Read(out this.m_PrecipitationAmount);
        reader.ReadProperty("precipitationAmountDeviation");
        reader.Read(out this.m_PrecipitationAmountDeviation);
        reader.ReadProperty("turbulence");
        reader.Read(out this.m_Turbulence);
        reader.ReadProperty("auroraAmount");
        reader.Read(out this.m_AuroraAmount);
        reader.ReadProperty("auroraChance");
        reader.Read(out this.m_AuroraChance);
        reader.ReadMapEnd();
      }
    }

    public enum WeatherClassification
    {
      Irrelevant,
      Clear,
      Few,
      Scattered,
      Broken,
      Overcast,
      Stormy,
    }

    public struct ClimateSample
    {
      public float temperature;
      public float precipitation;
      public float cloudiness;
      public float aurora;
      public float fog;
    }

    private struct WeatherTempData : IComparable<ClimateSystem.WeatherTempData>
    {
      public Entity m_Entity;
      public float m_Priority;

      public int CompareTo(ClimateSystem.WeatherTempData other)
      {
        return this.m_Priority.CompareTo(other.m_Priority);
      }
    }
  }
}
