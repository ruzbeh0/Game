// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.OverlayConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class OverlayConfigurationPrefab : PrefabBase
  {
    public Material m_CurveMaterial;
    public Material m_ObjectBrushMaterial;
    public FontInfo[] m_FontInfos;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<OverlayConfigurationData>());
    }
  }
}
