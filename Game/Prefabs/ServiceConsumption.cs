// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ServiceConsumption
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class ServiceConsumption : ComponentBase, IServiceUpgrade
  {
    public int m_Upkeep;
    public int m_ElectricityConsumption;
    public int m_WaterConsumption;
    public int m_GarbageAccumulation;
    public float m_TelecomNeed;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ConsumptionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (this.prefab.Has<ServiceUpgrade>())
        return;
      this.GetConsumptionData().AddArchetypeComponents(components);
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      this.GetConsumptionData().AddArchetypeComponents(components);
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<ConsumptionData>(entity, this.GetConsumptionData());
    }

    private ConsumptionData GetConsumptionData()
    {
      return new ConsumptionData()
      {
        m_Upkeep = this.m_Upkeep,
        m_ElectricityConsumption = (float) this.m_ElectricityConsumption,
        m_WaterConsumption = (float) this.m_WaterConsumption,
        m_GarbageAccumulation = (float) this.m_GarbageAccumulation,
        m_TelecomNeed = this.m_TelecomNeed
      };
    }
  }
}
