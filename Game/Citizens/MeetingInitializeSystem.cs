// Decompiled with JetBrains decompiler
// Type: Game.Citizens.MeetingInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  [CompilerGenerated]
  public class MeetingInitializeSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier5;
    private EntityQuery m_MeetingQuery;
    private MeetingInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier5 = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_MeetingQuery = this.GetEntityQuery(ComponentType.ReadOnly<CoordinatedMeeting>(), ComponentType.ReadWrite<CoordinatedMeetingAttendee>(), ComponentType.ReadOnly<Created>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_MeetingQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CoordinatedMeetingAttendee_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      MeetingInitializeSystem.InitializeMeetingJob jobData = new MeetingInitializeSystem.InitializeMeetingJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AttendeeType = this.__TypeHandle.__Game_Citizens_CoordinatedMeetingAttendee_RW_BufferTypeHandle,
        m_Attendings = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier5.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<MeetingInitializeSystem.InitializeMeetingJob>(this.m_MeetingQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier5.AddJobHandleForProducer(this.Dependency);
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
    public MeetingInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeMeetingJob : IJobChunk
    {
      public EntityTypeHandle m_EntityType;
      public BufferTypeHandle<CoordinatedMeetingAttendee> m_AttendeeType;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> m_Attendings;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CoordinatedMeetingAttendee> bufferAccessor = chunk.GetBufferAccessor<CoordinatedMeetingAttendee>(ref this.m_AttendeeType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray[index1];
          DynamicBuffer<CoordinatedMeetingAttendee> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity attendee = dynamicBuffer[index2].m_Attendee;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Attendings.HasComponent(attendee))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<AttendingMeeting>(attendee, new AttendingMeeting()
              {
                m_Meeting = entity
              });
            }
            else
            {
              dynamicBuffer.RemoveAt(index2);
              --index2;
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public BufferTypeHandle<CoordinatedMeetingAttendee> __Game_Citizens_CoordinatedMeetingAttendee_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> __Game_Citizens_AttendingMeeting_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CoordinatedMeetingAttendee_RW_BufferTypeHandle = state.GetBufferTypeHandle<CoordinatedMeetingAttendee>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_AttendingMeeting_RO_ComponentLookup = state.GetComponentLookup<AttendingMeeting>(true);
      }
    }
  }
}
