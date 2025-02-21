// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BaseProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {typeof (RenderPrefab)})]
  public class BaseProperties : ComponentBase
  {
    public RenderPrefab m_BaseType;
    public bool m_UseMinBounds;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_BaseType != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_BaseType);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }
  }
}
