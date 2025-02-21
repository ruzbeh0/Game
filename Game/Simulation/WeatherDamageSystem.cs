// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WeatherDamageSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WeatherDamageSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private IconCommandSystem m_IconCommandSystem;
    private CitySystem m_CitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_FacingQuery;
    private EntityQuery m_FireConfigQuery;
    private EntityQuery m_DisasterConfigQuery;
    private EntityArchetype m_DamageEventArchetype;
    private EntityArchetype m_DestroyEventArchetype;
    private WeatherDamageSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_FacingQuery = this.GetEntityQuery(ComponentType.ReadWrite<FacingWeather>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DisasterConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<DisasterConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DamageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Destroy>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FacingQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FireConfigQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DisasterConfigQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      FireConfigurationData singleton1 = this.m_FireConfigQuery.GetSingleton<FireConfigurationData>();
      // ISSUE: reference to a compiler-generated field
      DisasterConfigurationData singleton2 = this.m_DisasterConfigQuery.GetSingleton<DisasterConfigurationData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WeatherPhenomenon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_FacingWeather_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new WeatherDamageSystem.WeatherDamageJob()
      {
        m_DamageEventArchetype = this.m_DamageEventArchetype,
        m_DestroyEventArchetype = this.m_DestroyEventArchetype,
        m_DisasterConfigurationData = singleton2,
        m_StructuralIntegrityData = new EventHelpers.StructuralIntegrityData((SystemBase) this, singleton1),
        m_City = this.m_CitySystem.City,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_FacingWeatherType = this.__TypeHandle.__Game_Events_FacingWeather_RW_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle,
        m_WeatherPhenomenonData = this.__TypeHandle.__Game_Events_WeatherPhenomenon_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabWeatherPhenomenonData = this.__TypeHandle.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup
      }.ScheduleParallel<WeatherDamageSystem.WeatherDamageJob>(this.m_FacingQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public WeatherDamageSystem()
    {
    }

    [BurstCompile]
    private struct WeatherDamageJob : IJobChunk
    {
      [ReadOnly]
      public EntityArchetype m_DamageEventArchetype;
      [ReadOnly]
      public EntityArchetype m_DestroyEventArchetype;
      [ReadOnly]
      public DisasterConfigurationData m_DisasterConfigurationData;
      [ReadOnly]
      public EventHelpers.StructuralIntegrityData m_StructuralIntegrityData;
      [ReadOnly]
      public Entity m_City;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      public ComponentTypeHandle<FacingWeather> m_FacingWeatherType;
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public ComponentLookup<Game.Events.WeatherPhenomenon> m_WeatherPhenomenonData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<WeatherPhenomenonData> m_PrefabWeatherPhenomenonData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        float num = 1.06666672f;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<FacingWeather> nativeArray3 = chunk.GetNativeArray<FacingWeather>(ref this.m_FacingWeatherType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Damaged> nativeArray4 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray5 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        bool isBuilding = chunk.Has<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Destroyed>(ref this.m_DestroyedType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity entity1 = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          FacingWeather facingWeather = nativeArray3[index];
          Transform transform = nativeArray5[index];
          // ISSUE: reference to a compiler-generated field
          if (!flag && this.m_WeatherPhenomenonData.HasComponent(facingWeather.m_Event))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Events.WeatherPhenomenon weatherPhenomenon = this.m_WeatherPhenomenonData[facingWeather.m_Event];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WeatherPhenomenonData weatherPhenomenonData = this.m_PrefabWeatherPhenomenonData[this.m_PrefabRefData[facingWeather.m_Event].m_Prefab];
            facingWeather.m_Severity = EventUtils.GetSeverity(transform.m_Position, weatherPhenomenon, weatherPhenomenonData);
          }
          else
            facingWeather.m_Severity = 0.0f;
          if ((double) facingWeather.m_Severity > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            float structuralIntegrity = this.m_StructuralIntegrityData.GetStructuralIntegrity(prefabRef.m_Prefab, isBuilding);
            float x = facingWeather.m_Severity / structuralIntegrity;
            if (isBuilding)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
              CityUtils.ApplyModifier(ref x, cityModifier, CityModifierType.DisasterDamageRate);
            }
            x = math.min(0.5f, x * num);
            if ((double) x > 0.0)
            {
              if (nativeArray4.Length != 0)
              {
                Damaged damaged = nativeArray4[index];
                damaged.m_Damage.x = math.min(1f, damaged.m_Damage.x + x);
                if (!flag && (double) ObjectUtils.GetTotalDamage(damaged) == 1.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DestroyEventArchetype);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Destroy>(unfilteredChunkIndex, entity2, new Destroy(entity1, facingWeather.m_Event));
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, IconPriority.Problem);
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, IconPriority.FatalProblem);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(entity1, this.m_DisasterConfigurationData.m_WeatherDestroyedNotificationPrefab, IconPriority.FatalProblem, flags: IconFlags.IgnoreTarget, target: facingWeather.m_Event);
                  facingWeather.m_Severity = 0.0f;
                }
                nativeArray4[index] = damaged;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity3 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DamageEventArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Damage>(unfilteredChunkIndex, entity3, new Damage(entity1, new float3(x, 0.0f, 0.0f)));
              }
            }
          }
          if ((double) facingWeather.m_Severity > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Add(entity1, this.m_DisasterConfigurationData.m_WeatherDamageNotificationPrefab, (double) facingWeather.m_Severity >= 30.0 ? IconPriority.MajorProblem : IconPriority.Problem, flags: IconFlags.IgnoreTarget, target: facingWeather.m_Event);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<FacingWeather>(unfilteredChunkIndex, entity1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(entity1, this.m_DisasterConfigurationData.m_WeatherDamageNotificationPrefab);
          }
          nativeArray3[index] = facingWeather;
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      public ComponentTypeHandle<FacingWeather> __Game_Events_FacingWeather_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Events.WeatherPhenomenon> __Game_Events_WeatherPhenomenon_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WeatherPhenomenonData> __Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_FacingWeather_RW_ComponentTypeHandle = state.GetComponentTypeHandle<FacingWeather>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WeatherPhenomenon_RO_ComponentLookup = state.GetComponentLookup<Game.Events.WeatherPhenomenon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup = state.GetComponentLookup<WeatherPhenomenonData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
