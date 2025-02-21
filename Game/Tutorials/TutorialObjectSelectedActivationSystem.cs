// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialObjectSelectedActivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
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
  public class TutorialObjectSelectedActivationSystem : GameSystemBase
  {
    protected EntityCommandBufferSystem m_BarrierSystem;
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private NetToolSystem m_NetToolSystem;
    private AreaToolSystem m_AreaToolSystem;
    private RouteToolSystem m_RouteToolSystem;
    private EntityQuery m_TutorialQuery;
    private TutorialObjectSelectedActivationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BarrierSystem = (EntityCommandBufferSystem) this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetToolSystem = this.World.GetOrCreateSystemManaged<NetToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaToolSystem = this.World.GetOrCreateSystemManaged<AreaToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteToolSystem = this.World.GetOrCreateSystemManaged<RouteToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<ObjectSelectionActivationData>(), ComponentType.Exclude<TutorialActivated>(), ComponentType.Exclude<TutorialCompleted>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_TutorialQuery.IsEmptyIgnoreFilter)
        return;
      bool tool;
      // ISSUE: reference to a compiler-generated method
      Entity selection = this.GetSelection(out tool);
      if (!(selection != Entity.Null))
        return;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TutorialObjectSelectedActivationSystem.ActivateJob jobData = new TutorialObjectSelectedActivationSystem.ActivateJob()
      {
        m_ActivationDataType = this.__TypeHandle.__Game_Tutorials_ObjectSelectionActivationData_RO_BufferTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_Selection = selection,
        m_Tool = tool,
        m_Writer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<TutorialObjectSelectedActivationSystem.ActivateJob>(this.m_TutorialQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_AreaToolSystem && (Object) this.m_AreaToolSystem.prefab != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.m_PrefabSystem.GetEntity((PrefabBase) this.m_AreaToolSystem.prefab);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_RouteToolSystem && (Object) this.m_RouteToolSystem.prefab != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.m_PrefabSystem.GetEntity((PrefabBase) this.m_RouteToolSystem.prefab);
      }
      tool = false;
      return Entity.Null;
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
    public TutorialObjectSelectedActivationSystem()
    {
    }

    [BurstCompile]
    private struct ActivateJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<ObjectSelectionActivationData> m_ActivationDataType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public Entity m_Selection;
      public bool m_Tool;
      public EntityCommandBuffer.ParallelWriter m_Writer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ObjectSelectionActivationData> bufferAccessor = chunk.GetBufferAccessor<ObjectSelectionActivationData>(ref this.m_ActivationDataType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.Check(bufferAccessor[index]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Writer.AddComponent<TutorialActivated>(unfilteredChunkIndex, nativeArray[index]);
          }
        }
      }

      private bool Check(DynamicBuffer<ObjectSelectionActivationData> datas)
      {
        for (int index = 0; index < datas.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (datas[index].m_Prefab == this.m_Selection && (!this.m_Tool || datas[index].m_AllowTool))
            return true;
        }
        return false;
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
