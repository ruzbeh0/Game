// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StatisticTriggerPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Triggers;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Triggers/", new System.Type[] {})]
  public class StatisticTriggerPrefab : PrefabBase
  {
    public StatisticTriggerType m_Type;
    public StatisticsPrefab m_StatisticPrefab;
    public int m_StatisticParameter;
    public StatisticsPrefab m_NormalizeWithPrefab;
    public int m_NormalizeWithParameter;
    public int m_TimeFrame = 1;
    public int m_MinSamples = 1;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if ((UnityEngine.Object) this.m_StatisticPrefab != (UnityEngine.Object) null)
        prefabs.Add((PrefabBase) this.m_StatisticPrefab);
      if (!((UnityEngine.Object) this.m_NormalizeWithPrefab != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_NormalizeWithPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TriggerData>());
      components.Add(ComponentType.ReadWrite<StatisticTriggerData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      StatisticTriggerData componentData = new StatisticTriggerData();
      componentData.m_Type = this.m_Type;
      if ((UnityEngine.Object) this.m_StatisticPrefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        componentData.m_StatisticEntity = systemManaged.GetEntity((PrefabBase) this.m_StatisticPrefab);
      }
      componentData.m_StatisticParameter = this.m_StatisticParameter;
      if ((UnityEngine.Object) this.m_NormalizeWithPrefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        componentData.m_NormalizeWithPrefab = systemManaged.GetEntity((PrefabBase) this.m_NormalizeWithPrefab);
      }
      componentData.m_NormalizeWithParameter = this.m_NormalizeWithParameter;
      componentData.m_TimeFrame = this.m_TimeFrame;
      componentData.m_MinSamples = this.m_MinSamples;
      if ((UnityEngine.Object) this.m_StatisticPrefab != (UnityEngine.Object) null && this.m_StatisticPrefab.m_CollectionType == StatisticCollectionType.Daily || (UnityEngine.Object) this.m_NormalizeWithPrefab != (UnityEngine.Object) null && this.m_NormalizeWithPrefab.m_CollectionType == StatisticCollectionType.Daily)
        componentData.m_MinSamples = math.max(componentData.m_MinSamples, 32 + math.max(0, this.m_TimeFrame - 1));
      entityManager.SetComponentData<StatisticTriggerData>(entity, componentData);
      entityManager.GetBuffer<TriggerData>(entity).Add(new TriggerData()
      {
        m_TriggerType = TriggerType.StatisticsValue,
        m_TargetTypes = TargetType.Nothing,
        m_TriggerPrefab = entity
      });
    }
  }
}
