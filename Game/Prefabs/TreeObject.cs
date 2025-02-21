// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TreeObject
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
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab)})]
  public class TreeObject : ComponentBase
  {
    public float m_WoodAmount = 3000f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlantData>());
      components.Add(ComponentType.ReadWrite<TreeData>());
      components.Add(ComponentType.ReadWrite<GrowthScaleData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Plant>());
      components.Add(ComponentType.ReadWrite<Tree>());
      components.Add(ComponentType.ReadWrite<Color>());
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      TreeData componentData;
      componentData.m_WoodAmount = this.m_WoodAmount;
      entityManager.SetComponentData<TreeData>(entity, componentData);
      PlaceableObjectData component;
      if (!entityManager.TryGetComponent<PlaceableObjectData>(entity, out component) || (component.m_Flags & (Game.Objects.PlacementFlags.RoadNode | Game.Objects.PlacementFlags.RoadEdge)) != Game.Objects.PlacementFlags.None)
        return;
      component.m_SubReplacementType = SubReplacementType.Tree;
      entityManager.SetComponentData<PlaceableObjectData>(entity, component);
    }
  }
}
