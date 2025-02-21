// Decompiled with JetBrains decompiler
// Type: Game.Rendering.TerrainMaterialPropertiesPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public class TerrainMaterialPropertiesPrefab : PrefabBase
  {
    public Material m_SplatmapMaterial;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TerrainMaterialPropertiesData>());
    }
  }
}
