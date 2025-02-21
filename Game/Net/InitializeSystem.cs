// Decompiled with JetBrains decompiler
// Type: Game.Net.InitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class InitializeSystem : GameSystemBase
  {
    private EntityQuery m_CreatedEdgesQuery;
    private InitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedEdgesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Created>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadWrite<ServiceCoverage>(),
          ComponentType.ReadWrite<ResourceAvailability>(),
          ComponentType.ReadWrite<WaterPipeConnection>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedEdgesQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_WaterPipeConnection_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
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
      InitializeSystem.InitializeEdgesJob jobData = new InitializeSystem.InitializeEdgesJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WaterPipeConnectionData = this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup,
        m_ServiceCoverageType = this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle,
        m_ResourceAvailabilityType = this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle,
        m_WaterPipeConnectionType = this.__TypeHandle.__Game_Net_WaterPipeConnection_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<InitializeSystem.InitializeEdgesJob>(this.m_CreatedEdgesQuery, this.Dependency);
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
    public InitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeEdgesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<WaterPipeConnectionData> m_WaterPipeConnectionData;
      public BufferTypeHandle<ServiceCoverage> m_ServiceCoverageType;
      public BufferTypeHandle<ResourceAvailability> m_ResourceAvailabilityType;
      public ComponentTypeHandle<WaterPipeConnection> m_WaterPipeConnectionType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceCoverage> bufferAccessor1 = chunk.GetBufferAccessor<ServiceCoverage>(ref this.m_ServiceCoverageType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ResourceAvailability> bufferAccessor2 = chunk.GetBufferAccessor<ResourceAvailability>(ref this.m_ResourceAvailabilityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeConnection> nativeArray2 = chunk.GetNativeArray<WaterPipeConnection>(ref this.m_WaterPipeConnectionType);
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          DynamicBuffer<ServiceCoverage> dynamicBuffer = bufferAccessor1[index1];
          dynamicBuffer.ResizeUninitialized(9);
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            dynamicBuffer[index2] = new ServiceCoverage();
        }
        for (int index3 = 0; index3 < bufferAccessor2.Length; ++index3)
        {
          DynamicBuffer<ResourceAvailability> dynamicBuffer = bufferAccessor2[index3];
          dynamicBuffer.ResizeUninitialized(33);
          for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
            dynamicBuffer[index4] = new ResourceAvailability();
        }
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          WaterPipeConnection waterPipeConnection = nativeArray2[index] with
          {
            m_FreshCapacity = this.m_WaterPipeConnectionData[prefabRef.m_Prefab].m_FreshCapacity,
            m_SewageCapacity = this.m_WaterPipeConnectionData[prefabRef.m_Prefab].m_SewageCapacity,
            m_StormCapacity = this.m_WaterPipeConnectionData[prefabRef.m_Prefab].m_StormCapacity
          };
          nativeArray2[index] = waterPipeConnection;
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeConnectionData> __Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup;
      public BufferTypeHandle<ServiceCoverage> __Game_Net_ServiceCoverage_RW_BufferTypeHandle;
      public BufferTypeHandle<ResourceAvailability> __Game_Net_ResourceAvailability_RW_BufferTypeHandle;
      public ComponentTypeHandle<WaterPipeConnection> __Game_Net_WaterPipeConnection_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup = state.GetComponentLookup<WaterPipeConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceCoverage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RW_BufferTypeHandle = state.GetBufferTypeHandle<ResourceAvailability>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_WaterPipeConnection_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeConnection>();
      }
    }
  }
}
