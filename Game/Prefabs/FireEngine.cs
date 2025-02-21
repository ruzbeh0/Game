// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FireEngine
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using Game.Pathfind;
using Game.PSI;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ExcludeGeneratedModTag]
  [ComponentMenu("Vehicles/", new System.Type[] {typeof (CarPrefab), typeof (AircraftPrefab)})]
  public class FireEngine : ComponentBase
  {
    public float m_ExtinguishingRate = 7f;
    public float m_ExtinguishingSpread = 20f;
    public float m_ExtinguishingCapacity;
    public float m_DestroyedClearDuration = 10f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<FireEngineData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Vehicles.FireEngine>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()))
        return;
      components.Add(ComponentType.ReadWrite<PathInformation>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<FireEngineData>(entity, new FireEngineData(this.m_ExtinguishingRate, this.m_ExtinguishingSpread, this.m_ExtinguishingCapacity, this.m_DestroyedClearDuration));
      if (!entityManager.HasComponent<CarData>(entity))
        return;
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(4));
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        FireEngine fireEngine = this;
        foreach (string modTag in fireEngine.\u003C\u003En__0())
          yield return modTag;
        if ((UnityEngine.Object) fireEngine.GetComponent<AircraftPrefab>() != (UnityEngine.Object) null)
          yield return "FireEngineAircraft";
        else
          yield return nameof (FireEngine);
      }
    }
  }
}
