// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AreaTypePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new System.Type[] {})]
  public class AreaTypePrefab : PrefabBase
  {
    public AreaType m_Type;
    public Material m_Material;
    public Material m_NameMaterial;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AreaTypeData>());
    }
  }
}
