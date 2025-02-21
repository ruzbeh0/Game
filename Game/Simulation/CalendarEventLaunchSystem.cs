// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CalendarEventLaunchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CalendarEventLaunchSystem : GameSystemBase
  {
    private const int UPDATES_PER_DAY = 4;
    private EndFrameBarrier m_EndFrameBarrier;
    private TimeSystem m_TimeSystem;
    private EntityQuery m_CalendarEventQuery;
    private CalendarEventLaunchSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 65536;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CalendarEventQuery = this.GetEntityQuery(ComponentType.ReadOnly<CalendarEventData>());
      this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CalendarEventQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      CalendarEventMonths calendarEventMonths = (CalendarEventMonths) (1 << Mathf.FloorToInt(this.m_TimeSystem.normalizedDate * 12f));
      // ISSUE: reference to a compiler-generated field
      CalendarEventTimes calendarEventTimes = (CalendarEventTimes) (1 << Mathf.FloorToInt(this.m_TimeSystem.normalizedTime * 4f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      CalendarEventLaunchSystem.CheckEventLaunchJob jobData = new CalendarEventLaunchSystem.CheckEventLaunchJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EventType = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle,
        m_CalendarEventType = this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentTypeHandle,
        m_Month = calendarEventMonths,
        m_Time = calendarEventTimes,
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CalendarEventLaunchSystem.CheckEventLaunchJob>(this.m_CalendarEventQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public CalendarEventLaunchSystem()
    {
    }

    [BurstCompile]
    private struct CheckEventLaunchJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CalendarEventData> m_CalendarEventType;
      [ReadOnly]
      public ComponentTypeHandle<EventData> m_EventType;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public CalendarEventMonths m_Month;
      public CalendarEventTimes m_Time;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CalendarEventData> nativeArray2 = chunk.GetNativeArray<CalendarEventData>(ref this.m_CalendarEventType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EventData> nativeArray3 = chunk.GetNativeArray<EventData>(ref this.m_EventType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity prefab = nativeArray1[index];
          CalendarEventData calendarEventData = nativeArray2[index];
          EventData eventData = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Month & calendarEventData.m_AllowedMonths) != (CalendarEventMonths) 0 && (this.m_Time & calendarEventData.m_AllowedTimes) != (CalendarEventTimes) 0 && (double) random.NextInt(100) < (double) calendarEventData.m_OccurenceProbability.min)
          {
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, eventData.m_Archetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(unfilteredChunkIndex, entity, new PrefabRef(prefab));
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
      public ComponentTypeHandle<EventData> __Game_Prefabs_EventData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CalendarEventData> __Game_Prefabs_CalendarEventData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CalendarEventData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CalendarEventData>(true);
      }
    }
  }
}
