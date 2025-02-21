// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ResourceProducer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab)})]
  [RequireComponent(typeof (CityServiceBuilding))]
  public class ResourceProducer : ComponentBase, IServiceUpgrade
  {
    public ResourceProductionInfo[] m_Resources;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ResourceProductionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      components.Add(ComponentType.ReadWrite<Game.Buildings.ResourceProducer>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      components.Add(ComponentType.ReadWrite<Game.Buildings.ResourceProducer>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_Resources == null)
        return;
      DynamicBuffer<ResourceProductionData> buffer = entityManager.GetBuffer<ResourceProductionData>(entity);
      buffer.ResizeUninitialized(this.m_Resources.Length);
      for (int index = 0; index < this.m_Resources.Length; ++index)
      {
        ResourceProductionInfo resource = this.m_Resources[index];
        buffer[index] = new ResourceProductionData(EconomyUtils.GetResource(resource.m_Resource), resource.m_ProductionRate, resource.m_StorageCapacity);
      }
    }
  }
}
