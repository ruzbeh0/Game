// Decompiled with JetBrains decompiler
// Type: Game.Areas.ServiceDistrictSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Areas
{
  [CompilerGenerated]
  public class ServiceDistrictSystem : GameSystemBase
  {
    private EntityQuery m_DeletedDistrictQuery;
    private EntityQuery m_ServiceDistrictQuery;
    private ServiceDistrictSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedDistrictQuery = this.GetEntityQuery(ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<District>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceDistrictQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceDistrict>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DeletedDistrictQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ServiceDistrictQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync = this.m_DeletedDistrictQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_ServiceDistrict_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = new ServiceDistrictSystem.RemoveServiceDistrictsJob()
      {
        m_DeletedDistricts = entityListAsync,
        m_ServiceDistrictType = this.__TypeHandle.__Game_Areas_ServiceDistrict_RW_BufferTypeHandle
      }.ScheduleParallel<ServiceDistrictSystem.RemoveServiceDistrictsJob>(this.m_ServiceDistrictQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      entityListAsync.Dispose(inputDeps);
      this.Dependency = inputDeps;
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
    public ServiceDistrictSystem()
    {
    }

    [BurstCompile]
    private struct RemoveServiceDistrictsJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<Entity> m_DeletedDistricts;
      public BufferTypeHandle<ServiceDistrict> m_ServiceDistrictType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDistrict> bufferAccessor = chunk.GetBufferAccessor<ServiceDistrict>(ref this.m_ServiceDistrictType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<ServiceDistrict> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity district = dynamicBuffer[index2].m_District;
            // ISSUE: reference to a compiler-generated field
            for (int index3 = 0; index3 < this.m_DeletedDistricts.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_DeletedDistricts[index3] == district)
              {
                dynamicBuffer.RemoveAt(index2--);
                break;
              }
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
      public BufferTypeHandle<ServiceDistrict> __Game_Areas_ServiceDistrict_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_ServiceDistrict_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDistrict>();
      }
    }
  }
}
