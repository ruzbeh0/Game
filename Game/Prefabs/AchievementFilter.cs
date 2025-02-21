// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AchievementFilter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Achievements/", new Type[] {typeof (BuildingPrefab)})]
  public class AchievementFilter : ComponentBase
  {
    public AchievementId[] m_ValidFor;
    public AchievementId[] m_NotValidFor;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<AchievementFilterData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      DynamicBuffer<AchievementFilterData> buffer = entityManager.GetBuffer<AchievementFilterData>(entity);
      AchievementFilterData achievementFilterData;
      if (this.m_ValidFor != null)
      {
        for (int index = 0; index < this.m_ValidFor.Length; ++index)
        {
          ref DynamicBuffer<AchievementFilterData> local = ref buffer;
          achievementFilterData = new AchievementFilterData();
          achievementFilterData.m_AchievementID = this.m_ValidFor[index];
          achievementFilterData.m_Allow = true;
          AchievementFilterData elem = achievementFilterData;
          local.Add(elem);
        }
      }
      if (this.m_NotValidFor == null)
        return;
      for (int index = 0; index < this.m_NotValidFor.Length; ++index)
      {
        ref DynamicBuffer<AchievementFilterData> local = ref buffer;
        achievementFilterData = new AchievementFilterData();
        achievementFilterData.m_AchievementID = this.m_NotValidFor[index];
        achievementFilterData.m_Allow = false;
        AchievementFilterData elem = achievementFilterData;
        local.Add(elem);
      }
    }
  }
}
