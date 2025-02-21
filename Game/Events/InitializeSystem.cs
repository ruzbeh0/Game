// Decompiled with JetBrains decompiler
// Type: Game.Events.InitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class InitializeSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private CitySystem m_CitySystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_EventQuery;
    private EntityQuery m_InstanceQuery;
    private EntityQuery m_DisasterConfigQuery;
    private EntityQuery m_TargetQuery;
    private EntityQuery m_EDWSBuildingQuery;
    private EntityArchetype m_IgniteEventArchetype;
    private EntityArchetype m_ImpactEventArchetype;
    private EntityArchetype m_AccidentSiteEventArchetype;
    private EntityArchetype m_HealthEventArchetype;
    private EntityArchetype m_DamageEventArchetype;
    private EntityArchetype m_DestroyEventArchetype;
    private EntityArchetype m_SpectateEventArchetype;
    private EntityArchetype m_CriminalEventArchetype;
    private TriggerSystem m_TriggerSystem;
    private EntityCommandBuffer m_CommandBuffer;
    private InitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceQuery = this.GetEntityQuery(ComponentType.ReadOnly<Event>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DisasterConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<DisasterConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TargetQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Tree>(),
          ComponentType.ReadOnly<Road>(),
          ComponentType.ReadOnly<Citizen>(),
          ComponentType.ReadOnly<Household>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_EDWSBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.EarlyDisasterWarningSystem>());
      // ISSUE: reference to a compiler-generated field
      this.m_IgniteEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Ignite>());
      // ISSUE: reference to a compiler-generated field
      this.m_ImpactEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Impact>());
      // ISSUE: reference to a compiler-generated field
      this.m_AccidentSiteEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<AddAccidentSite>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<AddHealthProblem>());
      // ISSUE: reference to a compiler-generated field
      this.m_DamageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Destroy>());
      // ISSUE: reference to a compiler-generated field
      this.m_SpectateEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Spectate>());
      // ISSUE: reference to a compiler-generated field
      this.m_CriminalEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<AddCriminal>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_EventQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeQueue<TriggerAction> actionBuffer = this.m_TriggerSystem.CreateActionBuffer();
        for (int index1 = 0; index1 < entityArray.Length; ++index1)
        {
          Entity entity = entityArray[index1];
          PrefabRef componentData1 = this.EntityManager.GetComponentData<PrefabRef>(entity);
          EventData componentData2 = this.EntityManager.GetComponentData<EventData>(componentData1.m_Prefab);
          // ISSUE: reference to a compiler-generated method
          if (componentData2.m_ConcurrentLimit > 0 && this.CountInstances(componentData1.m_Prefab) > componentData2.m_ConcurrentLimit)
          {
            this.EntityManager.AddComponent<Deleted>(entity);
          }
          else
          {
            if (this.EntityManager.HasComponent<Fire>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeFire(entity);
            }
            if (this.EntityManager.HasComponent<TrafficAccident>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeTrafficAccident(entity);
            }
            if (this.EntityManager.HasComponent<WeatherPhenomenon>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeWeatherEvent(entity);
            }
            if (this.EntityManager.HasComponent<HealthEvent>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeHealthEvent(entity);
            }
            if (this.EntityManager.HasComponent<Destruction>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeDestruction(entity);
            }
            if (this.EntityManager.HasComponent<SpectatorEvent>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeSpectatorEvent(entity);
            }
            if (this.EntityManager.HasComponent<Crime>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeCrimeEvent(entity);
            }
            if (this.EntityManager.HasComponent<WaterLevelChange>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeWaterLevelChangeEvent(entity);
            }
            if (this.EntityManager.HasComponent<CalendarEvent>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeCalendarEvent(entity);
            }
            if (this.EntityManager.HasComponent<CoordinatedMeeting>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.InitializeMeetingEvent(entity);
            }
            DynamicBuffer<TargetElement> buffer;
            TriggerAction triggerAction1;
            if (this.EntityManager.TryGetBuffer<TargetElement>(entity, true, out buffer) && buffer.Length > 0)
            {
              for (int index2 = 0; index2 < buffer.Length; ++index2)
              {
                ref NativeQueue<TriggerAction> local = ref actionBuffer;
                triggerAction1 = new TriggerAction();
                triggerAction1.m_TriggerPrefab = componentData1.m_Prefab;
                triggerAction1.m_PrimaryTarget = buffer[index2].m_Entity;
                triggerAction1.m_SecondaryTarget = Entity.Null;
                triggerAction1.m_TriggerType = TriggerType.EventHappened;
                TriggerAction triggerAction2 = triggerAction1;
                local.Enqueue(triggerAction2);
              }
            }
            else
            {
              ref NativeQueue<TriggerAction> local = ref actionBuffer;
              triggerAction1 = new TriggerAction();
              triggerAction1.m_TriggerPrefab = componentData1.m_Prefab;
              triggerAction1.m_PrimaryTarget = Entity.Null;
              triggerAction1.m_SecondaryTarget = Entity.Null;
              triggerAction1.m_TriggerType = TriggerType.EventHappened;
              TriggerAction triggerAction3 = triggerAction1;
              local.Enqueue(triggerAction3);
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CommandBuffer.IsCreated)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.Playback(this.EntityManager);
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.Dispose();
    }

    private int CountInstances(Entity prefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabRef> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_InstanceQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      this.CompleteDependency();
      int num = 0;
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        NativeArray<PrefabRef> nativeArray = archetypeChunkArray[index1].GetNativeArray<PrefabRef>(ref componentTypeHandle);
        for (int index2 = 0; index2 < nativeArray.Length; ++index2)
        {
          if (nativeArray[index2].m_Prefab == prefab)
            ++num;
        }
      }
      archetypeChunkArray.Dispose();
      return num;
    }

    private EntityCommandBuffer GetCommandBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CommandBuffer.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer = new EntityCommandBuffer((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_CommandBuffer;
    }

    private void InitializeFire(Entity eventEntity)
    {
      FireData componentData = this.EntityManager.GetComponentData<FireData>(this.EntityManager.GetComponentData<PrefabRef>(eventEntity).m_Prefab);
      if (componentData.m_RandomTargetType == EventTargetType.None)
        return;
      DynamicBuffer<TargetElement> buffer = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
      if (buffer.Length == 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.AddRandomTarget(buffer, componentData.m_RandomTargetType, TransportType.None);
      }
      // ISSUE: reference to a compiler-generated method
      EntityCommandBuffer commandBuffer = this.GetCommandBuffer();
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity entity1 = buffer[index].m_Entity;
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = commandBuffer.CreateEntity(this.m_IgniteEventArchetype);
        commandBuffer.SetComponent<Ignite>(entity2, new Ignite()
        {
          m_Event = eventEntity,
          m_Target = entity1,
          m_Intensity = componentData.m_StartIntensity
        });
      }
    }

    private void InitializeTrafficAccident(Entity eventEntity)
    {
      TrafficAccidentData componentData = this.EntityManager.GetComponentData<TrafficAccidentData>(this.EntityManager.GetComponentData<PrefabRef>(eventEntity).m_Prefab);
      if (componentData.m_RandomSiteType == EventTargetType.None)
        return;
      DynamicBuffer<TargetElement> buffer = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
      if (buffer.Length == 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.AddRandomTarget(buffer, componentData.m_RandomSiteType, TransportType.None);
      }
      Unity.Mathematics.Random random = RandomSeed.Next().GetRandom(eventEntity.Index);
      // ISSUE: reference to a compiler-generated method
      EntityCommandBuffer commandBuffer = this.GetCommandBuffer();
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity entity1 = buffer[index].m_Entity;
        Moving component1;
        if (this.EntityManager.TryGetComponent<Moving>(entity1, out component1))
        {
          Impact component2 = new Impact()
          {
            m_Event = eventEntity,
            m_Target = entity1,
            m_Severity = 5f
          };
          if (random.NextBool())
          {
            component2.m_AngularVelocityDelta.y = -2f;
            component2.m_VelocityDelta.xz = component2.m_Severity * MathUtils.Left(math.normalizesafe(component1.m_Velocity.xz));
          }
          else
          {
            component2.m_AngularVelocityDelta.y = 2f;
            component2.m_VelocityDelta.xz = component2.m_Severity * MathUtils.Right(math.normalizesafe(component1.m_Velocity.xz));
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = commandBuffer.CreateEntity(this.m_ImpactEventArchetype);
          commandBuffer.SetComponent<Impact>(entity2, component2);
        }
        else if (this.EntityManager.TryGetComponent<Road>(entity1, out Road _))
        {
          AddAccidentSite component3 = new AddAccidentSite()
          {
            m_Event = eventEntity,
            m_Target = entity1,
            m_Flags = AccidentSiteFlags.StageAccident | AccidentSiteFlags.TrafficAccident
          };
          // ISSUE: reference to a compiler-generated field
          Entity entity3 = commandBuffer.CreateEntity(this.m_AccidentSiteEventArchetype);
          commandBuffer.SetComponent<AddAccidentSite>(entity3, component3);
        }
      }
    }

    private void InitializeWeatherEvent(Entity eventEntity)
    {
      WeatherPhenomenon componentData1 = this.EntityManager.GetComponentData<WeatherPhenomenon>(eventEntity);
      Duration componentData2 = this.EntityManager.GetComponentData<Duration>(eventEntity);
      DynamicBuffer<TargetElement> buffer1 = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
      PrefabRef componentData3 = this.EntityManager.GetComponentData<PrefabRef>(eventEntity);
      WeatherPhenomenonData componentData4 = this.EntityManager.GetComponentData<WeatherPhenomenonData>(componentData3.m_Prefab);
      DynamicBuffer<HotspotFrame> buffer2 = this.EntityManager.GetBuffer<HotspotFrame>(eventEntity);
      Unity.Mathematics.Random random = RandomSeed.Next().GetRandom(eventEntity.Index);
      if ((double) componentData1.m_PhenomenonRadius == 0.0)
        componentData1.m_PhenomenonRadius = random.NextFloat(componentData4.m_PhenomenonRadius.min, componentData4.m_PhenomenonRadius.max);
      if ((double) componentData1.m_HotspotRadius == 0.0)
        componentData1.m_HotspotRadius = componentData1.m_PhenomenonRadius * random.NextFloat(componentData4.m_HotspotRadius.min, componentData4.m_HotspotRadius.max);
      if (componentData2.m_StartFrame == 0U)
      {
        float num = 0.0f;
        if (componentData4.m_DangerFlags != (DangerFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CityModifier> buffer3 = this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
          CityUtils.ApplyModifier(ref num, buffer3, CityModifierType.DisasterWarningTime);
        }
        // ISSUE: reference to a compiler-generated field
        componentData2.m_StartFrame = this.m_SimulationSystem.frameIndex + (uint) ((double) num * 60.0);
      }
      if (componentData2.m_EndFrame == 0U)
      {
        float num = random.NextFloat(componentData4.m_Duration.min, componentData4.m_Duration.max);
        componentData2.m_EndFrame = componentData2.m_StartFrame + (uint) ((double) num * 60.0);
      }
      bool flag = !componentData1.m_PhenomenonPosition.Equals(new float3());
      for (int index = 0; index < buffer1.Length && !flag; ++index)
      {
        Game.Objects.Transform component;
        if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(buffer1[index].m_Entity, out component))
        {
          componentData1.m_PhenomenonPosition = component.m_Position;
          flag = true;
        }
      }
      if (!flag)
      {
        // ISSUE: reference to a compiler-generated method
        componentData1.m_PhenomenonPosition = this.FindRandomLocation(ref random);
      }
      if (componentData1.m_HotspotPosition.Equals(new float3()))
      {
        componentData1.m_HotspotPosition = componentData1.m_PhenomenonPosition;
        componentData1.m_HotspotPosition.xz += random.NextFloat2Direction() * random.NextFloat(componentData1.m_PhenomenonRadius - componentData1.m_HotspotRadius);
      }
      if ((double) componentData1.m_LightningTimer == 0.0 && (double) componentData4.m_LightningInterval.min > 1.0 / 1000.0)
      {
        float num = math.min(componentData4.m_LightningInterval.min, (float) (componentData2.m_EndFrame - componentData2.m_StartFrame) / 60f);
        componentData1.m_LightningTimer = 5f + math.max(0.0f, num - 10f);
      }
      buffer2.ResizeUninitialized(4);
      for (int index = 0; index < buffer2.Length; ++index)
        buffer2[index] = new HotspotFrame(componentData1);
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.HasComponent<EarlyDisasterWarningEventData>(componentData3.m_Prefab) && !this.m_EDWSBuildingQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        foreach (Entity entity in this.m_EDWSBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
          this.EntityManager.AddComponentData<EarlyDisasterWarningDuration>(entity, new EarlyDisasterWarningDuration()
          {
            m_EndFrame = componentData2.m_EndFrame
          });
      }
      this.EntityManager.SetComponentData<WeatherPhenomenon>(eventEntity, componentData1);
      this.EntityManager.SetComponentData<Duration>(eventEntity, componentData2);
      this.EntityManager.SetComponentData<InterpolatedTransform>(eventEntity, new InterpolatedTransform(componentData1));
    }

    private void InitializeHealthEvent(Entity eventEntity)
    {
      HealthEventData componentData = this.EntityManager.GetComponentData<HealthEventData>(this.EntityManager.GetComponentData<PrefabRef>(eventEntity).m_Prefab);
      if (componentData.m_RandomTargetType == EventTargetType.None)
        return;
      DynamicBuffer<TargetElement> buffer = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
      if (buffer.Length == 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.AddRandomTarget(buffer, componentData.m_RandomTargetType, TransportType.None);
      }
      Unity.Mathematics.Random random = RandomSeed.Next().GetRandom(eventEntity.Index);
      // ISSUE: reference to a compiler-generated method
      EntityCommandBuffer commandBuffer = this.GetCommandBuffer();
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity entity1 = buffer[index].m_Entity;
        Game.Creatures.Resident component1;
        if (this.EntityManager.TryGetComponent<Game.Creatures.Resident>(entity1, out component1))
        {
          entity1 = component1.m_Citizen;
          buffer[index] = new TargetElement(entity1);
        }
        Citizen component2;
        if (this.EntityManager.TryGetComponent<Citizen>(entity1, out component2))
        {
          HealthProblemFlags healthProblemFlags = HealthProblemFlags.None;
          switch (componentData.m_HealthEventType)
          {
            case HealthEventType.Disease:
              healthProblemFlags |= HealthProblemFlags.Sick;
              break;
            case HealthEventType.Injury:
              healthProblemFlags |= HealthProblemFlags.Injured;
              break;
            case HealthEventType.Death:
              healthProblemFlags |= HealthProblemFlags.Dead;
              break;
          }
          float num = math.lerp(componentData.m_TransportProbability.max, componentData.m_TransportProbability.min, (float) component2.m_Health * 0.01f);
          if ((double) random.NextFloat(100f) < (double) num)
            healthProblemFlags |= HealthProblemFlags.RequireTransport;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = commandBuffer.CreateEntity(this.m_HealthEventArchetype);
          if (componentData.m_RequireTracking)
            commandBuffer.SetComponent<AddHealthProblem>(entity2, new AddHealthProblem()
            {
              m_Event = eventEntity,
              m_Target = entity1,
              m_Flags = healthProblemFlags
            });
          else
            commandBuffer.SetComponent<AddHealthProblem>(entity2, new AddHealthProblem()
            {
              m_Target = entity1,
              m_Flags = healthProblemFlags
            });
        }
      }
      if (componentData.m_RequireTracking)
        return;
      buffer.Clear();
    }

    private void InitializeDestruction(Entity eventEntity)
    {
      DestructionData componentData = this.EntityManager.GetComponentData<DestructionData>(this.EntityManager.GetComponentData<PrefabRef>(eventEntity).m_Prefab);
      if (componentData.m_RandomTargetType == EventTargetType.None)
        return;
      DynamicBuffer<TargetElement> buffer = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
      if (buffer.Length == 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.AddRandomTarget(buffer, componentData.m_RandomTargetType, TransportType.None);
      }
      // ISSUE: reference to a compiler-generated method
      EntityCommandBuffer commandBuffer1 = this.GetCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      IconCommandBuffer commandBuffer2 = this.m_IconCommandSystem.CreateCommandBuffer();
      DisasterConfigurationData configurationData = new DisasterConfigurationData();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DisasterConfigQuery.IsEmpty)
      {
        // ISSUE: reference to a compiler-generated field
        configurationData = this.m_DisasterConfigQuery.GetSingleton<DisasterConfigurationData>();
      }
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity entity1 = buffer[index].m_Entity;
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = commandBuffer1.CreateEntity(this.m_DamageEventArchetype);
        commandBuffer1.SetComponent<Damage>(entity2, new Damage()
        {
          m_Object = entity1,
          m_Delta = new float3(1f, 0.0f, 0.0f)
        });
        // ISSUE: reference to a compiler-generated field
        Entity entity3 = commandBuffer1.CreateEntity(this.m_DestroyEventArchetype);
        commandBuffer1.SetComponent<Destroy>(entity3, new Destroy()
        {
          m_Event = eventEntity,
          m_Object = entity1
        });
        if (configurationData.m_DestroyedNotificationPrefab != Entity.Null)
        {
          commandBuffer2.Remove(entity1, IconPriority.Problem);
          commandBuffer2.Remove(entity1, IconPriority.FatalProblem);
          commandBuffer2.Add(entity1, configurationData.m_DestroyedNotificationPrefab, IconPriority.FatalProblem, flags: IconFlags.IgnoreTarget, target: eventEntity);
        }
      }
    }

    private void InitializeSpectatorEvent(Entity eventEntity)
    {
      Duration componentData1 = this.EntityManager.GetComponentData<Duration>(eventEntity);
      PrefabRef componentData2 = this.EntityManager.GetComponentData<PrefabRef>(eventEntity);
      SpectatorEventData componentData3 = this.EntityManager.GetComponentData<SpectatorEventData>(componentData2.m_Prefab);
      if (componentData3.m_RandomSiteType != EventTargetType.None)
      {
        DynamicBuffer<TargetElement> buffer = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
        if (buffer.Length == 0)
        {
          VehicleLaunchData component;
          if (this.EntityManager.TryGetComponent<VehicleLaunchData>(componentData2.m_Prefab, out component))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddRandomTarget(buffer, componentData3.m_RandomSiteType, component.m_TransportType);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddRandomTarget(buffer, componentData3.m_RandomSiteType, TransportType.None);
          }
        }
        // ISSUE: reference to a compiler-generated method
        EntityCommandBuffer commandBuffer = this.GetCommandBuffer();
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity entity1 = buffer[index].m_Entity;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = commandBuffer.CreateEntity(this.m_SpectateEventArchetype);
          commandBuffer.SetComponent<Spectate>(entity2, new Spectate()
          {
            m_Event = eventEntity,
            m_Target = entity1
          });
        }
      }
      // ISSUE: reference to a compiler-generated field
      componentData1.m_StartFrame = this.m_SimulationSystem.frameIndex + (uint) (262144.0 * (double) componentData3.m_PreparationDuration);
      componentData1.m_EndFrame = componentData1.m_StartFrame + (uint) (262144.0 * (double) componentData3.m_ActiveDuration);
      this.EntityManager.SetComponentData<Duration>(eventEntity, componentData1);
    }

    private void InitializeWaterLevelChangeEvent(Entity eventEntity)
    {
      WaterLevelChange componentData1 = this.EntityManager.GetComponentData<WaterLevelChange>(eventEntity);
      WaterLevelChangeData componentData2 = this.EntityManager.GetComponentData<WaterLevelChangeData>(this.EntityManager.GetComponentData<PrefabRef>(eventEntity).m_Prefab);
      Duration componentData3 = this.EntityManager.GetComponentData<Duration>(eventEntity);
      float num = RandomSeed.Next().GetRandom(eventEntity.Index).NextFloat();
      componentData1.m_MaxIntensity = (float) (0.30000001192092896 + 0.699999988079071 * (double) num * (double) num);
      componentData1.m_Direction = new float2(0.0f, 1f);
      EntityQuery entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Simulation.WaterSourceData>(), ComponentType.ReadOnly<Game.Objects.Transform>());
      NativeArray<Game.Simulation.WaterSourceData> componentDataArray1 = entityQuery.ToComponentDataArray<Game.Simulation.WaterSourceData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<Game.Objects.Transform> componentDataArray2 = entityQuery.ToComponentDataArray<Game.Objects.Transform>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      componentData3.m_StartFrame = this.m_SimulationSystem.frameIndex;
      componentData3.m_EndFrame = componentData2.m_ChangeType != WaterLevelChangeType.Sine ? componentData3.m_StartFrame + 10000U : (uint) ((ulong) componentData3.m_StartFrame + (ulong) Mathf.CeilToInt((float) WaterLevelChangeSystem.TsunamiEndDelay + 12000f * componentData1.m_MaxIntensity));
      this.EntityManager.SetComponentData<Duration>(eventEntity, componentData3);
      for (int index = 0; index < componentDataArray1.Length; ++index)
      {
        Game.Simulation.WaterSourceData source = componentDataArray1[index];
        Game.Objects.Transform transform = componentDataArray2[index];
        // ISSUE: reference to a compiler-generated method
        if ((source.m_ConstantDepth == 2 && (componentData2.m_TargetType & WaterLevelTargetType.River) != WaterLevelTargetType.None || source.m_ConstantDepth == 3 && (componentData2.m_TargetType & WaterLevelTargetType.Sea) != WaterLevelTargetType.None) && WaterSystem.SourceMatchesDirection(source, transform, componentData1.m_Direction))
          componentData1.m_DangerHeight = math.max(componentData1.m_DangerHeight, source.m_Amount + (float) ((double) source.m_Multiplier * (double) componentData1.m_MaxIntensity * 2.0));
      }
      componentDataArray1.Dispose();
      componentDataArray2.Dispose();
      this.EntityManager.SetComponentData<WaterLevelChange>(eventEntity, componentData1);
    }

    private void InitializeCrimeEvent(Entity eventEntity)
    {
      Game.Prefabs.CrimeData componentData = this.EntityManager.GetComponentData<Game.Prefabs.CrimeData>(this.EntityManager.GetComponentData<PrefabRef>(eventEntity).m_Prefab);
      if (componentData.m_RandomTargetType == EventTargetType.None)
        return;
      DynamicBuffer<TargetElement> buffer = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
      if (buffer.Length == 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.AddRandomTarget(buffer, componentData.m_RandomTargetType, TransportType.None);
      }
      RandomSeed.Next().GetRandom(eventEntity.Index);
      // ISSUE: reference to a compiler-generated method
      EntityCommandBuffer commandBuffer = this.GetCommandBuffer();
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity entity1 = buffer[index].m_Entity;
        Game.Creatures.Resident component;
        if (this.EntityManager.TryGetComponent<Game.Creatures.Resident>(entity1, out component))
        {
          entity1 = component.m_Citizen;
          buffer[index] = new TargetElement(entity1);
        }
        if (this.EntityManager.TryGetComponent<Citizen>(entity1, out Citizen _))
        {
          CriminalFlags criminalFlags = CriminalFlags.Planning;
          if (componentData.m_CrimeType == CrimeType.Robbery)
            criminalFlags |= CriminalFlags.Robber;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = commandBuffer.CreateEntity(this.m_CriminalEventArchetype);
          commandBuffer.SetComponent<AddCriminal>(entity2, new AddCriminal()
          {
            m_Event = eventEntity,
            m_Target = entity1,
            m_Flags = criminalFlags
          });
        }
      }
    }

    private void InitializeMeetingEvent(Entity eventEntity)
    {
      DynamicBuffer<CoordinatedMeetingAttendee> buffer1 = this.EntityManager.GetBuffer<CoordinatedMeetingAttendee>(eventEntity);
      DynamicBuffer<TargetElement> buffer2 = this.EntityManager.GetBuffer<TargetElement>(eventEntity);
      for (int index1 = 0; index1 < buffer2.Length; ++index1)
      {
        Entity entity = buffer2[index1].m_Entity;
        EntityManager entityManager = this.EntityManager;
        if (entityManager.HasComponent<HouseholdCitizen>(entity))
        {
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<AttendingEvent>(entity))
          {
            entityManager = this.EntityManager;
            DynamicBuffer<HouseholdCitizen> buffer3 = entityManager.GetBuffer<HouseholdCitizen>(entity, true);
            for (int index2 = 0; index2 < buffer3.Length; ++index2)
              buffer1.Add(new CoordinatedMeetingAttendee()
              {
                m_Attendee = buffer3[index2].m_Citizen
              });
            continue;
          }
        }
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<Citizen>(entity))
          buffer1.Add(new CoordinatedMeetingAttendee()
          {
            m_Attendee = entity
          });
      }
    }

    private void InitializeCalendarEvent(Entity eventEntity)
    {
      CalendarEventData componentData1 = this.EntityManager.GetComponentData<CalendarEventData>(this.EntityManager.GetComponentData<PrefabRef>(eventEntity).m_Prefab);
      // ISSUE: reference to a compiler-generated field
      Duration componentData2 = this.EntityManager.GetComponentData<Duration>(eventEntity) with
      {
        m_StartFrame = this.m_SimulationSystem.frameIndex
      };
      componentData2.m_EndFrame = componentData2.m_StartFrame + (uint) (componentData1.m_Duration * 262144 / 4);
      this.EntityManager.SetComponentData<Duration>(eventEntity, componentData2);
      // ISSUE: reference to a compiler-generated method
      this.GetCommandBuffer().AddComponent<FindingEventParticipants>(eventEntity);
    }

    private float3 FindRandomLocation(ref Unity.Mathematics.Random random)
    {
      return new float3()
      {
        xz = random.NextFloat2((float2) -6000f, (float2) 6000f)
      };
    }

    private void AddRandomTarget(
      DynamicBuffer<TargetElement> targets,
      EventTargetType targetType,
      TransportType transportType)
    {
      using (NativeValue<Entity> nativeValue = new NativeValue<Entity>(Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        new InitializeSystem.RandomEventTargetJob()
        {
          m_TargetChunks = this.m_TargetQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
          m_TargetType = targetType,
          m_TransportType = transportType,
          m_RandomSeed = RandomSeed.Next(),
          m_Result = nativeValue,
          m_EntitiesType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
          m_TransportDepotType = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle,
          m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle,
          m_RoadType = this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_PrefabTransportDepotData = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup
        }.Run<InitializeSystem.RandomEventTargetJob>();
        if (!(nativeValue.value != Entity.Null))
          return;
        targets.Add(new TargetElement(nativeValue.value));
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
    public InitializeSystem()
    {
    }

    [BurstCompile]
    private struct RandomEventTargetJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_TargetChunks;
      [ReadOnly]
      public EventTargetType m_TargetType;
      [ReadOnly]
      public TransportType m_TransportType;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public NativeValue<Entity> m_Result;
      [ReadOnly]
      public EntityTypeHandle m_EntitiesType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
      [ReadOnly]
      public ComponentTypeHandle<Tree> m_TreeType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Road> m_RoadType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        Entity target;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.FindRandomTarget(ref random, this.m_TargetType, this.m_TransportType, out target))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Result.value = target;
      }

      private bool FindRandomTarget(
        ref Unity.Mathematics.Random random,
        EventTargetType type,
        TransportType transportType,
        out Entity target)
      {
        int totalCount = 0;
        target = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_TargetChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk targetChunk = this.m_TargetChunks[index1];
          switch (type)
          {
            case EventTargetType.Building:
              // ISSUE: reference to a compiler-generated field
              if (targetChunk.Has<Building>(ref this.m_BuildingType))
                break;
              continue;
            case EventTargetType.WildTree:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!targetChunk.Has<Tree>(ref this.m_TreeType) || targetChunk.Has<Owner>(ref this.m_OwnerType))
                continue;
              break;
            case EventTargetType.Road:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!targetChunk.Has<Road>(ref this.m_RoadType) || !targetChunk.Has<Game.Net.Edge>(ref this.m_EdgeType))
                continue;
              break;
            case EventTargetType.Citizen:
              // ISSUE: reference to a compiler-generated field
              if (targetChunk.Has<Citizen>(ref this.m_CitizenType))
                break;
              continue;
            case EventTargetType.TransportDepot:
              // ISSUE: reference to a compiler-generated field
              if (!targetChunk.Has<Game.Buildings.TransportDepot>(ref this.m_TransportDepotType))
                continue;
              break;
            default:
              continue;
          }
          if (transportType != TransportType.None)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckDepotType(ref random, ref totalCount, ref target, targetChunk, transportType);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray = targetChunk.GetNativeArray(this.m_EntitiesType);
            int index2 = random.NextInt(-totalCount, nativeArray.Length);
            if (index2 >= 0)
              target = nativeArray[index2];
            totalCount += nativeArray.Length;
          }
        }
        return target != Entity.Null;
      }

      private void CheckDepotType(
        ref Unity.Mathematics.Random random,
        ref int totalCount,
        ref Entity target,
        ArchetypeChunk chunk,
        TransportType transportType)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntitiesType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTransportDepotData[nativeArray2[index].m_Prefab].m_TransportType == transportType)
          {
            if (random.NextInt(-totalCount, 1) >= 0)
              target = nativeArray1[index];
            ++totalCount;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Road> __Game_Net_Road_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TransportDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentLookup = state.GetComponentLookup<TransportDepotData>(true);
      }
    }
  }
}
