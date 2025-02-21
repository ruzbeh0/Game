// Decompiled with JetBrains decompiler
// Type: Game.Serialization.SecondaryPrefabReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class SecondaryPrefabReferencesSystem : GameSystemBase
  {
    private CheckPrefabReferencesSystem m_CheckPrefabReferencesSystem;
    private EntityQuery m_SpawnableBuildingQuery;
    private EntityQuery m_PlaceholderBuildingQuery;
    private EntityQuery m_ServiceObjectQuery;
    private EntityQuery m_NetLaneQuery;
    private EntityQuery m_ContentPrerequisiteQuery;
    private SecondaryPrefabReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CheckPrefabReferencesSystem = this.World.GetOrCreateSystemManaged<CheckPrefabReferencesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnableBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<SpawnableBuildingData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceholderBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<PlaceholderBuildingData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceObjectQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceObjectData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_NetLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<NetLaneData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ContentPrerequisiteQuery = this.GetEntityQuery(ComponentType.ReadOnly<ContentPrerequisiteData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Deleted>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabReferences prefabReferences = this.m_CheckPrefabReferencesSystem.GetPrefabReferences((SystemBase) this, out dependencies1);
      JobHandle dependsOn1 = JobHandle.CombineDependencies(this.Dependency, dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryPrefabReferencesSystem.FixSpawnableBuildingJob jobData1 = new SecondaryPrefabReferencesSystem.FixSpawnableBuildingJob()
      {
        m_SpawnableBuildingType = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryPrefabReferencesSystem.FixPlaceholderBuildingJob jobData2 = new SecondaryPrefabReferencesSystem.FixPlaceholderBuildingJob()
      {
        m_PlaceholderBuildingType = this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryPrefabReferencesSystem.FixServiceObjectDataJob jobData3 = new SecondaryPrefabReferencesSystem.FixServiceObjectDataJob()
      {
        m_ServiceObjectType = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryPrefabReferencesSystem.FixNetLaneDataJob jobData4 = new SecondaryPrefabReferencesSystem.FixNetLaneDataJob()
      {
        m_NetLaneType = this.__TypeHandle.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ContentPrerequisiteData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SecondaryPrefabReferencesSystem.FixContentPrerequisiteDataJob jobData5 = new SecondaryPrefabReferencesSystem.FixContentPrerequisiteDataJob()
      {
        m_ContentPrerequisiteType = this.__TypeHandle.__Game_Prefabs_ContentPrerequisiteData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = jobData1.ScheduleParallel<SecondaryPrefabReferencesSystem.FixSpawnableBuildingJob>(this.m_SpawnableBuildingQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle job1 = jobData2.ScheduleParallel<SecondaryPrefabReferencesSystem.FixPlaceholderBuildingJob>(this.m_PlaceholderBuildingQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle job2 = jobData3.ScheduleParallel<SecondaryPrefabReferencesSystem.FixServiceObjectDataJob>(this.m_ServiceObjectQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle job3 = jobData4.ScheduleParallel<SecondaryPrefabReferencesSystem.FixNetLaneDataJob>(this.m_NetLaneQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      EntityQuery prerequisiteQuery = this.m_ContentPrerequisiteQuery;
      JobHandle dependsOn2 = dependsOn1;
      JobHandle job4 = jobData5.ScheduleParallel<SecondaryPrefabReferencesSystem.FixContentPrerequisiteDataJob>(prerequisiteQuery, dependsOn2);
      JobHandle dependencies2 = JobUtils.CombineDependencies(job0, job1, job2, job3, job4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CheckPrefabReferencesSystem.AddPrefabReferencesUser(dependencies2);
      this.Dependency = dependencies2;
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
    public SecondaryPrefabReferencesSystem()
    {
    }

    [BurstCompile]
    private struct FixSpawnableBuildingJob : IJobChunk
    {
      public ComponentTypeHandle<SpawnableBuildingData> m_SpawnableBuildingType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<SpawnableBuildingData> nativeArray = chunk.GetNativeArray<SpawnableBuildingData>(ref this.m_SpawnableBuildingType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          SpawnableBuildingData spawnableBuildingData = nativeArray[index];
          if (spawnableBuildingData.m_ZonePrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref spawnableBuildingData.m_ZonePrefab);
          }
          nativeArray[index] = spawnableBuildingData;
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
    private struct FixPlaceholderBuildingJob : IJobChunk
    {
      public ComponentTypeHandle<PlaceholderBuildingData> m_PlaceholderBuildingType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PlaceholderBuildingData> nativeArray = chunk.GetNativeArray<PlaceholderBuildingData>(ref this.m_PlaceholderBuildingType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          PlaceholderBuildingData placeholderBuildingData = nativeArray[index];
          if (placeholderBuildingData.m_ZonePrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref placeholderBuildingData.m_ZonePrefab);
          }
          nativeArray[index] = placeholderBuildingData;
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
    private struct FixServiceObjectDataJob : IJobChunk
    {
      public ComponentTypeHandle<ServiceObjectData> m_ServiceObjectType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceObjectData> nativeArray = chunk.GetNativeArray<ServiceObjectData>(ref this.m_ServiceObjectType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ServiceObjectData serviceObjectData = nativeArray[index];
          if (serviceObjectData.m_Service != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref serviceObjectData.m_Service);
          }
          nativeArray[index] = serviceObjectData;
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
    private struct FixNetLaneDataJob : IJobChunk
    {
      public ComponentTypeHandle<NetLaneData> m_NetLaneType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetLaneData> nativeArray = chunk.GetNativeArray<NetLaneData>(ref this.m_NetLaneType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          NetLaneData netLaneData = nativeArray[index];
          if (netLaneData.m_PathfindPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref netLaneData.m_PathfindPrefab);
          }
          nativeArray[index] = netLaneData;
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
    private struct FixContentPrerequisiteDataJob : IJobChunk
    {
      public ComponentTypeHandle<ContentPrerequisiteData> m_ContentPrerequisiteType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ContentPrerequisiteData> nativeArray = chunk.GetNativeArray<ContentPrerequisiteData>(ref this.m_ContentPrerequisiteType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ContentPrerequisiteData prerequisiteData = nativeArray[index];
          if (prerequisiteData.m_ContentPrerequisite != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref prerequisiteData.m_ContentPrerequisite);
          }
          nativeArray[index] = prerequisiteData;
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
      public ComponentTypeHandle<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PlaceholderBuildingData> __Game_Prefabs_PlaceholderBuildingData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetLaneData> __Game_Prefabs_NetLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ContentPrerequisiteData> __Game_Prefabs_ContentPrerequisiteData_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableBuildingData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderBuildingData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PlaceholderBuildingData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ContentPrerequisiteData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ContentPrerequisiteData>();
      }
    }
  }
}
