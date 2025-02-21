// Decompiled with JetBrains decompiler
// Type: Game.Effects.LightCullingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Prefabs;
using Game.Prefabs.Effects;
using Game.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Effects
{
  [CompilerGenerated]
  public class LightCullingSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private RenderingSystem m_RenderingSystem;
    private EffectControlSystem m_EffectControlSystem;
    private EntityQuery m_LightEffectPrefabQuery;
    private NativeParallelHashMap<Entity, LightCullingSystem.LightEffectCullData> m_LightEffectCullData;
    private static LightCullingSystem.DefaultLightParams s_DefaultLightParams;
    private NativeQueue<LightCullingSystem.VisibleLightData> m_VisibleLights;
    private NativeReference<float> m_LastFrameMaxPunctualLightDistance;
    public static bool s_enableMinMaxLightCullingOptim = true;
    public static float s_maxLightDistanceScale = 1.5f;
    public static float s_minLightDistanceScale = 0.5f;
    private LightCullingSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlSystem = this.World.GetOrCreateSystemManaged<EffectControlSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LightEffectPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<LightEffectData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LightEffectCullData = new NativeParallelHashMap<Entity, LightCullingSystem.LightEffectCullData>(128, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_VisibleLights = new NativeQueue<LightCullingSystem.VisibleLightData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LastFrameMaxPunctualLightDistance = new NativeReference<float>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LastFrameMaxPunctualLightDistance.Value = -1f;
      // ISSUE: reference to a compiler-generated method
      this.ReadDefaultLightParams();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      HDRPDotsInputs.punctualLightsJobHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_LastFrameMaxPunctualLightDistance.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_VisibleLights.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LightEffectCullData.Dispose();
      HDRPDotsInputs.ClearFrameLightData();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      HDRPDotsInputs.punctualLightsJobHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      HDRPDotsInputs.MaxPunctualLights = this.m_RenderingSystem.maxLightCount;
      HDRPDotsInputs.ClearFrameLightData();
      Camera main = Camera.main;
      if ((UnityEngine.Object) main == (UnityEngine.Object) null)
        return;
      float4 lodParameters;
      float3 cameraPosition;
      float3 cameraDirection;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_EffectControlSystem.GetLodParameters(out lodParameters, out cameraPosition, out cameraDirection);
      // ISSUE: reference to a compiler-generated method
      this.ComputeLightEffectCullData(lodParameters);
      Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(main);
      NativeArray<float4> nativeArray = new NativeArray<float4>(6, Allocator.TempJob);
      for (int index = 0; index < frustumPlanes.Length; ++index)
        nativeArray[index] = new float4((float3) frustumPlanes[index].normal, frustumPlanes[index].distance);
      // ISSUE: reference to a compiler-generated field
      if (!LightCullingSystem.s_enableMinMaxLightCullingOptim)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastFrameMaxPunctualLightDistance.Value = -1f;
      }
      float num = 1f;
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_LastFrameMaxPunctualLightDistance.Value > 0.0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        num = this.m_LastFrameMaxPunctualLightDistance.Value * LightCullingSystem.s_maxLightDistanceScale;
      }
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LightCullingSystem.LightCullingJob jobData = new LightCullingSystem.LightCullingJob()
      {
        m_LightEffectCullData = this.m_LightEffectCullData,
        m_Planes = nativeArray,
        m_EnabledEffectData = this.m_EffectControlSystem.GetEnabledData(true, out dependencies),
        m_LodParameters = lodParameters,
        m_CameraPosition = cameraPosition,
        m_CameraDirection = cameraDirection,
        m_AutoRejectDistance = num,
        m_VisibleLights = this.m_VisibleLights.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData.Schedule<LightCullingSystem.LightCullingJob, EnabledEffectData>(jobData.m_EnabledEffectData, 16, dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_EffectControlSystem.AddEnabledDataReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      HDRPDotsInputs.punctualLightsJobHandle = new LightCullingSystem.SortAndBuildPunctualLightsJob()
      {
        m_maxLights = HDRPDotsInputs.MaxPunctualLights,
        m_minDistanceScale = LightCullingSystem.s_minLightDistanceScale,
        m_PunctualLightsOut = HDRPDotsInputs.s_punctualLightdata,
        m_LightEffectPrefabData = HDRPDotsInputs.s_lightEffectPrefabData,
        m_VisibleLightsOut = HDRPDotsInputs.s_punctualVisibleLights,
        m_LightEffectCullData = this.m_LightEffectCullData,
        m_VisibleLights = this.m_VisibleLights,
        m_MaxDistance = this.m_LastFrameMaxPunctualLightDistance
      }.Schedule<LightCullingSystem.SortAndBuildPunctualLightsJob>(jobHandle);
    }

    private void ReadDefaultLightParams()
    {
      GameObject gameObject = new GameObject("Default LightSource");
      HDAdditionalLightData additionalLightData = gameObject.AddHDLight(HDLightTypeAndShape.ConeSpot);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      LightCullingSystem.s_DefaultLightParams.shapeWidth = additionalLightData.shapeWidth;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      LightCullingSystem.s_DefaultLightParams.shapeHeight = additionalLightData.shapeHeight;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      LightCullingSystem.s_DefaultLightParams.spotIESCutoffPercent01 = additionalLightData.spotIESCutoffPercent01;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      LightCullingSystem.s_DefaultLightParams.shapeRadius = additionalLightData.shapeRadius;
      CoreUtils.Destroy((UnityEngine.Object) gameObject);
    }

    private UnityEngine.Rendering.HighDefinition.SpotLightShape GetUnitySpotShape(
      Game.Rendering.SpotLightShape spotlightShape)
    {
      switch (spotlightShape)
      {
        case Game.Rendering.SpotLightShape.Cone:
          return UnityEngine.Rendering.HighDefinition.SpotLightShape.Cone;
        case Game.Rendering.SpotLightShape.Pyramid:
          return UnityEngine.Rendering.HighDefinition.SpotLightShape.Pyramid;
        case Game.Rendering.SpotLightShape.Box:
          return UnityEngine.Rendering.HighDefinition.SpotLightShape.Box;
        default:
          return UnityEngine.Rendering.HighDefinition.SpotLightShape.Cone;
      }
    }

    private UnityEngine.Rendering.HighDefinition.AreaLightShape GetUnityAreaShape(
      Game.Rendering.AreaLightShape arealightShape)
    {
      return arealightShape != Game.Rendering.AreaLightShape.Rectangle && arealightShape == Game.Rendering.AreaLightShape.Tube ? UnityEngine.Rendering.HighDefinition.AreaLightShape.Tube : UnityEngine.Rendering.HighDefinition.AreaLightShape.Rectangle;
    }

    private void GetRenderDataFromLigthEffet(
      ref HDLightRenderData hdLightRenderData,
      LightEffectData lightEffectData,
      LightEffect lightEffect,
      float4 lodParameters)
    {
      hdLightRenderData.pointLightType = lightEffect.m_Type == Game.Rendering.LightType.Area ? HDAdditionalLightData.PointLightHDType.Area : HDAdditionalLightData.PointLightHDType.Punctual;
      // ISSUE: reference to a compiler-generated method
      hdLightRenderData.spotLightShape = this.GetUnitySpotShape(lightEffect.m_SpotShape);
      // ISSUE: reference to a compiler-generated method
      hdLightRenderData.areaLightShape = this.GetUnityAreaShape(lightEffect.m_AreaShape);
      hdLightRenderData.lightLayer = LightLayerEnum.Everything;
      hdLightRenderData.fadeDistance = 100000f;
      hdLightRenderData.distance = lightEffect.m_LuxAtDistance;
      hdLightRenderData.angularDiameter = lightEffect.m_SpotAngle;
      hdLightRenderData.volumetricFadeDistance = lightEffect.m_VolumetricFadeDistance;
      hdLightRenderData.includeForRayTracing = false;
      hdLightRenderData.useScreenSpaceShadows = false;
      hdLightRenderData.useRayTracedShadows = false;
      hdLightRenderData.colorShadow = false;
      hdLightRenderData.lightDimmer = lightEffect.m_LightDimmer;
      hdLightRenderData.volumetricDimmer = lightEffect.m_VolumetricDimmer;
      hdLightRenderData.shapeWidth = lightEffect.m_ShapeWidth;
      hdLightRenderData.shapeHeight = lightEffect.m_ShapeHeight;
      hdLightRenderData.aspectRatio = lightEffect.m_AspectRatio;
      hdLightRenderData.innerSpotPercent = lightEffect.m_InnerSpotPercentage;
      hdLightRenderData.spotIESCutoffPercent = 100f;
      hdLightRenderData.shadowDimmer = 1f;
      hdLightRenderData.volumetricShadowDimmer = 1f;
      hdLightRenderData.shadowFadeDistance = 0.0f;
      hdLightRenderData.shapeRadius = lightEffect.m_ShapeRadius;
      hdLightRenderData.barnDoorLength = lightEffect.m_BarnDoorLength;
      hdLightRenderData.barnDoorAngle = lightEffect.m_BarnDoorAngle;
      hdLightRenderData.flareSize = 0.0f;
      hdLightRenderData.flareFalloff = 0.0f;
      hdLightRenderData.affectVolumetric = lightEffect.m_UseVolumetric;
      hdLightRenderData.affectDiffuse = lightEffect.m_AffectDiffuse;
      hdLightRenderData.affectSpecular = lightEffect.m_AffectSpecular;
      hdLightRenderData.applyRangeAttenuation = lightEffect.m_ApplyRangeAttenuation;
      hdLightRenderData.penumbraTint = false;
      hdLightRenderData.interactsWithSky = false;
      hdLightRenderData.surfaceTint = Color.black;
      hdLightRenderData.shadowTint = Color.black;
      hdLightRenderData.flareTint = Color.black;
    }

    private void ComputeLightEffectCullData(float4 lodParameters)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LightEffectCullData.Clear();
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_LightEffectPrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LightEffectData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<LightEffectData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_LightEffectData_RO_ComponentTypeHandle;
      this.CompleteDependency();
      // ISSUE: reference to a compiler-generated field
      int entityCount = this.m_LightEffectPrefabQuery.CalculateEntityCount();
      float num = 1f / lodParameters.x;
      if (!HDRPDotsInputs.s_HdLightRenderData.IsCreated || entityCount + 8 > HDRPDotsInputs.s_HdLightRenderData.Length)
        HDRPDotsInputs.s_HdLightRenderData.ResizeArray<HDLightRenderData>(entityCount + 8);
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
        NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
        NativeArray<LightEffectData> nativeArray3 = archetypeChunk.GetNativeArray<LightEffectData>(ref componentTypeHandle2);
        for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
        {
          Entity key = nativeArray1[index2];
          PrefabData prefabData = nativeArray2[index2];
          LightEffectData lightEffectData = nativeArray3[index2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          LightEffect component = this.m_PrefabSystem.GetPrefab<EffectPrefab>(prefabData).GetComponent<LightEffect>();
          // ISSUE: variable of a compiler-generated type
          LightCullingSystem.LightEffectCullData lightEffectCullData;
          // ISSUE: reference to a compiler-generated field
          lightEffectCullData.m_LightEffectPrefabDataIndex = HDRPDotsInputs.s_lightEffectPrefabData.Length;
          // ISSUE: reference to a compiler-generated field
          lightEffectCullData.m_lightType = component.m_Type;
          // ISSUE: reference to a compiler-generated field
          lightEffectCullData.m_Range = component.m_Range;
          // ISSUE: reference to a compiler-generated field
          lightEffectCullData.m_SpotAngle = component.m_SpotAngle;
          // ISSUE: reference to a compiler-generated field
          lightEffectCullData.m_InvDistanceFactor = lightEffectData.m_InvDistanceFactor * num;
          HDRPDotsInputs.s_lightEffectPrefabData.Add(new HDRPDotsInputs.LightEffectPrefabData());
          HDRPDotsInputs.s_lightEffectPrefabCookies.Add(component.m_Cookie);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.GetRenderDataFromLigthEffet(ref HDRPDotsInputs.s_HdLightRenderData.ElementAt<HDLightRenderData>(lightEffectCullData.m_LightEffectPrefabDataIndex), lightEffectData, component, lodParameters);
          // ISSUE: reference to a compiler-generated field
          this.m_LightEffectCullData.Add(key, lightEffectCullData);
        }
      }
    }

    private static GPULightType GetGPULightType(LightEffect lightEffect)
    {
      if (lightEffect.m_Type == Game.Rendering.LightType.Spot)
      {
        if (lightEffect.m_SpotShape == Game.Rendering.SpotLightShape.Cone)
          return GPULightType.Spot;
        if (lightEffect.m_SpotShape == Game.Rendering.SpotLightShape.Pyramid)
          return GPULightType.ProjectorPyramid;
        if (lightEffect.m_SpotShape == Game.Rendering.SpotLightShape.Box)
          return GPULightType.ProjectorBox;
      }
      else
      {
        if (lightEffect.m_Type == Game.Rendering.LightType.Point)
          return GPULightType.Point;
        if (lightEffect.m_Type == Game.Rendering.LightType.Area)
        {
          if (lightEffect.m_AreaShape == Game.Rendering.AreaLightShape.Rectangle)
            return GPULightType.Rectangle;
          if (lightEffect.m_AreaShape == Game.Rendering.AreaLightShape.Tube)
            return GPULightType.Tube;
        }
      }
      throw new NotImplementedException(string.Format("Unsupported light type {0}", (object) lightEffect.m_Type));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
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

    [UnityEngine.Scripting.Preserve]
    public LightCullingSystem()
    {
    }

    private struct DefaultLightParams
    {
      public float shapeWidth;
      public float shapeHeight;
      public float spotIESCutoffPercent01;
      public float shapeRadius;
    }

    private struct LightEffectCullData
    {
      public float m_Range;
      public float m_SpotAngle;
      public float m_InvDistanceFactor;
      public int m_LightEffectPrefabDataIndex;
      public Game.Rendering.LightType m_lightType;
    }

    [BurstCompile]
    private struct LightCullingJob : IJobParallelForDefer
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<float4> m_Planes;
      [ReadOnly]
      public NativeParallelHashMap<Entity, LightCullingSystem.LightEffectCullData> m_LightEffectCullData;
      [ReadOnly]
      public NativeList<EnabledEffectData> m_EnabledEffectData;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public float m_AutoRejectDistance;
      public NativeQueue<LightCullingSystem.VisibleLightData>.ParallelWriter m_VisibleLights;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        EnabledEffectData enabledEffectData = this.m_EnabledEffectData[index];
        if ((enabledEffectData.m_Flags & (EnabledEffectFlags.IsEnabled | EnabledEffectFlags.IsLight)) != (EnabledEffectFlags.IsEnabled | EnabledEffectFlags.IsLight))
          return;
        float3 position = enabledEffectData.m_Position;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LightCullingSystem.LightEffectCullData lightEffectCullData = this.m_LightEffectCullData[enabledEffectData.m_Prefab];
        bool flag = (double) enabledEffectData.m_Intensity != 0.0;
        for (int index1 = 0; index1 < 6; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.dot(this.m_Planes[index1].xyz, position) + (double) this.m_Planes[index1].w < -(double) lightEffectCullData.m_Range)
            flag = false;
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num = RenderingUtils.CalculateMinDistance(new Bounds3(enabledEffectData.m_Position - lightEffectCullData.m_Range, enabledEffectData.m_Position + lightEffectCullData.m_Range), this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters) * lightEffectCullData.m_InvDistanceFactor;
        // ISSUE: reference to a compiler-generated field
        if ((double) num >= (double) this.m_AutoRejectDistance)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_VisibleLights.Enqueue(new LightCullingSystem.VisibleLightData()
        {
          m_Position = position,
          m_Rotation = enabledEffectData.m_Rotation,
          m_Prefab = enabledEffectData.m_Prefab,
          m_RelativeDistance = num,
          m_Color = enabledEffectData.m_Scale * enabledEffectData.m_Intensity
        });
      }
    }

    public struct VisibleLightData
    {
      public Entity m_Prefab;
      public float3 m_Position;
      public quaternion m_Rotation;
      public float3 m_Color;
      public float m_RelativeDistance;
    }

    [BurstCompile]
    private struct SortAndBuildPunctualLightsJob : IJob
    {
      public NativeQueue<LightCullingSystem.VisibleLightData> m_VisibleLights;
      [ReadOnly]
      public NativeParallelHashMap<Entity, LightCullingSystem.LightEffectCullData> m_LightEffectCullData;
      [WriteOnly]
      public NativeList<HDRPDotsInputs.PunctualLightData> m_PunctualLightsOut;
      [WriteOnly]
      public NativeList<HDRPDotsInputs.LightEffectPrefabData> m_LightEffectPrefabData;
      [WriteOnly]
      public NativeArray<VisibleLight> m_VisibleLightsOut;
      public NativeReference<float> m_MaxDistance;
      public int m_maxLights;
      public float m_minDistanceScale;

      private unsafe ref VisibleLight GetVisibleLightRef(int dataIndex)
      {
        // ISSUE: reference to a compiler-generated field
        return ref UnsafeUtility.AsRef<VisibleLight>((void*) ((VisibleLight*) this.m_VisibleLightsOut.GetUnsafePtr<VisibleLight>() + dataIndex));
      }

      private UnityEngine.LightType GetLightType(Game.Rendering.LightType lightType)
      {
        switch (lightType)
        {
          case Game.Rendering.LightType.Spot:
            return UnityEngine.LightType.Spot;
          case Game.Rendering.LightType.Point:
            return UnityEngine.LightType.Point;
          case Game.Rendering.LightType.Area:
            return UnityEngine.LightType.Area;
          default:
            return UnityEngine.LightType.Spot;
        }
      }

      public unsafe void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeList<int> list = new NativeList<int>(this.m_maxLights, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeList<int> nativeList1 = new NativeList<int>(this.m_maxLights, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeList<LightCullingSystem.VisibleLightData> nativeList2 = new NativeList<LightCullingSystem.VisibleLightData>(this.m_maxLights * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num1 = this.m_MaxDistance.Value * this.m_minDistanceScale;
        int num2 = 0;
        // ISSUE: variable of a compiler-generated type
        LightCullingSystem.VisibleLightData visibleLightData1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_VisibleLights.TryDequeue(out visibleLightData1))
        {
          // ISSUE: reference to a compiler-generated field
          float relativeDistance = visibleLightData1.m_RelativeDistance;
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_MaxDistance.Value > 0.0)
          {
            nativeList2.Add(in visibleLightData1);
            if ((double) relativeDistance < (double) num1)
              nativeList1.Add(in num2);
            else
              list.Add(in num2);
            ++num2;
          }
          else
          {
            nativeList2.Add(in visibleLightData1);
            list.Add(in num2);
            ++num2;
          }
        }
        float x = -1f;
        if (nativeList2.Length > 0)
        {
          NativeArray<LightCullingSystem.VisibleLightData> nativeArray = nativeList2.AsArray();
          // ISSUE: object of a compiler-generated type is created
          list.Sort<int, LightCullingSystem.SortAndBuildPunctualLightsJob.EffectInstanceDistanceComparer>(new LightCullingSystem.SortAndBuildPunctualLightsJob.EffectInstanceDistanceComparer((LightCullingSystem.VisibleLightData*) nativeArray.GetUnsafePtr<LightCullingSystem.VisibleLightData>()));
          // ISSUE: reference to a compiler-generated field
          int num3 = math.min(list.Length + nativeList1.Length, this.m_maxLights);
          float num4 = 1f;
          if (num3 < list.Length + nativeList1.Length)
          {
            // ISSUE: variable of a compiler-generated type
            LightCullingSystem.VisibleLightData visibleLightData2 = num3 >= nativeList1.Length ? nativeArray[list[num3 - nativeList1.Length]] : nativeArray[nativeList1[num3]];
            // ISSUE: reference to a compiler-generated field
            num4 = 1f / math.clamp(visibleLightData2.m_RelativeDistance, 1E-05f, 1f);
          }
          // ISSUE: reference to a compiler-generated field
          int num5 = math.min(num3, this.m_VisibleLightsOut.Length);
          for (int index = 0; index < num5; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            LightCullingSystem.VisibleLightData visibleLightData3 = index >= nativeList1.Length ? nativeArray[list[index - nativeList1.Length]] : nativeArray[nativeList1[index]];
            // ISSUE: reference to a compiler-generated field
            float3 position = visibleLightData3.m_Position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            LightCullingSystem.LightEffectCullData lightEffectCullData = this.m_LightEffectCullData[visibleLightData3.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            quaternion rotation = visibleLightData3.m_Rotation;
            float3 one = (float3) Vector3.one;
            float4x4 float4x4 = float4x4.TRS(position, rotation, one);
            // ISSUE: reference to a compiler-generated method
            ref VisibleLight local = ref this.GetVisibleLightRef(index);
            // ISSUE: reference to a compiler-generated field
            local.spotAngle = lightEffectCullData.m_SpotAngle;
            local.localToWorldMatrix = (Matrix4x4) float4x4;
            // ISSUE: reference to a compiler-generated field
            float num6 = math.saturate(1f - math.lengthsq(math.max(0.0f, (float) (5.0 * (double) (visibleLightData3.m_RelativeDistance * num4) - 4.0))));
            local.screenRect = new Rect(0.0f, 0.0f, 10f, 10f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            local.lightType = this.GetLightType(lightEffectCullData.m_lightType);
            // ISSUE: reference to a compiler-generated field
            local.finalColor = (Color) (Vector4) new float4(visibleLightData3.m_Color * num6, 1f);
            // ISSUE: reference to a compiler-generated field
            local.range = lightEffectCullData.m_Range;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PunctualLightsOut.AddNoResize(new HDRPDotsInputs.PunctualLightData()
            {
              lightEffectPrefabDataIndex = lightEffectCullData.m_LightEffectPrefabDataIndex
            });
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_LightEffectPrefabData[lightEffectCullData.m_LightEffectPrefabDataIndex] = new HDRPDotsInputs.LightEffectPrefabData()
            {
              cookieMode = CookieMode.Clamp
            };
            // ISSUE: reference to a compiler-generated field
            x = math.max(x, visibleLightData3.m_RelativeDistance);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_MaxDistance.Value = x;
        list.Dispose();
        nativeList1.Dispose();
        nativeList2.Dispose();
      }

      private struct EffectInstanceDistanceComparer : IComparer<int>
      {
        private unsafe LightCullingSystem.VisibleLightData* m_visibleLights;

        public unsafe EffectInstanceDistanceComparer(LightCullingSystem.VisibleLightData* arrayPtr)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_visibleLights = arrayPtr;
        }

        private unsafe ref LightCullingSystem.VisibleLightData GetVisibleLightRef(int dataIndex)
        {
          // ISSUE: reference to a compiler-generated field
          return ref UnsafeUtility.AsRef<LightCullingSystem.VisibleLightData>((void*) (this.m_visibleLights + dataIndex));
        }

        public int Compare(int x, int y)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          return this.GetVisibleLightRef(x).m_RelativeDistance.CompareTo(this.GetVisibleLightRef(y).m_RelativeDistance);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LightEffectData> __Game_Prefabs_LightEffectData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LightEffectData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LightEffectData>(true);
      }
    }
  }
}
