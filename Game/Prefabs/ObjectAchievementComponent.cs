// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectAchievementComponent
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
  [ComponentMenu("Achievements/", new Type[] {})]
  public class ObjectAchievementComponent : ComponentBase
  {
    public ObjectAchievementComponent.ObjectAchievementSetup[] m_Achievements;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ObjectAchievement>());
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ObjectAchievementData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      DynamicBuffer<ObjectAchievementData> buffer = entityManager.GetBuffer<ObjectAchievementData>(entity);
      foreach (ObjectAchievementComponent.ObjectAchievementSetup achievement in this.m_Achievements)
        buffer.Add(new ObjectAchievementData()
        {
          m_ID = achievement.m_ID,
          m_BypassCounter = achievement.m_BypassCounter
        });
    }

    [Serializable]
    public struct ObjectAchievementSetup
    {
      public AchievementId m_ID;
      public bool m_BypassCounter;
    }
  }
}
