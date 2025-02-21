// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WelfareOffice
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class WelfareOffice : ComponentBase, IServiceUpgrade
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WelfareOfficeData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Game.Buildings.WelfareOffice>());
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      if (!((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<ServiceDistrict>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.WelfareOffice>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(5));
    }
  }
}
