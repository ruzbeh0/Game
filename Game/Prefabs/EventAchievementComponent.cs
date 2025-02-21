// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EventAchievementComponent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Game.Achievements;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Achievements/", new Type[] {typeof (EventPrefab)})]
  public class EventAchievementComponent : ComponentBase
  {
    public EventAchievementComponent.EventAchievementSetup[] m_Achievements;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<EventAchievement>());
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<EventAchievementData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      DynamicBuffer<EventAchievementData> buffer = entityManager.GetBuffer<EventAchievementData>(entity);
      for (int index = 0; index < this.m_Achievements.Length; ++index)
        buffer.Add(new EventAchievementData()
        {
          m_ID = this.m_Achievements[index].m_ID,
          m_FrameDelay = this.m_Achievements[index].m_FrameDelay,
          m_BypassCounter = this.m_Achievements[index].m_BypassCounter
        });
    }

    [Serializable]
    public struct EventAchievementSetup
    {
      public AchievementId m_ID;
      public uint m_FrameDelay;
      public bool m_BypassCounter;
    }
  }
}
