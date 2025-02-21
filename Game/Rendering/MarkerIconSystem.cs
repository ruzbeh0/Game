// Decompiled with JetBrains decompiler
// Type: Game.Rendering.MarkerIconSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Citizens;
using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using Game.UI.InGame;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class MarkerIconSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private Entity m_SelectedMarker;
    private Entity m_FollowedMarker;
    private Entity m_SelectedLocation;
    private Entity m_FollowedLocation;
    private EntityQuery m_ConfigurationQuery;
    private EntityQuery m_IconQuery;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private ToolSystem m_ToolSystem;
    private MarkerIconSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<IconConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_IconQuery = this.GetEntityQuery(ComponentType.ReadOnly<Icon>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      Entity selected = this.m_ToolSystem.selected;
      // ISSUE: reference to a compiler-generated field
      int selectedIndex = this.m_ToolSystem.selectedIndex;
      Entity followedEntity = Entity.Null;
      int elementIndex = -1;
      // ISSUE: reference to a compiler-generated field
      if ((Object) this.m_CameraUpdateSystem.orbitCameraController != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        followedEntity = this.m_CameraUpdateSystem.orbitCameraController.followedEntity;
      }
      float3 position1 = new float3();
      float3 position2 = new float3();
      Entity location1 = Entity.Null;
      Entity location2 = Entity.Null;
      Bounds3 bounds1 = new Bounds3();
      Bounds3 bounds2 = new Bounds3();
      quaternion rotation;
      // ISSUE: reference to a compiler-generated method
      if (followedEntity != Entity.Null && !SelectedInfoUISystem.TryGetPosition(followedEntity, this.EntityManager, ref elementIndex, out location1, out position2, out bounds1, out rotation))
        location1 = Entity.Null;
      CurrentTransport component;
      if (followedEntity == selected && elementIndex == selectedIndex || this.EntityManager.TryGetComponent<CurrentTransport>(followedEntity, out component) && component.m_CurrentTransport == selected)
        selected = Entity.Null;
      // ISSUE: reference to a compiler-generated method
      if (selected != Entity.Null && (!SelectedInfoUISystem.TryGetPosition(selected, this.EntityManager, ref selectedIndex, out location2, out position1, out bounds2, out rotation) || location1 == location2))
        location2 = Entity.Null;
      if (followedEntity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        if (location1 != this.m_FollowedLocation)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RemoveMarker(ref this.m_FollowedMarker, location2 == this.m_FollowedLocation);
        }
        position2.y = bounds1.max.y;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateMarker(ref this.m_FollowedMarker, followedEntity, MarkerIconSystem.MarkerType.Followed, position2, this.m_SelectedLocation == location1);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.RemoveMarker(ref this.m_FollowedMarker, location2 == this.m_FollowedLocation);
      }
      if (selected != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        if (location2 != this.m_SelectedLocation)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RemoveMarker(ref this.m_SelectedMarker, location1 == this.m_SelectedLocation);
        }
        position1.y = bounds2.max.y;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateMarker(ref this.m_SelectedMarker, selected, MarkerIconSystem.MarkerType.Selected, position1, this.m_FollowedLocation == location2);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.RemoveMarker(ref this.m_SelectedMarker, location1 == this.m_SelectedLocation);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_FollowedLocation = location1;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedLocation = location2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_SelectedMarker != Entity.Null) && !(this.m_FollowedMarker != Entity.Null) || this.m_CameraUpdateSystem.activeCameraController == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AdjustLocations(position1, position2, (float3) this.m_CameraUpdateSystem.activeCameraController.position, (float3) (Quaternion.Euler(this.m_CameraUpdateSystem.activeCameraController.rotation) * Vector3.up));
    }

    private void UpdateMarker(
      ref Entity marker,
      Entity target,
      MarkerIconSystem.MarkerType markerType,
      float3 position,
      bool skipAnimation)
    {
      Owner component;
      if (this.EntityManager.HasComponent<Icon>(target) && this.EntityManager.TryGetComponent<Owner>(target, out component) && this.EntityManager.Exists(component.m_Owner))
        target = component.m_Owner;
      if (marker == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        marker = this.CreateMarker(target, position, markerType, skipAnimation);
      }
      else
      {
        Game.Common.Target componentData = this.EntityManager.GetComponentData<Game.Common.Target>(marker);
        if (!(componentData.m_Target != target))
          return;
        componentData.m_Target = target;
        this.EntityManager.SetComponentData<Game.Common.Target>(marker, componentData);
      }
    }

    private void RemoveMarker(ref Entity marker, bool skipAnimation)
    {
      if (!(marker != Entity.Null))
        return;
      // ISSUE: reference to a compiler-generated field
      if (skipAnimation || this.m_ConfigurationQuery.IsEmptyIgnoreFilter)
      {
        this.EntityManager.AddComponent<Deleted>(marker);
      }
      else
      {
        Game.Notifications.Animation component;
        if (this.EntityManager.TryGetComponent<Game.Notifications.Animation>(marker, out component))
        {
          if (component.m_Type != Game.Notifications.AnimationType.MarkerDisappear)
          {
            component.m_Type = Game.Notifications.AnimationType.MarkerDisappear;
            component.m_Timer = component.m_Duration - component.m_Timer;
            this.EntityManager.SetComponentData<Game.Notifications.Animation>(marker, component);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          float duration = this.EntityManager.GetBuffer<IconAnimationElement>(this.m_ConfigurationQuery.GetSingletonEntity(), true)[1].m_Duration;
          this.EntityManager.AddComponentData<Game.Notifications.Animation>(marker, new Game.Notifications.Animation(Game.Notifications.AnimationType.MarkerDisappear, UnityEngine.Time.deltaTime, duration));
        }
      }
      marker = Entity.Null;
    }

    private Entity CreateMarker(
      Entity target,
      float3 position,
      MarkerIconSystem.MarkerType markerType,
      bool skipAnimation)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ConfigurationQuery.IsEmptyIgnoreFilter)
        return Entity.Null;
      // ISSUE: reference to a compiler-generated field
      Entity singletonEntity = this.m_ConfigurationQuery.GetSingletonEntity();
      IconConfigurationData componentData1 = this.EntityManager.GetComponentData<IconConfigurationData>(singletonEntity);
      Entity entity1;
      switch (markerType)
      {
        case MarkerIconSystem.MarkerType.Selected:
          entity1 = componentData1.m_SelectedMarker;
          break;
        case MarkerIconSystem.MarkerType.Followed:
          entity1 = componentData1.m_FollowedMarker;
          break;
        default:
          return Entity.Null;
      }
      EntityManager entityManager = this.EntityManager;
      NotificationIconData componentData2 = entityManager.GetComponentData<NotificationIconData>(entity1);
      Icon componentData3 = new Icon();
      componentData3.m_Priority = IconPriority.Info;
      componentData3.m_Flags = IconFlags.Unique | IconFlags.OnTop;
      componentData3.m_Location = position;
      entityManager = this.EntityManager;
      Entity entity2 = entityManager.CreateEntity(componentData2.m_Archetype);
      entityManager = this.EntityManager;
      entityManager.SetComponentData<PrefabRef>(entity2, new PrefabRef(entity1));
      entityManager = this.EntityManager;
      entityManager.SetComponentData<Icon>(entity2, componentData3);
      entityManager = this.EntityManager;
      entityManager.AddComponentData<Game.Common.Target>(entity2, new Game.Common.Target(target));
      entityManager = this.EntityManager;
      entityManager.AddComponent<DisallowCluster>(entity2);
      if (!skipAnimation)
      {
        entityManager = this.EntityManager;
        float duration = entityManager.GetBuffer<IconAnimationElement>(singletonEntity, true)[0].m_Duration;
        entityManager = this.EntityManager;
        entityManager.AddComponentData<Game.Notifications.Animation>(entity2, new Game.Notifications.Animation(Game.Notifications.AnimationType.MarkerAppear, UnityEngine.Time.deltaTime, duration));
      }
      return entity2;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_SelectedMarker);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_FollowedMarker);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_SelectedMarker);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_FollowedMarker);
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedLocation = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_FollowedLocation = Entity.Null;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedMarker = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_FollowedMarker = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedLocation = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_FollowedLocation = Entity.Null;
    }

    private void AdjustLocations(
      float3 selectedLocation,
      float3 followedLocation,
      float3 cameraPos,
      float3 cameraUp)
    {
      NativeQueue<MarkerIconSystem.Overlap> nativeQueue = new NativeQueue<MarkerIconSystem.Overlap>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MarkerIconSystem.FindOverlapIconsJob jobData = new MarkerIconSystem.FindOverlapIconsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_IconType = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle,
        m_Entity1 = this.m_SelectedMarker,
        m_Entity2 = this.m_FollowedMarker,
        m_Location1 = selectedLocation,
        m_Location2 = followedLocation,
        m_OverlapQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      JobHandle inputDeps = new MarkerIconSystem.UpdateMarkerLocationJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_IconDisplayData = this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup,
        m_Entity1 = this.m_SelectedMarker,
        m_Entity2 = this.m_FollowedMarker,
        m_Location1 = selectedLocation,
        m_Location2 = followedLocation,
        m_CameraPos = cameraPos,
        m_CameraUp = cameraUp,
        m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentLookup,
        m_OverlapQueue = nativeQueue
      }.Schedule<MarkerIconSystem.UpdateMarkerLocationJob>(jobData.ScheduleParallel<MarkerIconSystem.FindOverlapIconsJob>(this.m_IconQuery, this.Dependency));
      nativeQueue.Dispose(inputDeps);
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
    public MarkerIconSystem()
    {
    }

    public enum MarkerType
    {
      Selected,
      Followed,
    }

    private struct Overlap
    {
      public Entity m_Entity;
      public Entity m_Other;

      public Overlap(Entity entity, Entity other)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Other = other;
      }
    }

    [BurstCompile]
    private struct FindOverlapIconsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Icon> m_IconType;
      [ReadOnly]
      public Entity m_Entity1;
      [ReadOnly]
      public Entity m_Entity2;
      [ReadOnly]
      public float3 m_Location1;
      [ReadOnly]
      public float3 m_Location2;
      public NativeQueue<MarkerIconSystem.Overlap>.ParallelWriter m_OverlapQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Icon> nativeArray2 = chunk.GetNativeArray<Icon>(ref this.m_IconType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Icon icon = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.distancesq(icon.m_Location, this.m_Location1) < 0.0099999997764825821 && nativeArray1[index] != this.m_Entity1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_OverlapQueue.Enqueue(new MarkerIconSystem.Overlap(this.m_Entity1, nativeArray1[index]));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.distancesq(icon.m_Location, this.m_Location2) < 0.0099999997764825821 && nativeArray1[index] != this.m_Entity2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_OverlapQueue.Enqueue(new MarkerIconSystem.Overlap(this.m_Entity2, nativeArray1[index]));
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

    [BurstCompile]
    private struct UpdateMarkerLocationJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> m_IconDisplayData;
      [ReadOnly]
      public Entity m_Entity1;
      [ReadOnly]
      public Entity m_Entity2;
      [ReadOnly]
      public float3 m_Location1;
      [ReadOnly]
      public float3 m_Location2;
      [ReadOnly]
      public float3 m_CameraPos;
      [ReadOnly]
      public float3 m_CameraUp;
      public ComponentLookup<Icon> m_IconData;
      public NativeQueue<MarkerIconSystem.Overlap> m_OverlapQueue;

      public void Execute()
      {
        Icon icon1 = new Icon();
        Icon icon2 = new Icon();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Entity1 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          icon1 = this.m_IconData[this.m_Entity1];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Entity2 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          icon2 = this.m_IconData[this.m_Entity2];
        }
        float x1 = 0.0f;
        float x2 = 0.0f;
        // ISSUE: variable of a compiler-generated type
        MarkerIconSystem.Overlap overlap;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OverlapQueue.TryDequeue(out overlap))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (overlap.m_Entity == this.m_Entity1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            x1 = math.max(x1, this.CalculateOffset(overlap.m_Other));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (overlap.m_Entity == this.m_Entity2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            x2 = math.max(x2, this.CalculateOffset(overlap.m_Other));
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        icon1.m_Location = this.m_Location1 + this.m_CameraUp * x1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        icon2.m_Location = this.m_Location2 + this.m_CameraUp * x2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Entity1 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_IconData[this.m_Entity1] = icon1;
        }
        // ISSUE: reference to a compiler-generated field
        if (!(this.m_Entity2 != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IconData[this.m_Entity2] = icon2;
      }

      private float CalculateOffset(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Icon icon = this.m_IconData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NotificationIconDisplayData notificationIconDisplayData = this.m_IconDisplayData[this.m_PrefabRefData[entity].m_Prefab];
        float s = (float) icon.m_Priority * 0.003921569f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return IconClusterSystem.IconCluster.CalculateRadius(math.lerp(notificationIconDisplayData.m_MinParams, notificationIconDisplayData.m_MaxParams, s).x, math.distance(icon.m_Location, this.m_CameraPos)) * 1.5f;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> __Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup;
      public ComponentLookup<Icon> __Game_Notifications_Icon_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup = state.GetComponentLookup<NotificationIconDisplayData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RW_ComponentLookup = state.GetComponentLookup<Icon>();
      }
    }
  }
}
