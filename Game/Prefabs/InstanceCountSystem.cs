// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.InstanceCountSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Serialization;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class InstanceCountSystem : GameSystemBase, IPreDeserialize
  {
    private EntityQuery m_UpdatedInstancesQuery;
    private EntityQuery m_AllInstancesQuery;
    private NativeParallelHashMap<Entity, int> m_InstanceCounts;
    private JobHandle m_ReadDependencies;
    private JobHandle m_WriteDependencies;
    private bool m_Loaded;
    private InstanceCountSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedInstancesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabRef>()
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
      this.m_AllInstancesQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceCounts = new NativeParallelHashMap<Entity, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceCounts.Dispose();
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
      EntityQuery query = this.GetLoaded() ? this.m_AllInstancesQuery : this.m_UpdatedInstancesQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new InstanceCountSystem.UpdateCountsJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_InstanceCounts = this.GetInstanceCounts(false, out dependencies)
      }.Schedule<InstanceCountSystem.UpdateCountsJob>(query, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.AddCountWriter(jobHandle);
      this.Dependency = jobHandle;
    }

    public NativeParallelHashMap<Entity, int> GetInstanceCounts(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_InstanceCounts;
    }

    public void AddCountReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, jobHandle);
    }

    public void AddCountWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, jobHandle);
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      NativeParallelHashMap<Entity, int> instanceCounts = this.GetInstanceCounts(false, out dependencies);
      dependencies.Complete();
      instanceCounts.Clear();
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
    public InstanceCountSystem()
    {
    }

    [BurstCompile]
    private struct UpdateCountsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      public NativeParallelHashMap<Entity, int> m_InstanceCounts;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            PrefabRef prefabRef = nativeArray[index];
            int num;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstanceCounts.TryGetValue(prefabRef.m_Prefab, out num))
            {
              if (--num > 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_InstanceCounts[prefabRef.m_Prefab] = num;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_InstanceCounts.Remove(prefabRef.m_Prefab);
              }
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            PrefabRef prefabRef = nativeArray[index];
            int num;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstanceCounts.TryGetValue(prefabRef.m_Prefab, out num))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_InstanceCounts[prefabRef.m_Prefab] = num + 1;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_InstanceCounts.Add(prefabRef.m_Prefab, 1);
            }
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

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
      }
    }
  }
}
