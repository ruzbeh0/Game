// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceholderObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new System.Type[] {typeof (ObjectPrefab)})]
  public class PlaceholderObject : ComponentBase
  {
    public bool m_RandomizeGroupIndex;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceholderObjectElement>());
      components.Add(ComponentType.ReadWrite<PlaceholderObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Placeholder>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.prefab.Has<SpawnableObject>())
        ComponentBase.baseLog.WarnFormat((UnityEngine.Object) this.prefab, "PlaceholderObject is SpawnableObject: {0}", (object) this.prefab.name);
      PlaceholderObjectData componentData = new PlaceholderObjectData()
      {
        m_RandomizeGroupIndex = this.m_RandomizeGroupIndex
      };
      entityManager.SetComponentData<PlaceholderObjectData>(entity, componentData);
    }
  }
}
