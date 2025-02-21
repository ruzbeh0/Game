// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ServiceCoverage
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Pathfind;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (StaticObjectPrefab)})]
  public class ServiceCoverage : ComponentBase
  {
    public float m_Range = 1000f;
    public float m_Capacity = 3000f;
    public float m_Magnitude = 1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CoverageData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CoverageServiceType>());
      components.Add(ComponentType.ReadWrite<CoverageElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      CoverageData componentData = new CoverageData();
      componentData.m_Range = this.m_Range;
      componentData.m_Capacity = this.m_Capacity;
      componentData.m_Magnitude = this.m_Magnitude;
      if (entityManager.HasComponent<HospitalData>(entity))
        componentData.m_Service = CoverageService.Healthcare;
      else if (entityManager.HasComponent<FireStationData>(entity))
        componentData.m_Service = CoverageService.FireRescue;
      else if (entityManager.HasComponent<PoliceStationData>(entity))
        componentData.m_Service = CoverageService.Police;
      else if (entityManager.HasComponent<ParkData>(entity))
        componentData.m_Service = CoverageService.Park;
      else if (entityManager.HasComponent<PostFacilityData>(entity) || entityManager.HasComponent<MailBoxData>(entity))
        componentData.m_Service = CoverageService.PostService;
      else if (entityManager.HasComponent<SchoolData>(entity))
        componentData.m_Service = CoverageService.Education;
      else if (entityManager.HasComponent<EmergencyShelterData>(entity))
        componentData.m_Service = CoverageService.EmergencyShelter;
      else if (entityManager.HasComponent<WelfareOfficeData>(entity))
        componentData.m_Service = CoverageService.Welfare;
      else
        ComponentBase.baseLog.ErrorFormat((UnityEngine.Object) this.prefab, "Unknown coverage service type: {0}", (object) this.prefab.name);
      entityManager.SetComponentData<CoverageData>(entity, componentData);
    }
  }
}
