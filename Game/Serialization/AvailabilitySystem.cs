// Decompiled with JetBrains decompiler
// Type: Game.Serialization.AvailabilitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class AvailabilitySystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private AvailabilitySystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<ResourceAvailability>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvailabilitySystem.AvailabilityJob jobData = new AvailabilitySystem.AvailabilityJob()
      {
        m_AvailabilityType = this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<AvailabilitySystem.AvailabilityJob>(this.m_Query, this.Dependency);
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
    public AvailabilitySystem()
    {
    }

    [BurstCompile]
    private struct AvailabilityJob : IJobChunk
    {
      public BufferTypeHandle<ResourceAvailability> m_AvailabilityType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ResourceAvailability> bufferAccessor = chunk.GetBufferAccessor<ResourceAvailability>(ref this.m_AvailabilityType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          DynamicBuffer<ResourceAvailability> dynamicBuffer = bufferAccessor[index];
          while (dynamicBuffer.Length < 33)
            dynamicBuffer.Add(new ResourceAvailability()
            {
              m_Availability = (float2) 0.0f
            });
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
      public BufferTypeHandle<ResourceAvailability> __Game_Net_ResourceAvailability_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RW_BufferTypeHandle = state.GetBufferTypeHandle<ResourceAvailability>();
      }
    }
  }
}
