// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LodProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new Type[] {typeof (RenderPrefab)})]
  public class LodProperties : ComponentBase
  {
    public float m_Bias;
    public float m_ShadowBias;
    public RenderPrefab[] m_LodMeshes;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_LodMeshes == null)
        return;
      for (int index = 0; index < this.m_LodMeshes.Length; ++index)
        prefabs.Add((PrefabBase) this.m_LodMeshes[index]);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<LodMesh>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
