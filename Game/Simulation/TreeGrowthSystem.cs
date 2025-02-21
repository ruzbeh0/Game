// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TreeGrowthSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
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
  public class TreeGrowthSystem : GameSystemBase
  {
    public const int UPDATES_PER_DAY = 32;
    public const int TICK_SPEED_CHILD = 1280;
    public const int TICK_SPEED_TEEN = 938;
    public const int TICK_SPEED_ADULT = 548;
    public const int TICK_SPEED_ELDERLY = 548;
    public const int TICK_SPEED_DEAD = 2304;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_TreeQuery;
    private TreeGrowthSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TreeQuery = this.GetEntityQuery(ComponentType.ReadWrite<Tree>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Overridden>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TreeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 32, 16);
      // ISSUE: reference to a compiler-generated field
      this.m_TreeQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_TreeQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(updateFrame));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new TreeGrowthSystem.TreeGrowthJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RW_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle,
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<TreeGrowthSystem.TreeGrowthJob>(this.m_TreeQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public TreeGrowthSystem()
    {
    }

    [BurstCompile]
    private struct TreeGrowthJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Tree> m_TreeType;
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Tree> nativeArray2 = chunk.GetNativeArray<Tree>(ref this.m_TreeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Destroyed> nativeArray3 = chunk.GetNativeArray<Destroyed>(ref this.m_DestroyedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Damaged> nativeArray4 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Tree tree = nativeArray2[index];
            Destroyed destroyed = nativeArray3[index];
            // ISSUE: reference to a compiler-generated method
            if (this.TickTree(ref tree, ref destroyed, ref random))
            {
              Entity e = nativeArray1[index];
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, e, new BatchesUpdated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Destroyed>(unfilteredChunkIndex, e);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Damaged>(unfilteredChunkIndex, e);
            }
            nativeArray2[index] = tree;
            nativeArray3[index] = destroyed;
          }
        }
        else if (nativeArray4.Length != 0)
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Tree tree = nativeArray2[index];
            Damaged damaged = nativeArray4[index];
            bool stateChanged;
            // ISSUE: reference to a compiler-generated method
            if (this.TickTree(ref tree, ref damaged, ref random, out stateChanged))
            {
              Entity e = nativeArray1[index];
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, e, new BatchesUpdated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Damaged>(unfilteredChunkIndex, e);
            }
            if (stateChanged)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, nativeArray1[index], new BatchesUpdated());
            }
            nativeArray2[index] = tree;
            nativeArray4[index] = damaged;
          }
        }
        else
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Tree tree = nativeArray2[index];
            // ISSUE: reference to a compiler-generated method
            if (this.TickTree(ref tree, ref random))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, nativeArray1[index], new BatchesUpdated());
            }
            nativeArray2[index] = tree;
          }
        }
      }

      private bool TickTree(ref Tree tree, ref Random random)
      {
        switch (tree.m_State & (TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump))
        {
          case TreeState.Teen:
            // ISSUE: reference to a compiler-generated method
            return this.TickTeen(ref tree, ref random);
          case TreeState.Adult:
            // ISSUE: reference to a compiler-generated method
            return this.TickAdult(ref tree, ref random);
          case TreeState.Elderly:
            // ISSUE: reference to a compiler-generated method
            return this.TickElderly(ref tree, ref random);
          case TreeState.Dead:
          case TreeState.Stump:
            // ISSUE: reference to a compiler-generated method
            return this.TickDead(ref tree, ref random);
          default:
            // ISSUE: reference to a compiler-generated method
            return this.TickChild(ref tree, ref random);
        }
      }

      private bool TickTree(
        ref Tree tree,
        ref Damaged damaged,
        ref Random random,
        out bool stateChanged)
      {
        switch (tree.m_State & (TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump))
        {
          case TreeState.Elderly:
            // ISSUE: reference to a compiler-generated method
            stateChanged = this.TickElderly(ref tree, ref random);
            damaged.m_Damage -= random.NextFloat3((float3) 0.03137255f);
            damaged.m_Damage = math.max(damaged.m_Damage, float3.zero);
            return damaged.m_Damage.Equals(float3.zero);
          case TreeState.Dead:
          case TreeState.Stump:
            // ISSUE: reference to a compiler-generated method
            stateChanged = this.TickDead(ref tree, ref random);
            return stateChanged;
          default:
            stateChanged = false;
            damaged.m_Damage -= random.NextFloat3((float3) 0.03137255f);
            damaged.m_Damage = math.max(damaged.m_Damage, float3.zero);
            return damaged.m_Damage.Equals(float3.zero);
        }
      }

      private bool TickTree(ref Tree tree, ref Destroyed destroyed, ref Random random)
      {
        destroyed.m_Cleared += random.NextFloat(0.03137255f);
        if ((double) destroyed.m_Cleared < 1.0)
          return false;
        tree.m_State &= ~(TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump);
        tree.m_Growth = (byte) 0;
        destroyed.m_Cleared = 1f;
        return true;
      }

      private bool TickChild(ref Tree tree, ref Random random)
      {
        int num = (int) tree.m_Growth + (random.NextInt(1280) >> 8);
        if (num < 256)
        {
          tree.m_Growth = (byte) num;
          return false;
        }
        tree.m_State |= TreeState.Teen;
        tree.m_Growth = (byte) 0;
        return true;
      }

      private bool TickTeen(ref Tree tree, ref Random random)
      {
        int num = (int) tree.m_Growth + (random.NextInt(938) >> 8);
        if (num < 256)
        {
          tree.m_Growth = (byte) num;
          return false;
        }
        tree.m_State = tree.m_State & ~TreeState.Teen | TreeState.Adult;
        tree.m_Growth = (byte) 0;
        return true;
      }

      private bool TickAdult(ref Tree tree, ref Random random)
      {
        int num = (int) tree.m_Growth + (random.NextInt(548) >> 8);
        if (num < 256)
        {
          tree.m_Growth = (byte) num;
          return false;
        }
        tree.m_State = tree.m_State & ~TreeState.Adult | TreeState.Elderly;
        tree.m_Growth = (byte) 0;
        return true;
      }

      private bool TickElderly(ref Tree tree, ref Random random)
      {
        int num = (int) tree.m_Growth + (random.NextInt(548) >> 8);
        if (num < 256)
        {
          tree.m_Growth = (byte) num;
          return false;
        }
        tree.m_State = tree.m_State & ~TreeState.Elderly | TreeState.Dead;
        tree.m_Growth = (byte) 0;
        return true;
      }

      private bool TickDead(ref Tree tree, ref Random random)
      {
        int num = (int) tree.m_Growth + (random.NextInt(2304) >> 8);
        if (num < 256)
        {
          tree.m_Growth = (byte) num;
          return false;
        }
        tree.m_State &= ~(TreeState.Dead | TreeState.Stump);
        tree.m_Growth = (byte) 0;
        return true;
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
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>();
      }
    }
  }
}
