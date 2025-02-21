// Decompiled with JetBrains decompiler
// Type: Game.Audio.AudioGroupingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Effects;
using Game.Events;
using Game.Objects;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Audio
{
  [CompilerGenerated]
  public class AudioGroupingSystem : GameSystemBase
  {
    private TrafficAmbienceSystem m_TrafficAmbienceSystem;
    private ZoneAmbienceSystem m_ZoneAmbienceSystem;
    private EffectFlagSystem m_EffectFlagSystem;
    private SimulationSystem m_SimulationSystem;
    private ClimateSystem m_ClimateSystem;
    private AudioManager m_AudioManager;
    private EntityQuery m_AudioGroupingConfigurationQuery;
    private EntityQuery m_AudioGroupingMiscSettingQuery;
    private NativeArray<Entity> m_AmbienceEntities;
    private NativeArray<Entity> m_NearAmbienceEntities;
    private NativeArray<AudioGroupingSettingsData> m_Settings;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_OnFireTreeQuery;
    private AudioGroupingSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficAmbienceSystem = this.World.GetOrCreateSystemManaged<TrafficAmbienceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneAmbienceSystem = this.World.GetOrCreateSystemManaged<ZoneAmbienceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectFlagSystem = this.World.GetOrCreateSystemManaged<EffectFlagSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioGroupingConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<AudioGroupingSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AudioGroupingMiscSettingQuery = this.GetEntityQuery(ComponentType.ReadOnly<AudioGroupingMiscSetting>());
      // ISSUE: reference to a compiler-generated field
      this.m_OnFireTreeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Tree>(), ComponentType.ReadOnly<OnFire>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AudioGroupingConfigurationQuery);
    }

    private Entity CreateEffect(Entity sfx)
    {
      Entity entity = this.EntityManager.CreateEntity();
      this.EntityManager.AddComponentData<EffectInstance>(entity, new EffectInstance());
      this.EntityManager.AddComponentData<PrefabRef>(entity, new PrefabRef()
      {
        m_Prefab = sfx
      });
      return entity;
    }

    private void Initialize()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_AudioGroupingConfigurationQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      List<AudioGroupingSettingsData> list = new List<AudioGroupingSettingsData>();
      foreach (Entity entity in entityArray)
        list.AddRange((IEnumerable<AudioGroupingSettingsData>) this.World.EntityManager.GetBuffer<AudioGroupingSettingsData>(entity, true).AsNativeArray());
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Settings.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Settings = list.ToNativeArray<AudioGroupingSettingsData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      }
      entityArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AmbienceEntities.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AmbienceEntities = new NativeArray<Entity>(this.m_Settings.Length, Allocator.Persistent);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NearAmbienceEntities.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NearAmbienceEntities = new NativeArray<Entity>(this.m_Settings.Length, Allocator.Persistent);
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Settings.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AmbienceEntities[index] = this.CreateEffect(this.m_Settings[index].m_GroupSoundFar);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NearAmbienceEntities[index] = this.m_Settings[index].m_GroupSoundNear != Entity.Null ? this.CreateEffect(this.m_Settings[index].m_GroupSoundNear) : Entity.Null;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_AmbienceEntities.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AmbienceEntities.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_NearAmbienceEntities.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NearAmbienceEntities.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_Settings.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Settings.Dispose();
      }
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      if (GameManager.instance.gameMode != GameMode.Game || GameManager.instance.isGameLoading)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_AmbienceEntities.Length == 0 || !this.EntityManager.HasComponent<EffectInstance>(this.m_AmbienceEntities[0]))
      {
        // ISSUE: reference to a compiler-generated method
        this.Initialize();
      }
      Camera main = Camera.main;
      if ((UnityEngine.Object) main == (UnityEngine.Object) null)
        return;
      float3 position = (float3) main.transform.position;
      // ISSUE: reference to a compiler-generated field
      AudioGroupingMiscSetting singleton = this.m_AudioGroupingMiscSettingQuery.GetSingleton<AudioGroupingMiscSetting>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EffectInstance_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      JobHandle dependencies1;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioGroupingSystem.AudioGroupingJob jobData = new AudioGroupingSystem.AudioGroupingJob()
      {
        m_CameraPosition = position,
        m_SourceUpdateData = this.m_AudioManager.GetSourceUpdateData(out deps),
        m_TrafficAmbienceMap = this.m_TrafficAmbienceSystem.GetMap(true, out dependencies1),
        m_AmbienceMap = this.m_ZoneAmbienceSystem.GetMap(true, out dependencies2),
        m_Settings = this.m_Settings,
        m_EffectFlagData = this.m_EffectFlagSystem.GetData(),
        m_AmbienceEntities = this.m_AmbienceEntities,
        m_NearAmbienceEntities = this.m_NearAmbienceEntities,
        m_OnFireTrees = this.m_OnFireTreeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_EffectInstances = this.__TypeHandle.__Game_Effects_EffectInstance_RW_ComponentLookup,
        m_EffectDatas = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_TerrainData = this.m_TerrainSystem.GetHeightData(),
        m_ForestFireDistance = singleton.m_ForestFireDistance,
        m_Precipitation = (float) this.m_ClimateSystem.precipitation,
        m_IsRaining = this.m_ClimateSystem.isRaining
      };
      this.Dependency = jobData.Schedule<AudioGroupingSystem.AudioGroupingJob>(JobHandle.CombineDependencies(JobHandle.CombineDependencies(dependencies2, deps), dependencies1, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.AddSourceUpdateWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficAmbienceSystem.AddReader(this.Dependency);
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
    public AudioGroupingSystem()
    {
    }

    [BurstCompile]
    private struct AudioGroupingJob : IJob
    {
      public ComponentLookup<EffectInstance> m_EffectInstances;
      [ReadOnly]
      public ComponentLookup<EffectData> m_EffectDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public NativeArray<TrafficAmbienceCell> m_TrafficAmbienceMap;
      [ReadOnly]
      public NativeArray<ZoneAmbienceCell> m_AmbienceMap;
      [ReadOnly]
      public NativeArray<AudioGroupingSettingsData> m_Settings;
      public SourceUpdateData m_SourceUpdateData;
      public EffectFlagSystem.EffectFlagData m_EffectFlagData;
      public float3 m_CameraPosition;
      public NativeArray<Entity> m_AmbienceEntities;
      public NativeArray<Entity> m_NearAmbienceEntities;
      [DeallocateOnJobCompletion]
      public NativeArray<Entity> m_OnFireTrees;
      [ReadOnly]
      public TerrainHeightData m_TerrainData;
      [ReadOnly]
      public float m_ForestFireDistance;
      [ReadOnly]
      public float m_Precipitation;
      [ReadOnly]
      public bool m_IsRaining;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        float3 cameraPosition = this.m_CameraPosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraPosition.y -= TerrainUtils.SampleHeight(ref this.m_TerrainData, this.m_CameraPosition);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_AmbienceEntities.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity ambienceEntity = this.m_AmbienceEntities[index];
          // ISSUE: reference to a compiler-generated field
          Entity nearAmbienceEntity = this.m_NearAmbienceEntities[index];
          // ISSUE: reference to a compiler-generated field
          AudioGroupingSettingsData setting = this.m_Settings[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_EffectInstances.HasComponent(ambienceEntity))
          {
            float num1 = 0.0f;
            float num2 = 0.0f;
            switch (setting.m_Type)
            {
              case GroupAmbienceType.Traffic:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                num1 = TrafficAmbienceSystem.GetTrafficAmbience2(this.m_CameraPosition, this.m_TrafficAmbienceMap, 1f / setting.m_Scale).m_Traffic;
                break;
              case GroupAmbienceType.Forest:
              case GroupAmbienceType.NightForest:
                // ISSUE: reference to a compiler-generated field
                GroupAmbienceType groupAmbienceType = this.m_EffectFlagData.m_IsNightTime ? GroupAmbienceType.NightForest : GroupAmbienceType.Forest;
                // ISSUE: reference to a compiler-generated method
                if (setting.m_Type == groupAmbienceType && !this.IsNearForestOnFire(cameraPosition))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num1 = ZoneAmbienceSystem.GetZoneAmbience(GroupAmbienceType.Forest, this.m_CameraPosition, this.m_AmbienceMap, 1f / this.m_Settings[index].m_Scale);
                  if (nearAmbienceEntity != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    num2 = ZoneAmbienceSystem.GetZoneAmbienceNear(GroupAmbienceType.Forest, this.m_CameraPosition, this.m_AmbienceMap, this.m_Settings[index].m_NearWeight, 1f / this.m_Settings[index].m_Scale);
                    break;
                  }
                  break;
                }
                break;
              case GroupAmbienceType.Rain:
                // ISSUE: reference to a compiler-generated field
                if (this.m_IsRaining)
                {
                  // ISSUE: reference to a compiler-generated field
                  num1 = math.min(1f / setting.m_Scale, math.max(0.0f, this.m_Precipitation) * 2f);
                  num2 = num1;
                  break;
                }
                break;
              default:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                num1 = ZoneAmbienceSystem.GetZoneAmbience(setting.m_Type, this.m_CameraPosition, this.m_AmbienceMap, 1f / setting.m_Scale);
                if (nearAmbienceEntity != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num2 = ZoneAmbienceSystem.GetZoneAmbienceNear(setting.m_Type, this.m_CameraPosition, this.m_AmbienceMap, this.m_Settings[index].m_NearWeight, 1f / setting.m_Scale);
                  break;
                }
                break;
            }
            bool flag1 = true;
            // ISSUE: reference to a compiler-generated field
            Entity prefab1 = this.m_PrefabRefs[ambienceEntity].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            bool flag2 = (this.m_EffectDatas[prefab1].m_Flags.m_RequiredFlags & EffectConditionFlags.Cold) != 0;
            // ISSUE: reference to a compiler-generated field
            bool flag3 = (this.m_EffectDatas[prefab1].m_Flags.m_ForbiddenFlags & EffectConditionFlags.Cold) != 0;
            if (flag2 | flag3)
            {
              // ISSUE: reference to a compiler-generated field
              bool isColdSeason = this.m_EffectFlagData.m_IsColdSeason;
              flag1 = flag2 & isColdSeason || flag3 && !isColdSeason;
            }
            if ((double) num1 > 1.0 / 1000.0 & flag1)
            {
              // ISSUE: reference to a compiler-generated field
              EffectInstance effectInstance = this.m_EffectInstances[ambienceEntity];
              // ISSUE: reference to a compiler-generated field
              float y = math.saturate(setting.m_Scale * num1) * math.saturate((float) (((double) setting.m_Height.y - (double) this.m_CameraPosition.y) / ((double) setting.m_Height.y - (double) setting.m_Height.x)));
              float x = math.lerp(effectInstance.m_Intensity, y, setting.m_FadeSpeed);
              effectInstance.m_Position = cameraPosition;
              effectInstance.m_Rotation = quaternion.identity;
              effectInstance.m_Intensity = math.saturate(x);
              // ISSUE: reference to a compiler-generated field
              this.m_EffectInstances[ambienceEntity] = effectInstance;
              // ISSUE: reference to a compiler-generated field
              this.m_SourceUpdateData.Add(ambienceEntity, new Game.Objects.Transform()
              {
                m_Position = cameraPosition,
                m_Rotation = quaternion.identity
              });
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_EffectInstances.HasComponent(ambienceEntity))
              {
                // ISSUE: reference to a compiler-generated field
                EffectInstance effectInstance = this.m_EffectInstances[ambienceEntity] with
                {
                  m_Intensity = 0.0f
                };
                // ISSUE: reference to a compiler-generated field
                this.m_EffectInstances[ambienceEntity] = effectInstance;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_SourceUpdateData.Remove(ambienceEntity);
            }
            bool flag4 = true;
            if (nearAmbienceEntity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              Entity prefab2 = this.m_PrefabRefs[nearAmbienceEntity].m_Prefab;
              // ISSUE: reference to a compiler-generated field
              bool flag5 = (this.m_EffectDatas[prefab2].m_Flags.m_RequiredFlags & EffectConditionFlags.Cold) != 0;
              // ISSUE: reference to a compiler-generated field
              bool flag6 = (this.m_EffectDatas[prefab2].m_Flags.m_ForbiddenFlags & EffectConditionFlags.Cold) != 0;
              if (flag5 | flag6)
              {
                // ISSUE: reference to a compiler-generated field
                bool isColdSeason = this.m_EffectFlagData.m_IsColdSeason;
                flag4 = flag5 & isColdSeason || flag6 && !isColdSeason;
              }
            }
            if ((double) num2 > 1.0 / 1000.0 & flag4)
            {
              // ISSUE: reference to a compiler-generated field
              EffectInstance effectInstance = this.m_EffectInstances[nearAmbienceEntity];
              // ISSUE: reference to a compiler-generated field
              float y = math.saturate(setting.m_Scale * num2) * math.saturate((float) (((double) setting.m_NearHeight.y - (double) this.m_CameraPosition.y) / ((double) setting.m_NearHeight.y - (double) setting.m_NearHeight.x)));
              float x = math.lerp(effectInstance.m_Intensity, y, setting.m_FadeSpeed);
              effectInstance.m_Position = cameraPosition;
              effectInstance.m_Rotation = quaternion.identity;
              effectInstance.m_Intensity = math.saturate(x);
              // ISSUE: reference to a compiler-generated field
              this.m_EffectInstances[nearAmbienceEntity] = effectInstance;
              // ISSUE: reference to a compiler-generated field
              this.m_SourceUpdateData.Add(nearAmbienceEntity, new Game.Objects.Transform()
              {
                m_Position = cameraPosition,
                m_Rotation = quaternion.identity
              });
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_EffectInstances.HasComponent(nearAmbienceEntity))
              {
                // ISSUE: reference to a compiler-generated field
                EffectInstance effectInstance = this.m_EffectInstances[nearAmbienceEntity] with
                {
                  m_Intensity = 0.0f
                };
                // ISSUE: reference to a compiler-generated field
                this.m_EffectInstances[nearAmbienceEntity] = effectInstance;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_SourceUpdateData.Remove(nearAmbienceEntity);
            }
          }
        }
      }

      private bool IsNearForestOnFire(float3 cameraPosition)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_OnFireTrees.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity onFireTree = this.m_OnFireTrees[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(onFireTree) && (double) math.distancesq(this.m_TransformData[onFireTree].m_Position, cameraPosition) < (double) this.m_ForestFireDistance * (double) this.m_ForestFireDistance)
            return true;
        }
        return false;
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<EffectInstance> __Game_Effects_EffectInstance_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EffectData> __Game_Prefabs_EffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EffectInstance_RW_ComponentLookup = state.GetComponentLookup<EffectInstance>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectData_RO_ComponentLookup = state.GetComponentLookup<EffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
      }
    }
  }
}
