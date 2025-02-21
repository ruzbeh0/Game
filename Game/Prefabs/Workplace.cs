// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Workplace
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Companies;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab)})]
  public class Workplace : ComponentBase, IServiceUpgrade
  {
    [Tooltip("The max amount of workers for City Service Buildings. ATTENTION: the other companies' max amount of workers changed dynamically, the max amount of workers of them depend on the m_MaxWorkersPerCell of each company prefab data")]
    public int m_Workplaces;
    [Tooltip("The minimum amount of workers of this workplace")]
    public int m_MinimumWorkersLimit;
    public WorkplaceComplexity m_Complexity;
    public float m_EveningShiftProbability;
    public float m_NightShiftProbability;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WorkplaceData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null) || this.m_Workplaces <= 0)
        return;
      components.Add(ComponentType.ReadWrite<WorkProvider>());
      components.Add(ComponentType.ReadWrite<Employee>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      if (this.m_Workplaces <= 0)
        return;
      components.Add(ComponentType.ReadWrite<WorkProvider>());
      components.Add(ComponentType.ReadWrite<Employee>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<WorkplaceData>(entity, new WorkplaceData()
      {
        m_MaxWorkers = this.m_Workplaces,
        m_MinimumWorkersLimit = this.m_MinimumWorkersLimit,
        m_Complexity = this.m_Complexity,
        m_EveningShiftProbability = this.m_EveningShiftProbability,
        m_NightShiftProbability = this.m_NightShiftProbability
      });
    }
  }
}
