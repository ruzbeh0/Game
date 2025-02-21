// Decompiled with JetBrains decompiler
// Type: Game.Common.RandomLocalizationInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Common
{
  [CompilerGenerated]
  public class RandomLocalizationInitializeSystem : GameSystemBase
  {
    private EntityQuery m_CreatedQuery;
    private RandomLocalizationInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadWrite<RandomLocalizationIndex>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Game.Objects.OutsideConnection>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalizationCount_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RandomLocalizationInitializeSystem.InitializeLocalizationJob jobData = new RandomLocalizationInitializeSystem.InitializeLocalizationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RandomLocalizationIndexType = this.__TypeHandle.__Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle,
        m_LocalizationCounts = this.__TypeHandle.__Game_Prefabs_LocalizationCount_RO_BufferLookup,
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<RandomLocalizationInitializeSystem.InitializeLocalizationJob>(this.m_CreatedQuery, this.Dependency);
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
    public RandomLocalizationInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeLocalizationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public BufferTypeHandle<RandomLocalizationIndex> m_RandomLocalizationIndexType;
      [ReadOnly]
      public BufferLookup<LocalizationCount> m_LocalizationCounts;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RandomLocalizationIndex> bufferAccessor = chunk.GetBufferAccessor<RandomLocalizationIndex>(ref this.m_RandomLocalizationIndexType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          DynamicBuffer<RandomLocalizationIndex> indices = bufferAccessor[index];
          DynamicBuffer<LocalizationCount> counts;
          // ISSUE: reference to a compiler-generated method
          if (this.TryGetLocalizationCount(prefab, out counts))
          {
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_RandomSeed.GetRandom(entity.Index + 1);
            RandomLocalizationIndex.GenerateRandomIndices(indices, counts, ref random);
          }
        }
      }

      private bool TryGetLocalizationCount(
        Entity prefab,
        out DynamicBuffer<LocalizationCount> counts)
      {
        SpawnableBuildingData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_LocalizationCounts.TryGetBuffer(prefab, out counts) || this.m_SpawnableBuildingData.TryGetComponent(prefab, out componentData) && this.m_LocalizationCounts.TryGetBuffer(componentData.m_ZonePrefab, out counts);
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
      public BufferTypeHandle<RandomLocalizationIndex> __Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<LocalizationCount> __Game_Prefabs_LocalizationCount_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle = state.GetBufferTypeHandle<RandomLocalizationIndex>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalizationCount_RO_BufferLookup = state.GetBufferLookup<LocalizationCount>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
      }
    }
  }
}
