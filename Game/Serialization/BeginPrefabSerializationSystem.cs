// Decompiled with JetBrains decompiler
// Type: Game.Serialization.BeginPrefabSerializationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class BeginPrefabSerializationSystem : GameSystemBase
  {
    private SaveGameSystem m_SaveGameSystem;
    private CheckPrefabReferencesSystem m_CheckPrefabReferencesSystem;
    private UpdateSystem m_UpdateSystem;
    private EntityQuery m_EnabledPrefabsQuery;
    private EntityQuery m_LoadedPrefabsQuery;
    private BeginPrefabSerializationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SaveGameSystem = this.World.GetOrCreateSystemManaged<SaveGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CheckPrefabReferencesSystem = this.World.GetOrCreateSystemManaged<CheckPrefabReferencesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledPrefabsQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedPrefabsQuery = this.GetEntityQuery(ComponentType.ReadOnly<LoadedIndex>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> array = new NativeArray<Entity>(this.m_LoadedPrefabsQuery.CalculateEntityCountWithoutFiltering(), Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependencies1 = new BeginPrefabSerializationSystem.BeginPrefabSerializationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabDataType = this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle,
        m_PrefabArray = array
      }.ScheduleParallel<BeginPrefabSerializationSystem.BeginPrefabSerializationJob>(this.m_LoadedPrefabsQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CheckPrefabReferencesSystem.BeginPrefabCheck(array, false, dependencies1);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.PrefabReferences);
      // ISSUE: reference to a compiler-generated field
      if (this.m_SaveGameSystem.context.purpose == Colossal.Serialization.Entities.Purpose.SaveGame)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        JobHandle dependencies2;
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_CheckPrefabReferencesSystem.AddPrefabReferencesUser(new BeginPrefabSerializationSystem.CheckSavedPrefabsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle,
          m_SignatureBuildingType = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle,
          m_PlacedSignatureBuildingType = this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle,
          m_CollectedCityServiceBudgetType = this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentTypeHandle,
          m_CollectedCityServiceFeeType = this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RO_BufferTypeHandle,
          m_CollectedCityServiceUpkeepType = this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferTypeHandle,
          m_PrefabReferences = this.m_CheckPrefabReferencesSystem.GetPrefabReferences((SystemBase) this, out dependencies2)
        }.ScheduleParallel<BeginPrefabSerializationSystem.CheckSavedPrefabsJob>(this.m_LoadedPrefabsQuery, dependencies2));
        // ISSUE: reference to a compiler-generated field
        this.m_CheckPrefabReferencesSystem.Update();
      }
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CheckPrefabReferencesSystem.EndPrefabCheck(out dependencies3);
      array.Dispose(dependencies3);
      dependencies3.Complete();
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_EnabledPrefabsQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, dependencies3, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LoadedIndex_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps = new BeginPrefabSerializationSystem.SetPrefabDataIndexJob()
      {
        m_PrefabChunks = archetypeChunkListAsync,
        m_PrefabDataType = this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle,
        m_LoadedIndexType = this.__TypeHandle.__Game_Prefabs_LoadedIndex_RW_BufferTypeHandle
      }.Schedule<BeginPrefabSerializationSystem.SetPrefabDataIndexJob>(outJobHandle);
      archetypeChunkListAsync.Dispose(inputDeps);
      this.Dependency = inputDeps;
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
    public BeginPrefabSerializationSystem()
    {
    }

    [BurstCompile]
    private struct BeginPrefabSerializationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;
      [NativeDisableParallelForRestriction]
      public NativeArray<Entity> m_PrefabArray;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> nativeArray2 = chunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
        // ISSUE: reference to a compiler-generated field
        EnabledMask enabledMask = chunk.GetEnabledMask<PrefabData>(ref this.m_PrefabDataType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          PrefabData prefabData = nativeArray2[index];
          enabledMask[index] = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_PrefabArray[math.select(prefabData.m_Index, this.m_PrefabArray.Length + prefabData.m_Index, prefabData.m_Index < 0)] = nativeArray1[index];
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

    [BurstCompile]
    private struct CheckSavedPrefabsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Locked> m_LockedType;
      [ReadOnly]
      public ComponentTypeHandle<SignatureBuildingData> m_SignatureBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<PlacedSignatureBuildingData> m_PlacedSignatureBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<CollectedCityServiceBudgetData> m_CollectedCityServiceBudgetType;
      [ReadOnly]
      public BufferTypeHandle<CollectedCityServiceFeeData> m_CollectedCityServiceFeeType;
      [ReadOnly]
      public BufferTypeHandle<CollectedCityServiceUpkeepData> m_CollectedCityServiceUpkeepType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        PrefabComponents prefabComponents1 = (PrefabComponents) 0;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<SignatureBuildingData>(ref this.m_SignatureBuildingType))
          prefabComponents1 |= PrefabComponents.PlacedSignatureBuilding;
        PrefabComponents prefabComponents2 = (PrefabComponents) 0;
        // ISSUE: reference to a compiler-generated field
        EnabledMask enabledMask = chunk.GetEnabledMask<Locked>(ref this.m_LockedType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<PlacedSignatureBuildingData>(ref this.m_PlacedSignatureBuildingType))
          prefabComponents2 |= PrefabComponents.PlacedSignatureBuilding;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = prefabComponents1 != prefabComponents2 || chunk.Has<CollectedCityServiceBudgetData>(ref this.m_CollectedCityServiceBudgetType) || chunk.Has<CollectedCityServiceFeeData>(ref this.m_CollectedCityServiceFeeType) || chunk.Has<CollectedCityServiceUpkeepData>(ref this.m_CollectedCityServiceUpkeepType);
        if (!flag && !enabledMask.EnableBit.IsValid)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          if (flag || enabledMask.EnableBit.IsValid && !enabledMask[index])
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.SetDirty(nativeArray[index]);
          }
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

    [BurstCompile]
    private struct SetPrefabDataIndexJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_PrefabChunks;
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;
      public BufferTypeHandle<LoadedIndex> m_LoadedIndexType;

      public void Execute()
      {
        int length = 0;
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
          // ISSUE: reference to a compiler-generated field
          EnabledMask enabledMask = prefabChunk.GetEnabledMask<PrefabData>(ref this.m_PrefabDataType);
          for (int index2 = 0; index2 < prefabChunk.Count; ++index2)
            length += math.select(0, 1, enabledMask[index2]);
        }
        NativeArray<int> array = new NativeArray<int>(length, Allocator.Temp);
        int num2 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index3 = 0; index3 < this.m_PrefabChunks.Length; ++index3)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk prefabChunk = this.m_PrefabChunks[index3];
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabData> nativeArray = prefabChunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LoadedIndex> bufferAccessor = prefabChunk.GetBufferAccessor<LoadedIndex>(ref this.m_LoadedIndexType);
          // ISSUE: reference to a compiler-generated field
          EnabledMask enabledMask = prefabChunk.GetEnabledMask<PrefabData>(ref this.m_PrefabDataType);
          for (int index4 = 0; index4 < prefabChunk.Count; ++index4)
          {
            if (enabledMask[index4])
            {
              PrefabData prefabData = nativeArray[index4];
              DynamicBuffer<LoadedIndex> dynamicBuffer = bufferAccessor[index4];
              dynamicBuffer.ResizeUninitialized(1);
              dynamicBuffer[0] = new LoadedIndex()
              {
                m_Index = prefabData.m_Index
              };
              array[num2++] = prefabData.m_Index;
              num1 += math.select(0, 1, prefabData.m_Index < 0);
            }
          }
        }
        array.Sort<int>();
        // ISSUE: reference to a compiler-generated field
        for (int index5 = 0; index5 < this.m_PrefabChunks.Length; ++index5)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk prefabChunk = this.m_PrefabChunks[index5];
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabData> nativeArray = prefabChunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
          // ISSUE: reference to a compiler-generated field
          EnabledMask enabledMask = prefabChunk.GetEnabledMask<PrefabData>(ref this.m_PrefabDataType);
          for (int index6 = 0; index6 < prefabChunk.Count; ++index6)
          {
            if (enabledMask[index6])
            {
              PrefabData prefabData = nativeArray[index6];
              int num3 = 0;
              int num4 = num2;
              while (num3 < num4)
              {
                int index7 = num3 + num4 >> 1;
                int num5 = array[index7];
                if (num5 < prefabData.m_Index)
                  num3 = index7 + 1;
                else if (num5 > prefabData.m_Index)
                {
                  num4 = index7;
                }
                else
                {
                  num3 = index7;
                  break;
                }
              }
              prefabData.m_Index = num3 - num1;
              nativeArray[index6] = prefabData;
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PlacedSignatureBuildingData> __Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CollectedCityServiceBudgetData> __Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<CollectedCityServiceFeeData> __Game_Simulation_CollectedCityServiceFeeData_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<CollectedCityServiceUpkeepData> __Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferTypeHandle;
      public BufferTypeHandle<LoadedIndex> __Game_Prefabs_LoadedIndex_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PlacedSignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CollectedCityServiceBudgetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceFeeData_RO_BufferTypeHandle = state.GetBufferTypeHandle<CollectedCityServiceFeeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferTypeHandle = state.GetBufferTypeHandle<CollectedCityServiceUpkeepData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LoadedIndex_RW_BufferTypeHandle = state.GetBufferTypeHandle<LoadedIndex>();
      }
    }
  }
}
