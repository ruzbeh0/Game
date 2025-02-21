// Decompiled with JetBrains decompiler
// Type: Game.Triggers.NotificationTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Triggers
{
  [CompilerGenerated]
  public class NotificationTriggerSystem : GameSystemBase
  {
    private TriggerSystem m_TriggerSystem;
    private EntityQuery m_CreatedNotificationsQuery;
    private EntityQuery m_DeletedNotificationsQuery;
    private EntityQuery m_AllNotificationsQuery;
    private NotificationTriggerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedNotificationsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Icon>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Applied>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedNotificationsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Icon>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllNotificationsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Icon>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      this.Enabled = false;
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NotificationTriggerSystem.TriggerJob jobData = new NotificationTriggerSystem.TriggerJob()
      {
        m_Created = this.m_CreatedNotificationsQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_CreatedPrefabRefs = this.m_CreatedNotificationsQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_Deleted = this.m_DeletedNotificationsQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_DeletedPrefabRefs = this.m_DeletedNotificationsQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_AllPrefabRefs = this.m_AllNotificationsQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_ActionQueue = this.m_TriggerSystem.CreateActionBuffer()
      };
      this.Dependency = jobData.Schedule<NotificationTriggerSystem.TriggerJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
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
    public NotificationTriggerSystem()
    {
    }

    [BurstCompile]
    private struct TriggerJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<Entity> m_Created;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<PrefabRef> m_CreatedPrefabRefs;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<Entity> m_Deleted;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<PrefabRef> m_DeletedPrefabRefs;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<PrefabRef> m_AllPrefabRefs;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Target> m_TargetData;
      public NativeQueue<TriggerAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Execute(this.m_Created, this.m_CreatedPrefabRefs, TriggerType.NewNotification);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Execute(this.m_Deleted, this.m_DeletedPrefabRefs, TriggerType.NotificationResolved);
      }

      private void Execute(
        NativeArray<Entity> entities,
        NativeArray<PrefabRef> prefabRefs,
        TriggerType triggerType)
      {
        for (int index = 0; index < entities.Length; ++index)
        {
          Owner componentData1;
          // ISSUE: reference to a compiler-generated field
          bool component1 = this.m_OwnerData.TryGetComponent(entities[index], out componentData1);
          Target componentData2;
          // ISSUE: reference to a compiler-generated field
          bool component2 = this.m_TargetData.TryGetComponent(entities[index], out componentData2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ActionQueue.Enqueue(new TriggerAction(triggerType, prefabRefs[index].m_Prefab, component1 ? componentData1.m_Owner : Entity.Null, component2 ? componentData2.m_Target : Entity.Null, (float) this.Count(prefabRefs[index].m_Prefab)));
        }
      }

      private int Count(Entity notification)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_AllPrefabRefs.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (notification == this.m_AllPrefabRefs[index].m_Prefab)
            ++num;
        }
        return num;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
      }
    }
  }
}
