// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AtmospherePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Diversity/", new System.Type[] {})]
  public class AtmospherePrefab : PrefabBase
  {
    public Texture2D m_MoonAlbedo;
    public Texture2D m_MoonNormal;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AtmospherePrefabData>());
    }
  }
}
