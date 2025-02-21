// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StorageArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Economy;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new Type[] {typeof (LotPrefab)})]
  public class StorageArea : ComponentBase
  {
    public ResourceInEditor[] m_StoredResources;
    public int m_Capacity;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<StorageAreaData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Storage>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      Resource resource = Resource.NoResource;
      if (this.m_StoredResources != null)
      {
        for (int index = 0; index < this.m_StoredResources.Length; ++index)
          resource |= EconomyUtils.GetResource(this.m_StoredResources[index]);
      }
      StorageAreaData componentData;
      componentData.m_Resources = resource;
      componentData.m_Capacity = this.m_Capacity;
      entityManager.SetComponentData<StorageAreaData>(entity, componentData);
    }
  }
}
