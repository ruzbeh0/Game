// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ToBeRemoved
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Prefabs/", new System.Type[] {})]
  public class ToBeRemoved : ComponentBase
  {
    public PrefabBase m_ReplaceWith;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      ComponentBase.baseLog.WarnFormat((UnityEngine.Object) this.prefab, "Loading prefab that is set to be removed ({0})", (object) this.prefab.name);
    }
  }
}
