// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PlanetarySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Atmosphere;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using Game.SceneFlow;
using Game.Settings;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PlanetarySystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private SunMoonData m_SunMoonData;
    private TimeSystem m_TimeSystem;
    private PlanetarySystem.LightData m_SunLight;
    private PlanetarySystem.LightData m_MoonLight;
    private PlanetarySystem.LightData m_NightLight;
    private RenderingSystem m_RenderingSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private PrefabSystem m_PrefabSystem;
    private const float kDaysInYear = 365f;
    private const float kInvDaysInYear = 0.002739726f;
    private const float kHoursInDay = 24f;
    private const float kInvHoursInDay = 0.0416666679f;
    private const float kSecsInMin = 60f;
    private const float kInvSecsInMin = 0.0166666675f;
    private const float kSecsInHour = 3600f;
    private const float kInvSecsInHour = 0.000277777785f;
    private const float kLunarCyclesPerYear = 12f;
    private const float kInvLunarCyclesPerYear = 0.0833333358f;
    private int m_Year = 2020;
    private int m_Day = (int) sbyte.MaxValue;
    private int m_Hour = 12;
    private int m_Minute;
    private float m_Second;
    private float m_Latitude = 41.9028f;
    private float m_Longitude = 12.4964f;
    private int m_NumberOfLunarCyclesPerYear = 1;
    private RenderTexture m_MoonTexture;
    private Material m_MoonMaterial;
    private int m_ClearPass;
    private int m_LitPass;
    private Vector2 m_OrenNayarCoefficients;
    private float m_SurfaceRoughness;
    private PlanetarySystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1383560598_0;
    private EntityQuery __query_1383560598_1;
    private EntityQuery __query_1383560598_2;

    public PlanetarySystem.LightData SunLight => this.m_SunLight;

    public PlanetarySystem.LightData MoonLight => this.m_MoonLight;

    public PlanetarySystem.LightData NightLight => this.m_NightLight;

    public bool overrideTime { get; set; }

    public float latitude
    {
      get => this.m_Latitude;
      set => this.m_Latitude = math.clamp(value, -90f, 90f);
    }

    public float longitude
    {
      get => this.m_Longitude;
      set => this.m_Longitude = math.clamp(value, -180f, 180f);
    }

    public float debugTimeMultiplier { get; set; } = 1f;

    public int year
    {
      get => this.m_Year;
      set => this.m_Year = value;
    }

    public int day
    {
      get => this.m_Day;
      set => this.m_Day = value;
    }

    public int hour
    {
      get => this.m_Hour;
      set => this.m_Hour = value;
    }

    public int minute
    {
      get => this.m_Minute;
      set => this.m_Minute = value;
    }

    public float second
    {
      get => this.m_Second;
      set => this.m_Second = value;
    }

    public float time
    {
      get
      {
        return (float) ((double) this.hour + (double) this.minute * 0.01666666753590107 + (double) this.second * 0.00027777778450399637);
      }
      set
      {
        this.hour = Mathf.FloorToInt(value);
        value -= (float) this.hour;
        this.minute = Mathf.FloorToInt(value * 60f);
        value -= (float) this.minute * 0.0166666675f;
        this.second = value * 3600f;
      }
    }

    public float dayOfYear
    {
      get => (float) this.day + this.normalizedTime;
      set
      {
        this.day = Mathf.FloorToInt(value);
        value -= (float) this.day;
        this.normalizedTime = value;
      }
    }

    public float normalizedDayOfYear
    {
      get => (float) (((double) this.dayOfYear - 1.0) * (1.0 / 365.0));
      set => this.dayOfYear = (float) ((double) value * 365.0 + 1.0);
    }

    public float normalizedTime
    {
      get => this.time * 0.0416666679f;
      set => this.time = value * 24f;
    }

    public int numberOfLunarCyclesPerYear
    {
      get => this.m_NumberOfLunarCyclesPerYear;
      set => this.m_NumberOfLunarCyclesPerYear = math.max(0, value);
    }

    public int moonDay
    {
      get
      {
        return Mathf.FloorToInt((float) this.day * 0.0833333358f * (float) this.numberOfLunarCyclesPerYear);
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      RenderTexture renderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
      renderTexture.name = "MoonTexture";
      renderTexture.hideFlags = HideFlags.DontSave;
      // ISSUE: reference to a compiler-generated field
      this.m_MoonTexture = renderTexture;
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Satellites"));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ClearPass = this.m_MoonMaterial.FindPass("Clear");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LitPass = this.m_MoonMaterial.FindPass("LitSatellite");
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_SunLight = new PlanetarySystem.LightData("SunLight");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_MoonLight = new PlanetarySystem.LightData("MoonLight");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_NightLight = new PlanetarySystem.LightData("NightLight");
      // ISSUE: reference to a compiler-generated field
      this.m_SunMoonData = new SunMoonData();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_MoonTexture);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_MoonMaterial);
    }

    private static DateTime CreateDateTime(
      int year,
      int day,
      int hour,
      int minute,
      float second,
      float longitude)
    {
      DateTime dateTime = new DateTime(0L, DateTimeKind.Utc);
      dateTime = dateTime.AddYears(year - 1);
      dateTime = dateTime.AddDays((double) (day - 1));
      dateTime = dateTime.AddHours((double) hour);
      dateTime = dateTime.AddMinutes((double) minute);
      dateTime = dateTime.AddSeconds((double) second);
      dateTime = dateTime.AddSeconds(-43200.0 * (double) longitude / 180.0);
      return dateTime;
    }

    private void UpdateTime(float date, float time, int year)
    {
      this.normalizedDayOfYear = date;
      this.normalizedTime = time;
      // ISSUE: reference to a compiler-generated field
      this.m_Year = year;
    }

    public TopocentricCoordinates GetSunPosition(DateTime date, double latitude, double longitude)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_SunMoonData.GetSunPosition((JulianDateTime) date, latitude, longitude);
    }

    public MoonCoordinate GetMoonPosition(DateTime date, double latitude, double longitude)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_SunMoonData.GetMoonPosition((JulianDateTime) date, latitude, longitude);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      float latitude = this.latitude;
      float longitude = this.longitude;
      if (GameManager.instance.gameMode == GameMode.Game)
      {
        bool flag = this.overrideTime;
        GameplaySettings gameplay = SharedSettings.instance?.gameplay;
        if (gameplay != null && !this.overrideTime && !gameplay.dayNightVisual)
        {
          latitude = 51.2277f;
          longitude = 6.7735f;
          this.time = 14.5f;
          this.day = 177;
          this.year = 2020;
          flag = true;
        }
        TimeSettingsData settings;
        TimeData data;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!flag && this.__query_1383560598_0.TryGetSingleton<TimeSettingsData>(out settings) && this.__query_1383560598_1.TryGetSingleton<TimeData>(out data))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          double renderingFrame = (double) (this.m_RenderingSystem.frameIndex - data.m_FirstFrame) + (double) this.m_RenderingSystem.frameTime;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.UpdateTime(this.m_TimeSystem.GetTimeOfYear(settings, data, renderingFrame), this.m_TimeSystem.GetTimeOfDay(settings, data, renderingFrame) * this.debugTimeMultiplier, this.m_TimeSystem.GetYear(settings, data));
        }
      }
      else
      {
        TimeSettingsData settings;
        TimeData data;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (GameManager.instance.gameMode == GameMode.Editor && !this.overrideTime && this.__query_1383560598_0.TryGetSingleton<TimeSettingsData>(out settings) && this.__query_1383560598_1.TryGetSingleton<TimeData>(out data))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          double renderingFrame = (double) (this.m_RenderingSystem.frameIndex - data.m_FirstFrame) + (double) this.m_RenderingSystem.frameTime;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.UpdateTime(this.m_TimeSystem.GetTimeOfYear(settings, data, renderingFrame), this.m_TimeSystem.GetTimeOfDay(settings, data, renderingFrame) * this.debugTimeMultiplier, this.m_TimeSystem.GetYear(settings, data));
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_SunLight.isValid)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float3 localCoordinates = this.m_SunMoonData.GetSunPosition((JulianDateTime) PlanetarySystem.CreateDateTime(this.year, this.day, this.hour, this.minute, this.second, longitude), (double) latitude, (double) longitude).ToLocalCoordinates(out float _);
        float4x4 float4x4 = float4x4.LookAt(localCoordinates, float3.zero, new float3(0.0f, 1f, 0.0f));
        float3 float3 = math.rotate(float4x4, new float3(0.0f, 0.0f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_SunLight.transform.position = (Vector3) localCoordinates;
        // ISSUE: reference to a compiler-generated field
        this.m_SunLight.transform.rotation = (Quaternion) new quaternion(float4x4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SunLight.additionalData.intensity = this.m_SunLight.initialIntensity * math.smoothstep(0.0f, 0.3f, math.abs(math.min(0.0f, float3.y)));
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_MoonLight.isValid)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        MoonCoordinate moonPosition = this.m_SunMoonData.GetMoonPosition((JulianDateTime) PlanetarySystem.CreateDateTime(this.year, this.moonDay, this.hour, this.minute, this.second, longitude), (double) latitude, (double) longitude);
        float3 localCoordinates = moonPosition.topoCoords.ToLocalCoordinates(out float _);
        float4x4 float4x4 = float4x4.LookAt(localCoordinates, float3.zero, new float3(0.0f, 1f, 0.0f));
        math.rotate(float4x4, new float3(0.0f, 0.0f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_MoonLight.transform.position = (Vector3) localCoordinates;
        // ISSUE: reference to a compiler-generated field
        this.m_MoonLight.transform.rotation = (Quaternion) new quaternion(float4x4);
        // ISSUE: reference to a compiler-generated field
        this.m_MoonLight.additionalData.distance = (float) moonPosition.distance;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SunLight.isValid)
        {
          // ISSUE: reference to a compiler-generated method
          this.RenderMoon();
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NightLight.isValid || !this.m_MoonLight.isValid)
        return;
      // ISSUE: reference to a compiler-generated field
      float3 position = (float3) this.m_MoonLight.transform.position;
      position.y = math.max(position.y, 0.3f);
      float4x4 m = float4x4.LookAt(position, float3.zero, new float3(0.0f, 1f, 0.0f));
      // ISSUE: reference to a compiler-generated field
      this.m_NightLight.transform.position = (Vector3) position;
      // ISSUE: reference to a compiler-generated field
      this.m_NightLight.transform.rotation = (Quaternion) new quaternion(m);
    }

    public float moonSurfaceRoughness
    {
      get => this.m_SurfaceRoughness;
      set
      {
        this.m_SurfaceRoughness = Mathf.Clamp01(value);
        double num1 = 1.5707963705062866 * (double) this.m_SurfaceRoughness;
        float num2 = (float) (num1 * num1);
        this.m_OrenNayarCoefficients = new Vector2((float) (1.0 - 0.5 * (double) num2 / ((double) num2 + 0.33000001311302185)), (float) (0.44999998807907104 * (double) num2 / ((double) num2 + 0.090000003576278687)));
      }
    }

    private void RenderMoon()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_MoonTexture != (UnityEngine.Object) null) || !((UnityEngine.Object) this.m_MoonMaterial != (UnityEngine.Object) null) || !((UnityEngine.Object) this.m_CameraUpdateSystem.activeCamera != (UnityEngine.Object) null))
        return;
      this.moonSurfaceRoughness = 0.8f;
      // ISSUE: reference to a compiler-generated field
      Camera activeCamera = this.m_CameraUpdateSystem.activeCamera;
      float y = Mathf.Tan((float) (0.5 * (double) activeCamera.fieldOfView * 3.1415927410125732 / 180.0));
      Vector4 vector4 = new Vector4(activeCamera.aspect * y, y, activeCamera.nearClipPlane, activeCamera.farClipPlane);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetMatrix(PlanetarySystem.ShaderIDs._Camera2World, activeCamera.cameraToWorldMatrix);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetVector(PlanetarySystem.ShaderIDs._CameraData, vector4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetVector(PlanetarySystem.ShaderIDs._SunDirection, (Vector4) this.m_SunLight.transform.forward);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetVector(PlanetarySystem.ShaderIDs._Direction, (Vector4) this.m_MoonLight.transform.forward);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetVector(PlanetarySystem.ShaderIDs._Tangent, (Vector4) this.m_MoonLight.transform.right);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetVector(PlanetarySystem.ShaderIDs._BiTangent, (Vector4) this.m_MoonLight.transform.up);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetColor(PlanetarySystem.ShaderIDs._Albedo, new Color(1f, 1f, 1f, 1f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetVector(PlanetarySystem.ShaderIDs._Corners, new Vector4(0.0f, 0.0f, 1f, 1f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetVector(PlanetarySystem.ShaderIDs._OrenNayarCoefficients, (Vector4) this.m_OrenNayarCoefficients);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonMaterial.SetFloat(PlanetarySystem.ShaderIDs._Luminance, 10f);
      AtmosphereData atmosphereData;
      AtmospherePrefab prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.__query_1383560598_2.TryGetSingleton<AtmosphereData>(out atmosphereData) && this.m_PrefabSystem.TryGetPrefab<AtmospherePrefab>(atmosphereData.m_AtmospherePrefab, out prefab))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MoonMaterial.SetTexture(PlanetarySystem.ShaderIDs._TexDiffuse, (Texture) prefab.m_MoonAlbedo);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MoonMaterial.SetTexture(PlanetarySystem.ShaderIDs._TexNormal, (Texture) prefab.m_MoonNormal);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Graphics.Blit((Texture) null, this.m_MoonTexture, this.m_MoonMaterial, this.m_ClearPass);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Graphics.Blit((Texture) null, this.m_MoonTexture, this.m_MoonMaterial, this.m_LitPass);
      // ISSUE: reference to a compiler-generated field
      this.m_MoonTexture.IncrementUpdateCount();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MoonLight.additionalData.surfaceTexture = (Texture) this.m_MoonTexture;
    }

    public void SetDefaults(Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Latitude = 41.9028f;
      // ISSUE: reference to a compiler-generated field
      this.m_Longitude = 12.4964f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_Latitude);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_Longitude);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_Latitude);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_Longitude);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1383560598_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TimeSettingsData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_1383560598_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TimeData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_1383560598_2 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<AtmosphereData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public PlanetarySystem()
    {
    }

    public struct LightData
    {
      private readonly string m_Tag;

      public Transform transform { get; private set; }

      public Light light { get; private set; }

      public HDAdditionalLightData additionalData { get; private set; }

      public float initialIntensity { get; private set; }

      public bool isValid
      {
        get
        {
          if ((UnityEngine.Object) this.transform == (UnityEngine.Object) null)
          {
            GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag(this.m_Tag);
            if ((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null)
            {
              this.transform = gameObjectWithTag.transform;
              this.light = gameObjectWithTag.GetComponent<Light>();
              this.additionalData = gameObjectWithTag.GetComponent<HDAdditionalLightData>();
              this.initialIntensity = this.additionalData.intensity;
            }
          }
          return (UnityEngine.Object) this.transform != (UnityEngine.Object) null;
        }
      }

      public LightData(string tag)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Tag = tag;
        this.transform = (Transform) null;
        this.light = (Light) null;
        this.additionalData = (HDAdditionalLightData) null;
        this.initialIntensity = 0.0f;
      }
    }

    private static class ShaderIDs
    {
      public static readonly int _Camera2World = Shader.PropertyToID(nameof (_Camera2World));
      public static readonly int _CameraData = Shader.PropertyToID(nameof (_CameraData));
      public static readonly int _SunDirection = Shader.PropertyToID(nameof (_SunDirection));
      public static readonly int _Luminance = Shader.PropertyToID(nameof (_Luminance));
      public static readonly int _Direction = Shader.PropertyToID(nameof (_Direction));
      public static readonly int _Tangent = Shader.PropertyToID(nameof (_Tangent));
      public static readonly int _BiTangent = Shader.PropertyToID(nameof (_BiTangent));
      public static readonly int _Albedo = Shader.PropertyToID(nameof (_Albedo));
      public static readonly int _OrenNayarCoefficients = Shader.PropertyToID(nameof (_OrenNayarCoefficients));
      public static readonly int _TexDiffuse = Shader.PropertyToID(nameof (_TexDiffuse));
      public static readonly int _TexNormal = Shader.PropertyToID(nameof (_TexNormal));
      public static readonly int _Corners = Shader.PropertyToID(nameof (_Corners));
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TypeHandle
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
      }
    }
  }
}
