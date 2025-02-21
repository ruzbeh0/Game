// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EditorAssetCategoryOverride
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("UI/", new Type[] {})]
  public class EditorAssetCategoryOverride : ComponentBase
  {
    public string[] m_IncludeCategories;
    public string[] m_ExcludeCategories;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      if ((this.m_IncludeCategories == null || this.m_IncludeCategories.Length == 0) && (this.m_ExcludeCategories == null || this.m_ExcludeCategories.Length == 0))
        return;
      components.Add(ComponentType.ReadWrite<EditorAssetCategoryOverrideData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
