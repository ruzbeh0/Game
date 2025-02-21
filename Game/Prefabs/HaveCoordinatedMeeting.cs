// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HaveCoordinatedMeeting
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Economy;
using Game.Events;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new System.Type[] {typeof (EventPrefab)})]
  public class HaveCoordinatedMeeting : ComponentBase
  {
    public CoordinatedMeetingPhase[] m_Phases;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<HaveCoordinatedMeetingData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CoordinatedMeeting>());
      components.Add(ComponentType.ReadWrite<CoordinatedMeetingAttendee>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
      components.Add(ComponentType.ReadWrite<PrefabRef>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Phases.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_Phases[index].m_Notification != (UnityEngine.Object) null)
          prefabs.Add(this.m_Phases[index].m_Notification);
      }
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<HaveCoordinatedMeetingData> buffer = entityManager.GetBuffer<HaveCoordinatedMeetingData>(entity);
      if (this.m_Phases == null)
        return;
      for (int index = 0; index < this.m_Phases.Length; ++index)
      {
        CoordinatedMeetingPhase phase = this.m_Phases[index];
        TravelPurpose travelPurpose = new TravelPurpose()
        {
          m_Purpose = phase.m_Purpose.m_Purpose,
          m_Data = phase.m_Purpose.m_Data,
          m_Resource = EconomyUtils.GetResource(phase.m_Purpose.m_Resource)
        };
        HaveCoordinatedMeetingData elem;
        elem.m_TravelPurpose = travelPurpose;
        elem.m_Delay = phase.m_Delay;
        // ISSUE: reference to a compiler-generated method
        elem.m_Notification = (UnityEngine.Object) phase.m_Notification != (UnityEngine.Object) null ? World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().GetEntity(phase.m_Notification) : Entity.Null;
        buffer.Add(elem);
      }
    }
  }
}
