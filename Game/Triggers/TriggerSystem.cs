// Decompiled with JetBrains decompiler
// Type: Game.Triggers.TriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Triggers
{
  [CompilerGenerated]
  public class TriggerSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private SimulationSystem m_SimulationSystem;
    private TriggerPrefabSystem m_TriggerPrefabSystem;
    private ModificationEndBarrier m_ModificationBarrier;
    private List<NativeQueue<TriggerAction>> m_Queues;
    private JobHandle m_Dependencies;
    private CreateChirpSystem m_CreateChirpSystem;
    private LifePathEventSystem m_LifePathEventSystem;
    private RadioTagSystem m_RadioTagSystem;
    private TutorialEventActivationSystem m_TutorialEventActivationSystem;
    private DateTime m_LastTimedEventTime;
    private TimeSpan m_TimedEventInterval;
    private EntityQuery m_EDWSBuildingQuery;
    private NativeParallelHashMap<Entity, uint> m_TriggerFrames;
    private TriggerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerPrefabSystem = this.World.GetOrCreateSystemManaged<TriggerPrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreateChirpSystem = this.World.GetOrCreateSystemManaged<CreateChirpSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LifePathEventSystem = this.World.GetOrCreateSystemManaged<LifePathEventSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RadioTagSystem = this.World.GetOrCreateSystemManaged<RadioTagSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialEventActivationSystem = this.World.GetOrCreateSystemManaged<TutorialEventActivationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Queues = new List<NativeQueue<TriggerAction>>();
      // ISSUE: reference to a compiler-generated field
      this.m_LastTimedEventTime = DateTime.MinValue;
      // ISSUE: reference to a compiler-generated field
      this.m_TimedEventInterval = new TimeSpan(0, 15, 0);
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerFrames = new NativeParallelHashMap<Entity, uint>(32, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_EDWSBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.EarlyDisasterWarningSystem>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      this.Enabled = false;
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Queues.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Queues[index].Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Queues.Clear();
      base.OnStopRunning();
    }

    public NativeQueue<TriggerAction> CreateActionBuffer()
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      NativeQueue<TriggerAction> actionBuffer = new NativeQueue<TriggerAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_Queues.Add(actionBuffer);
      return actionBuffer;
    }

    public void AddActionBufferWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies = JobHandle.CombineDependencies(this.m_Dependencies, handle);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      this.Dependency.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies.Complete();
      int length = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Queues.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        length += this.m_Queues[index].Count;
      }
      if (length == 0)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Queues.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Queues[index].Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Queues.Clear();
      }
      NativeArray<TriggerAction> nativeArray = new NativeArray<TriggerAction>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_Queues.Count; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        NativeQueue<TriggerAction> queue = this.m_Queues[index1];
        int count = queue.Count;
        for (int index2 = 0; index2 < count; ++index2)
          nativeArray[num++] = queue.Dequeue();
        queue.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Queues.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TriggerLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TriggerConditionData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PolicyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TutorialActivationEventData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RadioEventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LifePathEventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TriggerChirpData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      JobHandle deps1;
      JobHandle deps2;
      JobHandle deps3;
      JobHandle deps4;
      JobHandle dependency;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TriggerSystem.TriggerActionJob jobData = new TriggerSystem.TriggerActionJob()
      {
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_TriggerPrefabData = this.m_TriggerPrefabSystem.ReadTriggerPrefabData(out dependencies),
        m_Actions = nativeArray,
        m_ChirpQueue = this.m_CreateChirpSystem.GetQueue(out deps1),
        m_TriggerFrames = this.m_TriggerFrames,
        m_LifePathEventQueue = this.m_LifePathEventSystem.GetQueue(out deps2),
        m_RadioTagQueue = this.m_RadioTagSystem.GetInputQueue(out deps3),
        m_EmergencyRadioTagQueue = this.m_RadioTagSystem.GetEmergencyInputQueue(out deps4),
        m_TutorialTriggerQueue = this.m_TutorialEventActivationSystem.GetQueue(out dependency),
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceObjectData = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_ChirpData = this.__TypeHandle.__Game_Prefabs_TriggerChirpData_RO_BufferLookup,
        m_LifePathEventData = this.__TypeHandle.__Game_Prefabs_LifePathEventData_RO_ComponentLookup,
        m_RadioEventData = this.__TypeHandle.__Game_Prefabs_RadioEventData_RO_ComponentLookup,
        m_TutorialEventData = this.__TypeHandle.__Game_Prefabs_TutorialActivationEventData_RO_BufferLookup,
        m_CitizenData = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PolicyData = this.__TypeHandle.__Game_Prefabs_PolicyData_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_TriggerConditions = this.__TypeHandle.__Game_Prefabs_TriggerConditionData_RO_BufferLookup,
        m_CullingInfo = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_TrafficAccidentData = this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup,
        m_TriggerLimitData = this.__TypeHandle.__Game_Prefabs_TriggerLimitData_RO_ComponentLookup
      };
      Camera main = Camera.main;
      if ((UnityEngine.Object) main != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        jobData.m_CameraPosition = (float3) main.transform.position;
      }
      JobHandle jobHandle = jobData.Schedule<TriggerSystem.TriggerActionJob>(JobUtils.CombineDependencies(this.Dependency, dependencies, deps2, deps1, deps3, deps4, dependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CreateChirpSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LifePathEventSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_RadioTagSystem.AddInputQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_RadioTagSystem.AddEmergencyInputQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerPrefabSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialEventActivationSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!(DateTime.Now - this.m_LastTimedEventTime >= this.m_TimedEventInterval))
        return;
      Game.PSI.Telemetry.CityStats();
      // ISSUE: reference to a compiler-generated field
      this.m_LastTimedEventTime = DateTime.Now;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerFrames.Dispose();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerFrames.Clear();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      NativeKeyValueArrays<Entity, uint> keyValueArrays = this.m_TriggerFrames.GetKeyValueArrays((AllocatorManager.AllocatorHandle) Allocator.Temp);
      writer.Write(keyValueArrays.Length);
      for (int index = 0; index < keyValueArrays.Length; ++index)
      {
        writer.Write(keyValueArrays.Keys[index]);
        writer.Write(keyValueArrays.Values[index]);
      }
      keyValueArrays.Dispose();
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerFrames.Clear();
      int num1;
      reader.Read(out num1);
      for (int index = 0; index < num1; ++index)
      {
        Entity key;
        reader.Read(out key);
        uint num2;
        reader.Read(out num2);
        if (key != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TriggerFrames.Add(key, num2);
        }
      }
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
    public TriggerSystem()
    {
    }

    [BurstCompile]
    private struct TriggerActionJob : IJob
    {
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public TriggerPrefabData m_TriggerPrefabData;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<TriggerAction> m_Actions;
      public NativeQueue<ChirpCreationData> m_ChirpQueue;
      public NativeQueue<LifePathEventCreationData> m_LifePathEventQueue;
      public NativeQueue<RadioTag> m_RadioTagQueue;
      public NativeQueue<RadioTag> m_EmergencyRadioTagQueue;
      public NativeQueue<Entity> m_TutorialTriggerQueue;
      public NativeParallelHashMap<Entity, uint> m_TriggerFrames;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjectData;
      [ReadOnly]
      public BufferLookup<TriggerChirpData> m_ChirpData;
      [ReadOnly]
      public ComponentLookup<LifePathEventData> m_LifePathEventData;
      [ReadOnly]
      public ComponentLookup<RadioEventData> m_RadioEventData;
      [ReadOnly]
      public BufferLookup<TutorialActivationEventData> m_TutorialEventData;
      [ReadOnly]
      public ComponentLookup<TrafficAccidentData> m_TrafficAccidentData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PolicyData> m_PolicyData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfo;
      [ReadOnly]
      public BufferLookup<TriggerConditionData> m_TriggerConditions;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public ComponentLookup<TriggerLimitData> m_TriggerLimitData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Actions.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          TriggerAction action = this.m_Actions[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_TriggerPrefabData.HasAnyPrefabs(action.m_TriggerType, action.m_TriggerPrefab))
          {
            TargetType targetType = TargetType.Nothing;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.HasComponent(action.m_PrimaryTarget))
            {
              targetType = TargetType.Building;
              PrefabRef componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.TryGetComponent(action.m_PrimaryTarget, out componentData) && this.m_ServiceObjectData.HasComponent(componentData.m_Prefab))
                targetType |= TargetType.ServiceBuilding;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_CitizenData.HasComponent(action.m_PrimaryTarget))
              targetType = TargetType.Citizen;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PolicyData.HasComponent(action.m_TriggerPrefab))
              targetType = TargetType.Policy;
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoadData.HasComponent(action.m_PrimaryTarget))
            {
              targetType = TargetType.Road;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TrafficAccidentData.HasComponent(action.m_TriggerPrefab) && action.m_PrimaryTarget != Entity.Null && this.m_CullingInfo.HasComponent(action.m_PrimaryTarget))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                action.m_Value = math.distance(MathUtils.Center(this.m_CullingInfo[action.m_PrimaryTarget].m_Bounds), this.m_CameraPosition);
              }
            }
            Entity prefab;
            TriggerPrefabData.Iterator iterator;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TriggerPrefabData.TryGetFirstPrefab(action.m_TriggerType, targetType, action.m_TriggerPrefab, out prefab, out iterator))
            {
              // ISSUE: reference to a compiler-generated field
              do
              {
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                if (this.CheckInterval(prefab) && this.CheckConditions(prefab, action))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CreateEntity(ref random, prefab, action);
                }
              }
              while (this.m_TriggerPrefabData.TryGetNextPrefab(action.m_TriggerType, targetType, action.m_TriggerPrefab, out prefab, ref iterator));
            }
          }
        }
      }

      private bool CheckInterval(Entity prefab)
      {
        TriggerLimitData componentData;
        uint num;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !this.m_TriggerLimitData.TryGetComponent(prefab, out componentData) || !this.m_TriggerFrames.TryGetValue(prefab, out num) || this.m_SimulationFrame >= num + componentData.m_FrameInterval;
      }

      private bool CheckConditions(Entity prefab, TriggerAction action)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TriggerConditions.HasBuffer(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<TriggerConditionData> triggerCondition = this.m_TriggerConditions[prefab];
          for (int index = 0; index < triggerCondition.Length; ++index)
          {
            TriggerConditionData triggerConditionData = triggerCondition[index];
            switch (triggerConditionData.m_Type)
            {
              case TriggerConditionType.Equals:
                if ((double) Math.Abs(triggerConditionData.m_Value - action.m_Value) > 1.4012984643248171E-45)
                  return false;
                break;
              case TriggerConditionType.GreaterThan:
                if ((double) action.m_Value <= (double) triggerConditionData.m_Value)
                  return false;
                break;
              case TriggerConditionType.LessThan:
                if ((double) action.m_Value >= (double) triggerConditionData.m_Value)
                  return false;
                break;
            }
          }
        }
        return true;
      }

      private void CreateEntity(ref Unity.Mathematics.Random random, Entity prefab, TriggerAction triggerAction)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TriggerLimitData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TriggerFrames[prefab] = this.m_SimulationFrame;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ChirpData.HasBuffer(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ChirpQueue.Enqueue(new ChirpCreationData()
          {
            m_TriggerPrefab = prefab,
            m_Sender = triggerAction.m_PrimaryTarget,
            m_Target = triggerAction.m_SecondaryTarget
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LifePathEventData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LifePathEventQueue.Enqueue(new LifePathEventCreationData()
            {
              m_EventPrefab = prefab,
              m_Sender = triggerAction.m_PrimaryTarget,
              m_Target = triggerAction.m_SecondaryTarget,
              m_OriginalSender = Entity.Null
            });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_RadioEventData.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              RadioEventData radioEventData = this.m_RadioEventData[prefab];
              if (radioEventData.m_SegmentType == Game.Audio.Radio.Radio.SegmentType.Emergency)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_EmergencyRadioTagQueue.Enqueue(new RadioTag()
                {
                  m_Event = prefab,
                  m_Target = triggerAction.m_PrimaryTarget,
                  m_SegmentType = radioEventData.m_SegmentType,
                  m_EmergencyFrameDelay = radioEventData.m_EmergencyFrameDelay
                });
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_RadioTagQueue.Enqueue(new RadioTag()
                {
                  m_Event = prefab,
                  m_Target = triggerAction.m_PrimaryTarget,
                  m_SegmentType = radioEventData.m_SegmentType
                });
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_TutorialEventData.HasBuffer(prefab))
                return;
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TutorialActivationEventData> dynamicBuffer = this.m_TutorialEventData[prefab];
              for (int index = 0; index < dynamicBuffer.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_TutorialTriggerQueue.Enqueue(dynamicBuffer[index].m_Tutorial);
              }
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TriggerChirpData> __Game_Prefabs_TriggerChirpData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<LifePathEventData> __Game_Prefabs_LifePathEventData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RadioEventData> __Game_Prefabs_RadioEventData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TutorialActivationEventData> __Game_Prefabs_TutorialActivationEventData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PolicyData> __Game_Prefabs_PolicyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TriggerConditionData> __Game_Prefabs_TriggerConditionData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficAccidentData> __Game_Prefabs_TrafficAccidentData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TriggerLimitData> __Game_Prefabs_TriggerLimitData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TriggerChirpData_RO_BufferLookup = state.GetBufferLookup<TriggerChirpData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LifePathEventData_RO_ComponentLookup = state.GetComponentLookup<LifePathEventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RadioEventData_RO_ComponentLookup = state.GetComponentLookup<RadioEventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TutorialActivationEventData_RO_BufferLookup = state.GetBufferLookup<TutorialActivationEventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PolicyData_RO_ComponentLookup = state.GetComponentLookup<PolicyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TriggerConditionData_RO_BufferLookup = state.GetBufferLookup<TriggerConditionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup = state.GetComponentLookup<TrafficAccidentData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TriggerLimitData_RO_ComponentLookup = state.GetComponentLookup<TriggerLimitData>(true);
      }
    }
  }
}
