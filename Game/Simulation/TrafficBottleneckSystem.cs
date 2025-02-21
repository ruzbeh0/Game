// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrafficBottleneckSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TrafficBottleneckSystem : GameSystemBase
  {
    private IconCommandSystem m_IconCommandSystem;
    private TriggerSystem m_TriggerSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_BlockerQuery;
    private EntityQuery m_BottleneckQuery;
    private EntityQuery m_ConfigurationQuery;
    private TrafficBottleneckSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_BlockerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Blocker>(), ComponentType.ReadOnly<Vehicle>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_BottleneckQuery = this.GetEntityQuery(ComponentType.ReadOnly<Bottleneck>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<TrafficConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_BlockerQuery, this.m_BottleneckQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync1 = this.m_BlockerQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync2 = this.m_BottleneckQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Bottleneck_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new TrafficBottleneckSystem.TrafficBottleneckJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentTypeHandle,
        m_BottleneckType = this.__TypeHandle.__Game_Net_Bottleneck_RW_ComponentTypeHandle,
        m_CarCurrentLaneData = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup,
        m_TrainCurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_BlockerChunks = archetypeChunkListAsync1,
        m_BottleneckChunks = archetypeChunkListAsync2,
        m_TrafficConfigurationData = this.m_ConfigurationQuery.GetSingleton<TrafficConfigurationData>(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_EntityCommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_TriggerActionQueue = this.m_TriggerSystem.CreateActionBuffer()
      }.Schedule<TrafficBottleneckSystem.TrafficBottleneckJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2));
      archetypeChunkListAsync1.Dispose(jobHandle);
      archetypeChunkListAsync2.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public TrafficBottleneckSystem()
    {
    }

    private struct GroupData
    {
      public int m_Count;
      public int m_Merged;
    }

    private struct BottleneckData
    {
      public TrafficBottleneckSystem.BottleneckState m_State;
      public int2 m_Range;
    }

    private enum BottleneckState
    {
      Remove,
      Keep,
      Add,
    }

    [BurstCompile]
    private struct TrafficBottleneckJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Blocker> m_BlockerType;
      public ComponentTypeHandle<Bottleneck> m_BottleneckType;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> m_CarCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> m_TrainCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_BlockerChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_BottleneckChunks;
      [ReadOnly]
      public TrafficConfigurationData m_TrafficConfigurationData;
      public IconCommandBuffer m_IconCommandBuffer;
      public EntityCommandBuffer m_EntityCommandBuffer;
      public NativeQueue<TriggerAction> m_TriggerActionQueue;

      public void Execute()
      {
        NativeParallelHashMap<Entity, int> groupMap = new NativeParallelHashMap<Entity, int>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashMap<Entity, TrafficBottleneckSystem.BottleneckData> bottleneckMap = new NativeParallelHashMap<Entity, TrafficBottleneckSystem.BottleneckData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<TrafficBottleneckSystem.GroupData> groups = new NativeList<TrafficBottleneckSystem.GroupData>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_BottleneckChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FillBottlenecks(this.m_BottleneckChunks[index], bottleneckMap);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_BlockerChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FormGroups(this.m_BlockerChunks[index], groupMap, groups);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_BlockerChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddBottlenecks(this.m_BlockerChunks[index], groupMap, groups, bottleneckMap);
        }
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_BottleneckChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num += this.CheckBottlenecks(this.m_BottleneckChunks[index], bottleneckMap);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_TriggerActionQueue.Enqueue(new TriggerAction(TriggerType.TrafficBottleneck, Entity.Null, (float) num));
        groupMap.Dispose();
        bottleneckMap.Dispose();
        groups.Dispose();
      }

      private void FillBottlenecks(
        ArchetypeChunk chunk,
        NativeParallelHashMap<Entity, TrafficBottleneckSystem.BottleneckData> bottleneckMap)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Entity key = nativeArray[index];
          // ISSUE: object of a compiler-generated type is created
          bottleneckMap.Add(key, new TrafficBottleneckSystem.BottleneckData()
          {
            m_State = TrafficBottleneckSystem.BottleneckState.Remove
          });
        }
      }

      private void FormGroups(
        ArchetypeChunk chunk,
        NativeParallelHashMap<Entity, int> groupMap,
        NativeList<TrafficBottleneckSystem.GroupData> groups)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Blocker> nativeArray2 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity key = nativeArray1[index];
          Blocker blocker = nativeArray2[index];
          if (blocker.m_Type == BlockerType.Continuing && blocker.m_Blocker != Entity.Null)
          {
            Entity entity = blocker.m_Blocker;
            Controller componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControllerData.TryGetComponent(entity, out componentData))
              entity = componentData.m_Controller;
            if (entity == key)
            {
              UnityEngine.Debug.Log((object) string.Format("TrafficBottleneckSystem: Self blocking entity {0}", (object) key.Index));
            }
            else
            {
              int merged1;
              int num = groupMap.TryGetValue(key, out merged1) ? 1 : 0;
              int merged2;
              bool flag = groupMap.TryGetValue(entity, out merged2);
              if (num != 0)
              {
                // ISSUE: variable of a compiler-generated type
                TrafficBottleneckSystem.GroupData group1 = groups[merged1];
                // ISSUE: reference to a compiler-generated field
                if (group1.m_Merged != -1)
                {
                  // ISSUE: reference to a compiler-generated field
                  do
                  {
                    // ISSUE: reference to a compiler-generated field
                    merged1 = group1.m_Merged;
                    group1 = groups[merged1];
                  }
                  while (group1.m_Merged != -1);
                  groupMap[key] = merged1;
                }
                if (flag)
                {
                  // ISSUE: variable of a compiler-generated type
                  TrafficBottleneckSystem.GroupData group2;
                  // ISSUE: reference to a compiler-generated field
                  for (group2 = groups[merged2]; group2.m_Merged != -1; group2 = groups[merged2])
                  {
                    // ISSUE: reference to a compiler-generated field
                    merged2 = group2.m_Merged;
                  }
                  if (merged1 != merged2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    group1.m_Count += group2.m_Count;
                    // ISSUE: reference to a compiler-generated field
                    group2.m_Count = 0;
                    // ISSUE: reference to a compiler-generated field
                    group2.m_Merged = merged1;
                    groups[merged1] = group1;
                    groups[merged2] = group2;
                  }
                  groupMap[entity] = merged1;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  ++group1.m_Count;
                  groups[merged1] = group1;
                  groupMap.Add(entity, merged1);
                }
              }
              else if (flag)
              {
                // ISSUE: variable of a compiler-generated type
                TrafficBottleneckSystem.GroupData group = groups[merged2];
                // ISSUE: reference to a compiler-generated field
                if (group.m_Merged != -1)
                {
                  // ISSUE: reference to a compiler-generated field
                  do
                  {
                    // ISSUE: reference to a compiler-generated field
                    merged2 = group.m_Merged;
                    group = groups[merged2];
                  }
                  while (group.m_Merged != -1);
                  groupMap[entity] = merged2;
                }
                // ISSUE: reference to a compiler-generated field
                ++group.m_Count;
                groups[merged2] = group;
                groupMap.Add(key, merged2);
              }
              else
              {
                groupMap.Add(key, groups.Length);
                groupMap.Add(entity, groups.Length);
                // ISSUE: object of a compiler-generated type is created
                groups.Add(new TrafficBottleneckSystem.GroupData()
                {
                  m_Count = 2,
                  m_Merged = -1
                });
              }
            }
          }
        }
      }

      private void AddBottlenecks(
        ArchetypeChunk chunk,
        NativeParallelHashMap<Entity, int> groupMap,
        NativeList<TrafficBottleneckSystem.GroupData> groups,
        NativeParallelHashMap<Entity, TrafficBottleneckSystem.BottleneckData> bottleneckMap)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Blocker> nativeArray2 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Blocker blocker = nativeArray2[index];
          int merged;
          if (groupMap.TryGetValue(entity, out merged))
          {
            // ISSUE: variable of a compiler-generated type
            TrafficBottleneckSystem.GroupData group;
            // ISSUE: reference to a compiler-generated field
            for (group = groups[merged]; group.m_Merged != -1; group = groups[merged])
            {
              // ISSUE: reference to a compiler-generated field
              merged = group.m_Merged;
            }
            // ISSUE: reference to a compiler-generated field
            if (group.m_Count >= 10)
            {
              Entity lane = Entity.Null;
              float2 curvePosition = (float2) 0.0f;
              CarCurrentLane componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarCurrentLaneData.TryGetComponent(entity, out componentData1))
              {
                lane = !(componentData1.m_ChangeLane != Entity.Null) ? componentData1.m_Lane : componentData1.m_ChangeLane;
                curvePosition = componentData1.m_CurvePosition.xy;
              }
              else
              {
                TrainCurrentLane componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TrainCurrentLaneData.TryGetComponent(entity, out componentData2))
                {
                  lane = componentData2.m_Front.m_Lane;
                  curvePosition = componentData2.m_Front.m_CurvePosition.yz;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (blocker.m_Type == BlockerType.Continuing && blocker.m_Blocker != Entity.Null || group.m_Count < 50)
              {
                // ISSUE: reference to a compiler-generated method
                this.KeepBottleneck(bottleneckMap, lane, curvePosition);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.AddBottleneck(bottleneckMap, lane, curvePosition);
              }
            }
          }
        }
      }

      private void KeepBottleneck(
        NativeParallelHashMap<Entity, TrafficBottleneckSystem.BottleneckData> bottleneckMap,
        Entity lane,
        float2 curvePosition)
      {
        // ISSUE: variable of a compiler-generated type
        TrafficBottleneckSystem.BottleneckData bottleneckData;
        // ISSUE: reference to a compiler-generated field
        if (!bottleneckMap.TryGetValue(lane, out bottleneckData) || bottleneckData.m_State != TrafficBottleneckSystem.BottleneckState.Remove)
          return;
        // ISSUE: object of a compiler-generated type is created
        bottleneckMap[lane] = new TrafficBottleneckSystem.BottleneckData()
        {
          m_State = TrafficBottleneckSystem.BottleneckState.Keep
        };
      }

      private void AddBottleneck(
        NativeParallelHashMap<Entity, TrafficBottleneckSystem.BottleneckData> bottleneckMap,
        Entity lane,
        float2 curvePosition)
      {
        // ISSUE: variable of a compiler-generated type
        TrafficBottleneckSystem.BottleneckData bottleneckData;
        if (bottleneckMap.TryGetValue(lane, out bottleneckData))
        {
          curvePosition.y += curvePosition.y - curvePosition.x;
          int2 int2 = math.clamp(new int2(Mathf.RoundToInt(math.cmin(curvePosition) * (float) byte.MaxValue), Mathf.RoundToInt(math.cmax(curvePosition) * (float) byte.MaxValue)), (int2) 0, (int2) (int) byte.MaxValue);
          // ISSUE: reference to a compiler-generated field
          if (bottleneckData.m_State == TrafficBottleneckSystem.BottleneckState.Add)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bottleneckData.m_Range.x = math.min(bottleneckData.m_Range.x, int2.x);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bottleneckData.m_Range.y = math.max(bottleneckData.m_Range.y, int2.y);
            bottleneckMap[lane] = bottleneckData;
          }
          else
          {
            // ISSUE: object of a compiler-generated type is created
            bottleneckMap[lane] = new TrafficBottleneckSystem.BottleneckData()
            {
              m_State = TrafficBottleneckSystem.BottleneckState.Add,
              m_Range = int2
            };
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CurveData.HasComponent(lane))
            return;
          curvePosition.y += curvePosition.y - curvePosition.x;
          int2 int2 = math.clamp(new int2(Mathf.RoundToInt(math.cmin(curvePosition) * (float) byte.MaxValue), Mathf.RoundToInt(math.cmax(curvePosition) * (float) byte.MaxValue)), (int2) 0, (int2) (int) byte.MaxValue);
          // ISSUE: reference to a compiler-generated field
          this.m_EntityCommandBuffer.AddComponent<Bottleneck>(lane, new Bottleneck((byte) int2.x, (byte) int2.y, (byte) 5));
          // ISSUE: object of a compiler-generated type is created
          bottleneckMap.Add(lane, new TrafficBottleneckSystem.BottleneckData()
          {
            m_State = TrafficBottleneckSystem.BottleneckState.Add,
            m_Range = int2
          });
        }
      }

      private int CheckBottlenecks(
        ArchetypeChunk chunk,
        NativeParallelHashMap<Entity, TrafficBottleneckSystem.BottleneckData> bottleneckMap)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Bottleneck> nativeArray2 = chunk.GetNativeArray<Bottleneck>(ref this.m_BottleneckType);
        int num = 0;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Bottleneck bottleneck1 = nativeArray2[index];
          if (bottleneck1.m_Timer >= (byte) 20)
            ++num;
          // ISSUE: variable of a compiler-generated type
          TrafficBottleneckSystem.BottleneckData bottleneck2 = bottleneckMap[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          TrafficBottleneckSystem.BottleneckState state = bottleneck2.m_State;
          switch (state)
          {
            case TrafficBottleneckSystem.BottleneckState.Remove:
              if (bottleneck1.m_Timer >= (byte) 23)
              {
                bottleneck1.m_Timer -= (byte) 3;
                break;
              }
              if (bottleneck1.m_Timer >= (byte) 20)
              {
                bottleneck1.m_Timer = (byte) 0;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(entity, this.m_TrafficConfigurationData.m_BottleneckNotification);
                // ISSUE: reference to a compiler-generated field
                this.m_EntityCommandBuffer.RemoveComponent<Bottleneck>(entity);
                break;
              }
              if (bottleneck1.m_Timer > (byte) 3)
              {
                bottleneck1.m_Timer -= (byte) 3;
                break;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_EntityCommandBuffer.RemoveComponent<Bottleneck>(entity);
              break;
            case TrafficBottleneckSystem.BottleneckState.Keep:
              if (bottleneck1.m_Timer >= (byte) 21)
              {
                --bottleneck1.m_Timer;
                break;
              }
              if (bottleneck1.m_Timer >= (byte) 20)
              {
                bottleneck1.m_Timer = (byte) 0;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(entity, this.m_TrafficConfigurationData.m_BottleneckNotification);
                // ISSUE: reference to a compiler-generated field
                this.m_EntityCommandBuffer.RemoveComponent<Bottleneck>(entity);
                break;
              }
              if (bottleneck1.m_Timer > (byte) 1)
              {
                --bottleneck1.m_Timer;
                break;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_EntityCommandBuffer.RemoveComponent<Bottleneck>(entity);
              break;
            case TrafficBottleneckSystem.BottleneckState.Add:
              int position = (int) bottleneck1.m_Position;
              // ISSUE: reference to a compiler-generated field
              bottleneck1.m_MinPos = (byte) math.min((int) bottleneck1.m_MinPos + 2, bottleneck2.m_Range.x);
              // ISSUE: reference to a compiler-generated field
              bottleneck1.m_MaxPos = (byte) math.max((int) bottleneck1.m_MaxPos - 2, bottleneck2.m_Range.y);
              if ((int) bottleneck1.m_Position < (int) bottleneck1.m_MinPos || (int) bottleneck1.m_Position > (int) bottleneck1.m_MaxPos)
                bottleneck1.m_Position = (byte) ((int) bottleneck1.m_MinPos + (int) bottleneck1.m_MaxPos + 1 >> 1);
              if (bottleneck1.m_Timer >= (byte) 20)
              {
                bottleneck1.m_Timer = (byte) math.min(40, (int) bottleneck1.m_Timer + 5);
                if (position != (int) bottleneck1.m_Position)
                {
                  // ISSUE: reference to a compiler-generated field
                  float3 location = MathUtils.Position(this.m_CurveData[entity].m_Bezier, (float) bottleneck1.m_Position * 0.003921569f);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(entity, this.m_TrafficConfigurationData.m_BottleneckNotification, location, IconPriority.Problem);
                  break;
                }
                break;
              }
              if (bottleneck1.m_Timer >= (byte) 15)
              {
                bottleneck1.m_Timer = (byte) 40;
                // ISSUE: reference to a compiler-generated field
                float3 location = MathUtils.Position(this.m_CurveData[entity].m_Bezier, (float) bottleneck1.m_Position * 0.003921569f);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(entity, this.m_TrafficConfigurationData.m_BottleneckNotification, location, IconPriority.Problem);
                break;
              }
              bottleneck1.m_Timer += (byte) 5;
              break;
          }
          nativeArray2[index] = bottleneck1;
        }
        return num;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Bottleneck> __Game_Net_Bottleneck_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Bottleneck_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Bottleneck>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup = state.GetComponentLookup<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
      }
    }
  }
}
