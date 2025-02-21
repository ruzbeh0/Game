// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CityDangerLevelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CityDangerLevelSystem : GameSystemBase
  {
    private CitySystem m_CitySystem;
    private EntityQuery m_DangerLevelQuery;
    private CityDangerLevelSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DangerLevelQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Events.DangerLevel>(), ComponentType.Exclude<Deleted>());
      this.RequireForUpdate<Game.City.DangerLevel>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeAccumulator<MaxFloat> nativeAccumulator = new NativeAccumulator<MaxFloat>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DangerLevelQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_DangerLevel_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CityDangerLevelSystem.DangerLevelJob jobData = new CityDangerLevelSystem.DangerLevelJob()
        {
          m_DangerLevelType = this.__TypeHandle.__Game_Events_DangerLevel_RO_ComponentTypeHandle,
          m_Result = nativeAccumulator.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<CityDangerLevelSystem.DangerLevelJob>(this.m_DangerLevelQuery, this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_DangerLevel_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityDangerLevelSystem.UpdateCityJob jobData1 = new CityDangerLevelSystem.UpdateCityJob()
      {
        m_DangerLevel = this.__TypeHandle.__Game_City_DangerLevel_RW_ComponentLookup,
        m_City = this.m_CitySystem.City,
        m_Result = nativeAccumulator
      };
      this.Dependency = jobData1.Schedule<CityDangerLevelSystem.UpdateCityJob>(this.Dependency);
      nativeAccumulator.Dispose(this.Dependency);
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
    public CityDangerLevelSystem()
    {
    }

    [BurstCompile]
    private struct DangerLevelJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Events.DangerLevel> m_DangerLevelType;
      public NativeAccumulator<MaxFloat>.ParallelWriter m_Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Events.DangerLevel> nativeArray = chunk.GetNativeArray<Game.Events.DangerLevel>(ref this.m_DangerLevelType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Result.Accumulate(new MaxFloat(nativeArray[index].m_DangerLevel));
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
    private struct UpdateCityJob : IJob
    {
      public ComponentLookup<Game.City.DangerLevel> m_DangerLevel;
      public Entity m_City;
      [ReadOnly]
      public NativeAccumulator<MaxFloat> m_Result;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_DangerLevel[this.m_City] = new Game.City.DangerLevel()
        {
          m_DangerLevel = this.m_Result.GetResult().m_Value
        };
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Events.DangerLevel> __Game_Events_DangerLevel_RO_ComponentTypeHandle;
      public ComponentLookup<Game.City.DangerLevel> __Game_City_DangerLevel_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_DangerLevel_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Events.DangerLevel>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_DangerLevel_RW_ComponentLookup = state.GetComponentLookup<Game.City.DangerLevel>();
      }
    }
  }
}
