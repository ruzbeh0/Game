// Decompiled with JetBrains decompiler
// Type: Game.Simulation.SpawnableAmbienceSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Debug;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class SpawnableAmbienceSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 128;
    private SimulationSystem m_SimulationSystem;
    private ZoneAmbienceSystem m_ZoneAmbienceSystem;
    private EntityQuery m_SpawnableQuery;
    private SpawnableAmbienceSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (SpawnableAmbienceSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneAmbienceSystem = this.World.GetOrCreateSystemManaged<ZoneAmbienceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnableQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<BuildingCondition>(),
          ComponentType.ReadOnly<UpdateFrame>(),
          ComponentType.ReadOnly<Renter>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<ResidentialProperty>(),
          ComponentType.ReadOnly<Efficiency>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Abandoned>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Tree>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, SpawnableAmbienceSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnableQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnableQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(updateFrame));
      NativeParallelQueue<SpawnableAmbienceSystem.GroupAmbienceEffect> nativeParallelQueue = new NativeParallelQueue<SpawnableAmbienceSystem.GroupAmbienceEffect>(math.max(1, JobsUtility.JobWorkerCount / 2), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GroupAmbienceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle job0 = new SpawnableAmbienceSystem.SpawnableAmbienceJob()
      {
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_SpawnableAmbienceDatas = this.__TypeHandle.__Game_Prefabs_GroupAmbienceData_RO_ComponentLookup,
        m_Queue = nativeParallelQueue.AsWriter()
      }.ScheduleParallel<SpawnableAmbienceSystem.SpawnableAmbienceJob>(this.m_SpawnableQuery, this.Dependency);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new SpawnableAmbienceSystem.ApplyAmbienceJob()
      {
        m_SpawnableQueue = nativeParallelQueue.AsReader(),
        m_ZoneAmbienceMap = this.m_ZoneAmbienceSystem.GetMap(false, out dependencies)
      }.Schedule<SpawnableAmbienceSystem.ApplyAmbienceJob>(nativeParallelQueue.HashRange, 1, JobHandle.CombineDependencies(job0, dependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneAmbienceSystem.AddWriter(jobHandle);
      nativeParallelQueue.Dispose(jobHandle);
      this.Dependency = job0;
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
    public SpawnableAmbienceSystem()
    {
    }

    private struct GroupAmbienceEffect
    {
      public GroupAmbienceType m_Type;
      public float m_Amount;
      public int m_CellIndex;
    }

    [BurstCompile]
    private struct ApplyAmbienceJob : IJobParallelFor
    {
      public NativeParallelQueue<SpawnableAmbienceSystem.GroupAmbienceEffect>.Reader m_SpawnableQueue;
      public NativeArray<ZoneAmbienceCell> m_ZoneAmbienceMap;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelQueue<SpawnableAmbienceSystem.GroupAmbienceEffect>.Enumerator enumerator = this.m_SpawnableQueue.GetEnumerator(index);
        while (enumerator.MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          SpawnableAmbienceSystem.GroupAmbienceEffect current = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ZoneAmbienceMap.ElementAt<ZoneAmbienceCell>(current.m_CellIndex).m_Accumulator.AddAmbience(current.m_Type, current.m_Amount);
        }
        enumerator.Dispose();
      }
    }

    [BurstCompile]
    private struct SpawnableAmbienceJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<GroupAmbienceData> m_SpawnableAmbienceDatas;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      public NativeParallelQueue<SpawnableAmbienceSystem.GroupAmbienceEffect>.Writer m_Queue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray1 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor1 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        if (bufferAccessor1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity prefab = nativeArray2[index].m_Prefab;
            GroupAmbienceData componentData1;
            Game.Prefabs.BuildingData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnableAmbienceDatas.TryGetComponent(prefab, out componentData1) && this.m_BuildingDatas.TryGetComponent(prefab, out componentData2))
            {
              float3 position = nativeArray1[index].m_Position;
              int num1 = componentData2.m_LotSize.x * componentData2.m_LotSize.y;
              float num2 = (float) (bufferAccessor1[index].Length * num1) * BuildingUtils.GetEfficiency(bufferAccessor2, index);
              int kMapSize = CellMapSystem<ZoneAmbienceCell>.kMapSize;
              int kTextureSize = ZoneAmbienceSystem.kTextureSize;
              int2 cell = CellMapSystem<ZoneAmbienceCell>.GetCell(position, kMapSize, kTextureSize);
              int num3 = cell.x + cell.y * ZoneAmbienceSystem.kTextureSize;
              // ISSUE: reference to a compiler-generated field
              int hashCode = num3 * this.m_Queue.HashRange / (ZoneAmbienceSystem.kTextureSize * ZoneAmbienceSystem.kTextureSize);
              if (cell.x >= 0 && cell.y >= 0 && cell.x < ZoneAmbienceSystem.kTextureSize && cell.y < ZoneAmbienceSystem.kTextureSize)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_Queue.Enqueue(hashCode, new SpawnableAmbienceSystem.GroupAmbienceEffect()
                {
                  m_Amount = num2,
                  m_Type = componentData1.m_AmbienceType,
                  m_CellIndex = num3
                });
              }
            }
          }
        }
        else
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            int2 cell = CellMapSystem<ZoneAmbienceCell>.GetCell(nativeArray1[index].m_Position, CellMapSystem<ZoneAmbienceCell>.kMapSize, ZoneAmbienceSystem.kTextureSize);
            int num = cell.x + cell.y * ZoneAmbienceSystem.kTextureSize;
            // ISSUE: reference to a compiler-generated field
            int hashCode = num * this.m_Queue.HashRange / (ZoneAmbienceSystem.kTextureSize * ZoneAmbienceSystem.kTextureSize);
            if (cell.x >= 0 && cell.y >= 0 && cell.x < ZoneAmbienceSystem.kTextureSize && cell.y < ZoneAmbienceSystem.kTextureSize)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_Queue.Enqueue(hashCode, new SpawnableAmbienceSystem.GroupAmbienceEffect()
              {
                m_Amount = 1f,
                m_Type = GroupAmbienceType.Forest,
                m_CellIndex = num
              });
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
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GroupAmbienceData> __Game_Prefabs_GroupAmbienceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GroupAmbienceData_RO_ComponentLookup = state.GetComponentLookup<GroupAmbienceData>(true);
      }
    }
  }
}
