// Decompiled with JetBrains decompiler
// Type: Game.Notifications.IconAnimationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Notifications
{
  [CompilerGenerated]
  public class IconAnimationSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_AnimationQuery;
    private IconAnimationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationQuery = this.GetEntityQuery(ComponentType.ReadWrite<Animation>(), ComponentType.ReadOnly<Icon>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AnimationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Animation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new IconAnimationSystem.IconAnimationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AnimationType = this.__TypeHandle.__Game_Notifications_Animation_RW_ComponentTypeHandle,
        m_DeltaTime = UnityEngine.Time.deltaTime,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<IconAnimationSystem.IconAnimationJob>(this.m_AnimationQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
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
    public IconAnimationSystem()
    {
    }

    [BurstCompile]
    private struct IconAnimationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Animation> m_AnimationType;
      [ReadOnly]
      public float m_DeltaTime;
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
        NativeArray<Animation> nativeArray2 = chunk.GetNativeArray<Animation>(ref this.m_AnimationType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Animation animation = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          animation.m_Timer += this.m_DeltaTime;
          switch (animation.m_Type)
          {
            case AnimationType.MarkerAppear:
            case AnimationType.WarningAppear:
              if ((double) animation.m_Timer >= (double) animation.m_Duration)
              {
                Entity e = nativeArray1[index];
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Animation>(unfilteredChunkIndex, e);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, e, new Updated());
                break;
              }
              break;
            case AnimationType.MarkerDisappear:
            case AnimationType.WarningResolve:
            case AnimationType.Transaction:
              if ((double) animation.m_Timer >= (double) animation.m_Duration)
              {
                Entity e = nativeArray1[index];
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, e, new Deleted());
                break;
              }
              break;
          }
          nativeArray2[index] = animation;
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public ComponentTypeHandle<Animation> __Game_Notifications_Animation_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Animation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Animation>();
      }
    }
  }
}
