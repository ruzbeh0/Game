// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIMultiTagPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.OdinSerializer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("UI/", new System.Type[] {})]
  public class UIMultiTagPrefab : PrefabBase
  {
    [Tooltip("If set, the override is used as a UI tag instead of the generated tag.")]
    [CanBeNull]
    public string m_Override;
    [Tooltip("This prefab allows adding multiple tags in Tutorials, Tutorial Phases and Tutorial Triggers. The tags are treated as separate but hierarchically equal by the Tutorials system.")]
    public PrefabBase[] m_UITagProviders;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_UITagProviders == null)
        return;
      for (int index = 0; index < this.m_UITagProviders.Length; ++index)
        prefabs.Add(this.m_UITagProviders[index]);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> prefabComponents)
    {
      base.GetPrefabComponents(prefabComponents);
      prefabComponents.Add(ComponentType.ReadWrite<UITagPrefabData>());
    }

    public override string uiTag
    {
      get
      {
        if (!this.m_Override.IsNullOrWhitespace())
          return this.m_Override;
        return this.m_UITagProviders == null ? base.uiTag : string.Join("|", ((IEnumerable<PrefabBase>) this.m_UITagProviders).Select<PrefabBase, string>((Func<PrefabBase, string>) (t => t.uiTag)));
      }
    }
  }
}
