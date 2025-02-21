// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterLevelChangeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Events;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaterLevelChangeSystem : GameSystemBase
  {
    public static readonly int kUpdateInterval = 4;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_WaterLevelChangeQuery;
    private WaterLevelChangeSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return WaterLevelChangeSystem.kUpdateInterval;
    }

    public static int TsunamiEndDelay
    {
      get => Mathf.RoundToInt((float) WaterSystem.kMapSize / WaterSystem.WaveSpeed);
    }

    public static uint GetMinimumDelayAt(WaterLevelChange change, float3 position)
    {
      // ISSUE: reference to a compiler-generated field
      float2 float2_1 = (float) (WaterSystem.kMapSize / 2) * new float2(math.cos(-change.m_Direction.x), math.sin(-change.m_Direction.y));
      float2 x = new float2(change.m_Direction.y, -change.m_Direction.x);
      float2 float2_2 = math.dot(x, position.xz - float2_1) * x;
      return (uint) Mathf.RoundToInt(math.length(position.xz - float2_1 - float2_2) / WaterSystem.WaveSpeed);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterLevelChangeQuery = this.GetEntityQuery(ComponentType.ReadWrite<WaterLevelChange>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WaterLevelChangeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WaterLevelChange_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaterLevelChangeSystem.WaterLevelChangeJob jobData = new WaterLevelChangeSystem.WaterLevelChangeJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WaterLevelChangeType = this.__TypeHandle.__Game_Events_WaterLevelChange_RW_ComponentTypeHandle,
        m_DurationType = this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle,
        m_PrefabWaterLevelChangeData = this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<WaterLevelChangeSystem.WaterLevelChangeJob>(this.m_WaterLevelChangeQuery, this.Dependency);
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
    public WaterLevelChangeSystem()
    {
    }

    [BurstCompile]
    private struct WaterLevelChangeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<WaterLevelChange> m_WaterLevelChangeType;
      [ReadOnly]
      public ComponentTypeHandle<Duration> m_DurationType;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> m_PrefabWaterLevelChangeData;
      [ReadOnly]
      public uint m_SimulationFrame;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterLevelChange> nativeArray3 = chunk.GetNativeArray<WaterLevelChange>(ref this.m_WaterLevelChangeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Duration> nativeArray4 = chunk.GetNativeArray<Duration>(ref this.m_DurationType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          WaterLevelChange waterLevelChange = nativeArray3[index];
          Duration duration = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          WaterLevelChangeData waterLevelChangeData = this.m_PrefabWaterLevelChangeData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          float num1 = (float) (this.m_SimulationFrame - duration.m_StartFrame) / 60f - waterLevelChangeData.m_EscalationDelay;
          if ((double) num1 >= 0.0)
          {
            if (waterLevelChangeData.m_ChangeType == WaterLevelChangeType.Sine)
            {
              float num2 = (float) ((long) duration.m_EndFrame - (long) WaterLevelChangeSystem.TsunamiEndDelay - (long) duration.m_StartFrame) / 60f;
              waterLevelChange.m_Intensity = (double) num1 >= 0.05000000074505806 * (double) num2 ? ((double) num1 >= (double) num2 ? 0.0f : waterLevelChange.m_MaxIntensity * (float) (0.5 * (double) math.sin((float) (5.0 * ((double) num1 - 0.05000000074505806 * (double) num2) / (0.949999988079071 * (double) num2) * 2.0 * 3.1415927410125732)) + 0.5 * (double) math.saturate((float) (((double) num1 - 0.05000000074505806 * (double) num2) / (0.20000000298023224 * (double) num2))))) : -0.2f * waterLevelChange.m_MaxIntensity * math.sin((float) (20.0 * (double) num1 / (double) num2 * 3.1415927410125732));
              waterLevelChange.m_Intensity *= 4f;
            }
            else
            {
              int changeType = (int) waterLevelChangeData.m_ChangeType;
            }
            nativeArray3[index] = waterLevelChange;
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<WaterLevelChange> __Game_Events_WaterLevelChange_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Duration> __Game_Events_Duration_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> __Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WaterLevelChange_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterLevelChange>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup = state.GetComponentLookup<WaterLevelChangeData>(true);
      }
    }
  }
}
