// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.StatusSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Citizens;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class StatusSection : InfoSectionBase
  {
    private ImageSystem m_ImageSystem;
    private bool m_Dead;

    protected override string group => nameof (StatusSection);

    private NativeList<CitizenCondition> conditions { get; set; }

    private NativeList<Notification> notifications { get; set; }

    private CitizenHappiness happiness { get; set; }

    protected override void Reset()
    {
      this.conditions.Clear();
      this.notifications.Clear();
      this.happiness = new CitizenHappiness();
      // ISSUE: reference to a compiler-generated field
      this.m_Dead = false;
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.conditions = new NativeList<CitizenCondition>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.notifications = new NativeList<Notification>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.conditions.Dispose();
      this.notifications.Dispose();
      base.OnDestroy();
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Citizen>(this.selectedEntity) && this.EntityManager.HasComponent<HouseholdMember>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Citizen componentData1 = this.EntityManager.GetComponentData<Citizen>(this.selectedEntity);
      HouseholdMember componentData2 = this.EntityManager.GetComponentData<HouseholdMember>(this.selectedEntity);
      this.happiness = CitizenUIUtils.GetCitizenHappiness(componentData1);
      this.conditions = CitizenUIUtils.GetCitizenConditions(this.EntityManager, this.selectedEntity, componentData1, componentData2, this.conditions);
      // ISSUE: reference to a compiler-generated method
      this.notifications = NotificationsSection.GetNotifications(this.EntityManager, this.selectedEntity, this.notifications);
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(this.selectedEntity, out component))
      {
        // ISSUE: reference to a compiler-generated method
        this.notifications = NotificationsSection.GetNotifications(this.EntityManager, component.m_CurrentTransport, this.notifications);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Dead = CitizenUtils.IsDead(this.EntityManager, this.selectedEntity);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("happiness");
      // ISSUE: reference to a compiler-generated field
      if (this.m_Dead)
        writer.WriteNull();
      else
        writer.Write<CitizenHappiness>(this.happiness);
      writer.PropertyName("conditions");
      // ISSUE: reference to a compiler-generated field
      if (this.m_Dead)
      {
        writer.WriteEmptyArray();
      }
      else
      {
        writer.ArrayBegin(this.conditions.Length);
        for (int index = 0; index < this.conditions.Length; ++index)
          writer.Write<CitizenCondition>(this.conditions[index]);
        writer.ArrayEnd();
      }
      writer.PropertyName("notifications");
      IJsonWriter writer1 = writer;
      NativeList<Notification> notifications = this.notifications;
      int length1 = notifications.Length;
      writer1.ArrayBegin(length1);
      int index1 = 0;
      while (true)
      {
        int num = index1;
        notifications = this.notifications;
        int length2 = notifications.Length;
        if (num < length2)
        {
          notifications = this.notifications;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          NotificationIconPrefab prefab = this.m_PrefabSystem.GetPrefab<NotificationIconPrefab>(notifications[index1].entity);
          writer.TypeBegin("selectedInfo.NotificationData");
          writer.PropertyName("key");
          writer.Write(prefab.name);
          writer.PropertyName("iconPath");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
          writer.TypeEnd();
          ++index1;
        }
        else
          break;
      }
      writer.ArrayEnd();
    }

    [Preserve]
    public StatusSection()
    {
    }
  }
}
