// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FireHazardSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Events;
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
  public class FireHazardSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private const int UPDATES_PER_DAY = 64;
    private LocalEffectSystem m_LocalEffectSystem;
    private PrefabSystem m_PrefabSystem;
    private ClimateSystem m_ClimateSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_FlammableQuery;
    private EntityQuery m_FirePrefabQuery;
    private EntityQuery m_FireConfigQuery;
    private FireHazardSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4096;

    public float noRainDays { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalEffectSystem = this.World.GetOrCreateSystemManaged<LocalEffectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_FlammableQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Tree>()
        },
        None = new ComponentType[6]
        {
          ComponentType.ReadOnly<Game.Buildings.FireStation>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<OnFire>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Overridden>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_FirePrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<EventData>(), ComponentType.ReadOnly<FireData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FlammableQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FirePrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ClimateSystem.isRaining)
        this.noRainDays = 0.0f;
      else
        this.noRainDays += 1f / 64f;
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      LocalEffectSystem.ReadData readData = this.m_LocalEffectSystem.GetReadData(out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      FireConfigurationPrefab prefab = this.m_PrefabSystem.GetPrefab<FireConfigurationPrefab>(this.m_FireConfigQuery.GetSingletonEntity());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new FireHazardSystem.FireHazardJob()
      {
        m_FirePrefabChunks = this.m_FirePrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
        m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle,
        m_UnderConstructionType = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabEventType = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle,
        m_PrefabFireType = this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentTypeHandle,
        m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle,
        m_FireHazardData = new EventHelpers.FireHazardData((SystemBase) this, readData, prefab, (float) this.m_ClimateSystem.temperature, this.noRainDays),
        m_RandomSeed = RandomSeed.Next(),
        m_NaturalDisasters = this.m_CityConfigurationSystem.naturalDisasters,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<FireHazardSystem.FireHazardJob>(this.m_FlammableQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LocalEffectSystem.AddLocalEffectReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.noRainDays);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      float num;
      reader.Read(out num);
      this.noRainDays = num;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      this.noRainDays = 0.0f;
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
    public FireHazardSystem()
    {
    }

    [BurstCompile]
    private struct FireHazardJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_FirePrefabChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      [ReadOnly]
      public ComponentTypeHandle<Tree> m_TreeType;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> m_UnderConstructionType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<EventData> m_PrefabEventType;
      [ReadOnly]
      public ComponentTypeHandle<FireData> m_PrefabFireType;
      [ReadOnly]
      public ComponentTypeHandle<Locked> m_LockedType;
      [ReadOnly]
      public EventHelpers.FireHazardData m_FireHazardData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public bool m_NaturalDisasters;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        if (random.NextInt(64) != 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        float riskFactor;
        if (nativeArray3.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<CurrentDistrict> nativeArray4 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Damaged> nativeArray5 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<UnderConstruction> nativeArray6 = chunk.GetNativeArray<UnderConstruction>(ref this.m_UnderConstructionType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            PrefabRef prefabRef = nativeArray2[index];
            Building building = nativeArray3[index];
            CurrentDistrict currentDistrict = nativeArray4[index];
            Damaged damaged;
            CollectionUtils.TryGet<Damaged>(nativeArray5, index, out damaged);
            UnderConstruction underConstruction;
            if (!CollectionUtils.TryGet<UnderConstruction>(nativeArray6, index, out underConstruction))
              underConstruction = new UnderConstruction()
              {
                m_Progress = byte.MaxValue
              };
            float fireHazard;
            // ISSUE: reference to a compiler-generated field
            if (this.m_FireHazardData.GetFireHazard(prefabRef, building, currentDistrict, damaged, underConstruction, out fireHazard, out riskFactor))
            {
              // ISSUE: reference to a compiler-generated method
              this.TryStartFire(unfilteredChunkIndex, ref random, entity, fireHazard, EventTargetType.Building);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Tree>(ref this.m_TreeType) || !this.m_NaturalDisasters)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Damaged> nativeArray7 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray8 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            PrefabRef prefabRef = nativeArray2[index];
            Transform transform = nativeArray8[index];
            Damaged damaged = new Damaged();
            if (nativeArray7.Length != 0)
              damaged = nativeArray7[index];
            float fireHazard;
            // ISSUE: reference to a compiler-generated field
            if (this.m_FireHazardData.GetFireHazard(prefabRef, new Tree(), transform, damaged, out fireHazard, out riskFactor))
            {
              // ISSUE: reference to a compiler-generated method
              this.TryStartFire(unfilteredChunkIndex, ref random, entity, fireHazard, EventTargetType.WildTree);
            }
          }
        }
      }

      private void TryStartFire(
        int jobIndex,
        ref Random random,
        Entity entity,
        float fireHazard,
        EventTargetType targetType)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_FirePrefabChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk firePrefabChunk = this.m_FirePrefabChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = firePrefabChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<EventData> nativeArray2 = firePrefabChunk.GetNativeArray<EventData>(ref this.m_PrefabEventType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<FireData> nativeArray3 = firePrefabChunk.GetNativeArray<FireData>(ref this.m_PrefabFireType);
          // ISSUE: reference to a compiler-generated field
          EnabledMask enabledMask = firePrefabChunk.GetEnabledMask<Locked>(ref this.m_LockedType);
          for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
          {
            FireData fireData = nativeArray3[index2];
            if (fireData.m_RandomTargetType == targetType && (!enabledMask.EnableBit.IsValid || !enabledMask[index2]))
            {
              float num = fireHazard * fireData.m_StartProbability;
              if ((double) random.NextFloat(10000f) < (double) num)
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateFireEvent(jobIndex, entity, nativeArray1[index2], nativeArray2[index2], fireData);
                return;
              }
            }
          }
        }
      }

      private void CreateFireEvent(
        int jobIndex,
        Entity targetEntity,
        Entity eventPrefab,
        EventData eventData,
        FireData fireData)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, eventData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(eventPrefab));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetBuffer<TargetElement>(jobIndex, entity).Add(new TargetElement(targetEntity));
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
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EventData> __Game_Prefabs_EventData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<FireData> __Game_Prefabs_FireData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FireData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FireData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
      }
    }
  }
}
