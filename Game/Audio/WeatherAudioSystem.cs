// Decompiled with JetBrains decompiler
// Type: Game.Audio.WeatherAudioSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Effects;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Audio
{
  [CompilerGenerated]
  public class WeatherAudioSystem : GameSystemBase
  {
    private AudioManager m_AudioManager;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private EntityQuery m_WeatherAudioEntityQuery;
    private Entity m_SmallWaterAudioEntity;
    private int m_WaterAudioEnabledZoom;
    private int m_WaterAudioNearDistance;
    private WeatherAudioSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WeatherAudioEntityQuery = this.GetEntityQuery(ComponentType.ReadOnly<WeatherAudioData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WeatherAudioEntityQuery);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SmallWaterAudioEntity = Entity.Null;
    }

    private void Initialize()
    {
      // ISSUE: reference to a compiler-generated field
      WeatherAudioData componentData = this.EntityManager.GetComponentData<WeatherAudioData>(this.m_WeatherAudioEntityQuery.GetSingletonEntity());
      Entity entity = this.EntityManager.CreateEntity();
      this.EntityManager.AddComponentData<EffectInstance>(entity, new EffectInstance());
      this.EntityManager.AddComponentData<PrefabRef>(entity, new PrefabRef()
      {
        m_Prefab = componentData.m_WaterAmbientAudio
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SmallWaterAudioEntity = entity;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterAudioEnabledZoom = componentData.m_WaterAudioEnabledZoom;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterAudioNearDistance = componentData.m_WaterAudioNearDistance;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_WaterSystem.Loaded || this.m_CameraUpdateSystem.activeViewer == null || this.m_CameraUpdateSystem.activeCameraController == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_SmallWaterAudioEntity == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.Initialize();
      }
      // ISSUE: reference to a compiler-generated field
      IGameCameraController cameraController = this.m_CameraUpdateSystem.activeCameraController;
      // ISSUE: reference to a compiler-generated field
      float3 position = this.m_CameraUpdateSystem.activeViewer.position;
      EntityManager entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!entityManager.HasComponent<EffectInstance>(this.m_SmallWaterAudioEntity) || (double) cameraController.zoom >= (double) this.m_WaterAudioEnabledZoom)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EffectInstance_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WeatherAudioSystem.WeatherAudioJob weatherAudioJob = new WeatherAudioSystem.WeatherAudioJob();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      weatherAudioJob.m_WaterTextureSize = this.m_WaterSystem.TextureSize;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      weatherAudioJob.m_WaterAudioNearDistance = this.m_WaterAudioNearDistance;
      // ISSUE: reference to a compiler-generated field
      weatherAudioJob.m_CameraPosition = position;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      weatherAudioJob.m_WaterAudioEntity = this.m_SmallWaterAudioEntity;
      ref WeatherAudioSystem.WeatherAudioJob local = ref weatherAudioJob;
      entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      WeatherAudioData componentData = entityManager.GetComponentData<WeatherAudioData>(this.m_WeatherAudioEntityQuery.GetSingletonEntity());
      // ISSUE: reference to a compiler-generated field
      local.m_WeatherAudioData = componentData;
      JobHandle deps1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      weatherAudioJob.m_SourceUpdateData = this.m_AudioManager.GetSourceUpdateData(out deps1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      weatherAudioJob.m_TerrainData = this.m_TerrainSystem.GetHeightData();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      weatherAudioJob.m_EffectInstances = this.__TypeHandle.__Game_Effects_EffectInstance_RW_ComponentLookup;
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      weatherAudioJob.m_WaterDepths = this.m_WaterSystem.GetDepths(out deps2);
      // ISSUE: variable of a compiler-generated type
      WeatherAudioSystem.WeatherAudioJob jobData = weatherAudioJob;
      this.Dependency = jobData.Schedule<WeatherAudioSystem.WeatherAudioJob>(JobHandle.CombineDependencies(deps1, deps2, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.AddSourceUpdateWriter(this.Dependency);
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
    public WeatherAudioSystem()
    {
    }

    [BurstCompile]
    private struct WeatherAudioJob : IJob
    {
      public ComponentLookup<EffectInstance> m_EffectInstances;
      public SourceUpdateData m_SourceUpdateData;
      [ReadOnly]
      public int2 m_WaterTextureSize;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public int m_WaterAudioNearDistance;
      [ReadOnly]
      public Entity m_WaterAudioEntity;
      [ReadOnly]
      public WeatherAudioData m_WeatherAudioData;
      [ReadOnly]
      public NativeArray<SurfaceWater> m_WaterDepths;
      [ReadOnly]
      public TerrainHeightData m_TerrainData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (WeatherAudioSystem.WeatherAudioJob.NearWater(this.m_CameraPosition, this.m_WaterTextureSize, this.m_WaterAudioNearDistance, ref this.m_WaterDepths))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EffectInstance effectInstance = this.m_EffectInstances[this.m_WaterAudioEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float y = TerrainUtils.SampleHeight(ref this.m_TerrainData, this.m_CameraPosition);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float x = math.lerp(effectInstance.m_Intensity, this.m_WeatherAudioData.m_WaterAudioIntensity, this.m_WeatherAudioData.m_WaterFadeSpeed);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          effectInstance.m_Position = new float3(this.m_CameraPosition.x, y, this.m_CameraPosition.z);
          effectInstance.m_Rotation = quaternion.identity;
          effectInstance.m_Intensity = math.saturate(x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EffectInstances[this.m_WaterAudioEntity] = effectInstance;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SourceUpdateData.Add(this.m_WaterAudioEntity, new Transform()
          {
            m_Position = this.m_CameraPosition,
            m_Rotation = quaternion.identity
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EffectInstances.HasComponent(this.m_WaterAudioEntity))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EffectInstance effectInstance = this.m_EffectInstances[this.m_WaterAudioEntity];
          if ((double) effectInstance.m_Intensity <= 0.0099999997764825821)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_SourceUpdateData.Remove(this.m_WaterAudioEntity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            float x = math.lerp(effectInstance.m_Intensity, 0.0f, this.m_WeatherAudioData.m_WaterFadeSpeed);
            effectInstance.m_Intensity = math.saturate(x);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_EffectInstances[this.m_WaterAudioEntity] = effectInstance;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_SourceUpdateData.Add(this.m_WaterAudioEntity, new Transform()
            {
              m_Position = this.m_CameraPosition,
              m_Rotation = quaternion.identity
            });
          }
        }
      }

      private static bool NearWater(
        float3 position,
        int2 texSize,
        int distance,
        ref NativeArray<SurfaceWater> depthsCPU)
      {
        // ISSUE: reference to a compiler-generated field
        float2 float2 = (float) WaterSystem.kMapSize / (float2) texSize;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        int2 cell = WaterSystem.GetCell(position - new float3(float2.x / 2f, 0.0f, float2.y / 2f), WaterSystem.kMapSize, texSize);
        for (int index1 = -distance; index1 <= distance; ++index1)
        {
          for (int index2 = -distance; index2 <= distance; ++index2)
          {
            int2 int2;
            int2.x = math.clamp(cell.x + index1, 0, texSize.x - 2);
            int2.y = math.clamp(cell.y + index2, 0, texSize.y - 2);
            if ((double) depthsCPU[int2.x + 1 + texSize.x * int2.y].m_Depth > 0.0)
              return true;
          }
        }
        return false;
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<EffectInstance> __Game_Effects_EffectInstance_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EffectInstance_RW_ComponentLookup = state.GetComponentLookup<EffectInstance>();
      }
    }
  }
}
