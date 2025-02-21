// Decompiled with JetBrains decompiler
// Type: Game.Notifications.IconCommandSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Notifications
{
  [CompilerGenerated]
  public class IconCommandSystem : GameSystemBase
  {
    private ModificationEndBarrier m_ModificationBarrier;
    private EntityQuery m_ConfigurationQuery;
    private List<NativeQueue<IconCommandBuffer.Command>> m_Queues;
    private JobHandle m_Dependencies;
    private int m_BufferIndex;
    private IconCommandSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_Queues = new List<NativeQueue<IconCommandBuffer.Command>>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<IconConfigurationData>());
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

    public IconCommandBuffer CreateCommandBuffer()
    {
      NativeQueue<IconCommandBuffer.Command> nativeQueue = new NativeQueue<IconCommandBuffer.Command>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_Queues.Add(nativeQueue);
      // ISSUE: reference to a compiler-generated field
      return new IconCommandBuffer(nativeQueue.AsParallelWriter(), this.m_BufferIndex++);
    }

    public void AddCommandBufferWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies = JobHandle.CombineDependencies(this.m_Dependencies, handle);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_BufferIndex = 0;
      int length = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Queues.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        length += this.m_Queues[index].Count;
      }
      // ISSUE: reference to a compiler-generated field
      if (length == 0 || this.m_ConfigurationQuery.IsEmptyIgnoreFilter)
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
      else
      {
        NativeArray<IconCommandBuffer.Command> nativeArray = new NativeArray<IconCommandBuffer.Command>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Queues.Count; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          NativeQueue<IconCommandBuffer.Command> queue = this.m_Queues[index1];
          int count = queue.Count;
          for (int index2 = 0; index2 < count; ++index2)
            nativeArray[num++] = queue.Dequeue();
          queue.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Queues.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Notifications_IconElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IconAnimationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NotificationIconData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
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
        JobHandle producerJob = new IconCommandSystem.IconCommandPlaybackJob()
        {
          m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_NotificationIconData = this.__TypeHandle.__Game_Prefabs_NotificationIconData_RO_ComponentLookup,
          m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
          m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
          m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
          m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
          m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
          m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
          m_IconAnimations = this.__TypeHandle.__Game_Prefabs_IconAnimationElement_RO_BufferLookup,
          m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentLookup,
          m_IconElements = this.__TypeHandle.__Game_Notifications_IconElement_RW_BufferLookup,
          m_ConfigurationEntity = this.m_ConfigurationQuery.GetSingletonEntity(),
          m_DeltaTime = UnityEngine.Time.deltaTime,
          m_Commands = nativeArray,
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
        }.Schedule<IconCommandSystem.IconCommandPlaybackJob>(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
        this.Dependency = producerJob;
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
    public IconCommandSystem()
    {
    }

    [BurstCompile]
    private struct IconCommandPlaybackJob : IJob
    {
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NotificationIconData> m_NotificationIconData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<IconAnimationElement> m_IconAnimations;
      public ComponentLookup<Icon> m_IconData;
      public BufferLookup<IconElement> m_IconElements;
      [ReadOnly]
      public Entity m_ConfigurationEntity;
      [ReadOnly]
      public float m_DeltaTime;
      [DeallocateOnJobCompletion]
      public NativeArray<IconCommandBuffer.Command> m_Commands;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int length = this.m_Commands.Length;
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_Commands.Sort<IconCommandBuffer.Command>();
        int num2;
        for (; num1 < length; num1 = num2)
        {
          // ISSUE: reference to a compiler-generated field
          Entity owner = this.m_Commands[num1].m_Owner;
          num2 = num1;
          // ISSUE: reference to a compiler-generated field
          do
            ;
          while (++num2 < length && !(this.m_Commands[num2].m_Owner != owner));
          // ISSUE: reference to a compiler-generated field
          if (this.m_EntityLookup.Exists(owner))
          {
            // ISSUE: reference to a compiler-generated method
            this.ProcessCommands(owner, num1, num2);
          }
        }
      }

      private unsafe void ProcessCommands(Entity owner, int startIndex, int endIndex)
      {
        DynamicBuffer<IconElement> iconElements1 = new DynamicBuffer<IconElement>();
        DynamicBuffer<IconElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        this.m_IconElements.TryGetBuffer(owner, out bufferData);
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_DeletedData.HasComponent(owner);
        int2* int2Ptr = stackalloc int2[16];
        int num = 0;
label_96:
        for (int index1 = startIndex; index1 < endIndex; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          IconCommandBuffer.Command command1 = this.m_Commands[index1];
          if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.All) != (IconCommandBuffer.CommandFlags) 0)
          {
            for (int index2 = index1 + 1; index2 < endIndex; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              IconCommandBuffer.Command command2 = this.m_Commands[index2];
              if ((command2.m_CommandFlags & IconCommandBuffer.CommandFlags.All) != (IconCommandBuffer.CommandFlags) 0 && command2.m_Priority == command1.m_Priority)
                goto label_96;
            }
          }
          else
          {
            for (int index3 = index1 + 1; index3 < endIndex; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              IconCommandBuffer.Command command3 = this.m_Commands[index3];
              if (command3.m_Prefab == command1.m_Prefab && (((command1.m_Flags | command3.m_Flags) & IconFlags.IgnoreTarget) != (IconFlags) 0 || command3.m_Target == command1.m_Target) && (!flag || (command3.m_CommandFlags & IconCommandBuffer.CommandFlags.Remove) != (IconCommandBuffer.CommandFlags) 0))
                goto label_96;
            }
          }
          if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Add) != (IconCommandBuffer.CommandFlags) 0)
          {
            if (!flag || command1.m_ClusterLayer == IconClusterLayer.Transaction)
            {
              for (int index4 = 0; index4 < num; ++index4)
              {
                if (command1.m_Priority == (IconPriority) int2Ptr[index4].x && command1.m_BufferIndex != int2Ptr[index4].y)
                  goto label_96;
              }
              // ISSUE: reference to a compiler-generated method
              Icon iconData = this.GetIconData(command1);
              if (command1.m_ClusterLayer != IconClusterLayer.Transaction)
              {
                if (iconElements1.IsCreated)
                {
                  // ISSUE: reference to a compiler-generated method
                  int icon1 = this.FindIcon(iconElements1, command1);
                  if (icon1 >= 0)
                  {
                    Entity icon2 = iconElements1[icon1].m_Icon;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_DeletedData.HasComponent(icon2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Deleted>(icon2);
                    }
                    // ISSUE: reference to a compiler-generated field
                    Icon other = this.m_IconData[icon2];
                    iconData.m_ClusterIndex = other.m_ClusterIndex;
                    if (!iconData.Equals(other))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconData[icon2] = iconData;
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(icon2, new Updated());
                    }
                    if (command1.m_Target != Entity.Null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_TargetData.HasComponent(icon2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.AddComponent<Game.Common.Target>(icon2, new Game.Common.Target(command1.m_Target));
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_TargetData[icon2].m_Target != command1.m_Target)
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.SetComponent<Game.Common.Target>(icon2, new Game.Common.Target(command1.m_Target));
                        }
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Game.Common.Target>(icon2);
                    }
                    if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Temp) != (IconCommandBuffer.CommandFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated method
                      Temp tempData = this.GetTempData(command1);
                      // ISSUE: reference to a compiler-generated field
                      if (tempData.m_Flags != this.m_TempData[icon2].m_Flags)
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.SetComponent<Temp>(icon2, tempData);
                      }
                    }
                    if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Hidden) != (IconCommandBuffer.CommandFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_HiddenData.HasComponent(icon2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.AddComponent<Hidden>(icon2, new Hidden());
                        continue;
                      }
                      continue;
                    }
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_HiddenData.HasComponent(icon2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Hidden>(icon2);
                      continue;
                    }
                    continue;
                  }
                }
                else if (bufferData.IsCreated)
                {
                  // ISSUE: reference to a compiler-generated method
                  int icon3 = this.FindIcon(bufferData, command1);
                  if (icon3 >= 0)
                  {
                    Entity icon4 = bufferData[icon3].m_Icon;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_DeletedData.HasComponent(icon4))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Deleted>(icon4);
                    }
                    // ISSUE: reference to a compiler-generated field
                    Icon other = this.m_IconData[icon4];
                    iconData.m_ClusterIndex = other.m_ClusterIndex;
                    if (!iconData.Equals(other))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconData[icon4] = iconData;
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(icon4, new Updated());
                    }
                    if (command1.m_Target != Entity.Null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_TargetData.HasComponent(icon4))
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.AddComponent<Game.Common.Target>(icon4, new Game.Common.Target(command1.m_Target));
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_TargetData[icon4].m_Target != command1.m_Target)
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.SetComponent<Game.Common.Target>(icon4, new Game.Common.Target(command1.m_Target));
                        }
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Game.Common.Target>(icon4);
                    }
                    if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Temp) != (IconCommandBuffer.CommandFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated method
                      Temp tempData = this.GetTempData(command1);
                      // ISSUE: reference to a compiler-generated field
                      if (tempData.m_Flags != this.m_TempData[icon4].m_Flags)
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.SetComponent<Temp>(icon4, tempData);
                      }
                    }
                    if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Hidden) != (IconCommandBuffer.CommandFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_HiddenData.HasComponent(icon4))
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.AddComponent<Hidden>(icon4, new Hidden());
                        continue;
                      }
                      continue;
                    }
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_HiddenData.HasComponent(icon4))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Hidden>(icon4);
                      continue;
                    }
                    continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  iconElements1 = this.m_CommandBuffer.SetBuffer<IconElement>(owner);
                  iconElements1.CopyFrom(bufferData);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  iconElements1 = this.m_CommandBuffer.AddBuffer<IconElement>(owner);
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(this.m_NotificationIconData[command1.m_Prefab].m_Archetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef(command1.m_Prefab));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Icon>(entity, iconData);
              if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Temp) != (IconCommandBuffer.CommandFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                Temp tempData = this.GetTempData(command1);
                if (tempData.m_Original != Entity.Null || (command1.m_CommandFlags & IconCommandBuffer.CommandFlags.DisallowCluster) != (IconCommandBuffer.CommandFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<DisallowCluster>(entity, new DisallowCluster());
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Temp>(entity, tempData);
              }
              else
              {
                if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.DisallowCluster) != (IconCommandBuffer.CommandFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<DisallowCluster>(entity, new DisallowCluster());
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<IconAnimationElement> iconAnimation = this.m_IconAnimations[this.m_ConfigurationEntity];
                // ISSUE: reference to a compiler-generated method
                AnimationType appearAnimation = this.GetAppearAnimation(command1.m_ClusterLayer);
                float duration = iconAnimation[(int) appearAnimation].m_Duration;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Animation>(entity, new Animation(appearAnimation, this.m_DeltaTime - command1.m_Delay, duration));
              }
              if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Hidden) != (IconCommandBuffer.CommandFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Hidden>(entity, new Hidden());
              }
              if (command1.m_Target != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Game.Common.Target>(entity, new Game.Common.Target(command1.m_Target));
              }
              if (command1.m_ClusterLayer != IconClusterLayer.Transaction)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Owner>(entity, new Owner(owner));
                iconElements1.Add(new IconElement(entity));
              }
            }
          }
          else if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Remove) != (IconCommandBuffer.CommandFlags) 0)
          {
            DynamicBuffer<IconElement> iconElements2 = iconElements1.IsCreated ? iconElements1 : bufferData;
            if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.All) != (IconCommandBuffer.CommandFlags) 0)
            {
              if (iconElements2.IsCreated)
              {
                for (int index5 = 0; index5 < iconElements2.Length; ++index5)
                {
                  Entity icon = iconElements2[index5].m_Icon;
                  // ISSUE: reference to a compiler-generated field
                  if (icon.Index < 0 || this.m_IconData[icon].m_Priority != command1.m_Priority)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.DeleteIcon(icon);
                    iconElements2.RemoveAt(index5--);
                  }
                }
              }
              if (num < 16)
                int2Ptr[num++] = new int2((int) command1.m_Priority, command1.m_BufferIndex);
            }
            else if (iconElements2.IsCreated)
            {
              // ISSUE: reference to a compiler-generated method
              int icon = this.FindIcon(iconElements2, command1);
              if (icon != -1)
              {
                // ISSUE: reference to a compiler-generated method
                this.DeleteIcon(iconElements2[icon].m_Icon);
                iconElements2.RemoveAt(icon);
              }
            }
          }
          else if ((command1.m_CommandFlags & IconCommandBuffer.CommandFlags.Update) != (IconCommandBuffer.CommandFlags) 0)
          {
            DynamicBuffer<IconElement> dynamicBuffer = iconElements1.IsCreated ? iconElements1 : bufferData;
            if (dynamicBuffer.IsCreated)
            {
              for (int index6 = 0; index6 < dynamicBuffer.Length; ++index6)
              {
                Entity icon5 = dynamicBuffer[index6].m_Icon;
                if (icon5.Index >= 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  Icon icon6 = this.m_IconData[icon5];
                  if ((icon6.m_Flags & IconFlags.CustomLocation) == (IconFlags) 0)
                  {
                    float3 location = icon6.m_Location;
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    icon6.m_Location = (command1.m_Flags & IconFlags.TargetLocation) == (IconFlags) 0 ? this.FindLocation(command1.m_Owner) : this.FindLocation(command1.m_Target);
                    if (!location.Equals(icon6.m_Location))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconData[icon5] = icon6;
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(icon5, new Updated());
                    }
                  }
                }
              }
            }
          }
        }
        if (!bufferData.IsCreated || iconElements1.IsCreated || bufferData.Length != 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<IconElement>(owner);
      }

      private void DeleteIcon(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        if (entity.Index < 0 || this.m_TempData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(entity, new Deleted());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<IconAnimationElement> iconAnimation = this.m_IconAnimations[this.m_ConfigurationEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          AnimationType resolveAnimation = this.GetResolveAnimation(this.m_IconData[entity].m_ClusterLayer);
          float duration = iconAnimation[(int) resolveAnimation].m_Duration;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Animation>(entity, new Animation(resolveAnimation, this.m_DeltaTime, duration));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        }
      }

      private AnimationType GetAppearAnimation(IconClusterLayer layer)
      {
        switch (layer)
        {
          case IconClusterLayer.Default:
            return AnimationType.WarningAppear;
          case IconClusterLayer.Marker:
            return AnimationType.MarkerAppear;
          case IconClusterLayer.Transaction:
            return AnimationType.Transaction;
          default:
            return AnimationType.WarningAppear;
        }
      }

      private AnimationType GetResolveAnimation(IconClusterLayer layer)
      {
        return layer == IconClusterLayer.Default || layer != IconClusterLayer.Marker ? AnimationType.WarningResolve : AnimationType.MarkerDisappear;
      }

      private Temp GetTempData(IconCommandBuffer.Command command)
      {
        Temp tempData = new Temp();
        Temp componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.TryGetComponent(command.m_Owner, out componentData))
        {
          tempData.m_Flags |= componentData.m_Flags;
          DynamicBuffer<IconElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_IconElements.TryGetBuffer(componentData.m_Original, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            int icon = this.FindIcon(bufferData, command);
            if (icon >= 0)
              tempData.m_Original = bufferData[icon].m_Icon;
          }
        }
        return tempData;
      }

      private Icon GetIconData(IconCommandBuffer.Command command)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return new Icon()
        {
          m_Location = (command.m_Flags & IconFlags.CustomLocation) == (IconFlags) 0 ? ((command.m_Flags & IconFlags.TargetLocation) == (IconFlags) 0 ? this.FindLocation(command.m_Owner) : this.FindLocation(command.m_Target)) : command.m_Location,
          m_Priority = command.m_Priority,
          m_ClusterLayer = command.m_ClusterLayer,
          m_Flags = command.m_Flags
        };
      }

      private float3 FindLocation(Entity entity)
      {
        float3 location = new float3();
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Entity connected = this.m_ConnectedData[entity].m_Connected;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(connected))
            entity = connected;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentBuildingData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            entity = this.m_CurrentBuildingData[entity].m_CurrentBuilding;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              entity = this.m_OwnerData[entity].m_Owner;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentTransportData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              entity = this.m_CurrentTransportData[entity].m_CurrentTransport;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentVehicleData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.m_CurrentVehicleData[entity].m_Vehicle;
        }
        Game.Objects.Transform componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.TryGetComponent(entity, out componentData1))
        {
          location = componentData1.m_Position;
          PrefabRef componentData2;
          ObjectGeometryData componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.TryGetComponent(entity, out componentData2) && this.m_ObjectGeometryData.TryGetComponent(componentData2.m_Prefab, out componentData3))
          {
            if ((componentData3.m_Flags & Game.Objects.GeometryFlags.Marker) == Game.Objects.GeometryFlags.None)
            {
              Bounds3 bounds = ObjectUtils.CalculateBounds(componentData1.m_Position, componentData1.m_Rotation, componentData3);
              location.y = bounds.max.y;
            }
            Destroyed componentData4;
            // ISSUE: reference to a compiler-generated field
            if ((componentData3.m_Flags & (Game.Objects.GeometryFlags.Physical | Game.Objects.GeometryFlags.HasLot)) == (Game.Objects.GeometryFlags.Physical | Game.Objects.GeometryFlags.HasLot) && this.m_DestroyedData.TryGetComponent(entity, out componentData4) && (double) componentData4.m_Cleared >= 0.0)
              location.y = componentData1.m_Position.y + 5f;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            location = this.m_NodeData[entity].m_Position;
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[entity];
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetGeometryData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              location.y += this.m_NetGeometryData[prefabRef.m_Prefab].m_DefaultSurfaceHeight.max;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              location = MathUtils.Position(this.m_CurveData[entity].m_Bezier, 0.5f);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PositionData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                location = this.m_PositionData[entity].m_Position;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_RouteWaypoints.HasBuffer(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[entity];
                  if (routeWaypoint.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    location = this.m_PositionData[routeWaypoint[0].m_Waypoint].m_Position;
                  }
                }
              }
            }
          }
        }
        return location;
      }

      private int FindIcon(
        DynamicBuffer<IconElement> iconElements,
        IconCommandBuffer.Command command)
      {
        for (int index = 0; index < iconElements.Length; ++index)
        {
          Entity icon1 = iconElements[index].m_Icon;
          // ISSUE: reference to a compiler-generated field
          if (icon1.Index >= 0 && !(this.m_PrefabRefData[icon1].m_Prefab != command.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            Icon icon2 = this.m_IconData[icon1];
            if ((command.m_Flags & IconFlags.IgnoreTarget) == (IconFlags) 0 && (icon2.m_Flags & IconFlags.IgnoreTarget) == (IconFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TargetData.HasComponent(icon1))
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TargetData[icon1].m_Target != command.m_Target)
                  continue;
              }
              else if (command.m_Target != Entity.Null)
                continue;
            }
            if (((command.m_Flags ^ icon2.m_Flags) & IconFlags.SecondaryLocation) == (IconFlags) 0)
              return index;
          }
        }
        return -1;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NotificationIconData> __Game_Prefabs_NotificationIconData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<IconAnimationElement> __Game_Prefabs_IconAnimationElement_RO_BufferLookup;
      public ComponentLookup<Icon> __Game_Notifications_Icon_RW_ComponentLookup;
      public BufferLookup<IconElement> __Game_Notifications_IconElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconData_RO_ComponentLookup = state.GetComponentLookup<NotificationIconData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IconAnimationElement_RO_BufferLookup = state.GetBufferLookup<IconAnimationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RW_ComponentLookup = state.GetComponentLookup<Icon>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RW_BufferLookup = state.GetBufferLookup<IconElement>();
      }
    }
  }
}
