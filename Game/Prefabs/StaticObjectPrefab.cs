// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StaticObjectPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using Game.PSI;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {})]
  public class StaticObjectPrefab : ObjectGeometryPrefab
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<StaticObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Static>());
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        StaticObjectPrefab prefab = this;
        foreach (string modTag in prefab.\u003C\u003En__0())
          yield return modTag;
        if (ModTags.IsProp((PrefabBase) prefab))
          yield return "Prop";
      }
    }
  }
}
