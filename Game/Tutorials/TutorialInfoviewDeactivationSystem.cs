// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialInfoviewDeactivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialInfoviewDeactivationSystem : TutorialDeactivationSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_PendingTutorialQuery;
    private EntityQuery m_ActiveTutorialQuery;
    private TutorialInfoviewDeactivationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<InfoviewActivationData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<InfoviewActivationData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PendingTutorialQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CheckDeactivation(this.m_PendingTutorialQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveTutorialQuery.IsEmptyIgnoreFilter || !this.phaseCanDeactivate)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.CheckDeactivation(this.m_ActiveTutorialQuery);
    }

    private void CheckDeactivation(EntityQuery query)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_InfoviewActivationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TutorialInfoviewDeactivationSystem.CheckDeactivationJob jobData = new TutorialInfoviewDeactivationSystem.CheckDeactivationJob()
      {
        m_ActivationType = this.__TypeHandle.__Game_Tutorials_InfoviewActivationData_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_Infoview = this.GetActiveInfoview(),
        m_Buffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter()
      };
      this.Dependency = jobData.ScheduleParallel<TutorialInfoviewDeactivationSystem.CheckDeactivationJob>(query, this.Dependency);
      this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
    }

    private Entity GetActiveInfoview()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return (Object) this.m_ToolSystem.activeInfoview == (Object) null ? Entity.Null : this.m_PrefabSystem.GetEntity((PrefabBase) this.m_ToolSystem.activeInfoview);
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
    public TutorialInfoviewDeactivationSystem()
    {
    }

    [BurstCompile]
    private struct CheckDeactivationJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<InfoviewActivationData> m_ActivationType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public Entity m_Infoview;
      public EntityCommandBuffer.ParallelWriter m_Buffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<InfoviewActivationData> nativeArray1 = chunk.GetNativeArray<InfoviewActivationData>(ref this.m_ActivationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (nativeArray1[index].m_Infoview != this.m_Infoview)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Buffer.RemoveComponent<TutorialActivated>(unfilteredChunkIndex, nativeArray2[index]);
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
      public ComponentTypeHandle<InfoviewActivationData> __Game_Tutorials_InfoviewActivationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_InfoviewActivationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewActivationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
