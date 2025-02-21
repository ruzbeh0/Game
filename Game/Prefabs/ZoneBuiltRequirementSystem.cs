// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneBuiltRequirementSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Serialization;
using Game.Tools;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ZoneBuiltRequirementSystem : GameSystemBase, IPreDeserialize
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_UpdatedBuildingsQuery;
    private EntityQuery m_AllBuildingsQuery;
    private EntityQuery m_RequirementQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private NativeParallelHashMap<ZoneBuiltDataKey, ZoneBuiltDataValue> m_ZoneBuiltData;
    private NativeQueue<ZoneBuiltLevelUpdate> m_ZoneBuiltLevelQueue;
    private JobHandle m_WriteDeps;
    private JobHandle m_QueueWriteDeps;
    private bool m_Loaded;
    private ZoneBuiltRequirementSystem.TypeHandle __TypeHandle;

    public NativeQueue<ZoneBuiltLevelUpdate> GetZoneBuiltLevelQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_QueueWriteDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_ZoneBuiltLevelQueue;
    }

    public void AddWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_QueueWriteDeps = JobHandle.CombineDependencies(jobHandle, this.m_QueueWriteDeps);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBuildingsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Building>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllBuildingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_RequirementQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZoneBuiltRequirementData>(), ComponentType.ReadWrite<UnlockRequirementData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneBuiltData = new NativeParallelHashMap<ZoneBuiltDataKey, ZoneBuiltDataValue>(20, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneBuiltLevelQueue = new NativeQueue<ZoneBuiltLevelUpdate>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneBuiltData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneBuiltLevelQueue.Dispose();
      base.OnDestroy();
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = this.GetLoaded() ? this.m_AllBuildingsQuery : this.m_UpdatedBuildingsQuery;
      // ISSUE: reference to a compiler-generated field
      if (entityQuery.IsEmptyIgnoreFilter && this.m_ZoneBuiltLevelQueue.IsEmpty())
        return;
      JobHandle outJobHandle;
      NativeList<ArchetypeChunk> archetypeChunkListAsync = entityQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new ZoneBuiltRequirementSystem.UpdateZoneBuiltDataJob()
      {
        m_BuildingChunks = archetypeChunkListAsync,
        m_ZoneBuiltData = this.m_ZoneBuiltData,
        m_ZoneBuiltLevelQueue = this.m_ZoneBuiltLevelQueue,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup
      }.Schedule<ZoneBuiltRequirementSystem.UpdateZoneBuiltDataJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, this.m_QueueWriteDeps));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDeps = jobHandle;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RequirementQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ThemeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ZoneBuiltRequirementData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
        JobHandle producerJob = new ZoneBuiltRequirementSystem.ZoneBuiltRequirementJob()
        {
          m_ZoneBuiltData = this.m_ZoneBuiltData,
          m_UnlockEventArchetype = this.m_UnlockEventArchetype,
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_ZoneBuiltRequirementType = this.__TypeHandle.__Game_Prefabs_ZoneBuiltRequirementData_RO_ComponentTypeHandle,
          m_UnlockRequirementType = this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle,
          m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
          m_ThemeData = this.__TypeHandle.__Game_Prefabs_ThemeData_RO_ComponentLookup,
          m_ObjectRequirementElements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup
        }.ScheduleParallel<ZoneBuiltRequirementSystem.ZoneBuiltRequirementJob>(this.m_RequirementQuery, jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
        this.Dependency = producerJob;
      }
      else
        this.Dependency = jobHandle;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneBuiltData.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
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
    public ZoneBuiltRequirementSystem()
    {
    }

    private struct ZoneBuiltData
    {
      public Entity m_Theme;
      public Entity m_Zone;
      public int m_Squares;
      public int m_Count;
      public AreaType m_Type;
      public byte m_Level;
    }

    [BurstCompile]
    private struct UpdateZoneBuiltDataJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_BuildingChunks;
      public NativeParallelHashMap<ZoneBuiltDataKey, ZoneBuiltDataValue> m_ZoneBuiltData;
      public NativeQueue<ZoneBuiltLevelUpdate> m_ZoneBuiltLevelQueue;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_BuildingData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_BuildingChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk buildingChunk = this.m_BuildingChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray = buildingChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          bool flag = buildingChunk.Has<Deleted>(ref this.m_DeletedType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            PrefabRef prefabRef = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnableBuildingData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              BuildingData buildingData = this.m_BuildingData[prefabRef.m_Prefab];
              ZoneBuiltDataKey key = new ZoneBuiltDataKey()
              {
                m_Zone = spawnableBuildingData.m_ZonePrefab,
                m_Level = (int) spawnableBuildingData.m_Level
              };
              // ISSUE: reference to a compiler-generated field
              if (!this.m_ZoneBuiltData.ContainsKey(key))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ZoneBuiltData[key] = new ZoneBuiltDataValue(0, 0);
              }
              int num1;
              int num2 = (num1 = flag ? -1 : 1) * buildingData.m_LotSize.x * buildingData.m_LotSize.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ZoneBuiltData[key] = new ZoneBuiltDataValue()
              {
                m_Count = math.max(0, this.m_ZoneBuiltData[key].m_Count + num1),
                m_Squares = math.max(0, this.m_ZoneBuiltData[key].m_Squares + num2)
              };
            }
          }
        }
        ZoneBuiltLevelUpdate builtLevelUpdate;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ZoneBuiltLevelQueue.TryDequeue(out builtLevelUpdate))
        {
          ZoneBuiltDataKey zoneBuiltDataKey = new ZoneBuiltDataKey();
          zoneBuiltDataKey.m_Zone = builtLevelUpdate.m_Zone;
          zoneBuiltDataKey.m_Level = builtLevelUpdate.m_FromLevel;
          ZoneBuiltDataKey key1 = zoneBuiltDataKey;
          zoneBuiltDataKey = new ZoneBuiltDataKey();
          zoneBuiltDataKey.m_Zone = builtLevelUpdate.m_Zone;
          zoneBuiltDataKey.m_Level = builtLevelUpdate.m_ToLevel;
          ZoneBuiltDataKey key2 = zoneBuiltDataKey;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ZoneBuiltData.ContainsKey(key1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ZoneBuiltData[key1] = new ZoneBuiltDataValue(0, 0);
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ZoneBuiltData.ContainsKey(key2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ZoneBuiltData[key2] = new ZoneBuiltDataValue(0, 0);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ZoneBuiltData[key1] = new ZoneBuiltDataValue()
          {
            m_Count = math.max(0, this.m_ZoneBuiltData[key1].m_Count - 1),
            m_Squares = math.max(0, this.m_ZoneBuiltData[key1].m_Squares - builtLevelUpdate.m_Squares)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ZoneBuiltData[key2] = new ZoneBuiltDataValue()
          {
            m_Count = math.max(0, this.m_ZoneBuiltData[key2].m_Count + 1),
            m_Squares = math.max(0, this.m_ZoneBuiltData[key2].m_Squares + builtLevelUpdate.m_Squares)
          };
        }
      }
    }

    [BurstCompile]
    private struct ZoneBuiltRequirementJob : IJobChunk
    {
      [ReadOnly]
      public NativeParallelHashMap<ZoneBuiltDataKey, ZoneBuiltDataValue> m_ZoneBuiltData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [ReadOnly]
      public EntityArchetype m_UnlockEventArchetype;
      [ReadOnly]
      public ComponentLookup<ThemeData> m_ThemeData;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_ObjectRequirementElements;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ZoneBuiltRequirementData> m_ZoneBuiltRequirementType;
      public ComponentTypeHandle<UnlockRequirementData> m_UnlockRequirementType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ZoneBuiltRequirementData> nativeArray2 = chunk.GetNativeArray<ZoneBuiltRequirementData>(ref this.m_ZoneBuiltRequirementType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnlockRequirementData> nativeArray3 = chunk.GetNativeArray<UnlockRequirementData>(ref this.m_UnlockRequirementType);
        ChunkEntityEnumerator entityEnumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        int nextIndex;
        while (entityEnumerator.NextEntityIndex(out nextIndex))
        {
          ZoneBuiltRequirementData zoneBuiltRequirement = nativeArray2[nextIndex];
          UnlockRequirementData unlockRequirement = nativeArray3[nextIndex];
          // ISSUE: reference to a compiler-generated method
          if (this.ShouldUnlock(zoneBuiltRequirement, ref unlockRequirement))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_UnlockEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Unlock>(unfilteredChunkIndex, entity, new Unlock(nativeArray1[nextIndex]));
          }
          nativeArray3[nextIndex] = unlockRequirement;
        }
      }

      private bool ShouldUnlock(
        ZoneBuiltRequirementData zoneBuiltRequirement,
        ref UnlockRequirementData unlockRequirement)
      {
        int x1 = 0;
        int x2 = 0;
        if (zoneBuiltRequirement.m_RequiredZone != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          foreach (KeyValue<ZoneBuiltDataKey, ZoneBuiltDataValue> keyValue in this.m_ZoneBuiltData)
          {
            if (keyValue.Key.m_Zone == zoneBuiltRequirement.m_RequiredZone && keyValue.Key.m_Level >= (int) zoneBuiltRequirement.m_MinimumLevel)
            {
              x1 += keyValue.Value.m_Squares;
              x2 += keyValue.Value.m_Count;
            }
          }
        }
        else if (zoneBuiltRequirement.m_RequiredTheme != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          foreach (KeyValue<ZoneBuiltDataKey, ZoneBuiltDataValue> keyValue in this.m_ZoneBuiltData)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectRequirementElements.HasBuffer(keyValue.Key.m_Zone))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ObjectRequirementElement> requirementElement1 = this.m_ObjectRequirementElements[keyValue.Key.m_Zone];
              for (int index = 0; index < requirementElement1.Length; ++index)
              {
                ObjectRequirementElement requirementElement2 = requirementElement1[index];
                // ISSUE: reference to a compiler-generated field
                if (this.m_ThemeData.HasComponent(requirementElement2.m_Requirement) && requirementElement2.m_Requirement == zoneBuiltRequirement.m_RequiredTheme)
                {
                  x1 += keyValue.Value.m_Squares;
                  x2 += keyValue.Value.m_Count;
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          foreach (KeyValue<ZoneBuiltDataKey, ZoneBuiltDataValue> keyValue in this.m_ZoneBuiltData)
          {
            ZoneData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ZoneData.TryGetComponent(keyValue.Key.m_Zone, out componentData) && componentData.m_AreaType == zoneBuiltRequirement.m_RequiredType && keyValue.Key.m_Level >= (int) zoneBuiltRequirement.m_MinimumLevel)
            {
              x1 += keyValue.Value.m_Squares;
              x2 += keyValue.Value.m_Count;
            }
          }
        }
        unlockRequirement.m_Progress = x1 < zoneBuiltRequirement.m_MinimumSquares || zoneBuiltRequirement.m_MinimumCount == 0 ? math.min(x1, zoneBuiltRequirement.m_MinimumSquares) : math.min(x2, zoneBuiltRequirement.m_MinimumCount);
        return x1 >= zoneBuiltRequirement.m_MinimumSquares && x2 >= zoneBuiltRequirement.m_MinimumCount;
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
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ZoneBuiltRequirementData> __Game_Prefabs_ZoneBuiltRequirementData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<UnlockRequirementData> __Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ThemeData> __Game_Prefabs_ThemeData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneBuiltRequirementData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ZoneBuiltRequirementData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnlockRequirementData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ThemeData_RO_ComponentLookup = state.GetComponentLookup<ThemeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
      }
    }
  }
}
