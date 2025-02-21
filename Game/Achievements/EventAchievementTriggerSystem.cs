// Decompiled with JetBrains decompiler
// Type: Game.Achievements.EventAchievementTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Game.Common;
using Game.Events;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Achievements
{
  [CompilerGenerated]
  public class EventAchievementTriggerSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private ModificationEndBarrier m_ModifiactionEndBarrier;
    private EntityQuery m_TrackingQuery;
    private EntityQuery m_CreatedEventQuery;
    private EntityArchetype m_TrackingArchetype;
    private EventAchievementTriggerSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiactionEndBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedEventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Events.Event>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<EventAchievement>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TrackingArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<EventAchievementTrackingData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TrackingQuery = this.GetEntityQuery(ComponentType.ReadWrite<EventAchievementTrackingData>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedEventQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_CreatedEventQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_EventAchievementData_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<EventAchievementData> dataRoBufferLookup = this.__TypeHandle.__Game_Prefabs_EventAchievementData_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Duration> componentTypeHandle1 = this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_ModifiactionEndBarrier.CreateCommandBuffer();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          if (archetypeChunk.Has<Duration>(ref componentTypeHandle1))
          {
            archetypeChunk = archetypeChunkArray[index1];
            NativeArray<Duration> nativeArray1 = archetypeChunk.GetNativeArray<Duration>(ref componentTypeHandle1);
            archetypeChunk = archetypeChunkArray[index1];
            NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              DynamicBuffer<EventAchievementData> dynamicBuffer = dataRoBufferLookup[nativeArray2[index2].m_Prefab];
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated method
                this.StartTracking(dynamicBuffer[index3].m_ID, nativeArray1[index2].m_StartFrame + dynamicBuffer[index3].m_FrameDelay, commandBuffer);
              }
            }
          }
          else
          {
            archetypeChunk = archetypeChunkArray[index1];
            NativeArray<PrefabRef> nativeArray = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
            for (int index4 = 0; index4 < nativeArray.Length; ++index4)
            {
              DynamicBuffer<EventAchievementData> dynamicBuffer = dataRoBufferLookup[nativeArray[index4].m_Prefab];
              for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.StartTracking(dynamicBuffer[index5].m_ID, this.m_SimulationSystem.frameIndex + dynamicBuffer[index5].m_FrameDelay, commandBuffer);
              }
            }
          }
        }
        archetypeChunkArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_TrackingQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<EventAchievementTrackingData> componentDataArray = this.m_TrackingQuery.ToComponentDataArray<EventAchievementTrackingData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_TrackingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer1 = this.m_ModifiactionEndBarrier.CreateCommandBuffer();
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SimulationSystem.frameIndex > componentDataArray[index].m_StartFrame)
        {
          // ISSUE: reference to a compiler-generated method
          this.StopTracking(componentDataArray[index], entityArray[index], commandBuffer1);
        }
      }
      componentDataArray.Dispose();
      entityArray.Dispose();
    }

    private void StartTracking(AchievementId id, uint startFrame, EntityCommandBuffer buffer)
    {
      // ISSUE: reference to a compiler-generated field
      Entity entity = buffer.CreateEntity(this.m_TrackingArchetype);
      buffer.SetComponent<EventAchievementTrackingData>(entity, new EventAchievementTrackingData()
      {
        m_ID = id,
        m_StartFrame = startFrame
      });
    }

    private void StopTracking(
      EventAchievementTrackingData data,
      Entity entity,
      EntityCommandBuffer buffer)
    {
      buffer.AddComponent<Deleted>(entity);
      PlatformManager.instance.UnlockAchievement(data.m_ID);
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

    [Preserve]
    public EventAchievementTriggerSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<EventAchievementData> __Game_Prefabs_EventAchievementData_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Duration> __Game_Events_Duration_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventAchievementData_RO_BufferLookup = state.GetBufferLookup<EventAchievementData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
      }
    }
  }
}
