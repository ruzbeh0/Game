// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ElectricityParametersPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class ElectricityParametersPrefab : PrefabBase
  {
    [Tooltip("Initial charge of batteries")]
    [Range(0.0f, 1f)]
    public float m_InitialBatteryCharge = 0.1f;
    [Tooltip("Correlation between temperature (in °C) and electricity consumption")]
    public AnimationCurve m_TemperatureConsumptionMultiplier;
    [Tooltip("How much solar power plant electricity output is reduced when it is cloudy")]
    [Range(0.0f, 1f)]
    public float m_CloudinessSolarPenalty = 0.25f;
    public ServicePrefab m_ElectricityServicePrefab;
    public NotificationIconPrefab m_ElectricityNotificationPrefab;
    public NotificationIconPrefab m_LowVoltageNotConnectedPrefab;
    public NotificationIconPrefab m_HighVoltageNotConnectedPrefab;
    public NotificationIconPrefab m_BottleneckNotificationPrefab;
    public NotificationIconPrefab m_BuildingBottleneckNotificationPrefab;
    public NotificationIconPrefab m_NotEnoughProductionNotificationPrefab;
    public NotificationIconPrefab m_TransformerNotificationPrefab;
    public NotificationIconPrefab m_NotEnoughConnectedNotificationPrefab;
    public NotificationIconPrefab m_BatteryEmptyNotificationPrefab;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_ElectricityServicePrefab);
      prefabs.Add((PrefabBase) this.m_ElectricityNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_LowVoltageNotConnectedPrefab);
      prefabs.Add((PrefabBase) this.m_HighVoltageNotConnectedPrefab);
      prefabs.Add((PrefabBase) this.m_BottleneckNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_BuildingBottleneckNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_NotEnoughProductionNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_TransformerNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_NotEnoughConnectedNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_BatteryEmptyNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ElectricityParameterData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<ElectricityParameterData>(entity, new ElectricityParameterData()
      {
        m_InitialBatteryCharge = this.m_InitialBatteryCharge,
        m_TemperatureConsumptionMultiplier = new AnimationCurve1(this.m_TemperatureConsumptionMultiplier),
        m_CloudinessSolarPenalty = this.m_CloudinessSolarPenalty,
        m_ElectricityServicePrefab = systemManaged.GetEntity((PrefabBase) this.m_ElectricityServicePrefab),
        m_ElectricityNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_ElectricityNotificationPrefab),
        m_LowVoltageNotConnectedPrefab = systemManaged.GetEntity((PrefabBase) this.m_LowVoltageNotConnectedPrefab),
        m_HighVoltageNotConnectedPrefab = systemManaged.GetEntity((PrefabBase) this.m_HighVoltageNotConnectedPrefab),
        m_BottleneckNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_BottleneckNotificationPrefab),
        m_BuildingBottleneckNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_BuildingBottleneckNotificationPrefab),
        m_NotEnoughProductionNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_NotEnoughProductionNotificationPrefab),
        m_TransformerNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_TransformerNotificationPrefab),
        m_NotEnoughConnectedNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_NotEnoughConnectedNotificationPrefab),
        m_BatteryEmptyNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_BatteryEmptyNotificationPrefab)
      });
    }
  }
}
