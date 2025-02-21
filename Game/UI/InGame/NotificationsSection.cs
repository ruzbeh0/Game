// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.NotificationsSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Notifications;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class NotificationsSection : InfoSectionBase
  {
    private ImageSystem m_ImageSystem;
    private EntityQuery m_CitizenQuery;
    private NativeList<Notification> m_NotificationsResult;
    private NativeArray<bool> m_DisplayResult;
    private NotificationsSection.TypeHandle __TypeHandle;

    protected override string group => nameof (NotificationsSection);

    protected override bool displayForDestroyedObjects => true;

    protected override bool displayForOutsideConnections => true;

    protected override bool displayForUnderConstruction => true;

    protected override bool displayForUpgrades => true;

    private List<NotificationInfo> notifications { get; set; }

    protected override void Reset()
    {
      this.notifications.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_NotificationsResult.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_DisplayResult[0] = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<CurrentBuilding>(),
          ComponentType.ReadOnly<IconElement>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DisplayResult = new NativeArray<bool>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_NotificationsResult = new NativeList<Notification>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.notifications = new List<NotificationInfo>(10);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NotificationsResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DisplayResult.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      new NotificationsSection.CheckAndCacheNotificationsJob()
      {
        m_Entity = this.selectedEntity,
        m_CitizenDataFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_IconDataFromEntity = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_IconElementBufferFromEntity = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup,
        m_EmployeeBufferFromEntity = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_RenterBufferFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_HouseholdCitizenBufferFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_RouteWaypointBufferFromEntity = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_DisplayResult = this.m_DisplayResult,
        m_NotificationResult = this.m_NotificationsResult
      }.Schedule<NotificationsSection.CheckAndCacheNotificationsJob>(this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new NotificationsSection.CheckAndCacheVisitorNotificationsJob()
      {
        m_Entity = this.selectedEntity,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentBuildingTypeHandle = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_IconElementBufferFromEntity = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup,
        m_IconDataFromEntity = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup,
        m_DisplayResult = this.m_DisplayResult,
        m_NotificationResult = this.m_NotificationsResult
      }.Schedule<NotificationsSection.CheckAndCacheVisitorNotificationsJob>(this.m_CitizenQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      this.visible = this.m_DisplayResult[0];
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_NotificationsResult.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        NotificationInfo notificationInfo = new NotificationInfo(this.m_NotificationsResult[index1]);
        bool flag = false;
        for (int index2 = 0; index2 < this.notifications.Count; ++index2)
        {
          if (this.notifications[index2].entity == notificationInfo.entity)
          {
            if (this.EntityManager.HasComponent<Building>(this.selectedEntity))
              this.notifications[index2].AddTarget(notificationInfo.target);
            flag = true;
          }
        }
        if (!flag)
          this.notifications.Add(notificationInfo);
      }
      this.notifications.Sort();
    }

    public static bool HasNotifications(
      Entity entity,
      BufferLookup<IconElement> iconBuffer,
      ComponentLookup<Icon> iconDataFromEntity)
    {
      if (iconBuffer.HasBuffer(entity))
      {
        DynamicBuffer<IconElement> dynamicBuffer = iconBuffer[entity];
        for (int index = 0; index < dynamicBuffer.Length; ++index)
        {
          Entity icon = dynamicBuffer[index].m_Icon;
          if (iconDataFromEntity.HasComponent(icon) && iconDataFromEntity[icon].m_ClusterLayer != IconClusterLayer.Marker)
            return true;
        }
      }
      return false;
    }

    public static NativeList<Notification> GetNotifications(
      EntityManager EntityManager,
      Entity entity,
      NativeList<Notification> notifications)
    {
      DynamicBuffer<IconElement> buffer;
      if (EntityManager.TryGetBuffer<IconElement>(entity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity icon = buffer[index].m_Icon;
          Icon component1;
          PrefabRef component2;
          if (EntityManager.TryGetComponent<Icon>(icon, out component1) && component1.m_ClusterLayer != IconClusterLayer.Marker && EntityManager.TryGetComponent<PrefabRef>(icon, out component2))
            notifications.Add(new Notification(component2.m_Prefab, entity, component1.m_Priority));
        }
      }
      return notifications;
    }

    public static NativeList<Notification> GetNotifications(
      Entity entity,
      ComponentLookup<PrefabRef> prefabRefDataFromEntity,
      ComponentLookup<Icon> iconDataFromEntity,
      BufferLookup<IconElement> iconBufferFromEntity,
      NativeList<Notification> notifications)
    {
      if (iconBufferFromEntity.HasBuffer(entity))
      {
        DynamicBuffer<IconElement> dynamicBuffer = iconBufferFromEntity[entity];
        for (int index = 0; index < dynamicBuffer.Length; ++index)
        {
          Entity icon1 = dynamicBuffer[index].m_Icon;
          if (iconDataFromEntity.HasComponent(icon1))
          {
            Icon icon2 = iconDataFromEntity[icon1];
            if (icon2.m_ClusterLayer != IconClusterLayer.Marker && prefabRefDataFromEntity.HasComponent(icon1))
            {
              PrefabRef prefabRef = prefabRefDataFromEntity[icon1];
              notifications.Add(new Notification(prefabRef.m_Prefab, entity, icon2.m_Priority));
            }
          }
        }
      }
      return notifications;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("notifications");
      writer.ArrayBegin(this.notifications.Count);
      for (int index = 0; index < this.notifications.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(this.notifications[index].entity);
        writer.TypeBegin(typeof (Notification).FullName);
        writer.PropertyName("key");
        writer.Write(prefab.name);
        writer.PropertyName("count");
        writer.Write(this.notifications[index].count);
        writer.PropertyName("iconPath");
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        writer.Write(ImageSystem.GetIcon(prefab) ?? this.m_ImageSystem.placeholderIcon);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
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
    public NotificationsSection()
    {
    }

    [BurstCompile]
    private struct CheckAndCacheNotificationsJob : IJob
    {
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconDataFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      [ReadOnly]
      public BufferLookup<IconElement> m_IconElementBufferFromEntity;
      [ReadOnly]
      public BufferLookup<Employee> m_EmployeeBufferFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterBufferFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenBufferFromEntity;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypointBufferFromEntity;
      public NativeArray<bool> m_DisplayResult;
      public NativeList<Notification> m_NotificationResult;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_IconDataFromEntity.HasComponent(this.m_Entity) || this.m_CitizenDataFromEntity.HasComponent(this.m_Entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (NotificationsSection.HasNotifications(this.m_Entity, this.m_IconElementBufferFromEntity, this.m_IconDataFromEntity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_DisplayResult[0] = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NotificationResult = NotificationsSection.GetNotifications(this.m_Entity, this.m_PrefabRefDataFromEntity, this.m_IconDataFromEntity, this.m_IconElementBufferFromEntity, this.m_NotificationResult);
        }
        DynamicBuffer<Employee> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_EmployeeBufferFromEntity.TryGetBuffer(this.m_Entity, out bufferData1))
        {
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (NotificationsSection.HasNotifications(bufferData1[index].m_Worker, this.m_IconElementBufferFromEntity, this.m_IconDataFromEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_DisplayResult[0] = true;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_NotificationResult = NotificationsSection.GetNotifications(bufferData1[index].m_Worker, this.m_PrefabRefDataFromEntity, this.m_IconDataFromEntity, this.m_IconElementBufferFromEntity, this.m_NotificationResult);
            }
          }
        }
        DynamicBuffer<Renter> bufferData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_RenterBufferFromEntity.TryGetBuffer(this.m_Entity, out bufferData2))
        {
          for (int index1 = 0; index1 < bufferData2.Length; ++index1)
          {
            DynamicBuffer<HouseholdCitizen> bufferData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdCitizenBufferFromEntity.TryGetBuffer(bufferData2[index1].m_Renter, out bufferData3))
            {
              for (int index2 = 0; index2 < bufferData3.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (NotificationsSection.HasNotifications(bufferData3[index2].m_Citizen, this.m_IconElementBufferFromEntity, this.m_IconDataFromEntity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_DisplayResult[0] = true;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.m_NotificationResult = NotificationsSection.GetNotifications(bufferData3[index2].m_Citizen, this.m_PrefabRefDataFromEntity, this.m_IconDataFromEntity, this.m_IconElementBufferFromEntity, this.m_NotificationResult);
                }
              }
            }
            DynamicBuffer<Employee> bufferData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EmployeeBufferFromEntity.TryGetBuffer(bufferData2[index1].m_Renter, out bufferData4))
            {
              for (int index3 = 0; index3 < bufferData4.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (NotificationsSection.HasNotifications(bufferData4[index3].m_Worker, this.m_IconElementBufferFromEntity, this.m_IconDataFromEntity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_DisplayResult[0] = true;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.m_NotificationResult = NotificationsSection.GetNotifications(bufferData4[index3].m_Worker, this.m_PrefabRefDataFromEntity, this.m_IconDataFromEntity, this.m_IconElementBufferFromEntity, this.m_NotificationResult);
                }
              }
            }
          }
        }
        DynamicBuffer<RouteWaypoint> bufferData5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_RouteWaypointBufferFromEntity.TryGetBuffer(this.m_Entity, out bufferData5))
          return;
        for (int index = 0; index < bufferData5.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (NotificationsSection.HasNotifications(bufferData5[index].m_Waypoint, this.m_IconElementBufferFromEntity, this.m_IconDataFromEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DisplayResult[0] = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_NotificationResult = NotificationsSection.GetNotifications(bufferData5[index].m_Waypoint, this.m_PrefabRefDataFromEntity, this.m_IconDataFromEntity, this.m_IconElementBufferFromEntity, this.m_NotificationResult);
          }
        }
      }
    }

    [BurstCompile]
    private struct CheckAndCacheVisitorNotificationsJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingTypeHandle;
      [ReadOnly]
      public BufferLookup<IconElement> m_IconElementBufferFromEntity;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconDataFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      public NativeArray<bool> m_DisplayResult;
      public NativeList<Notification> m_NotificationResult;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray2 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingTypeHandle);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (nativeArray2[index].m_CurrentBuilding == this.m_Entity && NotificationsSection.HasNotifications(nativeArray1[index], this.m_IconElementBufferFromEntity, this.m_IconDataFromEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DisplayResult[0] = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_NotificationResult = NotificationsSection.GetNotifications(nativeArray1[index], this.m_PrefabRefDataFromEntity, this.m_IconDataFromEntity, this.m_IconElementBufferFromEntity, this.m_NotificationResult);
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
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Icon> __Game_Notifications_Icon_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<IconElement> __Game_Notifications_IconElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentLookup = state.GetComponentLookup<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferLookup = state.GetBufferLookup<IconElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RW_ComponentLookup = state.GetComponentLookup<PrefabRef>();
      }
    }
  }
}
