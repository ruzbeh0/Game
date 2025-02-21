// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialObjectSelectionTriggerSystem
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

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialObjectSelectionTriggerSystem : TutorialTriggerSystemBase
  {
    private ToolSystem m_ToolSystem;
    private EntityArchetype m_UnlockEventArchetype;
    private Entity m_LastSelection;
    private TutorialObjectSelectionTriggerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<ObjectSelectionTriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.Exclude<TriggerCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      this.RequireForUpdate(this.m_ActiveTriggerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.triggersChanged)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastSelection = this.m_ToolSystem.selected;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_ToolSystem.selected != Entity.Null) || !(this.m_ToolSystem.selected != this.m_LastSelection))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelection = this.m_ToolSystem.selected;
      PrefabRef component;
      // ISSUE: reference to a compiler-generated field
      if (!this.EntityManager.TryGetComponent<PrefabRef>(this.m_ToolSystem.selected, out component))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_ObjectSelectionTriggerData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      TutorialObjectSelectionTriggerSystem.CheckSelectionJob jobData = new TutorialObjectSelectionTriggerSystem.CheckSelectionJob()
      {
        m_ForcedUnlockDataFromEntity = this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup,
        m_UnlockRequirementFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup,
        m_TriggerType = this.__TypeHandle.__Game_Tutorials_ObjectSelectionTriggerData_RO_BufferTypeHandle,
        m_UnlockEventArchetype = this.m_UnlockEventArchetype,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_Selection = component.m_Prefab,
        m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter()
      };
      this.Dependency = jobData.ScheduleParallel<TutorialObjectSelectionTriggerSystem.CheckSelectionJob>(this.m_ActiveTriggerQuery, this.Dependency);
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
    public TutorialObjectSelectionTriggerSystem()
    {
    }

    [BurstCompile]
    private struct CheckSelectionJob : IJobChunk
    {
      [ReadOnly]
      public BufferLookup<ForceUIGroupUnlockData> m_ForcedUnlockDataFromEntity;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> m_UnlockRequirementFromEntity;
      [ReadOnly]
      public BufferTypeHandle<ObjectSelectionTriggerData> m_TriggerType;
      [ReadOnly]
      public EntityArchetype m_UnlockEventArchetype;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public Entity m_Selection;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ObjectSelectionTriggerData> bufferAccessor = chunk.GetBufferAccessor<ObjectSelectionTriggerData>(ref this.m_TriggerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          DynamicBuffer<ObjectSelectionTriggerData> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            ObjectSelectionTriggerData selectionTriggerData = dynamicBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            if (selectionTriggerData.m_Prefab == this.m_Selection)
            {
              if (selectionTriggerData.m_GoToPhase != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<TutorialNextPhase>(unfilteredChunkIndex, nativeArray[index1], new TutorialNextPhase()
                {
                  m_NextPhase = selectionTriggerData.m_GoToPhase
                });
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<TriggerPreCompleted>(unfilteredChunkIndex, nativeArray[index1]);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<TriggerCompleted>(unfilteredChunkIndex, nativeArray[index1]);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                TutorialSystem.ManualUnlock(nativeArray[index1], this.m_UnlockEventArchetype, ref this.m_ForcedUnlockDataFromEntity, ref this.m_UnlockRequirementFromEntity, this.m_CommandBuffer, unfilteredChunkIndex);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TriggerPreCompleted>(unfilteredChunkIndex, nativeArray[index1]);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TutorialNextPhase>(unfilteredChunkIndex, nativeArray[index1]);
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
      public BufferLookup<ForceUIGroupUnlockData> __Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> __Game_Prefabs_UnlockRequirement_RO_BufferLookup;
      [ReadOnly]
      public BufferTypeHandle<ObjectSelectionTriggerData> __Game_Tutorials_ObjectSelectionTriggerData_RO_BufferTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup = state.GetBufferLookup<ForceUIGroupUnlockData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirement_RO_BufferLookup = state.GetBufferLookup<UnlockRequirement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_ObjectSelectionTriggerData_RO_BufferTypeHandle = state.GetBufferTypeHandle<ObjectSelectionTriggerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
