// Decompiled with JetBrains decompiler
// Type: Game.Triggers.ChirpLikeCountSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Triggers
{
  [CompilerGenerated]
  public class ChirpLikeCountSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_ChirpQuery;
    private ChirpLikeCountSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpQuery = this.GetEntityQuery(ComponentType.ReadWrite<Chirp>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ChirpQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_Chirp_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ChirpLikeCountSystem.LikeCountUpdateJob jobData = new ChirpLikeCountSystem.LikeCountUpdateJob()
      {
        m_ChirpType = this.__TypeHandle.__Game_Triggers_Chirp_RW_ComponentTypeHandle,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ChirpLikeCountSystem.LikeCountUpdateJob>(this.m_ChirpQuery, this.Dependency);
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
    public ChirpLikeCountSystem()
    {
    }

    [BurstCompile]
    private struct LikeCountUpdateJob : IJobChunk
    {
      public ComponentTypeHandle<Chirp> m_ChirpType;
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public uint m_SimulationFrame;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(0);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Chirp> nativeArray = chunk.GetNativeArray<Chirp>(ref this.m_ChirpType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          ref Chirp local = ref nativeArray.ElementAt<Chirp>(index);
          // ISSUE: reference to a compiler-generated field
          if (local.m_InactiveFrame > local.m_CreationFrame && this.m_SimulationFrame <= local.m_InactiveFrame && (double) random.NextFloat() >= (double) local.m_ContinuousFactor)
          {
            // ISSUE: reference to a compiler-generated field
            float num = (1f * (float) this.m_SimulationFrame - (float) local.m_CreationFrame) / (float) (local.m_InactiveFrame - local.m_CreationFrame);
            local.m_Likes = math.max(local.m_Likes, (uint) ((double) local.m_TargetLikes * (double) math.lerp(0.0f, 1f, 1f - math.pow(1f - num, (float) local.m_ViralFactor))));
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
      public ComponentTypeHandle<Chirp> __Game_Triggers_Chirp_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_Chirp_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Chirp>();
      }
    }
  }
}
