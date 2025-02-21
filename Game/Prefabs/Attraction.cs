// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Attraction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab)})]
  public class Attraction : ComponentBase, IServiceUpgrade
  {
    public int m_Attractiveness;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<AttractionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null) || this.m_Attractiveness <= 0)
        return;
      components.Add(ComponentType.ReadWrite<AttractivenessProvider>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      if (this.m_Attractiveness <= 0)
        return;
      components.Add(ComponentType.ReadWrite<AttractivenessProvider>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      AttractionData componentData = new AttractionData()
      {
        m_Attractiveness = this.m_Attractiveness
      };
      entityManager.SetComponentData<AttractionData>(entity, componentData);
    }
  }
}
