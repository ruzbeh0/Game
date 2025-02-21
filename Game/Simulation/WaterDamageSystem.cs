// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterDamageSystem
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
  public class WaterDamageSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private IconCommandSystem m_IconCommandSystem;
    private CitySystem m_CitySystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_FloodedQuery;
    private EntityQuery m_FireConfigQuery;
    private EntityQuery m_DisasterConfigQuery;
    private EntityArchetype m_DamageEventArchetype;
    private EntityArchetype m_DestroyEventArchetype;
    private WaterDamageSystem.TypeHandle __TypeHandle;

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
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_FloodedQuery = this.GetEntityQuery(ComponentType.ReadWrite<Flooded>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DisasterConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<DisasterConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DamageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Destroy>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FloodedQuery);
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
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Flooded_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new WaterDamageSystem.WaterDamageJob()
      {
        m_DamageEventArchetype = this.m_DamageEventArchetype,
        m_DestroyEventArchetype = this.m_DestroyEventArchetype,
        m_DisasterConfigurationData = singleton2,
        m_StructuralIntegrityData = new EventHelpers.StructuralIntegrityData((SystemBase) this, singleton1),
        m_City = this.m_CitySystem.City,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_FloodedType = this.__TypeHandle.__Game_Events_Flooded_RW_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup
      }.ScheduleParallel<WaterDamageSystem.WaterDamageJob>(this.m_FloodedQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
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
    public WaterDamageSystem()
    {
    }

    [BurstCompile]
    private struct WaterDamageJob : IJobChunk
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
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
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
      public ComponentTypeHandle<Flooded> m_FloodedType;
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        float num1 = 1.06666672f;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Flooded> nativeArray3 = chunk.GetNativeArray<Flooded>(ref this.m_FloodedType);
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
          Flooded flooded = nativeArray3[index];
          Transform transform = nativeArray5[index];
          // ISSUE: reference to a compiler-generated method
          flooded.m_Depth = this.GetFloodDepth(transform.m_Position);
          float num2 = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (!flag && this.m_ObjectGeometryData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            num2 = math.min(this.m_DisasterConfigurationData.m_FloodDamageRate, flooded.m_Depth * this.m_DisasterConfigurationData.m_FloodDamageRate / math.max(0.5f, objectGeometryData.m_Size.y));
          }
          if ((double) num2 > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            float structuralIntegrity = this.m_StructuralIntegrityData.GetStructuralIntegrity(prefabRef.m_Prefab, isBuilding);
            float z = num2 / structuralIntegrity;
            if (isBuilding)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
              CityUtils.ApplyModifier(ref z, cityModifier, CityModifierType.DisasterDamageRate);
            }
            z = math.min(0.5f, z * num1);
            if ((double) z > 0.0)
            {
              if (nativeArray4.Length != 0)
              {
                Damaged damaged = nativeArray4[index];
                damaged.m_Damage.z = math.min(1f, damaged.m_Damage.z + z);
                if (!flag && (double) ObjectUtils.GetTotalDamage(damaged) == 1.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DestroyEventArchetype);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Destroy>(unfilteredChunkIndex, entity2, new Destroy(entity1, flooded.m_Event));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, this.m_DisasterConfigurationData.m_WaterDamageNotificationPrefab);
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, IconPriority.Problem);
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, IconPriority.FatalProblem);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(entity1, this.m_DisasterConfigurationData.m_WaterDestroyedNotificationPrefab, IconPriority.FatalProblem, flags: IconFlags.IgnoreTarget, target: flooded.m_Event);
                  num2 = 0.0f;
                }
                nativeArray4[index] = damaged;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity3 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DamageEventArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Damage>(unfilteredChunkIndex, entity3, new Damage(entity1, new float3(0.0f, 0.0f, z)));
              }
            }
          }
          if ((double) flooded.m_Depth > 0.0)
          {
            if ((double) num2 > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(entity1, this.m_DisasterConfigurationData.m_WaterDamageNotificationPrefab, (double) num2 >= 30.0 ? IconPriority.MajorProblem : IconPriority.Problem, flags: IconFlags.IgnoreTarget, target: flooded.m_Event);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Flooded>(unfilteredChunkIndex, entity1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(entity1, this.m_DisasterConfigurationData.m_WaterDamageNotificationPrefab);
          }
          nativeArray3[index] = flooded;
        }
      }

      private float GetFloodDepth(float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        float num = WaterUtils.SampleDepth(ref this.m_WaterSurfaceData, position);
        if ((double) num > 0.5)
        {
          // ISSUE: reference to a compiler-generated field
          float floodDepth = num + (TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, position) - position.y);
          if ((double) floodDepth > 0.5)
            return floodDepth;
        }
        return 0.0f;
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
      public ComponentTypeHandle<Flooded> __Game_Events_Flooded_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
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
        this.__Game_Events_Flooded_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Flooded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
