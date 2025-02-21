// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SubObjectDefaultProbability
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new System.Type[] {typeof (ObjectPrefab)})]
  public class SubObjectDefaultProbability : ComponentBase
  {
    [Range(0.0f, 100f)]
    public int m_DefaultProbability = 100;
    public RotationSymmetry m_RotationSymmetry;

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
      if (this.prefab.Has<ServiceUpgrade>())
        ComponentBase.baseLog.ErrorFormat((UnityEngine.Object) this.prefab, "ServiceUpgrade cannot have SubObjectDefaultProbability: {0}", (object) this.prefab.name);
      PlaceableObjectData componentData = entityManager.GetComponentData<PlaceableObjectData>(entity) with
      {
        m_DefaultProbability = (byte) this.m_DefaultProbability,
        m_RotationSymmetry = this.m_RotationSymmetry
      };
      componentData.m_Flags |= PlacementFlags.HasProbability;
      entityManager.SetComponentData<PlaceableObjectData>(entity, componentData);
    }
  }
}
