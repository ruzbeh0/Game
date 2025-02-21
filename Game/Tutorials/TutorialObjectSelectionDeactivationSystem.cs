// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialObjectSelectionDeactivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
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
  public class TutorialObjectSelectionDeactivationSystem : TutorialDeactivationSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private NetToolSystem m_NetToolSystem;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_PendingTutorialQuery;
    private EntityQuery m_ActiveTutorialQuery;
    private TutorialObjectSelectionDeactivationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<ObjectSelectionActivationData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<ObjectSelectionActivationData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetToolSystem = this.World.GetOrCreateSystemManaged<NetToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_PendingTutorialQuery.IsEmptyIgnoreFilter && this.m_ActiveTutorialQuery.IsEmptyIgnoreFilter)
        return;
      bool tool;
      // ISSUE: reference to a compiler-generated method
      Entity selection = this.GetSelection(out tool);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PendingTutorialQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CheckDeactivate(this.m_PendingTutorialQuery, selection, tool);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveTutorialQuery.IsEmptyIgnoreFilter || !this.phaseCanDeactivate)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.CheckDeactivate(this.m_ActiveTutorialQuery, selection, tool);
    }

    private Entity GetSelection(out bool tool)
    {
      tool = true;
      PrefabRef component;
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.TryGetComponent<PrefabRef>(this.m_ToolSystem.selected, out component))
      {
        tool = false;
        return component.m_Prefab;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem && (Object) this.m_ObjectToolSystem.prefab != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.m_PrefabSystem.GetEntity((PrefabBase) this.m_ObjectToolSystem.prefab);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_NetToolSystem && (Object) this.m_NetToolSystem.prefab != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.m_PrefabSystem.GetEntity((PrefabBase) this.m_NetToolSystem.prefab);
      }
      tool = false;
      return Entity.Null;
    }

    private void CheckDeactivate(EntityQuery query, Entity selection, bool tool)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_ObjectSelectionActivationData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TutorialObjectSelectionDeactivationSystem.CheckTutorialsJob jobData = new TutorialObjectSelectionDeactivationSystem.CheckTutorialsJob()
      {
        m_DeactivationDataType = this.__TypeHandle.__Game_Tutorials_ObjectSelectionActivationData_RO_BufferTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_Selection = selection,
        m_Tool = tool,
        m_Buffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter()
      };
      this.Dependency = jobData.ScheduleParallel<TutorialObjectSelectionDeactivationSystem.CheckTutorialsJob>(query, this.Dependency);
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
    public TutorialObjectSelectionDeactivationSystem()
    {
    }

    [BurstCompile]
    private struct CheckTutorialsJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<ObjectSelectionActivationData> m_DeactivationDataType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public Entity m_Selection;
      public bool m_Tool;
      public EntityCommandBuffer.ParallelWriter m_Buffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ObjectSelectionActivationData> bufferAccessor = chunk.GetBufferAccessor<ObjectSelectionActivationData>(ref this.m_DeactivationDataType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_Selection == Entity.Null || this.ShouldDeactivate(bufferAccessor[index]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Buffer.RemoveComponent<TutorialActivated>(unfilteredChunkIndex, nativeArray[index]);
          }
        }
      }

      private bool ShouldDeactivate(
        DynamicBuffer<ObjectSelectionActivationData> selections)
      {
        for (int index = 0; index < selections.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (selections[index].m_Prefab == this.m_Selection && (selections[index].m_AllowTool || !this.m_Tool))
            return false;
        }
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
      public BufferTypeHandle<ObjectSelectionActivationData> __Game_Tutorials_ObjectSelectionActivationData_RO_BufferTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_ObjectSelectionActivationData_RO_BufferTypeHandle = state.GetBufferTypeHandle<ObjectSelectionActivationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
