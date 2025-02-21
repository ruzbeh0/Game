// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WorkProviderParameterPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class WorkProviderParameterPrefab : PrefabBase
  {
    public NotificationIconPrefab m_UneducatedNotificationPrefab;
    public NotificationIconPrefab m_EducatedNotificationPrefab;
    [Tooltip("Delay in ticks for the 'missing uneducated workers' notification to appear (512 ticks per day)")]
    [Min(1f)]
    public short m_UneducatedNotificationDelay = 128;
    [Tooltip("Delay in ticks for the 'missing educated workers' notification to appear (512 ticks per day)")]
    [Min(1f)]
    public short m_EducatedNotificationDelay = 128;
    [Tooltip("Percentage of uneducated workers missing for the 'missing uneducated workers' notification to show up")]
    [Range(0.0f, 1f)]
    public float m_UneducatedNotificationLimit = 0.6f;
    [Tooltip("Percentage of educated workers missing for the 'missing educated workers' notification to show up")]
    [Range(0.0f, 1f)]
    public float m_EducatedNotificationLimit = 0.7f;
    public int m_SeniorEmployeeLevel = 3;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_UneducatedNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_EducatedNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<WorkProviderParameterData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<WorkProviderParameterData>(entity, new WorkProviderParameterData()
      {
        m_EducatedNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_EducatedNotificationPrefab),
        m_UneducatedNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_UneducatedNotificationPrefab),
        m_EducatedNotificationDelay = this.m_EducatedNotificationDelay,
        m_EducatedNotificationLimit = this.m_EducatedNotificationLimit,
        m_UneducatedNotificationDelay = this.m_UneducatedNotificationDelay,
        m_UneducatedNotificationLimit = this.m_UneducatedNotificationLimit,
        m_SeniorEmployeeLevel = this.m_SeniorEmployeeLevel
      });
    }
  }
}
