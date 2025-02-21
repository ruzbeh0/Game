// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UniqueObject
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
  public class UniqueObject : ComponentBase
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
      components.Add(ComponentType.ReadWrite<UniqueObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Objects.UniqueObject>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      PlaceableObjectData componentData = entityManager.GetComponentData<PlaceableObjectData>(entity);
      componentData.m_Flags |= PlacementFlags.Unique;
      entityManager.SetComponentData<PlaceableObjectData>(entity, componentData);
    }
  }
}
