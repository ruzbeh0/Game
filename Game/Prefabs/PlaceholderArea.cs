// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceholderArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new System.Type[] {typeof (AreaPrefab)})]
  public class PlaceholderArea : ComponentBase
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceholderObjectElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Placeholder>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (!this.prefab.Has<SpawnableArea>())
        return;
      ComponentBase.baseLog.WarnFormat((UnityEngine.Object) this.prefab, "PlaceholderArea is SpawnableArea: {0}", (object) this.prefab.name);
    }
  }
}
