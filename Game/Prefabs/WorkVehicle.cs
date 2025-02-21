// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WorkVehicle
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Economy;
using Game.Pathfind;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new Type[] {typeof (CarPrefab), typeof (CarTrailerPrefab)})]
  public class WorkVehicle : ComponentBase
  {
    public VehicleWorkType m_WorkType;
    public MapFeature m_MapFeature = MapFeature.None;
    public ResourceInEditor[] m_Resources;
    public float m_MaxWorkAmount = 30000f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WorkVehicleData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Vehicles.WorkVehicle>());
      components.Add(ComponentType.ReadWrite<PathInformation>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      Resource resource = Resource.NoResource;
      if (this.m_Resources != null)
      {
        for (int index = 0; index < this.m_Resources.Length; ++index)
          resource |= EconomyUtils.GetResource(this.m_Resources[index]);
      }
      WorkVehicleData componentData;
      componentData.m_WorkType = this.m_WorkType;
      componentData.m_MapFeature = this.m_MapFeature;
      componentData.m_MaxWorkAmount = this.m_MaxWorkAmount;
      componentData.m_Resources = resource;
      entityManager.SetComponentData<WorkVehicleData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(12));
    }
  }
}
