// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialAutoActivationSystem
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

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialAutoActivationSystem : GameSystemBase
  {
    protected EntityCommandBufferSystem m_BarrierSystem;
    private EntityQuery m_AutoActivateQuery;
    private TutorialAutoActivationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BarrierSystem = (EntityCommandBufferSystem) this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_AutoActivateQuery = this.GetEntityQuery(ComponentType.ReadOnly<AutoActivationData>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<TutorialActivated>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_AutoActivateQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_AutoActivationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TutorialAutoActivationSystem.ActivateJob jobData = new TutorialAutoActivationSystem.ActivateJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_LockedDataFromEntity = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup,
        m_AutoActivationDataTypeHandle = this.__TypeHandle.__Game_Tutorials_AutoActivationData_RO_ComponentTypeHandle,
        m_Writer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<TutorialAutoActivationSystem.ActivateJob>(this.m_AutoActivateQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
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
    public TutorialAutoActivationSystem()
    {
    }

    [BurstCompile]
    private struct ActivateJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<AutoActivationData> m_AutoActivationDataTypeHandle;
      [ReadOnly]
      public ComponentLookup<Locked> m_LockedDataFromEntity;
      public EntityCommandBuffer.ParallelWriter m_Writer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AutoActivationData> nativeArray2 = chunk.GetNativeArray<AutoActivationData>(ref this.m_AutoActivationDataTypeHandle);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_LockedDataFromEntity.HasEnabledComponent<Locked>(nativeArray2[index].m_RequiredUnlock))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Writer.AddComponent<TutorialActivated>(unfilteredChunkIndex, nativeArray1[index]);
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<AutoActivationData> __Game_Tutorials_AutoActivationData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_AutoActivationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AutoActivationData>(true);
      }
    }
  }
}
