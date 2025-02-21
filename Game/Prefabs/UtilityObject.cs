// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UtilityObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab)})]
  public class UtilityObject : ComponentBase
  {
    public UtilityTypes m_UtilityType = UtilityTypes.WaterPipe;
    public float3 m_UtilityPosition;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<UtilityObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Objects.UtilityObject>());
      components.Add(ComponentType.ReadWrite<Color>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      UtilityObjectData componentData;
      componentData.m_UtilityTypes = this.m_UtilityType;
      componentData.m_UtilityPosition = this.m_UtilityPosition;
      entityManager.SetComponentData<UtilityObjectData>(entity, componentData);
    }
  }
}
