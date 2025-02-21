// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ShorelineObject
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
  public class ShorelineObject : ComponentBase
  {
    public float m_ShorelineOffset;
    public bool m_AllowDryland;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      PlaceableObjectData componentData = entityManager.GetComponentData<PlaceableObjectData>(entity);
      componentData.m_PlacementOffset.z = this.m_ShorelineOffset;
      if (this.m_AllowDryland)
      {
        componentData.m_Flags |= PlacementFlags.OnGround | PlacementFlags.Shoreline;
      }
      else
      {
        componentData.m_Flags &= ~PlacementFlags.OnGround;
        componentData.m_Flags |= PlacementFlags.Shoreline;
      }
      entityManager.SetComponentData<PlaceableObjectData>(entity, componentData);
    }
  }
}
