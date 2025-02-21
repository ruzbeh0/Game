// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TempNotificationTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class TempNotificationTooltipSystem : TooltipSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_TempQuery;
    private NativeParallelHashMap<Entity, IconPriority> m_Priorities;
    private NativeList<TempNotificationTooltipSystem.ItemInfo> m_Items;
    private List<StringTooltip> m_Tooltips;
    private TempNotificationTooltipSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<Icon>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_Priorities = new NativeParallelHashMap<Entity, IconPriority>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Items = new NativeList<TempNotificationTooltipSystem.ItemInfo>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltips = new List<StringTooltip>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Priorities.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Items.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.CompleteDependency();
      // ISSUE: reference to a compiler-generated field
      this.m_Priorities.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Items.Clear();
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Owner> componentTypeHandle2 = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Icon> componentTypeHandle3 = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        foreach (ArchetypeChunk archetypeChunk in archetypeChunkArray)
        {
          NativeArray<Temp> nativeArray1 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle1);
          NativeArray<Icon> nativeArray2 = archetypeChunk.GetNativeArray<Icon>(ref componentTypeHandle3);
          NativeArray<Owner> nativeArray3 = archetypeChunk.GetNativeArray<Owner>(ref componentTypeHandle2);
          NativeArray<PrefabRef> nativeArray4 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle4);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Temp temp = nativeArray1[index];
            Icon icon = nativeArray2[index];
            PrefabRef prefabRef = nativeArray4[index];
            Temp component;
            // ISSUE: reference to a compiler-generated method
            if (icon.m_ClusterLayer != IconClusterLayer.Marker && (nativeArray3.Length == 0 || !this.EntityManager.TryGetComponent<Temp>(nativeArray3[index].m_Owner, out component) || !this.HasIcon(component.m_Original, prefabRef.m_Prefab, icon.m_Priority)) && (temp.m_Flags & (TempFlags.Dragging | TempFlags.Select)) != TempFlags.Select)
            {
              IconPriority iconPriority;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Priorities.TryGetValue(prefabRef.m_Prefab, out iconPriority))
              {
                if (icon.m_Priority > iconPriority)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_Priorities[prefabRef.m_Prefab] = icon.m_Priority;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Priorities.TryAdd(prefabRef.m_Prefab, icon.m_Priority);
              }
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValue<Entity, IconPriority> priority in this.m_Priorities)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_Items.Add(new TempNotificationTooltipSystem.ItemInfo()
        {
          m_Prefab = priority.Key,
          m_Priority = priority.Value
        });
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Items.Sort<TempNotificationTooltipSystem.ItemInfo>();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Items.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        TempNotificationTooltipSystem.ItemInfo itemInfo = this.m_Items[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NotificationIconPrefab prefab = this.m_PrefabSystem.GetPrefab<NotificationIconPrefab>(itemInfo.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        if (this.m_Tooltips.Count <= index)
        {
          // ISSUE: reference to a compiler-generated field
          List<StringTooltip> tooltips = this.m_Tooltips;
          StringTooltip stringTooltip = new StringTooltip();
          stringTooltip.path = (PathSegment) string.Format("notification{0}", (object) index);
          tooltips.Add(stringTooltip);
        }
        // ISSUE: reference to a compiler-generated field
        StringTooltip tooltip = this.m_Tooltips[index];
        UIObject component;
        if (prefab.TryGet<UIObject>(out component) && !string.IsNullOrEmpty(component.m_Icon))
          tooltip.icon = component.m_Icon;
        else
          tooltip.icon = (string) null;
        tooltip.value = LocalizedString.Id("Notifications.TITLE[" + prefab.name + "]");
        // ISSUE: reference to a compiler-generated field
        tooltip.color = NotificationTooltip.GetColor(itemInfo.m_Priority);
        this.AddMouseTooltip((IWidget) tooltip);
      }
    }

    private bool HasIcon(Entity entity, Entity prefab, IconPriority minPriority)
    {
      DynamicBuffer<IconElement> buffer;
      if (this.EntityManager.TryGetBuffer<IconElement>(entity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity icon = buffer[index].m_Icon;
          EntityManager entityManager = this.EntityManager;
          Icon componentData = entityManager.GetComponentData<Icon>(icon);
          entityManager = this.EntityManager;
          if (entityManager.GetComponentData<PrefabRef>(icon).m_Prefab == prefab && componentData.m_Priority >= minPriority)
            return true;
        }
      }
      return false;
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

    [Preserve]
    public TempNotificationTooltipSystem()
    {
    }

    private struct ItemInfo : IComparable<TempNotificationTooltipSystem.ItemInfo>
    {
      public Entity m_Prefab;
      public IconPriority m_Priority;

      public int CompareTo(TempNotificationTooltipSystem.ItemInfo other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return -this.m_Priority.CompareTo((object) other.m_Priority);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
      }
    }
  }
}
