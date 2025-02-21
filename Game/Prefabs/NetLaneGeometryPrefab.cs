// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetLaneGeometryPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Rendering;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Lane/", new Type[] {})]
  public class NetLaneGeometryPrefab : NetLanePrefab
  {
    public NetLaneMeshInfo[] m_Meshes;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Meshes.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Meshes[index].m_Mesh);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<NetLaneGeometryData>());
      components.Add(ComponentType.ReadWrite<SubMesh>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<MasterLane>()))
        return;
      components.Add(ComponentType.ReadWrite<LaneGeometry>());
      components.Add(ComponentType.ReadWrite<CullingInfo>());
      components.Add(ComponentType.ReadWrite<MeshBatch>());
      bool flag = false;
      if (this.m_Meshes != null)
      {
        for (int index = 0; index < this.m_Meshes.Length; ++index)
        {
          RenderPrefab mesh = this.m_Meshes[index].m_Mesh;
          flag |= mesh.Has<ColorProperties>();
        }
      }
      if (!flag)
        return;
      components.Add(ComponentType.ReadWrite<PseudoRandomSeed>());
      components.Add(ComponentType.ReadWrite<MeshColor>());
    }
  }
}
