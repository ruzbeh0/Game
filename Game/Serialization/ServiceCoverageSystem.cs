// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ServiceCoverageSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ServiceCoverageSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private ServiceCoverageSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<ServiceCoverage>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ServiceCoverageSystem.ServiceCoverageJob jobData = new ServiceCoverageSystem.ServiceCoverageJob()
      {
        m_CoverageType = this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ServiceCoverageSystem.ServiceCoverageJob>(this.m_Query, this.Dependency);
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
    public ServiceCoverageSystem()
    {
    }

    [BurstCompile]
    private struct ServiceCoverageJob : IJobChunk
    {
      public BufferTypeHandle<ServiceCoverage> m_CoverageType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceCoverage> bufferAccessor = chunk.GetBufferAccessor<ServiceCoverage>(ref this.m_CoverageType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          DynamicBuffer<ServiceCoverage> dynamicBuffer = bufferAccessor[index1];
          int num = 9 - dynamicBuffer.Length;
          if (num > 0)
          {
            dynamicBuffer.Capacity = 9;
            for (int index2 = 0; index2 < num; ++index2)
              dynamicBuffer.Add(new ServiceCoverage());
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
      public BufferTypeHandle<ServiceCoverage> __Game_Net_ServiceCoverage_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceCoverage>();
      }
    }
  }
}
