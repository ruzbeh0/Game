// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlantObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Net;
using Game.Objects;
using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (NetLaneGeometryPrefab)})]
  public class PlantObject : ComponentBase
  {
    public float m_PotCoverage;
    public bool m_TreeReplacement;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlantData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Plant>());
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      PlaceableObjectData component;
      if (!this.m_TreeReplacement || !entityManager.TryGetComponent<PlaceableObjectData>(entity, out component) || (component.m_Flags & (Game.Objects.PlacementFlags.RoadNode | Game.Objects.PlacementFlags.RoadEdge)) != Game.Objects.PlacementFlags.None)
        return;
      component.m_SubReplacementType = SubReplacementType.Tree;
      entityManager.SetComponentData<PlaceableObjectData>(entity, component);
    }
  }
}
