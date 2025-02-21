// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceableObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab)})]
  public class PlaceableObject : ComponentBase
  {
    public uint m_ConstructionCost = 1000;
    public int m_XPReward;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
      components.Add(ComponentType.ReadWrite<PlaceableInfoviewItem>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      PlaceableObjectData componentData = entityManager.GetComponentData<PlaceableObjectData>(entity) with
      {
        m_ConstructionCost = this.m_ConstructionCost,
        m_XPReward = this.m_XPReward
      };
      if ((componentData.m_Flags & (PlacementFlags.Shoreline | PlacementFlags.Floating | PlacementFlags.Hovering)) == PlacementFlags.None)
        componentData.m_Flags |= PlacementFlags.OnGround;
      entityManager.SetComponentData<PlaceableObjectData>(entity, componentData);
    }
  }
}
