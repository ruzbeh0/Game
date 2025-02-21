// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialControlSchemeDeactivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialControlSchemeDeactivationSystem : TutorialDeactivationSystemBase
  {
    private EntityQuery m_PendingTutorialQuery;
    private EntityQuery m_ActiveTutorialQuery;
    private TutorialControlSchemeDeactivationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<ControlSchemeActivationData>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<ControlSchemeActivationData>(), ComponentType.ReadOnly<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      if (InputManager.instance == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PendingTutorialQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CheckDeactivate(this.m_PendingTutorialQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveTutorialQuery.IsEmptyIgnoreFilter || !this.phaseCanDeactivate)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.CheckDeactivate(this.m_ActiveTutorialQuery);
    }

    private void CheckDeactivate(EntityQuery query)
    {
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_ControlSchemeActivationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TutorialControlSchemeDeactivationSystem.DeactivateJob jobData = new TutorialControlSchemeDeactivationSystem.DeactivateJob()
      {
        m_ControlSchemeDeactivationType = this.__TypeHandle.__Game_Tutorials_ControlSchemeActivationData_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ControlScheme = InputManager.instance.activeControlScheme,
        m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter()
      };
      this.Dependency = jobData.ScheduleParallel<TutorialControlSchemeDeactivationSystem.DeactivateJob>(query, this.Dependency);
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
    public TutorialControlSchemeDeactivationSystem()
    {
    }

    [BurstCompile]
    private struct DeactivateJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<ControlSchemeActivationData> m_ControlSchemeDeactivationType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public InputManager.ControlScheme m_ControlScheme;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ControlSchemeActivationData> nativeArray1 = chunk.GetNativeArray<ControlSchemeActivationData>(ref this.m_ControlSchemeDeactivationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (nativeArray1[index].m_ControlScheme != this.m_ControlScheme)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<TutorialActivated>(unfilteredChunkIndex, nativeArray2[index]);
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
      public ComponentTypeHandle<ControlSchemeActivationData> __Game_Tutorials_ControlSchemeActivationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_ControlSchemeActivationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ControlSchemeActivationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
