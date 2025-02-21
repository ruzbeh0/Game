// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterPipeParametersPrefab
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
  public class WaterPipeParametersPrefab : PrefabBase
  {
    public PrefabBase m_WaterService;
    public NotificationIconPrefab m_WaterNotification;
    public NotificationIconPrefab m_DirtyWaterNotification;
    public NotificationIconPrefab m_SewageNotification;
    public NotificationIconPrefab m_WaterPipeNotConnectedNotification;
    public NotificationIconPrefab m_SewagePipeNotConnectedNotification;
    public NotificationIconPrefab m_NotEnoughWaterCapacityNotification;
    public NotificationIconPrefab m_NotEnoughSewageCapacityNotification;
    public NotificationIconPrefab m_NotEnoughGroundwaterNotification;
    public NotificationIconPrefab m_NotEnoughSurfaceWaterNotification;
    public NotificationIconPrefab m_DirtyWaterPumpNotification;
    public float m_GroundwaterReplenish = 0.004f;
    [Tooltip("How much the groundwater cell purifies itself per tick (2048 ticks per day)")]
    public int m_GroundwaterPurification = 1;
    public float m_GroundwaterUsageMultiplier = 0.1f;
    public float m_GroundwaterPumpEffectiveAmount = 4000f;
    public float m_SurfaceWaterUsageMultiplier = 5E-05f;
    public float m_SurfaceWaterPumpEffectiveDepth = 4f;
    [Tooltip("If the fresh water pollution exceeds this percentage, notifications will be shown on the pump/consumer")]
    [Range(0.0f, 1f)]
    public float m_MaxToleratedPollution = 0.1f;
    [Tooltip("The interval at which pollution spreads in pipes. Higher numbers = slower spread and faster cleanup")]
    [Range(1f, 32f)]
    public int m_WaterPipePollutionSpreadInterval = 5;
    [Tooltip("How much pollution is removed from water pipes without any flow, per tick")]
    [Range(0.0f, 1f)]
    public float m_StaleWaterPipePurification = 1f / 1000f;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add(this.m_WaterService);
      prefabs.Add((PrefabBase) this.m_WaterNotification);
      prefabs.Add((PrefabBase) this.m_DirtyWaterNotification);
      prefabs.Add((PrefabBase) this.m_SewageNotification);
      prefabs.Add((PrefabBase) this.m_WaterPipeNotConnectedNotification);
      prefabs.Add((PrefabBase) this.m_SewagePipeNotConnectedNotification);
      prefabs.Add((PrefabBase) this.m_NotEnoughWaterCapacityNotification);
      prefabs.Add((PrefabBase) this.m_NotEnoughSewageCapacityNotification);
      prefabs.Add((PrefabBase) this.m_NotEnoughGroundwaterNotification);
      prefabs.Add((PrefabBase) this.m_NotEnoughSurfaceWaterNotification);
      prefabs.Add((PrefabBase) this.m_DirtyWaterPumpNotification);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<WaterPipeParameterData>());
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
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<WaterPipeParameterData>(entity, new WaterPipeParameterData()
      {
        m_WaterService = systemManaged.GetEntity(this.m_WaterService),
        m_WaterNotification = systemManaged.GetEntity((PrefabBase) this.m_WaterNotification),
        m_DirtyWaterNotification = systemManaged.GetEntity((PrefabBase) this.m_DirtyWaterNotification),
        m_SewageNotification = systemManaged.GetEntity((PrefabBase) this.m_SewageNotification),
        m_WaterPipeNotConnectedNotification = systemManaged.GetEntity((PrefabBase) this.m_WaterPipeNotConnectedNotification),
        m_SewagePipeNotConnectedNotification = systemManaged.GetEntity((PrefabBase) this.m_SewagePipeNotConnectedNotification),
        m_NotEnoughWaterCapacityNotification = systemManaged.GetEntity((PrefabBase) this.m_NotEnoughWaterCapacityNotification),
        m_NotEnoughSewageCapacityNotification = systemManaged.GetEntity((PrefabBase) this.m_NotEnoughSewageCapacityNotification),
        m_NotEnoughGroundwaterNotification = systemManaged.GetEntity((PrefabBase) this.m_NotEnoughGroundwaterNotification),
        m_NotEnoughSurfaceWaterNotification = systemManaged.GetEntity((PrefabBase) this.m_NotEnoughSurfaceWaterNotification),
        m_DirtyWaterPumpNotification = systemManaged.GetEntity((PrefabBase) this.m_DirtyWaterPumpNotification),
        m_GroundwaterReplenish = this.m_GroundwaterReplenish,
        m_GroundwaterPurification = this.m_GroundwaterPurification,
        m_GroundwaterUsageMultiplier = this.m_GroundwaterUsageMultiplier,
        m_GroundwaterPumpEffectiveAmount = this.m_GroundwaterPumpEffectiveAmount,
        m_SurfaceWaterUsageMultiplier = this.m_SurfaceWaterUsageMultiplier,
        m_SurfaceWaterPumpEffectiveDepth = this.m_SurfaceWaterPumpEffectiveDepth,
        m_MaxToleratedPollution = this.m_MaxToleratedPollution,
        m_WaterPipePollutionSpreadInterval = this.m_WaterPipePollutionSpreadInterval,
        m_StaleWaterPipePurification = this.m_StaleWaterPipePurification
      });
    }
  }
}
