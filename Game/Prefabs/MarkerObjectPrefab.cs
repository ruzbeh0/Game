// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MarkerObjectPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Rendering;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {})]
  public class MarkerObjectPrefab : ObjectPrefab
  {
    public RenderPrefab m_Mesh;
    public bool m_Circular;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_Mesh);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectGeometryData>());
      components.Add(ComponentType.ReadWrite<SubMesh>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Static>());
      components.Add(ComponentType.ReadWrite<Marker>());
      components.Add(ComponentType.ReadWrite<CullingInfo>());
      components.Add(ComponentType.ReadWrite<MeshBatch>());
      components.Add(ComponentType.ReadWrite<PseudoRandomSeed>());
    }
  }
}
